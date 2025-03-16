using System.Xml.Serialization;

namespace ExchangeRateGateway.Models {
    [XmlRoot("Envelope", Namespace = "http://www.gesmes.org/xml/2002-08-01")]
    public class ExchangeRateResponse {
        [XmlElement("Cube", Namespace = "http://www.ecb.int/vocabulary/2002-08-01/eurofxref")]
        public ExchangeRateCube? EnvelopeCube { get; set; }
    }
}
