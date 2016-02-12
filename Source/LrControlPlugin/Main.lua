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
local LrTasks = import 'LrTasks'

LrTasks.startAsyncTask(
function()
	local LrDialogs = import 'LrDialogs'
	
	LrDialogs.showBezel("Debug")
end	
)

--[[local LrDialogs   = import 'LrDialogs'
local LrLogger	  = import 'LrLogger'
local LrShell	  = import 'LrShell'
local LrPathUtils = import 'LrPathUtils'


local log = LrLogger("LrControl")
log:info("LrControl initializing")


-- Track loaded version, to detect reloads
math.randomseed(os.time())
currentLoadVersion = rawget (_G, "currentLoadVersion") or math.random ()
currentLoadVersion = currentLoadVersion + 1

LrDialogs.showBezel("LrControl starting...")


-- Main task
local function main()
	LrDialogs.showBezel ("LrControl running, running version " .. currentLoadVersion)

	LrShell.openFilesInApp ( { LrPathUtils.child (_PLUGIN.path, 'Info.lua')}, LrPathUtils.child (_PLUGIN.path, 'win\LrControl.exe'))

	local loadVersion = currentLoadVersion
	while (loadVersion == currentLoadVersion) do
		LrTasks.sleep(0.25)
	end

	LrDialogs.showBezel("LrControl shutting down")
end



-- Start main task
LrTasks.startAsyncTask(function()
	--LrDialogs.showBezel ("LrControl starting...")
	LrShell.openFilesInApp ( { LrPathUtils.child (_PLUGIN.path, 'Info.lua')}, LrPathUtils.child (_PLUGIN.path, 'win\LrControl.exe'))
end
)
--]]
