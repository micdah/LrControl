using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using LrControl.Api.Common;
using LrControl.Core.Device.Enums;
using Midi.Enums;

namespace LrControl.Gui.Tools
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
            get => _channel;
            set
            {
                if (value == _channel) return;
                _channel = value;
                OnPropertyChanged();
            }
        }

        public ControllerType ControllerType
        {
            get => _controllerType;
            set
            {
                if (value == _controllerType) return;
                _controllerType = value;
                OnPropertyChanged();
            }
        }

        public int ControlNumber
        {
            get => _controlNumber;
            set
            {
                if (value == _controlNumber) return;
                _controlNumber = value;
                OnPropertyChanged();
            }
        }

        public ControllerMessageType MessageType
        {
            get => _messageType;
            set
            {
                if (value == _messageType) return;
                _messageType = value;
                OnPropertyChanged();
            }
        }

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

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}