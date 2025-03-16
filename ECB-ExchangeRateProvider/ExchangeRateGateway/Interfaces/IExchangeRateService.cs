using ExchangeRateGateway.Models;

namespace ExchangeRateGateway.Interfaces {
    public interface IExchangeRateService {
        Task<ExchangeRateResponse> GetLatestRateAsync();
    }
}
