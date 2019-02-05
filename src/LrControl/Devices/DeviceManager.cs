using System.Collections.Concurrent;
using LrControl.Configurations;
using LrControl.Devices.Midi;
using LrControl.LrPlugin.Api.Common;
using LrControl.Profiles;
using RtMidi.Core.Devices;
using RtMidi.Core.Messages;

namespace LrControl.Devices
{
    public class DeviceManager
    {
        private readonly ISettings _settings;
        private readonly ProfileManager _profileManager;
        private readonly ConcurrentDictionary<ControllerId, Controller> _controllers =
            new ConcurrentDictionary<ControllerId, Controller>();
        private IMidiInputDevice _inputDevice;
        private IMidiOutputDevice _outputDevice;
        
        public InputDeviceInfo InputDevice => _inputDevice != null
            ? new InputDeviceInfo(_inputDevice)
            : null;

        public OutputDeviceInfo OutputDevice => _outputDevice != null
            ? new OutputDeviceInfo(_outputDevice)
            : null;

        public DeviceManager(ISettings settings, ProfileManager profileManager)
        {
            _settings = settings;
            _profileManager = profileManager;
        }
        
        public void SetInputDevice(InputDeviceInfo inputDeviceInfo)
        {
            if (_inputDevice != null)
            {
                if (_inputDevice.IsOpen)
                    _inputDevice.Close();
                
                _inputDevice.NoteOff -= InputDeviceOnNoteOff; 
                _inputDevice.NoteOn -= InputDeviceOnNoteOn;
                _inputDevice.PolyphonicKeyPressure -= InputDeviceOnPolyphonicKeyPressure;
                _inputDevice.ControlChange -= InputDeviceOnControlChange;
                _inputDevice.ProgramChange -= InputDeviceOnProgramChange;
                _inputDevice.ChannelPressure -= InputDeviceOnChannelPressure;
                _inputDevice.PitchBend -= InputDeviceOnPitchBend;
                _inputDevice.Nrpn -= InputDeviceOnNrpn;
                
                _inputDevice.Dispose();
            }

            if (inputDeviceInfo != null)
            {
                var inputDevice = inputDeviceInfo.CreateDevice();
                _inputDevice = new InputDeviceDecorator(inputDevice, _settings);
                
                _inputDevice.NoteOff += InputDeviceOnNoteOff; 
                _inputDevice.NoteOn += InputDeviceOnNoteOn;
                _inputDevice.PolyphonicKeyPressure += InputDeviceOnPolyphonicKeyPressure;
                _inputDevice.ControlChange += InputDeviceOnControlChange;
                _inputDevice.ProgramChange += InputDeviceOnProgramChange;
                _inputDevice.ChannelPressure += InputDeviceOnChannelPressure;
                _inputDevice.PitchBend += InputDeviceOnPitchBend;
                _inputDevice.Nrpn += InputDeviceOnNrpn;

                if (!_inputDevice.IsOpen)
                    _inputDevice.Open();
            }
            else
            {
                _inputDevice = null;
            }
        }

        public void SetOutputDevice(OutputDeviceInfo outputDeviceInfo)
        {
            if (_outputDevice != null)
            {
                if (_outputDevice.IsOpen)
                    _outputDevice.Close();

                _outputDevice.Dispose();
            }

            if (outputDeviceInfo != null)
            {
                _outputDevice = outputDeviceInfo.CreateDevice();
                
                if (!_outputDevice.IsOpen)
                    _outputDevice.Open();
            }
            else
            {
                _outputDevice = null;
            }
        }

        public void Clear()
        {
            _controllers.Clear();
        }

        private void OnInput(in ControllerId controllerId, int value)
        {
            _controllers.GetOrAdd(controllerId,
                    id => new Controller(id, new Range(value, value), _profileManager))
                .OnControllerInput(value);
        }
        
        private void InputDeviceOnNoteOff(IMidiInputDevice sender, in NoteOffMessage msg)
            => OnInput(new ControllerId(msg), msg.Velocity);

        private void InputDeviceOnNoteOn(IMidiInputDevice sender, in NoteOnMessage msg)
            => OnInput(new ControllerId(msg), msg.Velocity);

        private void InputDeviceOnPolyphonicKeyPressure(IMidiInputDevice sender, in PolyphonicKeyPressureMessage msg)
            => OnInput(new ControllerId(msg), msg.Pressure);

        private void InputDeviceOnControlChange(IMidiInputDevice sender, in ControlChangeMessage msg)
            => OnInput(new ControllerId(msg), msg.Value);

        private void InputDeviceOnProgramChange(IMidiInputDevice sender, in ProgramChangeMessage msg)
            => OnInput(new ControllerId(msg), msg.Program);
        
        private void InputDeviceOnChannelPressure(IMidiInputDevice sender, in ChannelPressureMessage msg)
            => OnInput(new ControllerId(msg), msg.Pressure);

        private void InputDeviceOnPitchBend(IMidiInputDevice sender, in PitchBendMessage msg)
            => OnInput(new ControllerId(msg), msg.Value);

        private void InputDeviceOnNrpn(IMidiInputDevice sender, in NrpnMessage msg)
            => OnInput(new ControllerId(msg), msg.Value);
    }
}