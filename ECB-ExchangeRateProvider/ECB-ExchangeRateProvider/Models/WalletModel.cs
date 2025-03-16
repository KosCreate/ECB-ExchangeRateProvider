namespace ECB_ExchangeRateProvider.Models {
    public class WalletModel {
        public long Id { get; set; }
        public decimal Balance { get; set; }
        public string? Currency { get; set; } // e.g., "USD", "EUR"
    }
}
