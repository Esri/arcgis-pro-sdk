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
using ArcGIS.Core.Data;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Editing.Events;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;

namespace EditingSDKExamples
{
  internal class Events : Button
  {
    protected override void OnClick()
    {
    }

    #region editevent
    protected void subEditEvents()
    {
      //subscribe to editcompleted
      var eceToken = EditCompletedEvent.Subscribe(onEce);
    }

    protected Task onEce(EditCompletedEventArgs args)
    {
      //show number of edits
      Console.WriteLine("Creates: " + args.Creates.Values.Sum(list => list.Count).ToString());
      Console.WriteLine("Modifies: " + args.Modifies.Values.Sum(list => list.Count).ToString());
      Console.WriteLine("Deletes: " + args.Deletes.Values.Sum(list => list.Count).ToString());
      return Task.FromResult(0);
    }
    #endregion

    #region rowevent
    protected void subRowEvent()
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
      // RowEvent callbacks are always called on the QueuedTask so there is no need 
      // to wrap your code within a QueuedTask.Run lambda.

      // update a separate table when a row is created
      // You MUST use the ArcGIS.Core.Data API to edit the table. Do NOT
      // use a new edit operation in the RowEvent callbacks
      try
      {
        // update Notes table with information about the new feature
        var geoDatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(Project.Current.DefaultGeodatabasePath)));
        var table = geoDatabase.OpenDataset<Table>("Notes");
        var tableDefinition = table.GetDefinition();
        using (var rowbuff = table.CreateRowBuffer())
        {
          // add a description
          rowbuff["Description"] = "OID: " + args.Row.GetObjectID().ToString() + " " + DateTime.Now.ToShortTimeString();
          table.CreateRow(rowbuff);
        }
      }
      catch (Exception e)
      {
        MessageBox.Show($@"Error in OnRowCreated for OID: {args.Row.GetObjectID()} : {e.ToString()}");
      }
    }


    private Guid _currentRowChangedGuid = Guid.Empty;

    protected void OnRowChanged(RowChangedEventArgs args)
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
        //Validate any change to “police district”
        //   cancel the edit if validation on the field fails
        if (row.HasValueChanged(fldIdx))
        {
          if (FailsValidation(row["POLICE_DISTRICT"].ToString()))
          {
            //Cancel edits with invalid “police district” values
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

    protected void OnRowDeleted(RowChangedEventArgs args)
    {
      var row = args.Row;

      // cancel the delete if the feature is in Police District 5

      var fldIdx = row.FindField("POLICE_DISTRICT");
      if (fldIdx != -1)
      {
        var value = row[fldIdx].ToString();
        if (value == "5")
          args.CancelEdit();
      }
    }
    #endregion

    private bool FailsValidation(string s) => true;

  }
}
