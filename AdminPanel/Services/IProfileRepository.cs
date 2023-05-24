using AdminPanel.Entities;
using OneOf;

namespace AdminPanel.Services;

public interface IProfileRepository
{
    Task<OneOf<UserProfile, Exception>> GetSelfProfile();
    Task<OneOf<Common.Domain.ValueTypes.EmptyResult, Exception>> UpdateProfile(Guid id, UserProfile user);
}