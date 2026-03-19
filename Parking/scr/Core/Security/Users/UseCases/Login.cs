using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities;
using Parking.API.scr.Shared.Interfaces;
using Parking.API.scr.Shared.Interfaces.Persistence.Repositories;
using Parking.API.scr.Shared.Models;
using Parking.API.scr.Shared.Resource.ApiResponse;
using SimpleResults;
using System.Text.Json.Serialization;

namespace Parking.API.scr.Core.Security.Users.UseCases
{
    public class UserLoginRequest
    {
        public string UserName { get; init; }
        public string Password { get; init; }
    }
    public class UserLoginValidator : AbstractValidator<UserLoginRequest>
    {
        public UserLoginValidator()
        {
            RuleFor(request => request.UserName)
                    .NotEmpty()
                    .EmailAddress();
            RuleFor(request => request.Password).NotEmpty();

        }
    }
    // This is to identify the base type in the payload.
    [JsonDerivedType(typeof(UserLoginResponse), typeDiscriminator: "user")]
    // [JsonDerivedType(typeof(EmployeeLoginResponse), typeDiscriminator: "employee")]

    public class UserLoginResponse
    {
        public int UserId { get; set; }
        public int PersonId { get; set; }
        public string UserName { get; set; }
        public string Names { get; set; }
      
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
    public static class UserLoginMapper
    {
        private static void MapToUserLoginResponse(User user, UserLoginResponse response)
        {
            
            response.Names = user.Person.Names;
           
            response.UserId = user.Id;
            response.PersonId = user.PersonId;
            response.UserName = user.UserName;
          
                

        }
        public static UserLoginResponse MapToUserLoginResponse(this User user)
        {
            var response = new UserLoginResponse();
            MapToUserLoginResponse(user, response);
            return response;
        }
        public static UserClaims MapToUserClaims(this UserLoginResponse response)
        {
            return new()
            {
                UserId = response.UserId,
                PersonId = response.PersonId,
                UserName = response.UserName,
              
            };
        }
    }
    public class UserLoginUseCase(
        DbContext context,
        IUserRepository userRepository,
        ITokenService tokenService,
        IPasswordHasher passwordHasher,
        UserLoginValidator validator)
    {
        public async Task<Result<UserLoginResponse>> ExecuteAsync(UserLoginRequest request)
        {
            var result = validator.Validate(request);
            if (result.IsFailed())
                return result.Invalid();

            var user = await userRepository.GetFullUserProfileAsync(request.UserName);
            if (user is null || !passwordHasher.Verify(request.Password, user.Password))
                return Result.Unauthorized(messages.EmailOrPasswordIncorrect);

            

            user.RefreshToken = tokenService.CreateRefreshToken();
            user.RefreshTokenExpiry = tokenService.CreateExpiryForRefreshToken();
            await context.SaveChangesAsync();

           
                var userLoginResponse = user.MapToUserLoginResponse();
                var userClaims = userLoginResponse.MapToUserClaims();
                userLoginResponse.AccessToken = tokenService.CreateAccessToken(userClaims);
                userLoginResponse.RefreshToken = user.RefreshToken;
                return Result.Success(userLoginResponse, messages.SuccessfulLogin);
            }
           

    }
}
