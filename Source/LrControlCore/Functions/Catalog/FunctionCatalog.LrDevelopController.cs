using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using LrControlCore.Functions.Factories;
using micdah.LrControlApi;
using micdah.LrControlApi.Common;
using micdah.LrControlApi.Modules.LrDevelopController;
using micdah.LrControlApi.Modules.LrDevelopController.Parameters;
using Panel = micdah.LrControlApi.Modules.LrDevelopController.Panel;

namespace LrControlCore.Functions.Catalog
{
    public partial class FunctionCatalog
    {
        private static IEnumerable<FunctionCatalogGroup> CreateDevelopGroups(LrApi api)
        {
            var groups = new List<FunctionCatalogGroup>();
            groups.Add(CreateDevelopGroup(api));
            groups.Add(CreateDevelopPanelGroup(api, Panel.Basic, null, Parameters.AdjustPanelParameters.AllParameters));
            groups.Add(CreateDevelopPanelGroup(api, Panel.ToneCurve, Parameters.EnablePanelParameters.ToneCurve, Parameters.TonePanelParameters.AllParameters));
            groups.Add(CreateDevelopPanelGroup(api, Panel.ColorAdjustment, Parameters.EnablePanelParameters.ColorAdjustments, Parameters.MixerPanelParameters.AllParameters));
            groups.Add(CreateDevelopPanelGroup(api, Panel.SplitToning, Parameters.EnablePanelParameters.SplitToning, Parameters.SplitToningPanelParameters.AllParameters));
            groups.Add(CreateDevelopPanelGroup(api, Panel.Detail, Parameters.EnablePanelParameters.Detail, Parameters.DetailPanelParameters.AllParameters));
            groups.Add(CreateDevelopPanelGroup(api, Panel.LensCorrections, Parameters.EnablePanelParameters.LensCorrections, Parameters.LensCorrectionsPanelParameters.AllParameters));
            groups.Add(CreateDevelopPanelGroup(api, Panel.Effects, Parameters.EnablePanelParameters.Effects, Parameters.EffectsPanelParameters.AllParameters));
            groups.Add(CreateDevelopPanelGroup(api, Panel.CameraCalibration, Parameters.EnablePanelParameters.Calibration, Parameters.CalibratePanelParameters.AllParameters));
            groups.Add(CreateDevelopCropGroup(api));
            groups.Add(CreateDevelopLocalizedGroup(api));

            return groups;
        }

        private static FunctionCatalogGroup CreateDevelopGroup(LrApi api)
        {
            var functions = new List<IFunctionFactory>();
            functions.AddRange(new []
            {
                new MethodFunctionFactory(api, "Reset all develop adjustments", "ResetAllDevelopAdjustments", a => a.LrDevelopController.ResetAllDevelopAdjustments()),
                new MethodFunctionFactory(api, "Reset Adjustment Brush", "ResetBrushing",a => a.LrDevelopController.ResetBrushing()),
                new MethodFunctionFactory(api, "Reset Radial Filter", "ResetCircularGradient",a => a.LrDevelopController.ResetCircularGradient()),
                new MethodFunctionFactory(api, "Reset Crop", "ResetCrop",a => a.LrDevelopController.ResetCrop()),
                new MethodFunctionFactory(api, "Reset Graduated Filter", "ResetGradient",a => a.LrDevelopController.ResetGradient()),
                new MethodFunctionFactory(api, "Reset Red Eye Correction", "ResetRedEye",a => a.LrDevelopController.ResetRedEye()),
                new MethodFunctionFactory(api, "Reset Spot Removal", "ResetSpotRemoval",a => a.LrDevelopController.ResetSpotRemoval()),
            });

            foreach (var tool in Tool.AllEnums)
            {
                functions.Add(new MethodFunctionFactory(api, $"Select Tool {tool.Name}", $"SelectTool{tool.Value}",
                    a => a.LrDevelopController.SelectTool(tool)));
            }

            return new FunctionCatalogGroup
            {
                DisplayName = "Develop",
                Key = "LrDevelop",
                Functions = new ObservableCollection<IFunctionFactory>(functions)
            };
        }

        private static FunctionCatalogGroup CreateDevelopPanelGroup(LrApi api, Panel panel, IParameter<bool> enablePanelParameter, IList<IParameter> parameters)
        {
            var functions = new List<IFunctionFactory>();
            functions.AddRange(new []
            {
                new EnablePanelFunctionFactory(api, panel, enablePanelParameter),
            });

            // Change parameter
            foreach (var param in parameters)
            {
                if (param is IParameter<int> || param is IParameter<double>)
                {
                    functions.Add(new ParameterFunctionFactory(api, param));
                }
            }

            // Change enum parameter
            functions.AddRange(CreateFunctionsForEnumParameter(api, parameters));

            // Reset parameter
            foreach (var param in parameters)
            {
                functions.Add(new MethodFunctionFactory(api, $"Reset {param.DisplayName} to default", $"ResetToDefault{param.Name}",
                    a =>
                    {
                        a.LrDevelopController.StopTracking();
                        a.LrDevelopController.ResetToDefault(param);
                    }));
            }


            // Toggle parameter
            functions.AddRange(parameters
                .OfType<IParameter<bool>>()
                .Select(parameter => new ToggleParameterFunctionFactory(api, parameter)));

            // Increment / Decrement
            foreach (var param in parameters)
            {
                if (param is IParameter<int> || param is IParameter<double>)
                {
                    functions.Add(new MethodFunctionFactory(api, $"Increment {param.DisplayName}", $"Increment{param.Name}", 
                        a => a.LrDevelopController.Increment(param)));
                    functions.Add(new MethodFunctionFactory(api, $"Decrement {param.DisplayName}", $"Decrement{param.Name}",
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

        private static IEnumerable<IFunctionFactory> CreateFunctionsForEnumParameter(LrApi api, IList<IParameter> parameters)
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
                        new MethodFunctionFactory(api, $"Set {param.DisplayName} to {name}",$"Set{param.Name}To{value}",
                            a => callSetValueMethod.Invoke(null, new[] {a, param, enumValue })));
                }
            }

            return enumFunctions;
        }

        private static FunctionCatalogGroup CreateDevelopCropGroup(LrApi api)
        {
            var functions = new List<IFunctionFactory>();
            
            // Change parameters
            foreach (var param in Parameters.CropParameters.AllParameters)
            {
                functions.Add(new ParameterFunctionFactory(api, param));
            }

            // Reset parameter
            foreach (var param in Parameters.CropParameters.AllParameters)
            {
                functions.Add(new MethodFunctionFactory(api, $"Reset {param.DisplayName} to default", $"ResetToDefault{param.Name}",
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

        private static FunctionCatalogGroup CreateDevelopLocalizedGroup(LrApi api)
        {
            var functions = new List<IFunctionFactory>();

            // Change parameters
            foreach (var param in Parameters.LocalizedAdjustmentsParameters.AllParameters)
            {
                functions.Add(new ParameterFunctionFactory(api, param));
            }

            // Reset parameter
            foreach (var param in Parameters.LocalizedAdjustmentsParameters.AllParameters)
            {
                functions.Add(new MethodFunctionFactory(api, $"Reset {param.DisplayName} to default", $"ResetToDefault{param.Name}",
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