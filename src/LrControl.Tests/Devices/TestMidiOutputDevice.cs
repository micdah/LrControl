using System.Collections.Concurrent;
using RtMidi.Core.Devices;
using RtMidi.Core.Messages;

namespace LrControl.Tests.Devices
{
    public class TestMidiOutputDevice : IMidiOutputDevice
    {
        public TestMidiOutputDevice(string name) => Name = name;
        public bool IsOpen { get; private set; }
        public string Name { get; }
        public BlockingCollection<object> Messages { get; } = new BlockingCollection<object>();
        
        public bool Open() => IsOpen = true;
        public void Close() => IsOpen = false;
        public void Dispose() => Close();
        public bool Send(in NoteOffMessage msg) => SaveAndReturnTrue(msg);
        public bool Send(in NoteOnMessage msg) => SaveAndReturnTrue(msg);
        public bool Send(in PolyphonicKeyPressureMessage msg) => SaveAndReturnTrue(msg);
        public bool Send(in ControlChangeMessage msg) => SaveAndReturnTrue(msg);
        public bool Send(in ProgramChangeMessage msg) => SaveAndReturnTrue(msg);
        public bool Send(in ChannelPressureMessage msg)  => SaveAndReturnTrue(msg);
        public bool Send(in PitchBendMessage msg) => SaveAndReturnTrue(msg);
        public bool Send(in NrpnMessage msg) => SaveAndReturnTrue(msg);
        public bool Send(in SysExMessage msg) => SaveAndReturnTrue(msg);

        private bool SaveAndReturnTrue(object msg)
        {
            Messages.Add(msg);
            return true;
        }
    }
}