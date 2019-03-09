using System.Diagnostics.CodeAnalysis;
using LrControl.Functions.Factories;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;
using LrControl.LrPlugin.Api.Modules.LrDevelopController.Parameters;
using LrControl.Tests.Devices;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace LrControl.Tests.Functions
{
    [SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
    public class ToggleParameterFunctionTests : ProfileManagerTestSuite
    {
        private readonly IParameter<bool> _parameter;

        public ToggleParameterFunctionTests(ITestOutputHelper output) : base(output)
        {
            _parameter = MixerPanelParameter.ConvertToGrayscale;
            var factory = GetFactory<ParameterFunctionFactory>(f => f.Parameter == _parameter);
            LoadFunction(DefaultModule, Id1, factory);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Should_Toggle_Boolean_Parameter(bool currentEnabled)
        {
            // Setup
            LrDevelopController
                .Setup(_ => _.GetValue(out currentEnabled, _parameter))
                .Returns(true)
                .Verifiable();
            
            LrDevelopController
                .Setup(_ => _.SetValue(_parameter, !currentEnabled))
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
            // Test
            ControllerInput(Id1, Range1.Maximum - 0.01d);

            // Verify
            LrApi.Verify(_ => _.LrDevelopController, Times.Never());
        }
    }
}