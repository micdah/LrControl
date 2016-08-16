namespace LrControlCore.Functions.Factories
{
    public interface IFunctionFactory
    {
        Function CreateFunction();
        string DisplayName { get; }
        string Key { get; }
    }
}