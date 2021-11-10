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
using ArcGIS.Core.Geometry;

namespace Layout_HelpExamples
{
  // cref: CreateLayoutExample1;ArcGIS.Desktop.Layouts.ILayoutFactory.CreateLayout(System.Double,System.Double,ArcGIS.Core.Geometry.LinearUnit,System.Boolean,System.Double)
  // cref: CreateLayoutExample1;ArcGIS.Desktop.Layouts.LayoutFactory.CreateLayout(ArcGIS.Core.CIM.CIMPage)
  // cref: CreateLayoutExample1;ArcGIS.Desktop.Layouts.LayoutFactory
  #region CreateLayoutExample1
  //This example creates a new layout using a minimum set of parameters.

  //Added references
  using ArcGIS.Desktop.Layouts;                    
  using ArcGIS.Desktop.Framework.Threading.Tasks; 
  using ArcGIS.Desktop.Core;

  public class CreateLayoutEx1
  {
    async public static Task<Layout> CreateBasicLayout(double width, double height, LinearUnit units, string LayoutName)
    {
      Layout layout = null;
      await QueuedTask.Run(() =>
      {
        layout = LayoutFactory.Instance.CreateLayout(width, height, units);
        layout.SetName(LayoutName);
      });

      //Open the layout in a pane
      await ProApp.Panes.CreateLayoutPaneAsync(layout);

      return layout;
    }
  }
  #endregion CreateLayoutExample1
}


namespace Layout_HelpExamples
{
  // cref: CreateLayoutExample2;ArcGIS.Desktop.Layouts.ILayoutFactory.CreateLayout(ArcGIS.Core.CIM.CIMPage)
  // cref: CreateLayoutExample2;ArcGIS.Desktop.Layouts.LayoutFactory.CreateLayout(System.Double,System.Double,ArcGIS.Core.Geometry.LinearUnit,System.Boolean,System.Double)
  #region CreateLayoutExample2
  //This example creates a new layout using a CIM page definion with rulers and guides included.

  //Added references
  using ArcGIS.Desktop.Layouts;                      
  using ArcGIS.Desktop.Framework.Threading.Tasks;    
  using ArcGIS.Desktop.Core;
  using ArcGIS.Core.CIM;

  public class CreateLayoutEx2
  {
    async public static Task<Layout> CreateCIMLayout(double width, double height, LinearUnit units, string LayoutName)
    {
      Layout CIMlayout = null;
      await QueuedTask.Run(() =>
      {
        //Set up a page
        CIMPage newPage = new CIMPage();

        //required
        newPage.Width = width;
        newPage.Height = height;
        newPage.Units = units;

        //optional rulers
        newPage.ShowRulers = true;
        newPage.SmallestRulerDivision = 5;

        //optional guides
        newPage.ShowGuides = true;
        CIMGuide guide1 = new CIMGuide();
        guide1.Position = 25;
        guide1.Orientation = Orientation.Vertical;
        CIMGuide guide2 = new CIMGuide();
        guide2.Position = 185;
        guide2.Orientation = Orientation.Vertical;
        CIMGuide guide3 = new CIMGuide();
        guide3.Position = 25;
        guide3.Orientation = Orientation.Horizontal;
        CIMGuide guide4 = new CIMGuide();
        guide4.Position = 272;
        guide4.Orientation = Orientation.Horizontal;

        List<CIMGuide> guideList = new List<CIMGuide>();
        guideList.Add(guide1);
        guideList.Add(guide2);
        guideList.Add(guide3);
        guideList.Add(guide4);
        newPage.Guides = guideList.ToArray();

        Layout layout = LayoutFactory.Instance.CreateLayout(newPage);

        layout.SetName(LayoutName);
      });

      //Open the layout in a pane
      await ProApp.Panes.CreateLayoutPaneAsync(CIMlayout);
      return CIMlayout;
    }
  }
  #endregion CreateLayoutExample2
}

namespace Layout_HelpExamples
{
  internal class LayoutFactoryClass : Button
  {
    async protected override void OnClick()
    {
      Layout lyt1 = await CreateLayoutEx1.CreateBasicLayout(210, 297, LinearUnit.Millimeters, "A4 Layout");
      Layout lyt2 = await CreateLayoutEx1.CreateBasicLayout(17, 11, LinearUnit.Inches, "17x11 Layout");
    }
  }
}
