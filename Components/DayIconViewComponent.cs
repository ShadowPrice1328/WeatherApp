using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using WeatherApp.ViewModels;

namespace WeatherApp.Components
{
    public class DayIconViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IGrouping<DateTime, Models.List> item, bool isSelected = false)
        {
            string iconName = item.FirstOrDefault(i => i.dt_txt.EndsWith("12:00:00"))?.weather.First().icon ?? item.First().weather.First().icon;

            var viewModel = new DayViewModel
            {
                Date = item.Key,
                IconUrl = $"/images/{iconName}.png",
                IsSelected = isSelected
            };

            return View(viewModel);
        }
    }
}