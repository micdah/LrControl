using System.Diagnostics;
using Midi.Messages;

namespace micdah.LrControl.Core.Midi
{
    internal class NrpnMessageHolder : MessageHolder<NrpnMessage>
    {
        public NrpnMessageHolder(NrpnMessage msg, Stopwatch timestampStopwatch) : base(msg, timestampStopwatch)
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