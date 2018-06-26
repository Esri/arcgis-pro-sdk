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

namespace SDKExamples
{
  /// <summary>
  /// Illustrates how to search from a Table.
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
  public class TableSearch
  {
    /// <summary>
    /// In order to illustrate that Geodatabase calls have to be made on the MCT
    /// </summary>
    /// <returns></returns>
    public async Task TableSearchAsync()
    {
      await QueuedTask.Run(() => MainMethodCode());
    }

    public void MainMethodCode()
    {
      using (Geodatabase geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(@"C:\Data\LocalGovernment.gdb"))))
      using (Table table             = OpenTable(geodatabase, "EmployeeInfo"))
      {
        // If you are not sure if EmployeeInfo Exists...
        if (table == null)
          return;

        // If you want to make sure the field Name exists...
        TableDefinition tableDefinition = table.GetDefinition();
        if (tableDefinition.FindField("COSTCTRN") < 0)
          //This could be a custom exception...
          throw new Exception("The desired Field Name does not exist. Need to investigate why this is missing");

        // ******************** WITHOUT USING RECYCLING ********************

        List<Row> informationTechnologyEmployees = null;
        List<Row> nullList                       = null;
        List<Row> possiblyEmptyListOfRows        = null;
        List<Row> partiallyPopulatedRows         = null;
        List<Row> distinctCombinationRows        = null;
        List<Row> orderedRows                    = null;

        try
        {
          // This should return a list of rows if the Field Name exists.
          informationTechnologyEmployees = GetRowListFor(table, new QueryFilter {
            WhereClause = "COSTCTRN = 'Information Technology'"
          });

          // This should return a null since EmployeeInfo Table does not have an ADDRESS field.
          nullList = GetRowListFor(table, new QueryFilter {
            WhereClause = "ADDRESS = 'Something'"
          });

          // This should return an empty list Since there is a mismatch in the case of the requested CostCenter Name and the actual.
          possiblyEmptyListOfRows = GetRowListFor(table, new QueryFilter {
            WhereClause = "COSTCTRN = 'Water'"
          });

          // This should return a list of Rows with only OBJECTID, KNOWNAS and EMAIL fields populated. Everything else will be null.  
          partiallyPopulatedRows = GetRowListFor(table, new QueryFilter {
            WhereClause = "COSTCTRN = 'Information Technology'",
            SubFields   = "KNOWNAS, EMAIL"
          });

          Row anyRow = partiallyPopulatedRows.First();
          // Keep in mind that the FindField method is provided as a convenience method. It is a costly operation where 
          // all the fields are enumerated to find the index. So, you might want to be judicious in using it.
          int knownAsFieldIndex = anyRow.FindField("KNOWNAS");
          int emailFieldIndex   = anyRow.FindField("EMAIL");

          foreach (Row partiallyPopulatedRow in partiallyPopulatedRows)
          {
            //do something with
            object knownAsValue = partiallyPopulatedRow[knownAsFieldIndex];
            object emailValue   = partiallyPopulatedRow[emailFieldIndex];
          }

          // This should return a list of Rows with name and location of one Elected Official per Wing .
          distinctCombinationRows = GetRowListFor(table, new QueryFilter {
            WhereClause  = "COSTCTRN = 'Elected Officials'",
            SubFields    = "KNOWNAS, LOCATION, WING",
            PrefixClause = "DISTINCT"
          });

          // This should return a list of Rows ordered by the office numbers of the IT employees.
          orderedRows = GetRowListFor(table, new QueryFilter {
            WhereClause   = "COSTCTRN = 'Information Technology'",
            SubFields     = "KNOWNAS, OFFICE, LOCATION",
            PostfixClause = "ORDER BY OFFICE"
          });
        }
        finally
        {
          Dispose(informationTechnologyEmployees);
          Dispose(nullList);
          Dispose(possiblyEmptyListOfRows);
          Dispose(partiallyPopulatedRows);
          Dispose(distinctCombinationRows);
          Dispose(orderedRows);
        }
        
        //************************ USING RECYCLING *****************************

        using (RowCursor rowCursor = table.Search(new QueryFilter
          {
            WhereClause  = "COSTCTRN = 'Elected Officials'",
            SubFields    = "KNOWNAS, LOCATION, WING",
            PrefixClause = "DISTINCT"
          }))
        {
          while (rowCursor.MoveNext())
          {
            // Do something with rowCursor.Current.  Also, remember to Dispose of it when done processing.
          }
        }
        
        // If you use try to assign the rowCursor.Current to Row references...

        using (RowCursor rowCursor = table.Search(new QueryFilter
          {
            WhereClause  = "COSTCTRN = 'Elected Officials'",
            SubFields    = "KNOWNAS, LOCATION, WING",
            PrefixClause = "DISTINCT"
          }))
        {
          List<Row> rows = new List<Row>();
          Row lastRow    = null;

          while (rowCursor.MoveNext())
          {
            rows.Add(rowCursor.Current);
            lastRow = rowCursor.Current;
          }

          // After the loop is done, lastRow will point to the last Row that was returned by the enumeration.  Each row in the rows
          // list will be pointing to the same Row Object as lastRow, which is the last object that was enumerated by the rowCursor
          // enumerator before moving past the last result, i.e. for each row in rows, the condition row == lastRow will be true.
          // Since Row encapsulates unmanaged resources, it is important to remember to call Dispose() on every entry in the list
          // when the list is no longer in use.   Alternatively, do not add the row to the list.  Instead, process each of them 
          // inside the cursor.

          Dispose(rows);
        }
      }
    }

    /// <summary>
    /// Searches a given Table to return the content of the complete row.  Note that this method is not using Recycling
    /// </summary>
    /// <remarks>using ArcGIS.Core.Data expected </remarks>
    /// <note>ReturnValue is typeof List&lt;Row&gt; </note>
    private List<Row> GetRowListFor(Table table, QueryFilter queryFilter)
    {
      List<Row> rows = new List<Row>();

      try
      {
        using (RowCursor rowCursor = table.Search(queryFilter, false))
        {
          while (rowCursor.MoveNext())
          {
            rows.Add(rowCursor.Current);
          }
        }
      }
      catch (GeodatabaseFieldException fieldException)
      {
        // One of the fields in the where clause might not exist. There are multiple ways this can be handled:
        // 1. You could rethrow the exception to bubble up to the caller after some debug or info logging 
        // 2. You could return null to signify that something went wrong. The logs added before return could tell you what went wrong.
        // 3. You could return empty list of rows. This might not be advisable because if there was no exception thrown and the
        //    query returned no results, you would also get an empty list of rows. This might cause problems when 
        //    differentiating between a failed Search attempt and a successful search attempt which gave no result.

        // logger.Error(fieldException.Message);
        return null;
      }
      catch (Exception exception)
      {
        // logger.Error(exception.Message);
        return null;
      }

      return rows;
    }

    /// <summary>
    /// Opens a table and returns reference if it exists
    /// </summary>
    private static Table OpenTable(Geodatabase geodatabase, string tableName)
    {
      Table table;
      try
      {
        table = geodatabase.OpenDataset<Table>(tableName);
      }
      catch (GeodatabaseCatalogDatasetException exception)
      {
        // logger.Error(exception.Message);
        return null;
      }
      return table;
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