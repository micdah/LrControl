using RtMidi.Core.Enums;
using RtMidi.Core.Messages;

namespace LrControl.Core.Midi
{
    public class ControlChangeKey
    {
        public ControlChangeKey(Channel channel, int control)
        {
            Channel = channel;
            Control = control;
        }

        public ControlChangeKey(in ControlChangeMessage msg) : this(msg.Channel, msg.Control)
        {
        }

        private Channel Channel { get; }
        private int Control { get; }

        private bool Equals(ControlChangeKey other)
        {
            return Channel == other.Channel && Control == other.Control;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ControlChangeKey) obj);
        }

        public override int GetHashCode()
        {
            return Control | (int) Channel << 4;
        }
    }
}