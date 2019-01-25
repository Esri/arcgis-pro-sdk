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

namespace TasksAPI
{
  public class GetTaskItemInfo
  {
    public async Task MainMethodCode()
    {
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
    }
  }}
