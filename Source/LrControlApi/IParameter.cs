using System;

namespace LrControlApi
{
    public interface IParameter
    {
        string Name { get; }
        string DisplayName { get; }
        Type ValueType { get; }
        string ToString();
    }
}