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

local LrApplicationView = import 'LrApplicationView'
local ModuleTools       = require 'ModuleTools'

return {
    getCurrentModuleName             = function()
        return LrApplicationView.getCurrentModuleName()
    end,
    
    getSecondaryViewName             = function()
        return LrApplicationView.getSecondaryViewName()
    end,
    
    isSecondaryDispalyOn             = function()
        return LrApplicationView.isSecondaryDispalyOn()
    end,
    
    showSecondaryView                = function(view)
        LrApplicationView.showSecondaryView(view)
    end,
    
    showView                         = function(view)
        LrApplicationView.showView(view)
    end,
    
    switchToModule                   = function(module)
        LrApplicationView.switchToModule(module)
    end,
    
    toggleSecondaryDisplay           = function()
        LrApplicationView.toggleSecondaryDisplay()
    end,
    
    toggleSecondaryDisplayFullscreen = function()
        LrApplicationView.toggleSecondaryDisplayFullscreen()
    end,
    
    toggleZoom                       = ModuleTools.RequireModule("library,develop", function()
        LrApplicationView.toggleZoom()
    end),
    
    zoomIn                           = ModuleTools.RequireModule("library,develop", function()
        LrApplicationView.zoomIn()
    end),
    
    zoomInSome                       = ModuleTools.RequireModule("library,develop", function()
        LrApplicationView.zoomInSome()
    end),
    
    zoomOut                          = ModuleTools.RequireModule("library,develop", function()
        LrApplicationView.zoomOut()
    end),
    
    zoomOutSome                      = ModuleTools.RequireModule("library,develop", function()
        LrApplicationView.zoomOutSome()
    end),
    
}