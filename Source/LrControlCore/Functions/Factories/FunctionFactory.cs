using micdah.LrControlApi;

namespace LrControlCore.Functions.Factories
{
    public abstract class FunctionFactory : IFunctionFactory
    {
        private readonly LrApi _api;

        protected FunctionFactory(LrApi api)
        {
            _api = api;
        }

        public Function CreateFunction()
        {
            return CreateFunction(_api);
        }

        public abstract string DisplayName { get; }

        public abstract string Key { get; }

        protected abstract Function CreateFunction(LrApi api);
    }
}