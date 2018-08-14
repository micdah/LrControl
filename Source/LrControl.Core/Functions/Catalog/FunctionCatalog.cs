using System.Collections.Generic;
using System.Linq;
using LrControl.Api;
using LrControl.Core.Configurations;
using LrControl.Core.Functions.Factories;

namespace LrControl.Core.Functions.Catalog
{
    internal partial class FunctionCatalog : IFunctionCatalog
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

        public static IFunctionCatalog CreateCatalog(ISettings settings, LrApi api)
        {
            return new FunctionCatalog(CreateGroups(settings, api));
        }

        private static IEnumerable<IFunctionCatalogGroup> CreateGroups(ISettings settings, LrApi api)
        {
            var groups = new List<IFunctionCatalogGroup>
            {
                CreateViewGroup(settings, api),
                CreateUndoGroup(settings, api),
                CreateSelectionGroup(settings, api)
            };
            groups.AddRange(CreateDevelopGroups(settings, api));
            return groups;
        }
    }
}