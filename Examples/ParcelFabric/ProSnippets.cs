/*

   Copyright 2020 Esri

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
      string errorMessage = await QueuedTask.Run(() =>
      {
        try
        {
          var myParcelFabricLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<ParcelLayer>().FirstOrDefault();
          //if there is no fabric in the map then bail
          if (myParcelFabricLayer == null)
            return "There is no fabric in the map.";
          var theActiveRecord = myParcelFabricLayer.GetActiveRecord();
          if (theActiveRecord == null)
            return "There is no Active Record. Please set the active record and try again.";
        }
        catch (Exception ex)
        {
          return ex.Message;
        }
        return "";
      });
      if (!string.IsNullOrEmpty(errorMessage))
        MessageBox.Show(errorMessage, "Get Active Record.");
      #endregion
    }
    protected async void SetActiveRecord()
    {
      #region Set the active record
      string errorMessage = await QueuedTask.Run(async () =>
      {
        try
        {
          string sExistingRecord = "MyRecordName";
          var myParcelFabricLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<ParcelLayer>().FirstOrDefault();
          //if there is no fabric in the map then bail
          if (myParcelFabricLayer == null)
            return "There is no fabric in the map.";

          bool bSuccess = await myParcelFabricLayer.SetActiveRecordAsync(sExistingRecord);
          if (!bSuccess)
            return "No record called " + sExistingRecord + " was found.";
        }
        catch (Exception ex)
        {
          return ex.Message;
        }
        return "";
      });
      if (!string.IsNullOrEmpty(errorMessage))
        MessageBox.Show(errorMessage, "Set Active Record.");
      #endregion
    }
    protected async void CreateNewRecord()
    {
      #region Create a new record
      string errorMessage = await QueuedTask.Run(async () =>
      {
        Dictionary<string, object> RecordAttributes = new Dictionary<string, object>();
        string sNewRecord = "MyRecordName";
        try
        {
          var myParcelFabricLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<ParcelLayer>().FirstOrDefault();
          //if there is no fabric in the map then bail
          if (myParcelFabricLayer == null)
            return "There is no fabric in the map.";
          var recordsLayer = await myParcelFabricLayer.GetRecordsLayerAsync();
          var editOper = new EditOperation()
          {
            Name = "Create Parcel Fabric Record",
            ProgressMessage = "Create Parcel Fabric Record...",
            ShowModalMessageAfterFailure = true,
            SelectNewFeatures = false,
            SelectModifiedFeatures = false
          };
          RecordAttributes.Add("Name", sNewRecord);
          var editRowToken = editOper.CreateEx(recordsLayer.FirstOrDefault(), RecordAttributes);
          if (!editOper.Execute())
            return editOper.ErrorMessage;

          var defOID = -1;
          var lOid = editRowToken.ObjectID.HasValue ? editRowToken.ObjectID.Value : defOID;
          await myParcelFabricLayer.SetActiveRecordAsync(lOid);
        }
        catch (Exception ex)
        {
          return ex.Message;
        }
        return "";
      });
      if (!string.IsNullOrEmpty(errorMessage))
        MessageBox.Show(errorMessage, "Create New Record.");
      #endregion
    }
    protected async void CopyLineFeaturesToParcelType()
    {
      #region Copy standard line features into a parcel type
      string errorMessage = await QueuedTask.Run( async () =>
      {
        // check for selected layer
        if (MapView.Active.GetSelectedLayers().Count == 0)
          return "Please select a target parcel polygon layer in the table of contents.";
        //get the feature layer that's selected in the table of contents
        var destPolygonL = MapView.Active.GetSelectedLayers().OfType<FeatureLayer>().FirstOrDefault();
        try
        {
          var myParcelFabricLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<ParcelLayer>().FirstOrDefault();
          //if there is no fabric in the map then bail
          if (myParcelFabricLayer == null)
            return "There is no fabric in the map.";
          var pRec = myParcelFabricLayer.GetActiveRecord();
          if (pRec == null)
            return "There is no Active Record. Please set the active record and try again.";
          string ParcelTypeName = "";
          IEnumerable<string> parcelTypeNames = await myParcelFabricLayer.GetParcelTypeNames();
          foreach (string parcelTypeNm in parcelTypeNames)
          {
            var polygonLyrParcelTypeEnum = await myParcelFabricLayer.GetParcelPolygonLayerByTypeName(parcelTypeNm);
            foreach (FeatureLayer lyr in polygonLyrParcelTypeEnum)
              if (lyr == destPolygonL)
              {
                ParcelTypeName = parcelTypeNm;
                break;
              }
          }
          if (String.IsNullOrEmpty(ParcelTypeName))
            return "Please select a target parcel polygon layer in the table of contents.";
          var srcFeatLyr = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault(l => l.Name.Contains("MySourceLines") && l.IsVisible);
          if (srcFeatLyr == null)
            return "Looking for a layer named 'MySourceLines' in the table of contents.";
          //now get the line layer for this parcel type
          var destLineLyrEnum = await myParcelFabricLayer.GetParcelLineLayerByTypeName(ParcelTypeName);
          if (destLineLyrEnum.Count() == 0) //make sure there is one in the map
            return ParcelTypeName + " not found.";
          var destLineL = destLineLyrEnum.FirstOrDefault();
          if (destLineL == null || destPolygonL == null)
            return "";
          var editOper = new EditOperation()
          {
            Name = "Copy Line Features To Parcel Type",
            ProgressMessage = "Copy Line Features To Parcel Type...",
            ShowModalMessageAfterFailure = true,
            SelectNewFeatures = true,
            SelectModifiedFeatures = false
          };
          var ids = new List<long>((srcFeatLyr as FeatureLayer).GetSelection().GetObjectIDs());
          if (ids.Count == 0)
            return "No selected lines were found. Please select line features and try again.";
          editOper.CopyLineFeaturesToParcelType(srcFeatLyr, ids, destLineL, destPolygonL);
          if (!editOper.Execute())
            return editOper.ErrorMessage;
        }
        catch (Exception ex)
        {
          return ex.Message;
        }
        return "";
      });
      if (!string.IsNullOrEmpty(errorMessage))
        MessageBox.Show(errorMessage, "Copy Line Features To Parcel Type.");
      #endregion
    }
    protected async void CopyParcelLinesToParcelType()
    {
      #region Copy parcel lines to a parcel type
      string errorMessage = await QueuedTask.Run( async () =>
      {
        // check for selected layer
        if (MapView.Active.GetSelectedLayers().Count == 0)
          return "Please select a source parcel polygon feature layer in the table of contents.";
        try
        {
          var myParcelFabricLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<ParcelLayer>().FirstOrDefault();
          if (myParcelFabricLayer == null)
            return "No parcel layer found in the map.";

          //get the feature layer that's selected in the table of contents
          var srcParcelFeatLyr = MapView.Active.GetSelectedLayers().OfType<FeatureLayer>().FirstOrDefault();
          string sTargetParcelType = "Tax";
          var destLineLEnum = await myParcelFabricLayer.GetParcelLineLayerByTypeName(sTargetParcelType);
          if (destLineLEnum.Count() == 0)
            return "";
          var destLineL = destLineLEnum.FirstOrDefault();
          var destPolygonLEnum = await myParcelFabricLayer.GetParcelPolygonLayerByTypeName(sTargetParcelType);
          if (destPolygonLEnum.Count() == 0)
            return "";
          var destPolygonL = destPolygonLEnum.FirstOrDefault();
          if (destLineL == null || destPolygonL == null)
            return "";
          var theActiveRecord = myParcelFabricLayer.GetActiveRecord();
          if (theActiveRecord == null)
            return "There is no Active Record. Please set the active record and try again.";
          var editOper = new EditOperation()
          {
            Name = "Copy Lines To Parcel Type",
            ProgressMessage = "Copy Lines To Parcel Type ...",
            ShowModalMessageAfterFailure = true,
            SelectNewFeatures = true,
            SelectModifiedFeatures = false
          };
          var ids = new List<long>(srcParcelFeatLyr.GetSelection().GetObjectIDs());
          if (ids.Count == 0)
            return "No selected parcels found. Please select parcels and try again.";
          //add the standard feature line layers source, and their feature ids to a new KeyValuePair
          var kvp = new KeyValuePair<MapMember, List<long>>(srcParcelFeatLyr, ids);
          var sourceParcelFeatures = new List<KeyValuePair<MapMember, List<long>>> { kvp };
          editOper.CopyParcelLinesToParcelType(myParcelFabricLayer, sourceParcelFeatures, destLineL, destPolygonL, true, false, true);
          if (!editOper.Execute())
            return editOper.ErrorMessage;
        }
        catch (Exception ex)
        {
          return ex.Message;
        }
        return "";
      });
      if (!string.IsNullOrEmpty(errorMessage))
        MessageBox.Show(errorMessage, "Copy Parcel Lines To Parcel Type.");
      #endregion
    }
    protected async void AssignFeaturesToRecord()
    {
      #region Assign features to active record
      string errorMessage = await QueuedTask.Run( () =>
      {
        //check for selected layer
        if (MapView.Active.GetSelectedLayers().Count == 0)
          return "Please select a source feature layer in the table of contents.";
        //first get the feature layer that's selected in the table of contents
        var srcFeatLyr = MapView.Active.GetSelectedLayers().OfType<FeatureLayer>().FirstOrDefault();
        var myParcelFabricLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<ParcelLayer>().FirstOrDefault();
        if (myParcelFabricLayer == null)
          return "No parcel layer found in the map.";
        try
        {
          var theActiveRecord = myParcelFabricLayer.GetActiveRecord();
          if (theActiveRecord == null)
            return "There is no Active Record. Please set the active record and try again.";
          var editOper = new EditOperation()
          {
            Name = "Assign Features to Record",
            ProgressMessage = "Assign Features to Record...",
            ShowModalMessageAfterFailure = true,
            SelectNewFeatures = true,
            SelectModifiedFeatures = false
          };
          //add parcel type layers and their feature ids to a new KeyValuePair
          var ids = new List<long>(srcFeatLyr.GetSelection().GetObjectIDs());
          var kvp = new KeyValuePair<MapMember, List<long>>(srcFeatLyr, ids);
          var sourceFeatures = new List<KeyValuePair<MapMember, List<long>>> { kvp };

          editOper.AssignFeaturesToRecord(myParcelFabricLayer, sourceFeatures, theActiveRecord);
          if (!editOper.Execute())
            return editOper.ErrorMessage;
        }
        catch (Exception ex)
        {
          return ex.Message;
        }
        return "";
      });
      if (!string.IsNullOrEmpty(errorMessage))
        MessageBox.Show(errorMessage, "Assign Features To Record.");
      #endregion
    }
    protected async void CreateParcelSeeds()
    {
      string errorMessage = await QueuedTask.Run( async () =>
      {
        // check for selected layer
        if (MapView.Active.GetSelectedLayers().Count == 0)
          return "Please select a target parcel polygon layer in the table of contents";
        //get the feature layer that's selected in the table of contents
        var destPolygonL = MapView.Active.GetSelectedLayers().FirstOrDefault() as FeatureLayer;
        var myParcelFabricLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<ParcelLayer>().FirstOrDefault();
        if (myParcelFabricLayer == null)
          return "Parcel layer not found in map.";

        //is it a fabric parcel type layer?
        string ParcelTypeName = "";
        IEnumerable<string> parcelTypeNames = await myParcelFabricLayer.GetParcelTypeNames();
        foreach (string parcelTypeNm in parcelTypeNames)
        {
          var polygonLyrParcelTypeEnum = await myParcelFabricLayer.GetParcelPolygonLayerByTypeName(parcelTypeNm);
          foreach (FeatureLayer lyr in polygonLyrParcelTypeEnum)
            if (lyr == destPolygonL)
            {
              ParcelTypeName = parcelTypeNm;
              break;
            }
        }
        if (String.IsNullOrEmpty(ParcelTypeName))
          return "Please select a target parcel polygon layer in the table of contents.";
        var destLineL = await myParcelFabricLayer.GetParcelLineLayerByTypeName(ParcelTypeName);
        if (destLineL == null)
          return "";
        #region Create parcel seeds
        try
        {
          var theActiveRecord = myParcelFabricLayer.GetActiveRecord();
          if (theActiveRecord == null)
            return "There is no Active Record. Please set the active record and try again.";

          var guid = theActiveRecord.Guid;
          var editOper = new EditOperation()
          {
            Name = "Create Parcel Seeds",
            ProgressMessage = "Create Parcel Seeds...",
            ShowModalMessageAfterFailure = true,
            SelectNewFeatures = true,
            SelectModifiedFeatures = false
          };
          editOper.CreateParcelSeedsByRecord(myParcelFabricLayer, guid, MapView.Active.Extent);
          if (!editOper.Execute())
            return editOper.ErrorMessage;
        }
        catch (Exception ex)
        {
          return ex.Message;
        }
        return "";
        #endregion
      });
      if (!string.IsNullOrEmpty(errorMessage))
        MessageBox.Show(errorMessage, "Create Parcel Seeds.");
    }
    protected async void BuildParcels()
    {
      string errorMessage = await QueuedTask.Run( () =>
      {
        #region Build parcels
        try
        {
          var myParcelFabricLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<ParcelLayer>().FirstOrDefault();
          if (myParcelFabricLayer == null)
            return "Parcel layer not found in map.";

          var theActiveRecord = myParcelFabricLayer.GetActiveRecord();
          var guid = theActiveRecord.Guid;
          var editOper = new EditOperation()
          {
            Name = "Build Parcels",
            ProgressMessage = "Build Parcels...",
            ShowModalMessageAfterFailure = true,
            SelectNewFeatures = true,
            SelectModifiedFeatures = true
          };
          editOper.BuildParcelsByRecord(myParcelFabricLayer, guid);
          if (!editOper.Execute())
            return editOper.ErrorMessage;
        }
        catch (Exception ex)
        {
          return ex.Message;
        }
        return "";
        #endregion
      });
      if (!string.IsNullOrEmpty(errorMessage))
        MessageBox.Show(errorMessage, "Build Parcels.");
    }
    protected async void DuplicateParcels()
    {
      string errorMessage = await QueuedTask.Run( async () =>
      {
        // check for selected layer
        if (MapView.Active.GetSelectedLayers().Count == 0)
          return "Please select the source layer in the table of contents.";
        #region Duplicate parcels
        var myParcelFabricLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<ParcelLayer>().FirstOrDefault();
        if (myParcelFabricLayer == null)
          return "Parecl layer not found in the map.";
        //get the source polygon layer from the parcel fabric layer type, in this case a layer called Lot
        var srcFeatLyrEnum = await myParcelFabricLayer.GetParcelPolygonLayerByTypeName("Lot");
        if (srcFeatLyrEnum.Count() == 0)
          return "";
        var sourcePolygonL = srcFeatLyrEnum.FirstOrDefault();
        //get the target polygon layer from the parcel fabric layer type, in this case a layer called Tax
        var targetFeatLyrEnum = await myParcelFabricLayer.GetParcelPolygonLayerByTypeName("Tax");
        if (targetFeatLyrEnum.Count() == 0)
          return "";
        var targetFeatLyr = targetFeatLyrEnum.FirstOrDefault();
        var ids = new List<long>(sourcePolygonL.GetSelection().GetObjectIDs());
        if (ids.Count == 0)
          return "No selected parcels found. Please select parcels and try again.";
        //add polygon layers and the feature ids to be duplicated to a new KeyValuePair
        var kvp = new KeyValuePair<MapMember, List<long>>(sourcePolygonL, ids);
        var sourceFeatures = new List<KeyValuePair<MapMember, List<long>>> { kvp };
        try
        {
          var theActiveRecord = myParcelFabricLayer.GetActiveRecord();
          if (theActiveRecord == null)
            return "There is no Active Record. Please set the active record and try again.";
          var editOper = new EditOperation()
          {
            Name = "Duplicate Parcels",
            ProgressMessage = "Duplicate Parcels...",
            ShowModalMessageAfterFailure = true,
            SelectNewFeatures = true,
            SelectModifiedFeatures = false
          };
          editOper.DuplicateParcels(myParcelFabricLayer, sourceFeatures, theActiveRecord, targetFeatLyr);
          if (!editOper.Execute())
            return editOper.ErrorMessage;
        }
        catch (Exception ex)
        {
          return ex.Message;
        }
        #endregion
        return "";
      });
      if (!string.IsNullOrEmpty(errorMessage))
        MessageBox.Show(errorMessage, "Duplicate Parcels.");
    }
    protected async void SetParcelsHistoric()
    {
      #region Set parcels historic
      string errorMessage = await QueuedTask.Run(async () =>
      {
        var myParcelFabricLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<ParcelLayer>().FirstOrDefault();
        if (myParcelFabricLayer == null)
          return "Please add a parcel fabric to the map";
        try
        {
          FeatureLayer destPolygonL = null;
          //find the first layer that is a parcel type, is non-historic, and has a selection
          bool bFound = false;
          var ParcelTypesEnum = await myParcelFabricLayer.GetParcelTypeNames();
          foreach (FeatureLayer mapFeatLyr in MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>())
          {
            foreach (string ParcelType in ParcelTypesEnum)
            {
              var layerEnum = await myParcelFabricLayer.GetParcelPolygonLayerByTypeName(ParcelType);
              foreach (FeatureLayer flyr in layerEnum)
              {
                if (flyr == mapFeatLyr)
                {
                  bFound = mapFeatLyr.SelectionCount > 0;
                  destPolygonL = mapFeatLyr;
                  break;
                }
              }
              if (bFound) break;
            }
            if (bFound) break;
          }
          if (!bFound)
            return "Please select parcels to set as historic.";

          var theActiveRecord = myParcelFabricLayer.GetActiveRecord();
          if (theActiveRecord == null)
            return "There is no Active Record. Please set the active record and try again.";

          var ids = new List<long>(destPolygonL.GetSelection().GetObjectIDs());
          //can do multi layer selection but using single per code above
          var kvp = new KeyValuePair<MapMember, List<long>>(destPolygonL, ids);
          var sourceFeatures = new List<KeyValuePair<MapMember, List<long>>> { kvp };
          var editOper = new EditOperation()
          {
            Name = "Set Parcels Historic",
            ProgressMessage = "Set Parcels Historic...",
            ShowModalMessageAfterFailure = true,
            SelectNewFeatures = true,
            SelectModifiedFeatures = false
          };
          editOper.SetParcelHistoryRetired(myParcelFabricLayer, sourceFeatures, theActiveRecord);
          if (!editOper.Execute())
            return editOper.ErrorMessage;
        }
        catch (Exception ex)
        {
          return ex.Message;
        }
        return "";
      });
      if (!string.IsNullOrEmpty(errorMessage))
        MessageBox.Show(errorMessage, "Set Parcels Historic.");
      #endregion
    }
    protected async void ShrinkParcelsToSeeds()
    {
      #region Shrink parcels to seeds
      string errorMessage = await QueuedTask.Run(async () =>
      {
        var myParcelFabricLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<ParcelLayer>().FirstOrDefault();
        if (myParcelFabricLayer == null)
          return "Please add a parcel fabric to the map";
        try
        {
          FeatureLayer parcelPolygonLyr = null;
          //find the first layer that is a polygon parcel type, is non-historic, and has a selection
          bool bFound = false;
          var ParcelTypesEnum = await myParcelFabricLayer.GetParcelTypeNames();
          foreach (FeatureLayer mapFeatLyr in MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>())
          {
            foreach (string ParcelType in ParcelTypesEnum)
            {
              var layerEnum = await myParcelFabricLayer.GetParcelPolygonLayerByTypeName(ParcelType);
              foreach (FeatureLayer flyr in layerEnum)
              {
                if (flyr == mapFeatLyr)
                {
                  bFound = mapFeatLyr.SelectionCount > 0;
                  parcelPolygonLyr = mapFeatLyr;
                  break;
                }
              }
              if (bFound) break;
            }
            if (bFound) break;
          }
          if (!bFound)
            return "Please select parcels to shrink to seeds.";
          var editOper = new EditOperation()
          {
            Name = "Shrink Parcels To Seeds",
            ProgressMessage = "Shrink Parcels To Seeds...",
            ShowModalMessageAfterFailure = true,
            SelectNewFeatures = true,
            SelectModifiedFeatures = false
          };
          var ids = new List<long>(parcelPolygonLyr.GetSelection().GetObjectIDs());
          var kvp = new KeyValuePair<MapMember, List<long>>(parcelPolygonLyr, ids);
          var sourceParcelFeatures = new List<KeyValuePair<MapMember, List<long>>> { kvp };
          editOper.ShrinkParcelsToSeeds(myParcelFabricLayer, sourceParcelFeatures);
          if (!editOper.Execute())
            return editOper.ErrorMessage;
        }
        catch (Exception ex)
        {
          return ex.Message;
        }
        return "";
      });
      if (!string.IsNullOrEmpty(errorMessage))
        MessageBox.Show(errorMessage, "Shrink Parcels To Seeds.");
      #endregion
    }
    protected async void ChangeParcelType()
    {
      string errorMessage = await QueuedTask.Run( async () =>
      {
        //check for selected layer
        if (MapView.Active.GetSelectedLayers().Count == 0)
          return "Please select a source layer in the table of contents";
        //get the feature layer that's selected in the table of contents
        var sourcePolygonL = MapView.Active.GetSelectedLayers().OfType<FeatureLayer>().FirstOrDefault();
        if (sourcePolygonL == null)
          return "";
        var myParcelFabricLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<ParcelLayer>().FirstOrDefault();
        if (myParcelFabricLayer == null)
          return "";
        var targetFeatLyrEnum = await myParcelFabricLayer.GetParcelPolygonLayerByTypeName("Tax");
        if (targetFeatLyrEnum.Count() == 0)
          return "";
        var targetFeatLyr = targetFeatLyrEnum.FirstOrDefault(); //the target parcel polygon feature layer
        #region Change parcel type
        //add polygon layers and the feature ids to change the type on to a new KeyValuePair
        var ids = new List<long>(sourcePolygonL.GetSelection().GetObjectIDs());
        var kvp = new KeyValuePair<MapMember, List<long>>(sourcePolygonL, ids);
        var sourceFeatures = new List<KeyValuePair<MapMember, List<long>>> { kvp };
        try
        {
          var editOper = new EditOperation()
          {
            Name = "Change Parcel Type",
            ProgressMessage = "Change Parcel Type...",
            ShowModalMessageAfterFailure = true,
            SelectNewFeatures = true,
            SelectModifiedFeatures = false
          };
          editOper.ChangeParcelType(myParcelFabricLayer, sourceFeatures, targetFeatLyr);
          if (!editOper.Execute())
            return editOper.ErrorMessage;
        }
        catch (Exception ex)
        {
          return ex.Message;
        }
        return "";
        #endregion
      });
      if (!string.IsNullOrEmpty(errorMessage))
        MessageBox.Show(errorMessage, "Change Parcel Type.");
    }
    #region Get parcel type name from feature layer
    private async Task<string> GetParcelTypeNameFromFeatureLayer(ParcelLayer myParcelFabricLayer, FeatureLayer featLayer, GeometryType geomType)
    {
      if (featLayer == null) //nothing to do, return empty string
        return String.Empty;
      IEnumerable<string> parcelTypeNames = await myParcelFabricLayer.GetParcelTypeNames();
      foreach (string parcelTypeName in parcelTypeNames)
      {
        if (geomType == GeometryType.Polygon)
        {
          var polygonLyrParcelTypeEnum = await myParcelFabricLayer.GetParcelPolygonLayerByTypeName(parcelTypeName);
          foreach (FeatureLayer lyr in polygonLyrParcelTypeEnum)
            if (lyr == featLayer)
              return parcelTypeName;

          polygonLyrParcelTypeEnum = await myParcelFabricLayer.GetHistoricParcelPolygonLayerByTypeName(parcelTypeName);
          foreach (FeatureLayer lyr in polygonLyrParcelTypeEnum)
            if (lyr == featLayer)
              return parcelTypeName;
        }
        if (geomType == GeometryType.Polyline)
        {
          var lineLyrParcelTypeEnum = await myParcelFabricLayer.GetParcelLineLayerByTypeName(parcelTypeName);
          foreach (FeatureLayer lyr in lineLyrParcelTypeEnum)
            if (lyr == featLayer)
              return parcelTypeName;

          lineLyrParcelTypeEnum = await myParcelFabricLayer.GetHistoricParcelLineLayerByTypeName(parcelTypeName);
          foreach (FeatureLayer lyr in lineLyrParcelTypeEnum)
            if (lyr == featLayer)
              return parcelTypeName;
        }
      }
      return String.Empty;
    }
    #endregion
  }
}  
