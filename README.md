## ArcGIS Pro 3.5 SDK for .NET

```
ArcGIS Pro Version: 3.5.0.57365
```

Extend ArcGIS Pro with the ArcGIS Pro SDK for Microsoft .NET.  The ArcGIS Pro SDK provides four main extensibility patterns:  add-ins, managed configurations, plugin datasources and CoreHost applications.  You can leverage modern .NET features and patterns such as Task Asynchronous Programming (TAP), LINQ, WPF Binding, and MVVM to build integrated 2D and 3D add-ins with the ArcGIS Pro APIs.

<a href="http://pro.arcgis.com/en/pro-app/sdk/" target="_blank">View it live</a>

### Table of Contents

#### Developing with ArcGIS Pro

* [Overview of the ArcGIS Pro SDK](#overview-of-the-arcgis-pro-sdk)
* [What's New for Developers at 3.5](#whats-new-for-developers-at-35)
   * [ArcGIS Pro 3.3.3 and 3.4.1 Patches](#arcgis-pro-333-and-341-patches)
* [Installing ArcGIS Pro SDK for .NET](#installing-arcgis-pro-sdk-for-net)
* [Release notes](#release-notes)
* [Resources](#resources)
* [Pro SDK Videos](https://mediaspace.esri.com/channel/ArcGIS+Pro+SDK+for+.NET/256320933)
* [ProSnippets](../../wiki/ProSnippets)  
* [ArcGIS Pro API](#arcgis-pro-api) 
* [ProGuide: ArcGIS Pro Extensions NuGet](../../wiki/ProGuide-ArcGIS-Pro-Extensions-NuGet)

&nbsp;&nbsp;&nbsp;&nbsp;**Migration**

* [ProConcepts: 3.0 Migration Guide](../../wiki/ProConcepts-3.0-Migration-Guide)
* [ProGuide: .NET 8 Upgrade](../../wiki/ProGuide-NET-8-Upgrade)

#### Framework

* [ProConcepts: Framework](../../wiki/ProConcepts-Framework)
* [ProConcepts: Asynchronous Programming in ArcGIS Pro](../../wiki/ProConcepts-Asynchronous-Programming-in-ArcGIS-Pro)
* [ProConcepts: Advanced topics](../../wiki/ProConcepts-Advanced-Topics)
* [ProGuide: Custom settings](../../wiki/ProGuide-Custom-settings)
* [ProGuide: Command line switches for ArcGISPro.exe](../../wiki/ProGuide-Command-line-switches-for-ArcGISPro.exe)
* [ProGuide: Reusing ArcGIS Pro Commands](../../wiki/ProGuide-Reusing-Pro-Commands)
* [ProGuide: Licensing](../../wiki/ProGuide-License-Your-Add-in)
* [ProGuide: Digital signatures](../../wiki/ProGuide-Digitally-signed-add-ins-and-configurations)
* [ProGuide: Command Search](../../wiki/ProGuide-Command-Search)
* [ProGuide: Keyboard shortcuts](../../wiki/ProGuide-Keyboard-shortcuts)
* [ProSnippets: Framework](../../wiki/ProSnippets-Framework)
* [ProSnippets: DAML](../../wiki/ProSnippets-DAML)


&nbsp;&nbsp;&nbsp;&nbsp;**Add-ins**

* [ProConcept: Localization](../../wiki/ProConcept-Localization)
* [Pro Guide: Installation and Upgrade](../../wiki/ProGuide-Installation-and-Upgrade)
* [Pro Guide: Your first add-in](../../wiki/ProGuide-Build-Your-First-Add-in)
* [ProGuide ArcGIS AllSource Project Template: Your first add-in](../../wiki/ProGuide-ArcGIS-AllSource-Project-Template)
* [ProGuide: Content and image resources](../../wiki/ProGuide-content-and-image-resources)
* [ProGuide: Embedding Toolboxes](../../wiki/ProGuide-content-and-image-resources#embedding-toolboxes)
* [ProGuide: Diagnosing ArcGIS Pro Add-ins](../../wiki/ProGuide-Diagnosing-ArcGIS-Pro-Add-ins)
* [ProGuide: Regression Testing](../../wiki/ProGuide-Regression-Testing)

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
* [ProGuide: Context Menus](../../wiki/ProGuide-Context-Menus)
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

### Arcade
* [ProConcepts: Arcade](../../wiki/ProConcepts-Arcade)
* [ProSnippets: Arcade](../../wiki/ProSnippets-Arcade)

-------------------------

### Content
* [ProConcepts: Project Content and Items](../../wiki/ProConcepts-Content-and-Items)
* [ProConcepts: Custom Items](../../wiki/ProConcepts-Custom-Items)
* [ProGuide: Custom Items](../../wiki/ProGuide-Custom-Items)
* [ProGuide: Custom Browse Dialog Filters](../../wiki/ProGuide-Custom-Browse-Dialog-Filters)
* [ArcGIS Pro TypeID Reference](../../wiki/ArcGIS-Pro-TypeID-Reference)
* [ProSnippets: Content](../../wiki/ProSnippets-content)
* [ProSnippets: Browse Filters](../../wiki/ProSnippets-OpenItemDialogBrowseFilter)

--------------------------

### CoreHost
* [ProConcepts: CoreHost](../../wiki/proconcepts-CoreHost)
* [ProSnippets: CoreHost](../../wiki/ProSnippets-CoreHost)

--------------------------

### DataReviewer

* [ProConcepts: DataReviewer](../../wiki/proconcepts-DataReviewer)

--------------------------

### Editing
* [ProConcepts: Editing](../../wiki/ProConcepts-Editing)
* [ProConcepts: COGO](../../wiki/ProConcepts-cogo)
* [ProConcepts: Annotation Editing](../../wiki/ProConcepts-Editing-Annotation)
* [ProConcepts: Dimension Editing](../../wiki/ProConcepts-Editing-Dimensions)
* [ProGuide: Editing Tool](../../wiki/ProGuide-Editing-Tool)
* [ProGuide: Sketch Tool With Halo ](../../wiki/ProGuide-Sketch-Tool-With-Halo)
* [ProGuide: Construction Tools with Options](../../wiki/ProGuide-Construction-Tools-with-Options)
* [ProGuide: Annotation Construction Tools](../../wiki/ProGuide-Annotation-Construction-Tools)
* [ProGuide: Annotation Editing Tools](../../wiki/ProGuide-Annotation-Editing-Tools)
* [ProGuide: Knowledge Graph Construction Tools ](../../wiki/ProGuide-Knowledge-Graph-Construction-Tools)
* [ProGuide: Templates](../../wiki/ProGuide-Templates)
* [ProSnippets: Editing](../../wiki/ProSnippets-Editing)

--------------------------

### Geodatabase
* [ProConcepts: Geodatabase](../../wiki/ProConcepts-Geodatabase)
* [ProConcepts: DDL](../../wiki/ProConcepts-DDL)
* [ProSnippets: Geodatabase](../../wiki/ProSnippets-Geodatabase)

&nbsp;&nbsp;&nbsp;&nbsp;**3D Analyst Data**
* [ProConcepts: 3D Analyst Data](../../wiki/ProConcepts-3D-Analyst-Data)
* [ProSnippets: 3D Analyst Data](../../wiki/ProSnippets-3D-Analyst-Data)

&nbsp;&nbsp;&nbsp;&nbsp;**Plugin Datasources**
* [ProGuide: Plugin Datasources](../../wiki/ProGuide-Plugin-Datasources)
* [ProConcepts: Plugin Datasources](../../wiki/ProConcepts-Plugin-Datasources)

&nbsp;&nbsp;&nbsp;&nbsp;**Topology**
* [ProConcepts: Topology](../../wiki/ProConcepts-Topology)
* [ProSnippets: Topology](../../wiki/ProSnippets-Topology)

&nbsp;&nbsp;&nbsp;&nbsp;**Linear Referencing**
* [ProConcepts: Linear Referencing](../../wiki/ProConcepts-Linear-Referencing)
* [ProSnippets: Linear Referencing](../../wiki/ProSnippets-Linear-Referencing)

&nbsp;&nbsp;&nbsp;&nbsp;**Object Model Diagram**

* [Object Model Diagram](http://ArcGIS.github.io/arcgis-pro-sdk/content/OMDs/Geodatabase-Object-Model-Diagram.pdf)

--------------------------

### Geometry
* [ProConcepts: Geometry](../../wiki/ProConcepts-Geometry)
* [ProConcepts: Multipatches](../../wiki/ProConcepts-Multipatches)
* [ProGuide: Building Multipatches](../../wiki/ProGuide-Building-Multipatches)
* [ProSnippets: Geometry](../../wiki/ProSnippets-Geometry)
* [ProSnippets: Geometry Engine](../../wiki/ProSnippets-GeometryEngine)

&nbsp;&nbsp;&nbsp;&nbsp;**Relational Operations**

* [ProGuide: RelationalOperations](../../wiki/ProGuide-Relational-Operations)
* [ProSnippets: Custom Relational Operations](../../wiki/ProGuide-Custom-Relational-Operations)

--------------------------

### Geoprocessing
* [ProConcepts: Geoprocessing](../../wiki/ProConcepts-Geoprocessing)
* [ProGuide: Embedding Toolboxes](../../wiki/ProGuide-content-and-image-resources#embedding-toolboxes)
* [ProSnippets: Geoprocessing](../../wiki/ProSnippets-Geoprocessing)

--------------------------

### Knowledge Graph
* [ProConcepts: Knowledge Graph](../../wiki/ProConcepts-Knowledge-Graph)
* [ProGuide: Knowledge Graph Construction Tools ](../../wiki/ProGuide-Knowledge-Graph-Construction-Tools)
* [ProSnippets: Knowledge Graph](../../wiki/ProSnippets-KnowledgeGraph)

--------------------------

### Layouts
* [ProConcepts: Layouts](../../wiki/ProConcepts-Layouts)
* [ProSnippets: Layouts](../../wiki/ProSnippets-Layouts)

&nbsp;&nbsp;&nbsp;&nbsp;**Reports**
* [ProConcepts: Reports](../../wiki/ProConcepts-Reports)
* [ProSnippets: Reports](../../wiki/ProSnippets-Reports)

&nbsp;&nbsp;&nbsp;&nbsp;**Presentations**
* [ProConcepts: Presentations](../../wiki/ProConcepts-Presentations)
* [ProSnippets: Presentations](../../wiki/ProSnippets-Presentations)

--------------------------

### Map Authoring
* [ProConcepts: Map Authoring](../../wiki/ProConcepts-Map-Authoring)
* [ProConcepts: Annotation](../../wiki/ProConcepts-Annotation)
* [ProConcepts: Dimensions](../../wiki/ProConcepts-Dimensions)
* [ProGuide: Tray buttons](../../wiki/ProGuide-Tray-buttons)
* [ProGuide: Custom Dictionary Style](../../wiki/ProGuide-Custom-Dictionary-Style)
* [ProGuide: Geocoding](../../wiki/ProGuide-Geocoding)
* [ProSnippets: Map Authoring](../../wiki/ProSnippets-MapAuthoring)
* [ProSnippets: Annotation](../../wiki/ProSnippets-Annotation)
* [ProSnippets: Charts](../../wiki/ProSnippets-Charts)
* [ProSnippets: Labeling](../../wiki/ProSnippets-Labeling)
* [ProSnippets: Renderers](../../wiki/ProSnippets-Renderer)
* [ProSnippets: Symbology](../../wiki/ProSnippets-Symbology)
* [ProSnippets: Text Symbols](../../ProSnippets-TextSymbols)

&nbsp;&nbsp;&nbsp;&nbsp;**3D Analyst**
* [ProConcepts: 3D Analyst Layers](../../wiki/ProConcepts-3D-Analyst-Layers)
* [ProSnippets: 3D Analyst Layers](../../wiki/ProSnippets-3D-Analyst-Layers)

&nbsp;&nbsp;&nbsp;&nbsp;**CIM**
* [CIM Resources](../../wiki/CIM-Resources)

&nbsp;&nbsp;&nbsp;&nbsp;**Graphics**
* [ProConcepts: Graphics Layers](../../wiki/ProConcepts-GraphicsLayers)
* [ProSnippets: Graphics Layers](../../wiki/ProSnippets-GraphicsLayers)

&nbsp;&nbsp;&nbsp;&nbsp;**Scene**
* [ProConcepts: Scene Layers](../../wiki/ProConcepts-Scene-Layers)
* [ProSnippets: Scene Layers](../../wiki/ProSnippets-SceneLayers)

&nbsp;&nbsp;&nbsp;&nbsp;**Stream**
* [ProConcepts: Stream Layers](../../wiki/ProConcepts-Stream-Layers)
* [ProSnippets: Stream Layers](../../wiki/ProSnippets-StreamLayers)

&nbsp;&nbsp;&nbsp;&nbsp;**Voxel**
* [ProConcepts: Voxel Layers](../../wiki/ProConcepts-Voxel-Layers)
* [ProSnippets: Voxel Layers](../../wiki/ProSnippets-VoxelLayers)
 
--------------------------

### Map Exploration
* [ProConcept: Map Exploration](../../wiki/ProConcepts-Map-Exploration)
* [ProGuide: Map Pane Impersonation](../../wiki/ProGuide-Map-Pane-Impersonation)
* [ProGuide: TableControl](../../wiki/ProGuide-TableControl)
* [ProSnippets: Map Exploration](../../wiki/ProSnippets-MapExploration)
* [ProSnippets: Custom Pane with Contents](../../wiki/ProSnippets-CustomPaneWithContents)

&nbsp;&nbsp;&nbsp;&nbsp;**Map Tools**<br>

* [ProGuide: Feature Selection](../../wiki/ProGuide-Feature-Selection)
* [ProGuide: Identify](../../wiki/ProGuide-Identify)
* [ProGuide: MapView Interaction](../../wiki/ProGuide-MapView-Interaction)
* [ProGuide: Embeddable Controls](../../wiki/ProGuide-Using-Embeddable-Controls)
* [ProGuide: Custom Popups](../../wiki/ProGuide-Custom-Popups)
* [ProGuide: Dynamic Popup Menu](../../wiki/ProGuide-Dynamic-Popup-Menu)

--------------------------

### Networks
* [ProConcepts: Utility Network](../../wiki/ProConcepts-Utility-Network)
* [ProSnippets: Utility Network](../../wiki/ProSnippets-UtilityNetwork)
* [Object Model Diagram](http://ArcGIS.github.io/arcgis-pro-sdk/content/OMDs/Utility-Network-Object-Model-Diagram.pdf)

&nbsp;&nbsp;&nbsp;&nbsp;**Network Diagrams**

* [ProConcepts: Network Diagrams](../../wiki/ProConcepts-Network-Diagrams)
* [Object Model diagram](http://ArcGIS.github.io/arcgis-pro-sdk/content/OMDs/Network-Diagrams-Object-Model-Diagram.pdf)

--------------------------

### Parcel Fabric
* [ProConcepts: Parcel Fabric](../../wiki/ProConcepts-Parcel-Fabric)
* [ProSnippets: Parcel Fabric](../../wiki/ProSnippets-ParcelFabric)

--------------------------

### Raster
* [ProConcept: Raster](../../wiki/ProConcepts-Raster)
* [ProSnippets: Raster](../../wiki/ProSnippets-Raster)

--------------------------

### Sharing
* [ProConcepts: Portal](../../wiki/ProConcepts-Portal)
* [ProSnippets: Sharing](../../wiki/ProSnippets-sharing)

--------------------------

### Tasks
* [ProConcepts: Tasks](../../wiki/ProConcepts-Tasks)
* [ProSnippets: Tasks](../../wiki/ProSnippets-Tasks)

--------------------------

### Workflow Manager

* [ProSnippets: Workflow Manager](../../wiki/ProSnippets-WorkflowManager)
* [ProConcepts: Workflow Manager Classic (deprecated)](../../wiki/ProConcepts-Workflow-Manager-Classic)

--------------------------

### Reference

* <a href="http://pro.arcgis.com/en/pro-app/sdk/api-reference/" target="_blank">ArcGIS Pro API Reference Guide</a>
* <a href="http://pro.arcgis.com/en/pro-app/sdk/" target="_blank">ArcGIS Pro SDK (pro.arcgis.com)</a>
* <a href="http://github.com/Esri/arcgis-pro-sdk-community-samples" target="_blank">arcgis-pro-sdk-community-samples</a>
* [ArcGISPro Registry Keys](../../wiki/ArcGIS-Pro-Registry-Keys)
* [ArcGIS Pro DAML ID Reference](../../wiki/ArcGIS-Pro-DAML-ID-Reference)
* [ArcGIS Pro Icon Reference](../../wiki/DAML-ID-Reference-Icons)
* [ArcGIS Pro TypeID Reference](../../wiki/ArcGIS-Pro-TypeID-Reference)
* [ProConcepts: Distributing Add-Ins Online](../../wiki/ProConcepts-Distributing-Add-Ins-Online)
* [ProConcepts: Migrating to ArcGIS Pro](../../wiki/ProConcepts-Migrating-to-ArcGIS-Pro)
* [FAQ](../../wiki/FAQ)
* [Dev Summit Tech Sessions](tech-sessions)

## Overview of the ArcGIS Pro SDK

### Requirements

#### ArcGIS Pro

* ArcGIS Pro 3.5

#### Supported platforms

* Windows 11 (Home, Pro, Enterprise)
* Windows 10 (Home, Pro, Enterprise) (64 bit)

#### Supported .NET

* Microsoft .NET Runtime 8.0.0 or better. [Download .NET 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

#### Supported IDEs

* Visual Studio 2022 (v17.8 or higher)  
   * Community Edition  
   * Professional Edition  
   * Enterprise Edition  

Released alongside .NET 8, the [Visual Studio 2022 17.8 release](https://devblogs.microsoft.com/visualstudio/visual-studio-17-8-now-available/) brings support for .NET 8.

#### Third party assemblies
_**Newtonsoft Json**_  
* At 3.5, ArcGIS Pro is using version 13.0.3.27908 of the Newtonsoft Json NuGet. If you require Newtonsoft NuGet in your add-ins it is recommended to use the same version.  

_**WebView2**_  
* Add-in developers can use the new WebViewBrowser control based on Microsoft Edge WebView2. Consult the WebView2 manifest in the Pro bin\WebView folder for the current WebView2 fixed version runtime in use by ArcGIS Pro.

[ArcGIS Pro system requirements](https://pro.arcgis.com/en/pro-app/get-started/arcgis-pro-system-requirements.htm) 

### ArcGIS Pro API

The ArcGIS Pro APIs are _managed .NET assemblies_ which are installed with each ArcGIS Pro installation. Intermediary assemblies containing .NET metadata or PIAs (Primary Interop Assemblies) are not required.

Add any of the ArcGIS Pro managed assemblies that comprise its API as references directly in your Visual Studio add-in projects

A complete list of the ArcGIS Pro assemblies in the public API is provided below. Consult the online [API Reference](https://pro.arcgis.com/en/pro-app/latest/sdk/api-reference) for specific details of each assembly:

#### Core

Core assemblies are located in the {ArcGIS Pro Installation folder}\bin.

Assembly           | Description
------------------------| -------------
ArcGIS.Core.dll        | Provides CIM, Geodatabase, Geometry and Utility Network APIs.
ArcGIS.CoreHost.dll | Provides Host.Initialize to initialize ArcGIS.Core.dll for stand-alone use.
ArcGIS.Desktop.Framework.dll        | Provides the application framework to include add-in contracts, DAML support, and base classes. This assembly **must be referenced** by every add-in.
ESRI.ArcGIS.ItemIndex.dll       | Provides functionality to create and work with Custom items.


#### Extensions

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
ArcGIS.Desktop.KnowledgeGraph.dll        | Provides access to KnowledgeGraphs in maps and link charts.
ArcGIS.Desktop.Layouts.dll        | Provides functionality for manipulating elements on a layout and exporting to a variety of image formats.
ArcGIS.Desktop.Mapping.dll        | Provides types to create maps and layers, label features, perform query operations, and visualize them in 2D or 3D. Provides a raster API to create raster layers and customize raster rendering, and an API to manage styles, style items, and symbols.
ArcGIS.Desktop.TaskAssistant.dll        | Provides the Tasks framework, allowing developers to access, open, close, or export task items.
ArcGIS.Desktop.Workflow.dll       | Provides functionality to create, configure, and execute Workflow Manager Classic jobs and queries. Provides functionality to retrieve configuration information from the Workflow Manager Classic database.
ArcGIS.Desktop.Workflow.Client.dll       | Provides functionality to retrieve job id and connection information for Workflow Manager.


#### Extensions with no public API

There are extension assemblies in {ArcGIS Pro Installation folder}\bin\Extensions subfolders) that do not have a public API. They are currently for Esri internal use only.

* ArcGIS.Desktop.Analyst3D.dll
* ArcGIS.Desktop.Assistant.dll
* ArcGIS.Desktop.Aviation.dll
* ArcGIS.Desktop.Bathymetry.dll
* ArcGIS.Desktop.BusinessAnalyst.dll
* ArcGIS.Desktop.CAD.dll
* ArcGIS.Desktop.Charts.dll
* ArcGIS.Desktop.DataEngineering.dll
* ArcGIS.Desktop.DataSourcesRaster.dll
* ArcGIS.Desktop.Defense.dll
* ArcGIS.Desktop.DefenseMapping.dll
* ArcGIS.Desktop.Editing.PushPull.dll
* ArcGIS.Desktop.FullMotionVideo.dll
* ArcGIS.Desktop.GAWizard.dll
* ArcGIS.Desktop.GeoProcessing.BDC.dll
* ArcGIS.Desktop.GeoProcessing.SAModels.dll
* ArcGIS.Desktop.Geostatistics.dll
* ArcGIS.Desktop.Indoors.dll
* ArcGIS.Desktop.Intelligence.dll
* ArcGIS.Desktop.Intelligence.Common.dll
* ArcGIS.Desktop.LocationReferencing.dll
* ArcGIS.Desktop.Maritime.dll
* ArcGIS.Desktop.Metadata.dll
* ArcGIS.Desktop.MotionImagery.dll
* ArcGIS.Desktop.NeighborhoodExplorer.dll
* ArcGIS.Desktop.NetworkAnalysis.Facility.dll
* ArcGIS.Desktop.NetworkAnalysis.NetworkDiagrams.dll
* ArcGIS.Desktop.NetworkAnalysis.Transportation.dll
* ArcGIS.Desktop.Search.dll
* ArcGIS.Desktop.Sharing.dll
* ArcGIS.Desktop.SpaceTimeCube.dll
* ArcGIS.Desktop.TerritoryDesign.dll

Note: Static string resource properties and image resources included within the public API assemblies are for Esri internal use only. They are not intended for use in third-party add-ins. 

#### ArcGIS Pro Extensions NuGet

The ArcGIS Pro Extensions NuGet contains all the Pro API assemblies needed to compile your Add-ins and Configurations and offers an alternate way to reference the ArcGIS Pro assemblies in your add-in and configuration over traditional file based references. 

To use the ArcGIS Pro Extensions NuGet, set the NuGet Package Management format setting in Visual Studio to be PackageReference.

[ProGuide: ArcGIS Pro Extensions NuGet](https://github.com/Esri/arcgis-pro-sdk/wiki/ProGuide-ArcGIS-Pro-Extensions-NuGet)

## What's New for Developers at 3.5

#### ArcGIS Pro 3.3.3 and 3.4.1 Patches

**AllSource 1.2 and 1.3 users**:
For users installing both the ArcGIS Pro and AllSource patches (whether 3.3.3 and 1.2.1 or 3.4.1 and 1.3.1) you may encounter an issue with the RegisterAddin.exe file association for `.esriAddinX` and `.proConfigX` files being broken. Please refer to this KB article for more information: [Restore RegisterAddin.exe file association for ArcGS Pro SDK Addins and Configurations](https://support.esri.com/en-us/knowledge-base/restore-registeraddin-file-association-for-arcgs-pro-sd-000034714)

**ArcGIS Pro Extensions NuGet Users**:
If you are using ArcGIS Pro Extensions NuGet with ArcGIS Pro 3.3 or 3.4, and you install the ArcGIS Pro 3.3.3 or 3.4.1 patch you may encounter an issue where the addin or configuration is no longer registered to the default well-known folder during compilation. This will affect your ability to debug your addins and configurations. To fix this issue, please upgrade your Esri.ArcGISPro.Extensions30 NuGet to either of Esri.ArcGISPro.Extensions30 3.3.3 or 3.4.1 depending on your ArcGIS Pro release.

#### 1. API enhancements
At 3.5 you can take advantage of API enhancements for: <br/>

**Knowledge Graph:**
* Knowledge Graph Investigation Project Item for integration into the Pro Catalog view.
	   * Open, Add to map, Add to project.
* New knowledge graph document description class for creating new document rows.
* New knowledge graph document provenance class for creating new provenance rows.

**Map Exploration and 3D Analyst:**
* Line of sight enhancements to allow use of multi-patches as obstructions.
* Create elevation profile graphs.
* Add and update map view overlay performance improvements.
* TINEditor class for creating and modifying TINs.

**Content:**
* Open and retrieve recently opened project list to include Portal Projects (uploaded to an enterprise portal).

**Layout:**
* Layout (and map view) printing enhancements.
* Ability to create presentations.

**Map Authoring:**
* Add spatial filters to layer definition queries.
* Generate feature outline geometries from feature layers.

* **Geodatabase:**
* DDL Enhancements for creating dimension feature classes.

A complete list of the API enhancements is provided in the [API Changes section of the API Reference](https://pro.arcgis.com/en/pro-app/latest/sdk/api-reference/topic15120.html).

#### 2. SDK Resources
There are many ProConcepts, ProGuide, ProSnippets, and samples to help you get up and running with the new SDK features. Updates to the SDK Resources include, but are not limited to: 
* [ProConcepts: Presentations](https://github.com/Esri/arcgis-pro-sdk/wiki/ProConcepts-Presentations)
* [ProSnippets: Presentations](https://github.com/Esri/arcgis-pro-sdk/wiki/ProSnippets-Presentations)
* [ProConcepts: Content and Items](https://github.com/Esri/arcgis-pro-sdk/wiki/ProConcepts-Content-and-Items)
* [ProSnippets: Content](https://github.com/Esri/arcgis-pro-sdk/wiki/ProSnippets-content)
* [ProGuide: Command Filter](https://github.com/Esri/arcgis-pro-sdk/wiki/ProGuide-Command-Filter)
* [ProGuide: How to Increment Addin Version Number](https://github.com/Esri/arcgis-pro-sdk/wiki/ProGuide-How-to-Increment-Addin-Version-Number)
* [ProConcepts: Knowledge Graph](https://github.com/Esri/arcgis-pro-sdk/wiki/ProConcepts-Knowledge-Graph)
* [ProSnippets: Knowledge Graph](https://github.com/Esri/arcgis-pro-sdk/wiki/ProSnippets-KnowledgeGraph)
* The [Pro Community Samples](https://github.com/Esri/arcgis-pro-sdk-community-samples) and [Snippets](https://github.com/Esri/arcgis-pro-sdk/wiki/ProSnippets)

## Installing ArcGIS Pro SDK for .NET

ArcGIS Pro SDK for .NET can be downloaded and installed from within Visual Studio. There will be 2 separate extensions you can install: 
* ArcGIS Pro SDK for .NET, 
* ArcGIS Pro SDK for .NET (Utilities) 

**Notes:**
Moving to ArcGIS Pro 3.5 SDK (or 3.0 SDK) from 2.x is not an upgrade. To install,
1. Consult [Migrating from 2.x](https://github.com/Esri/arcgis-pro-sdk/wiki/ProGuide-Installation-and-Upgrade#migrating-from-2x) if you want to install ArcGIS Pro 3.5 SDK and you are on 2.x, **_not_** 3.x.
2. If you are migrating a 2.x add-in to 3.5 you can install the migration tool to help automate the conversion process. Moving from 2.x to 3.x is a **_breaking change_**. Refer to the migration guide [ProConcepts: 3.0 Migration Guide](https://github.com/Esri/arcgis-pro-sdk/wiki/ProConcepts-3.0-Migration-Guide) and  [ProGuide:.NET 8 Upgrade](https://github.com/Esri/arcgis-pro-sdk/wiki/ProGuide-NET-8-Upgrade) for more details.

Read the [ProGuide: Installation and Upgrade](http://github.com/Esri/arcgis-pro-sdk/wiki/ProGuide-Installation-and-Upgrade) for detailed installation instructions.

## Release notes

### ArcGIS Pro SDK for .NET components

The following table summarizes the functionality of each .vsix file included in the SDK download:

| Name|File|Functionality|
| ------------- | ------------- |------------- |
| ArcGIS Pro SDK for .NET | proapp-sdk-templates.vsix | A collection of project and item templates to create ArcGIS Pro add-ins|
| ArcGIS Pro SDK for .NET (Utilities)  | proapp-sdk-utilities.vsix  | A collection of utilities to help create ArcGIS Pro add-ins|
| ArcGIS Pro SDK for .NET (Migration)  | proapp-sdk-migration.vsix  | Migrates ArcGIS Pro SDK 2.x extensions to ArcGIS Pro SDK 3.x for .NET|


#### ArcGIS Pro SDK for .NET templates 
Package: proapp-sdk-templates.vsix  

ArcGIS Pro SDK for .NET provides the following project and item templates:
<table>
<tr><th>C# </th> <th> VB</th><th> Name</th> </tr>
<tr><td><span title="ArcGIS Pro Add-in C#"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProModuleC32.png"></span> </td><td> <span title="ArcGIS Pro Add-in VB"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProModuleVB32.png"></span> </td><td> ArcGIS Pro Module Add-in Project template </td></tr>
<tr><td><span title="ArcGIS Pro Configurations C#"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProConfigurationsC32.png"></span> </td><td> <span title="ArcGIS Pro Configurations VB"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProConfigurationsVB32.png"></span> </td><td> ArcGIS Pro Managed Configurations Project template </td></tr>
<tr><td><span title="ArcGIS Pro Plugin C#"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProPluginC32.png"></span> </td><td> N/A</td><td> ArcGIS Pro Plugin Project template </td></tr>
<tr><td><span title="ArcGIS Pro CoreHost Application"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProCoreHostC32.png"></span> </td><td> N/A</td><td> ArcGIS Pro CoreHost Application Project template </td></tr>
<tr><td><span title="ArcGIS AllSource Add-in"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProModuleC32.png"></span> </td><td> N/A</td><td> ArcGIS AllSource Add-in Project template </td></tr>
<tr><td><span title="ArcGIS Pro Backstage Tab C#"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProBackstageTabC32.png"></span></td><td> <span title="ArcGIS Pro Backstage Tab VB"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProBackstageTabVB32.png"></span> </td><td> ArcGIS Pro Backstage Tab </td></tr>
<tr><td><span title="ArcGIS Pro Button C#"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProButtonC32.png"></span> </td><td> <span title="ArcGIS Pro Button VB"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProButtonVB32.png"></span> </td><td> ArcGIS Pro Button </td></tr>
<tr><td><span title="ArcGIS Pro Button Palette C#"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProButtonPaletteC32.png"></span> </td><td> <span title="ArcGIS Pro Button Palette VB"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProButtonPaletteVB32.png"></span> </td><td> ArcGIS Pro Button Palette</td></tr>
<tr><td><span title="ArcGIS Pro Combo Box C#"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProComboBoxC32.png"></span> </td><td> <span title="ArcGIS Pro Combo Box VB"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProComboBoxVB32.png"></span> </td><td> ArcGIS Pro Combo Box</td></tr>
<tr><td><span title="ArcGIS Pro Construction Tool C#"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProConstructionToolC32.png"></span> </td><td> <span title="ArcGIS Pro Construction Tool VB"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProConstructionToolVB32.png"></span> </td><td> ArcGIS Pro Construction Tool</td></tr>
<tr><td><span title="ArcGIS Pro Custom Control C#"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProCustomControlC32.png"></span> 
</td>
<td> <span title="ArcGIS Pro Custom Control VB"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProCustomControlVB32.png"> </span></td>
<td> ArcGIS Pro Custom Control </td></tr>
<tr><td><span title="ArcGIS Pro Custom Item C#"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProCustomItemC32.png"></span> 
</td>
<td> N/A</td>
<td> ArcGIS Pro Custom Item</td></tr>
<tr><td><span title="ArcGIS Pro Custom Project Item C#"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProCustomProjectItemC32.png"></span> 
</td>
<td> N/A</td>
<td> ArcGIS Pro Custom Project Item</td></tr>
<tr><td><span title="ArcGIS Pro Dockpane C#"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProDockPaneC32.png"></span> </td><td> <span title="ArcGIS Pro Dockpane VB"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProDockPaneVB32.png"></span> </td><td> ArcGIS Pro Dockpane</td></tr>
<tr><td><span title="ArcGIS Pro Dockpane with Burger Button C#"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProDockPaneC32.png"></span> </td><td> <span title="ArcGIS Pro Dockpane with Burger Button VB"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProDockPaneVB32.png"></span> </td><td> ArcGIS Pro Dockpane with Burger Button</td></tr>
<tr><td><span title="ArcGIS Pro Drop Handler C#"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProDropHandlerC32.png"></span> </td><td> <span title="ArcGIS Pro Drop Handler VB"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProDropHandlerVB32.png"></span> </td><td> ArcGIS Pro Drop Handler</td></tr>
<tr><td><span title="ArcGIS Pro Embeddable Control C#"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProEmbeddableControlC32.png"></span> </td><td> <span title="ArcGIS Pro Embeddable Control VB"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProEmbeddableControlVB32.png"></span> </td><td> ArcGIS Pro Embeddable Control</td></tr>
<tr><td><span title="ArcGIS Pro Gallery C#"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProGalleryC32.png"></span> </td><td> <span title="ArcGIS Pro Gallery VB"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProGalleryVB32.png"></span> </td><td> ArcGIS Pro Gallery</td></tr>
<tr><td><span title="ArcGIS Pro Inline-Gallery C#"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProInLineGalleryC32.png"></span> </td><td> <span title="ArcGIS Pro Inline-Gallery VB"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProInLineGalleryVB32.png"></span> </td><td> ArcGIS Pro Inline-Gallery</td></tr>
<tr><td><span title="ArcGIS Pro Layout Tool C#"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProLayoutToolC32.png"></span> </td><td> <span title="ArcGIS Pro Layout Tool">N/A</td><td> ArcGIS Pro Layout Tool</td></tr>
<tr><td><span title="ArcGIS Pro Layout Tray Button C#"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProLayoutTrayC32.png"></span> </td><td> <span title="ArcGIS Pro Layout Tray Button">N/A</td><td> ArcGIS Pro Layout Tray Button</td></tr>
<tr><td><span title="ArcGIS Pro Map Pane Impersonation C#"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProMapPaneC32.png"></span> </td><td> <span title="ArcGIS Pro Map Pane Impersonation VB"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProMapPaneVB32.png"></span> </td><td> ArcGIS Pro Map Pane Impersonation</td></tr>
<tr><td><span title="ArcGIS Pro Map Tool C#"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProMapToolC32.png"></span> </td><td> <span title="ArcGIS Pro Map Tool VB"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProMapToolVB32.png"></span> </td><td> ArcGIS Pro Map Tool</td></tr>
<tr><td><span title="ArcGIS Pro Map Tray Button C#"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProMapTrayC32.png"></span> </td><td> <span title="ArcGIS Pro Table Construction Tool">N/A</td><td> ArcGIS Pro Map Tray Button</td></tr>
<tr><td><span title="ArcGIS Pro Menu C#"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProMenuC32.png"></span> </td><td> <span title="ArcGIS Pro Menu VB"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProMenuVB32.png"></span> </td><td> ArcGIS Pro Menu</td></tr>
<tr><td><span title="ArcGIS Pro Pane C#"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProPaneC32.png"></span> </td><td> <span title="ArcGIS Pro Pane VB"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProPaneVB32.png"></span> </td><td> ArcGIS Pro Pane</td></tr>
<tr><td><span title="ArcGIS Pro Property Sheet C#"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProPropertySheetC32.png"></span> </td><td> <span title="ArcGIS Pro Property Sheet VB"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProPropertySheetVB32.png"></span> </td><td> ArcGIS Pro Property Sheet </td></tr>
<tr><td><span title="ArcGIS Pro ProWindow C#"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProWindowC32.png"></span> </td><td> <span title="ArcGIS Pro ProWindow VB"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProWindowVB32.png"></span> </td><td> ArcGIS Pro ProWindow</td></tr>
<tr><td><span title="ArcGIS Pro Split Button C#"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProSplitButtonC32.png"></span> </td><td> <span title="ArcGIS Pro Split Button VB"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProSplitButtonVB32.png"></span> </td><td> ArcGIS Pro Split Button</td></tr>
<tr><td><span title="ArcGIS Pro Table Construction Tool C#"><img src="https://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProTableConstToolC32.png"></span> </td><td> <span title="ArcGIS Pro Table Construction Tool">N/A</td><td> ArcGIS Pro Table Construction Tool</td></tr>
</table>

In general, there is a one-to-one correspondence between a framework UI element extensibility point (e.g. button, tool, menu, dockpane, etc) and an item template. Use the relevant item template to add the corresponding extensibility point to your Add-in. 

#### ArcGIS Pro SDK for .NET utilities 
Package: proapp-sdk-utilities.vsix  

ArcGIS Pro SDK for .NET (Utilities) provides the following utilities that extend the Visual Studio environment:

![pro-fix-references](https://ArcGIS.github.io/arcgis-pro-sdk/images/Home/proapp-sdk-utilities.png "ArcGIS Pro SDK(Utilities)") 

Name               | Description
----------------------------  | --------------------------------------
Pro Fix References utility | Fixes broken references in an ArcGIS Pro add-in, core host, configuration, or plug-in projects. Broken references can be caused when you share add-ins with other colleagues or download add-ins where the ArcGIS Pro assembly references point to a different location from where you installed them. Pro Fix References can be run on individual projects or all projects within a solution.
Pro Generate DAML Ids utility| Converts all of the ArcGIS Pro Desktop Application Markup Language (DAML) string IDs into static string properties organized by DAML element types (for example, Button, Dockpane, Tool, Condition, and so on). This allows you to use the IntelliSense feature of Visual Studio within your source code file to add IDs, rather than having to manually type DAML string IDs).

#### ArcGIS Pro SDK for .NET Migration 
Package: proapp-sdk-migration.vsix 

![pro-migration](https://ArcGIS.github.io/arcgis-pro-sdk/images/Home/proapp-sdk-migration.png "ArcGIS Pro SDK(Migration)") 

ArcGIS Pro SDK for .NET (Migration) provides support to migrate ArcGIS Pro SDK 2.x extensions to ArcGIS Pro SDK 3.X for .NET. **Note:** When you migrate your add-in, the dekstopVersion attribute in your config.daml will be set to the current version of ArcGIS Pro installed.

### Previous versions
* [ArcGIS Pro 3.5 SDK for .NET](https://github.com/Esri/arcgis-pro-sdk/releases/tag/3.5.0.57365)
* [ArcGIS Pro 3.4 SDK for .NET](https://github.com/Esri/arcgis-pro-sdk/releases/tag/3.4.0.55405)
* [ArcGIS Pro 3.3 SDK for .NET](https://github.com/Esri/arcgis-pro-sdk/releases/tag/3.3.0.52636)
* [ArcGIS Pro 3.2 SDK for .NET](https://github.com/Esri/arcgis-pro-sdk/releases/tag/3.2.0.49743)
* [ArcGIS Pro 3.1 SDK for .NET](https://github.com/Esri/arcgis-pro-sdk/releases/tag/3.1.0.41833)
* [ArcGIS Pro 3.0 SDK for .NET](https://github.com/Esri/arcgis-pro-sdk/releases/tag/3.0.0.36056)
* [ArcGIS Pro 2.9 SDK for .NET](https://github.com/Esri/arcgis-pro-sdk/releases/tag/2.9.0.32739)
* [ArcGIS Pro 2.8 SDK for .NET](https://github.com/Esri/arcgis-pro-sdk/releases/tag/2.8.0.29751)
* [ArcGIS Pro 2.7 SDK for .NET](https://github.com/Esri/arcgis-pro-sdk/releases/tag/2.7.0.26828)
* [ArcGIS Pro 2.6 SDK for .NET](https://github.com/Esri/arcgis-pro-sdk/releases/tag/2.6.0.24783)
* [ArcGIS Pro 2.5 SDK for .NET](https://github.com/Esri/arcgis-pro-sdk/releases/tag/2.5.0.22081)
* [ArcGIS Pro 2.4 SDK for .NET](https://github.com/Esri/arcgis-pro-sdk/releases/tag/2.4.0.19948)
* [ArcGIS Pro 2.3 SDK for .NET](https://github.com/Esri/arcgis-pro-sdk/releases/tag/2.3.0.15769)
* [ArcGIS Pro 2.2 SDK for .NET](https://github.com/Esri/arcgis-pro-sdk/releases/tag/2.2.0.12813)
* [ArcGIS Pro 2.1 SDK for .NET](https://github.com/Esri/arcgis-pro-sdk/releases/tag/2.1.0.10257)
* [ArcGIS Pro 2.0 SDK for .NET](https://github.com/Esri/arcgis-pro-sdk/releases/tag/2.0.0.8933)
* [ArcGIS Pro 1.4 SDK for .NET](https://github.com/Esri/arcgis-pro-sdk/releases/tag/1.4.0.7198)
* [ArcGIS Pro 1.3 SDK for .NET](https://github.com/Esri/arcgis-pro-sdk/releases/tag/1.3.0.5861)
* [ArcGIS Pro 1.2 SDK for .NET](https://github.com/Esri/arcgis-pro-sdk/releases/tag/1.2.0.5023)
* [ArcGIS Pro 1.1 SDK for .NET](https://github.com/Esri/arcgis-pro-sdk/releases/tag/1.1.0.3318)


## Resources

* [API Reference online](https://pro.arcgis.com/en/pro-app/latest/sdk/api-reference)
* [ProSnippets: ready-made snippets of code for your ArcGIS Pro add-ins.](https://github.com/Esri/arcgis-pro-sdk/wiki/ProSnippets).
* <a href="https://pro.arcgis.com/en/pro-app/sdk/" target="_blank">ArcGIS Pro SDK for .NET (pro.arcgis.com)</a>
* [arcgis-pro-sdk-community-samples](http://github.com/Esri/arcgis-pro-sdk-community-samples)
* [ArcGIS Pro DAML ID Reference](http://github.com/Esri/arcgis-pro-sdk/wiki/ArcGIS-Pro-DAML-ID-Reference)
* [FAQ](http://github.com/Esri/arcgis-pro-sdk/wiki/FAQ)
* [ArcGIS Pro SDK icons](https://github.com/Esri/arcgis-pro-sdk/releases/tag/3.5.0.57365)

## Contributing

Esri welcomes contributions from anyone and everyone. For more information, see our [guidelines for contributing](https://github.com/esri/contributing).

## Issues

Find a bug or want to request a new feature? Let us know by submitting an issue.

## Licensing
Copyright 2025 Esri

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


<p align = center><img src="http://esri.github.io/arcgis-pro-sdk/images/ArcGISPro.png"  alt="pre-req" align = "top" height = "20" width = "20" ><b> ArcGIS Pro 3.5 SDK for Microsoft .NET Framework</b></p>

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[Home](https://github.com/Esri/arcgis-pro-sdk/wiki) | <a href="http://pro.arcgis.com/en/pro-app/sdk/api-reference/index.html" target="_blank">API Reference</a> | [Requirements](#requirements) | [Download](#installing-arcgis-pro-sdk-for-net) |  <a href="http://github.com/esri/arcgis-pro-sdk-community-samples" target="_blank">Samples</a>



