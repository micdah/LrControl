#!/bin/bash

Configuration=$1
Output="../../Build/${Configuration}/LrControl.lrplugin/"
Project=""

dotnet build LrControl.Plugin/LrControl.Plugin.csproj --configuration ${Configuration}
dotnet publish LrControl.Console/LrControl.Console.csproj --configuration ${Configuration} --runtime osx-x64 --self-contained --output ${Output}/osx/
dotnet publish LrControl.Console/LrControl.Console.csproj --configuration ${Configuration} --runtime win-x64 --self-contained --output ${Output}/win/