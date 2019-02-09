using RtMidi.Core.Messages;

namespace LrControl.Devices.Midi
{
    internal interface IMidiEventDispatcher
    {
        void OnNoteOff(in NoteOffMessage msg);
        void OnNoteOn(in NoteOnMessage msg);
        void OnPolyphonicKeyPressure(in PolyphonicKeyPressureMessage msg);
        void OnControlChange(in ControlChangeMessage msg);
        void OnProgramChange(in ProgramChangeMessage msg);
        void OnChannelPressure(in ChannelPressureMessage msg);
        void OnPitchBend(in PitchBendMessage msg);
        void OnNrpn(in NrpnMessage msg);
        void OnSysEx(in SysExMessage msg);
    }
}