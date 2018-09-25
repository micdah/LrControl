using LrControl.Core.Configurations;
using LrControl.LrPlugin.Api;

namespace LrControl.Core.Functions.Factories
{
    internal abstract class FunctionFactory : IFunctionFactory
    {
        private readonly ISettings _settings;
        private readonly LrApi _api;
        
        protected FunctionFactory(ISettings settings, LrApi api)
        {
            _settings = settings;
            _api = api;
        }

        public IFunction CreateFunction()
        {
            return CreateFunction(_settings, _api);
        }

        public abstract string DisplayName { get; }

        public abstract string Key { get; }

        protected abstract IFunction CreateFunction(ISettings settings, LrApi api);
    }
}