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
  // cref: ArcGIS.Desktop.Layouts.PictureElement
  // cref: ArcGIS.Desktop.Layouts.PictureElement.SetSourcePath
  #region PictureElementExample
  ///This example references a PictureElement on a layout and changes the picture by setting a path to a file on disk using the

  //Added references
  using ArcGIS.Desktop.Core;
  using ArcGIS.Desktop.Layouts;
  using ArcGIS.Desktop.Framework.Threading.Tasks;

  public class PictureElementExample
  {
    public static Task<bool> UpdatePictureElementAsync(string LayoutName, string PictureName, string PicturePath)
    {
      //Reference a layoutitem in a project by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals(LayoutName));
      if (layoutItem == null)
        return Task.FromResult(false);

      return QueuedTask.Run<bool>(() =>
      {
        //Reference and load the layout associated with the layout item
        Layout lyt = layoutItem.GetLayout();

        //Reference a picture element by name
        PictureElement picElm = lyt.FindElement(PictureName) as PictureElement;
        if (picElm == null)
          return false;

        //Change the path to a new source
        picElm.SetSourcePath(PicturePath);

        return true;
      });
    }
  }
  #endregion PictureElementExample

  internal class PictureElementClass : Button
  {
    async protected override void OnClick()
    {
      string some_new_path;
      some_new_path = @"D:\Active\Layout\SDK\2.0\Layout_HelpExamples\WhitePass.jpg";

      bool x = await PictureElementExample.UpdatePictureElementAsync("Layout Name", "Picture", some_new_path);
      if (x == false)
        System.Windows.MessageBox.Show("Object Not found");
    }
  }
}
