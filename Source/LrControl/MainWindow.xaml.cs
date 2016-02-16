using System.Windows;
using micdah.LrControlApi.Modules.LrDevelopController.Parameters;

namespace micdah.LrControl
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly LrControlApi.LrControlApi _api;

        public MainWindow()
        {
            InitializeComponent();

            UpdateConnectionStatus(false, null);

            _api = new LrControlApi.LrControlApi(52008, 52009);
            _api.ConnectionStatus += UpdateConnectionStatus;
        }


        private void UpdateConnectionStatus(bool connected, string apiVersion)
        {
            Dispatcher.InvokeAsync(() =>
            {
                Connected.Text = connected ? "yes" : "no";
                ApiVersion.Text = connected ? apiVersion : string.Empty;
            });
        }

        private void DoStuff_OnClick(object sender, RoutedEventArgs e)
        {
            int value;
            if (_api.LrDevelopController.GetValue(out value, AdjustPanelParameter.Exposure))
            {
                Dispatcher.InvokeAsync(() => Response.Text = $"Value = {value}");
            }
        }

        private void Decrement_OnClick(object sender, RoutedEventArgs e)
        {
            _api.LrDevelopController.Decrement(AdjustPanelParameter.Exposure);
        }
    }
}