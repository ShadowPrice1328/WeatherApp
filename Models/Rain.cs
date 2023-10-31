using Newtonsoft.Json;

namespace WeatherApp.Models
{
    public class Rain
    {
        [JsonProperty("3h")]
        public double _3h { get; set; }
    }
}