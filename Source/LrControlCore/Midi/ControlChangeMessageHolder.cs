using Midi.Messages;

namespace LrControlCore.Midi
{
    internal class ControlChangeMessageHolder : MessageHolder<ControlChangeMessage>
    {
        public ControlChangeMessageHolder(ControlChangeMessage msg) : base(msg)
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