namespace ECB_ExchangeRateProvider.DTO {
    public class WalletDto {
        public decimal Balance { get; set; }
        public string? Currency { get; set; } // e.g., "USD", "EUR"
    }
}
