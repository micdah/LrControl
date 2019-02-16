using LrControl.Configurations;
using LrControl.Functions;
using LrControl.Functions.Factories;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;
using Xunit;
using Xunit.Abstractions;

namespace LrControl.Tests.Functions.Factories
{
    public class RevealOrTogglePanelFunctionFactoryTests
        : FunctionFactoryTestSuite<RevealOrTogglePanelFunctionFactory, Panel>
    {
        public RevealOrTogglePanelFunctionFactoryTests(ITestOutputHelper output) : base(output)
        {
        }

        private static readonly Panel[] Panels =
        {
            Panel.Basic,
            Panel.Detail,
            Panel.Effects,
            Panel.ToneCurve,
            Panel.SplitToning,
            Panel.ColorAdjustment,
            Panel.LensCorrections,
            Panel.CameraCalibration
        };

        protected override RevealOrTogglePanelFunctionFactory CreateFactory(ISettings settings, ILrApi lrApi, Panel arg)
            => new RevealOrTogglePanelFunctionFactory(settings, lrApi, arg);

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        public void Should_Create_RevealOrTogglePanelFunction(int panelIndex)
        {
            var panel = Panels[panelIndex];
            var (_, function) = Create<RevealOrTogglePanelFunction>(panel);
            Assert.Equal(panel, function.Panel);
        }
    }
}