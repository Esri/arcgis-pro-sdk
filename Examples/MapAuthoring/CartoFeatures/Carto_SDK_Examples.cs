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
      // cref: Get symbol from SymbolStyleItem;ArcGIS.Desktop.Mapping.SymbolStyleItem.Symbol
      // cref: Get symbol from SymbolStyleItem;ArcGIS.Desktop.Mapping.SymbolStyleItem.Symbol
      #region Get symbol from SymbolStyleItem
      SymbolStyleItem symbolItem = null;
      CIMSymbol symbol = await QueuedTask.Run<CIMSymbol>(() =>
      {
        return symbolItem.Symbol;
      });
      #endregion

      // cref: Get color from ColorStyleItem;ArcGIS.Desktop.Mapping.ColorStyleItem.Color
      // cref: Get color from ColorStyleItem;ArcGIS.Desktop.Mapping.ColorStyleItem.Color
      #region Get color from ColorStyleItem
      ColorStyleItem colorItem = null;
      CIMColor color = await QueuedTask.Run<CIMColor>(() =>
      {
        return colorItem.Color;
      });
      #endregion

      // cref: Get color ramp from ColorRampStyleItem;ArcGIS.Desktop.Mapping.ColorRampStyleItem.ColorRamp
      // cref: Get color ramp from ColorRampStyleItem;ArcGIS.Desktop.Mapping.ColorRampStyleItem.ColorRamp
      #region Get color ramp from ColorRampStyleItem
      ColorRampStyleItem colorRampItem = null;
      CIMColorRamp colorRamp = await QueuedTask.Run<CIMColorRamp>(() =>
      {
        return colorRampItem.ColorRamp;
      });
      #endregion

      // cref: Get north arrow from NorthArrowStyleItem;ArcGIS.Desktop.Mapping.NorthArrowStyleItem.NorthArrow
      // cref: Get north arrow from NorthArrowStyleItem;ArcGIS.Desktop.Mapping.NorthArrowStyleItem.NorthArrow
      #region Get north arrow from NorthArrowStyleItem
      NorthArrowStyleItem northArrowItem = null;
      CIMNorthArrow northArrow = await QueuedTask.Run<CIMNorthArrow>(() =>
      {
        return northArrowItem.NorthArrow;
      });
      #endregion

      // cref: Get scale bar from ScaleBarStyleItem;ArcGIS.Desktop.Mapping.ScaleBarStyleItem.ScaleBar
      // cref: Get scale bar from ScaleBarStyleItem;ArcGIS.Desktop.Mapping.ScaleBarStyleItem.ScaleBar
      #region Get scale bar from ScaleBarStyleItem
      ScaleBarStyleItem scaleBarItem = null;
      CIMScaleBar scaleBar = await QueuedTask.Run<CIMScaleBar>(() =>
      {
        return scaleBarItem.ScaleBar;
      });
      #endregion

      // cref: Get label placement from LabelPlacementStyleItem;ArcGIS.Desktop.Mapping.LabelPlacementStyleItem.LabelPlacement
      // cref: Get label placement from LabelPlacementStyleItem;ArcGIS.Desktop.Mapping.LabelPlacementStyleItem.LabelPlacement
      #region Get label placement from LabelPlacementStyleItem
      LabelPlacementStyleItem labelPlacementItem = null;
      CIMLabelPlacementProperties labelPlacement = await QueuedTask.Run<CIMLabelPlacementProperties>(() =>
      {
        return labelPlacementItem.LabelPlacement;
      });
      #endregion

      // cref: Get grid from GridStyleItem;ArcGIS.Desktop.Mapping.GridStyleItem.Grid
      // cref: Get grid from GridStyleItem;ArcGIS.Desktop.Mapping.GridStyleItem.Grid
      #region Get grid from GridStyleItem
      GridStyleItem gridItem = null;
      CIMMapGrid grid = await QueuedTask.Run<CIMMapGrid>(() =>
      {
        return gridItem.Grid;
      });
      #endregion

      // cref: Get legend from LegendStyleItem;ArcGIS.Desktop.Mapping.LegendStyleItem.Legend
      // cref: Get legend from LegendStyleItem;ArcGIS.Desktop.Mapping.LegendStyleItem.Legend
      #region Get legend from LegendStyleItem
      LegendStyleItem legendStyleItem = null;
      CIMLegend legend = await QueuedTask.Run<CIMLegend>(() =>
      {
        return legendStyleItem.Legend;
      });
      #endregion

      // cref: Get legend item from LegendItemStyleItem;ArcGIS.Desktop.Mapping.LegendItemStyleItem.LegendItem
      // cref: Get legend item from LegendItemStyleItem;ArcGIS.Desktop.Mapping.LegendItemStyleItem.LegendItem
      #region Get legend item from LegendItemStyleItem
      LegendItemStyleItem legendItemStyleItem = null;
      CIMLegendItem legendItem = await QueuedTask.Run<CIMLegendItem>(() =>
      {
        return legendItemStyleItem.LegendItem;
      });
      #endregion

      // cref: Get table frame from TableFrameStyleItem;ArcGIS.Desktop.Mapping.TableFrameStyleItem.TableFrame
      // cref: Get table frame from TableFrameStyleItem;ArcGIS.Desktop.Mapping.TableFrameStyleItem.TableFrame
      #region Get table frame from TableFrameStyleItem
      TableFrameStyleItem tableFrameItem = null;
      CIMTableFrame tableFrame = await QueuedTask.Run<CIMTableFrame>(() =>
      {
        return tableFrameItem.TableFrame;
      });
      #endregion

      // cref: Get table frame field from TableFrameFieldStyleItem;ArcGIS.Desktop.Mapping.TableFrameFieldStyleItem.TableFrameField
      // cref: Get table frame field from TableFrameFieldStyleItem;ArcGIS.Desktop.Mapping.TableFrameFieldStyleItem.TableFrameField
      #region Get table frame field from TableFrameFieldStyleItem
      TableFrameFieldStyleItem tableFrameFieldItem = null;
      CIMTableFrameField tableFrameField = await QueuedTask.Run<CIMTableFrameField>(() =>
      {
        return tableFrameFieldItem.TableFrameField;
      });
      #endregion

      // cref: Get map surround from MapSurroundStyleItem;ArcGIS.Desktop.Mapping.MapSurroundStyleItem.MapSurround
      // cref: Get map surround from MapSurroundStyleItem;ArcGIS.Desktop.Mapping.MapSurroundStyleItem.MapSurround
      #region Get map surround from MapSurroundStyleItem
      MapSurroundStyleItem mapSurroundItem = null;
      CIMMapSurround mapSurround = await QueuedTask.Run<CIMMapSurround>(() =>
      {
        return mapSurroundItem.MapSurround;
      });
      #endregion

      // cref: Get dimension style from DimensionStyleStyleItem;ArcGIS.Desktop.Mapping.DimensionStyleStyleItem.DimensionStyle
      // cref: Get dimension style from DimensionStyleStyleItem;ArcGIS.Desktop.Mapping.DimensionStyleStyleItem.DimensionStyle
      #region Get dimension style from DimensionStyleStyleItem
      DimensionStyleStyleItem dimensionStyleStyleItem = null;
      CIMDimensionStyle dimensionStyle = await QueuedTask.Run<CIMDimensionStyle>(() =>
      {
        return dimensionStyleStyleItem.DimensionStyle;
      });
      #endregion

    }


    // cref: GetSetObject;ArcGIS.Desktop.Mapping.StyleItem.SetObject(System.Object)
    // cref: GetSetObject;ArcGIS.Desktop.Mapping.StyleItem.SetObject(System.Object)
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
