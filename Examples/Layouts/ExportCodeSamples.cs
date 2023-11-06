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
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Layouts;
using ArcGIS.Desktop.Mapping;

namespace Layout_HelpExamples
{
  public class Export_Examples
  {
    async public static void ExportLayoutToPDF()
    {

      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals("Layout Name"));
      Layout layout = await QueuedTask.Run(() => layoutItem.GetLayout());
      String filePath = null;

      // cref: ArcGIS.Desktop.Layouts.Layout.Export
      // cref: ARCGIS.DESKTOP.MAPPING.PDFFORMAT
      // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT
      // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.VALIDATEOUTPUTFILEPATH
      // cref: ArcGIS.Desktop.Mapping.PDFFormat.ImageCompression
      // cref: ARCGIS.DESKTOP.MAPPING.PDFFORMAT.IMAGEQUALITY
      // cref: ARCGIS.DESKTOP.MAPPING.PDFFORMAT.LAYERSANDATTRIBUTES
      // cref: ARCGIS.DESKTOP.MAPPING.ImageCompression
      // cref: ARCGIS.DESKTOP.MAPPING.ImageQuality
      // cref: ARCGIS.DESKTOP.MAPPING.LayersAndAttributes
      #region Layout_ExportPDF
      //Export a layout to PDF

      //Create a PDF export format
      PDFFormat pdf = new PDFFormat()
      {
        OutputFileName = filePath,
        Resolution = 300,
        DoCompressVectorGraphics = true,
        DoEmbedFonts = true,
        HasGeoRefInfo = true,
        ImageCompression = ImageCompression.Adaptive,
        ImageQuality = ImageQuality.Best,
        LayersAndAttributes = LayersAndAttributes.LayersAndAttributes
    };

      //Check to see if the path is valid and export
      if (pdf.ValidateOutputFilePath())
      {
        await QueuedTask.Run(() => layout.Export(pdf)); //Export the PDF
      }
      #endregion Layout_ExportPDF
    }
  }
  
}


namespace Layout_HelpExamples
{
  // cref: ArcGIS.Desktop.Mapping.BMPFormat
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.VALIDATEOUTPUTFILEPATH
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.Resolution
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.OutputFileName
  // cref: ArcGIS.Desktop.Layouts.Layout.Export
  #region ExportBMP_Layout
  //This example demonstrates how to export a layout to BMP.

  //Added references
  using ArcGIS.Desktop.Core;                         //Project
  using ArcGIS.Desktop.Layouts;                      //Layout classes
  using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask
  using ArcGIS.Desktop.Mapping;                      //Export formats

  public class ExportLayoutToBMPExample
  {
    public static Task ExportLayoutToBMPAsync(string LayoutName, string Path)
    {
      //Reference a layoutitem in a project by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals(LayoutName));
      if (layoutItem == null)
        return Task.FromResult<Layout>(null);

      //Create BMP format with appropriate settings
      BMPFormat BMP = new BMPFormat();
      BMP.Resolution = 300;
      BMP.OutputFileName = Path;

      return QueuedTask.Run(() =>
      {
              //Export Layout
              Layout lyt = layoutItem.GetLayout(); //Loads and returns the layout associated with a LayoutItem
              if (BMP.ValidateOutputFilePath())
        {
          lyt.Export(BMP);
        }
      });
    }
  }
  #endregion ExportBMP_Layout
}

namespace Layout_HelpExamples
{
  // cref: ArcGIS.Desktop.Mapping.BMPFormat
  // cref: ArcGIS.Desktop.Mapping.BMPFormat.HasWorldFile
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.VALIDATEOUTPUTFILEPATH
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.Resolution
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.OutputFileName
  // cref: ArcGIS.Desktop.Layouts.MapFrame.Export
  #region ExportBMP_MapFrame
  //This example demonstrates how to export an individual map frame on a layout to BMP.

  //Added references
  using ArcGIS.Desktop.Core;                         //Project
  using ArcGIS.Desktop.Layouts;                      //Layout classes
  using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask
  using ArcGIS.Desktop.Mapping;                      //Export formats

  public class ExportMapFrameToBMPExample
  {
    public static Task ExportMapFrameToBMPAsync(string LayoutName, string MFName, string Path)
    {
      //Reference a layoutitem in a project by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals(LayoutName));
      if (layoutItem == null)
        return Task.FromResult<Layout>(null);

      //Create BMP format with appropriate settings
      BMPFormat BMP = new BMPFormat();
      BMP.HasWorldFile = true;
      BMP.Resolution = 300;
      BMP.OutputFileName = Path;

      return QueuedTask.Run(() =>
      {
              //Export MapFrame
              Layout lyt = layoutItem.GetLayout(); //Loads and returns the layout associated with a LayoutItem
              MapFrame mf = lyt.FindElement(MFName) as MapFrame;
        BMP.OutputFileName = Path;
        if (BMP.ValidateOutputFilePath())
        {
          mf.Export(BMP);
        }
      });
    }
  }
  #endregion ExportBMP_MapFrame
}

namespace Layout_HelpExamples
{
  // cref: ArcGIS.Desktop.Mapping.BMPFormat
  // cref: ArcGIS.Desktop.Mapping.BMPFormat.HasWorldFile
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.VALIDATEOUTPUTFILEPATH
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.Resolution
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.OutputFileName
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.Height
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.Width
  // cref: ArcGIS.Desktop.Mapping.MapView.Export
  #region ExportBMP_ActiveMap
  //This example demonstrates how to export the active mapview to BMP.

  //Added references
  using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask
  using ArcGIS.Desktop.Mapping;                      //MapView and Export formats

  public class ExportActiveMapToBMPExample
  {
    public static Task ExportActiveMapToBMPAsync(string Path)
    {
      return QueuedTask.Run(() =>
      {
        //Reference the active map view
        MapView map = MapView.Active;

        //Create BMP format with appropriate settings
        BMPFormat BMP = new BMPFormat();
        BMP.Resolution = 300;
        BMP.Height = 500;
        BMP.Width = 800;
        BMP.HasWorldFile = true;
        BMP.OutputFileName = Path;

        //Export active map view
        if (BMP.ValidateOutputFilePath())
        {
          map.Export(BMP);
        }
      });
    }
  }
  #endregion ExportBMP_ActiveMap
}

namespace Layout_HelpExamples
{
  // cref: ArcGIS.Desktop.Mapping.EMFFormat
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.VALIDATEOUTPUTFILEPATH
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.Resolution
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.OutputFileName
  // cref: ArcGIS.Desktop.Layouts.Layout.Export
  #region ExportEMF_Layout
  //This example demonstrates how to export a layout to EMF.

  //Added references
  using ArcGIS.Desktop.Core;                         //Project
  using ArcGIS.Desktop.Layouts;                      //Layout classes
  using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask
  using ArcGIS.Desktop.Mapping;                     //Export formats

  public class ExportLayoutToEMFExample
  {
    public static Task ExportLayoutToEMFAsync(string LayoutName, string Path)
    {
      //Reference a layoutitem in a project by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals(LayoutName));
      if (layoutItem == null)
        return Task.FromResult<Layout>(null);

      //Create EMF format with appropriate settings
      EMFFormat EMF = new EMFFormat();
      EMF.Resolution = 300;
      EMF.OutputFileName = Path;

      return QueuedTask.Run(() =>
      {
              //Export Layout
              Layout lyt = layoutItem.GetLayout(); //Loads and returns the layout associated with a LayoutItem
              if (EMF.ValidateOutputFilePath())
        {
          lyt.Export(EMF);
        }
      });
    }
  }
  #endregion ExportEMF_Layout
}

namespace Layout_HelpExamples
{

  // cref: ArcGIS.Desktop.Mapping.EMFFormat
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.VALIDATEOUTPUTFILEPATH
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.Resolution
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.OutputFileName
  // cref: ArcGIS.Desktop.Layouts.MapFrame.Export
  #region ExportEMF_MapFrame
  //This example demonstrates how to export an individual map frame on a layout to EMF.

  //Added references
  using ArcGIS.Desktop.Core;                         //Project
  using ArcGIS.Desktop.Layouts;                      //Layout classes
  using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask
  using ArcGIS.Desktop.Mapping;                      //Export formats

  public class ExportMapFrameToEMFExample
  {
    public static Task ExportMapFrameToEMFAsync(string LayoutName, string MFName, string Path)
    {
      //Reference a layoutitem in a project by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals(LayoutName));
      if (layoutItem == null)
        return Task.FromResult<Layout>(null);

      //Create EMF format with appropriate settings
      EMFFormat EMF = new EMFFormat();
      EMF.Resolution = 300;
      EMF.OutputFileName = Path;

      return QueuedTask.Run(() =>
      {
              //Export MapFrame
              Layout lyt = layoutItem.GetLayout(); //Loads and returns the layout associated with a LayoutItem
              MapFrame mf = lyt.FindElement(MFName) as MapFrame;
        EMF.OutputFileName = Path;
        if (EMF.ValidateOutputFilePath())
        {
          mf.Export(EMF);
        }
      });
    }
  }
  #endregion ExportEMF_MapFrame
}

namespace Layout_HelpExamples
{
  // cref: ArcGIS.Desktop.Mapping.EMFFormat
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.VALIDATEOUTPUTFILEPATH
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.Resolution
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.OutputFileName
  // cref: ArcGIS.Desktop.Mapping.MapView.Export
  #region ExportEMF_ActiveMap
  //This example demonstrates how to export the active mapview to EMF.

  //Added references
  using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask
  using ArcGIS.Desktop.Mapping;                      //MapView and Export formats

  public class ExportActiveMapToEMFExample
  {
    public static Task ExportActiveMapToEMFAsync(string Path)
    {
      return QueuedTask.Run(() =>
      {
              //Reference the active map view
              MapView map = MapView.Active;

              //Create EMF format with appropriate settings
              EMFFormat EMF = new EMFFormat();
        EMF.Resolution = 300;
        EMF.Height = 500;
        EMF.Width = 800;
        EMF.OutputFileName = Path;

              //Export active map view
              if (EMF.ValidateOutputFilePath())
        {
          map.Export(EMF);
        }
      });
    }
  }
  #endregion ExportEMF_ActiveMap
}

namespace Layout_HelpExamples
{
  // cref: ArcGIS.Desktop.Mapping.EPSFormat
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.VALIDATEOUTPUTFILEPATH
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.Resolution
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.OutputFileName
  // cref: ArcGIS.Desktop.Layouts.Layout.Export
  #region ExportEPS_Layout
  //This example demonstrates how to export a layout to EPS.

  //Added references
  using ArcGIS.Desktop.Core;                         //Project
  using ArcGIS.Desktop.Layouts;                      //Layout classes
  using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask
  using ArcGIS.Desktop.Mapping;                      //Export formats

  public class ExportLayoutToEPSExample
  {
    public static Task ExportLayoutToEPSAsync(string LayoutName, string Path)
    {
      //Reference a layoutitem in a project by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals(LayoutName));
      if (layoutItem == null)
        return Task.FromResult<Layout>(null);

      //Create EPS format with appropriate settings
      EPSFormat EPS = new EPSFormat();
      EPS.Resolution = 300;
      EPS.OutputFileName = Path;

      return QueuedTask.Run(() =>
      {
              //Export Layout
              Layout lyt = layoutItem.GetLayout(); //Loads and returns the layout associated with a LayoutItem
              if (EPS.ValidateOutputFilePath())
        {
          lyt.Export(EPS);
        }
      });
    }
  }
  #endregion ExportEPS_Layout
}

namespace Layout_HelpExamples
{
  // cref: ArcGIS.Desktop.Mapping.EPSFormat
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.VALIDATEOUTPUTFILEPATH
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.Resolution
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.OutputFileName
  // cref: ArcGIS.Desktop.Layouts.MapFrame.Export
  #region ExportEPS_MapFrame
  //This example demonstrates how to export an individual map frame on a layout to EPS.

  //Added references
  using ArcGIS.Desktop.Core;                         //Project
  using ArcGIS.Desktop.Layouts;                      //Layout classes
  using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask
  using ArcGIS.Desktop.Mapping;                      //Export formats

  public class ExportMapFrameToEPSExample
  {
    public static Task ExportMapFrameToEPSAsync(string LayoutName, string MFName, string Path)
    {
      //Reference a layoutitem in a project by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals(LayoutName));
      if (layoutItem == null)
        return Task.FromResult<Layout>(null);

      //Create EPS format with appropriate settings
      EMFFormat EPS = new EMFFormat();
      EPS.Resolution = 300;
      EPS.OutputFileName = Path;

      return QueuedTask.Run(() =>
      {
              //Export MapFrame
              Layout lyt = layoutItem.GetLayout(); //Loads and returns the layout associated with a LayoutItem
              MapFrame mf = lyt.FindElement(MFName) as MapFrame;
        EPS.OutputFileName = Path;
        if (EPS.ValidateOutputFilePath())
        {
          mf.Export(EPS);
        }
      });
    }
  }
  #endregion ExportEMF_MapFrame
}

namespace Layout_HelpExamples
{
  // cref: ArcGIS.Desktop.Mapping.EPSFormat
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.VALIDATEOUTPUTFILEPATH
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.Resolution
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.OutputFileName
  // cref: ArcGIS.Desktop.Mapping.MapView.Export
  #region ExportEPS_ActiveMap
  //This example demonstrates how to export the active mapview to EPS.

  //Added references
  using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask
  using ArcGIS.Desktop.Mapping;                      //MapView and Export formats

  public class ExportActiveMapToEPSExample
  {
    public static Task ExportActiveMapToEPSAsync(string Path)
    {
      return QueuedTask.Run(() =>
      {
              //Reference the active map view
              MapView mapv = MapView.Active;

              //Create EMF format with appropriate settings
              EPSFormat EPS = new EPSFormat();
        EPS.Resolution = 300;
        EPS.Height = 500;
        EPS.Width = 800;
        EPS.OutputFileName = Path;

              //Export active map view
              if (EPS.ValidateOutputFilePath())
        {
          mapv.Export(EPS);
        }
      });
    }
  }
  #endregion ExportEPS_ActiveMap
}

namespace Layout_HelpExamples
{
  // cref: ArcGIS.Desktop.Mapping.GIFFormat
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.VALIDATEOUTPUTFILEPATH
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.Resolution
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.OutputFileName
  // cref: ArcGIS.Desktop.Layouts.Layout.Export
  #region ExportGIF_Layout
  //This example demonstrates how to export a layout to GIF.

  //Added references
  using ArcGIS.Desktop.Core;                         //Project
  using ArcGIS.Desktop.Layouts;                      //Layout classes
  using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask
  using ArcGIS.Desktop.Mapping;                      //Export formats

  public class ExportLayoutToGIFExample
  {
    public static Task ExportLayoutToGIFAsync(string LayoutName, string Path)
    {
      //Reference a layoutitem in a project by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals(LayoutName));
      if (layoutItem == null)
        return Task.FromResult<Layout>(null);

      //Create GIF format with appropriate settings
      GIFFormat GIF = new GIFFormat();
      GIF.Resolution = 300;
      GIF.OutputFileName = Path;

      return QueuedTask.Run(() =>
      {
              //Export Layout
              Layout lyt = layoutItem.GetLayout(); //Loads and returns the layout associated with a LayoutItem
              if (GIF.ValidateOutputFilePath())
        {
          lyt.Export(GIF);
        }
      });
    }
  }
  #endregion ExportGIF_Layout
}

namespace Layout_HelpExamples
{
  // cref: ArcGIS.Desktop.Mapping.GIFFormat
  // cref: ArcGIS.Desktop.Mapping.GIFFormat.HasWorldFile
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.VALIDATEOUTPUTFILEPATH
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.Resolution
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.OutputFileName
  // cref: ArcGIS.Desktop.Layouts.MapFrame.Export
  #region ExportGIF_MapFrame
  //This example demonstrates how to export an individual map frame on a layout to GIF.

  //Added references
  using ArcGIS.Desktop.Core;                         //Project
  using ArcGIS.Desktop.Layouts;                      //Layout classes
  using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask
  using ArcGIS.Desktop.Mapping;                      //Export formats

  public class ExportMapFrameToGIFExample
  {
    public static Task ExportMapFrameToGIFAsync(string LayoutName, string MFName, string Path)
    {
      //Reference a layoutitem in a project by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals(LayoutName));
      if (layoutItem == null)
        return Task.FromResult<Layout>(null);

      //Create GIF format with appropriate settings
      GIFFormat GIF = new GIFFormat();
      GIF.HasWorldFile = true;
      GIF.Resolution = 300;
      GIF.OutputFileName = Path;

      return QueuedTask.Run(() =>
      {
              //Export MapFrame
              Layout lyt = layoutItem.GetLayout(); //Loads and returns the layout associated with a LayoutItem
              MapFrame mf = lyt.FindElement(MFName) as MapFrame;
        GIF.OutputFileName = Path;
        if (GIF.ValidateOutputFilePath())
        {
          mf.Export(GIF);
        }
      });
    }
  }
  #endregion ExportGIF_MapFrame
}

namespace Layout_HelpExamples
{
  // cref: ArcGIS.Desktop.Mapping.GIFFormat
  // cref: ArcGIS.Desktop.Mapping.GIFFormat.HasWorldFile
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.Height
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.Width
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.VALIDATEOUTPUTFILEPATH
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.Resolution
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.OutputFileName
  // cref: ArcGIS.Desktop.Mapping.MapView.Export
  #region ExportGIF_ActiveMap
  //This example demonstrates how to export the active mapview to GIF.

  //Added references
  using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask
  using ArcGIS.Desktop.Mapping;                      //MapView and Export formats

  public class ExportActiveMapToGIFExample
  {
    public static Task ExportActiveMapToGIFAsync(string Path)
    {
      return QueuedTask.Run(() =>
      {
              //Reference the active map view
              MapView mapv = MapView.Active;

              //Create GIF format with appropriate settings
              GIFFormat GIF = new GIFFormat();
        GIF.Resolution = 300;
        GIF.Height = 500;
        GIF.Width = 800;
        GIF.HasWorldFile = true;
        GIF.OutputFileName = Path;

              //Export active map view
              if (GIF.ValidateOutputFilePath())
        {
          mapv.Export(GIF);
        }
      });
    }
  }
  #endregion ExportGIF_ActiveMap
}

namespace Layout_HelpExamples
{
  // cref: ArcGIS.Desktop.Mapping.JPEGFormat
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.VALIDATEOUTPUTFILEPATH
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.Resolution
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.OutputFileName
  // cref: ArcGIS.Desktop.Layouts.Layout.Export
  #region ExportJPEG_Layout
  //This example demonstrates how to export a layout to JPEG.

  //Added references
  using ArcGIS.Desktop.Core;                         //Project
  using ArcGIS.Desktop.Layouts;                      //Layout classes
  using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask
  using ArcGIS.Desktop.Mapping;                      //Export formats

  public class ExportLayoutToJPEGExample
  {
    public static Task ExportLayoutToJPEGAsync(string LayoutName, string Path)
    {
      //Reference a layoutitem in a project by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals(LayoutName));
      if (layoutItem == null)
        return Task.FromResult<Layout>(null);

      //Create JPEG format with appropriate settings
      JPEGFormat JPEG = new JPEGFormat();
      JPEG.Resolution = 300;
      JPEG.OutputFileName = Path;

      return QueuedTask.Run(() =>
      {
              //Export Layout
              Layout lyt = layoutItem.GetLayout(); //Loads and returns the layout associated with a LayoutItem
              if (JPEG.ValidateOutputFilePath())
        {
          lyt.Export(JPEG);
        }
      });
    }
  }
  #endregion ExportJPEG_Layout
}

namespace Layout_HelpExamples
{
  // cref: ArcGIS.Desktop.Mapping.JPEGFormat
  // cref: ArcGIS.Desktop.Mapping.JPEGFormat.HasWorldFile
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.VALIDATEOUTPUTFILEPATH
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.Resolution
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.OutputFileName
  // cref: ArcGIS.Desktop.Layouts.MapFrame.Export
  #region ExportJPEG_MapFrame
  //This example demonstrates how to export an individual map frame on a layout to JPEG.

  //Added references
  using ArcGIS.Desktop.Core;                         //Project
  using ArcGIS.Desktop.Layouts;                      //Layout classes
  using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask
  using ArcGIS.Desktop.Mapping;                      //Export formats

  public class ExportMapFrameToJPEGExample
  {
    public static Task ExportMapFrameToJPEGAsync(string LayoutName, string MFName, string Path)
    {
      //Reference a layoutitem in a project by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals(LayoutName));
      if (layoutItem == null)
        return Task.FromResult<Layout>(null);

      //Create JPEG format with appropriate settings
      JPEGFormat JPEG = new JPEGFormat();
      JPEG.HasWorldFile = true;
      JPEG.Resolution = 300;
      JPEG.OutputFileName = Path;

      return QueuedTask.Run(() =>
      {
              //Export MapFrame
              Layout lyt = layoutItem.GetLayout(); //Loads and returns the layout associated with a LayoutItem
              MapFrame mf = lyt.FindElement(MFName) as MapFrame;
        if (JPEG.ValidateOutputFilePath())
        {
          mf.Export(JPEG);
        }
      });
    }
  }
  #endregion ExportJPEG_MapFrame
}

namespace Layout_HelpExamples
{
  // cref: ArcGIS.Desktop.Mapping.JPEGFormat
  // cref: ArcGIS.Desktop.Mapping.JPEGFormat.HasWorldFile
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.VALIDATEOUTPUTFILEPATH
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.Resolution
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.OutputFileName
  // cref: ArcGIS.Desktop.Mapping.MapView.Export
  #region ExportJPEG_ActiveMap
  //This example demonstrates how to export the active mapview to JPEG.

  //Added references
  using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask
  using ArcGIS.Desktop.Mapping;                      //MapView and Export formats

  public class ExportActiveMapToJPEGExample
  {
    public static Task ExportActiveMapToJPEGAsync(string Path)
    {
      return QueuedTask.Run(() =>
      {
              //Reference the active map view
              MapView mapv = MapView.Active;

              //Create JPEG format with appropriate settings
              JPEGFormat JPEG = new JPEGFormat();
        JPEG.Resolution = 300;
        JPEG.Height = 500;
        JPEG.Width = 800;
        JPEG.HasWorldFile = true;
        JPEG.OutputFileName = Path;

              //Export active map view
              if (JPEG.ValidateOutputFilePath())
        {
          mapv.Export(JPEG);
        }
      });
    }
  }
  #endregion ExportJPEG_ActiveMap
}

namespace Layout_HelpExamples
{
  // cref: ArcGIS.Desktop.Mapping.PDFFormat
  // cref: ArcGIS.Desktop.Mapping.PDFFormat.Password
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.VALIDATEOUTPUTFILEPATH
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.Resolution
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.OutputFileName
  // cref: ArcGIS.Desktop.Layouts.Layout.Export
  #region ExportPDF_Layout
  //This example demonstrates how to export a layout to PDF.

  //Added references
  using ArcGIS.Desktop.Core;                         //Project
  using ArcGIS.Desktop.Layouts;                      //Layout classes
  using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask
  using ArcGIS.Desktop.Mapping;                      //Export formats

  public class ExportLayoutToPDFExample
  {
    public static Task ExportLayoutToPDFAsync(string LayoutName, string Path)
    {
      //Reference a layoutitem in a project by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals(LayoutName));
      if (layoutItem == null)
        return Task.FromResult<Layout>(null);

      //Create PDF format with appropriate settings
      PDFFormat PDF = new PDFFormat();
      PDF.Resolution = 300;
      PDF.OutputFileName = Path;
      //PDF.Password = "xxx";

      return QueuedTask.Run(() =>
      {
              //Export Layout
              Layout lyt = layoutItem.GetLayout(); //Loads and returns the layout associated with a LayoutItem
              if (PDF.ValidateOutputFilePath())
        {
          lyt.Export(PDF);
        }
      });
    }
  }
  #endregion ExportPDF_Layout
}

namespace Layout_HelpExamples
{
  // cref: ArcGIS.Desktop.Mapping.PDFFormat
  // cref: ArcGIS.Desktop.Mapping.PDFFormat.Password
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.VALIDATEOUTPUTFILEPATH
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.Resolution
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.OutputFileName
  // cref: ArcGIS.Desktop.Layouts.MapFrame.Export
  #region ExportPDF_MapFrame
  //This example demonstrates how to export an individual map frame on a layout to PDF.

  //Added references
  using ArcGIS.Desktop.Core;                         //Project
  using ArcGIS.Desktop.Layouts;                      //Layout classes
  using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask
  using ArcGIS.Desktop.Mapping;                      //Export formats

  public class ExportMapFrameToPDFExample
  {
    public static Task ExportMapFrameToPDFAsync(string LayoutName, string MFName, string Path)
    {
      //Reference a layoutitem in a project by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals(LayoutName));
      if (layoutItem == null)
        return Task.FromResult<Layout>(null);

      //Create PDF format with appropriate settings
      PDFFormat PDF = new PDFFormat();
      PDF.Resolution = 300;
      PDF.OutputFileName = Path;
      //PDF.Password = "xxx";

      return QueuedTask.Run(() =>
      {
              //Export MapFrame
              Layout lyt = layoutItem.GetLayout(); //Loads and returns the layout associated with a LayoutItem
              MapFrame mf = lyt.FindElement(MFName) as MapFrame;
        PDF.OutputFileName = Path;
        if (PDF.ValidateOutputFilePath())
        {
          mf.Export(PDF);
        }
      });
    }
  }
  #endregion ExportPDF_MapFrame
}

namespace Layout_HelpExamples
{
  // cref: ArcGIS.Desktop.Mapping.PDFFormat
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.VALIDATEOUTPUTFILEPATH
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.Resolution
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.OutputFileName
  // cref: ArcGIS.Desktop.Mapping.MapView.Export
  #region ExportPDF_ActiveMap
  //This example demonstrates how to export the active mapview to PDF.

  //Added references
  using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask
  using ArcGIS.Desktop.Mapping;                      //MapView and Export formats

  public class ExportActiveMapToPDFExample
  {
    public static Task ExportActiveMapToPDFAsync(string Path)
    {
      return QueuedTask.Run(() =>
      {
              //Reference the active map view
              MapView mapv = MapView.Active;

              //Create PDF format with appropriate settings
              PDFFormat PDF = new PDFFormat();
        PDF.Resolution = 300;
        PDF.Height = 500;
        PDF.Width = 800;
        PDF.OutputFileName = Path;

              //Export active map view
              if (PDF.ValidateOutputFilePath())
        {
          mapv.Export(PDF);
        }
      });
    }
  }
  #endregion ExportPDF_ActiveMap
}

namespace Layout_HelpExamples
{
  // cref: ArcGIS.Desktop.Mapping.PNGFormat
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.VALIDATEOUTPUTFILEPATH
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.Resolution
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.OutputFileName
  // cref: ArcGIS.Desktop.Layouts.Layout.Export
  #region ExportPNG_Layout
  //This example demonstrates how to export a layout to PNG.

  //Added references
  using ArcGIS.Desktop.Core;                         //Project
  using ArcGIS.Desktop.Layouts;                      //Layout classes
  using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask
  using ArcGIS.Desktop.Mapping;                      //Export formats

  public class ExportLayoutToPNGExample
  {
    public static Task ExportLayoutToPNGAsync(string LayoutName, string Path)
    {
      //Reference a layoutitem in a project by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals(LayoutName));
      if (layoutItem == null)
        return Task.FromResult<Layout>(null);

      //Create PNG format with appropriate settings
      PNGFormat PNG = new PNGFormat();
      PNG.Resolution = 300;
      PNG.OutputFileName = Path;

      return QueuedTask.Run(() =>
      {
              //Export Layout
              Layout lyt = layoutItem.GetLayout(); //Loads and returns the layout associated with a LayoutItem
              if (PNG.ValidateOutputFilePath())
        {
          lyt.Export(PNG);
        }
      });
    }
  }
  #endregion ExportPNG_Layout
}

namespace Layout_HelpExamples
{
  // cref: ArcGIS.Desktop.Mapping.PNGFormat
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.VALIDATEOUTPUTFILEPATH
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.Resolution
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.OutputFileName
  // cref: ArcGIS.Desktop.Layouts.MapFrame.Export
  #region ExportPNG_MapFrame
  //This example demonstrates how to export an individual map frame on a layout to PNG.

  //Added references
  using ArcGIS.Desktop.Core;                         //Project
  using ArcGIS.Desktop.Layouts;                      //Layout classes
  using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask
  using ArcGIS.Desktop.Mapping;                      //Export formats

  public class ExportMapFrameToPNGExample
  {
    public static Task ExportMapFrameToPNGAsync(string LayoutName, string MFName, string Path)
    {
      //Reference a layoutitem in a project by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals(LayoutName));
      if (layoutItem == null)
        return Task.FromResult<Layout>(null);

      //Create PNG format with appropriate settings
      PNGFormat PNG = new PNGFormat();
      PNG.Resolution = 300;
      PNG.OutputFileName = Path;

      return QueuedTask.Run(() =>
      {
              //Export MapFrame
              Layout lyt = layoutItem.GetLayout(); //Loads and returns the layout associated with a LayoutItem
              MapFrame mf = lyt.FindElement(MFName) as MapFrame;
        PNG.OutputFileName = Path;
        if (PNG.ValidateOutputFilePath())
        {
          mf.Export(PNG);
        }
      });
    }
  }
  #endregion ExportPNG_MapFrame
}

namespace Layout_HelpExamples
{
  // cref: ArcGIS.Desktop.Mapping.PNGFormat
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.VALIDATEOUTPUTFILEPATH
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.Resolution
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.OutputFileName
  // cref: ArcGIS.Desktop.Mapping.MapView.Export
  #region ExportPNG_ActiveMap
  //This example demonstrates how to export the active mapview to PNG.

  //Added references
  using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask
  using ArcGIS.Desktop.Mapping;                      //MapView and Export formats

  public class ExportActiveMapToPNGExample
  {
    public static Task ExportActiveMapToPNGAsync(string Path)
    {
      return QueuedTask.Run(() =>
      {
              //Reference the active map view
              MapView mapv = MapView.Active;

              //Create PNG format with appropriate settings
              PNGFormat PNG = new PNGFormat();
        PNG.Resolution = 300;
        PNG.Height = 500;
        PNG.Width = 800;
        PNG.OutputFileName = Path;

              //Export active map view
              if (PNG.ValidateOutputFilePath())
        {
          mapv.Export(PNG);
        }
      });
    }
  }
  #endregion ExportPNG_ActiveMap
}

namespace Layout_HelpExamples
{
  // cref: ArcGIS.Desktop.Mapping.SVGFormat
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.VALIDATEOUTPUTFILEPATH
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.Resolution
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.OutputFileName
  // cref: ArcGIS.Desktop.Layouts.Layout.Export
  #region ExportSVG_Layout
  //This example demonstrates how to export a layout to SVG.

  //Added references
  using ArcGIS.Desktop.Core;                         //Project
  using ArcGIS.Desktop.Layouts;                      //Layout classes
  using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask
  using ArcGIS.Desktop.Mapping;               //Export formats

  public class ExportLayoutToSVGExample
  {
    public static Task ExportLayoutToSVGAsync(string LayoutName, string Path)
    {
      //Reference a layoutitem in a project by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals(LayoutName));
      if (layoutItem == null)
        return Task.FromResult<Layout>(null);

      //Create SVG format with appropriate settings
      SVGFormat SVG = new SVGFormat();
      SVG.Resolution = 300;
      SVG.OutputFileName = Path;

      return QueuedTask.Run(() =>
      {
              //Export Layout
              Layout lyt = layoutItem.GetLayout(); //Loads and returns the layout associated with a LayoutItem
              if (SVG.ValidateOutputFilePath())
        {
          lyt.Export(SVG);
        }
      });
    }
  }
  #endregion ExportSVG_Layout
}

namespace Layout_HelpExamples
{
  // cref: ArcGIS.Desktop.Mapping.SVGFormat
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.VALIDATEOUTPUTFILEPATH
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.Resolution
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.OutputFileName
  // cref: ArcGIS.Desktop.Layouts.MapFrame.Export
  #region ExportSVG_MapFrame
  //This example demonstrates how to export an individual map frame on a layout to SVG.

  //Added references
  using ArcGIS.Desktop.Core;                         //Project
  using ArcGIS.Desktop.Layouts;                      //Layout classes
  using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask
  using ArcGIS.Desktop.Mapping;                      //Export formats

  public class ExportMapFrameToSVGExample
  {
    public static Task ExportMapFrameToSVGAsync(string LayoutName, string MFName, string Path)
    {
      //Reference a layoutitem in a project by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals(LayoutName));
      if (layoutItem == null)
        return Task.FromResult<Layout>(null);

      //Create SVG format with appropriate settings
      SVGFormat SVG = new SVGFormat();
      SVG.Resolution = 300;
      SVG.OutputFileName = Path;

      return QueuedTask.Run(() =>
      {
              //Export MapFrame
              Layout lyt = layoutItem.GetLayout(); //Loads and returns the layout associated with a LayoutItem
              MapFrame mf = lyt.FindElement(MFName) as MapFrame;
        SVG.OutputFileName = Path;
        if (SVG.ValidateOutputFilePath())
        {
          mf.Export(SVG);
        }
      });
    }
  }
  #endregion ExportSVG_MapFrame
}

namespace Layout_HelpExamples
{
  // cref: ArcGIS.Desktop.Mapping.SVGFormat
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.VALIDATEOUTPUTFILEPATH
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.Resolution
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.OutputFileName
  // cref: ArcGIS.Desktop.Mapping.MapView.Export
  #region ExportSVG_ActiveMap
  //This example demonstrates how to export the active mapview to SVG.

  //Added references
  using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask
  using ArcGIS.Desktop.Mapping;                      //MapView and  Export formats

  public class ExportActiveMapToSVGExample
  {
    public static Task ExportActiveMapToSVGAsync(string Path)
    {
      return QueuedTask.Run(() =>
      {
              //Reference the active map view
              MapView mapv = MapView.Active;

              //Create SVG format with appropriate settings
              SVGFormat SVG = new SVGFormat();
        SVG.Resolution = 300;
        SVG.Height = 500;
        SVG.Width = 800;
        SVG.OutputFileName = Path;

              //Export active map view
              if (SVG.ValidateOutputFilePath())
        {
          mapv.Export(SVG);
        }
      });
    }
  }
  #endregion ExportSVG_ActiveMap
}

namespace Layout_HelpExamples
{
  // cref: ArcGIS.Desktop.Mapping.TGAFormat
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.VALIDATEOUTPUTFILEPATH
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.Resolution
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.OutputFileName
  // cref: ArcGIS.Desktop.Layouts.Layout.Export
  #region ExportTGA_Layout
  //This example demonstrates how to export a layout to TGA.

  //Added references
  using ArcGIS.Desktop.Core;                         //Project
  using ArcGIS.Desktop.Layouts;                      //Layout classes
  using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask
  using ArcGIS.Desktop.Mapping;                      //Export formats

  public class ExportLayoutToTGAExample
  {
    public static Task ExportLayoutToTGAAsync(string LayoutName, string Path)
    {
      //Reference a layoutitem in a project by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals(LayoutName));
      if (layoutItem == null)
        return Task.FromResult<Layout>(null);

      //Create TGA format with appropriate settings
      TGAFormat TGA = new TGAFormat();
      TGA.Resolution = 300;
      TGA.OutputFileName = Path;

      return QueuedTask.Run(() =>
      {
              //Export Layout
              Layout lyt = layoutItem.GetLayout(); //Loads and returns the layout associated with a LayoutItem
              if (TGA.ValidateOutputFilePath())
        {
          lyt.Export(TGA);
        }
      });
    }
  }
  #endregion ExportTGA_Layout
}

namespace Layout_HelpExamples
{
  // cref: ArcGIS.Desktop.Mapping.TGAFormat
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.VALIDATEOUTPUTFILEPATH
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.Resolution
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.OutputFileName
  // cref: ArcGIS.Desktop.Layouts.MapFrame.Export
  #region ExportTGA_MapFrame
  //This example demonstrates how to export an individual map frame on a layout to TGA.

  //Added references
  using ArcGIS.Desktop.Core;                         //Project
  using ArcGIS.Desktop.Layouts;                      //Layout classes
  using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask
  using ArcGIS.Desktop.Mapping;                      //Export formats

  public class ExportMapFrameToTGAExample
  {
    public static Task ExportMapFrameToTGAAsync(string LayoutName, string MFName, string Path)
    {
      //Reference a layoutitem in a project by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals(LayoutName));
      if (layoutItem == null)
        return Task.FromResult<Layout>(null);

      //Create TGA format with appropriate settings
      TGAFormat TGA = new TGAFormat();
      TGA.Resolution = 300;
      TGA.OutputFileName = Path;

      return QueuedTask.Run(() =>
      {
              //Export MapFrame
              Layout lyt = layoutItem.GetLayout(); //Loads and returns the layout associated with a LayoutItem
              MapFrame mf = lyt.FindElement(MFName) as MapFrame;
        TGA.OutputFileName = Path;
        if (TGA.ValidateOutputFilePath())
        {
          mf.Export(TGA);
        }
      });
    }
  }
  #endregion ExportTGA_MapFrame
}

namespace Layout_HelpExamples
{
  // cref: ArcGIS.Desktop.Mapping.TGAFormat
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.VALIDATEOUTPUTFILEPATH
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.Resolution
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.OutputFileName
  // cref: ArcGIS.Desktop.Mapping.MapView.Export
  #region ExportTGA_ActiveMap
  //This example demonstrates how to export the active mapview to TGA.

  //Added references
  using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask
  using ArcGIS.Desktop.Mapping;                      //MapView and Export formats

  public class ExportActiveMapToTGAExample
  {
    public static Task ExportActiveMapToTGAAsync(string Path)
    {
      return QueuedTask.Run(() =>
      {
              //Reference the active map view
              MapView mapv = MapView.Active;

              //Create TGA format with appropriate settings
              TGAFormat TGA = new TGAFormat();
        TGA.Resolution = 300;
        TGA.Height = 500;
        TGA.Width = 800;
        TGA.OutputFileName = Path;

              //Export active map view
              if (TGA.ValidateOutputFilePath())
        {
          mapv.Export(TGA);
        }
      });
    }
  }
  #endregion ExportTGA_ActiveMap
}

namespace Layout_HelpExamples
{
  // cref: ArcGIS.Desktop.Mapping.TIFFFormat
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.VALIDATEOUTPUTFILEPATH
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.Resolution
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.OutputFileName
  // cref: ArcGIS.Desktop.Layouts.Layout.Export
  #region ExportTIFF_Layout
  //This example demonstrates how to export a layout to TIFF.

  //Added references
  using ArcGIS.Desktop.Core;                         //Project
  using ArcGIS.Desktop.Layouts;                      //Layout classes
  using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask
  using ArcGIS.Desktop.Mapping;                      //Export formats

  public class ExportLayoutToTIFFExample
  {
    public static Task ExportLayoutToTIFFAsync(string LayoutName, string Path)
    {
      //Reference a layoutitem in a project by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals(LayoutName));
      if (layoutItem == null)
        return Task.FromResult<Layout>(null);

      //Create TIFF format with appropriate settings
      TIFFFormat TIFF = new TIFFFormat();
      TIFF.Resolution = 300;
      TIFF.OutputFileName = Path;

      return QueuedTask.Run(() =>
      {
              //Export Layout
              Layout lyt = layoutItem.GetLayout(); //Loads and returns the layout associated with a LayoutItem
              if (TIFF.ValidateOutputFilePath())
        {
          lyt.Export(TIFF);
        }
      });
    }
  }
  #endregion ExportTIFF_Layout
}

namespace Layout_HelpExamples
{
  // cref: ArcGIS.Desktop.Mapping.TIFFFormat
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.VALIDATEOUTPUTFILEPATH
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.Resolution
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.OutputFileName
  // cref: ArcGIS.Desktop.Layouts.MapFrame.Export
  #region ExportTIFF_MapFrame
  //This example demonstrates how to export an individual map frame on a layout to TIFF.

  //Added references
  using ArcGIS.Desktop.Core;                         //Project
  using ArcGIS.Desktop.Layouts;                      //Layout classes
  using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask
  using ArcGIS.Desktop.Mapping;                      //Export formats

  public class ExportMapFrameToTIFFExample
  {
    public static Task ExportMapFrameToTIFFAsync(string LayoutName, string MFName, string Path)
    {
      //Reference a layoutitem in a project by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals(LayoutName));
      if (layoutItem == null)
        return Task.FromResult<Layout>(null);

      //Create TIFF format with appropriate settings
      TIFFFormat TIFF = new TIFFFormat();
      TIFF.Resolution = 300;
      TIFF.OutputFileName = Path;

      return QueuedTask.Run(() =>
      {
              //Export MapFrame
              Layout lyt = layoutItem.GetLayout(); //Loads and returns the layout associated with a LayoutItem
              MapFrame mf = lyt.FindElement(MFName) as MapFrame;
        TIFF.OutputFileName = Path;
        if (TIFF.ValidateOutputFilePath())
        {
          mf.Export(TIFF);
        }
      });
    }
  }
  #endregion ExportTIFF_MapFrame
}

namespace Layout_HelpExamples
{
  // cref: ArcGIS.Desktop.Mapping.TIFFFormat
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.VALIDATEOUTPUTFILEPATH
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.Resolution
  // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.OutputFileName
  // cref: ArcGIS.Desktop.Mapping.MapView.Export
  #region ExportTIFF_ActiveMap
  //This example demonstrates how to export the active mapview to TIFF.

  //Added references
  using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask
  using ArcGIS.Desktop.Mapping;                      //MapView and Export formats

  public class ExportActiveMapToTIFFExample
  {
    public static Task ExportActiveMapToTIFFAsync(string Path)
    {
      return QueuedTask.Run(() =>
      {
              //Reference the active map view
              MapView mapv = MapView.Active;

              //Create TIFF format with appropriate settings
              TIFFFormat TIFF = new TIFFFormat();
        TIFF.Resolution = 300;
        TIFF.Height = 500;
        TIFF.Width = 800;
        TIFF.OutputFileName = Path;

              //Export active map view
              if (TIFF.ValidateOutputFilePath())
        {
          mapv.Export(TIFF);
        }
      });
    }
  }
  #endregion ExportTIFF_ActiveMap
}

namespace Layout_HelpExamples
{

  internal class ExportCodeSamples_button1 : Button  //Export to BMP
  {
    async protected override void OnClick()
    {
      await ExportLayoutToBMPExample.ExportLayoutToBMPAsync("Layout Name", @"C:\Temp\BMP_Layout.bmp");
      await ExportMapFrameToBMPExample.ExportMapFrameToBMPAsync("Layout Name", "Map1 Map Frame", @"C:\Temp\BMP_MapFrame.bmp");
      await ExportActiveMapToBMPExample.ExportActiveMapToBMPAsync(@"C:\Temp\BMP_ActiveMap.bmp");
    }
  }

  internal class ExportCodeSamples_button2 : Button  //Export to EMF
  {
    async protected override void OnClick()
    {
      await ExportLayoutToEMFExample.ExportLayoutToEMFAsync("Layout Name", @"C:\Temp\EMF_Layout.emf");
      await ExportMapFrameToEMFExample.ExportMapFrameToEMFAsync("Layout Name", "Map1 Map Frame", @"C:\Temp\EMF_MapFrame.emf");
      await ExportActiveMapToEMFExample.ExportActiveMapToEMFAsync(@"C:\Temp\EMF_ActiveMap.emf");
    }
  }

  internal class ExportCodeSamples_button3 : Button //Export to EPS
  {
    async protected override void OnClick()
    {
      await ExportLayoutToEPSExample.ExportLayoutToEPSAsync("Layout Name", @"C:\Temp\EPS_Layout.eps");
      await ExportMapFrameToEPSExample.ExportMapFrameToEPSAsync("Layout Name", "Map1 Map Frame", @"C:\Temp\EPS_MapFrame.eps");
      await ExportActiveMapToEPSExample.ExportActiveMapToEPSAsync(@"C:\Temp\EPS_ActiveMap.eps");
    }
  }

  internal class ExportCodeSamples_button4 : Button //Export to GIF
  {
    async protected override void OnClick()
    {
      await ExportLayoutToGIFExample.ExportLayoutToGIFAsync("Layout Name", @"C:\Temp\GIF_Layout.gif");
      await ExportMapFrameToGIFExample.ExportMapFrameToGIFAsync("Layout Name", "Map1 Map Frame", @"C:\Temp\GIF_MapFrame.gif");
      await ExportActiveMapToGIFExample.ExportActiveMapToGIFAsync(@"C:\Temp\GIF_ActiveMap.gif");
    }
  }

  internal class ExportCodeSamples_button5 : Button  //Export to JPEG
  {
    async protected override void OnClick()
    {
      await ExportLayoutToJPEGExample.ExportLayoutToJPEGAsync("Layout Name", @"C:\Temp\JPEG_Layout.jpg");
      await ExportMapFrameToJPEGExample.ExportMapFrameToJPEGAsync("Layout Name", "Map1 Map Frame", @"C:\Temp\JPEG_MapFrame.jpg");
      await ExportActiveMapToJPEGExample.ExportActiveMapToJPEGAsync(@"C:\Temp\JPEG_ActiveMap.jpg");
    }
  }

  internal class ExportCodeSamples_button6 : Button  //Export to PDF
  {
    async protected override void OnClick()
    {
      await ExportLayoutToPDFExample.ExportLayoutToPDFAsync("Layout Name", @"C:\Temp\PDF_Layout.pdf");
      await ExportMapFrameToPDFExample.ExportMapFrameToPDFAsync("Layout Name", "Map1 Map Frame", @"C:\Temp\PDF_MapFrame.pdf");
      await ExportActiveMapToPDFExample.ExportActiveMapToPDFAsync(@"C:\Temp\PDF_ActiveMap.pdf");
    }
  }

  internal class ExportCodeSamples_button7 : Button  //Export to PNG
  {
    async protected override void OnClick()
    {
      await ExportLayoutToPNGExample.ExportLayoutToPNGAsync("Layout Name", @"C:\Temp\PNG_Layout.png");
      await ExportMapFrameToPNGExample.ExportMapFrameToPNGAsync("Layout Name", "Map1 Map Frame", @"C:\Temp\PNG_MapFrame.png");
      await ExportActiveMapToPNGExample.ExportActiveMapToPNGAsync(@"C:\Temp\PNG_ActiveMap.png");
    }
  }

  internal class ExportCodeSamples_button8 : Button  //Export to SVG
  {
    async protected override void OnClick()
    {
      await ExportLayoutToSVGExample.ExportLayoutToSVGAsync("Layout Name", @"C:\Temp\SVG_Layout.svg");
      await ExportMapFrameToSVGExample.ExportMapFrameToSVGAsync("Layout Name", "Map1 Map Frame", @"C:\Temp\SVG_MapFrame.svg");
      await ExportActiveMapToSVGExample.ExportActiveMapToSVGAsync(@"C:\Temp\SVG_ActiveMap.svg");
    }
  }

  internal class ExportCodeSamples_button9 : Button  //Export to TGA
  {
    async protected override void OnClick()
    {
      await ExportLayoutToTGAExample.ExportLayoutToTGAAsync("Layout Name", @"C:\Temp\TGA_Layout.tga");
      await ExportMapFrameToTGAExample.ExportMapFrameToTGAAsync("Layout Name", "Map1 Map Frame", @"C:\Temp\TGA_MapFrame.tga");
      await ExportActiveMapToTGAExample.ExportActiveMapToTGAAsync(@"C:\Temp\TGA_ActiveMap.tga");
    }
  }

  internal class ExportCodeSamples_button10 : Button  //Export to TIFF
  {
    async protected override void OnClick()
    {
      await ExportLayoutToTIFFExample.ExportLayoutToTIFFAsync("Layout Name", @"C:\Temp\TIFF_Layout.tif");
      await ExportMapFrameToTIFFExample.ExportMapFrameToTIFFAsync("Layout Name", "Map1 Map Frame", @"C:\Temp\TIFF_MapFrame.tif");
      await ExportActiveMapToTIFFExample.ExportActiveMapToTIFFAsync(@"C:\Temp\TIFF_ActiveMap.tif");
    }
  }
}
