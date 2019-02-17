using LrControl.Configurations;
using LrControl.Enums;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Functions
{
    public class ZoomInOutFunction : ToggleFunction
    {
        public Zoom Zoom { get; }

        public ZoomInOutFunction(ISettings settings, ILrApi api, string displayName, string key, Zoom zoom)
            : base(settings, api, displayName, key)
        {
            Zoom = zoom;
        }

        protected override void Toggle(int value, Range range, Module activeModule, Panel activePanel)
        {
            switch (Zoom)
            {
                case Zoom.In:
                    Api.LrApplicationView.ZoomIn();
                    break;
                case Zoom.InSome:
                    Api.LrApplicationView.ZoomInSome();
                    break;
                case Zoom.Out:
                    Api.LrApplicationView.ZoomOut();
                    break;
                case Zoom.OutSome:
                    Api.LrApplicationView.ZoomOutSome();
                    break;
            }
        }
    }
}