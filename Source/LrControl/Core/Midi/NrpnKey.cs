using Midi.Enums;
using Midi.Messages;

namespace micdah.LrControl.Core.Midi
{
    internal class NrpnKey
    {
        private NrpnKey(Channel channel, int parameter)
        {
            Channel = channel;
            Parameter = parameter;
        }

        public NrpnKey(NrpnMessage msg) : this(msg.Channel, msg.Parameter)
        {
        }

        private Channel Channel { get; }
        private int Parameter { get; }

        private bool Equals(NrpnKey other)
        {
            return Channel == other.Channel && Parameter == other.Parameter;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((NrpnKey) obj);
        }

        public override int GetHashCode()
        {
            return (int) Channel | Parameter << 4;
        }
    }
}