namespace micdah.LrControlApi.Modules.LrDevelopController.Parameters
{
    public class DetailPanelParameter : Parameter<DetailPanelParameter>, IDevelopControllerParameter
    {
        public static readonly DetailPanelParameter Sharpness                       = new DetailPanelParameter("Sharpness", "Sharpening: Amount");
        public static readonly DetailPanelParameter SharpenRadius                   = new DetailPanelParameter("SharpenRadius", "Sharpening: Radius");
        public static readonly DetailPanelParameter SharpenDetail                   = new DetailPanelParameter("SharpenDetail", "Sharpening: Detail");
        public static readonly DetailPanelParameter SharpenEdgeMasking              = new DetailPanelParameter("SharpenEdgeMasking", "Sharpening: Masking");
        public static readonly DetailPanelParameter LuminanceSmoothing              = new DetailPanelParameter("LuminanceSmoothing", "Noise Reduction: Luminance");
        public static readonly DetailPanelParameter LuminanceNoiseReductionDetail   = new DetailPanelParameter("LuminanceNoiseReductionDetail", "Noise Reduction: Detail");
        public static readonly DetailPanelParameter LuminanceNoiseReductionContrast = new DetailPanelParameter("LuminanceNoiseReductionContrast", "Noise Reduction: Contrast");
        public static readonly DetailPanelParameter ColorNoiseReduction             = new DetailPanelParameter("ColorNoiseReduction", "Noise Reduction: Color");
        public static readonly DetailPanelParameter ColorNoiseReductionDetail       = new DetailPanelParameter("ColorNoiseReductionDetail", "Noise Reduction: Detail");
        public static readonly DetailPanelParameter ColorNoiseReductionSmoothness   = new DetailPanelParameter("ColorNoiseReductionSmoothness", "Noise Reduction: Smoothness");

        static DetailPanelParameter()
        {
            AddParameters(Sharpness, SharpenRadius, SharpenDetail, SharpenEdgeMasking, LuminanceSmoothing,
                LuminanceNoiseReductionDetail, LuminanceNoiseReductionContrast, ColorNoiseReduction,
                ColorNoiseReductionDetail, ColorNoiseReductionSmoothness);
        }
        
        private DetailPanelParameter(string value, string name) : base(name, value, typeof(int))
        {
        }
    }
}