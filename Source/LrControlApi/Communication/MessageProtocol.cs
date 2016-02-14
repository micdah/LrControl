using System;
using System.Text;
using LrControlApi.Common;

namespace LrControlApi.Communication
{
    internal class MessageProtocol<TModule>
    {
        private readonly string _moduleName;
        private readonly PluginClient _pluginClient;

        public MessageProtocol(PluginClient pluginClient)
        {
            _pluginClient = pluginClient;
            _moduleName = typeof (TModule).Name;
        }

        public void Invoke(string method, params object[] args)
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

        public string InvokeWithResult(string method, params object[] args)
        {
            var lowerFirstMethod = char.ToLowerInvariant(method[0]) + method.Substring(1);

            var message = FormatMessage(_moduleName, lowerFirstMethod, args);
            string response;
            if (!_pluginClient.SendMessage(message, out response))
            {
                throw new ApiException($"Unable to send '{message}'");
            }

            return response;
        }

        private static string FormatMessage(string module, string method, params object[] args)
        {
            var builder = new StringBuilder($"{module}.{method}");

            if (args != null && args.Length > 0)
            {
                builder.Append(' ');

                for (var i = 0; i < args.Length; i++)
                {
                    builder.Append(i == 0 ? ' ' : '\u001E');
                    var arg = args[i];

                    if (arg is IParameter)
                    {
                        var parameter = (IParameter) arg;
                        AppendTypedArgument(builder, parameter.Name);
                    }
                    else if (arg is ClassEnum)
                    {
                        var classEnum = (ClassEnum) arg;
                        AppendTypedArgument(builder, classEnum.ObjectValue);
                    }
                    else
                    {
                        AppendTypedArgument(builder, arg);
                    }
                }
            }

            return builder.ToString();
        }

        private static void AppendTypedArgument(StringBuilder builder, object arg)
        {
            if (arg is string)
            {
                builder.Append("S");
                builder.Append((string) arg);
            }
            else if (arg is int)
            {
                builder.Append("N");
                builder.Append((int) arg);
            }
            else if (arg is double)
            {
                builder.Append("N");
                builder.Append((double) arg);
            } else if (arg is bool)
            {
                builder.Append("B");
                builder.Append((bool) arg ? 1 : 0);
            }

            throw new ArgumentException($"Unsupported argument type {arg.GetType().Name}", nameof(arg));
        }
    }
}