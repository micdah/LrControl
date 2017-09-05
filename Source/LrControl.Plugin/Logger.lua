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
local LrLogger      = import 'LrLogger'

local log = LrLogger('LrControl')
log:enable("logfile")

local levels = {
	Trace = 0,
	Debug = 1,
	Info = 2,
	Warn = 3,
	Error = 4,
	Fatal = 5
}

local options = {
	Levels	= levels,
	Level	= levels.Debug
}

local function wrap(level,f)
	return function(...)
		if (level >= options.Level) then
			f(...)
		end
	end
end

local trace, debug, info, warn, error, fatal = log:quick('trace', 'debug', 'info', 'warn', 'error', 'fatal')
local tracef, debugf, infof, warnf, errorf, fatalf = log:quickf('trace', 'debug', 'info', 'warn', 'error', 'fatal')

local logger = {
	Options = options,
	Trace	= wrap(levels.Trace, trace),
	Tracef	= wrap(levels.Trace, tracef),
	Debug	= wrap(levels.Debug, debug),
	Debugf	= wrap(levels.Debug, debugf),
	Info	= wrap(levels.Info, info),
	Infof	= wrap(levels.Info, infof),
	Warn	= wrap(levels.Warn, warn),
	Warnf	= wrap(levels.Warn, warnf),
	Error	= wrap(levels.Error, error),
	Errorf	= wrap(levels.Error, errorf),
	Fatal	= wrap(levels.Fatal, fatal),
	Fatalf	= wrap(levels.Fatal, fatalf)
}

return logger