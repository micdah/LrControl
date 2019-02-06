using LrControl.Configurations;
using LrControl.Functions;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Core.Functions.Factories
{
    internal class RevealOrTogglePanelFunctionFactory : FunctionFactory
    {
        public Panel Panel { get; }

        public RevealOrTogglePanelFunctionFactory(ISettings settings, ILrApi api, Panel panel) : base(settings, api)
        {
            Panel = panel;
        }

        public override string DisplayName => $"Switch to panel {Panel.Name} (or toggle on/off)";
        public override string Key => $"RevealOrTogglePanelFunction:{Panel.Name}";

        protected override IFunction CreateFunction(ISettings settings, ILrApi api)
        {
            return new RevealOrTogglePanelFunction(settings, api, DisplayName, Key, Panel);
        }
    }
}