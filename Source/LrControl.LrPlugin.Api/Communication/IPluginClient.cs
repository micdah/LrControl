using System;
using LrControl.LrPlugin.Api.Communication.Sockets;

namespace LrControl.LrPlugin.Api.Communication
{
    internal interface IPluginClient : IDisposable
    {
        bool IsConnected { get; }
        event ConnectionHandler Connection;
        event ChangeMessageHandler ChangeMessage;
        event ModuleMessageHandler ModuleMessage;
        bool Open();
        bool SendMessage(string message, out string response);
    }
}