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

    public interface IParameter<out T> : IParameter
    {
    }

    public interface IClosedParameter<out T> : IParameter<T>
    {
        IEnumerable<T> ValidValues { get; }
    }

    public interface IEnumerationParameter<out TValue> : IParameter 
        where TValue : IComparable
    {
        IEnumerable<IEnumeration<TValue>> Values { get; }
    }
}