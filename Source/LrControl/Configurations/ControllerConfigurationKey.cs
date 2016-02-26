using micdah.LrControl.Mapping;
using Midi.Enums;

namespace micdah.LrControl.Configurations
{
    public class ControllerConfigurationKey
    {
        public Channel Channel { get; set; }
        public ControllerMessageType MessageType { get; set; }
        public int ControlNumber { get; set; }
    }
}