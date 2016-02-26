using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using micdah.LrControl.Annotations;
using micdah.LrControl.Mapping;
using micdah.LrControl.Mapping.Catalog;
using Midi.Devices;
using Prism.Commands;

namespace micdah.LrControl
{
    public class MainWindowModel : INotifyPropertyChanged
    {
        private ControllerManager _controllerManager;
        private FunctionCatalog _functionCatalog;
        private FunctionGroupCatalog _functionGroupCatalog;
        private IInputDevice _inputDevice;
        private IOutputDevice _outputDevice;
        private bool _showSettingsDialog;

        public MainWindowModel()
        {
            OpenSettingsCommand = new DelegateCommand(OpenSettings);
            SaveCommand         = new DelegateCommand(Save);
            LoadCommand         = new DelegateCommand(Load);
            ResetCommand        = new DelegateCommand(Reset);
        }

        public ICommand OpenSettingsCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand LoadCommand { get; }
        public ICommand ResetCommand { get; }

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

        public bool ShowSettingsDialog
        {
            get { return _showSettingsDialog; }
            set
            {
                if (value == _showSettingsDialog) return;
                _showSettingsDialog = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OpenSettings()
        {
            ShowSettingsDialog = !ShowSettingsDialog;
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Load()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}