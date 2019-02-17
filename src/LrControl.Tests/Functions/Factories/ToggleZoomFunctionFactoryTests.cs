using LrControl.Configurations;
using LrControl.Functions;
using LrControl.Functions.Factories;
using LrControl.LrPlugin.Api;
using Xunit;
using Xunit.Abstractions;

namespace LrControl.Tests.Functions.Factories
{
    public class ToggleZoomFunctionFactoryTests : FunctionFactoryTestSuite<ToggleZoomFunctionFactory>
    {
        public ToggleZoomFunctionFactoryTests(ITestOutputHelper output) : base(output)
        {
        }

        protected override ToggleZoomFunctionFactory CreateFactory(ISettings settings, ILrApi lrApi)
            => new ToggleZoomFunctionFactory(settings, lrApi);

        [Fact]
        public void Should_Create_ToggleZoomFunctionFactory()
        {
            Create<ToggleZoomFunction>();
        }
    }
}