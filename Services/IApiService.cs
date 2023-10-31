using WeatherApp.Models;

namespace WeatherApp.Services
{
    public interface IApiService
    {
        Task<string?> GetJsonResponse(string city);
        Task<WeatherResponse> GetWeatherResponse(string city);
    }
}