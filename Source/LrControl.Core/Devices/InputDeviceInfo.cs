using System;
using System.Linq.Expressions;
using Midi.Devices;

namespace LrControl.Core.Devices
{
    public class InputDeviceInfo
    {
        internal InputDeviceInfo(IInputDevice inputDevice)
        {
            Name = inputDevice.Name;
        }

        internal Func<IInputDevice, bool> MatchThisFunc => inputDevice => inputDevice.Name == Name;

        public string Name { get; }
    }
}