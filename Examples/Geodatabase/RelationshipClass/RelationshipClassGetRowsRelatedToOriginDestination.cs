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
using ArcGIS.Core;
using ArcGIS.Core.Data;
using ArcGIS.Desktop.Framework.Threading.Tasks;

namespace SDKExamples.GeodatabaseSDK
{
  /// <summary>
  /// Illustrates how to retrieve rows related to those in the origin table as well as the destination table from a RelationshipClass.
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
  public class RelationshipClassGetRowsRelatedToOriginDestination
  {
    /// <summary>
    /// In order to illustrate that Geodatabase calls have to be made on the MCT
    /// </summary>
    /// <returns></returns>
    public async Task RelationshipClassGetRowsRelatedToOriginDestinationAsync()
    {
      await QueuedTask.Run(() => MainMethodCode());
    }

    public void MainMethodCode()
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
      using (RelationshipClass relationshipClass = geodatabase.OpenDataset<RelationshipClass>("LocalGovernment.GDB.luCodeViolationHasInspections"))
      using (FeatureClass violationsFeatureClass = geodatabase.OpenDataset<FeatureClass>("LocalGovernment.GDB.luCodeViolation"))
      using (Table inspectionTable               = geodatabase.OpenDataset<Table>("LocalGovernment.GDB.luCodeInspection"))
      {
        List<Row> jeffersonAveViolations = new List<Row>();
        QueryFilter queryFilter          = new QueryFilter { WhereClause = "LOCDESC LIKE '///%Jefferson///%'" };

        // If you need to *cache* the rows retrieved from the cursor for processing later, it is important that you don't use recycling in
        // the Search() method (i.e., useRecyclingCursor must be set to *false*).  Also, the returned rows/features cached in the list
        // should be disposed when no longer in use so that unmanaged resources are not held on to longer than necessary.

        using (RowCursor rowCursor = violationsFeatureClass.Search(queryFilter, false))
        {
          while (rowCursor.MoveNext())
          {
            jeffersonAveViolations.Add(rowCursor.Current);
          }
        }

        IReadOnlyList<Row> relatedOriginRows      = null;
        IReadOnlyList<Row> relatedDestinationRows = null;

        try
        {
          QueryFilter filter  = new QueryFilter { WhereClause = "ACTION = '1st Notice'" };
          Selection selection = inspectionTable.Select(filter, SelectionType.ObjectID, SelectionOption.Normal);

          relatedOriginRows = relationshipClass.GetRowsRelatedToDestinationRows(selection.GetObjectIDs());

          // Find out if any of 1st Notice inspections were from Jefferson Avenue.
          FeatureClassDefinition featureClassDefinition = violationsFeatureClass.GetDefinition();
          int locationDescriptionIndex = featureClassDefinition.FindField("LOCDESC");
          bool containsJeffersonAve    = relatedOriginRows.Any(row => Convert.ToString(row[locationDescriptionIndex]).Contains("Jefferson"));

          List<long> jeffersonAveViolationObjectIds = jeffersonAveViolations.Select(row => row.GetObjectID()).ToList();

          relatedDestinationRows = relationshipClass.GetRowsRelatedToOriginRows(jeffersonAveViolationObjectIds);

          //Find out if any Jefferson Ave Violations have 1st Notice Inspections
          TableDefinition tableDefinition = inspectionTable.GetDefinition();
          int actionFieldIndex            = tableDefinition.FindField("ACTION");
          bool hasFirstNoticeInspections  = relatedDestinationRows.Any(row => Convert.ToString(row[actionFieldIndex]).Contains("1st Notice"));
        }
        finally
        {
          Dispose(jeffersonAveViolations);
          Dispose(relatedOriginRows);
          Dispose(relatedDestinationRows);
        }
      }
    }

    private static void Dispose<T>(IEnumerable<T> iterator) where T : CoreObjectsBase
    {
      if (iterator != null)
      {
        foreach (T coreObject in iterator)
        {
          if (coreObject != null)
            coreObject.Dispose();
        }
      }
    }
  }
}