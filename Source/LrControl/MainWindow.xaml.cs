using System.Collections.Generic;
using System.Text;
using System.Windows;
using micdah.LrControlApi.Modules.LrDevelopController;
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

        private void GetAllParameterValues_OnClick(object sender, RoutedEventArgs e)
        {
            var response = new StringBuilder();

            response.AppendLine("Adjust panel parameters");
            EnumerateParameters(response, Parameters.AdjustPanelParameters.AllParameters);

            Dispatcher.InvokeAsync(() => Response.Text = response.ToString());
        }

        private void EnumerateParameters(StringBuilder response, IList<IParameter> parameters)
        {
            foreach (var param in parameters)
            {
                if (param is IParameter<int>)
                {
                    int value;
                    response.AppendLine(_api.LrDevelopController.GetValue(out value, (IParameter<int>) param)
                        ? $"{param.DisplayName} = {value}"
                        : $"{param.DisplayName} = Error retrieving");
                }
                else if (param is IParameter<double>)
                {
                    double value;
                    response.AppendLine(_api.LrDevelopController.GetValue(out value, (IParameter<double>) param)
                        ? $"{param.DisplayName} = {value}"
                        : $"{param.DisplayName} = Error retrieving");
                }
                else if (param is IParameter<string>)
                {
                    string value;
                    response.AppendLine(_api.LrDevelopController.GetValue(out value, (IParameter<string>) param)
                        ? $"{param.DisplayName} = {value}"
                        : $"{param.DisplayName} = Error retrieving");
                }
                else if (param is IParameter<bool>)
                {
                    bool value;
                    response.AppendLine(_api.LrDevelopController.GetValue(out value, (IParameter<bool>) param)
                        ? $"{param.DisplayName} = {value}"
                        : $"{param.DisplayName} = Error retrieving");
                }
                else if (param is IParameter<Tool>)
                {
                    Tool value;
                    response.AppendLine(_api.LrDevelopController.GetValue(out value, (IParameter<Tool>) param)
                        ? $"{param.DisplayName} = {value}"
                        : $"{param.DisplayName} = Error retrieving");
                }
                else if (param is IParameter<UprightValue>)
                {
                    UprightValue value;
                    response.AppendLine(_api.LrDevelopController.GetValue(out value, (IParameter<UprightValue>) param)
                        ? $"{param.DisplayName} = {value}"
                        : $"{param.DisplayName} = Error retrieving");
                }
                else if (param is IParameter<WhiteBalanceValue>)
                {
                    WhiteBalanceValue value;
                    response.AppendLine(_api.LrDevelopController.GetValue(out value,
                        (IParameter<WhiteBalanceValue>) param)
                        ? $"{param.DisplayName} = {value}"
                        : $"{param.DisplayName} = Error retrieving");
                }
            }
        }

        private void Decrement_OnClick(object sender, RoutedEventArgs e)
        {
            _api.LrDevelopController.Decrement(Parameters.AdjustPanelParameters.Exposure);
        }
    }
}