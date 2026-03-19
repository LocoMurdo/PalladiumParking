using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities;
using Parking.API.scr.Shared.Interfaces.Persistence.Repositories;

namespace Parking.API.scr.Infrastructure.Persistence.Repositories
{
    public class UserRepository(DbContext context) : IUserRepository
    {
        public async Task<User> GetFullUserProfileAsync(string username)
        {
            var user = await context.Set<User>()
              .Include(user => user.Person)      
              .Where(user => user.UserName == username)
              .FirstOrDefaultAsync();

            return user;    
        }

        public async Task<bool> UserExistAsync(string username)
        {
            var user = await context.Set<User>()
           .Where(user => user.UserName == username)
           .Select(user => new { user.Id })
           .AsNoTracking()
           .FirstOrDefaultAsync();

            return user is not null;

        }
    }


}
