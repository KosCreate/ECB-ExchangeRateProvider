using ECB_ExchangeRateProvider.Models;
using ECB_ExchangeRateProvider.Repositories;
using ExchangeRateGateway.Interfaces;
using Quartz;

namespace ECB_ExchangeRateProvider.Jobs {
    public class UpdateExchangeRatesJob(
        IExchangeRateService exchangeRateService,
        IExchangeRateDbRepository exchangeRateDbRepository) : IJob {
        private readonly IExchangeRateService _exchangeRateService = exchangeRateService;
        private readonly IExchangeRateDbRepository _exchangeRateDbRepository = exchangeRateDbRepository;

        public async Task Execute(IJobExecutionContext context) {
            var exchangeRates = await _exchangeRateService.GetLatestRateAsync();

            if (exchangeRates == null) {
                Console.WriteLine("Error: Failed to fetch exchange rates from the API.");
                return;
            }

            foreach (var rateDate in exchangeRates.EnvelopeCube!.ExchangeRates!) {
                foreach (var exchangeRate in rateDate.Rates!) {
                    var exchangeRateModel = new ExchangeRateModel() {
                        Date = DateTime.Parse(rateDate.Date!).Date,
                        Rate = exchangeRate.Rate,
                        Currency = exchangeRate.Currency,
                    };

                    await _exchangeRateDbRepository.MergeExchangeRateAsync(exchangeRateModel);
                }
            }
        }
    }
}
