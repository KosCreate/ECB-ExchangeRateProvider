using ECB_ExchangeRateProvider.Models;

namespace ECB_ExchangeRateProvider.Repositories {
    public interface IExchangeRateDbRepository {
        Task MergeExchangeRateAsync(ExchangeRateModel rate);
        public Task<decimal?> GetLatestExchangeRateAsync(string currency);
    }
}
