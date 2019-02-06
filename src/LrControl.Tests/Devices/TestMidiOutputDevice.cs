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
        public bool Send(in NoteOffMessage msg) => Send(msg);
        public bool Send(in NoteOnMessage msg) => Send(msg);
        public bool Send(in PolyphonicKeyPressureMessage msg) => Send(msg);
        public bool Send(in ControlChangeMessage msg) => Send(msg);
        public bool Send(in ProgramChangeMessage msg) => Send(msg);
        public bool Send(in ChannelPressureMessage msg)  => Send(msg);
        public bool Send(in PitchBendMessage msg) => Send(msg);
        public bool Send(in NrpnMessage msg) => Send(msg);
        public bool Send(in SysExMessage msg) => Send(msg);

        private bool Send(object msg)
        {
            Messages.Add(msg);
            return true;
        }
    }
}