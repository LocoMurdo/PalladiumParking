using Parking.API.scr.Shared.Interfaces;

namespace Parking.API.scr.Infrastructure.services
{
    public class PasswordHasherBcrypt : IPasswordHasher
    {
        public string HashPassword(string text)
         => BCrypt.Net.BCrypt.EnhancedHashPassword(text, 13);

        public bool Verify(string text, string passwordHash)
        => BCrypt.Net.BCrypt.EnhancedVerify(text, passwordHash);
    }
}
