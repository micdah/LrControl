namespace LrControlApi.Modules.LrDevelopController.Parameters
{
    public class EffectsPanelParameter : Parameter<EffectsPanelParameter>, IDevelopControllerParameter
    {
        public static readonly EffectsPanelParameter PostCropVignetteStyle             = new EffectsPanelParameter("PostCropVignetteStyle", "Post-Crop Vignetting: Style");
        public static readonly EffectsPanelParameter PostCropVignetteAmount            = new EffectsPanelParameter("PostCropVignetteAmount", "Post-Crop Vignetting: Amount");
        public static readonly EffectsPanelParameter PostCropVignetteMidpoint          = new EffectsPanelParameter("PostCropVignetteMidpoint", "Post-Crop Vignetting: Midpoint");
        public static readonly EffectsPanelParameter PostCropVignetteRoundness         = new EffectsPanelParameter("PostCropVignetteRoundness", "Post-Crop Vignetting: Roundness");
        public static readonly EffectsPanelParameter PostCropVignetteFeather           = new EffectsPanelParameter("PostCropVignetteFeather", "Post-Crop Vignetting: Feather");
        public static readonly EffectsPanelParameter PostCropVignetteHighlightContrast = new EffectsPanelParameter("PostCropVignetteHighlightContrast", "Post-Crop Vignetting: Highlights");
        public static readonly EffectsPanelParameter GrainAmount                       = new EffectsPanelParameter("GrainAmount", "Grain: Amount");
        public static readonly EffectsPanelParameter GrainSize                         = new EffectsPanelParameter("GrainSize", "Grain: Size");
        public static readonly EffectsPanelParameter GrainFrequency                    = new EffectsPanelParameter("GrainFrequency", "Grain: Roughness");
        public static readonly EffectsPanelParameter Dehaze                            = new EffectsPanelParameter("Dehaze", "Dehaze: Amount");

        private EffectsPanelParameter(string value, string name) : base(name, value, typeof(int))
        {
        }
    }
}