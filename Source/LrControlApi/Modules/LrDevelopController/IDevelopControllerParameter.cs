using micdah.LrControlApi.Common;

namespace micdah.LrControlApi.Modules.LrDevelopController
{
    public interface IDevelopControllerParameter<T> : IParameter<T>, IDevelopControllerParameter
    {
    }

    public interface IDevelopControllerParameter : IParameter
    {
    }
}