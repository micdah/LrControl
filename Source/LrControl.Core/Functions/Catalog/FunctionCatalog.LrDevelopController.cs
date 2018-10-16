using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using LrControl.Core.Configurations;
using LrControl.Core.Functions.Factories;
using LrControl.Core.Util;
using LrControl.LrPlugin.Api;
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
            groups.Add(CreateDevelopPanelGroup(settings, api, Panel.ToneCurve, EnablePanelParameter.ToneCurve, Parameters.TonePanelParameters.AllParameters));
            groups.Add(CreateDevelopPanelGroup(settings, api, Panel.ColorAdjustment, EnablePanelParameter.ColorAdjustments, Parameters.MixerPanelParameters.AllParameters));
            groups.Add(CreateDevelopPanelGroup(settings, api, Panel.SplitToning, EnablePanelParameter.SplitToning, Parameters.SplitToningPanelParameters.AllParameters));
            groups.Add(CreateDevelopPanelGroup(settings, api, Panel.Detail, EnablePanelParameter.Detail, Parameters.DetailPanelParameters.AllParameters));
            groups.Add(CreateDevelopPanelGroup(settings, api, Panel.LensCorrections, EnablePanelParameter.LensCorrections, Parameters.LensCorrectionsPanelParameters.AllParameters));
            groups.Add(CreateDevelopPanelGroup(settings, api, Panel.Effects, EnablePanelParameter.Effects, Parameters.EffectsPanelParameters.AllParameters));
            groups.Add(CreateDevelopPanelGroup(settings, api, Panel.CameraCalibration, EnablePanelParameter.Calibration, Parameters.CalibratePanelParameters.AllParameters));
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

            foreach (var tool in Tool.GetAll())
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

        private static IFunctionCatalogGroup CreateDevelopPanelGroup(ISettings settings, LrApi api, Panel panel, IParameter<bool> enablePanelParameter, IReadOnlyCollection<IParameter> parameters)
        {
            var factories = new List<IFunctionFactory>();
            factories.AddRange(new []
            {
                new EnablePanelFunctionFactory(settings, api, panel, enablePanelParameter),
            });

            // Change parameter
            factories.AddRange(parameters
                .Where(p => p.GetType().IsTypeOf(typeof(IParameter<>)))
                .Select(p => new ParameterFunctionFactory(settings, api, p)));

            // Change enum parameter
            factories.AddRange(CreateFunctionsForEnumParameter(settings, api, parameters));

            // Reset parameter
            factories.AddRange(parameters.Select(param => new ResetParameterFunctionFactory(settings, api, param)));

            // Increment / Decrement
            foreach (var param in parameters)
            {
                if (!(param is IParameter<int>) && !(param is IParameter<double>)) continue;
                
                factories.Add(new UnaryOperatorParameterFunctionFactory(settings, api, param, UnaryOperation.Increment));
                factories.Add(new UnaryOperatorParameterFunctionFactory(settings, api, param, UnaryOperation.Decrement));
            }

            return new FunctionCatalogGroup
            {
                DisplayName = $"Develop {panel.Name}",
                Key = $"LrDevelop{panel.Value}",
                Functions = new ObservableCollection<IFunctionFactory>(factories)
            };
        }

        private static IEnumerable<IFunctionFactory> CreateFunctionsForEnumParameter(ISettings settings, LrApi api, IEnumerable<IParameter> parameters)
        {
            var enumFunctions = new List<IFunctionFactory>();

            void AddRange<TValue>(IEnumerationParameter<TValue> enumParam)
                where TValue : IComparable
            {
                enumFunctions.AddRange(
                    enumParam.Values.Select(value =>
                        new EnumerationParameterFunctionFactory<TValue>(settings, api, enumParam, value)));
            }

            foreach (var param in parameters)
            {
                if (!param.GetType().IsTypeOf(typeof(IEnumerationParameter<>)))
                    continue;

                switch (param)
                {
                    case IEnumerationParameter<double> doubleEnumParam:
                        AddRange(doubleEnumParam);
                        break;
                    case IEnumerationParameter<int> intEnumParam:
                        AddRange(intEnumParam);
                        break;
                    case IEnumerationParameter<bool> boolEnumParam:
                        AddRange(boolEnumParam);
                        break;
                    case IEnumerationParameter<string> stringEnumParam:
                        AddRange(stringEnumParam);
                        break;
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