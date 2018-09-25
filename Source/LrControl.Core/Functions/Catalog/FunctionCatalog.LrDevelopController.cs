using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using LrControl.Core.Configurations;
using LrControl.Core.Functions.Factories;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;
using LrControl.LrPlugin.Api.Modules.LrDevelopController.Parameters;
using Panel = LrControl.LrPlugin.Api.Modules.LrDevelopController.Panel;

namespace LrControl.Core.Functions.Catalog
{
    internal partial class FunctionCatalog
    {
        private static IEnumerable<IFunctionCatalogGroup> CreateDevelopGroups(ISettings settings, LrApi api)
        {
            var groups = new List<IFunctionCatalogGroup>();
            groups.Add(CreateDevelopGroup(settings, api));
            groups.Add(CreateDevelopPanelGroup(settings, api, Panel.Basic, null, Parameters.AdjustPanelParameters.AllParameters));
            groups.Add(CreateDevelopPanelGroup(settings, api, Panel.ToneCurve, Parameters.EnablePanelParameters.ToneCurve, Parameters.TonePanelParameters.AllParameters));
            groups.Add(CreateDevelopPanelGroup(settings, api, Panel.ColorAdjustment, Parameters.EnablePanelParameters.ColorAdjustments, Parameters.MixerPanelParameters.AllParameters));
            groups.Add(CreateDevelopPanelGroup(settings, api, Panel.SplitToning, Parameters.EnablePanelParameters.SplitToning, Parameters.SplitToningPanelParameters.AllParameters));
            groups.Add(CreateDevelopPanelGroup(settings, api, Panel.Detail, Parameters.EnablePanelParameters.Detail, Parameters.DetailPanelParameters.AllParameters));
            groups.Add(CreateDevelopPanelGroup(settings, api, Panel.LensCorrections, Parameters.EnablePanelParameters.LensCorrections, Parameters.LensCorrectionsPanelParameters.AllParameters));
            groups.Add(CreateDevelopPanelGroup(settings, api, Panel.Effects, Parameters.EnablePanelParameters.Effects, Parameters.EffectsPanelParameters.AllParameters));
            groups.Add(CreateDevelopPanelGroup(settings, api, Panel.CameraCalibration, Parameters.EnablePanelParameters.Calibration, Parameters.CalibratePanelParameters.AllParameters));
            groups.Add(CreateDevelopCropGroup(settings, api));
            groups.Add(CreateDevelopLocalizedGroup(settings, api));

            return groups;
        }

        private static IFunctionCatalogGroup CreateDevelopGroup(ISettings settings, LrApi api)
        {
            var functions = new List<IFunctionFactory>();
            functions.AddRange(new []
            {
                new MethodFunctionFactory(settings, api, "Reset all develop adjustments", "ResetAllDevelopAdjustments", a => a.LrDevelopController.ResetAllDevelopAdjustments()),
                new MethodFunctionFactory(settings, api, "Reset Adjustment Brush", "ResetBrushing",a => a.LrDevelopController.ResetBrushing()),
                new MethodFunctionFactory(settings, api, "Reset Radial Filter", "ResetCircularGradient",a => a.LrDevelopController.ResetCircularGradient()),
                new MethodFunctionFactory(settings, api, "Reset Crop", "ResetCrop",a => a.LrDevelopController.ResetCrop()),
                new MethodFunctionFactory(settings, api, "Reset Graduated Filter", "ResetGradient",a => a.LrDevelopController.ResetGradient()),
                new MethodFunctionFactory(settings, api, "Reset Red Eye Correction", "ResetRedEye",a => a.LrDevelopController.ResetRedEye()),
                new MethodFunctionFactory(settings, api, "Reset Spot Removal", "ResetSpotRemoval",a => a.LrDevelopController.ResetSpotRemoval()),
            });

            foreach (var tool in Tool.AllEnums)
            {
                functions.Add(new MethodFunctionFactory(settings, api, $"Select Tool {tool.Name}", $"SelectTool{tool.Value}",
                    a => a.LrDevelopController.SelectTool(tool)));
            }

            return new FunctionCatalogGroup
            {
                DisplayName = "Develop",
                Key = "LrDevelop",
                Functions = new ObservableCollection<IFunctionFactory>(functions)
            };
        }

        private static IFunctionCatalogGroup CreateDevelopPanelGroup(ISettings settings, LrApi api, Panel panel, IParameter<bool> enablePanelParameter, IList<IParameter> parameters)
        {
            var functions = new List<IFunctionFactory>();
            functions.AddRange(new []
            {
                new EnablePanelFunctionFactory(settings, api, panel, enablePanelParameter),
            });

            // Change parameter
            foreach (var param in parameters)
            {
                if (param is IParameter<int> || param is IParameter<double>)
                {
                    functions.Add(new ParameterFunctionFactory(settings, api, param));
                }
            }

            // Change enum parameter
            functions.AddRange(CreateFunctionsForEnumParameter(settings, api, parameters));

            // Reset parameter
            foreach (var param in parameters)
            {
                functions.Add(new MethodFunctionFactory(settings, api, $"Reset {param.DisplayName} to default", $"ResetToDefault{param.Name}",
                    a =>
                    {
                        a.LrDevelopController.StopTracking();
                        a.LrDevelopController.ResetToDefault(param);
                    }));
            }


            // Toggle parameter
            functions.AddRange(parameters
                .OfType<IParameter<bool>>()
                .Select(parameter => new ToggleParameterFunctionFactory(settings, api, parameter)));

            // Increment / Decrement
            foreach (var param in parameters)
            {
                if (param is IParameter<int> || param is IParameter<double>)
                {
                    functions.Add(new MethodFunctionFactory(settings, api, $"Increment {param.DisplayName}", $"Increment{param.Name}", 
                        a => a.LrDevelopController.Increment(param)));
                    functions.Add(new MethodFunctionFactory(settings, api, $"Decrement {param.DisplayName}", $"Decrement{param.Name}",
                        a => a.LrDevelopController.Decrement(param)));
                }
            }

            return new FunctionCatalogGroup
            {
                DisplayName = $"Develop {panel.Name}",
                Key = $"LrDevelop{panel.Value}",
                Functions = new ObservableCollection<IFunctionFactory>(functions)
            };
        }

        private static IEnumerable<IFunctionFactory> CreateFunctionsForEnumParameter(ISettings settings, LrApi api, IList<IParameter> parameters)
        {
            var enumFunctions = new List<IFunctionFactory>();

            foreach (var param in parameters)
            {
                var valueType = param.GetType().GetGenericArguments()[0];
                if (valueType.BaseType == null || !valueType.BaseType.IsGenericType || typeof (ClassEnum<,>) != valueType.BaseType.GetGenericTypeDefinition())
                    continue;

                var allEnumsProperty = valueType.BaseType.GetProperty("AllEnums");
                var nameProperty = valueType.BaseType.GetProperty("Name");
                var valueProperty = valueType.BaseType.GetProperty("Value");
                var callSetValueMethod = valueType.BaseType.GetMethod("CallSetValue");

                var allEnums = (IEnumerable)allEnumsProperty.GetMethod.Invoke(null, null);
                foreach (var enumValue in allEnums)
                {
                    var name = nameProperty.GetMethod.Invoke(enumValue, null);
                    var value = valueProperty.GetMethod.Invoke(enumValue, null);

                    enumFunctions.Add(
                        new MethodFunctionFactory(settings, api, $"Set {param.DisplayName} to {name}",$"Set{param.Name}To{value}",
                            a => callSetValueMethod.Invoke(null, new[] {a, param, enumValue })));
                }
            }

            return enumFunctions;
        }

        private static IFunctionCatalogGroup CreateDevelopCropGroup(ISettings settings, LrApi api)
        {
            var functions = new List<IFunctionFactory>();
            
            // Change parameters
            foreach (var param in Parameters.CropParameters.AllParameters)
            {
                functions.Add(new ParameterFunctionFactory(settings, api, param));
            }

            // Reset parameter
            foreach (var param in Parameters.CropParameters.AllParameters)
            {
                functions.Add(new MethodFunctionFactory(settings, api, $"Reset {param.DisplayName} to default", $"ResetToDefault{param.Name}",
                    a =>
                    {
                        a.LrDevelopController.StopTracking();
                        a.LrDevelopController.ResetToDefault(param);
                    }));
            }

            return new FunctionCatalogGroup
            {
                DisplayName = "Develop Crop",
                Key = "LrDevelopCrop",
                Functions = new ObservableCollection<IFunctionFactory>(functions)
            };
        }

        private static IFunctionCatalogGroup CreateDevelopLocalizedGroup(ISettings settings, LrApi api)
        {
            var functions = new List<IFunctionFactory>();

            // Change parameters
            foreach (var param in Parameters.LocalizedAdjustmentsParameters.AllParameters)
            {
                functions.Add(new ParameterFunctionFactory(settings, api, param));
            }

            // Reset parameter
            foreach (var param in Parameters.LocalizedAdjustmentsParameters.AllParameters)
            {
                functions.Add(new MethodFunctionFactory(settings, api, $"Reset {param.DisplayName} to default", $"ResetToDefault{param.Name}",
                    a =>
                    {
                        a.LrDevelopController.StopTracking();
                        a.LrDevelopController.ResetToDefault(param);
                    }));
            }

            return new FunctionCatalogGroup
            {
                DisplayName = "Develop Localized",
                Key = "LrDevelopLocalized",
                Functions = new ObservableCollection<IFunctionFactory>(functions)
            };
        }
    }
}