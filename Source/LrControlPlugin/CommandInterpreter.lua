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

local function getModule(modules,message)
    local i = string.find(message, "%.")
    if i ~= nil then
        local name = string.sub(message,1, i-1)
        local rest = string.sub(message,i+1)             
        local module = modules[name]
        
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

local function encodeValue(value) 
    local type = type(value)
    if type == "string" then
        return "S"..value
    elseif type == "number" then
        return "N"..tostring(value)
    elseif type == "boolean" then
        return "B"..(value and "1" or "0")
    else
        return value
    end
end

local function encodeResponse(results)
    if #results == 0 then
        return "ack"
    end

    local result = nil 
    for i = 1, #results do
        local encodedValue = encodeValue(results[i])
    
        if result ~= nil then
            result = result.."\30"..encodedValue
        else
            result = encodedValue
        end
    end
    
    return result
end

local function errorMessage(message) 
    return "E"..string.gsub(message, "\n", ", ")
end

local function interpretCommand(modules,command)
    -- Lookup module 
    local module,methodAndArgs = getModule(modules,command)
    if module == nil then
        return errorMessage("Unknown module")
    elseif methodAndArgs == nil then
        return errorMessage("Missing method and arguments")
    end
    
    -- Lookup method
    local method,args = getMethod(module,methodAndArgs)
    if method == nil then  
        return errorMessage("Unknown method: "..method)
    end
    
    -- Invoke method
    local result1, result2, result3
    if args ~= nil then
        result1, result2, result3 = method(interpretArguments(args))
    else
        result1, result2, result3 = method()
    end
    
    return encodeResponse({result1, result2, result3})
end

return {
    InterpretCommand = interpretCommand,
    ErrorMessage     = errorMessage,
}