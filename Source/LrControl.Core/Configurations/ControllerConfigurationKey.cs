using LrControl.Core.Devices.Enums;
using Midi.Enums;

namespace LrControl.Core.Configurations
{
    public class ControllerConfigurationKey
    {
        public Channel Channel { get; set; }
        public ControllerMessageType MessageType { get; set; }
        public int ControlNumber { get; set; }
    }
}