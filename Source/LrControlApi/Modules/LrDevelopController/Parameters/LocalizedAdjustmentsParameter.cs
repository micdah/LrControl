namespace micdah.LrControlApi.Modules.LrDevelopController.Parameters
{
    public class LocalizedAdjustmentsParameter : ParameterGroup
    {
        public readonly IParameter<int> Temperature    = new Parameter<int>("local_Temperature", "Temperature");
        public readonly IParameter<int> Tint           = new Parameter<int>("local_Tint", "Tint");
        public readonly IParameter<int> Exposure       = new Parameter<int>("local_Exposure", "Exposure");
        public readonly IParameter<int> Contrast       = new Parameter<int>("local_Contrast", "Contrast");
        public readonly IParameter<int> Highlights     = new Parameter<int>("local_Highlights", "Highlights");
        public readonly IParameter<int> Shadows        = new Parameter<int>("local_Shadows", "Shadows");
        public readonly IParameter<int> Whites         = new Parameter<int>("local_Whites2012", "Whites");
        public readonly IParameter<int> Blacks         = new Parameter<int>("local_Blacks2012", "Blacks");
        public readonly IParameter<int> Clarity        = new Parameter<int>("local_Clarity", "Clarity");
        public readonly IParameter<int> Dehaze         = new Parameter<int>("local_Dehaze", "Dehaze");
        public readonly IParameter<int> Saturation     = new Parameter<int>("local_Saturation", "Saturation");
        public readonly IParameter<int> Sharpness      = new Parameter<int>("local_Sharpness", "Sharpness");
        public readonly IParameter<int> LuminanceNoise = new Parameter<int>("local_LuminanceNoise", "Noise");
        public readonly IParameter<int> Moire          = new Parameter<int>("local_Moire", "Moiré");
        public readonly IParameter<int> Defringe       = new Parameter<int>("local_Defringe", "Defringe");

        internal LocalizedAdjustmentsParameter()
        {
            AddParameters(Temperature, Tint, Exposure, Contrast, Highlights, Shadows, Whites, Blacks, Clarity, Dehaze,
                Saturation, Sharpness, LuminanceNoise, Moire, Defringe);
        }
    }
}