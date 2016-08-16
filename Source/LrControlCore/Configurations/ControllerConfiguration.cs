using LrControlCore.Device;
using LrControlCore.Device.Enums;

namespace LrControlCore.Configurations
{
    public class ControllerConfiguration : ControllerConfigurationKey
    {
        public ControllerType ControllerType { get; set; }
        public int RangeMin { get; set; }
        public int RangeMax { get; set; }
    }
}