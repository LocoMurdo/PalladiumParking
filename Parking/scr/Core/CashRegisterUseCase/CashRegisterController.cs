using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parking.API.scr.Core.CashRegisterUseCase.UseCase;
using SimpleResults;

namespace Parking.API.scr.Core.CashRegisterUseCase
{
    [Route("CashRegister")]
    [ApiController]
    public class CashRegisterController : Controller
    {
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType<Result>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<Result>(StatusCodes.Status409Conflict)]
        [AllowAnonymous]
        [HttpPost("open")]
        public async Task<Result> Open(
            [FromBody] OpenCashRequest request,
            OpenCashUseCase useCase)
        {
            return await useCase.ExecuteAsync(request);
        }

        [HttpPost("close")]
        public async Task<IActionResult> Close(
            CloseCashUseCase useCase)
        {
            var result = await useCase.ExecuteAsync();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            else if (result.Status == ResultStatus.Conflict)
            {
                return Conflict(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("history")]
        public async Task<IActionResult> History(
            GetCashHistoryUseCase useCase)
        {
            var result = await useCase.ExecuteAsync();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            else if (result.Status == ResultStatus.Conflict)
            {
                return Conflict(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
