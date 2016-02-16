using System;
using micdah.LrControlApi.Communication;
using micdah.LrControlApi.Modules.LrControl;
using micdah.LrControlApi.Modules.LrDevelopController;

namespace micdah.LrControlApi
{
    public class LrControlApi : IDisposable
    {
        private readonly LrControl _lrControl;
        private readonly LrDevelopController _lrDevelopController;
        private readonly PluginClient _pluginClient;

        public LrControlApi(int sendPort, int receivePort)
        {
            _pluginClient = new PluginClient(sendPort, receivePort);
            _pluginClient.ConnectionStatus += OnConnectionStatus;

            _lrControl = new LrControl(new MessageProtocol<LrControl>(_pluginClient));
            _lrDevelopController =
                new LrDevelopController(
                    new MessageProtocol<LrDevelopController>(_pluginClient));


            _pluginClient.Open();
        }

        public bool IsConnected => _pluginClient.IsConnected;

        public ILrControl LrControl => _lrControl;
        public ILrDevelopController LrDevelopController => _lrDevelopController;

        public void Dispose()
        {
            _pluginClient.Close();
        }

        public event Action<bool, string> ConnectionStatus;

        private void OnConnectionStatus(bool connected)
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