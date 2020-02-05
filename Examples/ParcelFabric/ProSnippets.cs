/*

   Copyright 2019 Esri

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
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

//Internal namespaces
using ArcGIS.Desktop.Internal.Editing;
using ArcGIS.Desktop.Internal.Mapping;

namespace ParcelFabricSDKSamples
{
   class ProSnippets
   {
      //Enclose the snippet you want to share on the wiki with the region
      //Region name will show up as Snippet name.
      //Check out any of the ProSnippet md pages on the SDK wiki.
    protected async void GetActiveRecord()
    {
      #region Get the active record
        await QueuedTask.Run(async () =>
        {
          var layers = MapView.Active.Map.GetLayersAsFlattenedList();
          var myParcelFabricLayer = layers.FirstOrDefault(l => l is ParcelLayer) as ParcelLayer;
          var theActiveRecord = myParcelFabricLayer.GetActiveRecord();
          if (theActiveRecord == null)
          {
            System.Windows.MessageBox.Show("There is no Active Record. Please set the active record and try again.",
                      "Getting the Active Record");
            return;
          }
        });
      #endregion
    }

    protected async void SetActiveRecord()
    {
      #region Set the active record
          await QueuedTask.Run(async () =>
          {
            var layers = MapView.Active.Map.GetLayersAsFlattenedList();
            var myParcelFabricLayer = layers.FirstOrDefault(l => l is ParcelLayer) as ParcelLayer;
            var recordsLayer = layers.FirstOrDefault(l => l.Name == "Records" && l is FeatureLayer);
            string sExistingRecord = "MyRecordName";
            
            var pFeatClass = (recordsLayer as FeatureLayer).GetFeatureClass();
            QueryFilter queryFilter = new QueryFilter
            {
              WhereClause = "Name = '" + sExistingRecord + "'"
            };
            Guid guid = new Guid();
            long lOID = -1;

            using (RowCursor rowCursor = pFeatClass.Search(queryFilter, false))
            {
              while (rowCursor.MoveNext())
              {
                using (Row row = rowCursor.Current)
                {
                  guid = row.GetGlobalID();
                  long oid = row.GetObjectID();
                }
              }
            }

            var parcelRecord=new ParcelRecord(myParcelFabricLayer.Map, sExistingRecord, guid, lOID);
            await myParcelFabricLayer.SetActiveRecord(parcelRecord);
          });
      #endregion
    }

    protected async void CreateNewRecord()
    {
      #region Create a new record
          await QueuedTask.Run(async () =>
          {
            Dictionary<string, object> RecordAttributes = new Dictionary<string, object>();
            var layers = MapView.Active.Map.GetLayersAsFlattenedList();
            var recordsLayer = layers.FirstOrDefault(l => l.Name == "Records" && l is FeatureLayer);
            var myParcelFabricLayer = layers.FirstOrDefault(l => l is ParcelLayer) as ParcelLayer;
            var spatRef = recordsLayer.Map.SpatialReference;

            var editOper = new EditOperation()
            {
              Name = "Create Parcel Fabric Record",
              ProgressMessage = "Create Parcel Fabric Record...",
              ShowModalMessageAfterFailure = true,
              SelectNewFeatures = false,
              SelectModifiedFeatures = false
            };

            string sNewRecordName = "myNewRecord";
            Polygon newPolygon = null;
            newPolygon = PolygonBuilder.CreatePolygon(spatRef);
            if (newPolygon != null)
            {
              RecordAttributes.Add("Name", sNewRecordName);
              var editRowToken = editOper.CreateEx(recordsLayer, newPolygon, RecordAttributes);
              RecordAttributes.Clear();
              if (!await editOper.ExecuteAsync())
                return;

              //Default Guid
              var defGuid = new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd");
              var defOID = -1;
              var guid = editRowToken.GlobalID.HasValue ? editRowToken.GlobalID.Value : defGuid;
              var lOid = editRowToken.ObjectID.HasValue ? editRowToken.ObjectID.Value : defOID;

              if (guid == defGuid | lOid == defOID)
                return;

              ParcelRecord parcelRecord = new ParcelRecord(myParcelFabricLayer.Map, sNewRecordName, guid, lOid);
              await myParcelFabricLayer.SetActiveRecord(parcelRecord);
            }
          });
      #endregion
    }

    protected async void CopyLineFeaturesToParcelType()
    {
      #region Copy standard line features into a parcel type
      
      await QueuedTask.Run(async () =>
      {
        // check for selected layer
        if (MapView.Active.GetSelectedLayers().Count == 0)
        {
          System.Windows.MessageBox.Show("Please select a target parcel polygon layer in the table of contents", "Copy Line Features To");
          return;
        }

        //first get the feature layer that's selected in the table of contents
        var destPolygonL = MapView.Active.GetSelectedLayers().First() as FeatureLayer; //a parcel polygon feature layer
        var fcDefinition = destPolygonL.GetFeatureClass().GetDefinition();
        GeometryType geomType = fcDefinition.GetShapeType();
        if (geomType != GeometryType.Polygon)
        {
          System.Windows.MessageBox.Show("Please select a target parcel polygon layer in the table of contents", "Copy Line Features To");
          return;
        }

        var layers = MapView.Active.Map.GetLayersAsFlattenedList();
        var srcFeatLyr = layers.FirstOrDefault(l => l.Name == "Lines" && l is FeatureLayer);
        var myParcelFabricLayer = layers.FirstOrDefault(l => l is ParcelLayer) as ParcelLayer;

        string destLineLyrName1 = destPolygonL.Name + "_Lines";
        string destLineLyrName2 = destPolygonL.Name + " Lines";
        string destLineLyrName3 = destPolygonL.Name + "Lines";

        var destLineL = layers.FirstOrDefault(l => l.Name == destLineLyrName1 && l is FeatureLayer);
        if (destLineL == null)
          destLineL = layers.FirstOrDefault(l => l.Name == destLineLyrName2 && l is FeatureLayer);

        if (destLineL == null)
          destLineL = layers.FirstOrDefault(l => l.Name == destLineLyrName3 && l is FeatureLayer);

        if (myParcelFabricLayer == null || destLineL == null || destPolygonL == null)
          return;

        var theActiveRecord = myParcelFabricLayer.GetActiveRecord();

        if (theActiveRecord == null)
        {
          System.Windows.MessageBox.Show("There is no Active Record. Please set the active record and try again.", "Copy Line Features To");
          return;
        }

        var editOper = new EditOperation()
        {
          Name = "Copy Line Features To Parcel Type",
          ProgressMessage = "Copy Line Features To Parcel Type...",
          ShowModalMessageAfterFailure = true,
          SelectNewFeatures = true,
          SelectModifiedFeatures = false
        };
        //add the standard feature line layers source, and their feature ids to a new KeyValuePair
        MapMember mapMemberSource = srcFeatLyr as MapMember;
        var ids = new List<long>((srcFeatLyr as FeatureLayer).GetSelection().GetObjectIDs());

        if (ids.Count == 0)
        {
          System.Windows.MessageBox.Show("No selected lines were found. Please select line features and try again.", "Copy Line Features To");
          return;
        }
        editOper.CopyLineFeaturesToParcelType(srcFeatLyr, ids, destLineL, destPolygonL);
      });
      #endregion
    }

    protected async void CopyParcelLinesToParcelType()
    {
      await QueuedTask.Run(async () =>
      {
        // check for selected layer
        if (MapView.Active.GetSelectedLayers().Count == 0)
        {
          System.Windows.MessageBox.Show("Please select a source parcel polygon feature layer in the table of contents", "Copy Parcel Lines To");
          return;
        }

        //first get the feature layer that's selected in the table of contents
        var srcParcelFeatLyr = MapView.Active.GetSelectedLayers().First() as FeatureLayer;

        var layers = MapView.Active.Map.GetLayersAsFlattenedList();

        string sTargetParcelType = "Tax";

        if (sTargetParcelType.Trim().Length == 0)
          return;

        var myParcelFabricLayer = layers.FirstOrDefault(l => l is ParcelLayer) as ParcelLayer;

        string destLineLyrName1 = sTargetParcelType + "_Lines";
        string destLineLyrName2 = sTargetParcelType + " Lines";
        string destLineLyrName3 = sTargetParcelType + "Lines";

        var destLineL = layers.FirstOrDefault(l => l.Name == destLineLyrName1 && l is FeatureLayer);
        if (destLineL == null)
          destLineL = layers.FirstOrDefault(l => l.Name == destLineLyrName2 && l is FeatureLayer);

        if (destLineL == null)
          destLineL = layers.FirstOrDefault(l => l.Name == destLineLyrName3 && l is FeatureLayer);

        var destPolygonL = layers.FirstOrDefault(l => l.Name == sTargetParcelType && l is FeatureLayer);

        if (myParcelFabricLayer == null || destLineL == null || destPolygonL == null)
          return;

        #region Copy parcel lines to a parcel type
        var theActiveRecord = myParcelFabricLayer.GetActiveRecord();

        if (theActiveRecord == null)
        {
          System.Windows.MessageBox.Show("There is no Active Record. Please set the active record and try again.", "Copy Line Features To");
          return;
        }

        var editOper = new EditOperation()
        {
          Name = "Copy Lines To Parcel Type",
          ProgressMessage = "Copy Lines To Parcel Type ...",
          ShowModalMessageAfterFailure = true,
          SelectNewFeatures = true,
          SelectModifiedFeatures = false
        };

        MapMember mapMemberSource = srcParcelFeatLyr as MapMember;
        var ids = new List<long>(srcParcelFeatLyr.GetSelection().GetObjectIDs());

        if (ids.Count == 0)
        {
          System.Windows.MessageBox.Show("No selected parcels found. Please select parcels and try again.", "Copy Parcel Lines To");
          return;
        }
        //add the standard feature line layers source, and their feature ids to a new KeyValuePair
        var kvp = new KeyValuePair<MapMember, List<long>>(mapMemberSource, ids);
        var sourceParcelFeatures = new List<KeyValuePair<MapMember, List<long>>> { kvp };
        ParcelEditToken peToken = editOper.CopyParcelLinesToParcelType(myParcelFabricLayer, sourceParcelFeatures, destLineL, destPolygonL, null, true, false, true);
        var createdIDsSelectionSet = peToken.CreatedFeatures;
        await editOper.ExecuteAsync();
        #endregion
      });
    }

    protected async void AssignFeaturesToRecord()
    {
      await QueuedTask.Run(async () =>
      {
        //check for selected layer
        if (MapView.Active.GetSelectedLayers().Count == 0)
        {
          System.Windows.MessageBox.Show("Please select a source feature layer in the table of contents", "Assign Features To Record");
          return;
        }

        //first get the feature layer that's selected in the table of contents
        var srcFeatLyr = MapView.Active.GetSelectedLayers().First() as FeatureLayer;
        var layers = MapView.Active.Map.GetLayersAsFlattenedList();
        var myParcelFabricLayer = layers.FirstOrDefault(l => l is ParcelLayer) as ParcelLayer;
        if (myParcelFabricLayer == null)
          return;
        
        var theActiveRecord = myParcelFabricLayer.GetActiveRecord();
        if (theActiveRecord == null)
        {
          System.Windows.MessageBox.Show("There is no Active Record. Please set the active record and try again.", "Copy Line Features To");
          return;
        }
        #region Assign features to active record
        var guid = theActiveRecord.Guid;
        var editOper= new EditOperation()
        {
          Name = "Assign Features to Record",
          ProgressMessage = "Assign Features to Record...",
          ShowModalMessageAfterFailure = true,
          SelectNewFeatures = true,
          SelectModifiedFeatures = false
        };
        //add parcel type layers and their feature ids to a new KeyValuePair
        MapMember mapMemberSource = srcFeatLyr as MapMember;
        var ids = new List<long>(srcFeatLyr.GetSelection().GetObjectIDs());
        var kvp = new KeyValuePair<MapMember, List<long>>(mapMemberSource, ids);
        var sourceFeatures = new List<KeyValuePair<MapMember, List<long>>> { kvp };

        editOper.AssignFeaturesToRecord(myParcelFabricLayer, sourceFeatures, guid, ParcelRecordAttribute.CreatedByRecord); 
        await editOper.ExecuteAsync();
        #endregion
      });

    }

    protected async void CreateParcelSeeds()
    {
      await QueuedTask.Run(async () =>
      {
        // check for selected layer
        if (MapView.Active.GetSelectedLayers().Count == 0)
        {
          System.Windows.MessageBox.Show("Please select a target parcel polygon layer in the table of contents", "Create Parcel Seeds");
          return;
        }

        //first get the feature layer that's selected in the table of contents
        var destPolygonL = MapView.Active.GetSelectedLayers().First() as FeatureLayer;
        var fcDefinition = destPolygonL.GetFeatureClass().GetDefinition();
        GeometryType geomType = fcDefinition.GetShapeType();
        if (geomType != GeometryType.Polygon)
        {
          System.Windows.MessageBox.Show("Please select a target parcel polygon layer in the table of contents", "Create Parcel Seeds");
          return;
        }

        if (destPolygonL is FeatureLayer)
        {
          //is it a fabric parcel type template?
          string sTemplateID = destPolygonL.GetDefinition().LayerTemplate.LayerTemplateId;
          if (sTemplateID.ToLower() != "esriparcels")
          {
            System.Windows.MessageBox.Show("Please select a target parcel polygon layer in the table of contents", "Create Parcel Seeds");
            return;
          }  
        }
        
        var layers = MapView.Active.Map.GetLayersAsFlattenedList();
        var myParcelFabricLayer = layers.FirstOrDefault(l => l is ParcelLayer) as ParcelLayer;

        //hard-coded tests to find the line feature portion of the parcel type.
        string destLineLyrName1 = destPolygonL.Name + "_Lines";
        string destLineLyrName2 = destPolygonL.Name + " Lines";
        string destLineLyrName3 = destPolygonL.Name + "Lines";

        var destLineL = layers.FirstOrDefault(l => l.Name == destLineLyrName1 && l is FeatureLayer);
        if (destLineL == null)
          destLineL = layers.FirstOrDefault(l => l.Name == destLineLyrName2 && l is FeatureLayer);

        if (destLineL == null)
          destLineL = layers.FirstOrDefault(l => l.Name == destLineLyrName3 && l is FeatureLayer);

        if (myParcelFabricLayer == null || destLineL == null || destPolygonL == null)
          return;

        var theActiveRecord = myParcelFabricLayer.GetActiveRecord();

        if (theActiveRecord == null)
        {
          System.Windows.MessageBox.Show("There is no Active Record. Please set the active record and try again.", "Copy Line Features To");
          return;
        }

        #region Create parcel seeds
        var guid = theActiveRecord.Guid;
        var editOper = new EditOperation()
        {
          Name = "Create Parcel Seeds",
          ProgressMessage = "Create Parcel Seeds...",
          ShowModalMessageAfterFailure = true,
          SelectNewFeatures = true,
          SelectModifiedFeatures = false
        };

        editOper.CreateParcelSeeds(myParcelFabricLayer, MapView.Active.Extent, guid);
        await editOper.ExecuteAsync();
        #endregion
      });
    }

    protected async void BuildParcels()
    {
      await QueuedTask.Run(async () =>
      {
        var layers = MapView.Active.Map.GetLayersAsFlattenedList();

        var myParcelFabricLayer = layers.FirstOrDefault(l => l is ParcelLayer) as ParcelLayer;

        var theActiveRecord = myParcelFabricLayer.GetActiveRecord();
        await myParcelFabricLayer.SetActiveRecord(theActiveRecord);
        var guid = theActiveRecord.Guid;

        if (myParcelFabricLayer == null)
          return;
        #region Build parcels
        var editOper = new EditOperation()
        {
          Name = "Build Parcels",
          ProgressMessage = "Build Parcels...",
          ShowModalMessageAfterFailure = true,
          SelectNewFeatures = true,
          SelectModifiedFeatures = true
        };
        editOper.BuildParcelsByRecord(myParcelFabricLayer,guid);
        await editOper.ExecuteAsync();
        #endregion
      });

    }

    protected async void DuplicateParcels()
    {
      await QueuedTask.Run(async () =>
      {
        // check for selected layer
        if (MapView.Active.GetSelectedLayers().Count == 0)
        {
          System.Windows.MessageBox.Show("Please select the source layer in the table of contents", "Duplicate Parcels");
          return;
        }

        //first get the feature layer that's selected in the table of contents
        var sourcePolygonL = MapView.Active.GetSelectedLayers().First() as FeatureLayer;

        var layers = MapView.Active.Map.GetLayersAsFlattenedList();
        var myParcelFabricLayer = layers.FirstOrDefault(l => l is ParcelLayer) as ParcelLayer;

        string sTargetParcelType = "Tax"; //Microsoft.VisualBasic.Interaction.InputBox("Target Parcel Type:", "Copy Parcel Lines To", "Tax");

        if (sTargetParcelType.Trim().Length == 0)
          return;

        if (myParcelFabricLayer == null || sourcePolygonL == null)
          return;
        
        #region Duplicate parcels
        //get the polygon layer from the parcel fabric layer type, in this case a layer called Tax
        var targetFeatLyr = layers.FirstOrDefault(l => l.Name == "Tax" && l is FeatureLayer) as FeatureLayer;
        MapMember mapMemberSource = sourcePolygonL as MapMember; //a parcel polygon feature layer
        var ids = new List<long>(sourcePolygonL.GetSelection().GetObjectIDs());

        if (ids.Count == 0)
        {
          System.Windows.MessageBox.Show("No selected parcels found. Please select parcels and try again.", "Copy Parcel Lines To");
          return;
        }

        //add polygon layers and the feature ids to be duplicated to a new KeyValuePair
        var kvp = new KeyValuePair<MapMember, List<long>>(mapMemberSource, ids);
        var sourceFeatures = new List<KeyValuePair<MapMember, List<long>>> { kvp };
        var theActiveRecord = myParcelFabricLayer.GetActiveRecord();

        if (theActiveRecord == null)
        {
          System.Windows.MessageBox.Show("There is no Active Record. Please set the active record and try again.", "Copy Line Features To");
          return;
        }

        Guid guid=theActiveRecord.Guid;
        var editOper  = new EditOperation()
        {
          Name = "Duplicate Parcels",
          ProgressMessage = "Duplicate Parcels...",
          ShowModalMessageAfterFailure = true,
          SelectNewFeatures = true,
          SelectModifiedFeatures = false
        };
        editOper.DuplicateParcels(myParcelFabricLayer, sourceFeatures, guid, targetFeatLyr, -1);
        await editOper.ExecuteAsync();
        #endregion
      });

    }

    protected async void SetParcelsHistoric()
    {
      await QueuedTask.Run(async () =>
      {
        //Get selection from a parcel fabric type's polygon layer
        var layers = MapView.Active.Map.GetLayersAsFlattenedList();
        int countLayersWithSelection = 0;
        var destPolygonL = (layers.FirstOrDefault() as FeatureLayer);
        foreach (var flyr in layers)
        {
          if (flyr is FeatureLayer)
          {
            var fcDefinition = (flyr as FeatureLayer).GetFeatureClass().GetDefinition();
            GeometryType geomType = fcDefinition.GetShapeType();
            if (geomType == GeometryType.Polygon)
            {
              if ((flyr as FeatureLayer).SelectionCount > 0)
              {
                destPolygonL = (flyr as FeatureLayer);
                //test to check if the layer has a fabric parcel type layer template.
                string sTemplateID =flyr.GetDefinition().LayerTemplate.LayerTemplateId;
                if(sTemplateID.ToLower()=="esriparcels")
                  countLayersWithSelection++;
              }
            }
          }
        }
        if (countLayersWithSelection != 1)
        {
          System.Windows.MessageBox.Show("Please select features from only one source polygon parcel layer", "Set Historic");
          return;
        }
        var myParcelFabricLayer = layers.FirstOrDefault(l => l is ParcelLayer) as ParcelLayer;
        if (myParcelFabricLayer == null || destPolygonL == null)
          return;

        var theActiveRecord = myParcelFabricLayer.GetActiveRecord();

        if (theActiveRecord == null)
        {
          System.Windows.MessageBox.Show("There is no Active Record. Please set the active record and try again.", "Copy Line Features To");
          return;
        }

        #region Set parcels historic
        MapMember mapMemberSource = destPolygonL as MapMember; //a parcel polygon feature layer
        var ids = new List<long>(destPolygonL.GetSelection().GetObjectIDs()); 
        //add polygon layers and the feature ids to be made historic to a new KeyValuePair
        var kvp = new KeyValuePair<MapMember, List<long>>(mapMemberSource, ids);
        var sourceFeatures = new List<KeyValuePair<MapMember, List<long>>> { kvp };

        Guid guid = theActiveRecord.Guid;
        var editOper = new EditOperation()
        {
          Name = "Set Parcels Historic",
          ProgressMessage = "Set Parcels Historic...",
          ShowModalMessageAfterFailure = true,
          SelectNewFeatures = true,
          SelectModifiedFeatures = false
        };

        editOper.UpdateParcelHistory(myParcelFabricLayer, sourceFeatures, guid,true);
        await editOper.ExecuteAsync();
        #endregion
      });
    }

    protected async void ChangeParcelType()
    {
      await QueuedTask.Run(async () =>
      {
        //check for selected layer
        if (MapView.Active.GetSelectedLayers().Count == 0)
        {
          System.Windows.MessageBox.Show("Please select a source layer in the table of contents", "Change Parcel Type");
          return;
        }

        //first get the feature layer that's selected in the table of contents
        var sourcePolygonL = MapView.Active.GetSelectedLayers().First() as FeatureLayer;
        
        var layers = MapView.Active.Map.GetLayersAsFlattenedList();
        var pfL = layers.FirstOrDefault(l => l is ParcelLayer) as ParcelLayer;

        string sTargetParcelType = "Tax"; //Microsoft.VisualBasic.Interaction.InputBox("Target Parcel Type:", "Copy Parcel Lines To", "Tax");

        if (sTargetParcelType.Trim().Length == 0)
          return;

        #region Change parcel type
        var targetFeatLyr = layers.FirstOrDefault(l => l.Name == "Tax" && l is FeatureLayer) as FeatureLayer; //the target parcel polygon feature layer

        if (pfL == null || sourcePolygonL == null)
          return;

        //add polygon layers and the feature ids to change the type on to a new KeyValuePair
        MapMember mapMemberSource = sourcePolygonL as MapMember;
        var ids = new List<long>(sourcePolygonL.GetSelection().GetObjectIDs());
        var kvp = new KeyValuePair<MapMember, List<long>>(mapMemberSource, ids);
        var sourceFeatures = new List<KeyValuePair<MapMember, List<long>>> { kvp };

        var editOper = new EditOperation()
        {
          Name = "Change Parcel Type",
          ProgressMessage = "Change Parcel Type...",
          ShowModalMessageAfterFailure = true,
          SelectNewFeatures = true,
          SelectModifiedFeatures = false
        };

        editOper.ChangeParcelType(pfL, sourceFeatures, targetFeatLyr, -1);
        await editOper.ExecuteAsync();
        #endregion
      });

    }

   }



}  
