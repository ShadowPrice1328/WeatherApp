using Microsoft.AspNetCore.Mvc;
using Models;
using Services;
using System.Globalization;
using Microsoft.VisualBasic;
using ServiceContracts;
using WeatherApp.ViewModels;
using Microsoft.Extensions.Caching.Memory;

namespace WeatherApp.Controllers 
{
    public class WeatherController : Controller
    {
        private readonly IWeatherService _weatherService;
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(30);
        public WeatherController(IWeatherService weatherService, IMemoryCache memoryCache)
        {
            _weatherService = weatherService;
            _memoryCache = memoryCache;
        }
        [Route("/")]
        public IActionResult Index(string city = "Kyiv")
        {
            ViewData["City"] = city;
            return RedirectToAction("Search", new {city = "Kyiv"});
        }

        [Route("Search")]
        [Route("{city}")]
        public async Task<IActionResult> Search(string city = "Kyiv")
        {
            if (!string.IsNullOrEmpty(city))
            {
                city = city.Trim();

                if (!_memoryCache.TryGetValue(city, out WeatherResponse? weather))
                {
                    try
                    {
                        weather = await _weatherService.GetWeatherResponseAsync(city);

                        _memoryCache.Set(city, weather, _cacheDuration);
                    }
                    catch (Exception)
                    {
                        if (city == "Error")
                        {
                            return View("Error");
                        }
                        return View("NotFound");
                    }
                }
                
                var FiveDaysWeather = weather?.list
                    .GroupBy(item => DateTime.ParseExact(
                    item.dt_txt, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture).Date);

                var weatherViewModel = new WeatherViewModel()
                {
                    City = city,
                    GroupedWeather = FiveDaysWeather
                };

                return View("Index", weatherViewModel);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [Route("{city}/{date:length(10)}")]
        [HttpPost("{city}/{date:length(10)}")]
        public async Task<IActionResult> ShowDetails(string city, string date)
        {
            if (string.IsNullOrEmpty(city) ||
                !DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None,
                out DateTime parsedDate))
            {
                return BadRequest("Invalid city or date format.");
            }

            if (!_memoryCache.TryGetValue(city, out WeatherResponse? weather))
            {
                weather = await _weatherService.GetWeatherResponseAsync(city);
                _memoryCache.Set(city, weather, _cacheDuration);
            }

            var FiveDaysWeather = weather?.list
                .GroupBy(item => DateTime.ParseExact(item.dt_txt, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture).Date);

            var weatherViewModel = new WeatherViewModel()
            {
                City = city,
                Date = date,
                GroupedWeather = FiveDaysWeather
            };

            return View("Index", weatherViewModel);
        }
    }
}