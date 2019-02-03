using LrControl.Configurations;
using LrControl.Core.Configurations;
using LrControl.Functions;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Core.Functions.Factories
{
    internal class EnablePanelFunctionFactory : FunctionFactory
    {
        public IParameter<bool> EnablePanelParameter { get; }
        public Panel Panel { get; }

        public EnablePanelFunctionFactory(ISettings settings, ILrApi api, Panel panel,
            IParameter<bool> enablePanelParameter) : base(settings, api)
        {
            EnablePanelParameter = enablePanelParameter;
            Panel = panel;
        }

        public override string DisplayName => $"Switch to panel {Panel.Name} (or toggle on/off)";
        public override string Key => $"EnablePanelFunction:{Panel.Name}";

        protected override IFunction CreateFunction(ISettings settings, ILrApi api)
        {
            return new EnablePanelFunction(settings, api, DisplayName, Panel, EnablePanelParameter, Key);
        }
    }
}