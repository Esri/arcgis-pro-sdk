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
using ArcGIS.Desktop.Framework.Threading.Tasks;

namespace SDKExamples.GeodatabaseSDK
{
  /// <summary>
  /// Illustrates how to open dataset from a FeatureDataset.
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
  public class FeatureDatasetOpenDataset
  {
    /// <summary>
    /// In order to illustrate that Geodatabase calls have to be made on the MCT
    /// </summary>
    /// <returns></returns>
    public async Task FeatureDatasetOpenDatasetAsync()
    {
      await QueuedTask.Run(() => MainMethodCode());
    }

    public void MainMethodCode()
    {
      using (Geodatabase fileGeodatabase         = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(@"C:\Data\LocalGovernment.gdb"))))
      using (FeatureDataset featureDataset       = fileGeodatabase.OpenDataset<FeatureDataset>("Address"))
      using (FeatureClass featureClass           = featureDataset.OpenDataset<FeatureClass>("AddressPoint"))
      using (RelationshipClass relationshipClass = featureDataset.OpenDataset<RelationshipClass>("AddressPointHasSiteAddresses"))
      {
        IReadOnlyList<Definition> definitions = fileGeodatabase.GetRelatedDefinitions(featureDataset.GetDefinition(), DefinitionRelationshipType.DatasetInFeatureDataset);

        foreach (Definition definition in definitions)
        {
          Dataset dataset = null;

          try
          {
            if (definition.DatasetType == DatasetType.Table)
            {
              dataset = featureDataset.OpenDataset<Table>(definition.GetName());
            }
            else if (definition.DatasetType == DatasetType.FeatureClass)
            {
              dataset = featureDataset.OpenDataset<FeatureClass>(definition.GetName());
            }
            else if (definition.DatasetType == DatasetType.RelationshipClass)
            {
              dataset = featureDataset.OpenDataset<RelationshipClass>(definition.GetName());
            }
            else if (definition.DatasetType == DatasetType.AttributedRelationshipClass)
            {
              dataset = featureDataset.OpenDataset<AttributedRelationshipClass>(definition.GetName());
            }
              
            // Do something with dataset
          }
          catch (InvalidOperationException exception)
          {
            //This exception is thrown if one of the datasets in the FeatureDatasets are not supported and thus marked as Unknown
            Console.WriteLine(exception);
          }
          finally
          {
            // Should explicitly dispose of a Dataset.

            if (dataset != null)
              dataset.Dispose();
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
      
      using (Geodatabase geodatabase                 = new Geodatabase(connectionProperties))
      using (FeatureDataset enterpriseFeatureDataset = geodatabase.OpenDataset<FeatureDataset>("LocalGovernment.GDB.ReferenceData"))
      using (FeatureClass communityCenter            = enterpriseFeatureDataset.OpenDataset<FeatureClass>("LocalGovernment.GDB.CommunityCenter"))
      using (FeatureClass library                    = enterpriseFeatureDataset.OpenDataset<FeatureClass>("LocalGovernment.GDB.Library"))
      {
        try
        {
          // Trying to open non-existent RelationshipClasses and AttributedRelationshipClasses.
          Dataset badRelationshipClass           = enterpriseFeatureDataset.OpenDataset<RelationshipClass>("SomeGibberish");
          Dataset badAttributedRelationshipClass = enterpriseFeatureDataset.OpenDataset<AttributedRelationshipClass>("SomeGibberish");
        }
        catch (ArcGIS.Core.Data.Exceptions.GeodatabaseCatalogDatasetException exception)
        {
          // Thrown for RelationshipClass and AttributedRelationshipClass.
        }

        try
        {
          // Trying to open non-existent FeatureClasses.
          Dataset badFeatureClass = enterpriseFeatureDataset.OpenDataset<FeatureClass>("SomeGibberish");
        }
        catch (ArcGIS.Core.Data.Exceptions.GeodatabaseDatasetException exception)
        {
          // Thrown for FeatureClasses.
        }

        try
        {
          // Tables and FeatureDatasets cannot exist in a FeatureDataset.
          Dataset badFeatureDataset = enterpriseFeatureDataset.OpenDataset<FeatureDataset>("SomeGibberish");
          Dataset badTable          = enterpriseFeatureDataset.OpenDataset<Table>("SomeGibberish");
        }
        catch (InvalidOperationException exception)
        {
          // Thrown for Table and FeatureDataset.
        }
      }
    }
  }
}