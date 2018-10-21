using LrControl.Core.Configurations;
using LrControl.Core.Devices.Enums;
using LrControl.LrPlugin.Api.Common;
using RtMidi.Core.Enums;

namespace LrControl.Core.Devices
{
    public delegate void ControllerValueChangedHandler(int controllerValue);

    public class Controller
    {
        private readonly DeviceManager _deviceManager;

        internal Controller(DeviceManager deviceManager, ControllerMessageType messageType,
            ControllerType controllerType, Channel channel, int controlNumber, Range range)
        {
            _deviceManager = deviceManager;
            MessageType = messageType;
            ControllerType = controllerType;
            MidiChannel = channel;
            ControlNumber = controlNumber;
            Range = range;
        }

        public ControllerMessageType MessageType { get; }
        public ControllerType ControllerType { get; }
        internal Channel MidiChannel { get; }
        public int Channel => (int) MidiChannel + 1;
        public int ControlNumber { get; }
        public Range Range { get; }
        public int LastValue { get; private set; }

        public event ControllerValueChangedHandler ValueChanged;

        internal void UpdateValue(int value)
        {
            if (value < Range)
            {
                Range.Minimum = value;
            }
            else if (value > Range)
            {
                Range.Maximum = value;
            }

            LastValue = value;
            ValueChanged?.Invoke(value);
        }

        internal void UpdateController(int controllerValue)
        {
            _deviceManager.OnControllerUpdate(this, controllerValue);
        }

        internal void Reset()
        {
            if (Range != null)
                UpdateController((int) Range.Minimum);
        }

        internal bool IsController(ControllerConfigurationKey controllerKey)
        {
            return MidiChannel == controllerKey.Channel
                   && ControlNumber == controllerKey.ControlNumber
                   && MessageType == controllerKey.MessageType;
        }
    }
}