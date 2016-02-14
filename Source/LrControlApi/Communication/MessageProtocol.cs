using System.Text;
using LrControlApi.LrControl;

namespace LrControlApi.Communication
{
    internal class MessageProtocol<TModule>
    {
        private readonly PluginClient _pluginClient;
        private readonly string _moduleName;

        public MessageProtocol(PluginClient pluginClient)
        {
            _pluginClient = pluginClient;
            _moduleName = typeof (TModule).Name;
        }

        public void Send(string method, params string[] args)
        {
            var message = FormatMessage(_moduleName, method, args);

            string response;
            if (!_pluginClient.SendMessage(message, out response))
            {
                throw new ApiException($"Unable to send '{message}'");
            }

            if (response != "ack")
            {
                throw new ApiException($"Got unexpected response '{response}' after sending '{message}'");
            }
        }

        public string SendWithResponse(string method, params string[] args)
        {
            var message = FormatMessage(_moduleName, method, args);
            string response;
            if (!_pluginClient.SendMessage(message, out response))
            {
                throw new ApiException($"Unable to send '{message}'");
            }

            return response;
        }

        private static string FormatMessage(string module, string method, params string[] args)
        {
            var builder = new StringBuilder($"{module}.{method}");

            if (args != null && args.Length > 0)
            {
                builder.Append(' ');
                builder.Append(string.Join(",", args));
            }

            return builder.ToString();
        }
    }
}