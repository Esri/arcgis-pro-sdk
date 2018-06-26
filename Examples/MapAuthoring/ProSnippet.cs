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
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MapAuthoring.ProSnippet
{
  class ProSnippet
  {
    public void GetActiveMapAsync()
    {
      #region Get the active map

      Map map = MapView.Active.Map;
      #endregion
    }


    public async Task CreateMapAsync(string mapName)
    {

      #region Create a new map with a default basemap layer

      await QueuedTask.Run(() =>
    {
      var map = MapFactory.Instance.CreateMap(mapName, basemap: Basemap.ProjectDefault);
      //TODO: use the map...

    });

      #endregion
    }

    #region Find a map within a project and open it
    public static async Task<Map> FindOpenExistingMapAsync(string mapName)
    {
      return await QueuedTask.Run(async () =>
      {
        Map map = null;
        Project proj = Project.Current;

        //Finding the first project item with name matches with mapName
        MapProjectItem mpi =
            proj.GetItems<MapProjectItem>()
                .FirstOrDefault(m => m.Name.Equals(mapName, StringComparison.CurrentCultureIgnoreCase));
        if (mpi != null)
        {
          map = mpi.GetMap();
          //Opening the map in a mapview
          await ProApp.Panes.CreateMapPaneAsync(map);
        }
        return map;
      });

    }
    #endregion


    public async Task<Map> OpenWebMapAsync()
    {

      #region Open a webmap
      Map map = null;

      //Assume we get the selected webmap from the Project pane's Portal tab
      if (Project.Current.SelectedItems.Count > 0)
      {
        if (MapFactory.Instance.CanCreateMapFrom(Project.Current.SelectedItems[0]))
        {
          map = MapFactory.Instance.CreateMapFromItem(Project.Current.SelectedItems[0]);
          await ProApp.Panes.CreateMapPaneAsync(map);
        }
      }

      #endregion

      return map;
    }

    #region Get Map Panes
    public static IEnumerable<IMapPane> GetMapPanes()
    {
      //Sorted by Map Uri
      return ProApp.Panes.OfType<IMapPane>().OrderBy((mp) => mp.MapView.Map.URI ?? mp.MapView.Map.Name);
    }
    #endregion

    #region Get The Unique List of Maps From the Map Panes
    public static IReadOnlyList<Map> GetMapsFromMapPanes()
    {
      //Gets the unique list of Maps from all the MapPanes.
      //Note: The list of maps retrieved from the MapPanes
      //maybe less than the total number of Maps in the project.
      //It depends on what maps the user has actually opened.
      var mapPanes = ProApp.Panes.OfType<IMapPane>()
                  .GroupBy((mp) => mp.MapView.Map.URI).Select(grp => grp.FirstOrDefault());
      List<Map> uniqueMaps = new List<Map>();
      foreach (var pane in mapPanes)
        uniqueMaps.Add(pane.MapView.Map);
      return uniqueMaps;
    }
    #endregion

    public void ModifyMapAndPane()
    {
            QueuedTask.Run(() => {
                #region Change the Map name
                ////Note: call within QueuedTask.Run()
                MapView.Active.Map.SetName("Test");
                #endregion
            });

            #region Renames the caption of the pane
            ProApp.Panes.ActivePane.Caption = "Caption";
            #endregion

        }
        public List<Layer> FindLayersWithPartialName(string partialName)
    {

      // Find a layers using partial name search

      Map map = MapView.Active.Map;
      IEnumerable<Layer> matches = map.GetLayersAsFlattenedList().Where(l => l.Name.IndexOf(partialName, StringComparison.CurrentCultureIgnoreCase) >= 0);

      // endregion

      List<Layer> layers = new List<Layer>();
      foreach (Layer l in matches)
        layers.Add(l);

      return layers;
    }

    public async Task AddLayerAsync()
    {
      Map map = null;

      #region Create and add a layer to the active map

      /*
* string url = @"c:\data\project.gdb\DEM";  //Raster dataset from a FileGeodatabase
* string url = @"c:\connections\mySDEConnection.sde\roads";  //FeatureClass of a SDE
* string url = @"c:\connections\mySDEConnection.sde\States\roads";  //FeatureClass within a FeatureDataset from a SDE
* string url = @"c:\data\roads.shp";  //Shapefile
* string url = @"c:\data\imagery.tif";  //Image from a folder
* string url = @"c:\data\mySDEConnection.sde\roads";  //.lyrx or .lpkx file
* string url = @"http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/Demographics/ESRI_Census_USA/MapServer";  //map service
* string url = @"http://sampleserver6.arcgisonline.com/arcgis/rest/services/NapervilleShelters/FeatureServer/0";  //FeatureLayer off a map service or feature service
*/
      string url = @"c:\data\project.gdb\roads";  //FeatureClass of a FileGeodatabase

      Uri uri = new Uri(url);
      await QueuedTask.Run(() => LayerFactory.Instance.CreateLayer(uri, MapView.Active.Map));

      #endregion

    }

        public void AddWMSLayer()
        {
            #region Add a WMS service
            // Create a connection to the WMS server
            var serverConnection = new CIMInternetServerConnection { URL = "URL of the WMS service" };
            var connection = new CIMWMSServiceConnection { ServerConnection = serverConnection };

            // Add a new layer to the map
            QueuedTask.Run(() =>
            {
                var layer = LayerFactory.Instance.CreateLayer(connection, MapView.Active.Map);
            });
            #endregion
        }

        public static void MoveLayerTo3D()
        {
            #region Move a layer in the 2D group to the 3D Group in a Local Scene
            //The layer in the 2D group to move to the 3D Group in a Local Scene
            var layer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();
            QueuedTask.Run(() => {
                //Get the layer's definition
                var lyrDefn = layer.GetDefinition() as CIMBasicFeatureLayer;
                //setting this property moves the layer to 3D group in a scene
                lyrDefn.IsFlattened = false;
                //Set the definition back to the layer
                layer.SetDefinition(lyrDefn);
            });
            #endregion
        }


        private Task CreateNewElevationSurface()
        {
            return ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => {
                #region Create a New Elevation Surface
                //Note: call within QueuedTask.Run()
                //Define a ServiceConnection to use for the new Elevation surface
                var serverConnection = new CIMInternetServerConnection
                {
                    Anonymous = true,
                    HideUserProperty = true,
                    URL = "https://elevation.arcgis.com/arcgis/services"
                };
                CIMAGSServiceConnection serviceConnection = new CIMAGSServiceConnection
                {
                    ObjectName = "WorldElevation/Terrain",
                    ObjectType = "ImageServer",
                    URL = "https://elevation.arcgis.com/arcgis/services/WorldElevation/Terrain/ImageServer",
                    ServerConnection = serverConnection
                };
                //Defines a new elevation source set to the CIMAGSServiceConnection defined above
                var newElevationSource = new ArcGIS.Core.CIM.CIMElevationSource
                {
                    VerticalUnit = ArcGIS.Core.Geometry.LinearUnit.Meters,
                    DataConnection = serviceConnection,
                    Name = "WorldElevation/Terrain",
                    Visibility = true
                };
                //The elevation surface
                var newElevationSurface = new ArcGIS.Core.CIM.CIMMapElevationSurface
                {
                    Name = "New Elevation Surface",
                    BaseSources = new ArcGIS.Core.CIM.CIMElevationSource[1] { newElevationSource },
                    Visibility = true,
                    ElevationMode = ElevationMode.CustomSurface,
                    VerticalExaggeration = 1,
                    EnableSurfaceShading = false,
                    SurfaceTINShadingMode = SurfaceTINShadingMode.Smooth,
                    Offset = 0,
                    Expanded = false,
                    MapElevationID = "{3DEC3CC5-7C69-4132-A700-DCD5BDED14D6}"
                };
                //Get the active map
                var map = MapView.Active.Map;
                //Get the active map's definition
                var definition = map.GetDefinition();
                //Get the elevation surfaces defined in the map
                var listOfElevationSurfaces = definition.ElevationSurfaces.ToList();
                //Add the new elevation surface 
                listOfElevationSurfaces.Add(newElevationSurface);
                //Set the map definitions to ElevationSurface (this has the new elevation surface)
                definition.ElevationSurfaces = listOfElevationSurfaces.ToArray();
                //Set the map definition
                map.SetDefinition(definition);
                #endregion
            });
        }

        private Task SetElevationSurfaceToLayer(FeatureLayer featureLayer)
        {
            return QueuedTask.Run(() =>
            {
                #region Set a custom elevation surface to a Z-Aware layer
                
                //Define the custom elevation surface to use
                var layerElevationSurface = new CIMLayerElevationSurface
                {
                    MapElevationID = "{3DEC3CC5-7C69-4132-A700-DCD5BDED14D6}"
                };
                //Get the layer's definition
                var lyrDefn = featureLayer.GetDefinition() as CIMBasicFeatureLayer; 
                //Set the layer's elevation surface
                lyrDefn.LayerElevation = layerElevationSurface;
                //Set the layer's definition
                featureLayer.SetDefinition(lyrDefn);
                #endregion
            });
        }

        private static async Task<SurfaceZsResult> GetZValue()
        {
            #region Get Z values from a surface
            var geometry = await QueuedTask.Run<Geometry>(() => {
                Geometry mapCentergeometry = MapView.Active.Map.CalculateFullExtent().Center;
                return mapCentergeometry;
            });
            //Pass any Geometry type to GetZsFromSurfaceAsync
            var surfaceZResult = await MapView.Active.Map.GetZsFromSurfaceAsync(geometry);
            return surfaceZResult;
            #endregion
        }
        public async Task AddFeatureLayerClasBreaksAsync()
    {

      #region Create a feature layer with class breaks renderer with defaults
      await QueuedTask.Run(() =>
        LayerFactory.Instance.CreateFeatureLayer(
          new Uri(@"c:\data\countydata.gdb\counties"),
          MapView.Active.Map,
          layerName: "Population Density (sq mi) Year 2010",
          rendererDefinition: new GraduatedColorsRendererDefinition("POP10_SQMI")
        )
      );
      #endregion
    }

    public async Task AddFeatureLayerClasBreaksExAsync()
    {

      #region Create a feature layer with class breaks renderer

      string colorBrewerSchemesName = "ColorBrewer Schemes (RGB)";
      StyleProjectItem style = Project.Current.GetItems<StyleProjectItem>().First(s => s.Name == colorBrewerSchemesName);
      string colorRampName = "Greens (Continuous)";
      IList<ColorRampStyleItem> colorRampList = await QueuedTask.Run(() =>
      {
        return style.SearchColorRamps(colorRampName);
      });
      ColorRampStyleItem colorRamp = colorRampList[0];

      await QueuedTask.Run(() =>
      {
        GraduatedColorsRendererDefinition gcDef = new GraduatedColorsRendererDefinition()
        {
          ClassificationField = "CROP_ACR07",
          ClassificationMethod = ArcGIS.Core.CIM.ClassificationMethod.NaturalBreaks,
          BreakCount = 6,
          ColorRamp = colorRamp.ColorRamp,
          SymbolTemplate = SymbolFactory.Instance.ConstructPolygonSymbol(
                            ColorFactory.Instance.GreenRGB, SimpleFillStyle.Solid, null).MakeSymbolReference(),
          ExclusionClause = "CROP_ACR07 = -99",
          ExclusionSymbol = SymbolFactory.Instance.ConstructPolygonSymbol(
                            ColorFactory.Instance.RedRGB, SimpleFillStyle.Solid, null).MakeSymbolReference(),
          ExclusionLabel = "No yield",
        };

        LayerFactory.Instance.CreateFeatureLayer(new Uri(@"c:\Data\CountyData.gdb\Counties"),
            MapView.Active.Map, layerName: "Crop", rendererDefinition: gcDef);
      });

      #endregion
    }


    public async Task SetUniqueValueRendererAsync()
    {


      #region Set unique value renderer to the selected feature layer of the active map

      await QueuedTask.Run(() =>
      {
        String[] fields = new string[] { "Type" }; //field to be used to retrieve unique values
        CIMPointSymbol pointSym = SymbolFactory.Instance.ConstructPointSymbol(
                  ColorFactory.Instance.GreenRGB, 16.0, SimpleMarkerStyle.Pushpin);  //constructing a point symbol as a template symbol
        CIMSymbolReference symbolPointTemplate = pointSym.MakeSymbolReference();

        //constructing renderer definition for unique value renderer
        UniqueValueRendererDefinition uniqueValueRendererDef = new UniqueValueRendererDefinition(fields, symbolPointTemplate);

        //creating a unique value renderer
        var flyr = MapView.Active.GetSelectedLayers()[0] as FeatureLayer;
        CIMUniqueValueRenderer uniqueValueRenderer = (CIMUniqueValueRenderer)flyr.CreateRenderer(uniqueValueRendererDef);

        //setting the renderer to the feature layer
        flyr.SetRenderer(uniqueValueRenderer);
      });
      #endregion

    }

    public async Task AddQuerylayerAsync()
    {
      #region Create a query layer
      await QueuedTask.Run(() =>
      {
        Map map = MapView.Active.Map;
        Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri(@"C:\Connections\mySDE.sde")));
        CIMSqlQueryDataConnection sqldc = new CIMSqlQueryDataConnection()
        {
          WorkspaceConnectionString = geodatabase.GetConnectionString(),
          GeometryType = esriGeometryType.esriGeometryPolygon,
          OIDFields = "OBJECTID",
          Srid = "102008",
          SqlQuery = "select * from MySDE.dbo.STATES",
		  Dataset = "States"
        };
        FeatureLayer flyr = (FeatureLayer)LayerFactory.Instance.CreateLayer(sqldc, map, layerName: "States");
      });
      #endregion
    }


        private static void ResetDataConnectionFeatureService(Layer dataConnectionLayer, string newConnectionString)
        {
            #region Reset the URL of a feature service layer 
            CIMStandardDataConnection dataConnection = dataConnectionLayer.GetDataConnection() as CIMStandardDataConnection;
            dataConnection.WorkspaceConnectionString = newConnectionString;
            dataConnectionLayer.SetDataConnection(dataConnection);
            #endregion
        }

        public async Task ChangeGDBVersionAsync()
    {

      #region Change Geodatabase Version of layers off a specified version in a map using version name

      await QueuedTask.Run(() =>
    {
      //Getting the current version name from the first feature layer of the map
      FeatureLayer flyr = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();  //first feature layer
      Datastore dataStore = flyr.GetFeatureClass().GetDatastore();  //getting datasource
      Geodatabase geodatabase = dataStore as Geodatabase; //casting to Geodatabase
      if (geodatabase == null)
        return;

      VersionManager versionManager = geodatabase.GetVersionManager();
      String currentVersionName = versionManager.GetCurrentVersion().GetName();

      //Getting all available versions except the current one
      IEnumerable<ArcGIS.Core.Data.Version> versions = versionManager.GetVersions().Where(v => !v.GetName().Equals(currentVersionName, StringComparison.CurrentCultureIgnoreCase));

      //Assuming there is at least one other version we pick the first one from the list
      ArcGIS.Core.Data.Version toVersion = versions.FirstOrDefault();
      if (toVersion != null)
      {
        //Changing version
        MapView.Active.Map.ChangeVersion(currentVersionName, toVersion.GetName());
      }
    });
      #endregion

    }

    public async Task ChangeGDBVersion2Async()
    {

      #region Change Geodatabase Version of layers off a specified version in a map

      await QueuedTask.Run(() =>
    {
      //Getting the current version name from the first feature layer of the map
      FeatureLayer flyr = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();  //first feature layer
      Datastore dataStore = flyr.GetFeatureClass().GetDatastore();  //getting datasource
      Geodatabase geodatabase = dataStore as Geodatabase; //casting to Geodatabase
      if (geodatabase == null)
        return;

      VersionManager versionManager = geodatabase.GetVersionManager();
      ArcGIS.Core.Data.Version currentVersion = versionManager.GetCurrentVersion();

      //Getting all available versions except the current one
      IEnumerable<ArcGIS.Core.Data.Version> versions = versionManager.GetVersions().Where(v => !v.GetName().Equals(currentVersion.GetName(), StringComparison.CurrentCultureIgnoreCase));

      //Assuming there is at least one other version we pick the first one from the list
      ArcGIS.Core.Data.Version toVersion = versions.FirstOrDefault();
      if (toVersion != null)
      {
        //Changing version
        MapView.Active.Map.ChangeVersion(currentVersion, toVersion);
      }
    });
      #endregion

    }
    public async void SearchAndGetFeatureCount()
    {

      #region Querying a feature layer

      var count = await QueuedTask.Run(() =>
      {
        QueryFilter qf = new QueryFilter()
        {
          WhereClause = "Class = 'city'"
        };

        //Getting the first selected feature layer of the map view
        var flyr = (FeatureLayer)MapView.Active.GetSelectedLayers().OfType<FeatureLayer>().FirstOrDefault();
        RowCursor rows = flyr.Search(qf);//execute

        //Looping through to count
        int i = 0;
        while (rows.MoveNext()) i++;

        return i;
      });
      MessageBox.Show(String.Format("Total features that matched the search criteria: {0}", count));

      #endregion

    }

    public async Task RasterLayers()
    {
      Map aMap = MapView.Active.Map;
      string layerName = null;
      CIMColorRamp colorRamp = null;
      RasterLayer rasterLayer = null;

      #region Create a raster layer
      string url = @"C:\Images\Italy.tif";
      await QueuedTask.Run(() =>
      {
        // Create a raster layer using a path to an image.
        // Note: You can create a raster layer from a url, project item, or data connection.
        rasterLayer = (RasterLayer)LayerFactory.Instance.CreateLayer(new Uri(url), aMap);
      });
      #endregion

      #region Update the raster colorizer on a raster layer
      await QueuedTask.Run(() =>
      {
        // Get the colorizer from the raster layer.
        CIMRasterColorizer rasterColorizer = rasterLayer.GetColorizer();
        // Update raster colorizer properties.
        rasterColorizer.Brightness = 10;
        rasterColorizer.Contrast = -5;
        rasterColorizer.ResamplingType = RasterResamplingType.NearestNeighbor;
        // Update the raster layer with the changed colorizer.
        rasterLayer.SetColorizer(rasterColorizer);
      });
      #endregion

      #region Update the RGB colorizer on a raster layer
      await QueuedTask.Run(() =>
      {
        // Get the colorizer from the raster layer.
        CIMRasterColorizer rColorizer = rasterLayer.GetColorizer();
        // Check if the colorizer is an RGB colorizer.
        if (rColorizer is CIMRasterRGBColorizer)
        {
          CIMRasterRGBColorizer rasterRGBColorizer = (CIMRasterRGBColorizer)rColorizer;
          // Update RGB colorizer properties.
          rasterRGBColorizer.StretchType = RasterStretchType.ESRI;
          // Update the raster layer with the changed colorizer.
          rasterLayer.SetColorizer((CIMRasterColorizer)rasterRGBColorizer);
        }
      });
      #endregion

      #region Check if a certain colorizer can be applied to a raster layer 
      await QueuedTask.Run(() =>
      {
        // Get the list of colorizers that can be applied to the raster layer.
        IEnumerable<RasterColorizerType> applicableColorizerList = rasterLayer.GetApplicableColorizers();
        // Check if the RGB colorizer is part of the list.
        bool isTrue_ContainTheColorizerType = 
          applicableColorizerList.Contains(RasterColorizerType.RGBColorizer);
        });
      #endregion

      #region Create a new colorizer based on a default colorizer definition and apply it to the raster layer 
      await QueuedTask.Run(async () =>
      {
        // Check if the Stretch colorizer can be applied to the raster layer.
        if (rasterLayer.GetApplicableColorizers().Contains(RasterColorizerType.StretchColorizer))
        {
          // Create a new Stretch Colorizer Definition using the default constructor.
          StretchColorizerDefinition stretchColorizerDef_default = new StretchColorizerDefinition();
          // Create a new Stretch colorizer using the colorizer definition created above.
          CIMRasterStretchColorizer newStretchColorizer_default = 
            await rasterLayer.CreateColorizerAsync(stretchColorizerDef_default) as CIMRasterStretchColorizer;
          // Set the new colorizer on the raster layer.
          rasterLayer.SetColorizer(newStretchColorizer_default);
        }
      });
      #endregion

      #region Create a new colorizer based on a custom colorizer definition and apply it to the raster layer 
      await QueuedTask.Run(async () =>
      {
        // Check if the Stretch colorizer can be applied to the raster layer.
        if (rasterLayer.GetApplicableColorizers().Contains(RasterColorizerType.StretchColorizer))
        {
          // Create a new Stretch Colorizer Definition specifying parameters 
          // for band index, stretch type, gamma and color ramp.
          StretchColorizerDefinition stretchColorizerDef_custom = 
            new StretchColorizerDefinition(1, RasterStretchType.ESRI, 2, colorRamp);
          // Create a new stretch colorizer using the colorizer definition created above.
          CIMRasterStretchColorizer newStretchColorizer_custom = 
            await rasterLayer.CreateColorizerAsync(stretchColorizerDef_custom) as CIMRasterStretchColorizer;
          // Set the new colorizer on the raster layer.
          rasterLayer.SetColorizer(newStretchColorizer_custom);
        }
      });
      #endregion

      #region Create a raster layer with a new colorizer definition
      // Create a new stretch colorizer definition using default constructor.
      StretchColorizerDefinition stretchColorizerDef = new StretchColorizerDefinition();
      await QueuedTask.Run(() =>
      {
        // Create a raster layer using the colorizer definition created above.
        // Note: You can create a raster layer from a url, project item, or data connection.
        RasterLayer rasterLayerfromURL =
          LayerFactory.Instance.CreateRasterLayer(new Uri(url), aMap, 0, layerName, stretchColorizerDef) as RasterLayer;
      });
      #endregion
    }

    public async Task MosaicLayers()
    {
      Map aMap = MapView.Active.Map;
      string layerName = null;
      CIMColorRamp colorRamp = null;

      #region Create a mosaic layer
      MosaicLayer mosaicLayer = null;
      string url = @"C:\Images\countries.gdb\Italy";
      await QueuedTask.Run(() =>
      {
        // Create a mosaic layer using a path to a mosaic dataset.
        // Note: You can create a mosaic layer from a url, project item, or data connection.
        mosaicLayer = (MosaicLayer)LayerFactory.Instance.CreateLayer(new Uri(url), aMap);
      });
      #endregion

      #region Update the raster colorizer on a mosaic layer
      await QueuedTask.Run(() =>
      {
        // Get the image sub-layer from the mosaic layer.
        ImageMosaicSubLayer mosaicImageSubLayer = mosaicLayer.GetImageLayer();
        // Get the colorizer from the image sub-layer.
        CIMRasterColorizer rasterColorizer = mosaicImageSubLayer.GetColorizer();
        // Update raster colorizer properties.
        rasterColorizer.Brightness = 10;
        rasterColorizer.Contrast = -5;
        rasterColorizer.ResamplingType = RasterResamplingType.NearestNeighbor;
        // Update the image sub-layer with the changed colorizer.
        mosaicImageSubLayer.SetColorizer(rasterColorizer);
      });
      #endregion

      #region Update the RGB colorizer on a mosaic layer
      await QueuedTask.Run(() =>
      {
        // Get the image sub-layer from the mosaic layer.
        ImageMosaicSubLayer mosaicImageSubLayer = mosaicLayer.GetImageLayer();
        // Get the colorizer from the image sub-layer.
        CIMRasterColorizer rColorizer = mosaicImageSubLayer.GetColorizer();
        // Check if the colorizer is an RGB colorizer.
        if (rColorizer is CIMRasterRGBColorizer)
        {
          // Cast colorizer type from CIMRasterColorizer into CIMRasterRGBColorizer.
          CIMRasterRGBColorizer rasterRGBColorizer = (CIMRasterRGBColorizer)rColorizer;
          // Update RGB colorizer properties.
          rasterRGBColorizer.StretchType = RasterStretchType.ESRI;
          // Update the image sub-layer with the changed colorizer.
          mosaicImageSubLayer.SetColorizer((CIMRasterColorizer)rasterRGBColorizer);
        }
      });
      #endregion

      #region Check if a certain colorizer can be applied to a mosaic layer 
      await QueuedTask.Run(() =>
      {
        // Get the image sub-layer from the mosaic layer.
        ImageMosaicSubLayer mosaicImageSubLayer = mosaicLayer.GetImageLayer();
        // Get the list of colorizers that can be applied to the image sub-layer.
        IEnumerable<RasterColorizerType> applicableColorizerList = 
          mosaicImageSubLayer.GetApplicableColorizers();
        // Check if the RGB colorizer is part of the list.
        bool isTrue_ContainTheColorizerType =
          applicableColorizerList.Contains(RasterColorizerType.RGBColorizer);
      });
      #endregion

      #region Create a new colorizer based on a default colorizer definition and apply it to the mosaic layer 
      await QueuedTask.Run(async () =>
      {
        // Get the image sub-layer from the mosaic layer.
        ImageMosaicSubLayer mosaicImageSubLayer = mosaicLayer.GetImageLayer();
        // Check if the Stretch colorizer can be applied to the image sub-layer.
        if (mosaicImageSubLayer.GetApplicableColorizers().Contains(RasterColorizerType.StretchColorizer))
        {
          // Create a new Stretch Colorizer Definition using the default constructor.
          StretchColorizerDefinition stretchColorizerDef_default = new StretchColorizerDefinition();
          // Create a new Stretch colorizer using the colorizer definition created above.
          CIMRasterStretchColorizer newStretchColorizer_default =
            await mosaicImageSubLayer.CreateColorizerAsync(stretchColorizerDef_default) as CIMRasterStretchColorizer;
          // Set the new colorizer on the image sub-layer.
          mosaicImageSubLayer.SetColorizer(newStretchColorizer_default);
        }
      });
      #endregion

      #region Create a new colorizer based on a custom colorizer definition and apply it to the mosaic layer 
      await QueuedTask.Run(async () =>
      {
        // Get the image sub-layer from the mosaic layer.
        ImageMosaicSubLayer mosaicImageSubLayer = mosaicLayer.GetImageLayer();
        // Check if the Stretch colorizer can be applied to the image sub-layer.
        if (mosaicImageSubLayer.GetApplicableColorizers().Contains(RasterColorizerType.StretchColorizer))
        {
          // Create a new Stretch colorizer definition specifying parameters
          // for band index, stretch type, gamma and color ramp.
          StretchColorizerDefinition stretchColorizerDef_custom =
            new StretchColorizerDefinition(1, RasterStretchType.ESRI, 2, colorRamp);
          // Create a new stretch colorizer using the colorizer definition created above.
          CIMRasterStretchColorizer newStretchColorizer_custom =
            await mosaicImageSubLayer.CreateColorizerAsync(stretchColorizerDef_custom) as CIMRasterStretchColorizer;
          // Set the new colorizer on the image sub-layer.
          mosaicImageSubLayer.SetColorizer(newStretchColorizer_custom);
        }
      });
      #endregion

      #region Create a mosaic layer with a new colorizer definition
      // Create a new colorizer definition using default constructor.
      StretchColorizerDefinition stretchColorizerDef = new StretchColorizerDefinition();
      await QueuedTask.Run(() =>
      {
        // Create a mosaic layer using the colorizer definition created above.
        // Note: You can create a mosaic layer from a url, project item, or data connection.
        MosaicLayer newMosaicLayer =
          LayerFactory.Instance.CreateMosaicLayer(new Uri(url), aMap, 0, layerName, stretchColorizerDef) as MosaicLayer;
      });
      #endregion

      #region Update the sort order (mosaic method) on a mosaic layer
      await QueuedTask.Run(() =>
      {
        // Get the image sub-layer from the mosaic layer.
        ImageServiceLayer mosaicImageSubLayer = (ImageServiceLayer)mosaicLayer.GetImageLayer();
        // Get the mosaic rule.
        CIMMosaicRule mosaicingRule = mosaicImageSubLayer.GetMosaicRule();
        // Set the Mosaic Method to Center.
        mosaicingRule.MosaicMethod = RasterMosaicMethod.Center;
        // Update the mosaic with the changed mosaic rule.
        mosaicImageSubLayer.SetMosaicRule(mosaicingRule);
      });
      #endregion

      #region Update the resolve overlap (mosaic operator) on a mosaic layer
      await QueuedTask.Run(() =>
      {
        // Get the image sub-layer from the mosaic layer.
        ImageServiceLayer mosaicImageSublayer = (ImageServiceLayer)mosaicLayer.GetImageLayer();
        // Get the mosaic rule.
        CIMMosaicRule mosaicRule = mosaicImageSublayer.GetMosaicRule();
        // Set the Mosaic Operator to Mean.
        mosaicRule.MosaicOperatorType = RasterMosaicOperatorType.Mean;
        // Update the mosaic with the changed mosaic rule.
        mosaicImageSublayer.SetMosaicRule(mosaicRule);
      });
      #endregion
    }

    public async Task ImageServiceLayers()
    {
      Map aMap = MapView.Active.Map;
      string layerName = null;
      CIMColorRamp colorRamp = null;

      #region Create an image service layer
      ImageServiceLayer isLayer = null;
      string url = 
      @"http://imagery.arcgisonline.com/arcgis/services/LandsatGLS/GLS2010_Enhanced/ImageServer";
      await QueuedTask.Run(() =>
      {
        // Create an image service layer using the url for an image service.
        isLayer = (ImageServiceLayer)LayerFactory.Instance.CreateLayer(new Uri(url), aMap);
      });
      #endregion

      #region Update the raster colorizer on an image service layer
      await QueuedTask.Run(() =>
      {
        // Get the colorizer from the image service layer.
        CIMRasterColorizer rasterColorizer = isLayer.GetColorizer();
        // Update the colorizer properties.
        rasterColorizer.Brightness = 10;
        rasterColorizer.Contrast = -5;
        rasterColorizer.ResamplingType = RasterResamplingType.NearestNeighbor;
        // Update the image service layer with the changed colorizer.
        isLayer.SetColorizer(rasterColorizer);
      });
      #endregion

      #region Update the RGB colorizer on an image service layer
      await QueuedTask.Run(() =>
      {
        // Get the colorizer from the image service layer.
        CIMRasterColorizer rColorizer = isLayer.GetColorizer();
        // Check if the colorizer is an RGB colorizer.
        if (rColorizer is CIMRasterRGBColorizer)
        {
          // Cast colorizer type from CIMRasterColorizer to CIMRasterRGBColorizer.
          CIMRasterRGBColorizer rasterRGBColorizer = (CIMRasterRGBColorizer)rColorizer;
          // Update RGB colorizer properties.
          rasterRGBColorizer.StretchType = RasterStretchType.ESRI;
          // Update the image service layer with the changed colorizer.
          isLayer.SetColorizer((CIMRasterColorizer)rasterRGBColorizer);
        }
      });
      #endregion

      #region Check if a certain colorizer can be applied to an image service layer 
      await QueuedTask.Run(() =>
      {
        // Get the list of colorizers that can be applied to the imager service layer.
        IEnumerable<RasterColorizerType> applicableColorizerList = isLayer.GetApplicableColorizers();
        // Check if the RGB colorizer is part of the list.
        bool isTrue_ContainTheColorizerType =
          applicableColorizerList.Contains(RasterColorizerType.RGBColorizer);
      });
      #endregion

      #region Create a new colorizer based on a default colorizer definition and apply it to the image service layer 
      await QueuedTask.Run(async () =>
      {
        // Check if the Stretch colorizer can be applied to the image service layer.
        if (isLayer.GetApplicableColorizers().Contains(RasterColorizerType.StretchColorizer))
        {
          // Create a new Stretch Colorizer Definition using the default constructor.
          StretchColorizerDefinition stretchColorizerDef_default = new StretchColorizerDefinition();
          // Create a new Stretch colorizer using the colorizer definition created above.
          CIMRasterStretchColorizer newStretchColorizer_default =
            await isLayer.CreateColorizerAsync(stretchColorizerDef_default) as CIMRasterStretchColorizer;
          // Set the new colorizer on the image service layer.
          isLayer.SetColorizer(newStretchColorizer_default);
        }
      });
      #endregion

      #region Create a new colorizer based on a custom colorizer definition and apply it to the image service layer 
      await QueuedTask.Run(async () =>
      {
        // Check if the Stretch colorizer can be applied to the image service layer.
        if (isLayer.GetApplicableColorizers().Contains(RasterColorizerType.StretchColorizer))
        {
          // Create a new Stretch Colorizer Definition specifying parameters
          // for band index, stretch type, gamma and color ramp. 
          StretchColorizerDefinition stretchColorizerDef_custom =
            new StretchColorizerDefinition(1, RasterStretchType.ESRI, 2, colorRamp);
          // Create a new stretch colorizer using the colorizer definition created above.
          CIMRasterStretchColorizer newStretchColorizer_custom =
            await isLayer.CreateColorizerAsync(stretchColorizerDef_custom) as CIMRasterStretchColorizer;
          // Set the new colorizer on the image service layer.
          isLayer.SetColorizer(newStretchColorizer_custom);
        }
      });
      #endregion

      #region Create an image service layer with a new colorizer definition
      // Create a new colorizer definition using default constructor.
      StretchColorizerDefinition stretchColorizerDef = new StretchColorizerDefinition();
      await QueuedTask.Run(() =>
      {
        // Create an image service layer using the colorizer definition created above.
        ImageServiceLayer imageServiceLayer =
          LayerFactory.Instance.CreateRasterLayer(new Uri(url), aMap, 0, layerName, stretchColorizerDef) as ImageServiceLayer;
      });
      #endregion

      #region Update the sort order (mosaic method) on an image service layer
      await QueuedTask.Run(() =>
      {
        // Get the mosaic rule of the image service.
        CIMMosaicRule mosaicRule = isLayer.GetMosaicRule();
        // Set the Mosaic Method to Center.
        mosaicRule.MosaicMethod = RasterMosaicMethod.Center;
        // Update the image service with the changed mosaic rule.
        isLayer.SetMosaicRule(mosaicRule);
      });
      #endregion

      #region Update the resolve overlap (mosaic operator) on an image service layer
      await QueuedTask.Run(() =>
      {
        // Get the mosaic rule of the image service.
        CIMMosaicRule mosaicingRule = isLayer.GetMosaicRule();
        // Set the Mosaic Operator to Mean.
        mosaicingRule.MosaicOperatorType = RasterMosaicOperatorType.Mean;
        // Update the image service with the changed mosaic rule.
        isLayer.SetMosaicRule(mosaicingRule);
      });
      #endregion

    }
    public async void CreateHeatMapRenderer()
    {
      #region Create a Heatmap Renderer
      string colorBrewerSchemesName = "ArcGIS Colors";
      StyleProjectItem style = Project.Current.GetItems<StyleProjectItem>().First(s => s.Name == colorBrewerSchemesName);
      string colorRampName = "Heat Map 4 - Semitransparent";
      IList<ColorRampStyleItem> colorRampList = await QueuedTask.Run(() =>
      {
        return style.SearchColorRamps(colorRampName);
      });
      ColorRampStyleItem colorRamp = colorRampList[0];

      await QueuedTask.Run(() =>
      {
        //defining a heatmap renderer that uses values from Population field as the weights
        HeatMapRendererDefinition heatMapDef = new HeatMapRendererDefinition()
        {
          Radius = 20,
          WeightField = "Population",
          ColorRamp = colorRamp.ColorRamp,
          RendereringQuality = 8,
          UpperLabel = "High Density",
          LowerLabel = "Low Density"
        };

        FeatureLayer flyr = MapView.Active.Map.Layers[0] as FeatureLayer;
        CIMHeatMapRenderer heatMapRndr = (CIMHeatMapRenderer)flyr.CreateRenderer(heatMapDef);
        flyr.SetRenderer(heatMapRndr);
      });
      #endregion
    }

    public async void CreateUnclassedRenderer()
    {
      #region Create an Unclassed Renderer
      string colorBrewerSchemesName = "ArcGIS Colors";
      StyleProjectItem style = Project.Current.GetItems<StyleProjectItem>().First(s => s.Name == colorBrewerSchemesName);
      string colorRampName = "Heat Map 4 - Semitransparent";
      IList<ColorRampStyleItem> colorRampList = await QueuedTask.Run(() =>
      {
        return style.SearchColorRamps(colorRampName);
      });
      ColorRampStyleItem colorRamp = colorRampList[0];

      await QueuedTask.Run(() =>
      {
        CIMPointSymbol pointSym = SymbolFactory.Instance.ConstructPointSymbol(ColorFactory.Instance.GreenRGB, 16.0, SimpleMarkerStyle.Diamond);
        CIMSymbolReference symbolPointTemplate = pointSym.MakeSymbolReference();

        //defining an unclassed renderer with custom upper and lower stops
        //all features with value >= 5,000,000 will be drawn with the upper color from the color ramp
        //all features with value <= 50,000 will be drawn with the lower color from the color ramp
        UnclassedColorsRendererDefinition unclassRndrDef = new UnclassedColorsRendererDefinition
                                    ("Population", symbolPointTemplate, colorRamp.ColorRamp, "Highest", "Lowest", 5000000, 50000)
        {

          //drawing features with null values with a different symbol
          ShowNullValues = true,
          NullValueLabel = "Unknown"
        };
        CIMPointSymbol nullSym = SymbolFactory.Instance.ConstructPointSymbol(ColorFactory.Instance.RedRGB, 16.0, SimpleMarkerStyle.Circle);
        unclassRndrDef.NullValueSymbol = nullSym.MakeSymbolReference();
        FeatureLayer flyr = MapView.Active.Map.Layers[0] as FeatureLayer;
        CIMClassBreaksRenderer cbRndr = (CIMClassBreaksRenderer)flyr.CreateRenderer(unclassRndrDef);
        flyr.SetRenderer(cbRndr);
      });
      #endregion
    }

    private static void GetSymbol()
    {
        #region Modify a point symbol created from a character marker    
        //create marker from the Font, char index,size,color
        var cimMarker = SymbolFactory.Instance.ConstructMarker(125, "Wingdings 3", "Regular", 6, ColorFactory.Instance.BlueRGB) as CIMCharacterMarker; 
        var polygonMarker = cimMarker.Symbol;
        //modifying the polygon's outline and fill
        //This is the outline
        polygonMarker.SymbolLayers[0] = SymbolFactory.Instance.ConstructStroke(ColorFactory.Instance.GreenRGB, 2, SimpleLineStyle.Solid); 
        //This is the fill
        polygonMarker.SymbolLayers[1] = SymbolFactory.Instance.ConstructSolidFill(ColorFactory.Instance.BlueRGB); 
        //create a symbol from the marker 
        //Note this overload of ConstructPointSymbol does not need to be run within QueuedTask.Run.
        var pointSymbol = SymbolFactory.Instance.ConstructPointSymbol(cimMarker); 
        #endregion
    }

        public async void CreateProportionaRenderer()
    {
      #region Create a Proportion Renderer with max and min symbol size capped
      string colorBrewerSchemesName = "ArcGIS Colors";
      StyleProjectItem style = Project.Current.GetItems<StyleProjectItem>().First(s => s.Name == colorBrewerSchemesName);
      string colorRampName = "Heat Map 4 - Semitransparent";
      IList<ColorRampStyleItem> colorRampList = await QueuedTask.Run(() =>
      {
        return style.SearchColorRamps(colorRampName);
      });
      ColorRampStyleItem colorRamp = colorRampList[0];

      await QueuedTask.Run(() =>
      {
        CIMPointSymbol pointSym = SymbolFactory.Instance.ConstructPointSymbol(ColorFactory.Instance.GreenRGB, 1.0, SimpleMarkerStyle.Circle);
        CIMSymbolReference symbolPointTemplate = pointSym.MakeSymbolReference();

        //minimum symbol size is capped to 4 point while the maximum symbol size is set to 50 point
        ProportionalRendererDefinition prDef = new ProportionalRendererDefinition("POPULATION", symbolPointTemplate, 4, 50, true)
        {

          //setting upper and lower size stops to stop symbols growing or shrinking beyond those thresholds
          UpperSizeStop = 5000000,  //features with values >= 5,000,000 will be drawn with maximum symbol size
          LowerSizeStop = 50000    //features with values <= 50,000 will be drawn with minimum symbol size
        };
        FeatureLayer flyr = MapView.Active.Map.Layers[0] as FeatureLayer;
        CIMProportionalRenderer propRndr = (CIMProportionalRenderer)flyr.CreateRenderer(prDef);
        flyr.SetRenderer(propRndr);

      });
      #endregion
    }

    public async void CreateTrueProportionaRenderer()
    {
      #region Create a True Proportion Renderer
      string colorBrewerSchemesName = "ArcGIS Colors";
      StyleProjectItem style = Project.Current.GetItems<StyleProjectItem>().First(s => s.Name == colorBrewerSchemesName);
      string colorRampName = "Heat Map 4 - Semitransparent";
      IList<ColorRampStyleItem> colorRampList = await QueuedTask.Run(() =>
      {
        return style.SearchColorRamps(colorRampName);
      });
      ColorRampStyleItem colorRamp = colorRampList[0];

      await QueuedTask.Run(() =>
      {
        CIMPointSymbol pointSym = SymbolFactory.Instance.ConstructPointSymbol(ColorFactory.Instance.GreenRGB, 1.0, SimpleMarkerStyle.Circle);
        CIMSymbolReference symbolPointTemplate = pointSym.MakeSymbolReference();

        //Defining proportional renderer where size of symbol will be same as its value in field used in the renderer.
        ProportionalRendererDefinition prDef = new ProportionalRendererDefinition("POPULATION", esriUnits.esriMeters, symbolPointTemplate, SymbolShapes.Square, ValueRepresentations.Radius);

        FeatureLayer flyr = MapView.Active.Map.Layers[0] as FeatureLayer;
        CIMProportionalRenderer propRndr = (CIMProportionalRenderer)flyr.CreateRenderer(prDef);
        flyr.SetRenderer(propRndr);

      });
      #endregion
    }
        protected static void ArcadeRenderer()
        {
            #region Modify renderer using Arcade
            var lyr = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault(f => f.ShapeType == esriGeometryType.esriGeometryPolygon);
            if (lyr == null) return;
            QueuedTask.Run(() => {
                // GetRenderer from Layer (assumes it is a unique value renderer)
                var uvRenderer = lyr.GetRenderer() as CIMUniqueValueRenderer;
                if (uvRenderer == null) return;
                //layer has STATE_NAME field
                //community sample Data\Admin\AdminSample.aprx
                string expression = "if ($view.scale > 21000000) { return $feature.STATE_NAME } else { return 'All' }";
                CIMExpressionInfo updatedExpressionInfo = new CIMExpressionInfo
                {
                    Expression = expression,
                    Title = "Custom" // can be any string used for UI purpose.
                };
                //set the renderer's expression
                uvRenderer.ValueExpressionInfo = updatedExpressionInfo;

                //SetRenderer on Layer
                lyr.SetRenderer(uvRenderer);
            });
            #endregion
        }
        protected static void ArcadeLabeling()
        {
            #region Modify label expression using Arcade
            var lyr = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault(f => f.ShapeType == esriGeometryType.esriGeometryPolygon);
            if (lyr == null) return;
            QueuedTask.Run(() => {
                //Get the layer's definition
                //community sample Data\Admin\AdminSample.aprx
                var lyrDefn = lyr.GetDefinition() as CIMFeatureLayer;
                if (lyrDefn == null) return;
                //Get the label classes - we need the first one
                var listLabelClasses = lyrDefn.LabelClasses.ToList();
                var theLabelClass = listLabelClasses.FirstOrDefault();
                //set the label class Expression to use the Arcade expression
                theLabelClass.Expression = "return $feature.STATE_NAME + TextFormatting.NewLine + $feature.POP2000;";
                //Set the label definition back to the layer.
                lyr.SetDefinition(lyrDefn);
            });

            #endregion
        }
        public void MiscOneLiners()
    {
      Map aMap = MapView.Active.Map;

      #region Update a map's basemap layer
      aMap.SetBasemapLayers(Basemap.Gray);
      #endregion

      #region Remove basemap layer from a map
      aMap.SetBasemapLayers(Basemap.None);
            #endregion

        #region Find a layer
        //Finds layers by name and returns a read only list of Layers
        IReadOnlyList<Layer> layers = aMap.FindLayers("cities", true);
            
        //Finds a layer using a URI.
        //The Layer URI you pass in helps you search for a specific layer in a map
        var lyrFindLayer = MapView.Active.Map.FindLayer("CIMPATH=map/u_s__states__generalized_.xml");

        //This returns a collection of layers of the "name" specified. You can use any Linq expression to query the collection.  
        var lyrExists = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().Any(f => f.Name == "U.S. States (Generalized)");
            #endregion

        #region Count the features selected in a map
        var lyr = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();
        var noFeaturesSelected = lyr.SelectionCount;
        #endregion

            #region Get a list of layers filtered by layer type from a map
            List<FeatureLayer> featureLayerList = aMap.GetLayersAsFlattenedList().OfType<FeatureLayer>().ToList();
      #endregion

      #region Find a standalone table
      IReadOnlyList<StandaloneTable> tables = aMap.FindStandaloneTables("addresses");
      #endregion
    }

        public static void GetRotationFieldOfRenderer()
        {
            #region Get the attribute rotation field of a layer
            var featureLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();
            QueuedTask.Run(() =>
            {
                var cimRenderer = featureLayer.GetRenderer() as CIMUniqueValueRenderer;
                var cimRotationVariable = cimRenderer.VisualVariables.OfType<CIMRotationVisualVariable>().FirstOrDefault();
                var rotationInfoZ = cimRotationVariable.VisualVariableInfoZ;
                var rotationExpression = rotationInfoZ.ValueExpressionInfo.Expression; // this expression stores the field name  
            });
            #endregion
        }
        public void EnableLabeling()
    {
      #region Enable labeling on a layer
      var featureLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();
      ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
      {
        // toggle the label visibility
        featureLayer.SetLabelVisibility(!featureLayer.IsLabelVisible);
      });
      #endregion
    }


    public void AccessDisplayField()
    {
      #region Access the display field for a layer
      var featureLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();
      ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
      {
        // get the CIM definition from the layer
        var cimFeatureDefinition = featureLayer.GetDefinition() as ArcGIS.Core.CIM.CIMBasicFeatureLayer;
        // get the view of the source table underlying the layer
        var cimDisplayTable = cimFeatureDefinition.FeatureTable;
        // this field is used as the 'label' to represent the row
        var displayField = cimDisplayTable.DisplayField;
      });
      #endregion
    }

    public void FindConnectedAttribute()
    {
      #region Find connected attribute field for rotation
      var featureLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();
      ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
      {
        // get the CIM renderer from the layer
        var cimRenderer = featureLayer.GetRenderer() as ArcGIS.Core.CIM.CIMSimpleRenderer;
        // get the collection of connected attributes for rotation
        var cimRotationVariable = cimRenderer.VisualVariables.OfType<ArcGIS.Core.CIM.CIMRotationVisualVariable>().FirstOrDefault();
        // the z direction is describing the heading rotation
        var rotationInfoZ = cimRotationVariable.VisualVariableInfoZ;
        var rotationExpression = rotationInfoZ.Expression; // this expression stores the field name  
      });
      #endregion
    }

    public void ScaleSymbols()
    {
      #region Toggle "Scale layer symbols when reference scale is set"
      var featureLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();
      ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
      {
        // get the CIM layer definition
        var cimFeatureLayer = featureLayer.GetDefinition() as ArcGIS.Core.CIM.CIMFeatureLayer;
        // turn on the option to scale the symbols in this layer based in the map's reference scale
        cimFeatureLayer.ScaleSymbols = true;
      });
      #endregion
    }

    public void SetLayerCache()
    {
      #region Set the layer cache
      var featureLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();
      ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
      {
        // change the layer cache type to maximum age
        featureLayer.SetDisplayCacheType(ArcGIS.Core.CIM.DisplayCacheType.MaxAge);
        // change from the default 5 min to 2 min
        featureLayer.SetDisplayCacheMaxAge(TimeSpan.FromMinutes(2));
      });
      #endregion
    }

    public void ChangeSelectionColor()
    {
      #region Change the layer selection color
      var featureLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();
      ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
      {
        // get the CIM definition of the layer
        var layerDef = featureLayer.GetDefinition() as ArcGIS.Core.CIM.CIMBasicFeatureLayer;
        // disable the default symbol
        layerDef.UseSelectionSymbol = false;
        // assign a new color
        layerDef.SelectionColor = ColorFactory.Instance.RedRGB;
        // apply the definition to the layer
        featureLayer.SetDefinition(layerDef);

        if (!featureLayer.IsVisible) featureLayer.SetVisibility(true);
        //Do a selection

        MapView.Active.SelectFeatures(MapView.Active.Extent);
      });
      #endregion
    }
        public async void RemoveAllUncheckedLayers()
        {
            #region Removes all layers that are unchecked
            var map = MapView.Active.Map;
            if (map == null)
                return;
            //Get the group layers first
            IReadOnlyList<GroupLayer> groupLayers = map.Layers.OfType<GroupLayer>().ToList();
            //Iterate and remove the layers within the group layers that are unchecked.
            foreach (var groupLayer in groupLayers)
            {
                //Get layers that not visible within the group
                var layers = groupLayer.Layers.Where(l => l.IsVisible == false).ToList();
                //Remove all the layers that are not visible within the group
                await QueuedTask.Run(() => map.RemoveLayers(layers));
            }

            //Group Layers that are empty and are unchecked
            foreach (var group in groupLayers)
            {
                if (group.Layers.Count == 0 && group.IsVisible == false) //No layers in the group
                {
                    //remove the group
                    await QueuedTask.Run(() => map.RemoveLayer(group));
                }
            }

            //Get Layers that are NOT Group layers and are unchecked
            var notAGroupAndUnCheckedLayers = map.Layers.Where(l => !(l is GroupLayer) && l.IsVisible == false).ToList();
            //Remove all the non group layers that are not visible
            await QueuedTask.Run(() => map.RemoveLayers(notAGroupAndUnCheckedLayers));
            #endregion

        }
        public async void RemoveEmptyGroups()
        {
            #region Remove empty groups
            var map = MapView.Active.Map;
            if (map == null)
                return;
            //Get the group layers
            IReadOnlyList<GroupLayer> groupLayers = map.Layers.OfType<GroupLayer>().ToList();
            foreach (var group in groupLayers)
            {
                if (group.Layers.Count == 0) //No layers in the group
                {
                    //remove the group
                    await QueuedTask.Run(() => map.RemoveLayer(group));
                }
            }
            #endregion

        }

    }
}