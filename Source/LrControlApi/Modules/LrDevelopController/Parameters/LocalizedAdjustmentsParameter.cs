// ReSharper disable InconsistentNaming

using micdah.LrControlApi.Common;

namespace micdah.LrControlApi.Modules.LrDevelopController.Parameters
{
    public class LocalizedAdjustmentsParameter : ParameterGroup<LocalizedAdjustmentsParameter>
    {
        public static readonly IParameter<int> Temperature    = new Parameter<int>("local_Temperature", "Temperature");
        public static readonly IParameter<int> Tint           = new Parameter<int>("local_Tint", "Tint");
        public static readonly IParameter<int> Exposure       = new Parameter<int>("local_Exposure", "Exposure");
        public static readonly IParameter<int> Contrast       = new Parameter<int>("local_Contrast", "Contrast");
        public static readonly IParameter<int> Highlights     = new Parameter<int>("local_Highlights", "Highlights");
        public static readonly IParameter<int> Shadows        = new Parameter<int>("local_Shadows", "Shadows");
        public static readonly IParameter<int> Whites         = new Parameter<int>("local_Whites2012", "Whites");
        public static readonly IParameter<int> Blacks         = new Parameter<int>("local_Blacks2012", "Blacks");
        public static readonly IParameter<int> Clarity        = new Parameter<int>("local_Clarity", "Clarity");
        public static readonly IParameter<int> Dehaze         = new Parameter<int>("local_Dehaze", "Dehaze");
        public static readonly IParameter<int> Saturation     = new Parameter<int>("local_Saturation", "Saturation");
        public static readonly IParameter<int> Sharpness      = new Parameter<int>("local_Sharpness", "Sharpness");
        public static readonly IParameter<int> LuminanceNoise = new Parameter<int>("local_LuminanceNoise", "Noise");
        public static readonly IParameter<int> Moire          = new Parameter<int>("local_Moire", "Moiré");
        public static readonly IParameter<int> Defringe       = new Parameter<int>("local_Defringe", "Defringe");

        static LocalizedAdjustmentsParameter()
        {
            AddParameters(Temperature, Tint, Exposure, Contrast, Highlights, Shadows, Whites, Blacks, Clarity, Dehaze,
                Saturation, Sharpness, LuminanceNoise, Moire, Defringe);
        }
    }
}