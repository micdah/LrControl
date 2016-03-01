namespace micdah.LrControlApi.Modules.LrDevelopController
{
    internal class Parameter<T> : IParameter<T>
    {
        public Parameter(string name, string displayName)
        {
            Name = name;
            DisplayName = displayName;
        }

        public string Name { get; }
        public string DisplayName { get; }

        private bool Equals(Parameter<T> other)
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
}