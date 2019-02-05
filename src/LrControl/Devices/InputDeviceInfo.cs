using RtMidi.Core.Devices;
using RtMidi.Core.Devices.Infos;

namespace LrControl.Devices
{
    public interface IInputDeviceInfo
    {
        IMidiInputDevice CreateDevice();
        string Name { get; }
    }

    public class InputDeviceInfo : IInputDeviceInfo
    {
        private readonly IMidiInputDevice _midiInputDevice;
        private readonly IMidiInputDeviceInfo _midiInputDeviceInfo;
        
        public InputDeviceInfo(IMidiInputDeviceInfo midiInputDeviceInfo)
        {
            _midiInputDeviceInfo = midiInputDeviceInfo;
        }

        public InputDeviceInfo(IMidiInputDevice midiInputDevice)
        {
            _midiInputDevice = midiInputDevice;
        }

        public IMidiInputDevice CreateDevice()
        {
            return _midiInputDevice ?? _midiInputDeviceInfo.CreateDevice();
        }

        public string Name => _midiInputDevice?.Name ?? _midiInputDeviceInfo.Name;
    }
}