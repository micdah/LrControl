using System;

namespace micdah.LrControlApi.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Interface)]
    internal class LuaNativeModuleAttribute : Attribute
    {
        public string Module;

        public LuaNativeModuleAttribute(string module)
        {
            if (string.IsNullOrEmpty(module))
                throw new ArgumentException("Must not be null or empty", nameof(module));

            Module = module;
        }
    }
}