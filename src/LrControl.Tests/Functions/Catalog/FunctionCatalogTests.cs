using System;
using System.Collections.Generic;
using System.Linq;
using LrControl.Configurations;
using LrControl.Enums;
using LrControl.Functions.Catalog;
using LrControl.Functions.Factories;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;
using LrControl.LrPlugin.Api.Modules.LrDevelopController.Parameters;
using LrControl.Utils;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace LrControl.Tests.Functions.Catalog
{
    public class FunctionCatalogTests : TestSuite
    {
        private const string LrApplicationViewKey = "LrApplicationView";
        private const string LrDevelopKey = "LrDevelop";

        private readonly FunctionCatalog _catalog;

        public FunctionCatalogTests(ITestOutputHelper output) : base(output)
        {
            var settings = new Mock<ISettings>();
            var lrApi = new Mock<ILrApi>();
            _catalog = new FunctionCatalog(settings.Object, lrApi.Object);
        }

        private IFunctionCatalogGroup Group(string key)
        {
            var group = _catalog.Groups.SingleOrDefault(g => g.Key == key);
            Assert.NotNull(group);
            return group;
        }

        #region LrApplicationView

        [Fact]
        public void Should_Have_SwitchToModuleFunction_For_Each_Module()
        {
            var group = Group(LrApplicationViewKey);
            foreach (var module in Module.GetAll())
            {
                var factory = group.FunctionFactories
                    .OfType<SwitchToModuleFunctionFactory>()
                    .SingleOrDefault(f => f.Module == module);

                Assert.NotNull(factory);
                Assert.Equal(module, factory.Module);
            }
        }

        [Fact]
        public void Should_Have_ShowViewFunction_For_Each_PrimaryView()
        {
            var group = Group(LrApplicationViewKey);
            foreach (var primaryView in PrimaryView.GetAll())
            {
                var factory = group.FunctionFactories
                    .OfType<ShowViewFunctionFactory>()
                    .SingleOrDefault(f => f.PrimaryView == primaryView);

                Assert.NotNull(factory);
                Assert.Equal(primaryView, factory.PrimaryView);
            }
        }

        [Fact]
        public void Should_Have_ShowSecondaryViewFunction_For_Each_SecondaryView()
        {
            var group = Group(LrApplicationViewKey);
            foreach (var secondaryView in SecondaryView.GetAll())
            {
                var factory = group.FunctionFactories
                    .OfType<ShowSecondaryViewFunctionFactory>()
                    .SingleOrDefault(f => f.SecondaryView == secondaryView);

                Assert.NotNull(factory);
                Assert.Equal(secondaryView, factory.SecondaryView);
            }
        }

        [Fact]
        public void Should_Have_ToggleSecondaryDisplayFunction()
        {
            var group = Group(LrApplicationViewKey);
            var factory = group.FunctionFactories
                .OfType<ToggleSecondaryDisplayFunctionFactory>()
                .SingleOrDefault();

            Assert.NotNull(factory);
        }

        [Fact]
        public void Should_Have_ToggleSecondaryDisplayFullscreenFunction()
        {
            var group = Group(LrApplicationViewKey);
            var factory = group.FunctionFactories
                .OfType<ToggleSecondaryDisplayFullscreenFunctionFactory>()
                .SingleOrDefault();

            Assert.NotNull(factory);
        }

        [Fact]
        public void Should_Have_ToggleZoomFunction()
        {
            var group = Group(LrApplicationViewKey);
            var factory = group.FunctionFactories
                .OfType<ToggleZoomFunctionFactory>()
                .SingleOrDefault();

            Assert.NotNull(factory);
        }

        [Theory]
        [InlineData(Zoom.In), InlineData(Zoom.InSome), InlineData(Zoom.Out), InlineData(Zoom.OutSome)]
        public void Should_Have_ZoomInOutFunction(Zoom zoom)
        {
            var group = Group(LrApplicationViewKey);
            var factory = group.FunctionFactories
                .OfType<ZoomInOutFunctionFactory>()
                .SingleOrDefault(f => f.Zoom == zoom);

            Assert.NotNull(factory);
            Assert.Equal(zoom, factory.Zoom);
        }

        [Fact]
        public void Should_Have_ToggleDevelopBeforeAfterFunction()
        {
            var group = Group(LrApplicationViewKey);
            var factory = group.FunctionFactories
                .OfType<ToggleDevelopBeforeAfterFunctionFactory>()
                .SingleOrDefault();

            Assert.NotNull(factory);
        }

        #endregion

        #region LrDevelop

        [Theory]
        [InlineData("ResetAllDevelopAdjustments")]
        [InlineData("ResetBrushing")]
        [InlineData("ResetCircularGradient")]
        [InlineData("ResetCrop")]
        [InlineData("ResetGradient")]
        [InlineData("ResetRedEye")]
        [InlineData("ResetSpotRemoval")]
        public void Should_Have_MethodFunction(string key)
        {
            var group = Group(LrDevelopKey);
            var factory = group.FunctionFactories
                .OfType<MethodFunctionFactory>()
                .SingleOrDefault(f => f.Key == $"MethodFunction:{key}");
            Assert.NotNull(factory);
        }

        [Fact]
        public void Should_Have_MethodFunction_Per_Tool()
        {
            var group = Group(LrDevelopKey);
            foreach (var tool in Tool.GetAll())
            {
                var factory = group.FunctionFactories
                    .OfType<MethodFunctionFactory>()
                    .SingleOrDefault(f => f.Key == $"MethodFunction:SelectTool{tool.Value}");
                Assert.NotNull(factory);
            }
        }
        
        [Fact]
        public void Should_Have_RevealOrTogglePanelFunctionFactory_Per_Panel()
        {
            foreach (var panel in Panel.GetAll())
            {
                var group = Group($"{LrDevelopKey}{panel.Value}");
                var factory = group.FunctionFactories
                    .OfType<RevealOrTogglePanelFunctionFactory>()
                    .SingleOrDefault();

                Assert.NotNull(factory);
                Assert.Equal(panel, factory.Panel);
            }
        }

        [Fact]
        public void Should_Have_ParameterFunctionFactory_Per_Parameter_In_Panel()
        {
            foreach (var (panel, parameters) in new (Panel, IReadOnlyCollection<IParameter>)[]
            {
                (Panel.Basic, Parameters.AdjustPanelParameters.AllParameters),
                (Panel.ToneCurve, Parameters.TonePanelParameters.AllParameters),
                (Panel.ColorAdjustment, Parameters.MixerPanelParameters.AllParameters),
                (Panel.SplitToning, Parameters.SplitToningPanelParameters.AllParameters),
                (Panel.Detail, Parameters.DetailPanelParameters.AllParameters),
                (Panel.LensCorrections, Parameters.LensCorrectionsPanelParameters.AllParameters),
                (Panel.Effects, Parameters.EffectsPanelParameters.AllParameters),
                (Panel.CameraCalibration, Parameters.CalibratePanelParameters.AllParameters),
            })
            {
                var group = Group($"{LrDevelopKey}{panel.Value}");
                foreach (var parameter in parameters.Where(p => p.GetType().IsTypeOf(typeof(IParameter<>))))
                {
                    var factory = group.FunctionFactories
                        .OfType<ParameterFunctionFactory>()
                        .SingleOrDefault(f => ReferenceEquals(f.Parameter, parameter));
                    Assert.NotNull(factory);
                }
            }
        }

        [Fact]
        public void Should_Have_EnumerationParameterFunctionFactory_Per_EnumerationParameter_In_Panel()
        {
            foreach (var (panel, parameters) in new (Panel, IReadOnlyCollection<IParameter>)[]
            {
                (Panel.Basic, Parameters.AdjustPanelParameters.AllParameters),
                (Panel.ToneCurve, Parameters.TonePanelParameters.AllParameters),
                (Panel.ColorAdjustment, Parameters.MixerPanelParameters.AllParameters),
                (Panel.SplitToning, Parameters.SplitToningPanelParameters.AllParameters),
                (Panel.Detail, Parameters.DetailPanelParameters.AllParameters),
                (Panel.LensCorrections, Parameters.LensCorrectionsPanelParameters.AllParameters),
                (Panel.Effects, Parameters.EffectsPanelParameters.AllParameters),
                (Panel.CameraCalibration, Parameters.CalibratePanelParameters.AllParameters),
            })
            {
                var group = Group($"{LrDevelopKey}{panel.Value}");

                void VerifyAll<T>(IEnumerationParameter<T> enumerationParameter) where T : IComparable
                {
                    foreach (var value in enumerationParameter.Values)
                    {
                        var factory = group.FunctionFactories
                            .OfType<EnumerationParameterFunctionFactory<T>>()
                            .SingleOrDefault(f => f.Parameter == enumerationParameter &&
                                                  ReferenceEquals(f.Value, value));

                        Assert.NotNull(factory);
                    }
                }
                
                foreach (var parameter in parameters.Where(p => p.GetType().IsTypeOf(typeof(IEnumerationParameter<>))))
                {
                    switch (parameter)
                    {
                        case IEnumerationParameter<double> doubleEnumParam:
                            VerifyAll(doubleEnumParam);
                            break;
                        case IEnumerationParameter<int> intEnumParam:
                            VerifyAll(intEnumParam);
                            break;
                        case IEnumerationParameter<bool> boolEnumParam:
                            VerifyAll(boolEnumParam);
                            break;
                        case IEnumerationParameter<string> stringEnumParam:
                            VerifyAll(stringEnumParam);
                            break;
                    }
                }
            }
        }

        [Fact]
        public void Should_Have_ResetParameterFunctionFactory_Per_Parameter_In_Panel()
        {
            foreach (var (panel, parameters) in new (Panel, IReadOnlyCollection<IParameter>)[]
            {
                (Panel.Basic, Parameters.AdjustPanelParameters.AllParameters),
                (Panel.ToneCurve, Parameters.TonePanelParameters.AllParameters),
                (Panel.ColorAdjustment, Parameters.MixerPanelParameters.AllParameters),
                (Panel.SplitToning, Parameters.SplitToningPanelParameters.AllParameters),
                (Panel.Detail, Parameters.DetailPanelParameters.AllParameters),
                (Panel.LensCorrections, Parameters.LensCorrectionsPanelParameters.AllParameters),
                (Panel.Effects, Parameters.EffectsPanelParameters.AllParameters),
                (Panel.CameraCalibration, Parameters.CalibratePanelParameters.AllParameters),
            })
            {
                var group = Group($"{LrDevelopKey}{panel.Value}");
                foreach (var parameter in parameters)
                {
                    var factory = group.FunctionFactories
                        .OfType<ResetParameterFunctionFactory>()
                        .SingleOrDefault(f => ReferenceEquals(parameter, f.Parameter));
                    
                    Assert.NotNull(factory);
                }
            }
        }

        [Fact]
        public void Should_Have_UnaryOperatorParameterFunctionFactory_Per_Parameter_In_Panel()
        {
            foreach (var (panel, parameters) in new (Panel, IReadOnlyCollection<IParameter>)[]
            {
                (Panel.Basic, Parameters.AdjustPanelParameters.AllParameters),
                (Panel.ToneCurve, Parameters.TonePanelParameters.AllParameters),
                (Panel.ColorAdjustment, Parameters.MixerPanelParameters.AllParameters),
                (Panel.SplitToning, Parameters.SplitToningPanelParameters.AllParameters),
                (Panel.Detail, Parameters.DetailPanelParameters.AllParameters),
                (Panel.LensCorrections, Parameters.LensCorrectionsPanelParameters.AllParameters),
                (Panel.Effects, Parameters.EffectsPanelParameters.AllParameters),
                (Panel.CameraCalibration, Parameters.CalibratePanelParameters.AllParameters),
            })
            {
                var group = Group($"{LrDevelopKey}{panel.Value}");
                foreach (var parameter in parameters.Where(p => p is IParameter<int> || p is IParameter<double>).ToList())
                {
                    var factories = group.FunctionFactories
                        .OfType<UnaryOperatorParameterFunctionFactory>()
                        .Where(f => ReferenceEquals(f.Parameter, parameter))
                        .ToList();
                    
                    Assert.NotEmpty(factories);
                    Assert.Contains(factories, f => f.Operation == UnaryOperation.Increment);
                    Assert.Contains(factories, f => f.Operation == UnaryOperation.Decrement);
                }
            }
        }

        [Fact]
        public void Should_Have_ParameterFunctionFactory_Per_CropParameter()
        {
            var group = Group("LrDevelopCrop");
            foreach (var parameter in Parameters.CropParameters.AllParameters)
            {
                var factory = group.FunctionFactories
                    .OfType<ParameterFunctionFactory>()
                    .SingleOrDefault(f => ReferenceEquals(f.Parameter, parameter));
                
                Assert.NotNull(factory);
            }
        }

        [Fact]
        public void Should_Have_ResetParameterFunctionFactory_Per_CropParameter()
        {
            var group = Group("LrDevelopCrop");
            foreach (var parameter in Parameters.CropParameters.AllParameters)
            {
                var factory = group.FunctionFactories
                    .OfType<ResetParameterFunctionFactory>()
                    .SingleOrDefault(f => ReferenceEquals(f.Parameter, parameter));
                
                Assert.NotNull(factory);
            }
        }

        [Fact]
        public void Should_Have_ParameterFunctionFactory_Per_LocalizedAdjustmentsParameters()
        {
            var group = Group("LrDevelopLocalized");
            foreach (var parameter in Parameters.LocalizedAdjustmentsParameters.AllParameters)
            {
                var factory = group.FunctionFactories
                    .OfType<ParameterFunctionFactory>()
                    .SingleOrDefault(f => ReferenceEquals(f.Parameter, parameter));
                
                Assert.NotNull(factory);
            }
        }
        
        [Fact]
        public void Should_Have_ResetParameterFunctionFactory_Per_LocalizedAdjustmentsParameters()
        {
            var group = Group("LrDevelopLocalized");
            foreach (var parameter in Parameters.LocalizedAdjustmentsParameters.AllParameters)
            {
                var factory = group.FunctionFactories
                    .OfType<ResetParameterFunctionFactory>()
                    .SingleOrDefault(f => ReferenceEquals(f.Parameter, parameter));
                
                Assert.NotNull(factory);
            }
        }

        #endregion
    }
}