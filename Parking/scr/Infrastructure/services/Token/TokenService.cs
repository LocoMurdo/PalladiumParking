using Microsoft.IdentityModel.Tokens;
using Parking.API.scr.Shared.Configurations;
using Parking.API.scr.Shared.Constans;
using Parking.API.scr.Shared.Interfaces;
using Parking.API.scr.Shared.Models;
using System.Security.Claims;

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
        =>
            datetimeservice.Now.AddDays(settings.RefreshTokenExpires);


        public string CreateRefreshToken()
      => RandomHelper.GetRandomNumber();

        public ClaimsIdentity GetClaimsIdentity(string token)
        {
            throw new NotImplementedException();
        }

        public ClaimsPrincipal GetPrincipalFromExpiredAccessToken(string token)
        {
            throw new NotImplementedException();
        }
        private static string CreateJwt(IEnumerable<Claim> claims, DateTime expires, string key)
            => JwtEncoder.Create()
            .WithSubject(claims)
            .WithExpires(expires)
            .WithSigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            .Encode();
        //private static string  ValidateJwt(string token, string key)
        ///{ return;  }
        private static List<Claim> CreateClaimsForUser(UserClaims user)
        {
            var claims = new List<Claim>
            {
                new(CustomClaimsType.UserId, user.UserId.ToString()),
                new(CustomClaimsType.PersonId, user.PersonId.ToString()),
                new (CustomClaimsType.UserName, user.UserName)
              
            };
          

            return claims;

        }
    }
}
