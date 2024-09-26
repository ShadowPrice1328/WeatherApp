using Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServiceContracts;
using Azure.Security.KeyVault.Secrets;
using Azure.Core;

namespace Services
{
    public class WeatherService : IWeatherService
    {
        public WeatherService(IConfiguration configuration, SecretClient secretClient)
        {
            _configuration = configuration;
            _secretClient = secretClient;

            KeyVaultSecret openWeatherApiKeySecret = _secretClient.GetSecret("OpenWeatherAppId");
            _openWeatherApiKey = openWeatherApiKeySecret.Value;
        }

        private readonly string _openWeatherApiKey;
        private readonly SecretClient _secretClient;
        private readonly IConfiguration _configuration;
        public async Task<string?> GetJsonResponseAsync(string city)
        {
            var url = $"http://api.openweathermap.org/data/2.5/forecast?q={city}&appid={_openWeatherApiKey}&units=metric";

            using HttpClient client = new();
            var responseMessage = await client.GetAsync(url);

            return responseMessage.IsSuccessStatusCode ? await responseMessage.Content.ReadAsStringAsync() : null;
        }
        public async Task<WeatherResponse> GetWeatherResponseAsync(string city)
        {
            string jsonResponse = await GetJsonResponseAsync(city) ?? throw new Exception("Failed to retrieve weather information.");

            JToken? content = JsonConvert.DeserializeObject<JToken>(jsonResponse) 
                ?? throw new InvalidOperationException("Failed to deserialize JSON response");

            WeatherResponse? weatherResponse = content.ToObject<WeatherResponse>();

            return weatherResponse == null
                ? throw new InvalidOperationException("Failed to convert JToken to WeatherResponse")
                : weatherResponse;
        }
    }
}
