using System;
using RtMidi.Core.Devices;
using RtMidi.Core.Devices.Infos;

namespace LrControl.Core.Devices
{
    public class OutputDeviceInfo
    {
        private readonly IMidiOutputDevice _midiOutputDevice;
        private readonly IMidiOutputDeviceInfo _midiOutputDeviceInfo;

        internal OutputDeviceInfo(IMidiOutputDeviceInfo midiOutputDeviceInfo)
        {
            _midiOutputDeviceInfo = midiOutputDeviceInfo;
        }

        internal OutputDeviceInfo(IMidiOutputDevice midiOutputDevice)
        {
            _midiOutputDevice = midiOutputDevice;
        }

        internal IMidiOutputDevice CreateDevice()
        {
            return _midiOutputDevice ?? _midiOutputDeviceInfo.CreateDevice();
        }

        public string Name => _midiOutputDevice?.Name ?? _midiOutputDeviceInfo.Name;
    }
}