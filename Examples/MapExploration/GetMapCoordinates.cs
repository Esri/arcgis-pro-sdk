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
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Framework.Threading.Tasks;

namespace Examples
{
  internal class GetMapCoordinates : MapTool
  {
    protected override void OnToolMouseDown(MapViewMouseButtonEventArgs  e)
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
}
