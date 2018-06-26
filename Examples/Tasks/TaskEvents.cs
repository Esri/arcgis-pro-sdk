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
using ArcGIS.Desktop.TaskAssistant.Events;

namespace SDKExamples
{
  public class TaskEvents
  {
    public void MainMethodCode()
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
  }
}
