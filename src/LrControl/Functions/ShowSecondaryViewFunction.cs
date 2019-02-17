using LrControl.Configurations;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Functions
{
    public class ShowSecondaryViewFunction : ToggleFunction
    {
        public SecondaryView SecondaryView { get; }

        public ShowSecondaryViewFunction(ISettings settings, ILrApi api, string displayName, string key,
            SecondaryView secondaryView) : base(settings, api, displayName, key)
        {
            SecondaryView = secondaryView;
        }

        protected override void Toggle(int value, Range range, Module activeModule, Panel activePanel)
        {
            Api.LrApplicationView.ShowSecondaryView(SecondaryView);
        }
    }
}