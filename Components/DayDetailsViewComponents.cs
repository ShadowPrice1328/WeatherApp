using Microsoft.AspNetCore.Mvc;

namespace WeatherApp.Components
{
    public class DayDetailsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IGrouping<DateTime, Models.List> item)
        {
            return View(item);
        }
    }
}