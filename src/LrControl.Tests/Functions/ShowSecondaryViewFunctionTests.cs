using LrControl.Functions.Factories;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.Tests.Devices;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace LrControl.Tests.Functions
{
    public class ShowSecondaryViewFunctionTests : ProfileManagerTestSuite
    {
        public ShowSecondaryViewFunctionTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Should_ShowSecondaryView_When_Applied()
        {
            foreach (var secondaryView in SecondaryView.GetAll())
            {
                var factory = GetFactory<ShowSecondaryViewFunctionFactory>(f => f.SecondaryView == secondaryView);
                LoadFunction(DefaultModule, Id1, factory);

                ControllerInput(Id1, Range1.Maximum, Range1.Minimum);

                LrApplicationView
                    .Verify(_ => _.ShowSecondaryView(secondaryView), Times.Once());
            }
        }
    }
}