using System;
using JetBrains.Annotations;
using LrControl.Core.Devices;
using LrControl.Core.Devices.Enums;

namespace LrControl.Core.Configurations
{
    public class ControllerConfiguration : ControllerConfigurationKey
    {
        [UsedImplicitly]
        public ControllerConfiguration()
        {
        }

        public ControllerConfiguration(Controller controller) : base(controller)
        {
            ControllerType = controller.ControllerType;
            RangeMin = Convert.ToInt32(controller.Range.Minimum);
            RangeMax = Convert.ToInt32(controller.Range.Maximum);
        }

        public ControllerType ControllerType { get; set; }
        public int RangeMin { get; set; }
        public int RangeMax { get; set; }
    }
}