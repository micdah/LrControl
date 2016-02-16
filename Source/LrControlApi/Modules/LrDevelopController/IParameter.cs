using micdah.LrControlApi.Common;

namespace micdah.LrControlApi.Modules.LrDevelopController
{
    public interface IParameter<out T> : IParameter
    {
    }

    public interface IParameter
    {
        string Name { get; }
        string DisplayName { get; }
        string ToString();
    }

    internal class Parameter<T> : IParameter<T>
    {
        public Parameter(string name, string displayName)
        {
            Name = name;
            DisplayName = displayName;
        }

        public string Name { get; }
        public string DisplayName { get; }
    }
}