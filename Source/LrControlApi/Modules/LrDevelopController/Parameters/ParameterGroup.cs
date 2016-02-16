using System.Collections.Generic;

namespace micdah.LrControlApi.Modules.LrDevelopController.Parameters
{

    public abstract class ParameterGroup
    {
        private readonly List<IParameter> _allParameters = new List<IParameter>();

        public IList<IParameter> AllParameters => _allParameters.AsReadOnly();

        protected void AddParameters(params IParameter[] parameters)
        {
            _allParameters.AddRange(parameters);
        }
    }
}