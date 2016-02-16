using System.Collections.Generic;
using System.Linq;

namespace micdah.LrControlApi.Common
{
    public abstract class Parameter<TParameter> : IParameter
        where TParameter : Parameter<TParameter>
    {
        private static readonly List<TParameter> AllParametersLookup = new List<TParameter>();

        protected Parameter(string name, string displayName)
        {
            DisplayName = displayName;
            Name = name;
        }

        public string Name { get; }
        public string DisplayName { get; }

        public override string ToString()
        {
            return DisplayName;
        }

        protected static void AddParameters(params IParameter[] parameters)
        {
            AllParametersLookup.AddRange(parameters.Cast<TParameter>());
        }
    }
}