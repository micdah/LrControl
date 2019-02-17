using LrControl.Configurations;
using LrControl.LrPlugin.Api;

namespace LrControl.Functions.Factories
{
    public class ToggleSecondaryDisplayFullscreenFunctionFactory : FunctionFactory
    {
        public ToggleSecondaryDisplayFullscreenFunctionFactory(ISettings settings, ILrApi api) : base(settings, api)
        {
            DisplayName = "Toggle secondary display fullscreen";
            Key = "ToggleSecondaryDisplayFullscreen";
        }

        public override string DisplayName { get; }
        public override string Key { get; }

        protected override IFunction CreateFunction(ISettings settings, ILrApi api)
            => new ToggleSecondaryDisplayFullscreenFunction(settings, api, DisplayName, Key);
    }
}