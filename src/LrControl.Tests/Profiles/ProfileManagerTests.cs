using System;
using System.Linq;
using System.Threading;
using LrControl.Configurations;
using LrControl.Devices;
using LrControl.Functions;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;
using LrControl.LrPlugin.Api.Modules.LrDevelopController.Parameters;
using LrControl.Profiles;
using LrControl.Tests.Devices;
using Moq;
using RtMidi.Core.Enums;
using RtMidi.Core.Messages;
using Serilog;
using Xunit;
using Xunit.Abstractions;
using Range = LrControl.LrPlugin.Api.Common.Range;

namespace LrControl.Tests.Profiles
{
    public class ProfileManagerTests : TestSuite
    {
        private static readonly Module DefaultModule = Module.Library;

        private static readonly ControllerInfo Info1 = new ControllerInfo(
            new ControllerId(MessageType.Nrpn, Channel.Channel1, 1),
            new Range(0, 128));

        private static readonly ControllerInfo Info2 = new ControllerInfo(
            new ControllerId(MessageType.Nrpn, Channel.Channel1, 2),
            new Range(0, 128));

        private readonly ManualResetEvent _receivedEvent = new ManualResetEvent(false);
        private readonly TestMidiInputDevice _inputDevice;
        private readonly TestMidiOutputDevice _outputDevice;
        private readonly IProfileManager _profileManager;

        private int _value;

        public ProfileManagerTests(ITestOutputHelper output) : base(output)
        {
            // Create test device manager
            var settings = new Mock<ISettings>();
            var deviceManager = new DeviceManager(settings.Object, new[] {Info1, Info2});

            _inputDevice = new TestMidiInputDevice("Test Input Device");
            deviceManager.SetInputDevice(new TestInputDeviceInfo(_inputDevice));

            _outputDevice = new TestMidiOutputDevice("Test Output Device");
            deviceManager.SetOutputDevice(new TestOutputDeviceInfo(_outputDevice));

            // Create test profile manager
            _profileManager = new ProfileManager(deviceManager);

            deviceManager.Input += (in ControllerId id, Range range, int value) =>
            {
                Log.Debug("ControllerInput: {@Id}, {@Range}, {Value}", id, range, value);
                _receivedEvent.Set();
            };
        }

        private void OnControllerInput(in ControllerId controllerId, int value)
        {
            _receivedEvent.Reset();

            var msg = new NrpnMessage(controllerId.Channel, controllerId.Parameter, value);
            Log.Debug("Sending message {@Message}", msg);
            _inputDevice.OnNrpn(msg);

            Log.Debug("Waiting until message has been received...");
            if (!_receivedEvent.WaitOne(TimeSpan.FromSeconds(1)))
                throw new Exception("Message not received as expected");
        }

        [Fact]
        public void Should_Default_To_Library_Module()
        {
            Assert.Equal(DefaultModule, _profileManager.ActiveModule);
        }

        [Fact]
        public void Should_Default_To_No_Panel()
        {
            Assert.Null(_profileManager.ActivePanel);
        }

        [Fact]
        public void Should_Apply_Module_Function_On_Input_With_Controller_Value()
        {
            // Setup
            var function = new TestFunction();
            _profileManager.AssignFunction(DefaultModule, Info1.ControllerId, function);

            // Test
            OnControllerInput(Info1.ControllerId, _value);

            // Verify
            Assert.Equal(1, function.ApplyCount);
            Assert.Equal(Info1.Range, function.LastRange);
            Assert.Equal(_value, function.LastValue);
        }

        [Fact]
        public void Should_Only_Apply_Function_For_Active_Module()
        {
            // Setup
            var webFunction = new TestFunction();
            var libraryFunction = new TestFunction();

            _profileManager.AssignFunction(Module.Web, Info1.ControllerId, webFunction);
            _profileManager.AssignFunction(DefaultModule, Info1.ControllerId, libraryFunction);

            // Should not invoke any function
            _profileManager.OnModuleChanged(Module.Map);
            OnControllerInput(Info1.ControllerId, _value++);

            Assert.Equal(0, webFunction.ApplyCount);
            Assert.Equal(0, libraryFunction.ApplyCount);

            // Should only invoke web function when Web module active
            _profileManager.OnModuleChanged(Module.Web);
            OnControllerInput(Info1.ControllerId, _value++);

            Assert.Equal(1, webFunction.ApplyCount);
            Assert.Equal(0, libraryFunction.ApplyCount);

            // Should only invoke library function when Library module active
            _profileManager.OnModuleChanged(Module.Library);
            OnControllerInput(Info1.ControllerId, _value++);

            Assert.Equal(1, webFunction.ApplyCount);
            Assert.Equal(1, libraryFunction.ApplyCount);
        }

        [Fact]
        public void Should_Clear_Function()
        {
            // Setup
            var function = new TestFunction();
            _profileManager.AssignFunction(DefaultModule, Info1.ControllerId, function);

            // Test
            OnControllerInput(Info1.ControllerId, _value++);
            Assert.Equal(1, function.ApplyCount);

            _profileManager.ClearFunction(DefaultModule, Info1.ControllerId);
            OnControllerInput(Info1.ControllerId, _value++);
            Assert.Equal(1, function.ApplyCount);
        }

        [Fact]
        public void Should_Clear_Module_Function()
        {
            // Setup
            var function = new TestFunction();

            // Test
            _profileManager.AssignFunction(DefaultModule, Info1.ControllerId, function);
            OnControllerInput(Info1.ControllerId, _value++);
            Assert.Equal(1, function.ApplyCount);

            _profileManager.ClearFunction(DefaultModule, Info1.ControllerId);
            OnControllerInput(Info1.ControllerId, _value++);
            Assert.Equal(1, function.ApplyCount);
        }

        [Fact]
        public void Should_Only_Apply_Panel_Function_When_Develop_Module_Is_Active()
        {
            // Setup
            var function = new TestFunction();
            _profileManager.AssignPanelFunction(Panel.Basic, Info1.ControllerId, function);

            // Test
            _profileManager.OnPanelChanged(Panel.Basic);
            Assert.Equal(Panel.Basic, _profileManager.ActivePanel);

            // Should not apply function because Develop module is not active
            Assert.NotEqual(Module.Develop, _profileManager.ActiveModule);
            OnControllerInput(Info1.ControllerId, _value++);
            Assert.False(function.Applied);

            // Should be applied once Develop module is active
            _profileManager.OnModuleChanged(Module.Develop);
            OnControllerInput(Info1.ControllerId, _value++);
            Assert.True(function.Applied);
        }

        [Fact]
        public void Should_Only_Apply_Function_For_Active_Panel()
        {
            // Setup
            var basicFunction = new TestFunction();
            var detailFunction = new TestFunction();

            _profileManager.AssignPanelFunction(Panel.Basic, Info1.ControllerId, basicFunction);
            _profileManager.AssignPanelFunction(Panel.Detail, Info1.ControllerId, detailFunction);
            _profileManager.OnModuleChanged(Module.Develop);

            // Should not invoke any function
            _profileManager.OnPanelChanged(Panel.Effects);
            OnControllerInput(Info1.ControllerId, _value++);

            Assert.Equal(0, basicFunction.ApplyCount);
            Assert.Equal(0, detailFunction.ApplyCount);

            // Should only invoke basic panel function
            _profileManager.OnPanelChanged(Panel.Basic);
            OnControllerInput(Info1.ControllerId, _value++);

            Assert.Equal(1, basicFunction.ApplyCount);
            Assert.Equal(0, detailFunction.ApplyCount);

            // Should only invoke detail panel function
            _profileManager.OnPanelChanged(Panel.Detail);
            OnControllerInput(Info1.ControllerId, _value++);

            Assert.Equal(1, basicFunction.ApplyCount);
            Assert.Equal(1, detailFunction.ApplyCount);
        }

        [Fact]
        public void Should_Clear_Panel_Function_When_Set_To_Null()
        {
            // Setup
            var function = new TestFunction();
            _profileManager.AssignPanelFunction(Panel.Basic, Info1.ControllerId, function);
            _profileManager.OnModuleChanged(Module.Develop);
            _profileManager.OnPanelChanged(Panel.Basic);

            // Test
            OnControllerInput(Info1.ControllerId, _value++);
            Assert.Equal(1, function.ApplyCount);

            _profileManager.ClearPanelFunction(Panel.Basic, Info1.ControllerId);
            OnControllerInput(Info1.ControllerId, _value++);
            Assert.Equal(1, function.ApplyCount);
        }

        [Fact]
        public void Should_Apply_Develop_Module_Function_Unless_Panel_Function_Also_Defined()
        {
            // Setup
            var moduleFunction = new TestFunction();
            var panelFunction = new TestFunction();

            _profileManager.AssignFunction(Module.Develop, Info1.ControllerId, moduleFunction);
            _profileManager.AssignPanelFunction(Panel.ToneCurve, Info1.ControllerId, panelFunction);

            _profileManager.OnModuleChanged(Module.Develop);
            _profileManager.OnPanelChanged(Panel.Basic);

            // Should apply module function as no panel function defined
            OnControllerInput(Info1.ControllerId, _value++);
            Assert.Equal(1, moduleFunction.ApplyCount);
            Assert.Equal(0, panelFunction.ApplyCount);

            // Should apply panel function, as panel function defined
            _profileManager.OnPanelChanged(Panel.ToneCurve);
            OnControllerInput(Info1.ControllerId, _value++);
            Assert.Equal(1, moduleFunction.ApplyCount);
            Assert.Equal(1, panelFunction.ApplyCount);
        }

        [Fact]
        public void Should_Set_Active_Panel_After_Applying_RevealOrTogglePanelFunction()
        {
            // Setup
            var settings = new Mock<ISettings>();
            var lrDevelopController = new Mock<ILrDevelopController>();
            var lrApi = new Mock<ILrApi>();
            lrApi
                .Setup(m => m.LrDevelopController)
                .Returns(lrDevelopController.Object);

            var function = new RevealOrTogglePanelFunction(
                settings.Object, lrApi.Object, "Test", "Test", Panel.ToneCurve);

            _profileManager.OnModuleChanged(Module.Develop);
            _profileManager.OnPanelChanged(Panel.Basic);
            _profileManager.AssignFunction(Module.Develop, Info1.ControllerId, function);

            Assert.Equal(Panel.Basic, _profileManager.ActivePanel);

            // Test
            OnControllerInput(Info1.ControllerId, (int) Info1.Range.Maximum);

            // Verify
            Assert.Equal(Panel.ToneCurve, _profileManager.ActivePanel);
        }

        [Fact]
        public void Should_Set_Controllers_Without_Functions_To_Minimum_Range_Value_When_Changing_Module()
        {
            // Setup
            var function = new TestFunction();
            _profileManager.AssignFunction(Module.Develop, Info1.ControllerId, function);
            _profileManager.ClearFunction(Module.Develop, Info2.ControllerId);

            // Test
            _profileManager.OnModuleChanged(Module.Develop);

            // Verify
            Assert.True(_outputDevice.Messages.TryTake(out var msg));
            Assert.True(msg is NrpnMessage);

            var nrpn = (NrpnMessage) msg;
            Assert.Equal(Info2.ControllerId.Channel, nrpn.Channel);
            Assert.Equal(Info2.ControllerId.Parameter, nrpn.Parameter);
            Assert.Equal(Info2.Range.Minimum, nrpn.Value);

            Assert.False(_outputDevice.Messages.Any());
        }

        [Fact]
        public void Should_Set_Controllers_Without_Functions_To_Minimum_Range_Value_When_Changing_Panel()
        {
            // Setup
            var function = new TestFunction();
            _profileManager.AssignPanelFunction(Panel.ToneCurve, Info1.ControllerId, function);
            _profileManager.ClearPanelFunction(Panel.ToneCurve, Info2.ControllerId);

            // Test
            _profileManager.OnModuleChanged(Module.Develop);
            while (_outputDevice.Messages.TryTake(out _))
            {
            }

            _profileManager.OnPanelChanged(Panel.ToneCurve);

            // Verify
            Assert.True(_outputDevice.Messages.TryTake(out var msg));
            Assert.True(msg is NrpnMessage);

            var nrpn = (NrpnMessage) msg;
            Assert.Equal(Info2.ControllerId.Channel, nrpn.Channel);
            Assert.Equal(Info2.ControllerId.Parameter, nrpn.Parameter);
            Assert.Equal(Info2.Range.Minimum, nrpn.Value);

            Assert.False(_outputDevice.Messages.Any());
        }

        [Fact]
        public void Should_Set_Controller_When_Parameter_Changes_And_ParameterChangeFunction_Is_Assigned_Module()
        {
            // Setup
            var settings = new Mock<ISettings>();
            var parameter = AdjustPanelParameter.Exposure;
            var parameterRange = new Range(0, 255);
            var parameterValue = 255.0d;
            
            var lrDevelopController = new Mock<ILrDevelopController>();
            
            lrDevelopController
                .Setup(m => m.GetRange(out parameterRange, parameter))
                .Returns(true);

            lrDevelopController
                .Setup(m => m.GetValue(out parameterValue, parameter))
                .Returns(true);
            
            var lrApi = new Mock<ILrApi>();
            lrApi
                .Setup(m => m.LrDevelopController)
                .Returns(lrDevelopController.Object);
            
            var function = new ParameterFunction(settings.Object, lrApi.Object, "Test", "Test", parameter);
            
            _profileManager.AssignFunction(Module.Develop, Info1.ControllerId, function);
            _profileManager.OnModuleChanged(Module.Develop);
            _profileManager.OnPanelChanged(Panel.Basic);

            while (_outputDevice.Messages.TryTake(out _))
            {
            }

            // Test
            _profileManager.OnParameterChanged(parameter);

            // Verify
            Assert.True(_outputDevice.Messages.TryTake(out var msg));
            Assert.True(msg is NrpnMessage);

            var nrpn = (NrpnMessage) msg;
            Assert.Equal((int)Info1.Range.Maximum, nrpn.Value);
        }

        [Fact]
        public void Should_Set_Controller_When_Parameter_Changes_And_ParameterChangeFunction_Is_Assigned_Panel()
        {
            // Setup
            var settings = new Mock<ISettings>();
            var parameter = AdjustPanelParameter.Exposure;
            var parameterRange = new Range(0, 255);
            var parameterValue = 255.0d;
            
            var lrDevelopController = new Mock<ILrDevelopController>();
            
            lrDevelopController
                .Setup(m => m.GetRange(out parameterRange, parameter))
                .Returns(true);

            lrDevelopController
                .Setup(m => m.GetValue(out parameterValue, parameter))
                .Returns(true);
            
            var lrApi = new Mock<ILrApi>();
            lrApi
                .Setup(m => m.LrDevelopController)
                .Returns(lrDevelopController.Object);
            
            var function = new ParameterFunction(settings.Object, lrApi.Object, "Test", "Test", parameter);

            _profileManager.AssignPanelFunction(Panel.Basic, Info1.ControllerId, function);
            _profileManager.OnModuleChanged(Module.Develop);
            _profileManager.OnPanelChanged(Panel.Basic);

            while (_outputDevice.Messages.TryTake(out _))
            {
            }

            // Test
            _profileManager.OnParameterChanged(parameter);

            // Verify
            Assert.True(_outputDevice.Messages.TryTake(out var msg));
            Assert.True(msg is NrpnMessage);

            var nrpn = (NrpnMessage) msg;
            Assert.Equal((int)Info1.Range.Maximum, nrpn.Value);
        }

        [Fact]
        public void Should_Set_Controller_When_Parameter_Changes_But_Only_For_Panel()
        {
            // Setup
            var settings = new Mock<ISettings>();
            var parameter = AdjustPanelParameter.Exposure;
            var parameterRange = new Range(0, 255);
            var parameterValue = 255.0d;
            var moduleParameter = AdjustPanelParameter.Tint;
            
            var lrDevelopController = new Mock<ILrDevelopController>();
            
            lrDevelopController
                .Setup(m => m.GetRange(out parameterRange, parameter))
                .Returns(true);

            lrDevelopController
                .Setup(m => m.GetValue(out parameterValue, parameter))
                .Returns(true);
            
            var lrApi = new Mock<ILrApi>();
            lrApi
                .Setup(m => m.LrDevelopController)
                .Returns(lrDevelopController.Object);

            var function = new ParameterFunction(settings.Object, lrApi.Object, "Test", "Test", parameter);
            var moduleFunction = new ParameterFunction(settings.Object, lrApi.Object, "Test", "Test", moduleParameter);

            _profileManager.AssignFunction(Module.Develop, Info1.ControllerId, moduleFunction);
            _profileManager.AssignPanelFunction(Panel.Basic, Info1.ControllerId, function);
            _profileManager.OnModuleChanged(Module.Develop);
            _profileManager.OnPanelChanged(Panel.Basic);

            while (_outputDevice.Messages.TryTake(out _))
            {
            }

            // Test
            _profileManager.OnParameterChanged(parameter);
            _profileManager.OnParameterChanged(moduleParameter);

            // Verify
            Assert.True(_outputDevice.Messages.TryTake(out var msg));
            Assert.True(msg is NrpnMessage);

            var nrpn = (NrpnMessage) msg;
            Assert.Equal((int)Info1.Range.Maximum, nrpn.Value);

            Assert.False(_outputDevice.Messages.Any());
        }
    }
}