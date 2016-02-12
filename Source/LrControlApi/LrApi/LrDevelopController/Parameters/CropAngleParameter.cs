namespace LrControlApi.LrApi.LrDevelopController.Parameters
{
    public class CropAngleParameter : Parameter<CropAngleParameter>, IDevelopControllerParameter
    {
        public static readonly CropAngleParameter StraightenAngle = new CropAngleParameter("straightenAngle", "Angle");

        private CropAngleParameter(string name, string displayName) : base(name, displayName, typeof(int))
        {
        }
    }
}