/*

   Copyright 2023 Esri

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
using ArcGIS.Desktop.Workflow.Client.Exceptions;
using ArcGIS.Desktop.Workflow.Client.Models;
using ArcGIS.Desktop.Workflow.Client.Events;

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
      // cref: ArcGIS.Desktop.Workflow.Client.WorkflowClientModule.JobsManager
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
      // cref: ArcGIS.Desktop.Workflow.Client.WorkflowClientModule.JobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager.GetJobId(System.String)
      #region How to get the job Id associated with a map

      // Get the job Id associated with a map
      mapUri = "myMapUri"; // Get a reference to a map using the ArcGIS.Desktop.Mapping API (active view, project item, etc.)
      var jobManager = WorkflowClientModule.JobsManager;
      var jobId = jobManager.GetJobId(mapUri);
      #endregion
    }

    public static async Task GetJob(string jobId)
    {
      // cref: ArcGIS.Desktop.Workflow.Client.WorkflowClientModule.JobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager.GetJob(System.String,System.Boolean,System.Boolean)
      // cref: ArcGIS.Desktop.Workflow.Client.Models.Job
      #region How to get a job

      // GetJob returns an existing job
      try
      {
        var jobManager = WorkflowClientModule.JobsManager;
        var job = jobManager.GetJob(jobId);
        // Do something with the job
      }
      catch (NotConnectedException)
      {
        // Not connected to Workflow Manager server, do some error handling
      }
      #endregion
    }

    public static async Task SearchJobs()
    {
      // cref: ArcGIS.Desktop.Workflow.Client.WorkflowClientModule.JobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Swagger.Api.JobsApi.SearchJobs(System.String,System.String,System.String,ArcGIS.Desktop.Workflow.Client.Swagger.Model.EsriWorkflowModelsRestJobQuery)
      // cref: ArcGIS.Desktop.Workflow.Client.Models.SearchQuery
      // cref: ArcGIS.Desktop.Workflow.Client.Models.SearchResult
      #region Search for jobs using a detailed query

      var search = new SearchQuery()
      {
        // Search for all open high priority jobs assigned to users
        Q = "closed=0 AND assignedType='User' AND priority='High'",
        Fields = new List<string> { "jobId", "jobName", "assignedTo", "dueDate" },
        // Sort by job assignment in ascending order and due date in descending order
        SortFields = new List<SortField> 
        { 
          new SortField() { FieldName = "assignedTo", SortOrder = ArcGIS.Desktop.Workflow.Client.Models.SortOrder.Asc },
          new SortField() { FieldName = "dueDate", SortOrder = ArcGIS.Desktop.Workflow.Client.Models.SortOrder.Desc }
        }
      };
      var jobManager = WorkflowClientModule.JobsManager;
      var searchResults = jobManager.SearchJobs(search);
      var fields = searchResults.Fields;
      var results = searchResults.Results;
      #endregion
    }

    public static async Task SearchJobsArcadeExpression()
    {
      // cref: ArcGIS.Desktop.Workflow.Client.WorkflowClientModule.JobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Swagger.Api.JobsApi.SearchJobs(System.String,System.String,System.String,ArcGIS.Desktop.Workflow.Client.Swagger.Model.EsriWorkflowModelsRestJobQuery)
      // cref: ArcGIS.Desktop.Workflow.Client.Models.SearchQuery
      // cref: ArcGIS.Desktop.Workflow.Client.Models.SearchResult
      #region Search for jobs using a detailed query with an arcade expression

      var search = new SearchQuery()
      {
        // Search for jobs assigned to the current user using the arcade expression '$currentUser'
        Q = "\"assignedType='User' AND closed=0 AND assignedTo='\" + $currentUser + \"' \"",
        Fields = new List<string> { "jobId", "jobName", "assignedTo", "dueDate"},
        // Sort by job name in ascending order
        SortFields = new List<SortField> { new SortField() { FieldName = "jobName", SortOrder = ArcGIS.Desktop.Workflow.Client.Models.SortOrder.Asc }}
      };
      var jobManager = WorkflowClientModule.JobsManager;
      var searchResults = jobManager.SearchJobs(search);
      var fields = searchResults.Fields;
      var results = searchResults.Results;
      #endregion
    }

    public static async Task SearchJobsSimpleSearch()
    {
      // cref: ArcGIS.Desktop.Workflow.Client.WorkflowClientModule.JobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Swagger.Api.JobsApi.SearchJobs(System.String,System.String,System.String,ArcGIS.Desktop.Workflow.Client.Swagger.Model.EsriWorkflowModelsRestJobQuery)
      // cref: ArcGIS.Desktop.Workflow.Client.Models.SearchQuery
      // cref: ArcGIS.Desktop.Workflow.Client.Models.SearchResult
      #region Search for jobs using a simple string

      var search = new SearchQuery() { Search = "My Search String" };
      var jobManager = WorkflowClientModule.JobsManager;
      var searchResults = jobManager.SearchJobs(search);
      var fields = searchResults.Fields;
      var results = searchResults.Results;
      #endregion
    }

    public static async Task CalculateJobStatistics()
    {
      // cref: ArcGIS.Desktop.Workflow.Client.WorkflowClientModule.JobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager.CalculateJobStatistics()
      // cref: ArcGIS.Desktop.Workflow.Client.Models.JobStatisticsQuery
      // cref: ArcGIS.Desktop.Workflow.Client.Models.JobStatistics
      #region Get statistics for jobs

      var query = new JobStatisticsQuery()
      {
        // Search for open jobs assigned to users
        Q = "\"assignedType='User' AND closed=0 \""
      };
      var jobManager = WorkflowClientModule.JobsManager;
      var results = jobManager.CalculateJobStatistics(query);
      var totalJobs = results.Total;
      #endregion
    }

    public static async Task RunSteps(string jobId)
    {
      // cref: ArcGIS.Desktop.Workflow.Client.WorkflowClientModule.JobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager.RunSteps(System.String,System.Collections.Generic.List{System.String})
      #region How to run steps on a job

      var jobManager = WorkflowClientModule.JobsManager;
      jobManager.RunSteps(jobId);
      #endregion
    }

    public static async Task RunSteps(string jobId, List<string> stepIds)
    {
      // cref: ArcGIS.Desktop.Workflow.Client.WorkflowClientModule.JobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager.RunSteps(System.String,System.Collections.Generic.List{System.String})
      #region How to run specific steps on a job

      var jobManager = WorkflowClientModule.JobsManager;
      // Specify specific current steps in a job to run
      stepIds = new List<string> { "step12345", "step67890" };
      jobManager.RunSteps(jobId, stepIds);
      #endregion
    }

    public static async Task StopSteps()
    {
      // cref: ArcGIS.Desktop.Workflow.Client.WorkflowClientModule.JobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager.StopSteps(System.String,System.Collections.Generic.List{System.String})
      #region How to stop running steps on a job

      var jobManager = WorkflowClientModule.JobsManager;
      // Get the job Id associated with the active map view
      var jobId = jobManager.GetJobId();
      // Stop the current steps in the job with the given id.
      jobManager.StopSteps(jobId);
      #endregion
    }

    public static async Task StopSteps(List<string> stepIds)
    {
      // cref: ArcGIS.Desktop.Workflow.Client.WorkflowClientModule.JobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager.StopSteps(System.String,System.Collections.Generic.List{System.String})
      #region How to stop specific running steps on a job

      var jobManager = WorkflowClientModule.JobsManager;
      // Get the job Id associated with the active map view
      var jobId = jobManager.GetJobId();
      // Specify specific running steps in a job to stop
      stepIds = new List<string> { "step12345", "step67890" };
      jobManager.StopSteps(jobId, stepIds);
      #endregion
    }

    public static async Task FinishSteps(string jobId)
    {
      // cref: ArcGIS.Desktop.Workflow.Client.WorkflowClientModule.JobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager.FinishSteps(System.String,System.Collections.Generic.List{System.String})
      #region How to finish steps on a job

      var jobManager = WorkflowClientModule.JobsManager;
      // Finish the current steps in the job with the given id.
      jobManager.FinishSteps(jobId);
      #endregion
    }

    public static async Task FinishSteps(string jobId, List<string> stepIds)
    {
      // cref: ArcGIS.Desktop.Workflow.Client.WorkflowClientModule.JobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager.FinishSteps(System.String,System.Collections.Generic.List{System.String})
      #region How to finish specific steps on a job

      var jobManager = WorkflowClientModule.JobsManager;
      stepIds = new List<string> { "step12345", "step67890" };
      jobManager.FinishSteps(jobId, stepIds);
      #endregion
    }

    public static async Task SubscribeToAWorkflowConnectionChangedEvent()
    {
      // cref: ArcGIS.Desktop.Workflow.Client.Events.WorkflowConnectionChangedEvent
      #region How to subscribe to a workflow connection changed event.
      var subscriptionToken = WorkflowConnectionChangedEvent.Subscribe(e =>
      {
        // The connection has changed - Get the user's sign in status
        var isUserSignedIn = e.IsSignedIn;
        return Task.CompletedTask;
      });
      #endregion
    }

    public static async Task UnsubscribeFromAWorkflowConnectionChangedEvent()
    {
      // cref: ArcGIS.Desktop.Workflow.Client.Events.WorkflowConnectionChangedEvent
      #region How to unsubscribe from a workflow connection changed event.
      var subscriptionToken = WorkflowConnectionChangedEvent.Subscribe(e =>
      {
        var isUserSignedIn = e.IsSignedIn;
        return Task.CompletedTask;
      });

      WorkflowConnectionChangedEvent.Unsubscribe(subscriptionToken);
      #endregion
    }
  }
}
