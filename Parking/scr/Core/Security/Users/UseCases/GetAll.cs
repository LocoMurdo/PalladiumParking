using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities;
using SimpleResults;

namespace Parking.API.scr.Core.Security.Users.UseCases
{
    public class UserResponse
    {
        public int Id { get; init; }
        public string UserName { get; init; }
        public string Names { get; init; }
        public string LastNames { get; init; }
        public string CellPhone { get; init; }
        public DateTime? CreatedAt { get; init; }
    }

    public class GetAllUsersUseCase(DbContext context)
    {
        public async Task<Result<IEnumerable<UserResponse>>> ExecuteAsync()
        {
            var users = await context.Set<User>()
                .Include(u => u.Person)
                .Select(u => new UserResponse
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Names = u.Person.Names,
                    LastNames = u.Person.LastNames,
                    CellPhone = u.Person.CellPhone,
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync();

            if (!users.Any())
                return Result.Failure("No users found.");

            return Result.Success(users.AsEnumerable());
        }
    }
}
