using System.Linq;
using LrControl.Core.Util;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;
using Xunit;

namespace LrControl.Tests
{
    public class EnumerationTests
    {
        [Fact]
        public void Should_Return_All_Enumeration_Values_Statically()
        {
            var enums = TestEnumeration.GetAll();

            Assert.Equal(2, enums.Count);
            Assert.Contains(enums, e => TestEnumeration.Value1.Equals(e));
            Assert.Contains(enums, e => TestEnumeration.Value2.Equals(e));
        }

        [Fact]
        public void Should_Get_Enum_From_Value()
        {
            Assert.Same(TestEnumeration.Value1, TestEnumeration.GetEnumForValue(1));
            Assert.Same(TestEnumeration.Value2, TestEnumeration.GetEnumForValue(2));
            Assert.Null(TestEnumeration.GetEnumForValue(3));
        }

        [Fact]
        public void Test_IsTypeOf()
        {
            var @enum = TestEnumeration.Value1;
            Assert.True(@enum.GetType().IsTypeOf(typeof(IEnumeration<int>)));
        }

        [Fact]
        public void Should_Pattern_Match_For_Covariant_Generic_Types()
        {
            IParameter p1 = new Parameter<int>("IntegerTest", "Integer test");
            switch (p1)
            {
                case IParameter<int> intParam:
                    Assert.NotNull(intParam);
                    break;
                default:
                    Assert.True(false, "Should have matched as a IParameter<int>");
                    break;
            }

            IParameter p2 = new Parameter<TestEnumeration>("EnumerationTest", "Enumeration test");
            switch (p2)
            {
                case IParameter<IEnumeration<int>> intEnumParam:
                    Assert.NotNull(intEnumParam);
                    break;
                default:
                    Assert.True(false, "Should have matched as a IParameter<IEnumeration<int>>");
                    break;
            }
        }

        [Fact]
        public void Closed_Parameters_Should_List_Available_Values_Only()
        {
            IClosedParameter<IEnumeration<int>> param = new ClosedParameter<TestEnumeration>(
                "EnumerationTest", "Enumeration test", TestEnumeration.GetAll());

            var enums = param.ValidValues.ToList();
            
            Assert.Equal(2, enums.Count);
            Assert.Contains(enums, e => TestEnumeration.Value1.Equals(e));
            Assert.Contains(enums, e => TestEnumeration.Value2.Equals(e));
        }

        class TestEnumeration : Enumeration<TestEnumeration,int>
        {
            public static readonly TestEnumeration Value1 = new TestEnumeration(1, "Value 1");
            public static readonly TestEnumeration Value2 = new TestEnumeration(2, "Value 2");

            private TestEnumeration(int value, string name) : base(value, name)
            {
            }
        }
    }
}