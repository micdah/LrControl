using System.Diagnostics.CodeAnalysis;
using LrControl.Functions.Factories;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;
using LrControl.LrPlugin.Api.Modules.LrDevelopController.Parameters;
using LrControl.Tests.Devices;
using Xunit;
using Xunit.Abstractions;

namespace LrControl.Tests.Functions
{
    [SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
    public class ParameterFunctionTests : ProfileManagerTestSuite
    {
        public ParameterFunctionTests(ITestOutputHelper output) : base(output)
        {
        }

        private void Setup<T>(IParameter<T> parameter, Range parameterRange, bool setupRange = true)
        {
            var factory = GetFactory<ParameterFunctionFactory>(f => f.Parameter == parameter);
            LoadFunction(DefaultModule, Id1, factory);

            if (setupRange)
            {
                var expectedParameterRange = parameterRange;
                LrDevelopController
                    .Setup(m => m.GetRange(out expectedParameterRange, parameter))
                    .Returns(true)
                    .Verifiable($"Should get range for parameter {parameter}");
            }
        }

        [Fact]
        public void Should_Update_Parameter_Range_Before_Applying()
        {
            // Setup
            Setup(AdjustPanelParameter.Blacks, new Range(0, 256));

            // Test
            ControllerInput(Id1, Value++);

            // Verify
            LrDevelopController.Verify();
        }

        [Fact]
        public void Should_Set_Integer_Parameter_Based_On_Parameter_Range()
        {
            // Setup
            Setup(AdjustPanelParameter.Whites, new Range(0, 256));
            var value = (int)(Range1.Maximum - Range1.Minimum) / 2;
            
            LrDevelopController
                .Setup(m => m.SetValue(AdjustPanelParameter.Whites, 128))
                .Returns(true)
                .Verifiable("Should set parameter value to 128");

            // Test
            ControllerInput(Id1, value);

            // Verify
            LrDevelopController.Verify();
        }

        [Fact]
        public void Should_Set_Double_Parameter_Based_On_Parameter_Range()
        {
            // Setup
            Setup(AdjustPanelParameter.Tint, new Range(0, 256));
            var value = (int)(Range1.Maximum - Range1.Minimum) / 2;

            LrDevelopController
                .Setup(m => m.SetValue(AdjustPanelParameter.Tint, 128.0d))
                .Returns(true)
                .Verifiable("Should set parameter to value 128.0");

            // Test
            ControllerInput(Id1, value);

            // Verify
            LrDevelopController.Verify();
        }

        [Fact]
        public void Should_Fail_To_Get_Controller_Value_If_Cannot_Get_Parameter_Range()
        {
            // Setup
            Setup(AdjustPanelParameter.Blacks, new Range(0, 256), false);

            var range = new Range(0, 100);
            LrDevelopController
                .Setup(m => m.GetRange(out range, AdjustPanelParameter.Blacks))
                .Returns(false)
                .Verifiable();

            // Test
            ProfileManager.OnParameterChanged(AdjustPanelParameter.Blacks);

            // Verify
            LrDevelopController.Verify();
            AssertNoMoreOutput();
        }

        [Fact]
        public void Should_Get_Controller_Value()
        {
            // Setup
            Setup(AdjustPanelParameter.Blacks, new Range(0, 256));
            var controllerValue = (int) (Range1.Maximum - Range1.Minimum) / 2;

            var parameterValue = 128;
            LrDevelopController
                .Setup(m => m.GetValue(out parameterValue, AdjustPanelParameter.Blacks))
                .Returns(true)
                .Verifiable();

            // Test
            ProfileManager.OnParameterChanged(AdjustPanelParameter.Blacks);

            // Verify
            LrDevelopController.Verify();
            
            var msg = TakeOutput();
            Assert.Equal(controllerValue, msg.Value);
        }
    }
}