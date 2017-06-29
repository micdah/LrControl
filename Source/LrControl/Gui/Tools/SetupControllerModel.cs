using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using JetBrains.Annotations;
using LrControlCore.Device.Enums;
using micdah.LrControlApi.Common;
using Midi.Devices;
using Midi.Messages;

namespace micdah.LrControl.Gui.Tools
{
    public class SetupControllerModel : INotifyPropertyChanged
    {
        private readonly object _controllersLock = new object();
        private ObservableCollection<ControllerSetup> _controllers;
        private IInputDevice _inputDevice;

        public SetupControllerModel()
        {
            Controllers = new ObservableCollection<ControllerSetup>();
        }

        public IInputDevice InputDevice
        {
            get => _inputDevice;
            set
            {
                if (Equals(value, _inputDevice)) return;

                if (_inputDevice != null)
                {
                    _inputDevice.Nrpn          -= OnNrpn;
                    _inputDevice.ControlChange -= OnControlChange;
                }

                _inputDevice = value;

                if (_inputDevice != null)
                {
                    _inputDevice.Nrpn          += OnNrpn;
                    _inputDevice.ControlChange += OnControlChange;

                }

                OnPropertyChanged();
            }
        }

        public ObservableCollection<ControllerSetup> Controllers
        {
            get => _controllers;
            set
            {
                if (Equals(value, _controllers)) return;
                _controllers = value;
                BindingOperations.EnableCollectionSynchronization(_controllers, _controllersLock);
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnNrpn(NrpnMessage msg)
        {
            var controller = Controllers.FirstOrDefault(c =>
                c.Channel == msg.Channel &&
                c.MessageType == ControllerMessageType.Nrpn &&
                c.ControlNumber == msg.Parameter);

            if (controller == null)
            {
                controller = new ControllerSetup
                {
                    Channel = msg.Channel,
                    MessageType = ControllerMessageType.Nrpn,
                    ControlNumber = msg.Parameter,
                    ControllerType = ControllerType.Fader,
                    Range = new Range(0, 1)
                };
                Controllers.Add(controller);
            }

            UpdateRange(msg.Value, controller);
        }

        private void OnControlChange(ControlChangeMessage msg)
        {
            var controller = Controllers.FirstOrDefault(c =>
                c.Channel == msg.Channel &&
                c.MessageType == ControllerMessageType.ControlChange &&
                c.ControlNumber == (int) msg.Control);

            if (controller == null)
            {
                controller = new ControllerSetup
                {
                    Channel = msg.Channel,
                    MessageType = ControllerMessageType.ControlChange,
                    ControlNumber = (int) msg.Control,
                    ControllerType = ControllerType.Button,
                    Range = new Range(0, 1),
                };
                Controllers.Add(controller);
            }

            UpdateRange(msg.Value, controller);
        }

        private static void UpdateRange(int value, ControllerSetup controller)
        {
            var range = controller.Range;
            if (value < range.Minimum)
            {
                controller.Range = new Range(value, range.Maximum);
            }
            else if (value > range.Maximum)
            {
                controller.Range = new Range(range.Minimum, value);
            }
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}