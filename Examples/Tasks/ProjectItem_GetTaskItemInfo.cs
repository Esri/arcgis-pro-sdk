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
using System.Text;
using System.Threading.Tasks;
using ArcGIS.Desktop.TaskAssistant;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework.Threading.Tasks;

namespace TasksAPI
{
  public class ProjectItem_GetTaskItemInfo
  {
    public async Task Method1Code()
    {
      // find the first task item in the project
      var taskItem = Project.Current.GetItems<TaskProjectItem>().FirstOrDefault();
      // if there isn't a project task item, return
      if (taskItem == null)
        return;

      string message = await QueuedTask.Run(async () =>
      {
        bool isOpen = taskItem.IsOpen;
        Guid taskGuid = taskItem.TaskItemGuid;

        string msg = "";
        try
        {
          TaskItemInfo taskItemInfo = await taskItem.GetTaskItemInfoAsync();

          msg = "Name : " + taskItemInfo.Name;
          msg += "\r\n" + "Description : " + taskItemInfo.Description;
          msg += "\r\n" + "Guid : " + taskItemInfo.Guid.ToString("B");
          msg += "\r\n" + "Task Count : " + taskItemInfo.GetTasks().Count();

          // iterate the tasks in the task item
          IEnumerable<TaskInfo> taskInfos = taskItemInfo.GetTasks();
          foreach (TaskInfo taskInfo in taskInfos)
          {
            string name = taskInfo.Name;
            Guid guid = taskInfo.Guid;

            // do something 
          }
        }
        catch (OpenTaskException e)
        {
          // exception thrown if task file doesn't exist or has incorrect format
          msg = e.Message;
        }
        catch (TaskFileVersionException e)
        {
          // exception thrown if task file does not support returning task information
          msg = e.Message;
        }
        return msg;
      });

      ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(message, "Task Information");
    }
  }
}
