using LrControl.Functions;
using LrControl.Tests.Devices;
using LrControl.Tests.Mocks;
using Moq;
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

    public class ResetParameterFunctionTests : ProfileManagerTestSuite
    {
        public ResetParameterFunctionTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Should_Stop_Tracking_And_Reset_Parameter_To_Default_When_Applied()
        {
            // Setup
            var function = new ResetParameterFunction(
                Settings.Object,
                LrApi.Object,
                "Test Function",
                "TestFunction",
                TestParameter.IntegerParameter);

            ProfileManager.AssignFunction(DefaultModule, Id1, function);

            LrDevelopController
                .Setup(m => m.StopTracking())
                .Returns(true)
                .Verifiable();

            LrDevelopController
                .Setup(m => m.ResetToDefault(TestParameter.IntegerParameter))
                .Returns(true)
                .Verifiable();

            // Test
            ControllerInput(Id1, Range1.Maximum);

            // Verify
            LrDevelopController.Verify();
        }

        [Fact]
        public void Should_Only_Apply_When_Controller_Is_At_Maximum_Value()
        {
            // Setup
            var function = new ResetParameterFunction(
                Settings.Object,
                LrApi.Object,
                "Test Function",
                "TestFunction",
                TestParameter.IntegerParameter);

            ProfileManager.AssignFunction(DefaultModule, Id1, function);

            LrDevelopController
                .Setup(m => m.StopTracking())
                .Returns(true);

            LrDevelopController
                .Setup(m => m.ResetToDefault(TestParameter.IntegerParameter))
                .Returns(true);

            // Test
            ControllerInput(Id1, Range1.Maximum - 0.1d);

            // Verify
            LrDevelopController
                .Verify(m => m.StopTracking(), Times.Never());

            LrDevelopController
                .Verify(m => m.ResetToDefault(TestParameter.IntegerParameter), Times.Never());
        }
    }
}