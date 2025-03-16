using System.Xml.Serialization;

namespace ExchangeRateGateway.Models {
    public class ExchangeRateCube {
        [XmlElement("Cube")]
        public List<ExchangeRateDate>? ExchangeRates { get; set; }
    }
}
