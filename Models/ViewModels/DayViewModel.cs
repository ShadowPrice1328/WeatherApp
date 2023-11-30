namespace WeatherApp.ViewModels
{
    public class DayViewModel
    {
        public DateTime Date {get; set;}
        public string IconUrl {get; set;} = string.Empty;
        public bool IsSelected {get; set;} = false;
    }
}