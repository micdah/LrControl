using System;
using micdah.LrControlApi.Communication;
using micdah.LrControlApi.Modules.LrApplicationView;
using micdah.LrControlApi.Modules.LrControl;
using micdah.LrControlApi.Modules.LrDevelopController;
using micdah.LrControlApi.Modules.LrDialogs;
using micdah.LrControlApi.Modules.LrSelection;
using micdah.LrControlApi.Modules.LrUndo;

namespace micdah.LrControlApi
{
    public class LrApi : IDisposable
    {
        private readonly LrApplicationView _lrApplicationView;
        private readonly LrControl _lrControl;
        private readonly LrDevelopController _lrDevelopController;
        private readonly LrDialogs _lrDialogs;
        private readonly LrSelection _lrSelection;
        private readonly LrUndo _lrUndo;
        private readonly PluginClient _pluginClient;

        public LrApi(int sendPort = 52008, int receivePort = 52009)
        {
            _pluginClient             = new PluginClient(sendPort, receivePort);
            _lrControl                = new LrControl(new MessageProtocol<LrControl>(_pluginClient));
            _lrDevelopController      = new LrDevelopController(new MessageProtocol<LrDevelopController>(_pluginClient));
            _lrApplicationView        = new LrApplicationView(new MessageProtocol<LrApplicationView>(_pluginClient));
            _lrDialogs                = new LrDialogs(new MessageProtocol<LrDialogs>(_pluginClient));
            _lrSelection              = new LrSelection(new MessageProtocol<LrSelection>(_pluginClient));
            _lrUndo                   = new LrUndo(new MessageProtocol<LrUndo>(_pluginClient));

            _pluginClient.Connection    += PluginClientOnConnection;
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

        public void Dispose()
        {
            _pluginClient.Close();
        }

        public event Action<bool, string> ConnectionStatus;

        private void PluginClientOnConnection(bool connected)
        {
            if (connected)
            {
                string apiVersion;
                if (LrControl.GetApiVersion(out apiVersion))
                {
                    ConnectionStatus?.Invoke(true, apiVersion);
                }
                else
                {
                    ConnectionStatus?.Invoke(false, null);
                }
            }
            else
            {
                ConnectionStatus?.Invoke(false, null);
            }
        }
    }
}