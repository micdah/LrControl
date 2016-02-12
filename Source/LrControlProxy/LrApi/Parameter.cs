using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LrControlProxy.LrApi
{
    public abstract class Parameter<TParameter> : IParameter where TParameter : Parameter<TParameter>
    {
        private static readonly List<TParameter> AllParametersLookup = new List<TParameter>();

        protected Parameter(string name, string displayName, Type valueType)
        {
            Name = name;
            DisplayName = displayName;
            ValueType = valueType;

            AllParametersLookup.Add((TParameter) this);
        }

        public string Name { get; }
        public string DisplayName { get; }

        public override string ToString()
        {
            return DisplayName;
        }

        public Type ValueType { get; }

        public static IList<TParameter> AllParameters => new ReadOnlyCollection<TParameter>(AllParametersLookup);
    }
}