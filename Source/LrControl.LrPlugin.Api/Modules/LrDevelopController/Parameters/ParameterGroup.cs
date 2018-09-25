using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LrControl.LrPlugin.Api.Modules.LrDevelopController.Parameters
{
    public abstract class ParameterGroup
    {
        private readonly ReadOnlyCollection<IParameter> _allParameters;

        protected ParameterGroup(string name, params IParameter[] parameters)
        {
            Name = name;
            _allParameters = new ReadOnlyCollection<IParameter>(parameters);
        }

        public string Name { get; }

        public IReadOnlyCollection<IParameter> AllParameters => _allParameters;
    }
}