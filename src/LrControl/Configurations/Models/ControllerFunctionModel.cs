using Newtonsoft.Json;

namespace LrControl.Configurations.Models
{
    [JsonObject]
    public class ControllerFunctionModel
    {
        [JsonProperty("controller")]
        public ControllerIdModel Controller { get; set; }

        [JsonProperty("function")]
        public string Function { get; set; }
    }
}