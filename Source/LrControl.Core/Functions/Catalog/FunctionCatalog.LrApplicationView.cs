﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using LrControl.Api;
using LrControl.Api.Modules.LrApplicationView;
using LrControl.Core.Configurations;
using LrControl.Core.Functions.Factories;

namespace LrControl.Core.Functions.Catalog
{
    public partial class FunctionCatalog
    {
        private static IFunctionCatalogGroup CreateViewGroup(ISettings settings, LrApi api)
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
                Functions = new ObservableCollection<IFunctionFactory>(functions),
            };
        }

        private static IEnumerable<MethodFunctionFactory> CreateSwitchToModule(ISettings settings, LrApi api)
        {
            return Module.AllEnums.Select(module =>
                new MethodFunctionFactory(settings, api, $"Switch to {module.Name}", $"SwitchToModule{module.Value}",
                    a => a.LrApplicationView.SwitchToModule(module)));
        }

        private static IEnumerable<MethodFunctionFactory> CreateShowView(ISettings settings, LrApi api)
        {
            return PrimaryView.AllEnums.Select(view =>
                new MethodFunctionFactory(settings, api, $"Change view to {view.Name}", $"ShowView{view.Value}",
                    a => a.LrApplicationView.ShowView(view)));
        }

        private static IEnumerable<MethodFunctionFactory> CreateShowSecondaryView(ISettings settings, LrApi api)
        {
            return SecondaryView.AllEnums.Select(view =>
                new MethodFunctionFactory(settings, api, $"Change secondary monitor to {view.Name}", $"ShowSecondaryView{view.Value}",
                    a => a.LrApplicationView.ShowSecondaryView(view)));
        }
    }
}