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
    }
}