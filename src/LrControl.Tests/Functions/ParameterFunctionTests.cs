using System;
using LrControl.Functions;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;
using LrControl.Tests.Devices;
using LrControl.Tests.Mocks;
using Xunit;
using Xunit.Abstractions;
using Range = LrControl.LrPlugin.Api.Common.Range;

namespace LrControl.Tests.Functions
{
    public class ParameterFunctionTests : ProfileManagerTestSuite
    {
        private ParameterFunction _function;

        public ParameterFunctionTests(ITestOutputHelper output) : base(output)
        {
        }

        private ParameterFunction Create(IParameter parameter)
            => new ParameterFunction(
                Settings.Object,
                LrApi.Object,
                "Test Function",
                "TestFunction",
                parameter);

        private void Setup<T>(IParameter<T> parameter, Range parameterRange, bool setupRange = true)
        {
            _function = Create(parameter);
            ProfileManager.AssignFunction(DefaultModule, Id1, _function);

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
        public void Should_Only_Work_With_Double_And_Int_Parameters()
        {
            Assert.NotNull(Create(TestParameter.IntegerParameter));
            Assert.NotNull(Create(TestParameter.DoubleParameter));
            Assert.Throws<ArgumentException>(() =>
            {
                Create(new Parameter<string>("StringParameter", "String parameter"));
            });
        }

        [Fact]
        public void Should_Update_Parameter_Range_Before_Applying()
        {
            // Setup
            Setup(TestParameter.IntegerParameter, new Range(0, 256));

            // Test
            ControllerInput(Id1, Value++);

            // Verify
            LrDevelopController.Verify();
        }

        [Fact]
        public void Should_Set_Integer_Parameter_Based_On_Parameter_Range()
        {
            // Setup
            Setup(TestParameter.IntegerParameter, new Range(0, 256));
            var value = (int)(Range1.Maximum - Range1.Minimum) / 2;
            
            LrDevelopController
                .Setup(m => m.SetValue(TestParameter.IntegerParameter, 128))
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
            Setup(TestParameter.DoubleParameter, new Range(0, 256));
            var value = (int)(Range1.Maximum - Range1.Minimum) / 2;

            LrDevelopController
                .Setup(m => m.SetValue(TestParameter.DoubleParameter, 128.0d))
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
            Setup(TestParameter.IntegerParameter, new Range(0, 256), false);

            var range = new Range(0, 100);
            LrDevelopController
                .Setup(m => m.GetRange(out range, TestParameter.IntegerParameter))
                .Returns(false)
                .Verifiable();

            // Test
            ProfileManager.OnParameterChanged(TestParameter.IntegerParameter);

            // Verify
            LrDevelopController.Verify();
            AssertNoMoreOutput();
        }

        [Fact]
        public void Should_Get_Controller_Value()
        {
            // Setup
            Setup(TestParameter.IntegerParameter, new Range(0, 256));
            var controllerValue = (int) (Range1.Maximum - Range1.Minimum) / 2;

            var parameterValue = 128;
            LrDevelopController
                .Setup(m => m.GetValue(out parameterValue, TestParameter.IntegerParameter))
                .Returns(true)
                .Verifiable();

            // Test
            ProfileManager.OnParameterChanged(TestParameter.IntegerParameter);

            // Verify
            LrDevelopController.Verify();
            
            var msg = TakeOutput();
            Assert.Equal(controllerValue, msg.Value);
        }
    }
}