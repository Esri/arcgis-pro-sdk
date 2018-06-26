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
using System.Threading.Tasks;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Framework.Threading.Tasks;

namespace SDKExamples
{
  /// <summary>
  /// Illustrates how to create a Feature in a FeatureClass.
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
  public class FeatureClassCreateRow
  {
    /// <summary>
    /// In order to illustrate that Geodatabase calls have to be made on the MCT
    /// </summary>
    /// <returns></returns>
    public async Task MainMethodCode()
    {
      await QueuedTask.Run(async () =>
      {
        await EnterpriseGeodabaseWorkFlow();
        await FileGeodatabaseWorkFlow();
      });
    }

    private static async Task EnterpriseGeodabaseWorkFlow()
    {
      // Opening a Non-Versioned SQL Server instance.

      DatabaseConnectionProperties connectionProperties = new DatabaseConnectionProperties(EnterpriseDatabaseType.SQLServer)
      {
        AuthenticationMode = AuthenticationMode.DBMS,

        // Where testMachine is the machine where the instance is running and testInstance is the name of the SqlServer instance.
        Instance = @"testMachine\testInstance",

        // Provided that a database called LocalGovernment has been created on the testInstance and geodatabase has been enabled on the database.
        Database = "LocalGovernment",

        // Provided that a login called gdb has been created and corresponding schema has been created with the required permissions.
        User     = "gdb",
        Password = "password",
        Version  = "dbo.DEFAULT"
      };

      using (Geodatabase geodatabase             = new Geodatabase(connectionProperties))
      using (FeatureClass enterpriseFeatureClass = geodatabase.OpenDataset<FeatureClass>("LocalGovernment.GDB.FacilitySite"))
      {
        EditOperation editOperation = new EditOperation();
        editOperation.Callback(context => 
        {
          RowBuffer rowBuffer = null;
          Feature feature     = null;

          try
          {
            FeatureClassDefinition facilitySiteDefinition = enterpriseFeatureClass.GetDefinition();

            int facilityIdIndex = facilitySiteDefinition.FindField("FACILITYID");
            rowBuffer           = enterpriseFeatureClass.CreateRowBuffer();

            // Either the field index or the field name can be used in the indexer.
            rowBuffer[facilityIdIndex] = "FAC-400";
            rowBuffer["NAME"]          = "Griffith Park";
            rowBuffer["OWNTYPE"]       = "Municipal";
            rowBuffer["FCODE"]         = "Park";
            // Add it to Public Attractions Subtype.
            rowBuffer[facilitySiteDefinition.GetSubtypeField()] = 820;

            List<Coordinate2D> newCoordinates = new List<Coordinate2D>
            {
              new Coordinate2D(1021570, 1880583),
              new Coordinate2D(1028730, 1880994),
              new Coordinate2D(1029718, 1875644),
              new Coordinate2D(1021405, 1875397)
            };

            rowBuffer[facilitySiteDefinition.GetShapeField()] = new PolygonBuilder(newCoordinates).ToGeometry();

            feature = enterpriseFeatureClass.CreateRow(rowBuffer);

            //To Indicate that the Map has to draw this feature and/or the attribute table to be updated
            context.Invalidate(feature);

            long objectID   = feature.GetObjectID();
            Polygon polygon = feature.GetShape() as Polygon;

             // Do some other processing with the newly-created feature.
          }
          catch (GeodatabaseException exObj)
          {
            Console.WriteLine(exObj);
          }
          finally
          {
            if (rowBuffer != null)
              rowBuffer.Dispose();

            if (feature != null)
              feature.Dispose();
          }
        }, enterpriseFeatureClass);
      
        bool editResult = editOperation.Execute();
        
        // If the table is non-versioned this is a no-op. If it is versioned, we need the Save to be done for the edits to be persisted.
        bool saveResult = await Project.Current.SaveEditsAsync();
      }
    }

    // Illustrates creating a feature in a File GDB.
    private static async Task FileGeodatabaseWorkFlow()
    {
      using (Geodatabase fileGeodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(@"C:\Data\LocalGovernment.gdb"))))
      using (FeatureClass featureClass   = fileGeodatabase.OpenDataset<FeatureClass>("PollingPlace"))
      {
        RowBuffer rowBuffer = null;
        Feature feature     = null;

        try
        {
          EditOperation editOperation = new EditOperation();
          editOperation.Callback(context =>
          {
            FeatureClassDefinition featureClassDefinition = featureClass.GetDefinition();

            int nameIndex = featureClassDefinition.FindField("NAME");
            rowBuffer     = featureClass.CreateRowBuffer();

            // Either the field index or the field name can be used in the indexer.
            rowBuffer[nameIndex]   = "New School";
            rowBuffer["POLLINGID"] = "260";
            rowBuffer["FULLADD"]   = "100 New Street";
            rowBuffer["CITY"]      = "Naperville";
            rowBuffer["STATE"]     = "IL";
            rowBuffer["OPERHOURS"] = "Yes";
            rowBuffer["HANDICAP"]  = "6.00am=7.00pm";
            rowBuffer["NEXTELECT"] = "11/6/2012";
            rowBuffer["REGDATE"]   = "8/6/2012";
            rowBuffer["PHONE"]     = "815-740-4782";
            rowBuffer["EMAIL"]     = "elections@willcounty.gov";

            rowBuffer[featureClassDefinition.GetShapeField()] = new MapPointBuilder(1028367, 1809789).ToGeometry();

            feature = featureClass.CreateRow(rowBuffer);

            //To Indicate that the Map has to draw this feature and/or the attribute table to be updated
            context.Invalidate(feature);

            // Do some other processing with the newly-created feature.
          
          }, featureClass);

          bool editResult = editOperation.Execute();

          // At this point the changes are visible in the process, but not persisted/visible outside.
          long objectID     = feature.GetObjectID();
          MapPoint mapPoint = feature.GetShape() as MapPoint;

          // If the table is non-versioned this is a no-op. If it is versioned, we need the Save to be done for the edits to be persisted.
          bool saveResult = await Project.Current.SaveEditsAsync();
        }
        catch (GeodatabaseException exObj)
        {
          Console.WriteLine(exObj);
          throw;
        }
        finally
        {
          if (rowBuffer != null)
            rowBuffer.Dispose();

          if (feature != null)
            feature.Dispose();
        }
      }
    }
  }
}