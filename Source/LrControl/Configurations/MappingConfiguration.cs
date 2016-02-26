using System.Collections.Generic;
using micdah.LrControl.Mapping;
using Midi.Enums;

namespace micdah.LrControl.Configurations
{
    public class MappingConfiguration
    {
        public List<ControllerConfiguration> Controllers { get; set; }
    }

    public class ControllerConfiguration
    {
        public Channel Channel { get; set; }
        public ControllerMessageType MessageType { get; set; }
        public ControllerType Type { get; set; }
        public int ControlNumber { get; set; }
        public int RangeMin { get; set; }
        public int RangeMax { get; set; }
    }
}