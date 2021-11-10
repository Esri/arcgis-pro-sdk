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

namespace Snippets
{
    internal class ProSnippet
    {



        #region ProSnippet Group: Maps
        #endregion
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

        #region Rotate the map view

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
        private static void ClearSelectionMap()
        {

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
      #region Calculate Selection tolerance in map units
      //Selection tolerance for the map in pixels
      var selectionTolerance = SelectionEnvironment.SelectionTolerance;
      QueuedTask.Run(() => {
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

    private static async void AddMapViewOverlayControl ()
         {
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
                var selectedFeatures = mapView.Map.GetSelection()
                    .Where(kvp => kvp.Key is BasicFeatureLayer)
                    .ToDictionary(kvp => (BasicFeatureLayer)kvp.Key, kvp => kvp.Value);

                //Flash the collection of features.
                mapView.FlashFeature(selectedFeatures);
            });
        }

        #endregion

        
        private void CheckLayerVisiblityInView()
        {
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
    #region ProSnippet Group: Features
    #endregion
    private static void Masking()
        {
            QueuedTask.Run(() => {
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

        #region ProSnippet Group: Zoom
        #endregion

        #region Zoom to an extent

        public async Task<bool> ZoomToExtentAsync(double xMin, double yMin, double xMax, double yMax,
            ArcGIS.Core.Geometry.SpatialReference spatialReference)
        {
            //Get the active map view.
            var mapView = MapView.Active;
            if (mapView == null)
                return false;

            //Create the envelope
            var envelope =
                await
                    QueuedTask.Run(
                        () =>
                            ArcGIS.Core.Geometry.EnvelopeBuilder.CreateEnvelope(xMin, yMin, xMax, yMax, spatialReference));

            //Zoom the view to a given extent.
            return await mapView.ZoomToAsync(envelope, TimeSpan.FromSeconds(2));
        }

        #endregion

        private static void ZoomToGeographicCoordinates(double x, double y, double buffer_size)
        {
            QueuedTask.Run(() => {
                #region Zoom to a specified point
                //Note: Run within QueuedTask
                //Create a point
                var pt = MapPointBuilder.CreateMapPoint(x, y, SpatialReferenceBuilder.CreateSpatialReference(4326));
                //Buffer it - for purpose of zoom
                var poly = GeometryEngine.Instance.Buffer(pt, buffer_size);

                //do we need to project the buffer polygon?

                if (!MapView.Active.Map.SpatialReference.IsEqual(poly.SpatialReference))
                {
                    //project the polygon
                    poly = GeometryEngine.Instance.Project(poly, MapView.Active.Map.SpatialReference);
                }

                //Zoom - add in a delay for animation effect
                MapView.Active.ZoomTo(poly, new TimeSpan(0, 0, 0, 3));
                #endregion
            });
        }

        #region Zoom to visible layers

        public Task<bool> ZoomToVisibleLayersAsync()
        {
            //Get the active map view.
            var mapView = MapView.Active;
            if (mapView == null)
                return Task.FromResult(false);

            //Zoom to all visible layers in the map.
            var visibleLayers = mapView.Map.Layers.Where(l => l.IsVisible);
            return mapView.ZoomToAsync(visibleLayers);
        }

        #endregion

        #region Zoom to selected layers in TOC

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

        #region Zoom to previous camera

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
        #region Project camera into a new spatial reference

        public Task<Camera> ProjectCamera(Camera camera, ArcGIS.Core.Geometry.SpatialReference spatialReference)
        {
            return QueuedTask.Run(() =>
            {
                var mapPoint = MapPointBuilder.CreateMapPoint(camera.X, camera.Y, camera.Z, camera.SpatialReference);
                var newPoint = GeometryEngine.Instance.Project(mapPoint, spatialReference) as MapPoint;
                var newCamera = new Camera()
                {
                    X = newPoint.X,
                    Y = newPoint.Y,
                    Z = newPoint.Z,
                    Scale = camera.Scale,
                    Pitch = camera.Pitch,
                    Heading = camera.Heading,
                    Roll = camera.Roll,
                    Viewpoint = camera.Viewpoint,
                    SpatialReference = spatialReference
                };
                return newCamera;
            });
        }

        #endregion

        #region Zoom to a bookmark with a given name

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

        #region ProSnippet Group: Bookmarks
        #endregion

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

        #region Get the collection of bookmarks for the project

        public Task<ReadOnlyObservableCollection<Bookmark>> GetProjectBookmarksAsync()
        {
            //Get the collection of bookmarks for the project.
            return QueuedTask.Run(() => Project.Current.GetBookmarks());
        }

        #endregion

        #region Change the thumbnail for a bookmark

        public Task SetThumbnailAsync(Bookmark bookmark, string imagePath)
        {
            //Set the thumbnail to an image on disk, ie. C:\Pictures\MyPicture.png.
            BitmapImage image = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
            return QueuedTask.Run(() => bookmark.SetThumbnail(image));
        }

        #endregion

        #region ProSnippet Group: Time
        #endregion

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
            #region  Disable time in the map. 
            MapView.Active.Time.Start = null;
            MapView.Active.Time.End = null;
            #endregion
        }
        #region ProSnippet Group: Graphic overlay
        #endregion
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
            #region Graphic Overlay with CIMPictureGraphic

            // get the current mapview
            var mapView = MapView.Active;
            if (mapView == null)
                return;

            //Valid formats for PictureURL are:
            // e.g. local file URL:
            // file:///<path>
            // file:///c:/images/symbol.png
            //
            // e.g. network file URL:
            // file://<host>/<path>
            // file://server/share/symbol.png
            //
            // e.g. data URL:
            // data:<mediatype>;base64,<data>
            // data:image/jpeg;base64,/9j/4AAQSkZJRgABAQEASABIAAD/2wBDAAU ...
            //
            // image/bmp
            // image/gif
            // image/jpeg
            // image/png
            // image/tiff
            // image/x-esri-bglf

            var pictureGraphic = new CIMPictureGraphic
            {
                PictureURL = @"file:///C:/Images/MyImage.png",
                Box = envelope
            };
            
            IDisposable _graphic = mapView.AddOverlay(pictureGraphic);
            #endregion
        }
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

                //Show a message box reporting each layer the number of the features.
                MessageBox.Show(
                    String.Join("\n", results.Select(kvp => String.Format("{0}: {1}", kvp.Key.Name, kvp.Value.Count()))),
                    "Identify Result");
                    return true;
                });
            }
        }

        #endregion
   

        #region Change the cursor of a Tool
        internal class CustomMapTool : MapTool
        {
            public CustomMapTool()
            {
                IsSketchTool = true;
                SketchType = SketchGeometryType.Rectangle;
                SketchOutputMode = SketchOutputMode.Map;
                //A custom cursor file as an embedded resource
                var cursorEmbeddedResource = new Cursor(new MemoryStream(MapExploration.Resource1.red_cursor));
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
        
    }
}
