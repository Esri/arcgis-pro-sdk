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
using ArcGIS.Desktop.Framework.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Examples
{
  class MapView_Examples
  {
    /// MapView.Map
    /// <example>
    /// <code title="Get Active Map's Name" description="Get the active map's name." region="Get Active Map's Name" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\MapView_Examples.cs" lang="CS"/>
    /// </example>
    // cref: Get Active Map's Name;ArcGIS.Desktop.Mapping.MapView.Map
    #region Get Active Map's Name
    public string GetActiveMapName()
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return null;

      //Return the name of the map currently displayed in the active map view.
      return mapView.Map.Name;
    }
    #endregion

    /// MapView.SelectLayers(IReadOnlyCollection(Layer))
    /// <example>
    /// <code title="Select All Feature Layers in TOC" description="Select all the feature layers in the TOC for the active map." region="Select All Feature Layers in TOC" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\MapView_Examples.cs" lang="CS"/>
    /// </example>
    // cref: Select All Feature Layers in TOC;ArcGIS.Desktop.Mapping.MapView.SelectLayers(System.Collections.Generic.IReadOnlyCollection{ArcGIS.Desktop.Mapping.Layer})
    #region Select All Feature Layers in TOC
    public void SelectAllFeatureLayersInTOC()
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return;

      //Zoom to the selected layers in the TOC
      var featureLayers = mapView.Map.Layers.OfType<FeatureLayer>();
      mapView.SelectLayers(featureLayers.ToList());
    }
    #endregion

    /// MapView.HasPreviousCamera(), MapView.PreviousCameraAsync(TimeSpan?)
    /// <example>
    /// <code title="Go To Previous Camera" description="Zoom to the previous camera position." region="Go To Previous Camera" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\MapView_Examples.cs" lang="CS"/>
    /// </example>
    // cref: Go To Previous Camera;ArcGIS.Desktop.Mapping.MapView.HasPreviousCamera
    // cref: Go To Previous Camera;ArcGIS.Desktop.Mapping.MapView.PreviousCameraAsync(System.Nullable{System.TimeSpan})
    #region Go To Previous Camera
    public Task<bool> ZoomToPreviousCameraAsync()
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return Task.FromResult(false);

      //Zoom to the selected layers in the TOC
      if (mapView.HasPreviousCamera())
        return mapView.PreviousCameraAsync();

      return Task.FromResult(false);
    }
    #endregion

    /// MapView.HasNextCamera(), MapView.PreviousNextAsync(TimeSpan?)
    /// <example>
    /// <code title="Go To Next Camera" description="Zoom to the previous camera position." region="Go To Next Camera" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\MapView_Examples.cs" lang="CS"/>
    /// </example>
    // cref: Go To Next Camera;ArcGIS.Desktop.Mapping.MapView.HasNextCamera
    // cref: Go To Next Camera;ArcGIS.Desktop.Mapping.MapView.NextCameraAsync(System.Nullable{System.TimeSpan})
    #region Go To Next Camera
    public Task<bool> ZoomToNextCameraAsync()
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return Task.FromResult(false);

      //Zoom to the selected layers in the TOC
      if (mapView.HasNextCamera())
        return mapView.NextCameraAsync();

      return Task.FromResult(false);
    }
    #endregion

    /// MapView.FlashFeature(IReadOnlyDictionary(BasicFeatureLayer, List(Long)), Map.GetSelection()
    /// <example>
    /// <code title="Flash Selected Features" description="Flash the map's selcted features." region="Flash Selected Features" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\MapView_Examples.cs" lang="CS"/>
    /// </example>
    // cref: Flash Selected Features;ArcGIS.Desktop.Mapping.Map.GetSelection
    // cref: Flash Selected Features;ArcGIS.Desktop.Mapping.MapView.FlashFeature(System.Collections.Generic.IReadOnlyDictionary{ArcGIS.Desktop.Mapping.BasicFeatureLayer,System.Collections.Generic.List{System.Int64}})
    #region Flash Selected Features
    public Task FlashSelectedFeaturesAsync()
    {
      return QueuedTask.Run(() =>
      {
        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return;

        //Get the selected features from the map and filter out the standalone table selection.
        var selectedFeatures = mapView.Map.GetSelection()
          .Where(kvp => kvp.Key is BasicFeatureLayer)
          .ToDictionary(kvp => (BasicFeatureLayer)kvp.Key, kvp => kvp.Value);

        //Flash the collection of features.
        mapView.FlashFeature(selectedFeatures);
      });

    }
    #endregion

    /// MapView.CanSetViewingMode(), MapView.SetViewingMode()
    /// <example>
    /// <code title="Set ViewingMode" description="Change the active map view's viewing mode to SceneLocal." region="Set ViewingMode" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\MapView_Examples.cs" lang="CS"/>
    /// </example>
    // cref: Set ViewingMode;ArcGIS.Desktop.Mapping.MapView.CanSetViewingMode(ArcGIS.Core.CIM.MapViewingMode)
    // cref: Set ViewingMode;ArcGIS.Desktop.Mapping.MapView.SetViewingModeAsync(ArcGIS.Core.CIM.MapViewingMode)
    #region Set ViewingMode
    public void SetViewingModeToSceneLocal()
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return;

      //Check if the view can be set to SceneLocal and if it can set it.
      if (mapView.CanSetViewingMode(ArcGIS.Core.CIM.MapViewingMode.SceneLocal))
        mapView.SetViewingModeAsync(ArcGIS.Core.CIM.MapViewingMode.SceneLocal);
    }
    #endregion

    /// MapView.ViewingMode
    /// <example>
    /// <code title="Is View 3D" description="Test if the view is 3D." region="Is View 3D" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\MapView_Examples.cs" lang="CS"/>
    /// </example>
    // cref: Is View 3D;ArcGIS.Desktop.Mapping.MapView.ViewingMode
    #region Is View 3D
    public bool IsView3D()
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return false;

      //Return whether the viewing mode is SceneLocal or SceneGlobal
      return mapView.ViewingMode == ArcGIS.Core.CIM.MapViewingMode.SceneLocal || mapView.ViewingMode == ArcGIS.Core.CIM.MapViewingMode.SceneGlobal;
    }
    #endregion

    /// MapView.LinkMode
    /// <example>
    /// <code title="Enable View Linking" description="Set the View Linking mode to Center and Scale." region="Enable View Linking" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\MapView_Examples.cs" lang="CS"/>
    /// </example>
    // cref: Enable View Linking;ArcGIS.Desktop.Mapping.MapView.LinkMode
    #region Enable View Linking
    public void EnableViewLinking()
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return;

      //Set the view linking mode to Center and Scale.
      MapView.LinkMode = LinkMode.Center | LinkMode.Scale;
    }
    #endregion
  }

  class MapView_Examples_Synchronous
  {
    /// MapView.ZoomTo(Camera, TimeSpan?)
    /// <example>
    /// <code title="Rotate Map View" description="Rotate the active map view." region="Rotate Map View Synchronous" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\MapView_Examples.cs" lang="CS"/>
    /// </example>
    // cref: Rotate Map View Synchronous;ArcGIS.Desktop.Mapping.MapView.ZoomTo(ArcGIS.Desktop.Mapping.Camera,System.Nullable{System.TimeSpan})
    #region Rotate Map View Synchronous
    public Task<bool> RotateViewAsync(double heading)
    {
      return QueuedTask.Run(() =>
      {
        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return false;

        //Get the camera for the view, adjust the heading and zoom to the new camera position.
        var camera = mapView.Camera;
        camera.Heading = heading;
        return mapView.ZoomTo(camera, TimeSpan.Zero);
      });
    }
    #endregion

    /// MapView.Extent, MapView.ZoomTo(Geometry, TimeSpan?, bool)
    /// <example>
    /// <code title="Expand Extent" description="Expand the active map view's extent." region="Expand Extent Synchronous" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\MapView_Examples.cs" lang="CS"/>
    /// </example>
    // cref: Expand Extent Synchronous;ArcGIS.Desktop.Mapping.MapView.ZoomTo(ArcGIS.Core.Geometry.Geometry,System.Nullable{System.TimeSpan},System.Boolean)
    // cref: Expand Extent Synchronous;ArcGIS.Desktop.Mapping.MapView.Extent
    #region Expand Extent Synchronous
    public Task<bool> ExpandExtentAsync(double dx, double dy)
    {
      return QueuedTask.Run(() =>
      {
        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return false;

        //Expand the current extent by the given ratio.
        var extent = mapView.Extent;
        var newExtent = ArcGIS.Core.Geometry.GeometryEngine.Instance.Expand(extent, dx, dy, true);
        return mapView.ZoomTo(newExtent);
      });
    }
    #endregion

    /// MapView.ZoomToAsync(Bookmark, TimeSpan?)
    /// <example>
    /// <code title="Zoom to Bookmark" description="Zoom the active map view to a bookmark with a given name." region="Zoom To Bookmark Synchronous" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\MapView_Examples.cs" lang="CS"/>
    /// </example>
    // cref: Zoom To Bookmark Synchronous;ArcGIS.Desktop.Mapping.MapView.ZoomTo(ArcGIS.Desktop.Mapping.Bookmark,System.Nullable{System.TimeSpan})
    #region Zoom To Bookmark Synchronous
    public Task<bool> ZoomToBookmarkAsync(string bookmarkName)
    {
      return QueuedTask.Run(() =>
      {
        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return false;

        //Get the first bookmark with the given name.
        var bookmark = mapView.Map.GetBookmarks().FirstOrDefault(b => b.Name == bookmarkName);
        if (bookmark == null)
          return false;

        //Zoom the view to the bookmark.
        return mapView.ZoomTo(bookmark);
      });
    }
    #endregion

    /// MapView.ZoomTo(IEnumerable(Layer), bool, Timespan?, bool)
    /// <example>
    /// <code title="Zoom To Visible Layers" description="Zoom to all visible layers in the map." region="Zoom To Visible Layers Synchronous" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\MapView_Examples.cs" lang="CS"/>
    /// </example>
    // cref: Zoom To Visible Layers Synchronous;ArcGIS.Desktop.Mapping.MapView.ZoomTo(System.Collections.Generic.IEnumerable{ArcGIS.Desktop.Mapping.Layer},System.Boolean,System.Nullable{System.TimeSpan},System.Boolean)
    #region Zoom To Visible Layers Synchronous
    public Task<bool> ZoomToAllVisibleLayersAsync()
    {
      return QueuedTask.Run(() =>
      {
        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return false;

        //Zoom to all visible layers in the map.
        var visibleLayers = mapView.Map.Layers.Where(l => l.IsVisible);
        return mapView.ZoomTo(visibleLayers);
      });
    }
    #endregion

    /// MapView.ZoomToFullExtent(Timespan?)
    /// <example>
    /// <code title="Zoom To Full Extent" description="Zoom to the map's full extent." region="Zoom To Full Extent Synchronous" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\MapView_Examples.cs" lang="CS"/>
    /// </example>
    // cref: Zoom To Full Extent Synchronous;ArcGIS.Desktop.Mapping.MapView.ZoomToFullExtent(System.Nullable{System.TimeSpan})
    #region Zoom To Full Extent Synchronous
    public Task<bool> ZoomToFullExtentAsync()
    {
      return QueuedTask.Run(() =>
      {
        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return false;

        //Zoom to the map's full extent
        return mapView.ZoomToFullExtent();
      });
    }
    #endregion

    /// MapView.ZoomToSelectedAsync(Timespan?)
    /// <example>
    /// <code title="Zoom To Selected" description="Zoom to the map's selected features." region="Zoom To Selected Synchronous" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\MapView_Examples.cs" lang="CS"/>
    /// </example>
    // cref: Zoom To Selected Synchronous;ArcGIS.Desktop.Mapping.MapView.ZoomToSelected(System.Nullable{System.TimeSpan},System.Boolean)
    #region Zoom To Selected Synchronous
    public Task<bool> ZoomToSelectedAsync()
    {
      return QueuedTask.Run(() =>
      {
        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return false;

        //Zoom to the map's selected features.
        return mapView.ZoomToSelected();
      });
    }
    #endregion

    /// MapView.ZoomInFixedAsync(Timespan?)
    /// <example>
    /// <code title="Fixed Zoom In" description="Zoom in to the map view by a fixed amount." region="Fixed Zoom In Synchronous" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\MapView_Examples.cs" lang="CS"/>
    /// </example>
    // cref: Fixed Zoom In Synchronous;ArcGIS.Desktop.Mapping.MapView.ZoomInFixed(System.Nullable{System.TimeSpan})
    #region Fixed Zoom In Synchronous
    public Task<bool> ZoomInFixedAsync()
    {
      return QueuedTask.Run(() =>
      {
        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return false;

        //Zoom in the map view by a fixed amount.
        return mapView.ZoomInFixed();
      });

    }
    #endregion

    /// MapView.ZoomOutFixedAsync(Timespan?)
    /// <example>
    /// <code title="Fixed Zoom Out" description="Zoom out in the map view by a fixed amount." region="Fixed Zoom Out Synchronous" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\MapView_Examples.cs" lang="CS"/>
    /// </example>
    // cref: Fixed Zoom Out Synchronous;ArcGIS.Desktop.Mapping.MapView.ZoomOutFixed(System.Nullable{System.TimeSpan})
    #region Fixed Zoom Out Synchronous
    public Task<bool> ZoomOutFixedAsync()
    {
      return QueuedTask.Run(() =>
      {
        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return false;

        //Zoom out in the map view by a fixed amount.
        return mapView.ZoomInFixed();
      });

    }
    #endregion

    /// MapView.PanTo(Geometry, TimeSpan?, bool)
    /// <example>
    /// <code title="Pan to Extent" description="Pan the active map view to an extent." region="Pan To Extent Synchronous" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\MapView_Examples.cs" lang="CS"/>
    /// </example>
    // cref: Pan To Extent Synchronous;ArcGIS.Desktop.Mapping.MapView.PanTo(ArcGIS.Core.Geometry.Geometry,System.Nullable{System.TimeSpan})
    #region Pan To Extent Synchronous
    public Task<bool> PanToExtentAsync(double xMin, double yMin, double xMax, double yMax, ArcGIS.Core.Geometry.SpatialReference spatialReference)
    {
      return QueuedTask.Run(() =>
      {
        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return false;

        //Pan the view to a given extent.
        var envelope = ArcGIS.Core.Geometry.EnvelopeBuilder.CreateEnvelope(xMin, yMin, xMax, yMax, spatialReference);
        return mapView.PanTo(envelope);
      });
    }
    #endregion

    /// MapView.PanToAsync(Bookmark, TimeSpan?)
    /// <example>
    /// <code title="Pan to Bookmark" description="Pan the active map view to a bookmark with a given name." region="Pan To Bookmark Synchronous" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\MapView_Examples.cs" lang="CS"/>
    /// </example>
    // cref: Pan To Bookmark Synchronous;ArcGIS.Desktop.Mapping.MapView.PanTo(ArcGIS.Desktop.Mapping.Bookmark,System.Nullable{System.TimeSpan})
    #region Pan To Bookmark Synchronous
    public Task<bool> PanToBookmarkAsync(string bookmarkName)
    {
      return QueuedTask.Run(() =>
      {
        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return false;

        //Get the first bookmark with the given name.
        var bookmark = mapView.Map.GetBookmarks().FirstOrDefault(b => b.Name == bookmarkName);
        if (bookmark == null)
          return false;

        //Pan the view to the bookmark.
        return mapView.PanTo(bookmark);
      });
    }
    #endregion

    /// MapView.PanTo(IEnumerable(Layer), bool, Timespan?)
    /// <example>
    /// <code title="Pan To Visible Layers" description="Pan to all visible layers in the map." region="Pan To Visible Layers Synchronous" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\MapView_Examples.cs" lang="CS"/>
    /// </example>
    // cref: Pan To Visible Layers Synchronous;ArcGIS.Desktop.Mapping.MapView.PanTo(System.Collections.Generic.IEnumerable{ArcGIS.Desktop.Mapping.Layer},System.Boolean,System.Nullable{System.TimeSpan})
    #region Pan To Visible Layers Synchronous
    public Task<bool> PanToAllVisibleLayersAsync()
    {
      return QueuedTask.Run(() =>
      {
        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return false;

        //Pan to all visible layers in the map.
        var visibleLayers = mapView.Map.Layers.Where(l => l.IsVisible);
        return mapView.PanTo(visibleLayers);
      });
    }
    #endregion

    /// MapView.PanToSelectedAsync(Timespan?)
    /// <example>
    /// <code title="Pan To Selected" description="Pan to the map's selected features." region="Pan To Selected Synchronous" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\MapView_Examples.cs" lang="CS"/>
    /// </example>
    // cref: Pan To Selected Synchronous;ArcGIS.Desktop.Mapping.MapView.PanToSelected(System.Nullable{System.TimeSpan})
    #region Pan To Selected Synchronous
    public Task<bool> PanToSelectedAsync()
    {
      return QueuedTask.Run(() =>
      {
        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return false;

        //Pan to the map's selected features.
        return mapView.PanToSelected();
      });
    }
    #endregion
  }

  class MapView_Examples_Asynchronous
  {
    /// MapView.Camera, MapView.ZoomToAsync(Camera, TimeSpan?)
    /// <example>
    /// <code title="Rotate Map View" description="Rotate the active map view." region="Rotate Map View Asynchronous" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\MapView_Examples.cs" lang="CS"/>
    /// </example>
    // cref: Rotate Map View Asynchronous;ArcGIS.Desktop.Mapping.MapView.ZoomToAsync(ArcGIS.Desktop.Mapping.Camera,System.Nullable{System.TimeSpan})
    // cref: Rotate Map View Asynchronous;ArcGIS.Desktop.Mapping.Camera.Heading
    // cref: Rotate Map View Asynchronous;ArcGIS.Desktop.Mapping.MapView.Active
    // cref: Rotate Map View Asynchronous;ArcGIS.Desktop.Mapping.MapView.Camera
    // cref: Rotate Map View Asynchronous;ArcGIS.Desktop.Mapping.MapView
    #region Rotate Map View Asynchronous
    public void RotateView(double heading)
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return;

      //Get the camera for the view, adjust the heading and zoom to the new camera position.
      var camera = mapView.Camera;
      camera.Heading = heading;
      mapView.ZoomToAsync(camera, TimeSpan.Zero);
    }
    #endregion

    /// MapView.ZoomToAsync(Geometry, TimeSpan?, bool)
    /// <example>
    /// <code title="Zoom to Extent" description="Zoom the active map view to an extent." region="Zoom To Extent Asynchronous" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\MapView_Examples.cs" lang="CS"/>
    /// </example>
    // cref: Zoom To Extent Asynchronous;ArcGIS.Desktop.Mapping.MapView.ZoomToAsync(ArcGIS.Core.Geometry.Geometry,System.Nullable{System.TimeSpan},System.Boolean)
    #region Zoom To Extent Asynchronous
    public async Task<bool> ZoomToExtentAsync(double xMin, double yMin, double xMax, double yMax, ArcGIS.Core.Geometry.SpatialReference spatialReference)
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return false;

      //Create the envelope
      var envelope = await QueuedTask.Run(() => ArcGIS.Core.Geometry.EnvelopeBuilder.CreateEnvelope(xMin, yMin, xMax, yMax, spatialReference));

      //Zoom the view to a given extent.
      return await mapView.ZoomToAsync(envelope, TimeSpan.FromSeconds(2));
    }
    #endregion

    /// MapView.ZoomToAsync(Bookmark, TimeSpan?)
    /// <example>
    /// <code title="Zoom to Bookmark" description="Zoom the active map view to a bookmark with a given name." region="Zoom To Bookmark Asynchronous" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\MapView_Examples.cs" lang="CS"/>
    /// </example>
    // cref: Zoom To Bookmark Asynchronous;ArcGIS.Desktop.Mapping.MapView.ZoomToAsync(ArcGIS.Desktop.Mapping.Bookmark,System.Nullable{System.TimeSpan})
    #region Zoom To Bookmark Asynchronous
    public async Task<bool> ZoomToBookmarkAsync(string bookmarkName)
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return false;

      //Get the first bookmark with the given name.
      var bookmark = await QueuedTask.Run(() => mapView.Map.GetBookmarks().FirstOrDefault(b => b.Name == bookmarkName));
      if (bookmark == null)
        return false;

      //Zoom the view to the bookmark.
      return await mapView.ZoomToAsync(bookmark, TimeSpan.FromSeconds(2));
    }
    #endregion

    /// MapView.ZoomToAsync(IEnumerable(Layer), bool, Timespan?, bool), MapView.GetSelectedLayers(), 
    /// <example>
    /// <code title="Zoom To Selected Layers" description="Zoom to the selected layers in the TOC." region="Zoom To Selected Layers Asynchronous" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\MapView_Examples.cs" lang="CS"/>
    /// </example>
    // cref: Zoom To Selected Layers Asynchronous;ArcGIS.Desktop.Mapping.MapView.GetSelectedLayers
    // cref: Zoom To Selected Layers Asynchronous;ArcGIS.Desktop.Mapping.MapView.ZoomToAsync(System.Collections.Generic.IEnumerable{ArcGIS.Desktop.Mapping.Layer},System.Boolean,System.Nullable{System.TimeSpan},System.Boolean)
    #region Zoom To Selected Layers Asynchronous
    public Task<bool> ZoomToTOCSelectedLayersAsync()
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return Task.FromResult(false);

      //Zoom to the selected layers in the TOC
      var selectedLayers = mapView.GetSelectedLayers();
      return mapView.ZoomToAsync(selectedLayers);
    }
    #endregion

    /// MapView.ZoomToFullExtentAsync(Timespan?)
    /// <example>
    /// <code title="Zoom To Full Extent" description="Zoom to the map's full extent." region="Zoom To Full Extent Asynchronous" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\MapView_Examples.cs" lang="CS"/>
    /// </example>
    // cref: Zoom To Full Extent Asynchronous;ArcGIS.Desktop.Mapping.MapView.ZoomToFullExtentAsync(System.Nullable{System.TimeSpan})
    #region Zoom To Full Extent Asynchronous
    public Task<bool> ZoomToFullExtentAsync()
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return Task.FromResult(false);

      //Zoom to the map's full extent
      return mapView.ZoomToFullExtentAsync(TimeSpan.FromSeconds(2));
    }
    #endregion

    /// MapView.ZoomToSelectedAsync(Timespan?)
    /// <example>
    /// <code title="Zoom To Selected" description="Zoom to the map's selected features." region="Zoom To Selected Asynchronous" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\MapView_Examples.cs" lang="CS"/>
    /// </example>
    // cref: Zoom To Selected Asynchronous;ArcGIS.Desktop.Mapping.MapView.ZoomToSelectedAsync(System.Nullable{System.TimeSpan},System.Boolean)
    #region Zoom To Selected Asynchronous
    public Task<bool> ZoomToSelectedAsync()
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return Task.FromResult(false);

      //Zoom to the map's selected features.
      return mapView.ZoomToSelectedAsync(TimeSpan.FromSeconds(2));
    }
    #endregion

    /// MapView.ZoomInFixedAsync(Timespan?)
    /// <example>
    /// <code title="Fixed Zoom In" description="Zoom in to the map view by a fixed amount." region="Fixed Zoom In Asynchronous" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\MapView_Examples.cs" lang="CS"/>
    /// </example>
    // cref: Fixed Zoom In Asynchronous;ArcGIS.Desktop.Mapping.MapView.ZoomInFixedAsync(System.Nullable{System.TimeSpan})
    #region Fixed Zoom In Asynchronous
    public Task<bool> ZoomInFixedAsync()
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return Task.FromResult(false);

      //Zoom in the map view by a fixed amount.
      return mapView.ZoomInFixedAsync();
    }
    #endregion

    /// MapView.ZoomOutFixedAsync(Timespan?)
    /// <example>
    /// <code title="Fixed Zoom Out" description="Zoom out in the map view by a fixed amount." region="Fixed Zoom Out Asynchronous" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\MapView_Examples.cs" lang="CS"/>
    /// </example>
    // cref: Fixed Zoom Out Asynchronous;ArcGIS.Desktop.Mapping.MapView.ZoomOutFixedAsync(System.Nullable{System.TimeSpan})
    #region Fixed Zoom Out Asynchronous
    public Task<bool> ZoomOutFixedAsync()
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return Task.FromResult(false);

      //Zoom in the map view by a fixed amount.
      return mapView.ZoomOutFixedAsync();
    }
    #endregion

    /// MapView.PanToAsync(Geometry, TimeSpan?, bool)
    /// <example>
    /// <code title="Pan to Extent" description="Pan the active map view to an extent." region="Pan To Extent Asynchronous" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\MapView_Examples.cs" lang="CS"/>
    /// </example>
    // cref: Pan To Extent Asynchronous;ArcGIS.Desktop.Mapping.MapView.PanToAsync(ArcGIS.Core.Geometry.Geometry,System.Nullable{System.TimeSpan})
    #region Pan To Extent Asynchronous
    public async Task<bool> PanToExtentAsync(double xMin, double yMin, double xMax, double yMax, ArcGIS.Core.Geometry.SpatialReference spatialReference)
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return false;

      //Create the envelope
      var envelope = await QueuedTask.Run(() => ArcGIS.Core.Geometry.EnvelopeBuilder.CreateEnvelope(xMin, yMin, xMax, yMax, spatialReference));

      //Pan the view to a given extent.
      return await mapView.PanToAsync(envelope, TimeSpan.FromSeconds(2));
    }
    #endregion

    /// MapView.PanToAsync(Bookmark, TimeSpan?)
    /// <example>
    /// <code title="Pan to Bookmark" description="Pan the active map view to a bookmark with a given name." region="Pan To Bookmark Asynchronous" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\MapView_Examples.cs" lang="CS"/>
    /// </example>
    // cref: Pan To Bookmark Asynchronous;ArcGIS.Desktop.Mapping.MapView.PanToAsync(ArcGIS.Desktop.Mapping.Bookmark,System.Nullable{System.TimeSpan})
    #region Pan To Bookmark Asynchronous
    public async Task<bool> PanToBookmarkAsync(string bookmarkName)
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return false;

      //Get the first bookmark with the given name.
      var bookmark = await QueuedTask.Run(() => mapView.Map.GetBookmarks().FirstOrDefault(b => b.Name == bookmarkName));
      if (bookmark == null)
        return false;

      //Pan the view to the bookmark.
      return await mapView.PanToAsync(bookmark, TimeSpan.FromSeconds(2));
    }
    #endregion

    /// MapView.PanToAsync(IEnumerable(Layer), bool, Timespan?)
    /// <example>
    /// <code title="Pan To Selected Layers" description="Pan to the selected layers in the TOC." region="Pan To Selected Layers Asynchronous" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\MapView_Examples.cs" lang="CS"/>
    /// </example>
    // cref: Pan To Selected Layers Asynchronous;ArcGIS.Desktop.Mapping.MapView.PanToAsync(System.Collections.Generic.IEnumerable{ArcGIS.Desktop.Mapping.Layer},System.Boolean,System.Nullable{System.TimeSpan})
    #region Pan To Selected Layers Asynchronous
    public Task<bool> PanToTOCSelectedLayersAsync()
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return Task.FromResult(false);

      //Pan to the selected layers in the TOC
      var selectedLayers = mapView.GetSelectedLayers();
      return mapView.PanToAsync(selectedLayers);
    }
    #endregion

    /// MapView.PanToSelectedAsync(Timespan?)
    /// <example>
    /// <code title="Pan To Selected" description="Pan to the map's selected features." region="Pan To Selected Asynchronous" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\MapView_Examples.cs" lang="CS"/>
    /// </example>
    // cref: Pan To Selected Asynchronous;ArcGIS.Desktop.Mapping.MapView.PanToSelectedAsync(System.Nullable{System.TimeSpan})
    #region Pan To Selected Asynchronous
    public Task<bool> PanToSelectedAsync()
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return Task.FromResult(false);

      //Pan to the map's selected features.
      return mapView.PanToSelectedAsync(TimeSpan.FromSeconds(2));
    }
    #endregion
  }
}
