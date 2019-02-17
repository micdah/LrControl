using LrControl.Configurations;
using LrControl.LrPlugin.Api;

namespace LrControl.Functions.Factories
{
    public class ToggleSecondaryDisplayFunctionFactory : FunctionFactory
    {
        public ToggleSecondaryDisplayFunctionFactory(ISettings settings, ILrApi api) : base(settings, api)
        {
            DisplayName = "Toggle secondary display";
            Key = "ToggleSecondaryDisplay";
        }

        public override string DisplayName { get; }
        public override string Key { get; }

        protected override IFunction CreateFunction(ISettings settings, ILrApi api)
            => new ToggleSecondaryDisplayFunction(settings, api, DisplayName, Key);
    }
}