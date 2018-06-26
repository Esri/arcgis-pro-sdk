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
  public class GetTasks
  {
    public void Method1Code()
    {
      // find the task item which is open
      var taskItem = Project.Current.GetItems<TaskProjectItem>().FirstOrDefault(t => t.IsOpen == true);
      // if there isn't a project task item, return
      if (taskItem == null)
        return;

      // do something with the task item... eg export it
      try
      {
        // export the task item to the c:\Temp folder
        TaskAssistantModule.ExportTaskAsync(taskItem.TaskItemGuid, "c:\\temp");
      }
      catch (ExportTaskException e)
      {
        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Error saving task " + e.Message);
      }
    }
  }
}
