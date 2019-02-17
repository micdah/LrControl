using System.Collections.Generic;
using LrControl.Functions.Factories;

namespace LrControl.Functions.Catalog
{
    public class FunctionCatalogGroup : IFunctionCatalogGroup
    {
        public string DisplayName { get; set; }
        public string Key { get; set; }
        public IEnumerable<IFunctionFactory> Functions { get; set; }
    }
}