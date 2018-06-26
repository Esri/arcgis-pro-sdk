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
        var rowCreateToken = RowCreatedEvent.Subscribe(onRowEvent, layerTable);
        var rowChangeToken = RowChangedEvent.Subscribe(onRowEvent, layerTable);
        var rowDeleteToken = RowDeletedEvent.Subscribe(onRowEvent, layerTable);
      });
    }

    protected void onRowEvent(RowChangedEventArgs args)
    {
      Console.WriteLine("RowChangedEvent " + args.EditType.ToString());
    }
    #endregion
  }
}
