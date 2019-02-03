using LrControl.Functions;

namespace LrControl.Core.Functions.Factories
{
    public interface IFunctionFactory
    {
        IFunction CreateFunction();
        string DisplayName { get; }
        string Key { get; }
    }
}