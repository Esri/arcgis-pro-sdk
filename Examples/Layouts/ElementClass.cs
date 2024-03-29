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

//Added references
using ArcGIS.Core.CIM;                             //CIM
using ArcGIS.Desktop.Core;                         //Project
using ArcGIS.Desktop.Layouts;                      //Layout class
using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Mapping;

namespace Layout_HelpExamples
{

  internal class ElementClass : Button
  {
    protected override void OnClick()
    {
      ElementClassExamples.MethodSnippets();
    }
  }

  public class ElementClassExamples
  {
    async public static void MethodSnippets()
    {
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals("Layout Name"));
      Layout layout = await QueuedTask.Run(() => layoutItem.GetLayout());
      Element element = layout.FindElement("Group Element");


      // cref: Element_ConvertToGraphics;ArcGIS.Desktop.Layouts.Element.ConvertToGraphics
      // cref: Element_ConvertToGraphics;ArcGIS.Desktop.Mapping.IElement.ConvertToGraphics
      // cref: ArcGIS.Desktop.Layouts.GroupElement.ELEMENTS
      // cref: ArcGIS.Desktop.Layouts.Element.GETY      
      // cref: ArcGIS.Desktop.Layouts.Element.SETY
      // cref: ArcGIS.Desktop.Layouts.Element.Name
      // cref: ArcGIS.Desktop.Layouts.Element.SetTOCPositionAbsolute
      #region Element_ConvertToGraphics
      //Convert a legend to a graphic and move the Title to the bottom of the legend and also move
      //the label in the contents pane to the bottom of the list.

      //Perform on the worker thread
      await QueuedTask.Run(() =>
      {
        Legend leg = layout.FindElement("Legend") as Legend;
        GroupElement result = leg.ConvertToGraphics().First() as GroupElement;
        Element firstElm = result.Elements.First();  //Note: Bottom element is first in drawing order.
        foreach (Element elm in result.Elements)
        {
          if (elm.Name == "Title")
          {
            elm.SetY(firstElm.GetY() - 0.25);  //Move title below other legend elements
            elm.SetTOCPositionAbsolute(result, false);  // Move Title item in TOC to bottom as well
          }
        }
      });
      #endregion Element_ConvertToGraphics

      // cref: ArcGIS.Core.CIM.Anchor
      // cref: ArcGIS.Desktop.Layouts.Element.GetAnchor
      // cref: ArcGIS.Desktop.Layouts.Element.SetAnchor(ArcGIS.Core.CIM.Anchor)
      // cref: ArcGIS.Desktop.Mapping.IElement.SetAnchor(ArcGIS.Core.CIM.Anchor)
      // cref: ArcGIS.Desktop.Mapping.IElement.GetAnchor
      #region Element_GetSetAnchor
      //Change the element's anchor position

      //Perform on the worker thread
      await QueuedTask.Run(() =>
      {
        Anchor elmAnchor = element.GetAnchor();
        elmAnchor = Anchor.CenterPoint;

        element.SetAnchor(elmAnchor); //You don't have to get to set; a shortcut would be: element.SetAnchor(Anchor.CenterPoint);
      });
      #endregion Element_GetSetAnchor

      // cref: Element_GetCustomProperty;ArcGIS.Desktop.Layouts.Element.GetCustomProperty(System.String)
      // cref: Element_GetCustomProperty;ArcGIS.Desktop.Mapping.IElement.GetCustomProperty(System.String)
      #region Element_GetCustomProperty
      //Get a custom property that has been previously set.

      //Perform on the worker thread
      await QueuedTask.Run(() =>
      {
        String custProp = element.GetCustomProperty("MyKeyString");
      });
      #endregion Element_GetCustomProperty

      // cref: Element_GetSetDefinition;ArcGIS.Core.CIM.CIMElement
      // cref: Element_GetSetDefinition;ArcGIS.Desktop.Layouts.Element.GetDefinition
      // cref: Element_GetSetDefinition;ArcGIS.Desktop.Layouts.Element.SetDefinition(ArcGIS.Core.CIM.CIMElement)
      // cref: Element_GetSetDefinition;ArcGIS.Desktop.Mapping.IElement.GetDefinition
      // cref: Element_GetSetDefinition;ArcGIS.Desktop.Mapping.IElement.SetDefinition(ArcGIS.Core.CIM.CIMElement)
      #region Element_GetSetDefinition
      //Modify an element's CIM properties.

      //Perform on the worker thread
      await QueuedTask.Run(() =>
      {
        CIMElement CIMElm = element.GetDefinition();

        //Modify a CIM value

        element.SetDefinition(CIMElm);
      });
      #endregion Element_GetSetDefinition

      // cref: Element_GetSetHeight;ArcGIS.Desktop.Layouts.Element.GetHeight
      // cref: Element_GetSetHeight;ArcGIS.Desktop.Layouts.Element.SetHeight(System.Double)
      // cref: Element_GetSetHeight;ArcGIS.Desktop.Mapping.IElement.SetHeight(System.Double)
      // cref: Element_GetSetHeight;ArcGIS.Desktop.Mapping.IElement.GetHeight
      #region Element_GetSetHeight
      //Modify an element's hieght.

      //Perform on the worker thread
      await QueuedTask.Run(() =>
      {
        double elmHeight = element.GetHeight();
        elmHeight = 11;

        element.SetHeight(elmHeight); //You don't have to get to set; a shortcut would be: element.SetHieght(11);
      });
      #endregion Element_GetSetHeight

      // cref: Element_SetLocked;ArcGIS.Desktop.Layouts.Element.SetLocked(System.Boolean)
      // cref: ArcGIS.Desktop.Layouts.Element.IsLocked
      // cref: Element_SetLocked;ArcGIS.Desktop.Mapping.IElement.SetLocked(System.Boolean)
      // cref: Element_SetLocked;ArcGIS.Desktop.Mapping.IElement.IsLocked
      #region Element_SetLocked
      //Modify an element's locked state if it isn't already

      //Perform on the worker thread
      await QueuedTask.Run(() =>
      {
        if (!element.IsLocked)
        {
          element.SetLocked(true);
        }
      });
      #endregion Element_GetSetLocked

      // cref: Element_GetSetLockedAspectRatio;ArcGIS.Desktop.Layouts.Element.GetLockedAspectRatio
      // cref: Element_GetSetLockedAspectRatio;ArcGIS.Desktop.Layouts.Element.SetLockedAspectRatio(System.Boolean)
      // cref: Element_GetSetLockedAspectRatio;ArcGIS.Desktop.Mapping.IElement.SetLockedAspectRatio(System.Boolean)
      // cref: Element_GetSetLockedAspectRatio;ArcGIS.Desktop.Mapping.IElement.GetLockedAspectRatio
      #region Element_GetSetLockedAspectRatio
      //Modify an element's aspect ratio. 

      //Perform on the worker thread
      await QueuedTask.Run(() =>
      {
        bool elmLocked = element.GetLockedAspectRatio();
        elmLocked = false;  //Turn off the locked state.

        element.SetLockedAspectRatio(elmLocked); //You don't have to get to set; a shortcut would be: element.SetLockedAspectRatio(false);
      });
      #endregion Element_GetSetLockedAspectRatio

      // cref: Element_GetSetRotation;ArcGIS.Desktop.Layouts.Element.GetRotation
      // cref: Element_GetSetRotation;ArcGIS.Desktop.Layouts.Element.SetRotation(System.Double)
      // cref: Element_GetSetRotation;ArcGIS.Desktop.Mapping.IElement.SetRotation(System.Double)
      // cref: Element_GetSetRotation;ArcGIS.Desktop.Mapping.IElement.GetRotation
      #region Element_GetSetRotation
      //Modify and element's rotation value.

      //Perform on the worker thread
      await QueuedTask.Run(() =>
      {
        double elmRot = element.GetRotation();
        elmRot = 22.5;

        element.SetRotation(elmRot); //You don't have to get to set; a shortcut would be: element.SetRotation(22.5);
      });
      #endregion Element_GetSetRotation

      // cref: Element_GetSetWidth;ArcGIS.Desktop.Layouts.Element.GetWidth
      // cref: Element_GetSetWidth;ArcGIS.Desktop.Layouts.Element.SetWidth(System.Double)
      // cref: Element_GetSetWidth;ArcGIS.Desktop.Mapping.IElement.SetWidth(System.Double)
      // cref: Element_GetSetWidth;ArcGIS.Desktop.Mapping.IElement.GetWidth
      #region Element_GetSetWidth
      //Modify an element's width.

      //Perform on the worker thread
      await QueuedTask.Run(() =>
      {
        double elmWidth = element.GetWidth();
        elmWidth = 8.5;

        element.SetWidth(elmWidth); //You don't have to get to set; a shortcut would be: element.SetWidth(8.5);
      });
      #endregion Element_GetSetWidth

      // cref: Element_GetSetX;ArcGIS.Desktop.Layouts.Element.GetX
      // cref: Element_GetSetX;ArcGIS.Desktop.Layouts.Element.SetX(System.Double)
      // cref: Element_GetSetX;ArcGIS.Desktop.Mapping.IElement.SetX(System.Double)
      // cref: Element_GetSetX;ArcGIS.Desktop.Mapping.IElement.GetX
      #region Element_GetSetX
      //Modify an element's X position.

      //Perform on the worker thread
      await QueuedTask.Run(() =>
      {
        double elmX = element.GetX();
        elmX = 4.25;

        element.SetX(elmX); //You don't have to get to set; a shortcut would be: element.SetX(4.25);

      });
      #endregion Element_GetSetX

      // cref: Element_GetSetY;ArcGIS.Desktop.Layouts.Element.GetY
      // cref: Element_GetSetY;ArcGIS.Desktop.Layouts.Element.SetY(System.Double)
      // cref: Element_GetSetY;ArcGIS.Desktop.Mapping.IElement.SetY(System.Double)
      // cref: Element_GetSetY;ArcGIS.Desktop.Mapping.IElement.GetY
      #region Element_GetSetY
      //Modify an element's Y position.

      //Perform on the worker thread
      await QueuedTask.Run(() =>
      {
        double elmY = element.GetY();
        elmY = 5.5;

        element.SetY(elmY); //You don't have to get to set; a shortcut would be: element.SetY(5.5);
      });
      #endregion Element_GetSetY

      // cref: Element_SetCustomProperty;ArcGIS.Desktop.Layouts.Element.SetCustomProperty(System.String,System.String)
      // cref: Element_SetCustomProperty;ArcGIS.Desktop.Mapping.IElement.SetCustomProperty(System.String,System.String)
      #region Element_SetCustomProperty
      //Set a custom property on an element.

      //Perform on the worker thread
      await QueuedTask.Run(() =>
      {
        element.SetCustomProperty("MyKeyString", "MyValueString");
      });
      #endregion Element_SetCustomProperty

      // cref: Element_SetName;ArcGIS.Desktop.Layouts.Element.SetName(System.String)
      // cref: Element_SetName;ArcGIS.Desktop.Mapping.IElement.SetName(System.String)
      #region Element_SetName
      //Modify an element's name.

      //Perform on the worker thread
      await QueuedTask.Run(() =>
      {
        element.SetName("New Name");
      });
      #endregion Element_SetName

      // cref: Element_SetTOCPositionAbsolute;ArcGIS.Desktop.Layouts.Element.SetTOCPositionAbsolute
      // cref: Element_SetTOCPositionAbsolute;ArcGIS.Desktop.Mapping.IElement.SetTOCPositionAbsolute
      #region Element_SetTOCPositionAbsolute
      //Move an element to the top of the TOC

      //Perform on the worker thread
      await QueuedTask.Run(() =>
      {
        element.SetTOCPositionAbsolute(layout, true);
      });
      #endregion Element_SetTOCPositionAbsolute

      // cref: Element_SetTOCPositionRelative;ArcGIS.Desktop.Layouts.Element.SetTOCPositionRelative
      // cref: Element_SetTOCPositionRelative;ArcGIS.Desktop.Mapping.IElement.SetTOCPositionRelative
      #region Element_SetTOCPositionRelative
      //Move a layout element above an existing layout element.

      //Perform on the worker thread
      await QueuedTask.Run(() =>
      {
        element.SetTOCPositionRelative(element, true);
      });
      #endregion Element_SetTOCPositionRelative

      // cref: Element_SetVisible;ArcGIS.Desktop.Layouts.Element.SetVisible
      // cref: Element_SetVisible;ArcGIS.Desktop.Mapping.IElement.SetVisible
      #region Element_SetVisible
      //Modify an element's visibility.

      //Perform on the worker thread
      await QueuedTask.Run(() =>
      {
        element.SetVisible(true);  //Turn it on / make visible.
      });
      #endregion Element_SetVisible
    }

    async public static void CreateElementSnippets()
    {
      //There are already many Create element snippets in the PRO snippets section.  This section will only contain examples that are NOT in the Pro snippets section
      //Pro snippets region names includes:
      //    Create point graphic with symbology
      //    Create line graphic with symbology
      //    Create rectangle graphic with simple symbology
      //    Create text element with basic font properties
      //    Create rectangle text with more advanced symbol settings
      //    Create a new picture element with advanced symbol settings
      //    Create a map frame and zoom to a bookmark
      //    Create a legend for a specific map frame
      //    Creating group elements

      LayoutView lytView = LayoutView.Active;
      Layout layout = lytView.Layout;

      // cref: Create_BezierCurve;ArcGIS.Desktop.Layouts.IElementFactory.CreateGraphicElement
      // cref: Create_BezierCurve;ArcGIS.Desktop.Layouts.ElementFactory.CreateGraphicElement
      // cref: Create_BezierCurve;ArcGIS.Desktop.Layouts.GraphicElement
      // cref: ArcGIS.Core.Geometry.CubicBezierBuilderEx
      // cref: ArcGIS.Core.Geometry.CubicBezierBuilderEx.ToSegment
      #region Create_BezierCurve
      //Create a bezier curve element with a simple line style.

      //Construct on the worker thread
      await QueuedTask.Run(() =>
      {
        //Build geometry
        Coordinate2D pt1 = new Coordinate2D(1, 7.5);
        Coordinate2D pt2 = new Coordinate2D(1.66, 8);
        Coordinate2D pt3 = new Coordinate2D(2.33, 7.1);
        Coordinate2D pt4 = new Coordinate2D(3, 7.5);
        //At 2.x - CubicBezierBuilder bez = new CubicBezierBuilder(pt1, pt2, pt3, pt4);
        var bez = new CubicBezierBuilderEx(pt1, pt2, pt3, pt4);
        CubicBezierSegment bezSeg = bez.ToSegment();
        //At 2.x - Polyline bezPl = PolylineBuilder.CreatePolyline(bezSeg);
        Polyline bezPl = PolylineBuilderEx.CreatePolyline(bezSeg);

        //Set symbology, create and add element to layout
        CIMLineSymbol lineSym = SymbolFactory.Instance.ConstructLineSymbol(ColorFactory.Instance.RedRGB, 4.0, SimpleLineStyle.DashDot);
        //At 2.x - GraphicElement bezElm = LayoutElementFactory.Instance.CreateLineGraphicElement(layout, bezPl, lineSym);
        //         bezElm.SetName("New Bezier Curve");
        GraphicElement bezElm = ElementFactory.Instance.CreateGraphicElement(layout, bezPl, lineSym, "New Bezier Curve");
      });
      #endregion Create_BezierCurve

      // cref: ArcGIS.Desktop.Layouts.IElementFactory.CreateGraphicElement
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateGraphicElement
      // cref: ArcGIS.Desktop.Layouts.GraphicElement
      // cref: ArcGIS.Core.Geometry.PolylineBuilderEx.CreatePolyline
      #region Create_freehand
      //Create a graphic freehand element with a simple line style.

      //Construct on the worker thread
      await QueuedTask.Run(() =>
      {
        //Build geometry
        List<Coordinate2D> plCoords = new List<Coordinate2D>();
        plCoords.Add(new Coordinate2D(1.5, 10.5));
        plCoords.Add(new Coordinate2D(1.25, 9.5));
        plCoords.Add(new Coordinate2D(1, 10.5));
        plCoords.Add(new Coordinate2D(0.75, 9.5));
        plCoords.Add(new Coordinate2D(0.5, 10.5));
        plCoords.Add(new Coordinate2D(0.5, 1));
        plCoords.Add(new Coordinate2D(0.75, 2));
        plCoords.Add(new Coordinate2D(1, 1));
        //At 2.x - Polyline linePl = PolylineBuilder.CreatePolyline(plCoords);
        Polyline linePl = PolylineBuilderEx.CreatePolyline(plCoords);

        //Set symbolology, create and add element to layout
        CIMLineSymbol lineSym = SymbolFactory.Instance.ConstructLineSymbol(ColorFactory.Instance.BlackRGB, 2.0, SimpleLineStyle.Solid);
        //At 2.x - GraphicElement lineElm = LayoutElementFactory.Instance.CreateLineGraphicElement(layout, linePl, lineSym);
        //         lineElm.SetName("New Freehand"); 
        GraphicElement lineElm = ElementFactory.Instance.CreateGraphicElement(layout, linePl, lineSym, "New Freehand");
      });
      #endregion Create_freehand

      // cref: ArcGIS.Desktop.Layouts.IElementFactory.CreateGraphicElement
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateGraphicElement
      // cref: ArcGIS.Desktop.Layouts.GraphicElement
      // cref: ArcGIS.Core.Geometry.PolygonBuilderEx.CreatePolygon
      #region Create_polygon_poly
      //Create a polygon graphic with simple line and fill styles.

      //Construct on the worker thread
      await QueuedTask.Run(() =>
      {
        //Build geometry
        List<Coordinate2D> plyCoords = new List<Coordinate2D>();
        plyCoords.Add(new Coordinate2D(1, 7));
        plyCoords.Add(new Coordinate2D(2, 7));
        plyCoords.Add(new Coordinate2D(2, 6.7));
        plyCoords.Add(new Coordinate2D(3, 6.7));
        plyCoords.Add(new Coordinate2D(3, 6.1));
        plyCoords.Add(new Coordinate2D(1, 6.1));
        //At 2.x - Polygon poly = PolygonBuilder.CreatePolygon(plyCoords);
        Polygon poly = PolygonBuilderEx.CreatePolygon(plyCoords);

        //Set symbolology, create and add element to layout
        CIMStroke outline = SymbolFactory.Instance.ConstructStroke(ColorFactory.Instance.BlueRGB, 2.0, SimpleLineStyle.DashDotDot);
        CIMPolygonSymbol polySym = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.RedRGB, SimpleFillStyle.ForwardDiagonal, outline);
        //At 2.x - GraphicElement polyElm = LayoutElementFactory.Instance.CreatePolygonGraphicElement(layout, poly, polySym);
        //         polyElm.SetName("New Polygon"); 
        GraphicElement polyElm = ElementFactory.Instance.CreateGraphicElement(layout, poly, polySym, "New Polygon");
      });
      #endregion Create_polygon_poly

      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructStroke
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructPolygonSymbol
      // cref: ArcGIS.Core.CIM.CIMStroke
      // cref: ArcGIS.Core.CIM.CIMPolygonSymbol
      // cref: ArcGIS.Desktop.Layouts.IElementFactory.CreateGraphicElement
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateGraphicElement
      // cref: ArcGIS.Desktop.Layouts.GraphicElement
      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.CreateEnvelope
      #region Create_polygon_env
      //Create a polygon graphic using an envelope with simple line and fill styles.

      //Construct on the worker thread
      await QueuedTask.Run(() =>
      {
        //Build 2D envelope
        Coordinate2D env_ll = new Coordinate2D(1.0, 4.75);
        Coordinate2D env_ur = new Coordinate2D(3.0, 5.75);
        //At 2.x - Envelope env = EnvelopeBuilder.CreateEnvelope(env_ll, env_ur);
        Envelope env = EnvelopeBuilderEx.CreateEnvelope(env_ll, env_ur);

        //Set symbolology, create and add element to layout
        CIMStroke outline = SymbolFactory.Instance.ConstructStroke(
          ColorFactory.Instance.BlueRGB, 2.0, SimpleLineStyle.DashDotDot);
        CIMPolygonSymbol polySym = SymbolFactory.Instance.ConstructPolygonSymbol(
          ColorFactory.Instance.RedRGB, SimpleFillStyle.ForwardDiagonal, outline);
        //At 2.x - GraphicElement polyElm =
        //            LayoutElementFactory.Instance.CreatePolygonGraphicElement(layout, env, polySym);
        //         polyElm.SetName("New Polygon");
        GraphicElement polyElm = ElementFactory.Instance.CreateGraphicElement(
          layout, env, polySym, "New Polygon");
      });
      #endregion Create_polygon_env

      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx
      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx.ToSegment
      // cref: ArcGIS.Core.Geometry.EllipticArcSegment
      // cref: ArcGIS.Core.Geometry.ArcOrientation
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructStroke
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructPolygonSymbol
      // cref: ArcGIS.Core.CIM.CIMStroke
      // cref: ArcGIS.Core.CIM.CIMPolygonSymbol
      // cref: ArcGIS.Desktop.Layouts.IElementFactory.CreateGraphicElement
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateGraphicElement
      // cref: ArcGIS.Desktop.Layouts.GraphicElement
      #region Create_circle
      //Create a circle graphic element using a simple line and fill styles.

      //Construct on the worker thread
      await QueuedTask.Run(() =>
      {
        //Build geometry
        Coordinate2D center = new Coordinate2D(2, 4);
        //At 2.x - EllipticArcBuilder eabCir = new EllipticArcBuilder(center, 0.5, esriArcOrientation.esriArcClockwise);
        var eabCir = new EllipticArcBuilderEx(center, 0.5, ArcOrientation.ArcClockwise);
        EllipticArcSegment cir = eabCir.ToSegment();

        //Set symbolology, create and add element to layout
        CIMStroke outline = SymbolFactory.Instance.ConstructStroke(ColorFactory.Instance.BlackRGB, 2.0, SimpleLineStyle.Dash);
        CIMPolygonSymbol circleSym = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.RedRGB, SimpleFillStyle.Solid, outline);
        //At 2.x - GraphicElement cirElm = LayoutElementFactory.Instance.CreateCircleGraphicElement(layout, cir, circleSym);
        //         cirElm.SetName("New Circle");

        var polyLine = PolylineBuilderEx.CreatePolyline(cir);
        GraphicElement cirElm = ElementFactory.Instance.CreateGraphicElement(layout, polyLine, circleSym, "New Circle");
      });
      #endregion

      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx
      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx.ToSegment
      // cref: ArcGIS.Core.Geometry.PolylineBuilderEx.CreatePolyline
      // cref: ArcGIS.Core.Geometry.EllipticArcSegment
      // cref: ArcGIS.Core.Geometry.ArcOrientation
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructStroke
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructPolygonSymbol
      // cref: ArcGIS.Core.CIM.CIMStroke
      // cref: ArcGIS.Core.CIM.CIMPolygonSymbol
      // cref: ArcGIS.Desktop.Layouts.IElementFactory.CreateGraphicElement
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateGraphicElement
      // cref: ArcGIS.Desktop.Layouts.GraphicElement
      #region Create_ellipse
      //Create an ellipse graphic with simple line and fill styles.

      //Construct on the worker thread
      await QueuedTask.Run(() =>
      {
        //Build geometry
        Coordinate2D center = new Coordinate2D(2, 2.75);
        //At 2.x - EllipticArcBuilder eabElp = new EllipticArcBuilder(center, 0, 1, 0.45, esriArcOrientation.esriArcClockwise);
        var eabElp = new EllipticArcBuilderEx(center, 0, 1, 0.45, ArcOrientation.ArcClockwise);
        EllipticArcSegment ellipse = eabElp.ToSegment();

        //Set symbolology, create and add element to layout
        CIMStroke outline = SymbolFactory.Instance.ConstructStroke(ColorFactory.Instance.GreenRGB, 2.0, SimpleLineStyle.Dot);
        CIMPolygonSymbol ellipseSym = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.GreyRGB, SimpleFillStyle.Vertical, outline);
        //At 2.x - GraphicElement elpElm = LayoutElementFactory.Instance.CreateEllipseGraphicElement(layout, ellipse, ellipseSym);
        //         elpElm.SetName("New Ellipse");

        var polyLine = PolylineBuilderEx.CreatePolyline(ellipse);
        GraphicElement elpElm = ElementFactory.Instance.CreateGraphicElement(layout, polyLine, ellipseSym, "New Ellipse");
      });
      #endregion


      // cref: ArcGIS.Core.Geometry.PolygonBuilderEx.CreatePolygon
      // cref: ArcGIS.Desktop.Layouts.IElementFactory.CreateGraphicElement
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateGraphicElement
      // cref: ArcGIS.Desktop.Layouts.GraphicElement
      #region Create_lasso
      //Create a graphic lasso element with simple line and fill styles.

      //Construct on the worker thread
      await QueuedTask.Run(() =>
      {
        //Build geometry
        List<Coordinate2D> plyCoords = new List<Coordinate2D>();
        plyCoords.Add(new Coordinate2D(1, 1));
        plyCoords.Add(new Coordinate2D(1.25, 2));
        plyCoords.Add(new Coordinate2D(1.5, 1.1));
        plyCoords.Add(new Coordinate2D(1.75, 2));
        plyCoords.Add(new Coordinate2D(2, 1.1));
        plyCoords.Add(new Coordinate2D(2.25, 2));
        plyCoords.Add(new Coordinate2D(2.5, 1.1));
        plyCoords.Add(new Coordinate2D(2.75, 2));
        plyCoords.Add(new Coordinate2D(3, 1));
        //At 2.x - Polygon poly = PolygonBuilder.CreatePolygon(plyCoords);
        Polygon poly = PolygonBuilderEx.CreatePolygon(plyCoords);

        //Set symbolology, create and add element to layout
        CIMStroke outline = SymbolFactory.Instance.ConstructStroke(ColorFactory.Instance.BlackRGB, 2.0, SimpleLineStyle.Solid);
        CIMPolygonSymbol polySym = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.RedRGB, SimpleFillStyle.ForwardDiagonal, outline);
        //At 2.x - GraphicElement polyElm = LayoutElementFactory.Instance.CreatePolygonGraphicElement(layout, poly, polySym);
        //         polyElm.SetName("New Lasso"); 
        GraphicElement polyElm = ElementFactory.Instance.CreateGraphicElement(layout, poly, polySym, "New Lasso");
      });
      #endregion Create_lasso

      // cref: ArcGIS.Core.Geometry.CubicBezierBuilderEx
      // cref: ArcGIS.Core.Geometry.CubicBezierBuilderEx.ToSegment
      // cref: ArcGIS.Core.Geometry.PolylineBuilderEx.CreatePolyline
      // cref: ArcGIS.Core.Geometry.CubicBezierSegment
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructTextSymbol
      // cref: ArcGIS.Core.CIM.CIMTextSymbol
      // cref: ArcGIS.Desktop.Layouts.IElementFactory.CreateTextGraphicElement
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateTextGraphicElement
      // cref: ArcGIS.Desktop.Layouts.GraphicElement
      // cref: ArcGIS.Desktop.Layouts.TextType
      #region Create_CurveText
      //Create curve text with basic text properties.

      //Construct on the worker thread
      await QueuedTask.Run(() =>
      {
        //Build geometry
        Coordinate2D pt1 = new Coordinate2D(3.6, 7.5);
        Coordinate2D pt2 = new Coordinate2D(4.26, 8);
        Coordinate2D pt3 = new Coordinate2D(4.93, 7.1);
        Coordinate2D pt4 = new Coordinate2D(5.6, 7.5);
        //At 2.x - CubicBezierBuilder bez = new CubicBezierBuilder(pt1, pt2, pt3, pt4);
        var bez = new CubicBezierBuilderEx(pt1, pt2, pt3, pt4);
        CubicBezierSegment bezSeg = bez.ToSegment();
        //At 2.x - Polyline bezPl = PolylineBuilder.CreatePolyline(bezSeg);
        Polyline bezPl = PolylineBuilderEx.CreatePolyline(bezSeg);

        //Set symbolology, create and add element to layout
        CIMTextSymbol sym = SymbolFactory.Instance.ConstructTextSymbol(ColorFactory.Instance.BlackRGB, 24, "Comic Sans MS", "Regular");
        //At 2.x - GraphicElement bezTxtElm = LayoutElementFactory.Instance.CreateCurvedTextGraphicElement(layout, bezPl, "Curved Text", sym);
        //         bezTxtElm.SetName("New Splinned Text");
        GraphicElement bezTxtElm = ElementFactory.Instance.CreateTextGraphicElement(
                                      layout, TextType.SplinedText, bezPl, sym, "Curved Text", "New Splinned Text");
      });
      #endregion Create_CurveText

      // cref: ArcGIS.Desktop.Layouts.GraphicElement.GetGraphic
      // cref: ARCGIS.DESKTOP.LAYOUTS.GRAPHICELEMENT.SETGRAPHIC
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructTextSymbol
      // cref: ArcGIS.Core.CIM.CIMTextSymbol
      // cref: ArcGIS.Core.CIM.CIMGraphic
      // cref: ArcGIS.Core.CIM.CIMParagraphTextGraphic
      // cref: ArcGIS.Core.CIM.CIMParagraphTextGraphic.Frame
      // cref: ArcGIS.Core.CIM.CIMGraphicFrame
      // cref: ArcGIS.Core.CIM.CIMGraphicFrame.BorderSymbol
      // cref: ArcGIS.Desktop.Layouts.IElementFactory.CreateTextGraphicElement
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateTextGraphicElement
      // cref: ArcGIS.Desktop.Layouts.GraphicElement
      // cref: ArcGIS.Desktop.Layouts.TextType
      #region Create_PolygonText
      //Create polygon paragraph text with basic text properties.

      //Construct on the worker thread
      await QueuedTask.Run(() =>
      {
        //Build geometry
        List<Coordinate2D> plyCoords = new List<Coordinate2D>();
        plyCoords.Add(new Coordinate2D(3.5, 7));
        plyCoords.Add(new Coordinate2D(4.5, 7));
        plyCoords.Add(new Coordinate2D(4.5, 6.7));
        plyCoords.Add(new Coordinate2D(5.5, 6.7));
        plyCoords.Add(new Coordinate2D(5.5, 6.1));
        plyCoords.Add(new Coordinate2D(3.5, 6.1));
        //At 2.x - Polygon poly = PolygonBuilder.CreatePolygon(plyCoords);
        Polygon poly = PolygonBuilderEx.CreatePolygon(plyCoords);

        //Set symbolology, create and add element to layout
        CIMTextSymbol sym = SymbolFactory.Instance.ConstructTextSymbol(ColorFactory.Instance.GreyRGB, 10, "Arial", "Regular");
        string text = "Some Text String that is really long and is <BOL>forced to wrap to other lines</BOL> so that we can see the effects." as String;
        //At 2.x - GraphicElement polyTxtElm = LayoutElementFactory.Instance.CreatePolygonParagraphGraphicElement(layout, poly, text, sym);
        //         polyTxtElm.SetName("New Polygon Text"); 
        GraphicElement polyTxtElm = ElementFactory.Instance.CreateTextGraphicElement(
                                      layout, TextType.RectangleParagraph, poly, sym, text, "New Polygon Text");

        //(Optionally) Modify paragraph border 
        CIMGraphic polyTxtGra = polyTxtElm.GetGraphic();
        CIMParagraphTextGraphic cimPolyTxtGra = polyTxtGra as CIMParagraphTextGraphic;
        cimPolyTxtGra.Frame.BorderSymbol = new CIMSymbolReference();
        cimPolyTxtGra.Frame.BorderSymbol.Symbol = SymbolFactory.Instance.ConstructLineSymbol(ColorFactory.Instance.GreyRGB, 1.0, SimpleLineStyle.Solid);
        polyTxtElm.SetGraphic(polyTxtGra);
      });

      #endregion Create_PolygonText

      // cref: ArcGIS.Desktop.Layouts.GraphicElement.GetGraphic
      // cref: ARCGIS.DESKTOP.LAYOUTS.GRAPHICELEMENT.SETGRAPHIC
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructTextSymbol
      // cref: ArcGIS.Core.CIM.CIMTextSymbol
      // cref: ArcGIS.Core.CIM.CIMGraphic
      // cref: ArcGIS.Core.CIM.CIMParagraphTextGraphic
      // cref: ArcGIS.Core.CIM.CIMParagraphTextGraphic.Frame
      // cref: ArcGIS.Core.CIM.CIMGraphicFrame
      // cref: ArcGIS.Core.CIM.CIMGraphicFrame.BorderSymbol
      // cref: ArcGIS.Desktop.Layouts.IElementFactory.CreateTextGraphicElement
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateTextGraphicElement
      // cref: ArcGIS.Desktop.Layouts.GraphicElement
      // cref: ArcGIS.Desktop.Layouts.TextType
      #region Create_CircleText
      //Create circle paragraph text with basic text settings and optionally a modified border.

      //Construct on the worker thread
      await QueuedTask.Run(() =>
      {
        //Build geometry
        Coordinate2D center = new Coordinate2D(4.5, 4);
        //At 2.x - EllipticArcBuilder eabCir = new EllipticArcBuilder(center, 0.5, esriArcOrientation.esriArcClockwise);
        var eabCir = new EllipticArcBuilderEx(center, 0.5, ArcOrientation.ArcClockwise);
        EllipticArcSegment cir = eabCir.ToSegment();
        var polyCir = PolygonBuilderEx.CreatePolygon(
                        PolylineBuilderEx.CreatePolyline(cir));

        //Set symbolology, create and add element to layout
        CIMTextSymbol sym = SymbolFactory.Instance.ConstructTextSymbol(ColorFactory.Instance.GreenRGB, 10, "Arial", "Regular");
        string text = "Circle, circle, circle, circle, circle, circle, circle, circle, circle, circle, circle";
        //At 2.x - GraphicElement cirTxtElm = LayoutElementFactory.Instance.CreateCircleParagraphGraphicElement(layout, cir, text, sym);
        //         cirTxtElm.SetName("New Circle Text");
        GraphicElement cirTxtElm = ElementFactory.Instance.CreateTextGraphicElement(
                                      layout, TextType.CircleParagraph, polyCir, sym, text, "New Circle Text");

        //(Optionally) Modify paragraph border 
        CIMGraphic cirTxtGra = cirTxtElm.GetGraphic();
        CIMParagraphTextGraphic cimCirTxtGra = cirTxtGra as CIMParagraphTextGraphic;
        cimCirTxtGra.Frame.BorderSymbol = new CIMSymbolReference();
        cimCirTxtGra.Frame.BorderSymbol.Symbol = SymbolFactory.Instance.ConstructLineSymbol(ColorFactory.Instance.GreyRGB, 1.0, SimpleLineStyle.Solid);
        cirTxtElm.SetGraphic(cirTxtGra);
      });
      #endregion

      // cref: ArcGIS.Desktop.Layouts.GraphicElement.GetGraphic
      // cref: ARCGIS.DESKTOP.LAYOUTS.GRAPHICELEMENT.SETGRAPHIC
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructTextSymbol
      // cref: ArcGIS.Core.CIM.CIMTextSymbol
      // cref: ArcGIS.Core.CIM.CIMGraphic
      // cref: ArcGIS.Core.CIM.CIMParagraphTextGraphic
      // cref: ArcGIS.Core.CIM.CIMParagraphTextGraphic.Frame
      // cref: ArcGIS.Core.CIM.CIMGraphicFrame
      // cref: ArcGIS.Core.CIM.CIMGraphicFrame.BorderSymbol
      // cref: ArcGIS.Desktop.Layouts.IElementFactory.CreateTextGraphicElement
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateTextGraphicElement
      // cref: ArcGIS.Desktop.Layouts.GraphicElement
      // cref: ArcGIS.Desktop.Layouts.TextType
      #region Create_EllipseText
      //Create ellipse paragraph text with basic text settings and optionally a modified border.

      //Construct on the worker thread
      await QueuedTask.Run(() =>
      {
        //Build geometry
        Coordinate2D center = new Coordinate2D(4.5, 2.75);
        //At 2.x - EllipticArcBuilder eabElp = new EllipticArcBuilder(center, 0, 1, 0.45, esriArcOrientation.esriArcClockwise);
        var eabElp = new EllipticArcBuilderEx(center, 0, 1, 0.45, ArcOrientation.ArcClockwise);
        EllipticArcSegment ellipse = eabElp.ToSegment();
        var polyElp = PolygonBuilderEx.CreatePolygon(
                        PolylineBuilderEx.CreatePolyline(ellipse));

        //Set symbolology, create and add element to layout
        CIMTextSymbol sym = SymbolFactory.Instance.ConstructTextSymbol(ColorFactory.Instance.BlueRGB, 10, "Arial", "Regular");
        string text = "Ellipse, ellipse, ellipse, ellipse, ellipse, ellipse, ellipse, ellipse, ellipse, ellipse, ellipse, ellipse";
        //At 2.x - GraphicElement elpTxtElm = LayoutElementFactory.Instance.CreateEllipseParagraphGraphicElement(layout, ellipse, text, sym);
        //         elpTxtElm.SetName("New Ellipse Text");
        GraphicElement elpTxtElm = ElementFactory.Instance.CreateTextGraphicElement(
                                      layout, TextType.EllipseParagraph, polyElp, sym, text, "New Ellipse Text");

        //(Optionally) Modify paragraph border 
        CIMGraphic elpTxtGra = elpTxtElm.GetGraphic();
        CIMParagraphTextGraphic cimElpTxtGra = elpTxtGra as CIMParagraphTextGraphic;
        cimElpTxtGra.Frame.BorderSymbol = new CIMSymbolReference();
        cimElpTxtGra.Frame.BorderSymbol.Symbol = SymbolFactory.Instance.ConstructLineSymbol(ColorFactory.Instance.GreyRGB, 1.0, SimpleLineStyle.Solid);
        elpTxtElm.SetGraphic(elpTxtGra);
      });
      #endregion Create_EllipseText

      MapFrame mfElm = null;

      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.CreateEnvelope
      // cref: ArcGIS.Desktop.Layouts.IElementFactory.CreateMapFrameElement
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateMapFrameElement
      // cref: ArcGIS.Desktop.Layouts.MapFrame.SetCamera(Layer,Boolean)
      #region Create_MapFrame
      //Creates a new map frame and changes the camera's scale.

      //Constuct on the worker thread
      await QueuedTask.Run(() =>
      {
        //Build geometry
        Coordinate2D ll = new Coordinate2D(6.0, 8.5);
        Coordinate2D ur = new Coordinate2D(8.0, 10.5);
        //At 2.x - Envelope env = EnvelopeBuilder.CreateEnvelope(ll, ur);
        Envelope env = EnvelopeBuilderEx.CreateEnvelope(ll, ur);

        //Reference map, create MF and add to layout
        MapProjectItem mapPrjItem = Project.Current.GetItems<MapProjectItem>().FirstOrDefault(item => item.Name.Equals("Map"));
        Map mfMap = mapPrjItem.GetMap();
        //At 2.x - mfElm = LayoutElementFactory.Instance.CreateMapFrame(layout, env, mfMap);
        //         mfElm.SetName("New Map Frame"); 
        mfElm = ElementFactory.Instance.CreateMapFrameElement(layout, env, mfMap, "New Map Frame");

        //Set the camera
        Camera camera = mfElm.Camera;
        camera.Scale = 24000;
        mfElm.SetCamera(camera);
      });
      #endregion Create_MapFrame

      // cref: ArcGIS.Desktop.Mapping.StyleProjectItem
      // cref: ArcGIS.Desktop.Mapping.ScaleBarStyleItem
      // cref: ArcGIS.Desktop.Layouts.Layout.FindElement
      // cref: ArcGIS.Desktop.Layouts.MapFrame
      // cref: ArcGIS.Desktop.Layouts.ScaleBarInfo
      // cref: ArcGIS.Desktop.Layouts.MapSurroundInfo.MapFrameName
      // cref: ArcGIS.Desktop.Layouts.ScaleBarInfo.ScaleBarStyleItem
      // cref: ArcGIS.Desktop.Layouts.ScaleBar
      // cref: ARCGIS.DESKTOP.MAPPING.STYLEHELPER.SEARCHSCALEBARS
      // cref: ArcGIS.Desktop.Layouts.IElementFactory.CreateMapSurroundElement
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateMapSurroundElement
      // cref: ArcGIS.Desktop.Layouts.Element.SETX
      // cref: ArcGIS.Desktop.Layouts.Element.SETY
      // cref: ArcGIS.Desktop.Layouts.Element.SETWIDTH
      #region Create_ScaleBar
      //Create a scale bar for a specific map frame and assign a scale bar style item.

      //Construct on the worker thread
      await QueuedTask.Run(() =>
      {
        //Reference a North Arrow in a style
        StyleProjectItem stylePrjItm = Project.Current.GetItems<StyleProjectItem>().FirstOrDefault(item => item.Name == "ArcGIS 2D");
        ScaleBarStyleItem sbStyleItm = stylePrjItm.SearchScaleBars("Double Alternating Scale Bar 1")[0];

        //Build geometry
        Coordinate2D center = new Coordinate2D(7, 8);

        //Reference MF, create north arrow and add to layout 
        MapFrame mf = layout.FindElement("New Map Frame") as MapFrame;
        if (mf == null)
        {
          ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Map frame not found", "WARNING");
          return;
        }

        //At 2.x -
        //ScaleBar sbElm = LayoutElementFactory.Instance.CreateScaleBar(layout, center, mf, sbStyleItm);
        //sbElm.SetName("New Scale Bar");
        //sbElm.SetWidth(2);
        //sbElm.SetX(6);
        //sbElm.SetY(7.5); 

        var sbInfo = new ScaleBarInfo()
        {
          MapFrameName = mf.Name,
          ScaleBarStyleItem = sbStyleItm
        };
        var sbElm = ElementFactory.Instance.CreateMapSurroundElement(
                                            layout, center.ToMapPoint(), sbInfo, "New Scale Bar") as ScaleBar;
        sbElm.SetWidth(2);
        sbElm.SetX(6);
        sbElm.SetY(7.5);
      });
      #endregion Create_ScaleBar

      // cref: ArcGIS.Desktop.Mapping.StyleProjectItem
      // cref: ArcGIS.Desktop.Mapping.NorthArrowStyleItem
      // cref: ArcGIS.Desktop.Layouts.Layout.FindElement
      // cref: ArcGIS.Desktop.Layouts.MapFrame
      // cref: ArcGIS.Desktop.Layouts.NorthArrowInfo
      // cref: ArcGIS.Desktop.Layouts.MapSurroundInfo.MapFrameName
      // cref: ArcGIS.Desktop.Layouts.NorthArrowInfo.NorthArrowStyleItem
      // cref: ArcGIS.Desktop.Layouts.NorthArrow
      // cref: ARCGIS.DESKTOP.MAPPING.STYLEHELPER.SEARCHNORTHARROWS
      // cref: ArcGIS.Desktop.Layouts.IElementFactory.CreateMapSurroundElement
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateMapSurroundElement
      // cref: ArcGIS.Desktop.Layouts.Element.SETX
      // cref: ArcGIS.Desktop.Layouts.Element.SETY
      // cref: ArcGIS.Desktop.Layouts.Element.SetHeight
      #region Create_NorthArrow
      //Create a north arrow for a specific map frame and assign a north arrow style item.

      //Construct on the worker thread
      await QueuedTask.Run(() =>
      {
        //Reference a North Arrow in a style
        StyleProjectItem stylePrjItm = Project.Current.GetItems<StyleProjectItem>().FirstOrDefault(item => item.Name == "ArcGIS 2D");
        NorthArrowStyleItem naStyleItm = stylePrjItm.SearchNorthArrows("ArcGIS North 10")[0];

        //Build geometry
        Coordinate2D center = new Coordinate2D(7, 5.5);

        //Reference MF, create north arrow and add to layout 
        MapFrame mf = layout.FindElement("New Map Frame") as MapFrame;
        if (mf == null)
        {
          ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Map frame not found", "WARNING");
          return;
        }
        //At 2.x -
        //NorthArrow arrowElm = LayoutElementFactory.Instance.CreateNorthArrow(layout, center, mf, naStyleItm);
        //arrowElm.SetName("New North Arrow");
        //arrowElm.SetHeight(1.75);
        //arrowElm.SetX(7);
        //arrowElm.SetY(6);

        var naInfo = new NorthArrowInfo()
        {
          MapFrameName = mf.Name,
          NorthArrowStyleItem = naStyleItm
        };

        var arrowElm = ElementFactory.Instance.CreateMapSurroundElement(
                                  layout, center.ToMapPoint(), naInfo, "New North Arrow") as NorthArrow;
        arrowElm.SetHeight(1.75);
        arrowElm.SetX(7);
        arrowElm.SetY(6);
      });
      #endregion Create_NorthArrow

      // cref: ArcGIS.Desktop.Layouts.IElementFactory.CreateGroupElement
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateGroupElement
      // cref: ArcGIS.Desktop.Layouts.GroupElement
      #region Create_Empty_Group_Root
      //Create an empty group element at the root level of the contents pane.


      //Construct on the worker thread
      await QueuedTask.Run(() =>
      {
        //At 2.x - GroupElement emptyGroupAtRoot = LayoutElementFactory.Instance.CreateGroupElement(layout);
        //         emptyGroupAtRoot.SetName("Empty group at root");
        GroupElement emptyGroupAtRoot = ElementFactory.Instance.CreateGroupElement(layout, null, "Empty group at root");
      });
      #endregion

      // cref: ArcGIS.Desktop.Layouts.IElementFactory.CreateGroupElement
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateGroupElement
      // cref: ArcGIS.Desktop.Layouts.GroupElement
      // cref: ArcGIS.Desktop.Layouts.Layout.FindElement
      #region Create_Empty_Group_Group
      //Create an empty group element at the root level of another group element.

      //Find an existing group element
      GroupElement existingGroupAtRoot = layout.FindElement("Empty group at root") as GroupElement;

      //Construct on the worker thread
      await QueuedTask.Run(() =>
      {
        //At 2.x - GroupElement emptyGroupInGroupAtRoot = LayoutElementFactory.Instance.CreateGroupElement(existingGroupAtRoot);
        //         emptyGroupInGroupAtRoot.SetName("Empty group in group at root");
        GroupElement emptyGroupInGroupAtRoot = ElementFactory.Instance.CreateGroupElement(
                                                existingGroupAtRoot, null, "Empty group in group at root");

      });
      #endregion

      // cref: ArcGIS.Desktop.Layouts.IElementFactory.CreateGroupElement
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateGroupElement
      // cref: ArcGIS.Desktop.Layouts.GroupElement
      // cref: ArcGIS.Desktop.Layouts.Element
      // cref: ArcGIS.Desktop.Layouts.Layout.FindElement
      #region Create_Group_With_Single_Element_Root
      //Create a group with a single element at the root level of the contents pane.

      //Find an existing element
      Element titleElm = layout.FindElement("Title") as Element;

      //Construct on the worker thread
      await QueuedTask.Run(() =>
      {
        //At 2.x - GroupElement groupWithSingleElementAtRoot = LayoutElementFactory.Instance.CreateGroupElement(layout, titleElm);
        //         groupWithSingleElementAtRoot.SetName("Group with single element at root"); 
        GroupElement groupWithSingleElementAtRoot = 
                 ElementFactory.Instance.CreateGroupElement(layout, new List<Element>() { titleElm }, "Group with single element at root");
      });
      #endregion

      // cref: ArcGIS.Desktop.Layouts.IElementFactory.CreateGroupElement
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateGroupElement
      // cref: ArcGIS.Desktop.Layouts.GroupElement
      // cref: ArcGIS.Desktop.Layouts.Element
      // cref: ArcGIS.Desktop.Layouts.Layout.FindElement
      #region Create_Group_With_List_Elements_Root
      //Create a group with a list of elements at the root level of the contents pane.

      //Find an existing elements
      Element scaleBar = layout.FindElement("Scale Bar") as Element;
      Element northArrow = layout.FindElement("North Arrow") as Element;
      Element legend = layout.FindElement("Legend") as Element;

      //Build a list and add the elements
      List<Element> elmList = new List<Element>
      {
        scaleBar,
        northArrow,
        legend
      };

      //Construct on the worker thread
      await QueuedTask.Run(() =>
      {
        //At 2.x - GroupElement groupWithListOfElementsAtRoot = LayoutElementFactory.Instance.CreateGroupElement(layout, elmList);
        //         groupWithListOfElementsAtRoot.SetName("Group with list of elements at root");
        GroupElement groupWithListOfElementsAtRoot = 
          ElementFactory.Instance.CreateGroupElement(layout, elmList, "Group with list of elements at root");
      });
      #endregion

      // cref: ArcGIS.Desktop.Layouts.IElementFactory.CreateGroupElement
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateGroupElement
      // cref: ArcGIS.Desktop.Layouts.GroupElement
      // cref: ArcGIS.Desktop.Layouts.Layout.FindElements
      #region Create_Group_With_List_Element_Names_Root
      //Create a group using a list of element names at the root level of the contents pane.

      //Build list of element names
      var elmNameList = new[] { "Table Frame", "Chart Frame" };

      //Construct on the worker thread
      await QueuedTask.Run(() =>
      {
        //GroupElement groupWithListOfElementNamesAtRoot = LayoutElementFactory.Instance.CreateGroupElement(layout, elmNameList);
        //groupWithListOfElementNamesAtRoot.SetName("Group with list of element names at root");

        //At 3.x use FindElements to retrieve the respective elements first
        var elems = layout.FindElements(elmNameList);
        GroupElement groupWithListOfElementNamesAtRoot =
          ElementFactory.Instance.CreateGroupElement(layout, elems, "Group with list of element names at root");
      });
      #endregion


    }
  }
}
