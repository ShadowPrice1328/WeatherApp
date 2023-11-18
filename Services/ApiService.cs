using System.Text;
using WeatherApp.Config;
using WeatherApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WeatherApp.Services
{
    public class ApiService : IApiService
    {
        private const string Appid = Constants.OpenWeatherAppId;
        public async Task<string?> GetJsonResponse(string city)
        {
            var url = $"http://api.openweathermap.org/data/2.5/forecast?q={city}&appid={Appid}&units=metric";

            using HttpClient client = new();
            var responseMessage = await client.GetAsync(url);

            return responseMessage.IsSuccessStatusCode ? await responseMessage.Content.ReadAsStringAsync() : null;
        }
        public async Task<WeatherResponse> GetWeatherResponse(string city)
        {
            var content = JsonConvert.DeserializeObject<JToken>(await GetJsonResponse(city));
            return content.ToObject<WeatherResponse>();
        }
    }
}