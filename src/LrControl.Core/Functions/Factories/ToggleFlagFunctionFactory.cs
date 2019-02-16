using LrControl.Configurations;
using LrControl.Functions;
using LrControl.Functions.Factories;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Modules.LrSelection;
using Serilog;

namespace LrControl.Core.Functions.Factories
{
    internal class ToggleFlagFunctionFactory : FunctionFactory
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<ToggleFlagFunctionFactory>();
        public Flag Flag { get; }

        public ToggleFlagFunctionFactory(ISettings settings, ILrApi api, Flag flag) : base(settings, api)
        {
            Flag = flag;
            DisplayName = $"Toggle Flag as {flag.Name}";
            Key = $"ToggleFlagAs{flag.Name}";
        }

        public override string DisplayName { get; }
        public override string Key { get; }
        
        protected override IFunction CreateFunction(ISettings settings, ILrApi api)
        {
            return new ToggleFlagFunction(settings, api, DisplayName, Key, Flag);
        }
    }
}