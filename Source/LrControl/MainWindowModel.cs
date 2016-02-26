using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Windows.Input;
using micdah.LrControl.Annotations;
using micdah.LrControl.Configurations;
using micdah.LrControl.Mapping;
using micdah.LrControl.Mapping.Catalog;
using micdah.LrControlApi;
using micdah.LrControlApi.Modules.LrApplicationView;
using Midi.Devices;
using Prism.Commands;

namespace micdah.LrControl
{
    public class MainWindowModel : INotifyPropertyChanged
    {
        private readonly LrApi _api;
        private ControllerManager _controllerManager;
        private FunctionCatalog _functionCatalog;
        private FunctionGroupManager _functionGroupManager;
        private IInputDevice _inputDevice;
        private IOutputDevice _outputDevice;
        private bool _showSettingsDialog;

        public MainWindowModel(LrApi api)
        {
            _api = api;

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

        public FunctionGroupManager FunctionGroupManager
        {
            get { return _functionGroupManager; }
            set
            {
                if (Equals(value, _functionGroupManager)) return;
                _functionGroupManager = value;
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
            var conf = new MappingConfiguration
            {
                Controllers = ControllerManager.GetConfiguration()
            };

            MappingConfiguration.Save(conf);
        }

        public void Load()
        {
            var conf = MappingConfiguration.Load();
            if (conf == null) return;

            // Load controllers
            ControllerManager.Load(conf.Controllers);
            ControllerManager.SetInputDevice(InputDevice);
            ControllerManager.SetOutputDevice(OutputDevice);

            FunctionGroupManager.InitControllers(ControllerManager);


            EnableModuleGroupuForCurrentModule();
        }

        public void Reset()
        {
            ControllerManager?.Reset();
            FunctionGroupManager?.Reset();
        }

        public void EnableModuleGroupuForCurrentModule()
        {
            Module currentModule;
            if (_api.LrApplicationView.GetCurrentModuleName(out currentModule))
            {
                ModuleGroup.EnableGroupFor(currentModule);
            }
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}