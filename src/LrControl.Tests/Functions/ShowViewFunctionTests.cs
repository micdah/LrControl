using LrControl.Functions;
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

        private ShowViewFunction Create(PrimaryView primaryView) =>
            new ShowViewFunction(Settings.Object, LrApi.Object, "Test Function", "TestFunction", primaryView);

        [Fact]
        public void Should_ShowView_When_Applied()
        {
            foreach (var primaryView in PrimaryView.GetAll())
            {
                var function = Create(primaryView);
                ProfileManager.AssignFunction(DefaultModule, Id1, function);

                ControllerInput(Id1, Range1.Maximum, Range1.Minimum);

                LrApplicationView
                    .Verify(_ => _.ShowView(primaryView), Times.Once());
            }
        }
    }
}