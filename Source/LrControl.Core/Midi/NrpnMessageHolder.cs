using RtMidi.Core.Messages;

namespace LrControl.Core.Midi
{
    public class NrpnMessageHolder : MessageHolder<NrpnMessage>
    {
        public NrpnMessageHolder(in NrpnMessage msg) : base(in msg)
        {
        }

        protected override bool CalculateHasChanged()
        {
            return LastSent.Channel != Message.Channel
                   || LastSent.Parameter != Message.Parameter
                   || LastSent.Value != Message.Value;
        }
    }
}