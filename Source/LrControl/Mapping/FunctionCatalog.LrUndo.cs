using System.Collections.ObjectModel;
using micdah.LrControl.Mapping.Functions;
using micdah.LrControlApi;

namespace micdah.LrControl.Mapping
{
    public partial class FunctionCatalog
    {
        private static FunctionCatalogGroup CreateUndoGroup(LrApi api)
        {
            return new FunctionCatalogGroup
            {
                DisplayName = "Undo",
                Key = "LrUndo",
                Functions = new ObservableCollection<FunctionFactory>(new[]
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