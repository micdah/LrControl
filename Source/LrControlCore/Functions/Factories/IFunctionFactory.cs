namespace LrControlCore.Functions.Factories
{
    public interface IFunctionFactory
    {
        IFunction CreateFunction();
        string DisplayName { get; }
        string Key { get; }
    }
}