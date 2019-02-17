using LrControl.Configurations;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;

namespace LrControl.Functions.Factories
{
    public class SwitchToModuleFunctionFactory : FunctionFactory
    {
        private readonly Module _module;

        public SwitchToModuleFunctionFactory(ISettings settings, ILrApi api, Module module) : base(settings, api)
        {
            _module = module;
            DisplayName = $"Switch to {module.Name}";
            Key = $"SwitchToModule{module.Value}";
        }

        public override string DisplayName { get; }
        public override string Key { get; }

        protected override IFunction CreateFunction(ISettings settings, ILrApi api)
            => new SwitchToModuleFunction(settings, api, DisplayName, Key, _module);
    }
}