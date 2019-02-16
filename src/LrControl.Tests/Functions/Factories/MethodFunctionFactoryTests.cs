using System;
using LrControl.Configurations;
using LrControl.Functions;
using LrControl.Functions.Factories;
using LrControl.LrPlugin.Api;
using Xunit;
using Xunit.Abstractions;

namespace LrControl.Tests.Functions.Factories
{
    public class MethodFunctionFactoryTests : FunctionFactoryTestSuite<MethodFunctionFactory>
    {
        private static readonly Action<ILrApi> TestAction = api => { };
        
        public MethodFunctionFactoryTests(ITestOutputHelper output) : base(output)
        {
        }

        protected override MethodFunctionFactory CreateFactory(ISettings settings, ILrApi lrApi)
            => new MethodFunctionFactory(settings, lrApi, "Test Function", "TestFunction", TestAction);

        [Fact]
        public void Should_Create_MethodFunction()
        {
            var (_, function) = Create<MethodFunction>();
            Assert.Equal(TestAction, function.Method);
        }
    }
}