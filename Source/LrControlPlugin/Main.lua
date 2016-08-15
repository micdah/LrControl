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
local LrTasks			       = import 'LrTasks'
local LrDialogs                = import 'LrDialogs'
local LrFunctionContext        = import 'LrFunctionContext'
local LrSocket                 = import 'LrSocket'
local LrApplicationView        = import 'LrApplicationView'
local LrDevelopController      = import 'LrDevelopController' 
local LrControlApp             = require 'LrControlApp'
local Options                  = require 'Options'
local Modules                  = require 'Modules' 
local CommandInterpreter       = require 'CommandInterpreter'
local ChangeObserverParameters = require 'ChangeObserverParameters'
local ModuleTools              = require 'ModuleTools'

Sockets = {
    SendSocket = nil,
    ReceiveSocket = nil,
    AutoReconnect = true,
}

LrControl = {
    Running = true,
}

-- Main task
local function main(context)
    -- Open send socket
    local function openSendSocket()
        if Sockets.SendSocket ~= nil then
            Sockets.SendSocket:close()
        end

        Sockets.SendSocket = LrSocket.bind {
            functionContext = context,
            plugin = _PLUGIN,
            port = Options.MessageSendPort,
            mode = 'send',
            onError = function(socket, err)
                if Sockets.AutoReconnect then
                    socket:reconnect()
                end
            end,
            onClosed = function(socket)
                if Sockets.AutoReconnect then
                    socket:reconnect()
                end
            end
        }
    end

    openSendSocket()

    -- Open recieve socket  
    Sockets.ReceiveSocket = LrSocket.bind {
        functionContext = context,
        plugin			= _PLUGIN,
        port			= Options.MessageReceivePort,
        mode			= 'receive',
        onMessage		= function (socket, message)
            local status, result = pcall(CommandInterpreter.InterpretCommand, Modules,message)
            if status then
                Sockets.SendSocket:send(result .. "\n")
            else
                local err = CommandInterpreter.ErrorMessage("Lua error: " .. result)
                Sockets.SendSocket:send(err .. "\n")
            end
        end,
        onError         = function(socket, err)
            if err == "timeout" then
                if Sockets.AutoReconnect then
                    socket:reconnect()
                end
            end
        end,
        onClosed        = function(socket) 
            if Sockets.AutoReconnect then
                socket:reconnect()
                --openSendSocket()
            end
        end,
    }
    
    local function adjustmentChanged() 
        -- Determine which parameters have changed
        local changed = ""
        for _,param in pairs(ChangeObserverParameters.Parameters) do
            if ChangeObserverParameters.HasChanged(param) then
                if string.len(changed) > 0 then 
                    changed = changed..","
                end
                changed = changed..param
            end
        end
        
        if string.len(changed) > 0 then
            Sockets.SendSocket:send("Changed:" .. changed .. "\n")
        end
    end
    
    local function moduleChanged(module) 
        Sockets.SendSocket:send("Module:" .. module .. "\n")
    end
    
    
    -- Start LrControl application
    LrControlApp.Start()
    
    
    -- Update change observer cache when setValue is called
    ModuleTools.DoBeforeFunction("LrDevelopController.setValue", function(param,value)
        ChangeObserverParameters.RegisterValue(param,value)
    end)

    
    -- Enter main waiting loop
    local lastModule = nil
    local observerAdded = false
    
    while (LrControl.Running) do
        -- Chcek if module has changed
        local currentModule = LrApplicationView.getCurrentModuleName()
        if lastModule ~= currentModule then
            moduleChanged(currentModule)
            lastModule = currentModule
        end
        
        -- Check if we need to add adjustment change observer
        if not observerAdded and currentModule == "develop" then
            LrDevelopController.addAdjustmentChangeObserver(context, adjustmentChanged, function(observer)
                pcall(observer)
            end)
            
            observerAdded = true
        end
        
        LrTasks.sleep(1/4)
    end
end



-- Start main task
LrTasks.startAsyncTask(function()
    LrFunctionContext.callWithContext("LrControl context", main)
end)