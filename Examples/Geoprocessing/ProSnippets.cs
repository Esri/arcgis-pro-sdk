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
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Core.Geoprocessing;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Extensions;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Core.CIM;
using ArcGIS.Desktop.GeoProcessing;
using ArcGIS.Desktop.Editing.Templates;

namespace ProSnippetsGeoprocessing
{

  class GeoprocessingCodeExamples
  {

    public async void CodeExamples()
    {

      #region How to execute a Model tool
      // get the model tool's parameter syntax from the model's help
      string input_roads = @"C:\data\Input.gdb\PlanA_Roads";
      string buff_dist_field = "Distance";   // use values from a field
      string input_vegetation = @"C:\data\Input.gdb\vegtype";
      string output_data = @"C:\data\Output.gdb\ClippedFC2";

      // the model name is ExtractVegetation
      string tool_path = @"C:\data\MB\Models.tbx\ExtractVegetation";

      var args = Geoprocessing.MakeValueArray(input_roads, buff_dist_field, input_vegetation, output_data);

      var result = Geoprocessing.ExecuteToolAsync(tool_path, args);
      #endregion


      #region Set Geoprocessing extent environment

      var parameters = Geoprocessing.MakeValueArray(@"C:\data\data.gdb\HighwaysUTM11", @"C:\data\data.gdb\Highways_extent");
      var ext = Geoprocessing.MakeEnvironmentArray(extent: "460532 3773964 525111 3827494");
      var gp_result = await Geoprocessing.ExecuteToolAsync("management.CopyFeatures", parameters, ext);
      #endregion


      #region Open a script tool dialog in Geoprocessing pane
      string input_data = @"C:\data\data.gdb\Population";
      string out_pdf = @"C:\temp\Reports.pdf";
      string field_name = "INCOME";
      // use defaults for other parameters - no need to pass any value
      var arguments = Geoprocessing.MakeValueArray(input_data, out_pdf, field_name);

      string toolpath = @"C:\data\WorkflowTools.tbx\MakeHistogram";

      Geoprocessing.OpenToolDialog(toolpath, args);
      #endregion


      #region Get Geoprocessing project items
      var gpItems = CoreModule.CurrentProject.Items.OfType<GeoprocessingProjectItem>();

      // go through all the available toolboxes
      foreach (var gpItem in gpItems)
      {
        var itemsInsideToolBox = gpItem.GetItems();

        // then for each toolbox list the tools inside
        foreach (var toolItem in itemsInsideToolBox)
        {
          string newTool = String.Join(";", new string[] { toolItem.Path, toolItem.Name });
          // do something with the newTool
          // for example, add to a list to track or use them later
        }
      }
            #endregion

      #region Stop a featureclass created with GP from automatically adding to the map
      // However, settings in Pro App's Geoprocessing Options will override option set in code
      // for example, in Pro App's Options > Geoprocessing dialog, if you check 'Add output datasets to an open map'
      // then the output WILL BE added to history overriding settings in code
      var GPresult = Geoprocessing.ExecuteToolAsync(tool_path, args, null, null, null, GPExecuteToolFlags.None);
      #endregion

      #region GPExecuteToolFlags.AddToHistory will add the execution messages to Hisotry
      // However, settings in Pro App's Geoprocessing Options will override option set in code
      // for example, if in Options > Geoprocessing dialog, if you uncheck 'Write geoprocessing operations to Geoprocessing History'
      // then the output will not be added to history. 
      var result2 = Geoprocessing.ExecuteToolAsync(tool_path, args, null, null, null, GPExecuteToolFlags.AddToHistory);
      #endregion
      }

        #region Multi Ring Buffer
        //The data referenced in this snippet can be downloaded from the arcgis-pro-sdk-community-samples repo
        //https://github.com/Esri/arcgis-pro-sdk-community-samples
        protected async Task<string> CreateRings(EditingTemplate currentTemplate)
      {
          var valueArray = await QueuedTask.Run(() =>
          {
              return Geoprocessing.MakeValueArray(currentTemplate.MapMember.Name,
                    @"C:\Data\FeatureTest\FeatureTest.gdb\Points_MultipleRingBuffer",
                    new List<string> { "1000", "2000" }, "Meters", "Distance",
                    "ALL", "FULL");
          });
          IGPResult gpResult = await Geoprocessing.ExecuteToolAsync("Analysis.MultipleRingBuffer", valueArray);
          return string.IsNullOrEmpty(gpResult.ReturnValue)
                ? $@"Error in gp tool: {gpResult.ErrorMessages}"
                : $@"Ok: {gpResult.ReturnValue}";
      }
      #endregion

      #region Non-blocking execution of a Geoprocessing tool
      //The data referenced in this snippet can be downloaded from the arcgis-pro-sdk-community-samples repo
      //https://github.com/Esri/arcgis-pro-sdk-community-samples
      protected async Task<string> NonBlockingExecuteGP(EditingTemplate currentTemplate)
      {
        var valueArray = await QueuedTask.Run(() =>
        {
          string in_data = @"C:\tools\data.gdb\cities";
          string cities_buff = @"E:\data\data.gdb\cities_2km";

          return Geoprocessing.MakeValueArray(in_data, cities_buff, "2000 Meters");
        });

        // to let the GP tool run asynchronously without blocking the main thread
        // use the GPThread option of GPExecuteToolFlasgs
        //
        GPExecuteToolFlags flags = GPExecuteToolFlags.GPThread;  // instruct the tool run non-blocking GPThread
        IGPResult gpResult = await Geoprocessing.ExecuteToolAsync("Analysis.Buffer", valueArray, null, null, null, flags);

        return string.IsNullOrEmpty(gpResult.ReturnValue)
              ? $@"Error in gp tool: {gpResult.ErrorMessages}"
              : $@"Ok: {gpResult.ReturnValue}";
      }
      #endregion
         
      #region How to pass parameter with multiple or complex input values
      public async Task<IGPResult> ExecuteSnap()
      {
        var environments = Geoprocessing.MakeEnvironmentArray(overwriteoutput: true);

        string toolName = @"Snap_edit";
         
        // Snap tool takes multiple inputs each of which has 
        // Three (3) parts: a feature class or layer, a string value and a distance
        // Each part is separated by a semicolon - you can get example of sytax from the tool documentation page
        var snapEnv = @"'C:/SnapProject/fgdb.gdb/line_1' END '2 Meters';'C:/SnapProject/fgdb.gdb/points_1' VERTEX '1 Meters';'C:/SnapProject/fgdb.gdb/otherline_1' END '20 Meters'";

        var parameters = await QueuedTask.Run(() =>
        {
          var infc = @"C:/SnapProject/fgdb.gdb/poly_1";
          return Geoprocessing.MakeValueArray(infc, snapEnv);
        });

        GPExecuteToolFlags tokens = GPExecuteToolFlags.RefreshProjectItems | GPExecuteToolFlags.GPThread | GPExecuteToolFlags.AddToHistory;

        var gpResult = await Geoprocessing.ExecuteToolAsync(toolName, parameters, environments, null, null, flags: tokens);

        return gpResult;
      }
      #endregion
  }
}
