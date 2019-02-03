using System.Collections.Generic;
using System.Linq;
using LrControl.Configurations;
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
        private static IEnumerable<IFunctionCatalogGroup> CreateDevelopGroups(ISettings settings, ILrApi api)
        {
            yield return CreateDevelopGroup(settings, api);
            yield return CreateDevelopPanelGroup(settings, api, Panel.Basic, null, Parameters.AdjustPanelParameters.AllParameters);
            yield return CreateDevelopPanelGroup(settings, api, Panel.ToneCurve, EnablePanelParameter.ToneCurve, Parameters.TonePanelParameters.AllParameters);
            yield return CreateDevelopPanelGroup(settings, api, Panel.ColorAdjustment, EnablePanelParameter.ColorAdjustments, Parameters.MixerPanelParameters.AllParameters);
            yield return CreateDevelopPanelGroup(settings, api, Panel.SplitToning, EnablePanelParameter.SplitToning, Parameters.SplitToningPanelParameters.AllParameters);
            yield return CreateDevelopPanelGroup(settings, api, Panel.Detail, EnablePanelParameter.Detail, Parameters.DetailPanelParameters.AllParameters);
            yield return CreateDevelopPanelGroup(settings, api, Panel.LensCorrections, EnablePanelParameter.LensCorrections, Parameters.LensCorrectionsPanelParameters.AllParameters);
            yield return CreateDevelopPanelGroup(settings, api, Panel.Effects, EnablePanelParameter.Effects, Parameters.EffectsPanelParameters.AllParameters);
            yield return CreateDevelopPanelGroup(settings, api, Panel.CameraCalibration, EnablePanelParameter.Calibration, Parameters.CalibratePanelParameters.AllParameters);
            yield return CreateDevelopCropGroup(settings, api);
            yield return CreateDevelopLocalizedGroup(settings, api);
        }

        private static IFunctionCatalogGroup CreateDevelopGroup(ISettings settings, ILrApi api)
        {
            IEnumerable<IFunctionFactory> CreateFactories()
            {
                yield return new MethodFunctionFactory(settings, api, "Reset all develop adjustments", "ResetAllDevelopAdjustments", a => a.LrDevelopController.ResetAllDevelopAdjustments());
                yield return new MethodFunctionFactory(settings, api, "Reset Adjustment Brush", "ResetBrushing",a => a.LrDevelopController.ResetBrushing());
                yield return new MethodFunctionFactory(settings, api, "Reset Radial Filter", "ResetCircularGradient",a => a.LrDevelopController.ResetCircularGradient());
                yield return new MethodFunctionFactory(settings, api, "Reset Crop", "ResetCrop",a => a.LrDevelopController.ResetCrop());
                yield return new MethodFunctionFactory(settings, api, "Reset Graduated Filter", "ResetGradient",a => a.LrDevelopController.ResetGradient());
                yield return new MethodFunctionFactory(settings, api, "Reset Red Eye Correction", "ResetRedEye",a => a.LrDevelopController.ResetRedEye());
                yield return new MethodFunctionFactory(settings, api, "Reset Spot Removal", "ResetSpotRemoval",a => a.LrDevelopController.ResetSpotRemoval());
                
                // Select Tool
                foreach (var tool in Tool.GetAll())
                {
                    yield return new MethodFunctionFactory(settings, api, 
                        $"Select Tool {tool.Name}",
                        $"SelectTool{tool.Value}",
                        a => a.LrDevelopController.SelectTool(tool));
                }
            }
            
            return new FunctionCatalogGroup
            {
                DisplayName = "Develop",
                Key = "LrDevelop",
                Functions = new List<IFunctionFactory>(CreateFactories())
            };
        }

        internal static IFunctionCatalogGroup CreateDevelopPanelGroup(ISettings settings, ILrApi api, Panel panel, IParameter<bool> enablePanelParameter, IReadOnlyCollection<IParameter> parameters)
        {
            IEnumerable<IFunctionFactory> CreateFactories()
            {
                yield return new EnablePanelFunctionFactory(settings, api, panel, enablePanelParameter);
                
                // Change parameter
                foreach (var parameter in parameters.Where(p => p.GetType().IsTypeOf(typeof(IParameter<>))))
                    yield return new ParameterFunctionFactory(settings, api, parameter);
                
                // Change Enumeration parameter
                foreach (var param in parameters)
                {
                    if (!param.GetType().IsTypeOf(typeof(IEnumerationParameter<>)))
                        continue;

                    switch (param)
                    {
                        case IEnumerationParameter<double> doubleEnumParam:
                            foreach (var value in doubleEnumParam.Values)
                                yield return new EnumerationParameterFunctionFactory<double>(settings, api, doubleEnumParam, value);
                            break;
                        case IEnumerationParameter<int> intEnumParam:
                            foreach (var value in intEnumParam.Values)
                                yield return new EnumerationParameterFunctionFactory<int>(settings, api, intEnumParam, value);
                            break;
                        case IEnumerationParameter<bool> boolEnumParam:
                            foreach (var value in boolEnumParam.Values)
                                yield return new EnumerationParameterFunctionFactory<bool>(settings, api, boolEnumParam, value);
                            break;
                        case IEnumerationParameter<string> stringEnumParam:
                            foreach (var value in stringEnumParam.Values)
                                yield return new EnumerationParameterFunctionFactory<string>(settings, api, stringEnumParam, value);
                            break;
                    }
                }
                
                // Reset parameter
                foreach (var param in parameters)
                    yield return new ResetParameterFunctionFactory(settings, api, param);
                
                // Increment / Decrement
                foreach (var param in parameters)
                {
                    if (!(param is IParameter<int>) && !(param is IParameter<double>)) continue;
                
                    yield return new UnaryOperatorParameterFunctionFactory(settings, api, param, UnaryOperation.Increment);
                    yield return new UnaryOperatorParameterFunctionFactory(settings, api, param, UnaryOperation.Decrement);
                }
            }

            return new FunctionCatalogGroup
            {
                DisplayName = $"Develop {panel.Name}",
                Key = $"LrDevelop{panel.Value}",
                Functions = new List<IFunctionFactory>(CreateFactories())
            };
        }

        private static IFunctionCatalogGroup CreateDevelopCropGroup(ISettings settings, ILrApi api)
        {
            IEnumerable<IFunctionFactory> CreateFactories()
            {
                // Change parameters
                foreach (var param in Parameters.CropParameters.AllParameters)
                    yield return new ParameterFunctionFactory(settings, api, param);

                // Reset parameter
                foreach (var param in Parameters.CropParameters.AllParameters)
                    yield return new ResetParameterFunctionFactory(settings, api, param);
            }
            
            return new FunctionCatalogGroup
            {
                DisplayName = "Develop Crop",
                Key = "LrDevelopCrop",
                Functions = new List<IFunctionFactory>(CreateFactories())
            };
        }

        private static IFunctionCatalogGroup CreateDevelopLocalizedGroup(ISettings settings, ILrApi api)
        {
            IEnumerable<IFunctionFactory> CreateFactories()
            {
                // Change parameters
                foreach (var param in Parameters.LocalizedAdjustmentsParameters.AllParameters)
                    yield return new ParameterFunctionFactory(settings, api, param);

                // Reset parameter
                foreach (var param in Parameters.LocalizedAdjustmentsParameters.AllParameters)
                    yield return new ResetParameterFunctionFactory(settings, api, param);
            }

            return new FunctionCatalogGroup
            {
                DisplayName = "Develop Localized",
                Key = "LrDevelopLocalized",
                Functions = new List<IFunctionFactory>(CreateFactories())
            };
        }
    }
}