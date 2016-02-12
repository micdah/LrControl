using LrControlApi.Common;

namespace LrControlApi.LrSelection
{
    public class ColorLabel : ClassEnum<string, ColorLabel>
    {
        public static readonly ColorLabel Red    = new ColorLabel("Red", "red");
        public static readonly ColorLabel Yellow = new ColorLabel("Yellow", "yellow");
        public static readonly ColorLabel Green  = new ColorLabel("Green", "green");
        public static readonly ColorLabel Blue   = new ColorLabel("Blue", "blue");
        public static readonly ColorLabel Purple = new ColorLabel("Purple", "purple");
        public static readonly ColorLabel Other  = new ColorLabel("Other", "other");
        public static readonly ColorLabel None   = new ColorLabel("None", "none");

        private ColorLabel(string name, string value) : base(name, value)
        {
        }
    }
}