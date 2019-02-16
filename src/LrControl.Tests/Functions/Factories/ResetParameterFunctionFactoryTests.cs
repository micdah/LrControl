using LrControl.Configurations;
using LrControl.Functions;
using LrControl.Functions.Factories;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;
using LrControl.Tests.Mocks;
using Xunit;
using Xunit.Abstractions;

namespace LrControl.Tests.Functions.Factories
{
    public class ResetParameterFunctionFactoryTests : FunctionFactoryTestSuite<ResetParameterFunctionFactory, IParameter>
    {
        public ResetParameterFunctionFactoryTests(ITestOutputHelper output) : base(output)
        {
        }

        private static readonly IParameter[] Parameters =
        {
            TestParameter.DoubleParameter,
            TestParameter.BooleanParameter,
            TestParameter.IntegerParameter
        };

        protected override ResetParameterFunctionFactory CreateFactory(ISettings settings, ILrApi lrApi, IParameter arg)
            => new ResetParameterFunctionFactory(settings, lrApi, arg);

        [Theory]
        [InlineData(0), InlineData(1), InlineData(2)]
        public void Should_Create_ResetParameterFunction(int parameterIndex)
        {
            var parameter = Parameters[parameterIndex];
            var (_, function) = Create<ResetParameterFunction>(parameter);
            Assert.Equal(parameter, function.Parameter);
        }
    }
}