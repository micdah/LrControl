// ReSharper disable InconsistentNaming
namespace LrControlProxy.LrApi.LrDevelopController.Parameters
{
    public class LocalizedAdjustmentsParameter : Parameter<LocalizedAdjustmentsParameter>, IDevelopControllerParameter
    {
        public static readonly LocalizedAdjustmentsParameter Temperature    = new LocalizedAdjustmentsParameter("local_Temperature", "Temperature");
        public static readonly LocalizedAdjustmentsParameter Tint           = new LocalizedAdjustmentsParameter("local_Tint", "Tint");
        public static readonly LocalizedAdjustmentsParameter Exposure       = new LocalizedAdjustmentsParameter("local_Exposure", "Exposure");
        public static readonly LocalizedAdjustmentsParameter Contrast       = new LocalizedAdjustmentsParameter("local_Contrast", "Contrast");
        public static readonly LocalizedAdjustmentsParameter Highlights     = new LocalizedAdjustmentsParameter("local_Highlights", "Highlights");
        public static readonly LocalizedAdjustmentsParameter Shadows        = new LocalizedAdjustmentsParameter("local_Shadows", "Shadows");
        public static readonly LocalizedAdjustmentsParameter Whites         = new LocalizedAdjustmentsParameter("local_Whites2012", "Whites");
        public static readonly LocalizedAdjustmentsParameter Blacks         = new LocalizedAdjustmentsParameter("local_Blacks2012", "Blacks");
        public static readonly LocalizedAdjustmentsParameter Clarity        = new LocalizedAdjustmentsParameter("local_Clarity", "Clarity");
        public static readonly LocalizedAdjustmentsParameter Dehaze         = new LocalizedAdjustmentsParameter("local_Dehaze", "Dehaze");
        public static readonly LocalizedAdjustmentsParameter Saturation     = new LocalizedAdjustmentsParameter("local_Saturation", "Saturation");
        public static readonly LocalizedAdjustmentsParameter Sharpness      = new LocalizedAdjustmentsParameter("local_Sharpness", "Sharpness");
        public static readonly LocalizedAdjustmentsParameter LuminanceNoise = new LocalizedAdjustmentsParameter("local_LuminanceNoise", "Noise");
        public static readonly LocalizedAdjustmentsParameter Moire          = new LocalizedAdjustmentsParameter("local_Moire", "Moiré");
        public static readonly LocalizedAdjustmentsParameter Defringe       = new LocalizedAdjustmentsParameter("local_Defringe", "Defringe");

        private LocalizedAdjustmentsParameter(string name, string displayName) : base(name, displayName, typeof(int))
        {
        }
    }
}