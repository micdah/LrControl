using System;
using System.Collections;
using System.Collections.Generic;
using LrControl.Utils;
using Xunit;
using Xunit.Abstractions;

namespace LrControl.Tests.Utils
{
    public class TypeExtensionsTests : TestSuite
    {
        public TypeExtensionsTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void List_IsTypeOf_List()
        {
            Assert.True(typeof(List<string>).IsTypeOf(typeof(List<string>)));
        }

        [Fact]
        public void List_Not_IsTypeOf_List()
        {            
            Assert.False(typeof(List<string>).IsTypeOf(typeof(List<Object>)));
        }
        
        [Fact]
        public void List_IsTypeOf_List_Generic()
        {
            Assert.True(typeof(List<string>).IsTypeOf(typeof(List<>)));
        }

        [Fact]
        public void List_IsTypeOf_IList()
        {
            Assert.True(typeof(List<string>).IsTypeOf(typeof(IList<string>)));
        }

        [Fact]
        public void List_Not_IsTypeOf_IList()
        {
            Assert.False(typeof(List<string>).IsTypeOf(typeof(IList<object>)));
        }

        [Fact]
        public void List_IsTypOf_IList_Generic()
        {
            Assert.True(typeof(List<string>).IsTypeOf(typeof(IList<>)));
        }

        [Fact]
        public void List_Generic_IsTypeOf_List_Generic()
        {
            Assert.True(typeof(List<>).IsTypeOf(typeof(List<>)));
        }

        [Fact]
        public void List_Generic_IsTypeOf_IList_Generic()
        {
            Assert.True(typeof(List<>).IsTypeOf(typeof(IList<>)));
        }

        [Fact]
        public void List_IsTypeOf_IList_Non_Generic()
        {
            Assert.True(typeof(List<string>).IsTypeOf(typeof(IList)));
        }

        [Fact]
        public void List_Generic_IsTypeOf_List_Non_Generic()
        {
            Assert.True(typeof(List<>).IsTypeOf(typeof(IList)));
        }

        [Fact]
        public void String_IsTypeOf_Object()
        {
            Assert.True(typeof(string).IsTypeOf(typeof(object)));
        }

        [Fact]
        public void Integer_Not_IsTypeOf_String()
        {
            Assert.False(typeof(int).IsTypeOf(typeof(string)));
        }

        [Fact]
        public void List_IsTypeOf_Enumerable()
        {
            Assert.True(typeof(List<string>).IsTypeOf(typeof(IEnumerable<object>)));
        }

        [Fact]
        public void Base_IsTypeOf_All_Base_And_Inherited_Types_Including_Generics()
        {
            Assert.True(typeof(Base).IsTypeOf(typeof(Base)), "Base <= Base");
            
            Assert.True(typeof(Base).IsTypeOf(typeof(Super<>)), "Base <= Super<>");
            Assert.True(typeof(Base).IsTypeOf(typeof(Super<int>)), "Base <= Super<int>");
            Assert.False(typeof(Base).IsTypeOf(typeof(Super<double>)), "Base != Super<double>");
            
            Assert.True(typeof(Base).IsTypeOf(typeof(IInterface<>)), "Base <= IInterface<>");
            Assert.True(typeof(Base).IsTypeOf(typeof(IInterface<int>)), "Base <= IInterface<int>");
            Assert.False(typeof(Base).IsTypeOf(typeof(IInterface<double>)), "Base != IInterface<double>");
        }

        class Base : Super<int>
        {
            
        }

        class Super<T> : IInterface<T>
        {
            
        }

        interface IInterface<T>
        {
            
        }
    }
}