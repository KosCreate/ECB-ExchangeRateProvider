using ECB_ExchangeRateProvider.DBContexts;
using ExchangeRateGateway.Interfaces;
using Quartz;

namespace ECB_ExchangeRateProvider.Jobs {
    public class UpdateExchangeRatesJob(IExchangeRateService exchangeRateService, ExchangeRateDBContext exchangeRateDBContext) : IJob{
        private readonly IExchangeRateService _exchangeRateService = exchangeRateService;
        private readonly ExchangeRateDBContext _exchangeRateDBContext = exchangeRateDBContext;

        public async Task Execute(IJobExecutionContext context) {
            var exchangeRates = await _exchangeRateService.GetLatestRateAsync();

            if (exchangeRates == null) {
                return;
            }

            using var transaction = await _exchangeRateDBContext.Database.BeginTransactionAsync();

            foreach (var rateDate in exchangeRates.EnvelopeCube!.ExchangeRates!) {

            }
        }
    }
}
