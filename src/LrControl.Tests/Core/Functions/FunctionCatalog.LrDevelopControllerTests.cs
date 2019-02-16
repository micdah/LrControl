using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using LrControl.Configurations;
using LrControl.Core.Functions.Catalog;
using LrControl.Core.Functions.Factories;
using LrControl.Enums;
using LrControl.Functions.Factories;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;
using LrControl.Tests.Mocks;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace LrControl.Tests.Core.Functions
{
    [SuppressMessage("ReSharper", "CollectionNeverUpdated.Local")]
    public class FunctionCatalogLrDevelopControllerTests : TestSuite
    {
        private readonly Mock<ISettings> _settingsMock;
        private readonly Mock<ILrApi> _apiMock;
        private readonly Panel _panel;
        private readonly List<IParameter> _parameters;
        private readonly IFunctionCatalogGroup _group;

        public FunctionCatalogLrDevelopControllerTests(ITestOutputHelper output) : base(output)
        {
            _settingsMock = new Mock<ISettings>();
            _apiMock = new Mock<ILrApi>();

            // Setup
            _panel = Panel.Basic;
            _parameters = new List<IParameter>
            {
                IntParameter,
                DoubleParameter,
                BoolParameter,
                StringParameter,
                EnumerationParameter
            };

            // Test
            _group = FunctionCatalog.CreateDevelopPanelGroup(Settings, Api, _panel, _parameters);
        }

        private ISettings Settings => _settingsMock.Object;
        private ILrApi Api => _apiMock.Object;

        [Fact]
        public void Should_Add_EnablePanelFunction()
        {
            // Verify
            var factory = _group.Functions
                .OfType<RevealOrTogglePanelFunctionFactory>()
                .SingleOrDefault();
            Assert.NotNull(factory);
            Assert.Equal(_panel, factory.Panel);
        }

        [Fact]
        public void Should_Add_ParameterFunction_For_Each_Parameters()
        {
            // Verify
            void Verify(IParameter parameter)
            {
                var factory = _group.Functions
                    .OfType<ParameterFunctionFactory>()
                    .SingleOrDefault(x => parameter.Equals(x.Parameter));
                Assert.NotNull(factory);
            }

            var parameters = _parameters
                .Where(x => !EnumerationParameter.Equals(x))
                .ToList();
            
            Assert.NotEmpty(parameters);
            foreach (var @param in parameters)
            {
                Verify(@param);
            }
        }

        [Fact]
        public void Should_Not_Add_ParameterFunction_For_EnumerationParameter()
        {
            // Verify
            Assert.DoesNotContain(_group.Functions, x =>
                x is ParameterFunctionFactory factory &&
                EnumerationParameter.Equals(factory.Parameter));
        }

        [Fact]
        public void Should_Add_Reset_Parameter_Functions()
        {
            // Verify
            void Verify(IParameter parameter)
            {
                var factory = _group.Functions
                    .OfType<ResetParameterFunctionFactory>()
                    .SingleOrDefault(x => parameter.Equals(x.Parameter));
                Assert.NotNull(factory);
            }

            foreach (var @param in _parameters)
                Verify(@param);
        }

        [Fact]
        public void Should_Add_Increment_And_Decremenet_Functions_For_Number_Parameters()
        {
            // Verify
            void Verify(IParameter parameter)
            {
                var factories = _group.Functions
                    .OfType<UnaryOperatorParameterFunctionFactory>()
                    .Where(x => parameter.Equals(x.Parameter))
                    .ToList();

                Assert.Contains(factories, x => x.Operation == UnaryOperation.Increment);
                Assert.Contains(factories, x => x.Operation == UnaryOperation.Decrement);
            }
            
            var parameters = _parameters
                .Where(x => x is IParameter<int> || x is IParameter<double>)
                .ToList();
            
            Assert.NotEmpty(parameters);
            foreach (var @param in parameters)
                Verify(@param);
        }
        
        // TODO Enumeration parameters

        private static readonly IParameter<int> IntParameter =
            new Parameter<int>("IntParameter", "Int parameter");

        private static readonly IParameter<double> DoubleParameter =
            new Parameter<double>("DoubleParameter", "Double parameter");

        private static readonly IParameter<bool> BoolParameter =
            new Parameter<bool>("BoolParameter", "Bool parameter");

        private static readonly IParameter<string> StringParameter =
            new Parameter<string>("StringParameter", "String parameter");

        private static readonly IEnumerationParameter<string> EnumerationParameter =
            EnumerationParameter<string>.Create<TestStringEnumeration>("TestParameter", "Test parameter");
    }
}