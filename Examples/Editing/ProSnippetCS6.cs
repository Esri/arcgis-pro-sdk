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
using ArcGIS.Core.Data.Topology;
using ArcGIS.Desktop.Framework.Dialogs;

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


    #region ProSnippet Group: Edit Templates
    #endregion

    public void Templates()
    {
      // cref: ARCGIS.DESKTOP.MAPPING.MAP.FINDLAYERS
      // cref: ARCGIS.DESKTOP.MAPPING.MAP.FINDSTANDALONETABLES
      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.GetTemplate(ArcGIS.Desktop.Mapping.MapMember,System.String)
      // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate
      // cref: ArcGIS.Desktop.Editing.Templates.EditingRowTemplate
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

      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.GetTemplates(ArcGIS.Desktop.Mapping.MapMember)
      // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate
      // cref: ArcGIS.Desktop.Editing.Templates.EditingRowTemplate
      // cref: ARCGIS.DESKTOP.MAPPING.MAP.FINDSTANDALONETABLES(System.String)
      // cref: ArcGIS.Desktop.Mapping.Map.GetStandaloneTablesAsFlattenedList
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

      // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate.Current
      #region Current template
      EditingTemplate template = EditingTemplate.Current;
      #endregion

    }

    // cref: ArcGIS.Desktop.Mapping.MappingExtensions.GetTemplate(ArcGIS.Desktop.Mapping.MapMember,System.String)
    // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate
    // cref: ArcGIS.Desktop.Editing.Templates.EditingRowTemplate
    // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate
    // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate.DefaultToolID
    // cref: ArcGIS.Core.CIM.CIMBasicFeatureLayer.AutoGenerateFeatureTemplates
    // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate.GetDefinition
    // cref: ArcGIS.Core.CIM.CIMEditingTemplate
    // cref: ArcGIS.Core.CIM.CIMBasicRowTemplate
    // cref: ArcGIS.Core.CIM.CIMRowTemplate
    // cref: ArcGIS.Core.CIM.CIMEditingTemplate.DefaultToolGUID
    // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate.GetDefinition()
    // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate.SetDefinition(ArcGIS.Core.CIM.CIMEditingTemplate)
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

          //At 2.x -
          //templateDef.ToolProgID = toolContentGUID;
          templateDef.DefaultToolGUID = toolContentGUID;

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
      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.GetTemplates(ArcGIS.Desktop.Mapping.MapMember)
      // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate
      // cref: ArcGIS.Core.CIM.CIMEditingTemplate
      // cref: ArcGIS.Core.CIM.CIMBasicRowTemplate
      // cref: ArcGIS.Core.CIM.CIMRowTemplate
      // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate.GetDefinition()
      // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate.ActivateDefaultToolAsync
      // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate.ToolIDs
      // cref: ArcGIS.Core.CIM.CIMExtensions.GetExcludedToolIDs(ArcGIS.Core.CIM.CIMEditingTemplate)
      // cref: ArcGIS.Core.CIM.CIMExtensions.SetExcludedToolIDs(ArcGIS.Core.CIM.CIMEditingTemplate,System.String[])
      // cref: ArcGIS.Core.CIM.CIMExtensions.AllowToolID(ArcGIS.Core.CIM.CIMEditingTemplate,System.String)
      // cref: ArcGIS.Core.CIM.CIMBasicFeatureLayer.FeatureTemplates
      // cref: ARCGIS.DESKTOP.MAPPING.LAYER.GETDEFINITION
      // cref: ARCGIS.DESKTOP.MAPPING.LAYER.SETDEFINITION
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
          allTools.AddRange(cimEditTemplate.GetExcludedToolIDs().ToList());
          //hide all the tools then allow the line tool
  
          //At 2.x -
          //allTools.AddRange(cimEditTemplate.GetExcludedToolDamlIds().ToList());
          allTools.AddRange(cimEditTemplate.GetExcludedToolIDs().ToList());
          
          //At 2.x - 
          //cimEditTemplate.SetExcludedToolDamlIds(allTools.ToArray());
          //cimEditTemplate.AllowToolDamlID("esri_editing_SketchLineTool");
          
          cimEditTemplate.SetExcludedToolIDs(allTools.ToArray());
          cimEditTemplate.AllowToolID("esri_editing_SketchLineTool");
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

      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.LoadSchema(ArcGIS.Desktop.Mapping.MapMember)
      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.CreateTemplate(ArcGIS.Desktop.Mapping.MapMember,System.String,System.String,ArcGIS.Desktop.Editing.Attributes.Inspector,System.String,System.String[],System.String[])
      // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate
      // cref: ArcGIS.Desktop.Editing.Templates.EditingRowTemplate
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

      // cref: ARCGIS.DESKTOP.EDITING.TEMPLATES.EDITINGTEMPLATE.GETDEFINITION
      // cref: ArcGIS.Core.CIM.CIMEditingTemplate
      // cref: ArcGIS.Core.CIM.CIMEditingTemplate.Description
      // cref: ArcGIS.Core.CIM.CIMEditingTemplate.Name
      // cref: ArcGIS.Core.CIM.CIMBasicRowTemplate
      // cref: ArcGIS.Core.CIM.CIMRowTemplate
      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.CreateTemplate(ArcGIS.Desktop.Mapping.MapMember,ArcGIS.Core.CIM.CIMEditingTemplate)
      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.CreateTemplate(ArcGIS.Desktop.Mapping.MapMember,System.String,System.String,ArcGIS.Desktop.Editing.Attributes.Inspector,System.String,System.String[],System.String[])
      // cref: ArcGIS.Desktop.Editing.Templates.EditingTemplate
      // cref: ArcGIS.Desktop.Editing.Templates.EditingRowTemplate
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

      // cref: ARCGIS.DESKTOP.EDITING.TEMPLATES.EDITINGTEMPLATE.GETDEFINITION
      // cref: ARCGIS.DESKTOP.EDITING.TEMPLATES.EDITINGTEMPLATE.SETDEFINITION(ArcGIS.Core.CIM.CIMEditingTemplate)
      // cref: ArcGIS.Core.CIM.CIMEditingTemplate
      // cref: ArcGIS.Core.CIM.CIMEditingTemplate.Description
      // cref: ArcGIS.Core.CIM.CIMEditingTemplate.Name
      // cref: ArcGIS.Core.CIM.CIMBasicRowTemplate
      // cref: ArcGIS.Core.CIM.CIMRowTemplate
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

      // cref: ArcGIS.Core.Data.Mapping.AnnotationFeatureClassDefinition
      // cref: ArcGIS.Core.Data.Mapping.AnnotationFeatureClassDefinition.GetSymbolCollection
      // cref: ArcGIS.Core.Data.Mapping.AnnotationFeatureClassDefinition.GetLabelClassCollection
      // cref: ARCGIS.DESKTOP.MAPPING.FEATURELAYER.GETFEATURECLASS
      // cref: ArcGIS.Desktop.Editing.AnnotationProperties
      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.LoadSchema(ArcGIS.Desktop.Mapping.MapMember)
      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.GetAnnotationProperties
      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.SetAnnotationProperties
      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.CreateTemplate(ArcGIS.Desktop.Mapping.MapMember,System.String,System.String,ArcGIS.Desktop.Editing.Attributes.Inspector,System.String,System.String[],System.String[])
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
      // cref: ArcGIS.Desktop.Mapping.Map.GetStandaloneTablesAsFlattenedList
      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.RemoveTemplate(ArcGIS.Desktop.Mapping.MapMember,ArcGIS.Desktop.Editing.Templates.EditingTemplate)
      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.RemoveTemplate(ArcGIS.Desktop.Mapping.MapMember,System.String)
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
      // cref: ArcGIS.Desktop.Editing.Events.ActiveTemplateChangedEvent
      // cref: ArcGIS.Desktop.Editing.Events.ActiveTemplateChangedEventArgs
      // cref: ArcGIS.Desktop.Editing.Events.ActiveTemplateChangedEventArgs.IncomingTemplate
      // cref: ARCGIS.DESKTOP.EDITING.TEMPLATES.EDITINGTEMPLATE.ACTIVATETOOLASYNC
      // cref: ArcGIS.Desktop.Editing.Events.ActiveTemplateChangedEvent.Subscribe(System.Action{ArcGIS.Desktop.Editing.Events.ActiveTemplateChangedEventArgs},System.Boolean)
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

    // cref: ARCGIS.DESKTOP.MAPPING.MAPTOOL.ONSKETCHCOMPLETEASYNC
    // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.GetAnnotationProperties
    // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.SetAnnotationProperties(ArcGIS.Desktop.Editing.AnnotationProperties)
    // cref: ArcGIS.Desktop.Editing.AnnotationProperties
    // cref: ArcGIS.Core.CIM.HorizontalAlignment
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

        if (!createOperation.IsEmpty)
        {
          // Execute the operation
          return createOperation.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
        }
        else
          return false;
      });
      return result;
    }

    #endregion

    public static void StartEditAnnotationTool()
    {
      // cref: ARCGIS.DESKTOP.FRAMEWORK.FRAMEWORKAPPLICATION.GETPLUGINWRAPPER
      // cref: ARCGIS.DESKTOP.FRAMEWORK.IPLUGINWRAPPER
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

      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.GetAnnotationProperties
      // cref: ArcGIS.Desktop.Editing.AnnotationProperties.TextString
      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.SetAnnotationProperties(ArcGIS.Desktop.Editing.AnnotationProperties)
      // cref: ArcGIS.Desktop.Editing.EditOperation.Modify(ArcGIS.Desktop.Editing.Attributes.Inspector)
      #region Update Annotation Text 

      await QueuedTask.Run(() =>
      {
        //annoLayer is ~your~ Annotation layer...

        // use the inspector methodology
        //at 2.x - var insp = new Inspector(true);
        var insp = new Inspector();
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
        if (!op.IsEmpty)
        {
          var result = op.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
        }
      });
      #endregion

      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.GetAnnotationProperties
      // cref: ArcGIS.Desktop.Editing.AnnotationProperties.Shape
      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.SetAnnotationProperties(ArcGIS.Desktop.Editing.AnnotationProperties)
      // cref: ArcGIS.Desktop.Editing.EditOperation.Modify(ArcGIS.Desktop.Editing.Attributes.Inspector)
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
        //at 2.x - var insp = new Inspector(true);
        var insp = new Inspector();
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
          if (!op.IsEmpty)
          {
            var result = op.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
          }
        }
      });

      #endregion

      // cref: ArcGIS.Core.CIM.HorizontalAlignment
      // cref: ArcGIS.Desktop.Editing.AnnotationProperties.LoadFromTextGraphic(ArcGIS.Core.CIM.CIMTextGraphic)
      // cref: ArcGIS.Desktop.Editing.AnnotationProperties.TextGraphic
      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.GetAnnotationProperties
      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.SetAnnotationProperties(ArcGIS.Desktop.Editing.AnnotationProperties)
      // cref: ArcGIS.Desktop.Editing.EditOperation.Modify(ArcGIS.Desktop.Editing.Attributes.Inspector)
      #region Modify Annotation Text Graphic

      await QueuedTask.Run(() =>
      {

        var selection = annoLayer.GetSelection();
        if (selection.GetCount() == 0)
          return;

        // use the first selelcted feature 
        //at 2.x - var insp = new Inspector(true);
        var insp = new Inspector();
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
        if (!op.IsEmpty)
        {
          bool result = op.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
        }
      });

      #endregion
    }


    #region ProSnippet Group: Undo / Redo
    #endregion
    public void OpMgr()
    {
      var editOp = new EditOperation();
      editOp.Name = "My Name";
      if (!editOp.IsEmpty)
      {
        var result = editOp.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
      }

      //elsewhere
      editOp.UndoAsync();

      // cref: ArcGIS.Desktop.Framework.OperationManager
      // cref: ArcGIS.Desktop.Framework.OperationManager.CanUndo
      // cref: ArcGIS.Desktop.Framework.OperationManager.UndoAsync()
      // cref: ArcGIS.Desktop.Framework.OperationManager.CanRedo
      // cref: ArcGIS.Desktop.Framework.OperationManager.RedoAsync()
      // cref: ARCGIS.DESKTOP.MAPPING.MAP.OPERATIONMANAGER
      #region Undo/Redo the Most Recent Operation

      //undo
      if (MapView.Active.Map.OperationManager.CanUndo)
        MapView.Active.Map.OperationManager.UndoAsync();//await as needed

      //redo
      if (MapView.Active.Map.OperationManager.CanRedo)
        MapView.Active.Map.OperationManager.RedoAsync();//await as needed

      #endregion
    }

    #region ProSnippet Group: Topology Properties
    #endregion

    public async void Topology()
    {
      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.GetAvailableTopologiesAsync(ArcGIS.Desktop.Mapping.Map)
      // cref: ArcGIS.Desktop.Editing.TopologyProperties
      // cref: ArcGIS.Desktop.Editing.MapTopologyProperties
      // cref: ArcGIS.Desktop.Editing.GeodatabaseTopologyProperties
      #region Get List of available topologies in the map

      QueuedTask.Run(async () =>
      {
        var map = MapView.Active.Map;
        //Get a list of all the available topologies for the map
        var availableTopologies = await map.GetAvailableTopologiesAsync();

        var gdbTopologies = availableTopologies.OfType<GeodatabaseTopologyProperties>();
        var mapTopologies = availableTopologies.OfType<MapTopologyProperties>();
      });
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.GetActiveTopologyAsync(ArcGIS.Desktop.Mapping.Map, System.String)
      // cref: ArcGIS.Desktop.Editing.TopologyProperties
      // cref: ArcGIS.Desktop.Editing.MapTopologyProperties
      // cref: ArcGIS.Desktop.Editing.GeodatabaseTopologyProperties
      // cref: ArcGIS.Desktop.Editing.NoTopologyProperties
      #region Get the properties of the active topology in the map
      var map = MapView.Active.Map;
      var activeTopologyProperties = await map.GetActiveTopologyAsync();
      var isMapTopology = activeTopologyProperties is MapTopologyProperties;
      var isGdbTopology = activeTopologyProperties is GeodatabaseTopologyProperties;
      var isNoTopology = activeTopologyProperties is NoTopologyProperties;
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.GetTopologyAsync(ArcGIS.Desktop.Mapping.Map, System.String)
      // cref: ArcGIS.Desktop.Editing.MapTopologyProperties
      // cref: ArcGIS.Desktop.Editing.MapTopologyProperties.Tolerance
      // cref: ArcGIS.Desktop.Editing.MapTopologyProperties.DefaultTolerance
      #region Get map topology properties 
      var mapTopoProperties = await map.GetTopologyAsync("Map") as MapTopologyProperties;
      var tolerance_m = mapTopoProperties.Tolerance;
      var defaultTolerance_m = mapTopoProperties.DefaultTolerance;
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.GetTopologyAsync(ArcGIS.Desktop.Mapping.Map, System.String)
      // cref: ArcGIS.Desktop.Editing.GeodatabaseTopologyProperties
      // cref: ArcGIS.Desktop.Editing.GeodatabaseTopologyProperties.WorkspaceName
      // cref: ArcGIS.Desktop.Editing.GeodatabaseTopologyProperties.TopologyLayer
      // cref: ArcGIS.Desktop.Editing.GeodatabaseTopologyProperties.ClusterTolerance
      #region Get geodatabase topology properties by name
      var topoProperties = await map.GetTopologyAsync("TopologyName") as GeodatabaseTopologyProperties;

      var workspace = topoProperties.WorkspaceName;
      var topoLayer = topoProperties.TopologyLayer;
      var clusterTolerance = topoProperties.ClusterTolerance;

      #endregion

      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.CanSetMapTopology(ArcGIS.Desktop.Mapping.Map)
      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.SetMapTopologyAsync(ArcGIS.Desktop.Mapping.Map)
      // cref: ArcGIS.Desktop.Editing.MapTopologyProperties
      #region Set Map Topology as the current topology
      if (map.CanSetMapTopology())
      {
        //Set the topology of the map as map topology
        mapTopoProperties = await map.SetMapTopologyAsync() as MapTopologyProperties;
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.CanClearTopology(ArcGIS.Desktop.Mapping.Map)
      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.ClearTopologyAsync(ArcGIS.Desktop.Mapping.Map)
      #region Set 'No Tpology' as the current topology

      if (map.CanClearTopology())
      {
        //Clears the topology of the map - no topology
        await map.ClearTopologyAsync();
      }

      #endregion

      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.CanSetActiveTopology(ArcGIS.Desktop.Mapping.Map, System.String)
      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.SetActiveTopologyAsync(ArcGIS.Desktop.Mapping.Map, System.String)
      #region Set the current topology by name

      if (map.CanSetActiveTopology("TopologyName"))
      {
        await map.SetActiveTopologyAsync("TopologyName");
      }

      #endregion

      GeodatabaseTopologyProperties gdbTopoProperties = null;

      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.CanSetActiveTopology(ArcGIS.Desktop.Mapping.Map, ArcGIS.Desktop.Editing.TopologyProperties)
      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.SetActiveTopologyAsync(ArcGIS.Desktop.Mapping.Map, ArcGIS.Desktop.Editing.TopologyProperties)
      #region Set the current topology by topologyProperties 

      if (map.CanSetActiveTopology(gdbTopoProperties))
      {
        await map.SetActiveTopologyAsync(gdbTopoProperties);
      }

      #endregion
    }

    #region ProSnippet Group: Map Topology
    #endregion

    // cref: ArcGIS.Core.Data.Topology.TopologyDefinition
    // cref: ArcGIS.Desktop.Mapping.MappingExtensions.BuildMapTopologyGraph<T>(ArcGIS.Desktop.Mapping.MapView, System.Action<ArcGIS.Core.Data.Topology.TopologyGraph>)
    // cref: ArcGIS.Core.Data.Topology.TopologyGraph.GetNodes()
    // cref: ArcGIS.Core.Data.Topology.TopologyGraph.GetEdges()
    // cref: ArcGIS.Core.Data.Topology.TopologyNode
    // cref: ArcGIS.Core.Data.Topology.TopologyEdge
    #region Build Map Topology
    private async Task BuildGraphWithActiveView()
    {
      await QueuedTask.Run(() =>
      {
        //Build the map topology graph
        MapView.Active.BuildMapTopologyGraph<TopologyDefinition>(async topologyGraph =>
        {
          //Getting the nodes and edges present in the graph
          var topologyGraphNodes = topologyGraph.GetNodes();
          var topologyGraphEdges = topologyGraph.GetEdges();

          foreach (var node in topologyGraphNodes)
          {
            // do something with the node
          }
          foreach (var edge in topologyGraphEdges)
          {
            // do something with the edge
          }

          MessageBox.Show($"Number of topo graph nodes are:  {topologyGraphNodes.Count}.\n Number of topo graph edges are {topologyGraphEdges.Count}.", "Map Topology Info");
        });
      });
    }
    #endregion


    #region ProSnippet Group: Attributes Pane Context MenuItems
    #endregion

    protected async void ContextMenuItem()
    {
      // cref: ArcGIS.Desktop.Framework.FrameworkApplication.ContextMenuDataContextAs<T>
      #region Retrieve SelectionSet from command added to Attribute Pane Context Menu  
      await QueuedTask.Run(async () =>
      {
        var selSet = FrameworkApplication.ContextMenuDataContextAs<SelectionSet>();
        if (selSet == null)
          return;

        int count = selSet.Count;
        if (count == 0)
          return;

        var op = new EditOperation();
        op.Name = "Delete context";
        op.Delete(selSet);
        await op.ExecuteAsync();
      });
      #endregion
    }

    #region ProSnippet Group: Ground to Grid
    #endregion

    internal void G2G()
    {
      // cref: ArcGIS.Core.CIM.CIMGroundToGridCorrection
      // cref: ArcGIS.Desktop.Editing.GroundToGridCorrection.IsCorrecting(ArcGIS.Core.CIM.CIMGroundToGridCorrection)
      // cref: ArcGIS.Desktop.Editing.GroundToGridCorrection.UsingDirectionOffset(ArcGIS.Core.CIM.CIMGroundToGridCorrection)
      // cref: ArcGIS.Desktop.Editing.GroundToGridCorrection.GetDirectionOffset(ArcGIS.Core.CIM.CIMGroundToGridCorrection)
      // cref: ArcGIS.Desktop.Editing.GroundToGridCorrection.UsingDistanceFactor(ArcGIS.Core.CIM.CIMGroundToGridCorrection)
      // cref: ArcGIS.Desktop.Editing.GroundToGridCorrection.UsingElevationMode(ArcGIS.Core.CIM.CIMGroundToGridCorrection)
      // cref: ArcGIS.Desktop.Editing.GroundToGridCorrection.UsingConstantScaleFactor(ArcGIS.Core.CIM.CIMGroundToGridCorrection)
      // cref: ArcGIS.Desktop.Editing.GroundToGridCorrection.GetConstantScaleFactor(ArcGIS.Core.CIM.CIMGroundToGridCorrection)
      #region G2G Settings
      CIMGroundToGridCorrection correction = null;
      bool isCorecting = correction.IsCorrecting();   // equivalent to correction != null && correction.Enabled;
      bool UsingOffset = correction.UsingDirectionOffset();   // equivalent to correction.IsCorrecting() && correction.UseDirection;
      double dOffset = correction.GetDirectionOffset(); // equivalent to correction.UsingDirectionOffset() ? correction.Direction : DefaultDirectionOffset;
      bool usingDistanceFactor = correction.UsingDistanceFactor();  // equivalent to correction.IsCorrecting() && correction.UseScale;
      bool usingElevation = correction.UsingElevationMode(); // equivalent to correction.UsingDistanceFactor() && c.ScaleType == GroundToGridScaleType.ComputeUsingElevation;
      bool usingSFactor = correction.UsingConstantScaleFactor();  //; equivalent to correction.UsingDistanceFactor() && correction.ScaleType == GroundToGridScaleType.ConstantFactor;
      double dSFactor = correction.GetConstantScaleFactor(); // equivalent to correctionc.UsingDistanceFactor() ? correction.ConstantScaleFactor : DefaultConstantScaleFactor;
      #endregion
    }

    #region ProSnippet Group: EditingOptions
    #endregion

    public void EditingOptions()
		{
      // cref: ArcGIS.Desktop.Core.ApplicationOptions.EditingOptions
      // cref: ArcGIS.Desktop.Core.EditingOptions
      // cref: ArcGIS.Desktop.Core.UncommitedEditMode
      // cref: ArcGIS.Desktop.Core.ToolbarPosition
      // cref: ArcGIS.Desktop.Core.ToolbarSize
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

      // cref: ArcGIS.Desktop.Core.ApplicationOptions.EditingOptions
      // cref: ArcGIS.Desktop.Core.EditingOptions
      // cref: ArcGIS.Desktop.Core.AnnotationFollowMode
      // cref: ArcGIS.Desktop.Core.AnnotationPlacementMode
      #region Get/Set Editing Annotation Options
      var eOptions = ApplicationOptions.EditingOptions;

      var followLinkedLines = eOptions.AutomaticallyFollowLinkedLineFeatures;
      var followLinedPolygons = eOptions.AutomaticallyFollowLinkedPolygonFeatures;
      var usePlacementProps = eOptions.UseAnnotationPlacementProperties;
      var followMode = eOptions.AnnotationFollowMode;
      var placementMode = eOptions.AnnotationPlacementMode;

     
      eOptions.AnnotationFollowMode = AnnotationFollowMode.Parallel;
      eOptions.AnnotationPlacementMode = AnnotationPlacementMode.Left;
      #endregion
    }

    public void SymbologyOptions()
		{
      // cref: ArcGIS.Desktop.Core.ApplicationOptions.EditingOptions
      // cref: ArcGIS.Desktop.Core.EditingOptions
      // cref: ArcGIS.Desktop.Core.EditingOptions.GetVertexSymbolOptions(ArcGIS.Desktop.Core.VertexSymbolType)
      // cref: ArcGIS.Desktop.Core.VertexSymbolOptions
      // cref: ArcGIS.Desktop.Core.VertexSymbolOptions.GetPointSymbol()
      // cref: ArcGIS.Desktop.Core.VertexSymbolType
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

      // cref: ArcGIS.Desktop.Core.ApplicationOptions.EditingOptions
      // cref: ArcGIS.Desktop.Core.EditingOptions.GetSegmentSymbolOptions()
      // cref: ArcGIS.Desktop.Core.SegmentSymbolOptions
      // cref: ArcGIS.Desktop.Core.SegmentSymbolOptions.PrimaryColor
      // cref: ArcGIS.Desktop.Core.SegmentSymbolOptions.Width
      // cref: ArcGIS.Desktop.Core.SegmentSymbolOptions.HasSecondaryColor
      // cref: ArcGIS.Desktop.Core.SegmentSymbolOptions.SecondaryColor
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

      // cref: ArcGIS.Desktop.Core.VertexSymbolOptions
      // cref: ArcGIS.Desktop.Core.VertexSymbolType
      // cref: ArcGIS.Desktop.Core.VertexSymbolOptions.#ctor(ArcGIS.Desktop.Core.VertexSymbolType)
      // cref: ArcGIS.Desktop.Core.VertexSymbolOptions.OutlineColor
      // cref: ArcGIS.Desktop.Core.VertexSymbolOptions.MarkerType
      // cref: ArcGIS.Desktop.Core.VertexMarkerType
      // cref: ArcGIS.Desktop.Core.VertexSymbolOptions.Size
      // cref: ArcGIS.Desktop.Core.EditingOptions.CanSetVertexSymbolOptions(ArcGIS.Desktop.Core.VertexSymbolType, ArcGIS.Desktop.Core.VertexSymbolOptions)
      // cref: ArcGIS.Desktop.Core.EditingOptions.SetVertexSymbolOptions(ArcGIS.Desktop.Core.VertexSymbolType, ArcGIS.Desktop.Core.VertexSymbolOptions)
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

      // cref: ArcGIS.Desktop.Core.EditingOptions.GetSegmentSymbolOptions()
      // cref: ArcGIS.Desktop.Core.SegmentSymbolOptions
      // cref: ArcGIS.Desktop.Core.EditingOptions.CanSetSegmentSymbolOptions(ArcGIS.Desktop.Core.SegmentSymbolOptions)
      // cref: ArcGIS.desktop.Core.EditingOptions.SetSegmentSymbolOptions(ArcGIS.Desktop.Core.SegmentSymbolOptions)
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

      // cref: ArcGIS.Desktop.Core.VertexSymbolType
      // cref: ArcGIS.Desktop.Core.EditingOptions.GetDefaultVertexSymbolOptions(ArcGIS.Desktop.Core.VertexSymbolType)
      // cref: ArcGIS.Desktop.Core.VertexSymbolOptions
      // cref: ArcGIS.Desktop.Core.EditingOptions.SetVertexSymbolOptions(ArcGIS.Desktop.Core.VertexSymbolType, ArcGIS.Desktop.Core.VertexSymbolOptions)
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

      // cref: ArcGIS.Desktop.Core.EditingOptions.GetDefaultSegmentSymbolOptions
      // cref: ArcGIS.Desktop.Core.SegmentSymbolOptions
      // cref: ArcGIS.Desktop.Core.EditingOptions.SetSegmentSymbolOptions
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

      // cref: ArcGIS.Desktop.Core.ApplicationOptions.VersioningOptions
      // cref: ArcGIS.Desktop.Core.VersioningOptions
      // cref: ArcGIS.Desktop.Core.VersioningOptions.DefineConflicts
      // cref: ArcGIS.Desktop.Core.VersioningOptions.ConflictResolution
      // cref: ArcGIS.Desktop.Core.VersioningOptions.ShowConflictsDialog
      // cref: ArcGIS.Desktop.Core.VersioningOptions.ShowReconcileDialog
      // cref: ArcGIS.Core.Data.ConflictDetectionType
      // cref: ArcGIS.Core.Data.ConflictResolutionType
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
