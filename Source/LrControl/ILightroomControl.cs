using LrControl.LrPlugin.Api.Common;

namespace LrControl
{
    public interface ILightroomControl
    {
        void SetValue(int value, Range valueRange);
    }
}