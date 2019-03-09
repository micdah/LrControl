using LrControl.Functions.Factories;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.Tests.Devices;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace LrControl.Tests.Functions
{
    public class ShowViewFunctionTests : ProfileManagerTestSuite
    {
        public ShowViewFunctionTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Should_ShowView_When_Applied()
        {
            foreach (var primaryView in PrimaryView.GetAll())
            {
                var factory = GetFactory<ShowViewFunctionFactory>(f => f.PrimaryView == primaryView);
                LoadFunction(DefaultModule, Id1, factory);

                ControllerInput(Id1, Range1.Maximum, Range1.Minimum);

                LrApplicationView
                    .Verify(_ => _.ShowView(primaryView), Times.Once());
            }
        }
    }
}