using LrControl.Functions.Factories;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.Tests.Devices;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace LrControl.Tests.Functions
{
    public class ToggleDevelopBeforeAfterFunctionTests : ProfileManagerTestSuite
    {
        public ToggleDevelopBeforeAfterFunctionTests(ITestOutputHelper output) : base(output)
        {
        }

        private void Verify(int timesBefore, int timesAfter)
        {
            LrApplicationView.Verify(m => m.ShowView(PrimaryView.DevelopBefore), Times.Exactly(timesBefore));

            LrApplicationView.Verify(m => m.ShowView(PrimaryView.DevelopLoupe), Times.Exactly(timesAfter));
        }

        [Fact]
        public void Should_Toggle_Before_After_View_When_Applied()
        {
            // Setup
            var factory = GetFactory<ToggleDevelopBeforeAfterFunctionFactory>(f => true);
            LoadFunction(DefaultModule, Id1, factory);

            Verify(0, 0);

            // Should toggle to before initially
            ControllerInput(Id1, Range1.Maximum, Range1.Minimum);
            Verify(1, 0);

            // Should toggle to after
            ControllerInput(Id1, Range1.Maximum, Range1.Minimum);
            Verify(1, 1);

            // Should toggle back to before
            ControllerInput(Id1, Range1.Maximum, Range1.Minimum);
            Verify(2, 1);
        }

        [Fact]
        public void Should_Only_Apply_When_Controller_Is_At_Maximum_Value()
        {
            // Setup
            var factory = GetFactory<ToggleDevelopBeforeAfterFunctionFactory>(f => true);
            LoadFunction(DefaultModule, Id1, factory);

            Verify(0, 0);

            // Test
            ControllerInput(Id1, Range1.Maximum - 0.1d);

            // Verify
            Verify(0, 0);
        }
    }
}