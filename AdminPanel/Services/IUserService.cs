using Common.Domain.ValueTypes;
using OneOf;

namespace AdminPanel.Services;

public interface IUserService
{
    Task<OneOf<EmptyResult, Exception>> ChangeBanStatus(Guid id, bool isBanned);
}