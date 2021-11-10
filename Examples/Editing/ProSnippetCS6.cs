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
using ArcGIS.Desktop.Framework;
using System.Windows.Input;
using ArcGIS.Desktop.Core;

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

      #region ProSnippet Group: Undo / Redo
      #endregion

      #region Undo/Redo the Most Recent Operation

      //undo
      if (MapView.Active.Map.OperationManager.CanUndo)
        MapView.Active.Map.OperationManager.UndoAsync();//await as needed

      //redo
      if (MapView.Active.Map.OperationManager.CanRedo)
        MapView.Active.Map.OperationManager.RedoAsync();//await as needed

      #endregion
    }

    #region ProSnippet Group: Edit Templates
    #endregion

    public void FindTemplateByName()
    {
      #region Find edit template by name on a layer
      ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
      {
        //get the templates
        var map = ArcGIS.Desktop.Mapping.MapView.Active.Map;
        if (map == null)
          return;

        var mainTemplate = map.FindLayers("main").FirstOrDefault()?.GetTemplate("Distribution");
        var mhTemplate = map.FindLayers("Manhole").FirstOrDefault()?.GetTemplate("Active");
      });
      #endregion
    }

    public void FindTableTemplate()
    {
      #region Find table templates belonging to a standalone table
      ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
      {
        var map = ArcGIS.Desktop.Mapping.MapView.Active.Map;
        if (map == null)
          return;
        //Get a particular table template
        var tableTemplate = map.FindStandaloneTables("Address Points").FirstOrDefault()?.GetTemplate("Residences");
        //Get all the templates of a standalone table
        var ownersTableTemplates = map.FindStandaloneTables("Owners").FirstOrDefault()?.GetTemplates();
        var statisticsTableTemplates = MapView.Active.Map.GetStandaloneTablesAsFlattenedList().First(l => l.Name.Equals("Trading Statistics")).GetTemplates();
      });
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

    protected void FilterTemplateTools()
    {
      #region Hide or show editing tools on templates
      QueuedTask.Run(() =>
      {
        //hide all tools except line tool on layer
        var featLayer = MapView.Active.Map.FindLayers("Roads").First();

        var editTemplates = featLayer.GetTemplates();
        var newCIMEditingTemplates = new List<CIMEditingTemplate>();

        foreach (var et in editTemplates)
        {
          //initialize template by activating default tool
          et.ActivateDefaultToolAsync();
          var cimEditTemplate = et.GetDefinition();
          //get the visible tools on this template
          var allTools = et.ToolIDs.ToList();
          //add the hidden tools on this template
          allTools.AddRange(cimEditTemplate.GetExcludedToolDamlIds().ToList());
          //hide all the tools then allow the line tool
          cimEditTemplate.SetExcludedToolDamlIds(allTools.ToArray());
          cimEditTemplate.AllowToolDamlID("esri_editing_SketchLineTool");
          newCIMEditingTemplates.Add(cimEditTemplate);
        }
        //update the layer templates
        var layerDef = featLayer.GetDefinition() as CIMFeatureLayer;
        // Set AutoGenerateFeatureTemplates to false for template changes to stick
        layerDef.AutoGenerateFeatureTemplates = false;
        layerDef.FeatureTemplates = newCIMEditingTemplates.ToArray();
        featLayer.SetDefinition(layerDef);
      });
      #endregion
    }

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

      #region Create New Table Template using table.CreateTemplate
      var table = MapView.Active.Map.GetStandaloneTablesAsFlattenedList().FirstOrDefault();
      if (table == null)
        return;
      QueuedTask.Run(() =>
      {
        var tableTemplate = table.GetTemplate("Template1");
        
        var definition = tableTemplate.GetDefinition();
        definition.Description = "New definition";
        definition.Name = "New name";
        //Create new table template using this definition
        table.CreateTemplate(definition);

        //You can also create a new table template using this extension method. You can use this method the same way you use the layer.CreateTemplate method.
        table.CreateTemplate("New template name", "Template description", tags: new string[] { "tag 1", "tag 2" });
      });
      #endregion

      #region Update a Table Template
      QueuedTask.Run(() =>
      {
        var tableTemplate = table.GetTemplate("Template1");

        var definition = tableTemplate.GetDefinition();
        definition.Description = "New definition";
        definition.Name = "New name";
        // update the definition
        tableTemplate.SetDefinition(definition);
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

    public void RemoveTemplate()
    {
      #region Remove a table template
      var table = MapView.Active.Map.GetStandaloneTablesAsFlattenedList().FirstOrDefault();
      if (table == null)
        return;
      QueuedTask.Run(() =>
      {
        var tableTemplate = table.GetTemplate("Template1");
        //Removing a table template
        table.RemoveTemplate(tableTemplate);
        //Removing a template by name
        table.RemoveTemplate("Template2");
      });
      #endregion
    }

    public void TemplateChanged()
    {
      #region Active Template Changed

      ArcGIS.Desktop.Editing.Events.ActiveTemplateChangedEvent.Subscribe(OnActiveTemplateChanged);

      async void OnActiveTemplateChanged(ArcGIS.Desktop.Editing.Events.ActiveTemplateChangedEventArgs args)
      {
        // return if incoming template is null
        if (args.IncomingTemplate == null)
          return;

        // Activate two-point line tool for Freeway template in the Layers map
        if (args.IncomingTemplate.Name == "Freeway" && args.IncomingMapView.Map.Name == "Layers")
          await args.IncomingTemplate.ActivateToolAsync("esri_editing_SketchTwoPointLineTool");
      }
      #endregion
    }

    #region ProSnippet Group: Annotation
    #endregion

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

    public static void StartEditAnnotationTool()
    {
      #region Programmatically start Edit Annotation

      var plugin = FrameworkApplication.GetPlugInWrapper("esri_editing_EditVerticesText");
      if (plugin.Enabled)
        ((ICommand)plugin).Execute(null);
      #endregion
    }

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

    #region ProSnippet Group: EditingOptions
    #endregion

    public void EditingOptions()
		{
      #region Get/Set Editing Options

      //toggle, switch option values
      var options = ApplicationOptions.EditingOptions;

      options.EnforceAttributeValidation = !options.EnforceAttributeValidation;
      options.WarnOnSubtypeChange = !options.WarnOnSubtypeChange;
      options.InitializeDefaultValuesOnSubtypeChange = !options.InitializeDefaultValuesOnSubtypeChange;
      options.UncommitedAttributeEdits = (options.UncommitedAttributeEdits == 
        UncommitedEditMode.AlwaysPrompt) ? UncommitedEditMode.Apply : UncommitedEditMode.AlwaysPrompt;

      options.StretchGeometry = !options.StretchGeometry;
      options.StretchTopology = !options.StretchTopology;
      options.UncommitedGeometryEdits = (options.UncommitedGeometryEdits == 
        UncommitedEditMode.AlwaysPrompt) ? UncommitedEditMode.Apply : UncommitedEditMode.AlwaysPrompt;

      options.ActivateMoveAfterPaste = !options.ActivateMoveAfterPaste;
      options.ShowFeatureSketchSymbology = !options.ShowFeatureSketchSymbology;
      options.FinishSketchOnDoubleClick = !options.FinishSketchOnDoubleClick;
      options.AllowVertexEditingWhileSketching = !options.AllowVertexEditingWhileSketching;
      options.ShowDeleteDialog = !options.ShowDeleteDialog;
      options.EnableStereoEscape = !options.EnableStereoEscape;
      options.DragSketch = !options.DragSketch;
      options.ShowDynamicConstraints = !options.ShowDynamicConstraints;
      options.IsDeflectionDefaultDirectionConstraint = 
        !options.IsDeflectionDefaultDirectionConstraint;
      options.IsDirectionDefaultInputConstraint = !options.IsDirectionDefaultInputConstraint;
      options.ShowEditingToolbar = !options.ShowEditingToolbar;
      options.ToolbarPosition = (options.ToolbarPosition == ToolbarPosition.Bottom) ? 
                ToolbarPosition.Right : ToolbarPosition.Bottom;
      options.ToolbarSize = (options.ToolbarSize == ToolbarSize.Medium) ? 
                ToolbarSize.Small : ToolbarSize.Medium;
      options.MagnifyToolbar = !options.MagnifyToolbar;

      options.EnableEditingFromEditTab = !options.EnableEditingFromEditTab;
      options.AutomaticallySaveEdits = !options.AutomaticallySaveEdits;
      options.AutoSaveByTime = !options.AutoSaveByTime;
      options.SaveEditsInterval = (options.AutomaticallySaveEdits) ? 20 : 10;
      options.SaveEditsOperations = (options.AutomaticallySaveEdits) ? 60 : 30;
      options.SaveEditsOnProjectSave = !options.SaveEditsOnProjectSave;
      options.ShowSaveEditsDialog = !options.ShowSaveEditsDialog;
      options.ShowDiscardEditsDialog = !options.ShowDiscardEditsDialog;
      options.DeactivateToolOnSaveOrDiscard = !options.DeactivateToolOnSaveOrDiscard;
      options.NewLayersEditable = !options.NewLayersEditable;

      #endregion

    }

    public void SymbologyOptions()
		{
      #region Get Sketch Vertex Symbology Options

      var options = ApplicationOptions.EditingOptions;

      //Must use QueuedTask
      QueuedTask.Run(() =>
      {
        //There are 4 vertex symbol settings - selected, unselected and the
        //current vertex selected and unselected.
        var reg_select = options.GetVertexSymbolOptions(VertexSymbolType.RegularSelected);
        var reg_unsel = options.GetVertexSymbolOptions(VertexSymbolType.RegularUnselected);
        var curr_sel = options.GetVertexSymbolOptions(VertexSymbolType.CurrentSelected);
        var curr_unsel = options.GetVertexSymbolOptions(VertexSymbolType.CurrentUnselected);

        //to convert the options to a symbol use
        //GetPointSymbol
        var reg_sel_pt_symbol = reg_select.GetPointSymbol();
        //ditto for reg_unsel, curr_sel, curr_unsel
      });

      #endregion

      #region Get Sketch Segment Symbology Options

      //var options = ApplicationOptions.EditingOptions;
      QueuedTask.Run(() =>
      {
        var seg_options = options.GetSegmentSymbolOptions();
        //to convert the options to a symbol use
        //SymbolFactory. Note: this is approximate....sketch isn't using the
        //CIM directly for segments
        var layers = new List<CIMSymbolLayer>();
        var stroke0 = SymbolFactory.Instance.ConstructStroke(seg_options.PrimaryColor, 
          seg_options.Width, SimpleLineStyle.Dash);
        layers.Add(stroke0);
        if (seg_options.HasSecondaryColor) {
          var stroke1 = SymbolFactory.Instance.ConstructStroke(
            seg_options.SecondaryColor, seg_options.Width, SimpleLineStyle.Solid);
          layers.Add(stroke1);
        }
        //segment symbology only
        var sketch_line = new CIMLineSymbol() {
          SymbolLayers = layers.ToArray()
        };
      });
      #endregion

      #region Set Sketch Vertex Symbol Options

      //var options = ApplicationOptions.EditingOptions;
      QueuedTask.Run(() =>
      {
        //change the regular unselected vertex symbol
        //default is a green, hollow, square, 5pts. Change to
        //Blue outline diamond, 10 pts
        var vertexSymbol = new VertexSymbolOptions(VertexSymbolType.RegularUnselected);
        vertexSymbol.OutlineColor = ColorFactory.Instance.BlueRGB;
        vertexSymbol.MarkerType = VertexMarkerType.Diamond;
        vertexSymbol.Size = 10;

        //Are these valid?
        if (options.CanSetVertexSymbolOptions(
             VertexSymbolType.RegularUnselected, vertexSymbol)) {
          //apply them
          options.SetVertexSymbolOptions(VertexSymbolType.RegularUnselected, vertexSymbol);
				}
      });

      #endregion

      #region Set Sketch Segment Symbol Options

      //var options = ApplicationOptions.EditingOptions;
      QueuedTask.Run(() =>
      {
        //change the segment symbol primary color to green and
        //width to 1 pt
        var segSymbol = options.GetSegmentSymbolOptions();
        segSymbol.PrimaryColor = ColorFactory.Instance.GreenRGB;
        segSymbol.Width = 1;

        //Are these valid?
        if (options.CanSetSegmentSymbolOptions(segSymbol)) {
          //apply them
          options.SetSegmentSymbolOptions(segSymbol);
				}
      });

      #endregion

      #region Set Sketch Vertex Symbol Back to Default

      //var options = ApplicationOptions.EditingOptions;
      QueuedTask.Run(() =>
      {
        //ditto for reg selected and current selected, unselected
        var def_reg_unsel = 
          options.GetDefaultVertexSymbolOptions(VertexSymbolType.RegularUnselected);
        //apply default
        options.SetVertexSymbolOptions(
          VertexSymbolType.RegularUnselected, def_reg_unsel);
      });

      #endregion

      #region Set Sketch Segment Symbol Back to Default

      //var options = ApplicationOptions.EditingOptions;
      QueuedTask.Run(() =>
      {
        var def_seg = options.GetDefaultSegmentSymbolOptions();
        options.SetSegmentSymbolOptions(def_seg);
      });

      #endregion
    }

    #region ProSnippet Group: VersioningOptions
    #endregion

    public void VersioningOptions()
		{

			#region Get and Set Versioning Options

			var vOptions = ApplicationOptions.VersioningOptions;

      vOptions.DefineConflicts = (vOptions.DefineConflicts == ConflictDetectionType.ByRow) ? 
        ConflictDetectionType.ByColumn : ConflictDetectionType.ByRow;
      vOptions.ConflictResolution = (
        vOptions.ConflictResolution == ConflictResolutionType.FavorEditVersion) ? 
          ConflictResolutionType.FavorTargetVersion : ConflictResolutionType.FavorEditVersion;
      vOptions.ShowConflictsDialog = !vOptions.ShowConflictsDialog;
      vOptions.ShowReconcileDialog = !vOptions.ShowReconcileDialog;

      #endregion
    }
  }
}