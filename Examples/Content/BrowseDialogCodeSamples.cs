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

namespace ArcGIS.Desktop.Catalog.ApiTests.CodeSamples
{
  class BrowseDialogCodeSamples
  {
    /// <summary>
    /// Code Samples for Browse Dialog
    /// </summary>
    public void BrowseDialogCodeSample()
    {
      // Variables not used in samples
      OpenItemDialog selectItemDialog = new OpenItemDialog(); // in #region BrowseDialogItems

      // cref: OpenItemDialog;ArcGIS.Desktop.Catalog.ItemDialog.Filter
      // cref: OpenItemDialog;ArcGIS.Desktop.Catalog.ItemDialog.InitialLocation
      // cref: OpenItemDialog;ArcGIS.Desktop.Catalog.ItemDialog.Title
      // cref: OpenItemDialog;ArcGIS.Desktop.Catalog.OpenItemDialog
      #region OpenItemDialog

      /// Adds a single item to a map
      OpenItemDialog addToMapDialog = new OpenItemDialog()
      {
        Title = "Add To Map",
        InitialLocation = @"C:\Data\NewYork\Counties\Erie\Streets",
        Filter = ItemFilters.Composite_AddToMap
      };

      #endregion //OpenItemDialog

      // cref: Show_OpenItemDialog;ArcGIS.Desktop.Catalog.OpenItemDialog.ShowDialog
      // cref: Show_OpenItemDialog;ArcGIS.Desktop.Catalog.SaveItemDialog.ShowDialog
      // cref: Show_OpenItemDialog;ArcGIS.Desktop.Catalog.OpenItemDialog.Items
      // cref: Show_OpenItemDialog;ArcGIS.Desktop.Catalog.OpenItemDialog.MultiSelect
      #region Show_OpenItemDialog

      OpenItemDialog addToProjectDialog = new OpenItemDialog();
      addToProjectDialog.Title = "Add To Project";
      addToProjectDialog.InitialLocation = @"C:\Data\NewYork\Counties\Maps";
      addToProjectDialog.MultiSelect = true;
      addToProjectDialog.Filter = ItemFilters.Composite_Maps_Import;

      bool? ok = addToMapDialog.ShowDialog();

      if (ok == true)
      {
        IEnumerable<Item> selectedItems = addToMapDialog.Items;
        foreach (Item selectedItem in selectedItems)
          MapFactory.Instance.CreateMapFromItem(selectedItem);
      }

      #endregion //Show_OpenItemDialog

      // cref: SaveItemDialog;ArcGIS.Desktop.Catalog.ItemDialog.Filter
      // cref: SaveItemDialog;ArcGIS.Desktop.Catalog.ItemDialog.InitialLocation
      // cref: SaveItemDialog;ArcGIS.Desktop.Catalog.ItemDialog.Title
      // cref: SaveItemDialog;ArcGIS.Desktop.Catalog.SaveItemDialog
      #region SaveItemDialog

      SaveItemDialog saveLayerFileDialog = new SaveItemDialog()
      {
        Title = "Save Layer File",
        InitialLocation = @"C:\Data\ProLayers\Geographic\Streets",
        Filter = ItemFilters.Files_All
      };

      #endregion //SaveItemDialog

      // cref: Show_SaveItemDialog;ArcGIS.Desktop.Catalog.SaveItemDialog.DefaultExt
      // cref: Show_SaveItemDialog;ArcGIS.Desktop.Catalog.SaveItemDialog.FilePath
      // cref: Show_SaveItemDialog;ArcGIS.Desktop.Catalog.SaveItemDialog.OverwritePrompt
      #region Show_SaveItemDialog

      SaveItemDialog saveMapFileDialog = new SaveItemDialog()
      {
        Title = "Save Map File",
        InitialLocation = @"C:\Data\NewYork\Counties\Maps",
        DefaultExt = @"mapx",
        Filter = ItemFilters.Maps_All,
        OverwritePrompt = true
      };
      bool? result = saveMapFileDialog.ShowDialog();

      if (result == true)
      {
        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Returned file name: " + saveMapFileDialog.FilePath);
      }

      #endregion //Show_SaveItemDialog

      // cref: ArcGIS.Desktop.Catalog.OpenItemDialog.Items
      // cref: ArcGIS.Desktop.Catalog.OpenItemDialog
      // cref: BrowseDialogItems;ArcGIS.Desktop.Mapping.MapFactory.CreateMapFromItem
      #region BrowseDialogItems

      IEnumerable<Item> selectedDialogItems = selectItemDialog.Items;
      foreach (Item selectedDialogItem in selectedDialogItems)
        Mapping.MapFactory.Instance.CreateMapFromItem(selectedDialogItem);

      #endregion //BrowseDialogItems

    }

  }
}
