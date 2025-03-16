using ExchangeRateGateway.Interfaces;
using ExchangeRateGateway.Models;
using System.Xml;
using System.Xml.Serialization;

namespace ExchangeRateGateway {
    public class ExchangeRateService(HttpClient httpClient) : IExchangeRateService {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<ExchangeRateResponse> GetLatestRateAsync() {
            var response = await _httpClient.GetStringAsync("https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml");

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(response);
            var xmlRoot = xmlDoc.DocumentElement;

            var serializer = new XmlSerializer(typeof(ExchangeRateResponse), new XmlRootAttribute(xmlRoot!.Name));
            using var stringReader = new StringReader(xmlDoc.OuterXml);
            return (ExchangeRateResponse)serializer.Deserialize(stringReader)!;
        }
    }
}
