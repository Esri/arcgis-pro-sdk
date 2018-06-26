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
        #region Open raster dataset in a folder.
        // Create a FileSystemConnectionPath using the folder path.
        FileSystemConnectionPath connectionPath = new FileSystemConnectionPath(new System.Uri(@"C:\Temp"), FileSystemDatastoreType.Raster);
        // Create a new FileSystemDatastore using the FileSystemConnectionPath.
        FileSystemDatastore dataStore = new FileSystemDatastore(connectionPath);
        // Open the raster dataset.
        RasterDataset fileRasterDataset = dataStore.OpenDataset<RasterDataset>("Sample.tif");
        #endregion

        #region Open raster dataset in a geodatabase.
        // Create a FileGeodatabaseConnectionPath using the path to the gdb. Note: This can be a path to a .sde file.
        FileGeodatabaseConnectionPath geodatabaseConnectionPath = new FileGeodatabaseConnectionPath(new Uri(@"C:\Temp\rasters.gdb"));
        // Create a new Geodatabase object using the FileGeodatabaseConnectionPath.
        Geodatabase geodatabase = new Geodatabase(geodatabaseConnectionPath);
        // Open the raster dataset.
        RasterDataset gdbRasterDataset = geodatabase.OpenDataset<RasterDataset>("sample");
        #endregion

        RasterDataset rasterDataset = fileRasterDataset;
        #region Get the raster dataset definition from a raster dataset.
        await QueuedTask.Run(() =>
        {
          RasterDatasetDefinition rasterDatasetDefinition = rasterDataset.GetDefinition();
          rasterDatasetDefinition.GetBandCount();
        });
        #endregion

        {
          #region Create a raster cursor to iterate through the raster data.
          await QueuedTask.Run(() =>
          {
            // Create a full raster from the raster dataset.
            ArcGIS.Core.Data.Raster.Raster raster = rasterDataset.CreateFullRaster();

            // Calculate size of pixel blocks to process. Use 1000 or height/width of the raster, whichever is smaller.
            int pixelBlockHeight = raster.GetHeight() > 1000 ? 1000 : raster.GetHeight();
            int pixelBlockWidth = raster.GetWidth() > 1000 ? 1000 : raster.GetWidth();

            // Create the raster cursor using the height and width calculated.
            RasterCursor rasterCursor = raster.CreateCursor(pixelBlockWidth, pixelBlockHeight);

            // Use a do while loop to iterate through the pixel blocks of the raster using the raster cursor.
            do
            {
              // Get the current pixel block from the cursor.
              PixelBlock currentPixelBlock = rasterCursor.Current;
              // Do something with the pixel block...

              // Once you are done, move to the next pixel block.
            }
            while (rasterCursor.MoveNext());
          });
          #endregion
        }

        {
          #region Read and Write pixels from and to a raster dataset using pixel blocks.
          await QueuedTask.Run(() =>
          {
            // Create a full raster from the raster dataset.
            ArcGIS.Core.Data.Raster.Raster raster = rasterDataset.CreateFullRaster();

            // Calculate size of pixel block to create. Use 128 or height/width of the raster, whichever is smaller.
            int pixelBlockHeight = raster.GetHeight() > 128 ? 128 : raster.GetHeight();
            int pixelBlockWidth = raster.GetWidth() > 128 ? 128 : raster.GetWidth();

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

  }
}