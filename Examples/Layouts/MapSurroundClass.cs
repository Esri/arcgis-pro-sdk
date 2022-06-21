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

//added references
using ArcGIS.Desktop.Core;                         //Project
using ArcGIS.Desktop.Layouts;                      //Layout class
using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Core.CIM;

namespace Layout_HelpExamples
{
  // cref: ArcGIS.Desktop.Layouts.MapSurround
  // cref: ArcGIS.Desktop.Layouts.MapSurround.SetMapFrame
  // cref: ArcGIS.Desktop.Layouts.MapFrame
  #region MapSurroundExample
  //Added references
  using ArcGIS.Desktop.Core;                         //Project
  using ArcGIS.Desktop.Layouts;                      //Layout class
  using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask

  public class MapSurroundExample
  {
    public static Task<bool> UpdateMapSurroundAsync(string LayoutName, string SBName, string MFName)
    {
      //Reference a layoutitem in a project by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals(LayoutName));
      if (layoutItem == null)
        return Task.FromResult(false);

      return QueuedTask.Run<bool>(() =>
      {
              //Reference and load the layout associated with the layout item
              Layout lyt = layoutItem.GetLayout();

              //Reference a scale bar element by name
              MapSurround scaleBar = lyt.FindElement(SBName) as MapSurround;
        if (scaleBar == null)
          return false;

              //Reference a map frame element by name
              MapFrame mf = lyt.FindElement(MFName) as MapFrame;
        if (mf == null)
          return false;

              //Set the scale bar to the newly referenced map frame
              scaleBar.SetMapFrame(mf);
        return true;
      });
    }
  }
  #endregion MapSurroundExample

  internal class MapSurroundClass : Button
  {
    async protected override void OnClick()
    {
      bool b = await MapSurroundExample.UpdateMapSurroundAsync("Layout Name", "Scale Bar", "Map2 Map Frame");
      if (b == false)
        System.Windows.MessageBox.Show("Object Not found");
    }
  }
}

public class TableFrameClassSamples
{

  async public static void CreateNew()
  {
    // cref: ArcGIS.Desktop.Layouts.TableFrame
    // cref: ArcGIS.Desktop.Layouts.TableFrameInfo
    // cref: ArcGIS.Desktop.Layouts.TableFrameInfo.FieldNames
    // cref: ArcGIS.Desktop.Layouts.MapSurroundInfo.MapFrameName
    // cref: ArcGIS.Desktop.Layouts.MapSurroundInfo.MapMemberUri
    // cref: ArcGIS.Desktop.Layouts.MapSurround.SetMapFrame
    // cref: ArcGIS.Desktop.Layouts.MapFrame
    // cref: ArcGIS.Desktop.Layouts.MapFrame.Map
    // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateMapSurroundElement
    // cref: ArcGIS.Desktop.Layouts.IElementFactory.CreateMapSurroundElement
    #region TableFrame_CreateNew
    //Create a new table frame on the active layout.

    Layout layout = LayoutView.Active.Layout;

    //Perform on the worker thread
    await QueuedTask.Run(() =>
    {
      //Build 2D envelope geometry
      Coordinate2D rec_ll = new Coordinate2D(1.0, 3.5);
      Coordinate2D rec_ur = new Coordinate2D(7.5, 4.5);
      //At 2.x - Envelope rec_env = EnvelopeBuilder.CreateEnvelope(rec_ll, rec_ur);
      Envelope rec_env = EnvelopeBuilderEx.CreateEnvelope(rec_ll, rec_ur);

      //Reference map frame
      MapFrame mf = layout.FindElement("Map Frame") as MapFrame;

      //Reference layer
      Map m = mf.Map;
      FeatureLayer lyr = m.FindLayers("GreatLakes").First() as FeatureLayer;

      //Build fields list
      var fields = new[] { "NAME", "Shape_Area", "Shape_Length" };

      //Construct the table frame
      //At 2.x - TableFrame tabFrame = LayoutElementFactory.Instance.CreateTableFrame(layout, rec_env, mf, lyr, fields);
      var surroundInfo = new TableFrameInfo()
      {
        FieldNames = fields,
        MapFrameName = mf.Name,
        MapMemberUri = lyr.URI
      };
      var tabFrame = ElementFactory.Instance.CreateMapSurroundElement(layout, rec_env, surroundInfo) as TableFrame;
    });
    #endregion TableFrame_CreateNew

  }

  async public static void ModifyExisting()
  {
    // cref: ArcGIS.Desktop.Layouts.Element.GetDefinition
    // cref: ArcGIS.Desktop.Layouts.Element.SetDefinition
    // cref: ArcGIS.Desktop.Layouts.TableFrame
    // cref: ArcGIS.Core.CIM.CIMTableFrame
    // cref: ArcGIS.Core.CIM.CIMTableFrame.Alternate1RowBackgroundCount
    // cref: ArcGIS.Core.CIM.CIMTableFrame.Alternate2RowBackgroundCount
    #region TableFrame_ModifyExisting 
    //Modify an existing tableframe using CIM properties.

    //Reference the active layout
    Layout layout = LayoutView.Active.Layout;

    //Perform on the worker thread
    await QueuedTask.Run(() =>
    {
      //Reference table frame
      TableFrame TF = layout.FindElement("Table Frame") as TableFrame;

      //Modify CIM properties
      CIMTableFrame cimTF = TF.GetDefinition() as CIMTableFrame;
      cimTF.Alternate1RowBackgroundCount = 1;
      cimTF.Alternate2RowBackgroundCount = 1;

      //Apply the changes
      TF.SetDefinition(cimTF);
    });
    #endregion TableFrame_ModifyExisting
  }

}
