namespace LrControl.LrPlugin.Api.Modules.LrDevelopController.Parameters
{
    public class CropParameter : ParameterGroup
    {
        public readonly IParameter<double> StraightenAngle    = new Parameter<double>("straightenAngle", "Angle");
        public readonly IParameter<double> CropAngle          = new Parameter<double>("CropAngle", "Crop Angle");
        public readonly IParameter<double> CropLeft           = new Parameter<double>("CropLeft", "Crop Left");
        public readonly IParameter<double> CropRight          = new Parameter<double>("CropRight", "Crop Right");
        public readonly IParameter<double> CropTop            = new Parameter<double>("CropTop", "Crop Top");
        public readonly IParameter<double> CropBottom         = new Parameter<double>("CropBottom", "Crop Bottom");

        internal CropParameter() : base("Crop & Straigten")
        {
            AddParameters(StraightenAngle, CropAngle, CropLeft, CropRight, CropTop, CropBottom);
        }
    }
}