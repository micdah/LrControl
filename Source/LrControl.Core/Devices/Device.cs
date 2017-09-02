using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public Device()
        {
            Controllers = new Collection<Controller>();
        }

        public ICollection<Controller> Controllers { get; }

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
            foreach (var controller in Controllers)
                if (msg.Channel == controller.Channel && (int) msg.Control == controller.ControlNumber)
                    controller.OnDeviceInput(msg.Value);
        }

        private void InputDeviceOnNrpn(NrpnMessage msg)
        {
            foreach (var controller in Controllers)
                if (msg.Channel == controller.Channel && msg.Parameter == controller.ControlNumber)
                    controller.OnDeviceInput(msg.Value);
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
            Controllers.Clear();
        }

        public void Load(IEnumerable<ControllerConfiguration> controllerConfiguration)
        {
            Clear();

            foreach (var controller in controllerConfiguration)
                Controllers.Add(new Controller(this)
                {
                    Channel = controller.Channel,
                    MessageType = controller.MessageType,
                    ControlNumber = controller.ControlNumber,
                    ControllerType = controller.ControllerType,
                    Range = new Range(controller.RangeMin, controller.RangeMax)
                });
        }
    }
}