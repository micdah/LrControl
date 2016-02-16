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
    decrement                      = ModuleTools.RequireModule("develop", function(param)
        LrDevelopController.decrement(param)
    end),
    
    getProcessVersion              = ModuleTools.RequireModule("develop", function()
        return LrDevelopController.getProcessVersion()
    end),
    
    getRange                       = ModuleTools.RequireModule("develop", function(param)
        return LrDevelopController.getRange(param)
    end),
    
    getSelectedTool                = ModuleTools.RequireModule("develop", function()
        return LrDevelopController.getSelectedTool()
    end),
    
    getValue                       = ModuleTools.RequireModule("develop", function(param)
        return LrDevelopController.getValue(param)
    end),
    
    increment                      = ModuleTools.RequireModule("develop", function(param)
        LrDevelopController.increment(param)
    end),
    
    resetAllDevelopAdjustments     = ModuleTools.RequireModule("develop", function()
        LrDevelopController.resetAllDevelopAdjustments()
    end),
    
    resetBrushing                  = ModuleTools.RequireModule("develop", function()
        LrDevelopController.resetBrushing()
    end),
    
    resetCircularGradient          = ModuleTools.RequireModule("develop", function()
        LrDevelopController.resetCircularGradient()
    end),
    
    resetCrop                      = ModuleTools.RequireModule("develop", function()
        LrDevelopController.resetCrop()
    end),
    
    resetGradient                  = ModuleTools.RequireModule("develop", function()
        LrDevelopController.resetGradient()
    end),
    
    resetRedEye                    = ModuleTools.RequireModule("develop", function()
        LrDevelopController.resetRedEye()
    end),
    
    resetSpotRemoval               = ModuleTools.RequireModule("develop", function()
        LrDevelopController.resetSpotRemoval()
    end),
    
    resetToDefault                 = ModuleTools.RequireModule("develop", function(param)
        LrDevelopController.resetToDefault(param)
    end),
    
    revealAdjustedControls         = ModuleTools.RequireModule("develop", function(reveal)
        LrDevelopController.revealAdjustedControls(reveal)
    end),
    
    revealPanel                    = ModuleTools.RequireModule("develop", function(param)
        LrDevelopController.revealPanel(param)
    end),
    
    revealpanel                    = ModuleTools.RequireModule("develop", function(panel)
        LrDevelopController.revealpanel(panel)
    end),
    
    selectTool                     = ModuleTools.RequireModule("develop", function(tool)
        LrDevelopController.selectTool(tool)
    end),
    
    setMultipleAdjustmentThreshold = ModuleTools.RequireModule("develop", function(seconds)
        LrDevelopController.setMultipleAdjustmentThreshold(seconds)
    end),
    
    setProcessVersion              = ModuleTools.RequireModule("develop", function(version)
        LrDevelopController.setProcessVersion(version)
    end),
    
    setTrackingDelay               = ModuleTools.RequireModule("develop", function(seconds)
        LrDevelopController.setTrackingDelay(seconds)
    end),
    
    setValue                       = ModuleTools.RequireModule("develop", function(param,value)
        LrDevelopController.setValue(param,value)
    end),
    
    startTracking                  = ModuleTools.RequireModule("develop", function(param)
        LrDevelopController.startTracking(param)
    end),
    
    stopTracking                   = ModuleTools.RequireModule("develop", function()
        LrDevelopController.stopTracking()
    end),
    
}