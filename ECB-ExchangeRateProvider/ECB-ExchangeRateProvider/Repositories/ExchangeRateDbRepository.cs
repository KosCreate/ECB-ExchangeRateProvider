using ECB_ExchangeRateProvider.DTO;

namespace ECB_ExchangeRateProvider.Repositories {
    public class ExchangeRateDbRepository : IExchangeRateDbRepository {
        public Task<List<ExchangeRateDto>> GetExchangeRateHistoryAsync(string currency)
        {
            throw new NotImplementedException();
        }

        public Task<ExchangeRateDto> GetLatestExchangeRateAsync(string currency)
        {
            throw new NotImplementedException();
        }

        public Task MergeExchangeRateAsync(ExchangeRateDto rate)
        {
            throw new NotImplementedException();
        }
    }
}
