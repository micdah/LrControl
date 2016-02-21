using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using micdah.LrControlApi.Common;
using micdah.LrControlApi.Modules.LrApplicationView;
using micdah.LrControlApi.Modules.LrDevelopController;
using micdah.LrControlApi.Modules.LrDevelopController.Parameters;
using Midi.Devices;
using Midi.Enums;

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


            _api = new LrControlApi.LrControlApi();
            _api.ConnectionStatus += UpdateConnectionStatus;
            _api.LrDevelopController.ParameterChanged += LrDevelopControllerOnParameterChanged;
            _api.LrApplicationView.ModuleChanged += LrApplicationViewOnModuleChanged;

            var exposureRange = new Range(-5, 5);
            var faderRange = new Range(0, 1023);
            var inputDevice = DeviceManager.InputDevices.Single(x => x.Name == "BCF2000");
            inputDevice.Nrpn += msg =>
            {
                if (msg.Parameter == 0)
                {
                    var value = exposureRange.FromRange(faderRange, msg.Value);
                    _api.LrDevelopController.SetValue(Parameters.AdjustPanelParameters.Exposure, value);
                }
            };

            inputDevice.Open();
            inputDevice.StartReceiving(null);

            var outputDevice = DeviceManager.OutputDevices.Single(x => x.Name == "BCF2000");
            outputDevice.Open();

            _api.LrDevelopController.ParameterChanged += parameter =>
            {
                if (parameter == Parameters.AdjustPanelParameters.Exposure)
                {
                    double value;
                    if (_api.LrDevelopController.GetValue(out value, Parameters.AdjustPanelParameters.Exposure))
                    {
                        outputDevice.SendNrpn(Channel.Channel1, 0,
                            Convert.ToInt32(faderRange.FromRange(exposureRange, value)));
                    }
                }
            };
        }

        private void LrApplicationViewOnModuleChanged(Module newModule)
        {
            Dispatcher.InvokeAsync(() =>
            {
                Response.Text = $"Changed module: {newModule.Name}";
            });
        }

        private void LrDevelopControllerOnParameterChanged(IParameter parameter)
        {
            if (parameter == Parameters.AdjustPanelParameters.Exposure)
            {
                double exposure;
                if (_api.LrDevelopController.GetValue(out exposure, Parameters.AdjustPanelParameters.Exposure))
                {
                    Dispatcher.InvokeAsync(() =>
                    {
                        Response.Text = $"{parameter.DisplayName} = {exposure}";
                    });
                }
            }
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
            _api.LrDialogs.ShowBezel("Getting all develop parameters...");

            var response = new StringBuilder();

            Module module;
            if (_api.LrApplicationView.GetCurrentModuleName(out module))
            {
                if (module != Module.Develop)
                {
                    _api.LrApplicationView.SwitchToModule(Module.Develop);
                }

                foreach (var group in Parameters.AllGroups)
                {
                    response.AppendLine($"{new String('<', 20)}{group.Name} parameters{new String('>', 20)}");
                    EnumerateParameters(response, group.AllParameters);
                }
            }
            else
            {
                response.AppendLine("Unable to get current module");
            }

            Dispatcher.InvokeAsync(() => Response.Text = response.ToString());
        }

        private void EnumerateParameters(StringBuilder response, IList<IParameter> parameters)
        {
            foreach (var param in parameters)
            {
                if (param is IParameter<int>)
                {
                    int value;
                    response.Append(_api.LrDevelopController.GetValue(out value, (IParameter<int>) param)
                        ? $"{param.DisplayName} = {value}"
                        : $"{param.DisplayName} = Error retrieving");
                }
                else if (param is IParameter<double>)
                {
                    double value;
                    response.Append(_api.LrDevelopController.GetValue(out value, (IParameter<double>) param)
                        ? $"{param.DisplayName} = {value}"
                        : $"{param.DisplayName} = Error retrieving");
                }
                else if (param is IParameter<string>)
                {
                    string value;
                    response.Append(_api.LrDevelopController.GetValue(out value, (IParameter<string>) param)
                        ? $"{param.DisplayName} = {value}"
                        : $"{param.DisplayName} = Error retrieving");
                }
                else if (param is IParameter<bool>)
                {
                    bool value;
                    response.Append(_api.LrDevelopController.GetValue(out value, (IParameter<bool>) param)
                        ? $"{param.DisplayName} = {value}"
                        : $"{param.DisplayName} = Error retrieving");
                }
                else if (param is IParameter<Tool>)
                {
                    Tool value;
                    response.Append(_api.LrDevelopController.GetValue(out value, (IParameter<Tool>) param)
                        ? $"{param.DisplayName} = {value}"
                        : $"{param.DisplayName} = Error retrieving");
                }
                else if (param is IParameter<UprightValue>)
                {
                    UprightValue value;
                    response.Append(_api.LrDevelopController.GetValue(out value, (IParameter<UprightValue>) param)
                        ? $"{param.DisplayName} = {value}"
                        : $"{param.DisplayName} = Error retrieving");
                }
                else if (param is IParameter<WhiteBalanceValue>)
                {
                    WhiteBalanceValue value;
                    response.Append(_api.LrDevelopController.GetValue(out value,
                        (IParameter<WhiteBalanceValue>) param)
                        ? $"{param.DisplayName} = {value}"
                        : $"{param.DisplayName} = Error retrieving");
                } else if (param is IParameter<PostCropVignetteStyle>)
                {
                    PostCropVignetteStyle value;
                    response.Append(_api.LrDevelopController.GetValue(out value,
                        (IParameter<PostCropVignetteStyle>)param)
                        ? $"{param.DisplayName} = {value}"
                        : $"{param.DisplayName} = Error retrieving");
                } else if (param is IParameter<ProfileValue>)
                {
                    ProfileValue value;
                    response.Append(_api.LrDevelopController.GetValue(out value,
                        (IParameter<ProfileValue>) param)
                        ? $"{param.DisplayName} = {value}"
                        : $"{param.DisplayName} = Error retrieving");
                }
                else
                {
                    response.Append($"{new String('!', 20)} Unknown parameter {param.GetType()}");
                }

                Range range;
                response.AppendLine(_api.LrDevelopController.GetRange(out range, param)
                    ? $" (min = {range.Minimum}, max = {range.Maximum})"
                    : " (min = ?, max = ?)");
            }
        }

        private void Decrement_OnClick(object sender, RoutedEventArgs e)
        {
            double value;
            if (_api.LrDevelopController.GetValue(out value, Parameters.AdjustPanelParameters.Exposure))
            {
                _api.LrDevelopController.SetValue(Parameters.AdjustPanelParameters.Exposure, value + 0.25);
            }
        }
    }
}