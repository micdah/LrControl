using LrControl.Api;
using LrControl.Api.Modules.LrDevelopController;
using LrControl.Core.Configurations;

namespace LrControl.Core.Functions.Factories
{
    internal class EnablePanelFunctionFactory : FunctionFactory
    {
        private readonly IParameter<bool> _enablePanelParameter;
        private readonly Panel _panel;

        public EnablePanelFunctionFactory(ISettings settings, LrApi api, Panel panel,
            IParameter<bool> enablePanelParameter) : base(settings, api)
        {
            _enablePanelParameter = enablePanelParameter;
            _panel = panel;
        }

        public override string DisplayName => $"Switch to panel {_panel.Name} (or toggle on/off)";
        public override string Key => $"EnablePanelFunction:{_panel.Name}";

        protected override IFunction CreateFunction(ISettings settings, LrApi api)
        {
            return new EnablePanelFunction(settings, api, DisplayName, _panel, _enablePanelParameter, Key);
        }
    }
}