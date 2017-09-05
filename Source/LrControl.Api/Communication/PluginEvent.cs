namespace LrControl.Api.Communication
{
    internal struct PluginEvent
    {
        public PluginEvent(PluginEventType pluginEventType, string data)
        {
            PluginEventType = pluginEventType;
            Data = data;
        }

        public PluginEventType PluginEventType { get; }

        public string Data { get; }
    }
}