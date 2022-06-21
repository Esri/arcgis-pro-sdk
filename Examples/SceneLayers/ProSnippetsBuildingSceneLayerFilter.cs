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
using ArcGIS.Desktop.Framework;

//added references
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Layouts;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Core.CIM;
using ArcGIS.Core.Geometry;
using ArcGIS.Core.Data;

namespace ProSnippetsBuildingSceneLayerFilter
{
  class ProSnippetsBuildingSceneLayerFilter
  {

    #region ProSnippet Group: Building Scene Layer
    #endregion

    public async void Examples()
    {
      // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer
      // cref: ArcGIS.Desktop.Mapping.MapMember.Name
      #region Name of BuildingSceneLayer 
      var bsl = MapView.Active.Map.GetLayersAsFlattenedList()
                        .OfType<BuildingSceneLayer>().FirstOrDefault();
      var scenelayerName = bsl.Name;
      #endregion

      await QueuedTask.Run(() =>
      {
        // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.GetAvailableFieldsAndValues
        #region Query Building Scene Layer for available Types and Values
        //Must be called on the MCT
        //Retrieve the complete set of types and values for the building scene
        //var bsl = ...;
        //At 2.x - var dict = bsl.QueryAvailableFieldsAndValues();
        var dict = bsl.GetAvailableFieldsAndValues();

        //get a list of existing disciplines
        var disciplines = dict.SingleOrDefault(
                 kvp => kvp.Key == "Discipline").Value ?? new List<string>();

        //get a list of existing categories
        var categories = dict.SingleOrDefault(
                 kvp => kvp.Key == "Category").Value ?? new List<string>();

        //get a list of existing floors or "levels"
        var floors = dict.SingleOrDefault(
                 kvp => kvp.Key == "BldgLevel").Value ?? new List<string>();

        #endregion

        // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.CreateDefaultFilter
        // cref: ArcGIS.Desktop.Mapping.FilterDefinition.FilterBlockDefinitions
        // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.GetFilters
        // cref: ArcGIS.Desktop.Mapping.FilterBlockDefinition.SelectedValues
        #region Create a Default Filter and Get Filter Count
        //Must be called on the MCT
        //Creates a default filter on the building scene
        //var bsl = ...;
        var filter1 = bsl.CreateDefaultFilter();
        var values = filter1.FilterBlockDefinitions[0].SelectedValues;
        //values will be a single value for the type
        //"CreatedPhase", value "New Construction"

        //There will be at least one filter after "CreateDefaultFilter()" call
        var filtersCount = bsl.GetFilters().Count;

        #endregion

        // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.GetFilters
        // cref: ArcGIS.Desktop.Mapping.FilterDefinition.FilterBlockDefinitions
        // cref: ArcGIS.Desktop.Mapping.FilterBlockDefinition.FilterBlockMode
        // cref: ArcGIS.Core.CIM.Object3DRenderingMode
        #region Get all the Filters that Contain WireFrame Blocks

        //var bsl = ...;
        //Note: wire_frame_filters can be null in this example
        var wire_frame_filters = bsl.GetFilters().Where(
          f => f.FilterBlockDefinitions.Any(
            fb => fb.FilterBlockMode == Object3DRenderingMode.Wireframe));
        //substitute Object3DRenderingMode.None to get blocks with a solid mode (default)
        //and...
        //fb.FilterBlockMode == Object3DRenderingMode.Wireframe &&
        //fb.FilterBlockMode == Object3DRenderingMode.None
        //for blocks with both

        #endregion

        // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.HasFilter
        // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.SetActiveFilter
        // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.GetActiveFilter
        // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.ClearActiveFilter
        #region Set and Clear Active Filter for BuildingSceneLayer
        //Must be called on the MCT
        //Note: Use HasFilter to check if a given filter id exists in the layer
        //var bsl = ...;
        if (bsl.HasFilter(filter1.ID))
          bsl.SetActiveFilter(filter1.ID);
        var activeFilter = bsl.GetActiveFilter();

        //Clear the active filter
        bsl.ClearActiveFilter();

        #endregion

        // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.GetFilter
        // cref: ArcGIS.Desktop.Mapping.FilterDefinition.ID
        #region Get BuildingSceneLayer Filter ID and Filter     
        string filterID1 = filter1.ID;
        var filter = bsl.GetFilter(filterID1);
        //or via Linq
        //var filter = bsl.GetFilters().FirstOrDefault(f => f.ID == filterID1);

        #endregion

        // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.UpdateFilter
        // cref: ArcGIS.Desktop.Mapping.FilterDefinition.Name
        // cref: ArcGIS.Desktop.Mapping.FilterDefinition.Description
        #region Modify BuildingSceneLayer Filter Name and Description
        //Must be called on the MCT
        //var bsl = ...;
        //var filter1 = bsl.GetFilter(filterID1);

        filter1.Name = "Updated Filter Name";
        filter1.Description = "Updated Filter description";
        //At 2.x - bsl.SetFilter(filter1);
        bsl.UpdateFilter(filter);
        #endregion

        // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.GetAvailableFieldsAndValues
        // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.UpdateFilter
        // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.SetActiveFilter
        // cref: ArcGIS.Desktop.Mapping.FilterDefinition.Name
        // cref: ArcGIS.Desktop.Mapping.FilterDefinition.Description
        // cref: ArcGIS.Desktop.Mapping.FilterDefinition.ID
        // cref: ArcGIS.Desktop.Mapping.FilterDefinition.FilterBlockDefinitions
        // cref: ArcGIS.Desktop.Mapping.FilterBlockDefinition.FilterBlockMode
        // cref: ArcGIS.Desktop.Mapping.FilterBlockDefinition.Title
        // cref: ArcGIS.Desktop.Mapping.FilterBlockDefinition.SelectedValues
        #region Create a Filter using Building Level and Category
        //Must be called on the MCT

        //refer to "Query Building Scene Layer for available Types and Values"
        //...
        //var bsl = ...;
        //At 2.x
        //var dict = bsl.QueryAvailableFieldsAndValues();

        //var dict = bsl.GetAvailableFieldsAndValues();
        //var categories = dict.SingleOrDefault(kvp => kvp.Key == "Category").Value;
        //var floors = dict.SingleOrDefault(kvp => kvp.Key == "BldgLevel").Value;

        //Make a new filter definition
        var fd = new FilterDefinition()
        {
          Name = "Floor and Category Filter",
          Description = "Example filter",
        };
        //Set up the values for the filter
        var filtervals = new Dictionary<string, List<string>>();
        filtervals.Add("BldgLevel", new List<string>() { floors[0] });
        var category_vals = categories.Where(v => v == "Walls" || v == "Stairs").ToList() ?? new List<string>();
        if (category_vals.Count() > 0)
        {
          filtervals.Add("Category", category_vals);
        }
        //Create a solid block (other option is "Wireframe")
        var fdef = new FilterBlockDefinition()
        {
          FilterBlockMode = Object3DRenderingMode.None,
          Title = "Solid Filter",
          SelectedValues = filtervals//Floor and Category
        };
        //Apply the block
        fd.FilterBlockDefinitions = new List<FilterBlockDefinition>() { fdef };
        //Add the filter definition to the layer
        //At 2.x - bsl.SetFilter(fd);
        bsl.UpdateFilter(fd);
        //Set it active. The ID is auto-generated
        bsl.SetActiveFilter(fd.ID);

        #endregion

        // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.UpdateFilter
        // cref: ArcGIS.Desktop.Mapping.FilterDefinition.FilterBlockDefinitions
        // cref: ArcGIS.Desktop.Mapping.FilterBlockDefinition.FilterBlockMode
        // cref: ArcGIS.Desktop.Mapping.FilterBlockDefinition.SelectedValues
        // cref: ArcGIS.Core.CIM.Object3DRenderingMode
        #region Modify BuildingSceneLayer Filter Block
        //Must be called on the MCT
        //Assuming retrieve filter ok
        //var bsl = ...;
        //var filter1 = bsl.GetFilter(...);

        var filterBlock = new FilterBlockDefinition();
        filterBlock.FilterBlockMode = Object3DRenderingMode.Wireframe;
        
        var selectedValues = new Dictionary<string, List<string>>();
        //We assume QueryAvailableFieldsAndValues() contains "Walls" and "Doors"
        //For 'Category'
        selectedValues["Category"] = new List<string>() { "Walls", "Doors" };
        filterBlock.SelectedValues = selectedValues;

        //Overwrite
        filter1.FilterBlockDefinitions = new List<FilterBlockDefinition>() { filterBlock };
        //At 2.x - bsl.SetFilter(filter1);
        bsl.UpdateFilter(filter1);

        #endregion

        // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.HasFilter
        // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.RemoveFilter
        // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.RemoveAllFilters
        // cref: ArcGIS.Desktop.Mapping.FilterDefinition.ID
        #region Remove BuildingSceneLayer Filter
        //var bsl = ...;
        //Note: Use HasFilter to check if a given filter id exists in the layer
        //Must be called on the MCT
        if (bsl.HasFilter(filter1.ID))
          bsl.RemoveFilter(filter1.ID);
        //Or remove all filters
        bsl.RemoveAllFilters();

        #endregion
      });
    }
  }
}