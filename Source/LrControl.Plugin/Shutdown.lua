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
local Log          = require 'Logger'


return {
    LrShutdownFunction = function (doneFunction, progressFunction)
        Log:info("Shutting down LrControl")
        progressFunction (0, "Shutting down LrControl")

        local pf = function(progress,message)
            progressFunction(progress,message)
        end

        Log:debug("Calling LrControlApp.Stop")
        LrControlApp.Stop(pf)

        progressFunction (1, "Done")   
        doneFunction()
    end
}