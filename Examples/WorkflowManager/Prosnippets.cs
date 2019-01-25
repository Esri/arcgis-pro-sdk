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
using ArcGIS.Core.Data;
using ArcGIS.Core.Data.UtilityNetwork;
using ArcGIS.Core.Data.UtilityNetwork.Trace;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Core.Data.UtilityNetwork.NetworkDiagrams;
using ArcGIS.Desktop.Workflow;
using ArcGIS.Desktop.Workflow.Models;

namespace WorkflowManagerProSnippets

{
	class ProSnippetsWorkflowManager
	{
		public static async Task GetManagerObjectsAsync()
		{
			#region How to get managers objects

			// WorkflowModule.GetManager returns a manager of the type specified
			// keyword is currently just an empty string
			var wfCon = await WorkflowModule.ConnectAsync();
			var jobManager = wfCon.GetManager<JobsManager>();
			var configManager = wfCon.GetManager<ConfigurationManager>();
			#endregion
		}

		public static async Task GetGroupsAsync()
		{
			#region How to get groups

			// GetAllGroups returns a list of Workflow Manager groups
			var wfCon = await WorkflowModule.ConnectAsync();
			var configManager = wfCon.GetManager<ConfigurationManager>();
			var allGroups = configManager.GetAllGroups();
			#endregion
		}
		public static async Task GetUsersAsync()
		{
			#region How to get users

			// GetAllUsers returns a list of Workflow Manager users
			var wfCon = await WorkflowModule.ConnectAsync();
			var configManager = wfCon.GetManager<ConfigurationManager>();
			var allUsers = configManager.GetAllUsers();
			#endregion
		}

		public static async Task GetJobTypesAsync()
		{
			#region How to get job types

			// GetVisibleJobTypes returns a list of job types
			var wfCon = await WorkflowModule.ConnectAsync();
			var configManager = wfCon.GetManager<ConfigurationManager>();
			var jobTypes = configManager.GetVisibleJobTypes();
			#endregion
		}

		public static async Task CreateJobAsync(string jobTypeID)
		{
			#region How to create a job

			// CreateJob returns an ID of a new job
			// it is a passed a valid job type ID as an integer
			var wfCon = await WorkflowModule.ConnectAsync();
			var jobManager = wfCon.GetManager<JobsManager>();
			var jobID = jobManager.CreateNewJob(jobTypeID);
			#endregion
		}


		public static async Task GetJobAsync(string jobID)
		{
			#region How to get a job

			// GetJob returns an existing job
			// it is passed a valid job ID as an integer
			var wfCon = await WorkflowModule.ConnectAsync();
			var jobManager = wfCon.GetManager<JobsManager>();
			var job = jobManager.GetJob(jobID);
			#endregion
		}

        public static async Task GetJobAsync(Map map)
        {
            #region How to get a job associated with a map

            // Get a job associated with the map
            var wfCon = await WorkflowModule.ConnectAsync();
            var jobManager = wfCon.GetManager<JobsManager>();
            var job = jobManager.GetJob(map);
            if (job != null)
            {
                // Job found, do something with the job
                var jobId = job.ID;
            }
            #endregion
        }

        public static async Task CloseJobAsync(IReadOnlyList<string> jobIdsToClose)
		{
			#region How to close a job

			// CloseJobs returns a list of closed job IDs
			// it is passed a list of job IDs to close
			var wfCon = await WorkflowModule.ConnectAsync();
			var jobManager = wfCon.GetManager<JobsManager>();
			var jobIDs = jobManager.CloseJobs(jobIdsToClose);
			#endregion
		}

		public static async Task AccessJobChangeItAsync(string jobID)
		{
			#region How to access job info and change it

			// You can change many of the exposed properties of a job and then save them
			var wfCon = await WorkflowModule.ConnectAsync();
			var jobManager = wfCon.GetManager<JobsManager>();
			var job = jobManager.GetJob(jobID);
			job.Description = "This is a test";
			job.Save();
			#endregion
		}

		public static async void ExecuteStepOnJobAsync(string jobID)
		{
			#region How to Execute a step on a job

			// Gets the current step
			// checks to see if it can execute it
			// proceeds to do so if it can
			var wfCon = await WorkflowModule.ConnectAsync();
			var jobManager = wfCon.GetManager<JobsManager>();
			var job = jobManager.GetJob(jobID);
			string stepID = job.GetCurrentSteps().First();
			if (job.CanExecuteStep(stepID).Item1)
				job.ExecuteStep(stepID);
			#endregion
		}
		public static async Task ExecuteQueryAsync()
		{
			#region How to execute a Query

			// ExecuteQuery returns a query result
			// Its passed either an ID or a name
			var wfCon = await WorkflowModule.ConnectAsync();
			var jobManager = wfCon.GetManager<JobsManager>();
			var queryResultReturn = jobManager.ExecuteQuery("All Jobs");
			#endregion
		}

	}
}
