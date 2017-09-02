using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Threading;
using JetBrains.Annotations;
using LrControl.Api;
using LrControl.Api.Modules.LrApplicationView;
using LrControl.Core.Configurations;
using LrControl.Core.Devices;
using LrControl.Core.Functions.Catalog;
using LrControl.Core.Mapping;
using LrControl.Core.Midi;
using LrControl.Core.Util;
using LrControl.Ui.Core;
using LrControl.Ui.Gui;
using Midi.Devices;
using Prism.Commands;

namespace LrControl.Ui
{
    public class MainWindowModel : INotifyPropertyChanged
    {
        private Device _device;
        private IMainWindowDialogProvider _dialogProvider;
        private IFunctionCatalog _functionCatalog;
        private InputDeviceDecorator _inputDevice;
        private string _inputDeviceName;
        private ObservableCollection<IInputDevice> _inputDevices;
        private IOutputDevice _outputDevice;
        private string _outputDeviceName;
        private ObservableCollection<IOutputDevice> _outputDevices;
        private bool _showSettingsDialog;

        public MainWindowModel(LrApi api)
        {
            Api = api;
            InputDevices = new ObservableCollection<IInputDevice>();
            OutputDevices = new ObservableCollection<IOutputDevice>();

            // Commands
            OpenSettingsCommand = new DelegateCommand(OpenSettings);
            SaveCommand = new DelegateCommand(() => SaveConfiguration());
            LoadCommand = new DelegateCommand(() => LoadConfiguration());
            ExportCommand = new DelegateCommand(ExportConfiguration);
            ImportCommand = new DelegateCommand(ImportConfiguration);
            ResetCommand = new DelegateCommand(Reset);
            RefreshAvailableDevicesCommand = new DelegateCommand(RefreshAvailableDevices);
            
            // Initialize catalogs and controllers
            FunctionCatalog = LrControl.Core.Functions.Catalog.FunctionCatalog.DefaultCatalog(api);
            Device = new Device();
            FunctionGroupManager = FunctionGroupManager.DefaultGroups(api, FunctionCatalog, Device);
            FunctionGroupManagerViewModel = new FunctionGroupManagerViewModel(Dispatcher.CurrentDispatcher, FunctionGroupManager);

            // Hookup module listener
            api.LrApplicationView.ModuleChanged += FunctionGroupManager.EnableModule;

            Settings.Current.PropertyChanged += CurrentOnPropertyChanged;
        }

        public LrApi Api { get; }

        public IMainWindowDialogProvider DialogProvider
        {
            get => _dialogProvider;
            set
            {
                if (Equals(value, _dialogProvider)) return;
                _dialogProvider = value;
                OnPropertyChanged();
            }
        }

        public ICommand OpenSettingsCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand ExportCommand { get; }
        public ICommand LoadCommand { get; }
        public ICommand ImportCommand { get; }
        public ICommand ResetCommand { get; }
        public ICommand RefreshAvailableDevicesCommand { get; }
        
        public IInputDevice InputDevice
        {
            get => _inputDevice;
            set
            {
                if (Equals(value, _inputDevice)) return;

                _inputDevice?.Dispose();

                _inputDevice = value != null
                    ? new InputDeviceDecorator(value, 1000/Settings.Current.ParameterUpdateFrequency)
                    : null;

                Device.InputDevice = _inputDevice;

                if (_inputDevice != null)
                {
                    if (!_inputDevice.IsOpen) _inputDevice.Open();
                    if (!_inputDevice.IsReceiving) _inputDevice.StartReceiving(null);
                }

                OnPropertyChanged();

                InputDeviceName = _inputDevice?.Name;
            }
        }

        public string InputDeviceName
        {
            get => _inputDeviceName;
            set
            {
                if (value == _inputDeviceName) return;
                _inputDeviceName = value;
                OnPropertyChanged();

                InputDevice = InputDevices.FirstOrDefault(x => x.Name == value);
            }
        }

        public ObservableCollection<IInputDevice> InputDevices
        {
            get => _inputDevices;
            private set
            {
                if (Equals(value, _inputDevices)) return;
                _inputDevices = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<IOutputDevice> OutputDevices
        {
            get => _outputDevices;
            private set
            {
                if (Equals(value, _outputDevices)) return;
                _outputDevices = value;
                OnPropertyChanged();
            }
        }

        public IOutputDevice OutputDevice
        {
            get => _outputDevice;
            set
            {
                if (Equals(value, _outputDevice)) return;

                if (_outputDevice != null)
                {
                    if (_outputDevice.IsOpen) _outputDevice.Close();
                }

                _outputDevice = value;
                Device.OutputDevice = value;

                if (_outputDevice != null)
                {
                    if (!_outputDevice.IsOpen) _outputDevice.Open();
                }

                OnPropertyChanged();

                OutputDeviceName = _outputDevice?.Name;
            }
        }

        public string OutputDeviceName
        {
            get => _outputDeviceName;
            set
            {
                if (value == _outputDeviceName) return;
                _outputDeviceName = value;
                OnPropertyChanged();

                OutputDevice = OutputDevices.FirstOrDefault(x => x.Name == value);
            }
        }

        public IFunctionCatalog FunctionCatalog
        {
            get => _functionCatalog;
            private set
            {
                if (Equals(value, _functionCatalog)) return;
                _functionCatalog = value;
                OnPropertyChanged();
            }
        }

        public bool ShowSettingsDialog
        {
            get => _showSettingsDialog;
            set
            {
                if (value == _showSettingsDialog) return;
                _showSettingsDialog = value;
                OnPropertyChanged();
            }
        }

        private Device Device
        {
            get => _device;
            set
            {
                if (Equals(value, _device)) return;
                _device = value;
                OnPropertyChanged();
            }
        }

        public FunctionGroupManagerViewModel FunctionGroupManagerViewModel { get; }

        private FunctionGroupManager FunctionGroupManager { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void CurrentOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Settings.ParameterUpdateFrequency))
            {
                if (_inputDevice != null)
                {
                    _inputDevice.UpdateInterval = 1000 / Settings.Current.ParameterUpdateFrequency;
                }
            }
        }

        public void OpenSettings()
        {
            ShowSettingsDialog = !ShowSettingsDialog;
        }

        public void SaveConfiguration(string file = MappingConfiguration.ConfigurationsFile)
        {
            var conf = new MappingConfiguration
            {
                Controllers = Device.GetConfiguration(),
                Modules = FunctionGroupManager.GetConfiguration()
            };

            MappingConfiguration.Save(conf, file);
        }

        public void LoadConfiguration(string file = MappingConfiguration.ConfigurationsFile)
        {
            var conf = MappingConfiguration.Load(file);
            if (conf == null) return;

            Device.Load(conf.Controllers);
            Device.ResetAllControls();

            FunctionGroupManager.Load(conf.Modules);

            // Enable current module group
            Module currentModule;
            if (Api.LrApplicationView.GetCurrentModuleName(out currentModule))
            {
                FunctionGroupManager.EnableModule(currentModule);
            }
        }

        public void ExportConfiguration()
        {
            var file = _dialogProvider.ShowSaveDialog(GetSettingsFolder());
            if (!string.IsNullOrEmpty(file))
            {
                SaveConfiguration(file);
            }
        }

        public void ImportConfiguration()
        {
            var file = _dialogProvider.ShowOpenDialog(GetSettingsFolder());
            if (!string.IsNullOrEmpty(file))
            {
                LoadConfiguration(file);
            }
        }

        public async void Reset()
        {
            var result = await _dialogProvider.ShowMessage("Are you sure, you want to clear the current configuration?",
                "Confirm clear configuration", DialogButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                Device?.Clear();
                FunctionGroupManager?.Reset();
            }
        }

        public void RefreshAvailableDevices()
        {
            var inputDeviceName = InputDeviceName;
            InputDevices.Clear();
            DeviceManager.UpdateInputDevices();
            foreach (var inputDevice in DeviceManager.InputDevices)
            {
                InputDevices.Add(inputDevice);
            }
            InputDeviceName = inputDeviceName;

            var outputDeviceName = OutputDeviceName;
            OutputDevices.Clear();
            DeviceManager.UpdateOutputDevices();
            foreach (var outputDevice in DeviceManager.OutputDevices)
            {
                OutputDevices.Add(outputDevice);
            }
            OutputDeviceName = outputDeviceName;
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