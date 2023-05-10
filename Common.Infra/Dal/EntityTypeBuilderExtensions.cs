using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Common.Infra.Dal;

public static class EntityTypeBuilderExtension
{
    public static EntityTypeBuilder<TDependentEntity> IsDependentEntity<TDependentEntity, TPrincipalEntity>(
        this EntityTypeBuilder<TDependentEntity> builder,
        Expression<Func<TDependentEntity, object?>> keyExpression,
        bool isRequiredDependent = true)
        where TDependentEntity : class
        where TPrincipalEntity : class
    {
        builder.HasKey(keyExpression);
        builder.HasOne<TPrincipalEntity>()
            .WithOne()
            .HasForeignKey(keyExpression)
            .OnDelete(DeleteBehavior.Cascade)
            .Metadata.IsRequiredDependent = isRequiredDependent;

        return builder;
    }
}