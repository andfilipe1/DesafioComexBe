using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Distributed;
using RestCountries.Infrastructure.Caching;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text;

namespace RestCountries.UnitTests
{
    [TestFixture]
    public class CountryCacheTests
    {
        private Mock<IDistributedCache> _distributedCacheMock;
        private CountryCache _countryCache;

        [SetUp]
        public void Setup()
        {
            _distributedCacheMock = new Mock<IDistributedCache>();
            _countryCache = new CountryCache(_distributedCacheMock.Object);
        }

        [Test]
        public async Task GetCountriesFromCache_ReturnsNull_WhenNoCountriesCached()
        {
            // Arrange
            byte[] nullBytes = null;
            _distributedCacheMock.Setup(s => s.GetAsync(It.IsAny<string>(), default)).ReturnsAsync(nullBytes);

            // Act
            var result = await _countryCache.GetCountriesFromCache();

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetCountriesFromCache_ReturnsCountryList_WhenCountriesCached()
        {
            // Arrange
            var expectedCountries = new List<Country>
                {
                    new Country
                    {
                        name = new Name { common = "Country1", official = "Country1" },
                        cca3 = "Code1"
                    },
                    new Country
                    {
                        name = new Name { common = "Country2", official = "Country2" },
                        cca3 = "Code2"
                    },
                };

            var jsonString = JsonSerializer.Serialize(expectedCountries);
            var jsonData = Encoding.UTF8.GetBytes(jsonString);
            _distributedCacheMock.Setup(s => s.GetAsync(It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(jsonData);

            // Act
            var result = await _countryCache.GetCountriesFromCache();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Country1", result[0].name.common);
            Assert.AreEqual("Code1", result[0].cca3);
            Assert.AreEqual("Country2", result[1].name.common);
            Assert.AreEqual("Code2", result[1].cca3);
        }


        // Similar tests could be written for other methods
    }
}
