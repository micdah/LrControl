namespace LrControl.LrPlugin.Api.Modules.LrDevelopController.Parameters
{
    public class EffectsPanelParameter : ParameterGroup
    {
        public static readonly IEnumerationParameter<int> PostCropVignetteStyle  = EnumerationParameter<int>.Create<PostCropVignetteStyle>("PostCropVignetteStyle", "Post-Crop Vignetting: Style");
        public static readonly IParameter<int> PostCropVignetteAmount            = new Parameter<int>("PostCropVignetteAmount", "Post-Crop Vignetting: Amount");
        public static readonly IParameter<int> PostCropVignetteMidpoint          = new Parameter<int>("PostCropVignetteMidpoint", "Post-Crop Vignetting: Midpoint");
        public static readonly IParameter<int> PostCropVignetteRoundness         = new Parameter<int>("PostCropVignetteRoundness", "Post-Crop Vignetting: Roundness");
        public static readonly IParameter<int> PostCropVignetteFeather           = new Parameter<int>("PostCropVignetteFeather", "Post-Crop Vignetting: Feather");
        public static readonly IParameter<int> PostCropVignetteHighlightContrast = new Parameter<int>("PostCropVignetteHighlightContrast", "Post-Crop Vignetting: Highlights");
        public static readonly IParameter<int> GrainAmount                       = new Parameter<int>("GrainAmount", "Grain: Amount");
        public static readonly IParameter<int> GrainSize                         = new Parameter<int>("GrainSize", "Grain: Size");
        public static readonly IParameter<int> GrainFrequency                    = new Parameter<int>("GrainFrequency", "Grain: Roughness");
        public static readonly IParameter<int> Dehaze                            = new Parameter<int>("Dehaze", "Dehaze: Amount");

        internal EffectsPanelParameter() : base("Effects",
            PostCropVignetteStyle, PostCropVignetteAmount, PostCropVignetteMidpoint,
            PostCropVignetteRoundness, PostCropVignetteFeather, PostCropVignetteHighlightContrast, GrainAmount,
            GrainSize, GrainFrequency, Dehaze)
        {
        }
    }
}