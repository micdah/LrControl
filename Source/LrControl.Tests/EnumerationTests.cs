using LrControl.Core.Util;
using LrControl.LrPlugin.Api.Common;
using Xunit;

namespace LrControl.Tests
{
    public class EnumerationTests
    {
        [Fact]
        public void Should_Return_All_Enumeration_Values()
        {
            var enums = TestEnumeration.GetAll();

            Assert.Equal(2, enums.Count);
            Assert.Contains(enums, e => TestEnumeration.Value1.Equals(e));
            Assert.Contains(enums, e => TestEnumeration.Value2.Equals(e));
        }

        [Fact]
        public void Test_IsTypeOf()
        {
            var @enum = TestEnumeration.Value1;
            Assert.True(@enum.GetType().IsTypeOf(typeof(IEnumeration<int>)));
        }

        class TestEnumeration : Enumeration<TestEnumeration,int>, IEnumeration<int>
        {
            public static readonly TestEnumeration Value1 = new TestEnumeration(1, "Value 1");
            public static readonly TestEnumeration Value2 = new TestEnumeration(2, "Value 2");

            private TestEnumeration(int value, string name) : base(value, name)
            {
            }
        }
    }
}