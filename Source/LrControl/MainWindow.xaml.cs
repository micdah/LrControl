using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using micdah.LrControl.Mapping;
using micdah.LrControl.Mapping.Functions;
using micdah.LrControlApi;
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
        private readonly LrApi _api;
        private readonly IInputDevice _inputDevice;
        private readonly IOutputDevice _outputDevice;

        // Faders
        private Controller _fader0 = new Controller(Channel.Channel1, ControllerType.Nrpn, 0, new Range(0, 1023));
        private Controller _fader1 = new Controller(Channel.Channel1, ControllerType.Nrpn, 1, new Range(0, 1023));
        private Controller _fader2 = new Controller(Channel.Channel1, ControllerType.Nrpn, 2, new Range(0, 1023));
        private Controller _fader3 = new Controller(Channel.Channel1, ControllerType.Nrpn, 3, new Range(0, 1023));
        private Controller _fader4 = new Controller(Channel.Channel1, ControllerType.Nrpn, 4, new Range(0, 1023));
        private Controller _fader5 = new Controller(Channel.Channel1, ControllerType.Nrpn, 5, new Range(0, 1023));
        private Controller _fader6 = new Controller(Channel.Channel1, ControllerType.Nrpn, 6, new Range(0, 1023));
        private Controller _fader7 = new Controller(Channel.Channel1, ControllerType.Nrpn, 7, new Range(0, 1023));

        // Encoders
        private Controller _encoder0 = new Controller(Channel.Channel1, ControllerType.Nrpn, 8, new Range(0, 511));
        private Controller _encoder1 = new Controller(Channel.Channel1, ControllerType.Nrpn, 9, new Range(0, 511));
        private Controller _encoder2 = new Controller(Channel.Channel1, ControllerType.Nrpn, 10, new Range(0, 511));
        private Controller _encoder3 = new Controller(Channel.Channel1, ControllerType.Nrpn, 11, new Range(0, 511));
        private Controller _encoder4 = new Controller(Channel.Channel1, ControllerType.Nrpn, 12, new Range(0, 511));
        private Controller _encoder5 = new Controller(Channel.Channel1, ControllerType.Nrpn, 13, new Range(0, 511));
        private Controller _encoder6 = new Controller(Channel.Channel1, ControllerType.Nrpn, 14, new Range(0, 511));
        private Controller _encoder7 = new Controller(Channel.Channel1, ControllerType.Nrpn, 15, new Range(0, 511));

        // Buttons
        private Controller _button0 = new Controller(Channel.Channel1, ControllerType.ControlChange, 0, new Range(0,127));
        private Controller _button1 = new Controller(Channel.Channel1, ControllerType.ControlChange, 1, new Range(0, 127));
        private Controller _button2 = new Controller(Channel.Channel1, ControllerType.ControlChange, 2, new Range(0, 127));
        private Controller _button3 = new Controller(Channel.Channel1, ControllerType.ControlChange, 3, new Range(0, 127));
        private Controller _button4 = new Controller(Channel.Channel1, ControllerType.ControlChange, 4, new Range(0, 127));
        private Controller _button5 = new Controller(Channel.Channel1, ControllerType.ControlChange, 5, new Range(0, 127));
        private Controller _button6 = new Controller(Channel.Channel1, ControllerType.ControlChange, 6, new Range(0, 127));
        private Controller _button7 = new Controller(Channel.Channel1, ControllerType.ControlChange, 7, new Range(0, 127));
        private Controller _button8 = new Controller(Channel.Channel1, ControllerType.ControlChange, 8, new Range(0, 127));
        private Controller _button9 = new Controller(Channel.Channel1, ControllerType.ControlChange, 9, new Range(0, 127));
        private Controller _button10 = new Controller(Channel.Channel1, ControllerType.ControlChange, 10, new Range(0, 127));
        private Controller _button11 = new Controller(Channel.Channel1, ControllerType.ControlChange, 11, new Range(0, 127));
        private Controller _button12 = new Controller(Channel.Channel1, ControllerType.ControlChange, 12, new Range(0, 127));
        private Controller _button13 = new Controller(Channel.Channel1, ControllerType.ControlChange, 13, new Range(0, 127));
        private Controller _button14 = new Controller(Channel.Channel1, ControllerType.ControlChange, 14, new Range(0, 127));
        private Controller _button15 = new Controller(Channel.Channel1, ControllerType.ControlChange, 15, new Range(0, 127));
        private Controller _button16 = new Controller(Channel.Channel1, ControllerType.ControlChange, 16, new Range(0, 127));
        private Controller _button17 = new Controller(Channel.Channel1, ControllerType.ControlChange, 17, new Range(0, 127));
        private Controller _button18 = new Controller(Channel.Channel1, ControllerType.ControlChange, 18, new Range(0, 127));
        private Controller _button19 = new Controller(Channel.Channel1, ControllerType.ControlChange, 19, new Range(0, 127));
        private Controller _button20 = new Controller(Channel.Channel1, ControllerType.ControlChange, 20, new Range(0, 127));
        private Controller _button21 = new Controller(Channel.Channel1, ControllerType.ControlChange, 21, new Range(0, 127));
        private Controller _button22 = new Controller(Channel.Channel1, ControllerType.ControlChange, 22, new Range(0, 127));
        private Controller _button23 = new Controller(Channel.Channel1, ControllerType.ControlChange, 23, new Range(0, 127));
        private Controller _button24 = new Controller(Channel.Channel1, ControllerType.ControlChange, 24, new Range(0, 127));
        private Controller _button25 = new Controller(Channel.Channel1, ControllerType.ControlChange, 25, new Range(0, 127));
        private Controller _button26 = new Controller(Channel.Channel1, ControllerType.ControlChange, 26, new Range(0, 127));
        private Controller _button27 = new Controller(Channel.Channel1, ControllerType.ControlChange, 27, new Range(0, 127));
        private Controller _button28 = new Controller(Channel.Channel1, ControllerType.ControlChange, 28, new Range(0, 127));
        private Controller _button29 = new Controller(Channel.Channel1, ControllerType.ControlChange, 29, new Range(0, 127));
        private Controller _button30 = new Controller(Channel.Channel1, ControllerType.ControlChange, 30, new Range(0, 127));
        private Controller _button31 = new Controller(Channel.Channel1, ControllerType.ControlChange, 31, new Range(0, 127));
        private Controller _button32 = new Controller(Channel.Channel1, ControllerType.ControlChange, 32, new Range(0, 127));
        private Controller _button33 = new Controller(Channel.Channel1, ControllerType.ControlChange, 33, new Range(0, 127));
        private Controller _button34 = new Controller(Channel.Channel1, ControllerType.ControlChange, 34, new Range(0, 127));
        private Controller _button35 = new Controller(Channel.Channel1, ControllerType.ControlChange, 35, new Range(0, 127));
        private Controller _button36 = new Controller(Channel.Channel1, ControllerType.ControlChange, 36, new Range(0, 127));
        private Controller _button37 = new Controller(Channel.Channel1, ControllerType.ControlChange, 37, new Range(0, 127));

        public MainWindow()
        {
            InitializeComponent();

            UpdateConnectionStatus(false, null);

            _api = new LrApi();
            _api.ConnectionStatus += UpdateConnectionStatus;
            _api.LrDevelopController.ParameterChanged += LrDevelopControllerOnParameterChanged;
            _api.LrApplicationView.ModuleChanged += LrApplicationViewOnModuleChanged;

            _inputDevice = DeviceManager.InputDevices.Single(x => x.Name == "BCF2000");
            _inputDevice.Open();
            _inputDevice.StartReceiving(null);

            _outputDevice = DeviceManager.OutputDevices.Single(x => x.Name == "BCF2000");
            _outputDevice.Open();

            // Controllers
            var controllerManager = new ControllerManager(new List<Controller>
            {
                _fader0,_fader1,_fader2,_fader3,_fader4,_fader5,_fader6,_fader7,
                _encoder0,_encoder1,_encoder2,_encoder3,_encoder4,_encoder5,_encoder6,_encoder7,
                _button0,_button1,_button2,_button3,_button4,_button5,_button6,_button7,_button8,_button9,
                _button10,_button11,_button12,_button13,_button14,_button15,_button16,_button17,_button18,_button19,
                _button20,_button21,_button22,_button23,_button24,_button25,_button26,_button27,_button28,_button29,
                _button30,_button31,_button32,_button33,_button34,_button35,_button36,_button37
            });
            controllerManager.SetInputDevice(_inputDevice);
            controllerManager.SetOutputDevice(_outputDevice);

            var functionCatalog = FunctionCatalog.DefaultCatalog(_api);



            // Develop group
            var developGroup = new ModuleGroup(Module.Develop,
                new[] {CreateBasicGroup(), CreateToneCurveGroup(), CreateGlobalDevelopGroup()});

            // Library group
            var libraryGroup = new ModuleGroup(Module.Library,
                new[] {CreateGlobalLibraryGroup()});

            // Enable module group matching currently active module
            Module currentModule;
            if (_api.LrApplicationView.GetCurrentModuleName(out currentModule))
            {
                ModuleGroup.EnableGroupFor(currentModule);
            }

            // Automatically switch module group
            _api.LrApplicationView.ModuleChanged += ModuleGroup.EnableGroupFor;
        }

        private FunctionGroup CreateGlobalDevelopGroup()
        {
            return new FunctionGroup(_api, functions:new List<Function>
            {
                // Misc. functions
                CreateFunc(_button24, api => api.LrApplicationView.SwitchToModule(Module.Library)),
                CreateFunc(_button26, api => api.LrDevelopController.ResetAllDevelopAdjustments()),
                CreateToggleParam(_button27, Parameters.MixerPanelParameters.ConvertToGrayscale),

                // Switch images
                CreateFunc(_button32, api => api.LrSelection.PreviousPhoto()),
                CreateFunc(_button33, api => api.LrSelection.NextPhoto()),
                
                // Undo/Redo
                CreateFunc(_button34, api =>
                {
                    api.LrDevelopController.StopTracking();
                    api.LrUndo.Undo();
                }),
                CreateFunc(_button36, api =>
                {
                    api.LrDevelopController.StopTracking();
                    api.LrUndo.Redo();
                }),

                // Switch panels
                CreateEnableGroup(_button0, Panel.Basic, null),
                CreateEnableGroup(_button2, Panel.ToneCurve, Parameters.EnablePanelParameters.ToneCurve),
            });
        }

        private FunctionGroup CreateGlobalLibraryGroup()
        {
            return new FunctionGroup(_api, functions:new List<Function>
            {
                // Misc. functions
                CreateFunc(_button24, api => api.LrApplicationView.SwitchToModule(Module.Develop)),

                // Switch images
                CreateFunc(_button32, api => api.LrSelection.PreviousPhoto()),
                CreateFunc(_button33, api => api.LrSelection.NextPhoto()),

                // Undo/Redo
                CreateFunc(_button34, api => api.LrUndo.Undo()),
                CreateFunc(_button36, api => api.LrUndo.Redo()),
            });
        }

        private FunctionGroup CreateBasicGroup()
        {
            return new FunctionGroup(_api, Panel.Basic, new List<Function>
            {
                // Faders
                CreateParam(_fader0, Parameters.AdjustPanelParameters.Temperature),
                CreateParam(_fader1, Parameters.AdjustPanelParameters.Tint),
                CreateParam(_fader2, Parameters.AdjustPanelParameters.Exposure),
                CreateParam(_fader3, Parameters.AdjustPanelParameters.Contrast),
                CreateParam(_fader4, Parameters.AdjustPanelParameters.Highlights),
                CreateParam(_fader5, Parameters.AdjustPanelParameters.Shadows),
                CreateParam(_fader6, Parameters.AdjustPanelParameters.Whites),
                CreateParam(_fader7, Parameters.AdjustPanelParameters.Blacks),

                // Encoders
                CreateParam(_encoder0, Parameters.AdjustPanelParameters.Clarity),
                CreateParam(_encoder1, Parameters.AdjustPanelParameters.Vibrance),
                CreateParam(_encoder2, Parameters.AdjustPanelParameters.Saturation),

                // Reset buttons
                CreateFunc(_button1, api => api.LrDevelopController.ResetToDefault(Parameters.AdjustPanelParameters.Temperature)),
                CreateFunc(_button3, api => api.LrDevelopController.ResetToDefault(Parameters.AdjustPanelParameters.Tint)),
                CreateFunc(_button5, api => api.LrDevelopController.ResetToDefault(Parameters.AdjustPanelParameters.Exposure)),
                CreateFunc(_button7, api => api.LrDevelopController.ResetToDefault(Parameters.AdjustPanelParameters.Contrast)),
                CreateFunc(_button9, api => api.LrDevelopController.ResetToDefault(Parameters.AdjustPanelParameters.Highlights)),
                CreateFunc(_button11, api => api.LrDevelopController.ResetToDefault(Parameters.AdjustPanelParameters.Shadows)),
                CreateFunc(_button13, api => api.LrDevelopController.ResetToDefault(Parameters.AdjustPanelParameters.Whites)),
                CreateFunc(_button15, api => api.LrDevelopController.ResetToDefault(Parameters.AdjustPanelParameters.Blacks)),

                CreateFunc(_button16, api => api.LrDevelopController.ResetToDefault(Parameters.AdjustPanelParameters.Clarity)),
                CreateFunc(_button17, api => api.LrDevelopController.ResetToDefault(Parameters.AdjustPanelParameters.Vibrance)),
                CreateFunc(_button18, api => api.LrDevelopController.ResetToDefault(Parameters.AdjustPanelParameters.Saturation)),
            });
        }

        private FunctionGroup CreateToneCurveGroup()
        {
            return new FunctionGroup(_api, Panel.ToneCurve, new List<Function>
            {
                // Faders
                CreateParam(_fader0, Parameters.TonePanelParameters.ParametricHighlights),
                CreateParam(_fader1, Parameters.TonePanelParameters.ParametricLights),
                CreateParam(_fader2, Parameters.TonePanelParameters.ParametricDarks),
                CreateParam(_fader3, Parameters.TonePanelParameters.ParametricShadows),
                CreateParam(_fader4, Parameters.TonePanelParameters.ParametricShadowSplit),
                CreateParam(_fader5, Parameters.TonePanelParameters.ParametricMidtoneSplit),
                CreateParam(_fader6, Parameters.TonePanelParameters.ParametricHighlightSplit),

                // Reset buttons
                CreateFunc(_button1, api => api.LrDevelopController.ResetToDefault(Parameters.TonePanelParameters.ParametricHighlights)),
                CreateFunc(_button3, api => api.LrDevelopController.ResetToDefault(Parameters.TonePanelParameters.ParametricLights)),
                CreateFunc(_button5, api => api.LrDevelopController.ResetToDefault(Parameters.TonePanelParameters.ParametricDarks)),
                CreateFunc(_button7, api => api.LrDevelopController.ResetToDefault(Parameters.TonePanelParameters.ParametricShadows)),
                CreateFunc(_button9, api => api.LrDevelopController.ResetToDefault(Parameters.TonePanelParameters.ParametricShadowSplit)),
                CreateFunc(_button11, api => api.LrDevelopController.ResetToDefault(Parameters.TonePanelParameters.ParametricMidtoneSplit)),
                CreateFunc(_button13, api => api.LrDevelopController.ResetToDefault(Parameters.TonePanelParameters.ParametricHighlightSplit)),
            });
        }

        private ParameterFunction CreateParam(Controller controller, IParameter parameter)
        {
            return new ParameterFunction(_api, parameter) {Controller = controller};
        }

        private ToggleParameterFunction CreateToggleParam(Controller controller, IParameter<bool> parameter)
        {
            return new ToggleParameterFunction(_api, parameter) {Controller = controller};
        }

        private MethodFunction CreateFunc(Controller controller, Action<LrApi> method)
        {
            return new MethodFunction(_api, method, "Missing display text") {Controller = controller};
        }

        private EnablePanelFunction CreateEnableGroup(Controller controller, Panel panel, IParameter<bool> enablePanelParameter)
        {
            return new EnablePanelFunction(_api, panel, enablePanelParameter) {Controller = controller};
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
            //double value;
            //if (_api.LrDevelopController.GetValue(out value, Parameters.AdjustPanelParameters.Exposure))
            //{
            //    _api.LrDevelopController.SetValue(Parameters.AdjustPanelParameters.Exposure, value + 0.25);
            //}

            _api.LrDevelopController.SetValue(Parameters.EnablePanelParameters.ToneCurve, false);
        }
    }
}