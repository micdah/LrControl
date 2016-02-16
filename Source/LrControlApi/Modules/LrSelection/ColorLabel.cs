using micdah.LrControlApi.Common;

namespace micdah.LrControlApi.Modules.LrSelection
{
    public class ColorLabel : ClassEnum<string, ColorLabel>
    {
        public static readonly ColorLabel Red    = new ColorLabel("red", "Red");
        public static readonly ColorLabel Yellow = new ColorLabel("yellow", "Yellow");
        public static readonly ColorLabel Green  = new ColorLabel("green", "Green");
        public static readonly ColorLabel Blue   = new ColorLabel("blue", "Blue");
        public static readonly ColorLabel Purple = new ColorLabel("purple", "Purple");
        public static readonly ColorLabel Other  = new ColorLabel("other", "Other");
        public static readonly ColorLabel None   = new ColorLabel("none", "None");

        static ColorLabel()
        {
            AddEnums(Red, Yellow, Green, Blue, Purple, Other, None);
        }

        private ColorLabel(string value, string name) : base(value, name)
        {
        }
    }
}