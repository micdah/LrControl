using LrControl.Devices;
using Newtonsoft.Json;
using RtMidi.Core.Enums;

namespace LrControl.Configurations.Models
{
    [JsonObject]
    [JsonConverter(typeof(ControllerIdModelConverter))]
    public class ControllerIdModel
    {
        public Channel Channel { get; set; }
        public MessageType MessageType { get; set; }
        public int Parameter { get; set; }
    }
}