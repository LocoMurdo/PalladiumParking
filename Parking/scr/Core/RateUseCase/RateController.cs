using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parking.API.scr.Core.RateUseCase.UseCase;
using SimpleResults;

namespace Parking.API.scr.Core.RateUseCase
{
    [Authorize]
    [Route("rates")]
    [ApiController]
    public class RateController 
    {
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType<Result>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<Result>(StatusCodes.Status409Conflict)]
        [HttpPost]
        public async Task<Result> Create(
        [FromBody] CreateRateRequest request,
        CreateRateUseCase useCase)
        {
            return await useCase.ExecuteAsync(request);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [AllowAnonymous]
        [HttpGet]
        public async Task<Result<IEnumerable<RateResponse>>> GetAll(
        GetAllRatesUseCase useCase)
        {
            return await useCase.ExecuteAsync();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType<Result>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<Result>(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public async Task<Result> Update(
        int id,
        [FromBody] UpdateRateRequest request,
        UpdateRateUseCase useCase)
        {
            return await useCase.ExecuteAsync(id, request);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType<Result>(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<Result> Delete(
        int id,
        DeleteRateUseCase useCase)
        {
            return await useCase.ExecuteAsync(id);
        }
    }
}
