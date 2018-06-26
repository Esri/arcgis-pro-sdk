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
using System.Threading.Tasks;
using ArcGIS.Core.Data;
using ArcGIS.Desktop.Framework.Threading.Tasks;

namespace SDKExamples.GeodatabaseSDK.FeatureService
{
  /// <summary>
  /// Illustrates how to open a Dataset from a feature service (i.e., a web geodatabase).
  /// </summary>
  /// 
  /// <remarks>
  /// <para>
  /// While it is true classes that are derived from the <see cref="ArcGIS.Core.CoreObjectsBase"/> super class 
  /// consumes native resources (e.g., <see cref="ArcGIS.Core.Data.Geodatabase"/> or <see cref="ArcGIS.Core.Data.FeatureClass"/>), 
  /// you can rest assured that the garbage collector will properly dispose of the unmanaged resources during 
  /// finalization.  However, there are certain workflows that require a <b>deterministic</b> finalization of the 
  /// <see cref="ArcGIS.Core.Data.Geodatabase"/>.  Consider the case of a file geodatabase that needs to be deleted 
  /// on the fly at a particular moment.  Because of the <b>indeterministic</b> nature of garbage collection, we can't
  /// count on the garbage collector to dispose of the Geodatabase object, thereby removing the <b>lock(s)</b> at the  
  /// moment we want. To ensure a deterministic finalization of important native resources such as a 
  /// <see cref="ArcGIS.Core.Data.Geodatabase"/> or <see cref="ArcGIS.Core.Data.FeatureClass"/>, you should declare 
  /// and instantiate said objects in a <b>using</b> statement.  Alternatively, you can achieve the same result by 
  /// putting the object inside a try block and then calling Dispose() in a finally block.
  /// </para>
  /// <para>
  /// In general, you should always call Dispose() on the following types of objects: 
  /// </para>
  /// <para>
  /// - Those that are derived from <see cref="ArcGIS.Core.Data.Datastore"/> (e.g., <see cref="ArcGIS.Core.Data.Geodatabase"/>).
  /// </para>
  /// <para>
  /// - Those that are derived from <see cref="ArcGIS.Core.Data.Dataset"/> (e.g., <see cref="ArcGIS.Core.Data.Table"/>).
  /// </para>
  /// <para>
  /// - <see cref="ArcGIS.Core.Data.RowCursor"/> and <see cref="ArcGIS.Core.Data.RowBuffer"/>.
  /// </para>
  /// <para>
  /// - <see cref="ArcGIS.Core.Data.Row"/> and <see cref="ArcGIS.Core.Data.Feature"/>.
  /// </para>
  /// <para>
  /// - <see cref="ArcGIS.Core.Data.Selection"/>.
  /// </para>
  /// <para>
  /// - <see cref="ArcGIS.Core.Data.VersionManager"/> and <see cref="ArcGIS.Core.Data.Version"/>.
  /// </para>
  /// </remarks>
  public class FeatureServiceOpenDatasetByLayerID
  {
    /// <summary>
    /// In order to illustrate that feature service calls have to be made on the MCT.
    /// </summary>
    /// <returns></returns>
    public async Task GeodatabaseOpenDatasetAsync()
    {
      await QueuedTask.Run(() => MainMethodCode());
    }

    public void MainMethodCode()
    {
      Uri arcgisOnlineURL = new Uri("http://services1.arcgis.com/47GG2ga246DGaLwa/arcgis/rest/services/FeatureServiceName/FeatureServer");

      ServiceConnectionProperties arcGISOnline = new ServiceConnectionProperties(arcgisOnlineURL);

      using (Geodatabase featureService = new Geodatabase(arcGISOnline))
      {
        using (FeatureClass featureClass = featureService.OpenDataset<FeatureClass>("0"))
        {
          // Use the feature class opened from layer ID 0.
        }

        using (Table table = featureService.OpenDataset<Table>("4"))
        {
          // Use the table opened from layer ID 4.
        }

        using (AttributedRelationshipClass attributedRelationshipClass = featureService.OpenDataset<AttributedRelationshipClass>("5"))
        {
          // Use the attributed relationship class opened from layer ID 5.
        }

        try
        {
          string idOfLayerWhichIsNotTable = "3";
          featureService.OpenDataset<Table>(idOfLayerWhichIsNotTable);
        }
        catch (InvalidOperationException)
        {
          // Handle Exception.
        }
      }
    }
  }
}