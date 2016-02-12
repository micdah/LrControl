using LrControlApi.Common;

namespace LrControlApi.LrSelection
{
    public class Direction : ClassEnum<string, Direction>
    {
        public static readonly Direction Left  = new Direction("left", "Left");
        public static readonly Direction Right = new Direction("right", "Right");

        private Direction(string value, string name) : base(value, name)
        {
        }
    }
}