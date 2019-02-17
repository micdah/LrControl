using LrControl.Configurations;
using LrControl.Enums;
using LrControl.Functions;
using LrControl.Functions.Factories;
using LrControl.LrPlugin.Api;
using Xunit;
using Xunit.Abstractions;

namespace LrControl.Tests.Functions.Factories
{
    public class ZoomInOutFunctionFactoryTests : FunctionFactoryTestSuite<ZoomInOutFunctionFactory, Zoom>
    {
        public ZoomInOutFunctionFactoryTests(ITestOutputHelper output) : base(output)
        {
        }

        protected override ZoomInOutFunctionFactory CreateFactory(ISettings settings, ILrApi lrApi, Zoom arg)
            => new ZoomInOutFunctionFactory(settings, lrApi, arg);

        [Theory]
        [InlineData(Zoom.In), InlineData(Zoom.InSome), InlineData(Zoom.Out), InlineData(Zoom.OutSome)]
        public void Should_Create_ZoomInOutFunction(Zoom zoom)
        {
            var (_, function) = Create<ZoomInOutFunction>(zoom);
            Assert.Equal(zoom, function.Zoom);
        }
    }
}