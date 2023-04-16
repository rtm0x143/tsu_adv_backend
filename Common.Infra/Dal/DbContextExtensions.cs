using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Common.Infra.Dal;

public static class DbContextExtensions
{
    public static Task<IDbContextTransaction> NestTransaction(this IDbContextTransaction transaction)
        => NestedDbContextTransaction.Create(transaction);

    public static Task<IDbContextTransaction> BeginNestingTransaction(this DatabaseFacade database)
        => database.CurrentTransaction is IDbContextTransaction currentTransaction
            ? NestedDbContextTransaction.Create(currentTransaction)
            : database.BeginTransactionAsync();
}