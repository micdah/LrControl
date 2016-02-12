using LrControlApi.Common;

namespace LrControlApi.LrDevelopController
{
    public class Tool : ClassEnum<string, Tool>
    {
        public static readonly Tool Loupe               = new Tool("Loupe", "loupe");
        public static readonly Tool Crop                = new Tool("Crop", "crop");
        public static readonly Tool SpotRemoval         = new Tool("Spot Removal", "dust");
        public static readonly Tool RedEye              = new Tool("Red Eye Correction", "redeye");
        public static readonly Tool GraduatedFilter     = new Tool("Graduated Filter", "gradient");
        public static readonly Tool RadialFilter        = new Tool("Radial Filter", "circularGradient");
        public static readonly Tool AdjustmentBrush     = new Tool("Adjustment Brush", "localized");

        private Tool(string name, string value) : base(name, value)
        {
        }
    }
}