using LrControl.LrPlugin.Api.Common;

namespace LrControl.Tests.Mocks
{
    public class TestIntegerEnumeration : Enumeration<TestIntegerEnumeration,int>
    {
        public static readonly TestIntegerEnumeration Value1 = new TestIntegerEnumeration(1, "Value 1");
        public static readonly TestIntegerEnumeration Value2 = new TestIntegerEnumeration(2, "Value 2");

        private TestIntegerEnumeration(int value, string name) : base(value, name)
        {
        }
    }
}