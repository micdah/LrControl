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

local LrSelection = import 'LrSelection'
local ModuleTools = require 'ModuleTools'

return {
    clearLabels       = 
        ModuleTools.BeforeFunction("LrSelection.clearLabels", 
        ModuleTools.AfterFunction("LrSelection.clearLabels", 
        function()
            LrSelection.clearLabels()
        end)),
    
    decreaseRating    = 
        ModuleTools.BeforeFunction("LrSelection.decreaseRating", 
        ModuleTools.AfterFunction("LrSelection.decreaseRating", 
        function()
            LrSelection.decreaseRating()
        end)),
    
    deselectActive    = 
        ModuleTools.BeforeFunction("LrSelection.deselectActive", 
        ModuleTools.AfterFunction("LrSelection.deselectActive", 
        function()
            LrSelection.deselectActive()
        end)),
    
    deselectOthers    = 
        ModuleTools.BeforeFunction("LrSelection.deselectOthers", 
        ModuleTools.AfterFunction("LrSelection.deselectOthers", 
        function()
            LrSelection.deselectOthers()
        end)),
    
    extendSelection   = 
        ModuleTools.BeforeFunction("LrSelection.extendSelection", 
        ModuleTools.AfterFunction("LrSelection.extendSelection", 
        function(direction,amount)
            LrSelection.extendSelection(direction,amount)
        end)),
    
    flagAsPick        = 
        ModuleTools.BeforeFunction("LrSelection.flagAsPick", 
        ModuleTools.AfterFunction("LrSelection.flagAsPick", 
        function()
            LrSelection.flagAsPick()
        end)),
    
    flagAsReject      = 
        ModuleTools.BeforeFunction("LrSelection.flagAsReject", 
        ModuleTools.AfterFunction("LrSelection.flagAsReject", 
        function()
            LrSelection.flagAsReject()
        end)),
    
    getColorLabel     = 
        ModuleTools.BeforeFunction("LrSelection.getColorLabel", 
        ModuleTools.AfterFunction("LrSelection.getColorLabel", 
        function()
            return LrSelection.getColorLabel()
        end)),
    
    getFlag           = 
        ModuleTools.BeforeFunction("LrSelection.getFlag", 
        ModuleTools.AfterFunction("LrSelection.getFlag", 
        function()
            return LrSelection.getFlag()
        end)),
    
    getRating         = 
        ModuleTools.BeforeFunction("LrSelection.getRating", 
        ModuleTools.AfterFunction("LrSelection.getRating", 
        function()
            return LrSelection.getRating()
        end)),
    
    increaseRating    = 
        ModuleTools.BeforeFunction("LrSelection.increaseRating", 
        ModuleTools.AfterFunction("LrSelection.increaseRating", 
        function()
            LrSelection.increaseRating()
        end)),
    
    nextPhoto         = 
        ModuleTools.BeforeFunction("LrSelection.nextPhoto", 
        ModuleTools.AfterFunction("LrSelection.nextPhoto", 
        function()
            LrSelection.nextPhoto()
        end)),
    
    previousPhoto     = 
        ModuleTools.BeforeFunction("LrSelection.previousPhoto", 
        ModuleTools.AfterFunction("LrSelection.previousPhoto", 
        function()
            LrSelection.previousPhoto()
        end)),
    
    removeFlag        = 
        ModuleTools.BeforeFunction("LrSelection.removeFlag", 
        ModuleTools.AfterFunction("LrSelection.removeFlag", 
        function()
            LrSelection.removeFlag()
        end)),
    
    selectAll         = 
        ModuleTools.BeforeFunction("LrSelection.selectAll", 
        ModuleTools.AfterFunction("LrSelection.selectAll", 
        function()
            LrSelection.selectAll()
        end)),
    
    selectFirstPhoto  = 
        ModuleTools.BeforeFunction("LrSelection.selectFirstPhoto", 
        ModuleTools.AfterFunction("LrSelection.selectFirstPhoto", 
        function()
            LrSelection.selectFirstPhoto()
        end)),
    
    selectInverse     = 
        ModuleTools.BeforeFunction("LrSelection.selectInverse", 
        ModuleTools.AfterFunction("LrSelection.selectInverse", 
        function()
            LrSelection.selectInverse()
        end)),
    
    selectLastPhoto   = 
        ModuleTools.BeforeFunction("LrSelection.selectLastPhoto", 
        ModuleTools.AfterFunction("LrSelection.selectLastPhoto", 
        function()
            LrSelection.selectLastPhoto()
        end)),
    
    selectNone        = 
        ModuleTools.BeforeFunction("LrSelection.selectNone", 
        ModuleTools.AfterFunction("LrSelection.selectNone", 
        function()
            LrSelection.selectNone()
        end)),
    
    setColorLabel     = 
        ModuleTools.BeforeFunction("LrSelection.setColorLabel", 
        ModuleTools.AfterFunction("LrSelection.setColorLabel", 
        function(label)
            LrSelection.setColorLabel(label)
        end)),
    
    setRating         = 
        ModuleTools.BeforeFunction("LrSelection.setRating", 
        ModuleTools.AfterFunction("LrSelection.setRating", 
        function(rating)
            LrSelection.setRating(rating)
        end)),
    
    toggleBlueLabel   = 
        ModuleTools.BeforeFunction("LrSelection.toggleBlueLabel", 
        ModuleTools.AfterFunction("LrSelection.toggleBlueLabel", 
        function()
            LrSelection.toggleBlueLabel()
        end)),
    
    toggleGreenLabel  = 
        ModuleTools.BeforeFunction("LrSelection.toggleGreenLabel", 
        ModuleTools.AfterFunction("LrSelection.toggleGreenLabel", 
        function()
            LrSelection.toggleGreenLabel()
        end)),
    
    togglePurpleLabel = 
        ModuleTools.BeforeFunction("LrSelection.togglePurpleLabel", 
        ModuleTools.AfterFunction("LrSelection.togglePurpleLabel", 
        function()
            LrSelection.togglePurpleLabel()
        end)),
    
    toggleRedLabel    = 
        ModuleTools.BeforeFunction("LrSelection.toggleRedLabel", 
        ModuleTools.AfterFunction("LrSelection.toggleRedLabel", 
        function()
            LrSelection.toggleRedLabel()
        end)),
    
    toggleYellowLabel = 
        ModuleTools.BeforeFunction("LrSelection.toggleYellowLabel", 
        ModuleTools.AfterFunction("LrSelection.toggleYellowLabel", 
        function()
            LrSelection.toggleYellowLabel()
        end)),
    
}