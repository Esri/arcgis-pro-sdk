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
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Core.CIM;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Framework.Dialogs;

namespace LabelingSnippets
{
    internal class Label : Button
    {
        protected override async void OnClick()
        {
            var lyr = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().Where(f => f.ShapeType == esriGeometryType.esriGeometryPoint).FirstOrDefault() ;
            var symbol = await CreateTextSymbolWithHaloAsync();
            await ModifyLabelsLeaderLineAnchorPolygon(lyr);
        }
       
        /// <summary>
        /// Get the active map's labeling engine - Maplex or Standard labeling engine.
        /// </summary>
        /// <returns></returns>
        private static Task<CIMGeneralPlacementProperties> GetMapLabelEngineAsync()
        {
            return QueuedTask.Run<CIMGeneralPlacementProperties>(() =>
            {
                #region Get the active map's labeling engine - Maplex or Standard labeling engine
                //Note: call within QueuedTask.Run()
                //Get the active map's definition - CIMMap.
                var cimMap = MapView.Active.Map.GetDefinition();
                //Get the labeling engine from the map definition
                CIMGeneralPlacementProperties labelEngine = cimMap.GeneralPlacementProperties;
                #endregion
                return labelEngine;
            });
        }
        
        /// <summary>
        /// Change the active map's labeling engine from Standard to Maplex or vice versa
        /// </summary>
        /// <remarks>Switches between the Maplex and Standard labeling engines. The current label engine's properties are stored so they can be restored when swicthed back.</remarks>
        /// <returns></returns>
        private static Task ChangeLabelEngineAsync()
        {
            return QueuedTask.Run(() =>
            {
                #region Change the active map's labeling engine from Standard to Maplex or vice versa
                //Note: call within QueuedTask.Run()
                //Get the active map's definition - CIMMap.
                var cimMap = MapView.Active.Map.GetDefinition();
                //Get the labeling engine from the map definition
                var cimGeneralPlacement = cimMap.GeneralPlacementProperties;

                if (cimGeneralPlacement is CIMMaplexGeneralPlacementProperties) 
                {
                    //Current labeling engine is Maplex labeling engine
                    //store the current label engine props if needed.
                    var maplexLabelEngineProps = cimGeneralPlacement;
                    //Create a new standard label engine properties
                    var cimStandardPlacementProperties = new CIMStandardGeneralPlacementProperties();
                    //Set the CIMMap's GeneralPlacementProperties to the new label engine
                    cimMap.GeneralPlacementProperties = cimStandardPlacementProperties;                    
                }
                else 
                {
                    //Current labeling engine is Standard labeling engine
                    //store the current label engine props if needed.
                    var standardLabelEngine = cimGeneralPlacement;
                    //Create a new Maplex label engine properties
                    var cimMaplexGeneralPlacementProperties = new CIMMaplexGeneralPlacementProperties();
                    //Set the CIMMap's GeneralPlacementProperties to the new label engine
                    cimMap.GeneralPlacementProperties = cimMaplexGeneralPlacementProperties;
                }
                //Set the map's definition
                MapView.Active.Map.SetDefinition(cimMap);
                #endregion
            });
        }

        

        /// <summary>
        /// Apply text symbol to a feature layer. 
        /// </summary>
        private static Task ApplyLabelAsync(FeatureLayer featureLayer, CIMTextSymbol textSymbol)
        {
            return QueuedTask.Run(() =>
            {
                #region Apply text symbol to a feature layer
                //Note: call within QueuedTask.Run()
                //Get the layer's definition
                var lyrDefn = featureLayer.GetDefinition() as CIMFeatureLayer;
                //Get the label classes - we need the first one
                var listLabelClasses = lyrDefn.LabelClasses.ToList();
                var theLabelClass = listLabelClasses.FirstOrDefault();
                //Set the label classes' symbol to the custom text symbol
                //Refer to the ProSnippets-TextSymbols wiki page for help with creating custom text symbols.
                //Example: var textSymbol = await CreateTextSymbolWithHaloAsync();
                theLabelClass.TextSymbol.Symbol = textSymbol;
                lyrDefn.LabelClasses = listLabelClasses.ToArray(); //Set the labelClasses back
                featureLayer.SetDefinition(lyrDefn); //set the layer's definition
                //set the label's visiblity
                featureLayer.SetLabelVisibility(true);
                #endregion
            });
        }
        /// <summary>
        /// Enable labeling of a layer
        /// </summary>
        /// <param name="featureLayer"></param>
        /// <returns></returns>
        private static Task EnableLayerLabelVisibility(FeatureLayer featureLayer)
        {
            return QueuedTask.Run(() =>
            {
                #region Enable labeling of a layer
                //Note: call within QueuedTask.Run()
                //set the label's visiblity
                featureLayer.SetLabelVisibility(true);
                #endregion
            });
        }

        /// <summary>
        /// Modify the Placement/Position of labels - Point geometry
        /// </summary>
        /// <param name="featureLayer"></param>
        /// <returns></returns>
        /// <remarks>
        /// The label placement and positions are modified for a point, line or polygon feature class.
        /// The properties modified are based on the Standard or Maplex label engine used on the Map defintion.
        /// </remarks>
        private static Task ModifyLabelsPlacementPointAsync(FeatureLayer featureLayer)
        {
            if (featureLayer.ShapeType != esriGeometryType.esriGeometryPoint)
                return Task.FromResult(0);
            return  QueuedTask.Run(() =>
            {
                #region Modify the Placement/Position of labels - Point geometry
                //Note: call within QueuedTask.Run()
                //Get the layer's definition
                var lyrDefn = featureLayer.GetDefinition() as CIMFeatureLayer;
                //Get the label classes - we need the first one
                var listLabelClasses = lyrDefn.LabelClasses.ToList();
                var theLabelClass = listLabelClasses.FirstOrDefault();                
                //Modify label Placement 
                //Check if the label engine is Maplex or standard.
                CIMGeneralPlacementProperties labelEngine = MapView.Active.Map.GetDefinition().GeneralPlacementProperties;
                if (labelEngine is CIMStandardGeneralPlacementProperties) //Current labeling engine is Standard labeling engine               
                    theLabelClass.StandardLabelPlacementProperties.PointPlacementMethod = StandardPointPlacementMethod.OnTopPoint;                                
                else    //Current labeling engine is Maplex labeling engine            
                    theLabelClass.MaplexLabelPlacementProperties.PointPlacementMethod = MaplexPointPlacementMethod.CenteredOnPoint;

                lyrDefn.LabelClasses = listLabelClasses.ToArray(); //Set the labelClasses back
                featureLayer.SetDefinition(lyrDefn); //set the layer's definition
                #endregion

            });
        }
        /// <summary>
        /// Modify the Placement/Position of labels - Line geometry
        /// </summary>
        /// <param name="featureLayer"></param>
        /// <returns></returns>

        private static Task ModifyLabelsPlacementLineAsync(FeatureLayer featureLayer)
        {
            if (featureLayer.ShapeType != esriGeometryType.esriGeometryPolyline)
                return Task.FromResult(0);
            return QueuedTask.Run(() =>
            {
                #region Modify the Placement/Position of labels - Line geometry
                //Note: call within QueuedTask.Run()
                //Get the layer's definition
                var lyrDefn = featureLayer.GetDefinition() as CIMFeatureLayer;
                //Get the label classes - we need the first one
                var listLabelClasses = lyrDefn.LabelClasses.ToList();
                var theLabelClass = listLabelClasses.FirstOrDefault();
                //Modify label Placement 
                //Check if the label engine is Maplex or standard.
                CIMGeneralPlacementProperties labelEngine = MapView.Active.Map.GetDefinition().GeneralPlacementProperties;
                if (labelEngine is CIMStandardGeneralPlacementProperties)
                {
                    //Current labeling engine is Standard labeling engine
                    var lineLablePosition = new CIMStandardLineLabelPosition
                    {
                        Perpendicular = true,
                        Parallel = false,
                        ProduceCurvedLabels = false,
                        Horizontal = false,
                        OnTop = true
                    };
                    theLabelClass.StandardLabelPlacementProperties.LineLabelPosition = lineLablePosition;               
                }
                else //Current labeling engine is Maplex labeling engine
                {
                    theLabelClass.MaplexLabelPlacementProperties.LinePlacementMethod = MaplexLinePlacementMethod.CenteredPerpendicularOnLine;
                    theLabelClass.MaplexLabelPlacementProperties.LineFeatureType = MaplexLineFeatureType.General;
                }
                //theLabelClass.MaplexLabelPlacementProperties.LinePlacementMethod = MaplexLinePlacementMethod.CenteredPerpendicularOnLine;
                lyrDefn.LabelClasses = listLabelClasses.ToArray(); //Set the labelClasses back
                featureLayer.SetDefinition(lyrDefn); //set the layer's definition
                #endregion
            });
        }
        /// <summary>
        /// Modify the Placement/Position of labels - Polygon geometry
        /// </summary>
        /// <param name="featureLayer"></param>
        /// <returns></returns>
        private static Task ModifyLabelsPlacementPolygonAsync(FeatureLayer featureLayer)
        {
            if (featureLayer.ShapeType != esriGeometryType.esriGeometryPolygon)
                return Task.FromResult(0);
            return QueuedTask.Run(() =>
            {
                #region Modify the Placement/Position of labels - Polygon geometry
                //Note: call within QueuedTask.Run()
                //Get the layer's definition
                var lyrDefn = featureLayer.GetDefinition() as CIMFeatureLayer;
                //Get the label classes - we need the first one
                var listLabelClasses = lyrDefn.LabelClasses.ToList();
                var theLabelClass = listLabelClasses.FirstOrDefault();
                //Modify label Placement 
                //Check if the label engine is Maplex or standard.
                CIMGeneralPlacementProperties labelEngine = MapView.Active.Map.GetDefinition().GeneralPlacementProperties;
                if (labelEngine is CIMStandardGeneralPlacementProperties)
                {
                    //Current labeling engine is Standard Labeling engine
                    theLabelClass.StandardLabelPlacementProperties.PolygonPlacementMethod = StandardPolygonPlacementMethod.AlwaysHorizontal;
                    theLabelClass.StandardLabelPlacementProperties.PlaceOnlyInsidePolygon = true;
                }
                else
                {
                    //Current labeling engine is Maplex labeling engine
                    theLabelClass.MaplexLabelPlacementProperties.PolygonFeatureType = MaplexPolygonFeatureType.LandParcel;
                    theLabelClass.MaplexLabelPlacementProperties.AvoidPolygonHoles = true;
                    theLabelClass.MaplexLabelPlacementProperties.PolygonPlacementMethod = MaplexPolygonPlacementMethod.HorizontalInPolygon;
                    theLabelClass.MaplexLabelPlacementProperties.CanPlaceLabelOutsidePolygon = true;
                }

                lyrDefn.LabelClasses = listLabelClasses.ToArray(); //Set the labelClasses back
                featureLayer.SetDefinition(lyrDefn); //set the layer's definition
                //set the label's visiblity
                featureLayer.SetLabelVisibility(true);
             #endregion

            });
        }

        /// <summary>
        /// Modify Orientation of a label using the MaplexEngine - Points and Polygon geometry
        /// </summary>
        /// <param name="featureLayer"></param>
        /// <returns></returns>
        /// <remarks>
        /// Orientation can be modified for Maplex Label engine only.
        /// </remarks>
        private static Task ModifyLabelsOrientationPointPolygonAsync(FeatureLayer featureLayer)
        {
            if (featureLayer.ShapeType == esriGeometryType.esriGeometryLine)
                return Task.FromResult(0);
            return QueuedTask.Run( () =>
            {                
                //Check if the label engine is Maplex or standard.
                CIMGeneralPlacementProperties labelEngine = MapView.Active.Map.GetDefinition().GeneralPlacementProperties;
                if (labelEngine is CIMStandardGeneralPlacementProperties)
                    return;
                #region Modify Orientation of a label using the MaplexEngine - Points and Polygon geometry
                //Note: call within QueuedTask.Run()
                //Get the layer's definition
                var lyrDefn = featureLayer.GetDefinition() as CIMFeatureLayer;
                //Get the label classes - we need the first one
                var listLabelClasses = lyrDefn.LabelClasses.ToList();
                var theLabelClass = listLabelClasses.FirstOrDefault();
                //Modify label Orientation                 
                theLabelClass.MaplexLabelPlacementProperties.GraticuleAlignment = true;
                theLabelClass.MaplexLabelPlacementProperties.GraticuleAlignmentType = MaplexGraticuleAlignmentType.Curved;                                        

                lyrDefn.LabelClasses = listLabelClasses.ToArray(); //Set the labelClasses back
                featureLayer.SetDefinition(lyrDefn); //set the layer's definition
                #endregion
            });
        }

        /// <summary>
        /// Modify Orientation of a label using the MaplexEngine - Line geometry
        /// </summary>
        /// <param name="featureLayer"></param>
        /// <returns></returns>
        private static Task ModifyLabelsOrientationLineAsync(FeatureLayer featureLayer)
        {
            if (featureLayer.ShapeType != esriGeometryType.esriGeometryPolyline)
                return Task.FromResult(0);
            return QueuedTask.Run(() =>
            {                
                //Check if the label engine is Maplex or standard.
                CIMGeneralPlacementProperties labelEngine = MapView.Active.Map.GetDefinition().GeneralPlacementProperties;
                if (labelEngine is CIMStandardGeneralPlacementProperties)
                    return;
                #region Modify Orientation of a label using the MaplexEngine - Line geometry
                //Note: call within QueuedTask.Run()
                //Get the layer's definition
                var lyrDefn = featureLayer.GetDefinition() as CIMFeatureLayer;
                //Get the label classes - we need the first one
                var listLabelClasses = lyrDefn.LabelClasses.ToList();
                var theLabelClass = listLabelClasses.FirstOrDefault();
                //Modify label Orientation
                theLabelClass.MaplexLabelPlacementProperties.AlignLabelToLineDirection = true;

                lyrDefn.LabelClasses = listLabelClasses.ToArray(); //Set the labelClasses back
                featureLayer.SetDefinition(lyrDefn); //set the layer's definition
                #endregion
            });
        }
        /// <summary>
        /// Modify label Rotation (Point geomerty)
        /// </summary>
        /// <param name="featureLayer"></param>
        /// <returns></returns>
        private static Task ModifyLabelsRotationPointsAsync(FeatureLayer featureLayer)
        {
            if (featureLayer.ShapeType != esriGeometryType.esriGeometryPoint)
                return Task.FromResult(0);
            return QueuedTask.Run(() =>
            {
                //Check if the label engine is Maplex or standard.
                CIMGeneralPlacementProperties labelEngine = MapView.Active.Map.GetDefinition().GeneralPlacementProperties;
                if (labelEngine is CIMStandardGeneralPlacementProperties)
                    return;
                #region Modify label Rotation - Point geometry
                //Note: call within QueuedTask.Run()
                //Get the layer's definition
                var lyrDefn = featureLayer.GetDefinition() as CIMFeatureLayer;
                //Get the label classes - we need the first one
                var listLabelClasses = lyrDefn.LabelClasses.ToList();
                var theLabelClass = listLabelClasses.FirstOrDefault();
                //Modify label Rotation
                CIMMaplexRotationProperties rotationProperties = new CIMMaplexRotationProperties
                {
                    Enable = true, //Enable rotation
                    RotationField = "ELEVATION", //Field that is used to define rotation angle
                    AdditionalAngle = 15, //Additional rotation 
                    RotationType = MaplexLabelRotationType.Arithmetic,
                    AlignmentType = MaplexRotationAlignmentType.Perpendicular,
                    AlignLabelToAngle = true
                };
                theLabelClass.MaplexLabelPlacementProperties.RotationProperties = rotationProperties;
                lyrDefn.LabelClasses = listLabelClasses.ToArray(); //Set the labelClasses back
                featureLayer.SetDefinition(lyrDefn); //set the layer's definition
                #endregion
            });
        }
        /// <summary>
        /// Spread labels across Polygon geometry
        /// </summary>
        /// <param name="featureLayer"></param>
        /// <returns></returns>
        private static Task ModifyLabelsSpreadLabelPolygon(FeatureLayer featureLayer)
        {
            if (featureLayer.ShapeType != esriGeometryType.esriGeometryPolygon)
                return Task.FromResult(0);
            return QueuedTask.Run(() =>
            {
                //Check if the label engine is Maplex or standard.
                CIMGeneralPlacementProperties labelEngine = MapView.Active.Map.GetDefinition().GeneralPlacementProperties;
                if (labelEngine is CIMStandardGeneralPlacementProperties)
                    return;
                #region Spread labels across Polygon geometry
                //Note: call within QueuedTask.Run()
                //Get the layer's definition
                var lyrDefn = featureLayer.GetDefinition() as CIMFeatureLayer;
                //Get the label classes - we need the first one
                var listLabelClasses = lyrDefn.LabelClasses.ToList();
                var theLabelClass = listLabelClasses.FirstOrDefault();
                //Spread Labels (words and characters to fill feature)
                // Spread words to fill feature
                theLabelClass.MaplexLabelPlacementProperties.SpreadWords = true;
                //Spread Characters to a fixed limit of 50%
                theLabelClass.MaplexLabelPlacementProperties.SpreadCharacters = true;
                theLabelClass.MaplexLabelPlacementProperties.MaximumCharacterSpacing = 50.0;
                lyrDefn.LabelClasses = listLabelClasses.ToArray(); //Set the labelClasses back
                featureLayer.SetDefinition(lyrDefn); //set the layer's definition
                #endregion

            });
        }
        /// <summary>
        /// Modify label's Leader Line Anchor point properties - Polygon geometry
        /// </summary>
        /// <param name="featureLayer"></param>
        /// <returns></returns>
        private static Task ModifyLabelsLeaderLineAnchorPolygon(FeatureLayer featureLayer)
        {
            if (featureLayer.ShapeType != esriGeometryType.esriGeometryPolygon)
                return Task.FromResult(0);
            return QueuedTask.Run(() =>
            {
                //Check if the label engine is Maplex or standard.
                CIMGeneralPlacementProperties labelEngine = MapView.Active.Map.GetDefinition().GeneralPlacementProperties;
                if (labelEngine is CIMStandardGeneralPlacementProperties)
                    return;
                #region Modify label's Leader Line Anchor point properties - Polygon geometry
                //Note: call within QueuedTask.Run()
                //Get the layer's definition
                var lyrDefn = featureLayer.GetDefinition() as CIMFeatureLayer;
                //Get the label classes - we need the first one
                var listLabelClasses = lyrDefn.LabelClasses.ToList();
                var theLabelClass = listLabelClasses.FirstOrDefault();
                //If TextSymbol is a callout the leader line anachor point can be modified
                theLabelClass.MaplexLabelPlacementProperties.PolygonAnchorPointType = MaplexAnchorPointType.Perimeter;
                lyrDefn.LabelClasses = listLabelClasses.ToArray(); //Set the labelClasses back
                featureLayer.SetDefinition(lyrDefn); //set the layer's definition
                #endregion
            });
        }

        private static Task<CIMTextSymbol> CreateTextSymbolWithHaloAsync()
        {
            return QueuedTask.Run<CIMTextSymbol>(() =>
            {
                //create a polygon symbol for the halo
                var haloPoly = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.RedRGB, SimpleFillStyle.Solid);
                //create text symbol using the halo polygon
                return SymbolFactory.Instance.ConstructTextSymbol(haloPoly, 10, "Arial", "Bold");
            });
        }
    }
}
