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

using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Editing.Attributes;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Internal.Framework.Controls;
using ArcGIS.Desktop.Mapping;

namespace EditingSDKExamples
{
  internal class Attributes : Button
  {
    protected override void OnClick()
    {
    }

    protected void addvalidate()
    {
      //var featLayer = MappingModule.ActiveTOC.SelectedLayers[0] as FeatureLayer;
      var featLayer = MapView.Active.GetSelectedLayers().First() as FeatureLayer;
      
      #region AddValidate
      var insp = new Inspector();
      insp.LoadSchema(featLayer);
      var attrib = insp.Where(a => a.FieldName == "Mineral").First();

      attrib.AddValidate(() =>
      {
        if (attrib.CurrentValue.ToString() == "Salt")
          return Enumerable.Empty<ArcGIS.Desktop.Editing.Attributes.Attribute.ValidationError>();
        else return new[] { ArcGIS.Desktop.Editing.Attributes.Attribute.ValidationError.Create("Error", ArcGIS.Desktop.Editing.Attributes.Severity.Low) };
      });
      #endregion
    }

    protected void inspectorclass()
    {
      var featLayer = MapView.Active.GetSelectedLayers().First() as FeatureLayer;
      Int64 oid = 42;
    
      #region inspectorclass
      QueuedTask.Run(() =>
      {
        var insp = new Inspector();
        insp.Load(featLayer, oid);

        //get the shape of the feature
        var myGeometry = insp.Shape;

        //get an attribue value by name
        var propValue = insp["Prop_value"];

        //set an attribute value by name
        insp["Parcel_no"] = 42;

        //perform the edit
        var op = new EditOperation();
        op.Name = "Update parcel";
        op.Modify(insp);
        op.Execute();
      });
      #endregion
    }
  }
}
