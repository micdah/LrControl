using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using micdah.LrControl.Annotations;
using micdah.LrControl.Configurations;
using micdah.LrControl.Core;
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
        private readonly IDialogProvider _dialogProvider;
        private ControllerManager _controllerManager;
        private FunctionCatalog _functionCatalog;
        private FunctionGroupManager _functionGroupManager;
        private IInputDevice _inputDevice;
        private IOutputDevice _outputDevice;
        private bool _showSettingsDialog;

        public MainWindowModel(LrApi api, IDialogProvider dialogProvider)
        {
            _api = api;
            _dialogProvider = dialogProvider;

            OpenSettingsCommand = new DelegateCommand(OpenSettings);
            SaveCommand         = new DelegateCommand(() => SaveConfiguration());
            LoadCommand         = new DelegateCommand(() => LoadConfiguration());
            ExportCommand       = new DelegateCommand(ExportConfiguration);
            ImportCommand     = new DelegateCommand(ImportConfiguration);
            ResetCommand        = new DelegateCommand(Reset);

            // Initialize catalogs and controllers
            FunctionCatalog      = FunctionCatalog.DefaultCatalog(api);
            ControllerManager    = new ControllerManager();
            FunctionGroupManager = FunctionGroupManager.DefaultGroups(api, FunctionCatalog, ControllerManager);

            // Hookup module listener
            api.LrApplicationView.ModuleChanged += FunctionGroupManager.EnableModule;
        }

        public ICommand OpenSettingsCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand ExportCommand { get; }
        public ICommand LoadCommand { get; }
        public ICommand ImportCommand { get; }
        public ICommand ResetCommand { get; }

        public IInputDevice InputDevice
        {
            private get { return _inputDevice; }
            set
            {
                if (Equals(value, _inputDevice)) return;
                _inputDevice = value;
                OnPropertyChanged();
            }
        }

        public IOutputDevice OutputDevice
        {
            private get { return _outputDevice; }
            set
            {
                if (Equals(value, _outputDevice)) return;
                _outputDevice = value;
                OnPropertyChanged();
            }
        }

        private ControllerManager ControllerManager
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
            private set
            {
                if (Equals(value, _functionCatalog)) return;
                _functionCatalog = value;
                OnPropertyChanged();
            }
        }

        public FunctionGroupManager FunctionGroupManager
        {
            get { return _functionGroupManager; }
            private set
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

        public void SaveConfiguration(string file = MappingConfiguration.ConfigurationsFile)
        {
            var conf = new MappingConfiguration
            {
                Controllers = ControllerManager.GetConfiguration(),
                Modules = FunctionGroupManager.GetConfiguration(),
            };

            MappingConfiguration.Save(conf, file);
        }

        public void LoadConfiguration(string file = MappingConfiguration.ConfigurationsFile)
        {
            var conf = MappingConfiguration.Load(file);
            if (conf == null) return;

            // Load controllers
            ControllerManager.Load(conf.Controllers);
            ControllerManager.SetInputDevice(InputDevice);
            ControllerManager.SetOutputDevice(OutputDevice);
            ControllerManager.ResetAllControls();

            // Load function mapping
            FunctionGroupManager.Load(conf.Modules);


            EnableModuleGroupuForCurrentModule();
        }

        public void ExportConfiguration()
        {
            var file = _dialogProvider.ShowSaveDialog(GetSettingsFolder());
            if (!string.IsNullOrEmpty(file))
            {
                SaveConfiguration(file);
            }
        }

        private void ImportConfiguration()
        {
            var file = _dialogProvider.ShowOpenDialog(GetSettingsFolder());
            if (!string.IsNullOrEmpty(file))
            {
                LoadConfiguration(file);
            }
        }

        private async void Reset()
        {
            var result = await _dialogProvider.ShowMessage("Are you sure, you want to clear the current configuration?",
                "Confirm clear configuration", DialogButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                ControllerManager?.Reset();
                FunctionGroupManager?.Reset();
            }            
        }

        private void EnableModuleGroupuForCurrentModule()
        {
            Module currentModule;
            if (_api.LrApplicationView.GetCurrentModuleName(out currentModule))
            {
                FunctionGroupManager.EnableModule(currentModule);
            }
        }

        private static string GetSettingsFolder()
        {
            var settingsFolder =
                Path.GetDirectoryName(Serializer.ResolveRelativeFilename(MappingConfiguration.ConfigurationsFile));
            return settingsFolder;
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}