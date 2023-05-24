using AdminPanel.Entities;
using AdminPanel.Services;
using Common.Domain.ValueTypes;
using OneOf;

namespace AdminPanel.Infra.Http;

public partial class AuthHttpClient : IProfileRepository
{
    public Task<OneOf<UserProfile, Exception>> GetSelfProfile()
    {
        return SendAsJsonCheckedAsync<UserProfile>(HttpMethod.Get, "user/profile");
    }

    public Task<OneOf<EmptyResult, Exception>> UpdateProfile(Guid id, UserProfile profile)
    {
        return SendAsJsonCheckedAsync(HttpMethod.Put, $"user/{id}/profile", profile);
    }
}