using Midi.Enums;
using Midi.Messages;

namespace micdah.LrControl.Core.Midi
{
    internal class ControlChangeKey
    {
        public ControlChangeKey(Channel channel, Control control)
        {
            Channel = channel;
            Control = control;
        }

        public ControlChangeKey(ControlChangeMessage msg) : this(msg.Channel, msg.Control)
        {
        }

        private Channel Channel { get; }
        private Control Control { get; }

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
            return (int) Control | (int) Channel << 4;
        }
    }
}