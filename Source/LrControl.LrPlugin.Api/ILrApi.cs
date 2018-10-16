using System;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrControl;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;
using LrControl.LrPlugin.Api.Modules.LrDialogs;
using LrControl.LrPlugin.Api.Modules.LrSelection;
using LrControl.LrPlugin.Api.Modules.LrUndo;

namespace LrControl.LrPlugin.Api
{
    public interface ILrApi : IDisposable
    {
        bool IsConnected { get; }
        ILrControl LrControl { get; }
        ILrDevelopController LrDevelopController { get; }
        ILrApplicationView LrApplicationView { get; }
        ILrDialogs LrDialogs { get; }
        ILrSelection LrSelection { get; }
        ILrUndo LrUndo { get; }
        bool Connected { get; }
        string ApiVersion { get; }
        event ConnectionStatusHandler ConnectionStatus;
    }
}