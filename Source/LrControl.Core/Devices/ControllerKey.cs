using System;
using LrControl.Core.Devices.Enums;
using RtMidi.Core.Enums;

namespace LrControl.Core.Devices
{
    internal struct ControllerKey : IEquatable<ControllerKey>
    {
        public ControllerMessageType ControllerMessageType { get; }
        public Channel Channel { get; }
        public int ControlNumber { get; }

        public ControllerKey(ControllerMessageType controllerMessageType, Channel channel, int controlNumber)
        {
            ControllerMessageType = controllerMessageType;
            Channel = channel;
            ControlNumber = controlNumber;
        }

        public ControllerKey(Controller controller) : this(controller.MessageType, controller.MidiChannel,
            controller.ControlNumber)
        {
        }

        public bool Equals(ControllerKey other)
        {
            return ControllerMessageType == other.ControllerMessageType && Channel == other.Channel && ControlNumber == other.ControlNumber;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ControllerKey && Equals((ControllerKey) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) ControllerMessageType;
                hashCode = (hashCode * 397) ^ (int) Channel;
                hashCode = (hashCode * 397) ^ ControlNumber;
                return hashCode;
            }
        }
    }
}