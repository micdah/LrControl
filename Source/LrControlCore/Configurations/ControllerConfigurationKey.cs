using LrControlCore.Device;
using LrControlCore.Device.Enums;
using Midi.Enums;

namespace LrControlCore.Configurations
{
    public class ControllerConfigurationKey
    {
        public Channel Channel { get; set; }
        public ControllerMessageType MessageType { get; set; }
        public int ControlNumber { get; set; }
    }
}