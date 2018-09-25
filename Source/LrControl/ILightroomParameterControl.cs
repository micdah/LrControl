using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl
{
    public interface ILightroomParameterControl : ILightroomControl
    {
        IParameter Parameter { get; }
        int GetValue(Range valueRange);
    }
}