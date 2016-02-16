using System;
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

        protected bool Invoke(string method, params object[] args)
        {
            return _messageProtocol.Invoke(method, args);
        }

        protected bool Invoke<TResult>(out TResult result, string method, params object[] args)
        {
            return _messageProtocol.Invoke(out result, method, args);
        }

        protected bool Invoke<TResult1, TResult2>(out TResult1 result1, out TResult2 result2, string method,
            params object[] args)
        {
            return _messageProtocol.Invoke(out result1, out result2, method, args);
        }

        protected bool Invoke<TResult1, TResult2, TResult3>(out TResult1 result1, out TResult2 result2,
            out TResult3 result3, string method, params object[] args)
        {
            return _messageProtocol.Invoke(out result1, out result2, out result3, method, args);
        }

        protected static bool False<T1>(out T1 t1)
        {
            t1 = default(T1);
            return false;
        }

        protected static bool False<T1, T2>(out T1 t1, out T2 t2)
        {
            t1 = default(T1);
            t2 = default(T2);
            return false;
        }

        protected static bool False<T1, T2, T3>(out T1 t1, out T2 t2, out T3 t3)
        {
            t1 = default(T1);
            t2 = default(T2);
            t3 = default(T3);
            return false;
        }
    }
}