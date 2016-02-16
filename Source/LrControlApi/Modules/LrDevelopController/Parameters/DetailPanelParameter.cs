namespace micdah.LrControlApi.Modules.LrDevelopController.Parameters
{
    public class DetailPanelParameter : ParameterGroup
    {
        public readonly IParameter<int> Sharpness                          = new Parameter<int>("Sharpness", "Sharpening: Amount");
        public readonly IParameter<double> SharpenRadius                   = new Parameter<double>("SharpenRadius", "Sharpening: Radius");
        public readonly IParameter<int> SharpenDetail                      = new Parameter<int>("SharpenDetail", "Sharpening: Detail");
        public readonly IParameter<int> SharpenEdgeMasking                 = new Parameter<int>("SharpenEdgeMasking", "Sharpening: Masking");
        public readonly IParameter<int> LuminanceSmoothing                 = new Parameter<int>("LuminanceSmoothing", "Noise Reduction: Luminance");
        public readonly IParameter<int> LuminanceNoiseReductionDetail      = new Parameter<int>("LuminanceNoiseReductionDetail", "Noise Reduction: Detail");
        public readonly IParameter<int> LuminanceNoiseReductionContrast    = new Parameter<int>("LuminanceNoiseReductionContrast", "Noise Reduction: Contrast");
        public readonly IParameter<int> ColorNoiseReduction                = new Parameter<int>("ColorNoiseReduction", "Noise Reduction: Color");
        public readonly IParameter<int> ColorNoiseReductionDetail          = new Parameter<int>("ColorNoiseReductionDetail", "Noise Reduction: Detail");
        public readonly IParameter<int> ColorNoiseReductionSmoothness      = new Parameter<int>("ColorNoiseReductionSmoothness", "Noise Reduction: Smoothness");

        internal DetailPanelParameter() : base("Detail")
        {
            AddParameters(Sharpness, SharpenRadius, SharpenDetail, SharpenEdgeMasking, LuminanceSmoothing,
                LuminanceNoiseReductionDetail, LuminanceNoiseReductionContrast, ColorNoiseReduction,
                ColorNoiseReductionDetail, ColorNoiseReductionSmoothness);
        }
    }
}