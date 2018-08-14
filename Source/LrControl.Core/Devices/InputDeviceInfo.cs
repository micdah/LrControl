using System;
using RtMidi.Core.Devices;
using RtMidi.Core.Devices.Infos;

namespace LrControl.Core.Devices
{
    public class InputDeviceInfo
    {
        private readonly IMidiInputDevice _midiInputDevice;
        private readonly IMidiInputDeviceInfo _midiInputDeviceInfo;
        
        internal InputDeviceInfo(IMidiInputDeviceInfo midiInputDeviceInfo)
        {
            _midiInputDeviceInfo = midiInputDeviceInfo;
        }

        internal InputDeviceInfo(IMidiInputDevice midiInputDevice)
        {
            _midiInputDevice = midiInputDevice;
        }

        internal IMidiInputDevice CreateDevice()
        {
            return _midiInputDevice ?? _midiInputDeviceInfo.CreateDevice();
        }

        public string Name => _midiInputDevice?.Name ?? _midiInputDeviceInfo.Name;
    }
}