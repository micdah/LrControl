using System;
using LrControl.Configurations;
using LrControl.Functions;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;
using Moq;
using Xunit;
using Xunit.Abstractions;
using Range = LrControl.LrPlugin.Api.Common.Range;

namespace LrControl.Tests.Functions
{
    public class ParameterFunctionTests : TestSuite
    {
        private static readonly IParameter<int> IntegerParameter =
            new Parameter<int>("IntegerParameter", "Integer parameter");

        private static readonly IParameter<double> DoubleParameter =
            new Parameter<double>("DoubleParameter", "Double parameter");
        
        private readonly Mock<ISettings> _settings;
        private readonly Mock<ILrApi> _lrApi;
        private readonly Mock<ILrDevelopController> _lrDevelopController;

        public ParameterFunctionTests(ITestOutputHelper output) : base(output)
        {
            _settings = new Mock<ISettings>();
            
            _lrDevelopController = new Mock<ILrDevelopController>();
            
            _lrApi = new Mock<ILrApi>();
            _lrApi
                .Setup(m => m.LrDevelopController)
                .Returns(_lrDevelopController.Object);
        }

        private ParameterFunction Create(IParameter parameter)
            => new ParameterFunction(
                _settings.Object,
                _lrApi.Object,
                "Test Function",
                "TestFunction",
                parameter);

        [Fact]
        public void Should_Only_Work_With_Double_And_Int_Parameters()
        {
            Assert.NotNull(Create(IntegerParameter));
            Assert.NotNull(Create(DoubleParameter));
            Assert.Throws<ArgumentException>(() =>
            {
                Create(new Parameter<string>("StringParameter", "String parameter"));
            });
        }

        [Fact]
        public void Should_Update_Parameter_Range_Before_Applying()
        {
            // Setup
            var func = Create(IntegerParameter);

            var parameterRange = new Range(0, 255);
            _lrDevelopController
                .Setup(m => m.GetRange(out parameterRange, IntegerParameter))
                .Returns(true)
                .Verifiable("Should get range for parameter before applying");

            // Test
            func.Apply(0, new Range(0, 128), Module.Develop, null);

            // Verify
            _lrDevelopController.Verify();
        }

        [Fact]
        public void Should_Set_Integer_Parameter_Based_On_Parameter_Range()
        {
            // Setup
            var func = Create(IntegerParameter);

            var parameterRange = new Range(0, 256);
            _lrDevelopController
                .Setup(m => m.GetRange(out parameterRange, IntegerParameter))
                .Returns(true);
            
            _lrDevelopController
                .Setup(m => m.SetValue(IntegerParameter, 128))
                .Returns(true)
                .Verifiable("Should set parameter value to 128");

            // Test
            func.Apply(50, new Range(0, 100), Module.Develop, null);

            // Verify
            _lrDevelopController.Verify();
        }
    }
}