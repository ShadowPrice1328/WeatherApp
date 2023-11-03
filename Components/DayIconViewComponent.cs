using Microsoft.AspNetCore.Mvc;
using WeatherApp.ViewModels;

namespace WeatherApp.Components
{
    public class DayIconViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(DateTime date, string iconUrl)
        {
            var viewModel = new DayViewModel
            {
                Date = date,
                IconUrl = $"https://openweathermap.org/img/wn/{iconUrl}.png"
            };

            return View(viewModel);
        }
    }
}