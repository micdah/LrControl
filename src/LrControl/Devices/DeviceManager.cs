using System.Collections.Concurrent;
using System.Collections.Generic;
using LrControl.Configurations;
using LrControl.Devices.Midi;
using LrControl.LrPlugin.Api.Common;
using RtMidi.Core.Devices;
using RtMidi.Core.Messages;

namespace LrControl.Devices
{
    public delegate void ControllerInputHandler(in ControllerId controllerId, Range range, int value);
    
    public interface IDeviceManager
    {
        IInputDeviceInfo InputDevice { get; }
        IOutputDeviceInfo OutputDevice { get; }
        IEnumerable<ControllerInfo> ControllerInfos { get; }
        event ControllerInputHandler ControllerInput;
        void SetInputDevice(IInputDeviceInfo inputDeviceInfo);
        void SetOutputDevice(IOutputDeviceInfo outputDeviceInfo);
        void Clear();   
    }

    public class DeviceManager : IDeviceManager
    {
        private readonly ISettings _settings;
        private readonly ConcurrentDictionary<ControllerId, ControllerInfo> _controllers =
            new ConcurrentDictionary<ControllerId, ControllerInfo>();
        private IMidiInputDevice _inputDevice;
        private IMidiOutputDevice _outputDevice;
        
        public IInputDeviceInfo InputDevice => _inputDevice != null
            ? new InputDeviceInfo(_inputDevice)
            : null;

        public IOutputDeviceInfo OutputDevice => _outputDevice != null
            ? new OutputDeviceInfo(_outputDevice)
            : null;

        public IEnumerable<ControllerInfo> ControllerInfos => _controllers.Values;

        public DeviceManager(ISettings settings)
        {
            _settings = settings;
        }

        public DeviceManager(ISettings settings, IEnumerable<ControllerInfo> controllerInfos) : this(settings)
        {
            foreach (var info in controllerInfos)
            {
                _controllers[info.ControllerId] = info;
            }
        }
        
        public void SetInputDevice(IInputDeviceInfo inputDeviceInfo)
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

        public void SetOutputDevice(IOutputDeviceInfo outputDeviceInfo)
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

        public event ControllerInputHandler ControllerInput;

        private void OnInput(in ControllerId controllerId, int value)
        {
            // Update controller info
            var info = _controllers.GetOrAdd(controllerId, id => new ControllerInfo(id, new Range(value, value)));
            info.Update(value);
            
            // Trigger input event
            ControllerInput?.Invoke(in controllerId, info.Range, value);
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