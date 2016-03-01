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
local LrTasks      = import 'LrTasks'
local LrControlApp = require 'LrControlApp'


return {
    LrShutdownFunction = function (doneFunction, progressFunction)
        local totalWait = 10
        local increments = 100
        local loadVersion = nil
        
        for i=1,increments do
            progressFunction (increments/i, "Stopping LrControl: Closing connections")
            
            if i==1 then
                -- Stop application
                LrControlApp.Stop()

                -- Increment version to break main loop
                loadVersion = currentLoadVersion + 1
                currentLoadVersion = loadVersion
                
            else 
                if currentLoadVersion ~= loadVersion then
                    progressFunction(1, "Stopped LrControl")
                    break
                end
            end
            
            LrTasks.sleep(totalWait/increments)
        end
            
        doneFunction()
    end
}