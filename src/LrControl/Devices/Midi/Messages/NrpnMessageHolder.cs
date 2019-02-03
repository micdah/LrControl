using RtMidi.Core.Messages;

namespace LrControl.Devices.Midi.Messages
{
    internal class NrpnMessageHolder : MessageHolder<NrpnMessage>
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

        protected override void SendMessage(IMidiInputDeviceEventDispatcher dispatcher, in NrpnMessage msg)
        {
            dispatcher.OnNrpn(in msg);
        }
    }
}