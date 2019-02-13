using System;
using LrControl.LrPlugin.Api.Common;

namespace LrControl.Tests.Mocks
{
    public class TestEnumeration<T> : Enumeration<TestEnumeration<T>, T> where T : IComparable
    {
        public TestEnumeration(T value, string name) : base(value, name)
        {
        }
    }
}