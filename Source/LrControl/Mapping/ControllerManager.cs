using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using micdah.LrControl.Annotations;
using micdah.LrControl.Configurations;
using micdah.LrControlApi.Common;
using Midi.Devices;

namespace micdah.LrControl.Mapping
{
    public class ControllerManager : INotifyPropertyChanged
    {
        private readonly object _controllersLock = new object();
        private ObservableCollection<Controller> _controllers;

        public ControllerManager()
        {
            Controllers = new ObservableCollection<Controller>();
        }

        public ControllerManager(IEnumerable<Controller> controllers)
        {
            Controllers = new ObservableCollection<Controller>(controllers);
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

        public event PropertyChangedEventHandler PropertyChanged;

        public void SetInputDevice(IInputDevice inputDevice)
        {
            foreach (var controller in Controllers)
            {
                controller.SetInputDevice(inputDevice);
            }
        }

        public void SetOutputDevice(IOutputDevice outputDevice)
        {
            foreach (var controller in Controllers)
            {
                controller.SetOutputDevice(outputDevice);
            }
        }

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

        public void Load(List<ControllerConfiguration> controllerConfiguration)
        {
            Reset();

            foreach (var controller in controllerConfiguration)
            {
                Controllers.Add(new Controller
                {
                    Channel = controller.Channel,
                    MessageType = controller.MessageType,
                    ControlNumber = controller.ControlNumber,
                    ControllerType = controller.ControllerType,
                    Range = new Range(controller.RangeMin, controller.RangeMax),
                });
            }
        }

        public void Reset()
        {
            foreach (var controller in Controllers)
            {
                controller.Dispose();
            }
            Controllers.Clear();
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}