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

namespace ArcGIS.Desktop.Catalog.ApiTests.CodeSamples
{
  class BrowseDialogCodeSamples
  {

    /// <summary>
    /// Code Samples for Browse Dialog
    /// </summary>
    public async Task BrowseDialogCodeSample()
    {
      // Variables not used in samples
      OpenItemDialog selectItemDialog = new OpenItemDialog(); // in #region BrowseDialogItems

      #region OpenItemDialog

      /// Adds a single item to a map
      OpenItemDialog addToMapDialog = new OpenItemDialog()
      {
        Title = "Add To Map",
        InitialLocation = @"C:\Data\NewYork\Counties\Erie\Streets",
        Filter = ItemFilters.composite_addToMap
      };

      #endregion //OpenItemDialog

      #region Show_OpenItemDialog

      OpenItemDialog addToProjectDialog = new OpenItemDialog();
      addToMapDialog.Title = "Add To Project";
      addToMapDialog.InitialLocation = @"C:\Data\NewYork\Counties\Maps";
      addToMapDialog.MultiSelect = true;
      addToMapDialog.Filter = ItemFilters.composite_maps_import;

      bool? ok = addToMapDialog.ShowDialog();

      if (ok == true)
      {
        IEnumerable<Item> selectedItems = addToMapDialog.Items;
        foreach (Item selectedItem in selectedItems)
          Mapping.MapFactory.Instance.CreateMapFromItem(selectedItem);
      }

      #endregion //Show_OpenItemDialog

      #region SaveItemDialog

      SaveItemDialog saveLayerFileDialog = new SaveItemDialog()
      {
        Title = "Save Layer File",
        InitialLocation = @"C:\Data\ProLayers\Geographic\Streets",
        Filter = ItemFilters.layers_allFileTypes
      };

      #endregion //SaveItemDialog

      #region Show_SaveItemDialog

      SaveItemDialog saveMapFileDialog = new SaveItemDialog()
      {
        Title = "Save Map File",
        InitialLocation = @"C:\Data\NewYork\Counties\Maps",
        DefaultExt = @"mapx",
        Filter = ItemFilters.maps_all,
        OverwritePrompt = true
      };
      bool? result = saveMapFileDialog.ShowDialog();

      if (result == true)
      {
        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Returned file name: " + saveMapFileDialog.FilePath);
      }

      #endregion //Show_SaveItemDialog

      #region BrowseDialogItems

      IEnumerable<Item> selectedDialogItems = selectItemDialog.Items;
      foreach (Item selectedDialogItem in selectedDialogItems)
        Mapping.MapFactory.Instance.CreateMapFromItem(selectedDialogItem);

      #endregion //BrowseDialogItems

    }

  }
}
