using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using micdah.LrControl.Annotations;
using micdah.LrControlApi.Common;
using Midi.Devices;
using Midi.Enums;
using Midi.Messages;

namespace micdah.LrControl.Functions
{
    public abstract class Function : INotifyPropertyChanged
    {
        private Channel _channel;
        private Range _controllerRange;
        private int _controlNumber;
        private bool _enabled;
        private IOutputDevice _outputDevice;
        private ControllerType _controllerType;

        protected Function(LrControlApi.LrControlApi api, ControllerType controllerType, Channel channel, int controlNumber, Range controllerRange)
        {
            Api = api;
            ControllerType = controllerType;
            Channel = channel;
            ControlNumber = controlNumber;
            ControllerRange = controllerRange;
        }

        protected LrControlApi.LrControlApi Api { get; }

        public bool Enabled
        {
            get { return _enabled; }
            private set
            {
                if (value == _enabled) return;
                _enabled = value;
                OnPropertyChanged();
            }
        }

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

        public Range ControllerRange
        {
            get { return _controllerRange; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                if (Equals(value, _controllerRange)) return;
                _controllerRange = value;
                OnPropertyChanged();
            }
        }

        public IOutputDevice OutputDevice
        {
            get { return _outputDevice; }
            set
            {
                if (Equals(value, _outputDevice)) return;
                _outputDevice = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void Enable()
        {
            Enabled = true;
        }

        public void Disable()
        {
            Enabled = false;
        }

        public void Handle(NrpnMessage msg)
        {
            if (!Enabled) return;
            if (ControllerType != ControllerType.Nrpn) return;

            if (Channel == msg.Channel && ControlNumber == msg.Parameter)
            {
                ControllerChanged(msg.Value);
            }
        }

        public void Handle(ControlChangeMessage msg)
        {
            if (!Enabled) return;
            if (ControllerType != ControllerType.ControlChange) return;

            if (Channel == msg.Channel && ControlNumber == (int) msg.Control)
            {
                ControllerChanged(msg.Value);
            }
        }

        protected abstract void ControllerChanged(int controllerValue);

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}