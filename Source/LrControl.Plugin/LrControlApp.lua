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
local Log               = require 'Logger'

local appPath = LrPathUtils.child(_PLUGIN.path, LrPathUtils.child('win', 'LrControl.ui.exe'))


return {
    Start = function() 
		Log:info("Starting LrControl application")
        LrShell.openFilesInApp ({""}, appPath)
		Log:info("...running")
    end,
    Stop = function(progressFunction) 
        if progressFunction == nil then
            progressFunction = function(progress,message)end
        end

        -- Stop main thread
		Log:info("Stopping plugin")
        progressFunction(0.00, "Stopping plugin")
        LrControl.Running = false
        Sockets.AutoReconnect = false

		-- Close LrControl application
		progressFunction(0.25, "Closing LrControl application")
		Log:info("Closing LrControl application")
        LrShell.openFilesInApp({"/shutdown"}, appPath)
		Log:info("...closed")

		progressFunction(0.50, "Closing connections...")

		-- Close ReceiveSocket?
        if Sockets.ReceiveSocket ~= nil then
			Log:debug("Closing receive socket")
            Sockets.ReceiveSocket:close()
			Log:debug("...closed")
        end

		progressFunction(0.75, "Closing connections...")

		-- Close SendSocket?
        if Sockets.SendSocket ~= nil then
			Log:debug("Closing send socket")
            Sockets.SendSocket:close()
			Log:debug("...closed")
        end

		progressFunction(1.0, "Finished")
    end
}