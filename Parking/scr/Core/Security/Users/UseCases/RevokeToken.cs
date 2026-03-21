using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities;
using Parking.API.scr.Shared.Interfaces;
using SimpleResults;

namespace Parking.API.scr.Core.Security.Users.UseCases
{
    public class RevokeTokenRequest
    {
        public string RefreshToken { get; init; }
    }

    public class RevokeTokenUseCase(
        DbContext context,
        ITokenService tokenService,
        IDatetimeservice dateTime)
    {
        public async Task<Result> ExecuteAsync(RevokeTokenRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
                return Result.Invalid("Refresh token is required.");

            var tokenHash = tokenService.HashToken(request.RefreshToken);

            var storedToken = await context.Set<RefreshToken>()
                .FirstOrDefaultAsync(rt => rt.TokenHash == tokenHash);

            if (storedToken is null)
                return Result.NotFound("Refresh token not found.");

            if (storedToken.IsRevoked)
                return Result.Conflict("Token is already revoked.");

            storedToken.RevokedAt = dateTime.UtcNow;
            await context.SaveChangesAsync();

            return Result.Success("Session closed successfully.");
        }
    }
}
