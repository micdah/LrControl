using System.Linq;
using LrControl.Configurations;
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
                var factory = group.FunctionFactories.SingleOrDefault(x =>
                    x is SwitchToModuleFunctionFactory f &&
                    f.Module == module);
                
                Assert.NotNull(factory);
            }
        }

        [Fact]
        public void Should_Have_ShowViewFunction_For_Each_PrimaryView()
        {
            var group = Group(LrApplicationViewKey);
            foreach (var primaryView in PrimaryView.GetAll())
            {
                var factory = group.FunctionFactories.SingleOrDefault(x =>
                    x is ShowViewFunctionFactory f &&
                    f.PrimaryView == primaryView);

                Assert.NotNull(factory);
            }
        }
        
        #endregion
    }
}