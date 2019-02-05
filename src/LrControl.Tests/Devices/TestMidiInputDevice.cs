using RtMidi.Core.Devices;
using RtMidi.Core.Devices.Nrpn;
using RtMidi.Core.Messages;

namespace LrControl.Tests.Devices
{
    public class TestMidiInputDevice : IMidiInputDevice
    {
        public TestMidiInputDevice(string name) => Name = name;
        public bool IsOpen { get; private set; }
        public string Name { get; }
        public NrpnMode NrpnMode { get; private set; }

        public bool Open() => IsOpen = true;
        public void Close() => IsOpen = false;
        public void SetNrpnMode(NrpnMode mode) => NrpnMode = mode;
        public void Dispose()
        {
        }

        public event NoteOffMessageHandler NoteOff;
        public event NoteOnMessageHandler NoteOn;
        public event PolyphonicKeyPressureMessageHandler PolyphonicKeyPressure;
        public event ControlChangeMessageHandler ControlChange;
        public event ProgramChangeMessageHandler ProgramChange;
        public event ChannelPressureMessageHandler ChannelPressure;
        public event PitchBendMessageHandler PitchBend;
        public event NrpnMessageHandler Nrpn;
        public event SysExMessageHandler SysEx;

        public void OnNoteOff(NoteOffMessage msg) => NoteOff?.Invoke(this, msg);
        public void OnNoteOn(NoteOnMessage msg) => NoteOn?.Invoke(this, msg);
        public void OnPolyphonicKeyPressure(PolyphonicKeyPressureMessage msg) 
            => PolyphonicKeyPressure?.Invoke(this, msg);
        public void OnControlChange(ControlChangeMessage msg) => ControlChange?.Invoke(this, msg);
        public void OnProgramChange(ProgramChangeMessage msg) => ProgramChange?.Invoke(this, msg);
        public void OnChannelPressure(ChannelPressureMessage msg) => ChannelPressure?.Invoke(this, msg);
        public void OnPitchBend(PitchBendMessage msg) => PitchBend?.Invoke(this, msg);
        public void OnNrpn(NrpnMessage msg) => Nrpn?.Invoke(this, msg);
        public void OnSysEx(SysExMessage msg) => SysEx?.Invoke(this, msg);
    }
}