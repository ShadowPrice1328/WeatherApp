using Models;

namespace ServiceContracts
{
    public interface IWeatherService
    {
        Task<string?> GetJsonResponseAsync(string city);
        Task<WeatherResponse> GetWeatherResponseAsync(string city);
    }
}
