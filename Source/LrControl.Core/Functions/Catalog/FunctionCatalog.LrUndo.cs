using System.Collections.ObjectModel;
using LrControl.Api;
using LrControl.Core.Configurations;
using LrControl.Core.Functions.Factories;

namespace LrControl.Core.Functions.Catalog
{
    public partial class FunctionCatalog
    {
        private static IFunctionCatalogGroup CreateUndoGroup(ISettings settings, LrApi api)
        {
            return new FunctionCatalogGroup
            {
                DisplayName = "Undo",
                Key = "LrUndo",
                Functions = new ObservableCollection<IFunctionFactory>(new[]
                {
                    new MethodFunctionFactory(settings, api, "Undo", "Undo", a =>
                    {
                        api.LrUndo.CanUndo(out var canUndo);
                        if (canUndo)
                        {
                            api.LrDevelopController.StopTracking();
                            api.LrUndo.Undo();
                        }
                    }),
                    new MethodFunctionFactory(settings, api, "Redo", "Redo", a =>
                    {
                        api.LrUndo.CanRedo(out var canRedo);
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