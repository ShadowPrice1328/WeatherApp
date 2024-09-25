namespace WeatherApp.ViewModels
{
    public class DayDetailsViewModel
    {
        public string? City { get; set; }
        public IGrouping<DateTime, Models.List>? DayInfo {get; set;}
    }
}
