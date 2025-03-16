namespace ECB_ExchangeRateProvider.Validation {
    using System.Text.RegularExpressions;
    public static class CurrencyValidator {
        private static readonly Regex CurrencyRegex = new Regex(
            @"^(USD|JPY|BGN|CZK|DKK|GBP|HUF|PLN|RON|SEK|CHF|ISK|NOK|TRY|AUD|BRL|CAD|CNY|HKD|IDR|ILS|INR|KRW|MXN|MYR|NZD|PHP|SGD|THB|ZAR)$",
            RegexOptions.Compiled
        );

        public static bool IsValidCurrency(string currencyCode) {
            return CurrencyRegex.IsMatch(currencyCode);
        }
    }
}
