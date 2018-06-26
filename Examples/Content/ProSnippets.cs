/*

   Copyright 2018 Esri

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.

   See the License for the specific language governing permissions and
   limitations under the License.

*/
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System;
using ArcGIS.Desktop.Catalog;
using System.Linq;
using System.Collections.Generic;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Layouts;
using ArcGIS.Desktop.GeoProcessing;
using System.IO;
using ArcGIS.Core.CIM;

namespace Content.Snippets
{

  internal class ProSinppets1
  {
    public async void ContentSnippet()
    {
      #region New project
      //Create an empty project. The project will be created in the default folder
      //It will be named MyProject1, MyProject2, or similar...
      await Project.CreateAsync();
      #endregion

      #region New project with specified name
      //Settings used to create a new project
      CreateProjectSettings projectSettings = new CreateProjectSettings()
      {
        //Sets the name of the project that will be created
        Name = @"C:\Data\MyProject1\MyProject1.aprx"
      };
      //Create the new project
      await Project.CreateAsync(projectSettings);
      #endregion

    }

    public async void ContentSnippets2()
    {

      #region New project using a particular template
      //Settings used to create a new project
      CreateProjectSettings projectSettings = new CreateProjectSettings()
      {
        //Sets the project template that will be used to create the new project
        TemplatePath = @"C:\Data\MyProject1\CustomTemplate.aptx"
      };
      //Create the new project
      await Project.CreateAsync(projectSettings);
      #endregion

      #region Open project
      //Opens an existing project or project package
      await Project.OpenAsync(@"C:\Data\MyProject1\MyProject1.aprx");
      #endregion

      #region Current project
      //Gets the current project
      var project = Project.Current;
      #endregion

      #region Get location of current project
      //Gets the location of the current project; that is, the path to the current project file (*.aprx)  
      string projectPath = Project.Current.URI;
      #endregion

      #region Get the project's default gdb path
      var projGDBPath = Project.Current.DefaultGeodatabasePath;
      #endregion

      #region Save project
      //Saves the project
      await Project.Current.SaveAsync();
      
      #endregion

      #region SaveAs project
      //Saves a copy of the current project file (*.aprx) to the specified location with the specified file name, 
      //then opens the new project file
      await Project.Current.SaveAsAsync(@"C:\Data\MyProject1\MyNewProject1.aprx");
      #endregion

      #region Close project
      //A project cannot be closed using the ArcGIS Pro API. 
      //A project is only closed when another project is opened, a new one is created, or the application is shutdown.
      #endregion

      #region Adds item to the current project
      //Adding a folder connection
      string folderPath = "@C:\\myDataFolder";
      var folder = await QueuedTask.Run(() => {
        //Create the folder connection project item
        var item = ItemFactory.Instance.Create(folderPath) as IProjectItem;
        //If it is succesfully added to the project, return it otherwise null
        return Project.Current.AddItem(item) ? item as FolderConnectionProjectItem : null;
      });

      //Adding a Geodatabase:
      string gdbPath = "@C:\\myDataFolder\\myData.gdb";
      var newlyAddedGDB = await QueuedTask.Run(() => {
        //Create the File GDB project item
        var item = ItemFactory.Instance.Create(gdbPath) as IProjectItem;
        //If it is succesfully added to the project, return it otherwise null
        return Project.Current.AddItem(item) ? item as GDBProjectItem : null;
      });

      #endregion

      #region How to add a new map to a project
        await QueuedTask.Run(() =>
        {
          //Note: see also MapFactory in ArcGIS.Desktop.Mapping
          var map = MapFactory.Instance.CreateMap("New Map", MapType.Map, MapViewingMode.Map, Basemap.Oceans);
          ProApp.Panes.CreateMapPaneAsync(map);
        });

      #endregion
      
      #region Check if project needs to be saved
      //The project's dirty state indicates changes made to the project have not yet been saved. 
      bool isProjectDirty = Project.Current.IsDirty;
      #endregion

      #region Get all the project items
      IEnumerable<Item> allProjectItems = Project.Current.GetItems<Item>();
      foreach (var pi in allProjectItems)
      {
        //Do Something 
      }
      #endregion

      #region Gets all the "MapProjectItems"
      IEnumerable<MapProjectItem> newMapItemsContainer = project.GetItems<MapProjectItem>();

      await QueuedTask.Run(() =>
      {
        foreach (var mp in newMapItemsContainer)
        {
          //Do Something with the map. For Example:
          Map myMap = mp.GetMap();
        }
      });
      #endregion

      #region Gets a specific "MapProjectItem"
      MapProjectItem mapProjItem = Project.Current.GetItems<MapProjectItem>().FirstOrDefault(item => item.Name.Equals("EuropeMap"));
      #endregion

      #region Gets all the "StyleProjectItems"
      IEnumerable<StyleProjectItem> newStyleItemsContainer = null;
      newStyleItemsContainer = Project.Current.GetItems<StyleProjectItem>();
      foreach (var styleItem in newStyleItemsContainer)
      {
        //Do Something with the style.
      }
      #endregion

      #region Gets a specific "StyleProjectItem"
      var container = Project.Current.GetItems<StyleProjectItem>();
      StyleProjectItem testStyle = container.FirstOrDefault(style => (style.Name == "ArcGIS 3D"));
      StyleItem cone = null;
      if (testStyle != null)
        cone = testStyle.LookupItem(StyleItemType.PointSymbol, "Cone_Volume_3");
      #endregion

      #region Gets all the "GDBProjectItems"
      IEnumerable<GDBProjectItem> newGDBItemsContainer = null;
      newGDBItemsContainer = Project.Current.GetItems<GDBProjectItem>();
      foreach (var GDBItem in newGDBItemsContainer)
      {
        //Do Something with the GDB.
      }
      #endregion

      #region Gets a specific "GDBProjectItem"
      GDBProjectItem GDBProjItem = Project.Current.GetItems<GDBProjectItem>().FirstOrDefault(item => item.Name.Equals("myGDB"));
      #endregion

      #region Gets all the "ServerConnectionProjectItem"
      IEnumerable<ServerConnectionProjectItem> newServerConnections = null;
      newServerConnections = project.GetItems<ServerConnectionProjectItem>();
      foreach (var serverItem in newServerConnections)
      {
        //Do Something with the server connection.
      }
      #endregion

      #region Gets a specific "ServerConnectionProjectItem"
      ServerConnectionProjectItem serverProjItem = Project.Current.GetItems<ServerConnectionProjectItem>().FirstOrDefault(item => item.Name.Equals("myServer"));
      #endregion

      #region Gets all folder connections in a project
      //Gets all the folder connections in the current project
      var projectFolders = Project.Current.GetItems<FolderConnectionProjectItem>();
      foreach (var FolderItem in projectFolders)
      {
        //Do Something with the Folder connection.
      }
      #endregion

      #region Gets a specific folder connection
      //Gets a specific folder connection in the current project
      FolderConnectionProjectItem myProjectFolder = Project.Current.GetItems<FolderConnectionProjectItem>().FirstOrDefault(folderPI => folderPI.Name.Equals("myDataFolder"));
      #endregion

      #region Remove a specific folder connection
      // Remove a folder connection from a project; the folder stored on the local disk or the network is not deleted
      FolderConnectionProjectItem folderToRemove = Project.Current.GetItems<FolderConnectionProjectItem>().FirstOrDefault(myfolder => myfolder.Name.Equals("PlantSpecies"));
      if (folderToRemove != null)
        Project.Current.RemoveItem(folderToRemove as IProjectItem);
      #endregion
      
      #region Gets a specific "LayoutProjectItem"
      LayoutProjectItem layoutProjItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals("myLayout"));
      #endregion

      #region Gets all layouts in a project:
      //Gets all the layouts in the current project
      var projectLayouts = Project.Current.GetItems<LayoutProjectItem>();
      foreach (var layoutItem in projectLayouts)
      {
        //Do Something with the layout
      }
      #endregion

      #region Gets a specific "GeoprocessingProjectItem"
      GeoprocessingProjectItem GPProjItem = Project.Current.GetItems<GeoprocessingProjectItem>().FirstOrDefault(item => item.Name.Equals("myToolbox"));
      #endregion

      #region Gets all GeoprocessingProjectItems in a project:
      //Gets all the GeoprocessingProjectItem in the current project
      var GPItems = Project.Current.GetItems<GeoprocessingProjectItem>();
      foreach (var tbx in GPItems)
      {
        //Do Something with the toolbox
      }
      #endregion

      #region Search project for a specific item
      List<Item> _mxd = new List<Item>();
      //Gets all the folder connections in the current project
      var allFoldersItem = Project.Current.GetItems<FolderConnectionProjectItem>();
      if (allFoldersItem != null)
      {
        //iterate through all the FolderConnectionProjectItems found
        foreach (var folderItem in allFoldersItem)
        {
          //Search for mxd files in that folder connection and add it to the List<T>
          //Note:ArcGIS Pro automatically creates and dynamically updates a searchable index as you build and work with projects. 
          //Items are indexed when they are added to a project.
          //The first time a folder or database is indexed, indexing may take a while if it contains a large number of items. 
          //While the index is being created, searches will not return any results.
          _mxd.AddRange(folderItem.GetItems());
        }
      }
      #endregion

      #region Get The Default Project Folder

      var defaultProjectPath = System.IO.Path.Combine(
                  System.Environment.GetFolderPath(
                       Environment.SpecialFolder.MyDocuments),
                  @"ArcGIS\Projects");

      #endregion
    }

    public void ContentSnippet3()
    {
      #region Get Item Categories
      // Get the ItemCategories with which an item is associated
      Item gdb = ItemFactory.Instance.Create(@"E:\CurrentProject\RegionalPolling\polldata.gdb");
      List<ItemCategory> gdbItemCategories = gdb.ItemCategories;
      #endregion

      #region Using Item Categories
      // Browse items using an ItemCategory as a filter
      IEnumerable<Item> gdbContents = gdb.GetItems();
      IEnumerable<Item> filteredGDBContents1 = gdbContents.Where(item => item.ItemCategories.OfType<ItemCategoryDataSet>().Any());
      IEnumerable<Item> filteredGDBContents2 = new ItemCategoryDataSet().Items(gdbContents);
      #endregion

    }

    #region Method to Return The Default Template Folder

    public static string GetDefaultTemplateFolder()
    {
      string dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
      string root = dir.Split(new string[] { @"\bin" }, StringSplitOptions.RemoveEmptyEntries)[0];
      return System.IO.Path.Combine(root, @"Resources\ProjectTemplates");
    }

    #endregion

    #region Get The List of Installed Templates

    public static Task<List<string>> GetDefaultTemplatesAsync()
    {
      return Task.Run(() =>
      {
        string templatesDir = GetDefaultTemplateFolder();
        return
            Directory.GetFiles(templatesDir, "*", SearchOption.TopDirectoryOnly)
                .Where(f => f.EndsWith(".ppkx") || f.EndsWith(".aptx")).ToList();
      });
    }

    #endregion

    public static async void CreateProjectWithTemplate()
    {

      #region Create Project With Template
      var templates = await GetDefaultTemplatesAsync();
      var projectFolder = System.IO.Path.Combine(
          System.Environment.GetFolderPath(
              Environment.SpecialFolder.MyDocuments),
          @"ArcGIS\Projects");

      CreateProjectSettings ps = new CreateProjectSettings()
      {
        Name = "MyProject",
        LocationPath = projectFolder,
        TemplatePath = templates[2]//2D "Map" template
      };

      var project = await Project.CreateAsync(ps);
      #endregion
    }

    public static void ItemFindAndSelection()
    {

      #region Select project containers (for use with SelectItemAsync)

      //Use Project.Current.ProjectItemContainers
      var folderContainer = Project.Current.ProjectItemContainers.First(c => c.Path == "FolderConnection");
      var gdbContainer = Project.Current.ProjectItemContainers.First(c => c.Path == "GDB");
      var mapContainer = Project.Current.ProjectItemContainers.First(c => c.Path == "Map");
      var layoutContainer = Project.Current.ProjectItemContainers.First(c => c.Path == "Layout");
      var toolboxContainer = Project.Current.ProjectItemContainers.First(c => c.Path == "GP");
      //etc.

      //or...use Project.Current.GetProjectItemContainer

      folderContainer = Project.Current.GetProjectItemContainer("FolderConnection");
      gdbContainer = Project.Current.GetProjectItemContainer("GDB");
      mapContainer = Project.Current.GetProjectItemContainer("Map");
      layoutContainer = Project.Current.GetProjectItemContainer("Layout");
      toolboxContainer = Project.Current.GetProjectItemContainer("GP");
      //etc.

      #endregion

      #region ProjectItem: Get an Item or Find an Item

      //GetItems searches project content
      var map = Project.Current.GetItems<MapProjectItem>().FirstOrDefault(m => m.Name == "Map1");
      var layout = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(m => m.Name == "Layout1");
      var folders = Project.Current.GetItems<FolderConnectionProjectItem>();
      var style = Project.Current.GetItems<StyleProjectItem>().FirstOrDefault(s => s.Name == "ArcGIS 3D");

      //Find item uses a catalog path. The path can be to a file or dataset
      var fcPath = @"C:\Pro\CommunitySampleData\Interacting with Maps\Interacting with Maps.gdb\Crimes";
      var pdfPath = @"C:\Temp\Layout1.pdf";
      var imgPath = @"C:\Temp\AddinDesktop16.png";

      var fc = Project.Current.FindItem(fcPath);
      var pdf = Project.Current.FindItem(pdfPath);
      var img = Project.Current.FindItem(imgPath);

      #endregion

      #region Select an item in the Catalog pane

      //Get the catalog pane
      ArcGIS.Desktop.Core.IProjectWindow projectWindow = Project.GetCatalogPane();
      //or get the active catalog view...
      //ArcGIS.Desktop.Core.IProjectWindow projectWindow = Project.GetActiveCatalogWindow();

      //eg Find a toolbox in the project
      string gpName = "Interacting with Maps.tbx";
      var toolbox = Project.Current.GetItems<GeoprocessingProjectItem>().FirstOrDefault(tbx => tbx.Name == gpName);
      //Select it under Toolboxes
      projectWindow.SelectItemAsync(toolbox, true, true, null);//null selects it in the first container - optionally await
      //Note: Project.Current.GetProjectItemContainer("GP") would get toolbox container...

      //assume toolbox is also under Folders container. Select it under Folders instead of Toolboxes
      var foldersContainer = Project.Current.ProjectItemContainers.First(c => c.Path == "FolderConnection");
      //We must specify the container because Folders comes second (after Toolboxes)
      projectWindow.SelectItemAsync(toolbox, true, true, foldersContainer);//optionally await

      //Find a map and select it
      var mapItem = Project.Current.GetItems<MapProjectItem>().FirstOrDefault(m => m.Name == "Map");
      //Map only occurs under "Maps" so the container need not be specified
      projectWindow.SelectItemAsync(mapItem, true, false, null);

      #endregion

    }

    public async Task MedataExamples()
    {
      string sourceXMLMetadataAsString = string.Empty;


      #region Item: Get its IMetadata interface

      Item gdbItem = ItemFactory.Instance.Create(@"C:\projectAlpha\GDBs\regionFive.gdb");
      IMetadata gdbMetadataItem = gdbItem as IMetadata;

      #endregion

      #region Item: Get an item's metadata: GetXML

      string gdbXMLMetadataXmlAsString = string.Empty;
      gdbXMLMetadataXmlAsString = await QueuedTask.Run(() => gdbMetadataItem.GetXml());
      //check metadata was returned
      if (!string.IsNullOrEmpty(gdbXMLMetadataXmlAsString))
      {
        //use the metadata
      }

      #endregion

      IMetadata featureClassMetadataItem = null;

      #region Item: Set the metadata of an item: SetXML

      await QueuedTask.Run(() =>
      {
        var xml = System.IO.File.ReadAllText(@"E:\Data\Metadata\MetadataForFeatClass.xml");
        //Will throw InvalidOperationException if the metadata cannot be changed
        //so check "CanEdit" first
        if (featureClassMetadataItem.CanEdit())
          featureClassMetadataItem.SetXml(xml);
      });
           

      #endregion

      IMetadata metadataItemToCheck = null;

      #region Item: Check the metadata can be edited: CanEdit

      bool canEdit1;
      //Call CanEdit before calling SetXml
      await QueuedTask.Run(() => canEdit1 = metadataItemToCheck.CanEdit());

      #endregion

      IMetadata metadataItemToSync = null;

      #region Item: Updates metadata with the current properties of the item: Synchronize

      string syncedMetadataXml = string.Empty;
      await QueuedTask.Run(() => syncedMetadataXml = metadataItemToSync.Synchronize());

      #endregion

      IMetadata metadataCopyFrom = null;
      IMetadata metadataItemImport = null;
      #region Item: Copy metadata from the source item's metadata: CopyMetadataFromItem

      Item featureClassItem = ItemFactory.Instance.Create(@"C:\projectAlpha\GDBs\regionFive.gdb\SourceFeatureClass");
      await QueuedTask.Run(() => metadataItemImport.CopyMetadataFromItem(featureClassItem));

      #endregion


      //Item gdbItem = ItemFactory.Instance.Create(@"C:\projectAlpha\GDBs\regionFive.gdb");

      

      #region Item: Updates metadata with the imported metadata - the input path can be the path to an item with metadata, or a URI to a XML file: ImportMetadata

      await QueuedTask.Run(() => metadataItemImport.ImportMetadata(@"E:\YellowStone.gdb\MyDataset\MyFeatureClass", MDImportExportOption.esriCurrentMetadataStyle));

      #endregion

      IMetadata metadataItemExport = null;

      #region Item: export the metadata of the currently selected item: ExportMetadata

      await QueuedTask.Run(() => metadataItemExport.ExportMetadata(@"E:\Temp\OutputXML.xml",  MDImportExportOption.esriCurrentMetadataStyle, MDExportRemovalOption.esriExportExactCopy));

      #endregion

      IMetadata metadataItemToSaveAsXML = null;

      #region Item: Save the metadata of the current item as XML: SaveMetadataAsXML

      await QueuedTask.Run(() => metadataItemToSaveAsXML.SaveMetadataAsXML(@"E:\Temp\OutputXML.xml", MDSaveAsXMLOption.esriExactCopy));

      #endregion

      IMetadata metadataItemToSaveAsHTML = null;

      #region Item: Save the metadata of the current item as HTML: SaveMetadataAsHTML

      await QueuedTask.Run(() => metadataItemToSaveAsHTML.SaveMetadataAsHTML(@"E:\Temp\OutputHTML.htm", MDSaveAsHTMLOption.esriCurrentMetadataStyle));

      #endregion

      IMetadata metadataItemToSaveAsUsingCustomXSLT = null;

      #region Item: Save the metadata of the current item using customized XSLT: SaveMetadataAsUsingCustomXSLT

      await QueuedTask.Run(() => metadataItemToSaveAsUsingCustomXSLT.SaveMetadataAsUsingCustomXSLT(@"E:\Data\Metadata\CustomXSLT.xsl", @"E:\Temp\OutputXMLCustom.xml"));
     #endregion
     #region Item: Upgrade the metadata of the current item: UpgradeMetadata

      var fgdcItem = ItemFactory.Instance.Create(@"C:\projectAlpha\GDBs\testData.gdb");
      await QueuedTask.Run(() => fgdcItem.UpgradeMetadata(MDUpgradeOption.esriUpgradeFgdcCsdgm));
     #endregion

        }
    }
}
