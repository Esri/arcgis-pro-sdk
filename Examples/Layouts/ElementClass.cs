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
  #region ElementExample
  //This example finds and element by name, gets its CIM definition, modifies a locked property not exposed to the managed API, and then applies that change
  //back to the element.  The locked property is displayed in the layout TOC as a lock symbol next to each element.

  //Added references
  using ArcGIS.Core.CIM;                             //CIM
  using ArcGIS.Desktop.Core;                         //Project
  using ArcGIS.Desktop.Layouts;                      //Layout class
  using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask

  public class ElementExample
  {
    public static Task<bool> LockTOCElementAsync(string LayoutName, string ElementName, bool Toggle)
    {
      //Reference a layoutitem in a project by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals(LayoutName));
      if (layoutItem == null)
        return Task.FromResult(false);

      return QueuedTask.Run<bool>(() =>
      {
        //Reference and load the layout associated with the layout item
        Layout lyt = layoutItem.GetLayout();

        //Reference an element by name
        Element elm = lyt.FindElement(ElementName);
        if (elm == null)
          return false;

        //Modify the Locked property via the CIM
        CIMElement CIMElm = elm.GetDefinition() as CIMElement;
        CIMElm.Locked = Toggle;                //e.g. Toggle = true
        elm.SetDefinition(CIMElm);
        System.Windows.MessageBox.Show(string.Format("{0} is now locked in the TOC.", elm.Name));

        return true;
      });
    }
  }
  #endregion ElementExample


  internal class ElementClass : Button
  {
    async protected override void OnClick()
    {
      bool x = await ElementExample.LockTOCElementAsync("Layout Name", "Rectangle", true);
      if (x == false)
        System.Windows.MessageBox.Show("Object Not found");
    }
  }
}
