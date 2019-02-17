using System.Collections.Generic;
using LrControl.Functions.Factories;

namespace LrControl.Functions.Catalog
{
    public interface IFunctionCatalogGroup 
    {
        string DisplayName { get; }
        string Key { get; }
        IEnumerable<IFunctionFactory> FunctionFactories { get; }
    }
}