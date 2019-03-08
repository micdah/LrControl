using System.Collections.Generic;
using Newtonsoft.Json;

namespace LrControl.Configurations.Models
{
    [JsonObject]
    public class ProfileModel
    {
        [JsonProperty("modules")]
        public Dictionary<string,List<ControllerFunctionModel>> Modules { get; set; }

        [JsonProperty("panels")]
        public Dictionary<string,List<ControllerFunctionModel>> Panels { get; set; }
    }
}