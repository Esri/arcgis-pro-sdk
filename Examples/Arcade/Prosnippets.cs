/*

   Copyright 2023 Esri

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
using System.Text;
using System.Windows;
using System.Windows.Input;
using ArcGIS.Core.Arcade;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using System.Threading.Tasks;
using System;
using ArcGIS.Desktop.Catalog;
using System.Linq;
using System.Collections.Generic;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Layouts;
using ArcGIS.Desktop.GeoProcessing;
using System.IO;
using ArcGIS.Core.CIM;
using ArcGIS.Desktop.Core.UnitFormats;
using ArcGIS.Desktop.Core.Events;
using ArcGIS.Core.Data;

namespace Arcade
{
  public class Prosnippets
  {
    #region ProSnippet Group: Basic Queries
    #endregion

    public static void BasicQuery1()
    {
      var featLayer = MapView.Active.Map.GetLayersAsFlattenedList()
                .OfType<FeatureLayer>().FirstOrDefault();
      if (featLayer == null)
        return;

      //cref: ArcGIS.Core.CIM.CIMExpressionInfo
      //cref: ArcGIS.Core.CIM.CIMExpressionInfo.Expression
      //cref: ArcGIS.Core.CIM.CIMExpressionInfo.ReturnType
      //cref: ArcGIS.Core.Arcade.ArcadeScriptEngine.CreateEvaluator(ArcGIS.Core.CIM.CIMExpressionInfo,ArcGIS.Core.Arcade.ArcadeProfile)
      //cref: ArcGIS.Core.Arcade.ArcadeEvaluator.Evaluate(IEnumerable<KeyValuePair<string, object>>)
      //cref: ArcGIS.Core.Arcade.ArcadeEvaluationResult.GetResult()
      #region Basic Query

      //Consult https://github.com/Esri/arcade-expressions/ and
      //https://developers.arcgis.com/arcade/ for more examples
      //and arcade reference

      QueuedTask.Run(() =>
      {
        //construct an expression
        var query = @"Count($layer)";//count of features in "layer"

        //construct a CIMExpressionInfo
        var arcade_expr = new CIMExpressionInfo()
        {
          Expression = query.ToString(),
          //Return type can be string, numeric, or default
          //When set to default, addin is responsible for determining
          //the return type
          ReturnType = ExpressionReturnType.Default
        };

        //Construct an evaluator
        //select the relevant profile - it must support Pro and it must
        //contain any profile variables you are using in your expression.
        //Consult: https://developers.arcgis.com/arcade/profiles/
        using (var arcade = ArcadeScriptEngine.Instance.CreateEvaluator(
                                arcade_expr, ArcadeProfile.Popups))
        {
          //Provision  values for any profile variables referenced...
          //in our case '$layer'
          var variables = new List<KeyValuePair<string, object>>() {
              new KeyValuePair<string, object>("$layer", featLayer)
            };
          //evaluate the expression
          try
          {
            var result = arcade.Evaluate(variables).GetResult();
            System.Diagnostics.Debug.WriteLine($"Result: {result.ToString()}");
          }
          //handle any exceptions
          catch (InvalidProfileVariableException ipe)
          {
            //something wrong with the profile variable specified
            //TODO...
          }
          catch (EvaluationException ee)
          {
            //something wrong with the query evaluation
            //TODO...
          }

        }
      });

      #endregion
    }

    public static void BasicQuery2()
    {
      var featLayer = MapView.Active.Map.GetLayersAsFlattenedList()
                .OfType<FeatureLayer>().FirstOrDefault();
      if (featLayer == null)
        return;

      //cref: ArcGIS.Core.CIM.CIMExpressionInfo
      //cref: ArcGIS.Core.CIM.CIMExpressionInfo.Expression
      //cref: ArcGIS.Core.CIM.CIMExpressionInfo.ReturnType
      //cref: ArcGIS.Core.Arcade.ArcadeScriptEngine.CreateEvaluator(ArcGIS.Core.CIM.CIMExpressionInfo,ArcGIS.Core.Arcade.ArcadeProfile)
      //cref: ArcGIS.Core.Arcade.ArcadeEvaluator.Evaluate(IEnumerable<KeyValuePair<string, object>>)
      //cref: ArcGIS.Core.Arcade.ArcadeEvaluationResult.GetResult()
      #region Basic Query using Features

      //Consult https://github.com/Esri/arcade-expressions/ and
      //https://developers.arcgis.com/arcade/ for more examples
      //and arcade reference

      QueuedTask.Run(() =>
      {
        //construct an expression
        var query = @"$feature.AreaInAcres * 43560.0";//convert acres to ft 2

        //construct a CIMExpressionInfo
        var arcade_expr = new CIMExpressionInfo()
        {
          Expression = query.ToString(),
          //Return type can be string, numeric, or default
          //When set to default, addin is responsible for determining
          //the return type
          ReturnType = ExpressionReturnType.Default
        };

        //Construct an evaluator
        //select the relevant profile - it must support Pro and it must
        //contain any profile variables you are using in your expression.
        //Consult: https://developers.arcgis.com/arcade/profiles/
        using (var arcade = ArcadeScriptEngine.Instance.CreateEvaluator(
                                arcade_expr, ArcadeProfile.Popups))
        {
          //we are evaluating the expression against all features
          using (var rc = featLayer.Search())
          {

            while (rc.MoveNext())
            {
              //Provision  values for any profile variables referenced...
              //in our case '$feature'
              var variables = new List<KeyValuePair<string, object>>() {
                new KeyValuePair<string, object>("$feature", rc.Current)
              };
              //evaluate the expression (per feature in this case)
              try
              {
                var result = arcade.Evaluate(variables).GetResult();
                var val = ((double)result).ToString("0.0#");
                System.Diagnostics.Debug.WriteLine(
                  $"{rc.Current.GetObjectID()} area: {val} ft2");
              }
              //handle any exceptions
              catch (InvalidProfileVariableException ipe)
              {
                //something wrong with the profile variable specified
                //TODO...
              }
              catch (EvaluationException ee)
              {
                //something wrong with the query evaluation
                //TODO...
              }
            }
          }
        }
      });

      #endregion
    }

    public static void BasicQuery3()
    {
      var featLayer = MapView.Active.Map.GetLayersAsFlattenedList()
                .OfType<FeatureLayer>().FirstOrDefault();
      if (featLayer == null)
        return;

      //cref: ArcGIS.Core.Arcade.ArcadeEvaluator.Evaluate(IEnumerable<KeyValuePair<string, object>>)
      //cref: ArcGIS.Core.Arcade.ArcadeEvaluationResult.GetResult()
      #region Retrieve features using FeatureSetByName

      //Consult https://github.com/Esri/arcade-expressions/ and
      //https://developers.arcgis.com/arcade/ for more examples
      //and arcade reference

      var map = MapView.Active.Map;
      QueuedTask.Run(() =>
      {
        //construct a query
        var query = new StringBuilder();
        var layer_name = "USA Current Wildfires - Current Incidents";
        //https://developers.arcgis.com/arcade/function-reference/featureset_functions/
        query.AppendLine(
          $"var features = FeatureSetByName($map,'{layer_name}', ['*'], false);");
        query.AppendLine("return Count(features);");

        //construct a CIMExpressionInfo
        var arcade_expr = new CIMExpressionInfo()
        {
          Expression = query.ToString(),
          //Return type can be string, numeric, or default
          //When set to default, addin is responsible for determining
          //the return type
          ReturnType = ExpressionReturnType.Default
        };

        //Construct an evaluator
        //select the relevant profile - it must support Pro and it must
        //contain any profile variables you are using in your expression.
        //Consult: https://developers.arcgis.com/arcade/profiles/
        using (var arcade = ArcadeScriptEngine.Instance.CreateEvaluator(
                                arcade_expr, ArcadeProfile.Popups))
        {
          //Provision  values for any profile variables referenced...
          //in our case '$map'
          var variables = new List<KeyValuePair<string, object>>() {
              new KeyValuePair<string, object>("$map", map)
            };
          //evaluate the expression
          try
          {
            var result = arcade.Evaluate(variables).GetResult();
            System.Diagnostics.Debug.WriteLine($"Result: {result.ToString()}");
          }
          //handle any exceptions
          catch (InvalidProfileVariableException ipe)
          {
            //something wrong with the profile variable specified
            //TODO...
          }
          catch (EvaluationException ee)
          {
            //something wrong with the query evaluation
            //TODO...
          }
        }
      });

      #endregion
    }

    public static void BasicQuery4()
    {
      var featLayer = MapView.Active.Map.GetLayersAsFlattenedList()
                .OfType<FeatureLayer>().FirstOrDefault();
      if (featLayer == null)
        return;

      //cref: ArcGIS.Core.Arcade.ArcadeEvaluator.Evaluate(IEnumerable<KeyValuePair<string, object>>)
      //cref: ArcGIS.Core.Arcade.ArcadeEvaluationResult.GetResult()
      #region Retrieve features using Filter

      //Consult https://github.com/Esri/arcade-expressions/ and
      //https://developers.arcgis.com/arcade/ for more examples
      //and arcade reference

      QueuedTask.Run(() =>
      {
        //construct a query
        var query = new StringBuilder();
        //https://developers.arcgis.com/arcade/function-reference/featureset_functions/
        query.AppendLine(
          "var features = Filter($layer, 'DailyAcres is not NULL');");
        query.AppendLine("return Count(features);");

        //construct a CIMExpressionInfo
        var arcade_expr = new CIMExpressionInfo()
        {
          Expression = query.ToString(),
          //Return type can be string, numeric, or default
          //When set to default, addin is responsible for determining
          //the return type
          ReturnType = ExpressionReturnType.Default
        };

        //Construct an evaluator
        //select the relevant profile - it must support Pro and it must
        //contain any profile variables you are using in your expression.
        //Consult: https://developers.arcgis.com/arcade/profiles/
        using (var arcade = ArcadeScriptEngine.Instance.CreateEvaluator(
                                arcade_expr, ArcadeProfile.Popups))
        {
          //Provision  values for any profile variables referenced...
          //in our case '$layer'
          var variables = new List<KeyValuePair<string, object>>() {
              new KeyValuePair<string, object>("$layer", featLayer)
            };
          //evaluate the expression
          try
          {
            var result = arcade.Evaluate(variables).GetResult();
            System.Diagnostics.Debug.WriteLine($"Result: {result.ToString()}");
          }
          //handle any exceptions
          catch (InvalidProfileVariableException ipe)
          {
            //something wrong with the profile variable specified
            //TODO...
          }
          catch (EvaluationException ee)
          {
            //something wrong with the query evaluation
            //TODO...
          }
        }
      });

      #endregion
    }

    public static void BasicQuery6()
    {
      var featLayer = MapView.Active.Map.GetLayersAsFlattenedList()
                .OfType<FeatureLayer>().FirstOrDefault();
      if (featLayer == null)
        return;

      //cref: ArcGIS.Core.Arcade.ArcadeEvaluator.Evaluate(IEnumerable<KeyValuePair<string, object>>)
      //cref: ArcGIS.Core.Arcade.ArcadeEvaluationResult.GetResult()
      #region Calculate basic statistics with Math functions

      //Consult https://github.com/Esri/arcade-expressions/ and
      //https://developers.arcgis.com/arcade/ for more examples
      //and arcade reference

      QueuedTask.Run(() =>
      {
        //construct a query
        var query = new StringBuilder();
        //https://developers.arcgis.com/arcade/function-reference/math_functions

        query.AppendLine("var features = Filter($layer, 'DailyAcres is not NULL');");

        query.AppendLine("var count_feat = Count(features);");
        query.AppendLine("var sum_feat = Sum(features, 'DailyAcres');");
        query.AppendLine("var max_feat = Max(features, 'DailyAcres');");
        query.AppendLine("var min_feat = Min(features, 'DailyAcres');");
        query.AppendLine("var avg_feat = Average(features, 'DailyAcres');");

        query.AppendLine("var answer = [count_feat, sum_feat, max_feat, min_feat, avg_feat]");
        query.AppendLine("return Concatenate(answer,'|');");

        //construct a CIMExpressionInfo
        var arcade_expr = new CIMExpressionInfo()
        {
          Expression = query.ToString(),
          //Return type can be string, numeric, or default
          //When set to default, addin is responsible for determining
          //the return type
          ReturnType = ExpressionReturnType.Default
        };

        //Construct an evaluator
        //select the relevant profile - it must support Pro and it must
        //contain any profile variables you are using in your expression.
        //Consult: https://developers.arcgis.com/arcade/profiles/
        using (var arcade = ArcadeScriptEngine.Instance.CreateEvaluator(
                                arcade_expr, ArcadeProfile.Popups))
        {
          //Provision  values for any profile variables referenced...
          //in our case '$layer'
          var variables = new List<KeyValuePair<string, object>>() {
              new KeyValuePair<string, object>("$layer", featLayer)
            };
          //evaluate the expression
          try
          {
            var result = arcade.Evaluate(variables).GetResult();
            System.Diagnostics.Debug.WriteLine($"Result: {result.ToString()}");
          }
          //handle any exceptions
          catch (InvalidProfileVariableException ipe)
          {
            //something wrong with the profile variable specified
            //TODO...
          }
          catch (EvaluationException ee)
          {
            //something wrong with the query evaluation
            //TODO...
          }
        }
      });

      #endregion
    }

    public static void BasicQuery7()
    {
      var crimes_layer = MapView.Active.Map.GetLayersAsFlattenedList()
                .OfType<FeatureLayer>().FirstOrDefault();
      if (crimes_layer == null)
        return;

      //cref: ArcGIS.Core.Arcade.ArcadeEvaluator.Evaluate(IEnumerable<KeyValuePair<string, object>>)
      //cref: ArcGIS.Core.Arcade.ArcadeEvaluationResult.GetResult()
      #region Using FeatureSet functions Filter and Intersects

      //Consult https://github.com/Esri/arcade-expressions/ and
      //https://developers.arcgis.com/arcade/ for more examples
      //and arcade reference

      var map = MapView.Active.Map;
      QueuedTask.Run(() =>
      {
        //construct a query
        var query = new StringBuilder();
        //https://developers.arcgis.com/arcade/function-reference/featureset_functions/

        //Assume we have two layers - Oregon Counties (poly) and Crimes (points). Crimes
        //is from the Pro SDK community sample data.
        //Select all crime points within the relevant county boundaries and sum the count
        query.AppendLine("var results = [];");

        query.AppendLine("var counties = FeatureSetByName($map, 'Oregon_Counties', ['*'], true);");

        //'Clackamas','Multnomah','Washington'
        query.AppendLine("var sel_counties = Filter(counties, 'DHS_Districts IN (2, 15, 16)');");

        query.AppendLine("for(var county in sel_counties) {");
        query.AppendLine("   var name = county.County_Name;");
        query.AppendLine("   var cnt_crime = Count(Intersects($layer, Geometry(county)));");
        query.AppendLine("   Insert(results, 0, cnt_crime);");
        query.AppendLine("   Insert(results, 0, name);");
        query.AppendLine("}");

        query.AppendLine("return Concatenate(results,'|');");

        //construct a CIMExpressionInfo
        var arcade_expr = new CIMExpressionInfo()
        {
          Expression = query.ToString(),
          //Return type can be string, numeric, or default
          //When set to default, addin is responsible for determining
          //the return type
          ReturnType = ExpressionReturnType.Default
        };

        //Construct an evaluator
        //select the relevant profile - it must support Pro and it must
        //contain any profile variables you are using in your expression.
        //Consult: https://developers.arcgis.com/arcade/profiles/
        using (var arcade = ArcadeScriptEngine.Instance.CreateEvaluator(
                                arcade_expr, ArcadeProfile.Popups))
        {
          //Provision  values for any profile variables referenced...
          //in our case '$layer' and '$map'
          var variables = new List<KeyValuePair<string, object>>() {
              new KeyValuePair<string, object>("$layer", crimes_layer),
              new KeyValuePair<string, object>("$map", map)
            };
          //evaluate the expression
          try
          {
            var result = arcade.Evaluate(variables).GetResult();

            var results = result.ToString().Split('|', StringSplitOptions.None);
            var entries = results.Length / 2;
            int i = 0;
            for (var e = 0; e < entries; e++)
            {
              var name = results[i++];
              var count = results[i++];
              System.Diagnostics.Debug.WriteLine($"'{name}' crime count: {count}");
            }
          }
          //handle any exceptions
          catch (InvalidProfileVariableException ipe)
          {
            //something wrong with the profile variable specified
            //TODO...
          }
          catch (EvaluationException ee)
          {
            //something wrong with the query evaluation
            //TODO...
          }
        }
      });

      #endregion
    }

    #region ProSnippet Group: Evaluating Expressions
    #endregion

    public static void LabelQuery1()
    {
      var oregon_cnts = MapView.Active.Map.GetLayersAsFlattenedList()
                .OfType<FeatureLayer>().FirstOrDefault();
      if (oregon_cnts == null)
        return;

      //cref: ArcGIS.Core.CIM.CIMLabelClass
      //cref: ArcGIS.Core.Arcade.ArcadeEvaluator.Evaluate(IEnumerable<KeyValuePair<string, object>>)
      //cref: ArcGIS.Core.Arcade.ArcadeEvaluationResult.GetResult()
      #region Evaluating an Arcade Labelling Expression

      //Consult https://github.com/Esri/arcade-expressions/ and
      //https://developers.arcgis.com/arcade/ for more examples
      //and arcade reference

      var map = MapView.Active.Map;
      QueuedTask.Run(() =>
      {
        //Assume we a layer - Oregon County (poly) that has an arcade labelling
        //expression and we want to evaluate that interactively...
        var def = oregon_cnts.GetDefinition() as CIMFeatureLayer;
        //Get the label class
        var label_class = def.LabelClasses
                           .FirstOrDefault(lc => {
                             return lc.Name == "Arcade_Example_1" &&
                                    lc.ExpressionEngine == LabelExpressionEngine.Arcade;
                           });
        if (label_class == null)
          return;

        //evaluate the label expression against the features
        var expr_info = new CIMExpressionInfo()
        {
          Expression = label_class.Expression,
          ReturnType = ExpressionReturnType.String
        };

        //https://developers.arcgis.com/arcade/profiles/labeling/
        using (var arcade = ArcadeScriptEngine.Instance.CreateEvaluator(
                                                  expr_info, ArcadeProfile.Labeling))
        {
          //loop through the features
          using (var rc = oregon_cnts.Search())
          {
            while (rc.MoveNext())
            {
              var variables = new List<KeyValuePair<string, object>>() {
                new KeyValuePair<string, object>("$feature", rc.Current)
              };
              var result = arcade.Evaluate(variables).GetResult();
              //output
              System.Diagnostics.Debug.WriteLine(
                 $"[{rc.Current.GetObjectID()}]: {result}");
            }
          }
        }
      });

      #endregion
    }

    public static void VVQuery1()
    {
      var oregon_cnts = MapView.Active.Map.GetLayersAsFlattenedList()
                .OfType<FeatureLayer>().FirstOrDefault();
      if (oregon_cnts == null)
        return;

      //cref: ArcGIS.Core.CIM.CIMVisualVariable
      //cref: ArcGIS.Core.CIM.CIMColorVisualVariable
      //cref: ArcGIS.Core.CIM.CIMTransparencyVisualVariable
      //cref: ArcGIS.Core.CIM.CIMSizeVisualVariable
      //cref: ArcGIS.Core.CIM.CIMUniqueValueRenderer.ValueExpressionInfo
      //cref: ArcGIS.Core.Arcade.ArcadeEvaluator.Evaluate(IEnumerable<KeyValuePair<string, object>>)
      //cref: ArcGIS.Core.Arcade.ArcadeEvaluationResult.GetResult()
      #region Evaluating Arcade Visual Variable Expressions on a Renderer

      //Consult https://github.com/Esri/arcade-expressions/ and
      //https://developers.arcgis.com/arcade/ for more examples
      //and arcade reference

      var mv = MapView.Active;
      var map = mv.Map;

      QueuedTask.Run(() =>
      {
        //Assume we a layer - Oregon County (poly) that is using Visual Variable
        //expressions that we want to evaluate interactively...
        var def = oregon_cnts.GetDefinition() as CIMFeatureLayer;

        //Most all feature renderers have a VisualVariable collection
        var renderer = def.Renderer as CIMUniqueValueRenderer;
        var vis_variables = renderer.VisualVariables?.ToList() ??
                              new List<CIMVisualVariable>();
        if (vis_variables.Count == 0)
          return;//there are none
        var vis_var_with_expr = new Dictionary<string, string>();
        //see if any are using expressions
        foreach (var vv in vis_variables)
        {
          if (vv is CIMColorVisualVariable cvv)
          {
            if (!string.IsNullOrEmpty(cvv.ValueExpressionInfo?.Expression))
              vis_var_with_expr.Add("Color", cvv.ValueExpressionInfo?.Expression);
          }
          else if (vv is CIMTransparencyVisualVariable tvv)
          {
            if (!string.IsNullOrEmpty(tvv.ValueExpressionInfo?.Expression))
              vis_var_with_expr.Add("Transparency", tvv.ValueExpressionInfo?.Expression);
          }
          else if (vv is CIMSizeVisualVariable svv)
          {
            if (!string.IsNullOrEmpty(svv.ValueExpressionInfo?.Expression))
              vis_var_with_expr.Add("Outline", svv.ValueExpressionInfo?.Expression);
          }
        }
        if (vis_var_with_expr.Count == 0)
          return;//there arent any with expressions

        //loop through the features (outer)
        //per feature evaluate each visual variable.... (inner)
        //....
        //the converse is to loop through the expressions (outer)
        //then per feature evaluate the expression (inner)
        using (var rc = oregon_cnts.Search())
        {
          while (rc.MoveNext())
          {
            foreach (var kvp in vis_var_with_expr)
            {
              var expr_info = new CIMExpressionInfo()
              {
                Expression = kvp.Value,
                ReturnType = ExpressionReturnType.Default
              };
              //per feature eval each expression...
              using (var arcade = ArcadeScriptEngine.Instance.CreateEvaluator(
                                          expr_info, ArcadeProfile.Visualization))
              {

                var variables = new List<KeyValuePair<string, object>>() {
                  new KeyValuePair<string, object>("$feature", rc.Current)
                };
                //note 2D maps can also have view scale...
                //...if necessary...
                if (mv.ViewingMode == MapViewingMode.Map)
                {
                  variables.Add(new KeyValuePair<string, object>(
                    "$view.scale", mv.Camera.Scale));
                }
                var result = arcade.Evaluate(variables).GetResult().ToString();
                //output
                System.Diagnostics.Debug.WriteLine(
                   $"[{rc.Current.GetObjectID()}] '{kvp.Key}': {result}");
              }
            }
          }
        }

        ////foreach (var kvp in vis_var_with_expr)
        ////{
        ////  var expr_info = new CIMExpressionInfo()
        ////  {
        ////    Expression = kvp.Value,
        ////    ReturnType = ExpressionReturnType.Default
        ////  };

        ////  using (var arcade = ArcadeScriptEngine.Instance.CreateEvaluator(
        ////                                  expr_info, ArcadeProfile.Visualization))
        ////  {
        ////    //loop through the features
        ////    using (var rc = oregon_cnts.Search())
        ////    {
        ////      while (rc.MoveNext())
        ////      {
        ////        var variables = new List<KeyValuePair<string, object>>() {
        ////          new KeyValuePair<string, object>("$feature", rc.Current)
        ////        };

        ////        var result = arcade.Evaluate(variables).GetResult();
        ////        //output
        ////        //...
        ////      }
        ////    }
        ////  }
        ////}
      });

      #endregion
    }

    protected static void ArcadeRenderer()
    {
      // cref: ArcGIS.Core.CIM.CIMExpressionInfo
      // cref: ArcGIS.Core.CIM.CIMExpressionInfo.Expression
      // cref: ArcGIS.Core.CIM.CIMExpressionInfo.Title
      // cref: ArcGIS.Core.CIM.CIMUniqueValueRenderer.ValueExpressionInfo
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.GetRenderer
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetRenderer
      #region Modify renderer using Arcade
      var lyr = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault(f => f.ShapeType == esriGeometryType.esriGeometryPolygon);
      if (lyr == null) return;
      QueuedTask.Run(() =>
      {
        // GetRenderer from Layer (assumes it is a unique value renderer)
        var uvRenderer = lyr.GetRenderer() as CIMUniqueValueRenderer;
        if (uvRenderer == null) return;
        //layer has STATE_NAME field
        //community sample Data\Admin\AdminSample.aprx
        string expression = "if ($view.scale > 21000000) { return $feature.STATE_NAME } else { return 'All' }";
        CIMExpressionInfo updatedExpressionInfo = new CIMExpressionInfo
        {
          Expression = expression,
          Title = "Custom" // can be any string used for UI purpose.
        };
        //set the renderer's expression
        uvRenderer.ValueExpressionInfo = updatedExpressionInfo;

        //SetRenderer on Layer
        lyr.SetRenderer(uvRenderer);
      });
      #endregion
    }
    protected static void ArcadeLabeling()
    {
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.GetRenderer
      // cref: ArcGIS.Core.CIM.CIMGeoFeatureLayerBase.LabelClasses
      // cref: ArcGIS.Core.CIM.CIMLabelClass
      // cref: ArcGIS.Core.CIM.CIMLabelClass.Expression
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetRenderer
      #region Modify label expression using Arcade
      var lyr = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault(f => f.ShapeType == esriGeometryType.esriGeometryPolygon);
      if (lyr == null) return;
      QueuedTask.Run(() =>
      {
        //Get the layer's definition
        //community sample Data\Admin\AdminSample.aprx
        var lyrDefn = lyr.GetDefinition() as CIMFeatureLayer;
        if (lyrDefn == null) return;
        //Get the label classes - we need the first one
        var listLabelClasses = lyrDefn.LabelClasses.ToList();
        var theLabelClass = listLabelClasses.FirstOrDefault();
        //set the label class Expression to use the Arcade expression
        theLabelClass.Expression = "return $feature.STATE_NAME + TextFormatting.NewLine + $feature.POP2000;";
        //Set the label definition back to the layer.
        lyr.SetDefinition(lyrDefn);
      });

      #endregion
    }

    public static void AttributeRules1()
    {
      var featLayer = MapView.Active.Map.GetLayersAsFlattenedList()
                .OfType<FeatureLayer>().FirstOrDefault();
      if (featLayer == null)
        return;

      //cref: ArcGIS.Core.Data.TableDefinition.GetAttributeRules(AttributeRuleType)
      //cref: ArcGIS.Core.Data.AttributeRuleDefinition.GetScriptExpression()
      #region Evaluate AttributeRule Expression

      //Consult https://github.com/Esri/arcade-expressions/ and
      //https://developers.arcgis.com/arcade/profiles/attribute-rules/ for
      //more examples and arcade reference

      QueuedTask.Run(() =>
      {
        //Retrieve the desired feature class/table
        var def = featLayer.GetFeatureClass().GetDefinition();
        
        //get the desired attribute rule whose expression is to be
        //evaluated.
        //AttributeRuleType.All, Calculation, Constraint, Validation
        var validation_rule = def.GetAttributeRules(
                                    AttributeRuleType.Validation).FirstOrDefault();
        if (validation_rule == null)
          return;

        //Get the expression
        var expr = validation_rule.GetScriptExpression();
        //construct a CIMExpressionInfo
        var arcade_expr = new CIMExpressionInfo()
        {
          Expression = expr,
          //Return type can be string, numeric, or default
          ReturnType = ExpressionReturnType.Default
        };

        System.Diagnostics.Debug.WriteLine($"Evaluating {expr}:");
        //Construct an evaluator
        //we are using ArcadeProfile.AttributeRules profile...
        //Consult: https://developers.arcgis.com/arcade/profiles/
        using (var arcade = ArcadeScriptEngine.Instance.CreateEvaluator(
                                arcade_expr, ArcadeProfile.AttributeRuleValidation))
        {
          //we are evaluating the expression against all features
          using (var rc = featLayer.Search())
          {

            while (rc.MoveNext())
            {
              //Provision  values for any profile variables referenced...
              //in our case we assume '$feature'
              //...use arcade.ProfileVariablesUsed() if necessary...
              var variables = new List<KeyValuePair<string, object>>() {
                new KeyValuePair<string, object>("$feature", rc.Current)
              };

              //evaluate the expression per feature
              try
              {
                var result = arcade.Evaluate(variables).GetResult();
                //'Validation' attribute rules return true or false...
                var valid = System.Boolean.Parse(result.ToString());
                System.Diagnostics.Debug.WriteLine(
                  $"{rc.Current.GetObjectID()} valid: {valid}");
              }
              //handle any exceptions
              catch (InvalidProfileVariableException ipe)
              {
                //something wrong with the profile variable specified
                //TODO...
              }
              catch (EvaluationException ee)
              {
                //something wrong with the query evaluation
                //TODO...
              }
            }
          }
        }
      });

      #endregion
    }
  }
}