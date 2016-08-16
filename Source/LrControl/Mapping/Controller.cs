using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using LrControlCore.Configurations;
using LrControlCore.Device;
using micdah.LrControlApi.Common;
using Midi.Devices;
using Midi.Enums;
using Midi.Messages;

namespace micdah.LrControl.Mapping
{
    public delegate void ControllerChangedHandler(int controllerValue);

    public class Controller : INotifyPropertyChanged, IDisposable
    {
        private Channel _channel;
        private ControllerType _controllerType;
        private int _controlNumber;
        private IInputDevice _inputDevice;
        private int _lastValue;
        private ControllerMessageType _messageType;
        private Range _range;

        public ControllerMessageType MessageType
        {
            get { return _messageType; }
            set
            {
                if (value == _messageType) return;
                _messageType = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(MessageTypeShort));
            }
        }

        public ControllerType ControllerType
        {
            get { return _controllerType; }
            set
            {
                if (value == _controllerType) return;
                _controllerType = value;
                OnPropertyChanged();
            }
        }

        public string MessageTypeShort => MessageType == ControllerMessageType.Nrpn ? "NRPN" : "CC";

        public Channel Channel
        {
            get { return _channel; }
            set
            {
                if (value == _channel) return;
                _channel = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ChannelShort));
            }
        }

        public string ChannelShort => $"C{(int) Channel}";

        public int ControlNumber
        {
            get { return _controlNumber; }
            set
            {
                if (value == _controlNumber) return;
                _controlNumber = value;
                OnPropertyChanged();
            }
        }

        public Range Range
        {
            get { return _range; }
            set
            {
                if (Equals(value, _range)) return;
                _range = value;
                OnPropertyChanged();
            }
        }

        public int LastValue
        {
            get { return _lastValue; }
            private set
            {
                if (value == _lastValue) return;
                _lastValue = value;
                OnPropertyChanged();
            }
        }

        public IInputDevice InputDevice
        {
            set
            {
                if (_inputDevice != null)
                {
                    switch (MessageType)
                    {
                        case ControllerMessageType.ControlChange:
                            _inputDevice.ControlChange -= Handle;
                            break;
                        case ControllerMessageType.Nrpn:
                            _inputDevice.Nrpn -= Handle;
                            break;
                    }
                }

                _inputDevice = value;

                if (_inputDevice == null) return;
                switch (MessageType)
                {
                    case ControllerMessageType.ControlChange:
                        _inputDevice.ControlChange += Handle;
                        break;
                    case ControllerMessageType.Nrpn:
                        _inputDevice.Nrpn += Handle;
                        break;
                }
            }
        }

        public IOutputDevice OutputDevice { private get; set; }

        public void Dispose()
        {
            if (_inputDevice != null)
            {
                _inputDevice.Nrpn -= Handle;
                _inputDevice.ControlChange -= Handle;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public event ControllerChangedHandler ControllerChanged;

        public void SetControllerValue(int controllerValue)
        {
            if (OutputDevice == null) return;

            switch (MessageType)
            {
                case ControllerMessageType.ControlChange:
                    OutputDevice.SendControlChange(Channel, (Control) ControlNumber, controllerValue);
                    break;
                case ControllerMessageType.Nrpn:
                    OutputDevice.SendNrpn(Channel, ControlNumber, controllerValue);
                    break;
            }
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
            {
                SetControllerValue((int) Range.Minimum);
            }
        }

        private void Handle(NrpnMessage msg)
        {
            if (msg.Channel == Channel && msg.Parameter == ControlNumber)
            {
                OnControllerChanged(msg.Value);
            }
        }

        private void Handle(ControlChangeMessage msg)
        {
            if (msg.Channel == Channel && (int) msg.Control == ControlNumber)
            {
                OnControllerChanged(msg.Value);
            }
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnControllerChanged(int controllervalue)
        {
            var clampedValue = Convert.ToInt32(Range.ClampToRange(controllervalue));

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