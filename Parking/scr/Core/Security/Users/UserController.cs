using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Parking.API.scr.Core.Security.Users.UseCases;
using SimpleResults;

namespace Parking.API.scr.Core.Security.Users
{
    [Authorize]
    [Route("User")]
    [ApiController]
    public class UserController
    {
        /// <summary>
        /// Creates a new basic user.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType<Result>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<Result>(StatusCodes.Status409Conflict)]
        [HttpPost]
        public async Task<Result> Create(
        [FromBody] CreateBasicUserRequest request,
        CreateBasicUserUseCase useCase)
        {
            return await useCase.ExecuteAsync(request);
        }

        /// <summary>
        /// Allows the basic user or employee to authenticate.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType<Result>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<Result>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<Result>(StatusCodes.Status403Forbidden)]
        [AllowAnonymous]
        [EnableRateLimiting("auth")]
        [Route("login")]
        [HttpPost]
        public async Task<Result<UserLoginResponse>> Login(
            [FromBody] UserLoginRequest request,
            UserLoginUseCase useCase)
            => await useCase.ExecuteAsync(request);

        /// <summary>
        /// Renews the access token for an authenticated user.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType<Result>(StatusCodes.Status401Unauthorized)]
        [AllowAnonymous]
        [EnableRateLimiting("auth")]
        [HttpPost("refresh")]
        public async Task<Result<RefreshTokenResponse>> Refresh(
            [FromBody] RefreshTokenRequest request,
            RefreshTokenUseCase useCase)
            => await useCase.ExecuteAsync(request);

        /// <summary>
        /// Revokes the refresh token of the authenticated user.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType<Result>(StatusCodes.Status404NotFound)]
        [AllowAnonymous]
        [HttpPost("revoke")]
        public async Task<Result> Revoke(
            [FromBody] RevokeTokenRequest request,
            RevokeTokenUseCase useCase)
            => await useCase.ExecuteAsync(request);

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<Result<IEnumerable<UserResponse>>> GetAll(
        GetAllUsersUseCase useCase)
        {
            return await useCase.ExecuteAsync();
        }

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType<Result>(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<Result> Delete(
        int id,
        DeleteUserUseCase useCase)
        {
            return await useCase.ExecuteAsync(id);
        }
    }
}
