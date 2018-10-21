using System;
using System.Runtime.CompilerServices;
using LrControl.LrPlugin.Api.Communication;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrControl;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;
using LrControl.LrPlugin.Api.Modules.LrDialogs;
using LrControl.LrPlugin.Api.Modules.LrSelection;
using LrControl.LrPlugin.Api.Modules.LrUndo;

[assembly:InternalsVisibleTo("LrControl.Tests")]
[assembly:InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace LrControl.LrPlugin.Api
{
    public delegate void ConnectionStatusHandler(bool connected, string apiVersion);

    public class LrApi : ILrApi
    {
        private readonly LrApplicationView _lrApplicationView;
        private readonly Modules.LrControl.LrControl _lrControl;
        private readonly LrDevelopController _lrDevelopController;
        private readonly LrDialogs _lrDialogs;
        private readonly LrSelection _lrSelection;
        private readonly LrUndo _lrUndo;
        private readonly PluginClient _pluginClient;

        public LrApi(int sendPort = 52008, int receivePort = 52009)
        {
            _pluginClient = new PluginClient(sendPort, receivePort);
            _lrControl = new Modules.LrControl.LrControl(new MessageProtocol(_pluginClient, nameof(Modules.LrControl.LrControl)));
            _lrDevelopController = new LrDevelopController(new MessageProtocol(_pluginClient, nameof(LrDevelopController)));
            _lrApplicationView = new LrApplicationView(new MessageProtocol(_pluginClient, nameof(LrApplicationView)));
            _lrDialogs = new LrDialogs(new MessageProtocol(_pluginClient, nameof(LrDialogs)));
            _lrSelection = new LrSelection(new MessageProtocol(_pluginClient, nameof(LrSelection)));
            _lrUndo = new LrUndo(new MessageProtocol(_pluginClient, nameof(LrUndo)));

            _pluginClient.Connection += PluginClientOnConnection;
            _pluginClient.ChangeMessage += parameterNames => _lrDevelopController.OnParametersChanged(parameterNames);
            _pluginClient.ModuleMessage += moduleName => _lrApplicationView.OnModuleChanged(moduleName);
            
            _pluginClient.Open();
        }

        public bool IsConnected => _pluginClient.IsConnected;

        public ILrControl LrControl => _lrControl;

        public ILrDevelopController LrDevelopController => _lrDevelopController;

        public ILrApplicationView LrApplicationView => _lrApplicationView;

        public ILrDialogs LrDialogs => _lrDialogs;
        public ILrSelection LrSelection => _lrSelection;

        public ILrUndo LrUndo => _lrUndo;

        public bool Connected { get; private set; }

        public string ApiVersion { get; private set; }

        public void Dispose()
        {
            _pluginClient.Dispose();
        }

        public event ConnectionStatusHandler ConnectionStatus;

        private void PluginClientOnConnection(bool connected)
        {
            if (connected)
            {
                if (LrControl.GetApiVersion(out var apiVersion))
                {
                    OnConnectionStatus(true, apiVersion);
                }
                else
                {
                    OnConnectionStatus(false, null);
                }
            }
            else
            {
                OnConnectionStatus(false, null);
            }
        }

        private void OnConnectionStatus(bool connected, string apiVersion)
        {
            Connected = connected;
            ApiVersion = apiVersion;

            ConnectionStatus?.Invoke(connected, apiVersion);
        }
    }
}