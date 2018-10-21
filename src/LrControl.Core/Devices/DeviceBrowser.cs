using System.Collections.Generic;
using System.Linq;
using RtMidi.Core;
using RtMidi.Core.Devices.Infos;
using Serilog;

namespace LrControl.Core.Devices
{
    public interface IDeviceBrowser
    {
        /// <summary>
        /// Refreshes available input/output devices (<see cref="InputDevices"/> and <see cref="OutputDevices"/>)
        /// </summary>
        void Refresh();
        
        /// <summary>
        /// Returns currently available input devices
        /// </summary>
        IReadOnlyCollection<InputDeviceInfo> InputDevices { get; }
        
        /// <summary>
        /// Returns currently available output devices
        /// </summary>
        IReadOnlyCollection<OutputDeviceInfo> OutputDevices { get; }
    }

    internal class DeviceBrowser : IDeviceBrowser
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<DeviceBrowser>();
        private readonly List<InputDeviceInfo> _inputDevices = new List<InputDeviceInfo>();
        private readonly List<OutputDeviceInfo> _outputDevices = new List<OutputDeviceInfo>();
        
        public void Refresh()
        {
            _inputDevices.Clear();
            _inputDevices.AddRange(MidiDeviceManager.Default.InputDevices.Select(x => new InputDeviceInfo(x)));

            _outputDevices.Clear();
            _outputDevices.AddRange(MidiDeviceManager.Default.OutputDevices.Select(x => new OutputDeviceInfo(x)));
        }

        public IReadOnlyCollection<InputDeviceInfo> InputDevices => _inputDevices.AsReadOnly();
        public IReadOnlyCollection<OutputDeviceInfo> OutputDevices => _outputDevices.AsReadOnly();
    }
}