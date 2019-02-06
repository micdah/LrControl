using LrControl.Configurations;
using LrControl.Functions;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;
using LrControl.LrPlugin.Api.Modules.LrDevelopController.Parameters;
using Moq;
using Xunit;
using Range = LrControl.LrPlugin.Api.Common.Range;

namespace LrControl.Tests.Functions
{
    public class RevealOrTogglePanelFunctionTests
    {
        private readonly Range _range = new Range(0, 255);
        private readonly Module _module = Module.Develop;
        private readonly Mock<ISettings> _settings = new Mock<ISettings>();
        private readonly Mock<ILrApi> _lrApi = new Mock<ILrApi>();
        private readonly Mock<ILrDevelopController> _lrDevelopController = new Mock<ILrDevelopController>();

        public RevealOrTogglePanelFunctionTests()
        {
            _lrApi
                .Setup(m => m.LrDevelopController)
                .Returns(_lrDevelopController.Object);
        }

        private IFunction Create(Panel panel)
            => new RevealOrTogglePanelFunction(
                _settings.Object,
                _lrApi.Object,
                "Test Function",
                "TestFunction",
                panel);

        [Fact]
        public void Should_Reveal_Panel_When_Panel_Is_Not_Currently_Active()
        {
            // Setup
            var func = Create(Panel.Detail);

            // Test
            func.Apply((int)_range.Maximum, _range, _module, Panel.ToneCurve);

            // Verify
            _lrDevelopController
                .Verify(m => m.RevealPanel(Panel.Detail), Times.Once);
        }

        [Fact]
        public void Should_Only_Apply_When_Value_Is_Maximum_Of_Range()
        {
            // Setup
            var func = Create(Panel.Detail);

            // Test
            func.Apply((int) _range.Maximum - 1, _range, _module, Panel.ToneCurve);

            // Verify
            _lrDevelopController.VerifyNoOtherCalls();
        }

        [Fact]
        public void Should_Always_Reveal_Basic_Panel_Regardless_Of_Active_Panel()
        {
            // Setup
            var func = Create(Panel.Basic);

            // Test
            func.Apply((int) _range.Maximum, _range, _module, Panel.ToneCurve);
            func.Apply((int) _range.Maximum, _range, _module, Panel.Basic);

            // Verify
            _lrDevelopController
                .Verify(m => m.RevealPanel(Panel.Basic), Times.Exactly(2));
        }

        [Fact]
        public void Should_Disable_Panel_When_Panel_Is_Active_And_Currently_Enabled()
        {
            // Setup
            var func = Create(Panel.Detail);

            bool enabled = true;
            _lrDevelopController
                .Setup(m => m.GetValue(out enabled, EnablePanelParameter.Detail))
                .Returns(true);

            // Test
            func.Apply((int)_range.Maximum, _range, _module, Panel.Detail);

            // Verify
            _lrDevelopController
                .Verify(m => m.SetValue(EnablePanelParameter.Detail, false), Times.Once);
        }

        [Fact]
        public void Should_Enable_Panel_When_Panel_Is_Active_And_Currently_Disabled()
        {
            // Setup
            var func = Create(Panel.Detail);

            bool enabled = false;
            _lrDevelopController
                .Setup(m => m.GetValue(out enabled, EnablePanelParameter.Detail))
                .Returns(true);

            // Test
            func.Apply((int)_range.Maximum, _range, _module, Panel.Detail);

            // Verify
            _lrDevelopController
                .Verify(m => m.SetValue(EnablePanelParameter.Detail, true), Times.Once);
        }
    }
}