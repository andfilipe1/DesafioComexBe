using Moq;
using RestCountries.Application.Interfaces;
using RestCountries.Application.Services;
using RestCountries.Domain.Interfaces;

namespace RestCountries.Application.Tests
{
    [TestFixture]
    public class CountryServiceTests
    {
        private Mock<ICountryCache> _countryCacheMock;
        private Mock<IRestCountriesApiClient> _restCountriesApiClientMock;
        private CountryService _countryService;

        [SetUp]
        public void Setup()
        {
            _countryCacheMock = new Mock<ICountryCache>();
            _restCountriesApiClientMock = new Mock<IRestCountriesApiClient>();
            _countryService = new CountryService(_countryCacheMock.Object, _restCountriesApiClientMock.Object);
        }

        [Test]
        public async Task GetAllCountries_ReturnsCountries()
        {
            // Arrange
            var expectedCountries = new List<Country>();
            _restCountriesApiClientMock.Setup(s => s.GetRestCountriesAll()).ReturnsAsync(expectedCountries);

            // Act
            var result = await _countryService.GetAllCountries();

            // Assert
            Assert.AreEqual(expectedCountries, result);
        }

        [Test]
        public async Task GetCountryByCountry_ReturnsCountry()
        {
            // Arrange
            var countryName = "Brazil";
            var expectedCountries = new List<Country>();
            _countryCacheMock.Setup(s => s.GetCountriesFromCacheByName(countryName)).ReturnsAsync((List<Country>)null);
            _restCountriesApiClientMock.Setup(s => s.GetRestCountriesByName(countryName)).ReturnsAsync(expectedCountries);

            // Act
            var result = await _countryService.GetCountryByName(countryName);

            // Assert
            Assert.AreEqual(expectedCountries, result);
        }

        [Test]
        public async Task GetCountryByCode_ReturnsCode()
        {
            // Arrange
            var code = "BRA";
            var expectedCountries = new List<Country>();
            _countryCacheMock.Setup(s => s.GetCountriesFromCacheByCode(code)).ReturnsAsync((List<Country>)null);
            _restCountriesApiClientMock.Setup(s => s.GetRestCountriesByCode(code)).ReturnsAsync(expectedCountries);

            // Act
            var result = await _countryService.GetCountryByCode(code);

            // Assert
            Assert.AreEqual(expectedCountries, result);
        }

        [Test]
        public async Task GetCountryByCurrency_ReturnsCurrency()
        {
            // Arrange
            var currency = "BRL";
            var expectedCountries = new List<Country>();
            _countryCacheMock.Setup(s => s.GetCountriesFromCacheByCurrency(currency)).ReturnsAsync((List<Country>)null);
            _restCountriesApiClientMock.Setup(s => s.GetRestCountriesByCurrency(currency)).ReturnsAsync(expectedCountries);

            // Act
            var result = await _countryService.GetCountriesByCurrency(currency);

            // Assert
            Assert.AreEqual(expectedCountries, result);
        }

        // Para GetTradeRoute, você pode precisar de testes adicionais devido à complexidade adicional desse método.
    }
}
