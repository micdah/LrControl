using System.Diagnostics.CodeAnalysis;
using LrControl.Functions.Factories;
using LrControl.LrPlugin.Api.Modules.LrDevelopController.Parameters;
using LrControl.Tests.Devices;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace LrControl.Tests.Functions
{
    [SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
    public class ResetParameterFunctionTests : ProfileManagerTestSuite
    {
        public ResetParameterFunctionTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Should_Stop_Tracking_And_Reset_Parameter_To_Default_When_Applied()
        {
            // Setup
            var parameter = AdjustPanelParameter.Exposure;
            var factory = GetFactory<ResetParameterFunctionFactory>(f => f.Parameter == parameter);
            LoadFunction(DefaultModule, Id1, factory);

            LrDevelopController
                .Setup(m => m.StopTracking())
                .Returns(true)
                .Verifiable();

            LrDevelopController
                .Setup(m => m.ResetToDefault(parameter))
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
            var parameter = AdjustPanelParameter.Exposure;
            var factory = GetFactory<ResetParameterFunctionFactory>(f => f.Parameter == parameter);
            LoadFunction(DefaultModule, Id1, factory);

            LrDevelopController
                .Setup(m => m.StopTracking())
                .Returns(true);

            LrDevelopController
                .Setup(m => m.ResetToDefault(AdjustPanelParameter.Exposure))
                .Returns(true);

            // Test
            ControllerInput(Id1, Range1.Maximum - 0.1d);

            // Verify
            LrDevelopController
                .Verify(m => m.StopTracking(), Times.Never());

            LrDevelopController
                .Verify(m => m.ResetToDefault(AdjustPanelParameter.Exposure), Times.Never());
        }
    }
}