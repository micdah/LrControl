using LrControl.Functions;
using LrControl.Tests.Devices;
using Xunit;
using Xunit.Abstractions;

namespace LrControl.Tests.Functions
{
    public class ToggleZoomFunctionTests : ProfileManagerTestSuite
    {
        public ToggleZoomFunctionTests(ITestOutputHelper output) : base(output)
        {
        }

        private ToggleZoomFunction Create() =>
            new ToggleZoomFunction(Settings.Object, LrApi.Object, "Test Function", "TestFunction");

        [Fact]
        public void Should_ToggleZoom_When_Applied()
        {
            // Setup
            var function = Create();
            ProfileManager.AssignFunction(DefaultModule, Id1, function);

            // Test
            ControllerInput(Id1, Range1.Maximum);

            // Verify
            LrApplicationView
                .Verify(_ => _.ToggleZoom());
        }
    }
}