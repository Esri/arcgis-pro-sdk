## ArcGIS Pro 2.5 SDK for .NET

Extend ArcGIS Pro with ArcGIS Pro SDK for .NET. ArcGIS Pro SDK for .NET is based on the add-in and configurations extensibility pattern. Leverage modern .NET features and patterns such as Task Asynchronous Programming (TAP), LINQ, WPF Binding, and MVVM to write integrated 2D/3D add-ins using Pro’s new APIs.

<a href="http://pro.arcgis.com/en/pro-app/sdk/" target="_blank">View it live</a>

### Table of Contents

#### Developing with ArcGIS Pro

* [Requirements](#requirements)
* [Installing ArcGIS Pro SDK for .NET](#installing-arcgis-pro-sdk-for-net)
* [ArcGIS Pro SDK for .NET components](#arcgis-pro-sdk-for-net-components)
  * [ArcGIS Pro SDK for .NET templates](#arcgis-pro-sdk-for-net-templates)
  * [ArcGIS Pro SDK for .NET utilities](#arcgis-pro-sdk-for-net-utilities)
* [Getting started](#getting-started) 
* [ProGuide: ArcGIS Pro Extensions NuGet](../../wiki/ProGuide-ArcGIS-Pro-Extensions-NuGet)
* [ProConcepts: Migrating to ArcGIS Pro](../../wiki/ProConcepts-Migrating-to-ArcGIS-Pro)
* [ProSnippets](#prosnippets)  
* [ArcGIS Pro API](#arcgis-pro-api)
  * [Core](#core)
  * [Extensions](#extensions)
  * [Extensions with no public API](#extensions-with-no-public-api)
* [Release notes](#release-notes)
  * [ArcGIS Pro 2.5 SDK for .NET](#arcgis-pro-25-sdk-for-net-1)
       * [What's New](#whats-new)
* [Previous versions](#previous-versions)  
* [Resources](#resources)

#### Framework

* [ProSnippets: Framework](../../wiki/ProSnippets-Framework)
* [ProConcepts: Framework](../../wiki/ProConcepts-Framework)
* [ProConcepts: Advanced topics](../../wiki/ProConcepts-Advanced-Topics)
* [ProGuide: Command line switches for ArcGISPro.exe](../../wiki/ProGuide-Command-line-switches-for-ArcGISPro.exe)
* [ProGuide: Reusing ArcGIS Pro Commands](../../wiki/ProGuide-Reusing-Pro-Commands)
* [ProGuide: Licensing](../../wiki/ProGuide-License-Your-Add-in)
* [ProGuide: Digital signatures](../../wiki/ProGuide-Digitally-signed-add-ins-and-configurations)


&nbsp;&nbsp;&nbsp;&nbsp;**Add-ins**

* [Pro Guide: Installation](../../wiki/ProGuide-Installation-and-Upgrade)
* [Pro Guide: Your first add-in](../../wiki/ProGuide-Build-Your-First-Add-in)
* [ProConcept: Localization](../../wiki/ProConcept-Localization)
* [ProGuide: Content and image resources](../../wiki/ProGuide-content-and-image-resources)
* [ProGuide: Diagnosing ArcGIS Pro Add-ins](../wiki/ProGuide-Diagnosing-ArcGIS-Pro-Add-ins)

&nbsp;&nbsp;&nbsp;&nbsp;**Configurations**

* [ProConcepts: Configurations](../../wiki/ProConcepts-Configurations)
* [ProGuide: Configurations](../../wiki/ProGuide-Configurations)


&nbsp;&nbsp;&nbsp;&nbsp;**Customization**

* [ProGuide: Ribbon, tabs, and groups](../../wiki/ProGuide-Ribbon-Tabs-and-Groups)
* [ProGuide: Buttons](../../wiki/ProGuide-Buttons)
* [ProGuide: Label controls](../../wiki/ProGuide-Label-Controls)
* [ProGuide: Check boxes](../../wiki/ProGuide-Checkboxes)
* [ProGuide: Edit boxes](../../wiki/ProGuide-Edit-Boxes)
* [ProGuide: Combo boxes](../../wiki/ProGuide-Combo-boxes)
* [ProGuide: Palettes and Split buttons](../../wiki/ProGuide-Palettes-and-Split-Buttons)
* [ProGuide: Galleries](../../wiki/ProGuide-Galleries)
* [ProGuide: Dockpanes](../../wiki/ProGuide-Dockpanes)
* [ProGuide: Code your own states and conditions](../../wiki/ProGuide-Code-Your-Own-States-and-Conditions)

&nbsp;&nbsp;&nbsp;&nbsp;**Styling**

* [ProGuide: Style Guide](../../wiki/proguide-style-guide)
* [ProGuide: Applying custom styles](../../wiki/ProGuide-Applying-Custom-Styles)
* [Esri Brushes](http://ArcGIS.github.io/arcgis-pro-sdk/content/brushescolors/brushes.html)
* [Esri Colors](http://ArcGIS.github.io/arcgis-pro-sdk/content/brushescolors/colors.html)

-------------------------

### Content

* [ProSnippets: Content](../../wiki/ProSnippets-content)
* [ProSnippets: Browse Filters](../../wiki/ProSnippets-OpenItemDialogBrowseFilter)
* [ProConcepts: Project Items](../../wiki/ProConcepts-Content-and-Items)
* [ProConcepts: Custom Items](../../wiki/ProConcepts-Custom-Items)
* [ProGuide: Custom Items](../../wiki/ProGuide-Custom-Items)
* [ProGuide: Custom Browse Dialog Filters](../../wiki/ProGuide-Custom-Browse-Dialog-Filters)
* [ArcGIS Pro TypeID Reference](../../wiki/ArcGIS-Pro-TypeID-Reference)

--------------------------

### CoreHost

* [ProSnippets: CoreHost](../../wiki/ProSnippets-CoreHost)
* [ProConcepts: CoreHost](../../wiki/proconcepts-CoreHost)

--------------------------

### DataReviewer

* [ProConcepts: DataReviewer](../../wiki/proconcepts-DataReviewer)

--------------------------

### Editing

* [ProSnippets: Editing](../../wiki/ProSnippets-Editing)
* [ProConcepts: Editing](../../wiki/ProConcepts-Editing)
* [ProConcepts: Annotation Editing](../../wiki/ProConcepts-Editing-Annotation)
* [ProConcepts: Dimension Editing](../../wiki/ProConcepts-Editing-Dimensions)
* [ProGuide: Editing Tool](../../wiki/ProGuide-Editing-Tool)
* [ProGuide: Construction Tools with Options](../../wiki/ProGuide-Construction-Tools-with-Options)
* [ProGuide: Annotation Construction Tools](../../wiki/ProGuide-Annotation-Construction-Tools)
* [ProGuide: Annotation Editing Tools](../../wiki/ProGuide-Annotation-Editing-Tools)
* [ProGuide: Templates](../../wiki/ProGuide-Templates)

--------------------------

### Geodatabase

* [ProSnippets: Geodatabase](../../wiki/ProSnippets-Geodatabase)
* [ProConcepts: Geodatabase](../../wiki/ProConcepts-Geodatabase)
* [ProConcepts: Plugin Datasources](../../wiki/ProConcepts-Plugin-Datasources)
* [ProGuide: Plugin Datasources](../../wiki/ProGuide-Plugin-Datasource)

--------------------------

### Geometry

* [ProSnippets: Geometry](../../wiki/ProSnippets-Geometry)
* [ProConcepts: Geometry](../../wiki/ProConcepts-Geometry)

&nbsp;&nbsp;&nbsp;&nbsp;**Relational Operations**

* [ProGuide: RelationalOperations](../../wiki/ProGuide-Relational-Operations)
* [ProSnippets: Custom Relational Operations](../../wiki/ProGuide-Custom-Relational-Operations)

--------------------------

### Geoprocessing

* [ProSnippets: Geoprocessing](../../wiki/ProSnippets-Geoprocessing)
* [ProConcepts: Geoprocessing](../../wiki/ProConcepts-Geoprocessing)

--------------------------

### Layouts

* [ProSnippets: Layouts](../../wiki/ProSnippets-Layouts)
* [ProConcepts: Layouts](../../wiki/ProConcepts-Layouts)

&nbsp;&nbsp;&nbsp;&nbsp;**Reports**
* [ProSnippets: Reports](../../wiki/ProSnippets-Reports)

--------------------------

### Map Authoring

* [ProSnippets: Map Authoring](../../wiki/ProSnippets-MapAuthoring)
* [ProSnippets: Annotation](../../wiki/ProSnippets-Annotation)
* [ProSnippets: Labeling](../../wiki/ProSnippets-Labeling)
* [ProSnippets: Renderers](../../wiki/ProSnippets-Renderer)
* [ProSnippets: Symbology](../../wiki/ProSnippets-Symbology)
* [ProSnippets: Text Symbols](../../ProSnippets-TextSymbols)
* [ProConcepts: Map Authoring](../../wiki/ProConcepts-Map-Authoring)
* [ProConcepts: Annotation](../../wiki/ProConcepts-Annotation)
* [ProConcepts: Dimensions](../../wiki/ProConcepts-Dimensions)
* [ProGuide: Custom Dictionary Style](../../wiki/ProGuide-Custom-Dictionary-Style)
* [ProGuide: Geocoding](../../wiki/ProGuide-Geocoding)

&nbsp;&nbsp;&nbsp;&nbsp;**Scene**
* [ProSnippets: Scene Layers](../../wiki/ProSnippets-SceneLayers)
* [ProConcepts: Scene Layers](../../wiki/ProConcepts-Scene-Layers)

&nbsp;&nbsp;&nbsp;&nbsp;**Stream**
* [ProSnippets: Stream Layers](../../wiki/ProSnippets-StreamLayers)
* [ProConcepts: Stream Layers](../../wiki/ProConcepts-Stream-Layers)
 
--------------------------

### Map Exploration

* [ProSnippets: Map Exploration](../../wiki/ProSnippets-MapExploration)
* [ProSnippets: Custom Pane with Contents](../../wiki/ProSnippets-CustomPaneWithContents)
* [ProConcept: Map Exploration](../../wiki/ProConcepts-Map-Exploration)
* [ProGuide: Map Pane Impersonation](../../wiki/ProGuide-Map-Pane-Impersonation)
* [ProGuide: TableControl](../../wiki/ProGuide-TableControl)

&nbsp;&nbsp;&nbsp;&nbsp;**Map Tools**<br>

* [ProGuide: Feature Selection](../../wiki/ProGuide-Feature-Selection)
* [ProGuide: Identify](../../wiki/ProGuide-Identify)
* [ProGuide: MapView Interaction](../../wiki/ProGuide-MapView-Interaction)
* [ProGuide: Embeddable Controls](../../wiki/ProGuide-Using-Embeddable-Controls)
* [ProGuide: Custom Popups](../../wiki/ProGuide-Custom-Popups)
* [ProGuide: Dynamic Popup Menu](../../wiki/ProGuide-Dynamic-Popup-Menu)

--------------------------

### Parcel Fabric <img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/prerelease.png" width="10%">
* [ProSnippets: Parcel Fabric](../../wiki/ProSnippets-ParcelFabric)
* [ProConcepts: Parcel Fabric](../../wiki/ProConcepts-Parcel-Fabric)

--------------------------

### Raster

* [ProSnippets: Raster](../../wiki/ProSnippets-Raster)
* [ProConcept: Raster](../../wiki/ProConcepts-Raster)

--------------------------

### Sharing

* [ProSnippets: Sharing](../../wiki/ProSnippets-sharing)
* [ProConcepts: Portal](../../wiki/ProConcepts-Portal)

--------------------------

### Tasks

* [ProSnippets: Tasks](../../wiki/ProSnippets-Tasks)
* [ProConcepts: Tasks](../../wiki/ProConcepts-Tasks)

--------------------------

### Utility Network

* [ProSnippets: Utility Network](../../wiki/ProSnippets-UtilityNetwork)
* [ProConcepts: Utility Network](../../wiki/ProConcepts-Utility-Network)
* [Object Model Diagram](http://Esri.github.io/arcgis-pro-sdk/content/OMDs/Utility-Network-Object-Model-Diagram.pdf)

--------------------------

### Workflow Manager

* [ProSnippets: Workflow Manager](../../wiki/ProSnippets-WorkflowManager)
* [ProConcept: Workflow Manager](../../wiki/ProConcepts-Workflow-Manager)

--------------------------

## Resources

* [API Reference online](http://pro.arcgis.com/en/pro-app/sdk/api-reference)
* <a href="http://pro.arcgis.com/en/pro-app/sdk/" target="_blank">ArcGIS Pro SDK for .NET (pro.arcgis.com)</a>
* [arcgis-pro-sdk-community-samples](http://github.com/Esri/arcgis-pro-sdk-community-samples)
* [ArcGISPro Registry Keys](http://github.com/Esri/arcgis-pro-sdk/wiki/ArcGIS-Pro-Registry-Keys)
* [ArcGIS Pro DAML ID Reference](http://github.com/Esri/arcgis-pro-sdk/wiki/ArcGIS-Pro-DAML-ID-Reference)
* [ArcGIS Pro TypeID Reference](http://github.com/Esri/arcgis-pro-sdk/wiki/ArcGIS-Pro-TypeID-Reference)
* [FAQ](http://github.com/Esri/arcgis-pro-sdk/wiki/FAQ)
* [ArcGIS Pro SDK icons](https://github.com/Esri/arcgis-pro-sdk/releases/tag/2.5.0.8933)

## Requirements
The requirements for the machine on which you develop your ArcGIS Pro add-ins are listed here.
  
**.NET Framework 4.8:**<br/>
*  With the release of ArcGIS Pro 2.5, the minimum .NET target has been changed from 4.6.1 to 4.8. What does this mean for you and your add-ins?
      * Existing add-ins, already deployed, will work at 2.5 with no change to their forward compatibility.
      * New add-ins created at 2.5 will require the minimum target framework set to 4.8 or they will not compile (this is the default setting in the Pro SDK).
      * Existing add-ins which are recompiled at 2.5 (e.g. because a code change was made) will also require the minimum target framework set to 4.8 or they will not compile. Note: As always, if an existing add-in is changed for any reason, the desktopVersion attribute in its Config.daml file should be changed to reflect the version of Pro it was last compiled against, in this case, now 2.5..

#### ArcGIS Pro

* ArcGIS Pro 2.5

#### Supported platforms

* Windows 10 (Home, Pro, Enterprise) (64 bit)
* Windows 8.1 (Pro, and Enterprise) (64 bit) 

#### Supported .NET framework

* Microsoft .NET Framework 4.8 

#### Supported IDEs

* Visual Studio 2019 (Professional, Enterprise, and Community Editions)
* Visual Studio 2017 (Professional, Enterprise, and Community Editions)

#### Third party assemblies
_**Newtonsoft Json**_
* At 2.5 ArcGIS Pro is using version 12.0.1 of the Newtonsoft Json NuGet. If you require Newtonsoft NuGet in your add-ins it is recommended to use the same version.  

_**CefSharp**_
* At 2.5 ArcGIS is using version 75.1.141 of CefSharp. Pro includes the CefSharp.dll, CefSharp.Core.dll and CefSharp.Wpf.dll in the "C:\Program Files\ArcGIS\Pro\bin\cef" installation location.  To use the CefSharp ChromiumWebBrowser control, consult [ChromiumWebBrowser](https://github.com/ArcGIS/arcgis-pro-sdk/wiki/ProConcepts-Framework#chromiumwebbrowser) 
 
Note: [ArcGIS Pro system requirements](http://pro.arcgis.com/en/pro-app/get-started/arcgis-pro-system-requirements.htm) 

## Installing ArcGIS Pro SDK for .NET

ArcGIS Pro SDK for .NET can be downloaded and installed using either one of the following options:

* Download and install from within Visual Studio
* Download from MyEsri.com

Read the [ProGuide: Installation and Upgrade](http://github.com/Esri/arcgis-pro-sdk/wiki/ProGuide-Installation-and-Upgrade) for detailed installation instructions.

## ArcGIS Pro SDK for .NET components

The following table summarizes the functionality of each .vsix file included in the SDK download:

| Name|File|Functionality|
| ------------- | ------------- |------------- |
| ArcGIS Pro SDK for .NET | proapp-sdk-templates.vsix | A collection of project and item templates to create ArcGIS Pro add-ins|
| ArcGIS Pro SDK for .NET (Utilities)  | proapp-sdk-utilities.vsix  | A collection of utilities to help create ArcGIS Pro add-ins|


#### ArcGIS Pro SDK for .NET templates 
Package: proapp-sdk-templates.vsix  

ArcGIS Pro SDK for .NET provides the following project and item templates:

C#  | VB| Name
------------------------  | -------------| ---------
![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProModuleC32.png "ArcGIS Pro Module C#") | ![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProModuleVB32.png "ArcGIS Pro Module VB") |  ArcGIS Pro Module Add-in Project template  
![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProConfigurationsC32.png "ArcGIS Pro Configurations C#") | ![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProConfigurationsVB32.png "ArcGIS Pro Configurations VB") |  ArcGIS Pro Managed Configurations Project template  
![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProPluginC32.png "ArcGIS Pro Plugin C#") | N/A |  ArcGIS Pro Plugin Project template 
![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProCoreHostC32.png "ArcGIS Pro CoreHost Application C#") | N/A |  ArcGIS Pro CoreHost Application Project template 
![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProBackstageTabC32.png "ArcGIS Pro Backstage Tab C#") | ![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProBackstageTabVB32.png "ArcGIS Pro Backstage Tab VB") | ArcGIS Pro Backstage Tab 
![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProButtonC32.png "ArcGIS Pro Button C#") | ![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProButtonVB32.png "ArcGIS Pro Button VB") | ArcGIS Pro Button 
![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProButtonPaletteC32.png "ArcGIS Pro Button Palette C#") | ![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProButtonPaletteVB32.png "ArcGIS Pro Button Palette VB") | ArcGIS Pro Button Palette
![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProComboBoxC32.png "ArcGIS Pro Combo Box C#") | ![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProComboBoxVB32.png "ArcGIS Pro Combo Box VB") | ArcGIS Pro Combo Box
![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProConstructionToolC32.png "ArcGIS Pro Construction Tool C#") | ![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProConstructionToolVB32.png "ArcGIS Pro Construction Tool VB") | ArcGIS Pro Construction Tool
![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProCustomControlC32.png "ArcGIS Pro Custom Control C#") | ![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProCustomControlVB32.png "ArcGIS Pro Custom Control VB") | ArcGIS Pro Custom Control
![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProCustomControlC32.png "ArcGIS Pro Custom Item C#") | N/A | ArcGIS Pro Custom Item
![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProCustomControlC32.png "ArcGIS Pro Custom Project Item C#") | N/A | ArcGIS Pro Custom Project Item
![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProDockPaneC32.png "ArcGIS Pro Dockpane C#") | ![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProDockPaneVB32.png "ArcGIS Pro Dockpane VB") | ArcGIS Pro Dockpane
![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProDockPaneC32.png "ArcGIS Pro Dockpane with Burger Button C#") | ![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProDockPaneVB32.png "ArcGIS Pro Dockpane with Burger Button VB") | ArcGIS Pro Dockpane with Burger Button
![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProDropHandlerC32.png "ArcGIS Pro Drop Handler C#") | ![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProDropHandlerVB32.png "ArcGIS Pro Drop Handler VB") | ArcGIS Pro Drop Handler
![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProEmbeddableControlC32.png "ArcGIS Pro Embeddable Control C#") | ![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProEmbeddableControlVB32.png "ArcGIS Pro Embeddable Control VB") | ArcGIS Pro Embeddable Control
![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProGalleryC32.png "ArcGIS Pro Gallery C#") | ![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProGalleryVB32.png "ArcGIS Pro Gallery VB") | ArcGIS Pro Gallery
![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProInLineGalleryC32.png "ArcGIS Pro Inline-Gallery C#") | ![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProInLineGalleryVB32.png "ArcGIS Pro Inline-Gallery VB") | ArcGIS Pro Inline-Gallery
![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProMapPaneC32.png "ArcGIS Pro Map Pane Impersonation C#") | ![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProMapPaneVB32.png "ArcGIS Pro Map Pane Impersonation VB") | ArcGIS Pro Map Pane Impersonation
![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProMapToolC32.png "ArcGIS Pro Map Tool C#") | ![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProMapToolVB32.png "ArcGIS Pro Map Tool VB") | ArcGIS Pro Map Tool
![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProMenuC32.png "ArcGIS Pro Menu C#") | ![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProMenuVB32.png "ArcGIS Pro Menu VB") | ArcGIS Pro Menu
![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProPaneC32.png "ArcGIS Pro Pane C#") | ![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProPaneVB32.png "ArcGIS Pro Pane VB") | ArcGIS Pro Pane
![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProPropertySheetC32.png "ArcGIS Pro Property Sheet C#") | ![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProPropertySheetVB32.png "ArcGIS Pro Property Sheet VB") | ArcGIS Pro Property Sheet
![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProSplitButtonC32.png "ArcGIS Pro Split Button C#") | ![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProSplitButtonVB32.png "ArcGIS Pro Split Button VB") | ArcGIS Pro Split Button


#### ArcGIS Pro SDK for .NET utilities 
Package: proapp-sdk-utilities.vsix  

ArcGIS Pro SDK for .NET (Utilities) provides the following utilities that extend the Visual Studio environment:

![pro-fix-references](http://ArcGIS.github.io/arcgis-pro-sdk/images/Home/proapp-sdk-utilities.png "ArcGIS Pro SDK(Utilities)") 

Name               | Description
----------------------------  | --------------------------------------
Pro Fix References utility | Fixes broken references in an ArcGIS Pro add-in. Broken references can be caused when you share add-ins with other colleagues or download add-ins where the ArcGIS Pro assembly references point to a different location from where you installed them.
Pro Generate DAML Ids utility| Converts all of the ArcGIS Pro Desktop Application Markup Language (DAML) string IDs into static string properties organized by DAML element types (for example, Button, Dockpane, Tool, Condition, and so on). This allows you to use the IntelliSense feature of Visual Studio within your source code file to add IDs, rather than having to manually type DAML string IDs).

## Getting started
See [ProGuide: Build your first add-in](https://github.com/Esri/arcgis-pro-sdk/wiki/ProGuide-Build-Your-First-Add-in) for step-by-step instructions on creating a basic button that appears on the ArcGIS Pro ribbon.

## ProSnippets

ProSnippets are ready-made snippets of code you can quickly insert into your ArcGIS Pro add-in. [List of available ProSnippets](https://github.com/Esri/arcgis-pro-sdk/wiki/ProSnippets).

## ArcGIS Pro API

The ArcGIS Pro APIs are managed .NET assemblies. Intermediary assemblies containing .NET metadata or PIAs (Primary Interop Assemblies) are not required.

Add any of the ArcGIS Pro managed assemblies that comprise its API as references directly in your Visual Studio add-in projects

![pro-references.png](http://ArcGIS.github.io/arcgis-pro-sdk/images/Home/pro-references.png "ArcGIS Pro API References") 

A complete list of the ArcGIS Pro assemblies in the public API is provided below:

### Core

Core assemblies are located in the {ArcGIS Pro Installation folder}\bin.

Assembly           | Description
------------------------| -------------
ArcGIS.Core.dll        | Provides CIM, Geodatabase, Geometry and Utility Network APIs.
ArcGIS.CoreHost.dll | Provides Host.Initialize to initialize ArcGIS.Core.dll for stand-alone use.
ArcGIS.Desktop.Framework.dll        | Provides the application framework to include add-in contracts, DAML support, and base classes. This assembly must be referenced by every add-in.
ESRI.ArcGIS.ItemIndex.dll       | Provides functionality to create and work with Custom items.


### Extensions

Major subsystems within ArcGIS Pro are organized into units called extensions. Extension assemblies are located in the {ArcGIS Pro Installation folder}\bin\Extensions folder in their own individual subfolder. 
Extension subfolder names are logically named for the unit of functionality they represent, for example, Mapping, Editing, Layout, and so on.

Assembly           | Description
------------------------| -------------
ArcGIS.Desktop.Catalog.dll   | Provides access to project content items (map items, layout items, style items, folder items, and so on).
ArcGIS.Desktop.Core.dll    | Provides functionality to create and manage projects, access to events associated with the current project, and the ability to execute geoprocessing tools.
ArcGIS.Desktop.DataReviewer.dll | Provides functionality to establish and manage Reviewer results, sessions, and batch jobs in a project.
ArcGIS.Desktop.Editing.dll        | Provides access to the editing environment and core editing functionality required for custom edit tool implementations.
ArcGIS.Desktop.Extensions.dll        | Provides extension methods for other ArcGIS Pro classes. Provides a base class for custom map tools.
ArcGIS.Desktop.Geoprocessing.dll        | Provides access to geoprocessing history items stored in the project. (Note: Adds a reference to ArcGIS.Desktop.Core.dll to execute geoprocessing tools.)
ArcGIS.Desktop.Layouts.dll        | Provides functionality for manipulating elements on a layout and exporting to a variety of image formats.
ArcGIS.Desktop.Mapping.dll        | Provides types to create maps and layers, label features, perform query operations, and visualize them in 2D or 3D. Provides a raster API to create raster layers and customize raster rendering, and an API to manage styles, style items, and symbols.
ArcGIS.Desktop.TaskAssistant.dll        | Provides the Tasks framework, allowing developers to access, open, close, or export task items.
ArcGIS.Desktop.Workflow.dll       | Provides functionality to create, configure, and execute Workflow Manager jobs and queries. Provides functionality to retrieve configuration information from the Workflow Manager database.

### Extensions with no public API

There are extension assemblies in {ArcGIS Pro Installation folder}\bin\Extensions subfolders) that do not have a public API. They are currently for Esri internal use only.

* ArcGIS.Desktop.Analyst3D.dll
* ArcGIS.Desktop.Aviation.dll
* ArcGIS.Desktop.BusinessAnalyst.dll
* ArcGIS.Desktop.Charts.dll
* ArcGIS.Desktop.DataSourcesRaster.dll
* ArcGIS.Desktop.DefenseMapping.dll
* ArcGIS.Desktop.Editing.PushPull.dll
* ArcGIS.Desktop.FullMotionVideo.dll
* ArcGIS.Desktop.GAWizard.dll
* ArcGIS.Desktop.Geostatistics.dll
* ArcGIS.Desktop.LocationReferencing.dll
* ArcGIS.Desktop.Metadata.dll
* ArcGIS.Desktop.NetworkAnalysis.Facility.dll
* ArcGIS.Desktop.NetworkAnalysis.NetworkDiagrams.dll
* ArcGIS.Desktop.NetworkAnalysis.Transportation.dll
* ArcGIS.Desktop.Search.dll
* ArcGIS.Desktop.Sharing.dll
* ArcGIS.Desktop.TerritoryDesign.dll

Note: Static string resource properties and image resources included within the public API assemblies are for Esri internal use only. They are not intended for use in third-party add-ins.

## Release notes 

### ArcGIS Pro 2.5 SDK for .NET

These release notes describe details of the ArcGIS Pro 2.5 SDK for .NET release. Here you will find information about available functionality as well as known issues and limitations.

#### What's new

The following functionality is available at the ArcGIS Pro 2.5 SDK for .NET release:

#### 1. API Enhancements  

**Geometry API:**<br/>
* Support for writing multipatch features, with the ability to update geometry, and apply materials and textures.

**Content API**<br/>
* New custom search and browse filters.
 
**Map Authoring API:**<br/>
* Layer rendering enhancements.

**Other API Enhancements:**<br/>
* New classes and methods available in the Editing, Geodatabase, Mapping, and Raster APIs.

**.NET Framework 4.8:**<br/>
*  With the release of ArcGIS Pro 2.5, the minimum .NET target has been changed from 4.6.1 to 4.8. What does this mean for you and your add-ins?
      * Existing add-ins, already deployed, will work at 2.5 with no change to their forward compatibility.
      * New add-ins created at 2.5 will require the minimum target framework set to 4.8 or they will not compile (this is the default setting in the Pro SDK).
      * Existing add-ins which are recompiled at 2.5 (e.g. because a code change was made) will also require the minimum target framework set to 4.8 or they will not compile. Note: As always, if an existing add-in is changed for any reason, the desktopVersion attribute in its Config.daml file should be changed to reflect the version of Pro it was last compiled against, in this case, now 2.5..


For a detailed list of changes to the ArcGIS Pro API refer to the [What's new for developers at 2.5
](https://pro.arcgis.com/en/pro-app/sdk/api-reference/#topic15120.html) topic in the [ArcGIS Pro  API Reference Guide](https://pro.arcgis.com/en/pro-app/sdk/api-reference/#topic1.html).

#### 3. SDK Resources

There are many ProConcepts, ProGuide, ProSnippets, and samples to help you get up and running with the new SDK features including:

Updates to the SDK Resources include, but are not limited to:

* [ProConcepts: Framework](https://github.com/ArcGIS/arcgis-pro-sdk/wiki/ProConcepts-Framework)
* [ProGuide: Custom Dictionary Style](https://github.com/Esri/arcgis-pro-sdk/wiki/ProGuide-Custom-Dictionary-Style)
* [ProConcepts: Content and Items](https://github.com/Esri/arcgis-pro-sdk/wiki/ProConcepts-Content-and-Items)
* [ProConcepts: Custom Items](https://github.com/Esri/arcgis-pro-sdk/wiki/ProConcepts-Custom-Items)
* [ProGuide: Custom Items](https://github.com/Esri/arcgis-pro-sdk/wiki/ProGuide-Custom-Items)
* [ProGuide: Custom Browse Dialog Filters](https://github.com/Esri/arcgis-pro-sdk/wiki/ProGuide-Custom-Browse-Dialog-Filters)
* [ProConcepts: Editing](https://github.com/ArcGIS/arcgis-pro-sdk/wiki/ProConcepts-Editing)
* [ProSnippets: Browse Dialog Filters](https://github.com/ArcGIS/arcgis-pro-sdk/wiki/ProSnippets-Browse-Dialog-Filters)
* [ArcGIS Pro TypeID Reference](https://github.com/ArcGIS/arcgis-pro-sdk/wiki/ArcGIS-Pro-TypeID-Reference)
* The [Pro Community Samples](https://github.com/Esri/arcgis-pro-sdk-community-samples) and [Snippets](https://github.com/Esri/arcgis-pro-sdk/wiki/ProSnippets) have new samples and snippets.
* The API Changes section of the [What’s New for Developers 2.5](https://pro.arcgis.com/en/pro-app/sdk/api-reference/#topic15120.html) page. 

![ArcGIS Pro SDK for .NET Icons](https://esri.github.io/arcgis-pro-sdk/images/Home/Image-of-icons-first.png "ArcGIS Pro SDK Icons")
![ArcGIS Pro SDK for .NET Icons](https://esri.github.io/arcgis-pro-sdk/images/Home/Image-of-icons-second.png "ArcGIS Pro SDK Icons")

You can use the Pro SDK Icons as the image for your controls on the Pro Ribbon. Code snippet below provides the pack URI to be used in your add-in's config.daml.

```xml
<button...largeImage="pack://application:,,,/ArcGIS.Desktop.Resources;component/Images/<ImageNameHere>"/>
```

## Previous versions

* [ArcGIS Pro 2.4 SDK for .NET](https://github.com/Esri/arcgis-pro-sdk/releases/tag/2.4.0.19948)
* [ArcGIS Pro 2.3 SDK for .NET](https://github.com/Esri/arcgis-pro-sdk/releases/tag/2.3.0.15769)
* [ArcGIS Pro 2.2 SDK for .NET](https://github.com/Esri/arcgis-pro-sdk/releases/tag/2.2.0.12813)
* [ArcGIS Pro 2.1 SDK for .NET](https://github.com/Esri/arcgis-pro-sdk/releases/tag/2.1.0.10257)
* [ArcGIS Pro 2.0 SDK for .NET](https://github.com/Esri/arcgis-pro-sdk/releases/tag/2.0.0.8933)
* [ArcGIS Pro 1.4 SDK for .NET](https://github.com/Esri/arcgis-pro-sdk/releases/tag/1.4.0.7198)
* [ArcGIS Pro 1.3 SDK for .NET](https://github.com/Esri/arcgis-pro-sdk/releases/tag/1.3.0.5861)
* [ArcGIS Pro 1.2 SDK for .NET](https://github.com/Esri/arcgis-pro-sdk/releases/tag/1.2.0.5023)
* [ArcGIS Pro 1.1 SDK for .NET](https://github.com/Esri/arcgis-pro-sdk/releases/tag/1.1.0.3318)

## Contributing

Esri welcomes contributions from anyone and everyone. For more information, see our [guidelines for contributing](https://github.com/esri/contributing).

## Issues

Find a bug or want to request a new feature? Let us know by submitting an issue.

## Licensing
Copyright 2020 Esri

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


<p align = center><img src="http://esri.github.io/arcgis-pro-sdk/images/ArcGISPro.png"  alt="pre-req" align = "top" height = "20" width = "20" ><b> ArcGIS Pro 2.5 SDK for Microsoft .NET Framework</b></p>

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[Home](https://github.com/Esri/arcgis-pro-sdk/wiki) | <a href="http://pro.arcgis.com/en/pro-app/sdk/api-reference/index.html" target="_blank">API Reference</a> | [Requirements](#requirements) | [Download](#installing-arcgis-pro-sdk-for-net) |  <a href="http://github.com/esri/arcgis-pro-sdk-community-samples" target="_blank">Samples</a>



