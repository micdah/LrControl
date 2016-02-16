namespace micdah.LrControlApi.Modules.LrDevelopController.Parameters
{
    public class EffectsPanelParameter : ParameterGroup
    {
        public readonly IParameter<PostCropVignetteStyle> PostCropVignetteStyle             = new Parameter<PostCropVignetteStyle>("PostCropVignetteStyle", "Post-Crop Vignetting: Style");
        public readonly IParameter<int> PostCropVignetteAmount                              = new Parameter<int>("PostCropVignetteAmount", "Post-Crop Vignetting: Amount");
        public readonly IParameter<int> PostCropVignetteMidpoint                            = new Parameter<int>("PostCropVignetteMidpoint", "Post-Crop Vignetting: Midpoint");
        public readonly IParameter<int> PostCropVignetteRoundness                           = new Parameter<int>("PostCropVignetteRoundness", "Post-Crop Vignetting: Roundness");
        public readonly IParameter<int> PostCropVignetteFeather                             = new Parameter<int>("PostCropVignetteFeather", "Post-Crop Vignetting: Feather");
        public readonly IParameter<int> PostCropVignetteHighlightContrast                   = new Parameter<int>("PostCropVignetteHighlightContrast", "Post-Crop Vignetting: Highlights");
        public readonly IParameter<int> GrainAmount                                         = new Parameter<int>("GrainAmount", "Grain: Amount");
        public readonly IParameter<int> GrainSize                                           = new Parameter<int>("GrainSize", "Grain: Size");
        public readonly IParameter<int> GrainFrequency                                      = new Parameter<int>("GrainFrequency", "Grain: Roughness");
        public readonly IParameter<int> Dehaze                                              = new Parameter<int>("Dehaze", "Dehaze: Amount");

        internal EffectsPanelParameter() : base("Effects")
        {
            AddParameters(PostCropVignetteStyle, PostCropVignetteAmount, PostCropVignetteMidpoint,
                PostCropVignetteRoundness, PostCropVignetteFeather, PostCropVignetteHighlightContrast, GrainAmount,
                GrainSize, GrainFrequency, Dehaze);
        }
    }
}