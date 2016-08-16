using micdah.LrControlApi;

namespace LrControlCore.Functions.Factories
{
    public class ToggleDevelopBeforeAfterFunctionFactory : FunctionFactory
    {
        public ToggleDevelopBeforeAfterFunctionFactory(LrApi api) : base(api)
        {
        }

        public override string DisplayName => "Toggle Develop Before/After view";
        public override string Key => "ToggleDevelopBeforeAfterView";
        protected override Function CreateFunction(LrApi api)
        {
            return new ToggleDevelopBeforeAfterFunction(api, DisplayName, Key);
        }
    }
}