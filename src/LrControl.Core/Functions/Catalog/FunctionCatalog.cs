using System.Collections.Generic;
using System.Linq;
using LrControl.Configurations;
using LrControl.Core.Configurations;
using LrControl.Core.Functions.Factories;
using LrControl.LrPlugin.Api;

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

        public static IFunctionCatalog CreateCatalog(ISettings settings, ILrApi api)
        {
            return new FunctionCatalog(CreateGroups(settings, api));
        }

        private static IEnumerable<IFunctionCatalogGroup> CreateGroups(ISettings settings, ILrApi api)
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