using System.Collections.Generic;

namespace micdah.LrControlApi.Modules.LrDevelopController
{
    public abstract class ParameterGroup<TParameter>
        where TParameter : ParameterGroup<TParameter>
    {
        // ReSharper disable once StaticMemberInGenericType
        private static readonly List<IParameter> AllParametersLookup = new List<IParameter>();

        public static IList<IParameter> AllParameters => AllParametersLookup.AsReadOnly();

        protected static void AddParameters(params IParameter[] parameters)
        {
            AllParametersLookup.AddRange(parameters);
        }
    }
}