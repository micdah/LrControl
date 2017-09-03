using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;
using LrControl.Core;
using LrControl.Core.Functions.Catalog;
using LrControl.Ui.Core;
using LrControl.Ui.Gui;
using Midi.Devices;
using Prism.Commands;

namespace LrControl.Ui
{
    public class MainWindowModel : ViewModel
    {
        private readonly ILrControlApplication _lrControlApplication;
        private readonly IMainWindowDialogProvider _dialogProvider;
        private bool _showSettingsDialog;
        private string _connectionStatus = "Not connected";
        private ObservableCollection<IInputDevice> _inputDevices;
        private ObservableCollection<IOutputDevice> _outputDevices;

        public MainWindowModel(Dispatcher dispatcher, ILrControlApplication lrControlApplication, IMainWindowDialogProvider mainWindowDialogProvider) : base(dispatcher)
        {
            _lrControlApplication = lrControlApplication;
            _dialogProvider = mainWindowDialogProvider;
            
            // Commands
            OpenSettingsCommand = new DelegateCommand(OpenSettings);
            SaveCommand = new DelegateCommand(() => _lrControlApplication.SaveConfiguration());
            LoadCommand = new DelegateCommand(() => _lrControlApplication.LoadConfiguration());
            ExportCommand = new DelegateCommand(ExportConfiguration);
            ImportCommand = new DelegateCommand(ImportConfiguration);
            ResetCommand = new DelegateCommand(Reset);
            RefreshAvailableDevicesCommand = new DelegateCommand(() => _lrControlApplication.RefreshAvailableDevices());

            // Initialize catalogs and controllers
            FunctionGroupManagerViewModel = new FunctionGroupManagerViewModel(dispatcher, _lrControlApplication.FunctionGroupManager);

            InputDevices = new ObservableCollection<IInputDevice>(_lrControlApplication.InputDevices);
            OutputDevices = new ObservableCollection<IOutputDevice>(_lrControlApplication.OutputDevices);

            // Listen for connection status
            _lrControlApplication.ConnectionStatus += LrControlApplicationOnConnectionStatus;
            _lrControlApplication.PropertyChanged += LrControlApplicationOnPropertyChanged;

            _lrControlApplication.UpdateConnectionStatus();
        }

        public ICommand OpenSettingsCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand ExportCommand { get; }
        public ICommand LoadCommand { get; }
        public ICommand ImportCommand { get; }
        public ICommand ResetCommand { get; }
        public ICommand RefreshAvailableDevicesCommand { get; }

        public IFunctionCatalog FunctionCatalog => _lrControlApplication.FunctionCatalog;

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

        public string ConnectionStatus
        {
            get => _connectionStatus;
            set
            {
                if (value == _connectionStatus) return;
                _connectionStatus = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<IInputDevice> InputDevices
        {
            get => _inputDevices;
            set
            {
                if (Equals(value, _inputDevices)) return;
                _inputDevices = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<IOutputDevice> OutputDevices
        {
            get => _outputDevices;
            set
            {
                if (Equals(value, _outputDevices)) return;
                _outputDevices = value;
                OnPropertyChanged();
            }
        }

        public string InputDeviceName
        {
            get => _lrControlApplication.InputDevice?.Name;
            set => _lrControlApplication.SetInputDevice(
                _lrControlApplication.InputDevices.FirstOrDefault(x => x.Name == value));
        }

        public string OutputDeviceName
        {
            get => _lrControlApplication.OutputDevice?.Name;
            set => _lrControlApplication.SetOutputDevice(
                _lrControlApplication.OutputDevices.FirstOrDefault(x => x.Name == value));
        }

        public FunctionGroupManagerViewModel FunctionGroupManagerViewModel { get; }

        private void OpenSettings()
        {
            ShowSettingsDialog = !ShowSettingsDialog;
        }

        private void ExportConfiguration()
        {
            var file = _dialogProvider.ShowSaveDialog(_lrControlApplication.GetSettingsFolder());
            if (!string.IsNullOrEmpty(file))
                _lrControlApplication.SaveConfiguration(file);
        }

        private void ImportConfiguration()
        {
            var file = _dialogProvider.ShowOpenDialog(_lrControlApplication.GetSettingsFolder());
            if (!string.IsNullOrEmpty(file))
                _lrControlApplication.LoadConfiguration(file);
        }

        private async void Reset()
        {
            var result = await _dialogProvider.ShowMessage("Are you sure, you want to clear the current configuration?",
                "Confirm clear configuration", DialogButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                _lrControlApplication.Reset();
            }
        }

        protected override void Disposing()
        {
            _lrControlApplication.ConnectionStatus -= LrControlApplicationOnConnectionStatus;
        }

        private void LrControlApplicationOnConnectionStatus(bool connected, string apiVersion)
        {
            SafeInvoke(() =>
            {
                ConnectionStatus = $"{(connected ? $"Connected ({apiVersion})" : "Not connected")}";
            });
        }

        private void LrControlApplicationOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ILrControlApplication.InputDevices):
                    InputDevices = new ObservableCollection<IInputDevice>(_lrControlApplication.InputDevices);
                    break;
                case nameof(ILrControlApplication.OutputDevices):
                    OutputDevices = new ObservableCollection<IOutputDevice>(_lrControlApplication.OutputDevices);
                    break;
                case nameof(ILrControlApplication.InputDevice):
                    OnPropertyChanged(nameof(InputDeviceName));
                    break;
                case nameof(ILrControlApplication.OutputDevice):
                    OnPropertyChanged(nameof(OutputDeviceName));
                    break;
            }
        }
    }
}