Welcome to the Adobe® Lightroom® 6.0 Software Development Kit 
_____________________________________________________________________________

This file contains the latest information for the Adobe Lightroom SDK (6.0 Release). 
The information applies to Adobe Lightroom and includes the following sections:

1. Introduction
2. SDK content overview
3. Development environment
4. Sample plug-ins
5. Running the plug-ins
6. Adobe Add-ons

**********************************************
1. Introduction
**********************************************

The SDK provides information and examples for the scripting interface to Adobe 
Lightroom. The SDK defines a scripting interface for the Lua language.

A number of new namespaces have been added in the 6.0 SDK release. Please see the API
Reference for more information about each of the new namespaces:

1.	LrSocket: provides ability to send and receive data from other processes
	using sockets.
2.	LrApplicationView: provides access to the application's view state including 
	module, main view, secondary view, and zoom control. 
3.	LrDevelopController: provides access to controls in the Develop module.
4.	LrSelection: provides access to selection based commands including flags,
	color labels, ratings, and active image selection. 
5.	LrSlideshow: allows starting and stopping a slideshow.
6.	LrTether: gives control over tethered shooting.
7.	LrUndo: provides access to undo/redo commands.
8.	LrSounds: allows playing system sounds.


Key Bug Fixes:

1. 	catalog:createVirtualCopies(...) will now create the proper number of virutal copies
	when multiple photos are selected in library loupe or develop.
2.	LrKeyword:getAttributes() will now indicate if the keyword is a face tag.
3.	LrDialogs.runSavePanel(...) prompt parameter will now work correctly on Windows.
4.	"Direction" Exif metadata can now be accessed via LrPhoto.get/setRawMetadata(...)
5.	LrApplication.purchaseSource() and LrApplication.serialNumberHas() will behave more
	consistently with CC versions of Lr.
6.	catalog:batchGetFormattedMetadata(...) will now correctly return "maxAvailWidth" and
	"maxAvailHeight"
7.	viewFactory:scrolled_view(...) visible property can now be bound on mac.
8.	Docs have been updated to correct a number of issues.
 
**********************************************
2. SDK content overview
**********************************************

The SDK contents include the following:

- <sdkInstall>/Manual/Lightroom SDK Guide.pdf: 
	Describes the SDK and how to extend the functionality of 
	Adobe Lightroom.

- <sdkInstall>/API Reference/:  
	The Scripting API reference in HTML format. Start at index.html.

- <sdkInstall>/Sample Plugins: 
	Sample plug-ins and demonstration code (see section 4).

**********************************************
3. Development environment
**********************************************

You can use any text editor to write your Lua scripts, and you can
use the LrLogger namespace to write debugging information to a console. 
See the section on "Debugging your Plug-in" in the Lightroom SDK Guide.

**********************************************
4. Sample Plugins
**********************************************

The SDK provides the following samples:

- <sdkInstall>/Sample Plugins/flickr.lrdevplugin/: 
	Sample plug-in that demonstrates creating a plug-in which allows 
	images to be directly exported to a Flickr account.

- <sdkInstall>/Sample Plugins/ftp_upload.lrdevplugin/: 
	Sample plug-in that demonstrates how to export images to an FTP server.

- <sdkInstall>/Sample Plugins/helloworld.lrdevplugin/: 
	Sample code that accompanies the Getting Started section of the 
	Lightroom SDK Guide.

  <sdkInstall>/Sample Plugins/custommetadatasample.lrdevplugin/:
	Sample code that accompanies the custommetadatasample plug-in that
	demonstrates custom metadata.

- <sdkInstall>/Sample Plugins/metaexportfilter.lrdevplugin/: 
	Sample code that demonstrates using the metadata stored in a file 
	to filter the files exported via the export dialog.

- <sdkInstall>/Sample Plugins/websample.lrwebengine/: 
	Sample code that creates a new style of web gallery template 
	using the Web SDK.

**********************************************
5. Running the plug-ins
**********************************************

To run the sample code, load the plug-ins using the Plug-in Manager
available within Lightroom. See the Lightroom SDK Guide for more information.

*********************************************************
6. Adobe Add-ons
*********************************************************

To learn more about Adobe Add-ons, point your browser to:

  https://creative.adobe.com/addons

_____________________________________________________________________________

Copyright 2015 Adobe Systems Incorporated. All rights reserved.

Adobe, Lightroom, and Photoshop are registered trademarks or trademarks of 
Adobe Systems Incorporated in the United States and/or other countries. 
Windows is either a registered trademark or a trademark of Microsoft Corporation
in the United States and/or other countries. Macintosh is a trademark of 
Apple Computer, Inc., registered in the United States and  other countries.

_____________________________________________________________________________
