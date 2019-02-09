using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using LrControl.Configurations;
using LrControl.Devices.Midi;
using LrControl.LrPlugin.Api.Common;
using RtMidi.Core.Devices;
using RtMidi.Core.Enums;
using RtMidi.Core.Messages;

namespace LrControl.Devices
{
    public delegate void InputHandler(in ControllerId controllerId, Range range, int value);
    
    public interface IDeviceManager
    {
        IInputDeviceInfo InputDevice { get; }
        IOutputDeviceInfo OutputDevice { get; }
        IEnumerable<ControllerInfo> ControllerInfos { get; }
        event InputHandler Input;
        void SetInputDevice(IInputDeviceInfo inputDeviceInfo);
        void SetOutputDevice(IOutputDeviceInfo outputDeviceInfo);
        void OnOutput(in ControllerId controllerId, int value);
        void Clear();
        bool TryGetInfo(in ControllerId controllerId, out ControllerInfo controllerInfo);
    }

    public class DeviceManager : IDeviceManager
    {
        private readonly ISettings _settings;
        private readonly ConcurrentDictionary<ControllerId, ControllerInfo> _controllers;
        private IMidiInputDevice _inputDevice;
        private IMidiOutputDevice _outputDevice;
        
        public IInputDeviceInfo InputDevice => _inputDevice != null
            ? new InputDeviceInfo(_inputDevice)
            : null;

        public IOutputDeviceInfo OutputDevice => _outputDevice != null
            ? new OutputDeviceInfo(_outputDevice)
            : null;

        public IEnumerable<ControllerInfo> ControllerInfos => _controllers.Values;
        
        public event InputHandler Input;

        public DeviceManager(ISettings settings)
        {
            _settings = settings;
            _controllers = new ConcurrentDictionary<ControllerId, ControllerInfo>();
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

        public void OnOutput(in ControllerId controllerId, int value)
        {
            if (_outputDevice == null) return;
            var channel = controllerId.Channel;
            var parameter = controllerId.Parameter;

            switch (controllerId.MessageType)
            {
                case MessageType.NoteOff:
                    _outputDevice.Send(new NoteOffMessage(channel, (Key) parameter, value));
                    break;
                case MessageType.NoteOn:
                    _outputDevice.Send(new NoteOnMessage(channel, (Key) parameter, value));
                    break;
                case MessageType.PolyphonicKeyPressure:
                    _outputDevice.Send(new PolyphonicKeyPressureMessage(channel, (Key) parameter, value));
                    break;
                case MessageType.ControlChange:
                    _outputDevice.Send(new ControlChangeMessage(channel, parameter, value));
                    break;
                case MessageType.ProgramChange:
                    _outputDevice.Send(new ProgramChangeMessage(channel, value));
                    break;
                case MessageType.ChannelPressure:
                    _outputDevice.Send(new ChannelPressureMessage(channel, value));
                    break;
                case MessageType.PitchBend:
                    _outputDevice.Send(new PitchBendMessage(channel, value));
                    break;
                case MessageType.Nrpn:
                    _outputDevice.Send(new NrpnMessage(channel, parameter, value));
                    break;
                default:
                    throw new InvalidOperationException($"Unknown MessageType {controllerId.MessageType}");
            }
        }

        public void Clear()
        {
            _controllers.Clear();
        }

        public bool TryGetInfo(in ControllerId controllerId, out ControllerInfo controllerInfo)
        {
            return _controllers.TryGetValue(controllerId, out controllerInfo);
        }

        private void OnInput(in ControllerId controllerId, int value)
        {
            // Update controller info
            var info = _controllers.GetOrAdd(controllerId, id => new ControllerInfo(id, new Range(value, value)));
            info.Update(value);
            
            // Trigger input event
            Input?.Invoke(in controllerId, info.Range, value);
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