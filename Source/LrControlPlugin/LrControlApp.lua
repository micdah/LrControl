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
local LrShell	        = import 'LrShell'
local LrPathUtils       = import 'LrPathUtils'

local appPath = LrPathUtils.child(_PLUGIN.path, LrPathUtils.child('win', 'LrControl.exe'))


return {
    Start = function() 
        LrShell.openFilesInApp ({""}, appPath)
    end,
    Stop = function(progressFunction) 
        if progressFunction == nil then
            progressFunction = function(progress,message)end
        end

        -- Stop main thread
        progressFunction(0.25, "Stopping plugin")
        LrControl.Running = false
        Sockets.AutoReconnect = 0

        if Sockets.ReceiveSocket ~= nil then
            Sockets.ReceiveSocket:close()
        end

        if Sockets.SendSocket ~= nil then
            Sockets.SendSocket:close()
        end

        -- Close LrControl application
        progressFunction(0.75, "Closing LrControl application")
        LrShell.openFilesInApp({"/shutdown"}, appPath)
    end
}