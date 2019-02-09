using RtMidi.Core.Messages;

namespace LrControl.Devices.Midi.Messages
{
    internal class PitchBendMessageHolder : MessageHolder<PitchBendMessage>
    {
        public PitchBendMessageHolder(in PitchBendMessage msg) : base(in msg)
        {
        }

        protected override bool CalculateHasChanged()
        {
            return LastSent.Channel != Message.Channel
                   || LastSent.Value != Message.Value;
        }

        protected override void SendMessage(IMidiEventDispatcher dispatcher, in PitchBendMessage msg)
        {
            dispatcher.OnPitchBend(in msg);
        }
    }
}