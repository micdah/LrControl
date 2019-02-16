using System.Collections.Generic;
using LrControl.Functions.Factories;

namespace LrControl.Core.Functions.Catalog
{
    public interface IFunctionCatalog
    {
        IEnumerable<IFunctionCatalogGroup> Groups { get; }
        IFunctionFactory GetFunctionFactory(string functionKey);
    }
}