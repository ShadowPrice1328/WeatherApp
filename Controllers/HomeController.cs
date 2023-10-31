using WeatherApp.Config;
using Microsoft.AspNetCore.Mvc;
using WeatherApp.Models;
using WeatherApp.Services;

namespace WeatherApp.Controllers 
{
    public class HomeController : Controller
    {
        private readonly IApiService _apiservice;
        public HomeController(IApiService apiservice)
        {
            _apiservice = apiservice;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Route("{city}")]
        [Route("Home/Search")]
        public async Task<IActionResult> Search(string city)
        {
            if (!string.IsNullOrEmpty(city))
            {
                city = city.Trim();
                ViewData["City"] = city;

                // WeatherResponse weather = await _apiservice.GetWeatherResponse(city);

                return View("Index", await _apiservice.GetJsonResponse(city));
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}