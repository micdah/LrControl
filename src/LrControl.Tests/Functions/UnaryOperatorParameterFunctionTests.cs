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

    public class UndoRedoFunctionTests : ProfileManagerTestSuite
    {
        public UndoRedoFunctionTests(ITestOutputHelper output) : base(output)
        {
        }

        private UndoRedoFunction Create(Operation operation) =>
            new UndoRedoFunction(Settings.Object, LrApi.Object, "Test Function", "TestFunction",
                operation);

        [Theory]
        [InlineData(Operation.Undo)]
        [InlineData(Operation.Redo)]
        public void Should_Apply_Operation_If_Can(Operation operation)
        {
            // Setup
            var function = Create(operation);
            ProfileManager.AssignFunction(DefaultModule, Id1, function);

            var can = true;
            if (operation == Operation.Undo)
            {
                LrUndo.Setup(_ => _.CanUndo(out can)).Returns(true).Verifiable();
                LrUndo.Setup(_ => _.Undo()).Returns(true).Verifiable();
            }
            else
            {
                LrUndo.Setup(_ => _.CanRedo(out can)).Returns(true).Verifiable();
                LrUndo.Setup(_ => _.Redo()).Returns(true).Verifiable();
            }

            LrDevelopController.Setup(_ => _.StopTracking()).Returns(true).Verifiable();

            // Test
            ControllerInput(Id1, Range1.Maximum);

            // Verify
            LrUndo.Verify();
            LrDevelopController.Verify();
        }

        [Theory]
        [InlineData(Operation.Undo)]
        [InlineData(Operation.Redo)]
        public void Should_Not_Apply_Operation_If_Cannot(Operation operation)
        {
            // Setup
            var function = Create(operation);
            ProfileManager.AssignFunction(DefaultModule, Id1, function);

            var can = false;
            if (operation == Operation.Undo)
                LrUndo.Setup(_ => _.CanUndo(out can)).Returns(true).Verifiable();
            else
                LrUndo.Setup(_ => _.CanRedo(out can)).Returns(true).Verifiable();
            
            // Test
            ControllerInput(Id1, Range1.Maximum);

            // Verify
            if (operation == Operation.Undo)
                LrUndo.Verify(_ => _.Undo(), Times.Never);
            else
                LrUndo.Verify(_ => _.Redo(), Times.Never);
        }
    }
}