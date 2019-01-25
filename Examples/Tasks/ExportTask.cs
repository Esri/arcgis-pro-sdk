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
using System.Linq;
using System.Threading.Tasks;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.TaskAssistant;

namespace SDKExamples
{
  public class ExportTask
  {
    public async Task MainMethodCode()
    {
      // get the first project task item
      var taskItem = Project.Current.GetItems<TaskProjectItem>().FirstOrDefault();
      // if there isn't a project task item, return
      if (taskItem == null)
        return;

      try
      {
        // export the task item to the c:\Temp folder
        string exportFolder = @"c:\temp";
        string fileName = await TaskAssistantModule.ExportTaskAsync(taskItem.TaskItemGuid, exportFolder);
        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Task saved to " + fileName);
      }
      catch (ExportTaskException e)
      {
        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Error saving task " + e.Message);
      }
    }
  }
}
