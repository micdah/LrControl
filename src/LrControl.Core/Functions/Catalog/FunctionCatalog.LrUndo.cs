using System.Collections.Generic;
using LrControl.Configurations;
using LrControl.Core.Configurations;
using LrControl.Core.Functions.Factories;
using LrControl.LrPlugin.Api;

namespace LrControl.Core.Functions.Catalog
{
    internal partial class FunctionCatalog
    {
        private static IFunctionCatalogGroup CreateUndoGroup(ISettings settings, ILrApi api)
        {
            IEnumerable<IFunctionFactory> CreateFactories()
            {
                yield return new UndoRedoFunctionFactory(settings, api, UndoRedoFunction.Operation.Undo);
                yield return new UndoRedoFunctionFactory(settings, api, UndoRedoFunction.Operation.Redo);
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