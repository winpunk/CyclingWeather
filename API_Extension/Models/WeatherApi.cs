using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace API_Extension.Models
{
    public class WeatherApi : IApi
    {
        private HttpClient _httpClient;
        private readonly string baseUrl = "";
        private readonly string apiKey = "";

        public WeatherApi(IOptions<WeatherApiConfig> config, HttpClient httpClient)
        {
            _httpClient = httpClient;
            baseUrl = config.Value.weatherApiUrl;
            apiKey = config.Value.weatherApiKey;

            _httpClient.BaseAddress = new Uri(baseUrl);
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<WeatherData> getData(string city)
        {
            Uri requestUri = new Uri($"?appid={apiKey}&q={HttpUtility.UrlEncode(city)}&units=metric", UriKind.Relative);

            var response = await _httpClient.GetAsync(requestUri);
            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<WeatherData>(content);
        }
    }
}