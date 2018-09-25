using System;
using System.Collections.Generic;
using System.Linq;
using LrControl.LrPlugin.Api.Common;

namespace LrControl.LrPlugin.Api.Modules.LrDevelopController
{
    internal class EnumerationParameter<TValue> : IEnumerationParameter<TValue>
        where TValue : IComparable
    {
        private readonly ICollection<IEnumeration<TValue>> _values;

        public static EnumerationParameter<TValue> Create<TEnum>(string name, string displayName) 
            where TEnum : Enumeration<TEnum, TValue>
        {
            var enums = Enumeration<TEnum, TValue>.GetAll().Cast<IEnumeration<TValue>>();
            return new EnumerationParameter<TValue>(name, displayName, enums);
        }
        
        private EnumerationParameter(string name, string displayName, IEnumerable<IEnumeration<TValue>> values)
        {
            Name = name;
            DisplayName = displayName;
            _values = values.ToList();
        }
        
        public string Name { get; }
        public string DisplayName { get; }
        public IEnumerable<IEnumeration<TValue>> Values => _values;
    }
}