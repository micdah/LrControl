using System.Collections.Generic;
using LrControl.Configurations;
using LrControl.Functions.Factories;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;

namespace LrControl.Functions.Catalog
{
    public partial class FunctionCatalog
    {
        private static IFunctionCatalogGroup CreateViewGroup(ISettings settings, ILrApi api)
        {
            IEnumerable<IFunctionFactory> CreateFactories()
            {
                // Switch To Module functions
                foreach (var module in Module.GetAll())
                    yield return new SwitchToModuleFunctionFactory(settings, api, module);

                // Show View functions
                foreach (var view in PrimaryView.GetAll())
                    yield return new MethodFunctionFactory(settings, api,
                        $"Change view to {view.Name}",
                        $"ShowView{view.Value}",
                        a => a.LrApplicationView.ShowView(view));

                foreach (var view in SecondaryView.GetAll())
                    yield return new MethodFunctionFactory(settings, api,
                        $"Change secondary monitor to {view.Name}",
                        $"ShowSecondaryView{view.Value}",
                        a => a.LrApplicationView.ShowSecondaryView(view));

                yield return new MethodFunctionFactory(settings, api,
                    "Toggle secondary display",
                    "ToggleSecondaryDisplay",
                    a => a.LrApplicationView.ToggleSecondaryDisplay());

                yield return new MethodFunctionFactory(settings, api,
                    "Toggle secondary display fullscreen",
                    "ToggleSecondaryDisplayFullscreen",
                    a => a.LrApplicationView.ToggleSecondaryDisplayFullscreen());

                yield return new MethodFunctionFactory(settings, api,
                    "Toggle zoom",
                    "ToggleZoom",
                    a => a.LrApplicationView.ToggleZoom());

                yield return new MethodFunctionFactory(settings, api,
                    "Zoom in",
                    "ZoomIn",
                    a => a.LrApplicationView.ZoomIn());

                yield return new MethodFunctionFactory(settings, api,
                    "Zoom in some",
                    "ZoomInSome",
                    a => a.LrApplicationView.ZoomInSome());

                yield return new MethodFunctionFactory(settings, api,
                    "Zoom out",
                    "ZoomOut",
                    a => a.LrApplicationView.ZoomOut());

                yield return new MethodFunctionFactory(settings, api,
                    "Zoom out some",
                    "ZoomOutSome",
                    a => a.LrApplicationView.ZoomOutSome());

                yield return new ToggleDevelopBeforeAfterFunctionFactory(settings, api);
            }

            return new FunctionCatalogGroup
            {
                DisplayName = "View",
                Key = "LrApplicationView",
                FunctionFactories = new List<IFunctionFactory>(CreateFactories()),
            };
        }
    }
}