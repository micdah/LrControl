using LrControl.Functions.Factories;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;
using LrControl.LrPlugin.Api.Modules.LrDevelopController.Parameters;
using LrControl.Tests.Devices;
using Moq;
using Xunit;
using Xunit.Abstractions;

#pragma warning disable 252,253

namespace LrControl.Tests.Functions
{
    public class EnumerationParameterFunctionTests : ProfileManagerTestSuite
    {
        public EnumerationParameterFunctionTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Should_Set_Enumeration_Parameter_When_Applied()
        {
            // Setup
            var parameter = AdjustPanelParameter.WhiteBalance;
            var value = WhiteBalanceValue.Cloudy;
            var factory = GetFactory<EnumerationParameterFunctionFactory<string>>(f =>
                f.Parameter == parameter && f.Value == value);
            
            LoadFunction(DefaultModule, Id1, factory);

            LrDevelopController
                .Setup(m => m.SetValue(parameter, value))
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
            var parameter = AdjustPanelParameter.WhiteBalance;
            var value = WhiteBalanceValue.Cloudy;
            var factory = GetFactory<EnumerationParameterFunctionFactory<string>>(f =>
                f.Parameter == parameter && f.Value == value);

            LoadFunction(DefaultModule, Id1, factory);

            // Test
            ControllerInput(Id1, Range1.Maximum - 0.1d);

            // Verify
            LrDevelopController
                .Verify(m => m.SetValue(parameter, It.IsAny<IEnumeration<string>>()), Times.Never);
        }
    }
}