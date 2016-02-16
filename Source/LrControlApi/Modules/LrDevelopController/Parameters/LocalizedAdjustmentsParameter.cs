// ReSharper disable InconsistentNaming

using micdah.LrControlApi.Common;

namespace micdah.LrControlApi.Modules.LrDevelopController.Parameters
{
    public class LocalizedAdjustmentsParameter : Parameter<LocalizedAdjustmentsParameter>
    {
        public static readonly IDevelopControllerParameter<int> Temperature    = new IntParameter("local_Temperature", "Temperature");
        public static readonly IDevelopControllerParameter<int> Tint           = new IntParameter("local_Tint", "Tint");
        public static readonly IDevelopControllerParameter<int> Exposure       = new IntParameter("local_Exposure", "Exposure");
        public static readonly IDevelopControllerParameter<int> Contrast       = new IntParameter("local_Contrast", "Contrast");
        public static readonly IDevelopControllerParameter<int> Highlights     = new IntParameter("local_Highlights", "Highlights");
        public static readonly IDevelopControllerParameter<int> Shadows        = new IntParameter("local_Shadows", "Shadows");
        public static readonly IDevelopControllerParameter<int> Whites         = new IntParameter("local_Whites2012", "Whites");
        public static readonly IDevelopControllerParameter<int> Blacks         = new IntParameter("local_Blacks2012", "Blacks");
        public static readonly IDevelopControllerParameter<int> Clarity        = new IntParameter("local_Clarity", "Clarity");
        public static readonly IDevelopControllerParameter<int> Dehaze         = new IntParameter("local_Dehaze", "Dehaze");
        public static readonly IDevelopControllerParameter<int> Saturation     = new IntParameter("local_Saturation", "Saturation");
        public static readonly IDevelopControllerParameter<int> Sharpness      = new IntParameter("local_Sharpness", "Sharpness");
        public static readonly IDevelopControllerParameter<int> LuminanceNoise = new IntParameter("local_LuminanceNoise", "Noise");
        public static readonly IDevelopControllerParameter<int> Moire          = new IntParameter("local_Moire", "Moiré");
        public static readonly IDevelopControllerParameter<int> Defringe       = new IntParameter("local_Defringe", "Defringe");

        static LocalizedAdjustmentsParameter()
        {
            AddParameters(Temperature, Tint, Exposure, Contrast, Highlights, Shadows, Whites, Blacks, Clarity, Dehaze,
                Saturation, Sharpness, LuminanceNoise, Moire, Defringe);
        }

        private LocalizedAdjustmentsParameter(string name, string displayName) : base(name, displayName)
        {
        }

        private class IntParameter : LocalizedAdjustmentsParameter, IDevelopControllerParameter<int>
        {
            public IntParameter(string name, string displayName) : base(name, displayName)
            {
            }
        }
    }
}