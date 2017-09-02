using System.Collections.Generic;
using System.Linq;
using LrControl.Api;
using LrControl.Core.Functions.Factories;

namespace LrControl.Core.Functions.Catalog
{
    public partial class FunctionCatalog : IFunctionCatalog
    {
        private FunctionCatalog(IEnumerable<IFunctionCatalogGroup> groups)
        {
            Groups = groups;
        }

        public IEnumerable<IFunctionCatalogGroup> Groups { get; }

        public IFunctionFactory GetFunctionFactory(string functionKey)
        {
            return Groups
                .SelectMany(g => g.Functions)
                .FirstOrDefault(f => f.Key == functionKey);
        }

        public static IFunctionCatalog DefaultCatalog(LrApi api)
        {
            return new FunctionCatalog(CreateGroups(api));
        }

        private static IEnumerable<IFunctionCatalogGroup> CreateGroups(LrApi api)
        {
            var groups = new List<IFunctionCatalogGroup>
            {
                CreateViewGroup(api),
                CreateUndoGroup(api),
                CreateSelectionGroup(api)
            };
            groups.AddRange(CreateDevelopGroups(api));
            return groups;
        }
    }
}