using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parking.API.scr.Core.VehicleUseCase.UseCases;
using SimpleResults;

namespace Parking.API.scr.Core.VehicleUseCase
{
    [Authorize]
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
        [HttpPost]
        public async Task<Result> Create(
        [FromBody] CreateVehicleRequest request,
        CreateVehicleUseCase useCase)
        {
            return await useCase.ExecuteAsync(request);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<Result<IEnumerable<VehicleResponse>>> GetAll(
        GetAllVehiclesUseCase useCase)
        {
            return await useCase.ExecuteAsync();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType<Result>(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<Result<VehicleResponse>> GetById(
        int id,
        GetVehicleByIdUseCase useCase)
        {
            return await useCase.ExecuteAsync(id);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType<Result>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<Result>(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public async Task<Result> Update(
        int id,
        [FromBody] UpdateVehicleRequest request,
        UpdateVehicleUseCase useCase)
        {
            return await useCase.ExecuteAsync(id, request);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType<Result>(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<Result> Delete(
        int id,
        DeleteVehicleUseCase useCase)
        {
            return await useCase.ExecuteAsync(id);
        }
    }
}
