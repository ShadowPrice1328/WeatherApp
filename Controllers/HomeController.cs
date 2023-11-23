using WeatherApp.Config;
using Microsoft.AspNetCore.Mvc;
using WeatherApp.Models;
using WeatherApp.Services;
using System.Globalization;
using Microsoft.VisualBasic;

namespace WeatherApp.Controllers 
{
    public class HomeController : Controller
    {
        private readonly IApiService _apiservice;
        public HomeController(IApiService apiservice)
        {
            _apiservice = apiservice;
        }
        public IActionResult Index(string city = "Kyiv")
        {
            ViewData["City"] = city;
            return RedirectToAction("Search", new {city = "Kyiv"});
        }
        [Route("{city}")]
        [Route("Home")]
        [Route("Home/Search")]
        public async Task<IActionResult> Search(string city = "Kyiv")
        {
            if (!string.IsNullOrEmpty(city))
            {
                city = city.Trim();
                ViewData["City"] = city;

                WeatherResponse weather = await _apiservice.GetWeatherResponse(city);
                
                var FiveDaysWeather = weather.list.GroupBy(item => DateTime.ParseExact(item.dt_txt, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture).Date);

                return View("Index", FiveDaysWeather);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public async Task<IActionResult> ShowDetails(string date, string city)
        {
            ViewData["selectedDay"] = date;
            ViewData["City"] = city;
            WeatherResponse weather = await _apiservice.GetWeatherResponse(city);
            var FiveDaysWeather = weather.list.GroupBy(item => DateTime.ParseExact(item.dt_txt, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture).Date);
            return View("Index", FiveDaysWeather);
        }
    }
}