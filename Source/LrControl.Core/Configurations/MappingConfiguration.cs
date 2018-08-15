using System.Collections.Generic;
using System.IO;
using LrControl.Core.Util;
using Serilog;

namespace LrControl.Core.Configurations
{
    public class MappingConfiguration
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<MappingConfiguration>();
        public static readonly string ConfigurationsFile = Path.Combine("..", "Settings", "Configuration.xml");

        public List<ControllerConfiguration> Controllers { get; set; }
        public List<ModuleConfiguration> Modules { get; set; }

        public static MappingConfiguration Load(string file)
        {
            if (Serializer.Load(file, out MappingConfiguration conf))
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