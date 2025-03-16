using System.Xml.Serialization;

namespace ExchangeRateGateway.Models {
    [XmlRoot("Envelope")]
    public  class ExchangeRateResponse {
        [XmlElement("Cube")]
        public ExchangeRateCube? EnvelopeCube { get; set; }
    }
}
