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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples
{
  class MapTool_Examples
  {
    /// MapTool, MapTool.IsSketchTool, MapTool.SketchOutputMode, MapTool.SketchType, MapTool.OnSketchCompleteAsync
    /// <example>
    /// <code title="Custom Identify" description="Create a tool that allows you to sketch in the view and return the features that intersect the sketch." source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\CustomIdentify.cs" lang="CS"/>
    /// </example>

    /// MapTool, MapTool.OnToolMouseDown, MapTool.HandleMouseDownAsync
    /// <example>
    /// <code title="Get Map Coordinates" description="Create a tool that allows you to click in the view and return the point in map coordinates that was clicked." source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\GetMapCoordinates.cs" lang="CS"/>
    /// </example>
  }

  /// MapTool.SketchSymbol
  /// <example>
  /// <code title="Set Sketch Symbol" description="Change the default symbol used while sketching." region="Sketch Symbol" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\MapTool_examples.cs" lang="CS"/>
  /// </example>
  #region Sketch Symbol
  internal class SketchTool_WithSymbol : MapTool
  {
    public SketchTool_WithSymbol()
    {
      IsSketchTool = true;
      SketchOutputMode = SketchOutputMode.Map; //Changing the Sketch Symbol is only supported with map sketches.
      SketchType = SketchGeometryType.Rectangle;
    }

    protected override Task OnToolActivateAsync(bool hasMapViewChanged)
    {
      return QueuedTask.Run(() =>
      {
        //Set the Sketch Symbol if it hasn't already been set.
        if (SketchSymbol != null)
          return;
        var polygonSymbol = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.CreateRGBColor(24, 69, 59), SimpleFillStyle.Solid, SymbolFactory.Instance.ConstructStroke(ColorFactory.Instance.BlackRGB, 1.0, SimpleLineStyle.Dash));
        SketchSymbol = polygonSymbol.MakeSymbolReference();
      });
    }
  }
  #endregion

  /// MapTool.ControlID, MapTool.EmbeddableControl
  /// <example>
  /// <code title="Embeddable Control" description="Set the embeddable control for a MapTool." region="Embeddable Control" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\MapTool_examples.cs" lang="CS"/>
  /// <code source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\EmbeddedControl.xaml" lang="XAML"/>
  /// <code source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\EmbeddedControlViewModel.cs" lang="CS"/>
  /// <code source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\EmbeddableControl.daml" lang="XML"/>
  /// </example>
  #region Embeddable Control
  internal class MapTool_WithControl : MapTool
  {
    public MapTool_WithControl()
    {
      ControlID = "mapTool_EmbeddableControl";
    }

    protected override void OnToolMouseDown(MapViewMouseButtonEventArgs e)
    {
      e.Handled = true;
    }

    protected override Task HandleMouseDownAsync(MapViewMouseButtonEventArgs e)
    {
      //Get the instance of the ViewModel
      var vm = EmbeddableControl as EmbeddedControlViewModel;
      if (vm == null)
        return Task.FromResult(0);

      //Get the map coordinates from the click point and set the property on the ViewMode.
      return QueuedTask.Run(() =>
        {
          var mapPoint = MapView.Active.ClientToMap(e.ClientPoint);
          vm.ClickText = string.Format("X: {0}, Y: {1}, Z: {2}", mapPoint.X, mapPoint.Y, mapPoint.Z);
        });
    }
  }
  #endregion

  /// MapTool.OverlayControlID, MapTool.OverlayEmbeddableControl
  /// <example>
  /// <code title="Overlay Embeddable Control" description="Set the overlay embeddable control for a MapTool." region="Overlay Embeddable Control" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\MapTool_examples.cs" lang="CS"/>
  /// <code source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\EmbeddedControl.xaml" lang="XAML"/>
  /// <code source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\EmbeddedControlViewModel.cs" lang="CS"/>
  /// <code source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\EmbeddableControl.daml" lang="XML"/>
  /// </example>
  #region Overlay Embeddable Control
  internal class MapTool_WithOverlayControl : MapTool
  {
    public MapTool_WithOverlayControl()
    {
      OverlayControlID = "mapTool_EmbeddableControl";
    }

    protected override void OnToolMouseDown(MapViewMouseButtonEventArgs e)
    {
      e.Handled = true;
    }

    protected override Task HandleMouseDownAsync(MapViewMouseButtonEventArgs e)
    {
      //Get the instance of the ViewModel
      var vm = OverlayEmbeddableControl as EmbeddedControlViewModel;
      if (vm == null)
        return Task.FromResult(0);

      //Get the map coordinates from the click point and set the property on the ViewMode.
      return QueuedTask.Run(() =>
      {
        var mapPoint = MapView.Active.ClientToMap(e.ClientPoint);
        vm.ClickText = string.Format("X: {0}, Y: {1}, Z: {2}", mapPoint.X, mapPoint.Y, mapPoint.Z);
      });
    }
  }
  #endregion
}
