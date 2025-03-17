using ECB_ExchangeRateProvider.Models;

namespace ECB_ExchangeRateProvider.Repositories {
    public interface IExchangeRateDbRepository {
        Task MergeExchangeRatesAsync(List<ExchangeRateModel> rates);
        public Task<decimal?> GetLatestExchangeRateAsync(string currency);
    }
}
