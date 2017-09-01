using System.Collections.ObjectModel;
using LrControl.Api;
using LrControl.Core.Functions.Factories;

namespace LrControl.Core.Functions.Catalog
{
    public partial class FunctionCatalog
    {
        private static IFunctionCatalogGroup CreateUndoGroup(LrApi api)
        {
            return new FunctionCatalogGroup
            {
                DisplayName = "Undo",
                Key = "LrUndo",
                Functions = new ObservableCollection<IFunctionFactory>(new[]
                {
                    new MethodFunctionFactory(api, "Undo", "Undo", a =>
                    {
                        bool canUndo;
                        api.LrUndo.CanUndo(out canUndo);
                        if (canUndo)
                        {
                            api.LrDevelopController.StopTracking();
                            api.LrUndo.Undo();
                        }
                    }),
                    new MethodFunctionFactory(api, "Redo", "Redo", a =>
                    {
                        bool canRedo;
                        api.LrUndo.CanRedo(out canRedo);
                        if (canRedo)
                        {
                            api.LrDevelopController.StopTracking();
                            api.LrUndo.Redo();
                        }
                    }), 
                })
            };
        }
    }
}