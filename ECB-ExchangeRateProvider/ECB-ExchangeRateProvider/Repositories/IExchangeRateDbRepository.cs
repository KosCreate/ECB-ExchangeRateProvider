using ECB_ExchangeRateProvider.DTO;

namespace ECB_ExchangeRateProvider.Repositories {
    public interface IExchangeRateDbRepository {
        Task MergeExchangeRateAsync(ExchangeRateDto rate);
        Task<ExchangeRateDto> GetLatestExchangeRateAsync(string currency);
        Task<List<ExchangeRateDto>> GetExchangeRateHistoryAsync(string currency);
    }
}
