using System.Xml.Serialization;

namespace ExchangeRateGateway.Models {
    public class ExchangeRate {
        [XmlAttribute("currency")]
        public string? Currency { get; set; }
        [XmlAttribute("rate")]
        public decimal Rate { get; set; }
    }
}
