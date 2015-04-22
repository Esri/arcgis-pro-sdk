## ArcGIS Pro 1.1 SDK for .NET (Beta)

Extend ArcGIS Pro with the ArcGIS Pro SDK for .NET, available at the 1.1 Beta release. ArcGIS Pro SDK for .NET is based on the add-in extensibility pattern (first introduced at 10.1). Leverage modern .NET features and patterns such as **Task Asynchronous Programming (TAP), LINQ, WPF Binding, and MVVM** to write **integrated 2D/3D add-ins** using Pro’s new APIs. 

<a href="http://pro.arcgis.com/en/pro-app/beta/sdk/" target="_blank">View it live</a>

###Table of Contents

####Developing with ArcGIS Pro

* [Requirements](#requirements)
* [Download](#download)
* [Installing ArcGIS Pro SDK](#installing-arcgis-pro-sdk)
* [Getting started](#getting-started)
* [ArcGIS Pro API](#arcgis-pro-api)
  * [Core](#core)
  * [Extensions](#extensions)
  * [Extensions with no public API](#extensions-with-no-public-api)
* [Release notes](#release-notes)
 * [ArcGIS Pro 1.1 SDK Beta](#arcgis-pro-11-sdk-beta)
* [Resources](#resources)

####Add-in fundamentals 

 * [ProConcept: Localization](../../wiki/ProConcept-Localization)
 * [ProGuide: Digitally signed add-ins](../../wiki/ProGuide-Digitally-signed-add-ins)
 * [ProGuide: Content and image resources](../../wiki/ProGuide-content-and-image-resources)

####Framework

&nbsp;&nbsp;&nbsp;&nbsp;**Customization**

* [ProGuide: Ribbon, tabs, and groups](../../wiki/ProGuide-Ribbon,-Tabs-and-Groups)
* [ProGuide: Buttons](../../wiki/ProGuide-Buttons)
* [ProGuide: Label controls](../../wiki/ProGuide-Label-Controls)
* [ProGuide: Check boxes](../../wiki/ProGuide-Checkboxes)
* [ProGuide: Edit boxes](../../wiki/ProGuide-Edit-Boxes)
* [ProGuide: Combo boxes](../../wiki/ProGuide-Combo-boxes)
* [ProGuide: Palettes and Split buttons](../../wiki/ProGuide-Palettes-and-Split-Buttons)
* [ProGuide: Galleries](../../wiki/ProGuide-Galleries)
* [ProGuide: Dockpanes](../../wiki/ProGuide-Dockpanes)
* [ProGuide: Code your own states and conditions](../../wiki/ProGuide-Code-Your-Own-States-and-Conditions)

##Requirements 
The requirements for the machine on which you develop your ArcGIS Pro add-ins are listed here.

####Supported platforms
* Windows 8.1 Basic, Professional, and Enterprise (64 bit [EM64T]) 
* Windows 8 Basic, Professional, and Enterprise (64 bit [EM64T]) 
* Windows 7 SP1 Ultimate, Enterprise, Professional, and Home Premium (64 bit [EM64T]) 

####Supported .NET framework
* 4.5.2 
* 4.5.1 
* 4.5 

####Supported IDEs
* Visual Studio 2013 (Professional, Premium, Ultimate, and Community Editions) 

##Download

Download the ArcGIS Pro SDK from the [ArcGIS Pro Beta Community](http://pro.arcgis.com/en/pro-app/).

##Installing ArcGIS Pro SDK

Unzip the ArcGIS Pro SDK for .NET download from the Beta Community site. Double-click each of the Microsoft Visual Studio VSIX packages to start the installation process. You do not need administrative access or elevated user permissions.

The table below summarizes the functionality of each .vsix file included in the SDK download:

| Name|File name|What it does|
| :-------------: | :-------------: |:-------------: |
| ArcGIS Pro SDK for .NET | proapp-sdk-templates.vsix | A collection of project and item templates to create ArcGIS Pro add-ins|
| ArcGIS Pro SDK for .NET (Utilities)  | proapp-sdk-utilities.vsix  | A collection of utilities to help create ArcGIS Pro add-ins|


####ArcGIS Pro SDK (templates) 
Package: proapp-sdk-templates.vsix  

The ArcGIS Pro SDK for .NET provides the following project and item templates:

C#  | VB| Name
------------------------  | -------------| ---------
![](../../wiki/images/VisualStudioTemplates/ArcGISProModuleC32.png "ArcGIS Pro Module C#") | ![](../../wiki/images/VisualStudioTemplates/ArcGISProModuleVB32.png "ArcGIS Pro Module VB") | ArcGIS Pro Module Add-in   
![](../../wiki/images/VisualStudioTemplates/ArcGISProButtonC32.png "ArcGIS Pro Button C#") | ![](../../wiki/images/VisualStudioTemplates/ArcGISProButtonVB32.png "ArcGIS Pro Button VB") | ArcGIS Pro Button 
![](../../wiki/images/VisualStudioTemplates/ArcGISProButtonPaletteC32.png "ArcGIS Pro Button Palette C#") | ![](../../wiki/images/VisualStudioTemplates/ArcGISProButtonPaletteVB32.png "ArcGIS Pro Button Palette VB") | ArcGIS Pro Button Palette
![](../../wiki/images/VisualStudioTemplates/ArcGISProComboBoxC32.png "ArcGIS Pro Combo Box C#") | ![](../../wiki/images/VisualStudioTemplates/ArcGISProComboBoxVB32.png "ArcGIS Pro Combo Box VB") | ArcGIS Pro Combo Box
![](../../wiki/images/VisualStudioTemplates/ArcGISProConstructionToolC32.png "ArcGIS Pro Construction Tool C#") | ![](../../wiki/images/VisualStudioTemplates/ArcGISProConstructionToolVB32.png "ArcGIS Pro Combo Box VB") | ArcGIS Pro Construction Tool
![](../../wiki/images/VisualStudioTemplates/ArcGISProDockPaneC32.png "ArcGIS Pro Dockpane C#") | ![](../../wiki/images/VisualStudioTemplates/ArcGISProDockPaneVB32.png "ArcGIS Pro Dockpane VB") | ArcGIS Pro Dockpane
![](../../wiki/images/VisualStudioTemplates/ArcGISProDockPaneC32.png "ArcGIS Pro Dockpane with Burger Button C#") | ![](../../wiki/images/VisualStudioTemplates/ArcGISProDockPaneVB32.png "ArcGIS Pro Dockpane with Burger Button VB") | ArcGIS Pro Dockpane with Burger Button
![](../../wiki/images/VisualStudioTemplates/ArcGISProDropHandlerC32.png "ArcGIS Pro Drop Handler C#") | ![](../../wiki/images/VisualStudioTemplates/ArcGISProDropHandlerVB32.png "ArcGIS Pro Drop Handler VB") | ArcGIS Pro Drop Handler
![](../../wiki/images/VisualStudioTemplates/ArcGISProGalleryC32.png "ArcGIS Pro Gallery C#") | ![](../../wiki/images/VisualStudioTemplates/ArcGISProGalleryVB32.png "ArcGIS Pro Gallery VB") | ArcGIS Pro Gallery
![](../../wiki/images/VisualStudioTemplates/ArcGISProInLineGalleryC32.png "ArcGIS Pro Inline-Gallery C#") | ![](../../wiki/images/VisualStudioTemplates/ArcGISProInLineGalleryVB32.png "ArcGIS Pro Inline-Gallery VB") | ArcGIS Pro Inline-Gallery
![](../../wiki/images/VisualStudioTemplates/ArcGISProMapToolC32.png "ArcGIS Pro Map Tool C#") | ![](../../wiki/images/VisualStudioTemplates/ArcGISProMapToolVB32.png "ArcGIS Pro Map Tool VB") | ArcGIS Pro Map Tool
![](../../wiki/images/VisualStudioTemplates/ArcGISProMenuC32.png "ArcGIS Pro Menu C#") | ![](wiki/images/VisualStudioTemplates/ArcGISProMenuVB32.png "ArcGIS Pro Menu VB") | ArcGIS Pro Menu
![](../../wiki/images/VisualStudioTemplates/ArcGISProPaneC32.png "ArcGIS Pro Pane C#") | ![](../../wiki/images/VisualStudioTemplates/ArcGISProPaneVB32.png "ArcGIS Pro Pane VB") | ArcGIS Pro Pane
![](../../wiki/images/VisualStudioTemplates/ArcGISProSketchToolC32.png "ArcGIS Pro Sketch Tool C#") | ![](../../wiki/images/VisualStudioTemplates/ArcGISProSketchToolVB32.png "ArcGIS Pro Sketch Tool VB") | ArcGIS Pro Sketch Tool
![](../../wiki/images/VisualStudioTemplates/ArcGISProSplitButtonC32.png "ArcGIS Pro Split Button C#") | ![](../../wiki/images/VisualStudioTemplates/ArcGISProSplitButtonVB32.png "ArcGIS Pro Split Button VB") | ArcGIS Pro Split Button
![](../../wiki/images/VisualStudioTemplates/ArcGISProToolC32.png "ArcGIS Pro Tool C#") | ![](../../wiki/images/VisualStudioTemplates/ArcGISProToolVB32.png "ArcGIS Pro Tool VB") | ArcGIS Pro Tool


####ArcGIS Pro SDK (utilities) 
Package: proapp-sdk-utilities.vsix  

The ArcGIS Pro SDK for .NET (utilities) provides the following utilities that extend the Visual Studio environment:

![pro-fix-references](../../wiki/images/Home/proapp-sdk-utilities.png "ArcGIS Pro SDK(Utilities)") 

Name               | Description
----------------------------  | --------------------------------------
Pro Fix References utility | Fixes broken references in an ArcGIS Pro add-in. Broken references can be caused when you share add-ins with other colleagues, or download add-ins where the ArcGIS Pro assembly references point to a different location from where you installed them.
Pro Generate DAML IDs | Converts all of the ArcGIS Pro Desktop Application Markup Language (DAML) string IDs into static string properties organized by DAML element types (for example, Button, Dockpane, Tool, Condition, and so on). This allows you to use the IntelliSense feature of Visual Studio within your source code file to add IDs, rather than having to manually type DAML string IDs).

## Getting started
Refer to the [ProGuide: Build your first add-in](../../wiki/ProGuide-Build-Your-First-Add-in) for step-by-step instructions on creating a basic button that appears on the ArcGIS Pro ribbon.

##ArcGIS Pro API
The ArcGIS Pro APIs are written exclusively in C#, unlike ArcObject assemblies, which are written as COM DLLs. Because ArcGIS Pro assemblies are managed .NET assemblies, intermediary assemblies containing .NET metadata or PIAs (Primary Interop Assemblies) are not required.

Add any of the ArcGIS Pro managed assemblies that comprise its API as references directly in your Visual Studio add-in projects. 

![pro-references.png](../../wiki/images/Home/pro-references.png "ArcGIS Pro API References") 

A complete list of the ArcGIS Pro assemblies is provided below:

###Core

Core assemblies are located within the {ArcGIS Pro Installation folder}\bin.

Assembly           | Description
------------------------| -------------
ArcGIS.Core.dll        | Provides CIM, Geodatabase, and Geometry APIs.
ArcGIS.Desktop.Framework.dll        | Provides the application framework to include add-in contracts, DAML support, and base classes. This assembly **must be referenced** by every add-in.

###Extensions

Major subsystems within ArcGIS Pro are organized into units called extensions. Extension assemblies are located within the {ArcGIS Pro Installation folder}\bin\Extensions folder in their own individual subfolder. 
Extension subfolder names are logically named for the unit of functionality they represent. For example, Mapping, Editing, Layout, and so on.

Assembly           | Description
------------------------| -------------
ArcGIS.Desktop.Catalog.dll   | Provides access to project content items (map items, layout items, style items, folder items, and so on).
ArcGIS.Desktop.Core.dll    | Provides functionality to create and manage projects, access to events associated with the current project, and the ability to execute geoprocessing tools.
ArcGIS.Desktop.Editing.dll        | Provides access to the editing environment and core editing functionality required for custom edit tool implementations.
ArcGIS.Desktop.Extensions.dll        | Provides extension methods for other ArcGIS Pro classes. Provides a base class for custom map tools.
ArcGIS.Desktop.Geoprocessing.dll        | Provides access to geoprocessing history items stored in the project. (Note: Adds a reference to ArcGIS.Desktop.Core.dll to execute geoprocessing tools.)
ArcGIS.Desktop.Layouts.dll        | Provides functionality for manipulating and editing layout elements, resourcing pictures on layouts, and export of layouts to various image formats.
ArcGIS.Desktop.Mapping.dll        | Provides types to create maps and layers, label features, perform query operations, and visualize them in 2D or 3D. Provides a raster API to create raster layers and customize raster rendering and an API to manage styles, style items, and symbols.
ArcGIS.Desktop.TaskAssistant.dll        | Provides the Tasks framework, allowing developers to access, open, close, or export task items.
ArcGIS.Desktop.Workflow.dll       | Provides functionality to create, configure, and execute Workflow Manager jobs and queries. Provides functionality to retrieve configuration information from the Workflow Manager database.

### Extensions with no public API

There are extension assemblies in {ArcGIS Pro Installation folder}\bin\Extensions subfolders) that do not have a public API. They are currently for Esri internal use only.

* ArcGIS.Desktop.Analys3D.dll
* ArcGIS.Desktop.DataReviewer.dll
* ArcGIS.Desktop.DataSourcesRaster.dll
* ArcGIS.Desktop.Geostatics.dll
* ArcGIS.Desktop.NetworkAnalysis.Transportation.dll
* ArcGIS.Desktop.Search.dll
* ArcGIS.Desktop.Sharing.dll

##Release Notes 

###ArcGIS Pro 1.1 SDK Beta

These release notes describe details of the ArcGIS Pro 1.1 SDK for .NET beta release. Here you will find information about functionality available in the release and known issues and limitations.

####New functionality

Since this is the first release of ArcGIS Pro 1.1 SDK, there is technically no new functionality to describe. 

####Known Issues

The following are known issues or limitations with the ArcGIS Pro 1.1 SDK Beta release. Where one is available, a workaround is described. 

#####1. Config.daml not Updated by New Item Template
* You have edited your Config.daml file by hand in Visual Studio
* You have **not** saved your changes
* Config.daml is the **active** tab in Visual Studio
* You run one of the ArcGIS Pro SDK item templates (eg to add a new Button or Dockpane)
* The Config.daml file in Visual Studio does not appear to be updated.

**Workaround**
* Close the Config.daml file tab in Visual Studio.
* Re-open it from the Visual Studio solution explorer
* *If it still doesn't show the update, locate the Config.daml file on disk and copy and paste the contents of the file on disk into the Visual Studio Config.daml editing window.
* Save your edits

#####2. Install Issue if Locale is other Than English
* ArcGIS Pro SDK templates and utilities cannot be installed if your locale is not English. Change your locale to English and then run the vsix packages.
 
#####3. API Reference guide TOC Display issues on some desktops
* When you browse to the [API Reference guide](http://pro.arcgis.com/en/pro-app/beta/sdk/api-reference/) on some desktop machines, you will see the TOC like this image below:  
![TOC-issue.png](../../wiki/images/Home/TOC-issue.png "ArcGIS Pro API Reference guide TOC")  
* The TOC layout has adapted to a touch device. In that mode, the TOC, search, Index etc. are available from the icons on the toolbar at the top of the page. We have noticed this mainly on the Chrome browser which sometimes identifies a device as touch capable if you have a touchscreen laptop or are running on a Virtual Machine. 

#####4. Add-in is not loaded by Pro when you "build" it in Visual Studio
* You have deleted the add-in file (*.esriAddinX file) from the `C:\Users\<UserName>\Documents\ArcGIS\AddIns\ArcGISPro` folder.
* Without making any code changes, you "Build" the add-in project in Visual Studio and launch ArcGIS Pro or click the "Start" button in Visual Studio to launch the debugger.
* Your add-in does not load in ArcGIS Pro.

**Workaround**
* From Visual Studio's Build menu, click the Rebuild Solution menu item. This will create the add-in file (*.esriAddinX file) under `C:\Users\<UserName>\Documents\ArcGIS\AddIns\ArcGISPro` folder. When you launch ArcGIS Pro, your add-in will now load.

#####5. Controls do not Work in ArcGIS Pro after the Add-in Project's Namespace and/or Assembly is Changed  

* You changed **_one or more_** of the following:
    * You changed the Assembly name and/or Default namespace in your project Application properties within Visual Studio
    * You changed the namespace in your add-in Module and/or add-in class files.

When ArcGIS Pro loads your add-in, one or more of the controls defined in your add-in **do not work**. For example, a new button in your add-in is unresponsive when you click it and becomes permanently disabled.

**Fix**  
One or more of the following conditions may need to be fixed:  
* The **defaultAssembly** and **defaultNamespace** attributes on the root ```ArcGIS``` daml element within your Config.daml **must be changed** to match any changes you made to the corresponding Visual Studio project Application properties.

```xml

<?xml version="1.0" encoding="utf-8"?>
<ArcGIS defaultAssembly="MyRenamedAssembly.dll" defaultNamespace="MyRenamedAssembly" xmlns="..." xmlns:xsi="..." xsi:schemaLocation="...">
  <AddInInfo id="...
....

```

* If the namespace of an add-in class (in the code file) does **not** match the ```defaultNamespace``` attribute of the ```<ArcGIS>``` element in the Config.daml, you **must fully qualify** the ```className``` attribute of its daml element (with "namespace.classname") in Config.daml.

For example: Assume this is the class file of a button. Note the ```namespace```.
```C#

namespace MyRenamedAssembly.Addins {
     class Button1 : Button
    {
        protected override void OnClick() {
```

Assume this is the Config.daml. Note the ```<ArcGIS defaultNamespace```
```xml

<!-- the defaultNamespace is MyRenamedAssembly -->
<ArcGIS defaultAssembly="MyRenamedAssembly.dll" defaultNamespace="MyRenamedAssembly" xmlns="...

    <!-- the button className attribute is fully qualified -->
   <button .... className="MyRenamedAssembly.Addins.Button1" ... />
```

* Rebuild the add-in project

##Resources

* [ArcGIS Pro API Reference Guide](http://pro.arcgis.com/en/pro-app/beta/sdk/api-reference/index.html)
* [arcgis-pro-sdk-community-samples](../../../arcgis-pro-sdk-community-samples)
* <a href="http://pro.arcgis.com/en/pro-app/beta/sdk/" target="_blank">ArcGIS Pro SDK (pro.arcgis.com)</a>
* [FAQ](../../wiki/FAQ)
* [ArcGIS Pro SDK Icons]()  
![Image-of-icons.png](../../wiki/images/Home/Image-of-icons.png "ArcGIS Pro SDK Icons")

##Contributing

Esri welcomes contributions from anyone and everyone. For more information, see our [guidelines for contributing](https://github.com/esri/contributing).

##Issues

Find a bug or want to request a new feature? Let us know by submitting an issue.

## Licensing
Copyright 2015 Esri

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at:

   http://www.apache.org/licenses/LICENSE-2.0.

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

A copy of the license is available in the repository's [license.txt](./License.txt) file.

[](Esri Tags: ArcGIS-Pro-SDK)
[](Esri Language: C-Sharp)​


<p align = center><img src="https://github.com/Esri/arcgis-pro-sdk/wiki/images/ArcGISPro.png"  alt="pre-req" align = "top" height = "20" width = "20" >
<b> ArcGIS Pro 1.1 SDK for Microsoft .NET Framework (Beta)</b>
</p>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[Home](https://github.com/Esri/arcgis-pro-sdk/wiki) | <a href="http://pro.arcgis.com/en/pro-app/beta/sdk" target="_blank">ArcGIS Pro SDK</a> | <a href="http://pro.arcgis.com/en/pro-app/beta/sdk/api-reference/index.html" target="_blank">API Reference</a> | [Requirements](#requirements) | [Download](#download) |  <a href="http://github.com/esri/arcgis-pro-sdk-community-samples" target="_blank">Samples</a>



