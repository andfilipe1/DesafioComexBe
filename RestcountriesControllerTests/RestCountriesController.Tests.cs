//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Caching.Distributed;
//using Moq;
//using Moq.AutoMock;

//namespace RestcountriesController.Tests
//{
//    [TestClass]
//    public class RestCountriesControllerTests
//    {
//        private AutoMocker? _mocker;
//        private Mock<IGetRestCountries>? _restCountriesMock;
//        private readonly IGetRestCountries? _countries;
//        private readonly IDistributedCache? _distributedCache;
//        private RestCountriesController? _controller;

//        [TestInitialize]
//        public void Setup()
//        {
//            _mocker = new AutoMocker();
//            _restCountriesMock = _mocker.GetMock<IGetRestCountries>();
//            _controller = new RestCountriesController(_restCountriesMock.Object, _distributedCache, _countries);
//        }

//        [TestMethod]
//        public void Test_ControllerAbilityToGetOneCountryByName()
//        {
//            // Act
//            var result =  _controller.GetOneCountryByName("Brazil");
//            // Assert
//            Assert.IsNotNull(result);
//        }

//        [TestMethod]
//        public async Task Test_GetOneCountryByCode()
//        {
//            // Act
//            var result = await _controller.GetOneCountryByCode("br");
//            // Assert
//            Assert.IsNotNull(result);
//        }

//        [TestMethod]
//        public async Task Test_GetOneCountryByCurrency()
//        {
//            // Act
//            var result = await _controller.GetOneCountryByCurrency("brl");
//            // Assert
//            Assert.IsNotNull(result);
//        }
//    }
//}