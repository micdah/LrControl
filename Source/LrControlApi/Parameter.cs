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
        }

        public string Value { get; }
        public string Name { get; }

        public override string ToString()
        {
            return Name;
        }

        public Type ValueType { get; }

        protected static void AddParameters(params TParameter[] parameters)
        {
            AllParametersLookup.AddRange(parameters);
        }
    }
}