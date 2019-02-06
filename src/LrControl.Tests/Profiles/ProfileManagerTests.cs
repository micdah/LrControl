using System;
using System.Threading;
using LrControl.Configurations;
using LrControl.Devices;
using LrControl.Functions;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;
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
        private readonly Range _range = new Range(0, 255);
        private readonly ManualResetEvent _receivedEvent = new ManualResetEvent(false);
        private readonly ControllerId _id;
        private readonly TestMidiInputDevice _inputDevice;
        private readonly IProfileManager _profileManager;
        private int _value = 0;

        public ProfileManagerTests(ITestOutputHelper output) : base(output)
        {
            _id = new ControllerId(MessageType.Nrpn, Channel.Channel1, 12);

            // Create test device manager
            var settings = new Mock<ISettings>();
            var deviceManager = new DeviceManager(settings.Object, new[]
            {
                new ControllerInfo(_id, _range),
            });
            _inputDevice = new TestMidiInputDevice("Test Input Device");
            var inputDeviceInfo = new TestInputDeviceInfo(_inputDevice);
            deviceManager.SetInputDevice(inputDeviceInfo);
            
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
            _profileManager.AssignFunction(DefaultModule, _id, function);

            // Test
            OnControllerInput(_id, _value);

            // Verify
            Assert.Equal(1, function.ApplyCount);
            Assert.Equal(_range, function.LastRange);
            Assert.Equal(_value, function.LastValue);
        }

        [Fact]
        public void Should_Only_Apply_Function_For_Active_Module()
        {
            // Setup
            var webFunction = new TestFunction();
            var libraryFunction = new TestFunction();

            _profileManager.AssignFunction(Module.Web, _id, webFunction);
            _profileManager.AssignFunction(DefaultModule, _id, libraryFunction);

            // Should not invoke any function
            _profileManager.OnModuleChanged(Module.Map);
            OnControllerInput(_id, _value++);

            Assert.Equal(0, webFunction.ApplyCount);
            Assert.Equal(0, libraryFunction.ApplyCount);

            // Should only invoke web function when Web module active
            _profileManager.OnModuleChanged(Module.Web);
            OnControllerInput(_id, _value++);

            Assert.Equal(1, webFunction.ApplyCount);
            Assert.Equal(0, libraryFunction.ApplyCount);

            // Should only invoke library function when Library module active
            _profileManager.OnModuleChanged(Module.Library);
            OnControllerInput(_id, _value++);

            Assert.Equal(1, webFunction.ApplyCount);
            Assert.Equal(1, libraryFunction.ApplyCount);
        }

        [Fact]
        public void Should_Clear_Function()
        {
            // Setup
            var function = new TestFunction();
            _profileManager.AssignFunction(DefaultModule, _id, function);

            // Test
            OnControllerInput(_id, _value++);
            Assert.Equal(1, function.ApplyCount);

            _profileManager.ClearFunction(DefaultModule, _id);
            OnControllerInput(_id, _value++);
            Assert.Equal(1, function.ApplyCount);
        }

        [Fact]
        public void Should_Clear_Module_Function()
        {
            // Setup
            var function = new TestFunction();

            // Test
            _profileManager.AssignFunction(DefaultModule, _id, function);
            OnControllerInput(_id, _value++);
            Assert.Equal(1, function.ApplyCount);

            _profileManager.ClearFunction(DefaultModule, _id);
            OnControllerInput(_id, _value++);
            Assert.Equal(1, function.ApplyCount);
        }

        [Fact]
        public void Should_Only_Apply_Panel_Function_When_Develop_Module_Is_Active()
        {
            // Setup
            var function = new TestFunction();
            _profileManager.AssignPanelFunction(Panel.Basic, _id, function);

            // Test
            _profileManager.OnPanelChanged(Panel.Basic);
            Assert.Equal(Panel.Basic, _profileManager.ActivePanel);

            // Should not apply function because Develop module is not active
            Assert.NotEqual(Module.Develop, _profileManager.ActiveModule);
            OnControllerInput(_id, _value++);
            Assert.False(function.Applied);

            // Should be applied once Develop module is active
            _profileManager.OnModuleChanged(Module.Develop);
            OnControllerInput(_id, _value++);
            Assert.True(function.Applied);
        }

        [Fact]
        public void Should_Only_Apply_Function_For_Active_Panel()
        {
            // Setup
            var basicFunction = new TestFunction();
            var detailFunction = new TestFunction();

            _profileManager.AssignPanelFunction(Panel.Basic, _id, basicFunction);
            _profileManager.AssignPanelFunction(Panel.Detail, _id, detailFunction);
            _profileManager.OnModuleChanged(Module.Develop);

            // Should not invoke any function
            _profileManager.OnPanelChanged(Panel.Effects);
            OnControllerInput(_id, _value++);

            Assert.Equal(0, basicFunction.ApplyCount);
            Assert.Equal(0, detailFunction.ApplyCount);

            // Should only invoke basic panel function
            _profileManager.OnPanelChanged(Panel.Basic);
            OnControllerInput(_id, _value++);

            Assert.Equal(1, basicFunction.ApplyCount);
            Assert.Equal(0, detailFunction.ApplyCount);

            // Should only invoke detail panel function
            _profileManager.OnPanelChanged(Panel.Detail);
            OnControllerInput(_id, _value++);

            Assert.Equal(1, basicFunction.ApplyCount);
            Assert.Equal(1, detailFunction.ApplyCount);
        }

        [Fact]
        public void Should_Clear_Panel_Function_When_Set_To_Null()
        {
            // Setup
            var function = new TestFunction();
            _profileManager.AssignPanelFunction(Panel.Basic, _id, function);
            _profileManager.OnModuleChanged(Module.Develop);
            _profileManager.OnPanelChanged(Panel.Basic);

            // Test
            OnControllerInput(_id, _value++);
            Assert.Equal(1, function.ApplyCount);

            _profileManager.ClearPanelFunction(Panel.Basic, _id);
            OnControllerInput(_id, _value++);
            Assert.Equal(1, function.ApplyCount);
        }

        [Fact]
        public void Should_Apply_Develop_Module_Function_Unless_Panel_Function_Also_Defined()
        {
            // Setup
            var moduleFunction = new TestFunction();
            var panelFunction = new TestFunction();

            _profileManager.AssignFunction(Module.Develop, _id, moduleFunction);
            _profileManager.AssignPanelFunction(Panel.ToneCurve, _id, panelFunction);

            _profileManager.OnModuleChanged(Module.Develop);
            _profileManager.OnPanelChanged(Panel.Basic);

            // Should apply module function as no panel function defined
            OnControllerInput(_id, _value++);
            Assert.Equal(1, moduleFunction.ApplyCount);
            Assert.Equal(0, panelFunction.ApplyCount);

            // Should apply panel function, as panel function defined
            _profileManager.OnPanelChanged(Panel.ToneCurve);
            OnControllerInput(_id, _value++);
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
            _profileManager.AssignFunction(Module.Develop, _id, function);

            Assert.Equal(Panel.Basic, _profileManager.ActivePanel);

            // Test
            OnControllerInput(_id, (int) _range.Maximum);

            // Verify
            Assert.Equal(Panel.ToneCurve, _profileManager.ActivePanel);
        }
    }
}