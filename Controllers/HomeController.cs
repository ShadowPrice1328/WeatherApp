using Config;
using Microsoft.AspNetCore.Mvc;

namespace Controllers 
{
    public class HomeController : Controller
    {
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

                const string appid = Constants.OPEN_WEATHER_APP_ID;

                string url = $"http://api.openweathermap.org/data/2.5/forecast?q={city}&appid={appid}&units=metric";
                string responseBody = string.Empty;

                using (HttpClient client = new())
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    responseBody = await response.Content.ReadAsStringAsync();
                }
                return View("Index", responseBody);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}