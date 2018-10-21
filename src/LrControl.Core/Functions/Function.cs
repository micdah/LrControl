using System;
using LrControl.Core.Configurations;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Core.Functions
{
    internal abstract class Function : IFunction
    {
        private readonly ISettings _settings;
        private bool _disposed;
        private Action _onRequestUpdateControllerValue;

        protected Function(ISettings settings, ILrApi api, string displayName, string key)
        {
            _settings = settings;
            Api = api;
            DisplayName = displayName;
            Key = key;
        }

        protected ILrApi Api { get; }
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

        protected void RequestUpdateControllerValue(IParameter parameter)
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