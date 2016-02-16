using LrControlApi.Common;

namespace LrControlApi.LrDevelopController
{
    public class Tool : ClassEnum<string, Tool>
    {
        public static readonly Tool Loupe               = new Tool("loupe", "Loupe");
        public static readonly Tool Crop                = new Tool("crop", "Crop");
        public static readonly Tool SpotRemoval         = new Tool("dust", "Spot Removal");
        public static readonly Tool RedEye              = new Tool("redeye", "Red Eye Correction");
        public static readonly Tool GraduatedFilter     = new Tool("gradient", "Graduated Filter");
        public static readonly Tool RadialFilter        = new Tool("circularGradient", "Radial Filter");
        public static readonly Tool AdjustmentBrush     = new Tool("localized", "Adjustment Brush");

        static Tool()
        {
            AddEnums(Loupe, Crop, SpotRemoval, RedEye, GraduatedFilter, RadialFilter, AdjustmentBrush);
        }

        private Tool(string value, string name) : base(value, name)
        {
        }
    }
}