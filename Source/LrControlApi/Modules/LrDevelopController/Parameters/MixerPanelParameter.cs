using micdah.LrControlApi.Common;

namespace micdah.LrControlApi.Modules.LrDevelopController.Parameters
{
    public class MixerPanelParameter : Parameter<MixerPanelParameter>
    {
        public static readonly IDevelopControllerParameter<int> SaturationAdjustmentRed     = new IntParameter("SaturationAdjustmentRed", "Saturation: Red");
        public static readonly IDevelopControllerParameter<int> SaturationAdjustmentOrange  = new IntParameter("SaturationAdjustmentOrange", "Saturation: Orange");
        public static readonly IDevelopControllerParameter<int> SaturationAdjustmentYellow  = new IntParameter("SaturationAdjustmentYellow", "Saturation: Yellow");
        public static readonly IDevelopControllerParameter<int> SaturationAdjustmentGreen   = new IntParameter("SaturationAdjustmentGreen", "Saturation: Green");
        public static readonly IDevelopControllerParameter<int> SaturationAdjustmentAqua    = new IntParameter("SaturationAdjustmentAqua", "Saturation: Aqua");
        public static readonly IDevelopControllerParameter<int> SaturationAdjustmentBlue    = new IntParameter("SaturationAdjustmentBlue", "Saturation: Blue");
        public static readonly IDevelopControllerParameter<int> SaturationAdjustmentPurple  = new IntParameter("SaturationAdjustmentPurple", "Saturation: Purple");
        public static readonly IDevelopControllerParameter<int> SaturationAdjustmentMagenta = new IntParameter("SaturationAdjustmentMagenta", "Saturation: Magenta");
        public static readonly IDevelopControllerParameter<int> HueAdjustmentRed            = new IntParameter("HueAdjustmentRed", "Hue: Red");
        public static readonly IDevelopControllerParameter<int> HueAdjustmentOrange         = new IntParameter("HueAdjustmentOrange", "Hue: Orange");
        public static readonly IDevelopControllerParameter<int> HueAdjustmentYellow         = new IntParameter("HueAdjustmentYellow", "Hue: Yellow");
        public static readonly IDevelopControllerParameter<int> HueAdjustmentGreen          = new IntParameter("HueAdjustmentGreen", "Hue: Green");
        public static readonly IDevelopControllerParameter<int> HueAdjustmentAqua           = new IntParameter("HueAdjustmentAqua", "Hue: Aqua");
        public static readonly IDevelopControllerParameter<int> HueAdjustmentBlue           = new IntParameter("HueAdjustmentBlue", "Hue: Blue");
        public static readonly IDevelopControllerParameter<int> HueAdjustmentPurple         = new IntParameter("HueAdjustmentPurple", "Hue: Purple");
        public static readonly IDevelopControllerParameter<int> HueAdjustmentMagenta        = new IntParameter("HueAdjustmentMagenta", "Hue: Magenta");
        public static readonly IDevelopControllerParameter<int> LuminanceAdjustmentRed      = new IntParameter("LuminanceAdjustmentRed", "Luminance: Red");
        public static readonly IDevelopControllerParameter<int> LuminanceAdjustmentOrange   = new IntParameter("LuminanceAdjustmentOrange", "Luminance: Orange");
        public static readonly IDevelopControllerParameter<int> LuminanceAdjustmentYellow   = new IntParameter("LuminanceAdjustmentYellow", "Luminance: Yellow");
        public static readonly IDevelopControllerParameter<int> LuminanceAdjustmentGreen    = new IntParameter("LuminanceAdjustmentGreen", "Luminance: Green");
        public static readonly IDevelopControllerParameter<int> LuminanceAdjustmentAqua     = new IntParameter("LuminanceAdjustmentAqua", "Luminance: Aqua");
        public static readonly IDevelopControllerParameter<int> LuminanceAdjustmentBlue     = new IntParameter("LuminanceAdjustmentBlue", "Luminance: Blue");
        public static readonly IDevelopControllerParameter<int> LuminanceAdjustmentPurple   = new IntParameter("LuminanceAdjustmentPurple", "Luminance: Purple");
        public static readonly IDevelopControllerParameter<int> LuminanceAdjustmentMagenta  = new IntParameter("LuminanceAdjustmentMagenta", "Luminance: Magenta");
        public static readonly IDevelopControllerParameter<int> GrayMixerRed                = new IntParameter("GrayMixerRed", "Black & White Mix: Red");
        public static readonly IDevelopControllerParameter<int> GrayMixerOrange             = new IntParameter("GrayMixerOrange", "Black & White Mix: Orange");
        public static readonly IDevelopControllerParameter<int> GrayMixerYellow             = new IntParameter("GrayMixerYellow", "Black & White Mix: Yellow");
        public static readonly IDevelopControllerParameter<int> GrayMixerGreen              = new IntParameter("GrayMixerGreen", "Black & White Mix: Green");
        public static readonly IDevelopControllerParameter<int> GrayMixerAqua               = new IntParameter("GrayMixerAqua", "Black & White Mix: Aqua");
        public static readonly IDevelopControllerParameter<int> GrayMixerBlue               = new IntParameter("GrayMixerBlue", "Black & White Mix: Blue");
        public static readonly IDevelopControllerParameter<int> GrayMixerPurple             = new IntParameter("GrayMixerPurple", "Black & White Mix: Purple");
        public static readonly IDevelopControllerParameter<int> GrayMixerMagenta            = new IntParameter("GrayMixerMagenta", "Black & White Mix: Magenta");

        static MixerPanelParameter()
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

        private MixerPanelParameter(string name, string displayName) : base(name, displayName)
        {
        }

        private class IntParameter : MixerPanelParameter, IDevelopControllerParameter<int>
        {
            public IntParameter(string name, string displayName) : base(name, displayName)
            {
            }
        }
    }
}