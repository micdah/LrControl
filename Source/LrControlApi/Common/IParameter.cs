namespace micdah.LrControlApi.Common
{
    public interface IParameter
    {
        string Name { get; }
        string DisplayName { get; }
        string ToString();
    }

    public interface IParameter<out T> : IParameter
    {
    }
}