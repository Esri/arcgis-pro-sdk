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
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ArcGIS.Core.CIM;
using ArcGIS.Core.Internal.CIM;
using Geometry = ArcGIS.Core.Geometry.Geometry;
using ArcGIS.Desktop.Framework;
using System.Windows.Input;
using System.IO;
using System.Threading;
using System.Windows.Media;
using ArcGIS.Core.Data;
using ArcGIS.Desktop.Framework.Contracts;

namespace Snippets
{
  internal class ProSnippet
  {

    #region ProSnippet Group: MapView
    #endregion

    public void MapViewSnippets()
    {
      // cref: ArcGIS.Desktop.Framework.Contracts.Pane.Activate
      // cref: ArcGIS.Desktop.Mapping.IMapPane
      // cref: ArcGIS.Desktop.Mapping.MapView
      // cref: ArcGIS.Desktop.Framework.FrameworkApplication.Panes
      #region Find a MapView by its Caption
      // find a mapview by its Caption using the FrameworkApplication Panes collection
      var mapPaneCaption = "USNationalParks";
      var mapViewPane = FrameworkApplication.Panes.OfType<IMapPane>().FirstOrDefault((p) => p.Caption == mapPaneCaption);
      if (mapViewPane != null)
      {
        // activate the MapPane
        (mapViewPane as Pane).Activate();
        var mapView = mapViewPane.MapView;
        if (mapView != null)
        {
          // get the layers selected in the map's TOC
          var selectedLayers = mapView.GetSelectedLayers();
        }
      }

      #endregion Find a MapView by its Caption
    }

    // cref: ArcGIS.Desktop.Mapping.MapView.ViewingMode
    // cref: ArcGIS.Core.CIM.MapViewingMode
    #region Test if the view is 3D

    public bool IsView3D()
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return false;

      //Return whether the viewing mode is SceneLocal or SceneGlobal
      return mapView.ViewingMode == ArcGIS.Core.CIM.MapViewingMode.SceneLocal ||
             mapView.ViewingMode == ArcGIS.Core.CIM.MapViewingMode.SceneGlobal;
    }

    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapView.CanSetViewingMode(ArcGIS.Core.CIM.MapViewingMode)
    // cref: ArcGIS.Desktop.Mapping.MapView.SetViewingModeAsync(ArcGIS.Core.CIM.MapViewingMode)
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

    // cref: ArcGIS.Desktop.Mapping.MapView.LinkMode
    // cref: ArcGIS.Desktop.Mapping.LinkMode
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

    // cref: ArcGIS.Desktop.Mapping.MapView.ExportScene3DObjects
    // cref: ArcGIS.Desktop.Mapping.ExportSceneContentsFormat
    // cref: ArcGIS.Desktop.Mapping.STLExportSceneContentsFormat
    #region Export the contents of a scene to an exchange format such as STL.
    public void ExportSceneContents()
    {
      // Validate the current active view. Only a local scene can be exported.
      bool CanExportScene3DObjects = MapView.Active?.ViewingMode == MapViewingMode.SceneLocal;
      if (CanExportScene3DObjects)
      {
        //Gets the active map view.
        MapView mapView = MapView.Active;
        // Create a scene content export format, export the scene context as a stereolithography format
        var exportFormat = new STLExportSceneContentsFormat()
        {
          Extent = mapView.Extent, // sets Extent property
          FolderPath = @"C:\Temp", // sets FolderPath property
          FileName = "my-3d-objects.stl", //sets FileName property
          IsSingleFileOutput = true // sets whether to export to one single file
        };
        // Export the scene content as 3D objects
        mapView.ExportScene3DObjects(exportFormat);
      }
    }
    #endregion

    #region ProSnippet Group: Update MapView Extent (Zoom, Pan etc)
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapView.HasPreviousCamera
    // cref: ArcGIS.Desktop.Mapping.MapView.PreviousCameraAsync(System.Nullable{System.TimeSpan})
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

    // cref: ArcGIS.Desktop.Mapping.MapView.HasNextCamera
    // cref: ArcGIS.Desktop.Mapping.MapView.NextCameraAsync(System.Nullable{System.TimeSpan})
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

    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomToFullExtent(System.Nullable{System.TimeSpan})
    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomToFullExtentAsync(System.Nullable{System.TimeSpan})
    #region Zoom To Full Extent 
    public Task<bool> ZoomToFullExtent()
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

    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomInFixed(System.Nullable{System.TimeSpan})
    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomInFixedAsync(System.Nullable{System.TimeSpan})
    #region Fixed Zoom In
    public Task<bool> ZoomInFixed()
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

    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomOutFixed(System.Nullable{System.TimeSpan})
    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomOutFixedAsync(System.Nullable{System.TimeSpan})
    #region Fixed Zoom Out 
    public Task<bool> ZoomOutFixed()
    {
      return QueuedTask.Run(() =>
      {
        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return false;

        //Zoom out in the map view by a fixed amount.
        return mapView.ZoomOutFixed();
      });
    }

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

    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomToAsync(ArcGIS.Core.Geometry.Geometry,System.Nullable{System.TimeSpan},System.Boolean)
    #region Zoom To an Extent 
    public Task<bool> ZoomToExtent(double xMin, double yMin, double xMax, double yMax, ArcGIS.Core.Geometry.SpatialReference spatialReference)
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return Task.FromResult(false);

      //Create the envelope
      var envelope = ArcGIS.Core.Geometry.EnvelopeBuilderEx.CreateEnvelope(xMin, yMin, xMax, yMax, spatialReference);

      //Zoom the view to a given extent.
      return mapView.ZoomToAsync(envelope, TimeSpan.FromSeconds(2));
    }

    #endregion

    private double buffer_size;
    // cref: ArcGIS.Desktop.Mapping.Map.SpatialReference
    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomTo(ArcGIS.Core.Geometry.Geometry, System.TimeSpan, System.Boolean)
    #region Zoom To a Point 
    public Task<bool> ZoomToPoint(double x, double y, ArcGIS.Core.Geometry.SpatialReference spatialReference)
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return Task.FromResult(false);

      return QueuedTask.Run(() =>
      {
        //Note: Run within QueuedTask
        //Create a point
        var pt = MapPointBuilderEx.CreateMapPoint(x, y, spatialReference);
        //Buffer it - for purpose of zoom
        var poly = GeometryEngine.Instance.Buffer(pt, buffer_size);

        //do we need to project the buffer polygon?
        if (!MapView.Active.Map.SpatialReference.IsEqual(poly.SpatialReference))
        {
          //project the polygon
          poly = GeometryEngine.Instance.Project(poly, MapView.Active.Map.SpatialReference);
        }

        //Zoom - add in a delay for animation effect
        return mapView.ZoomTo(poly, new TimeSpan(0, 0, 0, 3));
      });
    }

    #endregion


    public void ZoomToSelected()
    {
     
        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return;
      // cref: ArcGIS.Desktop.Mapping.MapView.ZoomToSelected(System.Nullable{System.TimeSpan},System.Boolean)
      // cref: ArcGIS.Desktop.Mapping.MapView.ZoomToSelectedAsync(System.Nullable{System.TimeSpan},System.Boolean)

      #region Zoom To Selected Features with a timespan
      //Note: Run within QueuedTask
      //Zoom to the map's selected features.
      mapView.ZoomToSelected(TimeSpan.FromSeconds(3));
      #endregion

    }

    public void ZoomToSelectedAsync()
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return;
      #region Zoom To Selected Features asynchronously
      //Zoom to the map's selected features.
      mapView.ZoomToSelectedAsync(TimeSpan.FromSeconds(2));
      #endregion
    }


    // cref: ArcGIS.Desktop.Mapping.Map.GetBookmarks()
    // cref: ArcGIS.Desktop.Mapping.Bookmark
    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomTo(ArcGIS.Desktop.Mapping.Bookmark,System.Nullable{System.TimeSpan})
    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomToAsync(ArcGIS.Desktop.Mapping.Bookmark,System.Nullable{System.TimeSpan})
    #region Zoom To Bookmark by name
    public Task<bool> ZoomToBookmark(string bookmarkName)
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

    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomTo(System.Collections.Generic.IEnumerable{ArcGIS.Desktop.Mapping.Layer},System.Boolean,System.Nullable{System.TimeSpan},System.Boolean)
    #region Zoom To Visible Layers
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

    // cref: ArcGIS.Desktop.Mapping.MapView.GetSelectedLayers
    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomToAsync(System.Collections.Generic.IEnumerable{ArcGIS.Desktop.Mapping.Layer},System.Boolean,System.Nullable{System.TimeSpan},System.Boolean)
    #region Zoom To Selected Layers
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


    public void ZoomToObjectIDs(IEnumerable<long> objectIDs, FeatureLayer featureLayer, string whereClause)
    {
      // cref: ArcGIS.Desktop.Mapping.MapView.ZoomTo(ArcGIS.Desktop.Mapping.BasicFeatureLayer,System.Collections.Generic.IEnumerable{System.Int64},System.Nullable{System.TimeSpan},System.Boolean)
      #region Zoom to ObjectIDs
      //Execute a query on a layer to get a RowCursor.
      //You can pass in null (to get all the features)
      //or a "ArcGIS.Core.Data.QueryFilter" to filter the features.
      //var queryFilter = new QueryFilter
      //{
      //  WhereClause = whereClause
      //};
      using (var rowCursor = featureLayer.Search())
      {
        var objectIds = new List<long>();
        while (rowCursor.MoveNext())
        {
          using (var feature = rowCursor.Current as Feature)
          {
            objectIds.Add(feature.GetObjectID());
          }
        }
        if (objectIds.Count > 0)
        {
          MapView.Active.ZoomTo(featureLayer, objectIds, TimeSpan.FromSeconds(2));
        }
        #endregion
      }
    }
    
    // cref: ArcGIS.Desktop.Mapping.MapView.PanTo(ArcGIS.Core.Geometry.Geometry,System.Nullable{System.TimeSpan})
    // cref: ArcGIS.Desktop.Mapping.MapView.PanToAsync(ArcGIS.Core.Geometry.Geometry,System.Nullable{System.TimeSpan})
    #region Pan To an Extent 
    public Task<bool> PanToExtent(double xMin, double yMin, double xMax, double yMax, ArcGIS.Core.Geometry.SpatialReference spatialReference)
    {
      return QueuedTask.Run(() =>
      {
        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return false;

        //Pan the view to a given extent.
        var envelope = ArcGIS.Core.Geometry.EnvelopeBuilderEx.CreateEnvelope(xMin, yMin, xMax, yMax, spatialReference);
        return mapView.PanTo(envelope);
      });
    }

    public Task<bool> PanToExtentAsync(double xMin, double yMin, double xMax, double yMax, ArcGIS.Core.Geometry.SpatialReference spatialReference)
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return Task.FromResult(false);

      //Create the envelope
      var envelope = ArcGIS.Core.Geometry.EnvelopeBuilderEx.CreateEnvelope(xMin, yMin, xMax, yMax, spatialReference);

      //Pan the view to a given extent.
      return mapView.PanToAsync(envelope, TimeSpan.FromSeconds(2));
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapView.PanToSelected(System.Nullable{System.TimeSpan})
    // cref: ArcGIS.Desktop.Mapping.MapView.PanToSelectedAsync(System.Nullable{System.TimeSpan})
    #region Pan To Selected Features
    public Task<bool> PanToSelected()
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

    // cref: ArcGIS.Desktop.Mapping.MapView.PanTo(ArcGIS.Desktop.Mapping.Bookmark,System.Nullable{System.TimeSpan})
    // cref: ArcGIS.Desktop.Mapping.MapView.PanToAsync(ArcGIS.Desktop.Mapping.Bookmark,System.Nullable{System.TimeSpan})
    #region Pan To Bookmark 
    public Task<bool> PanToBookmark(string bookmarkName)
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

    // cref: ArcGIS.Desktop.Mapping.MapView.PanTo(System.Collections.Generic.IEnumerable{ArcGIS.Desktop.Mapping.Layer},System.Boolean,System.Nullable{System.TimeSpan})
    #region Pan To Visible Layers 
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

    // cref: ArcGIS.Desktop.Mapping.MapView.GetSelectedLayers
    // cref: ArcGIS.Desktop.Mapping.MapView.PanToAsync(System.Collections.Generic.IEnumerable{ArcGIS.Desktop.Mapping.Layer},System.Boolean,System.Nullable{System.TimeSpan})
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

    // cref: ArcGIS.Desktop.Mapping.MapView.Camera
    // cref: ArcGIS.Desktop.Mapping.Camera
    // cref: ArcGIS.Desktop.Mapping.Camera.Heading
    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomToAsync(ArcGIS.Desktop.Mapping.Camera, System.TimeSpan)
    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomTo(ArcGIS.Desktop.Mapping.Camera, System.TimeSpan)
    #region Rotate the map view

    public Task<bool> RotateView(double heading)
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return Task.FromResult(false);

      //Get the camera for the view, adjust the heading and zoom to the new camera position.
      var camera = mapView.Camera;
      camera.Heading = heading;
      return mapView.ZoomToAsync(camera, TimeSpan.Zero);
    }

    // or use the synchronous method
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

    // cref: ArcGIS.Desktop.Mapping.MapView.ZoomTo(ArcGIS.Core.Geometry.Geometry,System.Nullable{System.TimeSpan},System.Boolean)
    // cref: ArcGIS.Desktop.Mapping.MapView.Extent
    #region Expand Extent 
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


    #region ProSnippet Group: Maps
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapView
    // cref: ArcGIS.Desktop.Mapping.MapView.Active
    // cref: ArcGIS.Desktop.Mapping.MapView.Map
    // cref: ArcGIS.Desktop.Mapping.Map.Name
    #region Get the active map's name
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


    private static void ClearSelectionMap()
    {

      // cref: ArcGIS.Desktop.Mapping.Map.SetSelection(ArcGIS.Desktop.Mapping.SelectionSet, ArcGIS.Desktop.Mapping.SelectionCombinationMethod)
      #region Clear all selection in an Active map
      QueuedTask.Run(() =>
      {
        if (MapView.Active.Map != null)
        {
          MapView.Active.Map.SetSelection(null);
        }
      });
      #endregion
    }
    private static void CalculateSelectionTolerance()
    {
      // cref: ArcGIS.Desktop.Mapping.SelectionEnvironment.SelectionTolerance
      // cref: ArcGIS.Desktop.Mapping.Map.GetDefaultExtent()
      // cref: ArcGIS.Desktop.Mapping.MapView.MapToScreen(ArcGIS.Core.Geometry.MapPoint)
      // cref: ArcGIS.Desktop.Mapping.MapView.ScreenToMap(System.Windows.Point)
      #region Calculate Selection tolerance in map units
      //Selection tolerance for the map in pixels
      var selectionTolerance = SelectionEnvironment.SelectionTolerance;
      QueuedTask.Run(() =>
      {
        //Get the map center
        var mapExtent = MapView.Active.Map.GetDefaultExtent();
        var mapPoint = mapExtent.Center;
        //Map center as screen point
        var screenPoint = MapView.Active.MapToScreen(mapPoint);
        //Add selection tolerance pixels to get a "radius".
        var radiusScreenPoint = new System.Windows.Point((screenPoint.X + selectionTolerance), screenPoint.Y);
        var radiusMapPoint = MapView.Active.ScreenToMap(radiusScreenPoint);
        //Calculate the selection tolerance distance in map uints.
        var searchRadius = GeometryEngine.Instance.Distance(mapPoint, radiusMapPoint);
      });
      #endregion
    }

    private static async void AddMapViewOverlayControl()
    {
      // cref: ArcGIS.Desktop.Mapping.MapViewOverlayControl
      // cref: ArcGIS.Desktop.Mapping.MapViewOverlayControl.#ctor(System.Windows.FrameworkElement, System.Boolean, System.Boolean, System.Boolean, ArcGIS.Desktop.Mapping.OverlayControlRelativePosition, System.Double, System.Double)
      // cref: ArcGIS.Desktop.Mapping.MapView.AddOverlayControl(IMapViewOverlayControl)
      // cref: ArcGIS.Desktop.Mapping.MapView.RemoveOverlayControl(IMapViewOverlayControl)
      #region MapView Overlay Control
      //Creat a Progress Bar user control
      var progressBarControl = new System.Windows.Controls.ProgressBar();
      //Configure the progress bar
      progressBarControl.Minimum = 0;
      progressBarControl.Maximum = 100;
      progressBarControl.IsIndeterminate = true;
      progressBarControl.Width = 300;
      progressBarControl.Value = 10;
      progressBarControl.Height = 25;
      progressBarControl.Visibility = System.Windows.Visibility.Visible;
      //Create a MapViewOverlayControl. 
      var mapViewOverlayControl = new MapViewOverlayControl(progressBarControl, true, true, true, OverlayControlRelativePosition.BottomCenter, .5, .8);
      //Add to the active map
      MapView.Active.AddOverlayControl(mapViewOverlayControl);
      await QueuedTask.Run(() =>
      {
        //Wait 3 seconds to remove the progress bar from the map.
        Thread.Sleep(3000);

      });
      //Remove from active map
      MapView.Active.RemoveOverlayControl(mapViewOverlayControl);
      #endregion
    }

    #region ProSnippet Group: Layers
    #endregion

    // cref: ArcGIS.Desktop.Mapping.Map.Layers
    // cref: ArcGIS.Desktop.Mapping.MapView.SelectLayers(System.Colllections.Generic.IReadOnlyCollection<ArcGIS.Desktop.Mapping.Layer>)
    #region Select all feature layers in TOC

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

    // cref: ArcGIS.Desktop.Mapping.Map.GetSelection()
    // cref: ArcGIS.Desktop.Mapping.MapView.FlashFeature(ArcGIS.Desktop.Mapping.SelectionSet)
    #region Flash selected features

    public Task FlashSelectedFeaturesAsync()
    {
      return QueuedTask.Run(() =>
      {
        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return;

        //Get the selected features from the map and filter out the standalone table selection.

        //At 2.x
        //var selectedFeatures = mapView.Map.GetSelection()
        //    .Where(kvp => kvp.Key is BasicFeatureLayer)
        //    .ToDictionary(kvp => (BasicFeatureLayer)kvp.Key, kvp => kvp.Value);

        ////Flash the collection of features.
        //mapView.FlashFeature(selectedFeatures);

        var selectedFeatures = mapView.Map.GetSelection();

        //Flash the collection of features.
        mapView.FlashFeature(selectedFeatures);
      });
    }

    #endregion


    private void CheckLayerVisiblityInView()
    {
      // cref: ArcGIS.Desktop.Mapping.Layer.IsVisibleInView(ArcGIS.Desktop.Mapping.MapView)
      #region Check if Layer is visible in the given map view
      var mapView = MapView.Active;
      var layer = mapView.Map.GetLayersAsFlattenedList().OfType<Layer>().FirstOrDefault();
      if (mapView == null) return;
      bool isLayerVisibleInView = layer.IsVisibleInView(mapView);
      if (isLayerVisibleInView)
      {
        //Do Something
      }
      #endregion
    }

    private static void GetLayerPropertiesDialog()
    {
      // cref: ArcGIS.Desktop.Mapping.MapView.SelectLayers(System.Colllections.Generic.IReadOnlyCollection<ArcGIS.Desktop.Mapping.Layer>)
      #region Select a layer and open its layer properties page 
      // get the layer you want
      var layer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();

      // select it in the TOC
      List<Layer> layersToSelect = new List<Layer>();
      layersToSelect.Add(layer);
      MapView.Active.SelectLayers(layersToSelect);

      // now execute the layer properties command
      var wrapper = FrameworkApplication.GetPlugInWrapper("esri_mapping_selectedLayerPropertiesButton");
      var command = wrapper as ICommand;
      if (command == null)
        return;

      // execute the command
      if (command.CanExecute(null))
        command.Execute(null);
      #endregion
    }
    private static void ClearSelection()
    {
      // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.ClearSelection()
      #region Clear selection for a specific layer
      var lyr = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();
      QueuedTask.Run(() =>
      {
        lyr.ClearSelection();
      });
      #endregion
    }

    private static void OpenTablePane()
    {
      // cref: ArcGIS.Desktop.Core.FrameworkExtender.GetMapTableView(ArcGIS.Desktop.Framework.PaneCollection, ArcGIS.Desktop.Mapping.MapMember)
      // cref: ArcGIS.Core.CIM.CIMTableView.DisplaySubtypeDomainDescriptions
      // cref: ArcGIS.Core.CIM.CIMTableView.SelectionMode
      // cref: ArcGIS.Core.CIM.CIMTableView.ShowOnlyContingentValueFields
      // cref: ArcGIS.Core.CIM.CIMTableView.HighlightInvalidContingentValueFields
      // cref: ArcGIS.Desktop.Core.FrameworkExtender.OpenTablePane(ArcGIS.Desktop.Framework.PaneCollection, ArcGIS.Core.CIM.CIMMapTableView)
      #region Display Table pane for Map Member
      var mapMember = MapView.Active.Map.GetLayersAsFlattenedList().OfType<MapMember>().FirstOrDefault();
      //Gets or creates the CIMMapTableView for a MapMember.
      var tableView = FrameworkApplication.Panes.GetMapTableView(mapMember);
      //Configure the table view
      tableView.DisplaySubtypeDomainDescriptions = false;
      tableView.SelectionMode = false;
      tableView.ShowOnlyContingentValueFields = true;
      tableView.HighlightInvalidContingentValueFields = true;
      //Open the table pane using the configured tableView. If a table pane is already open it will be activated.
      //You must be on the UI thread to call this function.
      var tablePane = FrameworkApplication.Panes.OpenTablePane(tableView);
      #endregion
    }

    #region ProSnippet Group: TableView
    #endregion

    public void SetViewingModelTableView()
    {
      // cref: ArcGIS.Desktop.Mapping.TableView.SetViewMode(TableViewMode)
      // cref: ArcGIS.Desktop.Mapping.TableViewMode
      // cref: ArcGIS.Desktop.Mapping.TableViewMode.eSelectedRecords
      // cref: ArcGIS.Desktop.Mapping.TableViewMode.eAllRecords
      #region Set Table ViewingMode

      //Get the active table view.
      var tableView = TableView.Active;
      if (tableView == null)
        return;

      // change to "selected record" mode
      tableView.SetViewMode(TableViewMode.eSelectedRecords);
      #endregion
    }

    public void SetZoomLevelTableView()
    {
      // cref: ArcGIS.Desktop.Mapping.TableView.SetZoomLevel(System.Int32)
      // cref: ArcGIS.Desktop.Mapping.TableView.ZoomLevel
      #region Set ZoomLevel

      //Get the active table view.
      var tableView = TableView.Active;
      if (tableView == null)
        return;

      // change zoom level
      if (tableView.ZoomLevel > 100)
        tableView.SetZoomLevel(100);
      else
        tableView.SetZoomLevel(200);

      #endregion
    }

    public void AliasTableView()
    {
      // cref: ArcGIS.Desktop.Mapping.TableView.ShowFieldAlias
      // cref: ArcGIS.Desktop.Mapping.TableView.CanToggleFieldAlias
      // cref: ArcGIS.Desktop.Mapping.TableView.ToggleFieldAlias()
      #region Toggle Field Alias

      //Get the active table view.
      var tableView = TableView.Active;
      if (tableView == null)
        return;


      // set the value
      tableView.ShowFieldAlias = true;

      // OR toggle it
      if (tableView.CanToggleFieldAlias)
        tableView.ToggleFieldAlias();

      #endregion
    }

    public void DomainDescTableView()
    {
      // cref: ArcGIS.Desktop.Mapping.TableView.ShowSubtypeDomainDescriptions
      // cref: ArcGIS.Desktop.Mapping.TableView.CanToggleSubtypeDomainDescriptions
      // cref: ArcGIS.Desktop.Mapping.TableView.ToggleSubtypeDomainDescriptionsAsync()
      #region Toggle Subtype Descriptions

      //Get the active table view.
      var tableView = TableView.Active;
      if (tableView == null)
        return;


      // set the value
      tableView.ShowSubtypeDomainDescriptions = true;

      // OR toggle it
      if (tableView.CanToggleSubtypeDomainDescriptions)
        tableView.ToggleSubtypeDomainDescriptionsAsync();

      #endregion
    }

    public void GetAtiveRowIndexTableView()
    {
      // cref: ArcGIS.Desktop.Mapping.TableView.ActiveRowIndex
      #region Get the active row 

      //Get the active table view.
      var tableView = TableView.Active;
      if (tableView == null)
        return;

      // get the active rowindex
      int rowIndex = tableView.ActiveRowIndex;

      #endregion
    }

    public async void ChangeActiveRowIndexTableView()
    {
      // cref: ArcGIS.Desktop.Mapping.TableView.ActiveRowIndex
      // cref: ArcGIS.Desktop.Mapping.TableView.BringIntoView(System.Int32,System.Int32)
      #region Change the active row 

      //Get the active table view.
      var tableView = TableView.Active;
      if (tableView == null)
        return;

      // get the active rowindex
      int rowIndex = tableView.ActiveRowIndex;

      // move to a different row
      var newIndex = 10 + rowIndex;
      await tableView.BringIntoView(newIndex);

      #endregion
    }

    public void GetActiveOIDTableView()
    {
      // cref: ArcGIS.Desktop.Mapping.TableView.ActiveObjectId
      #region Get the active object ID

      //Get the active table view.
      var tableView = TableView.Active;
      if (tableView == null)
        return;

      // get the active objectID
      long? OID = tableView.ActiveObjectId;

      #endregion
    }

    public async void ConvertBetweenRowAndObjectIDTableView()
    {
      // cref: ArcGIS.Desktop.Mapping.TableView.ActiveRoWindex
      // cref: ArcGIS.Desktop.Mapping.TableView.GetObjectIdAsync(System.Int32)
      // cref: ArcGIS.Desktop.Mapping.TableView.GetRowIndexAsync(System.Int64, System.Boolean)
      #region Translate between rowIndex and objectID

      //Get the active table view.
      var tableView = TableView.Active;
      if (tableView == null)
        return;

      // get the active rowindex
      int rowIndex = tableView.ActiveRowIndex;
      // increase
      int newIndex = rowIndex + 10;
      // get the objectID
      long newOID = await tableView.GetObjectIdAsync(newIndex);

      // get the rowIndex for a specific objectID
      //   2nd parameter indicates if the search only occurs for the pages loaded
      //   if pass false, then in the worst case, a full table scan will occur to 
      //    find the objectID.
      long OID = 100;
      var idx = await tableView.GetRowIndexAsync(OID, true);

      #endregion
    }

    public void GetSelectionTableView()
    {
      // cref: ArcGIS.Desktop.Mapping.TableView.GetSelectedObjectIds
      // cref: ArcGIS.Desktop.Mapping.TableView.GetSelectedRowIndexes
      #region Get selected rows  or row indexes
      var tv = TableView.Active;
      if (tv == null)
        return;

      QueuedTask.Run(async () =>
      {
        // get the set of selected objectIDs 
        var selOids = tv.GetSelectedObjectIds();
        // get the set of selected row indexes
        var selRows = tv.GetSelectedRowIndexes();

      });
      #endregion
    }

    public void SetSelectionTableView()
    {
      // cref: ArcGIS.Desktop.Mapping.TableView.Select(System.Collections.Generic.IEnumerable{System.Int64},System.Boolean)
      // cref: ArcGIS.Desktop.Mapping.TableView.GetSelectedRowIndexes
      #region Change selected rows 
      var tv = TableView.Active;
      if (tv == null)
        return;

      QueuedTask.Run(async () =>
      {
        // set of selected OIDS
        var newoids = new List<long>();
        newoids.AddRange(new List<long>() { 10, 15, 17 });
        tv.Select(newoids, true);


        // add to set of selected row indexes
        var selRows = tv.GetSelectedRowIndexes();
        var newRows = new List<long>(selRows);
        newRows.AddRange(new List<long>() { 21, 35 });
        tv.Select(newRows, false);
      });
      #endregion
    }

    public void SelectAllTableView()
    {
      // cref: ArcGIS.Desktop.Mapping.TableView.CanSelectAll
      // cref: ArcGIS.Desktop.Mapping.TableView.SelectAll()
      #region Select all rows
      var tv = TableView.Active;
      if (tv == null)
        return;

      if (tv.CanSelectAll)
        tv.SelectAll();
      #endregion
    }

    public void OtherSelectionTableView()
    {
      // cref: ArcGIS.Desktop.Mapping.TableView.CanToggleRowSelection
      // cref: ArcGIS.Desktop.Mapping.TableView.ToggleRowSelection()
      // cref: ArcGIS.Desktop.Mapping.TableView.CanSwitchSelection
      // cref: ArcGIS.Desktop.Mapping.TableView.SwitchSelection()
      // cref: ArcGIS.Desktop.Mapping.TableView.CanClearSelection
      // cref: ArcGIS.Desktop.Mapping.TableView.ClearSelection()
      #region Toggle, Switch, Clear Selection
      var tv = TableView.Active;
      if (tv == null)
        return;

      // toggle the active rows selection
      if (tv.CanToggleRowSelection)
        tv.ToggleRowSelection();

      // switch the selection
      if (tv.CanSwitchSelection)
        tv.SwitchSelection();

      // clear the selection
      if (tv.CanClearSelection)
        tv.ClearSelection();
      #endregion
    }

    public void ZoomSelectedTableView()
    {
      // cref: ArcGIS.Desktop.Mapping.TableView.CanZoomToSelected
      // cref: ArcGIS.Desktop.Mapping.TableView.ZoomToSelected()
      // cref: ArcGIS.Desktop.Mapping.TableView.CanPanToSelected
      // cref: ArcGIS.Desktop.Mapping.TableView.PanToSelected()
      #region Zoom or Pan To Selected Rows
      var tv = TableView.Active;
      if (tv == null)
        return;

      if (tv.CanZoomToSelected)
        tv.ZoomToSelected();

      if (tv.CanPanToSelected)
        tv.PanToSelected();
      #endregion
    }

    public void DeleteSelectedTableView()
    {
      // cref: ArcGIS.Desktop.Mapping.TableView.CanDeleteSelected
      // cref: ArcGIS.Desktop.Mapping.TableView.DeleteSelected()
      #region Delete Selected Rows
      var tv = TableView.Active;
      if (tv == null)
        return;

      if (tv.CanDeleteSelected)
        tv.DeleteSelected(); 

      #endregion
    }

    public void GetHighlightTableView()
    {
      // cref: ArcGIS.Desktop.Mapping.TableView.CanGetHighlightedObjectIds
      // cref: ArcGIS.Desktop.Mapping.TableView.GetHighlightedObjectIds
      #region Get highlighted row indexes
      var tv = TableView.Active;
      if (tv == null)
        return;

      QueuedTask.Run(async () =>
      {
        IReadOnlyList<long> highlightedOIDs = null;
        if (tv.CanGetHighlightedObjectIds)
          // get the set of selected objectIDs 
          highlightedOIDs = tv.GetHighlightedObjectIds();
      });
      #endregion
    }

    public void SetHighlightedTableView()
    {
      // cref: ArcGIS.Desktop.Mapping.TableView.CanHighlight
      // cref: ArcGIS.Desktop.Mapping.TableView.Highlight(System.Collections.Generic.IEnumerable{System.Int64},System.Boolean)
      #region Change highlighted rows 
      var tv = TableView.Active;
      if (tv == null)
        return;

      QueuedTask.Run(() =>
      {
        // get list of current selected objectIDs
        var selectedObjectIds = tv.GetSelectedObjectIds();
        var idsToHighlight = new List<long>();

        // add the first two selected objectIds to highlight
        if (selectedObjectIds.Count >= 2)
        {
          idsToHighlight.Add(selectedObjectIds[0]);
          idsToHighlight.Add(selectedObjectIds[1]);
        }

        // highlight
        if (tv.CanHighlight)
          tv.Highlight(idsToHighlight, true);
      });
      #endregion
    }

    public void OtherHighlightedTableView()
    {
      // cref: ArcGIS.Desktop.Mapping.TableView.CanToggleRowHighlight
      // cref: ArcGIS.Desktop.Mapping.TableView.ToggleRowHighlight()
      // cref: ArcGIS.Desktop.Mapping.TableView.CanSwitchHighlight
      // cref: ArcGIS.Desktop.Mapping.TableView.SwitchHighlight()
      // cref: ArcGIS.Desktop.Mapping.TableView.CanClearHighlighted
      // cref: ArcGIS.Desktop.Mapping.TableView.ClearHighlighted()
      #region Toggle, Switch, Clear Highlights
      var tv = TableView.Active;
      if (tv == null)
        return;

      // toggle the active rows selection
      if (tv.CanToggleRowHighlight)
        tv.ToggleRowHighlight();

      // switch highlighted rows
      if (tv.CanSwitchHighlight)
        tv.SwitchHighlight();

      // clear the highlights
      if (tv.CanClearHighlighted)
        tv.ClearHighlighted();
      #endregion
    }

    public void ZoomHighlightedTableView()
    {
      // cref: ArcGIS.Desktop.Mapping.TableView.CanZoomToHighlighted
      // cref: ArcGIS.Desktop.Mapping.TableView.ZoomToHighlighted()
      // cref: ArcGIS.Desktop.Mapping.TableView.CanPanToHighlighted
      // cref: ArcGIS.Desktop.Mapping.TableView.PanToHighlighted()
      #region Zoom or Pan To Highlighted Rows
      var tv = TableView.Active;
      if (tv == null)
        return;

      if (tv.CanZoomToHighlighted)
        tv.ZoomToHighlighted();

      if (tv.CanPanToHighlighted)
        tv.PanToHighlighted();
      #endregion
    }

    public void DeleteHighlightedTableView()
    {
      // cref: ArcGIS.Desktop.Mapping.TableView.CanDeleteHighlighted
      // cref: ArcGIS.Desktop.Mapping.TableView.DeleteHighlighted()
      #region Delete Highlighted Rows
      var tv = TableView.Active;
      if (tv == null)
        return;

      if (tv.CanDeleteHighlighted)
        tv.DeleteHighlighted();

      #endregion
    }

    public void FieldsTableView()
    {
      // cref: ArcGIS.Desktop.Mapping.TableView.GetFields()
      // cref: ArcGIS.Desktop.Mapping.TableView.GetFieldIndex(System.String)
      // cref: ArcGIS.Desktop.Mapping.TableView.GetField(System.Int32)
      // cref: ArcGIS.Desktop.Mapping.FieldDescription
      #region Field Access
      var tv = TableView.Active;
      if (tv == null)
        return;

      // field access
      var flds = tv.GetFields();
      var fldIdx = tv.GetFieldIndex("STATE_NAME");
      var fldDesc = tv.GetField(fldIdx);

      #endregion
    }

    public void ActiveFielTableView()
    {
      // cref: ArcGIS.Desktop.Mapping.TableView.ActiveFieldIndex
      // cref: ArcGIS.Desktop.Mapping.TableView.GetField(System.Int32)
      // cref: ArcGIS.Desktop.Mapping.TableView.SetActiveField(System.String)
      // cref: ArcGIS.Desktop.Mapping.TableView.SetActiveField(System.Int32)
      // cref: ArcGIS.Desktop.Mapping.FieldDescription
      #region Get or set the Active Field
      var tv = TableView.Active;
      if (tv == null)
        return;

      // get active field, active field name
      var activeFieldIdx = tv.ActiveFieldIndex;
      var fldDesc = tv.GetField(activeFieldIdx);
      var fldName = fldDesc.Name;

      // set active field by name
      tv.SetActiveField("STATE_NAME");

      // or set active field by index
      tv.SetActiveField(3);
      #endregion
    }

    public void SelectFieldTableView()
    {
      // cref: ArcGIS.Desktop.Mapping.TableView.GetSelectedFields
      // cref: ArcGIS.Desktop.Mapping.TableView.SetSelectedFields(List<string>)
      #region Select Fields
      var tv = TableView.Active;
      if (tv == null)
        return;

      // get selected fields
      var selectedfields = tv.GetSelectedFields();

      // set selected fields
      tv.SetSelectedFields(new List<string> { "CITY_FIPS", "STATE_FIPS" });

      #endregion
    }

    public async void SetfieldOrderTableView()
    {
      // cref: ArcGIS.Desktop.Mapping.TableView.CanResetFieldOrder()
      // cref: ArcGIS.Desktop.Mapping.TableView.ResetFieldOrder()
      // cref: ArcGIS.Desktop.Mapping.TableView.SetFieldOrderAsync(List<string>)
      #region Set Field Order 
      var tv = TableView.Active;
      if (tv == null)
        return;

      if (tv.CanResetFieldOrder)
      {
        tv.ResetFieldOrder();

        var fldOrder = new List<string>();
        fldOrder.Add("STATE_NAME");
        fldOrder.Add("STATE_FIPS");
        await tv.SetFieldOrderAsync(fldOrder);
      }

      #endregion
    }

    public void HideFieldsTableView()
    {
      // cref: ArcGIS.Desktop.Mapping.TableView.GetHiddenFields()
      // cref: ArcGIS.Desktop.Mapping.TableView.CanShowAllFields
      // cref: ArcGIS.Desktop.Mapping.TableView.ShowAllFields()
      // cref: ArcGIS.Desktop.Mapping.TableView.SetHiddenFields(List<string>)
      // cref: ArcGIS.Desktop.Mapping.TableView.CanHideSelectedFields
      // cref: ArcGIS.Desktop.Mapping.TableView.HideSelectedFields()
      #region Show or Hide Fields
      var tv = TableView.Active;
      if (tv == null)
        return;

      // get list of hidden fields
      var hiddenFields = tv.GetHiddenFields();


      // show all fields
      if (tv.CanShowAllFields)
        tv.ShowAllFields();



      // hide only "CITY_FIPS", "STATE_FIPS"
      if (tv.CanShowAllFields)
      {
        // show all fields
        tv.ShowAllFields();
        tv.SetHiddenFields(new List<string> { "CITY_FIPS", "STATE_FIPS" });
      }



      // add "STATE_NAME to set of hidden fields
      tv.SetHiddenFields(new List<string> { "STATE_NAME" });


      // hide selected fields
      if (tv.CanHideSelectedFields)
        tv.HideSelectedFields();
      #endregion
    }

    public async void FreezeFieldsTableView()
    {
      // cref: ArcGIS.Desktop.Mapping.TableView.GetFrozenFields()
      // cref: ArcGIS.Desktop.Mapping.TableView.ClearAllFrozenFieldsAsync()
      // cref: ArcGIS.Desktop.Mapping.TableView.SetFrozenFieldsAsync(List<string>)
      #region Freeze Fields
      var tv = TableView.Active;
      if (tv == null)
        return;

      // get list of frozen fields
      var frozenfields = tv.GetFrozenFields();

      // un freeze all fields
      await tv.ClearAllFrozenFieldsAsync();


      // freeze a set of fields
      await tv.SetFrozenFieldsAsync(new List<string> { "CITY_FIPS", "STATE_FIPS" });

      #endregion
    }

    public async void SortTableView()
    {
      // cref: ArcGIS.Desktop.Mapping.TableView.CanSortDescending
      // cref: ArcGIS.Desktop.Mapping.TableView.SortDescending()
      // cref: ArcGIS.Desktop.Mapping.TableView.CanSortAscending
      // cref: ArcGIS.Desktop.Mapping.TableView.SortAscending()
      // cref: ArcGIS.Desktop.Mapping.TableView.CanCustomSort
      // cref: ArcGIS.Desktop.Mapping.TableView.CustomSort()
      // cref: ArcGIS.Desktop.Mapping.TableView.SortAsync
      #region Sort 
      var tv = TableView.Active;
      if (tv == null)
        return;

      // sort the active field descending
      if (tv.CanSortDescending)
        tv.SortDescending();


      // sort the active field ascending
      if (tv.CanSortAscending)
        tv.SortAscending();


      // peform a custom sort programmatically
      if (tv.CanCustomSort)
      {
        // sort fields
        var dict = new Dictionary<string, FieldSortInfo>();
        dict.Add("STATE_NAME", FieldSortInfo.Asc);
        dict.Add("CITY_NAME", FieldSortInfo.Desc);
        await tv.SortAsync(dict);
      }


      // perform a custom sort via the UI
      if (tv.CanCustomSort)
        tv.CustomSort();
      #endregion
    }


    public void FinReplaceTableView()
    {
      // cref: ArcGIS.Desktop.Mapping.TableView.CanFind
      // cref: ArcGIS.Desktop.Mapping.TableView.Find()
      // cref: ArcGIS.Desktop.Mapping.TableView.CanFindAndReplace
      // cref: ArcGIS.Desktop.Mapping.TableView.FindAndReplace()
      #region Find and Replace
      var tv = TableView.Active;
      if (tv == null)
        return;

      // launch the find UI
      if (tv.CanFind)
        tv.Find();


      // or launch the find and replace UI
      if (tv.CanFindAndReplace)
        tv.FindAndReplace();
      #endregion
    }

    public void GoToTableView()
    {
      // cref: ArcGIS.Desktop.Mapping.TableView.CanGoTo
      // cref: ArcGIS.Desktop.Mapping.TableView.GoTo()
      #region GoTo
      var tv = TableView.Active;
      if (tv == null)
        return;

      // launch the GoTo UI
      if (tv.CanGoTo)
        tv.GoTo();

      #endregion
    }

    public void RefreshTableView()
    {
      // cref: ArcGIS.Desktop.Mapping.TableView.CanRefresh
      // cref: ArcGIS.Desktop.Mapping.TableView.Refresh()
      #region Refresh
      var tv = TableView.Active;
      if (tv == null)
        return;

      // refresh
      if (tv.CanRefresh)
        tv.Refresh();

      #endregion
    }

    public void TableCaption()
    {
      // cref: ArcGIS.Desktop.Framework.FrameworkApplication.Panes
      // cref: ArcGIS.Desktop.Mapping.ITablePane
      // cref: ArcGIS.Desktop.Mapping.ITablePaneEx
      // cref: ArcGIS.Desktop.Mapping.ITablePaneEx.Caption
      // cref: ArcGIS.Desktop.Mapping.IExternalTablePane
      // cref: ArcGIS.Desktop.Mapping.IExternalTablePane.Caption
      #region Change table View caption

      // find all the table panes (table panes hosting map data)
      var tablePanes = FrameworkApplication.Panes.OfType<ITablePane>();
      var tablePane = tablePanes.FirstOrDefault(p => (p as ITablePaneEx)?.Caption == "oldcCaption");
      var tablePaneEx = tablePane as ITablePaneEx;
      if (tablePaneEx != null)
        tablePaneEx.Caption = "newCaption";

      // find all the external table panes (table panes hosting external data)
      var externalPanes = FrameworkApplication.Panes.OfType<IExternalTablePane>();
      var externalTablePane = externalPanes.FirstOrDefault(p => p.Caption == "oldcCaption");
      if (externalTablePane != null)
        externalTablePane.Caption = "newCaption";
      #endregion
    }

    public void TableViewFromPane()
    {
      // cref: ArcGIS.Desktop.Framework.FrameworkApplication.Panes
      // cref: ArcGIS.Desktop.Mapping.ITablePane
      // cref: ArcGIS.Desktop.Mapping.ITablePaneEx
      // cref: ArcGIS.Desktop.Mapping.ITablePaneEx.TableView
      // cref: ArcGIS.Desktop.Mapping.IExternalTablePane
      // cref: ArcGIS.Desktop.Mapping.IExternalTablePane.TableView
      #region Get TableView from table pane

      TableView tv = null;

      // find all the table panes (table panes hosting map data)
      var tablePanes = FrameworkApplication.Panes.OfType<ITablePane>();
      var tablePane = tablePanes.FirstOrDefault(p => (p as ITablePaneEx)?.Caption == "caption");
      var tablePaneEx = tablePane as ITablePaneEx;
      if (tablePaneEx != null)
        tv = tablePaneEx.TableView;

      // if it's not found, maybe it's an external table pane
      if (tv == null)
      {
        // find all the external table panes (table panes hosting external data)
        var externalPanes = FrameworkApplication.Panes.OfType<IExternalTablePane>();
        var externalTablePane = externalPanes.FirstOrDefault(p => p.Caption == "caption");
        if (externalTablePane != null)
          tv = externalTablePane.TableView;
      }
      #endregion
    }

    
    #region ProSnippet Group: Features
    #endregion

    private static void Masking()
    {
      QueuedTask.Run(() =>
      {
        // cref: ArcGIS.Core.CIM.CIMBaselayer.LayerMasks
        #region Mask feature
        //Get the layer to be masked
        var lineLyrToBeMasked = MapView.Active.Map.Layers.FirstOrDefault(lyr => lyr.Name == "TestLine") as FeatureLayer;
        //Get the layer's definition
        var lyrDefn = lineLyrToBeMasked.GetDefinition();
        //Create an array of Masking layers (polygon only)
        //Set the LayerMasks property of the Masked layer
        lyrDefn.LayerMasks = new string[] { "CIMPATH=map3/testpoly.xml" };
        //Re-set the Masked layer's defintion
        lineLyrToBeMasked.SetDefinition(lyrDefn);
        #endregion
      });
    }

    #region ProSnippet Group: Popups
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapView.ShowPopup(ArcGIS.Desktop.Mapping.MapMember, System.Int64)
    #region Show a pop-up for a feature

    public void ShowPopup(MapMember mapMember, long objectID)
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return;

      mapView.ShowPopup(mapMember, objectID);
    }

    #endregion

    // cref: ArcGIS.Desktop.Mapping.PopupContent
    // cref: ArcGIS.Desktop.Mapping.PopupContent.#ctor(System.String, System.String)
    // cref: ArcGIS.Desktop.Mapping.MapView.ShowCustomPopup
    #region Show a custom pop-up

    public void ShowCustomPopup()
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return;

      //Create custom popup content
      var popups = new List<PopupContent>
            {
                new PopupContent("<b>This text is bold.</b>", "Custom tooltip from HTML string"),
                new PopupContent(new Uri("http://www.esri.com/"), "Custom tooltip from Uri")
            };
      mapView.ShowCustomPopup(popups);
    }

    #endregion

    // cref: ArcGIS.Desktop.Mapping.PopupDefinition
    // cref: ArcGIS.Desktop.Mapping.PopupDefinition.#ctor()
    // cref: ArcGIS.Desktop.Mapping.MapView.ShowPopup(ArcGIS.Desktop.Mapping.MapMember, System.Int64, ArcGIS.Desktop.Mapping.PopupDefinition)
    #region Show a pop-up for a feature using pop-up window properties

    public void ShowPopupWithWindowDef(MapMember mapMember, long objectID)
    {
      if (MapView.Active == null) return;
      // Sample code: https://github.com/ArcGIS/arcgis-pro-sdk-community-samples/blob/master/Map-Exploration/CustomIdentify/CustomIdentify.cs
      var topLeftCornerPoint = new System.Windows.Point(200, 200);
      var popupDef = new PopupDefinition()
      {
        Append = true,      // if true new record is appended to existing (if any)
        Dockable = true,    // if true popup is dockable - if false Append is not applicable
        Position = topLeftCornerPoint,  // Position of top left corner of the popup (in pixels)
        Size = new System.Windows.Size(200, 400)    // size of the popup (in pixels)
      };
      MapView.Active.ShowPopup(mapMember, objectID, popupDef);
    }

    #endregion

    // cref: ArcGIS.Desktop.Mapping.PopupContent
    // cref: ArcGIS.Desktop.Mapping.PopupContent.#ctor(System.String, System.String)
    // cref: ArcGIS.Desktop.Mapping.PopupDefinition
    // cref: ArcGIS.Desktop.Mapping.PopupDefinition.#ctor()
    // cref: ArcGIS.Desktop.Mapping.MapView.ShowCustomPopup(System.Collections.Generic.IEnumerable<ArcGIS.Desktop.Mapping.PopupContent>, System.Collections.Generic.IEnumerable<ArcGIS.Desktop.Mapping.PopupCommand>, System.Boolean, ArcGIS.Desktop.Mapping.PopupDefinition)
    #region Show a custom pop-up using pop-up window properties

    public void ShowCustomPopupWithWindowDef()
    {
      if (MapView.Active == null) return;

      //Create custom popup content
      var popups = new List<PopupContent>
            {
                new PopupContent("<b>This text is bold.</b>", "Custom tooltip from HTML string"),
                new PopupContent(new Uri("http://www.esri.com/"), "Custom tooltip from Uri")
            };
      // Sample code: https://github.com/ArcGIS/arcgis-pro-sdk-community-samples/blob/master/Framework/DynamicMenu/DynamicFeatureSelectionMenu.cs
      var topLeftCornerPoint = new System.Windows.Point(200, 200);
      var popupDef = new PopupDefinition()
      {
        Append = true,      // if true new record is appended to existing (if any)
        Dockable = true,    // if true popup is dockable - if false Append is not applicable
        Position = topLeftCornerPoint,  // Position of top left corner of the popup (in pixels)
        Size = new System.Windows.Size(200, 400)    // size of the popup (in pixels)
      };
      MapView.Active.ShowCustomPopup(popups, null, true, popupDef);
    }

    #endregion

    // cref: ArcGIS.Desktop.Mapping.PopupContent
    // cref: ArcGIS.Desktop.Mapping.PopupContent.#ctor(ArcGIS.Desktop.Mapping.MapMember, System.Int64)
    // cref: ArcGIS.Desktop.Mapping.PopupCommand
    // cref: ArcGIS.Desktop.Mapping.PopupCommand.#ctor(System.Action{ArcGIS.Desktop.Mapping.PopupContent},System.Func{ArcGIS.Desktop.Mapping.PopupContent,System.Boolean},System.String,System.Windows.Media.ImageSource)
    // cref: ArcGIS.Desktop.Mapping.PopupCommand.Image
    // cref: ArcGIS.Desktop.Mapping.PopupCommand.Tooltip
    // cref: ArcGIS.Desktop.Mapping.MapView.ShowCustomPopup(System.Collections.Generic.IEnumerable{ArcGIS.Desktop.Mapping.PopupContent},System.Collections.Generic.IEnumerable{ArcGIS.Desktop.Mapping.PopupCommand},System.Boolean)
    #region Show A pop-up With Custom Commands
    public void ShowCustomPopup(MapMember mapMember, long objectID)
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return;

      //Create custom popup content from existing map member and object id
      var popups = new List<PopupContent>();
      popups.Add(new PopupContent(mapMember, objectID));

      //Create a new custom command to add to the popup window
      var commands = new List<PopupCommand>();
      commands.Add(new PopupCommand(
        p => MessageBox.Show(string.Format("Map Member: {0}, ID: {1}", p.MapMember, p.IDString)),
        p => { return p != null; },
        "My custom command",
        System.Windows.Application.Current.Resources["GenericCheckMark16"] as ImageSource));

      mapView.ShowCustomPopup(popups, commands, true);
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.PopupContent.OnCreateHtmlContent
    // cref: ArcGIS.Desktop.Mapping.PopupContent.IsDynamicContent
    // cref: ArcGIS.Desktop.Mapping.MapView.ShowCustomPopup(System.Collections.Generic.IEnumerable<ArcGIS.Desktop.Mapping.PopupContent>)
    #region Show A Dynamic Pop-up
    public void ShowDynamicPopup(MapMember mapMember, List<long> objectIDs)
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return;

      //Create popup whose content is created the first time the item is requested.
      var popups = new List<PopupContent>();
      foreach (var id in objectIDs)
      {
        popups.Add(new DynamicPopupContent(mapMember, id));
      }

      mapView.ShowCustomPopup(popups);
    }

    internal class DynamicPopupContent : PopupContent
    {
      public DynamicPopupContent(MapMember mapMember, long objectID)
      {
        MapMember = mapMember;
        IDString = objectID.ToString();
        IsDynamicContent = true;
      }

      //Called when the pop-up is loaded in the window.
      protected override Task<string> OnCreateHtmlContent()
      {
        return QueuedTask.Run(() => string.Format("<b>Map Member: {0}, ID: {1}</b>", MapMember, IDString));
      }
    }

    #endregion



    #region ProSnippet Group: Bookmarks
    #endregion

    // cref: ArcGIS.Desktop.Mapping.Map.AddBookmark(ArcGIS.Desktop.Mapping.MapView, System.String)
    // cref: ArcGIS.Desktop.Mapping.Bookmark
    #region Create a new bookmark using the active map view

    public Task<Bookmark> AddBookmarkAsync(string name)
    {
      return QueuedTask.Run(() =>
      {
        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return null;

        //Adding a new bookmark using the active view.
        return mapView.Map.AddBookmark(mapView, name);
      });
    }

    #endregion

    // cref: ArcGIS.Core.CIM.CIMBookmark
    // cref: ArcGIS.Core.CIM.CIMBookmark.Camera
    // cref: ArcGIS.Core.CIM.CIMBookmark.Name
    // cref: ArcGIS.Core.CIM.CIMBookmark.ThumbnailImagePath
    // cref: ArcGIS.Desktop.Mapping.Map.AddBookmark(ArcGIS.Core.CIM.CIMBookmark)
    #region Add New Bookmark from CIMBookmark
    public Task<Bookmark> AddBookmarkFromCameraAsync(Camera camera, string name)
    {
      return QueuedTask.Run(() =>
      {
        //Set properties for Camera
        CIMViewCamera cimCamera = new CIMViewCamera()
        {
          X = camera.X,
          Y = camera.Y,
          Z = camera.Z,
          Scale = camera.Scale,
          Pitch = camera.Pitch,
          Heading = camera.Heading,
          Roll = camera.Roll
        };

        //Create new CIM bookmark and populate its properties
        var cimBookmark = new CIMBookmark() { Camera = cimCamera, Name = name, ThumbnailImagePath = "" };

        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return null;

        //Add a new bookmark for the active map.
        return mapView.Map.AddBookmark(cimBookmark);
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.Map.GetBookmarks()
    // cref: ArcGIS.Desktop.Mapping.Bookmark
    #region Get the collection of bookmarks for the project

    public Task<ReadOnlyObservableCollection<Bookmark>> GetProjectBookmarksAsync()
    {
      //Get the collection of bookmarks for the project.
      return QueuedTask.Run(() => Project.Current.GetBookmarks());
    }

    #endregion

    // cref: ArcGIS.Desktop.Mapping.Map.GetBookmarks
    // cref: ArcGIS.Desktop.Mapping.Bookmark
    #region Get Map Bookmarks
    public Task<ReadOnlyObservableCollection<Bookmark>> GetActiveMapBookmarksAsync()
    {
      return QueuedTask.Run(() =>
      {
        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return null;

        //Return the collection of bookmarks for the map.
        return mapView.Map.GetBookmarks();
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.Map.MoveBookmark(ArcGIS.Desktop.Mapping.Bookmark,System.Int32)
    #region Move Bookmark to the Top
    public Task MoveBookmarkToTopAsync(Map map, string name)
    {
      return QueuedTask.Run(() =>
      {
        //Find the first bookmark with the name
        var bookmark = map.GetBookmarks().FirstOrDefault(b => b.Name == name);
        if (bookmark == null)
          return;

        //Remove the bookmark
        map.MoveBookmark(bookmark, 0);
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.Bookmark.Rename(System.String)
    #region Rename Bookmark
    public Task RenameBookmarkAsync(Bookmark bookmark, string newName)
    {
      return QueuedTask.Run(() => bookmark.Rename(newName));
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.Map.GetBookmarks()
    // cref: ArcGIS.Desktop.Mapping.Map.RemoveBookmark(ArcGIS.Desktop.Mapping.Bookmark)
    // cref: ArcGIS.Desktop.Mapping.Bookmark
    #region Remove bookmark with a given name

    public Task RemoveBookmarkAsync(Map map, string name)
    {
      return QueuedTask.Run(() =>
      {
        //Find the first bookmark with the name
        var bookmark = map.GetBookmarks().FirstOrDefault(b => b.Name == name);
        if (bookmark == null)
          return;

        //Remove the bookmark
        map.RemoveBookmark(bookmark);
      });
    }

    #endregion

    // cref: ArcGIS.Desktop.Mapping.Bookmark
    // cref: ArcGIS.Desktop.Mapping.Bookmark.SetThumbnail(System.Windows.Media.Imaging.BitmapSource)
    #region Change the thumbnail for a bookmark

    public Task SetThumbnailAsync(Bookmark bookmark, string imagePath)
    {
      //Set the thumbnail to an image on disk, ie. C:\Pictures\MyPicture.png.
      BitmapImage image = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
      return QueuedTask.Run(() => bookmark.SetThumbnail(image));
    }

    #endregion

    // cref: ArcGIS.Desktop.Mapping.Bookmark.Update(ArcGIS.Desktop.Mapping.MapView)
    #region Update Bookmark
    public Task UpdateBookmarkAsync(Bookmark bookmark)
    {
      return QueuedTask.Run(() =>
      {
        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return;

        //Update the bookmark using the active map view.
        bookmark.Update(mapView);
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.Bookmark.GetDefinition
    // cref: ArcGIS.Core.CIM.CIMBookmark
    // cref: ArcGIS.Core.CIM.CIMBookmark.Camera
    // cref: ArcGIS.Core.CIM.CIMBookmark.Location
    // cref: ArcGIS.Desktop.Mapping.Bookmark.SetDefinition(ArcGIS.Core.CIM.CIMBookmark)
    #region Update Extent for a Bookmark
    public Task UpdateBookmarkExtentAsync(Bookmark bookmark, ArcGIS.Core.Geometry.Envelope envelope)
    {
      return QueuedTask.Run(() =>
      {
        //Get the bookmark's definition
        var bookmarkDef = bookmark.GetDefinition();

        //Modify the bookmark's location
        bookmarkDef.Location = envelope;

        //Clear the camera as it is no longer valid.
        bookmarkDef.Camera = null;

        //Set the bookmark definition
        bookmark.SetDefinition(bookmarkDef);
      });
    }
    #endregion


    #region ProSnippet Group: Time
    #endregion

    // cref: ArcGIS.Desktop.Mapping.TimeDelta
    // cref: ArcGIS.Desktop.Mapping.TimeDelta.#ctor(System.Double, ArcGIS.Desktop.Mapping.TimeUnit)
    // cref: ArcGIS.Desktop.Mapping.TimeUnit
    // cref: ArcGIS.Desktop.Mapping.MapView.Time
    // cref: ArcGIS.Desktop.Mapping.TimeRange
    // cref: ArcGIS.Desktop.Mapping.TimeRange.Offset(ArcGIS.Desktop.Mapping.TimeDelta)
    #region Step forward in time by 1 month

    public void StepMapTime()
    {
      //Get the active view
      MapView mapView = MapView.Active;
      if (mapView == null)
        return;

      //Step current map time forward by 1 month
      TimeDelta timeDelta = new TimeDelta(1, TimeUnit.Months);
      mapView.Time = mapView.Time.Offset(timeDelta);
    }

    #endregion

    public void DisableTime()
    {
      // cref: ArcGIS.Desktop.Mapping.MapView.Time
      // cref: ArcGIS.Desktop.Mapping.TimeRange.Start
      // cref: ArcGIS.Desktop.Mapping.TimeRange.End
      #region  Disable time in the map. 
      MapView.Active.Time.Start = null;
      MapView.Active.Time.End = null;
      #endregion
    }


    #region ProSnippet Group: Animations
    #endregion

    // cref: ArcGIS.Desktop.Mapping.Map.Animation
    // cref: ArcGIS.Desktop.Mapping.Animation
    // cref: ArcGIS.Desktop.Mapping.Animation.Duration
    // cref: ArcGIS.Desktop.Mapping.Animation.ScaleDuration(System.Double)
    #region Set Animation Length
    public void SetAnimationLength(TimeSpan length)
    {
      var mapView = MapView.Active;
      if (mapView != null)
        return;

      var animation = mapView.Map.Animation;
      var duration = animation.Duration;
      if (duration == TimeSpan.Zero)
        return;

      var factor = length.TotalSeconds / duration.TotalSeconds;
      animation.ScaleDuration(factor);
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.Map.Animation
    // cref: ArcGIS.Desktop.Mapping.Animation
    // cref: ArcGIS.Desktop.Mapping.Animation.Duration
    // cref: ArcGIS.Desktop.Mapping.Animation.ScaleDuration(System.TimeSpan,System.TimeSpan,System.Double)
    #region Scale Animation
    public void ScaleAnimationAfterTime(TimeSpan afterTime, TimeSpan length)
    {
      var mapView = MapView.Active;
      if (mapView != null)
        return;

      var animation = mapView.Map.Animation;
      var duration = animation.Duration;
      if (duration == TimeSpan.Zero || duration <= afterTime)
        return;

      var factor = length.TotalSeconds / (duration.TotalSeconds - afterTime.TotalSeconds);
      animation.ScaleDuration(afterTime, duration, factor);
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.Map.Animation
    // cref: ArcGIS.Desktop.Mapping.Animation
    // cref: ArcGIS.Desktop.Mapping.Animation.Tracks
    // cref: ArcGIS.Desktop.Mapping.CameraTrack
    // cref: ArcGIS.Desktop.Mapping.Track.Keyframes
    // cref: ArcGIS.Desktop.Mapping.CameraKeyframe
    #region Camera Keyframes
    public List<CameraKeyframe> GetCameraKeyframes()
    {
      var mapView = MapView.Active;
      if (mapView != null)
        return null;

      var animation = mapView.Map.Animation;
      var cameraTrack = animation.Tracks.OfType<CameraTrack>().First(); //There will always be only 1 CameraTrack in the animation.
      return cameraTrack.Keyframes.OfType<CameraKeyframe>().ToList();
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.Animation.NumberOfFrames
    // cref: ArcGIS.Desktop.Mapping.ViewAnimation.GetCameraAtTime(System.TimeSpan)
    #region Interpolate Camera
    public Task<List<Camera>> GetInterpolatedCameras()
    {
      //Return the collection representing the camera for each frame in animation.
      return QueuedTask.Run(() =>
      {
        var mapView = MapView.Active;
        if (mapView != null || mapView.Animation == null)
          return null;

        var animation = mapView.Map.Animation;

        var cameras = new List<Camera>();
        //We will use ticks here rather than milliseconds to get the highest precision possible.
        var ticksPerFrame = Convert.ToInt64(animation.Duration.Ticks / (animation.NumberOfFrames - 1));
        for (int i = 0; i < animation.NumberOfFrames; i++)
        {
          var time = TimeSpan.FromTicks(i * ticksPerFrame);
          //Because of rounding for ticks the last calculated time may be greating than the duration.
          if (time > animation.Duration)
            time = animation.Duration;
          cameras.Add(mapView.Animation.GetCameraAtTime(time));
        }
        return cameras;
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.TimeRange
    // cref: ArcGIS.Desktop.Mapping.Animation.NumberOfFrames
    // cref: ArcGIS.Desktop.Mapping.ViewAnimation.GetCurrentTimeAtTime(System.TimeSpan)
    #region Interpolate Time
    public Task<List<TimeRange>> GetInterpolatedMapTimes()
    {
      //Return the collection representing the map time for each frame in animation.
      return QueuedTask.Run(() =>
      {
        var mapView = MapView.Active;
        if (mapView != null || mapView.Animation == null)
          return null;

        var animation = mapView.Map.Animation;

        var timeRanges = new List<TimeRange>();
        //We will use ticks here rather than milliseconds to get the highest precision possible.
        var ticksPerFrame = Convert.ToInt64(animation.Duration.Ticks / (animation.NumberOfFrames - 1));
        for (int i = 0; i < animation.NumberOfFrames; i++)
        {
          var time = TimeSpan.FromTicks(i * ticksPerFrame);
          //Because of rounding for ticks the last calculated time may be greating than the duration.
          if (time > animation.Duration)
            time = animation.Duration;
          timeRanges.Add(mapView.Animation.GetCurrentTimeAtTime(time));
        }
        return timeRanges;
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.Range
    // cref: ArcGIS.Desktop.Mapping.Animation.NumberOfFrames
    // cref: ArcGIS.Desktop.Mapping.ViewAnimation.GetCurrentRangeAtTime(System.TimeSpan)
    #region Interpolate Range
    public Task<List<ArcGIS.Desktop.Mapping.Range>> GetInterpolatedMapRanges()
    {
      //Return the collection representing the map time for each frame in animation.
      return QueuedTask.Run(() =>
      {
        var mapView = MapView.Active;
        if (mapView != null || mapView.Animation == null)
          return null;

        var animation = mapView.Map.Animation;

        var ranges = new List<ArcGIS.Desktop.Mapping.Range>();
        //We will use ticks here rather than milliseconds to get the highest precision possible.
        var ticksPerFrame = Convert.ToInt64(animation.Duration.Ticks / (animation.NumberOfFrames - 1));
        for (int i = 0; i < animation.NumberOfFrames; i++)
        {
          var time = TimeSpan.FromTicks(i * ticksPerFrame);
          //Because of rounding for ticks the last calculated time may be greeting than the duration.
          if (time > animation.Duration)
            time = animation.Duration;
          ranges.Add(mapView.Animation.GetCurrentRangeAtTime(time));
        }
        return ranges;
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.Animation
    // cref: ArcGIS.Desktop.Mapping.Animation.Tracks
    // cref: ArcGIS.Desktop.Mapping.CameraTrack
    // cref: ArcGIS.Desktop.Mapping.CameraTrack.CreateKeyframe(ArcGIS.Desktop.Mapping.Camera,System.TimeSpan,ArcGIS.Core.CIM.AnimationTransition)
    // cref: ArcGIS.Core.CIM.AnimationTransition
    #region Create Camera Keyframe
    public void CreateCameraKeyframe(TimeSpan atTime)
    {
      var mapView = MapView.Active;
      if (mapView != null)
        return;

      var animation = mapView.Map.Animation;
      var cameraTrack = animation.Tracks.OfType<CameraTrack>().First(); //There will always be only 1 CameraTrack in the animation.
      cameraTrack.CreateKeyframe(mapView.Camera, atTime, ArcGIS.Core.CIM.AnimationTransition.FixedArc);
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.Animation
    // cref: ArcGIS.Desktop.Mapping.Animation.Tracks
    // cref: ArcGIS.Desktop.Mapping.TimeTrack
    // cref: ArcGIS.Core.CIM.AnimationTransition
    // cref: ArcGIS.Desktop.Mapping.TimeTrack.CreateKeyframe(ArcGIS.Desktop.Mapping.TimeRange,System.TimeSpan,ArcGIS.Core.CIM.AnimationTransition)
    #region Create Time Keyframe
    public void CreateTimeKeyframe(TimeSpan atTime)
    {
      var mapView = MapView.Active;
      if (mapView != null)
        return;

      var animation = mapView.Map.Animation;
      var timeTrack = animation.Tracks.OfType<TimeTrack>().First(); //There will always be only 1 TimeTrack in the animation.
      timeTrack.CreateKeyframe(mapView.Time, atTime, ArcGIS.Core.CIM.AnimationTransition.Linear);
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.Animation
    // cref: ArcGIS.Desktop.Mapping.Animation.Tracks
    // cref: ArcGIS.Desktop.Mapping.RangeTrack
    // cref: ArcGIS.Core.CIM.AnimationTransition
    // cref: ArcGIS.Desktop.Mapping.RangeTrack.CreateKeyframe(ArcGIS.Desktop.Mapping.Range,System.TimeSpan,ArcGIS.Core.CIM.AnimationTransition)
    #region Create Range Keyframe
    public void CreateRangeKeyframe(ArcGIS.Desktop.Mapping.Range range, TimeSpan atTime)
    {
      var mapView = MapView.Active;
      if (mapView != null)
        return;

      var animation = mapView.Map.Animation;
      var rangeTrack = animation.Tracks.OfType<RangeTrack>().First(); //There will always be only 1 RangeTrack in the animation.
      rangeTrack.CreateKeyframe(range, atTime, ArcGIS.Core.CIM.AnimationTransition.Linear);
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.Animation
    // cref: ArcGIS.Desktop.Mapping.Animation.Tracks
    // cref: ArcGIS.Desktop.Mapping.LayerTrack
    // cref: ArcGIS.Core.CIM.AnimationTransition
    // cref: ArcGIS.Desktop.Mapping.LayerTrack.CreateKeyframe(ArcGIS.Desktop.Mapping.Layer,System.TimeSpan,System.Boolean,System.Double,ArcGIS.Core.CIM.AnimationTransition)
    #region Create Layer Keyframe
    public void CreateLayerKeyframe(Layer layer, double transparency, TimeSpan atTime)
    {
      var mapView = MapView.Active;
      if (mapView != null)
        return;

      var animation = mapView.Map.Animation;
      var layerTrack = animation.Tracks.OfType<LayerTrack>().First(); //There will always be only 1 LayerTrack in the animation.
      layerTrack.CreateKeyframe(layer, atTime, true, transparency, ArcGIS.Core.CIM.AnimationTransition.Linear);
    }
    #endregion




    #region ProSnippet Group: Graphic overlay
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MappingExtensions.AddOverlay(ArcGIS.Desktop.Mapping.MapView, ArcGIS.Core.CIM.CIMGraphic, System.Double)
    // cref: ArcGIS.Desktop.Mapping.MappingExtensions.AddOverlay(ArcGIS.Desktop.Mapping.MapView, ArcGIS.Core.Geometry.Geometry, ArcGIS.Core.CIM.CIMSymbolReference, System.Double)
    // cref: ArcGIS.Desktop.Mapping.MappingExtensions.AddOverlay(ArcGIS.Desktop.Mapping.MapView, ArcGIS.Core.Geometry.Geometry, ArcGIS.Core.CIM.CIMSymbolReference, System.Double, System.Double)
    #region Graphic Overlay
    //Defined elsewhere
    private IDisposable _graphic = null;
    public async void GraphicOverlaySnippetTest()
    {
      // get the current mapview and point
      var mapView = MapView.Active;
      if (mapView == null)
        return;
      var myextent = mapView.Extent;
      var point = myextent.Center;

      // add point graphic to the overlay at the center of the mapView
      _graphic = await QueuedTask.Run(() =>
      {
        //add these to the overlay
        return mapView.AddOverlay(point,
            SymbolFactory.Instance.ConstructPointSymbol(
                    ColorFactory.Instance.RedRGB, 30.0, SimpleMarkerStyle.Star).MakeSymbolReference());
      });

      // update the overlay with new point graphic symbol
      MessageBox.Show("Now to update the overlay...");
      await QueuedTask.Run(() =>
      {
        mapView.UpdateOverlay(_graphic, point, SymbolFactory.Instance.ConstructPointSymbol(
                                      ColorFactory.Instance.BlueRGB, 20.0, SimpleMarkerStyle.Circle).MakeSymbolReference());
      });

      // clear the overlay display by disposing of the graphic
      MessageBox.Show("Now to clear the overlay...");
      _graphic.Dispose();

    }
    #endregion

    public void CIMPictureGraphicOverlay(Geometry geometry, ArcGIS.Core.Geometry.Envelope envelope)
    {
      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.AddOverlay(ArcGIS.Desktop.Mapping.MapView, ArcGIS.Core.CIM.CIMGraphic, System.Double)
      // cref: ArcGIS.Core.CIM.CIMPictureGraphic
      // cref: ArcGIS.Core.CIM.CIMPictureGraphic.PictureURL
      // cref: ArcGIS.Core.CIM.CIMPictureGraphic.Shape
      #region Graphic Overlay with CIMPictureGraphic

      // get the current mapview
      var mapView = MapView.Active;
      if (mapView == null)
        return;

			// Use SourceURL for the URL to the image content. For
			// a local file, use a file path. For a web/internet file
      // location, use its URL
			//
			// Supported image types are:
      //
      // png, jpg, tiff, bmp, gif, svg

			var pictureGraphic = new CIMPictureGraphic
      {
        SourceURL = @"C:\Images\MyImage.png",
        Shape = envelope
      };

      IDisposable _graphic = mapView.AddOverlay(pictureGraphic);
      #endregion
    }


    // cref: ArcGIS.Desktop.Mapping.MapTool.AddOverlayAsync(ArcGIS.Core.Geometry.Geometry, ArcGIS.Core.CIM.CIMSymbolReference, System.Double, System.Double)
    // cref: ArcGIS.Desktop.Mapping.MappingExtensions.AddOverlay(ArcGIS.Desktop.Mapping.MapView, ArcGIS.Core.CIM.CIMGraphic, System.Double)
    #region Add overlay graphic with text
    internal class AddOverlayWithText : MapTool
    {
      private IDisposable _graphic = null;
      private CIMLineSymbol _lineSymbol = null;
      public AddOverlayWithText()
      {
        IsSketchTool = true;
        SketchType = SketchGeometryType.Line;
        SketchOutputMode = SketchOutputMode.Map;
      }

      protected override async Task<bool> OnSketchCompleteAsync(Geometry geometry)
      {
        //Add an overlay graphic to the map view
        _graphic = await this.AddOverlayAsync(geometry, _lineSymbol.MakeSymbolReference());

        //define the text symbol
        var textSymbol = new CIMTextSymbol();
        //define the text graphic
        var textGraphic = new CIMTextGraphic();

        await QueuedTask.Run(() =>
        {
          //Create a simple text symbol
          textSymbol = SymbolFactory.Instance.ConstructTextSymbol(ColorFactory.Instance.BlackRGB, 8.5, "Corbel", "Regular");
          //Sets the geometry of the text graphic
          textGraphic.Shape = geometry;
          //Sets the text string to use in the text graphic
          textGraphic.Text = "This is my line";
          //Sets symbol to use to draw the text graphic
          textGraphic.Symbol = textSymbol.MakeSymbolReference();
          //Draw the overlay text graphic
          _graphic = this.ActiveMapView.AddOverlay(textGraphic);
        });

        return true;
      }
    }
    #endregion


    #region ProSnippet Group: Tools
    #endregion


    // cref: ArcGIS.Desktop.Mapping.MapTool.SketchSymbol
    #region Change symbol for a sketch tool

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
          var polygonSymbol = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.CreateRGBColor(24, 69, 59),
                              SimpleFillStyle.Solid,
                            SymbolFactory.Instance.ConstructStroke(ColorFactory.Instance.BlackRGB, 1.0, SimpleLineStyle.Dash));
          SketchSymbol = polygonSymbol.MakeSymbolReference();
        });
      }
    }

    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapTool.OnToolMouseDown(ArcGIS.Desktop.Mapping.MapViewMouseButtonEventArgs)
    // cref: ArcGIS.Desktop.Mapping.MapViewMouseButtonEventArgs
    // cref: ArcGIS.Desktop.Mapping.MapTool.HandleMouseDownAsync(ArcGIS.Desktop.Mapping.MapViewMouseButtonEventArgs)
    // cref: ArcGIS.Desktop.Mapping.MapViewMouseButtonEventArgs.ClientPoint
    // cref: ArcGIS.Desktop.Mapping.MapView.ClientToMap(System.Windows.Point)
    #region Create a tool to the return coordinates of the point clicked in the map

    internal class GetMapCoordinates : MapTool
    {
      protected override void OnToolMouseDown(MapViewMouseButtonEventArgs e)
      {
        if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
          e.Handled = true; //Handle the event args to get the call to the corresponding async method
      }

      protected override Task HandleMouseDownAsync(MapViewMouseButtonEventArgs e)
      {
        return QueuedTask.Run(() =>
        {
          //Convert the clicked point in client coordinates to the corresponding map coordinates.
          var mapPoint = MapView.Active.ClientToMap(e.ClientPoint);
          ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(string.Format("X: {0} Y: {1} Z: {2}",
                      mapPoint.X, mapPoint.Y, mapPoint.Z), "Map Coordinates");
        });
      }
    }

    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapView.GetFeatures(ArcGIS.Core.Geometry.Geometry, bool, bool)
    // cref: ArcGIS.Desktop.Mapping.MapView.FlashFeature(ArcGIS.Desktop.Mapping.SelectionSet)
    #region Create a tool to identify the features that intersect the sketch geometry

    internal class CustomIdentify : MapTool
    {
      public CustomIdentify()
      {
        IsSketchTool = true;
        SketchType = SketchGeometryType.Rectangle;

        //To perform a interactive selection or identify in 3D or 2D, sketch must be created in screen coordinates.
        SketchOutputMode = SketchOutputMode.Screen;
      }

      protected override Task<bool> OnSketchCompleteAsync(Geometry geometry)
      {
        return QueuedTask.Run(() =>
        {
          var mapView = MapView.Active;
          if (mapView == null)
            return true;

          //Get all the features that intersect the sketch geometry and flash them in the view. 
          var results = mapView.GetFeatures(geometry);
          mapView.FlashFeature(results);

          var debug = String.Join("\n", results.ToDictionary()
                        .Select(kvp => String.Format("{0}: {1}", kvp.Key.Name, kvp.Value.Count())));
          System.Diagnostics.Debug.WriteLine(debug);
          return true;
        });
      }
    }

    #endregion

    internal class Resource1
    {
      public static byte[] red_cursor = new byte[256];
    }

    // cref: ArcGIS.Desktop.Framework.Contracts.Tool.Cursor
    #region Change the cursor of a Tool
    internal class CustomMapTool : MapTool
    {
      public CustomMapTool()
      {
        IsSketchTool = true;
        SketchType = SketchGeometryType.Rectangle;
        SketchOutputMode = SketchOutputMode.Map;
        //A custom cursor file as an embedded resource
        var cursorEmbeddedResource = new Cursor(new MemoryStream(Resource1.red_cursor));
        //A built in system cursor
        var systemCursor = System.Windows.Input.Cursors.ArrowCD;
        //Set the "CustomMapTool's" Cursor property to either one of the cursors defined above
        Cursor = cursorEmbeddedResource;
        //or
        Cursor = systemCursor;
      }
      #endregion
      protected override Task OnToolActivateAsync(bool active)
      {
        return base.OnToolActivateAsync(active);
      }

      protected override Task<bool> OnSketchCompleteAsync(Geometry geometry)
      {
        return base.OnSketchCompleteAsync(geometry);
      }
    }


    // cref: ArcGIS.Desktop.Mapping.MapTool.ControlID
    // cref: ArcGIS.Desktop.Mapping.MapTool.EmbeddableControl
    #region Tool with an Embeddable Control

    // Using the Visual Studio SDK templates, add a MapTool and an EmbeddableControl
    // The EmbeddableControl is registered in the "esri_embeddableControls" category in the config.daml file
    // 
    //  <categories>
    //    <updateCategory refID = "esri_embeddableControls" >
    //      <insertComponent id="mapTool_EmbeddableControl" className="EmbeddableControl1ViewModel">
    //        <content className = "EmbeddableControl1View" />
    //      </insertComponent>
    //    <updateCategory>
    //  </categories>
    internal class MapTool_WithControl : MapTool
    {
      public MapTool_WithControl()
      {
        // substitute this string with the daml ID of the embeddable control you added
        ControlID = "mapTool_EmbeddableControl";
      }

      protected override void OnToolMouseDown(MapViewMouseButtonEventArgs e)
      {
        e.Handled = true;
      }

      protected override Task HandleMouseDownAsync(MapViewMouseButtonEventArgs e)
      {
        //Get the instance of the ViewModel
        var vm = EmbeddableControl;
        if (vm == null)
          return Task.FromResult(0);

        // cast vm to your viewModel in order to access your properties

        //Get the map coordinates from the click point and set the property on the ViewMode.
        return QueuedTask.Run(() =>
        {
          var mapPoint = MapView.Active.ClientToMap(e.ClientPoint);
          string clickText = string.Format("X: {0}, Y: {1}, Z: {2}", mapPoint.X, mapPoint.Y, mapPoint.Z);
        });
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapTool.OverlayControlID
    // cref: ArcGIS.Desktop.Mapping.MapTool.OverlayEmbeddableControl
    #region Tool with an Overlay Embeddable Control

    // Using the Visual Studio SDK templates, add a MapTool and an EmbeddableControl
    // The EmbeddableControl is registered in the "esri_embeddableControls" category in the config.daml file
    // 
    //  <categories>
    //    <updateCategory refID = "esri_embeddableControls" >
    //      <insertComponent id="mapTool_EmbeddableControl" className="EmbeddableControl1ViewModel">
    //        <content className = "EmbeddableControl1View" />
    //      </insertComponent>
    //    <updateCategory>
    //  </categories>

    internal class MapTool_WithOverlayControl : MapTool
    {
      public MapTool_WithOverlayControl()
      {
        // substitute this string with the daml ID of the embeddable control you added
        OverlayControlID = "mapTool_EmbeddableControl";
      }

      protected override void OnToolMouseDown(MapViewMouseButtonEventArgs e)
      {
        e.Handled = true;
      }

      protected override Task HandleMouseDownAsync(MapViewMouseButtonEventArgs e)
      {
        //Get the instance of the ViewModel
        var vm = OverlayEmbeddableControl;
        if (vm == null)
          return Task.FromResult(0);

        // cast vm to your viewModel in order to access your properties

        //Get the map coordinates from the click point and set the property on the ViewMode.
        return QueuedTask.Run(() =>
        {
          var mapPoint = MapView.Active.ClientToMap(e.ClientPoint);
          string clickText = string.Format("X: {0}, Y: {1}, Z: {2}", mapPoint.X, mapPoint.Y, mapPoint.Z);
        });
      }
    }
    #endregion


    #region ProSnippet Group: Mapping Options
    #endregion
    public void SelectionOptions()
    {
      // cref: ArcGIS.Desktop.Core.ApplicationOptions.SelectionOptions
      // cref: ArcGIS.Desktop.Core.SelectionOptions
      // cref:ArcGIS.Desktop.Mapping.SelectionMethod
      // cref:ArcGIS.Desktop.Mapping.SelectionCombinationMethod
      #region Get/Set Selection Options
      var options = ApplicationOptions.SelectionOptions;

      QueuedTask.Run(() =>
      {
        var defaultColor = options.DefaultSelectionColor;

        var color = options.SelectionColor as CIMRGBColor;
        options.SetSelectionColor(ColorFactory.Instance.CreateRGBColor(255, 0, 0));


        var defaultFill = options.DefaultSelectionFillColor;
        var fill = options.SelectionFillColor;
        var isHatched = options.IsSelectionFillHatched;
        options.SetSelectionFillColor(ColorFactory.Instance.CreateRGBColor(100, 100, 0));
        if (!isHatched)
          options.SetSelectionFillIsHatched(true);

        var showSelectionChip = options.ShowSelectionChip;
        options.SetShowSelectionChip(!showSelectionChip);

        var showSelectionGraphic = options.ShowSelectionGraphic;
        options.SetShowSelectionGraphic(!showSelectionGraphic);

        var saveSelection = options.SaveSelection;
        options.SetSaveSelection(!saveSelection);

        var defaultTol = options.DefaultSelectionTolerance;
        var tol = options.SelectionTolerance;
        options.SetSelectionTolerance(2 * defaultTol);

        // extension methods available 
        var selMethod = options.SelectionMethod;
        options.SetSelectionMethod(SelectionMethod.Contains);

        var combMethod = options.CombinationMethod;
        options.SetCombinationMethod(SelectionCombinationMethod.Add);

        // note that the following SelectionCombinationMethod is not supported
        //options.SetCombinationMethod(SelectionCombinationMethod.XOR);
      });
      #endregion

    }

    public void TableOptions()
    {
      // cref: ArcGIS.Desktop.Core.ApplicationOptions.TableOptions
      // cref: ArcGIS.Desktop.Core.TableOptions
      // cref:ArcGIS.Core.CIM.TableRowHeightType
      #region Get/Set Table Options
      var options = ApplicationOptions.TableOptions;

      var hideAddNewRow = options.HideAddNewRow;
      options.HideAddNewRow = !hideAddNewRow;

      var overrides = options.HonorSelectionColorOverrides;
      options.HonorSelectionColorOverrides = !overrides;

      var activateMapView = options.ActivateMapViewAfterOperations;
      options.ActivateMapViewAfterOperations = !activateMapView;

      var defaultFontTName = options.DefaultFontName;
      var fontName = options.FontName;
      if (options.IsValidFontName("Arial"))
        options.FontName = "Arial";

      var defaultFontSize = options.DefaultFontSize;
      var fontSize = options.FontSize;
      if (options.IsValidFontSize(10))
        options.FontSize = 10;

      var heightType = options.ColumnHeaderHeightType;
      options.ColumnHeaderHeightType = TableRowHeightType.Double;

      var rowHeightType = options.RowHeightType;
      options.RowHeightType = TableRowHeightType.Single;

      var defaultColor = options.DefaultHighlightColor;
      var color = options.HighlightColor;
      QueuedTask.Run(() =>
      {
        options.SetHighlightColor(ColorFactory.Instance.CreateRGBColor(0, 0, 255));
      });
      #endregion

    }
  }
}
