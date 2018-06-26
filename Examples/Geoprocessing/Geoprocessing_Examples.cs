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
namespace SDKExamples
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ArcGIS.Core.Geometry;
    using ArcGIS.Desktop.Core;
    using ArcGIS.Desktop.Core.Geoprocessing;
    using ArcGIS.Desktop.Mapping;
    using ArcGIS.Desktop.Framework.Threading.Tasks;

    class GeoprocessinExamples
    {
        // Setting environments, MakeEnvironmentArray
        public async void SetEnvironment()   // Task<IGPResult>
        {
            #region gp_environments
            // get the syntax of the tool from Python window or from tool help page
            string in_features = @"C:\data\data.gdb\HighwaysWeb84";
            string out_features = @"C:\data\data.gdb\HighwaysUTM";
            var param_values = Geoprocessing.MakeValueArray(in_features, out_features);

            // crate the spatial reference object to pass as an argument to management.CopyFeatures tool
            var sp_ref = await QueuedTask.Run(() => {
                return SpatialReferenceBuilder.CreateSpatialReference(26911);    // UTM 83 11N: 26911
            });

            // set output coordinate system environment           
            var environments = Geoprocessing.MakeEnvironmentArray(outputCoordinateSystem: sp_ref);
            // set environments in the 3rd parameter
            var gp_result = await Geoprocessing.ExecuteToolAsync("management.CopyFeatures", param_values, environments, null, null, GPExecuteToolFlags.AddOutputsToMap);
            
            Geoprocessing.ShowMessageBox(gp_result.Messages, "Contents", GPMessageBoxStyle.Default, "Window Title");

            //return gp_result;

            #endregion
        }

        public async void ProgressDialogExample()
        {
            #region progress_dialog

            var progDlg = new ProgressDialog("Running Geoprocessing Tool", "Cancel", 100, true);
            progDlg.Show();
            
            var progSrc = new CancelableProgressorSource(progDlg);

            // prepare input parameter values to CopyFeatures tool
            string input_data = @"C:\data\california.gdb\ca_highways";
            string out_workspace = ArcGIS.Desktop.Core.Project.Current.DefaultGeodatabasePath;
            string out_data = System.IO.Path.Combine(out_workspace, "ca_highways2");

            // make a value array of strings to be passed to ExecuteToolAsync
            var parameters = Geoprocessing.MakeValueArray(input_data, out_data);

            // execute the tool
            await Geoprocessing.ExecuteToolAsync("management.CopyFeatures", parameters,
                null, new CancelableProgressorSource(progDlg).Progressor, GPExecuteToolFlags.Default);

            // dialog hides itself once the execution is complete
            progDlg.Hide();

            #endregion
        }


        public async void ExecuteEBK()
        {
            #region gp_events

            System.Threading.CancellationTokenSource _cts;

            string ozone_points = @"C:\data\ca_ozone.gdb\O3_Sep06_3pm";

            string[] args = { ozone_points, "OZONE", "", "in_memory\\raster", "300",
                                "EMPIRICAL", "300", "5", "5000",
                                "NBRTYPE=StandardCircular RADIUS=310833.272442914 ANGLE=0 NBR_MAX=10 SECTOR_TYPE=ONE_SECTOR",
                                "PREDICTION", "0.5", "EXCEED", "", "K_BESSEL" };

            string tool_path = "ga.EmpiricalBayesianKriging";

            _cts = new System.Threading.CancellationTokenSource();

            var result = Geoprocessing.ExecuteToolAsync(tool_path, args, null, _cts.Token,
                (event_name, o) =>  // implement delegate and handle events
                {
                    switch (event_name)
                    {
                        case "OnValidate": // stop execute if any warnings
                            if ((o as IGPMessage[]).Any(it => it.Type == GPMessageType.Warning))
                                _cts.Cancel();
                            break;

                        case "OnProgressMessage":
                            string msg = string.Format("{0}: {1}", new object[] { event_name, (string)o });
                            System.Windows.MessageBox.Show(msg);
                            _cts.Cancel();
                            break;

                        case "OnProgressPos":
                            string msg2 = string.Format("{0}: {1} %", new object[] { event_name, (int)o });
                            System.Windows.MessageBox.Show(msg2);
                            _cts.Cancel();
                            break;
                    }
                });

            var ret = await result;
            _cts = null;

            #endregion
        }

        public async void ShowMessageBox()
        {
            #region message_box
            var gp_result = await Geoprocessing.ExecuteToolAsync("management.GetCount", Geoprocessing.MakeValueArray(@"C:\data\f.gdb\hello"));
            // this icon shows up left of content_header
            string icon_src = @"C:\data\Icons\ModifyLink32.png";
            Geoprocessing.ShowMessageBox(gp_result.Messages, "Content Header", GPMessageBoxStyle.Error, "Window Title", icon_src);
            #endregion
        }

        private void OpenBufferToolDialog()
        {
            #region open_dialog
            string input_points = @"C:\data\ca_ozone.gdb\ozone_points";
            string output_polys = @"C:\data\ca_ozone.gdb\ozone_buff";
            string buffer_dist = "2000 Meters";

            var param_values = Geoprocessing.MakeValueArray(input_points, output_polys, buffer_dist);

            Geoprocessing.OpenToolDialog("analysis.Buffer", param_values);
            #endregion
        }
    }

}