using System.Collections.Generic;
using System.Linq;
using LrControl.Core.Configurations;
using LrControl.Core.Functions.Factories;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;

namespace LrControl.Core.Functions.Catalog
{
    internal partial class FunctionCatalog
    {
        private static IFunctionCatalogGroup CreateViewGroup(ISettings settings, ILrApi api)
        {
            var functions = new List<IFunctionFactory>();
            functions.AddRange(CreateSwitchToModule(settings, api));
            functions.AddRange(CreateShowView(settings, api));
            functions.AddRange(CreateShowSecondaryView(settings, api));
            functions.AddRange(new IFunctionFactory[]
            {
                new MethodFunctionFactory(settings, api, "Toggle secondary display", "ToggleSecondaryDisplay", a => a.LrApplicationView.ToggleSecondaryDisplay()),
                new MethodFunctionFactory(settings, api, "Toggle secondary display fullscreen", "ToggleSecondaryDisplayFullscreen", a => a.LrApplicationView.ToggleSecondaryDisplayFullscreen()),
                new MethodFunctionFactory(settings, api, "Toggle zoom", "ToggleZoom", a => a.LrApplicationView.ToggleZoom()),
                new MethodFunctionFactory(settings, api, "Zoom in", "ZoomIn", a => a.LrApplicationView.ZoomIn()),
                new MethodFunctionFactory(settings, api, "Zoom in some", "ZoomInSome", a => a.LrApplicationView.ZoomInSome()),
                new MethodFunctionFactory(settings, api, "Zoom out", "ZoomOut", a => a.LrApplicationView.ZoomOut()),
                new MethodFunctionFactory(settings, api, "Zoom out some", "ZoomOutSome", a => a.LrApplicationView.ZoomOutSome()),
                new ToggleDevelopBeforeAfterFunctionFactory(settings, api),
            });

            return new FunctionCatalogGroup
            {
                DisplayName = "View",
                Key = "LrApplicationView",
                Functions = new List<IFunctionFactory>(functions),
            };
        }

        private static IEnumerable<MethodFunctionFactory> CreateSwitchToModule(ISettings settings, ILrApi api)
        {
            return Module.GetAll().Select(module =>
                new MethodFunctionFactory(settings, api, $"Switch to {module.Name}", $"SwitchToModule{module.Value}",
                    a => a.LrApplicationView.SwitchToModule(module)));
        }

        private static IEnumerable<MethodFunctionFactory> CreateShowView(ISettings settings, ILrApi api)
        {
            return PrimaryView.GetAll().Select(view =>
                new MethodFunctionFactory(settings, api, $"Change view to {view.Name}", $"ShowView{view.Value}",
                    a => a.LrApplicationView.ShowView(view)));
        }

        private static IEnumerable<MethodFunctionFactory> CreateShowSecondaryView(ISettings settings, ILrApi api)
        {
            return SecondaryView.GetAll().Select(view =>
                new MethodFunctionFactory(settings, api, $"Change secondary monitor to {view.Name}", $"ShowSecondaryView{view.Value}",
                    a => a.LrApplicationView.ShowSecondaryView(view)));
        }
    }
}