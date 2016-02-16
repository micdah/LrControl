using System;
using micdah.LrControlApi.Common;

namespace micdah.LrControlApi.Modules.LrDevelopController.Parameters
{
    public class CropParameter : ParameterGroup<CropParameter>
    {
        public static readonly IParameter<int> StraightenAngle = new Parameter<int>("straightenAngle", "Angle");
        public static readonly IParameter<int> CropAngle       = new Parameter<int>("CropAngle", "Crop´Angle");
        public static readonly IParameter<int> CropLeft        = new Parameter<int>("CropLeft", "Crop Left");
        public static readonly IParameter<int> CropRight       = new Parameter<int>("CropRight", "Crop Right");
        public static readonly IParameter<int> CropTop         = new Parameter<int>("CropTop", "Crop Top");
        public static readonly IParameter<int> CropBottom      = new Parameter<int>("CropBottom", "Crop Bottom");

        static CropParameter()
        {
            AddParameters(StraightenAngle, CropAngle, CropLeft, CropRight, CropTop, CropBottom);
        }
    }
}