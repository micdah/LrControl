using LrControl.Core.Configurations;
using LrControl.LrPlugin.Api;

namespace LrControl.Core.Functions.Factories
{
    internal class ToggleDevelopBeforeAfterFunctionFactory : FunctionFactory
    {
        public ToggleDevelopBeforeAfterFunctionFactory(ISettings settings, LrApi api) : base(settings, api)
        {
        }

        public override string DisplayName => "Toggle Develop Before/After view";
        public override string Key => "ToggleDevelopBeforeAfterView";
        protected override IFunction CreateFunction(ISettings settings, LrApi api)
        {
            return new ToggleDevelopBeforeAfterFunction(settings, api, DisplayName, Key);
        }
    }
}