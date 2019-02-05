using LrControl.Devices;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
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
            
            // Test
            _profileManager.AssignFunction(DefaultModule, _id, function);
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

            // Test
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
    }
}