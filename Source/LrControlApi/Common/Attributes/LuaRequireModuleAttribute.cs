using System;

namespace micdah.LrControlApi.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class LuaRequireModuleAttribute : Attribute
    {
        public string Module;

        public LuaRequireModuleAttribute(string module)
        {
            if (string.IsNullOrEmpty(module))
                throw new ArgumentException("Must not be null or empty", nameof(module));

            Module = module;
        }
    }
}