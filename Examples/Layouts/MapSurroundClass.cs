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

//added references
using ArcGIS.Desktop.Core;                         //Project
using ArcGIS.Desktop.Layouts;                      //Layout class
using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask

namespace Layout_HelpExamples
{
  #region MapSurroundExample
  //Added references
  using ArcGIS.Desktop.Core;                         //Project
  using ArcGIS.Desktop.Layouts;                      //Layout class
  using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask

  public class MapSurroundExample
  {
    public static Task<bool> UpdateMapSurroundAsync(string LayoutName, string SBName, string MFName)
    {
      //Reference a layoutitem in a project by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals(LayoutName));
      if (layoutItem == null)
        return Task.FromResult(false);

      return QueuedTask.Run<bool>(() =>
      {
              //Reference and load the layout associated with the layout item
              Layout lyt = layoutItem.GetLayout();

              //Reference a scale bar element by name
              MapSurround scaleBar = lyt.FindElement(SBName) as MapSurround;
        if (scaleBar == null)
          return false;

              //Reference a map frame element by name
              MapFrame mf = lyt.FindElement(MFName) as MapFrame;
        if (mf == null)
          return false;

              //Set the scale bar to the newly referenced map frame
              scaleBar.SetMapFrame(mf);
        return true;
      });
    }
  }
  #endregion MapSurroundExample

  internal class MapSurroundClass : Button
  {
    async protected override void OnClick()
    {
      bool b = await MapSurroundExample.UpdateMapSurroundAsync("Layout Name", "Scale Bar", "Map2 Map Frame");
      if (b == false)
        System.Windows.MessageBox.Show("Object Not found");
    }
  }
}
