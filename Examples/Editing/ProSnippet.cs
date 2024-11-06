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
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Editing.Attributes;
using ArcGIS.Desktop.Editing.Events;
using ArcGIS.Desktop.Editing.Templates;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Mapping.Events;
using ArcGIS.Desktop.Core;
using System.Windows;
using ArcGIS.Core.Events;
using ArcGIS.Core.Data.Topology;
using System.Xml.Linq;
using Attribute = ArcGIS.Desktop.Editing.Attributes.Attribute;
using ArcGIS.Core.Data.UtilityNetwork.Trace;

namespace EditingSDKExamples
{
  class ProSnippet : MapTool
  {
    ArcGIS.Core.Geometry.Geometry geometry = null;

    public ProSnippet()
    {
      IsSketchTool = true;
      SketchType = SketchGeometryType.Rectangle;
      SketchOutputMode = SketchOutputMode.Map;
    }

    #region ProSnippet Group: Edit Operation Methods
    #endregion

    public void EditOperations()
    {

      var featureLayer = MapView.Active.Map.GetLayersAsFlattenedList()[0] as FeatureLayer;
      var polygon = new PolygonBuilderEx().ToGeometry();
      var clipPoly = new PolygonBuilderEx().ToGeometry();
      var cutLine = new PolylineBuilderEx().ToGeometry();
      var modifyLine = cutLine;
      var oid = 1;
      var layer = featureLayer;
      var standaloneTable = MapView.Active.Map.GetStandaloneTablesAsFlattenedList().FirstOrDefault();

      var opEdit = new EditOperation();
      // cref: ArcGIS.Desktop.Editing.EditOperation.IsEmpty
      // cref: ArcGIS.Desktop.Editing.EditOperation.Execute
      // cref: ArcGIS.Desktop.Editing.EditOperation.ExecuteAsync
      #region Edit Operation - check for actions before Execute

      // Some times when using EditOperation.Modify you can unknowingly be attempting to set
      //  an attribute to value 
      //  setting 
      // In this scenario the Modify action will detect that nothing is required
      // and do nothing. Because no actions have occurred, the
      // Consequently the Execute operation will fail. 
      if (!opEdit.IsEmpty)
        opEdit.Execute();

      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.#ctor()
      // cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Mapping.Layer, ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Desktop.Editing.EditOperation.IsSucceeded
      // cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Mapping.MapMember,System.Collections.Generic.Dictionary{System.String,System.Object})
      // cref: ArcGIS.Desktop.Editing.EditOperation.Execute
      // cref: ArcGIS.Desktop.Editing.EditOperation.ExecuteAsync
      #region Edit Operation Create Features

      var createFeatures = new EditOperation() { Name = "Create Features" };
      //Create a feature with a polygon
      var token = createFeatures.Create(featureLayer, polygon);
      if (createFeatures.IsSucceeded)
      {
        // token.ObjectID wll be populated with the objectID of the created feature after Execute has been successful
      }
      //Do a create features and set attributes
      var attributes = new Dictionary<string, object>();
      attributes.Add("SHAPE", polygon);
      attributes.Add("NAME", "Corner Market");
      attributes.Add("SIZE", 1200.5);
      attributes.Add("DESCRIPTION", "Corner Market");

      createFeatures.Create(featureLayer, attributes);

      //Create features using the current template
      //Must be within a MapTool
      createFeatures.Create(this.CurrentTemplate, polygon);

      //Execute to execute the operation
      //Must be called within QueuedTask.Run

      if (!createFeatures.IsEmpty)
      {
        createFeatures.Execute(); //Execute will return true if the operation was successful and false if not.
      }

      //or use async flavor
      //await createFeatures.ExecuteAsync();

      #endregion

      // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate.Current
      // cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Editing.Templates.EditingTemplate, ArcGIS.Core.Geometry.Geometry)
      #region Create a feature using the current template
      var myTemplate = ArcGIS.Desktop.Editing.Templates.EditingTemplate.Current;

      //Create edit operation and execute
      var op = new ArcGIS.Desktop.Editing.EditOperation() { Name = "Create my feature" };
      op.Create(myTemplate, geometry);
      if (!op.IsEmpty)
      {
        var result = op.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }
      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Mapping.MapMember, System.Collections.Generic.Dictionary<string, object>)
      #region Create feature from a modified inspector

      var insp = new ArcGIS.Desktop.Editing.Attributes.Inspector();
      insp.Load(layer, 86);

      ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
      {
        // modify attributes if necessary
        // insp["Field1"] = newValue;

        //Create new feature from an existing inspector (copying the feature)
        var createOp = new EditOperation() { Name = "Create from insp" };
        createOp.Create(insp.MapMember, insp.ToDictionary(a => a.FieldName, a => a.CurrentValue));

        if (!createOp.IsEmpty)
        {
          var result = createOp.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
        }
      });
      #endregion

      var csvData = new List<CSVData>();

      // cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Mapping.MapMember, System.Collections.Generic.Dictionary<string, object>)
      #region Create features from a CSV file
      //Run on MCT
      ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
      {
        //Create the edit operation
        var createOperation = new ArcGIS.Desktop.Editing.EditOperation() { Name = "Generate points", SelectNewFeatures = false };

        // determine the shape field name - it may not be 'Shape' 
        string shapeField = layer.GetFeatureClass().GetDefinition().GetShapeField();

        //Loop through csv data
        foreach (var item in csvData)
        {

          //Create the point geometry
          ArcGIS.Core.Geometry.MapPoint newMapPoint =
              ArcGIS.Core.Geometry.MapPointBuilderEx.CreateMapPoint(item.X, item.Y);

          // include the attributes via a dictionary
          var atts = new Dictionary<string, object>();
          atts.Add("StopOrder", item.StopOrder);
          atts.Add("FacilityID", item.FacilityID);
          atts.Add(shapeField, newMapPoint);

          // queue feature creation
          createOperation.Create(layer, atts);
        }

        // execute the edit (feature creation) operation
        if (createOperation.IsEmpty)
        {
          return createOperation.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
        }
        else
          return false;
      });
      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Editing.Templates.EditingTemplate)
      #region Edit Operation Create row in a table using a table template
      var tableTemplate = standaloneTable.GetTemplates().FirstOrDefault();
      var createRow = new EditOperation() { Name = "Create a row in a table" };
      //Creating a new row in a standalone table using the table template of your choice
      createRow.Create(tableTemplate);

      if (!createRow.IsEmpty)
      {
        var result = createRow.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.Clip(ArcGIS.Desktop.Mapping.Layer, System.Int64, ArcGIS.Core.Geometry.Geometry, ArcGIS.Desktop.Editing.ClipMode)
      #region Edit Operation Clip Features

      var clipFeatures = new EditOperation() { Name = "Clip Features" };
      clipFeatures.Clip(featureLayer, oid, clipPoly, ClipMode.PreserveArea);
      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      if (!clipFeatures.IsEmpty)
      {
        var result = clipFeatures.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //or use async flavor
      //await clipFeatures.ExecuteAsync();

      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.Split(ArcGIS.Desktop.Mapping.Layer, System.Int64, ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Desktop.Editing.EditOperation.Split(ArcGIS.Desktop.Mapping.SelectionSet, ArcGIS.Core.Geometry.Geometry)
      #region Edit Operation Cut Features

      var select = MapView.Active.SelectFeatures(clipPoly);

      var cutFeatures = new EditOperation() { Name = "Cut Features" };
      cutFeatures.Split(featureLayer, oid, cutLine);

      //Cut all the selected features in the active view
      //Select using a polygon (for example)
      //at 2.x - var kvps = MapView.Active.SelectFeatures(polygon).Select(
      //      k => new KeyValuePair<MapMember, List<long>>(k.Key as MapMember, k.Value));
      //cutFeatures.Split(kvps, cutLine);
      var sset = MapView.Active.SelectFeatures(polygon);
      cutFeatures.Split(sset, cutLine);

      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      if (!cutFeatures.IsEmpty)
      {
        var result = cutFeatures.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //or use async flavor
      //await cutFeatures.ExecuteAsync();

      #endregion
      {
        // cref: ArcGIS.Desktop.Editing.EditOperation.Delete(ArcGIS.Desktop.Mapping.MapMember, System.Int64)
        // cref: ArcGIS.Desktop.Editing.EditOperation.Delete(ArcGIS.Desktop.Mapping.SelectionSet)
        #region Edit Operation Delete Features

        var deleteFeatures = new EditOperation() { Name = "Delete Features" };
        var table = MapView.Active.Map.StandaloneTables[0];
        //Delete a row in a standalone table
        deleteFeatures.Delete(table, oid);

        //Delete all the selected features in the active view
        //Select using a polygon (for example)
        //at 2.x - var selection = MapView.Active.SelectFeatures(polygon).Select(
        //      k => new KeyValuePair<MapMember, List<long>>(k.Key as MapMember, k.Value));
        //deleteFeatures.Delete(selection);
        var selection = MapView.Active.SelectFeatures(polygon);
        deleteFeatures.Delete(selection);

        //Execute to execute the operation
        //Must be called within QueuedTask.Run
        if (!deleteFeatures.IsEmpty)
        {
          var result = deleteFeatures.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
        }

        //or use async flavor
        //await deleteFeatures.ExecuteAsync();

        #endregion
      }

      // cref: ARCGIS.DESKTOP.EDITING.EDITOPERATION.CREATE(ArcGIS.Desktop.Mapping.MapMember,System.Collections.Generic.Dictionary{System.String,System.Object})
      // cref: ARCGIS.DESKTOP.EDITING.EDITOPERATION.CREATECHAINEDOPERATION
      #region Edit Operation Duplicate Features
      {
        var duplicateFeatures = new EditOperation() { Name = "Duplicate Features" };

        //Duplicate with an X and Y offset of 500 map units
        //At 2.x duplicateFeatures.Duplicate(featureLayer, oid, 500.0, 500.0, 0.0);

        //Execute to execute the operation
        //Must be called within QueuedTask.Run
        var insp2 = new Inspector();
        insp2.Load(featureLayer, oid);
        var geom = insp2["SHAPE"] as Geometry;

        var rtoken = duplicateFeatures.Create(insp2.MapMember, insp2.ToDictionary(a => a.FieldName, a => a.CurrentValue));
        if (!duplicateFeatures.IsEmpty)
        {
          if (duplicateFeatures.Execute())//Execute and ExecuteAsync will return true if the operation was successful and false if not
          {
            var modifyOp = duplicateFeatures.CreateChainedOperation();
            modifyOp.Modify(featureLayer, (long)rtoken.ObjectID, GeometryEngine.Instance.Move(geom, 500.0, 500.0));
            if (!modifyOp.IsEmpty)
            {
              var result = modifyOp.Execute();
            }
          }
        }
      }

      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.Explode(ARCGIS.DESKTOP.MAPPING.Layer,SYSTEM.COLLECTIONS.GENERIC.IEnumerable{Int64},Boolean)
      #region Edit Operation Explode Features

      var explodeFeatures = new EditOperation() { Name = "Explode Features" };

      //Take a multipart and convert it into one feature per part
      //Provide a list of ids to convert multiple
      explodeFeatures.Explode(featureLayer, new List<long>() { oid }, true);

      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      if (!explodeFeatures.IsEmpty)
      {
        var result = explodeFeatures.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //or use async flavor
      //await explodeFeatures.ExecuteAsync();

      #endregion

      var destinationLayer = featureLayer;

      // cref: ArcGIS.Desktop.Editing.EditOperation.Merge(ARCGIS.DESKTOP.MAPPING.LAYER,ARCGIS.DESKTOP.MAPPING.LAYER,IENUMERABLE{INT64},INSPECTOR)
      // cref: ArcGIS.Desktop.Editing.EditOperation.Merge(EditingRowTemplate,ARCGIS.DESKTOP.MAPPING.Layer,IEnumerable{Int64})
      // cref: ArcGIS.Desktop.Editing.EditOperation.Merge(ARCGIS.DESKTOP.MAPPING.LAYER,IENUMERABLE{INT64},INSPECTOR)
      #region Edit Operation Merge Features

      var mergeFeatures = new EditOperation() { Name = "Merge Features" };

      //Merge three features into a new feature using defaults
      //defined in the current template
      //At 2.x -
      //mergeFeatures.Merge(this.CurrentTemplate as EditingFeatureTemplate, featureLayer, new List<long>() { 10, 96, 12 });
      mergeFeatures.Merge(this.CurrentTemplate as EditingRowTemplate, featureLayer, new List<long>() { 10, 96, 12 });

      //Merge three features into a new feature in the destination layer
      mergeFeatures.Merge(destinationLayer, featureLayer, new List<long>() { 10, 96, 12 });

      //Use an inspector to set the new attributes of the merged feature
      var inspector = new Inspector();
      inspector.Load(featureLayer, oid);//base attributes on an existing feature
      //change attributes for the new feature
      inspector["NAME"] = "New name";
      inspector["DESCRIPTION"] = "New description";

      //Merge features into a new feature in the same layer using the
      //defaults set in the inspector
      mergeFeatures.Merge(featureLayer, new List<long>() { 10, 96, 12 }, inspector);

      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      if (!mergeFeatures.IsEmpty)
      {
        var result = mergeFeatures.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //or use async flavor
      //await mergeFeatures.ExecuteAsync();

      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.Modify(ArcGIS.Desktop.Editing.Attributes.Inspector)
      // cref: ArcGIS.Desktop.Editing.EditOperation.Modify(ArcGIS.Desktop.Mapping.Layer, System.Int64, ArcGIS.Core.Geometry.Geometry, Nullable<System.Collections.Generic.Dictionary<System.String, System.object>>)
      #region Edit Operation Modify single feature

      var modifyFeature = new EditOperation() { Name = "Modify a feature" };

      //use an inspector
      var modifyInspector = new Inspector();
      modifyInspector.Load(featureLayer, oid);//base attributes on an existing feature

      //change attributes for the new feature
      modifyInspector["SHAPE"] = polygon;//Update the geometry
      modifyInspector["NAME"] = "Updated name";//Update attribute(s)

      modifyFeature.Modify(modifyInspector);

      //update geometry and attributes using overload
      var featureAttributes = new Dictionary<string, object>();
      featureAttributes["NAME"] = "Updated name";//Update attribute(s)
      modifyFeature.Modify(featureLayer, oid, polygon, featureAttributes);

      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      if (!modifyFeature.IsEmpty)
      {
        var result = modifyFeature.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //or use async flavor
      //await modifyFeatures.ExecuteAsync();

      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.Modify(ArcGIS.Desktop.Editing.Attributes.Inspector)
      #region Edit Operation Modify multiple features

      //Search by attribute
      var queryFilter = new QueryFilter() { WhereClause = "OBJECTID < 1000000" };
      //Create list of oids to update
      var oidSet = new List<long>();
      using (var rc = featureLayer.Search(queryFilter))
      {
        while (rc.MoveNext())
        {
          using (var record = rc.Current)
          {
            oidSet.Add(record.GetObjectID());
          }
        }
      }

      //create and execute the edit operation
      var modifyFeatures = new EditOperation() { Name = "Modify features" };
      modifyFeatures.ShowProgressor = true;

      var multipleFeaturesInsp = new Inspector();
      multipleFeaturesInsp.Load(featureLayer, oidSet);
      multipleFeaturesInsp["MOMC"] = 24;
      modifyFeatures.Modify(multipleFeaturesInsp);
      if (!modifyFeatures.IsEmpty)
      {
        var result = modifyFeatures.ExecuteAsync(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }
      #endregion

      #region Search for layer features and update a field
      ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
      {
        //find layer
        var disLayer = ArcGIS.Desktop.Mapping.MapView.Active.Map.FindLayers("Distribution mains").FirstOrDefault() as BasicFeatureLayer;

        //Search by attribute
        var filter = new ArcGIS.Core.Data.QueryFilter { WhereClause = "CONTRACTOR = 'KCGM'" };

        var oids = new List<long>();
        using (var rc = disLayer.Search(filter))
        {
          //Create list of oids to update
          while (rc.MoveNext())
          {
            using (var record = rc.Current)
            {
              oidSet.Add(record.GetObjectID());
            }
          }
        }

        //Create edit operation 
        var modifyOp = new EditOperation() { Name = "Update date" };

        // load features into inspector and update field
        var dateInsp = new ArcGIS.Desktop.Editing.Attributes.Inspector();
        dateInsp.Load(disLayer, oids);
        dateInsp["InspDate"] = "9/21/2013";

        // modify and execute
        modifyOp.Modify(insp);
        if (!modifyOp.IsEmpty)
        {
          var result = modifyOp.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
        }
      });
      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.Move(ArcGIs.Desktop.Mapping.SelectionSet, System.Double, System.Double)
      #region Move features

      //Get all of the selected ObjectIDs from the layer.
      var firstLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();
      var selectionfromMap = firstLayer.GetSelection();

      // set up a dictionary to store the layer and the object IDs of the selected features
      var selectionDictionary = new Dictionary<MapMember, List<long>>();
      selectionDictionary.Add(firstLayer as MapMember, selectionfromMap.GetObjectIDs().ToList());

      var moveFeature = new EditOperation() { Name = "Move features" };
      //at 2.x - moveFeature.Move(selectionDictionary, 10, 10);  //specify your units along axis to move the geometry
      moveFeature.Move(SelectionSet.FromDictionary(selectionDictionary), 10, 10);  //specify your units along axis to move the geometry
      if (!moveFeature.IsEmpty)
      {
        var result = moveFeature.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }
      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.Modify(LAYER,INT64,GEOMETRY,DICTIONARY{STRING,OBJECT})
      #region Move feature to a specific coordinate

      //Get all of the selected ObjectIDs from the layer.
      var abLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();
      var mySelection = abLayer.GetSelection();
      var selOid = mySelection.GetObjectIDs().FirstOrDefault();

      var moveToPoint = new MapPointBuilderEx(1.0, 2.0, 3.0, 4.0, MapView.Active.Map.SpatialReference); //can pass in coordinates.

      var modifyFeatureCoord = new EditOperation() { Name = "Move features" };
      modifyFeatureCoord.Modify(abLayer, selOid, moveToPoint.ToGeometry());  //Modify the feature to the new geometry 
      if (!modifyFeatureCoord.IsEmpty)
      {
        var result = modifyFeatureCoord.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.Planarize(Layer,IEnumerable{Int64},Nullable{Double})
      #region Edit Operation Planarize Features

      // note - EditOperation.Planarize requires a standard license. 
      //  An exception will be thrown if Pro is running under a basic license. 

      var planarizeFeatures = new EditOperation() { Name = "Planarize Features" };

      //Planarize one or more features
      planarizeFeatures.Planarize(featureLayer, new List<long>() { oid });

      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      if (!planarizeFeatures.IsEmpty)
      {
        var result = planarizeFeatures.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //or use async flavor
      //await planarizeFeatures.ExecuteAsync();

      #endregion

      // cref: ArcGIS.Desktop.Editing.ParallelOffset.Builder.#ctor
      // cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Editing.ParallelOffset.Builder)
      #region Edit Operation ParallelOffset
      //Create parallel features from the selected features

      //find the roads layer
      var roadsLayer = MapView.Active.Map.FindLayers("Roads").FirstOrDefault();

      //instantiate parallelOffset builder and set parameters
      var parOffsetBuilder = new ParallelOffset.Builder()
      {
        Selection = MapView.Active.Map.GetSelection(),
        Template = roadsLayer.GetTemplate("Freeway"),
        Distance = 200,
        Side = ParallelOffset.SideType.Both,
        Corner = ParallelOffset.CornerType.Mitered,
        Iterations = 1,
        AlignConnected = false,
        CopyToSeparateFeatures = false,
        RemoveSelfIntersectingLoops = true
      };

      //create editoperation and execute
      var parallelOp = new EditOperation();
      parallelOp.Create(parOffsetBuilder);
      if (!parallelOp.IsEmpty)
      {
        var result = parallelOp.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.Reshape(ArcGIS.Desktop.Mapping.SelectionSet, ArcGIS.Core.Geometry.Geometry)
      #region Edit Operation Reshape Features

      var reshapeFeatures = new EditOperation() { Name = "Reshape Features" };

      reshapeFeatures.Reshape(featureLayer, oid, modifyLine);

      //Reshape a set of features that intersect some geometry....

      //at 2.x - var selFeatures = MapView.Active.GetFeatures(modifyLine).Select(
      //    k => new KeyValuePair<MapMember, List<long>>(k.Key as MapMember, k.Value));
      //reshapeFeatures.Reshape(selFeatures, modifyLine);

      reshapeFeatures.Reshape(MapView.Active.GetFeatures(modifyLine), modifyLine);

      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      if (!reshapeFeatures.IsEmpty)
      {
        var result = reshapeFeatures.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //or use async flavor
      //await reshapeFeatures.ExecuteAsync();

      #endregion

      var origin = MapPointBuilderEx.CreateMapPoint(0, 0, null);

      // cref: ArcGIS.Desktop.Editing.EditOperation.Rotate(ArcGIS.Desktop.Mapping.SelectionSet, ArcGIS.Core.Geometry.MapPoint, System.Double)
      #region Edit Operation Rotate Features

      var rotateFeatures = new EditOperation() { Name = "Rotate Features" };

      //Rotate works on a selected set of features
      //Get all features that intersect a polygon

      //at 2.x - var rotateSelection = MapView.Active.GetFeatures(polygon).Select(
      //    k => new KeyValuePair<MapMember, List<long>>(k.Key as MapMember, k.Value));
      //rotateFeatures.Rotate(rotateSelection, origin, Math.PI / 2);

      //Rotate selected features 90 deg about "origin"
      rotateFeatures.Rotate(MapView.Active.GetFeatures(polygon), origin, Math.PI / 2);

      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      if (!rotateFeatures.IsEmpty)
      {
        var result = rotateFeatures.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //or use async flavor
      //await rotateFeatures.ExecuteAsync();

      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.Scale(ArcGIS.Desktop.Mapping.SelectionSet, ArcGIS.Core.Geometry.MapPoint, System.Double, System.Double, System.Double)
      #region Edit Operation Scale Features

      var scaleFeatures = new EditOperation() { Name = "Scale Features" };

      //Rotate works on a selected set of features

      //var scaleSelection = MapView.Active.GetFeatures(polygon).Select(
      //    k => new KeyValuePair<MapMember, List<long>>(k.Key as MapMember, k.Value));
      //scaleFeatures.Scale(scaleSelection, origin, 2.0, 2.0, 0.0);

      //Scale the selected features by 2.0 in the X and Y direction
      scaleFeatures.Scale(MapView.Active.GetFeatures(polygon), origin, 2.0, 2.0, 0.0);

      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      if (!scaleFeatures.IsEmpty)
      {
        var result = scaleFeatures.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //or use async flavor
      //await scaleFeatures.ExecuteAsync();

      #endregion

      var mp1 = MapPointBuilderEx.CreateMapPoint(0, 0, null);
      var mp2 = mp1;
      var mp3 = mp1;

      // cref: ArcGIS.Desktop.Editing.SplitByPercentage.#ctor
      // cref: ArcGIS.Desktop.Editing.SplitByEqualParts.#ctor
      // cref: ArcGIS.Desktop.Editing.SplitByDistance.#ctor
      // cref: ArcGIS.Desktop.Editing.SplitByVaryingDistance.#ctor
      // cref: ArcGIS.Desktop.Editing.EditOperation.Split(ArcGIS.Desktop.Mapping.Layer, System.Int64, System.Collections.Generic.IEnumerable<ArcGID.Core.Geometry.MapPoint>)
      // cref: ArcGIS.Desktop.Editing.EditOperation.Split(ArcGIS.Desktop.Mapping.Layer, System.Int64, ArcGIS.Desktop.Editing.SplitMethod)
      #region Edit Operation Split Features

      var splitFeatures = new EditOperation() { Name = "Split Features" };

      var splitPoints = new List<MapPoint>() { mp1, mp2, mp3 };

      //Split the feature at 3 points
      splitFeatures.Split(featureLayer, oid, splitPoints);

      // split using percentage
      var splitByPercentage = new SplitByPercentage() { Percentage = 33, SplitFromStartPoint = true };
      splitFeatures.Split(featureLayer, oid, splitByPercentage);

      // split using equal parts
      var splitByEqualParts = new SplitByEqualParts() { NumParts = 3 };
      splitFeatures.Split(featureLayer, oid, splitByEqualParts);

      // split using single distance
      var splitByDistance = new SplitByDistance() { Distance = 27.3, SplitFromStartPoint = false };
      splitFeatures.Split(featureLayer, oid, splitByDistance);

      // split using varying distance
      var distances = new List<double>() { 12.5, 38.2, 89.99 };
      var splitByVaryingDistance = new SplitByVaryingDistance() { Distances = distances, SplitFromStartPoint = true, ProportionRemainder = true };
      splitFeatures.Split(featureLayer, oid, splitByVaryingDistance);

      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      if (!splitFeatures.IsEmpty)
      {
        var result = splitFeatures.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //or use async flavor
      //await splitAtPointsFeatures.ExecuteAsync();

      #endregion

      var linkLayer = featureLayer;

      // cref: ArcGIS.Desktop.Editing.TransformByLinkLayer.#ctor
      // cref: ArcGIS.Desktop.Editing.TransformMethodType
      // cref: ArcGIS.Desktop.Editing.EditOperation.Transform(ArcGIS.Desktop.Mapping.Layer,ArcGIS.Desktop.Editing.TransformMethod)
      // cref: ArcGIS.Desktop.Editing.EditOperation.Transform(ArcGIS.Desktop.Mapping.SelectionSet,ArcGIS.Desktop.Editing.TransformMethod)
      #region Edit Operation Transform Features

      var transformFeatures = new EditOperation() { Name = "Transform Features" };

      //Transform a selected set of features
      //At 2.x - var transformSelection = MapView.Active.GetFeatures(polygon).Select(
      //    k => new KeyValuePair<MapMember, List<long>>(k.Key as MapMember, k.Value));
      //transformFeatures.Transform(transformSelection, linkLayer);
      ////Transform just a layer
      //transformFeatures.Transform(featureLayer, linkLayer);
      ////Perform an affine transformation
      //transformFeatures.TransformAffine(featureLayer, linkLayer);

      var affine_transform = new TransformByLinkLayer()
      {
        LinkLayer = linkLayer,
        TransformType = TransformMethodType.Affine //TransformMethodType.Similarity
      };
      //Transform a selected set of features
      transformFeatures.Transform(MapView.Active.GetFeatures(polygon), affine_transform);
      //Perform an affine transformation
      transformFeatures.Transform(featureLayer, affine_transform);

      //Execute to execute the operation
      //Must be called within QueuedTask.Run
      if (!transformFeatures.IsEmpty)
      {
        var result = transformFeatures.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //or use async flavor
      //await transformFeatures.ExecuteAsync();

      #endregion

      IEnumerable<Polyline> linkLines = new List<Polyline>();
      IEnumerable<MapPoint> anchorPoints = new List<MapPoint>();
      IEnumerable<Polygon> limitedAdjustmentAreas = new List<Polygon>();
      var anchorPointsLayer = featureLayer;
      var limitedAdjustmentAreaLayer = featureLayer;

      // cref: ArcGIS.Desktop.Editing.RubbersheetByGeometries.#ctor
      // cref: ArcGIS.Desktop.Editing.RubbersheetByLayers.#ctor
      // cref: ArcGIS.Desktop.Editing.EditOperation.Rubbersheet(ArcGIS.Desktop.Mapping.Layer, ArcGIS.Desktop.Editing.RubbersheetMethod)
      #region Edit Operation Rubbersheet Features

      //Perform rubbersheet by geometries
      var rubbersheetMethod = new RubbersheetByGeometries()
      {
        RubbersheetType = RubbersheetMethodType.Linear, //The RubbersheetType can be Linear of NearestNeighbor
        LinkLines = linkLines, //IEnumerable list of link lines (polylines)
        AnchorPoints = anchorPoints, //IEnumerable list of anchor points (map points)
        LimitedAdjustmentAreas = limitedAdjustmentAreas //IEnumerable list of limited adjustment areas (polygons)
      };

      var rubbersheetOp = new EditOperation();
      //Performs linear rubbersheet transformation on the features belonging to "layer" that fall within the limited adjustment areas
      rubbersheetOp.Rubbersheet(layer, rubbersheetMethod);
      //Execute the operation
      if (!rubbersheetOp.IsEmpty)
      {
        var result = rubbersheetOp.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //Alternatively, you can also perform rubbersheet by layer
      var rubbersheetMethod2 = new RubbersheetByLayers()
      {
        RubbersheetType = RubbersheetMethodType.NearestNeighbor, //The RubbersheetType can be Linear of NearestNeighbor
        LinkLayer = linkLayer,
        AnchorPointLayer = anchorPointsLayer,
        LimitedAdjustmentAreaLayer = limitedAdjustmentAreaLayer
      };

      //Performs nearest neighbor rubbersheet transformation on the features belonging to "layer" that fall within the limited adjustment areas
      rubbersheetOp.Rubbersheet(layer, rubbersheetMethod2);
      if (!rubbersheetOp.IsEmpty)
      {
        //Execute the operation
        var result = rubbersheetOp.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.Planarize(Layer,Int64,Nullable{Double})
      #region Edit Operation Perform a Clip, Cut, and Planarize

      //Multiple operations can be performed by a single
      //edit operation.
      var clipCutPlanarizeFeatures = new EditOperation() { Name = "Clip, Cut, and Planarize Features" };
      clipCutPlanarizeFeatures.Clip(featureLayer, oid, clipPoly);
      clipCutPlanarizeFeatures.Split(featureLayer, oid, cutLine);
      clipCutPlanarizeFeatures.Planarize(featureLayer, oid);

      if (!clipCutPlanarizeFeatures.IsEmpty)
      {
        //Note: An edit operation is a single transaction. 
        //Execute the operations (in the order they were declared)
        clipCutPlanarizeFeatures.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //or use async flavor
      //await clipCutPlanarizeFeatures.ExecuteAsync();

      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.CreateChainedOperation
      // cref: ArcGIS.Desktop.Editing.EditOperation.AddAttachment(ArcGIS.Desktop.Mapping.MapMember,System.Int64,System.String)
      #region Edit Operation Chain Edit Operations

      //Chaining operations is a special case. Use "Chained Operations" when you require multiple transactions 
      //to be undo-able with a single "Undo".

      //The most common use case for operation chaining is creating a feature with an attachment. 
      //Adding an attachment requires the object id (of a new feature) has already been created. 
      var editOperation1 = new EditOperation() { Name = string.Format("Create point in '{0}'", CurrentTemplate.Layer.Name) };

      long newFeatureID = -1;
      //The Create operation has to execute so we can get an object_id
      var token2 = editOperation1.Create(this.CurrentTemplate, polygon);

      //Must be within a QueuedTask
      editOperation1.Execute(); //Note: Execute and ExecuteAsync will return true if the operation was successful and false if not
      if (editOperation1.IsSucceeded)
      {
        newFeatureID = (long)token2.ObjectID;
        //Now, because we have the object id, we can add the attachment.  As we are chaining it, adding the attachment 
        //can be undone as part of the "Undo Create" operation. In other words, only one undo operation will show on the 
        //Pro UI and not two.
        var editOperation2 = editOperation1.CreateChainedOperation();
        //Add the attachment using the new feature id
        editOperation2.AddAttachment(this.CurrentTemplate.Layer, newFeatureID, @"C:\data\images\Hydrant.jpg");
        //Execute the chained edit operation. editOperation1 and editOperation2 show up as a single Undo operation
        //on the UI even though we had two transactions
        editOperation2.Execute();
      }

      #endregion

      // cref: ArcGIS.Desktop.Editing.RowToken
      // cref: ArcGIS.Desktop.Editing.EditOperation.AddAttachment(ArcGIS.Desktop.Editing.RowToken,System.String)
      #region Edit Operation add attachment via RowToken

      //ArcGIS Pro 2.5 extends the EditOperation.AddAttachment method to take a RowToken as a parameter.
      //This allows you to create a feature, using EditOperation.CreateEx, and add an attachment in one transaction.

      var editOpAttach = new EditOperation();
      editOperation1.Name = string.Format("Create point in '{0}'", CurrentTemplate.Layer.Name);

      var attachRowToken = editOpAttach.Create(this.CurrentTemplate, polygon);
      editOpAttach.AddAttachment(attachRowToken, @"c:\temp\image.jpg");

      //Must be within a QueuedTask
      if (!editOpAttach.IsEmpty)
      {
        var result = editOpAttach.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }
      #endregion

      string newName = "";
      FeatureClass fc = null;
      Geometry splitLine = null;

      // cref: ArcGIS.Desktop.Editing.EditOperation
      // cref: ArcGIS.Desktop.Editing.EditOperation.ExecuteMode
      // cref: ArcGIS.Desktop.Editing.EditOperation.Modify(ArcGIS.Core.Data.Row,System.String, System.object)
      // cref: ArcGIS.Desktop.Editing.EditOperation.Split(ArcGIS.Desktop.Mapping.Layer, System.Int64, ArcGIS.Core.Geometry.Geometry)
      #region Order edits sequentially

      // perform an edit and then a split as one operation.
      QueuedTask.Run(() =>
      {
        var queryFilter = new QueryFilter() { WhereClause = "OBJECTID = " + oid.ToString() };

        // create an edit operation and name.
        var op = new EditOperation() { Name = "modify followed by split" };
        // set the ExecuteMode
        op.ExecuteMode = ExecuteModeType.Sequential;

        using (var rowCursor = fc.Search(queryFilter, false))
        {
          while (rowCursor.MoveNext())
          {
            using (var feature = rowCursor.Current as Feature)
            {
              op.Modify(feature, "NAME", newName);
            }
          }
        }

        op.Split(layer, oid, splitLine);
        if (!op.IsEmpty)
        {
          bool result = op.Execute();
        }
        // else
        //  The operation doesn't make any changes to the database so if executed it will fail
      });
      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.SetOnUndone(System.Action)
      // cref: ArcGIS.Desktop.Editing.EditOperation.SetOnComitted(System.Action<System.Boolean>)
      // cref: ArcGIS.Desktop.Editing.EditOperation.SetOnRedone(System.Action)
      #region SetOnUndone, SetOnRedone, SetOnComitted

      // SetOnUndone, SetOnRedone and SetOnComitted can be used to manage 
      // external actions(such as writing to a log table) that are associated with 
      // each edit operation.

      //get selected feature and update attribute
      var selectedFeatures = MapView.Active.Map.GetSelection();
      var testInspector = new Inspector();
      testInspector.Load(selectedFeatures.ToDictionary().Keys.First(), selectedFeatures.ToDictionary().Values.First());
      testInspector["Name"] = "test";

      //create and execute the edit operation
      var updateTestField = new EditOperation() { Name = "Update test field" };
      updateTestField.Modify(insp);

      //actions for SetOn...
      updateTestField.SetOnUndone(() =>
      {
        //Sets an action that will be called when this operation is undone.
        Debug.WriteLine("Operation is undone");
      });

      updateTestField.SetOnRedone(() =>
      {
        //Sets an action that will be called when this editoperation is redone.
        Debug.WriteLine("Operation is redone");
      });

      updateTestField.SetOnComitted((bool b) => //called on edit session save(true)/discard(false).
      {
        // Sets an action that will be called when this editoperation is committed.
        Debug.WriteLine("Operation is committed");
      });

      if (!updateTestField.IsEmpty)
      {
        var result = updateTestField.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }
      #endregion
      
      var lineLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();

      var changeVertexIDOperation = new EditOperation();
      //cref: ArcGIS.Core.Geometry.MapPointBuilderEx.HasID
      //cref: ArcGIS.Core.Geometry.MapPointBuilderEx.ID
      #region Convert vertices in a polyline to a Control Point
      //Control points are special vertices used to apply symbol effects to line or polygon features.
      //By default, they appear as diamonds when you edit them.
      //They can also be used to migrate representations from ArcMap to features in ArcGIS Pro.
      QueuedTask.Run(() =>
      {
        //iterate through the points in the polyline.
        var lineLayerCursor = lineLayer.GetSelection().Search();
        var lineVertices = new List<MapPoint>();
        long oid = -1;
        while (lineLayerCursor.MoveNext())
        {
          var lineFeature = lineLayerCursor.Current as Feature;
          var line = lineFeature.GetShape() as Polyline;
          int vertexIndex = 1;
          oid = lineFeature.GetObjectID();
          //Each point is converted into a MapPoint and gets added to a list. 
          foreach (var point in line.Points)
          {
            MapPointBuilderEx mapPointBuilderEx = new MapPointBuilderEx(point);
            //Changing the vertex 6 and 7 to control points
            if (vertexIndex == 6 || vertexIndex == 7)
            {
              //These points are made "ID Aware" and the IDs are set to be 1.
              mapPointBuilderEx.HasID = true;
              mapPointBuilderEx.ID = 1;
            }
            
            lineVertices.Add(mapPointBuilderEx.ToGeometry() as MapPoint);
            vertexIndex++;
          }
        }
        //create a new polyline using the point collection.
        var newLine = PolylineBuilderEx.CreatePolyline(lineVertices);
        //edit operation to modify the original line with the new line that contains control points.
        changeVertexIDOperation.Modify(lineLayer, oid, newLine);
        changeVertexIDOperation.Execute();
      });
      #endregion

    }

    #region ProSnippet Group: Enable Editing
    #endregion

    private void CanWeEdit()
    {
      // cref: ArcGIs.Desktop.Core.Project.IsEditingEnabled
      // cref: ArcGIs.Desktop.Core.Project.SetIsEditingEnabledAsync(System.Boolean)
      #region Enable Editing

      // if not editing
      if (!Project.Current.IsEditingEnabled)
      {
        var res = MessageBox.Show("You must enable editing to use editing tools. Would you like to enable editing?",
                                                              "Enable Editing?", System.Windows.MessageBoxButton.YesNoCancel);
        if (res == System.Windows.MessageBoxResult.No ||
                      res == System.Windows.MessageBoxResult.Cancel)
        {
          return;
        }
        Project.Current.SetIsEditingEnabledAsync(true);
      }

      #endregion

      // cref: ArcGIs.Desktop.Core.Project.IsEditingEnabled
      // cref: ArcGIs.Desktop.Core.Project.HasEdits
      // cref: ArcGIs.Desktop.Core.Project.DiscardEditsAsync
      // cref: ArcGIs.Desktop.Core.Project.SaveEditsAsync
      // cref: ArcGIs.Desktop.Core.Project.SetIsEditingEnabledAsync(System.Boolean)
      #region Disable Editing

      // if editing
      if (Project.Current.IsEditingEnabled)
      {
        var res = MessageBox.Show("Do you want to disable editing? Editing tools will be disabled",
                                                               "Disable Editing?", System.Windows.MessageBoxButton.YesNoCancel);
        if (res == System.Windows.MessageBoxResult.No ||
                      res == System.Windows.MessageBoxResult.Cancel)
        {
          return;
        }

        //we must check for edits
        if (Project.Current.HasEdits)
        {
          res = MessageBox.Show("Save edits?", "Save Edits?", System.Windows.MessageBoxButton.YesNoCancel);
          if (res == System.Windows.MessageBoxResult.Cancel)
            return;
          else if (res == System.Windows.MessageBoxResult.No)
            Project.Current.DiscardEditsAsync();
          else
          {
            Project.Current.SaveEditsAsync();
          }
        }
        Project.Current.SetIsEditingEnabledAsync(false);
      }

      #endregion

    }


    #region ProSnippet Group: Row Events
    #endregion

    // cref: ArcGIS.Desktop.Editing.Events.RowChangedEvent.Subscribe(System.Action{ArcGIS.Desktop.Editing.Events.RowChangedEventArgs},ArcGIS.Core.Data.Table,System.Boolean)
    // cref: ArcGIS.Desktop.Editing.Events.RowCreatedEvent.Subscribe(System.Action{ArcGIS.Desktop.Editing.Events.RowChangedEventArgs},ArcGIS.Core.Data.Table,System.Boolean)
    // cref: ArcGIS.Desktop.Editing.Events.RowDeletedEvent.Subscribe(System.Action{ArcGIS.Desktop.Editing.Events.RowChangedEventArgs},ArcGIS.Core.Data.Table,System.Boolean)
    // cref: ArcGIS.Desktop.Editing.Events.RowChangedEvent
    // cref: ArcGIS.Desktop.Editing.Events.RowChangedEventArgs
    // cref: ArcGIS.Desktop.Editing.Events.RowCreatedEvent
    // cref: ArcGIS.Desktop.Editing.Events.RowDeletedEvent
    #region Subscribe to Row Events
    protected void SubscribeRowEvent()
    {
      QueuedTask.Run(() =>
      {
        //Listen for row events on a layer
        var featLayer = MapView.Active.GetSelectedLayers().First() as FeatureLayer;
        var layerTable = featLayer.GetTable();

        //subscribe to row events
        var rowCreateToken = RowCreatedEvent.Subscribe(OnRowCreated, layerTable);
        var rowChangeToken = RowChangedEvent.Subscribe(OnRowChanged, layerTable);
        var rowDeleteToken = RowDeletedEvent.Subscribe(OnRowDeleted, layerTable);
      });
    }

    protected void OnRowCreated(RowChangedEventArgs args)
    {
    }

    protected void OnRowChanged(RowChangedEventArgs args)
    {
    }

    protected void OnRowDeleted(RowChangedEventArgs args)
    {
    }
    #endregion

    private static Guid _lastEdit = Guid.Empty;

    // cref: ArcGIS.Desktop.Editing.Events.RowCreatedEvent
    // cref: ArcGIS.Desktop.Editing.Events.RowChangedEventArgs.Operation
    // cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Core.Data.Table, System.Collections.Generic.Dictionary<System.String, System.Object>)
    #region Create a record in a separate table in the Map within Row Events

    // Use the EditOperation in the RowChangedEventArgs to append actions to be executed. 
    //  Your actions will become part of the operation and combined into one item on the undo stack

    private void HookRowCreatedEvent()
    {
      // subscribe to the RowCreatedEvent
      Table table = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault().GetTable();
      RowCreatedEvent.Subscribe(MyRowCreatedEvent, table);
    }

    private void MyRowCreatedEvent(RowChangedEventArgs args)
    {
      // RowEvent callbacks are always called on the QueuedTask so there is no need 
      // to wrap your code within a QueuedTask.Run lambda.

      // get the edit operation
      var parentEditOp = args.Operation;

      // set up some attributes
      var attribs = new Dictionary<string, object> { };
      attribs.Add("Layer", "Parcels");
      attribs.Add("Description", "OID: " + args.Row.GetObjectID().ToString() + " " + DateTime.Now.ToShortTimeString());

      //create a record in an audit table
      var sTable = MapView.Active.Map.FindStandaloneTables("EditHistory").First();
      var table = sTable.GetTable();
      parentEditOp.Create(table, attribs);
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.Events.RowCreatedEvent
    // cref: ArcGIS.Desktop.Editing.Events.RowChangedEventArgs.Operation
    // cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Core.Data.Table, System.Collections.Generic.Dictionary<System.String, System.Object>)
    #region Create a record in a separate table within Row Events

    // Use the EditOperation in the RowChangedEventArgs to append actions to be executed. 
    //  Your actions will become part of the operation and combined into one item on the undo stack

    private void HookCreatedEvent()
    {
      // subscribe to the RowCreatedEvent
      Table table = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault().GetTable();
      RowCreatedEvent.Subscribe(OnRowCreatedEvent, table);
    }

    private void OnRowCreatedEvent(RowChangedEventArgs args)
    {
      // RowEvent callbacks are always called on the QueuedTask so there is no need 
      // to wrap your code within a QueuedTask.Run lambda.

      // update a separate table not in the map when a row is created
      // You MUST use the ArcGIS.Core.Data API to edit the table. Do NOT
      // use a new edit operation in the RowEvent callbacks
      try
      {
        // get the edit operation
        var parentEditOp = args.Operation;

        // set up some attributes
        var attribs = new Dictionary<string, object> { };
        attribs.Add("Description", "OID: " + args.Row.GetObjectID().ToString() + " " + DateTime.Now.ToShortTimeString());

        // update Notes table with information about the new feature
        using (var geoDatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(Project.Current.DefaultGeodatabasePath))))
        {
          using (var table = geoDatabase.OpenDataset<Table>("Notes"))
          {
            parentEditOp.Create(table, attribs);
          }
        }
      }
      catch (Exception e)
      {
        MessageBox.Show($@"Error in OnRowCreated for OID: {args.Row.GetObjectID()} : {e.ToString()}");
      }
    }
    #endregion


    // cref: ArcGIS.Desktop.Editing.Events.RowChangedEvent
    // cref: ArcGIS.Desktop.Editing.Events.RowChangedEventArgs.Row
    // cref: ArcGIS.Desktop.Editing.Events.RowChangedEventArgs.Guid
    // cref: ArcGIS.Core.Data.Row.HasValueChanged(System.Int32)
    // cref: ArcGIS.Core.Data.Row.Store
    #region Modify a record within Row Events - using Row.Store

    private void HookRowChangedEvent()
    {
      // subscribe to the RowChangedEvent
      Table table = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault().GetTable();
      RowChangedEvent.Subscribe(OnRowChangedEvent, table);
    }

    private Guid _currentRowChangedGuid = Guid.Empty;
    protected void OnRowChangedEvent(RowChangedEventArgs args)
    {
      // RowEvent callbacks are always called on the QueuedTask so there is no need 
      // to wrap your code within a QueuedTask.Run lambda.

      var row = args.Row;

      // check for re-entry  (only if row.Store is called)
      if (_currentRowChangedGuid == args.Guid)
        return;

      var fldIdx = row.FindField("POLICE_DISTRICT");
      if (fldIdx != -1)
      {
        //Validate any change to �police district�
        //   cancel the edit if validation on the field fails
        if (row.HasValueChanged(fldIdx))
        {
          // cancel edit with invalid district (5)
          var value = row["POLICE_DISTRICT"].ToString();
          if (value == "5")
          {
            //Cancel edits with invalid �police district� values
            args.CancelEdit($"Police district {row["POLICE_DISTRICT"]} is invalid");
          }
        }

        // update the description field
        row["Description"] = "Row Changed";

        //  this update with cause another OnRowChanged event to occur
        //  keep track of the row guid to avoid recursion
        _currentRowChangedGuid = args.Guid;
        row.Store();
        _currentRowChangedGuid = Guid.Empty;
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.Events.RowChangedEvent
    // cref: ArcGIS.Desktop.Editing.Events.RowChangedEventArgs.Operation
    // cref: ArcGIS.Desktop.Editing.EditOperation.Modify(ArcGIS.Core.Data.Row, System.String, System.Object)
    #region Modify a record within Row Events - using EditOperation.Modify
    private void HookChangedEvent()
    {
      // subscribe to the RowChangedEvent
      Table table = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault().GetTable();
      RowChangedEvent.Subscribe(MyRowChangedEvent, table);
    }

    private void MyRowChangedEvent(RowChangedEventArgs args)
    {
      // RowEvent callbacks are always called on the QueuedTask so there is no need 
      // to wrap your code within a QueuedTask.Run lambda.

      //example of modifying a field on a row that has been created
      var parentEditOp = args.Operation;

      // avoid recursion
      if (_lastEdit != args.Guid)
      {
        //update field on change
        parentEditOp.Modify(args.Row, "ZONING", "New");

        _lastEdit = args.Guid;
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.Events.RowChangedEvent
    // cref: ArcGIS.Desktop.Editing.Events.RowChangedEventArgs.Row
    // cref: ArcGIS.Core.Data.Row.GetOriginalvalue
    #region Determine if Geometry Changed while editing
    private static FeatureLayer featureLayer;
    private static void DetermineGeometryChange()
    {
      featureLayer = MapView.Active?.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();
      if (featureLayer == null)
        return;

      QueuedTask.Run(() =>
      {
        //Listen to the RowChangedEvent that occurs when a Row is changed.
        ArcGIS.Desktop.Editing.Events.RowChangedEvent.Subscribe(OnRowChangedEvent2, featureLayer.GetTable());
      });
    }
    private static void OnRowChangedEvent2(RowChangedEventArgs args)
    {
      // RowEvent callbacks are always called on the QueuedTask so there is no need 
      // to wrap your code within a QueuedTask.Run lambda.

      //Get the layer's definition
      var lyrDefn = featureLayer.GetFeatureClass().GetDefinition();
      //Get the shape field of the feature class
      string shapeField = lyrDefn.GetShapeField();
      //Index of the shape field
      var shapeIndex = lyrDefn.FindField(shapeField);
      //Original geometry of the modified row
      var geomOrig = args.Row.GetOriginalValue(shapeIndex) as Geometry;
      //New geometry of the modified row
      var geomNew = args.Row[shapeIndex] as Geometry;
      //Compare the two
      bool shapeChanged = geomOrig.IsEqual(geomNew);
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.Events.RowDeletedEvent
    // cref: ArcGIS.Desktop.Editing.Events.RowChangedEventArgs.CancelEdit(System.String, System.Boolean)
    // cref: ArcGIS.Desktop.Editing.Events.RowChangedEventArgs.CancelEdit()
    #region Cancel a delete
    public void StopADelete()
    {
      // subscribe to the RowDeletedEvent for the appropriate table
      Table table = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault().GetTable();
      RowDeletedEvent.Subscribe(OnRowDeletedEvent, table);
    }

    private Guid _currentRowDeletedGuid = Guid.Empty;
    private void OnRowDeletedEvent(RowChangedEventArgs args)
    {
      // RowEvent callbacks are always called on the QueuedTask so there is no need 
      // to wrap your code within a QueuedTask.Run lambda.

      var row = args.Row;

      // check for re-entry 
      if (_currentRowDeletedGuid == args.Guid)
        return;

      // cancel the delete if the feature is in Police District 5
      var fldIdx = row.FindField("POLICE_DISTRICT");
      if (fldIdx != -1)
      {
        var value = row[fldIdx].ToString();
        if (value == "5")
        {
          //cancel with dialog
          // Note - feature edits on Hosted and Standard Feature Services cannot be cancelled.
          args.CancelEdit("Delete Event\nAre you sure", true);

          // or cancel without a dialog
          // args.CancelEdit();
        }
      }
      _currentRowDeletedGuid = args.Guid;
    }
    #endregion


    #region ProSnippet Group: EditCompletedEvent
    #endregion

    // cref: ArcGIS.Desktop.Editing.Events.EditCompletedEvent
    // cref: ArcGIS.Desktop.Editing.Events.EditCompletedEventArgs
    // cref: ArcGIS.Desktop.Editing.Events.EditCompletedEventArgs.Creates
    // cref: ArcGIS.Desktop.Editing.Events.EditCompletedEventArgs.Modifies
    // cref: ArcGIS.Desktop.Editing.Events.EditCompletedEventArgs.Deletes
    #region Subscribe to EditCompletedEvent

    protected void SubEditEvents()
    {
      //subscribe to editcompleted
      var eceToken = EditCompletedEvent.Subscribe(OnEce);
    }

    protected Task OnEce(EditCompletedEventArgs args)
    {
      //show number of edits
      Console.WriteLine("Creates: " + args.Creates.ToDictionary().Values.Sum(list => list.Count).ToString());
      Console.WriteLine("Modifies: " + args.Modifies.ToDictionary().Values.Sum(list => list.Count).ToString());
      Console.WriteLine("Deletes: " + args.Deletes.ToDictionary().Values.Sum(list => list.Count).ToString());
      return Task.FromResult(0);
    }
    #endregion


    #region ProSnippet Group: Inspector
    #endregion

    public async void LoadFirstFeature2Inspector()
    {
      int oid = 0;

      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.#ctor
      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.LoadAsync(ArcGIS.Desktop.Mapping.MapMember, System.Int64)
      #region Load a feature from a layer into the inspector

      // get the first feature layer in the map
      var firstFeatureLayer = ArcGIS.Desktop.Mapping.MapView.Active.Map.GetLayersAsFlattenedList().
          OfType<ArcGIS.Desktop.Mapping.FeatureLayer>().FirstOrDefault();

      // create an instance of the inspector class
      var inspector = new ArcGIS.Desktop.Editing.Attributes.Inspector();
      // load the feature with ObjectID 'oid' into the inspector
      await inspector.LoadAsync(firstFeatureLayer, oid);

      #endregion
    }

    public async void LoadSelection2Inspector()
    {
      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.LoadAsync(MAPMEMBER,INT64)
      // cref: ArcGIS.Desktop.Mapping.MapMemberIDSet.ToDictionary
      // cref: ArcGIS.Desktop.Mapping.MapMemberIDSet.ToDictionary``1()
      // cref: ArcGIS.Desktop.Mapping.MapMemberIDSet.ToDictionary()
      // cref: ArcGIS.Desktop.Mapping.Map.GetSelection
      #region Load map selection into Inspector

      // get the currently selected features in the map
      var selectedFeatures = ArcGIS.Desktop.Mapping.MapView.Active.Map.GetSelection();
      // get the first layer and its corresponding selected feature OIDs
      var firstSelectionSet = selectedFeatures.ToDictionary().First();

      // create an instance of the inspector class
      var inspector = new ArcGIS.Desktop.Editing.Attributes.Inspector();
      // load the selected features into the inspector using a list of object IDs
      await inspector.LoadAsync(firstSelectionSet.Key, firstSelectionSet.Value);
      #endregion
    }

    public static void InspectorGetAttributeValue()
    {
      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.Load(ArcGIS.Desktop.Mapping.MapMember, System.Int64)
      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.Shape
      // cref: ArcGIS.Desktop.Mapping.MapMemberIDSet.ToDictionary
      // cref: ArcGIS.Desktop.Mapping.MapMemberIDSet.ToDictionary``1()
      // cref: ArcGIS.Desktop.Mapping.MapMemberIDSet.ToDictionary()
      // cref: ArcGIS.Desktop.Mapping.Map.GetSelection
      #region Get selected feature's attribute value
      QueuedTask.Run(() =>
      {

        // get the currently selected features in the map
        var selectedFeatures = ArcGIS.Desktop.Mapping.MapView.Active.Map.GetSelection();

        // get the first layer and its corresponding selected feature OIDs
        var firstSelectionSet = selectedFeatures.ToDictionary().First();

        // create an instance of the inspector class
        var inspector = new ArcGIS.Desktop.Editing.Attributes.Inspector();

        // load the selected features into the inspector using a list of object IDs
        inspector.Load(firstSelectionSet.Key, firstSelectionSet.Value);

        //get the value of
        var pscode = inspector["STATE_NAME"];
        var myGeometry = inspector.Shape;
      });
      #endregion
    }

    public async void InspectorChangeAttributes()
    {
      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.Item(System.String)
      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.ApplyAsync()
      #region Load map selection into Inspector and Change Attributes

      // get the currently selected features in the map
      var selectedFeatures = ArcGIS.Desktop.Mapping.MapView.Active.Map.GetSelection();
      // get the first layer and its corresponding selected feature OIDs
      var firstSelectionSet = selectedFeatures.ToDictionary().First();

      // create an instance of the inspector class
      var inspector = new ArcGIS.Desktop.Editing.Attributes.Inspector();
      // load the selected features into the inspector using a list of object IDs
      await inspector.LoadAsync(firstSelectionSet.Key, firstSelectionSet.Value);

      // assign the new attribute value to the field "Description"
      // if more than one features are loaded, the change applies to all features
      inspector["Description"] = "The new value.";
      // apply the changes as an edit operation 
      await inspector.ApplyAsync();
      #endregion
    }

    public void SchemaAttributes()
    {
      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.LoadSchema(ArcGIS.Desktop.Mapping.MapMember)
      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.GetEnumerator()
      // cref: ArcGIS.Desktop.Editing.Attributes.Attribute
      // cref: ArcGIS.Desktop.Editing.Attributes.Attribute.FieldName
      // cref: ArcGIS.Desktop.Editing.Attributes.Attribute.FieldAlias
      // cref: ArcGIS.Desktop.Editing.Attributes.Attribute.FieldType
      // cref: ArcGIS.Desktop.Editing.Attributes.Attribute.IsNullable
      // cref: ArcGIS.Desktop.Editing.Attributes.Attribute.IsEditable
      // cref: ArcGIS.Desktop.Editing.Attributes.Attribute.IsVisible
      // cref: ArcGIS.Desktop.Editing.Attributes.Attribute.IsSystemField
      // cref: ArcGIS.Desktop.Editing.Attributes.Attribute.IsGeometryField
      // cref: ArcGIS.Desktop.Editing.Attributes.Attribute.GetField()
      #region Get a layers schema using Inspector
      QueuedTask.Run(() =>
      {
        var firstFeatureLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<ArcGIS.Desktop.Mapping.FeatureLayer>().FirstOrDefault();

        // create an instance of the inspector class
        var inspector = new ArcGIS.Desktop.Editing.Attributes.Inspector();

        // load the layer
        inspector.LoadSchema(firstFeatureLayer);

        // iterate through the attributes, looking at properties
        foreach (var attribute in inspector)
        {
          var fldName = attribute.FieldName;
          var fldAlias = attribute.FieldAlias;
          var fldType = attribute.FieldType;
          int idxFld = attribute.FieldIndex;
          var fld = attribute.GetField();
          var isNullable = attribute.IsNullable;
          var isEditable = attribute.IsEditable;
          var isVisible = attribute.IsVisible;
          var isSystemField = attribute.IsSystemField;
          var isGeometryField = attribute.IsGeometryField;
        }
      });
      #endregion
    }
    protected void addvalidate()
    {
      var featLayer = MapView.Active.GetSelectedLayers().First() as FeatureLayer;

      // cref: ArcGIS.Desktop.Editing.Attributes.Attribute.ValidationError.Create(System.String,ArcGIS.Desktop.Editing.Attributes.Severity)
      // cref: ArcGIS.Desktop.Editing.Attributes.Attribute.AddValidate
      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.#ctor
      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector
      #region Inspector.AddValidate
      var insp = new Inspector();
      insp.LoadSchema(featLayer);
      var attrib = insp.Where(a => a.FieldName == "Mineral").First();

      attrib.AddValidate(() =>
      {
        if (attrib.CurrentValue.ToString() == "Salt")
          return Enumerable.Empty<ArcGIS.Desktop.Editing.Attributes.Attribute.ValidationError>();
        else return new[] { ArcGIS.Desktop.Editing.Attributes.Attribute.ValidationError.Create("Error", ArcGIS.Desktop.Editing.Attributes.Severity.Low) };
      });
      #endregion
    }

    #region ProSnippet Group: Accessing Blob Fields
    #endregion

    public static void ReadWriteBlobInspector()
    {
      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector
      // cref: ARCGIS.DESKTOP.EDITING.EDITOPERATION.MODIFY(INSPECTOR)
      // cref: ARCGIS.DESKTOP.EDITING.EDITOPERATION.EXECUTE
      #region Read and Write blob fields with the attribute inspector
      QueuedTask.Run(() =>
      {
        //get selected feature into inspector
        var selectedFeatures = MapView.Active.Map.GetSelection();

        var insp = new Inspector();
        insp.Load(selectedFeatures.ToDictionary().Keys.First(), selectedFeatures.ToDictionary().Values.First());

        //read a blob field and save to a file
        var msw = new MemoryStream();
        msw = insp["Blobfield"] as MemoryStream;
        using (FileStream file = new FileStream(@"d:\temp\blob.jpg", FileMode.Create, FileAccess.Write))
        {
          msw.WriteTo(file);
        }

        //read file into memory stream
        var msr = new MemoryStream();
        using (FileStream file = new FileStream(@"d:\images\Hydrant.jpg", FileMode.Open, FileAccess.Read))
        {
          file.CopyTo(msr);
        }

        //put the memory stream in the blob field and save to feature
        var op = new EditOperation() { Name = "Blob Inspector" };
        insp["Blobfield"] = msr;
        op.Modify(insp);
        if (!op.IsEmpty)
        {
          var result = op.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
        }
      });
      #endregion
    }

    public static void ReadWriteBlobRow()
    {
      // cref: ARCGIS.DESKTOP.EDITING.EDITOPERATION.CALLBACK(ACTION{IEDITCONTEXT},DATASET)
      // cref: ARCGIS.DESKTOP.EDITING.EDITOPERATION.CALLBACK(ACTION{IEDITCONTEXT},IENUMERABLE{DATASET})
      // cref: ARCGIS.DESKTOP.EDITING.EDITOPERATION.CALLBACK(ACTION{IEDITCONTEXT},DATASET[])
      #region Read and Write blob fields with a row cursor in a callback
      QueuedTask.Run(() =>
      {
        var editOp = new EditOperation() { Name = "Blob Cursor" };
        var featLayer = MapView.Active.Map.FindLayers("Hydrant").First() as FeatureLayer;

        editOp.Callback((context) =>
        {
          using (var rc = featLayer.GetTable().Search(null, false))
          {
            while (rc.MoveNext())
            {
              using (var record = rc.Current)
              {
                //read the blob field and save to a file
                var msw = new MemoryStream();
                msw = record["BlobField"] as MemoryStream;
                using (FileStream file = new FileStream(@"d:\temp\blob.jpg", FileMode.Create, FileAccess.Write))
                {
                  msw.WriteTo(file);
                }

                //read file into memory stream
                var msr = new MemoryStream();
                using (FileStream file = new FileStream(@"d:\images\Hydrant.jpg", FileMode.Open, FileAccess.Read))
                {
                  file.CopyTo(msr);
                }

                //put the memory stream in the blob field and save to feature
                record["BlobField"] = msr;
                record.Store();

              }
            }
          }
        }, featLayer.GetTable());
        if (!editOp.IsEmpty)
        {
          var result = editOp.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
        }
      });
      #endregion
    }

    #region ProSnippet Group: Accessing Raster Fields
    #endregion

    public static void ReadFromRasterField()
    {
      // cref: ARCGIS.DESKTOP.EDITING.ATTRIBUTES.INSPECTOR.LOAD(MAPMEMBER,INT64)
      #region Read from a raster field
      QueuedTask.Run(() =>
      {
        var sel = MapView.Active.Map.GetSelection();

        //Read a raster from a raster field as an InteropBitmap
        //the bitmap can then be used as an imagesource or written to disk
        var insp = new ArcGIS.Desktop.Editing.Attributes.Inspector();
        insp.Load(sel.ToDictionary().Keys.First(), sel.ToDictionary().Values.First());
        var ibmp = insp["Photo"] as System.Windows.Interop.InteropBitmap;
      });
      #endregion
    }

    public static void WriteImageToRasterField()
    {
      // cref: ARCGIS.DESKTOP.EDITING.ATTRIBUTES.INSPECTOR.LOAD(MAPMEMBER,INT64)
      #region Write an image to a raster field
      QueuedTask.Run(() =>
      {
        var sel = MapView.Active.Map.GetSelection();

        //Insert an image into a raster field
        //Image will be written with no compression
        var insp = new ArcGIS.Desktop.Editing.Attributes.Inspector();
        insp.Load(sel.ToDictionary().Keys.First(), sel.ToDictionary().Values.First());
        insp["Photo"] = @"e:\temp\Hydrant.jpg";

        var op = new EditOperation() { Name = "Raster Inspector" };
        op.Modify(insp);
        if (!op.IsEmpty)
        {
          var result = op.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
        }
      });
      #endregion
    }

    public static void WriteCompImageToRasterField()
    {
      // cref: ArcGIS.Core.Data.Raster.RasterStorageDef.#ctor
      // cref: ArcGIS.Core.Data.Raster.RasterStorageDef.#ctor()
      // cref: ARCGIS.CORE.DATA.RASTER.RASTERSTORAGEDEF.SETCOMPRESSIONTYPE
      // cref: ARCGIS.CORE.DATA.RASTER.RASTERSTORAGEDEF.SETCOMPRESSIONQUALITY
      // cref: ARCGIS.DESKTOP.EDITING.EDITOPERATION.MODIFY(INSPECTOR)
      // cref: ArcGIS.Core.Data.Raster.RasterValue.SetRasterStorageDef
      // cref: ArcGIS.Core.Data.Raster.RasterValue.SetRasterDataset
      #region Write a compressed image to a raster field
      QueuedTask.Run(() =>
      {
        //Open the raster dataset on disk and create a compressed raster value dataset object
        var dataStore = new ArcGIS.Core.Data.FileSystemDatastore(new ArcGIS.Core.Data.FileSystemConnectionPath(new System.Uri(@"e:\temp"), ArcGIS.Core.Data.FileSystemDatastoreType.Raster));
        using (var fileRasterDataset = dataStore.OpenDataset<ArcGIS.Core.Data.Raster.RasterDataset>("Hydrant.jpg"))
        {
          var storageDef = new ArcGIS.Core.Data.Raster.RasterStorageDef();
          storageDef.SetCompressionType(ArcGIS.Core.Data.Raster.RasterCompressionType.JPEG);
          storageDef.SetCompressionQuality(90);

          var rv = new ArcGIS.Core.Data.Raster.RasterValue();
          rv.SetRasterDataset(fileRasterDataset);
          rv.SetRasterStorageDef(storageDef);

          var sel = MapView.Active.Map.GetSelection();

          //insert a raster value object into the raster field
          var insp = new ArcGIS.Desktop.Editing.Attributes.Inspector();
          insp.Load(sel.ToDictionary().Keys.First(), sel.ToDictionary().Values.First());
          insp["Photo"] = rv;

          var op = new EditOperation() { Name = "Raster Inspector" };
          op.Modify(insp);
          if (!op.IsEmpty)
          {
            var result = op.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
          }
        }
      });
      #endregion
    }

    #region ProSnippet Group: Inspector Provider Class
    #endregion

    // cref: ArcGIS.Desktop.Editing.InspectorProvider
    #region How to create a custom Feature inspector provider class

    public class MyProvider : InspectorProvider
    {
      private System.Guid guid = System.Guid.NewGuid();
      internal MyProvider()
      {
      }
      public override System.Guid SharedFieldColumnSizeID()
      {
        return guid;
      }

      public override string CustomName(Attribute attr)
      {
        //Giving a custom name to be displayed for the field FeatureID
        if (attr.FieldName == "FeatureID")
          return "Feature Identification";

        return attr.FieldName;
      }
      public override bool? IsVisible(Attribute attr)
      {
        //The field FontStyle will not be visible
        if (attr.FieldName == "FontStyle")
          return false;

        return true;
      }
      public override bool? IsEditable(Attribute attr)
      {
        //The field DateField will not be editable
        if (attr.FieldName == "DateField")
          return false;

        return true;
      }
      public override bool? IsHighlighted(Attribute attr)
      {
        //ZOrder field will be highlighted in the feature inspector grid
        if (attr.FieldName == "ZOrder")
          return true;

        return false;
      }

      public override IEnumerable<Attribute> AttributesOrder(IEnumerable<Attribute> attrs)
      {
        //Reverse the order of display
        var newList = new List<Attribute>();
        foreach (var attr in attrs)
        {
          newList.Insert(0, attr);
        }
        return newList;
      }

      public override bool? IsDirty(Attribute attr)
      {
        //The field will not be marked dirty for FeatureID if you enter the value -1
        if ((attr.FieldName == "FeatureID") && (attr.CurrentValue.ToString() == "-1"))
          return false;

        return base.IsDirty(attr);
      }

      public override IEnumerable<ArcGIS.Desktop.Editing.Attributes.Attribute.ValidationError> Validate(Attribute attr)
      {
        var errors = new List<ArcGIS.Desktop.Editing.Attributes.Attribute.ValidationError>();

        if ((attr.FieldName == "FeatureID") && (attr.CurrentValue.ToString() == "2"))
          errors.Add(ArcGIS.Desktop.Editing.Attributes.Attribute.ValidationError.Create("Value not allowed", ArcGIS.Desktop.Editing.Attributes.Severity.Low));

        if ((attr.FieldName == "FeatureID") && (attr.CurrentValue.ToString() == "-1"))
          errors.Add(ArcGIS.Desktop.Editing.Attributes.Attribute.ValidationError.Create("Invalid value", ArcGIS.Desktop.Editing.Attributes.Severity.High));

        return errors;
      }
    }
    #endregion

    public async void InspectorProviderExample()
    {
      int oid = 1;
      // cref: ArcGIS.Desktop.Editing.InspectorProvider
      // cref: ArcGIS.Desktop.Editing.InspectorProvider.Create()
      #region Using the custom inspector provider class
      var layer = ArcGIS.Desktop.Mapping.MapView.Active.Map.GetLayersAsFlattenedList().OfType<ArcGIS.Desktop.Mapping.FeatureLayer>().FirstOrDefault();

      var provider = new MyProvider();
      Inspector _featureInspector = provider.Create();
      //Create an embeddable control from the inspector class to display on the pane
      var icontrol = _featureInspector.CreateEmbeddableControl();

      await _featureInspector.LoadAsync(layer, oid);
      var attribute = _featureInspector.Where(a => a.FieldName == "FontStyle").FirstOrDefault();
      var visibility = attribute.IsVisible; //Will return false

      attribute = _featureInspector.Where(a => a.FieldName == "ZOrder").FirstOrDefault();
      var highlighted = attribute.IsHighlighted; //Will return true
      #endregion
    }

    #region ProSnippet Group: Working with the Sketch
    #endregion

    // cref: ARCGIS.DESKTOP.MAPPING.MAPTOOL.ACTIVATESELECTASYNC
    #region Toggle sketch selection mode
    //UseSelection = true; (UseSelection must be set to true in the tool constructor or tool activate)
    private bool _inSelMode = false;

    public bool IsShiftKey(MapViewKeyEventArgs k)
    {
      return (k.Key == System.Windows.Input.Key.LeftShift ||
             k.Key == System.Windows.Input.Key.RightShift);
    }

    protected override async void OnToolKeyDown(MapViewKeyEventArgs k)
    {
      //toggle sketch selection mode with a custom key
      if (k.Key == System.Windows.Input.Key.W)
      {
        if (!_inSelMode)
        {
          k.Handled = true;

          // Toggle the tool to select mode.
          //  The sketch is saved if UseSelection = true;
          if (await ActivateSelectAsync(true))
            _inSelMode = true;
        }
      }
      else if (!_inSelMode)
      {
        //disable effect of Shift in the base class.
        //Mark the key event as handled to prevent further processing
        k.Handled = IsShiftKey(k);
      }
    }
    protected override void OnToolKeyUp(MapViewKeyEventArgs k)
    {
      if (k.Key == System.Windows.Input.Key.W)
      {
        if (_inSelMode)
        {
          _inSelMode = false;
          k.Handled = true;//process this one

          // Toggle back to sketch mode. If UseSelection = true
          //   the sketch will be restored
          ActivateSelectAsync(false);
        }
      }
      else if (_inSelMode)
      {
        //disable effect of Shift in the base class.
        //Mark the key event as handled to prevent further processing
        k.Handled = IsShiftKey(k);
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.Events.SketchModifiedEventArgs.CurrentSketch
    // cref: ArcGIS.Desktop.Mapping.Events.SketchModifiedEventArgs.PreviousSketch
    #region Listen to the sketch modified event

    // SketchModified event is fired by 
    //  - COTS construction tools (except annotation, dimension geometry types), 
    //  - Edit Vertices, Reshape, Align Features
    //  - 3rd party tools with FireSketchEvents = true


    //Subscribe the sketch modified event
    //ArcGIS.Desktop.Mapping.Events.SketchModifiedEvent.Subscribe(OnSketchModified);

    private void OnSketchModified(ArcGIS.Desktop.Mapping.Events.SketchModifiedEventArgs args)
    {
      // if not an undo operation
      if (!args.IsUndo)
      {
        // what was the sketch before the change?
        var prevSketch = args.PreviousSketch;
        // what is the current sketch?
        var currentSketch = args.CurrentSketch;
        if (currentSketch is Polyline polyline)
        {
          // Examine the current (last) vertex in the line sketch
          var lastSketchPoint = polyline.Points.Last();

          // do something with the last point
        }
      }
    }

    #endregion

    // cref: ArcGIS.Desktop.Mapping.Events.BeforeSketchCompletedEventArgs.Sketch
    // cref: ArcGIS.Desktop.Mapping.Events.BeforeSketchCompletedEventArgs.SetSketchGeometry
    #region Listen to the before sketch completed event and modify the sketch

    // BeforeSketchCompleted event is fired by 
    //  - COTS construction tools (except annotation, dimension geometry types), 
    //  - Edit Vertices, Reshape, Align Features
    //  - 3rd party tools with FireSketchEvents = true


    //Subscribe to the before sketch completed event
    //ArcGIS.Desktop.Mapping.Events.BeforeSketchCompletedEvent.Subscribe(OnBeforeSketchCompleted);

    private Task OnBeforeSketchCompleted(BeforeSketchCompletedEventArgs args)
    {
      //assign sketch Z values from default surface and set the sketch geometry
      var modifiedSketch = args.MapView.Map.GetZsFromSurfaceAsync(args.Sketch).Result;
      args.SetSketchGeometry(modifiedSketch.Geometry);
      return Task.CompletedTask;
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.Events.SketchCompletedEventArgs.Sketch
    #region Listen to the sketch completed event

    // SketchCompleted event is fired by 
    //  - COTS construction tools (except annotation, dimension geometry types), 
    //  - Edit Vertices, Reshape, Align Features
    //  - 3rd party tools with FireSketchEvents = true


    //Subscribe to the sketch completed event
    //ArcGIS.Desktop.Mapping.Events.SketchCompletedEvent.Subscribe(OnSketchCompleted);

    private void OnSketchCompleted(SketchCompletedEventArgs args)
    {
      // get the sketch
      var finalSketch = args.Sketch;

      // do something with the sketch - audit trail perhaps
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapTool
    // cref: ArcGIS.Desktop.Mapping.MapTool.FireSketchEvents
    #region Custom construction tool that fires sketch events

    internal class ConstructionTool1 : MapTool
    {
      public ConstructionTool1()
      {
        IsSketchTool = true;
        UseSnapping = true;
        // Select the type of construction tool you wish to implement.  
        // Make sure that the tool is correctly registered with the correct component category type in the daml 
        SketchType = SketchGeometryType.Line;
        //Gets or sets whether the sketch is for creating a feature and should use the CurrentTemplate.
        UsesCurrentTemplate = true;

        // set FireSketchEvents property to true
        FireSketchEvents = true;
      }

      //  ...
    }

    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapTool.GetSketchSegmentSymbolOptions()
    // cref: ArcGIS.Desktop.Core.SegmentSymbolOptions
    // cref: ArcGIS.Desktop.Mapping.MapTool.SetSketchSegmentSymbolOptions
    // cref: ArcGIS.Desktop.Mapping.MapTool.SetSketchVertexSymbolOptions
    // cref: ArcGIS.Desktop.Core.VertexSymbolOptions
    #region Customizing the Sketch Symbol of a Custom Sketch Tool

    //Custom tools have the ability to change the symbology used when sketching a new feature. 
    //Both the Sketch Segment Symbol and the Vertex Symbol can be modified using the correct set method. 
    //This is set in the activate method for the tool.
    protected override Task OnToolActivateAsync(bool active)
    {
      QueuedTask.Run(() =>
      {
        //Getting the current symbology options of the segment
        var segmentOptions = GetSketchSegmentSymbolOptions();
        //Modifying the primary and secondary color and the width of the segment symbology options
        var deepPurple = new CIMRGBColor() { R = 75, G = 0, B = 110 };
        segmentOptions.PrimaryColor = deepPurple;
        segmentOptions.Width = 4;
        segmentOptions.HasSecondaryColor = true;
        var pink = new CIMRGBColor() { R = 219, G = 48, B = 130 };
        segmentOptions.SecondaryColor = pink;
        //Creating a new vertex symbol options instance with the values you want
        var vertexOptions = new VertexSymbolOptions(VertexSymbolType.RegularUnselected);
        var yellow = new CIMRGBColor() { R = 255, G = 215, B = 0 };
        var purple = new CIMRGBColor() { R = 148, G = 0, B = 211 };
        vertexOptions.AngleRotation = 45;
        vertexOptions.Color = yellow;
        vertexOptions.MarkerType = VertexMarkerType.Star;
        vertexOptions.OutlineColor = purple;
        vertexOptions.OutlineWidth = 3;
        vertexOptions.Size = 5;

        //Setting the value of the segment symbol options
        SetSketchSegmentSymbolOptions(segmentOptions);
        //Setting the value of the vertex symbol options of the regular unselected vertices using the vertexOptions instance created above.
        SetSketchVertexSymbolOptions(VertexSymbolType.RegularUnselected, vertexOptions);
      });

      return base.OnToolActivateAsync(active);
    }
    #endregion
  }

  #region ProSnippet Group: SketchTool
  #endregion

  // cref: ArcGIS.Desktop.Mapping.MapTool.ContextMenuID
  // cref: ArcGIS.Desktop.Mapping.MapTool.ContextToolbarID
  #region Set a MiniToolbar, ContextMenuID

  // daml entries
  // 
  // <menus>
  //  <menu id="MyMenu" caption="Nav">
  //    <button refID="esri_mapping_prevExtentButton"/>
  //    <button refID="esri_mapping_fixedZoomInButton"/>
  //  </menu>
  // </menus>
  // <miniToolbars>
  //   <miniToolbar id="MyMiniToolbar">
  //    <row>
  //      <button refID="esri_mapping_fixedZoomInButton"/>
  //      <button refID="esri_mapping_prevExtentButton"/>
  //    </row>
  //   </miniToolbar>
  // </miniToolbars>

  public class SketchToolWithToolbar : MapTool
  {
    public SketchToolWithToolbar()
    {
      IsSketchTool = true;
      SketchType = SketchGeometryType.Line;
      SketchOutputMode = SketchOutputMode.Map;
      ContextMenuID = "MyMenu";
      ContextToolbarID = "MyMiniToolbar";
    }
  }
  #endregion

  // cref: ArcGIS.Desktop.Mapping.MapTool.SketchTip
  #region Set a simple sketch tip
  public class SketchToolWithSketchTip : MapTool
  {
    public SketchToolWithSketchTip()
    {
      IsSketchTool = true;
      SketchType = SketchGeometryType.Line;
      SketchOutputMode = SketchOutputMode.Map;
      SketchTip = "hello World";
    }
  }
  #endregion

  public class EmbeddableControl1ViewModel : ArcGIS.Desktop.Framework.Controls.EmbeddableControl
  {
    public EmbeddableControl1ViewModel(XElement options, bool canChangeOptions) : base(options, canChangeOptions)
    { }

    public string Text;
  }

  // cref: ArcGIS.Desktop.Mapping.MapTool.SketchTipID
  // cref: ArcGIS.Desktop.Mapping.MapTool.SketchTipEmbeddableControl
  #region Set a custom UI Sketch Tip

  // 1. Add an embeddable control using VS template.  This is the daml entry

  //<categories>
  //  <updateCategory refID = "esri_embeddableControls">
  //    <insertComponent id="SketchTip_EmbeddableControl1" className="EmbeddableControl1ViewModel">
  //      <content className = "EmbeddableControl1View"/>
  //    </insertComponent>
  //  </updateCategory>
  // </categories>

  // 2. Define UI controls on the EmbeddableControl1View
  // 3. Define properties on the EmbeddableControl1ViewModel which
  //    bind to the UI controls on the EmbeddableControl1View

  public class SketchToolWithUISketchTip : MapTool
  {
    public SketchToolWithUISketchTip()
    {
      IsSketchTool = true;
      SketchType = SketchGeometryType.Line;
      SketchOutputMode = SketchOutputMode.Map;
      SketchTipID = "SketchTip_EmbeddableControl1";
    }


    protected override Task<bool> OnSketchModifiedAsync()
    {
      var sketchTipVM = SketchTipEmbeddableControl as EmbeddableControl1ViewModel;
      if (sketchTipVM != null)
      {
        // modify properties on the sketchTipVM
        QueuedTask.Run(async () =>
        {
          var sketch = await GetCurrentSketchAsync();
          var line = sketch as Polyline;
          var count = line.PointCount;

          sketchTipVM.Text = "Vertex Count " + count.ToString();
        });


      }

      return base.OnSketchModifiedAsync();
    }

  }
  #endregion

  public class Snippets
  {

    #region ProSnippet Group: Snapping
    #endregion


    private async void Snapping()
    {
      Map myMap = null;
      FeatureLayer fLayer = null;
      IEnumerable<FeatureLayer> layerList = null;

      // cref: ArcGIS.Desktop.Mapping.Snapping.IsEnabled
      #region Configure Snapping - Turn Snapping on or off

      //enable snapping
      ArcGIS.Desktop.Mapping.Snapping.IsEnabled = true;

      // disable snapping
      ArcGIS.Desktop.Mapping.Snapping.IsEnabled = false;
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Snapping.SetSnapModes(System.Collections.Generic.IEnumerable<ArcGIS.Desktop.Mapping.SnapMode>)
      // cref: ArcGIS.Desktop.Mapping.Snapping.SetSnapMode(ArcGIS.Desktop.Mapping.SnapMode, System.Boolean)
      // cref: ArcGIS.Desktop.Mapping.Snapping.SnapModes
      // cref: ArcGIS.Desktop.Mapping.Snapping.GetSnapMode(ArcGIS.Desktop.Mapping.SnapMode)
      #region Configure Snapping - Application SnapModes

      // set only Point and Edge snapping modes, clear everything else
      //At 2.x - ArcGIS.Desktop.Mapping.Snapping.SetSnapModes(SnapMode.Point, SnapMode.Edge);
      ArcGIS.Desktop.Mapping.Snapping.SetSnapModes(
        new List<SnapMode>() { SnapMode.Point, SnapMode.Edge });

      // clear all snap modes
      //At 2.x - ArcGIS.Desktop.Mapping.Snapping.SetSnapModes();
      ArcGIS.Desktop.Mapping.Snapping.SetSnapModes(null);


      // set snap modes one at a time
      ArcGIS.Desktop.Mapping.Snapping.SetSnapMode(SnapMode.Edge, true);
      ArcGIS.Desktop.Mapping.Snapping.SetSnapMode(SnapMode.End, true);
      ArcGIS.Desktop.Mapping.Snapping.SetSnapMode(SnapMode.Intersection, true);

      // get current snap modes
      var snapModes = ArcGIS.Desktop.Mapping.Snapping.SnapModes;

      // get state of a specific snap mode
      bool isOn = ArcGIS.Desktop.Mapping.Snapping.GetSnapMode(SnapMode.Vertex);

      #endregion

      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.IsSnappable
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetSnappable(System.Boolean)
      // cref: ARCGIS.CORE.CIM.CIMGEOFEATURELAYERBASE.SNAPPABLE
      #region Configure Snapping - Layer Snappability

      // is the layer snappable?
      bool isSnappable = fLayer.IsSnappable;

      // set snappability for a specific layer - needs to run on the MCT
      await QueuedTask.Run(() =>
      {
        // use an extension method
        fLayer.SetSnappable(true);

        // or use the CIM directly
        //var layerDef = fLayer.GetDefinition() as ArcGIS.Core.CIM.CIMGeoFeatureLayerBase;
        //layerDef.Snappable = true;
        //fLayer.SetDefinition(layerDef);
      });


      // turn all layers snappability off
      layerList = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>();
      await QueuedTask.Run(() =>
      {
        foreach (var layer in layerList)
        {
          layer.SetSnappable(false);
        }
      });
      #endregion

      // cref: ArcGIS.Desktop.Mapping.LayerSnapModes.GetSnapMode(ArcGIS.Desktop.Mapping.SnapMode)
      // cref: ArcGIS.Desktop.Mapping.LayerSnapModes.SetSnapMode(ArcGIS.Desktop.Mapping.SnapMode, System.Boolean)
      // cref: ArcGIS.Desktop.Mapping.LayerSnapModes
      // cref: ArcGIS.Desktop.Mapping.LayerSnapModes.Edge
      // cref: ArcGIS.Desktop.Mapping.LayerSnapModes.End
      // cref: ArcGIS.Desktop.Mapping.LayerSnapModes.#ctor(System.Boolean)
      // cref: ArcGIS.Desktop.Mapping.LayerSnapModes.Vertex
      // cref: ArcGIS.Desktop.Mapping.Snapping.GetLayerSnapModes(ArcGIS.Desktop.Mapping.Layer)
      // cref: ArcGIS.Desktop.Mapping.Snapping.GetLayerSnapModes(IEnumerable{Layer})
      // cref: ARCGIS.DESKTOP.MAPPING.Snapping.SetLayerSnapModes(Layer,Boolean)
      // cref: ARCGIS.DESKTOP.MAPPING.Snapping.SetLayerSnapModes(IEnumerable{Layer},Boolean)
      // cref: ARCGIS.DESKTOP.MAPPING.Snapping.SetLayerSnapModes(Layer,LayerSnapModes)
      // cref: ARCGIS.DESKTOP.MAPPING.Snapping.SetLayerSnapModes(Layer,SnapMode,Boolean)
      // cref: ARCGIS.DESKTOP.MAPPING.Snapping.SetLayerSnapModes(IEnumerable{Layer},LayerSnapModes)
      // cref: ARCGIS.DESKTOP.MAPPING.Snapping.SetLayerSnapModes(IDictionary{Layer,LayerSnapModes},Boolean)      
      // cref: ArcGIS.Desktop.Mapping.LayerSnapModes.Intersection
      #region Configure Snapping - LayerSnapModes

      layerList = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>();

      // configure by layer
      foreach (var layer in layerList)
      {
        // find the state of the snapModes for the layer
        var lsm = ArcGIS.Desktop.Mapping.Snapping.GetLayerSnapModes(layer);
        bool vertexOn = lsm.Vertex;
        // or use 
        vertexOn = lsm.GetSnapMode(SnapMode.Vertex);

        bool edgeOn = lsm.Edge;
        // or use 
        edgeOn = lsm.GetSnapMode(SnapMode.Edge);

        bool endOn = lsm.End;
        // or use 
        endOn = lsm.GetSnapMode(SnapMode.End);

        // update a few snapModes 
        //   turn Vertex off
        lsm.SetSnapMode(SnapMode.Vertex, false);
        // intersections on
        lsm.SetSnapMode(SnapMode.Intersection, true);

        // and set back to the layer
        ArcGIS.Desktop.Mapping.Snapping.SetLayerSnapModes(layer, lsm);


        // assign a single snap mode at once
        ArcGIS.Desktop.Mapping.Snapping.SetLayerSnapModes(layer, SnapMode.Vertex, false);


        // turn ALL snapModes on
        ArcGIS.Desktop.Mapping.Snapping.SetLayerSnapModes(layer, true);
        // turn ALL snapModes off
        ArcGIS.Desktop.Mapping.Snapping.SetLayerSnapModes(layer, false);
      }


      // configure for a set of layers

      // set Vertex, edge, end on for a set of layers, other snapModes false
      var vee = new LayerSnapModes(false)
      {
        Vertex = true,
        Edge = true,
        End = true
      };
      ArcGIS.Desktop.Mapping.Snapping.SetLayerSnapModes(layerList, vee);


      // ensure intersection is on for a set of layers without changing any other snapModes

      // get the layer snapModes for the set of layers
      var dictLSM = ArcGIS.Desktop.Mapping.Snapping.GetLayerSnapModes(layerList);
      foreach (var layer in dictLSM.Keys)
      {
        var lsm = dictLSM[layer];
        lsm.Intersection = true;
      }
      ArcGIS.Desktop.Mapping.Snapping.SetLayerSnapModes(dictLSM);


      // set all snapModes off for a list of layers
      ArcGIS.Desktop.Mapping.Snapping.SetLayerSnapModes(layerList, false);


      #endregion

      // cref: ArcGIS.Desktop.Mapping.Snapping.SetSnapModes
      // cref: ArcGIS.Desktop.Mapping.Snapping.SetSnapMode(ArcGIS.Desktop.Mapping.SnapMode, System.Boolean)
      // cref: ARCGIS.DESKTOP.MAPPING.Snapping.SetLayerSnapModes(IDictionary{Layer,LayerSnapModes},Boolean)      
      // cref: ARCGIS.DESKTOP.MAPPING.FEATURELAYER.SETSNAPPABLE
      #region Configure Snapping - Combined Example

      // interested in only snapping to the vertices of a specific layer of interest and not the vertices of other layers
      //  all other snapModes should be off.

      // snapping must be on
      ArcGIS.Desktop.Mapping.Snapping.IsEnabled = true;

      // turn all application snapModes off
      //At 2.x - ArcGIS.Desktop.Mapping.Snapping.SetSnapModes();
      ArcGIS.Desktop.Mapping.Snapping.SetSnapModes(null);

      // set application snapMode vertex on 
      ArcGIS.Desktop.Mapping.Snapping.SetSnapMode(SnapMode.Vertex, true);

      // ensure layer snapping is on
      await QueuedTask.Run(() =>
      {
        fLayer.SetSnappable(true);
      });

      // set vertex snapping only
      var vertexOnly = new LayerSnapModes(false) { Vertex = true };

      // set vertex only for the specific layer, clearing all others
      var dict = new Dictionary<Layer, LayerSnapModes>();
      dict.Add(fLayer, vertexOnly);
      ArcGIS.Desktop.Mapping.Snapping.SetLayerSnapModes(dict, true);  // true = reset other layers
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Snapping.GetOptions(ArcGIS.Desktop.Mapping.Map)
      // cref: ArcGIS.Desktop.Mapping.SnappingOptions
      // cref: ArcGIS.Desktop.Mapping.Snapping.SetOptions(ArcGIS.Desktop.Mapping.Map, ArcGIS.Desktop.Mapping.Snapping.SnappingOptions)
      // cref: ArcGIS.Desktop.Mapping.SnappingOptions.IsSnapToSketchEnabled
      // cref: ArcGIS.Desktop.Mapping.SnappingOptions.XYTolerance
      // cref: ArcGIS.Desktop.Mapping.SnappingOptions.IsZToleranceEnabled
      // cref: ArcGIS.Desktop.Mapping.SnappingOptions.ZTolerance
      // cref: ArcGIS.Desktop.Mapping.SnappingOptions.SnapTipDisplayParts
      // cref: ArcGIS.Desktop.Mapping.SnappingOptions.SnapTipColor
      // cref: ArcGIS.Core.CIM.SnapTipDisplayPart
      // cref: ArcGIS.Desktop.Mapping.SnappingOptions.SnapTipDisplayParts
      #region Snap Options

      //Set snapping options via get/set options
      var snapOptions = ArcGIS.Desktop.Mapping.Snapping.GetOptions(myMap);
      //At 2.x - snapOptions.SnapToSketchEnabled = true;
      snapOptions.IsSnapToSketchEnabled = true;
      snapOptions.XYTolerance = 100;
      //At 2.x - snapOptions.ZToleranceEnabled = true;
      snapOptions.IsZToleranceEnabled = true;
      snapOptions.ZTolerance = 0.6;

      //turn on snap tip display parts
      snapOptions.SnapTipDisplayParts = (int)SnapTipDisplayPart.SnapTipDisplayLayer + (int)SnapTipDisplayPart.SnapTipDisplayType;

      //turn off all snaptips
      //snapOptions.SnapTipDisplayParts = (int)SnapTipDisplayPart.SnapTipDisplayNone;

      //turn on layer display only
      //snapOptions.SnapTipDisplayParts = (int)SnapTipDisplayPart.SnapTipDisplayLayer;

      //At 2.x - snapOptions.GeometricFeedbackColor = ColorFactory.Instance.RedRGB;
      snapOptions.SnapTipColor = ColorFactory.Instance.RedRGB;

      ArcGIS.Desktop.Mapping.Snapping.SetOptions(myMap, snapOptions);

      #endregion
    }

  }

  public class CSVData
  {
    public Double X, Y, StopOrder, FacilityID;
  }
}
