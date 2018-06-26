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
using System.IO;
using System.Threading.Tasks;
using ArcGIS.Core.Data;
using ArcGIS.Desktop.Framework.Threading.Tasks;

namespace SDKExamples.GeodatabaseSDK
{
  /// <summary>
  /// Illustrates how to retrieve an Attachment object from a Row or Feature.
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
  public class RowGetAttachments
  {
    /// <summary>
    /// In order to illustrate that Geodatabase calls have to be made on the MCT
    /// </summary>
    /// <returns></returns>
    public async Task RowGetAttachmentsAsync()
    {
      await QueuedTask.Run(() => MainMethodCode());
    }

    public void MainMethodCode()
    {
      using (Geodatabase fileGeodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(@"C:\Data\LocalGovernment.gdb"))))
      using (Table employeeTable         = fileGeodatabase.OpenDataset<Table>("EmployeeInfo"))
      {
        // This will be false because the "EmployeeInfo" table is not enabled for attachments.

        bool attachmentEnabledStatus = employeeTable.IsAttachmentEnabled();

        using (Table inspectionTable = fileGeodatabase.OpenDataset<Table>("luCodeInspection"))
        {
          QueryFilter queryFilter = new QueryFilter { WhereClause = "ACTION = '1st Notice'" };

          using (RowCursor cursor = inspectionTable.Search(queryFilter)) // Using Recycling.
          {
            // Adding some attachments to illustrate getting specific attachments.

            bool hasRow = cursor.MoveNext();

            using (Row currentRow = cursor.Current)
            {
              // The contentType is the MIME type indicating the type of file attached.

              Attachment pngAttachment = new Attachment("ImageAttachment.png", "image/png", CreateMemoryStreamFromContentsOf("geodatabaseAttachment.png"));
              long firstAttachmentId = currentRow.AddAttachment(pngAttachment);

              Attachment textAttachment = new Attachment("TextAttachment.txt", "text/plain", CreateMemoryStreamFromContentsOf("Sample.txt"));
              long secondAttachmentId = currentRow.AddAttachment(textAttachment);

              IReadOnlyList<Attachment> allAttachments = currentRow.GetAttachments();
              int count = allAttachments.Count; // This will be 2 provided there were no attachments before we added attachments.

              // This will only give the attachment object for the first attachment id.

              IReadOnlyList<Attachment> firstAttachmentOnlyList = currentRow.GetAttachments(new List<long> { firstAttachmentId });

              // This will only give the attachment object without the Data for the second attachment id.

              IReadOnlyList<Attachment> secondAttachmentInfoOnlyList = currentRow.GetAttachments(new List<long> { secondAttachmentId }, true);
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

      using (Geodatabase geodatabase              = new Geodatabase(connectionProperties))
      using (FeatureClass landUseCaseFeatureClass = geodatabase.OpenDataset<FeatureClass>("LocalGovernment.GDB.LandUseCase"))
      {
        QueryFilter filter = new QueryFilter { WhereClause = "CASETYPE = 'Rezoning'" };

        using (RowCursor landUseCursor = landUseCaseFeatureClass.Search(filter, false))
        {
          List<Attachment> rezoningAttachments = new List<Attachment>();

          while (landUseCursor.MoveNext())
          {
            Feature rezoningUseCase = (Feature)landUseCursor.Current;

            rezoningAttachments.AddRange(rezoningUseCase.GetAttachments());
          }

          foreach (Attachment attachment in rezoningAttachments)
          {
            // Process rezoning attachments in someway.
          }
        }
      }
    }

    /// <summary>
    /// This is one way of converting the content of any file into a MemoryStream
    /// </summary>
    /// <param name="fileNameWithPath"></param>
    /// <returns></returns>
    private static MemoryStream CreateMemoryStreamFromContentsOf(String fileNameWithPath)
    {
      MemoryStream memoryStream = new MemoryStream();

      using (FileStream file = new FileStream(fileNameWithPath, FileMode.Open, FileAccess.Read))
      {
        byte[] bytes = new byte[file.Length];
        file.Read(bytes, 0, (int)file.Length);
        memoryStream.Write(bytes, 0, (int)file.Length);
      }

      return memoryStream;
    }  
  }
}