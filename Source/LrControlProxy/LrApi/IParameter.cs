using System;

namespace LrControlProxy.LrApi
{
    public interface IParameter
    {
        string Name { get; }
        string DisplayName { get; }
        Type ValueType { get; }
        string ToString();
    }
}