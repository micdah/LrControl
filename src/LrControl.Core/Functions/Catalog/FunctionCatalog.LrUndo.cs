using System.Collections.Generic;
using LrControl.Configurations;
using LrControl.Enums;
using LrControl.Functions.Factories;
using LrControl.LrPlugin.Api;

namespace LrControl.Core.Functions.Catalog
{
    internal partial class FunctionCatalog
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
                Functions = new List<IFunctionFactory>(CreateFactories())
            };
        }
    }
}