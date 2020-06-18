using API_Extension.Models;
using API_Extension.Views;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ApiUnitTests
{
    public class Tests
    {
        [Test]
        public async Task weatherApiGetsData()
        {
            // ARRANGE
            var mockOptions = new Mock<IOptions<WeatherApiConfig>>();
            mockOptions.Setup(m => m.Value)
                .Returns(new WeatherApiConfig()
                {
                    weatherApiUrl = "http://testapi.com/",
                    weatherApiKey = "123456789"
                });

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent(JsonConvert.SerializeObject(new FakeWeatherData()))
               })
               .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object);

            var weatherApi = new WeatherApi(mockOptions.Object, httpClient);

            //ACT
            WeatherData result = await weatherApi.getData("Vilnius");

            // ASSERT
            Assert.IsNotNull(result);
            Assert.AreEqual("Vilnius", result.name);
            Assert.AreEqual(200, result.cod);

            var expectedUri = new Uri("http://testapi.com/?appid=123456789&q=Vilnius&units=metric");

            handlerMock.Protected().Verify(
               "SendAsync",
               Times.Exactly(1),
               ItExpr.Is<HttpRequestMessage>(req =>
                  req.Method == HttpMethod.Get
                  && req.RequestUri == expectedUri
               ),
               ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public void cyclabilityIsValidating()
        {
            //ARRANGE
            var cyclability = new Cyclability();

            var cyclableFakeWeatherData = new FakeWeatherData()
            {
                main = new Main() { temp = 20 },
                weather = new Weather[1] { new Weather() { id = 800 } },
                wind = new Wind() { speed = 2 }
            };

            var notCyclableFakeWeatherData = new FakeWeatherData()
            {
                main = new Main() { temp = 35 },
                weather = new Weather[1] { new Weather() { id = 300 } },
                wind = new Wind() { speed = 25 }
            };

            //ACT

            var isCyclableGoodWeather = cyclability.Validate(cyclableFakeWeatherData);
            var isCyclableBadWeather = cyclability.Validate(notCyclableFakeWeatherData);

            //ASSERT
            Assert.AreEqual(true, isCyclableGoodWeather);
            Assert.AreEqual(false, isCyclableBadWeather);
        }

        [Test]
        public void isCyclingConditionsResponseGeneratesResponse()
        {
            //ARRANGE
            var jsonResponse = new CyclingConditionsResponse();

            var cyclableFakeWeatherData = new FakeWeatherData()
            {
                main = new Main() { temp = 20 },
                weather = new Weather[1] { new Weather() { main = "Clear" } },
                wind = new Wind() { speed = 2, deg = 90 }
            };

            //ACT
            var result = jsonResponse.Create(true, cyclableFakeWeatherData);

            //ASSERT
            Assert.IsNotNull(result);
            Assert.AreEqual(20, result.Value.temperature);
            Assert.AreEqual("Clear", result.Value.weatherCondition);
            Assert.AreEqual(2, result.Value.windSpeed);
            Assert.AreEqual(90, result.Value.windDirectionDeg);
        }
    }
}