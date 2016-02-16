using micdah.LrControlApi.Common;

namespace micdah.LrControlApi.Modules.LrSelection
{
    public class Flag : ClassEnum<int, Flag>
    {
        public static readonly Flag None = new Flag(0, "None");
        public static readonly Flag Pick = new Flag(1, "Pick");
        public static readonly Flag Reject = new Flag(-1, "Reject");

        
        private Flag(int value, string name) : base(value, name)
        {
        }
    }
}