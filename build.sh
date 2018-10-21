#!/bin/bash

Configuration=$1
Output="../Build/${Configuration}/LrControl.lrplugin/"
Project=""

dotnet build src/LrControl.Plugin/LrControl.Plugin.csproj --configuration ${Configuration}
dotnet publish src/LrControl.Console/LrControl.Console.csproj --configuration ${Configuration} --runtime osx-x64 --self-contained --output ${Output}/osx/
dotnet publish src/LrControl.Console/LrControl.Console.csproj --configuration ${Configuration} --runtime win-x64 --self-contained --output ${Output}/win/
