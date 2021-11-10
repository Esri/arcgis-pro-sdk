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
using ArcGIS.Core.CIM;                             //CIM
using ArcGIS.Desktop.Core;                         //Project
using ArcGIS.Desktop.Layouts;                      //Layout class
using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask
using ArcGIS.Desktop.Mapping;


namespace Layout_HelpExamples
{

  internal class LayoutClass : Button
  {
    protected override void OnClick()
    {
      LayoutClassSamples.MethodSnippets();
    }
  }

  public class LayoutClassSamples
  {
    async public static void MethodSnippets()
    {
      #region LayoutProjectItem_GetLayout
      //Reference the layout associated with a layout project item.

      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals("Layout Name"));
      Layout layout = await QueuedTask.Run(() => layoutItem.GetLayout());  //Perform on the worker thread
      #endregion LayoutProjectItem_GetLayout


      TextElement elm = layout.FindElement("Text") as TextElement;
      // cref: Layout_DeleteElement;ArcGIS.Desktop.Layouts.Layout.DeleteElement(ArcGIS.Desktop.Layouts.Element)
      #region Layout_DeleteElement
      //Delete a single layout element.

      //Perform on the worker thread
      await QueuedTask.Run(() =>
      {
        layout.DeleteElement(elm);
      });
      #endregion Layout_DeleteElement


      // cref: Layout_DeleteElements;ArcGIS.Desktop.Layouts.Layout.DeleteElements(System.Func{ArcGIS.Desktop.Layouts.Element,System.Boolean})
      #region Layout_DeleteElements
      //Delete multiple layout elements.

      //Perform on the worker thread
      await QueuedTask.Run(() =>
      {
        layout.DeleteElements(item => item.Name.Contains("Clone"));
      });
      #endregion Layout_DeleteElements


      // cref: Layout_FindElement;ArcGIS.Desktop.Layouts.Layout.FindElement(System.String)
      #region Layout_FindElement
      //Find a layout element.  The example below is referencing an existing text element.

      TextElement txtElm = layout.FindElement("Text") as TextElement;
      #endregion Layout_FindElement


      // cref: Layout_GetSetDefinition;ArcGIS.Desktop.Layouts.Layout.GetDefinition
      // cref: Layout_GetSetDefinition;ArcGIS.Desktop.Layouts.Layout.SetDefinition(ArcGIS.Core.CIM.CIMLayout)
      #region Layout_GetSetDefinition
      //Modify a layout's CIM definition

      //Perform on the worker thread
      await QueuedTask.Run(() =>
      {
        CIMLayout cimLayout = layout.GetDefinition();

        //Do something

        layout.SetDefinition(cimLayout);
      });
      #endregion Layout_GetSetDefinition


      // cref: Layout_GetSetPage;ArcGIS.Desktop.Layouts.Layout.GetPage
      // cref: Layout_GetSetPage;ArcGIS.Desktop.Layouts.Layout.SetPage(ArcGIS.Core.CIM.CIMPage)
      #region Layout_GetSetPage
      //Modify a layouts page settings.

      //Perform on the worker thread
      await QueuedTask.Run(() =>
      {

        CIMPage page = layout.GetPage();

        //Do something 

        layout.SetPage(page);
      });
      #endregion Layout_GetSetPage


      String filePath = null;
      #region Layout_ExportPDF
      //See ProSnippets "Export layout to PDF"
      #endregion Layout_ExportPDF


      #region Layout_ExportMS_PDF
      //Export multiple map series pages to PDF

      //Create a PDF export format
      PDFFormat msPDF = new PDFFormat()
      {
        Resolution = 300,
        OutputFileName = filePath,
        DoCompressVectorGraphics = true
      };

      //Set up the export options for the map series
      MapSeriesExportOptions MSExport_custom = new MapSeriesExportOptions()
      {
        ExportPages = ExportPages.Custom,
        CustomPages = "1-3, 5",
        ExportFileOptions = ExportFileOptions.ExportAsSinglePDF,
        ShowSelectedSymbology = false
      };

      //Check to see if the path is valid and export
      if (msPDF.ValidateOutputFilePath())
      {
        layout.Export(msPDF, MSExport_custom);  //Export the PDF to a single, multiple page PDF. 
      }
      #endregion Layout_ExportMS_PDF


      #region Layout_ExportMS_TIFF
      //Export multiple map series pages to TIFF

      //Create a TIFF export format
      TIFFFormat msTIFF = new TIFFFormat()
      {
        Resolution = 300,
        OutputFileName = filePath,
        ColorMode = ColorMode.TwentyFourBitTrueColor,
        HasGeoTiffTags = true,
        HasWorldFile = true
      };

      //Set up the export options for the map series
      MapSeriesExportOptions MSExport_All = new MapSeriesExportOptions()
      {
        ExportPages = ExportPages.All,
        ExportFileOptions = ExportFileOptions.ExportMultipleNames,
        ShowSelectedSymbology = false
      };

      //Check to see if the path is valid and export
      if (msPDF.ValidateOutputFilePath())
      {
        layout.Export(msPDF, MSExport_All);  //Export each page to a TIFF and apppend the page name suffix to each output file 
      }
      #endregion Layout_ExportMS_TIFF


      // cref: Layout_RefreshMapSeries;ArcGIS.Desktop.Layouts.Layout.RefreshMapSeries
      #region Layout_RefreshMapSeries
      //Refresh the map series associated with the layout.

      //Perform on the worker thread
      await QueuedTask.Run(() =>
      {
        layout.RefreshMapSeries();
      });
      #endregion Layout_RefreshMapSeries


      // cref: Layout_SaveAsFile;ArcGIS.Desktop.Layouts.Layout.SaveAsFile(System.String,System.Boolean)
      #region Layout_SaveAsFile
      //Save a layout to a pagx file.

      //Perform on the worker thread
      await QueuedTask.Run(() =>
      {
        layout.SaveAsFile(filePath);
      });
      #endregion Layout_SaveAsFile


      // cref: Layout_SetName;ArcGIS.Desktop.Layouts.Layout.SetName(System.String)
      #region Layout_SetName
      //Change the name of a layout.

      //Perform on the worker thread
      await QueuedTask.Run(() =>
      {
        layout.SetName("New Name");
      });
      #endregion Layout_SetName


      SpatialMapSeries SMS = null;
      // cref: Layout_SetMapSeries;ArcGIS.Desktop.Layouts.Layout.SetMapSeries(ArcGIS.Desktop.Layouts.MapSeries)
      #region Layout_SetMapSeries
      //Change the properities of a spacial map series.

      //Perform on the worker thread
      await QueuedTask.Run(() =>
      {
        layout.SetMapSeries(SMS);
      });
      #endregion Layout_SetMapSeries


      // cref: Layout_ShowProperties;ArcGIS.Desktop.Layouts.Layout.ShowProperties
      #region Layout_ShowProperties
      //Open the layout properties dialog.

      //Get the layout associated with a layout project item
      LayoutProjectItem lytItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals("Layout Name"));
      Layout lyt = await QueuedTask.Run(() => lytItem.GetLayout());  //Worker thread

      //Open the properties dialog
      lyt.ShowProperties();  //GUI thread
      #endregion Layout_ShowProperties
    }
  }
}
