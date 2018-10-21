using LrControl.Core.Configurations;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Common;

namespace LrControl.Core.Functions
{
    internal abstract class Function : IFunction
    {
        private readonly ISettings _settings;

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

        public abstract void Apply(int value, Range range);

        protected void ShowHud(string message)
        {
            if (_settings.ShowHudMessages)
                Api.LrDialogs.ShowBezel(message, 0.25);
        }
    }
}