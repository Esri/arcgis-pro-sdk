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

using ArcGIS.Core.CIM;                             //CIM
using ArcGIS.Core.Data;
using ArcGIS.Desktop.Core;                         //Project
using ArcGIS.Desktop.Layouts;                      //Layout class
using ArcGIS.Desktop.Framework.Threading.Tasks;    //QueuedTask
using ArcGIS.Desktop.Mapping;                      //Export
using ArcGIS.Core.Geometry;

namespace Layout_HelpExamples
{
  internal class MethodSnippets : Button
  {
    protected override void OnClick()
    {
      //Snippets.LayoutClassSnippets();
      //Snippets.ElementSnippets();
      Snippets.GraphicElementSnippets();
      //Snippets.TextElementSnippets();
      //Snippets.MapFrameSnippets();
      Snippets.ExportSnippets();
    }
  }
  public class Snippets
  {
    //async public static void CreateElementSnippets()
    //{
    //  //There are already many Create element snippets in the PRO snippets section.  This section will only contain examples that are NOT in the Pro snippets section
    //  //Pro snippets region names includes:
    //  //    Create point graphic with symbology
    //  //    Create line graphic with symbology
    //  //    Create rectangle graphic with simple symbology
    //  //    Create text element with basic font properties
    //  //    Create rectangle text with more advanced symbol settings
    //  //    Create a new picture element with advanced symbol settings
    //  //    Create a map frame and zoom to a bookmark
    //  //    Create a legend for a specific map frame
    //  //    Creating group elements

    //  LayoutView lytView = LayoutView.Active;
    //  Layout layout = lytView.Layout;

    //  #region Create_BeizierCurve
    //  await QueuedTask.Run(() =>
    //  {
    //    //Build geometry
    //    Coordinate2D pt1 = new Coordinate2D(1, 7.5);
    //    Coordinate2D pt2 = new Coordinate2D(1.66, 8);
    //    Coordinate2D pt3 = new Coordinate2D(2.33, 7.1);
    //    Coordinate2D pt4 = new Coordinate2D(3, 7.5);
    //    CubicBezierBuilder bez = new CubicBezierBuilder(pt1, pt2, pt3, pt4);
    //    CubicBezierSegment bezSeg = bez.ToSegment();
    //    Polyline bezPl = PolylineBuilder.CreatePolyline(bezSeg);

    //    //Set symbology, create and add element to layout
    //    CIMLineSymbol lineSym = SymbolFactory.Instance.ConstructLineSymbol(ColorFactory.Instance.RedRGB, 4.0, SimpleLineStyle.DashDot);
    //    GraphicElement bezElm = LayoutElementFactory.Instance.CreateLineGraphicElement(layout, bezPl, lineSym);
    //    bezElm.SetName("New Bezier Curve");
    //  });
    //  #endregion Create_BeizierCurve

    //  #region Create_freehand
    //  await QueuedTask.Run(() =>
    //  {
    //    //Build geometry
    //    List<Coordinate2D> plCoords = new List<Coordinate2D>();
    //    plCoords.Add(new Coordinate2D(1.5, 10.5));
    //    plCoords.Add(new Coordinate2D(1.25, 9.5));
    //    plCoords.Add(new Coordinate2D(1, 10.5));
    //    plCoords.Add(new Coordinate2D(0.75, 9.5));
    //    plCoords.Add(new Coordinate2D(0.5, 10.5));
    //    plCoords.Add(new Coordinate2D(0.5, 1));
    //    plCoords.Add(new Coordinate2D(0.75, 2));
    //    plCoords.Add(new Coordinate2D(1, 1));
    //    Polyline linePl = PolylineBuilder.CreatePolyline(plCoords);

    //    //Set symbolology, create and add element to layout
    //    CIMLineSymbol lineSym = SymbolFactory.Instance.ConstructLineSymbol(ColorFactory.Instance.BlackRGB, 2.0, SimpleLineStyle.Solid);
    //    GraphicElement lineElm = LayoutElementFactory.Instance.CreateLineGraphicElement(layout, linePl, lineSym);
    //    lineElm.SetName("New Freehand");
    //  });
    //  #endregion Create_freehand

    //  #region Create_polygon_poly
    //  await QueuedTask.Run(() =>
    //  {
    //    //Build geometry
    //    List<Coordinate2D> plyCoords = new List<Coordinate2D>();
    //    plyCoords.Add(new Coordinate2D(1, 7));
    //    plyCoords.Add(new Coordinate2D(2, 7));
    //    plyCoords.Add(new Coordinate2D(2, 6.7));
    //    plyCoords.Add(new Coordinate2D(3, 6.7));
    //    plyCoords.Add(new Coordinate2D(3, 6.1));
    //    plyCoords.Add(new Coordinate2D(1, 6.1));
    //    Polygon poly = PolygonBuilder.CreatePolygon(plyCoords);

    //    //Set symbolology, create and add element to layout
    //    CIMStroke outline = SymbolFactory.Instance.ConstructStroke(ColorFactory.Instance.BlueRGB, 2.0, SimpleLineStyle.DashDotDot);
    //    CIMPolygonSymbol polySym = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.RedRGB, SimpleFillStyle.ForwardDiagonal, outline);
    //    GraphicElement polyElm = LayoutElementFactory.Instance.CreatePolygonGraphicElement(layout, poly, polySym);
    //    polyElm.SetName("New Polygon");
    //  });
    //  #endregion Create_polygon_poly

    //  #region Create_polygon_env
    //  await QueuedTask.Run(() =>
    //  {
    //    //Build 2D envelope
    //    Coordinate2D env_ll = new Coordinate2D(1.0, 4.75);
    //    Coordinate2D env_ur = new Coordinate2D(3.0, 5.75);
    //    Envelope env = EnvelopeBuilder.CreateEnvelope(env_ll, env_ur);

    //    //Set symbolology, create and add element to layout
    //    CIMStroke outline = SymbolFactory.Instance.ConstructStroke(ColorFactory.Instance.BlueRGB, 2.0, SimpleLineStyle.DashDotDot);
    //    CIMPolygonSymbol polySym = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.RedRGB, SimpleFillStyle.ForwardDiagonal, outline);
    //    GraphicElement polyElm = LayoutElementFactory.Instance.CreatePolygonGraphicElement(layout, env, polySym);
    //    polyElm.SetName("New Polygon");
    //  });
    //  #endregion Create_polygon_env

    //  #region Create_circle
    //  await QueuedTask.Run(() =>
    //  {
    //    //Build geometry
    //    Coordinate2D center = new Coordinate2D(2, 4);
    //    EllipticArcBuilder eabCir = new EllipticArcBuilder(center, 0.5, esriArcOrientation.esriArcClockwise);
    //    EllipticArcSegment cir = eabCir.ToSegment();

    //    //Set symbolology, create and add element to layout
    //    CIMStroke outline = SymbolFactory.Instance.ConstructStroke(ColorFactory.Instance.BlackRGB, 2.0, SimpleLineStyle.Dash);
    //    CIMPolygonSymbol circleSym = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.RedRGB, SimpleFillStyle.Solid, outline);
    //    GraphicElement cirElm = LayoutElementFactory.Instance.CreateCircleGraphicElement(layout, cir, circleSym);
    //    cirElm.SetName("New Circle");
    //  });
    //  #endregion Create_circle

    //  #region Create_ellipse
    //  await QueuedTask.Run(() =>
    //  {
    //    //Build geometry
    //    Coordinate2D center = new Coordinate2D(2, 2.75);
    //    EllipticArcBuilder eabElp = new EllipticArcBuilder(center, 0, 1, 0.45, esriArcOrientation.esriArcClockwise);
    //    EllipticArcSegment ellipse = eabElp.ToSegment();

    //    //Set symbolology, create and add element to layout
    //    CIMStroke outline = SymbolFactory.Instance.ConstructStroke(ColorFactory.Instance.GreenRGB, 2.0, SimpleLineStyle.Dot);
    //    CIMPolygonSymbol ellipseSym = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.GreyRGB, SimpleFillStyle.Vertical, outline);
    //    GraphicElement elpElm = LayoutElementFactory.Instance.CreateEllipseGraphicElement(layout, ellipse, ellipseSym);
    //    elpElm.SetName("New Ellipse");
    //  });
    //  #endregion Create_ellipse

    //  #region Create_lasso
    //  await QueuedTask.Run(() =>
    //  {
    //    //Build geometry
    //    List<Coordinate2D> plyCoords = new List<Coordinate2D>();
    //    plyCoords.Add(new Coordinate2D(1, 1));
    //    plyCoords.Add(new Coordinate2D(1.25, 2));
    //    plyCoords.Add(new Coordinate2D(1.5, 1.1));
    //    plyCoords.Add(new Coordinate2D(1.75, 2));
    //    plyCoords.Add(new Coordinate2D(2, 1.1));
    //    plyCoords.Add(new Coordinate2D(2.25, 2));
    //    plyCoords.Add(new Coordinate2D(2.5, 1.1));
    //    plyCoords.Add(new Coordinate2D(2.75, 2));
    //    plyCoords.Add(new Coordinate2D(3, 1));
    //    Polygon poly = PolygonBuilder.CreatePolygon(plyCoords);

    //    //Set symbolology, create and add element to layout
    //    CIMStroke outline = SymbolFactory.Instance.ConstructStroke(ColorFactory.Instance.BlackRGB, 2.0, SimpleLineStyle.Solid);
    //    CIMPolygonSymbol polySym = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.RedRGB, SimpleFillStyle.ForwardDiagonal, outline);
    //    GraphicElement polyElm = LayoutElementFactory.Instance.CreatePolygonGraphicElement(layout, poly, polySym);
    //    polyElm.SetName("New Lasso");
    //  });
    //  #endregion Create_lasso

    //  #region Create_CurveText
    //  await QueuedTask.Run(() =>
    //  {
    //    //Build geometry
    //    Coordinate2D pt1 = new Coordinate2D(3.6, 7.5);
    //    Coordinate2D pt2 = new Coordinate2D(4.26, 8);
    //    Coordinate2D pt3 = new Coordinate2D(4.93, 7.1);
    //    Coordinate2D pt4 = new Coordinate2D(5.6, 7.5);
    //    CubicBezierBuilder bez = new CubicBezierBuilder(pt1, pt2, pt3, pt4);
    //    CubicBezierSegment bezSeg = bez.ToSegment();
    //    Polyline bezPl = PolylineBuilder.CreatePolyline(bezSeg);

    //    //Set symbolology, create and add element to layout
    //    CIMTextSymbol sym = SymbolFactory.Instance.ConstructTextSymbol(ColorFactory.Instance.BlackRGB, 24, "Comic Sans MS", "Regular");
    //    GraphicElement bezTxtElm = LayoutElementFactory.Instance.CreateCurvedTextGraphicElement(layout, bezPl, "Curved Text", sym);
    //    bezTxtElm.SetName("New Splinned Text");
    //  });
    //  #endregion Create_CurveText

    //  #region Create_PolygonText
    //  await QueuedTask.Run(() =>
    //  {
    //    //Build geometry
    //    List<Coordinate2D> plyCoords = new List<Coordinate2D>();
    //    plyCoords.Add(new Coordinate2D(3.5, 7));
    //    plyCoords.Add(new Coordinate2D(4.5, 7));
    //    plyCoords.Add(new Coordinate2D(4.5, 6.7));
    //    plyCoords.Add(new Coordinate2D(5.5, 6.7));
    //    plyCoords.Add(new Coordinate2D(5.5, 6.1));
    //    plyCoords.Add(new Coordinate2D(3.5, 6.1));
    //    Polygon poly = PolygonBuilder.CreatePolygon(plyCoords);

    //    //Set symbolology, create and add element to layout
    //    CIMTextSymbol sym = SymbolFactory.Instance.ConstructTextSymbol(ColorFactory.Instance.GreyRGB, 10, "Arial", "Regular");
    //    string text = "Some Text String that is really long and is <BOL>forced to wrap to other lines</BOL> so that we can see the effects." as String;
    //    GraphicElement polyTxtElm = LayoutElementFactory.Instance.CreatePolygonParagraphGraphicElement(layout, poly, text, sym);
    //    polyTxtElm.SetName("New Polygon Text");

    //    //(Optionally) Modify paragraph border 
    //    CIMGraphic polyTxtGra = polyTxtElm.Graphic;
    //    CIMParagraphTextGraphic cimPolyTxtGra = polyTxtGra as CIMParagraphTextGraphic;
    //    cimPolyTxtGra.Frame.BorderSymbol = new CIMSymbolReference();
    //    cimPolyTxtGra.Frame.BorderSymbol.Symbol = SymbolFactory.Instance.ConstructLineSymbol(ColorFactory.Instance.GreyRGB, 1.0, SimpleLineStyle.Solid);
    //    polyTxtElm.SetGraphic(polyTxtGra);
    //  });

    //  #endregion Create_PolygonText

    //  #region Create_CircleText
    //  await QueuedTask.Run(() =>
    //  {
    //    //Build geometry
    //    Coordinate2D center = new Coordinate2D(4.5, 4);
    //    EllipticArcBuilder eabCir = new EllipticArcBuilder(center, 0.5, esriArcOrientation.esriArcClockwise);
    //    EllipticArcSegment cir = eabCir.ToSegment();

    //    //Set symbolology, create and add element to layout
    //    CIMTextSymbol sym = SymbolFactory.Instance.ConstructTextSymbol(ColorFactory.Instance.GreenRGB, 10, "Arial", "Regular");
    //    string text = "Circle, circle, circle, circle, circle, circle, circle, circle, circle, circle, circle";
    //    GraphicElement cirTxtElm = LayoutElementFactory.Instance.CreateCircleParagraphGraphicElement(layout, cir, text, sym);
    //    cirTxtElm.SetName("New Circle Text");

    //    //(Optionally) Modify paragraph border 
    //    CIMGraphic cirTxtGra = cirTxtElm.Graphic;
    //    CIMParagraphTextGraphic cimCirTxtGra = cirTxtGra as CIMParagraphTextGraphic;
    //    cimCirTxtGra.Frame.BorderSymbol = new CIMSymbolReference();
    //    cimCirTxtGra.Frame.BorderSymbol.Symbol = SymbolFactory.Instance.ConstructLineSymbol(ColorFactory.Instance.GreyRGB, 1.0, SimpleLineStyle.Solid);
    //    cirTxtElm.SetGraphic(cirTxtGra);
    //  });
    //  #endregion Create_CircleText

    //  #region Create_EllipseText
    //  await QueuedTask.Run(() =>
    //  {
    //    //Build geometry
    //    Coordinate2D center = new Coordinate2D(4.5, 2.75);
    //    EllipticArcBuilder eabElp = new EllipticArcBuilder(center, 0, 1, 0.45, esriArcOrientation.esriArcClockwise);
    //    EllipticArcSegment ellipse = eabElp.ToSegment();

    //    //Set symbolology, create and add element to layout
    //    CIMTextSymbol sym = SymbolFactory.Instance.ConstructTextSymbol(ColorFactory.Instance.BlueRGB, 10, "Arial", "Regular");
    //    string text = "Ellipse, ellipse, ellipse, ellipse, ellipse, ellipse, ellipse, ellipse, ellipse, ellipse, ellipse, ellipse";
    //    GraphicElement elpTxtElm = LayoutElementFactory.Instance.CreateEllipseParagraphGraphicElement(layout, ellipse, text, sym);
    //    elpTxtElm.SetName("New Ellipse Text");

    //    //(Optionally) Modify paragraph border 
    //    CIMGraphic elpTxtGra = elpTxtElm.Graphic;
    //    CIMParagraphTextGraphic cimElpTxtGra = elpTxtGra as CIMParagraphTextGraphic;
    //    cimElpTxtGra.Frame.BorderSymbol = new CIMSymbolReference();
    //    cimElpTxtGra.Frame.BorderSymbol.Symbol = SymbolFactory.Instance.ConstructLineSymbol(ColorFactory.Instance.GreyRGB, 1.0, SimpleLineStyle.Solid);
    //    elpTxtElm.SetGraphic(elpTxtGra);
    //  });
    //  #endregion Create_EllipseText

    //  MapFrame mfElm = null;
      
    //  #region Create_MapFrame
    //  //This example creates a new map frame and changes the camera scale.
    //  await QueuedTask.Run(() =>
    //  {
    //      //Build geometry
    //      Coordinate2D ll = new Coordinate2D(6.0, 8.5);
    //      Coordinate2D ur = new Coordinate2D(8.0, 10.5);
    //      Envelope env = EnvelopeBuilder.CreateEnvelope(ll, ur);

    //      //Reference map, create MF and add to layout
    //      MapProjectItem mapPrjItem = Project.Current.GetItems<MapProjectItem>().FirstOrDefault(item => item.Name.Equals("Map"));
    //      Map mfMap = mapPrjItem.GetMap();
    //      mfElm = LayoutElementFactory.Instance.CreateMapFrame(layout, env, mfMap);
    //      mfElm.SetName("New Map Frame");

    //      //Set the camera
    //      Camera camera = mfElm.Camera;
    //      camera.Scale = 24000;
    //      mfElm.SetCamera(camera);
    //  });
    //  #endregion Create_MapFrame

    //  #region Create_ScaleBar
    //  await QueuedTask.Run(() =>
    //  {
    //    //Reference a North Arrow in a style
    //    StyleProjectItem stylePrjItm = Project.Current.GetItems<StyleProjectItem>().FirstOrDefault(item => item.Name == "ArcGIS 2D");
    //    ScaleBarStyleItem sbStyleItm = stylePrjItm.SearchScaleBars("Double Alternating Scale Bar 1")[0];

    //    //Build geometry
    //    Coordinate2D center = new Coordinate2D(7, 8);

    //    //Reference MF, create north arrow and add to layout 
    //    MapFrame mf = layout.FindElement("New Map Frame") as MapFrame;
    //    if (mf == null)
    //    {
    //      ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Map frame not found", "WARNING");
    //      return;
    //    }
    //    ScaleBar sbElm = LayoutElementFactory.Instance.CreateScaleBar(layout, center, mf, sbStyleItm);
    //    sbElm.SetName("New Scale Bar");
    //    sbElm.SetWidth(2);
    //    sbElm.SetX(6);
    //    sbElm.SetY(7.5);
    //  });
    //  #endregion Create_ScaleBar

    //  #region Create_NorthArrow
    //  await QueuedTask.Run(() =>
    //  {
    //    //Reference a North Arrow in a style
    //    StyleProjectItem stylePrjItm = Project.Current.GetItems<StyleProjectItem>().FirstOrDefault(item => item.Name == "ArcGIS 2D");
    //    NorthArrowStyleItem naStyleItm = stylePrjItm.SearchNorthArrows("ArcGIS North 10")[0];

    //    //Build geometry
    //    Coordinate2D center = new Coordinate2D(7, 5.5);

    //    //Reference MF, create north arrow and add to layout 
    //    MapFrame mf = layout.FindElement("New Map Frame") as MapFrame;
    //    if (mf == null)
    //    {
    //      ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Map frame not found", "WARNING");
    //      return;
    //    }
    //    NorthArrow arrowElm = LayoutElementFactory.Instance.CreateNorthArrow(layout, center, mf, naStyleItm);
    //    arrowElm.SetName("New North Arrow");
    //    arrowElm.SetHeight(1.75);
    //    arrowElm.SetX(7);
    //    arrowElm.SetY(6);
    //  });
    //  #endregion Create_NorthArrow

    //  #region Create_Group_Root
    //  //Create an empty group element at the root level of the contents pane
    //  //Note: call within QueuedTask.Run()
    //  GroupElement grp1 = LayoutElementFactory.Instance.CreateGroupElement(layout);
    //  grp1.SetName("Group");
    //  #endregion

    //  #region Create_Group_Group
    //  //Create a group element inside another group element
    //  //Note: call within QueuedTask.Run()
    //  GroupElement grp2 = LayoutElementFactory.Instance.CreateGroupElement(grp1);
    //  grp2.SetName("Group in Group");
    //  #endregion


    //}

    async public static void GraphicElementSnippets()
    {
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals("Layout Name"));
      Layout lyt = await QueuedTask.Run(() => layoutItem.GetLayout());
      GraphicElement graElm = lyt.FindElement("Rectangle") as GraphicElement;

      await QueuedTask.Run(() =>
      {
        // cref: ArcGIS.Desktop.Layouts.GraphicElement.Clone(System.String)
        #region GraphicElement_Clone
        //Note: call within QueuedTask.Run() 
        GraphicElement cloneElm = graElm.Clone("Clone");
        #endregion GraphicElement_Clone

        // cref: ArcGIS.Desktop.Layouts.GraphicElement.SetGraphic(ArcGIS.Core.CIM.CIMGraphic)
        // cref: ArcGIS.Desktop.Layouts.GraphicElement.GetGraphic(ArcGIS.Core.CIM.CIMGraphic)
        // cref: ArcGIS.Core.CIM.CIMGraphic
        #region GraphicElement_Get_And_SetGraphic

        //Note: must call within QueuedTask.Run() 
        var CIMGra = graElm.GetGraphic() as CIMGraphic;
        //TODO - make changes to CIMGraphic
        //...
        graElm.SetGraphic(CIMGra);
        #endregion GraphicElement_SetGraphic
      });
    }

    async public static void TextElementSnippets()
    {
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals("Layout Name"));
      Layout layout = await QueuedTask.Run(() => layoutItem.GetLayout());
     

      // cref: ArcGIS.Desktop.Layouts.TextProperties
      // cref: ArcGIS.Desktop.Layouts.TextProperties._CTOR
      #region TextElement_TextPropertiesConstructor
      TextProperties txtProp = new TextProperties("String", "Times New Roman", 24, "Regular");
      #endregion TextElement_TextPropertiesConstructor

      // cref: ArcGIS.Desktop.Layouts.TextElement
      // cref: ArcGIS.Desktop.Layouts.TextElement.SetTextProperties
      #region TextElement_SetTextProperties
      await QueuedTask.Run(() =>
      {
        TextElement txtElm = layout.FindElement("Text") as TextElement;
        txtElm.SetTextProperties(txtProp);
      });
      
      #endregion TextElement_SetTextProperties

    }

    async public static void PictureElementSnippets()
    {
      // cref: ArcGIS.Desktop.Layouts.PictureElement
      // cref: ArcGIS.Desktop.Layouts.PictureElement.SetSourcePath(System.String)
      #region PictureElement_SetSourcePath
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals("Layout Name"));
      await QueuedTask.Run(() => 
      {
        Layout layout = layoutItem.GetLayout();
        PictureElement picElm = layout.FindElement("Rectangle") as PictureElement;

        picElm.SetSourcePath(@"C:\Some\New\Path\And\file_pic.png");
      });
      #endregion PictureElement_SetSourcePath
    }

    async public static void MapSurroundSnippets()
    {
      // cref: ArcGIS.Desktop.Layouts.MapSurround.SetMapFrame(ArcGIS.Desktop.Layouts.MapFrame)
      // cref: ArcGIS.Desktop.Layouts.MapFrame
      #region MapSurround_SetMapFrame
      await QueuedTask.Run(() =>
      {
        LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>()
                    .FirstOrDefault(item => item.Name.Equals("Layout Name"));
        Layout lyt = layoutItem.GetLayout();
        MapFrame mf = lyt.FindElement("Map1 Map Frame") as MapFrame;
        MapSurround ms = lyt.FindElement("Scale Bar") as MapSurround;

        ms.SetMapFrame(mf);
      });
      #endregion MapSurround_SetMapFrame
    }

    async public static void ExportSnippets()
    {
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>()
              .FirstOrDefault(item => item.Name.Equals("Layout Name"));

      Layout lyt = await QueuedTask.Run(() => layoutItem.GetLayout());
      MapFrame mf = lyt.FindElement("Map1 Map Frame") as MapFrame;

      // cref: BMP_Constructor;ArcGIS.Desktop.Mapping.BMPFormat
      // cref: BMP_Constructor;ArcGIS.Desktop.Mapping.BMPFormat.#ctor
      #region BMP_Constructor
      BMPFormat BMP = new BMPFormat();
      #endregion BMP_Constructor

      // cref: EMF_Constructor;ArcGIS.Desktop.Mapping.EMFFormat
      // cref: EMF_Constructor;ArcGIS.Desktop.Mapping.EMFFormat.#ctor
      #region EMF_Constructor
      EMFFormat EMF = new EMFFormat();
      #endregion EMF_Constructor

      // cref: EPS_Constructor;ArcGIS.Desktop.Mapping.EPSFormat
      // cref: EPS_Constructor;ArcGIS.Desktop.Mapping.EPSFormat.#ctor
      #region EPS_Constructor
      EPSFormat EPS = new EPSFormat();
      #endregion EPS_Constructor

      // cref: GIF_Constructor;ArcGIS.Desktop.Mapping.GIFFormat
      // cref: GIF_Constructor;ArcGIS.Desktop.Mapping.GIFFormat.#ctor
      #region GIF_Constructor
      GIFFormat GIF = new GIFFormat();
      #endregion GIF_Constructor

      // cref: JPEG_Constructor;ArcGIS.Desktop.Mapping.JPEGFormat
      // cref: JPEG_Constructor;ArcGIS.Desktop.Mapping.JPEGFormat.#ctor
      #region JPEG_Constructor
      JPEGFormat JPEG = new JPEGFormat();
      #endregion JPEG_Constructor

      // cref: PNG_Constructor;ArcGIS.Desktop.Mapping.PNGFormat
      // cref: PNG_Constructor;ArcGIS.Desktop.Mapping.PNGFormat.#ctor
      #region PNG_Constructor
      PNGFormat PNG = new PNGFormat();
      #endregion PNG_Constructor

      // cref: PDF_Constructor;ArcGIS.Desktop.Mapping.PDFFormat
      // cref: PDF_Constructor;ArcGIS.Desktop.Mapping.PDFFormat.#ctor
      #region PDF_Constructor
      PDFFormat PDF = new PDFFormat();
      #endregion PDF_Constructor

      // cref: SVG_Constructor;ArcGIS.Desktop.Mapping.SVGFormat
      // cref: SVG_Constructor;ArcGIS.Desktop.Mapping.SVGFormat.#ctor
      #region SVG_Constructor
      SVGFormat SVG = new SVGFormat();
      #endregion SVG_Constructor

      // cref: TGA_Constructor;ArcGIS.Desktop.Mapping.TGAFormat
      // cref: TGA_Constructor;ArcGIS.Desktop.Mapping.TGAFormat.#ctor
      #region TGA_Constructor
      TGAFormat TGA = new TGAFormat();
      #endregion TGA_Constructor

      // cref: TIFF_Constructor;ArcGIS.Desktop.Mapping.TIFFFormat
      // cref: TIFF_Constructor;ArcGIS.Desktop.Mapping.TIFFFormat.#ctor
      #region TIFF_Constructor
      TIFFFormat TIFF = new TIFFFormat();
      #endregion TIFF_Constructor


      PDF.OutputFileName = @"C:\Temp\output.pdf";

      // cref: ArcGIS.Desktop.Layouts.Layout.Export
      #region PDF_lyt_Export
      await QueuedTask.Run(() => lyt.Export(PDF));
      
      #endregion PDF_lyt_Export

    }
  }


}
