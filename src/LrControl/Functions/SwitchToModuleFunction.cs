using LrControl.Configurations;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Functions
{
    public class SwitchToModuleFunction : ToggleFunction
    {
        public Module Module { get; }

        public SwitchToModuleFunction(ISettings settings, ILrApi api, string displayName, string key, Module module) 
            : base(settings, api, displayName, key)
        {
            Module = module;
        }

        protected override void Toggle(int value, Range range, Module activeModule, Panel activePanel)
        {
            Api.LrApplicationView.SwitchToModule(Module);
        }
    }
}