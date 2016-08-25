## ArcGIS Pro 1.3 SDK for .NET

Extend ArcGIS Pro with ArcGIS Pro SDK for .NET. ArcGIS Pro SDK for .NET is based on the add-in extensibility pattern (first introduced at 10.0). Leverage modern .NET features and patterns such as Task Asynchronous Programming (TAP), LINQ, WPF Binding, and MVVM to write integrated 2D/3D add-ins using Pro’s new APIs.

<a href="http://pro.arcgis.com/en/pro-app/sdk/" target="_blank">View it live</a>

###Table of Contents

####Developing with ArcGIS Pro

* [Requirements](#requirements)
* [Installing ArcGIS Pro SDK for .NET](#installing-arcgis-pro-sdk-for-net)
* [ArcGIS Pro SDK for .NET components](#arcgis-pro-sdk-for-net-components)
  * [ArcGIS Pro SDK for .NET templates](#arcgis-pro-sdk-for-net-templates)
  * [ArcGIS Pro SDK for .NET utilities](#arcgis-pro-sdk-for-net-utilities)
* [Getting started](#getting-started) 
* [ProSnippets](#prosnippets)  
* [ArcGIS Pro API](#arcgis-pro-api)
  * [Core](#core)
  * [Extensions](#extensions)
  * [Extensions with no public API](#extensions-with-no-public-api)
* [Release notes](#release-notes)
 * [ArcGIS Pro 1.3 SDK for .NET](#arcgis-pro-13-sdk-for-net-1)
 * [ArcGIS Pro 1.2 SDK for .NET](https://github.com/Esri/arcgis-pro-sdk/releases/tag/1.2.0.5023)
 * [ArcGIS Pro 1.1 SDK for .NET](https://github.com/Esri/arcgis-pro-sdk/releases/tag/1.1.0.3318)
 
* [Resources](#resources)

####Add-in fundamentals 
 * [Pro Guide: Your first add-in](../../wiki/ProGuide-Build-Your-First-Add-in)
 * [ProConcept: Localization](../../wiki/ProConcept-Localization)
 * [ProGuide: Digitally signed add-ins](../../wiki/ProGuide-Digitally-signed-add-ins)
 * [ProGuide: Content and image resources](../../wiki/ProGuide-content-and-image-resources)
 * [ProGuide: Diagnosing ArcGIS Pro Add-ins](../../wiki/ProGuide-Diagnosing-ArcGIS-Pro-Addins)

&nbsp;&nbsp;&nbsp;&nbsp;**Regression Testing**

* [ProGuide: Regression Testing](../../wiki/ProGuide-Regression-Testing)

####Framework

* [ProConcepts: Framework](../../wiki/ProConcepts-Framework)
* [ProSnippets: Framework](../../wiki/ProSnippets-Framework)
* [ProGuide: Commandlines switches for ArcGISPro.exe](../../wiki/ProGuide-Command-lines-switches-for-ArcGISPro.exe)

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
* [ProGuide: ArcGIS Pro Styles](../../wiki/ProGuide-ArcGIS-Pro-Styles)
* [ProGuide: Customize the Splash Screen](../../wiki/ProGuide-ArcGISPro-Config-Xml#how-to-customize-the-splash-screen)
* [ProGuide: Apply DAML Customization](../../wiki/ProGuide-ArcGISPro-Config-Xml#how-to-apply-a-daml-customization)

-------------------------

###Content

* [ProSnippets: Content](../../wiki/ProSnippets-content)

--------------------------

###CoreHost

* [ProConcepts: CoreHost](../../wiki/proconcepts-CoreHost)
* [ProSnippets: CoreHost](../../wiki/ProSnippets-CoreHost)

--------------------------

###DataReviewer

* [ProConcepts: DataReviewer](../../wiki/proconcepts-DataReviewer)

--------------------------

###Editing

* [ProConcepts: Editing](../../wiki/ProConcepts-Editing)
* [ProSnippets: Editing](../../wiki/ProSnippets-Editing)
* [ProGuide: Editing Tool](../../wiki/ProGuide-Editing-Tool)

--------------------------

###Geodatabase

* [ProConcepts: Geodatabase](../../wiki/ProConcepts-Geodatabase)
* [ProSnippets: Geodatabase](../../wiki/ProSnippets-Geodatabase)

--------------------------

###Geometry

* [ProConcepts: Geometry](../../wiki/ProConcepts-Geometry)
* [ProSnippets: Geometry](../../wiki/ProSnippets-Geometry)

&nbsp;&nbsp;&nbsp;&nbsp;**Relational Operations**

* [ProGuide: RelationalOperations](../../wiki/ProGuide-Relational-Operations)
* [ProSnippets: Custom Relational Operations](../../wiki/ProGuide-Custom-Relational-Operations)

--------------------------

###Geoprocessing

* [ProConcepts: Geoprocessing](../../wiki/ProConcepts-Geoprocessing)
* [ProSnippets: Geoprocessing](../../wiki/ProSnippets-Geoprocessing)

--------------------------

###Layouts

* [ProSnippets: Layouts](../../wiki/ProSnippets-Layouts)

--------------------------

###Map Authoring

* [ProConcepts: Map Authoring](../../wiki/ProConcepts-Map-Authoring)
* [ProSnippets: Map Authoring](../../wiki/ProSnippets-MapAuthoring)
 
--------------------------

###Map Exploration

* [ProConcept: Map Exploration](../../wiki/ProConcepts-Map-Exploration)
* [ProSnippets: Map Exploration](../../wiki/ProSnippets-MapExploration)

&nbsp;&nbsp;&nbsp;&nbsp;**Map Tools**<br>

* [ProGuide: Feature Selection](../../wiki/ProGuide-Feature-Selection)
* [ProGuide: Identify](../../wiki/ProGuide-Identify)
* [ProGuide: MapView Interaction](../../wiki/ProGuide-MapView-Interaction)
* [ProGuide: Using Embeddable Controls](../../wiki/ProGuide-Using-Embeddable-Controls)
* [ProGuide: Custom Popups](../../wiki/ProGuide-Custom-Popups)
* [ProGuide: Dynamic Popup Menu](../../wiki/ProGuide-Dynamic-Popup-Menu)

--------------------------

###Sharing

* [ProSnippets: Sharing](../../wiki/ProSnippets-sharing)

--------------------------

###Tasks

* [ProConcepts: Tasks](../../wiki/ProConcepts-Tasks)
* [ProSnippets: Tasks](../../wiki/ProSnippets-Tasks)

--------------------------

###Workflow Manager

* [ProConcept: Workflow Manager](../../wiki/ProConcepts-Workflow-Manager)
* [ProSnippets: Workflow Manager](../../wiki/ProSnippets-Workflow-Manager)

--------------------------

##Requirements
The requirements for the machine on which you develop your ArcGIS Pro add-ins are listed here.

####ArcGIS Pro

* ArcGIS Pro 1.3

####Supported platforms

* Windows 10 (Home, Pro, Enterprise, Education) (64 bit [EM64T])
* Windows 8.1 Basic, Professional, and Enterprise (64 bit [EM64T]) 
* Windows 8 Basic, Professional, and Enterprise (64 bit [EM64T]) 
* Windows 7 SP1 Ultimate, Enterprise, Professional, and Home Premium (64 bit [EM64T]) 

####Supported .NET framework

* Microsoft .NET Framework 4.6.1 Developer Pack ![](http://Esri.github.io/arcgis-pro-sdk/images/Common/NewIcon_Blue.png)

####Supported IDEs

* Visual Studio 2015 (Professional, Enterprise, and Community Editions)
* Visual Studio 2013 (Professional, Premium, Ultimate, and Community Editions)
 
Note: [ArcGIS Pro system requirements](http://pro.arcgis.com/en/pro-app/get-started/arcgis-pro-system-requirements.htm) 

##Installing ArcGIS Pro SDK for .NET

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


####ArcGIS Pro SDK for .NET templates 
Package: proapp-sdk-templates.vsix  

ArcGIS Pro SDK for .NET provides the following project and item templates:

C#  | VB| Name
------------------------  | -------------| ---------
![](http://Esri.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProModuleC32.png "ArcGIS Pro Module C#") | ![](http://Esri.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProModuleVB32.png "ArcGIS Pro Module VB") | ArcGIS Pro Module Add-in  
![](http://Esri.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProBackstageTabC32.png "ArcGIS Pro Backstage Tab C#") | ![](http://Esri.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProBackstageTabVB32.png "ArcGIS Pro Backstage Tab VB") | ArcGIS Pro Backstage Tab 
![](http://Esri.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProButtonC32.png "ArcGIS Pro Button C#") | ![](http://Esri.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProButtonVB32.png "ArcGIS Pro Button VB") | ArcGIS Pro Button 
![](http://Esri.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProButtonPaletteC32.png "ArcGIS Pro Button Palette C#") | ![](http://Esri.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProButtonPaletteVB32.png "ArcGIS Pro Button Palette VB") | ArcGIS Pro Button Palette
![](http://Esri.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProComboBoxC32.png "ArcGIS Pro Combo Box C#") | ![](http://Esri.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProComboBoxVB32.png "ArcGIS Pro Combo Box VB") | ArcGIS Pro Combo Box
![](http://Esri.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProConstructionToolC32.png "ArcGIS Pro Construction Tool C#") | ![](http://Esri.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProConstructionToolVB32.png "ArcGIS Pro Construction Tool VB") | ArcGIS Pro Construction Tool
![](http://Esri.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProCustomControlC32.png "ArcGIS Pro Custom Control C#") | ![](http://Esri.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProCustomControlVB32.png "ArcGIS Pro Custom Control VB") | ArcGIS Pro Custom Control
![](http://Esri.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProDockPaneC32.png "ArcGIS Pro Dockpane C#") | ![](http://Esri.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProDockPaneVB32.png "ArcGIS Pro Dockpane VB") | ArcGIS Pro Dockpane
![](http://Esri.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProDockPaneC32.png "ArcGIS Pro Dockpane with Burger Button C#") | ![](http://Esri.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProDockPaneVB32.png "ArcGIS Pro Dockpane with Burger Button VB") | ArcGIS Pro Dockpane with Burger Button
![](http://Esri.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProDropHandlerC32.png "ArcGIS Pro Drop Handler C#") | ![](http://Esri.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProDropHandlerVB32.png "ArcGIS Pro Drop Handler VB") | ArcGIS Pro Drop Handler
![](http://Esri.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProEmbeddableControlC32.png "ArcGIS Pro Embeddable Control C#") | ![](http://Esri.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProEmbeddableControlVB32.png "ArcGIS Pro Embeddable Control VB") | ArcGIS Pro Embeddable Control
![](http://Esri.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProGalleryC32.png "ArcGIS Pro Gallery C#") | ![](http://Esri.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProGalleryVB32.png "ArcGIS Pro Gallery VB") | ArcGIS Pro Gallery
![](http://Esri.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProInLineGalleryC32.png "ArcGIS Pro Inline-Gallery C#") | ![](http://Esri.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProInLineGalleryVB32.png "ArcGIS Pro Inline-Gallery VB") | ArcGIS Pro Inline-Gallery
![](http://Esri.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProMapToolC32.png "ArcGIS Pro Map Tool C#") | ![](http://Esri.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProMapToolVB32.png "ArcGIS Pro Map Tool VB") | ArcGIS Pro Map Tool
![](http://Esri.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProMenuC32.png "ArcGIS Pro Menu C#") | ![](http://Esri.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProMenuVB32.png "ArcGIS Pro Menu VB") | ArcGIS Pro Menu
![](http://Esri.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProPaneC32.png "ArcGIS Pro Pane C#") | ![](http://Esri.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProPaneVB32.png "ArcGIS Pro Pane VB") | ArcGIS Pro Pane
![](http://Esri.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProPropertySheetC32.png "ArcGIS Pro Property Sheet C#") | ![](http://Esri.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProPropertySheetVB32.png "ArcGIS Pro Property Sheet VB") | ArcGIS Pro Property Sheet
![](http://Esri.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProSketchToolC32.png "ArcGIS Pro Sketch Tool C#") | ![](http://Esri.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProSketchToolVB32.png "ArcGIS Pro Sketch Tool VB") | ArcGIS Pro Sketch Tool _(Deprecated, use Map Tool.)_
![](http://Esri.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProSplitButtonC32.png "ArcGIS Pro Split Button C#") | ![](http://Esri.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProSplitButtonVB32.png "ArcGIS Pro Split Button VB") | ArcGIS Pro Split Button
![](http://Esri.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProToolC32.png "ArcGIS Pro Tool C#") | ![](http://Esri.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProToolVB32.png "ArcGIS Pro Tool VB") | ArcGIS Pro Tool


####ArcGIS Pro SDK for .NET utilities 
Package: proapp-sdk-utilities.vsix  

ArcGIS Pro SDK for .NET (Utilities) provides the following utilities that extend the Visual Studio environment:

![pro-fix-references](http://Esri.github.io/arcgis-pro-sdk/images/Home/proapp-sdk-utilities.png "ArcGIS Pro SDK(Utilities)") 

Name               | Description
----------------------------  | --------------------------------------
Pro Fix References utility | Fixes broken references in an ArcGIS Pro add-in. Broken references can be caused when you share add-ins with other colleagues or download add-ins where the ArcGIS Pro assembly references point to a different location from where you installed them.
Pro Generate DAML Ids utility| Converts all of the ArcGIS Pro Desktop Application Markup Language (DAML) string IDs into static string properties organized by DAML element types (for example, Button, Dockpane, Tool, Condition, and so on). This allows you to use the IntelliSense feature of Visual Studio within your source code file to add IDs, rather than having to manually type DAML string IDs).

## Getting started
See [ProGuide: Build your first add-in](https://github.com/Esri/arcgis-pro-sdk/wiki/ProGuide-Build-Your-First-Add-in) for step-by-step instructions on creating a basic button that appears on the ArcGIS Pro ribbon.

## ProSnippets

ProSnippets are ready-made snippets of code you can quickly insert into your ArcGIS Pro add-in. [List of available ProSnippets](https://github.com/Esri/arcgis-pro-sdk/wiki/ProSnippets).

##ArcGIS Pro API

The ArcGIS Pro APIs are managed .NET assemblies. Intermediary assemblies containing .NET metadata or PIAs (Primary Interop Assemblies) are not required.

Add any of the ArcGIS Pro managed assemblies that comprise its API as references directly in your Visual Studio add-in projects

![pro-references.png](http://Esri.github.io/arcgis-pro-sdk/images/Home/pro-references.png "ArcGIS Pro API References") 

A complete list of the ArcGIS Pro assemblies in the public API is provided below:

###Core

Core assemblies are located in the {ArcGIS Pro Installation folder}\bin.

Assembly           | Description
------------------------| -------------
ArcGIS.Core.dll        | Provides CIM, Geodatabase, and Geometry APIs.
ArcGIS.CoreHost.dll | Provides Host.Initialize to initialize ArcGIS.Core.dll for stand-alone use.
ArcGIS.Desktop.Framework.dll        | Provides the application framework to include add-in contracts, DAML support, and base classes. This assembly must be referenced by every add-in.


###Extensions

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
* ArcGIS.Desktop.Charts.dll
* ArcGIS.Desktop.DataSourcesRaster.dll
* ArcGIS.Desktop.Geostatistics.dll
* ArcGIS.Desktop.Metadata.dll
* ArcGIS.Desktop.NetworkAnalysis.Facility.dll
* ArcGIS.Desktop.NetworkAnalysis.NetworkDiagrams.dll
* ArcGIS.Desktop.NetworkAnalysis.Transportation.dll
* ArcGIS.Desktop.Search.dll
* ArcGIS.Desktop.Sharing.dll

Note: Static string resource properties and image resources included within the public API assemblies are for Esri internal use only. They are not intended for use in third-party add-ins.

##Release notes 

###ArcGIS Pro 1.3 SDK for .NET

These release notes describe details of the ArcGIS Pro 1.3 SDK for .NET release. Here you will find information about available functionality as well as known issues and limitations.

####What's new

The following functionality is available at the ArcGIS Pro 1.3 SDK for .NET release:

#####1. .NET Framework 4.6.1 is now supported.

The default target framework for ArcGIS Pro add-ins is now .NET Framework 4.6.1.

#####2. Sketch tool is now deprecated. 

The functionality of the Sketch tool is now available with the Map tool. Refer to the new ProGuides for Map Tools under the Map Exploration section. Also refer to the [Map Tool](https://github.com/Esri/arcgis-pro-sdk/wiki/ProConcepts-Map-Exploration#maptool) section in the [ProConcepts: Map Exploration](https://github.com/Esri/arcgis-pro-sdk/wiki/ProConcepts-Map-Exploration).

#####3. New classes and methods in the Pro API

For a detailed list of changes to the ArcGIS Pro API refer to the [What is new for developers at 1.3](http://pro.arcgis.com/en/pro-app/sdk/api-reference/#topic14079.html) topic in the [ArcGIS Pro  API Reference Guide](http://pro.arcgis.com/en/pro-app/sdk/api-reference/#topic1.html).

##Previous versions

* [ArcGIS Pro 1.2 SDK for .NET](https://github.com/Esri/arcgis-pro-sdk/releases/tag/1.2.0.5023)
* [ArcGIS Pro 1.1 SDK for .NET](https://github.com/Esri/arcgis-pro-sdk/releases/tag/1.1.0.3318)

##Resources

* [API Reference online](http://pro.arcgis.com/en/pro-app/sdk/api-reference)
* <a href="http://pro.arcgis.com/en/pro-app/sdk/" target="_blank">ArcGIS Pro SDK for .NET (pro.arcgis.com)</a>
* [arcgis-pro-sdk-community-samples](http://github.com/Esri/arcgis-pro-sdk-community-samples)
* [ArcGISPro Registry Keys](http://github.com/Esri/arcgis-pro-sdk/wiki/ArcGIS-Pro-Registry-Keys)
* [ArcGIS Pro DAML ID Reference](http://github.com/Esri/arcgis-pro-sdk/wiki/ArcGIS Pro DAML ID Reference)
* [ArcGISPro Config XML Reference](http://github.com/Esri/arcgis-pro-sdk/wiki/ArcGIS-Pro-Config-XML-Reference)
* [FAQ](http://github.com/Esri/arcgis-pro-sdk/wiki/FAQ)
* [ArcGIS Pro SDK icons](https://github.com/Esri/arcgis-pro-sdk/releases/tag/1.3.0.5861)


![ArcGIS Pro SDK for .NET Icons](https://esri.github.io/arcgis-pro-sdk/images/Home/Image-of-icons.png "ArcGIS Pro SDK Icons")

##Contributing

Esri welcomes contributions from anyone and everyone. For more information, see our [guidelines for contributing](https://github.com/esri/contributing).

##Issues

Find a bug or want to request a new feature? Let us know by submitting an issue.

## Licensing
Copyright 2016 Esri

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


<p align = center><img src="http://esri.github.io/arcgis-pro-sdk/images/ArcGISPro.png"  alt="pre-req" align = "top" height = "20" width = "20" >
<b> ArcGIS Pro 1.3 SDK for Microsoft .NET Framework</b>
</p>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[Home](https://github.com/Esri/arcgis-pro-sdk/wiki) | <a href="http://pro.arcgis.com/en/pro-app/sdk" target="_blank">ArcGIS Pro SDK</a> | <a href="http://pro.arcgis.com/en/pro-app/sdk/api-reference/index.html" target="_blank">API Reference</a> | [Requirements](#requirements) | [Download](#installing-arcgis-pro-sdk-for-net) |  <a href="http://github.com/esri/arcgis-pro-sdk-community-samples" target="_blank">Samples</a>



