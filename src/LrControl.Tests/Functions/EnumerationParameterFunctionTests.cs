using System;
using System.Linq;
using LrControl.Functions;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;
using LrControl.Tests.Devices;
using LrControl.Tests.Mocks;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace LrControl.Tests.Functions
{
    public class EnumerationParameterFunctionTests : ProfileManagerTestSuite
    {
        public EnumerationParameterFunctionTests(ITestOutputHelper output) : base(output)
        {
        }

        private IFunction Create<T>(IEnumerationParameter<T> parameter, IEnumeration<T> value) where T : IComparable
            => new EnumerationParameterFunction<T>(
                Settings.Object,
                LrApi.Object,
                "Test Function",
                "TestFunction",
                parameter,
                value);

        [Fact]
        public void Should_Set_Enumeration_Parameter_When_Applied()
        {
            // Setup
            var parameter = TestParameter.IntegerEnumerationParameter;
            var value = parameter.Values.First();
            var func = Create(parameter, value);

            ProfileManager.AssignFunction(DefaultModule, Id1, func);

            LrDevelopController
                .Setup(m => m.SetValue(parameter, value))
                .Returns(true)
                .Verifiable();

            // Test
            ControllerInput(Id1, Range1.Maximum);

            // Verify
            LrDevelopController.Verify();
        }

        [Fact]
        public void Should_Only_Apply_When_Controller_Is_At_Maximum_Value()
        {
            // Setup
            var parameter = TestParameter.IntegerEnumerationParameter;
            var value = parameter.Values.First();
            var func = Create(parameter, value);

            ProfileManager.AssignFunction(DefaultModule, Id1, func);

            // Test
            ControllerInput(Id1, Range1.Maximum - 0.1d);

            // Verify
            LrDevelopController
                .Verify(m => m.SetValue(parameter, It.IsAny<IEnumeration<int>>()), Times.Never);
        }
    }
}