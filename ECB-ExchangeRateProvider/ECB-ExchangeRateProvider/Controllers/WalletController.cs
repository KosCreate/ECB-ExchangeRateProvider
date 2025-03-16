using AutoMapper;
using ECB_ExchangeRateProvider.Data;
using ECB_ExchangeRateProvider.DTO;
using ECB_ExchangeRateProvider.Models;
using ECB_ExchangeRateProvider.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ECB_ExchangeRateProvider.Controllers {
    [ApiController]
    [Route("api/wallets")]
    public class WalletController(
        IWalletRepository walletRepository,
        IMapper mapper) : ControllerBase {
        private readonly IWalletRepository _walletRepository = walletRepository;
        private readonly IMapper _mapper = mapper;

        [HttpPost("createWallet")]
        public async Task<IActionResult> CreateWallet([FromBody] WalletDto walletDto) {
            var wallet = new WalletModel() {
                Currency = walletDto.Currency,
                Balance = walletDto.Balance,
            };

            await _walletRepository.CreateWalletAsync(wallet);

            return Ok(walletDto);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetWalletBalance([FromRoute] long id, string? currency) {
            var walletCurrency = await _walletRepository.RetrieveWalletBalanceAsync(id, currency);

            if (walletCurrency == null) {
                return NotFound("Could not find wallet");
            }

            return Ok(walletCurrency);
        }

        [HttpPost("{id:long}/adjustbalance")]
        public async Task<IActionResult> AdjustWalletBalance([FromRoute] long id, decimal amount, string currency, string strategy) {
            if(amount < 0) {
                return BadRequest("Amount should be a positive number");
            }

            if(!Enum.TryParse(strategy, out Strategy strat)) {
                return BadRequest("Strategy not valid");
            }

            var result = await _walletRepository.AdjustWalletBalanceAsync(id, amount, currency, strat);

            if(result == null) {
                return NotFound("Could not find wallet");
            }

            var walletDto = _mapper.Map<WalletDto>(result);

            return Ok(walletDto);
        }
    }
}
