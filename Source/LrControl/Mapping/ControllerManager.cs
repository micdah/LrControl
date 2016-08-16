using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using JetBrains.Annotations;
using micdah.LrControl.Configurations;
using micdah.LrControlApi.Common;
using Midi.Devices;

namespace micdah.LrControl.Mapping
{
    public class ControllerManager : INotifyPropertyChanged
    {
        private readonly object _controllersLock = new object();
        private ObservableCollection<Controller> _controllers;
        private IInputDevice _inputDevice;
        private IOutputDevice _outputDevice;

        public ControllerManager()
        {
            Controllers = new ObservableCollection<Controller>();
        }

        public ObservableCollection<Controller> Controllers
        {
            get { return _controllers; }
            private set
            {
                if (Equals(value, _controllers)) return;
                _controllers = value;
                BindingOperations.EnableCollectionSynchronization(_controllers, _controllersLock);
                OnPropertyChanged();
            }
        }

        public IInputDevice InputDevice
        {
            private get { return _inputDevice; }
            set
            {
                _inputDevice = value;

                foreach (var controller in Controllers)
                {
                    controller.InputDevice = value;
                }
            }
        }

        public IOutputDevice OutputDevice
        {
            private get { return _outputDevice; }
            set
            {
                _outputDevice = value;

                foreach (var controller in Controllers)
                {
                    controller.OutputDevice = value;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void ResetAllControls()
        {
            foreach (var controller in Controllers)
            {
                controller.Reset();
            }
        }

        public List<ControllerConfiguration> GetConfiguration()
        {
            return Controllers.Select(controller => controller.GetConfiguration()).ToList();
        }

        public void Clear()
        {
            foreach (var controller in Controllers)
            {
                controller.Dispose();
            }
            Controllers.Clear();
        }

        public void Load(IEnumerable<ControllerConfiguration> controllerConfiguration)
        {
            Clear();

            foreach (var controller in controllerConfiguration)
            {
                Controllers.Add(new Controller
                {
                    Channel = controller.Channel,
                    MessageType = controller.MessageType,
                    ControlNumber = controller.ControlNumber,
                    ControllerType = controller.ControllerType,
                    Range = new Range(controller.RangeMin, controller.RangeMax),
                    InputDevice = InputDevice,
                    OutputDevice = OutputDevice
                });
            }
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}