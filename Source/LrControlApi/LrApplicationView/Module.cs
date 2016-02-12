using System;
using LrControlApi.Common;

namespace LrControlApi.LrApplicationView
{
    public class Module : ClassEnum<string, Module>
    {
        public static readonly Module Library   = new Module("Module", "module");
        public static readonly Module Develop   = new Module("Develop", "develop");
        public static readonly Module Map       = new Module("Map", "map");
        public static readonly Module Slideshow = new Module("Slideshow", "slideshow");
        public static readonly Module Print     = new Module("Print", "print");
        public static readonly Module Web       = new Module("Web", "web");

        private Module(string name, string value) : base(name, value)
        {
        }
    }
}