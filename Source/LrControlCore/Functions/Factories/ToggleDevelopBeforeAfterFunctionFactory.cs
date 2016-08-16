using micdah.LrControlApi;

namespace LrControlCore.Functions.Factories
{
    internal class ToggleDevelopBeforeAfterFunctionFactory : FunctionFactory
    {
        public ToggleDevelopBeforeAfterFunctionFactory(LrApi api) : base(api)
        {
        }

        public override string DisplayName => "Toggle Develop Before/After view";
        public override string Key => "ToggleDevelopBeforeAfterView";
        protected override IFunction CreateFunction(LrApi api)
        {
            return new ToggleDevelopBeforeAfterFunction(api, DisplayName, Key);
        }
    }
}