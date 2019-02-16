using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Tests.Mocks
{
    public static class TestParameter
    {
        public static readonly IParameter<int> IntegerParameter = 
            new Parameter<int>("IntegerParameter", "Integer parameter");
        
        public static readonly IParameter<double> DoubleParameter =
            new Parameter<double>("DoubleParameter", "Double parameter");

        public static readonly IParameter<bool> BooleanParameter =
            new Parameter<bool>("BooleanParameter", "Boolean Parameter");

        public static readonly IEnumerationParameter<int> IntegerEnumerationParameter =
            EnumerationParameter<int>.Create<TestIntegerEnumeration>(
                "TestIntegerEnumerationParameter)",
                "Test Integer Enumeration Parameter");

        public static readonly IEnumerationParameter<string> StringEnumerationParameter =
            EnumerationParameter<string>.Create<TestStringEnumeration>(
                "TestStringEnumerationParameter",
                "Test String Enumeration Parameter");
    }
}