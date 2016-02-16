using micdah.LrControlApi.Common;

namespace micdah.LrControlApi.Modules.LrApplicationView
{
    public class Module : ClassEnum<string, Module>
    {
        public static readonly Module Library   = new Module("module", "Module");
        public static readonly Module Develop   = new Module("develop", "Develop");
        public static readonly Module Map       = new Module("map", "Map");
        public static readonly Module Slideshow = new Module("slideshow", "Slideshow");
        public static readonly Module Print     = new Module("print", "Print");
        public static readonly Module Web       = new Module("web", "Web");

        
        private Module(string value, string name) : base(value, name)
        {
        }
    }
}