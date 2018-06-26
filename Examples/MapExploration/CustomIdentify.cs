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
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Framework.Dialogs;

namespace Examples
{
  class CustomIdentify : MapTool
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
        MessageBox.Show(String.Join("\n", results.Select(kvp => String.Format("{0}: {1}", kvp.Key.Name, kvp.Value.Count()))), "Identify Result");
        return true;
      });
    }
  }
}
