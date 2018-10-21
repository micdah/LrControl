using LrControl.LrPlugin.Api.Common;

// ReSharper disable InconsistentNaming

namespace LrControl.LrPlugin.Api.Modules.LrDevelopController
{
    public class ProcessVersion : Enumeration<ProcessVersion,string>
    {
        public static readonly ProcessVersion PV2003 = new ProcessVersion("2003");
        public static readonly ProcessVersion PV2010 = new ProcessVersion("2010");
        public static readonly ProcessVersion PV2012 = new ProcessVersion("2012");

        
        private ProcessVersion(string name) : base(name, name)
        {
        }
    }
}