using AdminPanel.Entities;
using AdminPanel.ViewModels;
using Common.Domain.ValueTypes;
using OneOf;

namespace AdminPanel.Services;

public interface IUserRepository
{
    Task<OneOf<User, Exception>> Get(Guid id);
    Task<OneOf<User[], Exception>> Get(UsersQuery query);
    Task<OneOf<EmptyResult, Exception>> Delete(Guid id);
}