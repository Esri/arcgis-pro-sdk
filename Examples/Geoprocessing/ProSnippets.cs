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

    #region ProSnippet Group: General 
    #endregion

    public async void CodeExamples()
        {
      // cref: ARCGIS.DESKTOP.CORE.GEOPROCESSING.GEOPROCESSING.MakeValueArray
      // cref: ArcGIS.Desktop.Core.Geoprocessing.Geoprocessing.ExecuteToolAsync
      // cref: ArcGIS.Desktop.Core.Geoprocessing.Geoprocessing.ExecuteToolAsync(STRING,IENUMERABLE{STRING},IENUMERABLE{KEYVALUEPAIR{STRING,STRING}},NULLABLE{CANCELLATIONTOKEN},GPTOOLEXECUTEEVENTHANDLER,GPEXECUTETOOLFLAGS)
      // cref: ArcGIS.Desktop.Core.Geoprocessing.Geoprocessing.ExecuteToolAsync(STRING,IENUMERABLE{STRING},IENUMERABLE{KEYVALUEPAIR{STRING,STRING}},CANCELABLEPROGRESSOR,GPEXECUTETOOLFLAGS)
      #region How to execute a Model tool
      // get the model tool's parameter syntax from the model's help
      string input_roads = @"C:\data\Input.gdb\PlanA_Roads";
            string buff_dist_field = "Distance";   // use values from a field
            string input_vegetation = @"C:\data\Input.gdb\vegetation";
            string output_data = @"C:\data\Output.gdb\ClippedFC2";

            // the model name is ExtractVegetation
            string tool_path = @"C:\data\MB\Models.tbx\ExtractVegetation";

            var args = Geoprocessing.MakeValueArray(input_roads, buff_dist_field, input_vegetation, output_data);

            var result = await Geoprocessing.ExecuteToolAsync(tool_path, args);
      #endregion

      // cref: ARCGIS.DESKTOP.CORE.GEOPROCESSING.GEOPROCESSING.MAKEENVIRONMENTARRAY
      // cref: ARCGIS.DESKTOP.CORE.GEOPROCESSING.GEOPROCESSING.MakeValueArray
      // cref: ArcGIS.Desktop.Core.Geoprocessing.Geoprocessing.ExecuteToolAsync
      // cref: ArcGIS.Desktop.Core.Geoprocessing.Geoprocessing.ExecuteToolAsync(STRING,IENUMERABLE{STRING},IENUMERABLE{KEYVALUEPAIR{STRING,STRING}},NULLABLE{CANCELLATIONTOKEN},GPTOOLEXECUTEEVENTHANDLER,GPEXECUTETOOLFLAGS)
      // cref: ArcGIS.Desktop.Core.Geoprocessing.Geoprocessing.ExecuteToolAsync(STRING,IENUMERABLE{STRING},IENUMERABLE{KEYVALUEPAIR{STRING,STRING}},CANCELABLEPROGRESSOR,GPEXECUTETOOLFLAGS)
      #region Set Geoprocessing extent environment

      var parameters = Geoprocessing.MakeValueArray(@"C:\data\data.gdb\HighwaysUTM11", @"C:\data\data.gdb\Highways_extent");
            var ext = Geoprocessing.MakeEnvironmentArray(extent: "460532 3773964 525111 3827494");
            var gp_result = await Geoprocessing.ExecuteToolAsync("management.CopyFeatures", parameters, ext);
      #endregion

      // cref: ArcGIS.Desktop.Core.Geoprocessing.Geoprocessing.OpenToolDialog(System.String,System.Collections.Generic.IEnumerable{System.String},System.Collections.Generic.IEnumerable{System.Collections.Generic.KeyValuePair{System.String,System.String}},System.Boolean,ArcGIS.Desktop.Core.Geoprocessing.GPToolExecuteEventHandler)
      // cref: ARCGIS.DESKTOP.CORE.GEOPROCESSING.GEOPROCESSING.MakeValueArray
      #region Open a script tool dialog in Geoprocessing pane
      string input_data = @"C:\data\data.gdb\Population";
            string out_pdf = @"C:\temp\Reports.pdf";
            string field_name = "INCOME";
            // use defaults for other parameters - no need to pass any value
            var arguments = Geoprocessing.MakeValueArray(input_data, out_pdf, field_name);

            string toolpath = @"C:\data\WorkflowTools.tbx\MakeHistogram";

            Geoprocessing.OpenToolDialog(toolpath, arguments);
      #endregion

      // cref: ARCGIS.DESKTOP.GEOPROCESSING.GEOPROCESSINGPROJECTITEM
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

      // cref: ARCGIS.DESKTOP.CORE.GEOPROCESSING.GPEXECUTETOOLFLAGS
      // cref: ARCGIS.DESKTOP.CORE.GEOPROCESSING.GEOPROCESSING.MakeValueArray
      // cref: ArcGIS.Desktop.Core.Geoprocessing.Geoprocessing.ExecuteToolAsync
      // cref: ArcGIS.Desktop.Core.Geoprocessing.Geoprocessing.ExecuteToolAsync(STRING,IENUMERABLE{STRING},IENUMERABLE{KEYVALUEPAIR{STRING,STRING}},NULLABLE{CANCELLATIONTOKEN},GPTOOLEXECUTEEVENTHANDLER,GPEXECUTETOOLFLAGS)
      // cref: ArcGIS.Desktop.Core.Geoprocessing.Geoprocessing.ExecuteToolAsync(STRING,IENUMERABLE{STRING},IENUMERABLE{KEYVALUEPAIR{STRING,STRING}},CANCELABLEPROGRESSOR,GPEXECUTETOOLFLAGS)
      #region Stop a feature class created with GP from automatically adding to the map
      // However, settings in Pro App's Geoprocessing Options will override option set in code
      // for example, in Pro App's Options > Geoprocessing dialog, if you check 'Add output datasets to an open map'
      // then the output WILL BE added to history overriding settings in code
      var CopyfeaturesParams = Geoprocessing.MakeValueArray("C:\\data\\Input.gdb\\PlanA_Roads", "C:\\data\\Input.gdb\\Roads_copy");
            IGPResult gpResult = await Geoprocessing.ExecuteToolAsync("management.CopyFeatures", CopyfeaturesParams, null, null, null, GPExecuteToolFlags.None);
      #endregion

      // cref: ARCGIS.DESKTOP.CORE.GEOPROCESSING.GPEXECUTETOOLFLAGS
      // cref: ARCGIS.DESKTOP.CORE.GEOPROCESSING.GEOPROCESSING.MakeValueArray
      // cref: ArcGIS.Desktop.Core.Geoprocessing.Geoprocessing.ExecuteToolAsync
      // cref: ArcGIS.Desktop.Core.Geoprocessing.Geoprocessing.ExecuteToolAsync(STRING,IENUMERABLE{STRING},IENUMERABLE{KEYVALUEPAIR{STRING,STRING}},NULLABLE{CANCELLATIONTOKEN},GPTOOLEXECUTEEVENTHANDLER,GPEXECUTETOOLFLAGS)
      // cref: ArcGIS.Desktop.Core.Geoprocessing.Geoprocessing.ExecuteToolAsync(STRING,IENUMERABLE{STRING},IENUMERABLE{KEYVALUEPAIR{STRING,STRING}},CANCELABLEPROGRESSOR,GPEXECUTETOOLFLAGS)
      #region GPExecuteToolFlags.AddToHistory will add the execution messages to Hisotry
      // However, settings in Pro App's Geoprocessing Options will override option set in code
      // for example, if in Options > Geoprocessing dialog, if you uncheck 'Write geoprocessing operations to Geoprocessing History'
      // then the output will not be added to history.
      var args2 = Geoprocessing.MakeValueArray("C:\\data\\Vegetation.shp", "NewField", "TEXT");
            var result2 = await Geoprocessing.ExecuteToolAsync("management.AddField", args2, null, null, null, GPExecuteToolFlags.AddToHistory);
      #endregion

      // cref: ARCGIS.DESKTOP.CORE.GEOPROCESSING.IGPRESULT
      // cref: ARCGIS.DESKTOP.CORE.GEOPROCESSING.IGPRESULT.RETURNVALUE
      // cref: ARCGIS.DESKTOP.CORE.GEOPROCESSING.IGPRESULT.ERRORMESSAGES
      // cref: ARCGIS.DESKTOP.CORE.GEOPROCESSING.GPEXECUTETOOLFLAGS
      // cref: ARCGIS.DESKTOP.CORE.GEOPROCESSING.GEOPROCESSING.MakeValueArray
      // cref: ArcGIS.Desktop.Core.Geoprocessing.Geoprocessing.ExecuteToolAsync
      // cref: ArcGIS.Desktop.Core.Geoprocessing.Geoprocessing.ExecuteToolAsync(STRING,IENUMERABLE{STRING},IENUMERABLE{KEYVALUEPAIR{STRING,STRING}},NULLABLE{CANCELLATIONTOKEN},GPTOOLEXECUTEEVENTHANDLER,GPEXECUTETOOLFLAGS)
      // cref: ArcGIS.Desktop.Core.Geoprocessing.Geoprocessing.ExecuteToolAsync(STRING,IENUMERABLE{STRING},IENUMERABLE{KEYVALUEPAIR{STRING,STRING}},CANCELABLEPROGRESSOR,GPEXECUTETOOLFLAGS)
      #region Multi Ring Buffer
      //The data referenced in this snippet can be downloaded from the arcgis-pro-sdk-community-samples repo
      //https://github.com/Esri/arcgis-pro-sdk-community-samples
      async Task<IGPResult> CreateRings(EditingTemplate currentTemplate)
            {
                var paramsArray = Geoprocessing.MakeValueArray(currentTemplate.MapMember.Name,
                            @"C:\Data\FeatureTest\FeatureTest.gdb\Points_MultipleRingBuffer",
                            new List<string> { "1000", "2000" }, "Meters", "Distance",
                            "ALL", "FULL");

                IGPResult ringsResult = await Geoprocessing.ExecuteToolAsync("Analysis.MultipleRingBuffer", paramsArray);
                var messages = string.IsNullOrEmpty(gpResult.ReturnValue)
                        ? $@"Error in gp tool: {gpResult.ErrorMessages}"
                        : $@"Ok: {gpResult.ReturnValue}";

                return ringsResult;
            }
      #endregion

      // cref: ARCGIS.DESKTOP.CORE.GEOPROCESSING.IGPRESULT
      // cref: ARCGIS.DESKTOP.CORE.GEOPROCESSING.GPEXECUTETOOLFLAGS
      // cref: ARCGIS.DESKTOP.CORE.GEOPROCESSING.GEOPROCESSING.MakeValueArray
      // cref: ArcGIS.Desktop.Core.Geoprocessing.Geoprocessing.ExecuteToolAsync
      // cref: ArcGIS.Desktop.Core.Geoprocessing.Geoprocessing.ExecuteToolAsync(STRING,IENUMERABLE{STRING},IENUMERABLE{KEYVALUEPAIR{STRING,STRING}},NULLABLE{CANCELLATIONTOKEN},GPTOOLEXECUTEEVENTHANDLER,GPEXECUTETOOLFLAGS)
      // cref: ArcGIS.Desktop.Core.Geoprocessing.Geoprocessing.ExecuteToolAsync(STRING,IENUMERABLE{STRING},IENUMERABLE{KEYVALUEPAIR{STRING,STRING}},CANCELABLEPROGRESSOR,GPEXECUTETOOLFLAGS)
      #region Non-blocking execution of a Geoprocessing tool
      //The data referenced in this snippet can be downloaded from the arcgis-pro-sdk-community-samples repo
      //https://github.com/Esri/arcgis-pro-sdk-community-samples
      string in_data = @"C:\tools\data.gdb\cities";
            string cities_buff = @"E:\data\data.gdb\cities_2km";

            var valueArray = Geoprocessing.MakeValueArray(in_data, cities_buff, "2000 Meters");

            // to let the GP tool run asynchronously without blocking the main thread
            // use the GPThread option of GPExecuteToolFlasgs
            //
            GPExecuteToolFlags flags = GPExecuteToolFlags.GPThread;  // instruct the tool run non-blocking GPThread
            IGPResult bufferResult = await Geoprocessing.ExecuteToolAsync("Analysis.Buffer", valueArray, null, null, null, flags);
      #endregion

      // cref: ARCGIS.DESKTOP.CORE.GEOPROCESSING.GEOPROCESSING.ShowMessageBox
      // cref: ARCGIS.DESKTOP.CORE.GEOPROCESSING.GPEXECUTETOOLFLAGS
      // cref: ARCGIS.DESKTOP.CORE.GEOPROCESSING.GEOPROCESSING.MAKEENVIRONMENTARRAY
      // cref: ARCGIS.DESKTOP.CORE.GEOPROCESSING.GEOPROCESSING.MakeValueArray
      // cref: ArcGIS.Desktop.Core.Geoprocessing.Geoprocessing.ExecuteToolAsync
      // cref: ArcGIS.Desktop.Core.Geoprocessing.Geoprocessing.ExecuteToolAsync(STRING,IENUMERABLE{STRING},IENUMERABLE{KEYVALUEPAIR{STRING,STRING}},NULLABLE{CANCELLATIONTOKEN},GPTOOLEXECUTEEVENTHANDLER,GPEXECUTETOOLFLAGS)
      // cref: ArcGIS.Desktop.Core.Geoprocessing.Geoprocessing.ExecuteToolAsync(STRING,IENUMERABLE{STRING},IENUMERABLE{KEYVALUEPAIR{STRING,STRING}},CANCELABLEPROGRESSOR,GPEXECUTETOOLFLAGS)
      #region How to pass parameter with multiple or complex input values
      var environments = Geoprocessing.MakeEnvironmentArray(overwriteoutput: true);

            string toolName = "Snap_edit";  // or use edit.Snap

            // Snap tool takes multiple inputs each of which has
            // Three (3) parts: a feature class or layer, a string value and a distance
            // Each part is separated by a semicolon - you can get example of sytax from the tool documentation page
            var snapEnv = @"'C:/SnapProject/fgdb.gdb/line_1' END '2 Meters';'C:/SnapProject/fgdb.gdb/points_1' VERTEX '1 Meters';'C:/SnapProject/fgdb.gdb/otherline_1' END '20 Meters'";

            var infc = @"C:/SnapProject/fgdb.gdb/poly_1";

            var snapParams = Geoprocessing.MakeValueArray(infc, snapEnv);

            GPExecuteToolFlags tokens = GPExecuteToolFlags.RefreshProjectItems | GPExecuteToolFlags.GPThread | GPExecuteToolFlags.AddToHistory;

            IGPResult snapResult = await Geoprocessing.ExecuteToolAsync(toolName, parameters, environments, null, null, tokens);

            //return gpResult;

            #endregion
        }

    #region ProSnippet Group: Geoprocessing Options 
    #endregion

    public void GeoprocessingOptions()
		{
      // cref: ArcGIS.Desktop.Core.GeoprocessingOptions
      // cref: ArcGIS.Desktop.Core.ApplicationOptions.GeoprocessingOptions
      #region Get GeoprocessingOptions

      //These options are for behavior of interactive GP tools _only_.

      var overwrite_gp = ApplicationOptions.GeoprocessingOptions.OverwriteExistingDatasets;
      var remove_gp = ApplicationOptions.GeoprocessingOptions.RemoveOverwrittenLayers;
      var addoutput_gp = ApplicationOptions.GeoprocessingOptions.AddOutputDatasetsToOpenMap;
      var history_gp = ApplicationOptions.GeoprocessingOptions.WriteGPOperationsToHistory;

      #endregion

      // cref: ArcGIS.Desktop.Core.GeoprocessingOptions
      // cref: ArcGIS.Desktop.Core.ApplicationOptions.GeoprocessingOptions
      #region Set GeoprocessingOptions
      //Note: changing these options modifies behavior of interactive GP tools _only_.
      //Use the ArcGIS.Desktop.Core.Geoprocessing.GPExecuteToolFlags enum parameter
      //to modify the behavior of Geoprocessing.ExecuteToolAsync(...)

      //Note: setting GeoprocessingOptions requires the QueuedTask
      QueuedTask.Run(() =>
      {
        ApplicationOptions.GeoprocessingOptions.SetOverwriteExistingDatasets(true);
        ApplicationOptions.GeoprocessingOptions.SetRemoveOverwrittenLayers(false);
        ApplicationOptions.GeoprocessingOptions.SetAddOutputDatasetsToOpenMap(true);
        ApplicationOptions.GeoprocessingOptions.SetWriteGPOperationsToHistory(false);
      });

      #endregion
    }


  }


}
