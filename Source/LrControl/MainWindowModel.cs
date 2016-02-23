using System.ComponentModel;
using System.Runtime.CompilerServices;
using micdah.LrControl.Annotations;
using micdah.LrControl.Mapping;
using micdah.LrControl.Mapping.Catalog;
using Midi.Devices;

namespace micdah.LrControl
{
    public class MainWindowModel : INotifyPropertyChanged
    {
        private ControllerManager _controllerManager;
        private FunctionCatalog _functionCatalog;
        private FunctionGroupCatalog _functionGroupCatalog;
        private IInputDevice _inputDevice;
        private IOutputDevice _outputDevice;

        public IInputDevice InputDevice
        {
            get { return _inputDevice; }
            set
            {
                if (Equals(value, _inputDevice)) return;
                _inputDevice = value;
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

        public ControllerManager ControllerManager
        {
            get { return _controllerManager; }
            set
            {
                if (Equals(value, _controllerManager)) return;
                _controllerManager = value;
                OnPropertyChanged();
            }
        }

        public FunctionCatalog FunctionCatalog
        {
            get { return _functionCatalog; }
            set
            {
                if (Equals(value, _functionCatalog)) return;
                _functionCatalog = value;
                OnPropertyChanged();
            }
        }

        public FunctionGroupCatalog FunctionGroupCatalog
        {
            get { return _functionGroupCatalog; }
            set
            {
                if (Equals(value, _functionGroupCatalog)) return;
                _functionGroupCatalog = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}