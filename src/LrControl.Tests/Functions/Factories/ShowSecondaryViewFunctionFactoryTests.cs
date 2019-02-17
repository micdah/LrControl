using LrControl.Configurations;
using LrControl.Functions;
using LrControl.Functions.Factories;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using Xunit;
using Xunit.Abstractions;

namespace LrControl.Tests.Functions.Factories
{
    public class ShowSecondaryViewFunctionFactoryTests
        : FunctionFactoryTestSuite<ShowSecondaryViewFunctionFactory, SecondaryView>
    {
        public ShowSecondaryViewFunctionFactoryTests(ITestOutputHelper output) : base(output)
        {
        }

        protected override ShowSecondaryViewFunctionFactory CreateFactory(ISettings settings, ILrApi lrApi,
            SecondaryView arg)
            => new ShowSecondaryViewFunctionFactory(settings, lrApi, arg);

        [Fact]
        public void Should_Create_ShowSecondaryViewFunction()
        {
            foreach (var secondaryView in SecondaryView.GetAll())
            {
                var (_, function) = Create<ShowSecondaryViewFunction>(secondaryView);
                Assert.Equal(secondaryView, function.SecondaryView);
            }
        }
    }
}