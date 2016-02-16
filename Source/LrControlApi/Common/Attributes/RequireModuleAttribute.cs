using System;

namespace micdah.LrControlApi.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RequireModuleAttribute : Attribute
    {
        public RequireModuleAttribute(string module)
        {
            Module = module;
        }

        public String Module { get; private set; }
    }
}