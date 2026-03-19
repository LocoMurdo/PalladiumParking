using Parking.API.scr.Shared.Entities;

namespace Parking.API.scr.Shared.Interfaces.Persistence.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetFullUserProfileAsync(string username);
        Task<bool> UserExistAsync(string username);
    }
}
