using LrControl.Functions.Factories;
using LrControl.Tests.Devices;
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
            var factory = GetFactory<MethodFunctionFactory>(f => f.Key == "MethodFunction:ResetAllDevelopAdjustments");
            LoadFunction(DefaultModule, Id1, factory);

            // Test
            ControllerInput(Id1, Range1.Maximum);

            // Verify
            LrDevelopController.Verify(m => m.ResetAllDevelopAdjustments(), Times.Once());
        }

        [Fact]
        public void Should_Only_Apply_When_Controller_Is_At_Maximum_Value()
        {
            // Setup
            var factory = GetFactory<MethodFunctionFactory>(f => f.Key == "MethodFunction:ResetAllDevelopAdjustments");
            LoadFunction(DefaultModule, Id1, factory);

            // Test
            ControllerInput(Id1, Range1.Maximum - 0.1d);

            // Verify
            LrDevelopController.Verify(m => m.ResetAllDevelopAdjustments(), Times.Never);
        }
    }
}