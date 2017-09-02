using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using LrControl.Api.Common;
using LrControl.Core.Configurations;
using LrControl.Core.Devices.Enums;
using Midi.Enums;

namespace LrControl.Core.Devices
{
    public delegate void ControllerChangedHandler(int controllerValue);

    public class Controller : INotifyPropertyChanged
    {
        private readonly Device _device;
        private int _lastValue;
        private Range _range;

        public Controller(Device device, ControllerMessageType messageType, ControllerType controllerType, Channel channel, int controlNumber)
        {
            _device = device;
            MessageType = messageType;
            ControllerType = controllerType;
            Channel = channel;
            ControlNumber = controlNumber;
        }

        public ControllerMessageType MessageType { get; }
        public ControllerType ControllerType { get; }
        public string MessageTypeShort => MessageType == ControllerMessageType.Nrpn ? "NRPN" : "CC";
        public Channel Channel { get; }
        public string ChannelShort => $"C{(int) Channel}";
        public int ControlNumber { get; }

        public Range Range
        {
            get => _range;
            set
            {
                if (Equals(value, _range)) return;
                _range = value;
                OnPropertyChanged();
            }
        }

        public int LastValue
        {
            get => _lastValue;
            private set
            {
                if (value == _lastValue) return;
                _lastValue = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event ControllerChangedHandler ControllerChanged;

        public void SetControllerValue(int controllerValue)
        {
            _device.OnDeviceOutput(this, controllerValue);
        }

        public ControllerConfiguration GetConfiguration()
        {
            return new ControllerConfiguration
            {
                Channel = Channel,
                ControllerType = ControllerType,
                ControlNumber = ControlNumber,
                MessageType = MessageType,
                RangeMin = Convert.ToInt32(Range.Minimum),
                RangeMax = Convert.ToInt32(Range.Maximum)
            };
        }

        public ControllerConfigurationKey GetConfigurationKey()
        {
            return new ControllerConfigurationKey
            {
                Channel = Channel,
                ControlNumber = ControlNumber,
                MessageType = MessageType
            };
        }

        public void Reset()
        {
            if (Range != null)
                SetControllerValue((int) Range.Minimum);
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal void OnDeviceInput(int deviceValue)
        {
            // TODO Update Range if value falls outside current range
            var clampedValue = Convert.ToInt32(Range.ClampToRange(deviceValue));

            LastValue = clampedValue;
            ControllerChanged?.Invoke(clampedValue);
        }

        public bool IsController(ControllerConfigurationKey controllerKey)
        {
            return Channel == controllerKey.Channel
                   && ControlNumber == controllerKey.ControlNumber
                   && MessageType == controllerKey.MessageType;
        }
    }
}