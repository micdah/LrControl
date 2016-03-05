using System;
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

            // View model
            ViewModel = new MainWindowModel(api, new MetroWindowDialogProvider(this));
            
            // Find last used input device
            if (!String.IsNullOrEmpty(Settings.Current.LastUsedInputDevice))
            {
                var inputDevice =
                    DeviceManager.InputDevices.FirstOrDefault(x => x.Name == Settings.Current.LastUsedInputDevice);
                if (inputDevice != null)
                {
                    inputDevice.Open();
                    inputDevice.StartReceiving(null);
                    ViewModel.InputDevice = new InputDeviceDecorator(inputDevice);
                }
            }

            // Find last used output device
            if (!String.IsNullOrEmpty(Settings.Current.LastUsedOutputDevice))
            {
                var outputDevice = DeviceManager.OutputDevices
                    .FirstOrDefault(x => x.Name == Settings.Current.LastUsedOutputDevice);
                if (outputDevice != null)
                {
                    outputDevice.Open();
                    ViewModel.OutputDevice = outputDevice;
                }
            }

            // Load configuration
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

            Settings.Current.LastUsedInputDevice  = ViewModel.InputDevice?.Name;
            Settings.Current.LastUsedOutputDevice = ViewModel.OutputDevice?.Name;
            Settings.Current.Save();
        }
    }
}