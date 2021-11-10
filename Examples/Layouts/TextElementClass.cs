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
using ArcGIS.Desktop.Core;                         //Project
using ArcGIS.Desktop.Layouts;                      //Layout class
using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask

namespace Layout_HelpExamples
{
  internal class TextElementClass : Button
  {
    protected override void OnClick()
    {
      TextElementClassExamples.MethodSnippets();
    }
  }

  public class TextElementClassExamples
  {
    public static void MethodSnippets()
    {
      #region TextElement_SetTextProperties
      //see Prosnippets.cs: "Update text element properties"
      #endregion

      #region TextProperties_Constuctor
      //see Prosnippets.cs: "Update text element properties"
      #endregion
    }

    async public static void TextPropertiesExample()
    {
      #region Modify existing text element properties
      //Modify the text properties for an existing text element.

      //Reference a layoutitem in a project by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals("Layout"));

      //Perform on the worker thread
      await QueuedTask.Run(() =>
      {
        //Reference and load the layout associated with the layout item
        Layout lyt = layoutItem.GetLayout();

        //Reference a text element by name
        TextElement txtElm = lyt.FindElement("Title") as TextElement;

        //Change placement
        txtElm.SetX(4.25);
        txtElm.SetY(8);

        //Change TextProperties
        TextProperties txt_prop = txtElm.TextProperties;
        txt_prop.FontSize = 48;
        txt_prop.Font = "Times New Roman";
        txt_prop.Text = "Some new text";
        txtElm.SetTextProperties(txt_prop);
      });
      #endregion

    }
    async public static void TextPropertiesExample2() //TODO fis duplicated item above because of inconsistent region name use in TextElement.cs file in layout solution.
    {
      // cref: Modify existing text properties;ArcGIS.Desktop.Layouts.TextProperties
      #region Modify existing text properties
      //Modify the text properties for an existing text element.

      //Reference a layoutitem in a project by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals("Layout"));

      //Perform on the worker thread
      await QueuedTask.Run(() =>
      {
        //Reference and load the layout associated with the layout item
        Layout lyt = layoutItem.GetLayout();

        //Reference a text element by name
        TextElement txtElm = lyt.FindElement("Title") as TextElement;

        //Change placement
        txtElm.SetX(4.25);
        txtElm.SetY(8);

        //Change TextProperties
        TextProperties txt_prop = txtElm.TextProperties;
        txt_prop.FontSize = 48;
        txt_prop.Font = "Times New Roman";
        txt_prop.Text = "Some new text";
        txtElm.SetTextProperties(txt_prop);
      });
      #endregion

    }

  }

}
