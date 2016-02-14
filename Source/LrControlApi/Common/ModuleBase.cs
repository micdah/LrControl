using System;
using System.Security.Policy;
using LrControlApi.Communication;

namespace LrControlApi.Common
{
    internal abstract class ModuleBase<TModule>
    {
        private readonly MessageProtocol<TModule> _messageProtocol;

        protected ModuleBase(MessageProtocol<TModule> messageProtocol)
        {
            _messageProtocol = messageProtocol;
        }

        protected void Invoke(string method, params object[] args)
        {
            _messageProtocol.Invoke(method, args);
        }

        protected string InvokeWithResult(string method, params object[] args)
        {
            return _messageProtocol.InvokeWithResult(method, args);
        }

        protected TEnum InvokeWithResult<TEnum,TValue>(string method, params object[] args)
            where TEnum : ClassEnum<TValue, TEnum>
        {
            var result = InvokeWithResult(method, args);

            TValue value;
            if (typeof (TValue) == typeof (string))
            {
                value = (TValue) (object) result;
            } else if (typeof (TValue) == typeof (int))
            {
                value = (TValue) (object) Convert.ToInt32(result);
            }
            else
            {
                throw new ArgumentException($"Unsupported type of ClassEnum ({typeof (TValue).Name})", nameof(TValue));
            }

            return ClassEnum<TValue, TEnum>.GetEnumForValue(value);
        }
    }
}