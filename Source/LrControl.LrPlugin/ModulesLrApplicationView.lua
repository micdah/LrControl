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
    getCurrentModuleName             = 
        ModuleTools.BeforeFunction("LrApplicationView.getCurrentModuleName", 
        ModuleTools.AfterFunction("LrApplicationView.getCurrentModuleName", 
        function()
            return LrApplicationView.getCurrentModuleName()
        end)),
    
    getSecondaryViewName             = 
        ModuleTools.BeforeFunction("LrApplicationView.getSecondaryViewName", 
        ModuleTools.AfterFunction("LrApplicationView.getSecondaryViewName", 
        function()
            return LrApplicationView.getSecondaryViewName()
        end)),
    
    isSecondaryDispalyOn             = 
        ModuleTools.BeforeFunction("LrApplicationView.isSecondaryDispalyOn", 
        ModuleTools.AfterFunction("LrApplicationView.isSecondaryDispalyOn", 
        function()
            return LrApplicationView.isSecondaryDispalyOn()
        end)),
    
    showSecondaryView                = 
        ModuleTools.BeforeFunction("LrApplicationView.showSecondaryView", 
        ModuleTools.AfterFunction("LrApplicationView.showSecondaryView", 
        function(view)
            LrApplicationView.showSecondaryView(view)
        end)),
    
    showView                         = 
        ModuleTools.BeforeFunction("LrApplicationView.showView", 
        ModuleTools.AfterFunction("LrApplicationView.showView", 
        function(view)
            LrApplicationView.showView(view)
        end)),
    
    switchToModule                   = 
        ModuleTools.BeforeFunction("LrApplicationView.switchToModule", 
        ModuleTools.AfterFunction("LrApplicationView.switchToModule", 
        function(module)
            LrApplicationView.switchToModule(module)
        end)),
    
    toggleSecondaryDisplay           = 
        ModuleTools.BeforeFunction("LrApplicationView.toggleSecondaryDisplay", 
        ModuleTools.AfterFunction("LrApplicationView.toggleSecondaryDisplay", 
        function()
            LrApplicationView.toggleSecondaryDisplay()
        end)),
    
    toggleSecondaryDisplayFullscreen = 
        ModuleTools.BeforeFunction("LrApplicationView.toggleSecondaryDisplayFullscreen", 
        ModuleTools.AfterFunction("LrApplicationView.toggleSecondaryDisplayFullscreen", 
        function()
            LrApplicationView.toggleSecondaryDisplayFullscreen()
        end)),
    
    toggleZoom                       = 
        ModuleTools.RequireModule("library,develop", 
        ModuleTools.BeforeFunction("LrApplicationView.toggleZoom", 
        ModuleTools.AfterFunction("LrApplicationView.toggleZoom", 
        function()
            LrApplicationView.toggleZoom()
        end))),
    
    zoomIn                           = 
        ModuleTools.RequireModule("library,develop", 
        ModuleTools.BeforeFunction("LrApplicationView.zoomIn", 
        ModuleTools.AfterFunction("LrApplicationView.zoomIn", 
        function()
            LrApplicationView.zoomIn()
        end))),
    
    zoomInSome                       = 
        ModuleTools.RequireModule("library,develop", 
        ModuleTools.BeforeFunction("LrApplicationView.zoomInSome", 
        ModuleTools.AfterFunction("LrApplicationView.zoomInSome", 
        function()
            LrApplicationView.zoomInSome()
        end))),
    
    zoomOut                          = 
        ModuleTools.RequireModule("library,develop", 
        ModuleTools.BeforeFunction("LrApplicationView.zoomOut", 
        ModuleTools.AfterFunction("LrApplicationView.zoomOut", 
        function()
            LrApplicationView.zoomOut()
        end))),
    
    zoomOutSome                      = 
        ModuleTools.RequireModule("library,develop", 
        ModuleTools.BeforeFunction("LrApplicationView.zoomOutSome", 
        ModuleTools.AfterFunction("LrApplicationView.zoomOutSome", 
        function()
            LrApplicationView.zoomOutSome()
        end))),
    
}