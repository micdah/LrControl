using LrControl.LrPlugin.Api.Common;

namespace LrControl.LrPlugin.Api.Modules.LrSelection
{
    public class Direction : Enumeration<Direction,string>
    {
        public static readonly Direction Left  = new Direction("left", "Left");
        public static readonly Direction Right = new Direction("right", "Right");

        private Direction(string value, string name) : base(value, name)
        {
        }
    }
}