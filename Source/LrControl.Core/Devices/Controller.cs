using LrControl.Api.Common;
using LrControl.Core.Configurations;
using LrControl.Core.Devices.Enums;
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

        public event ControllerValueChangedHandler ControllerValueChanged;

        internal void SetControllerValue(int controllerValue)
        {
            _deviceManager.OnDeviceOutput(this, controllerValue);
        }

        internal void Reset()
        {
            if (Range != null)
                SetControllerValue((int) Range.Minimum);
        }

        internal void OnDeviceInput(int value)
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
            ControllerValueChanged?.Invoke(value);
        }

        internal bool IsController(ControllerConfigurationKey controllerKey)
        {
            return MidiChannel == controllerKey.Channel
                   && ControlNumber == controllerKey.ControlNumber
                   && MessageType == controllerKey.MessageType;
        }
    }
}