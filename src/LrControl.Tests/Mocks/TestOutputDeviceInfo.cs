using LrControl.Devices;
using RtMidi.Core.Devices;

namespace LrControl.Tests.Mocks
{
    public class TestOutputDeviceInfo : IOutputDeviceInfo
    {
        private readonly IMidiOutputDevice _outputDevice;
        public TestOutputDeviceInfo(IMidiOutputDevice outputDevice) => _outputDevice = outputDevice;
        public IMidiOutputDevice CreateDevice() => _outputDevice;

        public string Name => _outputDevice.Name;
    }
}