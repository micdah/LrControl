using LrControl.Configurations;
using LrControl.Enums;
using LrControl.Functions;
using LrControl.Functions.Factories;
using LrControl.LrPlugin.Api;
using Xunit;
using Xunit.Abstractions;

namespace LrControl.Tests.Functions.Factories
{
    public class UndoRedoFunctionFactoryTests : FunctionFactoryTestSuite<UndoRedoFunctionFactory, Operation>
    {
        public UndoRedoFunctionFactoryTests(ITestOutputHelper output) : base(output)
        {
        }

        protected override UndoRedoFunctionFactory CreateFactory(ISettings settings, ILrApi lrApi, Operation arg)
            => new UndoRedoFunctionFactory(settings, lrApi, arg);

        [Theory]
        [InlineData(Operation.Redo), InlineData(Operation.Undo)]
        public void Should_Create_UndoRedoFunction(Operation operation)
        {
            var (_, function) = Create<UndoRedoFunction>(operation);
            Assert.Equal(operation, function.Operation);
        }
    }
}