using LrControl.Configurations;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Functions
{
    public class ShowViewFunction : ToggleFunction
    {
        public PrimaryView PrimaryView { get; }

        public ShowViewFunction(ISettings settings, ILrApi api, string displayName, string key, PrimaryView primaryView)
            : base(settings, api, displayName, key)
        {
            PrimaryView = primaryView;
        }

        protected override void Toggle(int value, Range range, Module activeModule, Panel activePanel)
        {
            Api.LrApplicationView.ShowView(PrimaryView);
        }
    }
}