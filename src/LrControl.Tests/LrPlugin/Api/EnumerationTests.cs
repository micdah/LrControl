using System.Linq;
using LrControl.Core.Util;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;
using LrControl.Tests.Mocks;
using Xunit;
using Xunit.Abstractions;

namespace LrControl.Tests.LrPlugin.Api
{
    public class EnumerationTests : TestSuite
    {
        public EnumerationTests(ITestOutputHelper output) : base(output)
        {
        }
        
        [Fact]
        public void Should_Return_All_Enumeration_Values_Statically()
        {
            var enums = TestIntegerEnumeration.GetAll();

            Assert.Equal(2, enums.Count);
            Assert.Contains(enums, e => TestIntegerEnumeration.Value1.Equals(e));
            Assert.Contains(enums, e => TestIntegerEnumeration.Value2.Equals(e));
        }

        [Fact]
        public void Should_Get_Enum_From_Value()
        {
            Assert.Same(TestIntegerEnumeration.Value1, TestIntegerEnumeration.GetEnumForValue(1));
            Assert.Same(TestIntegerEnumeration.Value2, TestIntegerEnumeration.GetEnumForValue(2));
            Assert.Null(TestIntegerEnumeration.GetEnumForValue(3));
        }

        [Fact]
        public void Test_IsTypeOf()
        {
            var @enum = TestIntegerEnumeration.Value1;
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

            IParameter p2 = new Parameter<TestIntegerEnumeration>("EnumerationTest", "Enumeration test");
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
            IClosedParameter<IEnumeration<int>> param = new ClosedParameter<TestIntegerEnumeration>(
                "EnumerationTest", "Enumeration test", TestIntegerEnumeration.GetAll());

            var enums = param.ValidValues.ToList();
            
            Assert.Equal(2, enums.Count);
            Assert.Contains(enums, e => TestIntegerEnumeration.Value1.Equals(e));
            Assert.Contains(enums, e => TestIntegerEnumeration.Value2.Equals(e));
        }
    }
}