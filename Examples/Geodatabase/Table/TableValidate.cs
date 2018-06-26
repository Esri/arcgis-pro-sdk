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
using ArcGIS.Core;
using ArcGIS.Core.Data;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Framework.Threading.Tasks;

namespace SDKExamples
{
  /// <summary>
  /// Illustrates how to validate updated rows in a Table.
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
  public class TableValidate
  {
    /// <summary>
    /// In order to illustrate that Geodatabase calls have to be made on the MCT
    /// </summary>
    /// <returns></returns>
    public async Task TableValidateAsync()
    {
      await QueuedTask.Run(() => MainMethodCode());
    }

    public void MainMethodCode()
    {
      using (Geodatabase fileGeodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(@"C:\Data\LocalGovernment.gdb"))))
      using (Table table                 = fileGeodatabase.OpenDataset<Table>("EmployeeInfo"))
      {
        QueryFilter queryFilter = new QueryFilter
        {
          WhereClause = "COSTCTRN = 'Information Technology'"
        };

        using (Selection selection = table.Select(queryFilter, SelectionType.ObjectID, SelectionOption.Normal))
        {
          // The result is a mapping between those object ids which failed validation and the reason why the validation failed (a string message).
          IReadOnlyDictionary<long, string> validationResult = table.Validate(selection);

          RowCursor rowCursor = table.Search(queryFilter, false);
          List<Row> rows      = new List<Row>();

          try
          {
            while (rowCursor.MoveNext())
            {
              rows.Add(rowCursor.Current);
            }

            // This is equivalent to the validation performed using the selection.
            IReadOnlyDictionary<long, string> equivalentValidationResult = table.Validate(rows);

            // Again this is equivalent to both the above results.
            IReadOnlyDictionary<long, string> anotherEquivalentResult = table.Validate(queryFilter);
          }
          finally
          {
            rowCursor.Dispose();
            Dispose(rows);
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

      using (Geodatabase geodatabase = new Geodatabase(connectionProperties))
      using (Table enterpriseTable   = geodatabase.OpenDataset<Table>("LocalGovernment.GDB.piCIPCost"))
      {
        QueryFilter openCutFilter  = new QueryFilter { WhereClause = "ACTION = 'Open Cut'" };

        using (Selection openCutSelection = enterpriseTable.Select(openCutFilter, SelectionType.ObjectID, SelectionOption.Normal))
        {
          // Remember that validation cost is directly proprtional to the number of Rows validated. So, select the set of rows to be validated 
          // judiciously.  This will be empty because all the rows in the piCIPCost table are valid.
          IReadOnlyDictionary<long, string> emptyDictionary = enterpriseTable.Validate(openCutSelection);

          // We are adding an invalid row to illustrate a case where the validation result is not empty.

          long invalidRowObjectID = -1;

          EditOperation editOperation = new EditOperation();
          editOperation.Callback(context =>
          {
            RowBuffer rowBuffer = null;
            Row       row       = null;

            try
            {
              TableDefinition tableDefinition = enterpriseTable.GetDefinition();

              rowBuffer = enterpriseTable.CreateRowBuffer();

              rowBuffer["ASSETNA"] = "wMain";
              rowBuffer["COST"]    = 700;
              rowBuffer["ACTION"]  = "Open Cut";
              // Note that this is an invalid subtype value.
              rowBuffer[tableDefinition.GetSubtypeField()] = 4;

              row = enterpriseTable.CreateRow(rowBuffer);

              //To Indicate that the attribute table has to be updated
              context.Invalidate(row);

              invalidRowObjectID = row.GetObjectID();
            }
            catch (GeodatabaseException exObj)
            {
              Console.WriteLine(exObj);
            }
            finally
            {
              if (rowBuffer != null)
                rowBuffer.Dispose();

              if (row != null)
                row.Dispose();
            }
          }, enterpriseTable);

          editOperation.Execute();

          // This will have one keypair value for the invalid row that was added.
          IReadOnlyDictionary<long, string> result = enterpriseTable.Validate(openCutFilter);

          // This will say "invalid subtype code".
          string errorMessage = result[invalidRowObjectID];
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