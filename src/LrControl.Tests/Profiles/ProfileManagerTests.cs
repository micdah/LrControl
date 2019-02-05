using LrControl.Devices;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;
using LrControl.Profiles;
using RtMidi.Core.Enums;
using RtMidi.Core.Messages;
using Xunit;

namespace LrControl.Tests.Profiles
{
    public class ProfileManagerTests
    {
        private static readonly Module DefaultModule = Module.Library;
        private readonly Range _range;
        private readonly NrpnMessage _msg;
        private readonly ControllerId _id;
        private readonly IProfileManager _profileManager;
        
        public ProfileManagerTests()
        {
            _range = new Range(0, 255);
            _msg = new NrpnMessage(Channel.Channel1, 12, 24);
            _id = new ControllerId(_msg);
            _profileManager = new ProfileManager();
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
            _profileManager.OnControllerInput(_id, _range, _msg.Value);

            // Verify
            Assert.Equal(1, function.ApplyCount);
            Assert.Equal(_range, function.LastRange);
            Assert.Equal(_msg.Value, function.LastValue);
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
            _profileManager.OnControllerInput(_id, _range, _msg.Value);

            Assert.Equal(0, webFunction.ApplyCount);
            Assert.Equal(0, libraryFunction.ApplyCount);

            // Should only invoke web function when Web module active
            _profileManager.OnModuleChanged(Module.Web);
            _profileManager.OnControllerInput(_id, _range, _msg.Value);

            Assert.Equal(1, webFunction.ApplyCount);
            Assert.Equal(0, libraryFunction.ApplyCount);
            
            // Should only invoke library function when Library module active
            _profileManager.OnModuleChanged(Module.Library);
            _profileManager.OnControllerInput(_id, _range, _msg.Value);

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
            _profileManager.OnControllerInput(_id, _range, _msg.Value);
            Assert.Equal(1, function.ApplyCount);

            _profileManager.ClearFunction(DefaultModule, _id);
            _profileManager.OnControllerInput(_id, _range, _msg.Value);
            Assert.Equal(1, function.ApplyCount);

        }

        [Fact]
        public void Should_Clear_Module_Function()
        {
            // Setup
            var function = new TestFunction();

            // Test
            _profileManager.AssignFunction(DefaultModule, _id, function);
            _profileManager.OnControllerInput(_id, _range, _msg.Value);
            Assert.Equal(1, function.ApplyCount);

            _profileManager.ClearFunction(DefaultModule, _id);
            _profileManager.OnControllerInput(_id, _range, _msg.Value);
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
            _profileManager.OnControllerInput(_id, _range, _msg.Value);
            Assert.False(function.Applied);
            
            // Should be applied once Develop module is active
            _profileManager.OnModuleChanged(Module.Develop);
            _profileManager.OnControllerInput(_id, _range, _msg.Value);
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
            _profileManager.OnControllerInput(_id, _range, _msg.Value);

            Assert.Equal(0, basicFunction.ApplyCount);
            Assert.Equal(0, detailFunction.ApplyCount);
            
            // Should only invoke basic panel function
            _profileManager.OnPanelChanged(Panel.Basic);
            _profileManager.OnControllerInput(_id, _range, _msg.Value);

            Assert.Equal(1, basicFunction.ApplyCount);
            Assert.Equal(0, detailFunction.ApplyCount);
            
            // Should only invoke detail panel function
            _profileManager.OnPanelChanged(Panel.Detail);
            _profileManager.OnControllerInput(_id, _range, _msg.Value);

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
            _profileManager.OnControllerInput(_id, _range, _msg.Value);
            Assert.Equal(1, function.ApplyCount);

            _profileManager.ClearPanelFunction(Panel.Basic, _id);
            _profileManager.OnControllerInput(_id, _range, _msg.Value);
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
            _profileManager.OnControllerInput(_id, _range, _msg.Value);
            Assert.Equal(1, moduleFunction.ApplyCount);
            Assert.Equal(0, panelFunction.ApplyCount);
            
            // Should apply panel function, as panel function defined
            _profileManager.OnPanelChanged(Panel.ToneCurve);
            _profileManager.OnControllerInput(_id, _range, _msg.Value);
            Assert.Equal(1, moduleFunction.ApplyCount);
            Assert.Equal(1, panelFunction.ApplyCount);
        }
    }
}