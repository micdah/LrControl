using System;
using LrControl.Api.Communication;
using LrControl.Api.Modules.LrApplicationView;
using LrControl.Api.Modules.LrControl;
using LrControl.Api.Modules.LrDevelopController;
using LrControl.Api.Modules.LrDialogs;
using LrControl.Api.Modules.LrSelection;
using LrControl.Api.Modules.LrUndo;

namespace LrControl.Api
{
    public delegate void ConnectionStatusHandler(bool connected, string apiVersion);

    public class LrApi : IDisposable
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
            _lrControl = new Modules.LrControl.LrControl(new MessageProtocol<Modules.LrControl.LrControl>(_pluginClient));
            _lrDevelopController = new LrDevelopController(new MessageProtocol<LrDevelopController>(_pluginClient));
            _lrApplicationView = new LrApplicationView(new MessageProtocol<LrApplicationView>(_pluginClient));
            _lrDialogs = new LrDialogs(new MessageProtocol<LrDialogs>(_pluginClient));
            _lrSelection = new LrSelection(new MessageProtocol<LrSelection>(_pluginClient));
            _lrUndo = new LrUndo(new MessageProtocol<LrUndo>(_pluginClient));

            _pluginClient.Connection += PluginClientOnConnection;
            _pluginClient.ChangeMessage += name => _lrDevelopController.OnParameterChanged(name);
            _pluginClient.ModuleMessage += name => _lrApplicationView.OnModuleChanged(name);
            
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
            _pluginClient.Close();
        }

        public event ConnectionStatusHandler ConnectionStatus;

        private void PluginClientOnConnection(bool connected)
        {
            if (connected)
            {
                string apiVersion;
                if (LrControl.GetApiVersion(out apiVersion))
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