namespace ECB_ExchangeRateProvider.DTO {
    public class ExchangeRateDto {
        public string? Currency { get; set; }
        public decimal Rate { get; set; }
        public DateTime Date { get; set; }
    }
}
