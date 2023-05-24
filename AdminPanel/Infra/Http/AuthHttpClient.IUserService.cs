using AdminPanel.Services;
using Common.Domain.ValueTypes;
using OneOf;

namespace AdminPanel.Infra.Http;

public partial class AuthHttpClient : IUserService
{
    public Task<OneOf<EmptyResult, Exception>> ChangeBanStatus(Guid id, bool isBanned)
    {
        return SendCheckedAsync(HttpMethod.Patch, $"user/{id}/banned/{isBanned}");
    }
}