using System.Collections.Generic;
using micdah.LrControl.Core;

namespace micdah.LrControl.Configurations
{
    public class MappingConfiguration
    {
        private const string ConfigurationsFile = @"..\Settings\Configuration.xml";

        public MappingConfiguration()
        {
        }

        public List<ControllerConfiguration> Controllers { get; set; }
        public List<ModuleConfiguration> Modules { get; set; }

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

    public class ModuleConfiguration
    {
        public string ModuleName { get; set; }
        public List<FunctionGroupConfiguration> FunctionGroups { get; set; }
    }

    public class FunctionGroupConfiguration
    {
        public string Key { get; set; }
        public List<ControllerFunctionConfiguration> ControllerFunctions { get; set; }
    }

    public class ControllerFunctionConfiguration
    {
        public ControllerConfigurationKey ControllerKey { get; set; }
        public string FunctionKey { get; set; }
    }
}