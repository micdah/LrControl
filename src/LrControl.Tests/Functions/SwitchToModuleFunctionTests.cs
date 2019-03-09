using LrControl.Functions.Factories;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.Tests.Devices;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace LrControl.Tests.Functions
{
    public class SwitchToModuleFunctionTests : ProfileManagerTestSuite
    {
        public SwitchToModuleFunctionTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Should_SwitchToModule_When_Applied()
        {
            foreach (var module in Module.GetAll())
            {
                // Setup
                var factory = GetFactory<SwitchToModuleFunctionFactory>(f => f.Module == module);
                LoadFunction(DefaultModule, Id1, factory);
                
                // Test
                ControllerInput(Id1, Range1.Maximum, Range1.Minimum);
                
                // Verify
                LrApplicationView
                    .Verify(_ => _.SwitchToModule(module), Times.Once());
            }
        }
    }
}