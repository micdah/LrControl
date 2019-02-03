using RtMidi.Core.Messages;

namespace LrControl.Devices.Midi.Messages
{
    internal class ChannelPressureMessageHolder : MessageHolder<ChannelPressureMessage>
    {
        public ChannelPressureMessageHolder(in ChannelPressureMessage msg) : base(in msg)
        {
        }

        protected override bool CalculateHasChanged()
        {
            return LastSent.Channel != Message.Channel
                   || LastSent.Pressure != Message.Pressure;
        }

        protected override void SendMessage(IMidiInputDeviceEventDispatcher dispatcher, in ChannelPressureMessage msg)
        {
            dispatcher.OnChannelPressure(in msg);
        }
    }
}