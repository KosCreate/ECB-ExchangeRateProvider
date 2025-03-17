using ECB_ExchangeRateProvider.DBContexts;
using ECB_ExchangeRateProvider.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Data;

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

        /// <summary>
        /// Merge exchange rates to ensure fields mutate only when needed and in a single query
        /// </summary>
        /// <param name="rate"></param>
        /// <returns></returns>
        public async Task MergeExchangeRatesAsync(List<ExchangeRateModel> rates) {
            var dataTable = new DataTable();
            dataTable.Columns.Add("Date", typeof(DateTime));
            dataTable.Columns.Add("Currency", typeof(string));
            dataTable.Columns.Add("Rate", typeof(decimal));

            foreach (var rate in rates) {
                dataTable.Rows.Add(rate.Date, rate.Currency, rate.Rate);
            }

            var parameter = new SqlParameter("@ExchangeRates", dataTable) {
                TypeName = "dbo.ExchangeRateTableType",
                SqlDbType = SqlDbType.Structured
            };

            await _exchangeRateDBContext.Database.ExecuteSqlRawAsync(@"
                MERGE INTO ExchangeRates AS target
                USING @ExchangeRates AS source
                ON target.Date = source.Date AND target.Currency = source.Currency
                WHEN MATCHED THEN
                    UPDATE SET target.Rate = source.Rate
                WHEN NOT MATCHED THEN
                    INSERT (Date, Currency, Rate) VALUES (source.Date, source.Currency, source.Rate);",
            parameter);
        }
    }
}
