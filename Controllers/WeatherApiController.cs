using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Models;
using ServiceContracts;

namespace WeatherApp.Controllers
{
    [Route("api")]
    [ApiController]
    public class WeatherApiController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(30);
        public WeatherApiController(IWeatherService weatherService, IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _weatherService = weatherService;
        }

        [HttpGet("forecast/{city}")]
        public async Task<IActionResult> GetWeather(string city)
        {
            if (string.IsNullOrEmpty(city))
            {
                return BadRequest("City name is required.");
            }

            if (!_memoryCache.TryGetValue(city, out WeatherResponse? weatherResponse))
            {
                weatherResponse = await _weatherService.GetWeatherResponseAsync(city);
                _memoryCache.Set(city, weatherResponse, _cacheDuration);
            }

            if (weatherResponse == null)
            {

                return NotFound("Weather data not found.");
            }

            return Ok(weatherResponse);
        }

        [HttpGet("date/{city}/{date:length(10)}")]
        public async Task<IActionResult> GetWeatherForDayAsync(string city, string date)
        {
            if (string.IsNullOrEmpty(city))
            {
                return BadRequest("City name is required.");
            }

            if (!_memoryCache.TryGetValue(city, out WeatherResponse? weatherResponse))
            {
                weatherResponse = await _weatherService.GetWeatherResponseAsync(city);
                _memoryCache.Set(city, weatherResponse, _cacheDuration);
            }

            if (weatherResponse == null)
            {
                return NotFound("Weather data not found.");
            }

            var forecastForDate = weatherResponse.list.Where(l => l.dt_txt.StartsWith(date));

            if (forecastForDate == null)
            {
                return NotFound("No weather data available for the specified date.");
            }

            return Ok(forecastForDate);
        }

        [HttpGet("current/{city}")]
        public async Task<IActionResult> GetWeatherForToday(string city)
        {
            if (string.IsNullOrEmpty(city))
            {
                return BadRequest("City name is required.");
            }

            if (!_memoryCache.TryGetValue(city, out WeatherResponse? weatherResponse))
            {
                weatherResponse = await _weatherService.GetWeatherResponseAsync(city);
                _memoryCache.Set(city, weatherResponse, _cacheDuration);
            }

            if (weatherResponse == null)
            {
                return NotFound("Weather data not found.");
            }

            var current = weatherResponse.list.FirstOrDefault();

            if (current == null)
            {
                return NotFound("Weather data not found.");
            }
            
            return Ok(current);
        }
    }
}
