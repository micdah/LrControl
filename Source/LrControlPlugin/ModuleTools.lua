--[[----------------------------------------------------------------------------

Copyright © 2016 Michael Dahl

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
local LrApplicationView   = import 'LrApplicationView'

local function requireModule(moduleNames,f)
    local modules = {}
    for name in string.gmatch(moduleNames,"([^,]+)") do
        modules[#modules+1] = name
    end

    return function(...)
        local inCorrectModule = false
        
        for i=1, #modules do
            inCorrectModule = inCorrectModule or LrApplicationView.getCurrentModuleName() == modules[i]
        end
        
        if inCorrectModule then
           return f(...)
        else 
            error("Not in any of the required modules: " .. modulenames)
        end
    end
end

return {
    RequireModule = requireModule
}