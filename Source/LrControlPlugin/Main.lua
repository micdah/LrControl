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
local LrTasks			 = import 'LrTasks'
local LrDialogs          = import 'LrDialogs'
local LrFunctionContext  = import 'LrFunctionContext'
local LrSocket           = import 'LrSocket'
local LrControlApp       = require 'LrControlApp'
local Options            = require 'Options'
local Modules            = require 'Modules' 
local CommandInterpreter = require 'CommandInterpreter'


-- Track loaded version, to detect reloads
math.randomseed(os.time())
currentLoadVersion = rawget (_G, "currentLoadVersion") or math.random ()
currentLoadVersion = currentLoadVersion + 1


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
                if err == "timeout" then
                    if autoReconnect then
                        LrDialogs.showBezel("Send socket: reconnecting (" .. os.time() .. ")")
                        socket:reconnect()
                    end
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
            local status, result = pcall(CommandInterpreter.InterpretCommand, Modules,message)
            if status then
                sendSocket:send(result .. "\n")
            else
                local err = CommandInterpreter.ErrorMessage("Lua error: " .. result)
                sendSocket:send(err .. "\n")
            end
        end,
        onError         = function(socket, err)
            if err == "timeout" then
                if autoReconnect then
                    LrDialogs.showBezel("Receive socket: reconnecting (" .. os.time() .. ")") 
                    socket:reconnect() 
                end
            end
        end,
        onClosed        = function(socket) 
            if autoReconnect then
                LrDialogs.showBezel("Receive socket: reconnecting (" .. os.time() .. ")")
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
        LrTasks.sleep(1/2)
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