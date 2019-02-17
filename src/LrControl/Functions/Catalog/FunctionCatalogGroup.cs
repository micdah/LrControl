using System.Collections.Generic;
using LrControl.Functions.Factories;

namespace LrControl.Functions.Catalog
{
    internal class FunctionCatalogGroup : IFunctionCatalogGroup
    {
        public string DisplayName { get; set; }
        public string Key { get; set; }
        public IEnumerable<IFunctionFactory> FunctionFactories { get; set; }
    }
}