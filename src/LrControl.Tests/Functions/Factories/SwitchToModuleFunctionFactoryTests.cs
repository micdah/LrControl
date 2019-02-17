using LrControl.Configurations;
using LrControl.Functions;
using LrControl.Functions.Factories;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using Xunit;
using Xunit.Abstractions;

namespace LrControl.Tests.Functions.Factories
{
    public class SwitchToModuleFunctionFactoryTests : FunctionFactoryTestSuite<SwitchToModuleFunctionFactory, Module>
    {
        public SwitchToModuleFunctionFactoryTests(ITestOutputHelper output) : base(output)
        {
        }

        protected override SwitchToModuleFunctionFactory CreateFactory(ISettings settings, ILrApi lrApi, Module arg)
            => new SwitchToModuleFunctionFactory(settings, lrApi, arg);

        [Fact]
        public void Should_Create_SwitchToModuleFunction()
        {
            foreach (var module in Module.GetAll())
            {
                var (_, function) = Create<SwitchToModuleFunction>(module);
                Assert.Equal(module, function.Module);
            }
        }
    }
}