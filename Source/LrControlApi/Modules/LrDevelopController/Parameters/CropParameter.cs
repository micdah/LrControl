using System;
using micdah.LrControlApi.Common;

namespace micdah.LrControlApi.Modules.LrDevelopController.Parameters
{
    public class CropParameter : Parameter<CropParameter>
    {
        public static readonly IDevelopControllerParameter<int> StraightenAngle = new IntParameter("straightenAngle", "Angle");
        public static readonly IDevelopControllerParameter<int> CropAngle       = new IntParameter("CropAngle", "Crop´Angle");
        public static readonly IDevelopControllerParameter<int> CropLeft        = new IntParameter("CropLeft", "Crop Left");
        public static readonly IDevelopControllerParameter<int> CropRight       = new IntParameter("CropRight", "Crop Right");
        public static readonly IDevelopControllerParameter<int> CropTop         = new IntParameter("CropTop", "Crop Top");
        public static readonly IDevelopControllerParameter<int> CropBottom      = new IntParameter("CropBottom", "Crop Bottom");

        static CropParameter()
        {
            AddParameters(StraightenAngle, CropAngle, CropLeft, CropRight, CropTop, CropBottom);
        }

        private CropParameter(string name, string displayName) : base(name, displayName)
        {
        }

        private class IntParameter : CropParameter, IDevelopControllerParameter<int>
        {
            public IntParameter(string name, string displayName) : base(name, displayName)
            {
            }
        }
    }
}