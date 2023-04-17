using System.Transactions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Common.Infra.Dal;

public class NestedDbContextTransaction : IDbContextTransaction
{
    private readonly IDbContextTransaction _transaction;
    private bool _isReleased;

    private NestedDbContextTransaction(IDbContextTransaction transaction, Guid id)
    {
        _transaction = transaction;
        TransactionId = id;
    }

    const string SavePointName = "__NestedTransactionSavePoint";
    internal static Task<IDbContextTransaction> Create(IDbContextTransaction transaction) =>
        transaction.CreateSavepointAsync(SavePointName)
            .ContinueWith<IDbContextTransaction>(t => new NestedDbContextTransaction(transaction, Guid.NewGuid()));

    private ValueTask _tryReleaseAsync(CancellationToken cancellationToken = default) =>
        _isReleased != (_isReleased = true)
            ? new(_transaction.ReleaseSavepointAsync(SavePointName, cancellationToken))
            : ValueTask.CompletedTask;

    private void _tryRelease()
    {
        if (_isReleased != (_isReleased = true)) _transaction.ReleaseSavepoint(SavePointName);
    }

    public void Dispose() => _tryRelease();
    public ValueTask DisposeAsync() => _tryReleaseAsync();

    public void Commit() => _transaction.ReleaseSavepoint(SavePointName);

    public Task CommitAsync(CancellationToken cancellationToken = new()) =>
        _tryReleaseAsync(cancellationToken).AsTask();

    private void _trowIfReleased()
    {
        if (_isReleased)
            throw new TransactionAbortedException($"{nameof(NestedDbContextTransaction)} already released savepoint");
    }

    public void Rollback()
    {
        _trowIfReleased();
        _transaction.RollbackToSavepoint(SavePointName);
    }

    public Task RollbackAsync(CancellationToken cancellationToken = new())
    {
        _trowIfReleased();
        return _transaction.RollbackToSavepointAsync(SavePointName, cancellationToken);
    }

    public Guid TransactionId { get; }
}