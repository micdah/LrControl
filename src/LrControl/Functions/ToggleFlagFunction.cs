using LrControl.Configurations;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;
using LrControl.LrPlugin.Api.Modules.LrSelection;

namespace LrControl.Functions
{
    public class ToggleFlagFunction : ToggleFunction
    {
        public Flag Flag { get; }

        public ToggleFlagFunction(ISettings settings, ILrApi api, string displayName, string key, Flag flag) 
            : base(settings, api, displayName, key)
        {
            Flag = flag;
        }

        protected override void Toggle(int value, Range range, Module activeModule, Panel activePanel)
        {
            if (!Api.LrSelection.GetFlag(out var flag)) return;

            if (Flag.Equals(flag))
            {
                Api.LrSelection.RemoveFlag();
            }
            else
            {
                if (Flag.Equals(Flag.Pick))
                {
                    Api.LrSelection.FlagAsPick();
                } else
                {
                    Api.LrSelection.FlagAsReject();
                }
            } 
        }
    }
}