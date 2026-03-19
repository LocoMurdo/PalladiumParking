using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parking.API.scr.Core.RateUseCase.UseCase;
using Parking.API.scr.Core.VehicleUseCase.UseCases;
using SimpleResults;

namespace Parking.API.scr.Core.RateUseCase
{
    [Route("rates")]
    [ApiController]
    public class RateController 
    {

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType<Result>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<Result>(StatusCodes.Status409Conflict)]
        [AllowAnonymous]
        [HttpPost]
        public async Task<Result> Create(
        [FromBody] CreateRateRequest request,
        CreateRateUseCase useCase)
        {
            return await useCase.ExecuteAsync(request);
        }
    }
}
