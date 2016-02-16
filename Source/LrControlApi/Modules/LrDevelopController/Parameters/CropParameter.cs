namespace micdah.LrControlApi.Modules.LrDevelopController.Parameters
{
    public class CropParameter : Parameter<CropParameter>, IDevelopControllerParameter
    {
        public static readonly CropParameter StraightenAngle = new CropParameter("straightenAngle", "Angle");
        public static readonly CropParameter CropAngle       = new CropParameter("CropAngle", "Crop´Angle");
        public static readonly CropParameter CropLeft        = new CropParameter("CropLeft", "Crop Left");
        public static readonly CropParameter CropRight       = new CropParameter("CropRight", "Crop Right");
        public static readonly CropParameter CropTop         = new CropParameter("CropTop", "Crop Top");
        public static readonly CropParameter CropBottom      = new CropParameter("CropBottom", "Crop Bottom");

        static CropParameter()
        {
            AddParameters(StraightenAngle, CropAngle, CropLeft, CropRight, CropTop, CropBottom);
        }

        private CropParameter(string value, string name) : base(name, value, typeof(int))
        {
        }
    }
}