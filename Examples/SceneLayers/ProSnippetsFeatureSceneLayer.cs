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
using ArcGIS.Desktop.Editing.Attributes;

namespace ProSnippetsFeatureSceneLayer
{
  class ProSnippetsFeatureSceneLayer
  {
    #region ProSnippet Group: FeatureSceneLayer
    #endregion

    public async void Examples()
    {
      #region Name of FeatureSceneLayer 
      var featSceneLayer = MapView.Active.Map.GetLayersAsFlattenedList()
                       .OfType<FeatureSceneLayer>().FirstOrDefault();
      var scenelayerName = featSceneLayer?.Name;

      #endregion

      //#region Get the layer type
      // layerType = featureScenelayer.SceneServiceLayerType;
      //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show($"FeatureSceneLayer has an Associated FeatureLayer is {hasAssociatedFL}");
      //#endregion

      #region Get the Data Source type
      //Must be called on the MCT
      var dataSourceType = featSceneLayer?.GetDataSourceType() ?? 
                                 SceneLayerDataSourceType.Unknown;
      if (dataSourceType == SceneLayerDataSourceType.SLPK)
      {
        //Uses SLPK - only cached attributes
      }
      else if (dataSourceType == SceneLayerDataSourceType.Service)
      {
        //Hosted service - can have live attributes - check HasAssociatedFeatureService
      }

      #endregion

      #region Get the Associated Feature class

      //var featSceneLayer = ....;
      if (featSceneLayer.HasAssociatedFeatureService)
      {
        //Must be called on the MCT
        using (var fc = featSceneLayer.GetFeatureClass())
        {
          //TODO query underlying feature class
        }
      }

      #endregion

      #region Get Field Definitions

      //var featSceneLayer = ....;
      await QueuedTask.Run(() =>
      {
        //Get only the readonly fields
        var readOnlyFields = featSceneLayer.GetFieldDescriptions().Where(fdesc => fdesc.IsReadOnly);
        //etc
        
      });
      #endregion

      #region Set a Definition Query
      //Must be called on the MCT
      //var featSceneLayer = ...;
      featSceneLayer.SetDefinitionQuery("Name = 'Ponderosa Pine'");

      #endregion

      Geometry geometry = null;

      #region Select features via the MapView

      //assume the geometry used in SelectFeaturesEx() is coming from a 
      //map tool...
      //
      //SketchType = SketchGeometryType.Rectangle;
      //SketchOutputMode = SketchOutputMode.Screen;

      await QueuedTask.Run(() =>
      {
        var result = MapView.Active.SelectFeaturesEx(geometry);
        //Get scene layers with selections
        var scene_layers = result.Where(kvp => kvp.Key is FeatureSceneLayer);
        foreach (var kvp in scene_layers)
        {
          var scene_layer = kvp.Key as FeatureSceneLayer;
          var sel_oids = kvp.Value;
          //If there are attributes then get them
          if (scene_layer.HasAssociatedFeatureService)
          {
            var insp = new Inspector();
            foreach (var oid in sel_oids)
            {
              insp.Load(scene_layer, oid);
              //TODO something with retrieved attributes
            } 
          }
        }
      });
       
      #endregion

    }

    public async void Examples2()
    {
      //var scenelayerName = featSceneLayer?.Name;

      #region Has Associated FeatureService

      var featSceneLayer = MapView.Active.Map.GetLayersAsFlattenedList()
                           .OfType<FeatureSceneLayer>().First();
      if (featSceneLayer.HasAssociatedFeatureService)
      {
        //Can Select and Search...possibly edit

      }

      #endregion

      #region Search Rows on the FeatureSceneLayer

      //var featSceneLayer = ...;
      if (!featSceneLayer.HasAssociatedFeatureService)
        return;//Search and Select not supported

      //Multipatch (Object3D) or point?
      var is3dObject = ((ISceneLayerInfo)featSceneLayer).SceneServiceLayerType 
                                        == esriSceneServiceLayerType.Object3D;
      await QueuedTask.Run(() =>
      {
        var queryFilter = new QueryFilter
        {
          WhereClause = "Name = 'Ponderosa Pine'",
          SubFields = "*"
        };

        int rowCount = 0;
        //or select... var select = featSceneLayer.Select(queryFilter)
        using (RowCursor rowCursor = featSceneLayer.Search(queryFilter))
        {
          while (rowCursor.MoveNext())
          {
            var feature = rowCursor.Current as Feature;
            var oid = feature.GetObjectID();
            var shape = feature.GetShape();
            var attrib = feature["Name"];
            if (is3dObject)
            {
              //shape is a multipatch
            }
            else
            {
              //shape is a point
            }
            rowCount += 1;
          }
        }

      });
      #endregion

      #region Hide Selected features and Show Hidden features

      //var featSceneLayer = ...;
      if (featSceneLayer.HasAssociatedFeatureService)
        return;//Search and Select not supported

      await QueuedTask.Run(() =>
      {
        QueryFilter qf = new QueryFilter()
        {
          ObjectIDs = new List<long>() { 6069, 6070, 6071 },
          SubFields = "*"
        };
        featSceneLayer.Select(qf, SelectionCombinationMethod.New);

        featSceneLayer.HideSelectedFeatures();
        var selectionCount = featSceneLayer.SelectionCount;

        featSceneLayer.ShowHiddenFeatures();
        selectionCount = featSceneLayer.SelectionCount;

      });
      #endregion

      #region Use MapView Selection SelectFeaturesEx or GetFeaturesEx

      //var featSceneLayer = ...;
      var sname = featSceneLayer.Name;

      await QueuedTask.Run(() =>
      {
        //Select all features within the current map view
        var sz = MapView.Active.GetViewSize();

        var c_ll = new Coordinate2D(0, 0);
        var c_ur = new Coordinate2D(sz.Width, sz.Height);
        //Use screen coordinates for 3D selection on MapView
        var env = EnvelopeBuilder.CreateEnvelope(c_ll, c_ur);

        //HasAssociatedFeatureService does not matter for SelectFeaturesEx
        //or GetFeaturesEx
        var result = MapView.Active.SelectFeaturesEx(env);
        //var result = MapView.Active.GetFeaturesEx(env);

        //The list of object ids from SelectFeaturesEx
        var oids1 = result.Where(kvp => kvp.Key.Name == sname).First().Value;
        //TODO - use the object ids

        MapView.Active.Map.ClearSelection();
      });

      #endregion

      #region Use Select or Search with a Spatial Query

      //var featSceneLayer = ...;
      //var sname = featSceneLayer.Name;
      await QueuedTask.Run(() =>
      {
        if (!featSceneLayer.HasAssociatedFeatureService)
          return;//no search or select

        //Select all features within the current map view
        var sz = MapView.Active.GetViewSize();
        var map_pt1 = MapView.Active.ClientToMap(new System.Windows.Point(0, sz.Height));
        var map_pt2 = MapView.Active.ClientToMap(new System.Windows.Point(sz.Width, 0));

        //Convert to an envelope
        var temp_env = EnvelopeBuilder.CreateEnvelope(map_pt1, map_pt2, MapView.Active.Map.SpatialReference);

        //Project if needed to the layer spatial ref
        SpatialReference sr = null;
        using (var fc = featSceneLayer.GetFeatureClass())
        using (var fdef = fc.GetDefinition())
          sr = fdef.GetSpatialReference();

        var env = GeometryEngine.Instance.Project(temp_env, sr) as Envelope;

        //Set up a query filter
        var sf = new SpatialQueryFilter()
        {
          FilterGeometry = env,
          SpatialRelationship = SpatialRelationship.Intersects,
          SubFields = "*"
        };

        //Select against the feature service
        var select = featSceneLayer.Select(sf);
        if (select.GetCount() > 0)
        {
          //enumerate over the selected features
          using (var rc = select.Search())
          {
            while (rc.MoveNext())
            {
              var feature = rc.Current as Feature;
              var oid = feature.GetObjectID();
              //etc.
            }
          }
        }

        MapView.Active.Map.ClearSelection();

      });
      #endregion
    }
  }
}
