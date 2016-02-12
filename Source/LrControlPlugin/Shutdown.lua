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
local LrControlApp = require 'LrControlApp'


return {
	LrShutdownFunction = function (doneFunction, progressFunction)
		progressFunction (i, "Stopping LrControl, closing connections")
		
		-- Increment version to break main loop
		math.randomseed (os.time ())
		currentLoadVersion = rawget (_G, "currentLoadVersion") or math.random ()
		currentLoadVersion = currentLoadVersion + 1

		-- Stop application
        LrControlApp.Stop()

		doneFunction()
	end
}