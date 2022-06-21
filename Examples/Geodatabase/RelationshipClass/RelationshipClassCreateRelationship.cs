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
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Framework.Threading.Tasks;

namespace SDKExamples.GeodatabaseSDK
{
  /// <summary>
  /// Illustrates how to create a relationship in a RelationshipClass.
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
  public class RelationshipClassCreateRelationship
  {
    /// <summary>
    /// In order to illustrate that Geodatabase calls have to be made on the MCT
    /// </summary>
    /// <returns></returns>
    public async Task RelationshipClassGetRowsRelatedToOriginDestinationAsync()
    {
      await QueuedTask.Run(MainMethodCode);
    }

    public async Task MainMethodCode()
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
      using (RelationshipClass relationshipClass = geodatabase.OpenDataset<RelationshipClass>("LocalGovernment.GDB.OverviewToProject"))
      using (FeatureClass projectsFeatureClass   = geodatabase.OpenDataset<FeatureClass>("LocalGovernment.GDB.CIPProjects"))
      using (FeatureClass overviewFeatureClass   = geodatabase.OpenDataset<FeatureClass>("LocalGovernment.GDB.CIPProjectsOverview"))
      {
        FeatureClassDefinition overviewDefinition = overviewFeatureClass.GetDefinition();
        // Because the following fields are used in 2 places, it is more inefficient to get their respective indexes so that they can be resued.
        int projectManagerIndex = overviewDefinition.FindField("PROJECTMAN");
        int fundSourceIndex     = overviewDefinition.FindField("FUNDSOUR");

        RelationshipClassDefinition relationshipClassDefinition = relationshipClass.GetDefinition();
        // This will be PROJNAME. This can be used to get the field index or used directly as the field name.
        string originKeyField = relationshipClassDefinition.GetOriginKeyField();
        
        EditOperation editOperation = new EditOperation();
        editOperation.Callback(context => 
        {
          // The rows are being added to illustrate adding relationships. If one has existing rows, those can be used to add a relationship.
          RowBuffer projectsRowBuffer  = projectsFeatureClass.CreateRowBuffer();
          projectsRowBuffer["TOTCOST"] = 500000;

          RowBuffer overviewRowBufferWithoutPKValue            = overviewFeatureClass.CreateRowBuffer();
          overviewRowBufferWithoutPKValue[projectManagerIndex] = "John Doe";
          overviewRowBufferWithoutPKValue[fundSourceIndex]     = "Public";

          RowBuffer overviewRowBuffer            = overviewFeatureClass.CreateRowBuffer();
          overviewRowBuffer["PROJNAME"]          = "LibraryConstruction";
          overviewRowBuffer[projectManagerIndex] = "John Doe";
          overviewRowBuffer[fundSourceIndex]     = "Public";

          using (Row projectsRow               = projectsFeatureClass.CreateRow(projectsRowBuffer))
          using (Row overviewRowWithoutPKValue = overviewFeatureClass.CreateRow(overviewRowBufferWithoutPKValue))
          using (Row overviewRow               = overviewFeatureClass.CreateRow(overviewRowBuffer))
          {
            try
            {
              Relationship failingRelationship = relationshipClass.CreateRelationship(overviewRowWithoutPKValue, projectsRow);
            }
            catch (ArcGIS.Core.Data.Exceptions.GeodatabaseRelationshipClassException exception)
            {
              // This will have a message "Unable to obtain origin primary key value.". So, the origin row needs to have the origin *primary*   
              // key value referenced by the origin *foreign* key value in the RelationshipClass.
              Console.WriteLine(exception);
            }

            Relationship relationship = relationshipClass.CreateRelationship(overviewRow, projectsRow);

            //To Indicate that the Map has to draw this feature/row and/or the attribute table has to be updated
            context.Invalidate(projectsRow);
            context.Invalidate(overviewRow);
            context.Invalidate(relationshipClass);
          }          
        }, projectsFeatureClass, overviewFeatureClass);

        bool editResult = editOperation.Execute();

        //If the table is non-versioned this is a no-op. If it is versioned, we need the Save to be done for the edits to be persisted.
        bool saveResult = await Project.Current.SaveEditsAsync();
      }
    }
  }
}