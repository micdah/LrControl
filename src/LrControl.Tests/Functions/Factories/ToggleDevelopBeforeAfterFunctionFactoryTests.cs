using LrControl.Configurations;
using LrControl.Functions;
using LrControl.Functions.Factories;
using LrControl.LrPlugin.Api;
using Xunit;
using Xunit.Abstractions;

namespace LrControl.Tests.Functions.Factories
{
    public class ToggleDevelopBeforeAfterFunctionFactoryTests 
        : FunctionFactoryTestSuite<ToggleDevelopBeforeAfterFunctionFactory>
    {
        public ToggleDevelopBeforeAfterFunctionFactoryTests(ITestOutputHelper output) : base(output)
        {
        }

        protected override ToggleDevelopBeforeAfterFunctionFactory CreateFactory(ISettings settings, ILrApi lrApi)
            => new ToggleDevelopBeforeAfterFunctionFactory(settings, lrApi);

        [Fact]
        public void Should_Create_ToggleDevelopBeforeAfterFunction()
        {
            Create<ToggleDevelopBeforeAfterFunction>();
        }
    }
}