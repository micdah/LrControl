using System;
using Midi.Devices;

namespace LrControl.Core.Devices
{
    public class OutputDeviceInfo
    {
        internal OutputDeviceInfo(IOutputDevice outputDevice)
        {
            Name = outputDevice.Name;
        }

        internal Func<IOutputDevice, bool> MatchThisFunc => outputDevice => outputDevice.Name == Name;

        public string Name { get; }
    }
}