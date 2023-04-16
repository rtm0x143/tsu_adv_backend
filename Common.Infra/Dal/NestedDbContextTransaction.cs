using Microsoft.EntityFrameworkCore.Storage;

namespace Common.Infra.Dal;

public class NestedDbContextTransaction : IDbContextTransaction
{
    private readonly IDbContextTransaction _transaction;
    private readonly Guid _id;
    private readonly string _savepoint;

    private NestedDbContextTransaction(IDbContextTransaction transaction, Guid id, string savepointName)
    {
        _transaction = transaction;
        _id = id;
        _savepoint = savepointName;
    }

    internal static async Task<IDbContextTransaction> Create(IDbContextTransaction transaction)
    {
        var id = Guid.NewGuid();
        var name = id.ToString();
        await transaction.CreateSavepointAsync(name);
        return new NestedDbContextTransaction(transaction, id, name);
    }

    public void Dispose() => _transaction.ReleaseSavepoint(_savepoint);
    public ValueTask DisposeAsync() => new(_transaction.ReleaseSavepointAsync(_savepoint));

    public void Commit() => _transaction.ReleaseSavepoint(_savepoint);

    public Task CommitAsync(CancellationToken cancellationToken = new CancellationToken()) =>
        _transaction.ReleaseSavepointAsync(_savepoint, cancellationToken);

    public void Rollback() => _transaction.RollbackToSavepoint(_savepoint);

    public Task RollbackAsync(CancellationToken cancellationToken = new CancellationToken()) =>
        _transaction.RollbackToSavepointAsync(_savepoint, cancellationToken);

    public Guid TransactionId => _id;
}