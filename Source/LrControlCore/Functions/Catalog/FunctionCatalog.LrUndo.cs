using System.Collections.ObjectModel;
using LrControlCore.Functions.Factories;
using micdah.LrControlApi;

namespace LrControlCore.Functions.Catalog
{
    public partial class FunctionCatalog
    {
        private static FunctionCatalogGroup CreateUndoGroup(LrApi api)
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