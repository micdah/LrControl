using LrControl.Configurations;
using LrControl.Enums;
using LrControl.Functions;
using LrControl.Functions.Factories;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;
using LrControl.Tests.Mocks;
using Xunit;
using Xunit.Abstractions;

namespace LrControl.Tests.Functions.Factories
{
    public class UnaryOperatorParameterFunctionFactoryTests
        : FunctionFactoryTestSuite<UnaryOperatorParameterFunctionFactory, IParameter, UnaryOperation>
    {
        public UnaryOperatorParameterFunctionFactoryTests(ITestOutputHelper output) : base(output)
        {
        }

        private static readonly IParameter[] Parameters =
        {
            TestParameter.DoubleParameter,
            TestParameter.BooleanParameter,
            TestParameter.IntegerParameter
        };

        protected override UnaryOperatorParameterFunctionFactory CreateFactory(ISettings settings, ILrApi lrApi,
            IParameter arg1, UnaryOperation arg2)
            => new UnaryOperatorParameterFunctionFactory(settings, lrApi, arg1, arg2);

        [Theory]
        [InlineData(0, UnaryOperation.Increment), InlineData(0, UnaryOperation.Decrement)]
        [InlineData(1, UnaryOperation.Increment), InlineData(1, UnaryOperation.Decrement)]
        [InlineData(2, UnaryOperation.Increment), InlineData(2, UnaryOperation.Decrement)]
        public void Should_Create_UnaryOperatorParameterFunction(int parameterIndex, UnaryOperation operation)
        {
            var parameter = Parameters[parameterIndex];
            var (_, function) = Create<UnaryOperatorParameterFunction>(parameter, operation);
            Assert.Equal(parameter, function.Parameter);
            Assert.Equal(operation, function.Operation);
        }
    }
}