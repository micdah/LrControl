using System;

namespace micdah.LrControlApi.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Interface)]
    internal class NativeModuleAttribute : Attribute
    {
        public string Module;

        public NativeModuleAttribute(string module)
        {
            if (string.IsNullOrEmpty(module))
                throw new ArgumentException("Must not be null or empty", nameof(module));

            Module = module;
        }
    }
}