namespace LrControlProxy.LrApi.LrDevelopController.Parameters
{
    public enum LensCorrectionsPanelParameter
    {
        // Profile
        LensProfileDistortionScale,
        LensProfileChromaticAberrationScale,
        LensProfileVignettingScale,
        LensManualDistortionAmount,
        // Color
        DefringePurpleAmount,
        DefringePurpleHueLo,
        DefringePurpleHueHi,
        DefringeGreenAmount,
        DefringeGreenHueLo,
        DefringeGreenHueHi,
        // Manual Perspective
        PerspectiveVertical,
        PerspectiveHorizontal,
        PerspectiveRotate,
        PerspectiveScale,
        PerspectiveAspect,
        PerspectiveUpright
    }
}