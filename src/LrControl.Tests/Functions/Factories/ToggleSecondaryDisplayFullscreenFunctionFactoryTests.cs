using LrControl.Configurations;
using LrControl.Functions;
using LrControl.Functions.Factories;
using LrControl.LrPlugin.Api;
using Xunit;
using Xunit.Abstractions;

namespace LrControl.Tests.Functions.Factories
{
    public class ToggleSecondaryDisplayFullscreenFunctionFactoryTests
        : FunctionFactoryTestSuite<ToggleSecondaryDisplayFullscreenFunctionFactory>
    {
        public ToggleSecondaryDisplayFullscreenFunctionFactoryTests(ITestOutputHelper output) : base(output)
        {
        }

        protected override ToggleSecondaryDisplayFullscreenFunctionFactory CreateFactory(ISettings settings,
            ILrApi lrApi)
            => new ToggleSecondaryDisplayFullscreenFunctionFactory(settings, lrApi);

        [Fact]
        public void Should_Create_ToggleSecondaryDisplayFullscreenFunction()
        {
            Create<ToggleSecondaryDisplayFullscreenFunction>();
        }
    }
}