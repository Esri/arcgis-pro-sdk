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
using ArcGIS.Desktop.Core.UnitFormats;
using ArcGIS.Desktop.Core.Events;
using ArcGIS.Core.Data;

namespace Content.Snippets
{

  #region ProSnippet Group: Project
  #endregion

  internal class ProSnippets1
  {
    public async void ContentSnippet()
    {
      // cref: ArcGIS.Desktop.Core.Project.CreateAsync
      #region Create an empty project
      //Create an empty project. The project will be created in the default folder
      //It will be named MyProject1, MyProject2, or similar...
      await Project.CreateAsync();
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.CreateAsync(ArcGIS.Desktop.Core.CreateProjectSettings)
      // cref: ArcGIS.Desktop.Core.CreateProjectSettings
      #region Create a new project with specified name
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
      // cref: ArcGIS.Desktop.Core.Project.CreateAsync(ArcGIS.Desktop.Core.CreateProjectSettings)
      // cref: ArcGIS.Desktop.Core.CreateProjectSettings
      // cref: ArcGIS.Desktop.Core.Project.GetDefaultProjectSettings
      #region Create new project using Pro's default settings
      //Get Pro's default project settings.
      var defaultProjectSettings = Project.GetDefaultProjectSettings();
      //Create a new project using the default project settings
      await Project.CreateAsync(defaultProjectSettings);
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.CreateAsync(ArcGIS.Desktop.Core.CreateProjectSettings)
      // cref: ArcGIS.Desktop.Core.CreateProjectSettings
      #region New project using a custom template file
      //Settings used to create a new project
      CreateProjectSettings projectSettings = new CreateProjectSettings()
      {
        //Sets the project's name
        Name = "New Project",
        //Path where new project will be stored in
        LocationPath = @"C:\Data\NewProject",
        //Sets the project template that will be used to create the new project
        TemplatePath = @"C:\Data\MyProject1\CustomTemplate.aptx"
      };
      //Create the new project
      await Project.CreateAsync(projectSettings);
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.CreateAsync(ArcGIS.Desktop.Core.CreateProjectSettings)
      // cref: ArcGIS.Desktop.Core.CreateProjectSettings
      // cref: ArcGIS.Desktop.Core.TemplateType
      #region Create a project using template available with ArcGIS Pro
      //Settings used to create a new project
      CreateProjectSettings proTemplateSettings = new CreateProjectSettings()
      {
        //Sets the project's name
        Name = "New Project",
        //Path where new project will be stored in
        LocationPath = @"C:\Data\NewProject",
        //Select which Pro template you like to use
        TemplateType = TemplateType.Catalog
        //TemplateType = TemplateType.LocalScene
        //TemplateType = TemplateType.GlobalScene
        //TemplateType = TemplateType.Map
      };
      //Create the new project
      await Project.CreateAsync(proTemplateSettings);
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.OpenAsync(System.String)
      #region Open an existing project
      //Opens an existing project or project package
      await Project.OpenAsync(@"C:\Data\MyProject1\MyProject1.aprx");
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.Current 
      #region Get the Current project
      //Gets the current project
      var project = Project.Current;
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.Current
      // cref: ArcGIS.Desktop.Core.Project.URI
      #region Get location of current project
      //Gets the location of the current project; that is, the path to the current project file (*.aprx)  
      string projectPath = Project.Current.URI;
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.Current
      // cref: ArcGIS.Desktop.Core.Project.DefaultGeodatabasePath
      #region Get the project's default gdb path
      var projGDBPath = Project.Current.DefaultGeodatabasePath;
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.SaveAsync
      #region Save project
      //Saves the project
      await Project.Current.SaveAsync();

      #endregion

      // cref: ArcGIS.Desktop.Core.Project.Current
      // cref: ArcGIS.Desktop.Core.Project.IsDirty
      #region Check if project needs to be saved
      //The project's dirty state indicates changes made to the project have not yet been saved. 
      bool isProjectDirty = Project.Current.IsDirty;
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.SaveAsAsync
      #region SaveAs project
      //Saves a copy of the current project file (*.aprx) to the specified location with the specified file name, 
      //then opens the new project file
      await Project.Current.SaveAsAsync(@"C:\Data\MyProject1\MyNewProject1.aprx");
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.OpenAsync(System.String)
      #region Close a project
      //A project cannot be closed using the ArcGIS Pro API. 
      //A project is only closed when another project is opened, a new one is created, or the application is shutdown.
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapFactory.CreateMap(System.String,ArcGIS.Core.CIM.MapType,ArcGIS.Core.CIM.MapViewingMode,ArcGIS.Desktop.Mapping.Basemap)
      #region How to add a new map to a project
      await QueuedTask.Run(() =>
      {
        //Note: see also MapFactory in ArcGIS.Desktop.Mapping
        var map = MapFactory.Instance.CreateMap("New Map", MapType.Map, MapViewingMode.Map, Basemap.Oceans);
        ProApp.Panes.CreateMapPaneAsync(map);
      });

      #endregion

      // cref: ArcGIS.Desktop.Core.Project.GetRecentProjects()
      #region Get Recent Projects
      var recentProjects = ArcGIS.Desktop.Core.Project.GetRecentProjects();
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.ClearRecentProjects()
      #region Clear Recent Projects
      ArcGIS.Desktop.Core.Project.ClearRecentProjects();
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.RemoveRecentProject(string)
      #region Remove a Recent Project
      ArcGIS.Desktop.Core.Project.RemoveRecentProject(projectPath);
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.GetPinnedProjects()
      #region Get Pinned Projects

      var pinnedProjects = ArcGIS.Desktop.Core.Project.GetPinnedProjects();
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.ClearPinnedProjects()
      #region Clear Pinned Projects
      ArcGIS.Desktop.Core.Project.ClearPinnedProjects();
      #endregion

      string projectPath2 = "";
      // cref: ArcGIS.Desktop.Core.Project.PinProject(string)
      // cref: ArcGIS.Desktop.Core.Project.UnpinProject(string)
      #region Pin / UnPin Projects
      ArcGIS.Desktop.Core.Project.PinProject(projectPath);
      ArcGIS.Desktop.Core.Project.UnpinProject(projectPath2);

      #endregion

      string templatePath = "";
      // cref: ArcGIS.Desktop.Core.Project.GetRecentProjectTemplates()
      #region Get Recent Project Templates
      var recentTemplates = ArcGIS.Desktop.Core.Project.GetRecentProjectTemplates();
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.ClearRecentProjectTemplates()
      #region Clear Recent Project Templates
      ArcGIS.Desktop.Core.Project.ClearRecentProjectTemplates();
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.RemoveRecentProjectTemplate(string)
      #region Remove a Recent Project Template
      ArcGIS.Desktop.Core.Project.RemoveRecentProjectTemplate(templatePath);
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.GetPinnedProjectTemplates()
      #region Get Pinned Project Templates

      var pinnedTemplates = ArcGIS.Desktop.Core.Project.GetPinnedProjectTemplates();
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.ClearPinnedProjectTemplates()
      #region Clear Pinned Project Templates
      ArcGIS.Desktop.Core.Project.ClearPinnedProjectTemplates();
      #endregion

      string templatePath2 = "";
      // cref: ArcGIS.Desktop.Core.Project.PinProjectTemplate(string)
      // cref: ArcGIS.Desktop.Core.Project.UnpinTemplateProject(string)
      #region Pin / UnPin Project Templates
      ArcGIS.Desktop.Core.Project.PinProjectTemplate(templatePath);
      ArcGIS.Desktop.Core.Project.UnpinTemplateProject(templatePath2);

      #endregion


      #region ProSnippet Group: Project Items
      #endregion

      // cref: ArcGIS.Desktop.Core.ItemFactory
      // cref: ArcGIS.Desktop.Core.IItemFactory.Create
      // cref: ArcGIS.Desktop.Core.Project.AddItem(ArcGIS.Desktop.Core.IProjectItem)
      #region Add a folder connection item to the current project
      //Adding a folder connection
      string folderPath = "@C:\\myDataFolder";
      var folder = await QueuedTask.Run(() =>
      {
        //Create the folder connection project item
        var item = ItemFactory.Instance.Create(folderPath) as IProjectItem;
        //If it is successfully added to the project, return it otherwise null
        return Project.Current.AddItem(item) ? item as FolderConnectionProjectItem : null;
      });

      //Adding a Geodatabase:
      string gdbPath = "@C:\\myDataFolder\\myData.gdb";
      var newlyAddedGDB = await QueuedTask.Run(() =>
      {
        //Create the File GDB project item
        var item = ItemFactory.Instance.Create(gdbPath) as IProjectItem;
        //If it is successfully added to the project, return it otherwise null
        return Project.Current.AddItem(item) ? item as GDBProjectItem : null;
      });

      #endregion

      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      #region Get all project items
      IEnumerable<Item> allProjectItems = Project.Current.GetItems<Item>();
      foreach (var pi in allProjectItems)
      {
        //Do Something 
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapProjectItem
      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      #region Get all "MapProjectItems" for a project
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

      // cref: ArcGIS.Desktop.Mapping.MapProjectItem
      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      #region Get a specific "MapProjectItem"
      MapProjectItem mapProjItem = Project.Current.GetItems<MapProjectItem>().FirstOrDefault(item => item.Name.Equals("EuropeMap"));
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StyleProjectItem
      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      #region Get all "StyleProjectItems"
      IEnumerable<StyleProjectItem> newStyleItemsContainer = null;
      newStyleItemsContainer = Project.Current.GetItems<StyleProjectItem>();
      foreach (var styleItem in newStyleItemsContainer)
      {
        //Do Something with the style.
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StyleProjectItem
      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      #region Get a specific "StyleProjectItem"
      var container = Project.Current.GetItems<StyleProjectItem>();
      StyleProjectItem testStyle = container.FirstOrDefault(style => (style.Name == "ArcGIS 3D"));
      StyleItem cone = null;
      if (testStyle != null)
        cone = testStyle.LookupItem(StyleItemType.PointSymbol, "Cone_Volume_3");
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StyleProjectItem
      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      #region Get the "Favorite" StyleProjectItem

      var fav_style_item = await QueuedTask.Run(() =>
      {
        var containerStyle = Project.Current.GetProjectItemContainer("Style");
        return containerStyle.GetItems().OfType<StyleProjectItem>().First(item => item.TypeID == "personal_style");
      });

      #endregion

      // cref: ArcGIS.Desktop.Catalog.GDBProjectItem
      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      #region Get all "GDBProjectItems"
      IEnumerable<GDBProjectItem> newGDBItemsContainer = null;
      newGDBItemsContainer = Project.Current.GetItems<GDBProjectItem>();
      foreach (var GDBItem in newGDBItemsContainer)
      {
        //Do Something with the GDB.
      }
      #endregion

      // cref: ArcGIS.Desktop.Catalog.GDBProjectItem
      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      #region Get a specific "GDBProjectItem"
      GDBProjectItem GDBProjItem = Project.Current.GetItems<GDBProjectItem>().FirstOrDefault(item => item.Name.Equals("myGDB"));
      #endregion

      // cref: ArcGIS.Desktop.Catalog.ServerConnectionProjectItem
      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      #region Get all "ServerConnectionProjectItems"
      IEnumerable<ServerConnectionProjectItem> newServerConnections = null;
      newServerConnections = project.GetItems<ServerConnectionProjectItem>();
      foreach (var serverItem in newServerConnections)
      {
        //Do Something with the server connection.
      }
      #endregion

      // cref: ArcGIS.Desktop.Catalog.ServerConnectionProjectItem
      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      #region Get a specific "ServerConnectionProjectItem"
      ServerConnectionProjectItem serverProjItem = Project.Current.GetItems<ServerConnectionProjectItem>().FirstOrDefault(item => item.Name.Equals("myServer"));
      #endregion

      // cref: ArcGIS.Desktop.Catalog.FolderConnectionProjectItem
      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      #region Get all folder connections in a project
      //Gets all the folder connections in the current project
      var projectFolders = Project.Current.GetItems<FolderConnectionProjectItem>();
      foreach (var FolderItem in projectFolders)
      {
        //Do Something with the Folder connection.
      }
      #endregion

      // cref: ArcGIS.Desktop.Catalog.FolderConnectionProjectItem
      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      #region Get a specific folder connection
      //Gets a specific folder connection in the current project
      FolderConnectionProjectItem myProjectFolder = Project.Current.GetItems<FolderConnectionProjectItem>().FirstOrDefault(folderPI => folderPI.Name.Equals("myDataFolder"));
      #endregion

      // cref: ArcGIS.Desktop.Catalog.FolderConnectionProjectItem
      // cref: ArcGIS.Desktop.Core.Project.RemoveItem
      #region Remove a specific folder connection
      // Remove a folder connection from a project; the folder stored on the local disk or the network is not deleted
      FolderConnectionProjectItem folderToRemove = Project.Current.GetItems<FolderConnectionProjectItem>().FirstOrDefault(myfolder => myfolder.Name.Equals("PlantSpecies"));
      if (folderToRemove != null)
        Project.Current.RemoveItem(folderToRemove as IProjectItem);
      #endregion

      // cref: ArcGIS.Desktop.Layouts.LayoutProjectItem
      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      #region Gets a specific "LayoutProjectItem"
      LayoutProjectItem layoutProjItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals("myLayout"));
      #endregion

      // cref: ArcGIS.Desktop.Layouts.LayoutProjectItem
      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      #region Get all layouts in a project
      //Gets all the layouts in the current project
      var projectLayouts = Project.Current.GetItems<LayoutProjectItem>();
      foreach (var layoutItem in projectLayouts)
      {
        //Do Something with the layout
      }
      #endregion

      // cref: ArcGIS.Desktop.GeoProcessing.GeoprocessingProjectItem
      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      #region Get a specific "GeoprocessingProjectItem"
      GeoprocessingProjectItem GPProjItem = Project.Current.GetItems<GeoprocessingProjectItem>().FirstOrDefault(item => item.Name.Equals("myToolbox"));
      #endregion

      // cref: ArcGIS.Desktop.GeoProcessing.GeoprocessingProjectItem
      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      #region Get all GeoprocessingProjectItems in a project
      //Gets all the GeoprocessingProjectItem in the current project
      var GPItems = Project.Current.GetItems<GeoprocessingProjectItem>();
      foreach (var tbx in GPItems)
      {
        //Do Something with the toolbox
      }
      #endregion

      // cref: ArcGIS.Desktop.Catalog.FolderConnectionProjectItem
      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
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

      // cref: ArcGIS.Desktop.Core.CreateProjectSettings.LocationPath
      // cref: ArcGIS.Desktop.Core.Project.GetDefaultProjectSettings
      #region Get the Default Project Folder
      //Get Pro's default project settings.
      var defaultSettings = Project.GetDefaultProjectSettings();
      var defaultProjectPath = defaultSettings.LocationPath;
      if (defaultProjectPath == null)
      {
        // If not set, projects are saved in the user's My Documents\ArcGIS\Projects folder;
        // this folder is created if it doesn't already exist.
        defaultProjectPath = System.IO.Path.Combine(
                    System.Environment.GetFolderPath(
                         Environment.SpecialFolder.MyDocuments),
                    @"ArcGIS\Projects");
      }
      #endregion

      // cref: ArcGIS.Desktop.Catalog.FolderConnectionProjectItem
      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      // cref: ArcGIS.Desktop.Core.Item.Refresh
      #region Refresh the child item for a folder connection Item
      var contentItem = Project.Current.GetItems<FolderConnectionProjectItem>().First();
      //var contentItem = ...
      //Check if the MCT is required for Refresh()
      if (contentItem.IsMainThreadRequired)
      {
        //QueuedTask.Run must be used if item.IsMainThreadRequired
        //returns true
        QueuedTask.Run(() => contentItem.Refresh());
      }
      else
      {
        //if item.IsMainThreadRequired returns false, any
        //thread can be used to invoke Refresh(), though
        //BackgroundTask is preferred.
        contentItem.Refresh();

        //Or, via BackgroundTask
        ArcGIS.Core.Threading.Tasks.BackgroundTask.Run(() =>
          contentItem.Refresh(), ArcGIS.Core.Threading.Tasks.BackgroundProgressor.None);
      }
      #endregion
    }

    public void ContentSnippet3()
    {
      // cref: ArcGIS.Desktop.Core.Item.ItemCategories
      #region Get Item Categories
      // Get the ItemCategories with which an item is associated
      Item gdb = ItemFactory.Instance.Create(@"E:\CurrentProject\RegionalPolling\polldata.gdb");
      List<ItemCategory> gdbItemCategories = gdb.ItemCategories;
      #endregion

      // cref: ArcGIS.Desktop.Core.ItemCategory.Items(System.Collections.Generic.IEnumerable{ArcGIS.Desktop.Core.Item})
      #region Using Item Categories
      // Browse items using an ItemCategory as a filter
      IEnumerable<Item> gdbContents = gdb.GetItems();
      IEnumerable<Item> filteredGDBContents1 = gdbContents.Where(item => item.ItemCategories.OfType<ItemCategoryDataSet>().Any());
      IEnumerable<Item> filteredGDBContents2 = new ItemCategoryDataSet().Items(gdbContents);
      #endregion

    }


    //removed at 2.3
    public static string GetDefaultTemplateFolder()
    {
      string dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
      string root = dir.Split(new string[] { @"\bin" }, StringSplitOptions.RemoveEmptyEntries)[0];
      return System.IO.Path.Combine(root, @"Resources\ProjectTemplates");
    }



    //removed at 2.3

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



    public static async void CreateProjectWithTemplate()
    {
      // cref: ArcGIS.Desktop.Core.Project.CreateAsync(ArcGIS.Desktop.Core.CreateProjectSettings)
      // cref: ArcGIS.Desktop.Core.CreateProjectSettings
      #region Create Project with Template      
      var projectFolder = System.IO.Path.Combine(
          System.Environment.GetFolderPath(
              Environment.SpecialFolder.MyDocuments),
          @"ArcGIS\Projects");

      CreateProjectSettings ps = new CreateProjectSettings()
      {
        Name = "MyProject",
        LocationPath = projectFolder,
        TemplatePath = @"C:\data\my_templates\custom_template.aptx"
      };

      var project = await Project.CreateAsync(ps);
      #endregion
    }

    public static void ItemFindAndSelection()
    {

      // cref: ArcGIS.Desktop.Core.Project.ProjectItemContainers
      // cref: ArcGIS.Desktop.Core.Project.GetProjectItemContainer(System.String)
      #region Select project containers - for use with SelectItemAsync

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

      // cref: ArcGIS.Desktop.Catalog.FolderConnectionProjectItem
      // cref: ArcGIS.Desktop.Layouts.LayoutProjectItem
      // cref: ArcGIS.Desktop.Mapping.MapProjectItem
      // cref: ArcGIS.Desktop.Mapping.StyleProjectItem
      // cref: ArcGIS.Desktop.Core.Project.FindItem(System.String)
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


      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      // cref: ArcGIS.Desktop.Core.Project.GetCatalogPane
      // cref: ArcGIS.Desktop.Core.IProjectWindow
      // cref: ArcGIS.Desktop.Core.Project.ProjectItemContainers
      // cref: ArcGIS.Desktop.Core.IProjectWindow.SelectItemAsync
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

    #region ProSnippet Group: Geodatabase Content 
    #endregion

    public async void Browse()
    {
      // cref: ArcGIS.Desktop.Catalog.OpenItemDialog
      // cref: ArcGIS.Desktop.Catalog.OpenItemDialog.ShowDialog
      // cref: ArcGIS.Desktop.Core.BrowseProjectFilter
      #region Geodatabase Content from Browse Dialog

      var openDlg = new OpenItemDialog
      {
        Title = "Select a Feature Class",
        InitialLocation = @"C:\Data",
        MultiSelect = false,
        BrowseFilter = BrowseProjectFilter.GetFilter(ArcGIS.Desktop.Catalog.ItemFilters.GeodatabaseItems_All)
      };

      //show the browse dialog
      bool? ok = openDlg.ShowDialog();
      if (!ok.HasValue || openDlg.Items.Count() == 0)
        return;   //nothing selected

      await QueuedTask.Run(() =>
      {
        // get the item
        var item = openDlg.Items.First();

        // see if the item has a dataset
        if (ItemFactory.Instance.CanGetDataset(item))
        {
          // get it
          using (var ds = ItemFactory.Instance.GetDataset(item))
          {
            // access some properties
            var name = ds.GetName();
            var path = ds.GetPath();

            // if it's a featureclass
            if (ds is ArcGIS.Core.Data.FeatureClass fc)
            {
              // create a layer 
              var featureLayerParams = new FeatureLayerCreationParams(fc)
              {
                MapMemberIndex = 0
              };
              var layer = LayerFactory.Instance.CreateLayer<FeatureLayer>(featureLayerParams, MapView.Active.Map);

              // continue
            }
          }
        }
      });
      #endregion
    }

    public void CatalogWindow()
    {
      // cref: ArcGIS.Desktop.Core.Events.ProjectWindowSelectedItemsChangedEventArgs
      // cref: ArcGIS.Desktop.Core.Events.ProjectWindowSelectedItemsChangedEvent
      // cref: ArcGIS.Desktop.Core.Events.ProjectWindowSelectedItemsChangedEvent.Subscribe
      // cref: ArcGIS.Desktop.Core.IItemFactory.CanGetDataset
      // cref: ArcGIS.Desktop.Core.ItemFactory.CanGetDataset
      #region Geodatabase Content from Catalog selection

      // subscribe to event
      ProjectWindowSelectedItemsChangedEvent.Subscribe(async (ProjectWindowSelectedItemsChangedEventArgs args) =>
      {
        if (args.IProjectWindow.SelectionCount > 0)
        {
          // get the first selected item
          var selectedItem = args.IProjectWindow.SelectedItems.First();

          await QueuedTask.Run(() =>
          {
            // datasetType
            var dataType = ItemFactory.Instance.GetDatasetType(selectedItem);

            // get the dataset Definition
            if (ItemFactory.Instance.CanGetDefinition(selectedItem))
            {
              using (var def = ItemFactory.Instance.GetDefinition(selectedItem))
              {
                if (def is ArcGIS.Core.Data.FeatureClassDefinition fcDef)
                {
                  var oidField = fcDef.GetObjectIDField();
                  var shapeField = fcDef.GetShapeField();
                  var shapeType = fcDef.GetShapeType();
                }
                else if (def is ArcGIS.Core.Data.Parcels.ParcelFabricDefinition pfDef)
                {
                  string ver = pfDef.GetSchemaVersion();
                  bool enabled = pfDef.GetTopologyEnabled();
                }

                // etc
              }
            }

            // get the dataset
            if (ItemFactory.Instance.CanGetDataset(selectedItem))
            {
              using (var ds = ItemFactory.Instance.GetDataset(selectedItem))
              {
                if (ds is ArcGIS.Core.Data.FeatureDataset fds)
                {
                  // open featureclasses within the feature dataset
                  // var fcPoint = fds.OpenDataset<FeatureClass>("Point");
                  // var fcPolyline = fds.OpenDataset<FeatureClass>("Polyline");
                }
                else if (ds is FeatureClass fc)
                {
                  var name = fc.GetName() + "_copy";

                  // create
                  var featureLayerParams = new FeatureLayerCreationParams(fc)
                  {
                    Name = name,
                    MapMemberIndex = 0
                  };
                  LayerFactory.Instance.CreateLayer<FeatureLayer>(featureLayerParams, MapView.Active.Map);
                }
                else if (ds is Table table)
                {
                  var name = table.GetName() + "_copy";
                  var tableParams = new StandaloneTableCreationParams(table)
                  {
                    Name = name
                  };
                  // create
                  StandaloneTableFactory.Instance.CreateStandaloneTable(tableParams, MapView.Active.Map);
                }
              }
            }
          });
        }
      });
      #endregion
    }

    #region ProSnippet Group: Favorites

    public void Favorites()
    {
      void AddFavorite()
      {
        // cref: ArcGIS.Desktop.Core.FavoritesManager.GetFavorite
        // cref: ArcGIS.Desktop.Core.FavoritesManager.CanAddAsFavorite
        // cref: ArcGIS.Desktop.Core.FavoritesManager.AddFavorite(ArcGIS.Desktop.Core.Item)
        #region Add a Favorite - Folder

        var itemFolder = ItemFactory.Instance.Create(@"d:\data");

        // is the folder item already a favorite?
        var fav = FavoritesManager.Current.GetFavorite(itemFolder);
        if (fav == null)
        {
          if (FavoritesManager.Current.CanAddAsFavorite(itemFolder))
          {
            fav = FavoritesManager.Current.AddFavorite(itemFolder);
          }
        }

        #endregion
      }

      void InsertFavorite()
      {
      // cref: ArcGIS.Desktop.Core.FavoritesManager.GetFavorite
      // cref: ArcGIS.Desktop.Core.FavoritesManager.CanAddAsFavorite
      // cref: ArcGIS.Desktop.Core.FavoritesManager.InsertFavorite(ArcGIS.Desktop.Core.Item,Int32)
      // cref: ArcGIS.Desktop.Core.FavoritesManager.InsertFavorite(ArcGIS.Desktop.Core.Item,Int32,Boolean)
        #region Insert a Favorite - Geodatabase path

        string gdbPath = "@C:\\myDataFolder\\myData.gdb";

        var itemGDB = ItemFactory.Instance.Create(gdbPath);

        // is the item already a favorite?
        var fav = FavoritesManager.Current.GetFavorite(itemGDB);
        // no; add it with IsAddedToAllNewProjects set to true
        if (fav != null)
        {
          if (FavoritesManager.Current.CanAddAsFavorite(itemGDB))
            FavoritesManager.Current.InsertFavorite(itemGDB, 1, true);
        }
        #endregion
      }

      void AddFavoriteStyle()
      {
        // cref: ArcGIS.Desktop.Core.FavoritesManager.GetFavorite
        // cref: ArcGIS.Desktop.Core.FavoritesManager.CanAddAsFavorite
        // cref: ArcGIS.Desktop.Core.FavoritesManager.AddFavorite(ArcGIS.Desktop.Core.Item)
        #region Add a Favorite - Style project item

        StyleProjectItem styleItem = Project.Current.GetItems<StyleProjectItem>().
                                FirstOrDefault(style => (style.Name == "ArcGIS 3D"));

        if (FavoritesManager.Current.CanAddAsFavorite(styleItem))
        {
          // add to favorites with IsAddedToAllNewProjects set to false
          FavoritesManager.Current.AddFavorite(styleItem);
        }

        #endregion
      }

      void ToggleFavoriteFlag()
      {
        // cref: ArcGIS.Desktop.Core.FavoritesManager.GetFavorite
        // cref: ArcGIS.Desktop.Core.FavoritesManager.ClearIsAddedToAllNewProjects
        // cref: ArcGIS.Desktop.Core.FavoritesManager.SetIsAddedToAllNewProjects
        #region Toggle the flag IsAddedToAllNewProjects for a favorite

        var itemFolder = ItemFactory.Instance.Create(@"d:\data");

        // is the folder item already a favorite?
        var fav = FavoritesManager.Current.GetFavorite(itemFolder);
        if (fav != null)
        {
          if (fav.IsAddedToAllNewProjects)
            FavoritesManager.Current.ClearIsAddedToAllNewProjects(fav.Item);
          else
            FavoritesManager.Current.SetIsAddedToAllNewProjects(fav.Item);
        }
        #endregion
      }

      void GetFavorites()
      {
        // cref: ArcGIS.Desktop.Core.FavoritesManager.GetFavorites
        #region Get the set of favorites and iterate
        var favorites = FavoritesManager.Current.GetFavorites();
        foreach (var favorite in favorites)
        {
          bool isAddedToAllProjects = favorite.IsAddedToAllNewProjects;
          // retrieve the underlying item of the favorite
          Item item = favorite.Item;

          // Item properties
          var itemType = item.TypeID;
          var path = item.Path;

          // if it's a folder item
          if (item is FolderConnectionProjectItem)
          {
          }
          // if it's a goedatabase item
          else if (item is GDBProjectItem)
          {
          }
          // else 
        }

        #endregion
      }

      void RemoveAllFavorites()
      {
        // cref: ArcGIS.Desktop.Core.FavoritesManager.GetFavorites
        // cref: ArcGIS.Desktop.Core.FavoritesManager.RemoveFavorite(ArcGIS.Desktop.Core.Item)
        #region Remove All Favorites

        var favorites = FavoritesManager.Current.GetFavorites();
        foreach (var favorite in favorites)
          FavoritesManager.Current.RemoveFavorite(favorite.Item);

        #endregion
      }

      void FavoriteEvent()
      {
        // cref: ArcGIS.Desktop.Core.Events.FavoritesChangedEvent
        // cref: ArcGIS.Desktop.Core.Events.FavoritesChangedEvent.Subscribe
        // cref: ArcGIS.Desktop.Core.FavoritesManager.GetFavorites
        #region FavoritesChangedEvent

        ArcGIS.Desktop.Core.Events.FavoritesChangedEvent.Subscribe((args) =>
        {
          // favorites have changed
          int count = FavoritesManager.Current.GetFavorites().Count;
        });

        #endregion
      }
    }
    #endregion

    #region ProSnippet Group: Metadata
    #endregion

    public async Task MedataExamples()
    {
      string sourceXMLMetadataAsString = string.Empty;


      // cref: Item: Get its IMetadata interface;ArcGIS.Desktop.Core.IMetadata
      #region Item: Get its IMetadata interface

      Item gdbItem = ItemFactory.Instance.Create(@"C:\projectAlpha\GDBs\regionFive.gdb");
      IMetadata gdbMetadataItem = gdbItem as IMetadata;

      #endregion

      // cref: Item: Get an item's metadata: GetXML;ArcGIS.Desktop.Core.IMetadata.GetXml
      // cref: Item: Get an item's metadata: GetXML;ArcGIS.Desktop.Core.Item.GetXml
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

      // cref: Item: Set the metadata of an item: SetXML;ArcGIS.Desktop.Core.IMetadata.SetXml(System.String)
      // cref: Item: Set the metadata of an item: SetXML;ArcGIS.Desktop.Core.Item.SetXml(System.String)
      // cref: Item: Set the metadata of an item: SetXML;ArcGIS.Desktop.Core.Project.SetXml(System.String)
      // cref: Item: Set the metadata of an item: SetXML;ARCGIS.DESKTOP.CORE.ITEM.CANEDIT
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

      // cref: Item: Check the metadata can be edited: CanEdit;ArcGIS.Desktop.Core.IMetadata.CanEdit
      // cref: Item: Check the metadata can be edited: CanEdit;ArcGIS.Desktop.Core.Item.CanEdit
      // cref: Item: Check the metadata can be edited: CanEdit;ArcGIS.Desktop.Core.Project.CanEdit
      #region Item: Check the metadata can be edited: CanEdit

      bool canEdit1;
      //Call CanEdit before calling SetXml
      await QueuedTask.Run(() => canEdit1 = metadataItemToCheck.CanEdit());

      #endregion

      IMetadata metadataItemToSync = null;

      // cref: Item: Updates metadata with the current properties of the item: Synchronize;ArcGIS.Desktop.Core.IMetadata.Synchronize
      // cref: Item: Updates metadata with the current properties of the item: Synchronize;ArcGIS.Desktop.Core.Item.Synchronize
      // cref: Item: Updates metadata with the current properties of the item: Synchronize;ArcGIS.Desktop.Core.Project.Synchronize
      #region Item: Updates metadata with the current properties of the item: Synchronize

      string syncedMetadataXml = string.Empty;
      await QueuedTask.Run(() => syncedMetadataXml = metadataItemToSync.Synchronize());

      #endregion

      IMetadata metadataCopyFrom = null;
      IMetadata metadataItemImport = null;
      // cref: ArcGIS.Desktop.Core.IMetadata.CopyMetadataFromItem(ArcGIS.Desktop.Core.Item)
      // cref: ArcGIS.Desktop.Core.Item.CopyMetadataFromItem(ArcGIS.Desktop.Core.Item)
      // cref: ArcGIS.Desktop.Core.Project.CopyMetadataFromItem(ArcGIS.Desktop.Core.Item)
      // cref: ARCGIS.DESKTOP.CORE.ITEMFACTORY.CREATE
      #region Item: Copy metadata from the source item's metadata: CopyMetadataFromItem

      Item featureClassItem = ItemFactory.Instance.Create(@"C:\projectAlpha\GDBs\regionFive.gdb\SourceFeatureClass");
      await QueuedTask.Run(() => metadataItemImport.CopyMetadataFromItem(featureClassItem));

      #endregion

      // cref: ArcGIS.Desktop.Core.IMetadata.DeleteMetadataContent(ArcGIS.Desktop.Core.MDDeleteContentOption)
      // cref: ArcGIS.Desktop.Core.Item.DeleteMetadataContent(ArcGIS.Desktop.Core.MDDeleteContentOption)
      // cref: ArcGIS.Desktop.Core.Project.DeleteMetadataContent(ArcGIS.Desktop.Core.MDDeleteContentOption)
      // cref: ARCGIS.DESKTOP.CORE.ITEMFACTORY.CREATE
      #region Item: Delete certain content from the metadata of the current item: DeleteMetadataContent

      Item featureClassWithMetadataItem = ItemFactory.Instance.Create(@"C:\projectBeta\GDBs\regionFive.gdb\SourceFeatureClass");
      //Delete thumbnail content from item's metadata
      await QueuedTask.Run(() => featureClassWithMetadataItem.DeleteMetadataContent(MDDeleteContentOption.esriMDDeleteThumbnail));

      #endregion

      //Item gdbItem = ItemFactory.Instance.Create(@"C:\projectAlpha\GDBs\regionFive.gdb");

      // cref: ArcGIS.Desktop.Core.IMetadata.ImportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption)
      // cref: ArcGIS.Desktop.Core.IMetadata.ImportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,System.String)
      // cref: ArcGIS.Desktop.Core.Item.ImportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption)
      // cref: ArcGIS.Desktop.Core.Item.ImportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,System.String)
      // cref: ArcGIS.Desktop.Core.Project.ImportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption)
      // cref: ArcGIS.Desktop.Core.Project.ImportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,System.String)
      #region Item: Updates metadata with the imported metadata - the input path can be the path to an item with metadata, or a URI to a XML file: ImportMetadata

      // the input path can be the path to an item with metadata, or a URI to an XML file
      IMetadata metadataItemImport1 = null;
      await QueuedTask.Run(() => metadataItemImport1.ImportMetadata(@"E:\YellowStone.gdb\MyDataset\MyFeatureClass", MDImportExportOption.esriCurrentMetadataStyle));

      #endregion

      IMetadata metadataItemImport2 = null;

      // cref: ArcGIS.Desktop.Core.IMetadata.ImportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,System.String)
      #region Item: Updates metadata with the imported metadata: ImportMetadata

      // the input path can be the path to an item with metadata, or a URI to an XML file

      await QueuedTask.Run(() => metadataItemImport2.ImportMetadata(@"E:\YellowStone.gdb\MyDataset\MyFeatureClass", MDImportExportOption.esriCustomizedStyleSheet, @"E:\StyleSheets\Import\MyImportStyleSheet.xslt"));

      #endregion

      IMetadata metadataItemExport1 = null;

      // cref: ArcGIS.Desktop.Core.IMetadata.ExportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,ArcGIS.Desktop.Core.MDExportRemovalOption)
      // cref: ArcGIS.Desktop.Core.IMetadata.ExportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,ArcGIS.Desktop.Core.MDExportRemovalOption,System.String)
      // cref: ArcGIS.Desktop.Core.Item.ExportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,ArcGIS.Desktop.Core.MDExportRemovalOption)
      // cref: ArcGIS.Desktop.Core.Item.ExportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,ArcGIS.Desktop.Core.MDExportRemovalOption,System.String)
      // cref: ArcGIS.Desktop.Core.Project.ExportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,ArcGIS.Desktop.Core.MDExportRemovalOption)
      // cref: ArcGIS.Desktop.Core.Project.ExportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,ArcGIS.Desktop.Core.MDExportRemovalOption,System.String)
      #region Item: export the metadata of the currently selected item: ExportMetadata

      await QueuedTask.Run(() => metadataItemExport1.ExportMetadata(@"E:\Temp\OutputXML.xml", MDImportExportOption.esriCurrentMetadataStyle, MDExportRemovalOption.esriExportExactCopy));

      #endregion

      IMetadata metadataItemExport2 = null;

      // cref: ArcGIS.Desktop.Core.IMetadata.ExportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,ArcGIS.Desktop.Core.MDExportRemovalOption)
      // cref: ArcGIS.Desktop.Core.IMetadata.ExportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,ArcGIS.Desktop.Core.MDExportRemovalOption,System.String)
      // cref: ArcGIS.Desktop.Core.Item.ExportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,ArcGIS.Desktop.Core.MDExportRemovalOption)
      // cref: ArcGIS.Desktop.Core.Item.ExportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,ArcGIS.Desktop.Core.MDExportRemovalOption,System.String)
      // cref: ArcGIS.Desktop.Core.Project.ExportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,ArcGIS.Desktop.Core.MDExportRemovalOption)
      // cref: ArcGIS.Desktop.Core.Project.ExportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,ArcGIS.Desktop.Core.MDExportRemovalOption,System.String)
      #region Item: export the metadata of the currently selected item: ExportMetadata

      await QueuedTask.Run(() => metadataItemExport2.ExportMetadata(@"E:\Temp\OutputXML.xml", MDImportExportOption.esriCustomizedStyleSheet, MDExportRemovalOption.esriExportExactCopy, @"E:\StyleSheets\Export\MyExportStyleSheet.xslt"));

      #endregion

      IMetadata metadataItemToSaveAsXML = null;

      // cref: ArcGIS.Desktop.Core.IMetadata.SaveMetadataAsXML(System.String,ArcGIS.Desktop.Core.MDSaveAsXMLOption)
      // cref: ArcGIS.Desktop.Core.Item.SaveMetadataAsXML(System.String,ArcGIS.Desktop.Core.MDSaveAsXMLOption)
      // cref: ArcGIS.Desktop.Core.Project.SaveMetadataAsXML(System.String,ArcGIS.Desktop.Core.MDSaveAsXMLOption)
      #region Item: Save the metadata of the current item as XML: SaveMetadataAsXML

      await QueuedTask.Run(() => metadataItemToSaveAsXML.SaveMetadataAsXML(@"E:\Temp\OutputXML.xml", MDSaveAsXMLOption.esriExactCopy));

      #endregion

      IMetadata metadataItemToSaveAsHTML = null;

      // cref: ArcGIS.Desktop.Core.IMetadata.SaveMetadataAsHTML(System.String,ArcGIS.Desktop.Core.MDSaveAsHTMLOption)
      // cref: ArcGIS.Desktop.Core.Item.SaveMetadataAsHTML(System.String,ArcGIS.Desktop.Core.MDSaveAsHTMLOption)
      // cref: ArcGIS.Desktop.Core.Project.SaveMetadataAsHTML(System.String,ArcGIS.Desktop.Core.MDSaveAsHTMLOption)
      #region Item: Save the metadata of the current item as HTML: SaveMetadataAsHTML

      await QueuedTask.Run(() => metadataItemToSaveAsHTML.SaveMetadataAsHTML(@"E:\Temp\OutputHTML.htm", MDSaveAsHTMLOption.esriCurrentMetadataStyle));

      #endregion

      IMetadata metadataItemToSaveAsUsingCustomXSLT = null;

      // cref: ArcGIS.Desktop.Core.IMetadata.SaveMetadataAsUsingCustomXSLT(System.String,System.String)
      // cref: ArcGIS.Desktop.Core.Item.SaveMetadataAsUsingCustomXSLT(System.String,System.String)
      // cref: ArcGIS.Desktop.Core.Project.SaveMetadataAsUsingCustomXSLT(System.String,System.String)
      #region Item: Save the metadata of the current item using customized XSLT: SaveMetadataAsUsingCustomXSLT

      await QueuedTask.Run(() => metadataItemToSaveAsUsingCustomXSLT.SaveMetadataAsUsingCustomXSLT(@"E:\Data\Metadata\CustomXSLT.xsl", @"E:\Temp\OutputXMLCustom.xml"));
      #endregion

      // cref: ArcGIS.Desktop.Core.IMetadata.UpgradeMetadata(ArcGIS.Desktop.Core.MDUpgradeOption)
      // cref: ArcGIS.Desktop.Core.Item.UpgradeMetadata(ArcGIS.Desktop.Core.MDUpgradeOption)
      // cref: ArcGIS.Desktop.Core.Project.UpgradeMetadata(ArcGIS.Desktop.Core.MDUpgradeOption)
      #region Item: Upgrade the metadata of the current item: UpgradeMetadata

      var fgdcItem = ItemFactory.Instance.Create(@"C:\projectAlpha\GDBs\testData.gdb");
      await QueuedTask.Run(() => fgdcItem.UpgradeMetadata(MDUpgradeOption.esriUpgradeFgdcCsdgm));
      #endregion

    }

    #region ProSnippet Group: Project Units
    #endregion

    public void ProjectUnits1()
    {
      // cref: ArcGIS.Desktop.Core.UnitFormats.UnitFormatType
      // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormats
      // cref: ArcGIS.Desktop.Core.UnitFormats.IDisplayUnitFormats.GetPredefinedProjectUnitFormats
      #region Get The Full List of All Available Unit Formats

      //Must be on the QueuedTask.Run()

      var unit_formats = Enum.GetValues(typeof(UnitFormatType))
                            .OfType<UnitFormatType>().ToList();
      System.Diagnostics.Debug.WriteLine("All available units\r\n");

      foreach (var unit_format in unit_formats)
      {
        var units = DisplayUnitFormats.Instance.GetPredefinedProjectUnitFormats(unit_format);
        System.Diagnostics.Debug.WriteLine(unit_format.ToString());

        foreach (var display_unit_format in units)
        {
          var line = $"{display_unit_format.DisplayName}, {display_unit_format.UnitCode}";
          System.Diagnostics.Debug.WriteLine(line);
        }
        System.Diagnostics.Debug.WriteLine("");
      }
      #endregion
    }

    public void ProjectUnits2()
    {
      // cref: ArcGIS.Desktop.Core.UnitFormats.UnitFormatType
      // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormats
      // cref: ArcGIS.Desktop.Core.UnitFormats.IDisplayUnitFormats.GetProjectUnitFormats
      #region Get The List of Unit Formats for the Current Project

      //Must be on the QueuedTask.Run()

      var unit_formats = Enum.GetValues(typeof(UnitFormatType))
                            .OfType<UnitFormatType>().ToList();
      System.Diagnostics.Debug.WriteLine("Project units\r\n");

      foreach (var unit_format in unit_formats)
      {
        var units = DisplayUnitFormats.Instance.GetProjectUnitFormats(unit_format);
        System.Diagnostics.Debug.WriteLine(unit_format.ToString());

        foreach (var display_unit_format in units)
        {
          var line = $"{display_unit_format.DisplayName}, {display_unit_format.UnitCode}";
          System.Diagnostics.Debug.WriteLine(line);
        }
        System.Diagnostics.Debug.WriteLine("");
      }
      #endregion
    }

    public void ProjectUnits3()
    {
      // cref: ArcGIS.Desktop.Core.UnitFormats.UnitFormatType
      // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormats
      // cref: ArcGIS.Desktop.Core.UnitFormats.IDisplayUnitFormats.GetProjectUnitFormats
      #region Get A Specific List of Unit Formats for the Current Project

      //Must be on the QueuedTask.Run()

      //UnitFormatType.Angular, UnitFormatType.Area, UnitFormatType.Distance, 
      //UnitFormatType.Direction, UnitFormatType.Location, UnitFormatType.Page
      //UnitFormatType.Symbol2D, UnitFormatType.Symbol3D
      var units = DisplayUnitFormats.Instance.GetProjectUnitFormats(UnitFormatType.Distance);

      #endregion
    }

    public void ProjectUnits4()
    {
      // cref: ArcGIS.Desktop.Core.UnitFormats.UnitFormatType
      // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormats
      // cref: ArcGIS.Desktop.Core.UnitFormats.IDisplayUnitFormats.GetDefaultProjectUnitFormat
      #region Get The List of Default Formats for the Current Project

      //Must be on the QueuedTask.Run()

      var unit_formats = Enum.GetValues(typeof(UnitFormatType))
                            .OfType<UnitFormatType>().ToList();
      System.Diagnostics.Debug.WriteLine("Default project units\r\n");

      foreach (var unit_format in unit_formats)
      {
        var default_unit = DisplayUnitFormats.Instance.GetDefaultProjectUnitFormat(unit_format);
        var line = $"{unit_format.ToString()}: {default_unit.DisplayName}, {default_unit.UnitCode}";
        System.Diagnostics.Debug.WriteLine(line);
      }
      System.Diagnostics.Debug.WriteLine("");

      #endregion
    }

    public void ProjectUnits5()
    {
      // cref: ArcGIS.Desktop.Core.UnitFormats.UnitFormatType
      // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormats
      // cref: ArcGIS.Desktop.Core.UnitFormats.IDisplayUnitFormats.GetDefaultProjectUnitFormat
      #region Get A Specific Default Unit Format for the Current Project

      //Must be on the QueuedTask.Run()

      //UnitFormatType.Angular, UnitFormatType.Area, UnitFormatType.Distance, 
      //UnitFormatType.Direction, UnitFormatType.Location, UnitFormatType.Page
      //UnitFormatType.Symbol2D, UnitFormatType.Symbol3D
      var default_unit = DisplayUnitFormats.Instance.GetDefaultProjectUnitFormat(
                                                           UnitFormatType.Distance);

      #endregion
    }

    public void ProjectUnits6()
    {
      // cref: ArcGIS.Desktop.Core.UnitFormats.UnitFormatType
      // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormats
      // cref: ArcGIS.Desktop.Core.UnitFormats.IDisplayUnitFormats.GetPredefinedProjectUnitFormats
      // cref: ArcGIS.Desktop.Core.UnitFormats.IDisplayUnitFormats.SetProjectUnitFormats
      #region Set a Specific List of Unit Formats for the Current Project

      //Must be on the QueuedTask.Run()

      //UnitFormatType.Angular, UnitFormatType.Area, UnitFormatType.Distance, 
      //UnitFormatType.Direction, UnitFormatType.Location

      //Get the full list of all available location units
      var all_units = DisplayUnitFormats.Instance.GetPredefinedProjectUnitFormats(
                                                            UnitFormatType.Location);
      //keep units with an even factory code
      var list_units = all_units.Where(du => du.UnitCode % 2 == 0).ToList();

      //set them as the new location unit collection. A new default is not being specified...
      DisplayUnitFormats.Instance.SetProjectUnitFormats(list_units);

      //set them as the new location unit collection along with a new default
      DisplayUnitFormats.Instance.SetProjectUnitFormats(
                                              list_units, list_units.First());

      //Note: UnitFormatType.Page, UnitFormatType.Symbol2D, UnitFormatType.Symbol3D
      //cannot be set.
      #endregion
    }

    public void ProjectUnits7()
    {
      // cref: ArcGIS.Desktop.Core.UnitFormats.UnitFormatType
      // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormats
      // cref: ArcGIS.Desktop.Core.UnitFormats.IDisplayUnitFormats.GetDefaultProjectUnitFormat
      // cref: ArcGIS.Desktop.Core.UnitFormats.IDisplayUnitFormats.SetDefaultProjectUnitFormat

      #region Set the Defaults for the Project Unit Formats

      //Must be on the QueuedTask.Run()

      var unit_formats = Enum.GetValues(typeof(UnitFormatType)).OfType<UnitFormatType>().ToList();
      foreach (var unit_type in unit_formats)
      {
        var current_default = DisplayUnitFormats.Instance.GetDefaultProjectUnitFormat(unit_type);
        //Arbitrarily pick the last unit in each unit format list
        var replacement = DisplayUnitFormats.Instance.GetProjectUnitFormats(unit_type).Last();
        DisplayUnitFormats.Instance.SetDefaultProjectUnitFormat(replacement);

        var line = $"{current_default.DisplayName}, {current_default.UnitName}, {current_default.UnitCode}";
        var line2 = $"{replacement.DisplayName}, {replacement.UnitName}, {replacement.UnitCode}";

        System.Diagnostics.Debug.WriteLine($"Format: {unit_type.ToString()}");
        System.Diagnostics.Debug.WriteLine($" Current default: {line}");
        System.Diagnostics.Debug.WriteLine($" Replacement default: {line2}");
      }

      #endregion
    }

    public void Project8()
    {
      // cref: ArcGIS.Desktop.Core.UnitFormats.UnitFormatType
      // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormats
      // cref: ArcGIS.Desktop.Core.UnitFormats.IDisplayUnitFormats.GetProjectUnitFormats
      // cref: ArcGIS.Desktop.Core.UnitFormats.IDisplayUnitFormats.SetProjectUnitFormats
      #region Update Unit Formats for the Project

      //UnitFormatType.Angular, UnitFormatType.Area, UnitFormatType.Distance, 
      //UnitFormatType.Direction, UnitFormatType.Location
      var angle_units = DisplayUnitFormats.Instance.GetProjectUnitFormats(UnitFormatType.Angular);

      //Edit the display name of each unit - append the abbreviation
      foreach (var unit in angle_units)
      {
        unit.DisplayName = $"{unit.DisplayName} ({unit.Abbreviation})";
      }
      //apply the changes to the units and set the default to be the first entry
      DisplayUnitFormats.Instance.SetProjectUnitFormats(angle_units, angle_units.First());

      //The project must be saved to persist the changes...
      #endregion

    }

    #region ProSnippet Group: Application Options 
    #endregion

    public void GeneralOptions1()
    {
      // cref: ArcGIS.Desktop.Core.GeneralOptions
      // cref: ArcGIS.Desktop.Core.GeneralOptions.StartupOption
      // cref: ArcGIS.Desktop.Core.GeneralOptions.StartupProjectPath
      // cref: ArcGIS.Desktop.Core.GeneralOptions.HomeFolderOption
      // cref: ArcGIS.Desktop.Core.GeneralOptions.CustomHomeFolder
      // cref: ArcGIS.Desktop.Core.GeneralOptions.DefaultGeodatabaseOption
      // cref: ArcGIS.Desktop.Core.GeneralOptions.CustomDefaultGeodatabase
      // cref: ArcGIS.Desktop.Core.GeneralOptions.DefaultToolboxOption
      // cref: ArcGIS.Desktop.Core.GeneralOptions.CustomDefaultToolbox
      // cref: ArcGIS.Desktop.Core.GeneralOptions.ProjectCreateInFolder
      #region Get GeneralOptions

      var startMode = ApplicationOptions.GeneralOptions.StartupOption;
      var aprx_path = ApplicationOptions.GeneralOptions.StartupProjectPath;

      var hf_option = ApplicationOptions.GeneralOptions.HomeFolderOption;
      var folder = ApplicationOptions.GeneralOptions.CustomHomeFolder;

      var gdb_option = ApplicationOptions.GeneralOptions.DefaultGeodatabaseOption;
      var def_gdb = ApplicationOptions.GeneralOptions.CustomDefaultGeodatabase;

      var tbx_option = ApplicationOptions.GeneralOptions.DefaultToolboxOption;
      var def_tbx = ApplicationOptions.GeneralOptions.CustomDefaultToolbox;

      var create_in_folder = ApplicationOptions.GeneralOptions.ProjectCreateInFolder;

      #endregion

      // cref: ArcGIS.Desktop.Core.GeneralOptions
      // cref: ArcGIS.Desktop.Core.GeneralOptions.StartupOption
      // cref: ArcGIS.Desktop.Core.GeneralOptions.StartupProjectPath
      // cref: ArcGIS.Desktop.Core.GeneralOptions.HomeFolderOption
      // cref: ArcGIS.Desktop.Core.GeneralOptions.CustomHomeFolder
      // cref: ArcGIS.Desktop.Core.GeneralOptions.DefaultGeodatabaseOption
      // cref: ArcGIS.Desktop.Core.GeneralOptions.CustomDefaultGeodatabase
      // cref: ArcGIS.Desktop.Core.GeneralOptions.DefaultToolboxOption
      // cref: ArcGIS.Desktop.Core.GeneralOptions.CustomDefaultToolbox
      // cref: ArcGIS.Desktop.Core.GeneralOptions.ProjectCreateInFolder
      #region Set GeneralOptions to Use Custom Settings

      //Set the application to use a custom project, home folder, gdb, and toolbox
      //In each case, the custom _path_ must be set _first_ before 
      //setting the "option". This ensures the application remains 
      //in a consistent state. This is the same behavior as on the Pro UI.
      if (string.IsNullOrEmpty(ApplicationOptions.GeneralOptions.StartupProjectPath))
        ApplicationOptions.GeneralOptions.StartupProjectPath = @"D:\data\usa.aprx";//custom project path first
      ApplicationOptions.GeneralOptions.StartupOption = StartProjectMode.WithDefaultProject;//option to use it second

      if (string.IsNullOrEmpty(ApplicationOptions.GeneralOptions.CustomHomeFolder))
        ApplicationOptions.GeneralOptions.CustomHomeFolder = @"D:\home_folder";//custom home folder first
      ApplicationOptions.GeneralOptions.HomeFolderOption = OptionSetting.UseCustom;//option to use it second

      if (string.IsNullOrEmpty(ApplicationOptions.GeneralOptions.CustomDefaultGeodatabase))
        ApplicationOptions.GeneralOptions.CustomDefaultGeodatabase = @"D:\data\usa.gdb";//custom gdb path first
      ApplicationOptions.GeneralOptions.DefaultGeodatabaseOption = OptionSetting.UseCustom;//option to use it second

      if (string.IsNullOrEmpty(ApplicationOptions.GeneralOptions.CustomDefaultToolbox))
        ApplicationOptions.GeneralOptions.CustomDefaultToolbox = @"D:\data\usa.tbx";//custom toolbox path first
      ApplicationOptions.GeneralOptions.DefaultToolboxOption = OptionSetting.UseCustom;//option to use it second

      #endregion


      // cref: ArcGIS.Desktop.Core.GeneralOptions
      // cref: ArcGIS.Desktop.Core.GeneralOptions.StartupOption
      // cref: ArcGIS.Desktop.Core.GeneralOptions.StartupProjectPath
      // cref: ArcGIS.Desktop.Core.GeneralOptions.HomeFolderOption
      // cref: ArcGIS.Desktop.Core.GeneralOptions.CustomHomeFolder
      // cref: ArcGIS.Desktop.Core.GeneralOptions.DefaultGeodatabaseOption
      // cref: ArcGIS.Desktop.Core.GeneralOptions.CustomDefaultGeodatabase
      // cref: ArcGIS.Desktop.Core.GeneralOptions.DefaultToolboxOption
      // cref: ArcGIS.Desktop.Core.GeneralOptions.CustomDefaultToolbox
      // cref: ArcGIS.Desktop.Core.GeneralOptions.ProjectCreateInFolder
      // cref: ArcGIS.Desktop.Core.StartProjectMode
      // cref: ArcGIS.Desktop.Core.OptionSetting
      #region Set GeneralOptions to Use Defaults

      //Default options can be set regardless of the value of the "companion"
      //path (to a project, folder, gdb, toolbox, etc.). The path value is ignored if
      //the option setting does not use it. This is the same behavior as on the Pro UI.
      ApplicationOptions.GeneralOptions.StartupOption = StartProjectMode.ShowStartPage;
      ApplicationOptions.GeneralOptions.HomeFolderOption = OptionSetting.UseDefault;
      ApplicationOptions.GeneralOptions.DefaultGeodatabaseOption = OptionSetting.UseDefault;
      ApplicationOptions.GeneralOptions.DefaultToolboxOption = OptionSetting.UseDefault;//set default option first

      //path values can (optionally) be set (back) to null if their 
      //"companion" option setting is the default option.
      if (ApplicationOptions.GeneralOptions.StartupOption != StartProjectMode.WithDefaultProject)
        ApplicationOptions.GeneralOptions.StartupProjectPath = null;
      if (ApplicationOptions.GeneralOptions.HomeFolderOption == OptionSetting.UseDefault)
        ApplicationOptions.GeneralOptions.CustomHomeFolder = null;
      if (ApplicationOptions.GeneralOptions.DefaultGeodatabaseOption == OptionSetting.UseDefault)
        ApplicationOptions.GeneralOptions.CustomDefaultGeodatabase = null;
      if (ApplicationOptions.GeneralOptions.DefaultToolboxOption == OptionSetting.UseDefault)
        ApplicationOptions.GeneralOptions.CustomDefaultToolbox = null;

      #endregion
    }

    public void DownloadOptions1()
    {
      // cref: ArcGIS.Desktop.Core.ApplicationOptions.DownloadOptions
      // cref: ArcGIS.Desktop.Core.DownloadOptions
      #region Get DownloadOptions

      var staging = ApplicationOptions.DownloadOptions.StagingLocation;

      var ppkx_loc = ApplicationOptions.DownloadOptions.UnpackPPKXLocation;
      var ask_ppkx_loc = ApplicationOptions.DownloadOptions.AskForUnpackPPKXLocation;

      var other_loc = ApplicationOptions.DownloadOptions.UnpackOtherLocation;
      var ask_other_loc = ApplicationOptions.DownloadOptions.AskForUnpackOtherLocation;
      var use_proj_folder = ApplicationOptions.DownloadOptions.UnpackOtherToProjectLocation;

      var offline_loc = ApplicationOptions.DownloadOptions.OfflineMapsLocation;
      var ask_offline_loc = ApplicationOptions.DownloadOptions.AskForOfflineMapsLocation;
      var use_proj_folder_offline = ApplicationOptions.DownloadOptions.OfflineMapsToProjectLocation;

      #endregion

      // cref: ArcGIS.Desktop.Core.DownloadOptions
      // cref: ArcGIS.Desktop.Core.DownloadOptions.StagingLocation
      #region Set Staging Location for Sharing and Publishing

      ApplicationOptions.DownloadOptions.StagingLocation = @"D:\data\staging";

      #endregion

      // cref: ArcGIS.Desktop.Core.DownloadOptions
      // cref: ArcGIS.Desktop.Core.DownloadOptions.AskForUnpackPPKXLocation
      #region Set DownloadOptions for PPKX
      //Options are mutually exclusive.

      //Setting ApplicationOptions.DownloadOptions.AskForUnpackPPKXLocation = true
      //supersedes any value in ApplicationOptions.DownloadOptions.UnpackPPKXLocation
      //and will prompt the user on an unpack. The value of 
      //ApplicationOptions.DownloadOptions.UnpackPPKXLocation will be unaffected
      //and is ignored. This is the same behavior as on the Pro UI.
      ApplicationOptions.DownloadOptions.AskForUnpackPPKXLocation = true;//override location

      //The default location is typically <My Documents>\ArcGIS\Packages
      //Setting ApplicationOptions.DownloadOptions.UnpackPPKXLocation to any
      //location overrides ApplicationOptions.DownloadOptions.AskForUnpackPPKXLocation
      //and sets it to false. This is the same behavior as on the Pro UI.
      ApplicationOptions.DownloadOptions.UnpackPPKXLocation = @"D:\data\for_ppkx";

      //Or, if ApplicationOptions.DownloadOptions.UnpackPPKXLocation already
      //contains a valid path, set ApplicationOptions.DownloadOptions.AskForUnpackPPKXLocation
      //explicitly to false to use the UnpackPPKXLocation
      if (!string.IsNullOrEmpty(ApplicationOptions.DownloadOptions.UnpackPPKXLocation))
        ApplicationOptions.DownloadOptions.AskForUnpackPPKXLocation = false;

      #endregion

      // cref: ArcGIS.Desktop.Core.DownloadOptions
      // cref: ArcGIS.Desktop.Core.DownloadOptions.UnpackOtherLocation
      #region Set DownloadOptions for UnpackOther
      //UnpackOther settings control unpacking of anything _other than_
      //a ppkx or aptx. Options are mutually exclusive.

      //Set ApplicationOptions.DownloadOptions.UnpackOtherLocation explicitly to
      //toggle ApplicationOptions.DownloadOptions.AskForUnpackOtherLocation and
      //ApplicationOptions.DownloadOptions.UnpackOtherToProjectLocation to false
      //Note: default is typically <My Documents>\ArcGIS\Packages, _not_ null.
      //This is the same behavior as on the Pro UI.
      ApplicationOptions.DownloadOptions.UnpackOtherLocation = @"D:\data\for_other";

      //or...to use a location already stored in UnpackOtherLocation as the
      //default without changing it, 
      //set ApplicationOptions.DownloadOptions.AskForUnpackOtherLocation and
      //ApplicationOptions.DownloadOptions.UnpackOtherToProjectLocation to false
      //explicitly. This is the same behavior as on the Pro UI.
      if (!string.IsNullOrEmpty(ApplicationOptions.DownloadOptions.UnpackOtherLocation))
      {
        ApplicationOptions.DownloadOptions.AskForUnpackOtherLocation = false;
        ApplicationOptions.DownloadOptions.UnpackOtherToProjectLocation = false;
      }

      //Setting ApplicationOptions.DownloadOptions.AskForUnpackOtherLocation to
      //true overrides any UnpackOtherLocation value and sets 
      //ApplicationOptions.DownloadOptions.UnpackOtherToProjectLocation to false.
      //This is the same behavior as on the Pro UI.
      ApplicationOptions.DownloadOptions.AskForUnpackOtherLocation = true;

      //Setting ApplicationOptions.DownloadOptions.UnpackOtherToProjectLocation to
      //true overrides any UnpackOtherLocation value and sets 
      //ApplicationOptions.DownloadOptions.AskForUnpackOtherLocation to false.
      //This is the same behavior as on the Pro UI.
      ApplicationOptions.DownloadOptions.UnpackOtherToProjectLocation = false;

      #endregion

      // cref: ArcGIS.Desktop.Core.DownloadOptions
      // cref: ArcGIS.Desktop.Core.DownloadOptions.OfflineMapsLocation
      #region Set DownloadOptions for OfflineMaps
      //OfflineMaps settings control where map content that is taken
      //offline is copied to on the local machine. Options are mutually exclusive.

      //Set ApplicationOptions.DownloadOptions.OfflineMapsLocation explicitly to
      //toggle ApplicationOptions.DownloadOptions.AskForOfflineMapsLocation and
      //ApplicationOptions.DownloadOptions.OfflineMapsToProjectLocation to false
      //Note: default is typically <My Documents>\ArcGIS\OfflineMaps, _not_ null.
      //This is the same behavior as on the Pro UI.
      ApplicationOptions.DownloadOptions.OfflineMapsLocation = @"D:\data\for_offline";

      //or...to use a location already stored in OfflineMapsLocation as the
      //default without changing it, 
      //set ApplicationOptions.DownloadOptions.AskForOfflineMapsLocation and
      //ApplicationOptions.DownloadOptions.OfflineMapsToProjectLocation to false
      //explicitly.
      if (!string.IsNullOrEmpty(ApplicationOptions.DownloadOptions.OfflineMapsLocation))
      {
        ApplicationOptions.DownloadOptions.AskForOfflineMapsLocation = false;
        ApplicationOptions.DownloadOptions.OfflineMapsToProjectLocation = false;
      }

      //Setting ApplicationOptions.DownloadOptions.AskForOfflineMapsLocation to
      //true overrides any OfflineMapsLocation value and sets 
      //ApplicationOptions.DownloadOptions.OfflineMapsToProjectLocation to false.
      //This is the same behavior as on the Pro UI.
      ApplicationOptions.DownloadOptions.AskForOfflineMapsLocation = true;

      //Setting ApplicationOptions.DownloadOptions.OfflineMapsToProjectLocation to
      //true overrides any OfflineMapsLocation value and sets 
      //ApplicationOptions.DownloadOptions.AskForOfflineMapsLocation to false.
      //This is the same behavior as on the Pro UI.
      ApplicationOptions.DownloadOptions.OfflineMapsToProjectLocation = true;

      #endregion

    }

    public void PortalProjectOptions()
    {
      // cref: ArcGIS.Desktop.Core.GeneralOptions
      // cref: ArcGIS.Desktop.Core.GeneralOptions.PortalProjectCustomHomeFolder
      // cref: ArcGIS.Desktop.Core.GeneralOptions.PortalProjectCustomDefaultGeodatabase
      // cref: ArcGIS.Desktop.Core.GeneralOptions.PortalProjectCustomDefaultToolbox
      // cref: ArcGIS.Desktop.Core.GeneralOptions.PortalProjectDeleteLocalCopyOnClose
      // cref: ArcGIS.Desktop.Core.GeneralOptions.PortalProjectDownloadLocation
      #region Portal Project Options

      // access the current options
      var def_home = ApplicationOptions.GeneralOptions.PortalProjectCustomHomeFolder;
      var def_gdb = ApplicationOptions.GeneralOptions.PortalProjectCustomDefaultGeodatabase;
      var def_tbx = ApplicationOptions.GeneralOptions.PortalProjectCustomDefaultToolbox;
      var deleteOnClose = ApplicationOptions.GeneralOptions.PortalProjectDeleteLocalCopyOnClose;
      var def_location = ApplicationOptions.GeneralOptions.PortalProjectDownloadLocation;


      // set the options
      ApplicationOptions.GeneralOptions.PortalProjectCustomHomeFolder = @"E:\data";
      ApplicationOptions.GeneralOptions.PortalProjectCustomDefaultGeodatabase = @"E:\data\usa.gdb";
      ApplicationOptions.GeneralOptions.PortalProjectCustomDefaultToolbox = @"E:\data\usa.tbx";
      ApplicationOptions.GeneralOptions.PortalProjectDeleteLocalCopyOnClose = false;
      ApplicationOptions.GeneralOptions.PortalProjectDownloadLocation = @"E:\data";
      #endregion
    }
  }
}
