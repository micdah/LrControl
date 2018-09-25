using System;

namespace LrControl.LrPlugin.Api.Common
{
    public interface IEnumeration<out TValue> : IComparable
        where TValue : IComparable
    {
        string Name { get; }
        TValue Value { get; }
    }
}