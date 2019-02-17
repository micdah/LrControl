using LrControl.Functions;
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

        private SwitchToModuleFunction Create(Module module) =>
            new SwitchToModuleFunction(Settings.Object, LrApi.Object, "Test Function", "TestFunction", module);

        [Fact]
        public void Should_SwitchToModule_When_Applied()
        {
            foreach (var module in Module.GetAll())
            {
                // Setup
                var function = Create(module);
                ProfileManager.AssignFunction(DefaultModule, Id1, function);
                
                // Test
                ControllerInput(Id1, Range1.Maximum, Range1.Minimum);
                
                // Verify
                LrApplicationView
                    .Verify(_ => _.SwitchToModule(module), Times.Once());
            }
        }
    }
}