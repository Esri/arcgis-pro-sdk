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
using ArcGIS.Core.CIM;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System.Threading.Tasks;

namespace Carto_SDK_Examples
{
  class Carto_SDK_Examples
  {
    public async void Examples()
    {
      #region Get symbol from SymbolStyleItem
      SymbolStyleItem symbolItem = null;
      CIMSymbol symbol = await QueuedTask.Run<CIMSymbol>(() =>
      {
        return symbolItem.Symbol;
      });
      #endregion

      #region Get color from ColorStyleItem
      ColorStyleItem colorItem = null;
      CIMColor color = await QueuedTask.Run<CIMColor>(() =>
      {
        return colorItem.Color;
      });
      #endregion

      #region Get color ramp from ColorRampStyleItem
      ColorRampStyleItem colorRampItem = null;
      CIMColorRamp colorRamp = await QueuedTask.Run<CIMColorRamp>(() =>
      {
        return colorRampItem.ColorRamp;
      });
      #endregion

      #region Get north arrow from NorthArrowStyleItem
      NorthArrowStyleItem northArrowItem = null;
      CIMNorthArrow northArrow = await QueuedTask.Run<CIMNorthArrow>(() =>
      {
        return northArrowItem.NorthArrow;
      });
      #endregion

      #region Get scale bar from ScaleBarStyleItem
      ScaleBarStyleItem scaleBarItem = null;
      CIMScaleBar scaleBar = await QueuedTask.Run<CIMScaleBar>(() =>
      {
        return scaleBarItem.ScaleBar;
      });
      #endregion

      #region Get label placement from LabelPlacementStyleItem
      LabelPlacementStyleItem labelPlacementItem = null;
      CIMLabelPlacementProperties labelPlacement = await QueuedTask.Run<CIMLabelPlacementProperties>(() =>
      {
        return labelPlacementItem.LabelPlacement;
      });
      #endregion

      #region Get grid from GridStyleItem
      GridStyleItem gridItem = null;
      CIMMapGrid grid = await QueuedTask.Run<CIMMapGrid>(() =>
      {
        return gridItem.Grid;
      });
      #endregion

      #region Get legend from LegendStyleItem
      LegendStyleItem legendItem = null;
      CIMLegend legend = await QueuedTask.Run<CIMLegend>(() =>
      {
        return legendItem.Legend;
      });
      #endregion

      #region Get table frame from TableFrameStyleItem
      TableFrameStyleItem tableFrameItem = null;
      CIMTableFrame tableFrame = await QueuedTask.Run<CIMTableFrame>(() =>
      {
        return tableFrameItem.TableFrame;
      });
      #endregion

      #region Get map surround from MapSurroundStyleItem
      MapSurroundStyleItem mapSurroundItem = null;
      CIMMapSurround mapSurround = await QueuedTask.Run<CIMMapSurround>(() =>
      {
        return mapSurroundItem.MapSurround;
      });
      #endregion
    }


    #region GetSetObject
    //Creates a new style item and sets its properties from an existing style item
    public Task<StyleItem> CreateNewStyleItemAsync(StyleItem existingItem)
    {
      if (existingItem == null)
        throw new System.ArgumentNullException();

      return QueuedTask.Run(() =>
      {
        StyleItem item = new StyleItem();

        //Get object from existing style item
        object obj = existingItem.GetObject();

        //New style item's properties are set from the existing item
        item.SetObject(obj);

        //Key, Name, Tags and Category for the new style item have to be set
        //These values are NOT populated from the object passed in to SetObject
        item.Key = "KeyOfTheNewItem";
        item.Name = "NameOfTheNewItem";
        item.Tags = "TagsForTheNewItem";
        item.Category = "CategoryOfTheNewItem";

        return item;
      });
    }
    #endregion
  }
}
