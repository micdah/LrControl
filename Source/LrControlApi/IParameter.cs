﻿using System;

namespace micdah.LrControlApi
{
    public interface IParameter
    {
        string Value { get; }
        string Name { get; }
        Type ValueType { get; }
        string ToString();
    }
}