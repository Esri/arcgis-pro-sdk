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
using System.Threading.Tasks;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Framework.Threading.Tasks;

namespace SDKExamples.GeodatabaseSDK
{
  /// <summary>
  /// Illustrates how to add to a Selection object.
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
  public class SelectionAdd
  {
    /// <summary>
    /// In order to illustrate that Geodatabase calls have to be made on the MCT
    /// </summary>
    /// <returns></returns>
    public async Task SelectionAddAsync()
    {
      await QueuedTask.Run(() => MainMethodCode());
    }

    public void MainMethodCode()
    {
      using (Geodatabase fileGeodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(@"C:\Data\LocalGovernment.gdb"))))
      using (Table table                 = fileGeodatabase.OpenDataset<Table>("EmployeeInfo"))
      {
        QueryFilter queryFilter        = new QueryFilter { WhereClause = "COSTCTRN = 'Finance'" };
        QueryFilter anotherQueryFilter = new QueryFilter { WhereClause = "COSTCTRN = 'Information Technology'" };

        using (Selection onlyOneFinance = table.Select(queryFilter, SelectionType.ObjectID, SelectionOption.OnlyOne))
        using (Selection itFolks        = table.Select(anotherQueryFilter, SelectionType.ObjectID, SelectionOption.Normal))
        {
          itFolks.Add(onlyOneFinance.GetObjectIDs());
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

      QueryFilter filter = new QueryFilter { WhereClause = "DISTRCTNAME = 'Indian Prairie School District 204'" };

      using (Geodatabase geodatabase          = new Geodatabase(connectionProperties))
      using (FeatureClass featureClass        = geodatabase.OpenDataset<FeatureClass>("LocalGovernment.GDB.SchoolBoundary"))
      using (Selection indianPrairieSelection = featureClass.Select(filter, SelectionType.ObjectID, SelectionOption.Normal))
      {
        // Although the Add method can be used with Normal and OnlyOne selections, Empty selections are the ones which will absolutely need Add method.
        using (Selection emptySelection = indianPrairieSelection.Select(new QueryFilter(), SelectionOption.Empty))
        {
          // Using SpatialQueryFilter with Selection.Select.
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

          //Another SpatialQueryFilter
          SpatialQueryFilter anotherSpatialQueryFilter = new SpatialQueryFilter
          {
            FilterGeometry = new PolygonBuilderEx(new List<Coordinate2D>
            {
              new Coordinate2D(1015106, 1829421),
              new Coordinate2D(1013413, 1834760),
              new Coordinate2D(1020705, 1840359),
              new Coordinate2D(1026304, 1838145)
            }).ToGeometry(),

            SpatialRelationship = SpatialRelationship.Intersects
          };

          using (Selection spatialSelection        = indianPrairieSelection.Select(spatialQueryFilter))
          using (Selection anotherSpatialSelection = indianPrairieSelection.Select(anotherSpatialQueryFilter))
          {

            //Now that we have two selections, we can add the ObjectIds of these selections to the empty selection without affecting the original selections
            //Add does not add object ids which are already present in the selection. So, there should be no exceptions or duplicate Ids.

            emptySelection.Add(spatialSelection.GetObjectIDs());
            emptySelection.Add(anotherSpatialSelection.GetObjectIDs());

            Selection selection  = featureClass.Select(null, SelectionType.ObjectID, SelectionOption.Normal);
            long invalidObjectId = GetHighestObjectId(selection.GetObjectIDs()) + 1;

            // This will not throw an exception or fail. Further, the invalid ids will not be added to the selection.
            selection.Add(new List<long> { invalidObjectId });
          }
        }
      }
    }

    private long GetHighestObjectId(IEnumerable<long> objectIds)
    {
      return objectIds.OrderByDescending(i => i).FirstOrDefault();
    }
  }
}