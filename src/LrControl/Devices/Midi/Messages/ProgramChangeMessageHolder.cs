using RtMidi.Core.Messages;

namespace LrControl.Devices.Midi.Messages
{
    internal class ProgramChangeMessageHolder : MessageHolder<ProgramChangeMessage>
    {
        public ProgramChangeMessageHolder(in ProgramChangeMessage msg) : base(in msg)
        {
        }

        protected override bool CalculateHasChanged()
        {
            return LastSent.Channel != Message.Channel
                   || LastSent.Program != Message.Program;
        }

        protected override void SendMessage(IMidiEventDispatcher dispatcher, in ProgramChangeMessage msg)
        {
            dispatcher.OnProgramChange(in msg);
        }
    }
}