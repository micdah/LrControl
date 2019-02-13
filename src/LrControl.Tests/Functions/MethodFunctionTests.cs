using LrControl.Functions;
using LrControl.Tests.Devices;
using Xunit;
using Xunit.Abstractions;

namespace LrControl.Tests.Functions
{
    public class MethodFunctionTests : ProfileManagerTestSuite
    {
        public MethodFunctionTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Should_Invoke_Method_When_Applied()
        {
            // Setup
            var invoked = false;
            var function = new MethodFunction(
                Settings.Object,
                LrApi.Object,
                "Method Function",
                "MethodFunction",
                _ => { invoked = true; },
                "Display text");

            ProfileManager.AssignFunction(DefaultModule, Id1, function);

            // Test
            ControllerInput(Id1, Range1.Maximum);

            // Verify
            Assert.True(invoked);
        }

        [Fact]
        public void Should_Only_Apply_When_Controller_Is_At_Maximum_Value()
        {
            // Setup
            var invoked = false;
            var function = new MethodFunction(
                Settings.Object,
                LrApi.Object,
                "Method Function",
                "MethodFunction",
                _ => { invoked = true; },
                "Display text");

            ProfileManager.AssignFunction(DefaultModule, Id1, function);

            // Test
            ControllerInput(Id1, Range1.Maximum - 0.1d);

            // Verify
            Assert.False(invoked);
        }
    }
}