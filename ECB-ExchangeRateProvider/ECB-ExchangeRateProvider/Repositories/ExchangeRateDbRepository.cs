using AutoMapper;
using ECB_ExchangeRateProvider.DBContexts;
using ECB_ExchangeRateProvider.Models;
using ExchangeRateGateway.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ECB_ExchangeRateProvider.Repositories {
    public class ExchangeRateDbRepository(
        ExchangeDbContext exchangeRateDBContext,
        IMemoryCache memoryCache) : IExchangeRateDbRepository {
        private readonly ExchangeDbContext _exchangeRateDBContext = exchangeRateDBContext;
        private readonly IMemoryCache _memoryCache = memoryCache;

        public async Task<decimal?> GetLatestExchangeRateAsync(string currency) {
            if(_memoryCache.TryGetValue("ExchangeRates", out Dictionary<string, decimal>? rates)) {
                var hasValue = rates!.TryGetValue(currency, out var ratesValue);
                return hasValue ? ratesValue : null;
            }

            var exchangeRate = await _exchangeRateDBContext.ExchangeRates.FirstOrDefaultAsync(item => item.Currency!.Equals(currency));

            if (exchangeRate == null) {
                return null;
            }

            return exchangeRate.Rate;
        }

        public async Task MergeExchangeRateAsync(ExchangeRateModel rate) {
            // Ensure Date is within SQL Server valid range
            rate.Date = DateTime.Now;

            await _exchangeRateDBContext.Database.ExecuteSqlRawAsync(@"
                    MERGE INTO ExchangeRates AS target
                    USING (SELECT @Date AS Date, @Currency AS Currency, @Rate AS Rate) AS source
                    ON target.Date = source.Date AND target.Currency = source.Currency
                    WHEN MATCHED THEN
                        UPDATE SET target.Rate = source.Rate
                    WHEN NOT MATCHED THEN
                        INSERT (Date, Currency, Rate) VALUES (source.Date, source.Currency, source.Rate);",
            new SqlParameter("@Date", rate.Date),
            new SqlParameter("@Currency", rate.Currency),
            new SqlParameter("@Rate", rate.Rate));
        }

    }
}
