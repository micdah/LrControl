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
        private ControllerType _type;

        public Controller()
        {
        }

        public Controller(Channel channel, ControllerType type, int controlNumber, Range range)
        {
            _channel = channel;
            _controlNumber = controlNumber;
            _range = range;
            _type = type;
        }

        public ControllerType Type
        {
            get { return _type; }
            set
            {
                if (value == _type) return;
                _type = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TypeShort));
            }
        }

        public string TypeShort => Type == ControllerType.Nrpn ? "NRPN" : "CC";

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


        public event PropertyChangedEventHandler PropertyChanged;

        public event ControllerChangedHandler ControllerChanged;

        public void SetInputDevice(IInputDevice inputDevice)
        {
            if (_inputDevice != null)
            {
                switch (Type)
                {
                    case ControllerType.ControlChange:
                        _inputDevice.ControlChange -= Handle;
                        break;
                    case ControllerType.Nrpn:
                        _inputDevice.Nrpn -= Handle;
                        break;
                }
            }

            _inputDevice = inputDevice;
            switch (Type)
            {
                case ControllerType.ControlChange:
                    _inputDevice.ControlChange += Handle;
                    break;
                case ControllerType.Nrpn:
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

            switch (Type)
            {
                case ControllerType.ControlChange:
                    _outputDevice.SendControlChange(Channel, (Control) ControlNumber, controllerValue);
                    break;
                case ControllerType.Nrpn:
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
            ControllerChanged?.Invoke(controllervalue);
        }
    }
}