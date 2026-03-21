using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities;
using SimpleResults;

namespace Parking.API.scr.Core.Security.Users.UseCases
{
    public class DeleteUserUseCase(DbContext context)
    {
        public async Task<Result> ExecuteAsync(int id)
        {
            var user = await context.Set<User>()
                .Include(u => u.Person)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user is null)
                return Result.NotFound("User not found.");

            context.Set<User>().Remove(user);
            if (user.Person is not null)
                context.Set<Person>().Remove(user.Person);

            await context.SaveChangesAsync();

            return Result.Success("User deleted successfully.");
        }
    }
}
