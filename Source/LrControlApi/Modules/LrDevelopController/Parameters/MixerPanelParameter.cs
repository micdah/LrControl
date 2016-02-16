namespace LrControlApi.Modules.LrDevelopController.Parameters
{
    public class MixerPanelParameter : Parameter<MixerPanelParameter>, IDevelopControllerParameter
    {
        public static readonly MixerPanelParameter SaturationAdjustmentRed     = new MixerPanelParameter("SaturationAdjustmentRed", "Saturation: Red");
        public static readonly MixerPanelParameter SaturationAdjustmentOrange  = new MixerPanelParameter("SaturationAdjustmentOrange", "Saturation: Orange");
        public static readonly MixerPanelParameter SaturationAdjustmentYellow  = new MixerPanelParameter("SaturationAdjustmentYellow", "Saturation: Yellow");
        public static readonly MixerPanelParameter SaturationAdjustmentGreen   = new MixerPanelParameter("SaturationAdjustmentGreen", "Saturation: Green");
        public static readonly MixerPanelParameter SaturationAdjustmentAqua    = new MixerPanelParameter("SaturationAdjustmentAqua", "Saturation: Aqua");
        public static readonly MixerPanelParameter SaturationAdjustmentBlue    = new MixerPanelParameter("SaturationAdjustmentBlue", "Saturation: Blue");
        public static readonly MixerPanelParameter SaturationAdjustmentPurple  = new MixerPanelParameter("SaturationAdjustmentPurple", "Saturation: Purple");
        public static readonly MixerPanelParameter SaturationAdjustmentMagenta = new MixerPanelParameter("SaturationAdjustmentMagenta", "Saturation: Magenta");
        public static readonly MixerPanelParameter HueAdjustmentRed            = new MixerPanelParameter("HueAdjustmentRed", "Hue: Red");
        public static readonly MixerPanelParameter HueAdjustmentOrange         = new MixerPanelParameter("HueAdjustmentOrange", "Hue: Orange");
        public static readonly MixerPanelParameter HueAdjustmentYellow         = new MixerPanelParameter("HueAdjustmentYellow", "Hue: Yellow");
        public static readonly MixerPanelParameter HueAdjustmentGreen          = new MixerPanelParameter("HueAdjustmentGreen", "Hue: Green");
        public static readonly MixerPanelParameter HueAdjustmentAqua           = new MixerPanelParameter("HueAdjustmentAqua", "Hue: Aqua");
        public static readonly MixerPanelParameter HueAdjustmentBlue           = new MixerPanelParameter("HueAdjustmentBlue", "Hue: Blue");
        public static readonly MixerPanelParameter HueAdjustmentPurple         = new MixerPanelParameter("HueAdjustmentPurple", "Hue: Purple");
        public static readonly MixerPanelParameter HueAdjustmentMagenta        = new MixerPanelParameter("HueAdjustmentMagenta", "Hue: Magenta");
        public static readonly MixerPanelParameter LuminanceAdjustmentRed      = new MixerPanelParameter("LuminanceAdjustmentRed", "Luminance: Red");
        public static readonly MixerPanelParameter LuminanceAdjustmentOrange   = new MixerPanelParameter("LuminanceAdjustmentOrange", "Luminance: Orange");
        public static readonly MixerPanelParameter LuminanceAdjustmentYellow   = new MixerPanelParameter("LuminanceAdjustmentYellow", "Luminance: Yellow");
        public static readonly MixerPanelParameter LuminanceAdjustmentGreen    = new MixerPanelParameter("LuminanceAdjustmentGreen", "Luminance: Green");
        public static readonly MixerPanelParameter LuminanceAdjustmentAqua     = new MixerPanelParameter("LuminanceAdjustmentAqua", "Luminance: Aqua");
        public static readonly MixerPanelParameter LuminanceAdjustmentBlue     = new MixerPanelParameter("LuminanceAdjustmentBlue", "Luminance: Blue");
        public static readonly MixerPanelParameter LuminanceAdjustmentPurple   = new MixerPanelParameter("LuminanceAdjustmentPurple", "Luminance: Purple");
        public static readonly MixerPanelParameter LuminanceAdjustmentMagenta  = new MixerPanelParameter("LuminanceAdjustmentMagenta", "Luminance: Magenta");
        public static readonly MixerPanelParameter GrayMixerRed                = new MixerPanelParameter("GrayMixerRed", "Black & White Mix: Red");
        public static readonly MixerPanelParameter GrayMixerOrange             = new MixerPanelParameter("GrayMixerOrange", "Black & White Mix: Orange");
        public static readonly MixerPanelParameter GrayMixerYellow             = new MixerPanelParameter("GrayMixerYellow", "Black & White Mix: Yellow");
        public static readonly MixerPanelParameter GrayMixerGreen              = new MixerPanelParameter("GrayMixerGreen", "Black & White Mix: Green");
        public static readonly MixerPanelParameter GrayMixerAqua               = new MixerPanelParameter("GrayMixerAqua", "Black & White Mix: Aqua");
        public static readonly MixerPanelParameter GrayMixerBlue               = new MixerPanelParameter("GrayMixerBlue", "Black & White Mix: Blue");
        public static readonly MixerPanelParameter GrayMixerPurple             = new MixerPanelParameter("GrayMixerPurple", "Black & White Mix: Purple");
        public static readonly MixerPanelParameter GrayMixerMagenta            = new MixerPanelParameter("GrayMixerMagenta", "Black & White Mix: Magenta");

        private MixerPanelParameter(string name, string displayName) : base(name, displayName, typeof(int))
        {
        }
    }
}