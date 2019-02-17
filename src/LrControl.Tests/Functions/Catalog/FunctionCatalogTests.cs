using System.Linq;
using LrControl.Configurations;
using LrControl.Enums;
using LrControl.Functions.Catalog;
using LrControl.Functions.Factories;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace LrControl.Tests.Functions.Catalog
{
    public class FunctionCatalogTests : TestSuite
    {
        private const string LrApplicationViewKey = "LrApplicationView";

        private readonly FunctionCatalog _catalog;

        public FunctionCatalogTests(ITestOutputHelper output) : base(output)
        {
            var settings = new Mock<ISettings>();
            var lrApi = new Mock<ILrApi>();
            _catalog = new FunctionCatalog(settings.Object, lrApi.Object);
        }

        private IFunctionCatalogGroup Group(string key)
        {
            var group = _catalog.Groups.SingleOrDefault(g => g.Key == key);
            Assert.NotNull(group);
            return group;
        }
        
        #region LrApplicationView

        [Fact]
        public void Should_Have_SwitchToModuleFunction_For_Each_Module()
        {
            var group = Group(LrApplicationViewKey);
            foreach (var module in Module.GetAll())
            {
                var factory = group.FunctionFactories
                    .OfType<SwitchToModuleFunctionFactory>()
                    .SingleOrDefault(f => f.Module == module);
                
                Assert.NotNull(factory);
                Assert.Equal(module, factory.Module);
            }
        }

        [Fact]
        public void Should_Have_ShowViewFunction_For_Each_PrimaryView()
        {
            var group = Group(LrApplicationViewKey);
            foreach (var primaryView in PrimaryView.GetAll())
            {
                var factory = group.FunctionFactories
                    .OfType<ShowViewFunctionFactory>()
                    .SingleOrDefault(f => f.PrimaryView == primaryView);

                Assert.NotNull(factory);
                Assert.Equal(primaryView, factory.PrimaryView);
            }
        }

        [Fact]
        public void Should_Have_ShowSecondaryViewFunction_For_Each_SecondaryView()
        {
            var group = Group(LrApplicationViewKey);
            foreach (var secondaryView in SecondaryView.GetAll())
            {
                var factory = group.FunctionFactories
                    .OfType<ShowSecondaryViewFunctionFactory>()
                    .SingleOrDefault(f => f.SecondaryView == secondaryView);

                Assert.NotNull(factory);
                Assert.Equal(secondaryView, factory.SecondaryView);
            }
        }

        [Fact]
        public void Should_Have_ToggleSecondaryDisplayFunction()
        {
            var group = Group(LrApplicationViewKey);
            var factory = group.FunctionFactories
                .OfType<ToggleSecondaryDisplayFunctionFactory>()
                .SingleOrDefault();
            
            Assert.NotNull(factory);
        }

        [Fact]
        public void Should_Have_ToggleSecondaryDisplayFullscreenFunction()
        {
            var group = Group(LrApplicationViewKey);
            var factory = group.FunctionFactories
                .OfType<ToggleSecondaryDisplayFullscreenFunctionFactory>()
                .SingleOrDefault();
            
            Assert.NotNull(factory);
        }

        [Fact]
        public void Should_Have_ToggleZoomFunction()
        {
            var group = Group(LrApplicationViewKey);
            var factory = group.FunctionFactories
                .OfType<ToggleZoomFunctionFactory>()
                .SingleOrDefault();
            
            Assert.NotNull(factory);
        }

        [Theory]
        [InlineData(Zoom.In), InlineData(Zoom.InSome), InlineData(Zoom.Out), InlineData(Zoom.OutSome)]
        public void Should_Have_ZoomInOutFunction(Zoom zoom)
        {
            var group = Group(LrApplicationViewKey);
            var factory = group.FunctionFactories
                .OfType<ZoomInOutFunctionFactory>()
                .SingleOrDefault(f => f.Zoom == zoom);
            
            Assert.NotNull(factory);
            Assert.Equal(zoom, factory.Zoom);
        }

        [Fact]
        public void Should_Have_ToggleDevelopBeforeAfterFunction()
        {
            var group = Group(LrApplicationViewKey);
            var factory = group.FunctionFactories
                .OfType<ToggleDevelopBeforeAfterFunctionFactory>()
                .SingleOrDefault();

            Assert.NotNull(factory);
        }
        
        #endregion
    }
}