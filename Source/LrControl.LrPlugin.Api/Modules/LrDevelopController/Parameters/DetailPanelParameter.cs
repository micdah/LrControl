namespace LrControl.LrPlugin.Api.Modules.LrDevelopController.Parameters
{
    public class DetailPanelParameter : ParameterGroup
    {
        public static readonly IParameter<int> Sharpness                          = new Parameter<int>("Sharpness", "Sharpening: Amount");
        public static readonly IParameter<double> SharpenRadius                   = new Parameter<double>("SharpenRadius", "Sharpening: Radius");
        public static readonly IParameter<int> SharpenDetail                      = new Parameter<int>("SharpenDetail", "Sharpening: Detail");
        public static readonly IParameter<int> SharpenEdgeMasking                 = new Parameter<int>("SharpenEdgeMasking", "Sharpening: Masking");
        public static readonly IParameter<int> LuminanceSmoothing                 = new Parameter<int>("LuminanceSmoothing", "Noise Reduction: Luminance");
        public static readonly IParameter<int> LuminanceNoiseReductionDetail      = new Parameter<int>("LuminanceNoiseReductionDetail", "Noise Reduction: Detail");
        public static readonly IParameter<int> LuminanceNoiseReductionContrast    = new Parameter<int>("LuminanceNoiseReductionContrast", "Noise Reduction: Contrast");
        public static readonly IParameter<int> ColorNoiseReduction                = new Parameter<int>("ColorNoiseReduction", "Noise Reduction: Color");
        public static readonly IParameter<int> ColorNoiseReductionDetail          = new Parameter<int>("ColorNoiseReductionDetail", "Noise Reduction: Detail");
        public static readonly IParameter<int> ColorNoiseReductionSmoothness      = new Parameter<int>("ColorNoiseReductionSmoothness", "Noise Reduction: Smoothness");

        internal DetailPanelParameter() : base("Detail",
            Sharpness, SharpenRadius, SharpenDetail, SharpenEdgeMasking, LuminanceSmoothing,
            LuminanceNoiseReductionDetail, LuminanceNoiseReductionContrast, ColorNoiseReduction,
            ColorNoiseReductionDetail, ColorNoiseReductionSmoothness)
        {
        }
    }
}