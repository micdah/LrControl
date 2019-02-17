using LrControl.Configurations;
using LrControl.Functions;
using LrControl.Functions.Factories;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using Xunit;
using Xunit.Abstractions;

namespace LrControl.Tests.Functions.Factories
{
    public class ShowViewFunctionFactoryTests : FunctionFactoryTestSuite<ShowViewFunctionFactory, PrimaryView>
    {
        public ShowViewFunctionFactoryTests(ITestOutputHelper output) : base(output)
        {
        }

        protected override ShowViewFunctionFactory CreateFactory(ISettings settings, ILrApi lrApi, PrimaryView arg)
            => new ShowViewFunctionFactory(settings, lrApi, arg);

        [Fact]
        public void Should_Create_ShowViewFunction()
        {
            foreach (var primaryView in PrimaryView.GetAll())
            {
                var (_, function) = Create<ShowViewFunction>(primaryView);
                Assert.Equal(primaryView, function.PrimaryView);
            }
        }
    }
}