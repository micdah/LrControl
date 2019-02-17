using LrControl.Functions;
using LrControl.Tests.Devices;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace LrControl.Tests.Functions
{
    public class ToggleSecondaryDisplayFullscreenFunctionTests : ProfileManagerTestSuite
    {
        public ToggleSecondaryDisplayFullscreenFunctionTests(ITestOutputHelper output) : base(output)
        {
        }

        private ToggleSecondaryDisplayFullscreenFunction Create() =>
            new ToggleSecondaryDisplayFullscreenFunction(Settings.Object, LrApi.Object, "Test Function",
                "TestFunction");

        [Fact]
        public void Should_ToggleSecondaryDisplayFullscreen_When_Applied()
        {
            // Setup
            var function = Create();
            ProfileManager.AssignFunction(DefaultModule, Id1, function);

            // Test
            ControllerInput(Id1, Range1.Maximum);

            // Verify
            LrApplicationView
                .Verify(_ => _.ToggleSecondaryDisplayFullscreen(), Times.Once);
        }
    }
}