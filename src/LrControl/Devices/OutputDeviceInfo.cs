using RtMidi.Core.Devices;
using RtMidi.Core.Devices.Infos;

namespace LrControl.Devices
{
    public class OutputDeviceInfo
    {
        private readonly IMidiOutputDevice _midiOutputDevice;
        private readonly IMidiOutputDeviceInfo _midiOutputDeviceInfo;

        public OutputDeviceInfo(IMidiOutputDeviceInfo midiOutputDeviceInfo)
        {
            _midiOutputDeviceInfo = midiOutputDeviceInfo;
        }

        public OutputDeviceInfo(IMidiOutputDevice midiOutputDevice)
        {
            _midiOutputDevice = midiOutputDevice;
        }

        public IMidiOutputDevice CreateDevice()
        {
            return _midiOutputDevice ?? _midiOutputDeviceInfo.CreateDevice();
        }

        public string Name => _midiOutputDevice?.Name ?? _midiOutputDeviceInfo.Name;
    }
}