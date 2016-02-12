using LrControlApi.Common;

namespace LrControlApi.LrSelection
{
    public class Direction : ClassEnum<string, Direction>
    {
        public static readonly Direction Left  = new Direction("Left", "left");
        public static readonly Direction Right = new Direction("Right", "right");

        private Direction(string name, string value) : base(name, value)
        {
        }
    }
}