using System;
using System.Collections.Generic;
using System.Linq;
using LrControl.Api.Common;
using LrControl.Core.Configurations;
using LrControl.Core.Devices.Enums;
using Midi.Devices;
using Midi.Enums;
using Midi.Messages;
using Serilog;

namespace LrControl.Core.Devices
{
    public delegate void ControllerAddedHandler(Controller controller);

    public class Device
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<Device>();
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
                    _inputDevice.ControlChange -= InputDeviceOnChannelMessage;
                    _inputDevice.Nrpn -= InputDeviceOnChannelMessage;
                }

                _inputDevice = value;

                if (_inputDevice != null)
                {
                    _inputDevice.ControlChange += InputDeviceOnChannelMessage;
                    _inputDevice.Nrpn += InputDeviceOnChannelMessage;
                }
            }
        }

        public IOutputDevice OutputDevice { private get; set; }

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

        private void InputDeviceOnChannelMessage(ChannelMessage channelMessage)
        {
            ControllerKey key;
            int value;

            switch (channelMessage)
            {
                case ControlChangeMessage controlChangeMessage:
                    key = new ControllerKey(ControllerMessageType.ControlChange, controlChangeMessage.Channel, (int) controlChangeMessage.Control);
                    value = controlChangeMessage.Value;
                    break;
                case NrpnMessage nrpnMessage:
                    key = new ControllerKey(ControllerMessageType.Nrpn, nrpnMessage.Channel, nrpnMessage.Parameter);
                    value = nrpnMessage.Value;
                    break;
                default:
                    Log.Error("Unsupported ChannelMessage {@Message}", channelMessage);
                    return;
            }

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

        private void OnControllerAdded(Controller controller)
        {
            ControllerAdded?.Invoke(controller);
        }
    }
}