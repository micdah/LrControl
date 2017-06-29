using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace micdah.LrControl
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private MainWindowModel _viewModel;

        public MainWindow(MainWindowModel viewModel)
        {
            InitializeComponent();

            UpdateConnectionStatus(false, null);

            ViewModel = viewModel;
            ViewModel.Api.ConnectionStatus += UpdateConnectionStatus;

            UpdateConnectionStatus(ViewModel.Api.Connected, viewModel.Api.ApiVersion);
        }

        public MainWindowModel ViewModel
        {
            get => _viewModel;
            private set
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
    }
}