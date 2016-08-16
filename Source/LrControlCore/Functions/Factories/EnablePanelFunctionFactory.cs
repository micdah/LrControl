using micdah.LrControlApi;
using micdah.LrControlApi.Modules.LrDevelopController;

namespace LrControlCore.Functions.Factories
{
    internal class EnablePanelFunctionFactory : FunctionFactory
    {
        private readonly IParameter<bool> _enablePanelParameter;
        private readonly Panel _panel;

        public EnablePanelFunctionFactory(LrApi api, Panel panel,
            IParameter<bool> enablePanelParameter) : base(api)
        {
            _enablePanelParameter = enablePanelParameter;
            _panel = panel;
        }

        public override string DisplayName => $"Switch to panel {_panel.Name} (or toggle on/off)";
        public override string Key => $"EnablePanelFunction:{_panel.Name}";

        protected override IFunction CreateFunction(LrApi api)
        {
            return new EnablePanelFunction(api, DisplayName, _panel, _enablePanelParameter, Key);
        }
    }
}