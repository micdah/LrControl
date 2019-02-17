using LrControl.Configurations;
using LrControl.Functions;
using LrControl.Functions.Factories;
using LrControl.LrPlugin.Api;
using Xunit;
using Xunit.Abstractions;

namespace LrControl.Tests.Functions.Factories
{
    public class ToggleSecondaryDisplayFunctionFactoryTests
        : FunctionFactoryTestSuite<ToggleSecondaryDisplayFunctionFactory>
    {
        public ToggleSecondaryDisplayFunctionFactoryTests(ITestOutputHelper output) : base(output)
        {
        }

        protected override ToggleSecondaryDisplayFunctionFactory CreateFactory(ISettings settings, ILrApi lrApi)
            => new ToggleSecondaryDisplayFunctionFactory(settings, lrApi);

        [Fact]
        public void Should_Create_ToggleSecondaryDisplayFunctionFactory()
        {
            Create<ToggleSecondaryDisplayFunction>();
        }
    }
}