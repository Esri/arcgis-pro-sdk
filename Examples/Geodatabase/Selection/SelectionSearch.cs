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
using ArcGIS.Desktop.Framework.Threading.Tasks;

namespace SDKExamples.GeodatabaseSDK
{
  /// <summary>
  /// Illustrates how to search from a Selection object.
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
  public class SelectionSearch
  {
    /// <summary>
    /// In order to illustrate that Geodatabase calls have to be made on the MCT
    /// </summary>
    /// <returns></returns>
    public async Task SelectionSearchAsync()
    {
      await QueuedTask.Run(() => MainMethodCode());
    }

    public void MainMethodCode()
    {
      using (Geodatabase fileGeodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(@"C:\Data\LocalGovernment.gdb"))))
      using (Table table                 = fileGeodatabase.OpenDataset<Table>("EmployeeInfo"))
      {
        QueryFilter anotherQueryFilter = new QueryFilter { WhereClause = "COSTCTRN = 'Information Technology'" };
        using (Selection itFolks = table.Select(anotherQueryFilter, SelectionType.ObjectID, SelectionOption.Normal))
        {
          // Search with a null or a QueryFilter with an empty where clause will return all the Rows for the ObjectIds.
          // This can be used to obtain the rows for all the ObjectIds in the selection.

          using (RowCursor rowCursor = itFolks.Search(null, false))
          {
            while (rowCursor.MoveNext())
            {
              using (Row row = rowCursor.Current)
              {
                // Process the row.
              }
            }
          }
        }
      }
      
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
      
      using (Geodatabase geodatabase   = new Geodatabase(connectionProperties))
      using (FeatureClass featureClass = geodatabase.OpenDataset<FeatureClass>("LocalGovernment.GDB.SchoolBoundary"))
      {
        QueryFilter indianPrairieFilter  = new QueryFilter { WhereClause = "DISTRCTNAME = 'Indian Prairie School District 204'" };
        using (Selection indianPrairieSelection = featureClass.Select(indianPrairieFilter, SelectionType.ObjectID, SelectionOption.Normal))
        {
          // Using SpatialQueryFilter with Selection.Search.
          SpatialQueryFilter spatialQueryFilter = new SpatialQueryFilter
          {
            FilterGeometry = new PolygonBuilderEx(new List<Coordinate2D>
            {
              new Coordinate2D(1021880, 1867396),
              new Coordinate2D(1028223, 1870705),
              new Coordinate2D(1031165, 1866844),
              new Coordinate2D(1025373, 1860501),
              new Coordinate2D(1021788, 1863810)
            }).ToGeometry(),

            SpatialRelationship = SpatialRelationship.Intersects
          };

          // Note that we are using Recycling.
          using (RowCursor spatialSearch = indianPrairieSelection.Search(spatialQueryFilter, true))
          {
            // Note that we are calling the FindField outside the loop because this searches all the fields. There is no point in performing it for every Row.
            int nameFieldIndex = spatialSearch.FindField("NAME");

            while (spatialSearch.MoveNext())
            {
              using (Row row = spatialSearch.Current)
              {
                Console.WriteLine(row[nameFieldIndex]);
              }
            }
          }

          QueryFilter queryFilter = new QueryFilter { WhereClause = "NAME LIKE '///%Elementary///%'" };
          using (Selection elementarySelection = featureClass.Select(queryFilter, SelectionType.ObjectID, SelectionOption.Normal))
          {
            // Note that the cursor will contain only those rows which match "NAME LIKE '%Elementary%'" and "DISTRCTNAME = 'Indian Prairie School District 204'".
            // This is to illustrate that the seach will only happen on the object Ids in the selection.
            using (RowCursor cursor = elementarySelection.Search(indianPrairieFilter, false))
            {
              while (cursor.MoveNext())
              {
                using (Feature feature = (Feature)cursor.Current)
                {
                  // Process feature.
                }
              }
            }
          }

          using (Selection onlyOneIndianPrairie = indianPrairieSelection.Select(null, SelectionOption.OnlyOne))
          {
            QueryFilter napervilleFilter = new QueryFilter { WhereClause = "DISTRCTNAME LIKE '///%Naperville///%'" };

            using (RowCursor napervilleSearch = onlyOneIndianPrairie.Search(napervilleFilter, false))
            {
              // This will be false because there are no object ids in onlyOneIndianPrairie which have NaperVille in their DistrictName.
              bool hasRow = napervilleSearch.MoveNext();

              using (Selection emptySelection = indianPrairieSelection.Select(null, SelectionOption.Empty))
              using (RowCursor searchOnEmptySelection = emptySelection.Search(napervilleFilter, false))
              {
                // Again, this will be false because there are no object ids in emptySelection.
                hasRow = searchOnEmptySelection.MoveNext();
              }
            }
          }
        }
      }
    }
  }
}