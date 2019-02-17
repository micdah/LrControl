using LrControl.Configurations;
using LrControl.Enums;
using LrControl.LrPlugin.Api;

namespace LrControl.Functions.Factories
{
    public class ZoomInOutFunctionFactory : FunctionFactory
    {
        public Zoom Zoom { get; }

        public ZoomInOutFunctionFactory(ISettings settings, ILrApi api, Zoom zoom) : base(settings, api)
        {
            Zoom = zoom;
            switch (zoom)
            {
                case Zoom.In:
                    DisplayName = "Zoom in";
                    Key = "ZoomIn";
                    break;
                case Zoom.InSome:
                    DisplayName = "Zoom in some";
                    Key = "ZoomInSome";
                    break;
                case Zoom.Out:
                    DisplayName = "Zoom out";
                    Key = "ZoomOut";
                    break;
                case Zoom.OutSome:
                    DisplayName = "Zoom out some";
                    Key = "ZoomOutSome";
                    break;
            }
        }

        public override string DisplayName { get; }
        public override string Key { get; }

        protected override IFunction CreateFunction(ISettings settings, ILrApi api)
            => new ZoomInOutFunction(settings, api, DisplayName, Key, Zoom);
    }
}