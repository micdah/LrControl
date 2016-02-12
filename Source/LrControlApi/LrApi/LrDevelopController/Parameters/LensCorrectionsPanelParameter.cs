﻿using System;
using LrControlApi.Common;

namespace LrControlApi.LrApi.LrDevelopController.Parameters
{
    public class LensCorrectionsPanelParameter : Parameter<LensCorrectionsPanelParameter>, IDevelopControllerParameter
    {
        public static readonly LensCorrectionsPanelParameter LensProfileDistortionScale          = new LensCorrectionsPanelParameter("LensProfileDistortionScale", "Profile: Distortion Scale");
        public static readonly LensCorrectionsPanelParameter LensProfileVignettingScale          = new LensCorrectionsPanelParameter("LensProfileVignettingScale", "Profile: Vignetting Scale");
        public static readonly LensCorrectionsPanelParameter LensProfileChromaticAberrationScale = new LensCorrectionsPanelParameter("LensProfileChromaticAberrationScale", "Profile: Chromatic Aberration Scale");
        public static readonly LensCorrectionsPanelParameter DefringePurpleAmount                = new LensCorrectionsPanelParameter("DefringePurpleAmount", "Defringe: Purple Amount");
        public static readonly LensCorrectionsPanelParameter DefringePurpleHueLo                 = new LensCorrectionsPanelParameter("DefringePurpleHueLo", "Defringe: Purple Hue (Low)");
        public static readonly LensCorrectionsPanelParameter DefringePurpleHueHi                 = new LensCorrectionsPanelParameter("DefringePurpleHueHi", "Defringe: Purple Hue (High)");
        public static readonly LensCorrectionsPanelParameter DefringeGreenAmount                 = new LensCorrectionsPanelParameter("DefringeGreenAmount", "Defringe: Green Amount");
        public static readonly LensCorrectionsPanelParameter DefringeGreenHueLo                  = new LensCorrectionsPanelParameter("DefringeGreenHueLo", "Defringe: Green Hue (Low)");
        public static readonly LensCorrectionsPanelParameter DefringeGreenHueHi                  = new LensCorrectionsPanelParameter("DefringeGreenHueHi", "Defringe: Green Hue (High)");
        public static readonly LensCorrectionsPanelParameter LensManualDistortionAmount          = new LensCorrectionsPanelParameter("LensManualDistortionAmount", "Transform: Distortion");
        public static readonly LensCorrectionsPanelParameter PerspectiveVertical                 = new LensCorrectionsPanelParameter("PerspectiveVertical", "Transform: Vertical");
        public static readonly LensCorrectionsPanelParameter PerspectiveHorizontal               = new LensCorrectionsPanelParameter("PerspectiveHorizontal", "Transform: Horizontal");
        public static readonly LensCorrectionsPanelParameter PerspectiveRotate                   = new LensCorrectionsPanelParameter("PerspectiveRotate", "Transform: Rotate");
        public static readonly LensCorrectionsPanelParameter PerspectiveScale                    = new LensCorrectionsPanelParameter("PerspectiveScale", "Transform: Scale");
        public static readonly LensCorrectionsPanelParameter PerspectiveAspect                   = new LensCorrectionsPanelParameter("PerspectiveAspect", "Transform: Aspect");
        public static readonly LensCorrectionsPanelParameter PerspectiveUpright                  = new LensCorrectionsPanelParameter("PerspectiveUpright", "Upright", typeof(UprightValue));

        private LensCorrectionsPanelParameter(string name, string displayName, Type valueType) : base(name, displayName, valueType)
        {
        }

        private LensCorrectionsPanelParameter(string name, string displayName) : base(name, displayName, typeof(int))
        {
        }

        public class UprightValue : ClassEnum<int,UprightValue>
        {
            public static readonly UprightValue Off = new UprightValue("Auto", 0);
            public static readonly UprightValue Auto = new UprightValue("Auto", 1);
            public static readonly UprightValue Full = new UprightValue("Auto", 2);
            public static readonly UprightValue Level = new UprightValue("Auto", 3);
            public static readonly UprightValue Vertical = new UprightValue("Auto", 4);

            private UprightValue(string name, int value) : base(name, value)
            {
            }
        }
    }
}