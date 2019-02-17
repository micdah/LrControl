using System.Collections.Generic;
using LrControl.Functions.Factories;

namespace LrControl.Functions.Catalog
{
    public interface IFunctionCatalog
    {
        IEnumerable<IFunctionCatalogGroup> Groups { get; }
        bool TryGetFunctionFactory(string functionKey, out IFunctionFactory functionFactory);
    }
}