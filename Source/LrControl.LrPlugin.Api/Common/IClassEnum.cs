namespace LrControl.LrPlugin.Api.Common
{
    public interface IClassEnum<out TValue>
    {
        string Name { get; }
        TValue Value { get; }
    }
}