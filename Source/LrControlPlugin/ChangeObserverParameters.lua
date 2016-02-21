
--[[----------------------------------------------------------------------------

Copyright ? 2016 Michael Dahl

This file is part of LrControl.

LrControl is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

LrControl is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with LrControl.  If not, see <http://www.gnu.org/licenses/>.

------------------------------------------------------------------------------]]
local LrDevelopController = import 'LrDevelopController'

local cache = {}

local function registerValue(parameter, value)
	cache[parameter] = value
end

local function hasChanged(parameter) 
    local currentValue = LrDevelopController.getValue(parameter)
    if currentValue ~= cache[parameter] then
        registerValue(parameter, currentValue)
        return true
    else
        return false
    end
end

local parameters = {
    "WhiteBalance",
    "Temperature",
    "Tint",
    "Exposure",
    "Contrast",
    "Highlights",
    "Shadows",
    "Whites",
    "Blacks",
    "Clarity",
    "Vibrance",
    "Saturation",
    "CameraProfile",
    "ShadowTint",
    "RedHue",
    "RedSaturation",
    "GreenHue",
    "GreenSaturation",
    "BlueHue",
    "BlueSaturation",
    "straightenAngle",
    "CropAngle",
    "CropLeft",
    "CropRight",
    "CropTop",
    "CropBottom",
    "Sharpness",
    "SharpenRadius",
    "SharpenDetail",
    "SharpenEdgeMasking",
    "LuminanceSmoothing",
    "LuminanceNoiseReductionDetail",
    "LuminanceNoiseReductionContrast",
    "ColorNoiseReduction",
    "ColorNoiseReductionDetail",
    "ColorNoiseReductionSmoothness",
    "PostCropVignetteStyle",
    "PostCropVignetteAmount",
    "PostCropVignetteMidpoint",
    "PostCropVignetteRoundness",
    "PostCropVignetteFeather",
    "PostCropVignetteHighlightContrast",
    "GrainAmount",
    "GrainSize",
    "GrainFrequency",
    "Dehaze",
    "EnableToneCurve",
    "EnableColorAdjustments",
    "EnableSplitToning",
    "EnableDetail",
    "EnableLensCorrections",
    "EnableEffects",
    "EnableCalibration",
    "EnableRetouch",
    "EnableRedEye",
    "EnableGradientBasedCorrections",
    "EnableCircularGradientBasedCorrections",
    "EnablePaintBasedCorrections",
    "EnableGrayscaleMix",
    "LensProfileDistortionScale",
    "LensProfileVignettingScale",
    "LensProfileChromaticAberrationScale",
    "DefringePurpleAmount",
    "DefringePurpleHueLo",
    "DefringePurpleHueHi",
    "DefringeGreenAmount",
    "DefringeGreenHueLo",
    "DefringeGreenHueHi",
    "LensManualDistortionAmount",
    "PerspectiveVertical",
    "PerspectiveHorizontal",
    "PerspectiveRotate",
    "PerspectiveScale",
    "PerspectiveAspect",
    "PerspectiveUpright",
    "local_Temperature",
    "local_Tint",
    "local_Exposure",
    "local_Contrast",
    "local_Highlights",
    "local_Shadows",
    "local_Whites2012",
    "local_Blacks2012",
    "local_Clarity",
    "local_Dehaze",
    "local_Saturation",
    "local_Sharpness",
    "local_LuminanceNoise",
    "local_Moire",
    "local_Defringe",
    "SaturationAdjustmentRed",
    "SaturationAdjustmentOrange",
    "SaturationAdjustmentYellow",
    "SaturationAdjustmentGreen",
    "SaturationAdjustmentAqua",
    "SaturationAdjustmentBlue",
    "SaturationAdjustmentPurple",
    "SaturationAdjustmentMagenta",
    "HueAdjustmentRed",
    "HueAdjustmentOrange",
    "HueAdjustmentYellow",
    "HueAdjustmentGreen",
    "HueAdjustmentAqua",
    "HueAdjustmentBlue",
    "HueAdjustmentPurple",
    "HueAdjustmentMagenta",
    "LuminanceAdjustmentRed",
    "LuminanceAdjustmentOrange",
    "LuminanceAdjustmentYellow",
    "LuminanceAdjustmentGreen",
    "LuminanceAdjustmentAqua",
    "LuminanceAdjustmentBlue",
    "LuminanceAdjustmentPurple",
    "LuminanceAdjustmentMagenta",
    "GrayMixerRed",
    "GrayMixerOrange",
    "GrayMixerYellow",
    "GrayMixerGreen",
    "GrayMixerAqua",
    "GrayMixerBlue",
    "GrayMixerPurple",
    "GrayMixerMagenta",
    "ConvertToGrayscale",
    "SplitToningHighlightHue",
    "SplitToningHighlightSaturation",
    "SplitToningBalance",
    "SplitToningShadowHue",
    "SplitToningShadowSaturation",
    "ParametricHighlights",
    "ParametricLights",
    "ParametricDarks",
    "ParametricShadows",
    "ParametricShadowSplit",
    "ParametricMidtoneSplit",
    "ParametricHighlightSplit",
}

return {
	Parameters    = parameters,
	HasChanged    = hasChanged,
	RegisterValue = registerValue, 
}