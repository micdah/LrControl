using RtMidi.Core.Messages;

namespace LrControl.Devices.Midi.Messages
{
    internal class SysExMessageHolder : MessageHolder<SysExMessage>
    {
        public SysExMessageHolder(in SysExMessage msg) : base(in msg)
        {
        }

        protected override bool CalculateHasChanged()
        {
            return LastSentTimestamp != MessageTimestamp;
        }

        protected override void SendMessage(IMidiInputDeviceEventDispatcher dispatcher, in SysExMessage msg)
        {
            dispatcher.OnSysEx(in msg);
        }
    }
}