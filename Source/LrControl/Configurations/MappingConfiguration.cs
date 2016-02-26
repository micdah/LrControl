using System.Collections.Generic;
using micdah.LrControl.Core;
using micdah.LrControl.Mapping;
using Midi.Enums;

namespace micdah.LrControl.Configurations
{
    public class MappingConfiguration
    {
        private const string ConfigurationsFile = @"..\Settings\Configuration.xml";

        public MappingConfiguration()
        {
        }

        public List<ControllerConfiguration> Controllers { get; set; }

        public static MappingConfiguration Load()
        {
            MappingConfiguration conf;
            if (Serializer.Load(ConfigurationsFile, out conf))
            {
                return conf;
            }
            return null;
        }

        public static void Save(MappingConfiguration conf)
        {
            Serializer.Save(ConfigurationsFile, conf);
        }
    }

    public class ControllerConfiguration
    {
        public Channel Channel { get; set; }
        public ControllerMessageType MessageType { get; set; }
        public ControllerType ControllerType { get; set; }
        public int ControlNumber { get; set; }
        public int RangeMin { get; set; }
        public int RangeMax { get; set; }
    }
}