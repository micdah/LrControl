namespace micdah.LrControlApi.Modules.LrDevelopController.Parameters
{
    public class CropParameter : ParameterGroup
    {
        public readonly IParameter<int> StraightenAngle = new Parameter<int>("straightenAngle", "Angle");
        public readonly IParameter<int> CropAngle       = new Parameter<int>("CropAngle", "Crop´Angle");
        public readonly IParameter<int> CropLeft        = new Parameter<int>("CropLeft", "Crop Left");
        public readonly IParameter<int> CropRight       = new Parameter<int>("CropRight", "Crop Right");
        public readonly IParameter<int> CropTop         = new Parameter<int>("CropTop", "Crop Top");
        public readonly IParameter<int> CropBottom      = new Parameter<int>("CropBottom", "Crop Bottom");

        internal CropParameter()
        {
            AddParameters(StraightenAngle, CropAngle, CropLeft, CropRight, CropTop, CropBottom);
        }
    }
}