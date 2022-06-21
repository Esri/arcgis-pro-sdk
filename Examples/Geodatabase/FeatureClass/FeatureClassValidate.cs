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
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Framework.Threading.Tasks;

namespace SDKExamples
{
  /// <summary>
  /// Illustrates how to validate updated rows in a FeatureClass.
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
  public class FeatureClassValidate
  {
    /// <summary>
    /// In order to illustrate that Geodatabase calls have to be made on the MCT
    /// </summary>
    /// <returns></returns>
    public async Task FeatureClassValidateAsync()
    {
      await QueuedTask.Run(() => MainMethodCode());
    }

    public void MainMethodCode()
    {
      using (Geodatabase fileGeodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(@"C:\Data\LocalGovernment.gdb"))))
      using (FeatureClass featureClass   = fileGeodatabase.OpenDataset<FeatureClass>("PollingPlace"))
      {
        QueryFilter queryFilter = new QueryFilter { WhereClause = "CITY = 'Plainfield'" };
        Selection selection     = featureClass.Select(queryFilter, SelectionType.ObjectID, SelectionOption.Normal);

        // The result is a mapping between those object ids which failed validation and the reason why the validation failed (a string message).
        IReadOnlyDictionary<long, string> validationResult = featureClass.Validate(selection);
        
        RowCursor rowCursor    = featureClass.Search(queryFilter, false);
        List<Feature> features = new List<Feature>();

        try
        {
          while (rowCursor.MoveNext())
          {
            features.Add(rowCursor.Current as Feature);
          }

          // This is equivalent to the validation performed using the selection.
          IReadOnlyDictionary<long, string> equivalentValidationResult = featureClass.Validate(features);

          // Again this is equivalent to both the above results.
          IReadOnlyDictionary<long, string> anotherEquivalentResult = featureClass.Validate(queryFilter);

          SpatialQueryFilter spatialQueryFilter = new SpatialQueryFilter
          {
            FilterGeometry = new EnvelopeBuilderEx(
              new MapPointBuilderEx(1052803, 1812751).ToGeometry(), 
              new MapPointBuilderEx(1034600, 1821320).ToGeometry()).ToGeometry(),

            SpatialRelationship = SpatialRelationship.Within
          };

          IReadOnlyDictionary<long, string> spatialFilterBasedValidationResult = featureClass.Validate(spatialQueryFilter);
        }
        finally
        {
          rowCursor.Dispose();
          Dispose(features);
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

      using (Geodatabase geodatabase             = new Geodatabase(connectionProperties))
      using (FeatureClass enterpriseFeatureClass = geodatabase.OpenDataset<FeatureClass>("LocalGovernment.GDB.FacilitySite"))
      {
        QueryFilter parkFilter  = new QueryFilter { WhereClause = "FCODE = 'Park'" };
        Selection parkSelection = enterpriseFeatureClass.Select(parkFilter, SelectionType.ObjectID, SelectionOption.Normal);

        // Remember that validation cost is directly proprtional to the number of Features validated. So, select the set of features to be 
        // validated judiciously.  This will be empty because all the Park Features are valid.
        IReadOnlyDictionary<long, string> emptyDictionary = enterpriseFeatureClass.Validate(parkSelection);
        
        // We are adding an invalid feature to illustrate a case where the validation result is not empty.

        long invalidFeatureObjectID = -1;

        EditOperation editOperation = new EditOperation();
        editOperation.Callback(context =>
        {
          RowBuffer rowBuffer = null;
          Feature feature     = null;

          try
          {
            FeatureClassDefinition facilitySiteDefinition = enterpriseFeatureClass.GetDefinition();

            rowBuffer = enterpriseFeatureClass.CreateRowBuffer();

            rowBuffer["FACILITYID"] = "FAC-400";
            rowBuffer["NAME"]       = "Griffith Park";
            rowBuffer["OWNTYPE"]    = "Municipal";
            rowBuffer["FCODE"]      = "Park";
            // Note that this is an invalid subtype value.
            rowBuffer[facilitySiteDefinition.GetSubtypeField()] = 890;
            rowBuffer[facilitySiteDefinition.GetShapeField()]   = new PolygonBuilderEx(new List<Coordinate2D>
            {
              new Coordinate2D(1021570, 1880583),
              new Coordinate2D(1028730, 1880994),
              new Coordinate2D(1029718, 1875644),
              new Coordinate2D(1021405, 1875397)
            }).ToGeometry();

            feature = enterpriseFeatureClass.CreateRow(rowBuffer);

            invalidFeatureObjectID = feature.GetObjectID();
          }
          catch (ArcGIS.Core.Data.Exceptions.GeodatabaseException exObj)
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

        editOperation.Execute();

        // This will have one keypair value for the invalid row that was added.
        IReadOnlyDictionary<long, string> result = enterpriseFeatureClass.Validate(parkFilter);

        // This will say "invalid subtype code".
        string errorMessage = result[invalidFeatureObjectID];
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