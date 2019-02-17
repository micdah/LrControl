using System;
using System.Collections.Generic;
using System.Linq;
using LrControl.Configurations;
using LrControl.Enums;
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
                foreach (var primaryView in PrimaryView.GetAll())
                    yield return new ShowViewFunctionFactory(settings, api, primaryView);

                foreach (var secondaryView in SecondaryView.GetAll())
                    yield return new ShowSecondaryViewFunctionFactory(settings, api, secondaryView);

                yield return new ToggleSecondaryDisplayFunctionFactory(settings, api);

                yield return new ToggleSecondaryDisplayFullscreenFunctionFactory(settings, api);

                yield return new ToggleZoomFunctionFactory(settings, api);
                
                foreach (var zoom in Enum.GetValues(typeof(Zoom)).Cast<Zoom>())
                    yield return new ZoomInOutFunctionFactory(settings, api, zoom);

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