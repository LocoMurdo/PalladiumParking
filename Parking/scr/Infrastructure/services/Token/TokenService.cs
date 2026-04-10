using Microsoft.IdentityModel.Tokens;
using Parking.API.scr.Shared.Configurations;
using Parking.API.scr.Shared.Constans;
using Parking.API.scr.Shared.Interfaces;
using Parking.API.scr.Shared.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Parking.API.scr.Infrastructure.services.Token
{
    public class TokenService(AppSettings settings, IDatetimeservice datetimeservice) : ITokenService
    {
        public string CreateAccessToken(UserClaims user)
            => CreateAccessToken(CreateClaimsForUser(user));

        public string CreateAccessToken(IEnumerable<Claim> claims)
            => CreateJwt(
                claims,
                expires: datetimeservice.UtcNow.AddMinutes(settings.AccessTokenExpires),
                key: settings.AccessTokenKey);

        public DateTime CreateExpiryForRefreshToken()
            => datetimeservice.UtcNow.AddDays(settings.RefreshTokenExpires);

        public string CreateRefreshToken()
            => RandomHelper.GetRandomNumber();

        public string HashToken(string token)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
            return Convert.ToBase64String(bytes);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredAccessToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.AccessTokenKey)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        public ClaimsIdentity GetClaimsIdentity(string token)
        {
            var principal = GetPrincipalFromExpiredAccessToken(token);
            return principal.Identity as ClaimsIdentity;
        }

        private static string CreateJwt(IEnumerable<Claim> claims, DateTime expires, string key)
            => JwtEncoder.Create()
                .WithSubject(claims)
                .WithExpires(expires)
                .WithSigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
                .Encode();

        private static List<Claim> CreateClaimsForUser(UserClaims user)
        {
            var claims = new List<Claim>
            {
                new(CustomClaimsType.UserId, user.UserId.ToString()),
                new(CustomClaimsType.PersonId, user.PersonId.ToString()),
                new(CustomClaimsType.UserName, user.UserName),
                new(CustomClaimsType.Role, user.Role),
                new(ClaimTypes.Role, user.Role),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            return claims;
        }
    }
}
