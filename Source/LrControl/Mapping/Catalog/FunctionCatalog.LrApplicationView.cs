using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using micdah.LrControl.Mapping.Functions;
using micdah.LrControlApi;
using micdah.LrControlApi.Modules.LrApplicationView;

namespace micdah.LrControl.Mapping.Catalog
{
    public partial class FunctionCatalog
    {
        private static FunctionCatalogGroup CreateViewGroup(LrApi api)
        {
            var functions = new List<FunctionFactory>();
            functions.AddRange(CreateSwitchToModule(api));
            functions.AddRange(CreateShowView(api));
            functions.AddRange(CreateShowSecondaryView(api));
            functions.AddRange(new[]
            {
                new MethodFunctionFactory(api, "Toggle secondary display", "ToggleSecondaryDisplay",
                    a => a.LrApplicationView.ToggleSecondaryDisplay()),
                new MethodFunctionFactory(api, "Toggle secondary display fullscreen", "ToggleSecondaryDisplayFullscreen",
                    a => a.LrApplicationView.ToggleSecondaryDisplayFullscreen()),
                new MethodFunctionFactory(api, "Toggle zoom", "ToggleZoom", a => a.LrApplicationView.ToggleZoom()),
                new MethodFunctionFactory(api, "Zoom in", "ZoomIn", a => a.LrApplicationView.ZoomIn()),
                new MethodFunctionFactory(api, "Zoom in some", "ZoomInSome", a => a.LrApplicationView.ZoomInSome()),
                new MethodFunctionFactory(api, "Zoom out", "ZoomOut", a => a.LrApplicationView.ZoomOut()),
                new MethodFunctionFactory(api, "Zoom out some", "ZoomOutSome", a => a.LrApplicationView.ZoomOutSome())
            });

            return new FunctionCatalogGroup
            {
                DisplayName = "View",
                Key = "LrApplicationView",
                Functions = new ObservableCollection<FunctionFactory>(functions),
            };
        }

        private static IEnumerable<MethodFunctionFactory> CreateSwitchToModule(LrApi api)
        {
            return Module.AllEnums.Select(module =>
                new MethodFunctionFactory(api, $"Switch to {module.Name}", $"SwitchToModule{module.Value}",
                    a => a.LrApplicationView.SwitchToModule(module)));
        }

        private static IEnumerable<MethodFunctionFactory> CreateShowView(LrApi api)
        {
            return PrimaryView.AllEnums.Select(view =>
                new MethodFunctionFactory(api, $"Change view to {view.Name}", $"ShowView{view.Value}",
                    a => a.LrApplicationView.ShowView(view)));
        }

        private static IEnumerable<MethodFunctionFactory> CreateShowSecondaryView(LrApi api)
        {
            return SecondaryView.AllEnums.Select(view =>
                new MethodFunctionFactory(api, $"Change secondary monitor to {view.Name}", $"ShowSecondaryView{view.Value}",
                    a => a.LrApplicationView.ShowSecondaryView(view)));
        }
    }
}