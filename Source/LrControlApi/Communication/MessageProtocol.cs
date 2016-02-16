using System;
using System.Text;
using log4net;
using micdah.LrControlApi.Common;

namespace micdah.LrControlApi.Communication
{
    internal class MessageProtocol<TModule>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (MessageProtocol<TModule>));

        private const char RecordSeparator = '\u001E';

        private readonly string _moduleName;
        private readonly PluginClient _pluginClient;

        public MessageProtocol(PluginClient pluginClient)
        {
            _pluginClient = pluginClient;
            _moduleName = typeof (TModule).Name;
        }

        public bool Invoke(string method, params object[] args)
        {
            string response;
            if (!SendMessage(out response, method, args))
                return false;

            if (response == "ack") return true;

            Log.Warn($"Got unexpected response '{response}', was expecting 'ack'");
            return false;
        }

        public bool Invoke<TResult>(out TResult result, string method, params object[] args)
        {
            string response;
            if (!SendMessage(out response, method, args))
                return False(out result);

            return DecodeTypedString(response, out result);
        }

        public bool Invoke<TResult1, TResult2>(out TResult1 result1, out TResult2 result2, string method, params object[] args)
        {
            string response;
            if (!SendMessage(out response, method, args))
                return False(out result1, out result2);

            string[] results;
            if (!SplitResponse(response, 2, out results))
                return False(out result1, out result2);

            bool result = true;
            result &= DecodeTypedString(results[0], out result1);
            result &= DecodeTypedString(results[1], out result2);
            return result;
        }

        public bool Invoke<TResult1, TResult2, TResult3>(out TResult1 result1, out TResult2 result2, out TResult3 result3, string method, params object[] args)
        {
            string response;
            if (!SendMessage(out response, method, args))
                return False(out result1, out result2, out result3);

            string[] results;
            if (!SplitResponse(response, 3, out results))
                return False(out result1, out result2, out result3);

            bool result = true;
            result &= DecodeTypedString(results[0], out result1);
            result &= DecodeTypedString(results[1], out result2);
            result &= DecodeTypedString(results[3], out result3);

            return result;
        }

        private static string FormatMessage(string module, string method, params object[] args)
        {
            var builder = new StringBuilder($"{module}.{method}");

            if (args != null && args.Length > 0)
            {
                for (var i = 0; i < args.Length; i++)
                {
                    builder.Append(i == 0 ? ' ' : RecordSeparator);
                    var arg = args[i];

                    if (arg is IParameter)
                    {
                        var parameter = (IParameter) arg;
                        AppendTypedArgument(builder, parameter.Value);
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
            else
            {
                throw new ArgumentException($"Unsupported argument type {arg.GetType().Name}", nameof(arg));
            }
        }

        private bool SendMessage(out string response, string method, params object[] args)
        {
            var lowerFirstMethod = char.ToLowerInvariant(method[0]) + method.Substring(1);
            var message = FormatMessage(_moduleName, lowerFirstMethod, args);

            if (!_pluginClient.IsConnected)
            {
                Log.Warn("Not connected to plugin, cannot send message '{message}'");
                return False(out response);
            }

            if (!_pluginClient.SendMessage(message, out response))
            {
                Log.Warn($"Was unable to send message '{message}'");
                return false;
            }

            // Check for error codes
            if (response[0] != 'E') return true;

            Log.Warn($"Error received '{response.Substring(1)}' after sending message '{message}'");
            return false;
        }

        private static bool SplitResponse(string response, int expectedValues, out String[] values)
        {
            var split = response.Split(RecordSeparator);

            if (split.Length != expectedValues)
            {
                Log.Warn($"Got {split.Length} values, when expecting {expectedValues} from response '{response}'");
                values = null;
                return false;
            }

            values = split;
            return true;
        }
        
        private static bool DecodeTypedString<TExpectedType>(string typedString, out TExpectedType value)
        {
            var typeArg = typedString[0];
            var valueString = typedString.Substring(1);

            switch (typeArg)
            {
                case 'S':
                    if (typeof (TExpectedType) == typeof (string))
                    {
                        value = (TExpectedType) (object) valueString;
                        return true;
                    }
                    break;
                case 'N':
                    if (typeof (TExpectedType) == typeof (int))
                    {
                        value = (TExpectedType) (object) Convert.ToInt32(valueString);
                        return true;
                    }
                    if (typeof (TExpectedType) == typeof (double))
                    {
                        value = (TExpectedType) (object) Convert.ToDouble(valueString);
                        return true;
                    }
                    break;
                case 'B':
                    if (typeof (TExpectedType) == typeof (bool))
                    {
                        value = (TExpectedType) (object) (valueString == "1");
                        return true;
                    }
                    break;
            }

            Log.Warn($"Unable to decude value '{valueString}' denoted by {typeArg} to {typeof(TExpectedType).Name}");

            value = default(TExpectedType);
            return false;
        }

        private static bool False<T1>(out T1 t1)
        {
            t1 = default(T1);
            return false;
        }

        private static bool False<T1, T2>(out T1 t1, out T2 t2)
        {
            t1 = default(T1);
            t2 = default(T2);
            return false;
        }

        private static bool False<T1, T2, T3>(out T1 t1, out T2 t2, out T3 t3)
        {
            t1 = default(T1);
            t2 = default(T2);
            t3 = default(T3);
            return false;
        }
    }
}