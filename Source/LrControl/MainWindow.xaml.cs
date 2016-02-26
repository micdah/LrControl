using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using micdah.LrControl.Annotations;
using micdah.LrControl.Configurations;
using micdah.LrControlApi;
using Midi.Devices;

namespace micdah.LrControl
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private readonly LrApi _api;
        private readonly IInputDevice _inputDevice;
        private readonly IOutputDevice _outputDevice;
        private MainWindowModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            UpdateConnectionStatus(false, null);

            _api = new LrApi();
            _api.ConnectionStatus += UpdateConnectionStatus;

            _inputDevice = DeviceManager.InputDevices.Single(x => x.Name == "BCF2000");
            _inputDevice.Open();
            _inputDevice.StartReceiving(null);

            _outputDevice = DeviceManager.OutputDevices.Single(x => x.Name == "BCF2000");
            _outputDevice.Open();

            // View model
            ViewModel = new MainWindowModel(_api)
            {
                InputDevice = _inputDevice,
                OutputDevice = _outputDevice
            };
            ViewModel.LoadConfiguration();
        }

        public MainWindowModel ViewModel
        {
            get { return _viewModel; }
            set
            {
                if (Equals(value, _viewModel)) return;
                _viewModel = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void UpdateConnectionStatus(bool connected, string apiVersion)
        {
            Dispatcher.InvokeAsync(
                () => { Connected.Text = $"{(connected ? $"Connected ({apiVersion})" : "Not connected")}"; });
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            if (Settings.Current.AutoSaveOnClose)
            {
                ViewModel.SaveConfiguration();
            }

            Settings.Current.Save();
        }
    }
}