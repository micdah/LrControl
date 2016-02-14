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
local Options = require 'Options'

local Modules = {
    LrControl = {
        getApiVersion = function() 
            return "LrControl " .. Options.Version.major .. "." .. Options.Version.minor
        end
    }
}

local function getModule(message)
    local i = string.find(message, "%.")
    if i ~= nil then
        local name = string.sub(message,1, i-1)
        local rest = string.sub(message,i+1)             
        local module = Modules[name]
        
        return module,rest
    else 
        return nil,message
    end
end

local function getMethod(module,methodAndArgs)
    local i = string.find(methodAndArgs, "%s")
    local methodName = methodAndArgs
    local args = nil
    
    -- Check if there are arguments
    if i ~= nil then
        methodName = string.sub(methodAndArgs, 1, i-1)
        args = string.sub(methodAndArgs, i+1)
    end
    
    local method = module[methodName]
    return method,args
end

local function deserializeArgument(arg) 
    local typeArg = string.sub(arg, 1, 1)
    local value   = string.sub(arg, 2)
    
    if typeArg == "N" then
        -- Number
        return tonumber(value)
    elseif typeArg == "S" then
        -- String
        return value
    elseif typeArg == "B" then
        -- Boolean 0/1
        return value == "1"
    end
    
    -- Unknown
    return nil
end

local function interpretArguments(args)
    local list = {}
    for arg in string.gmatch(args,"([^\30]+)") do
        list[#list+1] = deserializeArgument(arg)
    end
    return unpack(list)
end

local function interpretCommand(command)
    -- Lookup module 
    local module,methodAndArgs = getModule(command)
    if module == nil or methodAndArgs == nil then
        return false,nil
    end
    
    -- Lookup method
    local method,args = getMethod(module,methodAndArgs)
    if method == nil then  
        return false,nil
    end
    
    -- Invoke method
    local result = nil
    if args ~= nil then
        return true,method(interpretArguments(args))
    else
        return true, method()
    end
end

return {
    InterpretCommand = interpretCommand
}