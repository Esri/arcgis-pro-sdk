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
  #region CIMGraphic
  //This example references a graphic element on a layout and sets its Transparency property (which is not available in the managed API)
  //by accessing the element's CIMGraphic.

  //Added references
  using ArcGIS.Core.CIM;                             //CIM
  using ArcGIS.Desktop.Core;                         //Project
  using ArcGIS.Desktop.Layouts;                      //Layout class
  using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask

  public class GraphicElementExample1
  {
    public static Task<bool> UpdateElementTransparencyAsync(string LayoutName, string ElementName, int TransValue)
    {
      //Reference a layoutitem in a project by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals(LayoutName));
      if (layoutItem == null)
        return Task.FromResult(false);

      return QueuedTask.Run<bool>(() =>
      {
        //Reference and load the layout associated with the layout item
        Layout lyt = layoutItem.GetLayout();

        //Reference a element by name
        GraphicElement graElm = lyt.FindElement(ElementName) as GraphicElement;
        if (graElm == null)
          return false;

        //Modify the Transparency property that exists only in the CIMGraphic class.
        CIMGraphic CIMGra = graElm.GetGraphic() as CIMGraphic;
        CIMGra.Transparency = TransValue;             //e.g., TransValue = 50
        graElm.SetGraphic(CIMGra);

        return true;
      });
    }
  }
  #endregion CIMGraphic
}


namespace Layout_HelpExamples
{
  #region CloneGraphic
  //This example finds a layout element by name and clones it a specified number of times and applies an accumlative offset for each 
  //cloned element.

  //Added references
  using ArcGIS.Core.CIM;                             //CIM
  using ArcGIS.Desktop.Core;                         //Project
  using ArcGIS.Desktop.Layouts;                      //Layout class
  using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask

  public class GraphicElementExample2
  {
    public static Task<bool> CloneElementAsync(string LayoutName, string ElementName, double offset, int numCopies)
    {
      //Reference a layoutitem in a project by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals(LayoutName));
      if (layoutItem == null)
        return Task.FromResult(false);

      return QueuedTask.Run<bool>(() =>
      {
        //Reference and load the layout associated with the layout item
        Layout lyt = layoutItem.GetLayout();

        //Reference a element by name
        GraphicElement graElm = lyt.FindElement(ElementName) as GraphicElement;
        if (graElm == null)
          return false;

        //Loop through the number of copies, clone, and set the offsets for each new element
        double orig_offset = offset;
        while (numCopies != 0)
        {
          GraphicElement cloneElm = graElm.Clone("Clone");
          cloneElm.SetX(cloneElm.GetX() + offset);
          cloneElm.SetY(cloneElm.GetY() - offset);
          offset = offset + orig_offset;
          numCopies = numCopies - 1;
        }

        return true;
      });
    }
  }
  #endregion CloneGraphic
}


namespace Layout_HelpExamples
{
  internal class GraphicElementClass : Button
  {
    async protected override void OnClick()
    {
      //Set Transparency
      bool b1 = await GraphicElementExample1.UpdateElementTransparencyAsync("Layout Name", "Rectangle", 50);
      if (b1)
        System.Windows.MessageBox.Show("Successfully Changed Transparency ");
      else
        System.Windows.MessageBox.Show("Could Not Change Transparency");


      //Clone Element
      bool b2 = await GraphicElementExample2.CloneElementAsync("Layout Name", "Rectangle", 0.25, 5);
      if (b2)
        System.Windows.MessageBox.Show("Successfully Cloned Element ");
      else
        System.Windows.MessageBox.Show("Could Not Clone Element");

    }
  }
}
