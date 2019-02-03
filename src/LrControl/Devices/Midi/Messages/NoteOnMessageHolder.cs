using RtMidi.Core.Messages;

namespace LrControl.Devices.Midi.Messages
{
    internal class NoteOnMessageHolder : MessageHolder<NoteOnMessage>
    {
        public NoteOnMessageHolder(in NoteOnMessage msg) : base(in msg)
        {
        }

        protected override bool CalculateHasChanged()
        {
            return LastSent.Channel != Message.Channel
                   || LastSent.Key != Message.Key
                   || LastSent.Velocity != Message.Velocity;
        }

        protected override void SendMessage(IMidiInputDeviceEventDispatcher dispatcher, in NoteOnMessage msg)
        {
            dispatcher.OnNoteOn(in msg);
        }
    }
}