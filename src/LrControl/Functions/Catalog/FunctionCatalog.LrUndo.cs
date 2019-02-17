using System.Collections.Generic;
using LrControl.Configurations;
using LrControl.Enums;
using LrControl.Functions.Factories;
using LrControl.LrPlugin.Api;

namespace LrControl.Functions.Catalog
{
    public partial class FunctionCatalog
    {
        private static IFunctionCatalogGroup CreateUndoGroup(ISettings settings, ILrApi api)
        {
            IEnumerable<IFunctionFactory> CreateFactories()
            {
                yield return new UndoRedoFunctionFactory(settings, api, Operation.Undo);
                yield return new UndoRedoFunctionFactory(settings, api, Operation.Redo);
            }
            
            return new FunctionCatalogGroup
            {
                DisplayName = "Undo",
                Key = "LrUndo",
                FunctionFactories = new List<IFunctionFactory>(CreateFactories())
            };
        }
    }
}