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

local LrDialogs   = import 'LrDialogs'
local ModuleTools = require 'ModuleTools'

return {
    confirm   = function(message,info,actionVerb,cancelVerb,otherVerb)
        return LrDialogs.confirm(message,info,actionVerb,cancelVerb,otherVerb)
    end,
    
    message   = function(message,info,style)
        LrDialogs.message(message,info,style)
    end,
    
    showBezel = function(message,fadeDelay)
        LrDialogs.showBezel(message,fadeDelay)
    end,
    
    showError = function(errorString)
        LrDialogs.showError(errorString)
    end,
    
}