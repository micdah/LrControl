using System.Collections.Generic;
using LrControl.Core.Util;
using Serilog;

namespace LrControl.Core.Configurations
{
    public class MappingConfiguration
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<MappingConfiguration>();
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
}