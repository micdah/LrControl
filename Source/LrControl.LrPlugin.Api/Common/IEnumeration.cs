using System;

namespace LrControl.LrPlugin.Api.Common
{
    /// <summary>
    /// Defines a class-based enumeration type based on values of <typeparamref name="TValue"/>
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public interface IEnumeration<out TValue> : IComparable
        where TValue : IComparable
    {
        /// <summary>
        /// Name of enumeration
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// Value of enumeration (must be unique)
        /// </summary>
        TValue Value { get; }
    }
}