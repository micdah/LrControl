using LrControl.Enums;
using LrControl.Functions;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;
using LrControl.Tests.Devices;
using LrControl.Tests.Mocks;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace LrControl.Tests.Functions
{
    public class ToggleParameterFunctionTests : ProfileManagerTestSuite
    {
        private readonly IParameter<bool> _parameter;

        public ToggleParameterFunctionTests(ITestOutputHelper output) : base(output)
        {
            _parameter = TestParameter.BooleanParameter;
            var function = new ToggleParameterFunction(Settings.Object, LrApi.Object,
                "Test Function", "TestFunction", _parameter);
            
            ProfileManager.AssignFunction(DefaultModule, Id1, function);
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

    public class UnaryOperatorParameterFunctionTests : ProfileManagerTestSuite
    {
        private static readonly IParameter<double> Parameter = TestParameter.DoubleParameter;

        public UnaryOperatorParameterFunctionTests(ITestOutputHelper output) : base(output)
        {
        }

        private UnaryOperatorParameterFunction Create(UnaryOperation operation) =>
            new UnaryOperatorParameterFunction(Settings.Object, LrApi.Object, "Test Function", "TestFunction",
                Parameter, operation);

        [Theory]
        [InlineData(UnaryOperation.Increment)]
        [InlineData(UnaryOperation.Decrement)]
        public void Should_Increment_Or_Decrement_Parameter(UnaryOperation operation)
        {
            // Setup
            var function = Create(operation);
            ProfileManager.AssignFunction(DefaultModule, Id1, function);

            LrDevelopController.Setup(_ => _.Increment(Parameter)).Returns(true);
            LrDevelopController.Setup(_ => _.Decrement(Parameter)).Returns(false);

            // Test
            ControllerInput(Id1, Range1.Maximum);

            // Verify
            if (operation == UnaryOperation.Increment)
                LrDevelopController.Verify(_ => _.Increment(Parameter), Times.Once());
            else
                LrDevelopController.Verify(_ => _.Decrement(Parameter), Times.Once);
        }

        [Fact]
        public void Should_Only_Apply_When_Controller_Is_At_Maximum_Value()
        {
            // Setup
            var function = Create(UnaryOperation.Increment);
            ProfileManager.AssignFunction(DefaultModule, Id1, function);

            // Test
            ControllerInput(Id1, Range1.Maximum - 0.01d);

            // Verify
            LrApi.Verify(_ => _.LrDevelopController, Times.Never());
        }
    }
}