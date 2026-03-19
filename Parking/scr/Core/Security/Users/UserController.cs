using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [AllowAnonymous]
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
        [Route("login")]
        [HttpPost]
        public async Task<Result<UserLoginResponse>> Login(
            [FromBody] UserLoginRequest request,
            UserLoginUseCase useCase)
            => await useCase.ExecuteAsync(request);
    }
}
