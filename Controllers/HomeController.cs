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

        [Route("Search")]
        [Route("{city}")]
        public async Task<IActionResult> Search(string city = "Kyiv")
        {
            if (!string.IsNullOrEmpty(city))
            {
                city = city.Trim();
                ViewData["City"] = city;
                
                WeatherResponse weather;

                try
                {
                    weather = await _apiservice.GetWeatherResponse(city);
                }
                catch (Exception)
                {   
                    // IGNORE IT PLEASE I'M BEGGING YOU IGNORE IT I CAN'T PLEASEEEEEE
                    if (city == "Error")
                    {
                        return View("Error");
                    }
                    return View("NotFound");
                }
                
                var FiveDaysWeather = weather.list.GroupBy(
                    item => DateTime.ParseExact(
                    item.dt_txt, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture).Date);

                return View("Index", FiveDaysWeather);
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
            ViewData["selectedDay"] = date;
            ViewData["City"] = city;
            WeatherResponse weather = await _apiservice.GetWeatherResponse(city);
            var FiveDaysWeather = weather.list.GroupBy(item => DateTime.ParseExact(item.dt_txt, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture).Date);
            return View("Index", FiveDaysWeather);
        }
    }
}