using System.Collections.Generic;
using RtMidi.Core.Devices.Infos;
using Serilog;

namespace LrControl.Core.Devices
{
    internal class DeviceBrowser : IDeviceBrowser
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<DeviceBrowser>();

        private readonly List<IMidiInputDeviceInfo> _midiInputDeviceInfos = new List<IMidiInputDeviceInfo>();
        private readonly List<IMidiOutputDeviceInfo> _midiOutputDeviceInfos = new List<IMidiOutputDeviceInfo>();
        
        public void Refresh()
        {
            
        }
    }

    public interface IDeviceBrowser
    {
        /// <summary>
        /// Refreshes available input/output devices
        /// </summary>
        void Refresh();
    }
}