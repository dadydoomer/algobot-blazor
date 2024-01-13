using Algo.Bot.Application.Ports.Storage;
using Algo.Bot.Domain.Models;
using Algo.Bot.Infrastructure.Adapters;
using Algo.Bot.Infrastructure.Adapters.Storages;
using BlazorAlgoBot.Server.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Algobot.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CoinController : ControllerBase
    {
        private readonly ICoinStorage _coinStorage;

        public CoinController(ICoinStorage coinStorage)
        {
            _coinStorage = coinStorage;
        }

        [Authorize(Policy = ApiKeySchemeOptions.PolicyName)]
        [HttpGet]
        public async Task<IActionResult> GetSymbols()
        {
            var coins = await _coinStorage.Get();
            var candles = coins.Select(x => x.Symbol)?.ToList() ?? new List<string>();

            return Ok(candles);
        }

        [Authorize(Policy = ApiKeySchemeOptions.PolicyName)]
        [HttpGet("{symbol}")]
        public async Task<IActionResult> GetCandles([FromRoute]string symbol)
        {
            var coin = await _coinStorage.GetBySymbol(symbol);

            if (coin is null)
            {
                return NotFound();
            }

            return Ok(coin.Candles ?? new List<Candle>());
        }
    }
}
