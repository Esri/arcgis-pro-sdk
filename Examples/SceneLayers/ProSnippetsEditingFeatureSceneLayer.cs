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
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Editing.Attributes;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Core.CIM;
using ArcGIS.Core.Geometry;
using ArcGIS.Core.Data;

namespace ProSnippetsEditingFeatureSceneLayer
{

  #region ProSnippet Group: FeatureSceneLayer Editing
  #endregion

  class ProSnippetsEditingFeatureSceneLayer
  {
    public async void Examples()
    {
      //#region Name of FeatureSceneLayer 
      //var featureSceneLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureSceneLayer>().FirstOrDefault();
      //var scenelayerName = featureSceneLayer?.Name;

      //#endregion
    }

    public async void Examples2()
    {
      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.HasAssociatedFeatureService
      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.IsEditable
      #region Determine if a FeatureSceneLayer supports editing

      var featSceneLayer = MapView.Active.Map.GetLayersAsFlattenedList()
                         .OfType<FeatureSceneLayer>().FirstOrDefault();
      if (!featSceneLayer.HasAssociatedFeatureService || 
          !featSceneLayer.IsEditable)
        return;//not supported

      //TODO continue editing here...

      #endregion

      // create a new point at specified coordinates
      MapPoint mapPoint = MapPointBuilderEx.CreateMapPoint(122.39, 37.78);

      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.HasAssociatedFeatureService
      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.IsEditable
      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.ShapeType
      #region Create a new Point feature in FeatureSceneLayer

      //must support editing!
      //var featSceneLayer = ... ;
      if (!featSceneLayer.HasAssociatedFeatureService || 
          !featSceneLayer.IsEditable)
        return;
      //Check geometry type...must be point in this example
      if (featSceneLayer.ShapeType != esriGeometryType.esriGeometryPoint)
        return;

      var editOp = new EditOperation()
      {
        Name = "Create new 3d point feature",
        SelectNewFeatures = true
      };

      var attributes = new Dictionary<string, object>();
      //mapPoint contains the new 3d point location
      attributes.Add("SHAPE", mapPoint);
      attributes.Add("TreeID", "1");
      editOp.Create(featSceneLayer, attributes);
      editOp.ExecuteAsync();//fyi, no await

      #endregion

      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.HasAssociatedFeatureService
      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.IsEditable
      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.GetSelection
      // cref: ArcGIS.Desktop.Editing.EditOperation.Delete(ArcGIS.Desktop.Mapping.MapMember,System.Collections.Generic.IEnumerable{System.Int64)
      #region Delete all the selected features in FeatureSceneLayer

      //must support editing!
      //var featSceneLayer = .... ;
      if (!featSceneLayer.HasAssociatedFeatureService || 
          !featSceneLayer.IsEditable)
        return;

      var delOp = new EditOperation()
      {
        Name = "Delete selected features"
      };
      //Assuming we have a selection on the layer...
      delOp.Delete(featSceneLayer, featSceneLayer.GetSelection().GetObjectIDs());
      await delOp.ExecuteAsync();//await if needed but not required

      #endregion
    }

    public async void Examples3()
    {
      var oid = 1;

      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.HasAssociatedFeatureService
      // cref: ArcGIS.Desktop.Mapping.FeatureSceneLayer.IsEditable
      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.Load
      // cref: ArcGIS.Desktop.Editing.EditOperation.Modify
      #region Edit the attributes of a FeatureSceneLayer
      //must support editing!
      var featSceneLayer = MapView.Active.Map.GetLayersAsFlattenedList()
                         .OfType<FeatureSceneLayer>().FirstOrDefault();
      if (!featSceneLayer.HasAssociatedFeatureService || 
          !featSceneLayer.IsEditable)
        return;

      var ok = await QueuedTask.Run(() =>
      {
        var editOp = new EditOperation()
        {
          Name = "Edit FeatureSceneLayer Attributes",
          SelectModifiedFeatures = true
        };
        //make an inspector
        var inspector = new Inspector();
        //get the attributes for the specified oid
        inspector.Load(featSceneLayer, oid);
        inspector["PermitNotes"] = "test";//modify
        editOp.Modify(inspector);
        return editOp.Execute();//synchronous flavor
      });

      #endregion
    }
  }
}
