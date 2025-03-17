using ExchangeRateGateway.Interfaces;
using ExchangeRateGateway.Models;
using System.Xml;
using System.Xml.Serialization;

namespace ExchangeRateGateway {
    public class ExchangeRateService(HttpClient httpClient) : IExchangeRateService {
        private readonly HttpClient _httpClient = httpClient;

        /// <summary>
        /// Accesses available exchange rates from https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml
        /// parses the xml response and returns the resulting object
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ExchangeRateResponse> GetLatestRateAsync() {
            try {
                var response = await _httpClient.GetStringAsync("https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml");

                var xmlSerializer = new XmlSerializer(typeof(ExchangeRateResponse));

                using var stringReader = new StringReader(response);
                using var xmlReader = XmlReader.Create(stringReader, new XmlReaderSettings { IgnoreWhitespace = true });
                var exchangeRate = (ExchangeRateResponse)xmlSerializer.Deserialize(xmlReader)!;
                return exchangeRate;
            }
            catch (InvalidOperationException ex) {
                throw new Exception($"XML Deserialization Error: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex) {
                throw new Exception($"Error: Failed to fetch exchange rates. Exception: {ex.Message}");
            }
        }
    }
}
