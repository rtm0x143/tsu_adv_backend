using AdminPanel.Models;
using OneOf;

namespace AdminPanel.Services;

public interface IProfileRepository
{
    Task<OneOf<ProfileViewModel, Exception>> GetSelfProfile();
}