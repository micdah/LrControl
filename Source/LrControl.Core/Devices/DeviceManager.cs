using System;
using System.Collections.Generic;
using System.Linq;
using LrControl.Api.Common;
using LrControl.Core.Configurations;
using LrControl.Core.Devices.Enums;
using RtMidi.Core.Devices;
using RtMidi.Core.Messages;

namespace LrControl.Core.Devices
{
    internal delegate void ControllerAddedHandler(Controller controller);

    internal class DeviceManager
    {
        private IMidiInputDevice _inputDevice;
        private readonly Dictionary<ControllerKey, Controller> _controllers;

        public DeviceManager()
        {
            _controllers = new Dictionary<ControllerKey, Controller>();
        }

        public IEnumerable<Controller> Controllers => _controllers.Values;

        public IMidiInputDevice InputDevice
        {
            set
            {
                if (_inputDevice != null)
                {
                    _inputDevice.ControlChange -= InputDeviceOnControlChange;
                    _inputDevice.Nrpn -= InputDeviceOnNrpn;
                }

                _inputDevice = value;

                if (_inputDevice != null)
                {
                    _inputDevice.ControlChange += InputDeviceOnControlChange;
                    _inputDevice.Nrpn += InputDeviceOnNrpn;
                }
            }
        }

        public IMidiOutputDevice OutputDevice { private get; set; }

        public event ControllerAddedHandler ControllerAdded;

        public void ResetAllControls()
        {
            foreach (var controller in _controllers.Values)
                controller.Reset();
        }

        public List<ControllerConfiguration> GetConfiguration()
        {
            return _controllers.Values.Select(x => new ControllerConfiguration(x)).ToList();
        }

        public void Clear()
        {
            _controllers.Clear();
        }

        public void Load(IEnumerable<ControllerConfiguration> controllerConfiguration)
        {
            Clear();

            foreach (var conf in controllerConfiguration)
            {
                var controller = new Controller(this, conf.MessageType, conf.ControllerType, conf.Channel,
                    conf.ControlNumber, new Range(conf.RangeMin, conf.RangeMax));

                _controllers[new ControllerKey(controller)] = controller;
            }
        }

        private void InputDeviceOnControlChange(IMidiInputDevice sender, in ControlChangeMessage message)
        {
            var key = new ControllerKey(ControllerMessageType.ControlChange, message.Channel, message.Control);
            UpdateControllerValue(key, message.Value);
        }

        private void InputDeviceOnNrpn(IMidiInputDevice sender, in NrpnMessage message)
        {
            var key = new ControllerKey(ControllerMessageType.Nrpn, message.Channel, message.Parameter);
            UpdateControllerValue(key, message.Value);
        }

        private void UpdateControllerValue(ControllerKey key, int value)
        {
            // Get controller or create new if not previously seen
            if (!_controllers.TryGetValue(key, out var controller))
            {
                _controllers[key] = controller = new Controller(this, key.ControllerMessageType, ControllerType.Encoder,
                    key.Channel, key.ControlNumber, new Range(value, value));

                OnControllerAdded(controller);
            }

            controller.OnDeviceInput(value);
        }

        internal void OnDeviceOutput(Controller controller, int controllerValue)
        {
            if (OutputDevice == null) return;

            switch (controller.MessageType)
            {
                case ControllerMessageType.ControlChange:
                    OutputDevice.Send(new ControlChangeMessage(controller.MidiChannel, controller.ControlNumber,
                        controllerValue));
                    break;
                case ControllerMessageType.Nrpn:
                    OutputDevice.Send(new NrpnMessage(controller.MidiChannel, controller.ControlNumber, controllerValue));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnControllerAdded(Controller controller)
        {
            ControllerAdded?.Invoke(controller);
        }
    }
}