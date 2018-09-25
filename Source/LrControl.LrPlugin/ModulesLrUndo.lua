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

local LrUndo      = import 'LrUndo'
local ModuleTools = require 'ModuleTools'

return {
    canRedo = 
        ModuleTools.BeforeFunction("LrUndo.canRedo", 
        ModuleTools.AfterFunction("LrUndo.canRedo", 
        function()
            return LrUndo.canRedo()
        end)),
    
    canUndo = 
        ModuleTools.BeforeFunction("LrUndo.canUndo", 
        ModuleTools.AfterFunction("LrUndo.canUndo", 
        function()
            return LrUndo.canUndo()
        end)),
    
    redo    = 
        ModuleTools.BeforeFunction("LrUndo.redo", 
        ModuleTools.AfterFunction("LrUndo.redo", 
        function()
            LrUndo.redo()
        end)),
    
    undo    = 
        ModuleTools.BeforeFunction("LrUndo.undo", 
        ModuleTools.AfterFunction("LrUndo.undo", 
        function()
            LrUndo.undo()
        end)),
    
}