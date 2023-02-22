/*

   Copyright 2022 Esri

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
using ArcGIS.Core.Data;
using ArcGIS.Core.Data.UtilityNetwork;
using ArcGIS.Core.Data.UtilityNetwork.Trace;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Workflow;
using ArcGIS.Desktop.Workflow.Client;
using ArcGIS.Desktop.Workflow.Client.Models;

namespace WorkflowManagerProSnippets

{
  class ProSnippetsWorkflowManager
  {

    public static async Task GetIsConnected()
    {
      // cref: ArcGIS.Desktop.Workflow.Client.WorkflowClientModule.IsConnected
      #region How to determine if there is an active Workflow Manager connection

      // determine if there is an active Workflow Manager connection
      var isConnected = WorkflowClientModule.IsConnected;
      #endregion
    }

    public static async Task GetItemId()
    {
      // cref: ArcGIS.Desktop.Workflow.Client.WorkflowClientModule.ItemId
      #region How to get the Workflow Manager item Id

      // Get the Workflow Manager item Id
      var itemId = WorkflowClientModule.ItemId;
      #endregion
    }

    public static async Task GetServerUrl()
    {
      // cref: ArcGIS.Desktop.Workflow.Client.WorkflowClientModule.ServerUrl
      #region How to get the Workflow Manager server url

      // Get the Workflow Manager server url
      var serverUrl = WorkflowClientModule.ServerUrl;
      #endregion
    }

    public static async Task GetJobId()
    {
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager.GetJobId()
      #region How to get the job Id associated with the active map view

      // Get the job Id associated with the active map view
      var jobManager = WorkflowClientModule.JobsManager;
      var jobId = jobManager.GetJobId();
      #endregion
    }

    public static async Task GetJobId(string mapUri)
    {
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager.GetJobId()
      #region How to get the job Id associated with a map

      // Get the job Id associated with a map
      mapUri = "myMapUri"; // Get a reference to a map using the ArcGIS.Desktop.Mapping API (active view, project item, etc.)
      var jobManager = WorkflowClientModule.JobsManager;
      var jobId = jobManager.GetJobId(mapUri);
      #endregion
    }
  }
}
