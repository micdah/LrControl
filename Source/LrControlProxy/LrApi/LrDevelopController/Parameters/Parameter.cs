using System;
using LrControlProxy.Common;

namespace LrControlProxy.LrApi.LrDevelopController.Parameters
{
    public abstract class Parameter
    {
        protected Parameter(string name, string displayName, Type valueType)
        {
            Name = name;
            DisplayName = displayName;
            ValueType = valueType;
        }

        public string Name { get; }
        public string DisplayName { get; }

        public override string ToString()
        {
            return DisplayName;
        }

        public Type ValueType { get; }
    }
}