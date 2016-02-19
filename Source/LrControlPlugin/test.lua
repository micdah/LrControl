local function addFunction(lookup, uniqueIdentifier, f) 
    if lookup[uniqueIdentifier] == nil then
        lookup[uniqueIdentifier] = {}
    end
    
    local list = lookup[uniqueIdentifier]
    list[#list+1] = f
end

local list = {}

addFunction(list, "test", "hej")

print(list.test[0])