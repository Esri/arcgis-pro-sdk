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
using ArcGIS.Desktop.TaskAssistant.Events;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework.Threading.Tasks;

namespace ProSnippetsTasks 
{
  class TasksCodeExamples 
  {

    public async void OpenTask()
    {
      #region Retrieve all the Task Items in a Project
      IEnumerable<TaskProjectItem> taskItems = Project.Current.GetItems<TaskProjectItem>();
      foreach (var item in taskItems)
      {
        // do something
      }

      #endregion

      #region Open a Task File - .esriTasks file
      // Open a task file
      try
      {
        // TODO - substitute your own .esriTasks file to be opened
        string taskFile = @"c:\Tasks\Get Started.esriTasks";
        System.Guid guid = await TaskAssistantModule.OpenTaskAsync(taskFile);

        // TODO - retain the guid returned for use with CloseTaskAsync
      }
      catch (OpenTaskException e)
      {
        // exception thrown if task file doesn't exist or has incorrect format
        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(e.Message);
      }

      #endregion

      #region Open a Project Task Item
      // get the first project task item
      var taskItem = Project.Current.GetItems<TaskProjectItem>().FirstOrDefault();
      // if there isn't a project task item, return
      if (taskItem == null)
        return;

      try
      {
        // Open it
        System.Guid guid = await TaskAssistantModule.OpenTaskItemAsync(taskItem.TaskItemGuid);

        // TODO - retain the guid returned for use with CloseTaskAsync
      }
      catch (OpenTaskException e)
      {
        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(e.Message);
      }
      #endregion
    }

    public void CloseTaskItem()
    {
      #region Close a Task Item
      // find the first project task item which is open
      var taskItem = Project.Current.GetItems<TaskProjectItem>().FirstOrDefault(t => t.IsOpen == true);
      // if there isn't a project task item, return
      if (taskItem == null)
        return;

      // close it
      // NOTE : The task item will also be removed from the project
      TaskAssistantModule.CloseTaskAsync(taskItem.TaskItemGuid);

      #endregion
    }

    public async void ExportTaskItem()
    {
      #region Export a Task Item
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
      #endregion Export a Task Item
    }

    public async void GetTaskItemInfo_ProjectItem()
    {
      #region Get Task Information - from a TaskProjectItem

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
      #endregion
    }

    public async void GetTaskItemInfo_EsriTasksFile()
    { 
      #region Get Task Information - from an .esriTasks file

      // TODO - substitute your own .esriTasks file
      string taskFile = @"c:\Tasks\Get Started.esriTasks";

      try
      {
        // retrieve the task item information
        TaskItemInfo taskItemInfo = await TaskAssistantModule.GetTaskItemInfoAsync(taskFile);

        string message = "Name : " + taskItemInfo.Name;
        message += "\r\n" + "Description : " + taskItemInfo.Description;
        message += "\r\n" + "Guid : " + taskItemInfo.Guid.ToString("B");
        message += "\r\n" + "Task Count : " + taskItemInfo.GetTasks().Count();

        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(message, "Task Information");
      }
      catch (OpenTaskException e)
      {
        // exception thrown if task file doesn't exist or has incorrect format
        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(e.Message, "Task Information");
      }
      catch (TaskFileVersionException e)
      {
        // exception thrown if task file does not support returning task information
        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(e.Message, "Task Information");
      }
      #endregion Get Task item information
    }

    public async void OpenSpecificTask()
    {
      #region Open a specific Task in a Task File - .esriTasks file

      // TODO - substitute your own .esriTasks file to be opened
      string taskFile = @"c:\Tasks\Get Started.esriTasks";

      try
      {
        // retrieve the task item information
        TaskItemInfo taskItemInfo = await TaskAssistantModule.GetTaskItemInfoAsync(taskFile);

        // find the first task
        TaskInfo taskInfo = taskItemInfo.GetTasks().FirstOrDefault();

        Guid guid = Guid.Empty;
        if (taskInfo != null)
        {
          // if a task exists, open it
          guid = await TaskAssistantModule.OpenTaskAsync(taskFile, taskInfo.Guid);
        }
        else
        {
          // else just open the task item
          guid = await TaskAssistantModule.OpenTaskAsync(taskFile);
        }

        // TODO - retain the guid returned for use with CloseTaskAsync 
      }
      catch (OpenTaskException e)
      {
        // exception thrown if task file doesn't exist or has incorrect format
        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(e.Message);
      }
      catch (TaskFileVersionException e)
      {
        // exception thrown if task file does not support returning task information
        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(e.Message);
      }
      #endregion
    }

    #region Subscribe to Task Events
    public void TaskEvents()
    {
      TaskStartedEvent.Subscribe(OnTaskStarted);
      TaskEndedEvent.Subscribe(OnTaskCompletedOrCancelled);
    }

    private void OnTaskStarted(TaskStartedEventArgs args)
    {
      string userName = args.UserID;    // ArcGIS Online signed in userName.  If not signed in to ArcGIS Online then returns the name of the user logged in to the Windows OS.
      string projectName = args.ProjectName;

      Guid taskItemGuid = args.TaskItemGuid;
      string taskItemName = args.TaskItemName;
      string taskItemVersion = args.TaskItemVersion;

      Guid taskGuid = args.TaskGuid;
      string taskName = args.TaskName;

      DateTime startTime = args.StartTime;
    }

    private void OnTaskCompletedOrCancelled(TaskEndedEventArgs args)
    {
      string userName = args.UserID;    // ArcGIS Online signed in userName.  If not signed in to ArcGIS Online then returns the name of the user logged in to the Windows OS.
      string projectName = args.ProjectName;

      Guid taskItemGuid = args.TaskItemGuid;
      string taskItemName = args.TaskItemName;
      string taskItemVersion = args.TaskItemVersion;

      Guid taskGuid = args.TaskGuid;
      string taskName = args.TaskName;

      DateTime startTime = args.StartTime;
      DateTime endTime = args.EndTime;
      double duration = args.Duration;

      bool completed = args.Completed;    // completed or cancelled
    }
      #endregion
  }
}
