using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parking.API.scr.Core.SubscriptionUseCase.UseCase;
using SimpleResults;

namespace Parking.API.scr.Core.SubscriptionUseCase
{
    [Authorize]
    [Route("subscriptions")]
    [ApiController]
    public class SubscriptionController
    {
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType<Result>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<Result>(StatusCodes.Status409Conflict)]
        [HttpPost]
        public async Task<Result<CreateSubscriptionResponse>> Create(
        [FromBody] CreateSubscriptionRequest request,
        CreateSubscriptionUseCase useCase)
        {
            return await useCase.ExecuteAsync(request);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<Result<IEnumerable<SubscriptionResponse>>> GetAll(
        GetAllSubscriptionsUseCase useCase)
        {
            return await useCase.ExecuteAsync();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("active")]
        public async Task<Result<IEnumerable<SubscriptionResponse>>> GetActive(
        GetActiveSubscriptionsUseCase useCase)
        {
            return await useCase.ExecuteAsync();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("check/{plate}")]
        public async Task<Result<CheckSubscriptionResponse>> CheckByPlate(
        string plate,
        CheckSubscriptionByPlateUseCase useCase)
        {
            return await useCase.ExecuteAsync(plate);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [AllowAnonymous]
        [HttpGet("prices")]
        public async Task<Result<IEnumerable<SubscriptionPriceResponse>>> GetPrices(
        GetSubscriptionPricesUseCase useCase)
        {
            return await useCase.ExecuteAsync();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType<Result>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<Result>(StatusCodes.Status409Conflict)]
        [HttpPost("{id}/cancel")]
        public async Task<Result> Cancel(
        int id,
        CancelSubscriptionUseCase useCase)
        {
            return await useCase.ExecuteAsync(id);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType<Result>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<Result>(StatusCodes.Status409Conflict)]
        [HttpPost("{id}/close")]
        public async Task<Result<CloseSubscriptionResponse>> Close(
            int id,
            [FromServices] CloseSubscriptionUseCase useCase)
        {
            return await useCase.ExecuteAsync(id);
        }
    }
}
