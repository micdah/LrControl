using LrControl.Devices;
using RtMidi.Core.Devices;

namespace LrControl.Tests.Mocks
{
    public class TestInputDeviceInfo : IInputDeviceInfo
    {
        private readonly IMidiInputDevice _inputDevice;
        public TestInputDeviceInfo(IMidiInputDevice inputDevice) => _inputDevice = inputDevice;
        public IMidiInputDevice CreateDevice() => _inputDevice;
        public string Name => _inputDevice.Name;
    }
}