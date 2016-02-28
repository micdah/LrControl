using System.Diagnostics;
using Midi.Messages;

namespace micdah.LrControl.Core.Midi
{
    internal class ControlChangeMessageHolder : MessageHolder<ControlChangeMessage>
    {
        public ControlChangeMessageHolder(ControlChangeMessage msg, Stopwatch timestampStopwatch)
            : base(msg, timestampStopwatch)
        {
        }

        protected override bool CalculateHasChanged()
        {
            return LastSent.Channel != Message.Channel
                   || LastSent.Control != Message.Control
                   || LastSent.Value != Message.Value;
        }
    }
}