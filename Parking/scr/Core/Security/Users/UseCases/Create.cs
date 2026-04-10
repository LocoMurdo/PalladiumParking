using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities;
using Parking.API.scr.Shared.Interfaces;
using Parking.API.scr.Shared.Interfaces.Persistence.Repositories;
using Parking.API.scr.Shared.Resource.ApiResponse;
using Parking.API.scr.Shared.ValidationRules;
using SimpleResults;
using System.Data;

namespace Parking.API.scr.Core.Security.Users.UseCases
{
    public class CreateBasicUserRequest
    {
        public string UserName { get; init; }
       
        public string Password { get; init; }
        public string Names { get; init; }
        public string LastNames { get; init; }

        public string CellPhone { get; init; }
        public UserRole Role { get; init; } = UserRole.Collaborator;

        public User MapToUser(string password)
        {
            var person = new Person
            {
               
                Names = Names,
                LastNames = LastNames,
                CellPhone = CellPhone,
                Email = UserName,
                
            };
            var user = new User
            {
                UserName = UserName,
                Password = password,
                Role = Role,
                Person = person
            };
            return user;
        }

    }
    public class CreatedBasicUserValidatior : AbstractValidator<CreateBasicUserRequest>
    {
        public CreatedBasicUserValidatior()
        {
            RuleFor(request => request.UserName)
                .NotEmpty()
                .EmailAddress();
            RuleFor(request => request.Password)
                .NotEmpty()
                .MustBeSecurePassword();
            RuleFor(request => request.Names).NotEmpty();
          
            RuleFor(request => request.LastNames).NotEmpty();
           
        }
    }
    public class CreateBasicUserUseCase(
            DbContext context,
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            CreatedBasicUserValidatior validator)


    {
        public async Task<Result> ExecuteAsync(CreateBasicUserRequest request)
        {
            var result = validator.Validate(request);
            if (result.IsFailed())
                return result.Invalid();
            if (await userRepository.UserExistAsync(request.UserName))
                return Result.Conflict(messages.UsernameAlreadyExists);
            var passwordHash = passwordHasher.HashPassword(request.Password);
            var user = request.MapToUser(passwordHash);
           context.Add(user);
            await context.SaveChangesAsync();
            return Result.CreatedResource(messages.CreateBasicUserAccount);

        }
    }
}
