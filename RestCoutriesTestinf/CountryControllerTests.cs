using Microsoft.AspNetCore.Mvc;
using Moq;
using RestCountries.Api.Controllers;
using RestCountries.Application.Interfaces;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestCountries.Api.Tests
{
    [TestFixture]
    public class CountryControllerTests
    {
        private Mock<ICountryService> _countryServiceMock;
        private CountryController _controller;

        [SetUp]
        public void Setup()
        {
            _countryServiceMock = new Mock<ICountryService>();
            _controller = new CountryController(_countryServiceMock.Object);
        }

        [Test]
        public async Task Test_GetCountryByName()
        {
            // Arrange
            var countryName = "Brazil";
            _countryServiceMock.Setup(s => s.GetCountryByName(countryName)).ReturnsAsync(new List<Country>());

            // Act
            var result = await _controller.GetCountryByName(countryName);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Test_GetCountryByCode()
        {
            // Arrange
            var countryCode = "br";
            _countryServiceMock.Setup(s => s.GetCountryByCode(countryCode)).ReturnsAsync(new List<Country>());

            // Act
            var result = await _controller.GetCountryByCode(countryCode);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Test_GetCountriesByCurrency()
        {
            // Arrange
            var currency = "brl";
            _countryServiceMock.Setup(s => s.GetCountriesByCurrency(currency)).ReturnsAsync(new List<Country>());

            // Act
            var result = await _controller.GetCountriesByCurrency(currency);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }
    }
}
