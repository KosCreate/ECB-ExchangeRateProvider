namespace ECB_ExchangeRateProvider.Options {
    public class JobSettings {
        public const string JobSettingsSection = "JobSettings";

        public string JobKey { get; set; } = string.Empty;
        public string JobTrigger { get; set; } = string.Empty;
    }
}
