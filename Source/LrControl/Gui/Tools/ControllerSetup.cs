using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using LrControlCore.Device;
using LrControlCore.Device.Enums;
using micdah.LrControl.Mapping;
using micdah.LrControlApi.Common;
using Midi.Enums;

namespace micdah.LrControl.Gui.Tools
{
    public class ControllerSetup : INotifyPropertyChanged
    {
        private Channel _channel;
        private ControllerType _controllerType;
        private int _controlNumber;
        private ControllerMessageType _messageType;
        private Range _range;
        public event PropertyChangedEventHandler PropertyChanged;

        public Channel Channel
        {
            get { return _channel; }
            set
            {
                if (value == _channel) return;
                _channel = value;
                OnPropertyChanged();
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

        public ControllerMessageType MessageType
        {
            get { return _messageType; }
            set
            {
                if (value == _messageType) return;
                _messageType = value;
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

        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}