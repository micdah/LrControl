using System;
using System.Collections.Generic;
using System.Linq;
using LrControl.Api.Common;
using LrControl.Core.Configurations;
using LrControl.Core.Devices.Enums;
using Midi.Devices;
using Midi.Enums;
using Midi.Messages;

namespace LrControl.Core.Devices
{
    public class Device
    {
        private IInputDevice _inputDevice;
        private readonly Dictionary<ControllerKey, Controller> _controllers;

        public Device()
        {
            _controllers = new Dictionary<ControllerKey, Controller>();
        }

        public IEnumerable<Controller> Controllers => _controllers.Values;

        public IInputDevice InputDevice
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

        public IOutputDevice OutputDevice { private get; set; }

        private void InputDeviceOnControlChange(ControlChangeMessage msg)
        {
            var key = new ControllerKey(ControllerMessageType.ControlChange, msg.Channel, (int) msg.Control);
            if (_controllers.TryGetValue(key, out var controller))
            {
                controller.OnDeviceInput(msg.Value);
            }

            // TODO Add controller if not found
        }

        private void InputDeviceOnNrpn(NrpnMessage msg)
        {
            var key = new ControllerKey(ControllerMessageType.Nrpn, msg.Channel, msg.Parameter);
            if (_controllers.TryGetValue(key, out var controller))
            {
                controller.OnDeviceInput(msg.Value);
            }

            // TODO Add controller if not found
        }

        internal void OnDeviceOutput(Controller controller, int controllerValue)
        {
            if (OutputDevice == null) return;

            switch (controller.MessageType)
            {
                case ControllerMessageType.ControlChange:
                    OutputDevice.SendControlChange(controller.Channel, (Control) controller.ControlNumber,
                        controllerValue);
                    break;
                case ControllerMessageType.Nrpn:
                    OutputDevice.SendNrpn(controller.Channel, controller.ControlNumber, controllerValue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void ResetAllControls()
        {
            foreach (var controller in Controllers)
                controller.Reset();
        }

        public List<ControllerConfiguration> GetConfiguration()
        {
            return Controllers.Select(controller => controller.GetConfiguration()).ToList();
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
                    conf.ControlNumber)
                {
                    Range = new Range(conf.RangeMin, conf.RangeMax)
                };

                _controllers[new ControllerKey(controller)] = controller;
            }
        }
    }
}