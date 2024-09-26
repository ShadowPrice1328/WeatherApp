using Newtonsoft.Json;

namespace Models
{
    public class Rain
    {
        [JsonProperty("3h")]
        public double _3h { get; set; }
    }
}