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
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Core;
using ArcGIS.Core.CIM;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Threading.Tasks;

namespace ArcGIS.Desktop.Mapping.ArcGIS.Desktop.Mapping.CartoFeatures
{
    class ProSnippetCS6
    {
        #region How to apply a color ramp from a style to a feature layer

        public async Task ApplyColorRampAsync(FeatureLayer featureLayer, string[] fields)
        {

            StyleProjectItem style =
              Project.Current.GetItems<StyleProjectItem>().FirstOrDefault(s => s.Name == "ColorBrewer Schemes (RGB)");
            if (style == null) return;
            var colorRampList = await QueuedTask.Run(() => style.SearchColorRamps("Red-Gray (10 Classes)"));
            if (colorRampList == null || colorRampList.Count == 0) return;
            CIMColorRamp cimColorRamp = null;
            CIMRenderer renderer = null;
            await QueuedTask.Run(() =>
            {
                cimColorRamp = colorRampList[0].ColorRamp;
                var rendererDef = new UniqueValueRendererDefinition(fields, null, cimColorRamp);
                renderer = featureLayer?.CreateRenderer(rendererDef);
                featureLayer?.SetRenderer(renderer);
            });

        }

        #endregion
    }
}
