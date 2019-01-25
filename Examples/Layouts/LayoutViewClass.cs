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
using ArcGIS.Core.CIM;                             //CIM
using ArcGIS.Core.Data;
using ArcGIS.Desktop.Core;                         //Project
using ArcGIS.Desktop.Layouts;                      //Layout class
using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask
using ArcGIS.Desktop.Mapping;                      //Export
using ArcGIS.Core.Geometry;

namespace Layout_HelpExamples
{

  internal class LayoutViewClass : Button
  {
    protected override void OnClick()
    {
      LayoutViewClassSamples.MethodSnippets();
    }
  }


  public class LayoutViewClassSamples
  {
    async public static void MethodSnippets()
    {
      LayoutView layoutView = LayoutView.Active;

      #region LayoutView_ZoomTo_Extent
      //Set the page extent for a layout view.

      //Process on worker thread
      await QueuedTask.Run(() =>
      { 
        var lytExt = layoutView.Extent;
        layoutView.ZoomTo(lytExt);
      });
      #endregion LayoutView_ZoomTo_Extent

      #region LayoutView_ZoomTo_Percent
      //Set the layout view to 100 percent.

      //Process on worker thread
      await QueuedTask.Run(() =>
      { 
        layoutView.ZoomTo100Percent();
      });
      #endregion LayoutView_ZoomTo_Percent

      #region LayoutView_ZoomTo_Next
      //Advance the layout view extent to the previous forward extent

      //Process on worker thread
      await QueuedTask.Run(() =>
      {
        layoutView.ZoomToNext();
      });
      #endregion LayoutView_ZoomTo_Next

      #region LayoutView_ZoomTo_PageWidth
      //Set the layout view extent to accomodate the width of the page.

      //Process on worker thread
      await QueuedTask.Run(() =>
      {
        layoutView.ZoomToPageWidth();
      });
      #endregion LayoutView_ZoomTo_PageWidth

      #region LayoutView_ZoomTo_Previous
      //Set the layout view extent to the previous extent.

      //Process on worker thread
      await QueuedTask.Run(() =>
      {
        layoutView.ZoomToPrevious();
      });
      #endregion LayoutView_ZoomTo_Previous

      #region LayoutView_ZoomTo_SelectedElements
      //Set the layout view extent to the selected elements.

      //Process on worker thread
      await QueuedTask.Run(() =>
      {
        layoutView.ZoomToSelectedElements();
      });
      #endregion LayoutView_ZoomTo_SelectedElements

      #region LayoutView_ZoomTo_WholePage
      //Set the layout view extent to fit the entire page.

      //Process on worker thread
      await QueuedTask.Run(() =>
      {
        layoutView.ZoomToWholePage();
      });
      #endregion LayoutView_ZoomTo_WholePage

      #region LayoutView_Refresh
      //Refresh the layout view.

      //Process on worker thread
      await QueuedTask.Run(() => layoutView.Refresh());
      #endregion

      #region LayoutView_GetSelection
      //Get the selected layout elements.

      var selectedElements = layoutView.GetSelectedElements();
      #endregion

      #region LayoutView_SetSelection
      //Set the layout's element selection.

      Element rec = layoutView.Layout.FindElement("Rectangle");
      Element rec2 = layoutView.Layout.FindElement("Rectangle 2");

      List<Element> elmList = new List<Element>();
      elmList.Add(rec);
      elmList.Add(rec2);

      layoutView.SelectElements(elmList);
      #endregion LayoutView_SetSelection

      #region LayoutView_SelectAll
      //Select all element on a layout.

      layoutView.SelectAllElements();
      #endregion LayoutView_SelectAll

      #region LayoutView_ClearSelection
      //Clear the layout's element selection.

      layoutView.ClearElementSelection();
      #endregion LayoutView_ClearSelection


      Layout layout = LayoutFactory.Instance.CreateLayout(5, 5, LinearUnit.Inches);
      #region LayoutView_FindAndCloseLayoutPanes
      //Find and close all layout panes associated with a specific layout.

      LayoutProjectItem findLytItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals("Layout"));
      Layout findLyt = await QueuedTask.Run(() => findLytItem.GetLayout());  //Perform on the worker thread

      var panes = ProApp.Panes.FindLayoutPanes(findLyt);
      foreach (Pane pane in panes)
      {
        ProApp.Panes.CloseLayoutPanes(findLyt.URI);  //Close the pane
      }
      #endregion LayoutView_FindandCloseLayoutPanes

      #region LayoutView_LayoutFrameWorkExtender
      //This sample checks to see if a layout project item already has an open application pane.  
      //If it does, it checks if it is the active layout view, if not, it creates, activates and opens a new pane.

      //Reference a layoutitem in a project by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals("Layout View"));

      //Get the layout associated with the layoutitem
      Layout lyt = await QueuedTask.Run(() => layoutItem.GetLayout());
      
      //Iterate through each pane in the application and check to see if the layout is already open and if so, activate it
      foreach (var pane in ProApp.Panes)
      {
        var layoutPane = pane as ILayoutPane;
        if (layoutPane == null)  //if not a layout pane, continue to the next pane
          continue;
        if (layoutPane.LayoutView.Layout == lyt)  //if the layout pane does match the layout, activate it.
        {
          (layoutPane as Pane).Activate();
          layoutPane.Caption = "This is a test";
          System.Windows.MessageBox.Show(layoutPane.Caption);
          return;
        }
      }
      //Otherwise, create, open, and activate the layout if not already open
      ILayoutPane newPane = await ProApp.Panes.CreateLayoutPaneAsync(lyt);

      //Zoom to the full extent of the layout
      await QueuedTask.Run(() => newPane.LayoutView.ZoomTo100Percent());
      #endregion LayoutView_LayoutFrameWorkExtender
    }

    async public static void LayoutViewExample()
    {
      #region LayoutViewClassExample1
      //This example references the active layout view.  Next it finds a couple of elements and adds them to a collection. 
      //The elements in the collection are selected within the layout view.  Finally, the layout view is zoomed to the
      //extent of the selection.

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
        await QueuedTask.Run(() => lytView.SelectElements(elmList));
        await QueuedTask.Run(() => lytView.ZoomToSelectedElements());
      }
      #endregion LayoutViewClassExample1
    }
  }
}

namespace Layout_HelpExamples
{


  public class LayoutViewExample2
  {
    async public static Task<bool> SetLayoutViewSelection()
    {
#region LayoutViewClassExample2
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
        await QueuedTask.Run(() => lytView.SelectElements(elmList));
        await QueuedTask.Run(() => lytView.ZoomToSelectedElements());
        return true;
      }
      return false;
#endregion LayoutViewClassExample2
        }
    }
 
}



