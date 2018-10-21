namespace LrControl.LrPlugin.Api.Modules.LrDevelopController.Parameters
{
    public class CropParameter : ParameterGroup
    {
        public static readonly IParameter<double> StraightenAngle    = new Parameter<double>("straightenAngle", "Angle");
        public static readonly IParameter<double> CropAngle          = new Parameter<double>("CropAngle", "Crop Angle");
        public static readonly IParameter<double> CropLeft           = new Parameter<double>("CropLeft", "Crop Left");
        public static readonly IParameter<double> CropRight          = new Parameter<double>("CropRight", "Crop Right");
        public static readonly IParameter<double> CropTop            = new Parameter<double>("CropTop", "Crop Top");
        public static readonly IParameter<double> CropBottom         = new Parameter<double>("CropBottom", "Crop Bottom");

        internal CropParameter() : base("Crop & Straighten",
            StraightenAngle, CropAngle, CropLeft, CropRight, CropTop, CropBottom)
        {
        }
    }
}