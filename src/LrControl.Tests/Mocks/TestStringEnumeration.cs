using LrControl.LrPlugin.Api.Common;

namespace LrControl.Tests.Mocks
{
    public class TestStringEnumeration : Enumeration<TestStringEnumeration, string>
    {
        public static readonly TestStringEnumeration Value1 = new TestStringEnumeration("Value1", "Value 1");
        public static readonly TestStringEnumeration Value2 = new TestStringEnumeration("Value2", "Value 2");

        private TestStringEnumeration(string value, string name) : base(value, name)
        {
        }
    }
}