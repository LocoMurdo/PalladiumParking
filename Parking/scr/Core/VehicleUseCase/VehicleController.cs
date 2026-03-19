using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parking.API.scr.Core.Security.Users.UseCases;
using Parking.API.scr.Core.VehicleUseCase.UseCases;
using SimpleResults;

namespace Parking.API.scr.Core.VehicleUseCase
{
    //[Authorize]
    [Route("vehicles")]
    [ApiController]
    public class VehicleController : ControllerBase
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
        [FromBody] CreateVehicleRequest request,
        CreateVehicleUseCase useCase)
        {
            return await useCase.ExecuteAsync(request);
        }

    }
}
