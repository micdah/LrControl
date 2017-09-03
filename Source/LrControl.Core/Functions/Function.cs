using System;
using LrControl.Api;
using LrControl.Api.Common;
using LrControl.Core.Configurations;

namespace LrControl.Core.Functions
{
    internal abstract class Function : IFunction
    {
        protected readonly ISettings Settings;
        private bool _dispoed;
        private Action _onRequestUpdateControllerValue;

        protected Function(ISettings settings, LrApi api, string displayName, string key)
        {
            Settings = settings;
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
            controllerValue = default(int);
            return false;
        }

        public Action OnRequestUpdateControllerValue
        {
            set => _onRequestUpdateControllerValue = value;
        }

        public void Dispose()
        {
            if (_dispoed) return;

            Disposing();
            _dispoed = true;
        }

        protected void RequestUpdateControllerValue()
        {
            _onRequestUpdateControllerValue?.Invoke();
        }

        protected void ShowHud(string message)
        {
            if (Settings.ShowHudMessages)
                Api.LrDialogs.ShowBezel(message, 0.25);
        }

        protected virtual void Disposing()
        {
        }
    }
}