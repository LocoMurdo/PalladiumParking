using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Constans;
using Parking.API.scr.Shared.Entities;
using Parking.API.scr.Shared.Interfaces;
using Parking.API.scr.Shared.Models;
using SimpleResults;

namespace Parking.API.scr.Core.Security.Users.UseCases
{
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; init; }
    }

    public class RefreshTokenResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }

    public class RefreshTokenUseCase(
        DbContext context,
        ITokenService tokenService,
        IDatetimeservice dateTime,
        IHttpContextAccessor httpContextAccessor)
    {
        public async Task<Result<RefreshTokenResponse>> ExecuteAsync(RefreshTokenRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
                return Result.Unauthorized("Refresh token is required.");

            var tokenHash = tokenService.HashToken(request.RefreshToken);

            var storedToken = await context.Set<RefreshToken>()
                .Include(rt => rt.User)
                    .ThenInclude(u => u.Person)
                .FirstOrDefaultAsync(rt => rt.TokenHash == tokenHash);

            if (storedToken is null)
                return Result.Unauthorized("Invalid refresh token.");

            // Replay detection: if token is already revoked, revoke all tokens for this user
            if (storedToken.IsRevoked)
            {
                await RevokeAllUserTokensAsync(storedToken.UserId);
                return Result.Unauthorized("Token reuse detected. All sessions have been revoked.");
            }

            if (storedToken.IsExpired)
                return Result.Unauthorized("Refresh token has expired.");

            // Rotate: revoke old token and create new one
            var newRawRefreshToken = tokenService.CreateRefreshToken();
            var newTokenHash = tokenService.HashToken(newRawRefreshToken);

            storedToken.RevokedAt = dateTime.UtcNow;
            storedToken.ReplacedByTokenHash = newTokenHash;

            var newRefreshToken = new RefreshToken
            {
                UserId = storedToken.UserId,
                TokenHash = newTokenHash,
                ExpiresAt = tokenService.CreateExpiryForRefreshToken(),
                CreatedAt = dateTime.UtcNow,
                CreatedByIp = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString()
            };

            context.Set<RefreshToken>().Add(newRefreshToken);
            await context.SaveChangesAsync();

            // Generate new access token
            var user = storedToken.User;
            var userClaims = new UserClaims
            {
                UserId = user.Id,
                PersonId = user.PersonId,
                UserName = user.UserName
            };

            var response = new RefreshTokenResponse
            {
                AccessToken = tokenService.CreateAccessToken(userClaims),
                RefreshToken = newRawRefreshToken
            };

            return Result.Success(response, "Tokens refreshed successfully.");
        }

        private async Task RevokeAllUserTokensAsync(int userId)
        {
            var activeTokens = await context.Set<RefreshToken>()
                .Where(rt => rt.UserId == userId && rt.RevokedAt == null)
                .ToListAsync();

            foreach (var token in activeTokens)
            {
                token.RevokedAt = dateTime.UtcNow;
            }

            await context.SaveChangesAsync();
        }
    }
}
