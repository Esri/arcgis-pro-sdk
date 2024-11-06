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
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Core;
using ArcGIS.Core.CIM;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Core.Data;
using ArcGIS.Core.Data.Raster;

namespace Raster.ProSnippet
{
  class ProSnippet
  {

    public async void RasterDatasetSnippets()
    {
      try
      {
        // cref: ArcGIS.Core.Data.FileSystemConnectionPath.#ctor(System.Uri, ArcGIS.Core.Data.FileSystemDatastoreType)
        // cref: ArcGIS.Core.Data.FileSystemDatastoreType
        // cref: ArcGIS.Core.Data.FileSystemDatastore.#ctor(ArcGIS.Core.Data.FileSystemConnectionPath)
        // cref: ArcGIS.Core.Data.FileSystemDatastore.OpenDataset<T>(System.String)
        // cref: ArcGIS.Core.Data.Raster.RasterDataset
        #region Open raster dataset in a folder
        // Create a FileSystemConnectionPath using the folder path.
        FileSystemConnectionPath connectionPath = new FileSystemConnectionPath(new System.Uri(@"C:\Temp"), FileSystemDatastoreType.Raster);
        // Create a new FileSystemDatastore using the FileSystemConnectionPath.
        FileSystemDatastore dataStore = new FileSystemDatastore(connectionPath);
        // Open the raster dataset.
        RasterDataset fileRasterDataset = dataStore.OpenDataset<RasterDataset>("Sample.tif");
        #endregion

        // cref: ArcGIS.Core.Data.FileGeodatabaseConnectionPath.#ctor(System.Uri)
        // cref: ArcGIS.Core.Data.Geodatabase.#ctor(ArcGIS.Core.Data.FileGeodatabaseConnectionPath)
        // cref: ArcGIS.Core.Data.Geodatabase.OpenDataset<T>(System.String)
        // cref: ArcGIS.Core.Data.Raster.RasterDataset
        #region Open raster dataset in a geodatabase
        // Create a FileGeodatabaseConnectionPath using the path to the gdb. Note: This can be a path to a .sde file.
        FileGeodatabaseConnectionPath geodatabaseConnectionPath = new FileGeodatabaseConnectionPath(new Uri(@"C:\Temp\rasters.gdb"));
        // Create a new Geodatabase object using the FileGeodatabaseConnectionPath.
        Geodatabase geodatabase = new Geodatabase(geodatabaseConnectionPath);
        // Open the raster dataset.
        RasterDataset gdbRasterDataset = geodatabase.OpenDataset<RasterDataset>("sample");
        #endregion

        RasterDataset rasterDataset = fileRasterDataset;
        // cref: ArcGIS.Core.Data.Raster.RasterDataset.GetDefinition()
        // cref: ArcGIS.Core.Data.Raster.RasterDatasetDefinition
        // cref: ArcGIS.Core.Data.Raster.RasterDatasetDefinition.GetBandCount()
        #region Get the raster dataset definition from a raster dataset
        await QueuedTask.Run(() =>
        {
          RasterDatasetDefinition rasterDatasetDefinition = rasterDataset.GetDefinition();

          // access the dataset definition properties
          rasterDatasetDefinition.GetBandCount();
        });
        #endregion


        {
          string sBandName = "";

          // cref: ArcGIS.Core.Data.Raster.RasterDataset.GetBandCount()
          // cref: ArcGIS.Core.Data.Raster.RasterDataset.GetBandByName(System.String)
          // cref: ArcGIS.Core.Data.Raster.RasterDataset.GetBandIndex(System.String)
          // cref: ArcGIS.Core.Data.Raster.RasterDataset.GetBand(System.Int32)
          // cref: ArcGIS.Core.Data.Raster.RasterBand
          // cref: ArcGIS.Core.Data.Raster.RasterBand.GetDefinition()
          // cref: ArcGIS.Core.Data.Raster.RasterBandDefinition
          // cref: ArcGIS.Core.Data.Definition.GetName()
          #region Access the bands in a raster dataset

          var count = rasterDataset.GetBandCount();
          RasterBand rasterBandByName = rasterDataset.GetBandByName(sBandName);
          var index = rasterDataset.GetBandIndex(sBandName);

          // Get a RasterBand from the raster dataset
          RasterBand rasterBand = rasterDataset.GetBand(0);

          // Get the RasterBandDefinition from the raster band.
          RasterBandDefinition rasterBandDefinition = rasterBand.GetDefinition();
          // Get the name of the raster band from the raster band.
          string bandName = rasterBandDefinition.GetName();
          #endregion

        }

        {
          // cref: ArcGIS.Desktop.Mapping.RasterLayer
          // cref: ArcGIS.Desktop.Mapping.BasicRasterLayer.GetRaster()
          // cref: ArcGIS.Core.Data.Raster.Raster
          // cref: ArcGIS.Core.Data.Raster.Raster.GetAttributeTable()
          #region Access rows in a raster attribute table
          var raster = MapView.Active.Map.GetLayersAsFlattenedList().OfType<RasterLayer>().FirstOrDefault();
          if (raster != null)
          {
            await QueuedTask.Run(() =>
            {
              var rasterTbl = raster.GetRaster().GetAttributeTable();
              var cursor = rasterTbl.Search();
              while (cursor.MoveNext())
              {
                var row = cursor.Current;
              }
            });
          }
          #endregion
        }

        {
          // cref: ArcGIS.Core.Data.Raster.RasterDataset.CreateFullRaster()
          // cref: ArcGIS.Core.Data.Raster.Raster.GetHeight()
          // cref: ArcGIS.Core.Data.Raster.Raster.GetWidth()
          // cref: ArcGIS.Core.Data.Raster.Raster.CreateCursor(System.Int32, System.Int32)
          // cref: ArcGIS.Core.Data.Raster.RasterCursor
          // cref: ArcGIS.Core.Data.Raster.RasterCursor.Current
          // cref: ArcGIS.Core.Data.Raster.RasterCursor.MoveNext
          // cref: ArcGIS.Core.Data.Raster.PixelBlock
          #region Create a raster cursor to iterate through the raster data
          await QueuedTask.Run(() =>
          {
            // Create a full raster from the raster dataset.
            ArcGIS.Core.Data.Raster.Raster raster = rasterDataset.CreateFullRaster();

            // Calculate size of pixel blocks to process. Use 1000 or height/width of the raster, whichever is smaller.
            var height = raster.GetHeight();
            var width = raster.GetWidth();
            int pixelBlockHeight = height > 1000 ? 1000 : height;
            int pixelBlockWidth = width > 1000 ? 1000 : width;

            // Create the raster cursor using the height and width calculated.
            RasterCursor rasterCursor = raster.CreateCursor(pixelBlockWidth, pixelBlockHeight);

            // Use a do-while loop to iterate through the pixel blocks of the raster using the raster cursor.
            do
            {
              // Get the current pixel block from the cursor.
              using (PixelBlock currentPixelBlock = rasterCursor.Current)
              {
                // Do something with the pixel block...
              }

              // Once you are done, move to the next pixel block.
            }
            while (rasterCursor.MoveNext());
          });
          #endregion
        }

        {
          // cref: ArcGIS.Core.Data.Raster.RasterDataset.CreateFullRaster()
          // cref: ArcGIS.Core.Data.Raster.Raster.GetHeight()
          // cref: ArcGIS.Core.Data.Raster.Raster.GetWidth()
          // cref: ArcGIS.Core.Data.Raster.Raster.CreatePixelBlock(System.Int32, System.Int32)
          // cref: ArcGIS.Core.Data.Raster.PixelBlock
          // cref: ArcGIS.Core.Data.Raster.Raster.Read(System.Int32, System.Int32, ArcGIS.Core.Data.Raster.PixelBlock)
          // cref: ArcGIS.Core.Data.Raster.Raster.Write(System.Int32, System.Int32, ArcGIS.Core.Data.Raster.PixelBlock)
          #region Read and Write pixels from and to a raster dataset using pixel blocks
          await QueuedTask.Run(() =>
          {
            // Create a full raster from the raster dataset.
            ArcGIS.Core.Data.Raster.Raster raster = rasterDataset.CreateFullRaster();

            // Calculate size of pixel block to create. Use 128 or height/width of the raster, whichever is smaller.
            var height = raster.GetHeight();
            var width = raster.GetWidth();
            int pixelBlockHeight = height > 128 ? 128 : height;
            int pixelBlockWidth = width > 128 ? 128 : width;

            // Create a new (blank) pixel block.
            PixelBlock currentPixelBlock = raster.CreatePixelBlock(pixelBlockWidth, pixelBlockHeight);

            // Read pixel values from the raster dataset into the pixel block starting from the given top left corner.
            raster.Read(0, 0, currentPixelBlock);

            // Do something with the pixel block...

            // Write the pixel block to the raster dataset starting from the given top left corner.
            raster.Write(0, 0, currentPixelBlock);
          });
          #endregion
        }

        {
          // Create a full raster from the raster dataset.
          ArcGIS.Core.Data.Raster.Raster raster = rasterDataset.CreateFullRaster();

          // Calculate size of pixel block to create. Use 128 or height/width of the raster, whichever is smaller.
          int pixelBlockHeight = raster.GetHeight() > 128 ? 128 : raster.GetHeight();
          int pixelBlockWidth = raster.GetWidth() > 128 ? 128 : raster.GetWidth();

          // Create a new (blank) pixel block.
          PixelBlock currentPixelBlock = raster.CreatePixelBlock(pixelBlockWidth, pixelBlockHeight);

          // cref: ArcGIS.Core.Data.Raster.Raster.Read(System.Int32, System.Int32, ArcGIS.Core.Data.Raster.PixelBlock)
          // cref: ArcGIS.Core.Data.Raster.PixelBlock.GetPlaneCount()
          // cref: ArcGIS.Core.Data.Raster.PixelBlock.GetPixelData(System.Int32, System.Boolean)
          // cref: ArcGIS.Core.Data.Raster.PixelBlock.GetHeight()
          // cref: ArcGIS.Core.Data.Raster.PixelBlock.GetWidth()
          // cref: ArcGIS.Core.Data.Raster.PixelBlock.GetNoDataMaskValue(System.Int32, System.Int32, System.Int32)
          // cref: ArcGIS.Core.Data.Raster.PixelBlock.SetPixelData(System.Int32, System.Array)
          // cref: ArcGIS.Core.Data.Raster.Raster.Write(System.Int32, System.Int32, ArcGIS.Core.Data.Raster.PixelBlock)
          #region Process pixels using a pixel block
          await QueuedTask.Run(() =>
          {
            // Read pixel values from the raster dataset into the pixel block starting from the given top left corner.
            raster.Read(0, 0, currentPixelBlock);

            // For each plane (band) in the pixel block
            for (int plane = 0; plane < currentPixelBlock.GetPlaneCount(); plane++)
            {
              // Get a copy of the array of pixels from the pixel block corresponding to the current plane.
              Array sourcePixels = currentPixelBlock.GetPixelData(plane, true);
              // Get the height and width of the pixel block.
              int pBHeight = currentPixelBlock.GetHeight();
              int pBWidth = currentPixelBlock.GetWidth();

              // Iterate through the pixels in the array.
              for (int i = 0; i < pBHeight; i++)
              {
                for (int j = 0; j < pBWidth; j++)
                {
                  // Get the NoData mask value to see if the pixel is a valid pixel.
                  if (Convert.ToByte(currentPixelBlock.GetNoDataMaskValue(plane, j, i)) == 1)
                  {
                    // Get the pixel value from the array and process it (add 5 to the value).
                    // Note: This is assuming the pixel type is Unisigned 8bit.
                    int pixelValue = Convert.ToInt16(sourcePixels.GetValue(j, i)) + 5;
                    // Make sure the pixel value does not go above the range of the pixel type.
                    pixelValue = pixelValue > 254 ? 254 : pixelValue;
                    // Set the new pixel value to the array.
                    // Note: This is assuming the pixel type is Unisigned 8bit.
                    sourcePixels.SetValue(Convert.ToByte(pixelValue), j, i);
                  }
                }
              }
              // Set the modified array of pixels back to the pixel block.
              currentPixelBlock.SetPixelData(plane, sourcePixels);
            }
            // Write the pixel block to the raster dataset starting from the given top left corner.
            raster.Write(0, 0, currentPixelBlock);
          });
          #endregion

          // cref: ArcGIS.Desktop.Mapping.BasicRasterLayer.GetColorizer()
          // cref: ArcGIS.Core.CIM.CIMRasterStretchColorizer
          // cref: ArcGIS.Core.CIM.CIMRasterStretchColorizer.StretchStats
          // cref: ArcGIS.Core.CIM.StatsHistogram
          // cref: ArcGIS.Core.CIM.StatsHistogram.max
          // cref: ArcGIS.Core.CIM.StatsHistogram.min
          #region Calculate Raster statistics
          //If a raster dataset has statistics, you can create a raster layer and get these statistics by accessing the colorizer.
          await QueuedTask.Run(() =>
          {
            //Accessing the raster layer
            var lyr = MapView.Active.Map.GetLayersAsFlattenedList().OfType<BasicRasterLayer>().FirstOrDefault();
            //Getting the colorizer
            var colorizer = lyr.GetColorizer() as CIMRasterStretchColorizer;
            //Accessing the statistics
            var stats = colorizer.StretchStats;
            var max = stats.max;
            var min = stats.min;
          });
          #endregion

        }
      }
      catch (Exception)
      {
      }
    }

    #region ProSnippet Group: Raster and Imagery Options
    #endregion

    internal void Options()
    {
      // cref: ArcGIS.Desktop.Core.ApplicationOptions.RasterImageryOptions
      // cref: ArcGIS.Desktop.Core.RasterImageryOptions
      // cref: ArcGIS.Core.CIM.RasterResamplingType
      // cref: ArcGIS.Core.CIM.RasterStretchType
      // cref:ArcGIS.Desktop.Core.BuildPyramidOption
      // cref:ArcGIS.Desktop.Core.PyramidResamplingMode
      // cref:ArcGIS.Desktop.Core.PyramidCompressionType
      // cref:ArcGIS.Desktop.Core.CalculateStatisticOption
      #region Get/Set Raster and Imagery Options

      var ro = ApplicationOptions.RasterImageryOptions;

      QueuedTask.Run(() =>
      {
        var validCategories = ro.GetValidColorRampCategories();
        var validCategory = validCategories.FirstOrDefault();

        var colorRamps = ColorFactory.Instance.GetColorRampNames(validCategory);
        var newColorRampName = colorRamps.FirstOrDefault();

        var stretchedColorRamp = ro.GetStretchedColorRamp();
        var colorRampName = ro.GetStretchedColorRampName();
        ro.SetStretchedColorRampName(null);

        var classifyColorRamp = ro.GetClassifyColorRamp();
        colorRampName = ro.GetClassifyColorRampName();
        ro.SetClassifyColorRampName(newColorRampName);

        var discreteColorRamp = ro.GetDiscreteColorRamp();
        colorRampName = ro.GetDiscreteColorRampName();
        ro.SetDiscreteColorRampName(newColorRampName);

        var uniqueValueColorRamp = ro.GetUniqueValueColorRamp();
        colorRampName = ro.GetUniqueValueColorRampName();
        ro.SetUniqueValueColorRampName(newColorRampName);

        var customRendering = ro.GetEnableCustomRenderingDefaults();
        ro.SetEnableCustomRenderingDefaults(!customRendering);

        var sampleType = ro.GetResampleType();
        ro.SetResampleType(RasterResamplingType.CubicConvolution);

        var stretchType = ro.GetStretchType();
        ro.SetStretchType(RasterStretchType.PercentMinimumMaximum);

        var val = ro.GetNumberOfStandardDeviation();
        ro.SetNumberOfStandardDeviation(3);

        var (clipMin, clipMax) = ro.GetClipPercentage();
        ro.SetClipPercentage(12, 23);

        var red = ro.GetGammaStretchValueRed();
        var green = ro.GetGammaStretchValueGreen();
        var blue = ro.GetGammaStretchValueBlue();

        ro.SetGammaStretchValueRed(2);
        ro.SetGammaStretchValueGreen(20);
        ro.SetGammaStretchValueBlue(60);

        var displayBackground = ro.GetDisplayBackground();
        ro.SetDisplayBackground(!displayBackground);

        var (r, g, b) = ro.GetBackgroundValue();
        ro.SetBackgroundValue(4, 21, 61);

        var backColor = ro.GetBackgroundColor();
        ro.SetBackgroundColor(CIMColor.CreateRGBColor(255, 0, 0));

        var noData = ro.GetNoDataColor();
        ro.SetNoDataColor(CIMColor.CreateRGBColor(0, 255, 0));


        // cache
        var useCache = ro.GetUseImageServiceCache();
        ro.SetUseImageServiceCache(!useCache);

        //dataset

        var opt = ro.GetPyramidOption();
        ro.SetPyramidOption(ArcGIS.Desktop.Core.BuildPyramidOption.NeverBuild);
        var sample = ro.GetPyramidResampleMethod();
        ro.SetPyramidResampleMethod(PyramidResamplingMode.Bilinear);
        var compression = ro.GetPyramidCompressionMethod();
        ro.SetPyramidCompressionMethod(PyramidCompressionType.JPEG_YCbCr);

        var quality = ro.GetPyramidCompressionQuality();
        ro.SetPyramidCompressionQuality(32);

        var stats = ro.GetStatisticsOption();
        ro.SetStatisticsOption(CalculateStatisticOption.AlwaysCalculate);
        var x = ro.GetSkipFactorX();
        ro.SetSkipFactorX(3);
        var y = ro.GetSkipFactorY();
        ro.SetSkipFactorY(23);

        var useWorldFile = ro.GetUseWorldFile();
        ro.SetUseWorldFile(!useWorldFile);

        var createTiledTiff = ro.GetCreateTiledTiff();
        ro.SetCreateTiledTiff(!createTiledTiff);

        var maxUniqueValues = ro.GetMaximumUniqueValues();
        ro.SetMaximumUniqueValues(123456);

        var path = ro.GetProxyFileLocation();
        ro.SetProxyFileLocation("c:\\temp");

        var isExpanded = ro.GetIsMosaicLayerExpanded();
        ro.SetIsMosaicLayerExpanded(!isExpanded);
        var isVisible = ro.GetIsMosaicBoundaryVisible();
        ro.SetIsMosaicBoundaryVisible(!isVisible);
        isVisible = ro.GetIsMosaicFootprintVisible();
        ro.SetIsMosaicFootprintVisible(!isVisible);
        isVisible = ro.GetIsMosaicSeamlinesVisible();
        ro.SetIsMosaicSeamlinesVisible(!isVisible);
        isVisible = ro.GetIsMosaicPreviewVisible();
        ro.SetIsMosaicPreviewVisible(!isVisible);

        var (red_3band, green_3band, blue_3band) = ro.Get3BandColor();
        ro.Set3BandColor(2, 2, 2);
        var (ms_red, ms_green, ms_blue) = ro.GetMSColor();
        ro.SetMSColor(23, 24, 25);

        var enableCustomColorSchemes = ro.GetEnableCustomColorSchemes();
        ro.SetEnableCustomColorSchemes(!enableCustomColorSchemes);

        var useWavelengthInfo = ro.GetUseWavelengthInformation();
        ro.SetUseWavelengthInformation(!useWavelengthInfo);
      });
      #endregion
    }
  }
}