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
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Core.CIM;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Core.Data;
using Attribute = ArcGIS.Desktop.Editing.Attributes.Attribute;

namespace EditingSDKExamples
{

  class ProSnippet2 : MapTool
  {
    public ProSnippet2()
    {
      IsSketchTool = true;
      SketchType = SketchGeometryType.Rectangle;
      SketchOutputMode = SketchOutputMode.Map;
    }

    public void OpMgr()
    {
      var editOp = new EditOperation();
      editOp.Name = "My Name";
      editOp.Execute();

      //elsewhere
      editOp.UndoAsync();

      #region Undo/Redo the Most Recent Operation

      //undo
      if (MapView.Active.Map.OperationManager.CanUndo)
        MapView.Active.Map.OperationManager.UndoAsync();//await as needed

      //redo
      if (MapView.Active.Map.OperationManager.CanRedo)
        MapView.Active.Map.OperationManager.RedoAsync();//await as needed

      #endregion
    }
    
    #region Change Default Edit tool for a template
    public Task ChangeTemplateDefaultToolAsync(ArcGIS.Desktop.Mapping.FeatureLayer flayer,
                      string toolContentGUID, string templateName)
    {
      return ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
      {

        // retrieve the edit template form the layer by name
        var template = flayer?.GetTemplate(templateName) as ArcGIS.Desktop.Editing.Templates.EditingTemplate;
        // get the definition of the layer
        var layerDef = flayer?.GetDefinition() as ArcGIS.Core.CIM.CIMFeatureLayer;
        if ((template == null) || (layerDef == null))
          return;

        if (template.DefaultToolID != this.ID)
        {
          bool updateLayerDef = false;
          if (layerDef.AutoGenerateFeatureTemplates)
          {
            layerDef.AutoGenerateFeatureTemplates = false;
            updateLayerDef = true;
          }

          // retrieve the CIM edit template definition
          var templateDef = template.GetDefinition();

          // assign the GUID from the tool DAML definition, for example
          // <tool id="TestConstructionTool_SampleSDKTool" categoryRefID="esri_editing_construction_polyline" ….>
          //   <tooltip heading="">Tooltip text<disabledText /></tooltip>
          //   <content guid="e58239b3-9c69-49e5-ad4d-bb2ba29ff3ea" />
          // </tool>
          // then the toolContentGUID would be "e58239b3-9c69-49e5-ad4d-bb2ba29ff3ea"
          templateDef.ToolProgID = toolContentGUID;

          // set the definition back to 
          template.SetDefinition(templateDef);

          // update the layer definition too
          if (updateLayerDef)
            flayer.SetDefinition(layerDef);
        }
      });
    }

    #endregion

    public void CreateTemplate()
    {
      string value1 = "";
      string value2 = "";
      string value3 = "";

      #region Create New Template using layer.CreateTemplate

      var layer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();
      if (layer == null)
        return;
      QueuedTask.Run(() =>
      {
        var insp = new Inspector();
        insp.LoadSchema(layer);

        insp["Field1"] = value1;
        insp["Field2"] = value2;
        insp["Field3"] = value3;

        var tags = new[] { "Polygon", "tag1", "tag2" };

        // set defaultTool using a daml-id 
        string defaultTool = "esri_editing_SketchCirclePolygonTool";

        // tool filter is the tools to filter OUT
        var toolFilter = new[] { "esri_editing_SketchTracePolygonTool" };

        // create a new template  
        var newTemplate = layer.CreateTemplate("My new template", "description", insp, defaultTool, tags, toolFilter);
      });
      #endregion

      #region Create Annotation Template

      // get an anno layer
      AnnotationLayer annoLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<AnnotationLayer>().FirstOrDefault();
      if (annoLayer == null)
        return;

      QueuedTask.Run(() =>
      {
        Inspector insp = null;
        // get the anno feature class
        var fc = annoLayer.GetFeatureClass() as ArcGIS.Core.Data.Mapping.AnnotationFeatureClass;

        // get the featureclass CIM definition which contains the labels, symbols
        var cimDefinition = fc.GetDefinition() as ArcGIS.Core.Data.Mapping.AnnotationFeatureClassDefinition;
        var labels = cimDefinition.GetLabelClassCollection();
        var symbols = cimDefinition.GetSymbolCollection();

        // make sure there are labels, symbols
        if ((labels.Count == 0) || (symbols.Count == 0))
          return;

        // find the label class required
        //   typically you would use a subtype name or some other characteristic
        // in this case lets just use the first one

        var label = labels[0];

        // each label has a textSymbol
        // the symbolName *should* be the symbolID to be used
        var symbolName = label.TextSymbol.SymbolName;
        int symbolID = -1;
        if (!int.TryParse(symbolName, out symbolID))
        {
          // int.TryParse fails - attempt to find the symbolName in the symbol collection
          foreach (var symbol in symbols)
          {
            if (symbol.Name == symbolName)
            {
              symbolID = symbol.ID;
              break;
            }
          }
        }
        // no symbol?
        if (symbolID == -1)
          return;

        // load the schema
        insp = new Inspector();
        insp.LoadSchema(annoLayer);

        // ok to assign these fields using the inspector[fieldName] methodology
        //   these fields are guaranteed to exist in the annotation schema
        insp["AnnotationClassID"] = label.ID;
        insp["SymbolID"] = symbolID;

        // set up some additional annotation properties
        AnnotationProperties annoProperties = insp.GetAnnotationProperties();
        annoProperties.FontSize = 36;
        annoProperties.TextString = "My Annotation feature";
        annoProperties.VerticalAlignment = VerticalAlignment.Top;
        annoProperties.HorizontalAlignment = HorizontalAlignment.Justify;

        insp.SetAnnotationProperties(annoProperties);

        var tags = new[] { "Annotation", "tag1", "tag2" };

        // use daml-id rather than guid
        string defaultTool = "esri_editing_SketchStraightAnnoTool";

        // tool filter is the tools to filter OUT
        var toolFilter = new[] { "esri_editing_SketchCurvedAnnoTool" };

        // create a new template 
        var newTemplate = annoLayer.CreateTemplate("new anno template", "description", insp, defaultTool, tags, toolFilter);
      });

      #endregion

    }

    #region Annotation Construction Tool

    //In your config.daml...set the categoryRefID
    //<tool id="..." categoryRefID="esri_editing_construction_annotation" caption="Create Anno" ...>

    //Sketch type Point or Line or BezierLine in the constructor...
    //internal class AnnoConstructionTool : MapTool  {
    //  public AnnoConstructionTool()  {
    //    IsSketchTool = true;
    //    UseSnapping = true;
    //    SketchType = SketchGeometryType.Point;
    //

    protected async override Task<bool> OnSketchCompleteAsync(Geometry geometry)
    {
      if (CurrentTemplate == null || geometry == null)
        return false;

      // Create an edit operation
      var createOperation = new EditOperation();
      createOperation.Name = string.Format("Create {0}", CurrentTemplate.Layer.Name);
      createOperation.SelectNewFeatures = true;

      var insp = CurrentTemplate.Inspector;
      var result = await QueuedTask.Run(() =>
      {
        // get the annotation properties class
        AnnotationProperties annoProperties = insp.GetAnnotationProperties();
        // set custom annotation properties
        annoProperties.TextString = "my custom text";
        annoProperties.Color = ColorFactory.Instance.RedRGB;
        annoProperties.FontSize = 24;
        annoProperties.FontName = "Arial";
        annoProperties.HorizontalAlignment = ArcGIS.Core.CIM.HorizontalAlignment.Right;
        annoProperties.Shape = geometry;
        // assign annotation properties back to the inspector
        insp.SetAnnotationProperties(annoProperties);

        // Queue feature creation
        createOperation.Create(CurrentTemplate.Layer, insp);

        // Execute the operation
        return createOperation.Execute();
      });
      return result;
    }

    #endregion


    //Using Inspector...
    internal async void UpdateAnnotation()
    {
      BasicFeatureLayer annoLayer = MapView.Active.Map.GetLayersAsFlattenedList().First() as BasicFeatureLayer;
      var oid = 1;

      #region Update Annotation Text 

      await QueuedTask.Run(() =>
      {
        //annoLayer is ~your~ Annotation layer...

        // use the inspector methodology
        var insp = new Inspector(true);
        insp.Load(annoLayer, oid);

        // get the annotation properties
        AnnotationProperties annoProperties = insp.GetAnnotationProperties();
        // set the attribute
        annoProperties.TextString = "Hello World";
        // assign the annotation proeprties back to the inspector
        insp.SetAnnotationProperties(annoProperties);

        //create and execute the edit operation
        EditOperation op = new EditOperation();
        op.Name = "Update annotation";
        op.Modify(insp);
        op.Execute();
      });
      #endregion

      #region Modify Annotation Shape

      await QueuedTask.Run(() =>
      {
        //Don't use 'Shape'....Shape is the bounding box of the annotation text. This is NOT what you want...
        //
        //var insp = new Inspector();
        //insp.Load(annoLayer, oid);
        //var shape = insp["SHAPE"] as Polygon;
        //...wrong shape...

        //Instead, we must use the AnnotationProperties

        //annoLayer is ~your~ Annotation layer
        var insp = new Inspector(true);
        insp.Load(annoLayer, oid);

        AnnotationProperties annoProperties = insp.GetAnnotationProperties();
        var shape = annoProperties.Shape;
        if (shape.GeometryType != GeometryType.GeometryBag)
        {
          var newGeometry = GeometryEngine.Instance.Move(shape, 10, 10);
          annoProperties.Shape = newGeometry;
          insp.SetAnnotationProperties(annoProperties);

          EditOperation op = new EditOperation();
          op.Name = "Change annotation angle";
          op.Modify(insp);
          op.Execute();
        }
      });

      #endregion

      #region Modify Annotation Text Graphic

      await QueuedTask.Run(() =>
      {

        var selection = annoLayer.GetSelection();
        if (selection.GetCount() == 0)
          return;

        // use the first selelcted feature 
        var insp = new Inspector(true);
        insp.Load(annoLayer, selection.GetObjectIDs().FirstOrDefault());

        // getAnnoProperties should return null if not an annotation feature
        AnnotationProperties annoProperties = insp.GetAnnotationProperties();
        // get the textGraphic
        CIMTextGraphic textGraphic = annoProperties.TextGraphic;

        // change text
        textGraphic.Text = "Hello world";

        // set x,y offset via the symbol
        var symbol = textGraphic.Symbol.Symbol;
        var textSymbol = symbol as CIMTextSymbol;
        textSymbol.OffsetX = 2;
        textSymbol.OffsetY = 3;

        textSymbol.HorizontalAlignment = HorizontalAlignment.Center;

        // load the updated textGraphic
        annoProperties.LoadFromTextGraphic(textGraphic);
        // assign the annotation properties back 
        insp.SetAnnotationProperties(annoProperties);

        EditOperation op = new EditOperation();
        op.Name = "modify symbol";
        op.Modify(insp);
        bool result = op.Execute();
      });

      #endregion
    }
  }
}