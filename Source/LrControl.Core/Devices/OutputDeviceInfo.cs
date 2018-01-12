using System;
using RtMidi.Core.Devices;
using RtMidi.Core.Devices.Infos;

namespace LrControl.Core.Devices
{
    public class OutputDeviceInfo
    {
        internal OutputDeviceInfo(IMidiOutputDeviceInfo info)
        {
            Name = info.Name;
        }

        internal OutputDeviceInfo(IMidiOutputDevice device)
        {
            Name = device.Name;
        }

        internal Func<IMidiOutputDeviceInfo, bool> MatchThisFunc => outputDevice => outputDevice.Name == Name;

        public string Name { get; }
    }
}