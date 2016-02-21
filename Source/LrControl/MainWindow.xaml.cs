using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using micdah.LrControl.Functions;
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
        private readonly IInputDevice _inputDevice;
        private readonly IOutputDevice _outputDevice;

        public MainWindow()
        {
            InitializeComponent();

            UpdateConnectionStatus(false, null);

            _api = new LrControlApi.LrControlApi();
            _api.ConnectionStatus += UpdateConnectionStatus;
            _api.LrDevelopController.ParameterChanged += LrDevelopControllerOnParameterChanged;
            _api.LrApplicationView.ModuleChanged += LrApplicationViewOnModuleChanged;

            _inputDevice = DeviceManager.InputDevices.Single(x => x.Name == "BCF2000");
            _inputDevice.Open();
            _inputDevice.StartReceiving(null);

            _outputDevice = DeviceManager.OutputDevices.Single(x => x.Name == "BCF2000");
            _outputDevice.Open();

            // Develop group
            var developGroup = new ModuleGroup(_api, Module.Develop);

            var basicGroup = CreateBasicGroup();
            basicGroup.SetOutputDevice(_outputDevice);
            basicGroup.SetInputDevice(_inputDevice);
            developGroup.FunctionGroups.Add(basicGroup);

            var toneCurveGroup = CreateToneCurveGroup();
            toneCurveGroup.SetOutputDevice(_outputDevice);
            toneCurveGroup.SetInputDevice(_inputDevice);
            developGroup.FunctionGroups.Add(toneCurveGroup);


            var globalDevelopGroup = CreateGlobalDevelopGroup(basicGroup, toneCurveGroup);
            globalDevelopGroup.SetOutputDevice(_outputDevice);
            globalDevelopGroup.SetInputDevice(_inputDevice);
            developGroup.FunctionGroups.Add(globalDevelopGroup);

            // Library group
            var libraryGroup = new ModuleGroup(_api, Module.Library);

            var globalLibraryGroup = CreateGlobalLibraryGroup();
            globalLibraryGroup.SetOutputDevice(_outputDevice);
            globalLibraryGroup.SetInputDevice(_inputDevice);
            libraryGroup.FunctionGroups.Add(globalLibraryGroup);

            // Enable module group matching currently active module
            Module currentModule;
            if (_api.LrApplicationView.GetCurrentModuleName(out currentModule))
            {
                ModuleGroup.EnableGroupFor(currentModule);
            }

            // Automatically switch module group
            _api.LrApplicationView.ModuleChanged += ModuleGroup.EnableGroupFor;
        }

        private FunctionGroup CreateGlobalDevelopGroup(FunctionGroup basicGroup, FunctionGroup toneCurveGroup)
        {
            var global = new FunctionGroup(_api);

            // Misc. functions
            global.Functions.Add(CreateFunc(24, api => api.LrApplicationView.SwitchToModule(Module.Library)));
            global.Functions.Add(CreateFunc(26, api => api.LrDevelopController.ResetAllDevelopAdjustments()));
            //global.Functions.Add(CreateToggleParam(25, Parameters.)
            global.Functions.Add(CreateToggleParam(27, Parameters.MixerPanelParameters.ConvertToGrayscale));

            // Switch images
            global.Functions.Add(CreateFunc(32, api => api.LrSelection.PreviousPhoto()));
            global.Functions.Add(CreateFunc(33, api => api.LrSelection.NextPhoto()));

            // Undo/Redo
            global.Functions.Add(CreateFunc(34, api => api.LrUndo.Undo()));
            global.Functions.Add(CreateFunc(35, api => api.LrUndo.Redo()));

            // Switch panels
            global.Functions.Add(CreateEnableGroup(0, basicGroup, null));
            global.Functions.Add(CreateEnableGroup(2, toneCurveGroup, Parameters.EnablePanelParameters.ToneCurve));

            return global;
        }

        private FunctionGroup CreateGlobalLibraryGroup()
        {
            var global = new FunctionGroup(_api);

            // Misc. functions
            global.Functions.Add(CreateFunc(24, api => api.LrApplicationView.SwitchToModule(Module.Develop)));

            // Switch images
            global.Functions.Add(CreateFunc(32, api => api.LrSelection.PreviousPhoto()));
            global.Functions.Add(CreateFunc(33, api => api.LrSelection.NextPhoto()));

            // Undo/Redo
            global.Functions.Add(CreateFunc(34, api => api.LrUndo.Undo()));
            global.Functions.Add(CreateFunc(35, api => api.LrUndo.Redo()));

            return global;
        }

        private FunctionGroup CreateBasicGroup()
        {
            var basic = new FunctionGroup(_api, Panel.Basic);

            // Faders
            basic.Functions.Add(CreateParam(ControllerType.Nrpn, 0,new Range(0, 1023), Parameters.AdjustPanelParameters.Temperature));
            basic.Functions.Add(CreateParam(ControllerType.Nrpn, 1,new Range(0, 1023), Parameters.AdjustPanelParameters.Tint));
            basic.Functions.Add(CreateParam(ControllerType.Nrpn, 2,new Range(0, 1023), Parameters.AdjustPanelParameters.Exposure));
            basic.Functions.Add(CreateParam(ControllerType.Nrpn, 3,new Range(0, 1023), Parameters.AdjustPanelParameters.Contrast));
            basic.Functions.Add(CreateParam(ControllerType.Nrpn, 4,new Range(0, 1023), Parameters.AdjustPanelParameters.Highlights));
            basic.Functions.Add(CreateParam(ControllerType.Nrpn, 5,new Range(0, 1023), Parameters.AdjustPanelParameters.Shadows));
            basic.Functions.Add(CreateParam(ControllerType.Nrpn, 6,new Range(0, 1023), Parameters.AdjustPanelParameters.Whites));
            basic.Functions.Add(CreateParam(ControllerType.Nrpn, 7,new Range(0, 1023), Parameters.AdjustPanelParameters.Blacks));

            // Encoders
            basic.Functions.Add(CreateParam(ControllerType.Nrpn,8,new Range(0, 511), Parameters.AdjustPanelParameters.Clarity));
            basic.Functions.Add(CreateParam(ControllerType.Nrpn,9,new Range(0, 511), Parameters.AdjustPanelParameters.Vibrance));
            basic.Functions.Add(CreateParam(ControllerType.Nrpn,10,new Range(0, 511), Parameters.AdjustPanelParameters.Saturation));

            // Reset buttons
            basic.Functions.Add(CreateFunc(1, api => api.LrDevelopController.ResetToDefault(Parameters.AdjustPanelParameters.Temperature)));
            basic.Functions.Add(CreateFunc(3, api => api.LrDevelopController.ResetToDefault(Parameters.AdjustPanelParameters.Tint)));
            basic.Functions.Add(CreateFunc(5, api => api.LrDevelopController.ResetToDefault(Parameters.AdjustPanelParameters.Exposure)));
            basic.Functions.Add(CreateFunc(7, api => api.LrDevelopController.ResetToDefault(Parameters.AdjustPanelParameters.Contrast)));
            basic.Functions.Add(CreateFunc(9, api => api.LrDevelopController.ResetToDefault(Parameters.AdjustPanelParameters.Highlights)));
            basic.Functions.Add(CreateFunc(11, api => api.LrDevelopController.ResetToDefault(Parameters.AdjustPanelParameters.Shadows)));
            basic.Functions.Add(CreateFunc(13, api => api.LrDevelopController.ResetToDefault(Parameters.AdjustPanelParameters.Whites)));
            basic.Functions.Add(CreateFunc(15, api => api.LrDevelopController.ResetToDefault(Parameters.AdjustPanelParameters.Blacks)));

            basic.Functions.Add(CreateFunc(16, api => api.LrDevelopController.ResetToDefault(Parameters.AdjustPanelParameters.Clarity)));
            basic.Functions.Add(CreateFunc(17, api => api.LrDevelopController.ResetToDefault(Parameters.AdjustPanelParameters.Vibrance)));
            basic.Functions.Add(CreateFunc(18, api => api.LrDevelopController.ResetToDefault(Parameters.AdjustPanelParameters.Saturation)));


            return basic;
        }

        private FunctionGroup CreateToneCurveGroup()
        {
            var toneCurve = new FunctionGroup(_api, Panel.ToneCurve);

            // Faders
            toneCurve.Functions.Add(CreateParam(ControllerType.Nrpn, 0, new Range(0, 1023), Parameters.TonePanelParameters.ParametricHighlights));
            toneCurve.Functions.Add(CreateParam(ControllerType.Nrpn, 1, new Range(0, 1023), Parameters.TonePanelParameters.ParametricLights));
            toneCurve.Functions.Add(CreateParam(ControllerType.Nrpn, 2, new Range(0, 1023), Parameters.TonePanelParameters.ParametricDarks));
            toneCurve.Functions.Add(CreateParam(ControllerType.Nrpn, 3, new Range(0, 1023), Parameters.TonePanelParameters.ParametricShadows));
            toneCurve.Functions.Add(CreateParam(ControllerType.Nrpn, 4, new Range(0, 1023), Parameters.TonePanelParameters.ParametricShadowSplit));
            toneCurve.Functions.Add(CreateParam(ControllerType.Nrpn, 5, new Range(0, 1023), Parameters.TonePanelParameters.ParametricMidtoneSplit));
            toneCurve.Functions.Add(CreateParam(ControllerType.Nrpn, 6, new Range(0, 1023), Parameters.TonePanelParameters.ParametricHighlightSplit));

            // Reset buttons
            toneCurve.Functions.Add(CreateFunc(1, api => api.LrDevelopController.ResetToDefault(Parameters.TonePanelParameters.ParametricHighlights)));
            toneCurve.Functions.Add(CreateFunc(3, api => api.LrDevelopController.ResetToDefault(Parameters.TonePanelParameters.ParametricLights)));
            toneCurve.Functions.Add(CreateFunc(5, api => api.LrDevelopController.ResetToDefault(Parameters.TonePanelParameters.ParametricDarks)));
            toneCurve.Functions.Add(CreateFunc(7, api => api.LrDevelopController.ResetToDefault(Parameters.TonePanelParameters.ParametricShadows)));
            toneCurve.Functions.Add(CreateFunc(9, api => api.LrDevelopController.ResetToDefault(Parameters.TonePanelParameters.ParametricShadowSplit)));
            toneCurve.Functions.Add(CreateFunc(11, api => api.LrDevelopController.ResetToDefault(Parameters.TonePanelParameters.ParametricMidtoneSplit)));
            toneCurve.Functions.Add(CreateFunc(13, api => api.LrDevelopController.ResetToDefault(Parameters.TonePanelParameters.ParametricHighlightSplit)));

            return toneCurve;
        }

        private ParameterFunction CreateParam(ControllerType controllerType, int controlNumber, Range controllerrange,
            IParameter parameter)
        {
            return new ParameterFunction(_api, controllerType, Channel.Channel1, controlNumber, controllerrange,
                parameter);
        }

        private ToggleParameterFunction CreateToggleParam(int controlNumber, IParameter<bool> parameter)
        {
            return new ToggleParameterFunction(_api, ControllerType.ControlChange, Channel.Channel1, controlNumber,
                new Range(0, 127), parameter);
        }

        private MethodFunction CreateFunc(int controlNumber, Action<LrControlApi.LrControlApi> method)
        {
            return new MethodFunction(_api, ControllerType.ControlChange, Channel.Channel1, controlNumber,
                new Range(0, 127), method);
        }

        private EnableFunctionGroupFunction CreateEnableGroup(int controlNumber, FunctionGroup group, IParameter<bool> enablePanelParameter)
        {
            return new EnableFunctionGroupFunction(_api, ControllerType.ControlChange, Channel.Channel1, controlNumber,
                new Range(0, 127), group, enablePanelParameter);
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