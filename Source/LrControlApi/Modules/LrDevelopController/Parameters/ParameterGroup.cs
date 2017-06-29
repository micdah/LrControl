using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LrControl.Api.Modules.LrDevelopController.Parameters
{
    public abstract class ParameterGroup
    {
        private ReadOnlyCollection<IParameter> _allParameters;

        protected ParameterGroup(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public IList<IParameter> AllParameters => _allParameters;

        protected void AddParameters(params IParameter[] parameters)
        {
            _allParameters = new ReadOnlyCollection<IParameter>(parameters);
        }
    }
}