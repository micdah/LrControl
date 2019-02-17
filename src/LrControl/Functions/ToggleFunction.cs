using LrControl.Configurations;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Functions
{
    public abstract class ToggleFunction : Function
    {
        protected ToggleFunction(ISettings settings, ILrApi api, string displayName, string key) 
            : base(settings, api, displayName, key)
        {
        }

        public sealed override void Apply(int value, Range range, Module activeModule, Panel activePanel)
        {
            if (!range.IsMaximum(value)) return;
            
            Toggle(value, range, activeModule, activePanel);
        }

        protected abstract void Toggle(int value, Range range, Module activeModule, Panel activePanel);
    }
}