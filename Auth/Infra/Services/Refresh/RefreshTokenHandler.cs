using Auth.Features.Auth.Common;
using Auth.Infra.Data;
using Auth.Infra.Data.Entities;
using Auth.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OneOf;

namespace Auth.Infra.Services.Refresh;

// ReSharper disable once UnusedType.Global
public class RefreshTokenHandler : IRefreshTokenHandler
{
    private readonly AuthDbContext _dbContext;
    private readonly RefreshTokenConfigurationProperties _tokenProperties;

    public RefreshTokenHandler(AuthDbContext dbContext, IOptions<RefreshTokenConfigurationProperties> options)
    {
        _dbContext = dbContext;
        _tokenProperties = options.Value;
    }

    private async Task<OneOf<RefreshToken, ArgumentException>> IssuerFor(Guid userId)
    {
        var createResult = RefreshToken.CreateNew(userId, _tokenProperties.LifeTime);
        if (createResult.IsT1) return createResult.AsT1;

        var tokenEntity = createResult.AsT0.AdaptToUser();
        var entry = await _dbContext.UserRefreshTokens.AddAsync(tokenEntity);
        await _dbContext.SaveChangesAsync();

        return entry.Entity.ToRefreshTokenModel();
    }

    public Task<OneOf<RefreshToken, ArgumentException>> IssuerFor(AppUser user) => IssuerFor(user.Id);

    public async Task<OneOf<RefreshToken, ArgumentException>> ReIssue(RefreshToken token)
    {
        var entity = await _dbContext.UserRefreshTokens.FindAsync(token.Id);
        if (entity != null) entity.IsUsed = true;
        return await IssuerFor(token.UserId);
    }

    public async ValueTask Revoke(RefreshToken token)
    {
        var entity = await _dbContext.UserRefreshTokens.FindAsync(token.Id);
        if (entity != null) entity.IsUsed = true;
    }

    public Task DropFamily(RefreshToken token) => _dropFamily(token.UserId);
    public Task DropFamily(AppUser user) => _dropFamily(user.Id);

    private Task _dropFamily(Guid userId)
    {
        return _dbContext.UserRefreshTokens
            .Where(t => t.UserId == userId)
            .ExecuteDeleteAsync();
    }

    public string Write(RefreshToken token) => Base64UrlEncoder.Encode(token.Id.ToString());

    public async ValueTask<OneOf<RefreshToken, ArgumentException, KeyNotFoundException>> Read(string refreshTokenString)
    {
        Guid tokenId;
        try
        {
            tokenId = Guid.Parse(Base64UrlEncoder.Decode(refreshTokenString));
        }
        catch (Exception)
        {
            return new ArgumentException("Token was invalid");
        }

        var token = await _dbContext.UserRefreshTokens.FindAsync(tokenId);
        if (token == null) return new KeyNotFoundException("TokenId");

        return token.ToRefreshTokenModel()
            .Match<OneOf<RefreshToken, ArgumentException, KeyNotFoundException>>(
                tokenModel => tokenModel,
                ex => ex);
    }
}