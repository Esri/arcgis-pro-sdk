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
  #region LayoutViewClassExample1
  //This example opens an existing layout in a project.  It iterates through all panes to check to see if the layout view  
  //is already open. If a pane already references that layout, it will make it active.  If no panes reference the layout,
  //then it will open and active the layout in a new pane.

  //Added references
  using ArcGIS.Desktop.Core;                         
  using ArcGIS.Desktop.Layouts;                      
  using ArcGIS.Desktop.Framework.Threading.Tasks;    

  public class LayoutViewExample1
  {
    async public static Task<bool> OpenLayoutView(string LayoutName)
    {
      //Reference a layoutitem in a project by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals(LayoutName));

      if (layoutItem == null)
        return false;

      //Reference and load the layout associated with the layout item
      Layout lyt = await QueuedTask.Run(() => layoutItem.GetLayout());

      //Iterate through each pane to see if one already references the layout, if so, activate it
      foreach (var pane in ProApp.Panes)
      {
        var layoutPane = pane as ILayoutPane;
        if (layoutPane == null)
          continue;
        LayoutView lytView = layoutPane.LayoutView;
        if (lytView.Layout == lyt)
        {
          (layoutPane as Pane).Activate();
          await QueuedTask.Run(() => lytView.ZoomTo100Percent());
          return true;
        }
      }
      //Open a new pane, activate it, and zoom to 100%
      var newPane = await  QueuedTask.Run(() =>ProApp.Panes.CreateLayoutPaneAsync(lyt));
      await QueuedTask.Run(() => newPane.LayoutView.ZoomTo100Percent());
      return true;
    }
}
    #endregion LayoutViewClassExample1
}

namespace Layout_HelpExamples
{
  #region LayoutViewClassExample2
  //This example references the active layout view.  Next it finds a couple of elements and adds them to a collection. 
  //The elements in the collection are selected within the layout view.  Finally, the layout view is zoomed to the
  //extent of the selection.

  //Added references
  using ArcGIS.Desktop.Layouts;                     
  using ArcGIS.Desktop.Framework.Threading.Tasks;    

  public class LayoutViewExample2
  {
    async public static Task<bool> SetLayoutViewSelection()
    {
      // Make sure the active pane is a layout view and then reference it
      if (LayoutView.Active != null)
      {
        LayoutView lytView = LayoutView.Active;
        //Reference the layout associated with the layout view
        Layout lyt = await QueuedTask.Run(() => lytView.Layout);

        //Find a couple of layout elements and add them to a collection
        Element map1 = lyt.FindElement("Map1 Map Frame");
        Element map2 = lyt.FindElement("Map2 Map Frame");
        List<Element> elmList = new List<Element>();
        elmList.Add(map1);
        elmList.Add(map2);

        //Set the selection on the layout view and zoom to the selected elements
        await QueuedTask.Run(() =>  lytView.SelectElements(elmList));
        await QueuedTask.Run(() =>  lytView.ZoomToSelectedElements());
        return true;
      }
      return false;
    }
  }
  #endregion LayoutViewClassExample2
}


namespace Layout_HelpExamples
{

  internal class LayoutViewClass : Button
  {
    async protected override void OnClick()
    {
      bool x = await LayoutViewExample1.OpenLayoutView("Layout Name");
      if (x == false)
        System.Windows.MessageBox.Show("Object Not found");
      bool y = await LayoutViewExample2.SetLayoutViewSelection();
      if (y == false)
        System.Windows.MessageBox.Show("Could not set selection");
    }
  }
}
