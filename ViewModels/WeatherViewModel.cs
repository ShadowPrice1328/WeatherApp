using Models;

namespace WeatherApp.ViewModels
{
    public class WeatherViewModel
    {
        public IEnumerable<IGrouping<DateTime, List>>? GroupedWeather { get; set; }
        public string? Date {  get; set; }
        public string? City { get; set; }
    }
}
