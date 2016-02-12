using LrControlApi.Common;

namespace LrControlApi.LrSelection
{
    public class Flag : ClassEnum<int, Flag>
    {
        public static readonly Flag None = new Flag("None", 0);
        public static readonly Flag Pick = new Flag("Pick", 1);
        public static readonly Flag Reject = new Flag("Reject", -1);
        
        private Flag(string name, int value) : base(name, value)
        {
        }
    }
}