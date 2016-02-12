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
local LrTasks			= import 'LrTasks'
local LrShell	        = import 'LrShell'
local LrDialogs         = import 'LrDialogs'
local LrPathUtils       = import 'LrPathUtils'
local LrFunctionContext = import 'LrFunctionContext'
local LrSocket          = import 'LrSocket'

local Options = {
    MessageReceivePort = 52008,
    MessageSendPort    = 52009
}


-- Track loaded version, to detect reloads
math.randomseed(os.time())
currentLoadVersion = rawget (_G, "currentLoadVersion") or math.random ()
currentLoadVersion = currentLoadVersion + 1

-- Main task
local function main(context)
    LrDialogs.showBezel ("LrControl running, loaded version " .. currentLoadVersion)
      
    -- Open sockets  
    local recieveClient = LrSocket.bind {
        functionContext = context,
        plugin			= _PLUGIN,
        port			= Options.MessageReceivePort,
        mode			= 'receive',
        onConnected		= function (socket) end,
        onClosed		= function(socket) socket:reconnect() end,
        onError			= function (socket, err) socket:reconnect()	end,
        onMessage		= function (_, message)
            if type (message) == "string" then
                LrDialogs.showBezel ("Received message:" .. message)
            else
                LrDialogs.showbezel("Received non-string message...")
            end
        end
    }


    -- Start LrControl application
    --LrShell.openFilesInApp ({""}, LrPathUtils.child(_PLUGIN.path, LrPathUtils.child('win', 'LrControl.exe')))


    -- Start wait loop
    local loadVersion = currentLoadVersion
    while (loadVersion == currentLoadVersion) do
        LrTasks.sleep(0.25)
    end


    -- Close sockets
    recieveClient.close()

    LrDialogs.showBezel("LrControl shutting down")
end



-- Start main task
--LrTasks.startAsyncTask(main, "LrControl Main task")
--LrFunctionContext.postAsyncTaskWithContext("LrControl Main task", main)
LrTasks.startAsyncTask(function()
    LrFunctionContext.callWithContext("LrControl context", main)
end)