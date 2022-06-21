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
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Editing.Attributes;
using ArcGIS.Desktop.Editing.Templates;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Internal.Framework.Controls;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Editing.Events;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Core.CIM;

namespace EditingSDKExamples
{
  class Concepts : MapTool
  {
    public Concepts()
    {
      IsSketchTool = true;
      SketchType = SketchGeometryType.Rectangle;
      SketchOutputMode = SketchOutputMode.Map;
    }

    protected override Task<bool> OnSketchCompleteAsync(Geometry geometry)
    {

      var myTemplate = EditingTemplate.Current;
      var myGeometry = geometry;

      //Create a new feature using a template and a geometry
      var op = new EditOperation()
      {
        Name = "Create my feature"
      };
      op.Create(myTemplate, myGeometry);
      op.Execute();

      return Task.FromResult(true);

    }

    protected void UpdateAttrib()
    {
      QueuedTask.Run(() =>
      {
        //get the geometry of the selected feature.
        var selectedFeatures = MapView.Active.Map.GetSelection();
        var insp = new Inspector();
        insp.Load(selectedFeatures.ToDictionary().Keys.First(), selectedFeatures.ToDictionary().Values.First());
        var selGeom = insp.Shape;

        //var extPolyline = some geometry operation.
        var extPolyline = new MapPointBuilderEx(1.0, 1.0).ToGeometry();

        //set the new geometry back on the feature
        insp.Shape = extPolyline;

        //update an attribute value
        insp["RouteNumber"] = 42;

        //create and execute the edit operation
        var op = new EditOperation()
        {
          Name = "Extend"
        };
        op.Modify(insp);
        op.Execute();
      });
    }

    protected void SetupEvents()
    {
      QueuedTask.Run(() =>
      {
        var featLayer = MapView.Active.GetSelectedLayers().First() as FeatureLayer;
        var layerTable = featLayer.GetTable();

        //subscribe to row events for a layer
        var rowCreateToken = RowCreatedEvent.Subscribe(onRowEvent, layerTable);
        var rowChangeToken = RowChangedEvent.Subscribe(onRowEvent, layerTable);
        var rowDeleteToken = RowDeletedEvent.Subscribe(onRowEvent, layerTable);
      });

      //subscribe to editevents
      var editComplete = EditCompletedEvent.Subscribe(onEditComplete);
    }

    protected void onRowEvent(RowChangedEventArgs args)
    {
      //show the type of edit
      Console.WriteLine("RowEvent " + args.EditType.ToString());
    }

    protected Task onEditComplete(EditCompletedEventArgs args)
    {
      //show the count of features changed
      Console.WriteLine("Creates: " + args.Creates.ToDictionary().Values.Sum(list => list.Count).ToString());
      Console.WriteLine("Modifies: " + args.Modifies.ToDictionary().Values.Sum(list => list.Count).ToString());
      Console.WriteLine("Deletes: " + args.Deletes.ToDictionary().Values.Sum(list => list.Count).ToString());
      return Task.FromResult(0);
    }

    protected async void SetSnapping()
    {

      var myMap = MapView.Active.Map;

      //using ArcGIS.Desktop.Mapping
      //enable snapping
      Snapping.IsEnabled = true;

      //enable a snap mode, others are not changed.
      Snapping.SetSnapMode(SnapMode.Point, true);

      //set multiple snap modes exclusively. All others will be disabled.
      //at 2.x - Snapping.SetSnapModes(SnapMode.Edge, SnapMode.Point);
      Snapping.SetSnapModes(new List<SnapMode>() { SnapMode.Edge, SnapMode.Point });

      await QueuedTask.Run(() =>
      {
        //set snapping options via get/set options
        var snapOptions = Snapping.GetOptions(myMap);
        //at 2.x - snapOptions.SnapToSketchEnabled = true;
        snapOptions.IsSnapToSketchEnabled = true;
        snapOptions.XYTolerance = 100;
        Snapping.SetOptions(myMap, snapOptions);
      });


      var myFeatureLayer = MapView.Active.GetSelectedLayers().First() as FeatureLayer;

      //read snapping availability for a layer
      var canSnap = myFeatureLayer.IsSnappable;

      //set snapping availability for a layer
      var featLayerDef = myFeatureLayer.GetDefinition() as ArcGIS.Core.CIM.CIMGeoFeatureLayerBase;
      featLayerDef.Snappable = true;
      myFeatureLayer.SetDefinition(featLayerDef);

    }

    protected void TemplateExamples()
    {
      //get selected layer in toc
      var featLayer = MapView.Active.GetSelectedLayers().First() as FeatureLayer;

      QueuedTask.Run(() =>
      {
        //get selected template in create features pane
        var currTemplate = ArcGIS.Desktop.Editing.Templates.EditingTemplate.Current;

        //get all templates for a layer. 
        var layerTemplates = featLayer.GetTemplates();

        //find a template on a layer by name
        var resTemplate = featLayer.GetTemplate("Residential");

        //Activate the default tool on a template. Sets the template as current.
        resTemplate.ActivateDefaultToolAsync();
      });
    }

    protected void createTemplate()
    {
      //get parcels layer
      var featLayer = MapView.Active.Map.FindLayers("Parcels").First();

      QueuedTask.Run(() =>
      {
        //find a template on a layer by name
        var resTemplate = featLayer.GetTemplate("Residential");

        //get CIM layer definition
        var layerDef = featLayer.GetDefinition() as CIMFeatureLayer;
        //get all templates on this layer
        var layerTemplates = layerDef.FeatureTemplates.ToList();
        //copy template to new temporary one
        //At 2.x - var resTempDef = resTemplate.GetDefinition() as CIMFeatureTemplate;
        var resTempDef = resTemplate.GetDefinition() as CIMRowTemplate;
        //could also create a new one here 
        //var newTemplate = new CIMRowTemplate();

        //set template values
        resTempDef.Name = "Residential copy";
        resTempDef.Description = "This is the description for the copied template";
        resTempDef.WriteTags(new[] { "Testertag" });
        resTempDef.DefaultValues = new Dictionary<string, object>
          {
                    { "YEARBUILT", "1999" }
          };

        //add the new template to the layer template list
        layerTemplates.Add(resTempDef);
        //set the layer definition templates from the list
        layerDef.FeatureTemplates = layerTemplates.ToArray();
        //finally set the layer definition
        featLayer.SetDefinition(layerDef);
      });
    }

    protected void RemoveTemplate()
    {
      QueuedTask.Run(() =>
      {
        //get parcels layer
        var featLayer = MapView.Active.Map.FindLayers("Parcels").First();
        //get CIM layer definition
        var layerDef = featLayer.GetDefinition() as CIMFeatureLayer;
        //get all templates on this layer
        var layerTemplates = layerDef.FeatureTemplates.ToList();

        //remove templates matching a pattern
        layerTemplates.RemoveAll(t => t.Description.Contains("Commercial"));

        //set the templates and layer definition back on the layer
        layerDef.FeatureTemplates = layerTemplates.ToArray();
        featLayer.SetDefinition(layerDef);
      });
    }
  }
}
