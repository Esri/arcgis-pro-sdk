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
  /// Illustrates how to get a Dataset's Definition from a geodatabase.
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
  public class GeodatabaseGetDefinition
  {
    /// <summary>
    /// In order to illustrate that Geodatabase calls have to be made on the MCT
    /// </summary>
    /// <returns></returns>
    public async Task GeodatabaseGetDefinitionAsync()
    {
      await QueuedTask.Run(() => MainMethodCode());
    }

    public void MainMethodCode()
    {
      // Opens a file geodatabase  This will open the geodatabase if the folder exists and contains a valid geodatabase.

      using (Geodatabase fileGeodatabase     = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(@"C:\Data\LocalGovernment.gdb"))))
      {
        TableDefinition tableDefinition = fileGeodatabase.GetDefinition<TableDefinition>("CitizenContactInfo");

        // Use the fileGeodatabase and tableDefinition.
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
      
      using (Geodatabase geodatabase = new Geodatabase((connectionProperties)))
      {
        // Remember that for Enterprise databases you have to qualify your dataset names with the DatabaseName and UserName.
        TableDefinition enterpriseTableDefinition = geodatabase.GetDefinition<TableDefinition>("LocalGovernment.GDB.CitizenContactInfo");

        // It does not matter if the dataset is within a FeatureDataset or not.
        FeatureClassDefinition featureClassDefinition = geodatabase.GetDefinition<FeatureClassDefinition>("LocalGovernment.GDB.FireStation");

        // GetDefinition For a RelationshipClass.
        RelationshipClassDefinition relationshipClassDefinition = geodatabase.GetDefinition<RelationshipClassDefinition>("LocalGovernment.GDB.AddressPointHasSiteAddresses");

        // GetDefinition For a FeatureDataset.
        FeatureDatasetDefinition featureDatasetDefinition = geodatabase.GetDefinition<FeatureDatasetDefinition>("LocalGovernment.GDB.Address")
          ;
        try
        {
          // Note that although you can open a FeatureClass as a Table using OpenDataset, you cannot call GetDefinition on a FeatureClass with DatasetType.Table.
          geodatabase.GetDefinition<TableDefinition>("LocalGovernment.GDB.FireStation");

          // Attempting to open a FeatureDataset as a RelationshipClass will fail. 
          // This is an example i.e. any mismatch between DatasetType specified and Actual DatasetType will cause GetDefinition to fail.
          geodatabase.GetDefinition<RelationshipClassDefinition>("LocalGovernment.GDB.Address");

          // If you do not qualify your Dataset Name GetDefinition fails. Note that this worked for File Geodatabase.
          geodatabase.GetDefinition<TableDefinition>("CitizenContactInfo");

          // Trying to GetDefinition of a non existent dataset.
          geodatabase.GetDefinition<TableDefinition>("SomeRandomString");
        }
        catch (GeodatabaseCatalogDatasetException catalogDatasetException)
        {
          // All of the above scenarios will result in GeodatabaseCatalogDatasetException with a message "Item Not Found".
          // Remember that when you get this exception, most likely the following are the reasons.
          // 1. The Item might not exist.
          // 2. There might be a mismatch between DatasetType specified and Actual DatasetType.
          // 3. The qualification for the DatasetName is wrong.
          // 4. The User does not have visibility to the dataset
        }
      }
    }
  }
}