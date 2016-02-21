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
local ModuleTools         = require 'ModuleTools'

return {
    decrement                      = 
        ModuleTools.RequireModule("develop", 
        ModuleTools.BeforeFunction("LrDevelopController.decrement", 
        ModuleTools.AfterFunction("LrDevelopController.decrement", 
        function(param)
            LrDevelopController.decrement(param)
        end))),
    
    getProcessVersion              = 
        ModuleTools.RequireModule("develop", 
        ModuleTools.BeforeFunction("LrDevelopController.getProcessVersion", 
        ModuleTools.AfterFunction("LrDevelopController.getProcessVersion", 
        function()
            return LrDevelopController.getProcessVersion()
        end))),
    
    getRange                       = 
        ModuleTools.RequireModule("develop", 
        ModuleTools.BeforeFunction("LrDevelopController.getRange", 
        ModuleTools.AfterFunction("LrDevelopController.getRange", 
        function(param)
            return LrDevelopController.getRange(param)
        end))),
    
    getSelectedTool                = 
        ModuleTools.RequireModule("develop", 
        ModuleTools.BeforeFunction("LrDevelopController.getSelectedTool", 
        ModuleTools.AfterFunction("LrDevelopController.getSelectedTool", 
        function()
            return LrDevelopController.getSelectedTool()
        end))),
    
    getValue                       = 
        ModuleTools.RequireModule("develop", 
        ModuleTools.BeforeFunction("LrDevelopController.getValue", 
        ModuleTools.AfterFunction("LrDevelopController.getValue", 
        function(param)
            return LrDevelopController.getValue(param)
        end))),
    
    increment                      = 
        ModuleTools.RequireModule("develop", 
        ModuleTools.BeforeFunction("LrDevelopController.increment", 
        ModuleTools.AfterFunction("LrDevelopController.increment", 
        function(param)
            LrDevelopController.increment(param)
        end))),
    
    resetAllDevelopAdjustments     = 
        ModuleTools.RequireModule("develop", 
        ModuleTools.BeforeFunction("LrDevelopController.resetAllDevelopAdjustments", 
        ModuleTools.AfterFunction("LrDevelopController.resetAllDevelopAdjustments", 
        function()
            LrDevelopController.resetAllDevelopAdjustments()
        end))),
    
    resetBrushing                  = 
        ModuleTools.RequireModule("develop", 
        ModuleTools.BeforeFunction("LrDevelopController.resetBrushing", 
        ModuleTools.AfterFunction("LrDevelopController.resetBrushing", 
        function()
            LrDevelopController.resetBrushing()
        end))),
    
    resetCircularGradient          = 
        ModuleTools.RequireModule("develop", 
        ModuleTools.BeforeFunction("LrDevelopController.resetCircularGradient", 
        ModuleTools.AfterFunction("LrDevelopController.resetCircularGradient", 
        function()
            LrDevelopController.resetCircularGradient()
        end))),
    
    resetCrop                      = 
        ModuleTools.RequireModule("develop", 
        ModuleTools.BeforeFunction("LrDevelopController.resetCrop", 
        ModuleTools.AfterFunction("LrDevelopController.resetCrop", 
        function()
            LrDevelopController.resetCrop()
        end))),
    
    resetGradient                  = 
        ModuleTools.RequireModule("develop", 
        ModuleTools.BeforeFunction("LrDevelopController.resetGradient", 
        ModuleTools.AfterFunction("LrDevelopController.resetGradient", 
        function()
            LrDevelopController.resetGradient()
        end))),
    
    resetRedEye                    = 
        ModuleTools.RequireModule("develop", 
        ModuleTools.BeforeFunction("LrDevelopController.resetRedEye", 
        ModuleTools.AfterFunction("LrDevelopController.resetRedEye", 
        function()
            LrDevelopController.resetRedEye()
        end))),
    
    resetSpotRemoval               = 
        ModuleTools.RequireModule("develop", 
        ModuleTools.BeforeFunction("LrDevelopController.resetSpotRemoval", 
        ModuleTools.AfterFunction("LrDevelopController.resetSpotRemoval", 
        function()
            LrDevelopController.resetSpotRemoval()
        end))),
    
    resetToDefault                 = 
        ModuleTools.RequireModule("develop", 
        ModuleTools.BeforeFunction("LrDevelopController.resetToDefault", 
        ModuleTools.AfterFunction("LrDevelopController.resetToDefault", 
        function(param)
            LrDevelopController.resetToDefault(param)
        end))),
    
    revealAdjustedControls         = 
        ModuleTools.RequireModule("develop", 
        ModuleTools.BeforeFunction("LrDevelopController.revealAdjustedControls", 
        ModuleTools.AfterFunction("LrDevelopController.revealAdjustedControls", 
        function(reveal)
            LrDevelopController.revealAdjustedControls(reveal)
        end))),
    
    revealPanel                    = 
        ModuleTools.RequireModule("develop", 
        ModuleTools.BeforeFunction("LrDevelopController.revealPanel", 
        ModuleTools.AfterFunction("LrDevelopController.revealPanel", 
        function(param)
            LrDevelopController.revealPanel(param)
        end))),
    
    selectTool                     = 
        ModuleTools.RequireModule("develop", 
        ModuleTools.BeforeFunction("LrDevelopController.selectTool", 
        ModuleTools.AfterFunction("LrDevelopController.selectTool", 
        function(tool)
            LrDevelopController.selectTool(tool)
        end))),
    
    setMultipleAdjustmentThreshold = 
        ModuleTools.RequireModule("develop", 
        ModuleTools.BeforeFunction("LrDevelopController.setMultipleAdjustmentThreshold", 
        ModuleTools.AfterFunction("LrDevelopController.setMultipleAdjustmentThreshold", 
        function(seconds)
            LrDevelopController.setMultipleAdjustmentThreshold(seconds)
        end))),
    
    setProcessVersion              = 
        ModuleTools.RequireModule("develop", 
        ModuleTools.BeforeFunction("LrDevelopController.setProcessVersion", 
        ModuleTools.AfterFunction("LrDevelopController.setProcessVersion", 
        function(version)
            LrDevelopController.setProcessVersion(version)
        end))),
    
    setTrackingDelay               = 
        ModuleTools.RequireModule("develop", 
        ModuleTools.BeforeFunction("LrDevelopController.setTrackingDelay", 
        ModuleTools.AfterFunction("LrDevelopController.setTrackingDelay", 
        function(seconds)
            LrDevelopController.setTrackingDelay(seconds)
        end))),
    
    setValue                       = 
        ModuleTools.RequireModule("develop", 
        ModuleTools.BeforeFunction("LrDevelopController.setValue", 
        ModuleTools.AfterFunction("LrDevelopController.setValue", 
        function(param,value)
            LrDevelopController.setValue(param,value)
        end))),
    
    startTracking                  = 
        ModuleTools.RequireModule("develop", 
        ModuleTools.BeforeFunction("LrDevelopController.startTracking", 
        ModuleTools.AfterFunction("LrDevelopController.startTracking", 
        function(param)
            LrDevelopController.startTracking(param)
        end))),
    
    stopTracking                   = 
        ModuleTools.RequireModule("develop", 
        ModuleTools.BeforeFunction("LrDevelopController.stopTracking", 
        ModuleTools.AfterFunction("LrDevelopController.stopTracking", 
        function()
            LrDevelopController.stopTracking()
        end))),
    
}