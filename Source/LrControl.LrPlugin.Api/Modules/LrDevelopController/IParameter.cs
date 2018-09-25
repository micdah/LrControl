using System;
using System.Collections.Generic;
using LrControl.LrPlugin.Api.Common;

namespace LrControl.LrPlugin.Api.Modules.LrDevelopController
{
    public interface IParameter
    {
        string Name { get; }
        string DisplayName { get; }
    }

    public interface IParameter<T> : IParameter
    {
    }

    public interface IEnumerationParameter<out TValue> : IParameter 
        where TValue : IComparable
    {
        IEnumerable<IEnumeration<TValue>> Values { get; }
    }
}