using LrControl.Enums;
using LrControl.Functions;
using LrControl.Tests.Devices;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace LrControl.Tests.Functions
{
    public class ZoomInOutFunctionTests : ProfileManagerTestSuite
    {
        public ZoomInOutFunctionTests(ITestOutputHelper output) : base(output)
        {
        }

        private ZoomInOutFunction Create(Zoom zoom) =>
            new ZoomInOutFunction(Settings.Object, LrApi.Object, "Test Function", "TestFunction", zoom);

        [Theory]
        [InlineData(Zoom.In), InlineData(Zoom.InSome), InlineData(Zoom.Out), InlineData(Zoom.OutSome)]
        public void Should_Zoom_When_Applied(Zoom zoom)
        {
            // Setup
            var function = Create(zoom);
            ProfileManager.AssignFunction(DefaultModule, Id1, function);

            // Test
            ControllerInput(Id1, Range1.Maximum);

            // Verify
            switch (zoom)
            {
                case Zoom.In:
                    LrApplicationView.Verify(_ => _.ZoomIn(), Times.Once);
                    break;
                case Zoom.InSome:
                    LrApplicationView.Verify(_ => _.ZoomInSome(), Times.Once);
                    break;
                case Zoom.Out:
                    LrApplicationView.Verify(_ => _.ZoomOut(), Times.Once);
                    break;
                case Zoom.OutSome:
                    LrApplicationView.Verify(_ => _.ZoomOutSome(), Times.Once());
                    break;
            }
        }
    }
}