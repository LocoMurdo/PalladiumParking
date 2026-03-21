using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parking.API.scr.Core.ParkingSessionUseCase.UseCase;
using SimpleResults;

namespace Parking.API.scr.Core.ParkingSessionUseCase
{
    [Authorize]
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
        [HttpGet("GetParkingsession")]
        public async Task<Result<IEnumerable<OpenParkingSessionResponse>>> GetOpen(
    GetOpenParkingSessionsUseCase useCase)
        {
            return await useCase.ExecuteAsync();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("history")]
        public async Task<Result<IEnumerable<ParkingSessionDetailResponse>>> GetAll(
        GetAllParkingSessionsUseCase useCase)
        {
            return await useCase.ExecuteAsync();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType<Result>(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<Result<ParkingSessionDetailResponse>> GetById(
        int id,
        GetParkingSessionByIdUseCase useCase)
        {
            return await useCase.ExecuteAsync(id);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType<Result>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<Result>(StatusCodes.Status409Conflict)]
        [HttpPost("{id}/cancel")]
        public async Task<Result> Cancel(
        int id,
        CancelParkingSessionUseCase useCase)
        {
            return await useCase.ExecuteAsync(id);
        }
    }
}
