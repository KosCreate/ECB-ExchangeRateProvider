namespace ECB_ExchangeRateProvider.Models {
    public class ExchangeRateModel {
        public int Id { get; set; }
        public string? Currency {  get; set; }
        public decimal Rate { get; set; }
        public DateTime Date { get; set; }
    }
}
