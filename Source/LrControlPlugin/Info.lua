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

return {
    LrSdkVersion        = 6.0,
    LrSdkMinimumVersion = 6.0,
    LrPluginName        = "LrControl",
    LrToolkitIdentifier = "dk.micdah.lrcontrol",
    LrForceInitPlugin	= true,
    LrInitPlugin        = "Main.lua",
    LrShutdownPlugin    = "ShutdownPlugin.lua",
    LrShutdownApp       = "Shutdown.lua",
    LrDisablePlugin     = "DisablePlugin.lua",
    LrEnablePlugin      = "Main.lua",
    LrPluginInfoUrl     = "https://github.com/micdah/LrControl",
    LrExportMenuItems	= {
        {
            title = "About",
            file = "About.lua"
        }
    },
    VERSION             = { major = 1, minor = 0, revision = 0, build = 0 }
}