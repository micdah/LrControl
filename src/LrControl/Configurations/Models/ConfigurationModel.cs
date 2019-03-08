using Newtonsoft.Json;

namespace LrControl.Configurations.Models
{
    [JsonObject]
    public class ConfigurationModel
    {
        [JsonProperty("profile")]
        public ProfileModel Profile { get; set; }
    }
}