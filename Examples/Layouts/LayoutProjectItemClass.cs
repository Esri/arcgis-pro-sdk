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

//namespace Layout_HelpExamples
//{
//  #region FindLayout
//  //This example references a layout by name using GetLayout on the LayoutProjectItem.

//  //Added references
//  using ArcGIS.Desktop.Core;
//  using ArcGIS.Desktop.Layouts;
//  using ArcGIS.Desktop.Framework.Threading.Tasks;  

//  public class FindLayoutExample
//  {
//    public static Task<Layout> FindLayoutAsync(string LayoutName)
//    {
//      //Reference a layoutitem in a project by name
//      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals(LayoutName));

//      //Check for layoutItem
//      if (layoutItem == null)
//        return Task.FromResult<Layout>(null);

//      //Reference and load the layout associated with the layout item
//      return QueuedTask.Run(() => layoutItem.GetLayout());
//    }
//  }
//  #endregion FindLayout
//}

namespace Layout_HelpExamples
{
  #region DeleteLayout
  //This example deletes a layout in a project after finding it by name.

  //Added references
  using ArcGIS.Desktop.Core; 
  using ArcGIS.Desktop.Layouts;

  public class DeleteLayoutExample
  {
    public static Task<bool> DeleteLayoutAsync(string LayoutName)
    {
      //Reference a layoutitem in a project by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals(LayoutName));

      //Check for layoutItem
      if (layoutItem == null)
        return Task.FromResult<bool>(false);

      //Delete the layout from the project
      return Task.FromResult<bool>(Project.Current.RemoveItem(layoutItem));
    }
  }
  #endregion DeleteLayout
}

namespace Layout_HelpExamples
{
  using ArcGIS.Desktop.Layouts;                      //Layout class
  internal class LayoutProjectItemClass : Button
  {
    async protected override void OnClick()
    {
      ////ReferenceLayout
      //Layout lyt1 = await FindLayoutExample.FindLayoutAsync("Layout Name");
      //if (lyt1 == null)
      //  System.Windows.MessageBox.Show("Layout Not Found");
      //else
      //  System.Windows.MessageBox.Show("Found: " + lyt1.Name);

      //DeleteLayout
      bool b1 = await DeleteLayoutExample.DeleteLayoutAsync("Layout");
      if (b1)
        System.Windows.MessageBox.Show("Successfully Deleted Layout ");
      else
        System.Windows.MessageBox.Show("Could Not Delete Layout");
    }
  }
}
