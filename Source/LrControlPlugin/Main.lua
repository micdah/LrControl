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
local LrDialogs         = import 'LrDialogs'
local LrFunctionContext = import 'LrFunctionContext'
local LrSocket          = import 'LrSocket'
local LrControlApp      = require 'LrControlApp'
local serpent           = require 'serpent'
local Options           = require 'Options'
local Modules           = require 'Modules' 


-- Track loaded version, to detect reloads
math.randomseed(os.time())
currentLoadVersion = rawget (_G, "currentLoadVersion") or math.random ()
currentLoadVersion = currentLoadVersion + 1


local function processMessage(message, sendSocket)
    local success, result = Modules.InterpretCommand(message)
    
    if success then
        if result == nil then
            result = "ack"
        end
        
        sendSocket:send(result .."\n")
    else
        senSocket:send("unknown command\n")
    end
end


-- Main task
local function main(context)
    LrDialogs.showBezel ("LrControl running, loaded version " .. currentLoadVersion)

    local autoReconnect = true

    -- Open send socket
    local sendSocket = nil
    local function openSendSocket()
        if sendSocket ~= nil then
            sendSocket:close()
        end

        sendSocket = LrSocket.bind {
            functionContext = context,
            plugin = _PLUGIN,
            port = Options.MessageSendPort,
            mode = 'send',
            onError = function(socket, err)
                if autoReconnect then
                    socket:reconnect()
                end
            end,
        }
    end

    openSendSocket()

    -- Open recieve socket  
    local receiveSocket = LrSocket.bind {
        functionContext = context,
        plugin			= _PLUGIN,
        port			= Options.MessageReceivePort,
        mode			= 'receive',
        onMessage		= function (socket, message)
            processMessage(message, sendSocket)
        end,
        onError         = function(socket, err)
            if autoReconnect then
                socket:reconnect() 
            end
        end,
        onClosed        = function(socket) 
            if autoReconnect then
                socket:reconnect() 
                openSendSocket()
            end
        end,
    }

    
    -- Start LrControl application
    LrControlApp.Start()


    -- Start wait loop
    local loadVersion = currentLoadVersion
    while (loadVersion == currentLoadVersion) do
        LrTasks.sleep(0.25)
    end

    LrDialogs.showBezel("Stopping LrControl")

    -- Close sockets
    autoReconnect = false
    receiveSocket:close()
    if sendSocket ~= nil then
        sendSocket:close()
    end

    LrDialogs.showBezel("LrControl stopped")
end



-- Start main task
LrTasks.startAsyncTask(function()
    LrFunctionContext.callWithContext("LrControl context", main)
end)