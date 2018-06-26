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
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Core;
using ArcGIS.Core.CIM;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Threading.Tasks;

namespace CartoFeatures.ProSnippet
{
  class ProSnippet
  {
    //style management
    public void GetStyleInProjectByName()
    {
      #region How to get a style in project by name

      //Get all styles in the project
      var ProjectStyles = Project.Current.GetItems<StyleProjectItem>();

      //Get a specific style in the project by name
      StyleProjectItem style = ProjectStyles.First(x => x.Name == "NameOfTheStyle");
      #endregion
    }

    public async void CreateNewStyle()
    {
      #region How to create a new style

      //Full path for the new style file (.stylx) to be created
      string styleToCreate = @"C:\Temp\NewStyle.stylx";
      await QueuedTask.Run(() => StyleHelper.CreateStyle(Project.Current, styleToCreate));
      #endregion
    }

    public async void AddStyleToProject()
    {
      #region How to add a style to project

      //For ArcGIS Pro system styles, just pass in the name of the style to add to the project
      await QueuedTask.Run(() => StyleHelper.AddStyle(Project.Current, "3D Vehicles"));

      //For custom styles, pass in the full path to the style file on disk
      string customStyleToAdd = @"C:\Temp\CustomStyle.stylx";
      await QueuedTask.Run(() => StyleHelper.AddStyle(Project.Current, customStyleToAdd));

      #endregion
    }

    public async void RemoveStyleFromProject()
    {
      #region How to remove a style from project

      //For ArcGIS Pro system styles, just pass in the name of the style to remove from the project
      await QueuedTask.Run(() => StyleHelper.RemoveStyle(Project.Current, "3D Vehicles"));

      //For custom styles, pass in the full path to the style file on disk
      string customStyleToAdd = @"C:\Temp\CustomStyle.stylx";
      await QueuedTask.Run(() => StyleHelper.RemoveStyle(Project.Current, customStyleToAdd));

      #endregion
    }

    #region How to add a style item to a style
    public Task AddStyleItemAsync(StyleProjectItem style, StyleItem itemToAdd)
    {
      return QueuedTask.Run(() =>
      {
        if (style == null || itemToAdd == null)
          throw new System.ArgumentNullException();

        //Add the item to style
        style.AddItem(itemToAdd);
      });
    }
    #endregion

    #region How to remove a style item from a style
    public Task RemoveStyleItemAsync(StyleProjectItem style, StyleItem itemToRemove)
    {
      return QueuedTask.Run(() =>
      {
        if (style == null || itemToRemove == null)
          throw new System.ArgumentNullException();

        //Remove the item from style
        style.RemoveItem(itemToRemove);
      });
    }
    #endregion

    #region How to determine if a style can be upgraded
    //Pass in the full path to the style file on disk
    public async Task<bool> CanUpgradeStyleAsync(string stylePath)
    {
      //Add the style to the current project
      await QueuedTask.Run(() => StyleHelper.AddStyle(Project.Current, stylePath));
      StyleProjectItem style = Project.Current.GetItems<StyleProjectItem>().First(x => x.Path == stylePath);

      //returns true if style can be upgraded
      return style.CanUpgrade;
    }
    #endregion

    #region How to determine if a style is read-only
    //Pass in the full path to the style file on disk
    public async Task<bool> IsReadOnly(string stylePath)
    {
      //Add the style to the current project
      await QueuedTask.Run(() => StyleHelper.AddStyle(Project.Current, stylePath));
      StyleProjectItem style = Project.Current.GetItems<StyleProjectItem>().First(x => x.Path == stylePath);

      //returns true if style is read-only
      return style.IsReadOnly;
    }
    #endregion

    #region How to determine if a style is current
    //Pass in the full path to the style file on disk
    public async Task<bool> IsCurrent(string stylePath)
    {
      //Add the style to the current project
      await QueuedTask.Run(() => StyleHelper.AddStyle(Project.Current, stylePath));
      StyleProjectItem style = Project.Current.GetItems<StyleProjectItem>().First(x => x.Path == stylePath);

      //returns true if style matches the current Pro version
      return style.IsCurrent;
    }
    #endregion

    #region How to upgrade a style
    //Pass in the full path to the style file on disk
    public async Task<bool> UpgradeStyleAsync(string stylePath)
    {
      bool success = false;

      //Add the style to the current project
      await QueuedTask.Run(() => StyleHelper.AddStyle(Project.Current, stylePath));
      StyleProjectItem style = Project.Current.GetItems<StyleProjectItem>().First(x => x.Path == stylePath);

      //Verify that style can be upgraded
      if (style.CanUpgrade)
      {
        success = await QueuedTask.Run(() => StyleHelper.UpgradeStyle(style));
      }
      //return true if style was upgraded
      return success;
    }
    #endregion

    //construct point symbol
    public async Task ConstructPointSymbol_1()
    {
      #region How to construct a point symbol of a specific color and size

      await QueuedTask.Run(() =>
      {
        CIMPointSymbol pointSymbol = SymbolFactory.Instance.ConstructPointSymbol(ColorFactory.Instance.RedRGB, 10.0);
      });
      #endregion
    }

    public async Task ConstructPointSymbol_2()
    {
      #region How to construct a point symbol of a specific color, size and shape

      await QueuedTask.Run(() =>
      {
        CIMPointSymbol starPointSymbol = SymbolFactory.Instance.ConstructPointSymbol(ColorFactory.Instance.RedRGB, 10.0, SimpleMarkerStyle.Star);
      });
      #endregion
    }

    public async Task ConstructPointSymbol_3()
    {
      #region How to construct a point symbol from a marker

      await QueuedTask.Run(() =>
      {
        CIMMarker marker = SymbolFactory.Instance.ConstructMarker(ColorFactory.Instance.GreenRGB, 8.0, SimpleMarkerStyle.Pushpin);
        CIMPointSymbol pointSymbolFromMarker = SymbolFactory.Instance.ConstructPointSymbol(marker);
      });
      #endregion
    }

    public async Task ConstructPointSymbol_4()
    {
      #region How to construct a point symbol from a file on disk

      //The following file formats can be used to create the marker: DAE, 3DS, FLT, EMF, JPG, PNG, BMP, GIF
      CIMMarker markerFromFile = await QueuedTask.Run(() => SymbolFactory.Instance.ConstructMarkerFromFile(@"C:\Temp\fileName.dae"));

      CIMPointSymbol pointSymbolFromFile = SymbolFactory.Instance.ConstructPointSymbol(markerFromFile);

      #endregion
    }

    //construct polygon symbol
    public void ConstructPolygonSymbol_1()
    {
      #region How to construct a polygon symbol of specific color and fill style

      CIMPolygonSymbol polygonSymbol = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.RedRGB, SimpleFillStyle.Solid);

      #endregion
    }

    public void ConstructPolygonSymbol_2()
    {
      #region How to construct a polygon symbol of specific color, fill style and outline

      CIMStroke outline = SymbolFactory.Instance.ConstructStroke(ColorFactory.Instance.BlueRGB, 2.0, SimpleLineStyle.Solid);
      CIMPolygonSymbol fillWithOutline = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.RedRGB, SimpleFillStyle.Solid, outline);

      #endregion
    }

    public void ConstructPolygonSymbol_3()
    {
      #region How to construct a polygon symbol without an outline

      CIMPolygonSymbol fillWithoutOutline = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.RedRGB, SimpleFillStyle.Solid, null);

      #endregion
    }

    //construct line symbol
    public void ContructLineSymbol_1()
    {
      #region How to construct a line symbol of specific color, size and line style

      CIMLineSymbol lineSymbol = SymbolFactory.Instance.ConstructLineSymbol(ColorFactory.Instance.BlueRGB, 4.0, SimpleLineStyle.Solid);

      #endregion
    }

    public void ContructLineSymbol_2()
    {
      #region How to construct a line symbol from a stroke

      CIMStroke stroke = SymbolFactory.Instance.ConstructStroke(ColorFactory.Instance.BlackRGB, 2.0);
      CIMLineSymbol lineSymbolFromStroke = SymbolFactory.Instance.ConstructLineSymbol(stroke);

      #endregion
    }

    //multilayer symbols
    public void ContructMultilayerLineSymbol_1()
    {
      #region How to construct a multilayer line symbol with circle markers on the line ends

      //These methods must be called within the lambda passed to QueuedTask.Run
      var lineStrokeRed = SymbolFactory.Instance.ConstructStroke(ColorFactory.Instance.RedRGB, 4.0);
      var markerCircle = SymbolFactory.Instance.ConstructMarker(ColorFactory.Instance.RedRGB, 12, SimpleMarkerStyle.Circle);
      markerCircle.MarkerPlacement = new CIMMarkerPlacementOnVertices()
      {
        AngleToLine = true,
        PlaceOnEndPoints = true,
        Offset = 0
      };
      var lineSymbolWithCircles = new CIMLineSymbol()
      {
        SymbolLayers = new CIMSymbolLayer[2] { markerCircle, lineStrokeRed }
      };

      #endregion
    }

    public void ContructMultilayerLineSymbol_2()
    {
      #region How to construct a multilayer line symbol with an arrow head on the end

      //These methods must be called within the lambda passed to QueuedTask.Run
      var markerTriangle = SymbolFactory.Instance.ConstructMarker(ColorFactory.Instance.RedRGB, 12, SimpleMarkerStyle.Triangle);
      markerTriangle.Rotation = -90; // or -90
      markerTriangle.MarkerPlacement = new CIMMarkerPlacementOnLine() { AngleToLine = true, RelativeTo = PlacementOnLineRelativeTo.LineEnd };

      var lineSymbolWithArrow = new CIMLineSymbol()
      {
        SymbolLayers = new CIMSymbolLayer[2] { markerTriangle,
                    SymbolFactory.Instance.ConstructStroke(ColorFactory.Instance.RedRGB, 2)
                }
      };

      #endregion
    }

    //symbol reference
    public void GetSymbolReference()
    {
      #region How to get symbol reference from a symbol

      CIMPolygonSymbol symbol = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.RedRGB);

      //Get symbol reference from the symbol
      CIMSymbolReference symbolReference = symbol.MakeSymbolReference();

      #endregion
    }

    //symbol search
    #region How to search for a specific item in a style
    public Task<SymbolStyleItem> GetSymbolFromStyleAsync(StyleProjectItem style, string key)
    {
      return QueuedTask.Run(() =>
      {
        if (style == null)
          throw new System.ArgumentNullException();

        //Search for a specific point symbol in style
        SymbolStyleItem item = (SymbolStyleItem)style.LookupItem(StyleItemType.PointSymbol, key);
        return item;
      });
    }
    #endregion

    #region How to search for point symbols in a style
    public Task<IList<SymbolStyleItem>> GetPointSymbolsFromStyleAsync(StyleProjectItem style, string searchString)
    {
      if (style == null)
        throw new System.ArgumentNullException();

      //Search for point symbols
      return QueuedTask.Run(() => style.SearchSymbols(StyleItemType.PointSymbol, searchString));
    }
    #endregion

    #region How to search for line symbols in a style
    public Task<IList<SymbolStyleItem>> GetLineSymbolsFromStyleAsync(StyleProjectItem style, string searchString)
    {
      if (style == null)
        throw new System.ArgumentNullException();

      //Search for line symbols
      return QueuedTask.Run(() => style.SearchSymbols(StyleItemType.LineSymbol, searchString));
    }
    #endregion

    #region How to search for polygon symbols in a style
    public async Task<IList<SymbolStyleItem>> GetPolygonSymbolsFromStyleAsync(StyleProjectItem style, string searchString)
    {
      if (style == null)
        throw new System.ArgumentNullException();

      //Search for polygon symbols
      return await QueuedTask.Run(() => style.SearchSymbols(StyleItemType.PolygonSymbol, searchString));
    }
    #endregion

    #region How to search for colors in a style
    public async Task<IList<ColorStyleItem>> GetColorsFromStyleAsync(StyleProjectItem style, string searchString)
    {
      if (style == null)
        throw new System.ArgumentNullException();

      //Search for colors
      return await QueuedTask.Run(() => style.SearchColors(searchString));
    }
    #endregion

    #region How to search for color ramps in a style
    public async Task<IList<ColorRampStyleItem>> GetColorRampsFromStyleAsync(StyleProjectItem style, string searchString)
    {
      //StyleProjectItem can be "ColorBrewer Schemes (RGB)", "ArcGIS 2D"...
      if (style == null)
        throw new System.ArgumentNullException();

      //Search for color ramps
      //Color Ramp searchString can be "Spectral (7 Classes)", "Pastel 1 (3 Classes)", "Red-Gray (10 Classes)"..
      return await QueuedTask.Run(() => style.SearchColorRamps(searchString));
    }
    #endregion

    #region How to search for north arrows in a style
    public Task<IList<NorthArrowStyleItem>> GetNorthArrowsFromStyleAsync(StyleProjectItem style, string searchString)
    {
      if (style == null)
        throw new System.ArgumentNullException();

      //Search for north arrows
      return QueuedTask.Run(() => style.SearchNorthArrows(searchString));
    }
    #endregion

    #region How to search for scale bars in a style
    public Task<IList<ScaleBarStyleItem>> GetScaleBarsFromStyleAsync(StyleProjectItem style, string searchString)
    {
      if (style == null)
        throw new System.ArgumentNullException();

      //Search for scale bars
      return QueuedTask.Run(() => style.SearchScaleBars(searchString));
    }
    #endregion

    #region How to search for label placements in a style
    public Task<IList<LabelPlacementStyleItem>> GetLabelPlacementsFromStyleAsync(StyleProjectItem style, string searchString)
    {
      if (style == null)
        throw new System.ArgumentNullException();

      //Search for standard label placement
      return QueuedTask.Run(() => style.SearchLabelPlacements(StyleItemType.StandardLabelPlacement, searchString));
    }
    #endregion

    //feature layer symbology
    #region How to set symbol for a feature layer symbolized with simple renderer

    public Task SetFeatureLayerSymbolAsync(FeatureLayer ftrLayer, CIMSymbol symbolToApply)
    {
      if (ftrLayer == null || symbolToApply == null)
        throw new System.ArgumentNullException();

      return QueuedTask.Run(() =>
      {

        //Get simple renderer from the feature layer
        CIMSimpleRenderer currentRenderer = ftrLayer.GetRenderer() as CIMSimpleRenderer;
        if (currentRenderer == null)
          return;

        //Set symbol's real world setting to be the same as that of the feature layer
        symbolToApply.SetRealWorldUnits(ftrLayer.UsesRealWorldSymbolSizes);

        //Update the symbol of the current simple renderer
        currentRenderer.Symbol = symbolToApply.MakeSymbolReference();
        //Update the feature layer renderer
        ftrLayer.SetRenderer(currentRenderer);
      });
    }

    #endregion

    #region How to apply a symbol from style to a feature layer

    public Task SetFeatureLayerSymbolFromStyleItemAsync(FeatureLayer ftrLayer, SymbolStyleItem symbolItem)
    {
      if (ftrLayer == null || symbolItem == null)
        throw new System.ArgumentNullException();

      return QueuedTask.Run(() =>
      {
        //Get simple renderer from the feature layer
        CIMSimpleRenderer currentRenderer = ftrLayer.GetRenderer() as CIMSimpleRenderer;
        if (currentRenderer == null)
          return;
        //Get symbol from the SymbolStyleItem
        CIMSymbol symbol = symbolItem.Symbol;

        //Set symbol's real world setting to be the same as that of the feature layer
        symbol.SetRealWorldUnits(ftrLayer.UsesRealWorldSymbolSizes);

        //Update the symbol of the current simple renderer
        currentRenderer.Symbol = symbol.MakeSymbolReference();
        //Update the feature layer renderer
        ftrLayer.SetRenderer(currentRenderer);
      });
    }

    #endregion

    #region How to apply a point symbol from a style to a feature layer

    // var map = MapView.Active.Map;
    // if (map == null)
    //        return;
    // var pointFeatureLayer =
    //       map.GetLayersAsFlattenedList()
    //          .OfType<FeatureLayer>()
    //         .Where(fl => fl.ShapeType == esriGeometryType.esriGeometryPoint);
    //   await ApplySymbolToFeatureLayerAsync(pointFeatureLayer.FirstOrDefault(), "Fire Station");

    public Task ApplySymbolToFeatureLayerAsync(FeatureLayer featureLayer, string symbolName)
    {
      return QueuedTask.Run(async () =>
      {
              //Get the ArcGIS 2D System style from the Project
              var arcGIS2DStyle = Project.Current.GetItems<StyleProjectItem>().FirstOrDefault(s => s.Name == "ArcGIS 2D");

              //Search for the symbolName style items within the ArcGIS 2D style project item.
              var items = await QueuedTask.Run(() => arcGIS2DStyle.SearchSymbols(StyleItemType.PointSymbol, symbolName));

              //Gets the CIMSymbol
              CIMSymbol symbol = items.FirstOrDefault().Symbol;

              //Get the renderer of the point feature layer
              CIMSimpleRenderer renderer = featureLayer.GetRenderer() as CIMSimpleRenderer;

              //Set symbol's real world setting to be the same as that of the feature layer
              symbol.SetRealWorldUnits(featureLayer.UsesRealWorldSymbolSizes);

              //Apply the symbol to the feature layer's current renderer
              renderer.Symbol = symbol.MakeSymbolReference();

              //Appy the renderer to the feature layer
              featureLayer.SetRenderer(renderer);
      });
    }
    #endregion

  }
}