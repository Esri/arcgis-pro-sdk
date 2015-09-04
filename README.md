## ArcGIS Pro 1.1 SDK for .NET

Extend ArcGIS Pro with ArcGIS Pro SDK for .NET. ArcGIS Pro SDK for .NET is based on the add-in extensibility pattern (first introduced at 10.0). Leverage modern .NET features and patterns such as Task Asynchronous Programming (TAP), LINQ, WPF Binding, and MVVM to write integrated 2D/3D add-ins using Pro’s new APIs.

<a href="http://pro.arcgis.com/en/pro-app/sdk/" target="_blank">View it live</a>

###Table of Contents

####Developing with ArcGIS Pro

* [Requirements](#requirements)
* [Download](#download)
* [Installing ArcGIS Pro SDK for .NET](#installing-arcgis-pro-sdk-for-net)
 * [Upgrading from ArcGIS Pro 1.1 SDK for .NET (Beta)](#upgrading-from-arcgis-pro-11-sdk-for-net-beta)
 * [Install from within Visual Studio](#install-from-within-visual-studio)
 * [Install from My Esri download](#install-from-my-esri-download)
   * [VSIX Installation (Option 1)](#vsix-installation-option-1)
    * [MSI  Installation (Option 2)](#msi-installation-option-2)
* [Uninstalling ArcGIS Pro SDK for .NET](#uninstalling-arcgis-pro-sdk-for-net)
* [ArcGIS Pro SDK for .NET components](#arcgis-pro-sdk-for-net-components)
  * [ArcGIS Pro SDK for .NET templates](#arcgis-pro-sdk-for-net-templates)
  * [ArcGIS Pro SDK for .NET utilities](#arcgis-pro-sdk-for-net-utilities)
* [Getting started](#getting-started) 
* [ArcGIS Pro API](#arcgis-pro-api)
  * [Core](#core)
  * [Extensions](#extensions)
  * [Extensions with no public API](#extensions-with-no-public-api)
* [Release notes](#release-notes)
 * [ArcGIS Pro 1.1 SDK for .NET](#arcgis-pro-11-sdk-for-net-1)
* [Resources](#resources)

####Add-in fundamentals 
 * [Pro Guide: Your first add-in](../../wiki/ProGuide-Build-Your-First-Add-in)
 * [ProConcept: Localization](../../wiki/ProConcept-Localization)
 * [ProGuide: Digitally signed add-ins](../../wiki/ProGuide-Digitally-signed-add-ins)
 * [ProGuide: Content and image resources](../../wiki/ProGuide-content-and-image-resources)

####Framework

* [ProConcepts: Framework](../../wiki/ProConcepts-Framework)
* [ProSnippets: Framework](../../wiki/ProSnippets-Framework)

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

--------------------------

###Geodatabase

* [ProConcepts: Geodatabase](../../wiki/ProConcepts-Geodatabase)
* [ProSnippets: Geodatabase](../../wiki/ProSnippets-Geodatabase)

--------------------------

###Geometry

* [ProConcepts: Geometry](../../wiki/ProConcepts-Geometry)
* [ProSnippets: Geometry](../../wiki/ProSnippets-Geometry)

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

* ArcGIS Pro 1.1

####Supported platforms

* Windows 8.1 Basic, Professional, and Enterprise (64 bit [EM64T]) 
* Windows 8 Basic, Professional, and Enterprise (64 bit [EM64T]) 
* Windows 7 SP1 Ultimate, Enterprise, Professional, and Home Premium (64 bit [EM64T]) 

####Supported .NET framework

* 4.5.2 
* 4.5.1 
* 4.5 

####Supported IDEs

* Visual Studio 2015 (Professional, Enterprise, and Community Editions)
* Visual Studio 2013 (Professional, Premium, Ultimate, and Community Editions) 

##Download

ArcGIS Pro SDK for .NET can be downloaded and installed using either one of the following options:

* Download and install from within Visual Studio
* Download from MyEsri.com (Visual Studio 2013 only)

Read the [Installing ArcGIS Pro SDK for .NET](#installing-arcgis-pro-sdk-for-net) section below for more information.

##Installing ArcGIS Pro SDK for .NET

ArcGIS Pro SDK for .NET templates and utilities are available as Visual Studio extensions (.vsix files) or as a Windows Installer (.msi file) program. You do not need administrative access or elevated user permissions to install ArcGIS Pro SDK for .NET. It is recommended that you uninstall the Beta version of ArcGIS Pro SDK for .NET from within Visual Studio before installing ArcGIS Pro 1.1 SDK for .NET. See [FAQ](http://github.com/Esri/arcgis-pro-sdk/wiki/FAQ#installation) for instructions. 

Choose either **one** of the following installation mechanisms:  
* [Install from within Visual Studio](#install-from-within-visual-studio)
* [Install from My Esri download](#install-from-my-esri-download)

###Upgrading from ArcGIS Pro 1.1 SDK for .NET Beta

It is recommended that you uninstall the Beta version of ArcGIS Pro SDK for .NET from within Visual Studio before installing ArcGIS Pro 1.1 SDK for .NET. See [FAQ](http://github.com/Esri/arcgis-pro-sdk/wiki/FAQ#installation) for instructions.

### Install from within Visual Studio

To install ArcGIS Pro SDK for .NET from within Visual Studio, complete the following steps:

1. Open Visual Studio.
2. From the **Tools** menu, choose **Extensions and Updates**. 

 ![VS_StartPage1.png](http://ArcGIS.github.io/arcgis-pro-sdk/images/Installation-Instructions/VS_StartPage1.png "Extension and Updates")

3. Expand the **Online** folder on the left and choose **Visual Studio Gallery**.

 ![Online_Option1.png](http://ArcGIS.github.io/arcgis-pro-sdk/images/Installation-Instructions/Online_Option1.png "Visual Studio Gallery")  

4. Type **ArcGIS Pro SDK** in the search box in the upper right corner of the **Extensions and Updates** dialog box.  Among the search results found, the items you will need are ArcGIS Pro SDK for .NET and ArcGIS Pro SDK for .NET (Utilities) as shown in the following screen shot:  

 ![Search1.png](http://ArcGIS.github.io/arcgis-pro-sdk/images/Installation-Instructions/Search1.png "ArcGIS Pro SDK Search")  

5. Choose the ArcGIS Pro SDK for .NET package and click the **Download** button. 

 ![Download_Templates1.png](http://ArcGIS.github.io/arcgis-pro-sdk/images/Installation-Instructions/Download_Templates1.png "ArcGIS Pro SDK Download")  

6. The **Download and Install** dialog box for ArcGIS Pro SDK for .NET will be displayed.  If you're satisfied with the license agreement, click the **Install** button to complete the installation. 

 ![SDK_Install.png](http://ArcGIS.github.io/arcgis-pro-sdk/images/Installation-Instructions/SDK_Install.png "ArcGIS Pro SDK Install")  

7. Choose the ArcGIS Pro SDK for .NET (Utilities) package and click the **Download** button. Install the ArcGIS Pro SDK for .NET (Utilities) package in the same manner.  

8. Once the installations of the ArcGIS Pro SDK for .NET and ArcGIS Pro SDK for .NET (Utilities) packages are complete, you'll see a green check mark next to them in the **Extensions and Updates** dialog box.  </li>

 ![Green_Check1.png](http://ArcGIS.github.io/arcgis-pro-sdk/images/Installation-Instructions/Green_Check1.png "ArcGIS Pro SDK Green Check") 

9. Restart Visual Studio by clicking **Restart Now** at the lower right corner of the **Extensions and Updates** dialog box. 

 ![VS_Restart1.png](http://ArcGIS.github.io/arcgis-pro-sdk/images/Installation-Instructions/VS_Restart1.png "ArcGIS Pro SDK VS Restart") 

10. Verify that the ArcGIS Pro SDK for .NET templates and utilities are installed in Visual Studio. You should see an ArcGIS folder within the set of New Project templates for Visual C# and Visual Basic.
 ![ArcGIS_Pro_Addins_CS1.png](http://ArcGIS.github.io/arcgis-pro-sdk/images/Installation-Instructions/ArcGIS_Pro_Addins_CS1.png "ArcGIS Pro SDK templates")  

Note: After installation, the **Extensions and Updates** dialog box can be used to enable, disable, update, or uninstall the ArcGIS Pro SDK for .NET packages.

See [ProGuide: Build your first add-in](https://github.com/Esri/arcgis-pro-sdk/wiki/ProGuide-Build-Your-First-Add-in) to learn how to create add-ins to customize and extend ArcGIS Pro.   

###Install from My Esri download

Note: Visual Studio 2013 only.

You can also download ArcGIS Pro SDK for .NET from My Esri (http://my.esri.com). The content of the ArcGISProSDK.msi file is identical to the content of the proapp-sdk-templates.vsix and proapp-sdk-utilities.vsix files. Install using either the VSIX method or the msi **but not both.** 

| Install method (Pick One)|Use these files|
| :------------- | :------------- |
| VSIX| proapp-sdk-templates.vsix *, proapp-sdk-utilities.vsix * | 
|MSI    |ArcGISProSDK.msi*, ArcGIS ProSDK.cab| 

*Double click these files to install.


####VSIX installation Option 1

To download the two .vsix files (proapp-sdk-templates.vsix and proapp-sdk-utilities.vsix), complete the following steps:

1. Double-click proapp-sdk-templates.vsix.

2. The **VSIX Installer** dialog box for ArcGIS Pro SDK for .NET opens. If you're satisfied with the license agreement, click the **Install** button to complete the installation.

 ![VSIX-1.png](http://ArcGIS.github.io/arcgis-pro-sdk/images/Installation-Instructions/VSIX-1.png "ArcGIS Pro SDK for .NET install")  

3. Double-click proapp-sdk-utilities.vsix.

4. The **VSIX Installer** dialog box for ArcGIS Pro SDK for .NET (Utilities) opens. If you're satisfied with the license agreement, click the **Install** button to complete the installation. 

5. If Visual Studio was open, close and restart it to refresh its templates.

6. Verify that the ArcGIS Pro SDK for .NET templates and utilities are installed in Visual Studio. You should see an ArcGIS folder within the set of New Project templates for Visual C# and Visual Basic.

![ArcGIS_Pro_Addins_CS1.png](http://ArcGIS.github.io/arcgis-pro-sdk/images/Installation-Instructions/ArcGIS_Pro_Addins_CS1.png "ArcGIS Pro SDK templates")  

Note: After installation, the **Extensions and Updates** dialog box can be used to enable, disable, update, or uninstall the ArcGIS Pro SDK for .NET packages.

See [ProGuide: Build your first add-in](https://github.com/Esri/arcgis-pro-sdk/wiki/ProGuide-Build-Your-First-Add-in) to learn how to create add-ins to customize and extend ArcGIS Pro. 

####MSI installation Option 2

An .msi file has been provided for those who prefer the .msi mechanism or their organization policies require it. To download the .msi file, complete the following steps:

1. Double-click ArcGISProSDK.msi.  </li>
 
2. Follow the instructions displayed on the dialog box to complete the ArcGIS Pro SDK for .NET installation.  

3. If Visual Studio was open, close and restart it to refresh its templates.</li>

4. Verify that the ArcGIS Pro SDK for .NET templates and utilities are installed in Visual Studio. You should see an ArcGIS folder within the set of New Project templates for Visual C# and Visual Basic.

 ![ArcGIS_Pro_Addins_CS1.png](http://ArcGIS.github.io/arcgis-pro-sdk/images/Installation-Instructions/ArcGIS_Pro_Addins_CS1.png "ArcGIS Pro SDK templates")  

Note: After installation, use the Control Panel to modify or uninstall the ArcGIS Pro SDK for .NET package.

See [ProGuide: Build your first add-in](https://github.com/Esri/arcgis-pro-sdk/wiki/ProGuide-Build-Your-First-Add-in) to learn how to create add-ins to customize and extend ArcGIS Pro. 

## Uninstalling ArcGIS Pro SDK for .NET

###Uninstall VSIX from within Visual Studio
If you have installed ArcGIS Pro SDK for .NET using the vsix packages from "MyEsri" or from Visual Studio Gallery, you can uninstall it from within Visual Studio following the steps below:  

1. Open Visual Studio.  
2. From the **Tools** menu choose **Extensions and Updates** item.  
3. Expand the **Installed** folder on the left and select **All**.   
4. Find the ArcGIS Pro SDK for .NET and ArcGIS Pro SDK for .NET (Utilities) packages and click the Uninstall button.  
 ![vsix-uninstall.png](http://ArcGIS.github.io/arcgis-pro-sdk/images/FAQ/vsix-uninstall.png "Uninstall ArcGIS Pro SDK")  

###Uninstall msi from Control Panel
If you have installed ArcGIS Pro SDK for .NET using the ArcGISProSDK.msi from "MyEsri" you can uninstall it from the Windows Control Panel following the steps below:  
1. Open the Windows Control Panel.  
2. Access the Programs and Features pane from the Control panel.  
3. Select the ArcGIS Pro SDK for Microsoft .NET Framework entry and click Uninstall\Change. Follow the instructions to uninstall the program.  

 ![msi-control-panel.png](http://ArcGIS.github.io/arcgis-pro-sdk/images/FAQ/msi-control-panel.png "Uninstall ArcGIS Pro SDK")  

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
![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProModuleC32.png "ArcGIS Pro Module C#") | ![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProModuleVB32.png "ArcGIS Pro Module VB") | ArcGIS Pro Module Add-in   
![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProButtonC32.png "ArcGIS Pro Button C#") | ![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProButtonVB32.png "ArcGIS Pro Button VB") | ArcGIS Pro Button 
![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProButtonPaletteC32.png "ArcGIS Pro Button Palette C#") | ![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProButtonPaletteVB32.png "ArcGIS Pro Button Palette VB") | ArcGIS Pro Button Palette
![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProComboBoxC32.png "ArcGIS Pro Combo Box C#") | ![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProComboBoxVB32.png "ArcGIS Pro Combo Box VB") | ArcGIS Pro Combo Box
![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProConstructionToolC32.png "ArcGIS Pro Construction Tool C#") | ![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProConstructionToolVB32.png "ArcGIS Pro Combo Box VB") | ArcGIS Pro Construction Tool
![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProDockPaneC32.png "ArcGIS Pro Dockpane C#") | ![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProDockPaneVB32.png "ArcGIS Pro Dockpane VB") | ArcGIS Pro Dockpane
![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProDockPaneC32.png "ArcGIS Pro Dockpane with Burger Button C#") | ![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProDockPaneVB32.png "ArcGIS Pro Dockpane with Burger Button VB") | ArcGIS Pro Dockpane with Burger Button
![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProDropHandlerC32.png "ArcGIS Pro Drop Handler C#") | ![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProDropHandlerVB32.png "ArcGIS Pro Drop Handler VB") | ArcGIS Pro Drop Handler
![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProGalleryC32.png "ArcGIS Pro Gallery C#") | ![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProGalleryVB32.png "ArcGIS Pro Gallery VB") | ArcGIS Pro Gallery
![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProInLineGalleryC32.png "ArcGIS Pro Inline-Gallery C#") | ![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProInLineGalleryVB32.png "ArcGIS Pro Inline-Gallery VB") | ArcGIS Pro Inline-Gallery
![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProMapToolC32.png "ArcGIS Pro Map Tool C#") | ![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProMapToolVB32.png "ArcGIS Pro Map Tool VB") | ArcGIS Pro Map Tool
![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProMenuC32.png "ArcGIS Pro Menu C#") | ![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProMenuVB32.png "ArcGIS Pro Menu VB") | ArcGIS Pro Menu
![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProPaneC32.png "ArcGIS Pro Pane C#") | ![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProPaneVB32.png "ArcGIS Pro Pane VB") | ArcGIS Pro Pane
![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProSketchToolC32.png "ArcGIS Pro Sketch Tool C#") | ![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProSketchToolVB32.png "ArcGIS Pro Sketch Tool VB") | ArcGIS Pro Sketch Tool
![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProSplitButtonC32.png "ArcGIS Pro Split Button C#") | ![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProSplitButtonVB32.png "ArcGIS Pro Split Button VB") | ArcGIS Pro Split Button
![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProToolC32.png "ArcGIS Pro Tool C#") | ![](http://ArcGIS.github.io/arcgis-pro-sdk/images/VisualStudioTemplates/ArcGISProToolVB32.png "ArcGIS Pro Tool VB") | ArcGIS Pro Tool


####ArcGIS Pro SDK for .NET utilities 
Package: proapp-sdk-utilities.vsix  

ArcGIS Pro SDK for .NET (Utilities) provides the following utilities that extend the Visual Studio environment:

![pro-fix-references](http://ArcGIS.github.io/arcgis-pro-sdk/images/Home/proapp-sdk-utilities.png "ArcGIS Pro SDK(Utilities)") 

Name               | Description
----------------------------  | --------------------------------------
Pro Fix References utility | Fixes broken references in an ArcGIS Pro add-in. Broken references can be caused when you share add-ins with other colleagues or download add-ins where the ArcGIS Pro assembly references point to a different location from where you installed them.
Pro Generate DAML Ids utility| Converts all of the ArcGIS Pro Desktop Application Markup Language (DAML) string IDs into static string properties organized by DAML element types (for example, Button, Dockpane, Tool, Condition, and so on). This allows you to use the IntelliSense feature of Visual Studio within your source code file to add IDs, rather than having to manually type DAML string IDs).

## Getting started
See [ProGuide: Build your first add-in](https://github.com/Esri/arcgis-pro-sdk/wiki/ProGuide-Build-Your-First-Add-in) for step-by-step instructions on creating a basic button that appears on the ArcGIS Pro ribbon.

##ArcGIS Pro API

The ArcGIS Pro APIs are managed .NET assemblies. Intermediary assemblies containing .NET metadata or PIAs (Primary Interop Assemblies) are not required.

Add any of the ArcGIS Pro managed assemblies that comprise its API as references directly in your Visual Studio add-in projects

![pro-references.png](http://ArcGIS.github.io/arcgis-pro-sdk/images/Home/pro-references.png "ArcGIS Pro API References") 

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
* ArcGIS.Desktop.DataSourcesRaster.dll
* ArcGIS.Desktop.Geostatistics.dll
* ArcGIS.Desktop.NetworkAnalysis.Facility.dll
* ArcGIS.Desktop.NetworkAnalysis.NetworkDiagrams.dll
* ArcGIS.Desktop.NetworkAnalysis.Transportation.dll
* ArcGIS.Desktop.Search.dll
* ArcGIS.Desktop.Sharing.dll

Note: Static string resource properties and image resources included within the public API assemblies are for Esri internal use only. They are not intended for use in third-party add-ins. 

##Release notes 

###ArcGIS Pro 1.1 SDK for .NET

These release notes describe details of the ArcGIS Pro 1.1 SDK for .NET release. Here you will find information about functionality available in the release and known issues and limitations.

####What's new

Support for Visual Studio 2015 is now available for download from within Visual Studio.

####Known issues

The following are known issues or limitations with the ArcGIS Pro 1.1 SDK for .NET release. 

#####1. API Reference guide TOC Display issues on some desktops
* When you browse to the [API Reference guide](http://pro.arcgis.com/en/pro-app/sdk/api-reference/) on some desktop machines, the TOC appears as in the following screen shot:  
![TOC-issue.png](http://ArcGIS.github.io/arcgis-pro-sdk/images/Home/TOC-issue.png "ArcGIS Pro API Reference guide TOC")  
* The TOC layout has adapted to a touch device. In that mode, the TOC, search, index, etc., are available from the icons on the toolbar at the top of the page. This occurs mainly in the Chrome browser, which sometimes identifies a device as touch capable if you have a touchscreen laptop or are running on a virtual machine. 

**Workaround**  
Use [this URL](http://pro.arcgis.com/en/pro-app/sdk/api-reference/webframedesktop.html) to view the API Reference guide when you notice this issue.  

#####2. Add-in is not loaded by Pro when you "build" it in Visual Studio
* You have deleted the add-in file (*.esriAddinX file) from the `C:\Users\<UserName>\Documents\ArcGIS\AddIns\ArcGISPro` folder.
* Without making any code changes, you build the add-in project in Visual Studio and launch ArcGIS Pro or click the Start button in Visual Studio to launch the debugger.
* Your add-in does not load in ArcGIS Pro.

**Workaround**
* From Visual Studio's Build menu, click the Rebuild Solution menu item. This will create the add-in file (*.esriAddinX file) under the `C:\Users\<UserName>\Documents\ArcGIS\AddIns\ArcGISPro` folder. When you launch ArcGIS Pro, your add-in will now load.

#####3. Controls do not work in ArcGIS Pro after the add-in project's namespace and/or assembly is changed  

* You changed one or more of the following:
    * The Assembly name and/or default namespace in your project application properties in Visual Studio
    * The namespace in your add-in module and/or add-in class files.

When ArcGIS Pro loads your add-in, one or more of the controls defined in your add-in do not work. For example, a new button in your add-in is unresponsive when you click it and becomes permanently disabled.

**Fix**  
One or more of the following conditions may need to be fixed:  
* The **defaultAssembly** and **defaultNamespace** attributes on the root ```ArcGIS``` daml element within your Config.daml must be changed to match any changes you made to the corresponding Visual Studio project application properties.

```xml

<?xml version="1.0" encoding="utf-8"?>
<ArcGIS defaultAssembly="MyRenamedAssembly.dll" defaultNamespace="MyRenamedAssembly" xmlns="..." xmlns:xsi="..." xsi:schemaLocation="...">
  <AddInInfo id="...
....

```

* If the namespace of an add-in class (in the code file) does not match the ```defaultNamespace``` attribute of the ```<ArcGIS>``` element in the Config.daml, you must fully qualify the ```className``` attribute of its daml element (with "namespace.classname") in Config.daml.

For example: Assume this is the class file of a button. Note the ```namespace```.
```C#

namespace MyRenamedAssembly.Addins {
     class Button1 : Button
    {
        protected override void OnClick() {
```

Assume this is the Config.daml. Note the ```<ArcGIS defaultNamespace```.
```xml

<!-- the defaultNamespace is MyRenamedAssembly -->
<ArcGIS defaultAssembly="MyRenamedAssembly.dll" defaultNamespace="MyRenamedAssembly" xmlns="...

    <!-- the button className attribute is fully qualified -->
   <button .... className="MyRenamedAssembly.Addins.Button1" ... />
```

* Rebuild the add-in project

#####4.  In a Visual Basic add-in, adding two items with the same name (for example, button.vb) in two different project folders causes a conflict in namespace compile error.

Visual Basic does not automatically create the .vb file pre-populated with the namespace. To fix it, open the button.vb in the folder, and enclose the Button class with the namespace. See the following example:

```vb
Namespace ProAppModule9.Test
    Friend Class Button
        Inherits Button

        Protected Overrides Sub OnClick()

        End Sub
    End Class
End Namespace
```

#####5. Custom Project Settings - ProjectService::SaveProjectAsync Failed

If the ArcGIS.Desktop.Framework.Contracts.Module overrides for reading and writing custom Add-in project settings are used in an Add-in, the project file can no longer be saved. A message box will be displayed with the error message: ProjectService::SaveProjectAsync Failed.

![ProjectSettingsError](http://ArcGIS.github.io/arcgis-pro-sdk/images/Home/ProjectSettingsError.png "Custom Project Settings error")

This will be fixed in 1.1.1

The overrides in question on the Module class are:
```c#
protected override Task OnReadSettingsAsync(ModuleSettingsReader settings) {
}

protected override Task OnWriteSettingsAsync(ModuleSettingsWriter settings) {
}

```
##Resources

* [API Reference online](http://pro.arcgis.com/en/pro-app/sdk/api-reference)
* <a href="http://pro.arcgis.com/en/pro-app/sdk/" target="_blank">ArcGIS Pro SDK for .NET (pro.arcgis.com)</a>
* [arcgis-pro-sdk-community-samples](http://github.com/Esri/arcgis-pro-sdk-community-samples)
* [FAQ](http://github.com/Esri/arcgis-pro-sdk/wiki/FAQ)
* [ArcGIS Pro SDK icons](https://github.com/Esri/arcgis-pro-sdk/releases/tag/1.1.0.3308)

![ArcGIS Pro SDK for .NET Icons](http://ArcGIS.github.io/arcgis-pro-sdk/images/Home/Image-of-icons.png  "ArcGIS Pro SDK Icons")

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
<b> ArcGIS Pro 1.1 SDK for Microsoft .NET Framework</b>
</p>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[Home](https://github.com/Esri/arcgis-pro-sdk/wiki) | <a href="http://pro.arcgis.com/en/pro-app/beta/sdk" target="_blank">ArcGIS Pro SDK</a> | <a href="http://pro.arcgis.com/en/pro-app/beta/sdk/api-reference/index.html" target="_blank">API Reference</a> | [Requirements](#requirements) | [Download](#download) |  <a href="http://github.com/esri/arcgis-pro-sdk-community-samples" target="_blank">Samples</a>



