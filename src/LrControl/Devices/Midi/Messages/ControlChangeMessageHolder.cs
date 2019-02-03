using RtMidi.Core.Messages;

namespace LrControl.Devices.Midi.Messages
{
    internal class ControlChangeMessageHolder : MessageHolder<ControlChangeMessage>
    {
        public ControlChangeMessageHolder(in ControlChangeMessage msg) : base(in msg)
        {
        }

        protected override bool CalculateHasChanged()
        {
            return LastSent.Channel != Message.Channel
                   || LastSent.Control != Message.Control
                   || LastSent.Value != Message.Value;
        }

        protected override void SendMessage(IMidiInputDeviceEventDispatcher dispatcher, in ControlChangeMessage msg)
        {
            dispatcher.OnControlChange(in msg);
        }
    }
}