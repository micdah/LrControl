using LrControl.Configurations;
using LrControl.LrPlugin.Api;

namespace LrControl.Functions.Factories
{
    public class ToggleDevelopBeforeAfterFunctionFactory : FunctionFactory
    {
        public ToggleDevelopBeforeAfterFunctionFactory(ISettings settings, ILrApi api) : base(settings, api)
        {
        }

        public override string DisplayName => "Toggle Develop Before/After view";
        public override string Key => "ToggleDevelopBeforeAfterView";

        protected override IFunction CreateFunction(ISettings settings, ILrApi api)
            => new ToggleDevelopBeforeAfterFunction(settings, api, DisplayName, Key);
    }
}