using ECB_ExchangeRateProvider.Models;
using ECB_ExchangeRateProvider.Repositories;
using ExchangeRateGateway.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Quartz;

namespace ECB_ExchangeRateProvider.Jobs {
    public class UpdateExchangeRatesJob(
        IExchangeRateService exchangeRateService,
        IExchangeRateDbRepository exchangeRateDbRepository,
        IMemoryCache memoryCache) : IJob {
        private readonly IExchangeRateService _exchangeRateService = exchangeRateService;
        private readonly IExchangeRateDbRepository _exchangeRateDbRepository = exchangeRateDbRepository;
        private readonly IMemoryCache _memoryCache = memoryCache;

        public async Task Execute(IJobExecutionContext context) {
            var exchangeRates = await _exchangeRateService.GetLatestRateAsync();

            if (exchangeRates == null) {
                Console.WriteLine("Error: Failed to fetch exchange rates from the API.");
                return;
            }

            var cachedRates = new Dictionary<string, decimal>();

            foreach (var rateDate in exchangeRates.EnvelopeCube!.ExchangeRates!) {
                foreach (var exchangeRate in rateDate.Rates!) {

                    var exchangeRateModel = new ExchangeRateModel() {
                        Date = DateTime.Parse(rateDate.Date!).Date,
                        Rate = exchangeRate.Rate,
                        Currency = exchangeRate.Currency,
                    };

                    await _exchangeRateDbRepository.MergeExchangeRateAsync(exchangeRateModel);

                    cachedRates[exchangeRate.Currency!] = exchangeRate.Rate;
                }
            }

            _memoryCache.Set("ExchangeRates", cachedRates, TimeSpan.FromHours(1));
        }
    }
}
