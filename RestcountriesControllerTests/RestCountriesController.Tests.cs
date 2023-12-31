using Microsoft.AspNetCore.Mvc;
using Moq;
using RestCountries.Api.Controllers;
using RestCountries.Application.Interfaces;

namespace RestCountries.Api.Tests
{
    [TestClass]
    public class CountryControllerTests
    {
        private Mock<ICountryService> _countryServiceMock;
        private CountryController _controller;

        [TestInitialize]
        public void Setup()
        {
            _countryServiceMock = new Mock<ICountryService>();
            _controller = new CountryController(_countryServiceMock.Object);
        }

        [TestMethod]
        public async Task Test_GetCountryByName()
        {
            // Arrange
            var countryName = "Brazil";
            _countryServiceMock.Setup(s => s.GetCountryByName(countryName)).ReturnsAsync(new List<Country>());  // Here

            // Act
            var result = await _controller.GetCountryByName(countryName);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task Test_GetCountryByCode()
        {
            // Arrange
            var countryCode = "br";
            _countryServiceMock.Setup(s => s.GetCountryByCode(countryCode)).ReturnsAsync(new List<Country>());  // Here

            // Act
            var result = await _controller.GetCountryByCode(countryCode);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task Test_GetCountriesByCurrency()
        {
            // Arrange
            var currency = "brl";
            _countryServiceMock.Setup(s => s.GetCountriesByCurrency(currency)).ReturnsAsync(new List<Country>());  // Here

            // Act
            var result = await _controller.GetCountriesByCurrency(currency);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
    }
}
