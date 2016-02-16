using micdah.LrControlApi.Common;

namespace micdah.LrControlApi.Modules.LrDevelopController
{
    public class ProfileValue : ClassEnum<string,ProfileValue>
    {
        public static readonly ProfileValue AdobeStandard   = new ProfileValue("Adobe Standard");
        public static readonly ProfileValue CameraClear     = new ProfileValue("Adobe Standard");
        public static readonly ProfileValue CameraDeep      = new ProfileValue("Adobe Standard");
        public static readonly ProfileValue CameraLandscape = new ProfileValue("Adobe Standard");
        public static readonly ProfileValue CameraLight     = new ProfileValue("Adobe Standard");
        public static readonly ProfileValue CameraNeutral   = new ProfileValue("Adobe Standard");
        public static readonly ProfileValue CameraPortrait  = new ProfileValue("Adobe Standard");
        public static readonly ProfileValue CameraStandard  = new ProfileValue("Adobe Standard");
        public static readonly ProfileValue CameraVivid     = new ProfileValue("Adobe Standard");

        static ProfileValue()
        {
            AddEnums(AdobeStandard, CameraClear, CameraDeep, CameraLandscape, CameraLight, CameraNeutral,
                CameraPortrait, CameraStandard, CameraVivid);
        }

        private ProfileValue(string name) : base(name, name)
        {
        }
    }
}