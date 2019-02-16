using LrControl.Configurations;
using LrControl.Functions;
using LrControl.Functions.Factories;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;
using LrControl.Tests.Mocks;
using Xunit;
using Xunit.Abstractions;

namespace LrControl.Tests.Functions.Factories
{
    public class EnumerationParameterFunctionFactoryTests : FunctionFactoryTestSuite<
        EnumerationParameterFunctionFactory<int>, IEnumerationParameter<int>, IEnumeration<int>>
    {
        public EnumerationParameterFunctionFactoryTests(ITestOutputHelper output) : base(output)
        {
        }

        protected override EnumerationParameterFunctionFactory<int> CreateFactory(ISettings settings, ILrApi lrApi,
            IEnumerationParameter<int> arg1, IEnumeration<int> arg2)
            => new EnumerationParameterFunctionFactory<int>(settings, lrApi, arg1, arg2);

        [Fact]
        public void Should_Create_EnumerationParameterFunction()
        {
            var (_, function) = Create<EnumerationParameterFunction<int>>(
                TestParameter.IntegerEnumerationParameter, TestIntegerEnumeration.Value1);

            Assert.Equal(TestParameter.IntegerEnumerationParameter, function.Parameter);
            Assert.Equal(TestIntegerEnumeration.Value1, function.Value);
        }
    }
}