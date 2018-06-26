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
  #region TextElementExample
  //Added references
  using ArcGIS.Desktop.Core;                         //Project
  using ArcGIS.Desktop.Layouts;                      //Layout class
  using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask

  public class TextElementExample
  {
    public static Task<bool> UpdateLayoutTextAsync(string LayoutName, string TextElementName, double X, double Y, int FontSize, string FontName, string TextString)
    {
      //Reference a layoutitem in a project by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals(LayoutName));
      if (layoutItem == null)
        return Task.FromResult(false);

      return QueuedTask.Run<bool>(() =>
      {
        //Reference and load the layout associated with the layout item
        Layout lyt = layoutItem.GetLayout();

        //Reference a text element by name
        TextElement txtElm = lyt.FindElement(TextElementName) as TextElement;
        if (txtElm == null)
          return false;

        //Change placement
        txtElm.SetX(X);
        txtElm.SetY(Y);

        //Change TextProperties
        TextProperties txt_prop = txtElm.TextProperties;
        txt_prop.FontSize = FontSize;           //e.g., FontSize = 48
        txt_prop.Font = FontName;               //e.g., FontName = "Times New Roman"
        txt_prop.Text = TextString;             //e.g., TextString = "Some new text";
        txtElm.SetTextProperties(txt_prop);

        return true;
      });
    }
  }
  #endregion TextElementExample

  internal class TextElementClass : Button
  {
    async protected override void OnClick()
    {
      bool x = await TextElementExample.UpdateLayoutTextAsync("Layout Name", "Text", 3, 4, 48, "Times New Roman", "Some new text");
      if (x == false)
        System.Windows.MessageBox.Show("Object Not found");
    }
  }
}
