using System;
using RtMidi.Core.Enums;
using RtMidi.Core.Messages;

namespace LrControl.Devices
{
    /// <summary>
    /// Uniquely identifies a controller
    /// </summary>
    public readonly struct ControllerId : IEquatable<ControllerId>
    {
        public ControllerId(in NoteOffMessage msg) 
            : this(MessageType.NoteOff, msg.Channel, (int) msg.Key)
        {
        }

        public ControllerId(in NoteOnMessage msg) 
            : this(MessageType.NoteOn, msg.Channel, (int) msg.Key)
        {
        }

        public ControllerId(in PolyphonicKeyPressureMessage msg)
            : this(MessageType.PolyphonicKeyPressure, msg.Channel, (int) msg.Key)
        {
        }

        public ControllerId(in ControlChangeMessage msg)
            : this(MessageType.ControlChange, msg.Channel, msg.Control)
        {
        }

        public ControllerId(in ProgramChangeMessage msg)
            : this(MessageType.ProgramChange, msg.Channel, 0)
        {
        }

        public ControllerId(in ChannelPressureMessage msg)
            : this(MessageType.ChannelPressure, msg.Channel, 0)
        {
        }

        public ControllerId(in PitchBendMessage msg)
            : this(MessageType.PitchBend, msg.Channel, 0)
        {
        }

        public ControllerId(in NrpnMessage msg)
            : this(MessageType.Nrpn, msg.Channel, msg.Parameter)
        {
        }

        public ControllerId(MessageType messageType, Channel channel, int parameter)
        {
            MessageType = messageType;
            Channel = channel;
            Parameter = parameter;
        }
        
        public MessageType MessageType { get; }
        public Channel Channel { get; }
        public int Parameter { get; }

        public static bool operator ==(in ControllerId some, in ControllerId other)
        {
            return some.Equals(other);
        }

        public static bool operator !=(in ControllerId some, in ControllerId other)
        {
            return !some.Equals(other);
        }

        public bool Equals(ControllerId other)
        {
            return MessageType == other.MessageType && 
                   Channel == other.Channel && 
                   Parameter == other.Parameter;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ControllerId other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) MessageType;
                hashCode = (hashCode * 397) ^ (int) Channel;
                hashCode = (hashCode * 397) ^ Parameter;
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"{nameof(MessageType)}: {MessageType}, {nameof(Channel)}: {Channel}, {nameof(Parameter)}: {Parameter}";
        }
    }
}