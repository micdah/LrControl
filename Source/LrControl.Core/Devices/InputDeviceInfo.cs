using System;
using RtMidi.Core.Devices;
using RtMidi.Core.Devices.Infos;

namespace LrControl.Core.Devices
{
    public class InputDeviceInfo
    {
        internal InputDeviceInfo(IMidiInputDeviceInfo info)
        {
            Name = info.Name;
        }

        internal InputDeviceInfo(IMidiInputDevice device)
        {
            Name = device.Name;
        }

        internal Func<IMidiInputDeviceInfo, bool> MatchThisFunc => inputDevice => inputDevice.Name == Name;

        public string Name { get; }
    }
}