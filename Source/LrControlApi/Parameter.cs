using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace micdah.LrControlApi
{
    public abstract class Parameter<TParameter> : IParameter where TParameter : Parameter<TParameter>
    {
        private static readonly List<TParameter> AllParametersLookup = new List<TParameter>();

        protected Parameter(string name, string value, Type valueType)
        {
            Value = value;
            Name = name;
            ValueType = valueType;

            AllParametersLookup.Add((TParameter) this);
        }

        public string Value { get; }
        public string Name { get; }

        public override string ToString()
        {
            return Name;
        }

        public Type ValueType { get; }

        public static IList<TParameter> AllParameters => new ReadOnlyCollection<TParameter>(AllParametersLookup);
    }
}