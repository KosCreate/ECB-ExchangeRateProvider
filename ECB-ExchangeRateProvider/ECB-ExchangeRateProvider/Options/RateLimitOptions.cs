namespace ECB_ExchangeRateProvider.Options {
    public class RateLimitOpt {
        public const string RateLimitSection = "RateLimitOptions";

        public string EndPoints { get; set; } = string.Empty;
        public int Limit { get; set; } = int.MinValue;
        public string Period { get; set; } = string.Empty;
    }
}
