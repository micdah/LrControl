namespace micdah.LrControlApi.Modules.LrDevelopController.Parameters
{
    public class MixerPanelParameter : ParameterGroup
    {
        public readonly IParameter<int> SaturationAdjustmentRed     = new Parameter<int>("SaturationAdjustmentRed", "Saturation: Red");
        public readonly IParameter<int> SaturationAdjustmentOrange  = new Parameter<int>("SaturationAdjustmentOrange", "Saturation: Orange");
        public readonly IParameter<int> SaturationAdjustmentYellow  = new Parameter<int>("SaturationAdjustmentYellow", "Saturation: Yellow");
        public readonly IParameter<int> SaturationAdjustmentGreen   = new Parameter<int>("SaturationAdjustmentGreen", "Saturation: Green");
        public readonly IParameter<int> SaturationAdjustmentAqua    = new Parameter<int>("SaturationAdjustmentAqua", "Saturation: Aqua");
        public readonly IParameter<int> SaturationAdjustmentBlue    = new Parameter<int>("SaturationAdjustmentBlue", "Saturation: Blue");
        public readonly IParameter<int> SaturationAdjustmentPurple  = new Parameter<int>("SaturationAdjustmentPurple", "Saturation: Purple");
        public readonly IParameter<int> SaturationAdjustmentMagenta = new Parameter<int>("SaturationAdjustmentMagenta", "Saturation: Magenta");
        public readonly IParameter<int> HueAdjustmentRed            = new Parameter<int>("HueAdjustmentRed", "Hue: Red");
        public readonly IParameter<int> HueAdjustmentOrange         = new Parameter<int>("HueAdjustmentOrange", "Hue: Orange");
        public readonly IParameter<int> HueAdjustmentYellow         = new Parameter<int>("HueAdjustmentYellow", "Hue: Yellow");
        public readonly IParameter<int> HueAdjustmentGreen          = new Parameter<int>("HueAdjustmentGreen", "Hue: Green");
        public readonly IParameter<int> HueAdjustmentAqua           = new Parameter<int>("HueAdjustmentAqua", "Hue: Aqua");
        public readonly IParameter<int> HueAdjustmentBlue           = new Parameter<int>("HueAdjustmentBlue", "Hue: Blue");
        public readonly IParameter<int> HueAdjustmentPurple         = new Parameter<int>("HueAdjustmentPurple", "Hue: Purple");
        public readonly IParameter<int> HueAdjustmentMagenta        = new Parameter<int>("HueAdjustmentMagenta", "Hue: Magenta");
        public readonly IParameter<int> LuminanceAdjustmentRed      = new Parameter<int>("LuminanceAdjustmentRed", "Luminance: Red");
        public readonly IParameter<int> LuminanceAdjustmentOrange   = new Parameter<int>("LuminanceAdjustmentOrange", "Luminance: Orange");
        public readonly IParameter<int> LuminanceAdjustmentYellow   = new Parameter<int>("LuminanceAdjustmentYellow", "Luminance: Yellow");
        public readonly IParameter<int> LuminanceAdjustmentGreen    = new Parameter<int>("LuminanceAdjustmentGreen", "Luminance: Green");
        public readonly IParameter<int> LuminanceAdjustmentAqua     = new Parameter<int>("LuminanceAdjustmentAqua", "Luminance: Aqua");
        public readonly IParameter<int> LuminanceAdjustmentBlue     = new Parameter<int>("LuminanceAdjustmentBlue", "Luminance: Blue");
        public readonly IParameter<int> LuminanceAdjustmentPurple   = new Parameter<int>("LuminanceAdjustmentPurple", "Luminance: Purple");
        public readonly IParameter<int> LuminanceAdjustmentMagenta  = new Parameter<int>("LuminanceAdjustmentMagenta", "Luminance: Magenta");
        public readonly IParameter<int> GrayMixerRed                = new Parameter<int>("GrayMixerRed", "Black & White Mix: Red");
        public readonly IParameter<int> GrayMixerOrange             = new Parameter<int>("GrayMixerOrange", "Black & White Mix: Orange");
        public readonly IParameter<int> GrayMixerYellow             = new Parameter<int>("GrayMixerYellow", "Black & White Mix: Yellow");
        public readonly IParameter<int> GrayMixerGreen              = new Parameter<int>("GrayMixerGreen", "Black & White Mix: Green");
        public readonly IParameter<int> GrayMixerAqua               = new Parameter<int>("GrayMixerAqua", "Black & White Mix: Aqua");
        public readonly IParameter<int> GrayMixerBlue               = new Parameter<int>("GrayMixerBlue", "Black & White Mix: Blue");
        public readonly IParameter<int> GrayMixerPurple             = new Parameter<int>("GrayMixerPurple", "Black & White Mix: Purple");
        public readonly IParameter<int> GrayMixerMagenta            = new Parameter<int>("GrayMixerMagenta", "Black & White Mix: Magenta");

        internal MixerPanelParameter()
        {
            AddParameters(SaturationAdjustmentRed, SaturationAdjustmentOrange, SaturationAdjustmentYellow,
                SaturationAdjustmentGreen, SaturationAdjustmentAqua, SaturationAdjustmentBlue,
                SaturationAdjustmentPurple, SaturationAdjustmentMagenta, HueAdjustmentRed, HueAdjustmentOrange,
                HueAdjustmentYellow, HueAdjustmentGreen, HueAdjustmentAqua, HueAdjustmentBlue, HueAdjustmentPurple,
                HueAdjustmentMagenta, LuminanceAdjustmentRed, LuminanceAdjustmentOrange, LuminanceAdjustmentYellow,
                LuminanceAdjustmentGreen, LuminanceAdjustmentAqua, LuminanceAdjustmentBlue, LuminanceAdjustmentPurple,
                LuminanceAdjustmentMagenta, GrayMixerRed, GrayMixerOrange, GrayMixerYellow, GrayMixerGreen,
                GrayMixerAqua, GrayMixerBlue, GrayMixerPurple, GrayMixerMagenta);
        }
    }
}