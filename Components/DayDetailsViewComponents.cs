using Microsoft.AspNetCore.Mvc;
using WeatherApp.ViewModels;

namespace WeatherApp.Components
{
    public class DayDetailsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IGrouping<DateTime, Models.List> item)
        {
            if (item == null || string.IsNullOrEmpty(TempData["City"]?.ToString())) 
            {
                return View("Error", "No data available for the selected day.");
            }

            var weatherViewModel = new DayDetailsViewModel()
            {
                City = TempData["City"]?.ToString(),
                DayInfo = item
            };

            return View(weatherViewModel);
        }
    }
}