namespace Parking.API.scr.Shared.Interfaces
{
    public interface IPasswordHasher
    {
        bool Verify(string text, string passwordHash);
        string HashPassword(string text);
    }
}
