using LrControl.Core.Devices;
using LrControl.Core.Devices.Enums;
using Midi.Enums;

namespace LrControl.Core.Configurations
{
    public class ControllerConfigurationKey
    {
        public ControllerConfigurationKey()
        {
        }

        public ControllerConfigurationKey(Controller controller)
        {
            Channel = controller.MidiChannel;
            MessageType = controller.MessageType;
            ControlNumber = controller.ControlNumber;
        }

        public Channel Channel { get; set; }
        public ControllerMessageType MessageType { get; set; }
        public int ControlNumber { get; set; }
    }
}