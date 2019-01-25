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
using ArcGIS.Desktop.Framework.Contracts;

//Added references
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Layouts;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Core.Geometry;

namespace Layout_HelpExamples
{
  public class MapFrameClassSamples
  {
    async public static void MethodSnippets()
    {
      Layout layout = LayoutView.Active.Layout;

      #region MapFrame_Export
      //See ProSnippets.cs "Export a map frame to JPG"
      #endregion MapFrame_Export


      #region MapFrame_GetMapView
      //see ProSnippets "Export the map view associated with a map frame to BMP"
      #endregion MapFrame_GetMapView


      #region MapFrame_SetCamera_Camera
      // see ProSnippets "Change map frames camera settings"
      #endregion MapFrame_SetCamera_Camera


      #region MapFrame_SetCamera_Bookmark
      //Set the extent of a map frame to a bookmark.

      //Perform on the worker thread
      await QueuedTask.Run(() =>
      {
        //Reference MapFrame
        MapFrame mf_bk = layout.FindElement("Map Frame") as MapFrame;

        //Reference a bookmark that belongs to a map associated with the map frame
        Map m = mf_bk.Map;
        Bookmark bk = m.GetBookmarks().FirstOrDefault(item => item.Name.Equals("Lakes"));

        //Set the map frame extent using the bookmark
        mf_bk.SetCamera(bk);
      });
      #endregion MapFrame_SetCamera_Bookmark


      #region MapFrame_SetCamera_Envelope
      //Set the extent of a map frame to the envelope of a feature.

      //Perform on the worker thread
      await QueuedTask.Run(() =>
      {
        //Reference MapFrame
        MapFrame mf_env = layout.FindElement("Map Frame") as MapFrame;

        //Get map and a layer of interest
        Map m = mf_env.Map;
        //Get the specific layer you want from the map and its extent
        FeatureLayer lyr = m.FindLayers("GreatLakes").First() as FeatureLayer;
        Envelope lyrEnv = lyr.QueryExtent();

        //Set the map frame extent to the feature layer's extent / envelope
        mf_env.SetCamera(lyrEnv);  //Note - you could have also used the lyr as an overload option
      });
      #endregion MapFrame_SetCamera_Envelope


      #region MapFrame_SetCamera_Layer
      //See ProSnppets "Zoom map frame to extent of a single layer"
      #endregion MapFrame_SetCamera_Layer


      #region MapFrame_SetCamera_Layers
      //See ProSnippets "Change map frame extent to selected features in multiple layers"
      #endregion MapFrame_SetCamera_Layers

      MapFrame mf = null;
      Map map = null;
      #region MapFrame_SetMap
      //Set the map that is associated with a map frame.

      //Perform on worker thread
      await QueuedTask.Run(() =>
      {
        mf.SetMap(map);
      });

      #endregion

      #region MapFrame_SetName
      //See ProSnppets "Zoom map frame to extent of a single layer"
      #endregion MapFrame_SetName

    }
  }
}


