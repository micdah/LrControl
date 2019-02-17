using LrControl.Functions;
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

        private ShowSecondaryViewFunction Create(SecondaryView secondaryView) =>
            new ShowSecondaryViewFunction(Settings.Object, LrApi.Object, "Test Function", "TestFunction",
                secondaryView);

        [Fact]
        public void Should_ShowSecondaryView_When_Applied()
        {
            foreach (var secondaryView in SecondaryView.GetAll())
            {
                var function = Create(secondaryView);
                ProfileManager.AssignFunction(DefaultModule, Id1, function);

                ControllerInput(Id1, Range1.Maximum, Range1.Minimum);

                LrApplicationView
                    .Verify(_ => _.ShowSecondaryView(secondaryView), Times.Once());
            }
        }
    }
}