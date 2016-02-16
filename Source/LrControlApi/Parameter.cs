using System;
using System.Collections.Generic;

namespace micdah.LrControlApi
{
    public abstract class Parameter<TParameter> : IParameter 
        where TParameter : Parameter<TParameter>
    {
        private static readonly List<TParameter> AllParametersLookup = new List<TParameter>();

        protected Parameter(string name, string displayName, Type valueType)
        {
            DisplayName = displayName;
            Name = name;
            ValueType = valueType;
        }

        public string Name { get; }
        public string DisplayName { get; }

        public override string ToString()
        {
            return DisplayName;
        }

        public Type ValueType { get; }

        protected static void AddParameters(params TParameter[] parameters)
        {
            AllParametersLookup.AddRange(parameters);
        }
    }
}