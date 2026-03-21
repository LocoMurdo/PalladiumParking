using Parking.API.scr.Infrastructure.services;

namespace TestParking.Services
{
    public class PasswordHasherTest
    {
        private readonly PasswordHasherBcrypt _hasher = new();

        [Fact]
        public void HashPassword_ShouldReturnHash()
        {
            var hash = _hasher.HashPassword("Test123!");

            Assert.NotNull(hash);
            Assert.NotEmpty(hash);
            Assert.NotEqual("Test123!", hash);
        }

        [Fact]
        public void Verify_CorrectPassword_ShouldReturnTrue()
        {
            var hash = _hasher.HashPassword("Test123!");
            var result = _hasher.Verify("Test123!", hash);

            Assert.True(result);
        }

        [Fact]
        public void Verify_WrongPassword_ShouldReturnFalse()
        {
            var hash = _hasher.HashPassword("Test123!");
            var result = _hasher.Verify("WrongPassword", hash);

            Assert.False(result);
        }

        [Fact]
        public void HashPassword_SameInput_ShouldReturnDifferentHashes()
        {
            var hash1 = _hasher.HashPassword("Test123!");
            var hash2 = _hasher.HashPassword("Test123!");

            Assert.NotEqual(hash1, hash2);
        }
    }
}
