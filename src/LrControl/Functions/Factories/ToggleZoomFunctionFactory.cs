using LrControl.Configurations;
using LrControl.LrPlugin.Api;

namespace LrControl.Functions.Factories
{
    public class ToggleZoomFunctionFactory : FunctionFactory
    {
        public ToggleZoomFunctionFactory(ISettings settings, ILrApi api) : base(settings, api)
        {
            DisplayName = "Toggle Zoom";
            Key = "ToggleZoom";
        }

        public override string DisplayName { get; }
        public override string Key { get; }

        protected override IFunction CreateFunction(ISettings settings, ILrApi api)
            => new ToggleZoomFunction(settings, api, DisplayName, Key);
    }
}