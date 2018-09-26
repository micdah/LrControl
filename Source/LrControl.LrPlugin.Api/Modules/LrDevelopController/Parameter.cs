using System.Collections.Generic;
using System.Linq;

namespace LrControl.LrPlugin.Api.Modules.LrDevelopController
{
    public class Parameter<T> : IParameter<T>
    {
        public Parameter(string name, string displayName)
        {
            Name = name;
            DisplayName = displayName;
        }

        public string Name { get; }
        public string DisplayName { get; }

        private bool Equals(IParameter other)
        {
            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Parameter<T>) obj);
        }

        public override int GetHashCode()
        {
            return Name?.GetHashCode() ?? 0;
        }
    }

    public class ClosedParameter<T> : Parameter<T>, IClosedParameter<T>
    {
        private readonly List<T> _validValues;
        
        public ClosedParameter(string name, string displayName, IEnumerable<T> validValues) : base(name, displayName)
        {
            _validValues = validValues.ToList();
        }

        public IEnumerable<T> ValidValues => _validValues;
    }
}