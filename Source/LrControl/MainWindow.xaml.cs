using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using micdah.LrControl.Annotations;
using micdah.LrControl.Configurations;
using micdah.LrControl.Core;
using micdah.LrControl.Core.Midi;
using micdah.LrControlApi;
using Midi.Devices;

namespace micdah.LrControl
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private MainWindowModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            UpdateConnectionStatus(false, null);

            var api = new LrApi();
            api.ConnectionStatus += UpdateConnectionStatus;

            IInputDevice inputDevice =
                new InputDeviceDecorator(DeviceManager.InputDevices.Single(x => x.Name == "BCF2000"));
            inputDevice.Open();
            inputDevice.StartReceiving(null);

            var outputDevice = DeviceManager.OutputDevices.Single(x => x.Name == "BCF2000");
            outputDevice.Open();

            // View model
            ViewModel = new MainWindowModel(api, new MetroWindowDialogProvider(this))
            {
                InputDevice = inputDevice,
                OutputDevice = outputDevice
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