using micdah.LrControlApi.Common;

namespace micdah.LrControlApi.Modules.LrDevelopController.Parameters
{
    public class EffectsPanelParameter : Parameter<EffectsPanelParameter>
    {
        public static readonly IDevelopControllerParameter<int> PostCropVignetteStyle             = new IntParameter("PostCropVignetteStyle", "Post-Crop Vignetting: Style");
        public static readonly IDevelopControllerParameter<int> PostCropVignetteAmount            = new IntParameter("PostCropVignetteAmount", "Post-Crop Vignetting: Amount");
        public static readonly IDevelopControllerParameter<int> PostCropVignetteMidpoint          = new IntParameter("PostCropVignetteMidpoint", "Post-Crop Vignetting: Midpoint");
        public static readonly IDevelopControllerParameter<int> PostCropVignetteRoundness         = new IntParameter("PostCropVignetteRoundness", "Post-Crop Vignetting: Roundness");
        public static readonly IDevelopControllerParameter<int> PostCropVignetteFeather           = new IntParameter("PostCropVignetteFeather", "Post-Crop Vignetting: Feather");
        public static readonly IDevelopControllerParameter<int> PostCropVignetteHighlightContrast = new IntParameter("PostCropVignetteHighlightContrast", "Post-Crop Vignetting: Highlights");
        public static readonly IDevelopControllerParameter<int> GrainAmount                       = new IntParameter("GrainAmount", "Grain: Amount");
        public static readonly IDevelopControllerParameter<int> GrainSize                         = new IntParameter("GrainSize", "Grain: Size");
        public static readonly IDevelopControllerParameter<int> GrainFrequency                    = new IntParameter("GrainFrequency", "Grain: Roughness");
        public static readonly IDevelopControllerParameter<int> Dehaze                            = new IntParameter("Dehaze", "Dehaze: Amount");

        static EffectsPanelParameter()
        {
            AddParameters(PostCropVignetteStyle, PostCropVignetteAmount, PostCropVignetteMidpoint,
                PostCropVignetteRoundness, PostCropVignetteFeather, PostCropVignetteHighlightContrast, GrainAmount,
                GrainSize, GrainFrequency, Dehaze);
        }

        private EffectsPanelParameter(string name, string displayName) : base(name, displayName)
        {
        }

        private class IntParameter : EffectsPanelParameter, IDevelopControllerParameter<int>
        {
            public IntParameter(string name, string displayName) : base(name, displayName)
            {
            }
        }
    }
}