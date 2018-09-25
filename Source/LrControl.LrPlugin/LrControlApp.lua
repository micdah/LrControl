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
local LrTasks			= import 'LrTasks'
local Log               = require 'Logger'

local appPath
local debug = false

if (WIN_ENV) then
	appPath = LrPathUtils.child(_PLUGIN.path, LrPathUtils.child('win', 'LrControl.Console.exe'))
else
	appPath = LrPathUtils.child(_PLUGIN.path, LrPathUtils.child('osx', 'LrControl.Console'))
end

return {
    Start = function() 
		Log.Info("Starting LrControl application")
		if not debug then
			--LrShell.openFilesInApp ({""}, appPath)
			LrTasks.startAsyncTask(function()
				Log.Debugf("Executing '%s'", appPath)
				local exitCode = LrTasks.execute(appPath)
				Log.Debugf("LrControl application exited: %s", exitCode)
			end)
		end
    end,
    Stop = function(progressFunction) 
		Log.Info("Stopping LrControl application")
		
		if progressFunction == nil then
            progressFunction = function(progress,message)end
		end

		-- Close LrControl application
		Log.Info("Shutting down control application")
		progressFunction(0.00, "Closing LrControl application")
		if not debug then
			LrShell.openFilesInApp({"/Terminate"}, appPath)
		end

        -- Stop main thread
		progressFunction(0.25, "Stopping LrControl application")
        LrControl.Running = false
        Sockets.AutoReconnect = false

		progressFunction(0.50, "Closing connections...")

		-- Close ReceiveSocket?
        if Sockets.ReceiveSocket ~= nil then
			Log.Debug("Closing receive socket")
            Sockets.ReceiveSocket:close()
        end

		progressFunction(0.75, "Closing connections...")

		-- Close SendSocket?
        if Sockets.SendSocket ~= nil then
			Log.Debug("Closing send socket")
            Sockets.SendSocket:close()
        end

		Log.Info("Finished shutting down LrControl application")
		progressFunction(1.0, "Finished")
    end
}