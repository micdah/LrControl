using LrControl.Configurations;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;

namespace LrControl.Functions
{
    public abstract class Function : IFunction
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

        public abstract void Apply(int value, Range range, Module activeModule, Panel activePanel);

        protected void ShowHud(string message)
        {
            if (_settings.ShowHudMessages)
                Api.LrDialogs.ShowBezel(message, 0.25);
        }
    }
}