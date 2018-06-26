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

namespace SDKExamples
{
  /// <summary>
  /// Illustrates how to open a Dataset from a geodatabase.
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
  public class GeodatabaseOpenDataset
  {
    /// <summary>
    /// In order to illustrate that Geodatabase calls have to be made on the MCT
    /// </summary>
    /// <returns></returns>
    public async Task GeodatabaseOpenDatasetAsync()
    {
      await QueuedTask.Run(() => MainMethodCode());
    }

    public void MainMethodCode()
    {
      // To run this code import the LocalGovernment.gdb to a sql server instance with the following settings.

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
      {
        // Safe cast to Table to get a table reference.
        using (Table table = geodatabase.OpenDataset<Table>("LocalGovernment.GDB.EmployeeInfo"))
        {
        }
        
        // Open a featureClass (within a feature dataset or outside a feature dataset).
        using (FeatureClass featureClass = geodatabase.OpenDataset<FeatureClass>("LocalGovernment.GDB.AddressPoint"))
        {
        }
        
        // You can open a FeatureClass as a Table which will give you a Table Reference.
        using (Table featureClassAsTable = geodatabase.OpenDataset<Table>("LocalGovernment.GDB.AddressPoint"))
        {
          // But it is really a FeatureClass object.
          FeatureClass featureClassOpenedAsTable = featureClassAsTable as FeatureClass;
        }
        
        // Open a FeatureDataset.
        using (FeatureDataset featureDataset = geodatabase.OpenDataset<FeatureDataset>("LocalGovernment.GDB.Address"))
        {
        }
        
        // Open a RelationsipClass.  Just as you can open a FeatureClass as a Table, you can also open an AttributedRelationshipClass as a RelationshipClass.
        using (RelationshipClass relationshipClass = geodatabase.OpenDataset<RelationshipClass>("LocalGovernment.GDB.AddressPointHasSiteAddresses"))
        {
        }

        try
        {
          geodatabase.OpenDataset<Table>(null);
        }
        catch (ArgumentException exception)
        {
          //Providing a null for dataset name gives argument exception
        }

        try
        {
          geodatabase.OpenDataset<Table>("LocalGovernment.GDB.Something");
        }
        catch (InvalidOperationException exception)
        {
          // Providing a non-existent dataset name gives Invalid Operation Exception
        }
      }
    }
  }
}