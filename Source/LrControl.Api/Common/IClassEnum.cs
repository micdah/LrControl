namespace LrControl.Api.Common
{
    public interface IClassEnum<out TValue>
    {
        string Name { get; }
        TValue Value { get; }
    }
}