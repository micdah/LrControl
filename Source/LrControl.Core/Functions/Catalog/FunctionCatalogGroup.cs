using System.Collections.Generic;
using LrControl.Core.Functions.Factories;

namespace LrControl.Core.Functions.Catalog
{
    internal class FunctionCatalogGroup : IFunctionCatalogGroup
    {
        public string DisplayName { get; internal set; }
        public string Key { get; internal set; }
        public IEnumerable<IFunctionFactory> Functions { get; internal set; }
    }
}