using micdah.LrControlApi.Common;

namespace micdah.LrControlApi.Modules.LrDevelopController
{
    public class ProfileValue : ClassEnum<string,ProfileValue>
    {
        public static readonly ProfileValue AdobeStandard   = new ProfileValue("Adobe Standard");
        public static readonly ProfileValue CameraFaithful  = new ProfileValue("Camera Faithful");
        public static readonly ProfileValue CameraLandscape = new ProfileValue("Camera Landscape");
        public static readonly ProfileValue CameraNeutral   = new ProfileValue("Camera Neutral");
        public static readonly ProfileValue CameraPortrait  = new ProfileValue("Camera Portrait");
        public static readonly ProfileValue CameraStandard  = new ProfileValue("Camera Standard");
        //public static readonly ProfileValue CameraClear     = new ProfileValue("CameraClear");
        //public static readonly ProfileValue CameraDeep      = new ProfileValue("CameraDeep");
        //public static readonly ProfileValue CameraLandscape = new ProfileValue("CameraLandscape");
        //public static readonly ProfileValue CameraLight     = new ProfileValue("CameraLight");
        //public static readonly ProfileValue CameraNeutral   = new ProfileValue("CameraNeutral");
        //public static readonly ProfileValue CameraPortrait  = new ProfileValue("CameraPortrait");
        //public static readonly ProfileValue CameraStandard  = new ProfileValue("CameraStandard");
        //public static readonly ProfileValue CameraVivid     = new ProfileValue("CameraVivid");

        
        private ProfileValue(string name) : base(name, name)
        {
        }
    }
}