using System;
using LrControlApi.Communication;
using LrControlApi.LrControl;
using LrControlApi.LrDevelopController;

namespace LrControlApi
{
    public class LrControlApi : IDisposable
    {
        private readonly LrControl.LrControl _lrControl;
        private readonly LrDevelopController.LrDevelopController _lrDevelopController;
        private readonly PluginClient _pluginClient;

        public LrControlApi(int sendPort, int receivePort)
        {
            _pluginClient = new PluginClient(sendPort, receivePort);
            _pluginClient.ConnectionStatus += OnConnectionStatus;

            _lrControl = new LrControl.LrControl(new MessageProtocol<LrControl.LrControl>(_pluginClient));
            _lrDevelopController =
                new LrDevelopController.LrDevelopController(
                    new MessageProtocol<LrDevelopController.LrDevelopController>(_pluginClient));


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
                try
                {
                    var apiVersion = LrControl.GetApiVersion();
                    ConnectionStatus?.Invoke(true, apiVersion);
                }
                catch (ApiException)
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