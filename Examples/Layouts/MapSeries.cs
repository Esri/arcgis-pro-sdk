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
using ArcGIS.Core.CIM;                                   
using ArcGIS.Desktop.Layouts;                 
using ArcGIS.Desktop.Framework.Threading.Tasks;  
using ArcGIS.Desktop.Mapping;
using ArcGIS.Core.Data;

namespace Layout_HelpExamples
{

  internal class MapSeriesSnippets : Button
  {
    protected override void OnClick()
    {
      MapSeriesClassExamples.MethodSnippets();
    }
  }
  public class MapSeriesClassExamples
  {
    async public static void MethodSnippets()
    {
      #region MapSeries_Constructor1
      //Set up map series export options

      MapSeriesExportOptions MSExport_SinglePage = new MapSeriesExportOptions()
      {
        ExportPages = ExportPages.Custom,  //Provide a specific list of pages
        CustomPages = "1-3, 5",  //Only used if ExportPages.Custom is set
        ExportFileOptions = ExportFileOptions.ExportAsSinglePDF,  //Export all pages to a single, multi-page PDF
        ShowSelectedSymbology = false  //Do no show selection symbology in the output
      };
      #endregion MapSeries_Constructor1


      #region MapSeries_Constructor2
      //Set up map series export options

      MapSeriesExportOptions MSExport_IndivPages = new MapSeriesExportOptions()
      {
        ExportPages = ExportPages.All,  //All pages
        ExportFileOptions = ExportFileOptions.ExportMultipleNames,  //Export each page to an individual file using page name as a suffix.
        ShowSelectedSymbology = true  //Include selection symbology in the output
      };
      #endregion MapSeries_Constructor2



      #region MapSeries_FindPageNumber
      //Return the page number that corresponds to the page name field for an index feature

      Layout layout = LayoutView.Active.Layout;

      //Perform on the worker thread
      await QueuedTask.Run(() =>
      {
        SpatialMapSeries SMS = layout.MapSeries as SpatialMapSeries;
        Row msRow = SMS.CurrentRow;
        System.Windows.MessageBox.Show(SMS.FindPageNumber(msRow.GetOriginalValue(msRow.FindField("NAME")).ToString()));
      });
      #endregion MapSeries_FindPageNumber


      #region MapSeries_GetSetDefinition 
      //Get and modify a map series CIM definition and set the changes back to the layout

      //Perform on the worker thread
      await QueuedTask.Run(() =>
      {
        MapSeries MS = layout.MapSeries as MapSeries;
        CIMMapSeries CIM_MS = MS.GetDefinition();
        CIM_MS.Enabled = false;
        MS.SetDefinition(CIM_MS);
        layout.SetMapSeries(MS);
      });
      #endregion MapSeries_GetSetDefinition


      #region MapSeries_SetCurrentPageNumber
      //Set the current page to match a specific page number

      //Perform on the worker thread
      await QueuedTask.Run(() =>
      {
        MapSeries MS = layout.MapSeries as MapSeries;
        MS.SetCurrentPageNumber("IV");
      });
      #endregion MapSeries_SetCurrentPageNumber

    }
  }

  //public void snippets_CIMSpatialMapSeries()
  //{
  //  #region Create a Spatial Map Series using the CIM

  //  //Get the project item
  //  LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals("MyLayout"));
  //  if (layoutItem != null)
  //  {
  //    QueuedTask.Run(() =>
  //    {
  //      //Get the layout
  //      Layout layout = layoutItem.GetLayout();
  //      if (layout != null)
  //      {
  //        // Define CIMSpatialMapSeries in CIMLayout
  //        CIMLayout layCIM = layout.GetDefinition();

  //        layCIM.MapSeries = new CIMSpatialMapSeries();
  //        CIMSpatialMapSeries ms = layCIM.MapSeries as CIMSpatialMapSeries;
  //        ms.Enabled = true;
  //        ms.MapFrameName = "Railroad Map Frame";
  //        ms.StartingPageNumber = 1;
  //        ms.CurrentPageID = 1;
  //        ms.IndexLayerURI = "CIMPATH=map/railroadmaps.xml";
  //        ms.NameField = "ServiceAreaName";
  //        ms.SortField = "SeqId";
  //        ms.RotationField = "Angle";
  //        ms.SortAscending = true;
  //        ms.ScaleRounding = 1000;
  //        ms.ExtentOptions = ExtentFitType.BestFit;
  //        ms.MarginType = ArcGIS.Core.CIM.UnitType.Percent;
  //        ms.Margin = 2;

  //        layout.SetDefinition(layCIM);
  //      }
  //    });
  //  }
  //  #endregion
  //}

}