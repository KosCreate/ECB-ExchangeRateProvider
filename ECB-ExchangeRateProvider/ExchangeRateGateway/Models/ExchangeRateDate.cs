using System.Xml.Serialization;

namespace ExchangeRateGateway.Models {
    public class ExchangeRateDate {
        [XmlAttribute("time")]
        public string? Date { get; set; }

        [XmlAttribute("Cube")]
        public List<ExchangeRate>? Rates { get; set; }
    }
}
