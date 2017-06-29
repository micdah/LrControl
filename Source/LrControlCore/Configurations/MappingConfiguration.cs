using System.Collections.Generic;
using LrControlCore.Util;
using Serilog;

namespace LrControlCore.Configurations
{
    public class MappingConfiguration
    {
        private static readonly ILogger Log = Serilog.Log.Logger.ForContext<MappingConfiguration>();
        public const string ConfigurationsFile = @"..\Settings\Configuration.xml";

        public List<ControllerConfiguration> Controllers { get; set; }
        public List<ModuleConfiguration> Modules { get; set; }

        public static MappingConfiguration Load(string file)
        {

            MappingConfiguration conf;
            if (Serializer.Load(file, out conf))
            {
                return conf;
            }
            return null;
        }

        public static void Save(MappingConfiguration conf, string file)
        {
            Serializer.Save(file, conf);
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