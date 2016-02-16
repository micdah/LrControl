using micdah.LrControlApi.Common;

namespace micdah.LrControlApi.Modules.LrDevelopController.Parameters
{
    public class DetailPanelParameter : Parameter<DetailPanelParameter>
    {
        public static readonly IDevelopControllerParameter<int> Sharpness                       = new IntParameter("Sharpness", "Sharpening: Amount");
        public static readonly IDevelopControllerParameter<int> SharpenRadius                   = new IntParameter("SharpenRadius", "Sharpening: Radius");
        public static readonly IDevelopControllerParameter<int> SharpenDetail                   = new IntParameter("SharpenDetail", "Sharpening: Detail");
        public static readonly IDevelopControllerParameter<int> SharpenEdgeMasking              = new IntParameter("SharpenEdgeMasking", "Sharpening: Masking");
        public static readonly IDevelopControllerParameter<int> LuminanceSmoothing              = new IntParameter("LuminanceSmoothing", "Noise Reduction: Luminance");
        public static readonly IDevelopControllerParameter<int> LuminanceNoiseReductionDetail   = new IntParameter("LuminanceNoiseReductionDetail", "Noise Reduction: Detail");
        public static readonly IDevelopControllerParameter<int> LuminanceNoiseReductionContrast = new IntParameter("LuminanceNoiseReductionContrast", "Noise Reduction: Contrast");
        public static readonly IDevelopControllerParameter<int> ColorNoiseReduction             = new IntParameter("ColorNoiseReduction", "Noise Reduction: Color");
        public static readonly IDevelopControllerParameter<int> ColorNoiseReductionDetail       = new IntParameter("ColorNoiseReductionDetail", "Noise Reduction: Detail");
        public static readonly IDevelopControllerParameter<int> ColorNoiseReductionSmoothness   = new IntParameter("ColorNoiseReductionSmoothness", "Noise Reduction: Smoothness");

        static DetailPanelParameter()
        {
            AddParameters(Sharpness, SharpenRadius, SharpenDetail, SharpenEdgeMasking, LuminanceSmoothing,
                LuminanceNoiseReductionDetail, LuminanceNoiseReductionContrast, ColorNoiseReduction,
                ColorNoiseReductionDetail, ColorNoiseReductionSmoothness);
        }
        
        private DetailPanelParameter(string name, string displayName) : base(name, displayName)
        {
        }

        private class IntParameter : DetailPanelParameter, IDevelopControllerParameter<int>
        {
            public IntParameter(string name, string displayName) : base(name, displayName)
            {
            }
        }
    }
}