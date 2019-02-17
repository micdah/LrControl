using System.Collections.Generic;
using System.Linq;
using LrControl.Configurations;
using LrControl.Functions.Factories;
using LrControl.LrPlugin.Api;

namespace LrControl.Functions.Catalog
{
    public partial class FunctionCatalog : IFunctionCatalog
    {
        private readonly List<IFunctionCatalogGroup> _groups;
        private readonly Dictionary<string, IFunctionFactory> _functionFactories;
        
        public FunctionCatalog(ISettings settings, ILrApi api)
        {
            _groups = CreateGroups(settings, api);
            _functionFactories = new Dictionary<string, IFunctionFactory>();

            // Build function factory lookup
            foreach (var functionFactory in _groups.SelectMany(g => g.FunctionFactories))
            {
                _functionFactories[functionFactory.Key] = functionFactory;
            }
        }

        public IEnumerable<IFunctionCatalogGroup> Groups => _groups.AsReadOnly();

        public bool TryGetFunctionFactory(string functionKey, out IFunctionFactory functionFactory)
            => _functionFactories.TryGetValue(functionKey, out functionFactory);

        private static List<IFunctionCatalogGroup> CreateGroups(ISettings settings, ILrApi api)
        {
            return new List<IFunctionCatalogGroup>(CreateDevelopGroups(settings, api))
            {
                CreateViewGroup(settings, api),
                CreateUndoGroup(settings, api),
                CreateSelectionGroup(settings, api)
            };
        }
    }
}