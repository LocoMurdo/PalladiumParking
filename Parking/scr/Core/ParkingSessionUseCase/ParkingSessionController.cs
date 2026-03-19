using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parking.API.scr.Core.ParkingSessionUseCase.UseCase;
using Parking.API.scr.Core.VehicleUseCase.UseCases;
using SimpleResults;

namespace Parking.API.scr.Core.ParkingSessionUseCase
{
    [Route("ParkingSeassion")]
    [ApiController]
    public class ParkingSessionController
    {
        /// <summary>
        /// Creates a new parkingseassion.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType<Result>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<Result>(StatusCodes.Status409Conflict)]
        [AllowAnonymous]
        [HttpPost]
        public async Task<Result> Create(
        [FromBody] CreateParkingSessionRequest request,
        CreateParkingSessionUseCase useCase)
        {
            return await useCase.ExecuteAsync(request);
        }

        /// <summary>
        /// Cerrar sesión de parqueo
        /// </summary>
        [ProducesResponseType(typeof(CloseParkingSessionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType<Result>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<Result>(StatusCodes.Status409Conflict)]
        [AllowAnonymous]
        [HttpPost("CloseParkingSession")]
        public async Task<Result<CloseParkingSessionResponse>> Close(
    [FromBody] CloseParkingSessionRequest request,
    CloseParkingSessionUseCase useCase)
        {
            return await useCase.ExecuteAsync(request);
        }




        [ProducesResponseType(typeof(CloseParkingSessionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType<Result>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<Result>(StatusCodes.Status409Conflict)]
        [AllowAnonymous]
        [HttpGet("GetParkingsession")]
        public async Task<Result<IEnumerable<OpenParkingSessionResponse>>> GetOpen(
    GetOpenParkingSessionsUseCase useCase)
        {
            return await useCase.ExecuteAsync();
        }

    }
}
