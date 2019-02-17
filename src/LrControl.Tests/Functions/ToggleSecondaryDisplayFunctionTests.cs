using LrControl.Functions;
using LrControl.Tests.Devices;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace LrControl.Tests.Functions
{
    public class ToggleSecondaryDisplayFunctionTests : ProfileManagerTestSuite
    {
        public ToggleSecondaryDisplayFunctionTests(ITestOutputHelper output) : base(output)
        {
        }

        private ToggleSecondaryDisplayFunction Create() =>
            new ToggleSecondaryDisplayFunction(Settings.Object, LrApi.Object, "Test Function", "TestFunction");

        [Fact]
        public void Should_ToggleSecondaryDisplay_When_Applied()
        {
            // Setup
            var function = Create();
            ProfileManager.AssignFunction(DefaultModule, Id1, function);

            // Test
            ControllerInput(Id1, Range1.Maximum);

            // Verify
            LrApplicationView
                .Verify(_ => _.ToggleSecondaryDisplay(), Times.Once());
        }
    }
}