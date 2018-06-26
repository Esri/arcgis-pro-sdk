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

namespace Layout_HelpExamples
{
  #region MapFrameExample1
  //This example sets a map frame's extent to a bookmark.

  //Added references
  using ArcGIS.Desktop.Core;                         //Project
  using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask
  using ArcGIS.Desktop.Layouts;                      //Layout class
  using ArcGIS.Desktop.Mapping;                      //Bookmark

  public class MapFrame_ZoomToBookmarksExample
  {
    public static Task<bool> ZoomToBookmarksAsync(string LayoutName, string MFName, string BkmkName)
    {
      //Reference a layoutitem in a project by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals(LayoutName));

      return QueuedTask.Run<bool>(() =>
      {
        //Reference and load the layout associated with the layout item
        Layout lyt = layoutItem.GetLayout();

        //Reference a mapframe by name
        MapFrame mfElm = lyt.Elements.FirstOrDefault(item => item.Name.Equals(MFName)) as MapFrame;

        //Set the map frame's extent to a bookmark
        Bookmark bookmark = mfElm.Map.GetBookmarks().FirstOrDefault(b => b.Name == BkmkName);
        mfElm.SetCamera(bookmark);

        return true;
      });
    }
  }
  #endregion

  #region MapFrameExample2
    //Next example goes here
  #endregion


}