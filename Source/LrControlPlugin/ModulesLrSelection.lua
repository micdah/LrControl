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
    clearLabels       = function()
        LrSelection.clearLabels()
    end,
    
    decreaseRating    = function()
        LrSelection.decreaseRating()
    end,
    
    deselectActive    = function()
        LrSelection.deselectActive()
    end,
    
    deselectOthers    = function()
        LrSelection.deselectOthers()
    end,
    
    extendSelection   = function(direction,amount)
        LrSelection.extendSelection(direction,amount)
    end,
    
    flagAsPicked      = function()
        LrSelection.flagAsPicked()
    end,
    
    flagAsRejected    = function()
        LrSelection.flagAsRejected()
    end,
    
    getColorLabel     = function()
        return LrSelection.getColorLabel()
    end,
    
    getFlag           = function()
        return LrSelection.getFlag()
    end,
    
    getRating         = function()
        return LrSelection.getRating()
    end,
    
    increaseRating    = function()
        LrSelection.increaseRating()
    end,
    
    nextPhoto         = function()
        LrSelection.nextPhoto()
    end,
    
    previousPhoto     = function()
        LrSelection.previousPhoto()
    end,
    
    removeFlag        = function()
        LrSelection.removeFlag()
    end,
    
    selectAll         = function()
        LrSelection.selectAll()
    end,
    
    selectFirstPhoto  = function()
        LrSelection.selectFirstPhoto()
    end,
    
    selectInverse     = function()
        LrSelection.selectInverse()
    end,
    
    selectLastPhoto   = function()
        LrSelection.selectLastPhoto()
    end,
    
    selectNone        = function()
        LrSelection.selectNone()
    end,
    
    setColorLabel     = function(label)
        LrSelection.setColorLabel(label)
    end,
    
    setRating         = function(rating)
        LrSelection.setRating(rating)
    end,
    
    toggleBlueLabel   = function()
        LrSelection.toggleBlueLabel()
    end,
    
    toggleGreenLabel  = function()
        LrSelection.toggleGreenLabel()
    end,
    
    togglePurpleLabel = function()
        LrSelection.togglePurpleLabel()
    end,
    
    toggleRedLabel    = function()
        LrSelection.toggleRedLabel()
    end,
    
    toggleYellowLabel = function()
        LrSelection.toggleYellowLabel()
    end,
    
}