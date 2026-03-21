using Parking.API.scr.Shared.Models;
using System.Security.Claims;

namespace Parking.API.scr.Shared.Interfaces
{
    public interface ITokenService
    {
        string CreateAccessToken(UserClaims user);

        string CreateAccessToken(IEnumerable<Claim> claims);

        string CreateRefreshToken();
        string HashToken(string token);
        DateTime CreateExpiryForRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredAccessToken(string token);
        ClaimsIdentity GetClaimsIdentity(string token);
    }
}
