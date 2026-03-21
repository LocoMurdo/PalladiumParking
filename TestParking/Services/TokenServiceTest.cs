using Parking.API.scr.Infrastructure.services;
using Parking.API.scr.Infrastructure.services.Token;
using Parking.API.scr.Shared.Configurations;
using Parking.API.scr.Shared.Models;

namespace TestParking.Services
{
    public class TokenServiceTest
    {
        private readonly TokenService _tokenService;

        public TokenServiceTest()
        {
            var settings = new AppSettings
            {
                AccessTokenKey = "super-secret-key-that-is-long-enough-for-hmac-256",
                AccessTokenExpires = 15,
                RefreshTokenExpires = 7
            };
            var dateTimeService = new DateTimeService();
            _tokenService = new TokenService(settings, dateTimeService);
        }

        [Fact]
        public void CreateAccessToken_ShouldReturnValidJwt()
        {
            var claims = new UserClaims
            {
                UserId = 1,
                PersonId = 1,
                UserName = "admin@test.com"
            };

            var token = _tokenService.CreateAccessToken(claims);

            Assert.NotNull(token);
            Assert.NotEmpty(token);
            Assert.Contains(".", token);
        }

        [Fact]
        public void CreateRefreshToken_ShouldReturnBase64String()
        {
            var token = _tokenService.CreateRefreshToken();

            Assert.NotNull(token);
            Assert.NotEmpty(token);

            var bytes = Convert.FromBase64String(token);
            Assert.Equal(32, bytes.Length);
        }

        [Fact]
        public void CreateRefreshToken_ShouldBeUnique()
        {
            var token1 = _tokenService.CreateRefreshToken();
            var token2 = _tokenService.CreateRefreshToken();

            Assert.NotEqual(token1, token2);
        }

        [Fact]
        public void HashToken_ShouldReturnConsistentHash()
        {
            var token = "test-token-123";

            var hash1 = _tokenService.HashToken(token);
            var hash2 = _tokenService.HashToken(token);

            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void HashToken_DifferentTokens_ShouldReturnDifferentHashes()
        {
            var hash1 = _tokenService.HashToken("token-1");
            var hash2 = _tokenService.HashToken("token-2");

            Assert.NotEqual(hash1, hash2);
        }

        [Fact]
        public void GetPrincipalFromExpiredAccessToken_ShouldReturnClaims()
        {
            var claims = new UserClaims
            {
                UserId = 42,
                PersonId = 10,
                UserName = "test@test.com"
            };

            var token = _tokenService.CreateAccessToken(claims);
            var principal = _tokenService.GetPrincipalFromExpiredAccessToken(token);

            Assert.NotNull(principal);
            Assert.Contains(principal.Claims, c => c.Value == "42");
            Assert.Contains(principal.Claims, c => c.Value == "test@test.com");
        }

        [Fact]
        public void CreateExpiryForRefreshToken_ShouldBe7DaysFromNow()
        {
            var expiry = _tokenService.CreateExpiryForRefreshToken();

            var expectedMin = DateTime.UtcNow.AddDays(6.9);
            var expectedMax = DateTime.UtcNow.AddDays(7.1);

            Assert.InRange(expiry, expectedMin, expectedMax);
        }
    }
}
