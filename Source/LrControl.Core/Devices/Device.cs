using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using LrControl.Api.Common;
using LrControl.Core.Configurations;
using Midi.Devices;

namespace LrControl.Core.Devices
{
    public class Device
    {
        private IInputDevice _inputDevice;
        private IOutputDevice _outputDevice;

        public Device()
        {
            Controllers = new Collection<Controller>();
        }

        public ICollection<Controller> Controllers { get; private set; }

        public IInputDevice InputDevice
        {
            private get { return _inputDevice; }
            set
            {
                _inputDevice = value;

                foreach (var controller in Controllers)
                {
                    controller.InputDevice = value;
                }
            }
        }

        public IOutputDevice OutputDevice
        {
            private get { return _outputDevice; }
            set
            {
                _outputDevice = value;

                foreach (var controller in Controllers)
                {
                    controller.OutputDevice = value;
                }
            }
        }

        public void ResetAllControls()
        {
            foreach (var controller in Controllers)
            {
                controller.Reset();
            }
        }

        public List<ControllerConfiguration> GetConfiguration()
        {
            return Controllers.Select(controller => controller.GetConfiguration()).ToList();
        }

        public void Clear()
        {
            foreach (var controller in Controllers)
            {
                controller.Dispose();
            }
            Controllers.Clear();
        }

        public void Load(IEnumerable<ControllerConfiguration> controllerConfiguration)
        {
            Clear();

            foreach (var controller in controllerConfiguration)
            {
                Controllers.Add(new Controller
                {
                    Channel = controller.Channel,
                    MessageType = controller.MessageType,
                    ControlNumber = controller.ControlNumber,
                    ControllerType = controller.ControllerType,
                    Range = new Range(controller.RangeMin, controller.RangeMax),
                    InputDevice = InputDevice,
                    OutputDevice = OutputDevice
                });
            }
        }
    }
}