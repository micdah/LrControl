using System.ComponentModel;
using System.Runtime.CompilerServices;
using micdah.LrControl.Annotations;
using micdah.LrControlApi.Common;
using Midi.Devices;
using Midi.Enums;
using Midi.Messages;

namespace micdah.LrControl.Mapping
{
    public delegate void ControllerChangedHandler(int controllerValue);

    public class Controller : INotifyPropertyChanged
    {
        private Channel _channel;
        private int _controlNumber;
        private IInputDevice _inputDevice;
        private IOutputDevice _outputDevice;
        private Range _range;
        private ControllerMessageType _messageType;
        private ControllerType _controllerType;
        private int _lastValue;

        public Controller()
        {
        }

        public Controller(Channel channel, ControllerMessageType messageType, int controlNumber, Range range)
        {
            _channel = channel;
            _controlNumber = controlNumber;
            _range = range;
            _messageType = messageType;
        }

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


        public event PropertyChangedEventHandler PropertyChanged;

        public event ControllerChangedHandler ControllerChanged;

        public void SetInputDevice(IInputDevice inputDevice)
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

            _inputDevice = inputDevice;
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

        public void SetOutputDevice(IOutputDevice outputDevice)
        {
            _outputDevice = outputDevice;
        }

        public void SetControllerValue(int controllerValue)
        {
            if (_outputDevice == null) return;

            switch (MessageType)
            {
                case ControllerMessageType.ControlChange:
                    _outputDevice.SendControlChange(Channel, (Control) ControlNumber, controllerValue);
                    break;
                case ControllerMessageType.Nrpn:
                    _outputDevice.SendNrpn(Channel, ControlNumber, controllerValue);
                    break;
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
            LastValue = controllervalue;
            ControllerChanged?.Invoke(controllervalue);
        }
    }
}