using System;
using LrControl.Functions;
using LrControl.LrPlugin.Api.Common;

namespace LrControl.Tests.Profiles
{
    public class TestFunction : IFunction
    {
        public TestFunction()
        {
            Key = Guid.NewGuid().ToString();
            DisplayName = Guid.NewGuid().ToString();
        }
        
        public string Key { get; }
        public string DisplayName { get; }
        public int ApplyCount { get; private set; } = 0;
        public bool Applied => ApplyCount > 0;
        public int LastValue { get; private set; }
        public Range LastRange { get; private set; }

        public void Apply(int value, Range range)
        {
            ApplyCount++;
            LastValue = value;
            LastRange = range;
        }
    }
}