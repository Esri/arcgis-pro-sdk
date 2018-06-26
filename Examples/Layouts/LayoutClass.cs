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
using ArcGIS.Desktop.Layouts;

namespace Layout_HelpExamples
{
  #region LayoutHelpExample1
  //This example references a layout by name using GetLayout on the LayoutProjectItem.

  //Added references
  using ArcGIS.Desktop.Core;                         //Project
  using ArcGIS.Desktop.Layouts;                      //Layout class
  using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask

  public class FindLayout
  {
    public static Task<Layout> FindLayoutAsync(string LayoutName)
    {
      //Reference a layoutitem in a project by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals(LayoutName));

      //Check for layoutItem
      if (layoutItem == null)
        return Task.FromResult<Layout>(null);

      //Reference and load the layout associated with the layout item
      return QueuedTask.Run(() => layoutItem.GetLayout());
    }
  }
  #endregion LayoutHelpExample1
}

namespace Layout_HelpExamples
{
  #region LayoutHelpExample2
  //This example loops through each layout in a project and changes the page size.

  //Added references
  using ArcGIS.Desktop.Core;                         //Project
  using ArcGIS.Desktop.Layouts;                      //Layout class
  using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask
  using ArcGIS.Core.CIM;


  public class ResizeLayouts
  {
    public static Task LoopLayoutsAsync()
    {
      return QueuedTask.Run(() =>
      {
        //Loop through each item in the list and reference and load each Layout
        foreach (var layoutItem in Project.Current.GetItems<LayoutProjectItem>())
        {
          Layout lyt = layoutItem.GetLayout();
          CIMPage page = lyt.GetPage();
          page.Width = 8.5;
          page.Height = 11;
          lyt.SetPage(page);
        }
      });
    }
  }
  #endregion LayoutHelpExample2
}



namespace Layout_HelpExamples
{

  internal class LayoutClass : Button
  {
    async protected override void OnClick()
    {
      Layout lyt1 = await FindLayout.FindLayoutAsync("Layout Name");
      if (lyt1 == null)
        System.Windows.MessageBox.Show("Layout Not found.");
      else
        System.Windows.MessageBox.Show(lyt1.Name);

      await ResizeLayouts.LoopLayoutsAsync();
    }
  }
}
