using System;
using LrControl.Api;
using LrControl.Api.Common;
using LrControl.Core.Configurations;

namespace LrControl.Core.Functions
{
    internal abstract class Function : IFunction
    {
        private readonly ISettings _settings;
        private bool _disposed;
        private Action _onRequestUpdateControllerValue;

        protected Function(ISettings settings, LrApi api, string displayName, string key)
        {
            _settings = settings;
            Api = api;
            DisplayName = displayName;
            Key = key;
        }

        protected LrApi Api { get; }
        public string Key { get; }
        public string DisplayName { get; }

        public abstract void ControllerValueChanged(int controllerValue, Range controllerRange);
        
        public virtual bool UpdateControllerValue(out int controllerValue, Range controllerRange)
        {
            controllerValue = default;
            return false;
        }

        public Action OnRequestUpdateControllerValue
        {
            set => _onRequestUpdateControllerValue = value;
        }

        public void Dispose()
        {
            if (_disposed) return;

            Disposing();
            _disposed = true;
        }

        protected void RequestUpdateControllerValue()
        {
            _onRequestUpdateControllerValue?.Invoke();
        }

        protected void ShowHud(string message)
        {
            if (_settings.ShowHudMessages)
                Api.LrDialogs.ShowBezel(message, 0.25);
        }

        protected virtual void Disposing()
        {
        }
    }
}