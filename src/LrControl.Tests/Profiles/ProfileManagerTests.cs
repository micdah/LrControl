using LrControl.Functions;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;
using LrControl.LrPlugin.Api.Modules.LrDevelopController.Parameters;
using LrControl.Tests.Devices;
using LrControl.Tests.Mocks;
using Xunit;
using Xunit.Abstractions;
using Range = LrControl.LrPlugin.Api.Common.Range;

namespace LrControl.Tests.Profiles
{
    public class ProfileManagerTests : ProfileManagerTestSuite
    {    
        public ProfileManagerTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Should_Default_To_Library_Module()
        {
            Assert.Equal(DefaultModule, ProfileManager.ActiveModule);
        }

        [Fact]
        public void Should_Default_To_No_Panel()
        {
            Assert.Null(ProfileManager.ActivePanel);
        }

        [Fact]
        public void Should_Apply_Module_Function_On_Input_With_Controller_Value()
        {
            // Setup
            var function = new TestFunction();
            ProfileManager.AssignFunction(DefaultModule, Id1, function);

            // Test
            ControllerInput(Id1, Value);

            // Verify
            Assert.Equal(1, function.ApplyCount);
            Assert.Equal(Range1, function.LastRange);
            Assert.Equal(Value, function.LastValue);
        }

        [Fact]
        public void Should_Only_Apply_Function_For_Active_Module()
        {
            // Setup
            var webFunction = new TestFunction();
            var libraryFunction = new TestFunction();

            ProfileManager.AssignFunction(Module.Web, Id1, webFunction);
            ProfileManager.AssignFunction(DefaultModule, Id1, libraryFunction);

            // Should not invoke any function
            ProfileManager.OnModuleChanged(Module.Map);
            ControllerInput(Id1, Value++);

            Assert.Equal(0, webFunction.ApplyCount);
            Assert.Equal(0, libraryFunction.ApplyCount);

            // Should only invoke web function when Web module active
            ProfileManager.OnModuleChanged(Module.Web);
            ControllerInput(Id1, Value++);

            Assert.Equal(1, webFunction.ApplyCount);
            Assert.Equal(0, libraryFunction.ApplyCount);

            // Should only invoke library function when Library module active
            ProfileManager.OnModuleChanged(Module.Library);
            ControllerInput(Id1, Value++);

            Assert.Equal(1, webFunction.ApplyCount);
            Assert.Equal(1, libraryFunction.ApplyCount);
        }

        [Fact]
        public void Should_Clear_Function()
        {
            // Setup
            var function = new TestFunction();
            ProfileManager.AssignFunction(DefaultModule, Id1, function);

            // Test
            ControllerInput(Id1, Value++);
            Assert.Equal(1, function.ApplyCount);

            ProfileManager.ClearFunction(DefaultModule, Id1);
            ControllerInput(Id1, Value++);
            Assert.Equal(1, function.ApplyCount);
        }

        [Fact]
        public void Should_Clear_Module_Function()
        {
            // Setup
            var function = new TestFunction();

            // Test
            ProfileManager.AssignFunction(DefaultModule, Id1, function);
            ControllerInput(Id1, Value++);
            Assert.Equal(1, function.ApplyCount);

            ProfileManager.ClearFunction(DefaultModule, Id1);
            ControllerInput(Id1, Value++);
            Assert.Equal(1, function.ApplyCount);
        }

        [Fact]
        public void Should_Only_Apply_Panel_Function_When_Develop_Module_Is_Active()
        {
            // Setup
            var function = new TestFunction();
            ProfileManager.AssignPanelFunction(Panel.Basic, Id1, function);

            // Test
            ProfileManager.OnPanelChanged(Panel.Basic);
            Assert.Equal(Panel.Basic, ProfileManager.ActivePanel);

            // Should not apply function because Develop module is not active
            Assert.NotEqual(Module.Develop, ProfileManager.ActiveModule);
            ControllerInput(Id1, Value++);
            Assert.False(function.Applied);

            // Should be applied once Develop module is active
            ProfileManager.OnModuleChanged(Module.Develop);
            ControllerInput(Id1, Value++);
            Assert.True(function.Applied);
        }

        [Fact]
        public void Should_Only_Apply_Function_For_Active_Panel()
        {
            // Setup
            var basicFunction = new TestFunction();
            var detailFunction = new TestFunction();

            ProfileManager.AssignPanelFunction(Panel.Basic, Id1, basicFunction);
            ProfileManager.AssignPanelFunction(Panel.Detail, Id1, detailFunction);
            ProfileManager.OnModuleChanged(Module.Develop);

            // Should not invoke any function
            ProfileManager.OnPanelChanged(Panel.Effects);
            ControllerInput(Id1, Value++);

            Assert.Equal(0, basicFunction.ApplyCount);
            Assert.Equal(0, detailFunction.ApplyCount);

            // Should only invoke basic panel function
            ProfileManager.OnPanelChanged(Panel.Basic);
            ControllerInput(Id1, Value++);

            Assert.Equal(1, basicFunction.ApplyCount);
            Assert.Equal(0, detailFunction.ApplyCount);

            // Should only invoke detail panel function
            ProfileManager.OnPanelChanged(Panel.Detail);
            ControllerInput(Id1, Value++);

            Assert.Equal(1, basicFunction.ApplyCount);
            Assert.Equal(1, detailFunction.ApplyCount);
        }

        [Fact]
        public void Should_Clear_Panel_Function_When_Set_To_Null()
        {
            // Setup
            var function = new TestFunction();
            ProfileManager.AssignPanelFunction(Panel.Basic, Id1, function);
            ProfileManager.OnModuleChanged(Module.Develop);
            ProfileManager.OnPanelChanged(Panel.Basic);

            // Test
            ControllerInput(Id1, Value++);
            Assert.Equal(1, function.ApplyCount);

            ProfileManager.ClearPanelFunction(Panel.Basic, Id1);
            ControllerInput(Id1, Value++);
            Assert.Equal(1, function.ApplyCount);
        }

        [Fact]
        public void Should_Apply_Develop_Module_Function_Unless_Panel_Function_Also_Defined()
        {
            // Setup
            var moduleFunction = new TestFunction();
            var panelFunction = new TestFunction();

            ProfileManager.AssignFunction(Module.Develop, Id1, moduleFunction);
            ProfileManager.AssignPanelFunction(Panel.ToneCurve, Id1, panelFunction);

            ProfileManager.OnModuleChanged(Module.Develop);
            ProfileManager.OnPanelChanged(Panel.Basic);

            // Should apply module function as no panel function defined
            ControllerInput(Id1, Value++);
            Assert.Equal(1, moduleFunction.ApplyCount);
            Assert.Equal(0, panelFunction.ApplyCount);

            // Should apply panel function, as panel function defined
            ProfileManager.OnPanelChanged(Panel.ToneCurve);
            ControllerInput(Id1, Value++);
            Assert.Equal(1, moduleFunction.ApplyCount);
            Assert.Equal(1, panelFunction.ApplyCount);
        }

        [Fact]
        public void Should_Set_Active_Panel_After_Applying_RevealOrTogglePanelFunction()
        {
            // Setup
            var function = new RevealOrTogglePanelFunction(
                Settings.Object, LrApi.Object, "Test", "Test", Panel.ToneCurve);

            ProfileManager.OnModuleChanged(Module.Develop);
            ProfileManager.OnPanelChanged(Panel.Basic);
            ProfileManager.AssignFunction(Module.Develop, Id1, function);

            Assert.Equal(Panel.Basic, ProfileManager.ActivePanel);

            // Test
            ControllerInput(Id1, (int) Range1.Maximum);

            // Verify
            Assert.Equal(Panel.ToneCurve, ProfileManager.ActivePanel);
        }

        [Fact]
        public void Should_Set_Controllers_Without_Functions_To_Minimum_Range_Value_When_Changing_Module()
        {
            // Setup
            var function = new TestFunction();
            ProfileManager.AssignFunction(Module.Develop, Id1, function);
            ProfileManager.ClearFunction(Module.Develop, Id2);

            // Test
            ProfileManager.OnModuleChanged(Module.Develop);

            // Verify
            var nrpn = TakeOutput();
            Assert.Equal(Id2.Channel, nrpn.Channel);
            Assert.Equal(Id2.Parameter, nrpn.Parameter);
            Assert.Equal(Range2.Minimum, nrpn.Value);
            
            AssertNoMoreOutput();
        }

        [Fact]
        public void Should_Set_Controllers_Without_Functions_To_Minimum_Range_Value_When_Changing_Panel()
        {
            // Setup
            var function = new TestFunction();
            ProfileManager.AssignPanelFunction(Panel.ToneCurve, Id1, function);
            ProfileManager.ClearPanelFunction(Panel.ToneCurve, Id2);

            // Test
            ProfileManager.OnModuleChanged(Module.Develop);
            ClearOutput();

            ProfileManager.OnPanelChanged(Panel.ToneCurve);

            // Verify
            var nrpn = TakeOutput();
            Assert.Equal(Id2.Channel, nrpn.Channel);
            Assert.Equal(Id2.Parameter, nrpn.Parameter);
            Assert.Equal(Range2.Minimum, nrpn.Value);

            AssertNoMoreOutput();
        }

        [Fact]
        public void Should_Set_Controller_When_Parameter_Changes_And_ParameterChangeFunction_Is_Assigned_Module()
        {
            // Setup
            var parameter = AdjustPanelParameter.Exposure;
            var parameterRange = new Range(0, 255);
            var parameterValue = 255.0d;
            
            LrDevelopController
                .Setup(m => m.GetRange(out parameterRange, parameter))
                .Returns(true);

            LrDevelopController
                .Setup(m => m.GetValue(out parameterValue, parameter))
                .Returns(true);
            
            var function = new ParameterFunction(Settings.Object, LrApi.Object, "Test", "Test", parameter);
            
            ProfileManager.AssignFunction(Module.Develop, Id1, function);
            ProfileManager.OnModuleChanged(Module.Develop);
            ProfileManager.OnPanelChanged(Panel.Basic);
            ClearOutput();

            // Test
            ProfileManager.OnParameterChanged(parameter);

            // Verify
            var nrpn = TakeOutput();
            Assert.Equal((int)Range1.Maximum, nrpn.Value);
        }

        [Fact]
        public void Should_Set_Controller_When_Parameter_Changes_And_ParameterChangeFunction_Is_Assigned_Panel()
        {
            // Setup
            var parameter = AdjustPanelParameter.Exposure;
            var parameterRange = new Range(0, 255);
            var parameterValue = 255.0d;
            
            LrDevelopController
                .Setup(m => m.GetRange(out parameterRange, parameter))
                .Returns(true);

            LrDevelopController
                .Setup(m => m.GetValue(out parameterValue, parameter))
                .Returns(true);
            
            var function = new ParameterFunction(Settings.Object, LrApi.Object, "Test", "Test", parameter);

            ProfileManager.AssignPanelFunction(Panel.Basic, Id1, function);
            ProfileManager.OnModuleChanged(Module.Develop);
            ProfileManager.OnPanelChanged(Panel.Basic);
            ClearOutput();

            // Test
            ProfileManager.OnParameterChanged(parameter);

            // Verify
            var nrpn = TakeOutput();
            Assert.Equal((int)Range1.Maximum, nrpn.Value);
        }

        [Fact]
        public void Should_Set_Controller_When_Parameter_Changes_But_Only_For_Panel()
        {
            // Setup
            var moduleParameter = AdjustPanelParameter.Tint;
            var parameter = AdjustPanelParameter.Exposure;
            var parameterRange = new Range(0, 255);
            var parameterValue = 255.0d;
            
            LrDevelopController
                .Setup(m => m.GetRange(out parameterRange, parameter))
                .Returns(true);

            LrDevelopController
                .Setup(m => m.GetValue(out parameterValue, parameter))
                .Returns(true);

            var function = new ParameterFunction(Settings.Object, LrApi.Object, "Test", "Test", parameter);
            var moduleFunction = new ParameterFunction(Settings.Object, LrApi.Object, "Test", "Test", moduleParameter);

            ProfileManager.AssignFunction(Module.Develop, Id1, moduleFunction);
            ProfileManager.AssignPanelFunction(Panel.Basic, Id1, function);
            ProfileManager.OnModuleChanged(Module.Develop);
            ProfileManager.OnPanelChanged(Panel.Basic);
            ClearOutput();

            // Test
            ProfileManager.OnParameterChanged(parameter);
            ProfileManager.OnParameterChanged(moduleParameter);

            // Verify
            var nrpn = TakeOutput();
            Assert.Equal((int)Range1.Maximum, nrpn.Value);

            AssertNoMoreOutput();
        }

        [Fact]
        public void Should_Update_Controller_From_Assigned_ParameterFunction_For_Module()
        {
            // Setup
            var parameter = AdjustPanelParameter.Exposure;
            var parameterRange = new Range(0, 255);
            var parameterValue = 255.0d;

            LrDevelopController
                .Setup(m => m.GetRange(out parameterRange, parameter))
                .Returns(true);

            LrDevelopController
                .Setup(m => m.GetValue(out parameterValue, parameter))
                .Returns(true);

            var function = new ParameterFunction(Settings.Object, LrApi.Object, "Test", "Test", parameter);

            ProfileManager.AssignFunction(Module.Develop, Id1, function);
            ProfileManager.AssignFunction(Module.Develop, Id2, new TestFunction());

            // Test
            ProfileManager.OnModuleChanged(Module.Develop);

            // Verify
            var nrpn = TakeOutput();
            Assert.Equal((int) Range1.Maximum, nrpn.Value);

            AssertNoMoreOutput();
        }

        [Fact]
        public void Should_Update_Controller_From_Assigned_ParameterFunction_For_Panel()
        {
            // Setup
            var parameter = AdjustPanelParameter.Exposure;
            var parameterRange = new Range(0, 255);
            var parameterValue = 255.0d;

            LrDevelopController
                .Setup(m => m.GetRange(out parameterRange, parameter))
                .Returns(true);

            LrDevelopController
                .Setup(m => m.GetValue(out parameterValue, parameter))
                .Returns(true);

            var function = new ParameterFunction(Settings.Object, LrApi.Object, "Test", "Test", parameter);

            ProfileManager.AssignPanelFunction(Panel.ToneCurve, Id1, function);
            ProfileManager.AssignFunction(Module.Develop, Id2, new TestFunction());
            ProfileManager.OnModuleChanged(Module.Develop);
            ClearOutput();

            // Test
            ProfileManager.OnPanelChanged(Panel.ToneCurve);

            // Verify
            var nrpn = TakeOutput();
            Assert.Equal((int) Range1.Maximum, nrpn.Value);

            AssertNoMoreOutput();
        }
        
        [Fact]
        public void Should_Update_Controller_From_Assigned_ParameterFunction_For_Panel_Before_Module()
        {
            // Setup
            var moduleParameter = AdjustPanelParameter.Tint;
            var parameter = AdjustPanelParameter.Exposure;
            var parameterRange = new Range(0, 255);
            var parameterValue = 255.0d;

            LrDevelopController
                .Setup(m => m.GetRange(out parameterRange, parameter))
                .Returns(true);

            LrDevelopController
                .Setup(m => m.GetValue(out parameterValue, parameter))
                .Returns(true);

            var moduleFunction =
                new ParameterFunction(Settings.Object, LrApi.Object, "Test", "Test", moduleParameter);
            var function = new ParameterFunction(Settings.Object, LrApi.Object, "Test", "Test", parameter);

            ProfileManager.AssignPanelFunction(Panel.ToneCurve, Id1, function);
            ProfileManager.AssignFunction(Module.Develop, Id1, moduleFunction);
            ProfileManager.AssignFunction(Module.Develop, Id2, new TestFunction());
            ProfileManager.OnModuleChanged(Module.Develop);
            ClearOutput();

            // Test
            ProfileManager.OnPanelChanged(Panel.ToneCurve);

            // Verify
            var nrpn = TakeOutput();
            Assert.Equal((int) Range1.Maximum, nrpn.Value);

            AssertNoMoreOutput();
        }
    }
}