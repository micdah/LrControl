using System;
using LrControl.Configurations;
using LrControl.Functions;
using LrControl.Functions.Factories;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;
using LrControl.LrPlugin.Api.Modules.LrDevelopController.Parameters;
using LrControl.Tests.Mocks;
using Xunit;
using Xunit.Abstractions;

namespace LrControl.Tests.Functions.Factories
{
    public class ParameterFunctionFactoryTests : FunctionFactoryTestSuite<ParameterFunctionFactory, IParameter>
    {
        public ParameterFunctionFactoryTests(ITestOutputHelper output) : base(output)
        {
        }

        private static readonly IParameter[] Parameters =
        {
            TestParameter.IntegerParameter,
            TestParameter.DoubleParameter
        };

        [Fact]
        public void Should_Require_Generic_Parameter()
        {
            Assert.Throws<ArgumentException>(() =>
                new ParameterFunctionFactory(Settings.Object, LrApi.Object,
                    TestParameter.StringEnumerationParameter));
        }

        protected override ParameterFunctionFactory CreateFactory(ISettings settings, ILrApi lrApi, IParameter arg)
            => new ParameterFunctionFactory(settings, lrApi, arg);

        [Fact]
        public void Should_Create_TemperatureParameterFunction()
        {
            var (_, function) = Create(AdjustPanelParameter.Temperature);
            Assert.IsAssignableFrom<TemperatureParameterFunction>(function);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void Should_Create_ParameterFunction(int parameterIndex)
        {
            var parameter = Parameters[parameterIndex];
            var (_, function) = Create(parameter);
            var parameterFunction = function as ParameterFunction;
            Assert.NotNull(parameterFunction);
            Assert.Equal(parameter, parameterFunction.Parameter);
        }

        [Fact]
        public void Should_Create_ToggleParameterFunction()
        {
            var (_, function) = Create(TestParameter.BooleanParameter);
            var toggleParameterFunction = function as ToggleParameterFunction;
            Assert.NotNull(toggleParameterFunction);
            Assert.Equal(TestParameter.BooleanParameter, toggleParameterFunction.Parameter);
        }
    }
}