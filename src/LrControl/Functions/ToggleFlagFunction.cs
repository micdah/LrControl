using LrControl.Configurations;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;
using LrControl.LrPlugin.Api.Modules.LrSelection;

namespace LrControl.Functions
{
    public class ToggleFlagFunction : Function
    {
        private readonly Flag _flag;

        public ToggleFlagFunction(ISettings settings, ILrApi api, string displayName, string key, Flag flag) 
            : base(settings, api, displayName, key)
        {
            _flag = flag;
        }

        public override void Apply(int value, Range range, Module activeModule, Panel activePanel)
        {
            if (!range.IsMaximum(value)) return;
            
            if (!Api.LrSelection.GetFlag(out var flag)) return;

            if (_flag.Equals(flag))
            {
                Api.LrSelection.RemoveFlag();
            }
            else
            {
                if (_flag.Equals(Flag.Pick))
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