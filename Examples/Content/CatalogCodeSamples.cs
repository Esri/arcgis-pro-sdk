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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Internal.Core;

namespace ArcGIS.Desktop.Catalog.ApiTests.CodeSamples
{
  class CatalogCodeSamples
  {

    /// <summary>
    /// Sample code for ProjectItem
    /// </summary>
    public async Task ProjectItemExamples()
    {
      // not to be included in sample regions
      var projectFolderConnection = (Project.Current.GetItems<FolderConnectionProjectItem>()).First();

      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      // cref: ArcGIS.Desktop.Mapping.MapProjectItem
      #region Get MapProjectItems

      /// Get all the maps in a project
      IEnumerable<MapProjectItem> projectMaps = Project.Current.GetItems<MapProjectItem>();

      #endregion //GetMapProjectItems

      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      // cref: ArcGIS.Desktop.Catalog.FolderConnectionProjectItem
      #region Get FolderConnectionProjectItems

      /// Get all the folder connections in a project
      IEnumerable<FolderConnectionProjectItem> projectFolders = Project.Current.GetItems<FolderConnectionProjectItem>();

      #endregion //GetFolderConnectiontProjectItems

      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      // cref: ArcGIS.Desktop.Catalog.ServerConnectionProjectItem
      #region Get ServerConnectionProjectItems

      /// Get all the server connections in a project
      IEnumerable<ServerConnectionProjectItem> projectServers = Project.Current.GetItems<ServerConnectionProjectItem>();

      #endregion //Get ServerConnectionProjectItems

      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      // cref: ArcGIS.Desktop.Catalog.LocatorsConnectionProjectItem
      #region Get LocatorConnectionProjectItems

      /// Get all the locator connections in a project
      IEnumerable<LocatorsConnectionProjectItem> projectLocators = Project.Current.GetItems<LocatorsConnectionProjectItem>();

      #endregion //Get LocatorConnectionProjectItems

      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      // cref: ArcGIS.Desktop.Catalog.FolderConnectionProjectItem
      // cref: ArcGIS.Desktop.Core.Item.GetItems
      #region Get Project Items by ProjectItem type

      /// Get all the items that can be accessed from a folder connection. The items immediately 
      /// contained by a folder, that is, the folder's children, are returned including folders
      /// and individual items that can be used in ArcGIS Pro. This method does not return all 
      /// items contained by any sub-folder that can be accessed from the folder connection.
      FolderConnectionProjectItem folderConnection = Project.Current.GetItems<FolderConnectionProjectItem>()
                                                          .FirstOrDefault((folder => folder.Name.Equals("Data")));
      await QueuedTask.Run(() =>
      {
        IEnumerable<Item> folderContents = folderConnection.GetItems();
      });
      #endregion //GetProjectItems

      // cref: ArcGIS.Desktop.Core.Project.AddItem(ArcGIS.Desktop.Core.IProjectItem)
      #region Add Folder to Project as IProjectItem

      /// Add a folder connection to a project
      Item folderToAdd = ItemFactory.Instance.Create(@"C:\Data\Oregon\Counties\Streets");
      bool wasAdded = await QueuedTask.Run(() => Project.Current.AddItem(folderToAdd as IProjectItem));

      #endregion //AddFolderConnectionProjectItem

      // cref: ArcGIS.Desktop.Catalog.GDBProjectItem
      // cref: ArcGIS.Desktop.Core.Project.AddItem(ArcGIS.Desktop.Core.IProjectItem)
      #region Add GDBProjectItem to Project as IProjectItem

      /// Add a file geodatabase or a SQLite or enterprise database connection to a project
      Item gdbToAdd = folderToAdd.GetItems().FirstOrDefault(folderItem => folderItem.Name.Equals("CountyData.gdb"));
      var addedGeodatabase = await QueuedTask.Run(() => Project.Current.AddItem(gdbToAdd as IProjectItem));

      #endregion //AddGDBProjectItem

      // cref: ArcGIS.Desktop.Catalog.FolderConnectionProjectItem
      // cref: ArcGIS.Desktop.Core.Project.RemoveItem
      #region Remove FolderConnection From Project

      /// Remove a folder connection from a project; the folder stored on the local disk 
      /// or the network is not deleted
      FolderConnectionProjectItem folderToRemove = Project.Current.GetItems<FolderConnectionProjectItem>().FirstOrDefault(folder => folder.Name.Equals("PlantSpecies"));
      if (folderToRemove != null)
        Project.Current.RemoveItem(folderToRemove as IProjectItem);

      #endregion //RemoveFolderConnectionFromProject

      // cref: ArcGIS.Desktop.Mapping.MapProjectItem
      // cref: ArcGIS.Desktop.Core.Project.RemoveItem
      #region Remove Map From Project

      /// Remove a map from a project; the map is deleted
      IProjectItem mapToRemove = Project.Current.GetItems<MapProjectItem>().FirstOrDefault(map => map.Name.Equals("OldStreetRoutes"));
      var removedMapProjectItem = await QueuedTask.Run(
               () => Project.Current.RemoveItem(mapToRemove));

      #endregion //RemoveMapFromProject

      // cref: ArcGIS.Desktop.Core.ItemFactory
      // cref: ArcGIS.Desktop.Core.ItemFactory.Create
      // cref: ArcGIS.Desktop.Core.ItemFactory.Create
      // cref: ArcGIS.Desktop.Core.Project.AddItem(ArcGIS.Desktop.Core.IProjectItem)
      #region Importing Maps To Project

      /// Import a mxd
      Item mxdToImport = ItemFactory.Instance.Create(@"C:\Projects\RegionalSurvey\LatestResults.mxd");
      var addedMxd = await QueuedTask.Run(
                    () => Project.Current.AddItem(mxdToImport as IProjectItem));

      /// Add map package      
      Item mapPackageToAdd = ItemFactory.Instance.Create(@"c:\Data\Map.mpkx");
      var addedMapPackage = await QueuedTask.Run(
                    () => Project.Current.AddItem(mapPackageToAdd as IProjectItem));

      /// Add an exported Pro map
      Item proMapToAdd = ItemFactory.Instance.Create(@"C:\ExportedMaps\Election\Districts.mapx");
      var addedMapProjectItem = await QueuedTask.Run(
                    () => Project.Current.AddItem(proMapToAdd as IProjectItem));

      #endregion //ImportToProject

    }

    /// <summary>
    /// Sample Code for Item.
    /// </summary>
    public async Task ItemExamples()
    {
      // not to be included in sample regions
      var projectfolderConnection = (Project.Current.GetItems<FolderConnectionProjectItem>()).First();

      // cref: ArcGIS.Desktop.Core.ItemFactory.Create
      // cref: ArcGIS.Desktop.Core.Item
      #region Create An Item

      Item mxdItem = ItemFactory.Instance.Create(@"C:\Projects\RegionalSurvey\LatestResults.mxd");

      #endregion //CreateAnItem

      // cref: ArcGIS.Desktop.Core.ItemFactory.Create
      // cref: ArcGIS.Desktop.Core.ItemFactory.ItemType
      // cref: ArcGIS.Desktop.Core.Portal.PortalItem
      #region Create A PortalItem

      // Creates an Item from an existing portal item base on its ID
      string portalItemID = "9801f878ff4a22738dff3f039c43e395";
      Item portalItem = ItemFactory.Instance.Create(portalItemID, ItemFactory.ItemType.PortalItem);

      #endregion

      // cref: ArcGIS.Desktop.Core.ItemFactory.Create
      // cref: ArcGIS.Desktop.Core.ItemFactory.ItemType
      // cref: ArcGIS.Desktop.Core.Portal.PortalItem 
      #region Create A PortalFolder

      // Creates an Item from an existing portal folder base on its ID
      string portalFolderID = "39c43e39f878f4a2279838dfff3f0015";
      Item portalFolder = ItemFactory.Instance.Create(portalFolderID, ItemFactory.ItemType.PortalFolderItem);

      #endregion

      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      // cref: ArcGIS.Desktop.Core.Portal.PortalItem 
      #region Get Folder Item Content from Project PortalItem

      var folderConnectionContent = projectfolderConnection.GetItems();
      var folder = folderConnectionContent.FirstOrDefault(folderItem => folderItem.Name.Equals("Tourist Sites"));
      var folderContents = folder.GetItems();

      #endregion //GetItemContent


      #region

      #endregion
    }

    #region

    #endregion

  }
}
