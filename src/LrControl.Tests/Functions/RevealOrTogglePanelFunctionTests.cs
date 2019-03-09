using LrControl.Functions.Factories;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;
using LrControl.LrPlugin.Api.Modules.LrDevelopController.Parameters;
using LrControl.Tests.Devices;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace LrControl.Tests.Functions
{
    public class RevealOrTogglePanelFunctionTests : ProfileManagerTestSuite
    {
        public RevealOrTogglePanelFunctionTests(ITestOutputHelper output) : base(output)
        {
            ProfileManager.OnModuleChanged(Module.Develop);
            ClearOutput();
            
            LrDevelopController
                .Setup(m => m.RevealPanel(It.IsAny<Panel>()))
                .Returns(true);
        }

        private void Setup(Panel panel)
        {
            var factory = GetFactory<RevealOrTogglePanelFunctionFactory>(f => f.Panel == panel);
            LoadFunction(Module.Develop, Id1, factory);
        }

        [Fact]
        public void Should_Reveal_Panel_When_Panel_Is_Not_Currently_Active()
        {
            // Setup
            Setup(Panel.Detail);
            ProfileManager.OnPanelChanged(Panel.ToneCurve);
                

            // Test
            ControllerInput(Id1, Range1.Maximum);

            // Verify
            LrDevelopController
                .Verify(m => m.RevealPanel(Panel.Detail), Times.Once);
        }

        [Fact]
        public void Should_Only_Apply_When_Value_Is_Maximum_Of_Range()
        {
            // Setup
            Setup(Panel.Detail);
            ProfileManager.OnPanelChanged(Panel.ToneCurve);

            // Test
            ControllerInput(Id1, Range1.Maximum - 0.1d);

            // Verify
            LrDevelopController.VerifyNoOtherCalls();
        }

        [Fact]
        public void Should_Always_Reveal_Basic_Panel_Regardless_Of_Active_Panel()
        {
            // Setup
            Setup(Panel.Basic);

            // Test
            ProfileManager.OnPanelChanged(Panel.ToneCurve);
            ControllerInput(Id1, Range1.Maximum, Range1.Minimum);

            ProfileManager.OnPanelChanged(Panel.Basic);
            ControllerInput(Id1, Range1.Maximum, Range1.Minimum);

            // Verify
            LrDevelopController
                .Verify(m => m.RevealPanel(Panel.Basic), Times.Exactly(2));
        }

        [Fact]
        public void Should_Disable_Panel_When_Panel_Is_Active_And_Currently_Enabled()
        {
            // Setup
            Setup(Panel.Detail);
            
            var enabled = true;
            LrDevelopController
                .Setup(m => m.GetValue(out enabled, EnablePanelParameter.Detail))
                .Returns(true);
            
            ProfileManager.OnPanelChanged(Panel.Detail);

            // Test
            ControllerInput(Id1, Range1.Maximum);

            // Verify
            LrDevelopController
                .Verify(m => m.SetValue(EnablePanelParameter.Detail, false), Times.Once);
        }

        [Fact]
        public void Should_Enable_Panel_When_Panel_Is_Active_And_Currently_Disabled()
        {
            // Setup
            Setup(Panel.Detail);

            var enabled = false;
            LrDevelopController
                .Setup(m => m.GetValue(out enabled, EnablePanelParameter.Detail))
                .Returns(true);

            ProfileManager.OnPanelChanged(Panel.Detail);

            // Test
            ControllerInput(Id1, Range1.Maximum);

            // Verify
            LrDevelopController
                .Verify(m => m.SetValue(EnablePanelParameter.Detail, true), Times.Once);
        }
    }
}