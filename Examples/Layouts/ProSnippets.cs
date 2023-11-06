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

//added references
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Layouts;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Core.CIM;
using ArcGIS.Core.Geometry;
using ArcGIS.Core.Data;

namespace ProSnippetsTasks
{
  class Snippets
  {
        #region ProSnippet Group: Layout Project Items
        #endregion
        async public void snippets_ProjectItems()
    {
      // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
      // cref: ArcGIS.Desktop.Layouts.LayoutProjectItem
      #region Reference layout project items and their associated layout
      //Reference layout project items and their associated layout.
      //A layout project item is an item that appears in the Layouts
      //folder in the Catalog pane.

      //Reference all the layout project items
      IEnumerable<LayoutProjectItem> layouts = 
                           Project.Current.GetItems<LayoutProjectItem>();

      //Or reference a specific layout project item by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>()
                                 .FirstOrDefault(item => item.Name.Equals("MyLayout"));
      #endregion

      // cref: ArcGIS.Desktop.Layouts.LayoutProjectItem
      // cref: ArcGIS.Desktop.Layouts.LayoutProjectItem.GetLayout
      // cref: ArcGIS.Desktop.Core.LayoutFrameworkExtender.CreateLayoutPaneAsync
      #region Open a layout project item in a new view
      //Open a layout project item in a new view.
      //A layout project item may exist but it may not be open in a view. 

      //Reference a layout project item by name
      LayoutProjectItem someLytItem = Project.Current.GetItems<LayoutProjectItem>()
                                .FirstOrDefault(item => item.Name.Equals("MyLayout"));

      //Get the layout associated with the layout project item
      Layout layout = await QueuedTask.Run(() => someLytItem.GetLayout());  //Worker thread

      //Create the new pane - call on UI
      ILayoutPane iNewLayoutPane = await ProApp.Panes.CreateLayoutPaneAsync(layout); //GUI thread
      #endregion

      // cref: ArcGIS.Desktop.Layouts.ILayoutPane
      // cref: ArcGIS.Desktop.Layouts.ILayoutPane.LayoutView
      // cref: ArcGIS.Desktop.FRAMEWORK.FRAMEWORKAPPLICATION.Panes
      // cref: ArcGIS.Desktop.FRAMEWORK.Contracts.Pane.Activate
      #region Activate an already open layout view
      //Activate an already open layout view.
      //A layout view may be open but it may not be active.

      //Find the pane that references the layout and activate it. 
      //Note - there can be multiple panes referencing the same layout.
      foreach (var pane in ProApp.Panes)
      {
        var layoutPane = pane as ILayoutPane;
        if (layoutPane == null)  //if not a layout view, continue to the next pane
          continue;
        if (layoutPane.LayoutView.Layout == layout) //activate the view
        {
          (layoutPane as Pane).Activate();
          return;
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Layouts.LayoutView
      // cref: ArcGIS.Desktop.Layouts.LayoutView.Active
      #region Reference the active layout view
      //Reference the active layout view.

      //Confirm if the current, active view is a layout view.
      //If it is, do something.
      LayoutView activeLayoutView = LayoutView.Active;
      if (activeLayoutView != null)
      {
        // do something
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.IProjectItem
      // cref: ArcGIS.Desktop.Core.ItemFactory
      // cref: ArcGIS.Desktop.Core.ItemFactory.Create
      // cref: ArcGIS.Desktop.Core.Project.AddItem(ArcGIS.Desktop.Core.IProjectItem)
      #region Import a pagx into a project
      //Import a pagx into a project.

      //Create a layout project item from importing a pagx file
      await QueuedTask.Run(() =>
      {
        IProjectItem pagx = ItemFactory.Instance.Create(
                                  @"C:\Temp\Layout.pagx") as IProjectItem;
        Project.Current.AddItem(pagx);
      });
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.RemoveItem
      #region Remove a layout project item
      //Remove a layout project item.

      //Remove the layout fro the project
      await QueuedTask.Run(() => Project.Current.RemoveItem(layoutItem));
      #endregion

      // cref: ArcGIS.Desktop.Layouts.LayoutFactory
      // cref: ArcGIS.Desktop.Layouts.LayoutFactory.CreateLayout
      // cref: ArcGIS.Desktop.Layouts.Layout.SetName
      // cref: ArcGIS.Desktop.Core.LayoutFrameworkExtender.CreateLayoutPaneAsync
      #region Create a new, basic layout and open it
      //Create a new, basic layout and open it.

      //Create layout with minimum set of parameters on the worker thread
      Layout lyt = await QueuedTask.Run(() =>
      {
        var newLayout = LayoutFactory.Instance.CreateLayout(8.5, 11, LinearUnit.Inches);
        newLayout.SetName("New 8.5x11 Layout");
        return newLayout;
      });
      
      //Open new layout on the GUI thread
      await ProApp.Panes.CreateLayoutPaneAsync(lyt);
      #endregion

      // cref: ArcGIS.Desktop.Layouts.ILayoutFactory.CreateLayout(ArcGIS.Core.CIM.CIMPage)
      // cref: ArcGIS.Desktop.Layouts.LayoutFactory.CreateLayout(ArcGIS.Core.CIM.CIMPage)
      // cref: ArcGIS.Core.CIM.CIMPAGE
      // cref: ArcGIS.Core.CIM.CIMPAGE.ShowGuides
      // cref: ArcGIS.Core.CIM.CIMPAGE.ShowRulers
      // cref: ArcGIS.Core.CIM.CIMPAGE.SmallestRulerDivision
      // cref: ArcGIS.Core.CIM.CIMPAGE.Guides
      // cref: ArcGIS.Core.CIM.CIMPAGE.Units
      // cref: ArcGIS.Core.CIM.CIMPAGE.Width
      // cref: ArcGIS.Core.CIM.CIMPAGE.Height
      // cref: ArcGIS.Core.CIM.CIMGUIDE
      // cref: ArcGIS.Core.CIM.CIMGUIDE.Position
      // cref: ArcGIS.Core.CIM.CIMGUIDE.Orientation
      // cref: ArcGIS.Core.CIM.Orientation
      // cref: ArcGIS.Desktop.Core.LayoutFrameworkExtender.CreateLayoutPaneAsync
      // cref: ArcGIS.Core.Geometry.LinearUnit
      #region Create a new layout using a modified CIM and open it
      //Create a new layout using a modified CIM and open it.
      //The CIM exposes additional members that may not be
      //available through the managed API.  
      //In this example, optional guides are added.

      //Create a new CIMLayout on the worker thread
      Layout newCIMLayout = await QueuedTask.Run(() =>
      {
        //Set up a CIM page
        CIMPage newPage = new CIMPage
        {
          //required parameters
          Width = 8.5,
          Height = 11,
          Units = LinearUnit.Inches,

          //optional rulers
          ShowRulers = true,
          SmallestRulerDivision = 0.5,

          //optional guides
          ShowGuides = true
        };
        CIMGuide guide1 = new CIMGuide
        {
          Position = 1,
          Orientation = Orientation.Vertical
        };
        CIMGuide guide2 = new CIMGuide
        {
          Position = 6.5,
          Orientation = Orientation.Vertical
        };
        CIMGuide guide3 = new CIMGuide
        {
          Position = 1,
          Orientation = Orientation.Horizontal
        };
        CIMGuide guide4 = new CIMGuide
        {
          Position = 10,
          Orientation = Orientation.Horizontal
        };

        List<CIMGuide> guideList = new List<CIMGuide>
        {
          guide1,
          guide2,
          guide3,
          guide4
        };
        newPage.Guides = guideList.ToArray();

        //Construct the new layout using the customized cim definitions
        var layout_local = LayoutFactory.Instance.CreateLayout(newPage);
        layout_local.SetName("New 8.5x11 Layout");
        return layout_local;
      });

      //Open new layout on the GUI thread
      await ProApp.Panes.CreateLayoutPaneAsync(newCIMLayout);
      #endregion

      // cref: ArcGIS.Desktop.Layouts.LayoutProjectItem.GetLayout
      // cref: ArcGIS.Desktop.Layouts.Layout.GetPage
      // cref: ArcGIS.Desktop.Layouts.Layout.SetPage
      // cref: ArcGIS.Core.CIM.CIMPAGE
      // cref: ArcGIS.Core.CIM.CIMPAGE.Width
      // cref: ArcGIS.Core.CIM.CIMPAGE.Height
      #region Change the layout page size
      //Change the layout page size.

      //Reference the layout project item
      LayoutProjectItem lytItem = Project.Current.GetItems<LayoutProjectItem>()
                               .FirstOrDefault(item => item.Name.Equals("MyLayout"));
      if (layoutItem != null)
      {
        await QueuedTask.Run(() =>
        {
          //Get the layout
          Layout lyt = lytItem.GetLayout();
          if (lyt != null)
          {
            //Change properties
            CIMPage page = lyt.GetPage();
            page.Width = 8.5;
            page.Height = 11;

            //Apply the changes to the layout
            lyt.SetPage(page);
          }
        });
      }
      #endregion
    }

    #region ProSnippet Group CIM Graphics and GraphicFactory
    #endregion

    public static void CreateCircleGraphic(IElementContainer container)
    {
      // cref: ArcGIS.Desktop.Layouts.GraphicFactory.CreateSimpleGraphic
      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx.CreateCircle
      // cref: ArcGIS.Core.Geometry.ArcOrientation
      #region Create Circle Graphic

      //Must be on QueuedTask.Run(() => { ...

      //Build geometry
      Coordinate2D center = new Coordinate2D(2, 4);
      EllipticArcSegment circle_seg = EllipticArcBuilderEx.CreateCircle(
        new Coordinate2D(2, 4), 0.5, ArcOrientation.ArcClockwise, null);
      var circle_poly = PolygonBuilderEx.CreatePolygon(PolylineBuilderEx.CreatePolyline(circle_seg));

      //PolylineBuilderEx.CreatePolyline(cir, AttributeFlags.AllAttributes));
      //Set symbolology, create and add element to layout
      CIMStroke outline = SymbolFactory.Instance.ConstructStroke(
        ColorFactory.Instance.BlackRGB, 2.0, SimpleLineStyle.Dash);

      CIMPolygonSymbol circleSym = SymbolFactory.Instance.ConstructPolygonSymbol(
        ColorFactory.Instance.RedRGB, SimpleFillStyle.Solid, outline);
      SymbolFactory.Instance.ConstructPolygonSymbol(null,
        SymbolFactory.Instance.ConstructStroke(ColorFactory.Instance.RedRGB, 2));

      var circleGraphic = GraphicFactory.Instance.CreateSimpleGraphic(circle_poly, circleSym);

      //Make an element to add to GraphicsLayer or Layout
      //var elemInfo = new ElementInfo() { Anchor = Anchor.CenterPoint };
      //GraphicElement cirElm = ElementFactory.Instance.CreateGraphicElement(
      //  container, circleGraphic, "New Circle", true, elemInfo);

      #endregion
    }

    public static void CreateCircleTextGraphic(IElementContainer container)
    {
      // cref: ArcGIS.Desktop.Layouts.GraphicFactory.CreateSimpleTextGraphic
      // cref: ArcGIS.Desktop.Layouts.TextType
      #region Create Circle Text Graphic

      //Must be on QueuedTask.Run(() => { ...

      //Build geometry
      Coordinate2D center = new Coordinate2D(4.5, 4);
      var eabCir = new EllipticArcBuilderEx(center, 0.5, ArcOrientation.ArcClockwise);
      var cir = eabCir.ToSegment();

      var poly = PolygonBuilderEx.CreatePolygon(
        PolylineBuilderEx.CreatePolyline(cir, AttributeFlags.AllAttributes));

      //Set symbolology, create and add element to layout
      CIMTextSymbol sym = SymbolFactory.Instance.ConstructTextSymbol(
        ColorFactory.Instance.GreenRGB, 10, "Arial", "Regular");
      string text = "Circle, circle, circle";

      var graphic = GraphicFactory.Instance.CreateSimpleTextGraphic(
        TextType.CircleParagraph, poly, sym, text);

      //Make an element to add to GraphicsLayer or Layout
      //var ge = ElementFactory.Instance.CreateGraphicElement(container, graphic,
      //  "New Circle Text", true);
      #endregion
    }

    public static void CreateBezierTextGraphic(IElementContainer container)
    {
      // cref: ArcGIS.Desktop.Layouts.GraphicFactory.CreateSimpleTextGraphic
      // cref: ArcGIS.Desktop.Layouts.TextType
      #region Create Bezier Graphic

      //Must be on QueuedTask.Run(() => { ...

      //Build geometry
      Coordinate2D pt1 = new Coordinate2D(3.5, 7.5);
      Coordinate2D pt2 = new Coordinate2D(4.16, 8);
      Coordinate2D pt3 = new Coordinate2D(4.83, 7.1);
      Coordinate2D pt4 = new Coordinate2D(5.5, 7.5);
      var bez = new CubicBezierBuilderEx(pt1, pt2, pt3, pt4);
      var bezSeg = bez.ToSegment();
      Polyline bezPl = PolylineBuilderEx.CreatePolyline(bezSeg, AttributeFlags.AllAttributes);

      //Set symbolology, create and add element to layout
      CIMTextSymbol sym = SymbolFactory.Instance.ConstructTextSymbol(
            ColorFactory.Instance.BlackRGB, 24, "Comic Sans MS", "Regular");

      var graphic = GraphicFactory.Instance.CreateSimpleTextGraphic(
        TextType.SplinedText, bezPl, sym, "Splined text");

      //Make an element to add to GraphicsLayer or Layout
      //var ge = ElementFactory.Instance.CreateGraphicElement(container, graphic);
      #endregion
    }

    public static void CreateLegendPatchGraphic(IElementContainer container)
    {
      // cref: ArcGIS.Desktop.Layouts.GraphicFactory.CreateLegendPatchGraphic
      // cref: ArcGIS.Core.CIM.PatchShape
      #region Create Legend Patch Graphic

      //Must be on QueuedTask.Run(() => { ...

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
      Polygon poly = PolygonBuilderEx.CreatePolygon(plyCoords);

      //Set symbolology, create and add element to layout
      CIMStroke outline = SymbolFactory.Instance.ConstructStroke(
                ColorFactory.Instance.BlackRGB, 2.0, SimpleLineStyle.Solid);
      CIMPolygonSymbol polySym = SymbolFactory.Instance.ConstructPolygonSymbol(
                ColorFactory.Instance.RedRGB, SimpleFillStyle.ForwardDiagonal, outline);

      var graphic = GraphicFactory.Instance.CreateLegendPatchGraphic(
                  PatchShape.AreaBoundary, poly.Extent, polySym);

      //Make an element to add to GraphicsLayer or Layout
      //
      //var elemInfo = new ElementInfo()
      //{
      //  CustomProperties = null,
      //  Anchor = Anchor.LeftMidPoint
      //};
      //var ge = ElementFactory.Instance.CreateGraphicElement(container, graphic,
      //  "New Legend Patch", true, elemInfo);
      #endregion
    }

    public static void CreateLineArrowGraphic(IElementContainer container)
    {

      // cref: ArcGIS.Desktop.Layouts.GraphicFactory.CreateSimpleGraphic
      // cref: ArcGIS.Desktop.Layouts.ArrowInfo
      // cref: ArcGIS.Desktop.Layouts.ArrowInfo.ArrowHeadKey
      // cref: ArcGIS.Desktop.Layouts.ArrowInfo.ArrowOnBothEnds
      // cref: ArcGIS.Desktop.Layouts.ArrowInfo.ArrowSizePoints
      // cref: ArcGIS.Desktop.Layouts.ArrowInfo.LineWidthPoints
      #region Create Arrow Graphic

      //Must be on QueuedTask.Run(() => { ...

      //Build geometry
      List<Coordinate2D> plCoords = new List<Coordinate2D>();
      plCoords.Add(new Coordinate2D(1, 8.5));
      plCoords.Add(new Coordinate2D(1.66, 9));
      plCoords.Add(new Coordinate2D(2.33, 8.1));
      plCoords.Add(new Coordinate2D(3, 8.5));
      Polyline linePl = PolylineBuilderEx.CreatePolyline(plCoords);

      //Set up the arrow info
      var arrowInfo = new ArrowInfo()
      {
        ArrowHeadKey = ArrowInfo.DefaultArrowHeadKeys[1],
        ArrowOnBothEnds = true,
        ArrowSizePoints = 30,
        LineWidthPoints = 15
      };

      var graphic = GraphicFactory.Instance.CreateArrowGraphic(linePl, arrowInfo);

      //Make an element to add to GraphicsLayer or Layout
      //var ge = ElementFactory.Instance.CreateGraphicElement(
      //  container, graphic, "Arrow Line", false);
      #endregion
    }

    public static void CreatePictureGraphic(Layout layout, string picPath)
    {
      // cref: ArcGIS.Desktop.Layouts.GraphicFactory.CreatePictureGraphic
      #region Create Picture Graphic

      //Must be on QueuedTask.Run(() => { ...

      //Build geometry
      Coordinate2D ll = new Coordinate2D(3.5, 1);
      Coordinate2D ur = new Coordinate2D(5.5, 2);
      Envelope env = EnvelopeBuilderEx.CreateEnvelope(ll, ur);

      //Create and add element to layout
      //string picPath = ApplicationUtilities.BASEPATH + _settings.baseFolder + "irefusetowatchthismovié.jpg";
      var graphic = GraphicFactory.Instance.CreatePictureGraphic(env.Center, picPath);

      //Make an element to add to GraphicsLayer or Layout
      //var ge = ElementFactory.Instance.CreateGraphicElement(layout, graphic);
      #endregion
    }

    public static void CreateOutlineGraphic(Layout layout, CIMGraphic cim_graphic)
    {
      // cref: ArcGIS.Desktop.Layouts.GraphicFactory.GetGraphicOutline
      #region Get Graphic Outline
      //given a graphic, extract its outline geometry

      var graphic_outline = GraphicFactory.Instance.GetGraphicOutline(
                              layout, cim_graphic);
      //TODO - use the geometry - eg, make another graphic
      var outline_graphic = GraphicFactory.Instance.CreateSimpleGraphic(graphic_outline);
      //... etc.

      #endregion

      // cref: ArcGIS.Desktop.Layouts.GraphicFactory.GetGraphicOutline
      #region Get Graphic Outline from Graphic Element
      //given a graphic, extract its outline geometry

      var graphic_elem = layout.GetElementsAsFlattenedList().OfType<GraphicElement>()?.FirstOrDefault();
      if (graphic_elem != null)//can be point, line, poly, or text
        return;

      var outline = GraphicFactory.Instance.GetGraphicOutline(
                              layout, graphic_elem.GetGraphic());
      //create an element using the outline
      var elem = ElementFactory.Instance.CreateGraphicElement(
                          layout, outline);
      //... etc.

      #endregion
    }


    #region ProSnippet Group: Create Layout Graphic Elements
    #endregion

    public static void CreateEllipseElement(IElementContainer container)
    {
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateGraphicElement
      // cref: ArcGIS.Core.CIM.CIMStroke
      // cref: ArcGIS.Core.CIM.CIMPolygonSymbol
      // cref: ArcGIS.Desktop.Mapping.SimpleLineStyle
      // cref: ArcGIS.Desktop.Mapping.SimpleFillStyle
      // cref: ArcGIS.Desktop.Mapping.ColorFactory
      // cref: ArcGIS.Desktop.Mapping.ColorFactory.GreyRGB
      #region Create Ellipse Graphic Element

      //Must be on QueuedTask.Run(() => { ...

      //Build geometry
      Coordinate2D center = new Coordinate2D(2, 2.75);
      var eabElp = new EllipticArcBuilderEx(center, 0, 1, 0.45, 
                                             ArcOrientation.ArcClockwise);
      var ellipse = eabElp.ToSegment();

      //Set symbolology, create and add element to layout
      CIMStroke outline = SymbolFactory.Instance.ConstructStroke(
                                      ColorFactory.Instance.GreenRGB, 2.0,
                              SimpleLineStyle.Dot);
      CIMPolygonSymbol ellipseSym = SymbolFactory.Instance.ConstructPolygonSymbol(
                               ColorFactory.Instance.GreyRGB, SimpleFillStyle.Vertical, 
                                                        outline);

      var poly = PolygonBuilderEx.CreatePolygon(
        PolylineBuilderEx.CreatePolyline(ellipse, AttributeFlags.AllAttributes));

      var elpElm = ElementFactory.Instance.CreateGraphicElement(
        container, poly, ellipseSym, "New Ellipse");
      #endregion

    }

    public static void CreateLassoOpenElement(IElementContainer container)
    {
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateGraphicElement
      // cref: ArcGIS.Core.CIM.CIMLineSymbol
      // cref: ArcGIS.Desktop.Mapping.SimpleLineStyle
      // cref: ArcGIS.Desktop.Mapping.ColorFactory.BlackRGB
      // cref: ArcGIS.Core.Geometry.PolylineBuilderEx.CreatePolyline
      #region Create Lasso Line, Freehand Graphic Element

      //Must be on QueuedTask.Run(() => { ...

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
      Polyline linePl = PolylineBuilderEx.CreatePolyline(plCoords);

      //Set symbolology, create and add element to layout
      CIMLineSymbol lineSym = SymbolFactory.Instance.ConstructLineSymbol(
                ColorFactory.Instance.BlackRGB, 2.0, SimpleLineStyle.Solid);
      //var graphic = GraphicFactory.Instance.CreateShapeGraphic(linePl, lineSym);

      var ge = ElementFactory.Instance.CreateGraphicElement(
                              container, linePl, lineSym, "New Freehand");
      #endregion
    }

    public static void CreateLassoElement(IElementContainer container)
    {
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateGraphicElement
      // cref: ArcGIS.Core.CIM.CIMStroke
      // cref: ArcGIS.Core.CIM.CIMPolygonSymbol
      // cref: ArcGIS.Desktop.Mapping.SimpleLineStyle
      // cref: ArcGIS.Desktop.Mapping.SimpleFillStyle
      // cref: ArcGIS.Desktop.Mapping.ColorFactory.BlackRGB
      // cref: ArcGIS.Desktop.Mapping.ColorFactory.RedRGB
      // cref: ArcGIS.Core.Geometry.PolygonBuilderEx.CreatePolygon
      #region Create Lasso Polygon, Freehand Element

      //Must be on QueuedTask.Run(() => { ...

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
      Polygon poly = PolygonBuilderEx.CreatePolygon(plyCoords);

      //Set symbolology, create and add element to layout
      CIMStroke outline = SymbolFactory.Instance.ConstructStroke(
                  ColorFactory.Instance.BlackRGB, 2.0, SimpleLineStyle.Solid);
      CIMPolygonSymbol polySym = SymbolFactory.Instance.ConstructPolygonSymbol(
               ColorFactory.Instance.RedRGB, SimpleFillStyle.ForwardDiagonal, outline);

      ElementFactory.Instance.CreateGraphicElement(
        container, poly, polySym, "New Lasso");
      #endregion
    }

    public static void CreateLineElement(IElementContainer container)
    {
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateGraphicElement
      // cref: ArcGIS.Desktop.Mapping.StyleProjectItem
      // cref: ArcGIS.Desktop.Mapping.StyleItemType
      // cref: ARCGIS.DESKTOP.MAPPING.SymbolStyleItem.Symbol
      // cref: ARCGIS.DESKTOP.MAPPING.STYLEHELPER.SEARCHSYMBOLS
      // cref: ARCGIS.DESKTOP.MAPPING.SYMBOLEXTENSIONMETHODS.SETSIZE
      // cref: ArcGIS.Core.Geometry.PolylineBuilderEx.CreatePolyline
      #region Create Line Element

      //Must be on QueuedTask.Run(() => { ...

      //Build geometry
      List<Coordinate2D> plCoords = new List<Coordinate2D>();
      plCoords.Add(new Coordinate2D(1, 8.5));
      plCoords.Add(new Coordinate2D(1.66, 9));
      plCoords.Add(new Coordinate2D(2.33, 8.1));
      plCoords.Add(new Coordinate2D(3, 8.5));
      Polyline linePl = PolylineBuilderEx.CreatePolyline(plCoords);

      //Reference a line symbol in a style
      var ProjectStyles = Project.Current.GetItems<StyleProjectItem>();
      StyleProjectItem style = ProjectStyles.First(x => x.Name == "ArcGIS 2D");
      var symStyle = style.SearchSymbols(StyleItemType.LineSymbol, "Line with 2 Markers")[0];
      CIMLineSymbol lineSym = symStyle.Symbol as CIMLineSymbol;
      lineSym.SetSize(20);

      //Set symbolology, create and add element to layout
      //CIMLineSymbol lineSym = SymbolFactory.Instance.ConstructLineSymbol(ColorFactory.Instance.BlueRGB, 4.0, SimpleLineStyle.Solid);
      ElementFactory.Instance.CreateGraphicElement(
        container, linePl, lineSym, "New Line");

      #endregion
    }

    public static void CreatePointElement(IElementContainer container)
    {
      // cref: ArcGIS.Desktop.Layouts.GraphicFactory.CreateSimpleGraphic
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateGraphicElement
      // cref: ArcGIS.Desktop.Mapping.StyleProjectItem
      // cref: ArcGIS.Desktop.Mapping.StyleItemType
      // cref: ArcGIS.Core.CIM.Anchor
      // cref: ARCGIS.DESKTOP.MAPPING.SymbolStyleItem.Symbol
      // cref: ArcGIS.Core.CIM.CIMStringMap
      // cref: ArcGIS.Core.CIM.CIMStringMap.Key
      // cref: ArcGIS.Core.CIM.CIMStringMap.Value
      // cref: ARCGIS.DESKTOP.MAPPING.STYLEHELPER.SEARCHSYMBOLS
      // cref: ARCGIS.DESKTOP.MAPPING.SYMBOLEXTENSIONMETHODS.SETSIZE
      // cref: ArcGIS.Desktop.Layouts.ElementInfo
      // cref: ArcGIS.Desktop.Layouts.ElementInfo.CustomProperties
      // cref: ArcGIS.Desktop.Layouts.ElementInfo.Anchor
      // cref: ArcGIS.Desktop.Layouts.ElementInfo.Rotation
      #region Create Point Element

      //Must be on QueuedTask.Run(() => { ...

      //Build geometry
      Coordinate2D coord2D = new Coordinate2D(2.0, 10.0);

      //Reference a point symbol in a style
      StyleProjectItem stylePrjItm = Project.Current.GetItems<StyleProjectItem>()
               .FirstOrDefault(item => item.Name == "ArcGIS 2D");
      SymbolStyleItem symStyleItm = stylePrjItm.SearchSymbols(
                            StyleItemType.PointSymbol, "City Hall")[0];
      CIMPointSymbol pointSym = symStyleItm.Symbol as CIMPointSymbol;
      pointSym.SetSize(50);

      var elemInfo = new ElementInfo()
      {
        CustomProperties = new List<CIMStringMap>() {
           new CIMStringMap() { Key = "Key1", Value = "Value1"},
           new CIMStringMap() { Key = "Key2", Value = "Value2"}
         },
        Anchor = Anchor.TopRightCorner,
        Rotation = 45.0
      };

      var graphic = GraphicFactory.Instance.CreateSimpleGraphic(
                                    coord2D.ToMapPoint(), pointSym);

      ElementFactory.Instance.CreateGraphicElement(
        container, graphic, "New Point", true, elemInfo);

      #endregion
    }

    public static void CreatePolygonElement(IElementContainer container)
    {
      // cref: ArcGIS.Core.Geometry.PolygonBuilderEx.CreatePolygon
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateGraphicElement
      #region Create Polygon Element

      //Must be on QueuedTask.Run(() => { ...

      //Build geometry
      List<Coordinate2D> plyCoords = new List<Coordinate2D>();
      plyCoords.Add(new Coordinate2D(1, 7));
      plyCoords.Add(new Coordinate2D(2, 7));
      plyCoords.Add(new Coordinate2D(2, 6.7));
      plyCoords.Add(new Coordinate2D(3, 6.7));
      plyCoords.Add(new Coordinate2D(3, 6.1));
      plyCoords.Add(new Coordinate2D(1, 6.1));
      Polygon poly = PolygonBuilderEx.CreatePolygon(plyCoords);

      //Set symbolology, create and add element to layout
      CIMStroke outline = SymbolFactory.Instance.ConstructStroke(
        ColorFactory.Instance.BlueRGB, 2.0, SimpleLineStyle.DashDotDot);
      CIMPolygonSymbol polySym = SymbolFactory.Instance.ConstructPolygonSymbol(
        ColorFactory.Instance.RedRGB, SimpleFillStyle.ForwardDiagonal, outline);

      ElementFactory.Instance.CreateGraphicElement(
        container, poly, polySym, "New Polygon", false);

      #endregion
    }

    public static void CreateRectangleElement(IElementContainer container)
    {
      // cref: ArcGIS.Desktop.Layouts.GraphicFactory.CreateSimpleGraphic
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateGraphicElement
      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.CreateEnvelope
      // cref: ArcGIS.Core.CIM.Anchor
      // cref: ArcGIS.Desktop.Layouts.ElementInfo
      // cref: ArcGIS.Desktop.Layouts.ElementInfo.Anchor
      // cref: ArcGIS.Desktop.Layouts.ElementInfo.Rotation
      // cref: ArcGIS.Desktop.Layouts.ElementInfo.CornerRounding
      #region Create Rectangle Element

      //Must be on QueuedTask.Run(() => { ...

      //Build geometry
      Coordinate2D ll = new Coordinate2D(1.0, 4.75);
      Coordinate2D ur = new Coordinate2D(3.0, 5.75);
      Envelope env = EnvelopeBuilderEx.CreateEnvelope(ll, ur);

      //Set symbolology, create and add element to layout
      CIMStroke outline = SymbolFactory.Instance.ConstructStroke(
        ColorFactory.Instance.BlackRGB, 5.0, SimpleLineStyle.Solid);
      CIMPolygonSymbol polySym = SymbolFactory.Instance.ConstructPolygonSymbol(
        ColorFactory.Instance.GreenRGB, SimpleFillStyle.DiagonalCross, outline);

      var ge = GraphicFactory.Instance.CreateSimpleGraphic(env, polySym);
      var elemInfo = new ElementInfo()
      {
        Anchor = Anchor.CenterPoint,
        Rotation = 45.0,
        CornerRounding = 5.0
      };

      ElementFactory.Instance.CreateGraphicElement(
        container, env, polySym, "New Rectangle", false, elemInfo);
      #endregion
    }

    public static void CreateBezierElement(IElementContainer container)
    {
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateGraphicElement
      // cref: ArcGIS.Core.Geometry.CubicBezierBuilderEx.CreateCubicBezierSegment(Coordinate2D,Coordinate2D,Coordinate2D,Coordinate2D,SpatialReference)
      // cref: ArcGIS.Core.Geometry.CubicBezierBuilderEx.ToSegment
      // cref: ArcGIS.Core.Geometry.PolylineBuilderEx.CreatePolyline(Segment,SpatialReference)
      #region Create Bezier Curve Element

      //Must be on QueuedTask.Run(() => { ...
      //Build geometry
      Coordinate2D pt1 = new Coordinate2D(1, 7.5);
      Coordinate2D pt2 = new Coordinate2D(1.66, 8);
      Coordinate2D pt3 = new Coordinate2D(2.33, 7.1);
      Coordinate2D pt4 = new Coordinate2D(3, 7.5);
      var bez = new CubicBezierBuilderEx(pt1, pt2, pt3, pt4);
      var bezSeg = bez.ToSegment();
      Polyline bezPl = PolylineBuilderEx.CreatePolyline(bezSeg, AttributeFlags.AllAttributes);

      //Set symbology, create and add element to layout
      CIMLineSymbol lineSym = SymbolFactory.Instance.ConstructLineSymbol(
        ColorFactory.Instance.RedRGB, 4.0, SimpleLineStyle.DashDot);

      ElementFactory.Instance.CreateGraphicElement(container, bezPl, lineSym, "New Bezier");

      #endregion
    }

    public static void CreateGraphicElements(IElementContainer container)
    {
      // cref: ArcGIS.Desktop.Layouts.GraphicFactory.CreateSimpleGraphic
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateGraphicElements
      #region Create Graphic Elements

      //Must be on QueuedTask.Run(() => { ...

      //Build geometry
      List<Coordinate2D> plyCoords = new List<Coordinate2D>();
      plyCoords.Add(new Coordinate2D(1, 7));
      plyCoords.Add(new Coordinate2D(2, 7));
      plyCoords.Add(new Coordinate2D(2, 6.7));
      plyCoords.Add(new Coordinate2D(3, 6.7));
      plyCoords.Add(new Coordinate2D(3, 6.1));
      plyCoords.Add(new Coordinate2D(1, 6.1));
      Polygon poly = PolygonBuilderEx.CreatePolygon(plyCoords);

      //Build geometry
      Coordinate2D ll = new Coordinate2D(1.0, 4.75);
      Coordinate2D ur = new Coordinate2D(3.0, 5.75);
      Envelope env = EnvelopeBuilderEx.CreateEnvelope(ll, ur);

      //Build geometry
      Coordinate2D coord2D = new Coordinate2D(2.0, 10.0);

      var g1 = GraphicFactory.Instance.CreateSimpleGraphic(poly);
      var g2 = GraphicFactory.Instance.CreateSimpleGraphic(env);
      var g3 = GraphicFactory.Instance.CreateSimpleGraphic(coord2D.ToMapPoint());

      var ge = ElementFactory.Instance.CreateGraphicElements(
        container, new List<CIMGraphic>() { g1, g2, g3 },
        new List<string>() { "Poly", "Envelope", "MapPoint" },
        true);
      #endregion
    }

    public async void snippets_CreateLayoutGraphicElements()
    {
      LayoutView layoutView = LayoutView.Active;
      Layout container = layoutView.Layout;

      await QueuedTask.Run(() =>
      {
        // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateGraphicElement
        // cref: ArcGIS.Desktop.Layouts.IElementFactory.CreateGraphicElement
        // cref: ArcGIS.Desktop.Layouts.GraphicFactory
        // cref: ArcGIS.Desktop.Layouts.GraphicFactory.CreateSimpleGraphic
        // cref: ARCGIS.CORE.CIM.CIMPOINTGRAPHIC
        // cref: ARCGIS.CORE.CIM.CIMPOINTGRAPHIC.Location
        // cref: ARCGIS.CORE.CIM.CIMGRAPHIC.Symbol
        // cref: ArcGIS.CORE.CIM.CIMGraphicElement
        // cref: ArcGIS.CORE.CIM.CIMGraphicElement.Graphic
        #region Create Graphic Element using CIMGraphic
        //on the QueuedTask
        //Place symbol on the layout
        //At 2.x - MapPoint location = MapPointBuilder.CreateMapPoint(
        //                                               new Coordinate2D(9, 1));
        MapPoint location = MapPointBuilderEx.CreateMapPoint(new Coordinate2D(9, 1));

        //specify a symbol
        var pt_symbol = SymbolFactory.Instance.ConstructPointSymbol(
                              ColorFactory.Instance.GreenRGB);

        //create a CIMGraphic 
        var graphic = new CIMPointGraphic()
        {
          Symbol = pt_symbol.MakeSymbolReference(),
          Location = location //center of map
        };
        //Or use GraphicFactory
        var graphic2 = GraphicFactory.Instance.CreateSimpleGraphic(location, pt_symbol);

        //At 2.x - LayoutElementFactory.Instance.CreateGraphicElement(layout, graphic);
        ElementFactory.Instance.CreateGraphicElement(container, graphic);
        ElementFactory.Instance.CreateGraphicElement(container, graphic2);
        #endregion
      });

      await QueuedTask.Run(() =>
      {
        // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateGraphicElement
        // cref: ArcGIS.Desktop.Layouts.IElementFactory.CreateGraphicElement
        // cref: ARCGIS.CORE.CIM.CIMPOINTGRAPHIC
        #region Create Graphic Element using CIMSymbol

        //Must be on QueuedTask.Run(() => { ...

        //Place symbol on the layout
        //At 2.x - MapPoint location = MapPointBuilder.CreateMapPoint(
        //                                       new Coordinate2D(9, 1));
        MapPoint location = MapPointBuilderEx.CreateMapPoint(new Coordinate2D(9, 1));

        //specify a symbol
        var pt_symbol = SymbolFactory.Instance.ConstructPointSymbol(
                              ColorFactory.Instance.GreenRGB);
        //At 2.x -
        //     LayoutElementFactory.Instance.CreateGraphicElement(
        //                                       layout, location, pt_symbol);

        ElementFactory.Instance.CreateGraphicElement(container, location, pt_symbol);
        #endregion
      });

      await QueuedTask.Run(() => {
        // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateGraphicElements
        // cref: ArcGIS.Desktop.Layouts.IElementFactory.CreateGraphicElements
        // cref: ARCGIS.CORE.CIM.CIMPOINTGRAPHIC
        // cref: ARCGIS.CORE.CIM.CIMPOINTGRAPHIC.Location
        // cref: ARCGIS.CORE.CIM.CIMGRAPHIC.Symbol
        // cref: ArcGIS.Desktop.Layouts.GraphicFactory
        // cref: ArcGIS.Desktop.Layouts.GraphicFactory.CreateSimpleGraphic
        #region Bulk Element creation

        //Must be on QueuedTask.Run(() => { ...

        //List of Point graphics
        var listGraphics = new List<CIMPointGraphic>();
        var listGraphics2 = new List<CIMPointGraphic>();
        //Symbol
        var pointSymbol = SymbolFactory.Instance.ConstructPointSymbol(
                                            ColorFactory.Instance.BlackRGB);
        //Define size of the array
        int dx = 5;
        int dy = 5;
        MapPoint point = null;
        //Create the List of graphics for the array
        for (int row = 0; row <= dx; ++row)
        {
          for (int col = 0; col <= dy; ++col)
          {
            //At 2.x - point = MapPointBuilder.CreateMapPoint(col, row);
            point = MapPointBuilderEx.CreateMapPoint(col, row);
            //create a CIMGraphic 
            var graphic = new CIMPointGraphic()
            {
              Symbol = pointSymbol.MakeSymbolReference(),
              Location = point
            };
            listGraphics.Add(graphic);
            //Or use GraphicFactory
            var graphic2 = GraphicFactory.Instance.CreateSimpleGraphic(
                                          point, pointSymbol) as CIMPointGraphic;
            listGraphics2.Add(graphic2);
          }
        }
        //Draw the array of graphics
        //At 2.x - var bulkgraphics =
        //              LayoutElementFactory.Instance.CreateGraphicElements(
        //                                              layout, listGraphics, null);

        var bulkgraphics = ElementFactory.Instance.CreateGraphicElements(
                                                           container, listGraphics);
        var bulkgraphics2 = ElementFactory.Instance.CreateGraphicElements(
                                                           container, listGraphics2);
        #endregion

      });

      await QueuedTask.Run(() =>
      {
        // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateElement
        // cref: ARCGIS.CORE.CIM.CIMPOINTGRAPHIC
        // cref: ARCGIS.CORE.CIM.CIMPOINTGRAPHIC.Location
        // cref: ARCGIS.CORE.CIM.CIMGRAPHIC.Symbol
        // cref: ArcGIS.CORE.CIM.CIMGraphicElement
        // cref: ArcGIS.CORE.CIM.CIMGraphicElement.Graphic
        #region Create Element using a CIMGraphicElement

        //Must be on QueuedTask.Run(() => { ...

        //Place symbol on the layout
        //At 2.x - MapPoint point = MapPointBuilder.CreateMapPoint(new Coordinate2D(9, 1));
        MapPoint point = MapPointBuilderEx.CreateMapPoint(new Coordinate2D(9, 1));

        //specify a symbol
        var pt_symbol = SymbolFactory.Instance.ConstructPointSymbol(
                              ColorFactory.Instance.GreenRGB);

        //create a CIMGraphic 
        var graphic = new CIMGraphicElement()
        {
          Graphic = new CIMPointGraphic()
          {
            Symbol = pt_symbol.MakeSymbolReference(),
            Location = point //A point in the layout
          }
        };
        //At 2.x - LayoutElementFactory.Instance.CreateElement(layout, graphic);
        ElementFactory.Instance.CreateElement(container, graphic);
        #endregion
      });

      // cref: ArcGIS.Desktop.Mapping.StyleProjectItem
      // cref: ArcGIS.Desktop.Mapping.SymbolStyleItem
      // cref: ARCGIS.DESKTOP.MAPPING.STYLEHELPER.SEARCHSYMBOLS
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateGraphicElement
      // cref: ARCGIS.DESKTOP.MAPPING.SYMBOLEXTENSIONMETHODS.SETSIZE
      #region Create point graphic with symbology
      //Create a simple 2D point graphic and apply an existing point style item as the symbology.

      //Construct on the worker thread
      await QueuedTask.Run(() =>
      {
        //Build 2D point geometry  
        Coordinate2D coord2D = new Coordinate2D(2.0, 10.0);

        //(optionally) Reference a point symbol in a style
        StyleProjectItem ptStylePrjItm = Project.Current.GetItems<StyleProjectItem>()
                                      .FirstOrDefault(item => item.Name == "ArcGIS 2D");
        SymbolStyleItem ptSymStyleItm = ptStylePrjItm.SearchSymbols(
                                               StyleItemType.PointSymbol, "City Hall")[0];
        CIMPointSymbol pointSym = ptSymStyleItm.Symbol as CIMPointSymbol;
        pointSym.SetSize(50);

        //Set symbolology, create and add element to layout

        //An alternative simple symbol is also commented out below.
        //This would elminate the four optional lines of code above that
        //reference a style.

        //CIMPointSymbol pointSym = SymbolFactory.Instance.ConstructPointSymbol(
        //                  ColorFactory.Instance.RedRGB, 25.0, SimpleMarkerStyle.Star);  
        //At 2.x - GraphicElement ptElm =
        //                    LayoutElementFactory.Instance.CreatePointGraphicElement(
        //                                        layout, coord2D, pointSym);

        GraphicElement ptElm = ElementFactory.Instance.CreateGraphicElement(
                                       container, coord2D.ToMapPoint(), pointSym);
        ptElm.SetName("New Point");
      });
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StyleProjectItem
      // cref: ArcGIS.Desktop.Mapping.SymbolStyleItem
      // cref: ArcGIS.Core.CIM.CIMLineSymbol
      // cref: ARCGIS.DESKTOP.MAPPING.STYLEHELPER.SEARCHSYMBOLS
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateGraphicElement
      // cref: ARCGIS.DESKTOP.MAPPING.SYMBOLEXTENSIONMETHODS.SETSIZE
      #region Create line graphic with symbology
      //Create a simple 2D line graphic and apply an existing line
      //style item as the symbology.

      //Construct on the worker thread
      await QueuedTask.Run(() =>
      {
        //Build 2d line geometry
        List<Coordinate2D> plCoords = new List<Coordinate2D>();
        plCoords.Add(new Coordinate2D(1, 8.5));
        plCoords.Add(new Coordinate2D(1.66, 9));
        plCoords.Add(new Coordinate2D(2.33, 8.1));
        plCoords.Add(new Coordinate2D(3, 8.5));
        //At 2.x - Polyline linePl = PolylineBuilder.CreatePolyline(plCoords);
        Polyline linePl = PolylineBuilderEx.CreatePolyline(plCoords);

        //(optionally) Reference a line symbol in a style
        StyleProjectItem lnStylePrjItm = Project.Current.GetItems<StyleProjectItem>()
                                      .FirstOrDefault(item => item.Name == "ArcGIS 2D");
        SymbolStyleItem lnSymStyleItm = lnStylePrjItm.SearchSymbols(
                                      StyleItemType.LineSymbol, "Line with 2 Markers")[0];
        CIMLineSymbol lineSym = lnSymStyleItm.Symbol as CIMLineSymbol;
        lineSym.SetSize(20);

        //Set symbolology, create and add element to layout

        //An alternative simple symbol is also commented out below.
        //This would elminate the four optional lines of code above that
        //reference a style.
        //
        //CIMLineSymbol lineSym = SymbolFactory.Instance.ConstructLineSymbol(
        //         ColorFactory.Instance.BlueRGB, 4.0, SimpleLineStyle.Solid);  
        //At 2.x - GraphicElement lineElm =
        //        LayoutElementFactory.Instance.CreateLineGraphicElement(
        //                                            layout, linePl, lineSym);

        GraphicElement lineElm = ElementFactory.Instance.CreateGraphicElement(
                                                    container, linePl, lineSym);
        lineElm.SetName("New Line");
      });
      #endregion

      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.CreateEnvelope
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructStroke(CIMCOLOR,DOUBLE,SIMPLELINESTYLE)
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructPolygonSymbol(CIMCOLOR,SIMPLEFILLSTYLE,CIMSTROKE)
      // cref: ArcGIS.Desktop.Mapping.SimpleLineStyle
      // cref: ArcGIS.Desktop.Mapping.SimpleFillStyle
      // cref: ArcGIS.Desktop.Mapping.StyleProjectItem
      // cref: ArcGIS.Desktop.Mapping.SymbolStyleItem
      // cref: ArcGIS.Core.CIM.CIMStroke
      // cref: ArcGIS.Core.CIM.CIMPolygonSymbol
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateGraphicElement
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreatePredefinedShapeGraphicElement
      // cref: ArcGIS.Desktop.Layouts.PredefinedShape
      // cref: ArcGIS.Desktop.Layouts.GraphicElement
      #region Create rectangle graphic with simple symbology
      //Create a simple 2D rectangle graphic and apply simple fill and
      //outline symbols.

      //Construct on the worker thread
      await QueuedTask.Run(() =>
      {
        //Build 2D envelope geometry
        Coordinate2D rec_ll = new Coordinate2D(1.0, 4.75);
        Coordinate2D rec_ur = new Coordinate2D(3.0, 5.75);
        //At 2.x - Envelope rec_env = EnvelopeBuilder.CreateEnvelope(rec_ll, rec_ur);
        Envelope rec_env = EnvelopeBuilderEx.CreateEnvelope(rec_ll, rec_ur);

        //Set symbolology, create and add element to layout
        CIMStroke outline = SymbolFactory.Instance.ConstructStroke(
          ColorFactory.Instance.BlackRGB, 5.0, SimpleLineStyle.Solid);
        CIMPolygonSymbol polySym = SymbolFactory.Instance.ConstructPolygonSymbol(
          ColorFactory.Instance.GreenRGB, SimpleFillStyle.DiagonalCross, outline);

        //At 2.x - GraphicElement recElm =
        //           LayoutElementFactory.Instance.CreateRectangleGraphicElement(
        //                                                  layout, rec_env, polySym);
        //         recElm.SetName("New Rectangle");
        //
        GraphicElement recElm = ElementFactory.Instance.CreateGraphicElement(
          container, rec_env, polySym, "New Rectangle");
        //Or use Predefined shape
        GraphicElement recElm2 = ElementFactory.Instance.CreatePredefinedShapeGraphicElement(
                                  container, PredefinedShape.Rectangle, rec_env, polySym, 
                                  "New Rectangle2");
      });
      #endregion

    }

    #region ProSnippet Group: Create Text Graphic Elements
    #endregion

    public async void snippets_CreateTextElements()
    {
      LayoutView layoutView = LayoutView.Active;
      Layout container = layoutView.Layout;

      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.CreateEnvelope
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructTextSymbol(CIMPOLYGONSYMBOL,DOUBLE,STRING,STRING)
      // cref: ArcGIS.Core.CIM.CIMTextSymbol
      // cref: ArcGIS.Core.CIM.Anchor
      // cref: ArcGIS.Desktop.Layouts.ElementInfo
      // cref: ArcGIS.Desktop.Layouts.ElementInfo.Anchor
      // cref: ArcGIS.Desktop.Layouts.ElementInfo.Rotation
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateTextGraphicElement
      // cref: ArcGIS.Desktop.Layouts.TextType
      // cref: ArcGIS.Desktop.Layouts.Element.SetX
      // cref: ArcGIS.Desktop.Layouts.Element.SetY
      #region Create Point Text Element 1
      //Create a simple point text element and assign basic symbology and text settings.

      //Construct on the worker thread
      await QueuedTask.Run(() =>
      {
        //Build 2D point geometry
        Coordinate2D coord2D = new Coordinate2D(3.5, 10);

        //Set symbolology, create and add element to layout
        CIMTextSymbol sym = SymbolFactory.Instance.ConstructTextSymbol(
                      ColorFactory.Instance.RedRGB, 32, "Arial", "Regular");
        string textString = "Point text";
        //At 2.x - GraphicElement ptTxtElm =
        //         LayoutElementFactory.Instance.CreatePointTextGraphicElement(
        //                           layout, coord2D, textString, sym);
        //ptTxtElm.SetName("New Point Text");
        //ptTxtElm.SetAnchor(Anchor.CenterPoint);
        //ptTxtElm.SetX(4.5);
        //ptTxtElm.SetY(9.5);
        //ptTxtElm.SetRotation(45);

        //use ElementInfo to set placement properties during create
        var elemInfo = new ElementInfo()
        {
          Anchor = Anchor.CenterPoint,
          Rotation = 45
        };
        var ptTxtElm = ElementFactory.Instance.CreateTextGraphicElement(
          container, TextType.PointText, coord2D.ToMapPoint(), sym, textString,
                             "New Point Text", true, elemInfo);

        //Change additional text properties
        ptTxtElm.SetX(4.5);
        ptTxtElm.SetY(9.5);
      });
      #endregion

      // cref: ArcGIS.Core.Geometry.PolygonBuilderEx.CreatePolygon(IEnumerable{Coordinate3D},SpatialReference)
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructTextSymbol(CIMPOLYGONSYMBOL,DOUBLE,STRING,STRING)
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateTextGraphicElement
      // cref: ArcGIS.Desktop.Layouts.TextType
      // cref: ArcGIS.Core.CIM.CIMTextSymbol
      // cref: ArcGIS.Core.CIM.CIMGraphic
      // cref: ArcGIS.Core.CIM.CIMParagraphTextGraphic
      // cref: ArcGIS.Core.CIM.CIMParagraphTextGraphic.Frame
      // cref: ArcGIS.Core.CIM.CIMGraphicFrame
      // cref: ArcGIS.Core.CIM.CIMGraphicFrame.BorderSymbol
      // cref: ArcGIS.Desktop.Layouts.GraphicElement.GetGraphic
      // cref: ArcGIS.Desktop.Layouts.GraphicElement.SetGraphic
      #region Create Rectangle Paragraph Text Element 1
      //Create rectangle text with background and border symbology.  

      //Construct on the worker thread
      await QueuedTask.Run(() =>
      {
        //Build 2D polygon geometry
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
        //Also notice how formatting tags are using within the text string.
        CIMTextSymbol sym = SymbolFactory.Instance.ConstructTextSymbol(
                          ColorFactory.Instance.GreyRGB, 10, "Arial", "Regular");
        string text = "Some Text String that is really long and is " +
                      "<BOL>forced to wrap to other lines</BOL> so that " +
                      "we can see the effects." as String;
        //At 2.x - GraphicElement polyTxtElm =
        //           LayoutElementFactory.Instance.CreatePolygonParagraphGraphicElement(
        //                                      layout, poly, text, sym);
        //         polyTxtElm.SetName("New Polygon Text");

        GraphicElement polyTxtElm = ElementFactory.Instance.CreateTextGraphicElement(
          container, TextType.RectangleParagraph, poly, sym, text, "Polygon Paragraph");

        //(Optionally) Modify paragraph border 
        CIMGraphic polyTxtGra = polyTxtElm.GetGraphic();
        CIMParagraphTextGraphic cimPolyTxtGra = polyTxtGra as CIMParagraphTextGraphic;
        cimPolyTxtGra.Frame.BorderSymbol = new CIMSymbolReference();
        cimPolyTxtGra.Frame.BorderSymbol.Symbol =
                     SymbolFactory.Instance.ConstructLineSymbol(
                                ColorFactory.Instance.GreyRGB, 1.0, SimpleLineStyle.Solid);
        polyTxtElm.SetGraphic(polyTxtGra);
      });
      #endregion

      // cref: ArcGIS.Desktop.Layouts.TextElement
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateTextGraphicElement
      // cref: ArcGIS.Desktop.Layouts.TextElement.SetTextProperties
      // cref: ArcGIS.Desktop.Layouts.TextProperties
      #region Create a Dynamic Point Text Element
      //Create a dynamic text element.

      //Set the string with tags and the location
      String title = @"<dyn type = ""page"" property = ""name"" />";
      Coordinate2D llTitle = new Coordinate2D(6, 2.5);

      //Construct element on worker thread
      await QueuedTask.Run(() =>
      {
        //Create with default text properties
        //At 2.x - TextElement titleGraphics =
        //          LayoutElementFactory.Instance.CreatePointTextGraphicElement(
        //                                  layout, llTitle, null) as TextElement;
        TextElement titleGraphics = ElementFactory.Instance.CreateTextGraphicElement(
                      container, TextType.PointText, llTitle.ToMapPoint(), null, title) as TextElement;

        //Modify the text properties
        titleGraphics.SetTextProperties(new TextProperties(title, "Arial", 24, "Bold"));
      });
      #endregion
    }

    public static void CreatePointTextElement(IElementContainer container)
    {
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateTextGraphicElement
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructTextSymbol
      // cref: ArcGIS.Desktop.Layouts.TextType
      // cref: ArcGIS.Desktop.Layouts.ElementInfo.Anchor
      #region Create Point Text Element 2

      //Must be on QueuedTask.Run(() => { ...

      //Build geometry
      Coordinate2D coord2D = new Coordinate2D(3.5, 10);

      //Set symbolology, create and add element to layout
      CIMTextSymbol sym = SymbolFactory.Instance.ConstructTextSymbol(
        ColorFactory.Instance.RedRGB, 32, "Arial", "Regular");
      string textString = "Point text";

      var elemInfo = new ElementInfo() { Anchor = Anchor.BottomLeftCorner };
      GraphicElement ptTxtElm = ElementFactory.Instance.CreateTextGraphicElement(
        container, TextType.PointText, coord2D.ToMapPoint(), sym, textString,
        "New Point Text", true, elemInfo);

      #endregion
    }

    public static void CreatePolygonTextElement(IElementContainer container)
    {
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateTextGraphicElement
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructTextSymbol
      // cref: ArcGIS.Desktop.Layouts.TextType
      #region Create Polygon Paragraph Text Element

      //Must be on QueuedTask.Run(() => { ...

      //Build geometry
      List<Coordinate2D> plyCoords = new List<Coordinate2D>();
      plyCoords.Add(new Coordinate2D(3.5, 7));
      plyCoords.Add(new Coordinate2D(4.5, 7));
      plyCoords.Add(new Coordinate2D(4.5, 6.7));
      plyCoords.Add(new Coordinate2D(5.5, 6.7));
      plyCoords.Add(new Coordinate2D(5.5, 6.1));
      plyCoords.Add(new Coordinate2D(3.5, 6.1));
      Polygon poly = PolygonBuilderEx.CreatePolygon(plyCoords);

      //Set symbolology, create and add element to layout
      CIMTextSymbol sym = SymbolFactory.Instance.ConstructTextSymbol(
           ColorFactory.Instance.GreyRGB, 10, "Arial", "Regular");
      string text = "Some text string that is really long and " +
                    "<BOL>wraps to other lines</BOL>" +
                    " so that we can see the effects.";

      var ge = ElementFactory.Instance.CreateTextGraphicElement(
        container, TextType.PolygonParagraph, poly, sym, text,
              "New Polygon Text", true);
      #endregion
    }

    public static void CreateRectangleText(IElementContainer container)
    {
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateTextGraphicElement
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructTextSymbol
      // cref: ArcGIS.Desktop.Layouts.TextType
      #region Create Rectangle Paragraph Text Element 2

      //Must be on QueuedTask.Run(() => { ...

      //Build geometry
      Coordinate2D ll = new Coordinate2D(3.5, 4.75);
      Coordinate2D ur = new Coordinate2D(5.5, 5.75);
      Envelope env = EnvelopeBuilderEx.CreateEnvelope(ll, ur);

      //Set symbolology, create and add element to layout
      CIMTextSymbol sym = SymbolFactory.Instance.ConstructTextSymbol(
                 ColorFactory.Instance.WhiteRGB, 10, "Arial", "Regular");
      string text = "Some text string that is really long and " +
                    "<BOL>wraps to other lines</BOL>" +
                    " so that we can see the effects.";

      //(Optionally) Modify border and background with 50% transparency 
      //CIMGraphic recTxtGra = recTxtElm.Graphic;
      //CIMParagraphTextGraphic cimRecTxtGra = recTxtGra as CIMParagraphTextGraphic;
      //CIMSymbolReference cimRecTxtBorder = cimRecTxtGra.Frame.BorderSymbol;
      //
      //CIMLineSymbol lineSym = SymbolFactory.Instance.ConstructLineSymbol(
      //                    ColorFactory.Instance.BlackRGB, 1.0, SimpleLineStyle.Solid);
      //cimRecTxtBorder.Symbol = lineSym;
      //
      //CIMSymbolReference cimRecTxtBkgrd = cimRecTxtGra.Frame.BackgroundSymbol;
      //CIMPolygonSymbol polySym = SymbolFactory.Instance.ConstructPolygonSymbol(
      //                            ColorFactory.Instance.GreyRGB, SimpleFillStyle.Solid);
      //
      //CIMColor symCol = polySym.GetColor(IElementContainer container);
      //symCol.SetAlphaValue(50);
      //cimRecTxtBkgrd.Symbol = polySym;
      //recTxtElm.SetGraphic(recTxtGra);

      var ge = ElementFactory.Instance.CreateTextGraphicElement(container,
                  TextType.RectangleParagraph, env, sym, text, "New Rectangle Text");
      #endregion
    }

    public static void CreateCircleTextElement(IElementContainer container)
    {
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateTextGraphicElement
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructTextSymbol
      // cref: ArcGIS.Desktop.Layouts.TextType
      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx.#ctor
      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx.ToSegment
      #region Create Circle Text Element

      //Must be on QueuedTask.Run(() => { ...

      //Build geometry
      Coordinate2D center = new Coordinate2D(4.5, 4);
      var eabCir = new EllipticArcBuilderEx(center, 0.5, ArcOrientation.ArcClockwise);
      var cir = eabCir.ToSegment();

      var poly = PolygonBuilderEx.CreatePolygon(
        PolylineBuilderEx.CreatePolyline(cir, AttributeFlags.AllAttributes));

      //Set symbolology, create and add element to layout
      CIMTextSymbol sym = SymbolFactory.Instance.ConstructTextSymbol(
                      ColorFactory.Instance.GreenRGB, 10, "Arial", "Regular");
      string text = "Circle, circle, circle";

      GraphicElement cirTxtElm = ElementFactory.Instance.CreateTextGraphicElement(
        container, TextType.CircleParagraph, poly, sym, text, "New Circle Text", false);

      #endregion
    }

    public static void CreateBezierTextElement(IElementContainer container)
    {
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateTextGraphicElement
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructTextSymbol
      // cref: ArcGIS.Desktop.Layouts.TextType
      // cref: ArcGIS.Core.Geometry.CubicBezierBuilderEx.#ctor(Coordinate2D,Coordinate2D,Coordinate2D,Coordinate2D,SpatialReference)
      // cref: ArcGIS.Core.Geometry.CubicBezierBuilderEx.ToSegment
      #region Create Bezier Text Element

      //Must be on QueuedTask.Run(() => { ...

      //Build geometry
      Coordinate2D pt1 = new Coordinate2D(3.5, 7.5);
      Coordinate2D pt2 = new Coordinate2D(4.16, 8);
      Coordinate2D pt3 = new Coordinate2D(4.83, 7.1);
      Coordinate2D pt4 = new Coordinate2D(5.5, 7.5);
      var bez = new CubicBezierBuilderEx(pt1, pt2, pt3, pt4);
      var bezSeg = bez.ToSegment();
      Polyline bezPl = PolylineBuilderEx.CreatePolyline(bezSeg, AttributeFlags.AllAttributes);

      //Set symbolology, create and add element to layout
      CIMTextSymbol sym = SymbolFactory.Instance.ConstructTextSymbol(
        ColorFactory.Instance.BlackRGB, 24, "Comic Sans MS", "Regular");

      var ge = ElementFactory.Instance.CreateTextGraphicElement(
        container, TextType.SplinedText, bezPl, sym, "this is the bezier text",
              "New Bezier Text", true, new ElementInfo() { Anchor = Anchor.CenterPoint });

      #endregion
    }

    public static void CreateEllipseTextElement(IElementContainer container)
    {
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateTextGraphicElement
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructTextSymbol
      // cref: ArcGIS.Desktop.Layouts.TextType
      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx.#ctor(Coordinate2D,Double,Double,Double,ArcOrientation,SpatialReference)
      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx.ToSegment
      // cref: ArcGIS.Core.Geometry.ArcOrientation
      #region Create Ellipse Text Element

      //Must be on QueuedTask.Run(() => { ...

      //Build geometry
      Coordinate2D center = new Coordinate2D(4.5, 2.75);
      var eabElp = new EllipticArcBuilderEx(center, 0, 1, 0.45, ArcOrientation.ArcClockwise);
      var ellipse = eabElp.ToSegment();

      var poly = PolygonBuilderEx.CreatePolygon(
        PolylineBuilderEx.CreatePolyline(ellipse, AttributeFlags.AllAttributes));

      //Set symbolology, create and add element to layout
      CIMTextSymbol sym = SymbolFactory.Instance.ConstructTextSymbol(
                            ColorFactory.Instance.BlueRGB, 10, "Arial", "Regular");
      string text = "Ellipse, ellipse, ellipse";

      GraphicElement ge = ElementFactory.Instance.CreateTextGraphicElement(
        container, TextType.PolygonParagraph, poly, sym, text, "New Ellipse Text", false);

      #endregion
    }

    #region ProSnippet Group: Create Predefined Shapes And Arrows
    #endregion

    public static void CreatePredefinedShapeElement(IElementContainer container)
    {
      PredefinedShape shapeType = PredefinedShape.Circle;

      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreatePredefinedShapeGraphicElement
      // cref: ArcGIS.Desktop.Layouts.PredefinedShape
      #region Create Predefined Shape Graphic Element

      //Must be on QueuedTask.Run(() => { ...

      //PredefinedShape shapeType =
      //              PredefinedShape.Circle | Cloud | Cross |Circle | Triangle | ... ;

      //Build geometry
      Coordinate2D ll = new Coordinate2D(4, 2.5);
      Coordinate2D ur = new Coordinate2D(6, 4.5);
      Envelope env = EnvelopeBuilderEx.CreateEnvelope(ll, ur);

      var outline = SymbolFactory.Instance.ConstructStroke(
                      ColorFactory.Instance.BlueRGB, 2);
      var poly_sym = SymbolFactory.Instance.ConstructPolygonSymbol(
                       null, outline);

      var ge = ElementFactory.Instance.CreatePredefinedShapeGraphicElement(
                             container, shapeType, env.Center, env.Width, env.Height, 
                              poly_sym, shapeType.ToString(), true);

      #endregion
    }

    public static void CreatePredefinedRoundedRectElement(IElementContainer container)
    {
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreatePredefinedShapeGraphicElement
      // cref: ArcGIS.Desktop.Layouts.PredefinedShape
      #region Create Predefined Shape Graphic Element

      //Must be on QueuedTask.Run(() => { ...

      //Build geometry
      Coordinate2D ll = new Coordinate2D(6.5, 7);
      Coordinate2D ur = new Coordinate2D(9, 9);
      Envelope env = EnvelopeBuilderEx.CreateEnvelope(ll, ur);

      var outline = SymbolFactory.Instance.ConstructStroke(
                      ColorFactory.Instance.GreenRGB, 2);
      var poly_sym = SymbolFactory.Instance.ConstructPolygonSymbol(
                       null, outline);

      var ge = ElementFactory.Instance.CreatePredefinedShapeGraphicElement(
        container, PredefinedShape.RoundedRectangle, env, poly_sym, "Rounded Rect", true);
      #endregion
    }

    public static void CreatePredefinedEllipseElement(IElementContainer container)
    {
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreatePredefinedShapeGraphicElement
      // cref: ArcGIS.Desktop.Layouts.PredefinedShape
      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx.#ctor(MapPoint,MapPoint,Double,Double,Double,MinorOrMajor,ArcOrientation,SpatialReference)
      // cref: ArcGIS.Core.Geometry.ArcOrientation
      #region Create Predefined Shape Graphic Element

      //Must be on QueuedTask.Run(() => { ...

      //Build geometry
      Coordinate2D center = new Coordinate2D(2, 2.75);
      var eabElp = new EllipticArcBuilderEx(
                               center, 0, 1, 0.45, ArcOrientation.ArcClockwise);
      var ellipse = eabElp.ToSegment();

      //Set symbolology, create and add element to layout
      CIMStroke outline = SymbolFactory.Instance.ConstructStroke(
                            ColorFactory.Instance.GreenRGB, 2.0, SimpleLineStyle.Dot);
      CIMPolygonSymbol ellipseSym = SymbolFactory.Instance.ConstructPolygonSymbol(
                       ColorFactory.Instance.GreyRGB, SimpleFillStyle.Vertical, outline);

      var poly = PolygonBuilderEx.CreatePolygon(
        PolylineBuilderEx.CreatePolyline(ellipse, AttributeFlags.AllAttributes));

      var ge = ElementFactory.Instance.CreatePredefinedShapeGraphicElement(
        container, PredefinedShape.Ellipse, poly.Extent.Center, 0, 0, ellipseSym, 
        "New Ellipse2", false, new ElementInfo() { Anchor = Anchor.TopRightCorner });
      #endregion
    }

    public static void CreateLineArrowElement(IElementContainer container)
    {

      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateArrowGraphicElement
      // cref: ArcGIS.Desktop.Layouts.ArrowInfo
      // cref: ArcGIS.Desktop.Layouts.ArrowInfo.ArrowHeadKey
      // cref: ArcGIS.Desktop.Layouts.ArrowInfo.ArrowOnBothEnds
      // cref: ArcGIS.Desktop.Layouts.ArrowInfo.ArrowSizePoints
      // cref: ArcGIS.Desktop.Layouts.ArrowInfo.LineWidthPoints
      // cref: ArcGIS.Desktop.Layouts.ElementInfo.Rotation
      #region Create Line Arrow Element

      //Must be on QueuedTask.Run(() => { ...

      //Build geometry
      List<Coordinate2D> plCoords = new List<Coordinate2D>();
      plCoords.Add(new Coordinate2D(1, 8.5));
      plCoords.Add(new Coordinate2D(1.66, 9));
      plCoords.Add(new Coordinate2D(2.33, 8.1));
      plCoords.Add(new Coordinate2D(3, 8.5));
      Polyline linePl = PolylineBuilderEx.CreatePolyline(plCoords);

      var arrowInfo = new ArrowInfo()
      {
        ArrowHeadKey = ArrowInfo.DefaultArrowHeadKeys[8],
        ArrowOnBothEnds = true,
        ArrowSizePoints = 24,
        LineWidthPoints = 12
      };

      //Create and add element to layout
      GraphicElement lineElm = ElementFactory.Instance.CreateArrowGraphicElement(
        container, linePl, arrowInfo, "Arrow Line", true, 
                                  new ElementInfo() { Rotation = 15.0 });
      //lineElm.SetName("New Line");

      #endregion
    }

    #region ProSnippet Group: Picture Elements
    #endregion

    public static void CreatePictureElement(Layout layout, string picPath)
    {
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateGraphicElement
      // cref: ArcGIS.Desktop.Layouts.IElementFactory.CreateGraphicElement
      // cref: ARCGIS.CORE.CIM.CIMPOINTGRAPHIC
      #region Create Picture Graphic Element using CIMSymbol

      //Must be on QueuedTask.Run(() => { ...

      //Build geometry
      Coordinate2D ll = new Coordinate2D(0.5, 1);
      Coordinate2D ur = new Coordinate2D(2.5, 2);
      Envelope env = EnvelopeBuilderEx.CreateEnvelope(ll, ur);

      //Create and add element to layout
      //string picPath = ApplicationUtilities.BASEPATH + _settings.baseFolder + "irefusetowatchthismovié.jpg";
      var pic_gr = ElementFactory.Instance.CreatePictureGraphicElement(
        layout, env.Center, picPath, "New Picture", true, new ElementInfo() { Anchor = Anchor.CenterPoint });
      #endregion
    }

    public async void snippets_CreateLayoutElements()
    {
      LayoutView layoutView = LayoutView.Active;
      Layout layout = layoutView.Layout;

      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreatePictureGraphicElement
      // cref: ArcGIS.Core.CIM.CIMPictureGraphic
      // cref: ArcGIS.Core.CIM.CIMPictureGraphic.Frame
      // cref: ArcGIS.Core.CIM.CIMGraphicFrame.BorderSymbol
      // cref: ArcGIS.Desktop.Layouts.GraphicElement.GetGraphic
      // cref: ArcGIS.Desktop.Layouts.GraphicElement.SetGraphic
      #region Create a new picture element with advanced symbol settings
      //Create a picture element and also set background and border symbology.

      //Construct on the worker thread
      await QueuedTask.Run(() =>
      {
        //Build 2D envelope geometry
        Coordinate2D pic_ll = new Coordinate2D(6, 1);
        Coordinate2D pic_ur = new Coordinate2D(8, 2);
        //At 2.x - Envelope env = EnvelopeBuilder.CreateEnvelope(pic_ll, pic_ur);
        Envelope env = EnvelopeBuilderEx.CreateEnvelope(pic_ll, pic_ur);

        //Create and add element to layout
        string picPath = @"C:\Temp\WhitePass.jpg";
        //At 2.x - GraphicElement picElm =
        //    LayoutElementFactory.Instance.CreatePictureGraphicElement(
        //                                             layout, env, picPath);
        //         picElm.SetName("New Picture");
        //
        GraphicElement picElm = ElementFactory.Instance.CreatePictureGraphicElement(
                                             layout, env, picPath, "New Picture");

        //(Optionally) Modify the border and shadow 
        CIMGraphic picGra = picElm.GetGraphic();
        CIMPictureGraphic cimPicGra = picGra as CIMPictureGraphic;
        cimPicGra.Frame.BorderSymbol = new CIMSymbolReference();
        cimPicGra.Frame.BorderSymbol.Symbol =
              SymbolFactory.Instance.ConstructLineSymbol(
                     ColorFactory.Instance.BlueRGB, 2.0, SimpleLineStyle.Solid);

        cimPicGra.Frame.ShadowSymbol = new CIMSymbolReference();
        cimPicGra.Frame.ShadowSymbol.Symbol =
                    SymbolFactory.Instance.ConstructPolygonSymbol(
                          ColorFactory.Instance.BlackRGB, SimpleFillStyle.Solid);

        //Update the element
        picElm.SetGraphic(picGra);
      });
      #endregion
    }

    #region ProSnippet Group: Create MapFrame and Surrounds
    #endregion

    public async void snippets_CreateLayoutElements2()
    {
      LayoutView layoutView = LayoutView.Active;
      Layout layout = layoutView.Layout;

      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.CreateEnvelope(Coordinate2D,Coordinate2D,SpatialReference)
      // cref: ArcGIS.Desktop.Mapping.MapProjectItem
      // cref: ArcGIS.Desktop.Mapping.MapProjectItem.GetMap
      // cref: ArcGIS.Desktop.Mapping.Map.GetBookmarks
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateMapFrameElement
      // cref: ArcGIS.Desktop.Layouts.MapFrame
      // cref: ArcGIS.Desktop.Layouts.MapFrame.SetCamera(BOOKMARK)
      #region Create Map Frame and Set Camera
      //Create a map frame and set its camera by zooming to the extent of an existing bookmark.

      //Construct on the worker thread
      await QueuedTask.Run(() =>
      {
        //Build 2D envelope geometry
        Coordinate2D mf_ll = new Coordinate2D(6.0, 8.5);
        Coordinate2D mf_ur = new Coordinate2D(8.0, 10.5);
        //At 2.x - Envelope mf_env = EnvelopeBuilder.CreateEnvelope(mf_ll, mf_ur);
        Envelope mf_env = EnvelopeBuilderEx.CreateEnvelope(mf_ll, mf_ur);

        //Reference map, create MF and add to layout
        MapProjectItem mapPrjItem = Project.Current.GetItems<MapProjectItem>()
                             .FirstOrDefault(item => item.Name.Equals("Map"));
        Map mfMap = mapPrjItem.GetMap();
        Bookmark bookmark = mfMap.GetBookmarks().FirstOrDefault(
                              b => b.Name == "Great Lakes");

        //At 2.x - MapFrame mfElm =
        //                  LayoutElementFactory.Instance.CreateMapFrame(
        //                                             layout, mf_env, mfMap);
        //         mfElm.SetName("New Map Frame");
        //
        MapFrame mfElm = ElementFactory.Instance.CreateMapFrameElement(
                             layout, mf_env, mfMap, "New Map Frame");

        //Zoom to bookmark
        mfElm.SetCamera(bookmark);
      });
      #endregion

      // cref: ArcGIS.Desktop.Layouts.Legend
      // cref: ArcGIS.Desktop.Layouts.LegendInfo
      // cref: ArcGIS.Desktop.Layouts.MapSurroundInfo.MapFrameName
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateMapSurroundElement
      #region Create Legend
      //Create a legend for an associated map frame.

      //Construct on the worker thread
      await QueuedTask.Run(() =>
      {
        //Build 2D envelope geometry
        Coordinate2D leg_ll = new Coordinate2D(6, 2.5);
        Coordinate2D leg_ur = new Coordinate2D(8, 4.5);
        //At 2.x - Envelope leg_env = EnvelopeBuilder.CreateEnvelope(leg_ll, leg_ur);
        Envelope leg_env = EnvelopeBuilderEx.CreateEnvelope(leg_ll, leg_ur);

        //Reference MF, create legend and add to layout
        MapFrame mapFrame = layout.FindElement("New Map Frame") as MapFrame;
        if (mapFrame == null)
        {
          //TODO handle null map frame
          return;
        }
        //At 2.x - Legend legendElm = LayoutElementFactory.Instance.CreateLegend(
        //                                               layout, leg_env, mapFrame);
        //         legendElm.SetName("New Legend"); 
        var legendInfo = new LegendInfo()
        {
          MapFrameName = mapFrame.Name
        };
        Legend legendElm = ElementFactory.Instance.CreateMapSurroundElement(
                            layout, leg_env, legendInfo, "New Legend") as Legend;
      });
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StyleProjectItem
      // cref: ArcGIS.Desktop.Mapping.ScaleBarStyleItem
      // cref: ARCGIS.DESKTOP.MAPPING.STYLEHELPER.SEARCHSCALEBARS
      // cref: ArcGIS.Desktop.Layouts.ScaleBarInfo
      // cref: ArcGIS.Desktop.Layouts.ScaleBarInfo.ScaleBarStyleItem
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateMapSurroundElement
      #region Create Scale Bar From StyleItem
      //Create a scale bar using a style.

      //Search for a style project item by name
      StyleProjectItem arcgis_2dStyle = Project.Current.GetItems<StyleProjectItem>()
                                  .First(si => si.Name == "ArcGIS 2D");

      //Construct on the worker thread
      await QueuedTask.Run(() =>
      {
        //Reference the specific scale bar by name 
        ScaleBarStyleItem scaleBarItem = arcgis_2dStyle.SearchScaleBars(
                           "Double Alternating Scale Bar").FirstOrDefault();

        //Reference the map frame and define the location
        MapFrame myMapFrame = layout.FindElement("Map Frame") as MapFrame;
        Coordinate2D coord2D = new Coordinate2D(10.0, 7.0);

        //Construct the scale bar
        //At 2.x - LayoutElementFactory.Instance.CreateScaleBar(
        //             layout, coord2D, myMapFrame, scaleBarItem);
        var sbarInfo = new ScaleBarInfo()
        {
          MapFrameName = myMapFrame.Name,
          ScaleBarStyleItem = scaleBarItem
        };
        ElementFactory.Instance.CreateMapSurroundElement(
                layout, coord2D.ToMapPoint(), sbarInfo);
      });
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StyleProjectItem
      // cref: ArcGIS.Desktop.Mapping.NorthArrowStyleItem
      // cref: ARCGIS.DESKTOP.MAPPING.STYLEHELPER.SearchNorthArrows
      // cref: ArcGIS.Desktop.Layouts.NorthArrowInfo
      // cref: ArcGIS.Desktop.Layouts.NorthArrowInfo.NorthArrowStyleItem
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateMapSurroundElement
      #region Create North Arrow From StyleItem 1
      //Create a north arrow using a style.

      //Search for a style project item by name
      StyleProjectItem arcgis2dStyles = Project.Current.GetItems<StyleProjectItem>()
                        .First(si => si.Name == "ArcGIS 2D");

      //Construct on the worker thread
      await QueuedTask.Run(() =>
      {
        NorthArrowStyleItem naStyleItem = arcgis2dStyles.SearchNorthArrows(
                      "ArcGIS North 13").FirstOrDefault();

        //Reference the map frame and define the location
        MapFrame newFrame = layout.FindElement("New Map Frame") as MapFrame;
        Coordinate2D nArrow = new Coordinate2D(6, 2.5);

        //Construct the north arrow
        //At 2.x - var newNorthArrow = LayoutElementFactory.Instance.CreateNorthArrow(
        //                        layout, nArrow, newFrame, naStyleItem);

        var naInfo = new NorthArrowInfo()
        {
          MapFrameName = newFrame.Name,
          NorthArrowStyleItem = naStyleItem
        };
        var newNorthArrow = ElementFactory.Instance.CreateMapSurroundElement(
                                layout, nArrow.ToMapPoint(), naInfo);
      });
      #endregion

      // cref: ArcGIS.Desktop.Layouts.TableFrameInfo
      // cref: ArcGIS.Desktop.Layouts.TableFrameInfo.FieldNames
      // cref: ArcGIS.Desktop.Layouts.MapSurroundInfo
      // cref: ArcGIS.Desktop.Layouts.MapSurroundInfo.MapFrameName
      // cref: ArcGIS.Desktop.Layouts.MapSurroundInfo.MapMemberUri
      // cref: ArcGIS.Desktop.Layouts.TableFrame
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateMapSurroundElement
      // cref: ArcGIS.Desktop.Mapping.Map.FindLayers
      #region Create Table Frame
      //Create a table frame.

      //Construct on the worker thread
      await QueuedTask.Run(() =>
      {
        //Build 2D envelope geometry
        Coordinate2D rec_ll = new Coordinate2D(1.0, 3.5);
        Coordinate2D rec_ur = new Coordinate2D(7.5, 4.5);
        //At 2.x - Envelope rec_env = EnvelopeBuilder.CreateEnvelope(rec_ll, rec_ur);
        Envelope rec_env = EnvelopeBuilderEx.CreateEnvelope(rec_ll, rec_ur);

        //Reference map frame and layer
        MapFrame mf = layout.FindElement("Map Frame") as MapFrame;
        FeatureLayer lyr = mf.Map.FindLayers("GreatLakes").First() as FeatureLayer;

        //Build fields list
        var fields = new[] { "NAME", "Shape_Area", "Shape_Length" };

        //Construct the table frame
        //At 2.x - TableFrame tabFrame = LayoutElementFactory.Instance.CreateTableFrame(
        //              layout, rec_env, mf, lyr, fields);

        var tableFrameInfo = new TableFrameInfo()
        {
          FieldNames = fields,
          MapFrameName = mf.Name,
          MapMemberUri = lyr.URI
        };
        var tabFrame = ElementFactory.Instance.CreateMapSurroundElement(
          layout, rec_env, tableFrameInfo) as TableFrame;
      });
      #endregion        
    }

    public static void CreateMapFrameElement(Layout layout, Map map)
    {
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateMapFrameElement
      // cref: ArcGIS.Desktop.Layouts.MapFrame
      #region Create Map Frame 1

      //Must be on QueuedTask.Run(() => { ...

      //Build geometry
      Coordinate2D ll = new Coordinate2D(2.0, 4.5);
      Coordinate2D ur = new Coordinate2D(4.0, 6.5);
      Envelope env = EnvelopeBuilderEx.CreateEnvelope(ll, ur);

      //Reference map, create MF and add to layout
      //var map = MapView.Active.Map;
      //var map = mapProjectItem.GetMap();
      //...

      MapFrame mfElm = ElementFactory.Instance.CreateMapFrameElement(
                                                      layout, env, map);
      #endregion
    }

    public static void CreateMapFrameElement2(Layout layout, Map map)
    {
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateMapFrameElement
      // cref: ArcGIS.Desktop.Layouts.MapFrame
      #region Create Map Frame 2

      //Must be on QueuedTask.Run(() => { ...

      //Build geometry
      Coordinate2D ll = new Coordinate2D(4.0, 2.5);
      Coordinate2D ur = new Coordinate2D(7.0, 5.5);
      Envelope env = EnvelopeBuilderEx.CreateEnvelope(ll, ur);

      //Reference map, create MF and add to layout
      //var map = MapView.Active.Map;
      //var map = mapProjectItem.GetMap();
      //...
      MapFrame mfElm = ElementFactory.Instance.CreateMapFrameElement(
        layout, env.Center, map);
      #endregion
    }

    public static void CreateLegendElement(Layout layout, string mapFrameName)
    {
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateMapSurroundElement
      // cref: ArcGIS.Desktop.Layouts.LegendInfo
      // cref: ArcGIS.Desktop.Layouts.MapSurroundInfo.MapFrameName
      // cref: ArcGIS.Desktop.Layouts.Element.SetName
      // cref: ArcGIS.Desktop.Layouts.Legend
      #region Create Legend 2

      //Must be on QueuedTask.Run(() => { ...

      //Build geometry
      Coordinate2D ll = new Coordinate2D(6, 2.5);
      Coordinate2D ur = new Coordinate2D(8, 4.5);
      Envelope env = EnvelopeBuilderEx.CreateEnvelope(ll, ur);

      //Reference MF, create legend and add to layout
      MapFrame mf = layout.FindElement(mapFrameName) as MapFrame;
      var surroundInfo = new LegendInfo()
      {
        MapFrameName = mf.Name
      };

      var legendElm = ElementFactory.Instance.CreateMapSurroundElement(
        layout, env.Center, surroundInfo) as Legend;
      legendElm.SetName("New Legend");
      #endregion
    }

    public static void CreateNorthArrowElement(Layout layout, string MapFrameName)
    {
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateMapSurroundElement
      // cref: ArcGIS.Desktop.Layouts.NorthArrowInfo
      // cref: ArcGIS.Desktop.Layouts.NorthArrowInfo.NorthArrowStyleItem
      // cref: ArcGIS.Desktop.Layouts.MapSurroundInfo.MapFrameName
      // cref: ArcGIS.Desktop.Layouts.Element.SetName
      // cref: ArcGIS.Desktop.Layouts.Element.SetHeight
      // cref: ArcGIS.Desktop.Layouts.Element.SetX
      // cref: ArcGIS.Desktop.Layouts.Element.SetY
      // cref: ArcGIS.Desktop.Layouts.NorthArrow
      // cref: ArcGIS.Desktop.Mapping.NorthArrowStyleItem
      // cref: ARCGIS.DESKTOP.MAPPING.STYLEHELPER.SearchNorthArrows
      #region Create North Arrow From StyleItem 2

      //Must be on QueuedTask.Run(() => { ...

      //Build geometry
      Coordinate2D center = new Coordinate2D(7, 5.5);

      //Reference a North Arrow in a style
      StyleProjectItem stylePrjItm = Project.Current.GetItems<StyleProjectItem>()
                                     .FirstOrDefault(item => item.Name == "ArcGIS 2D");
      NorthArrowStyleItem naStyleItm = stylePrjItm.SearchNorthArrows(
                                             "ArcGIS North 10")[0];

      //Reference MF, create north arrow and add to layout 
      //var mf = container.FindElement("New Map Frame") as MapFrame;
      var mf = layout.FindElement(MapFrameName) as MapFrame;
      var narrow_info = new NorthArrowInfo()
      {
        MapFrameName = mf.Name,
        NorthArrowStyleItem = naStyleItm
      };
      var arrowElm = (NorthArrow)ElementFactory.Instance.CreateMapSurroundElement(
        layout, center.ToMapPoint(), narrow_info) as NorthArrow;
      arrowElm.SetName("New North Arrow");
      arrowElm.SetHeight(1.75);
      arrowElm.SetX(7);
      arrowElm.SetY(6);
      #endregion
    }

    public static void CreateTableFrameElement(Layout layout, string mapFrameName, string uri)
    {
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateMapSurroundElement
      // cref: ArcGIS.Desktop.Layouts.TableFrameInfo
      // cref: ArcGIS.Desktop.Layouts.MapSurroundInfo.MapFrameName
      // cref: ArcGIS.Desktop.Layouts.MapSurroundInfo.MapMemberUri
      // cref: ArcGIS.Core.CIM.CIMStringMap.Key
      // cref: ArcGIS.Core.CIM.CIMStringMap.Value
      // cref: ArcGIS.Desktop.Layouts.ElementInfo.CustomProperties
      // cref: ArcGIS.Desktop.Layouts.TableFrame
      #region Create Table Frame

      //Must be on QueuedTask.Run(() => { ...

      //Build geometry
      Coordinate2D ll = new Coordinate2D(1, 1);
      Coordinate2D ur = new Coordinate2D(4, 4);
      Envelope env = EnvelopeBuilderEx.CreateEnvelope(ll, ur);

      var tableFrameInfo = new TableFrameInfo()
      {
        MapFrameName = mapFrameName,
        MapMemberUri = uri
      };
      var attribs = new List<CIMStringMap>();
      for (int i = 1; i < 6; i++)
      {
        attribs.Add(new CIMStringMap
        {
          Key = $"Key {i}",
          Value = $"Value {i}"
        });
      }
      var elemInfo = new ElementInfo()
      {
        CustomProperties = attribs
      };
      var tableFrameElem = ElementFactory.Instance.CreateMapSurroundElement(
                                    layout, env.Center, tableFrameInfo,
                                    "New Table Frame", false, elemInfo) as TableFrame;
      #endregion
    }

    public static void CreateScaleBar(IElementContainer layout, MapFrame mapFrame)
    {
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateMapSurroundElement
      // cref: ArcGIS.Desktop.Layouts.ScaleBarInfo
      // cref: ArcGIS.Desktop.Layouts.MapSurroundInfo.MapFrameName
      // cref: ArcGIS.Desktop.Mapping.StyleProjectItem
      // cref: ArcGIS.Desktop.Mapping.ScaleBarStyleItem
      // cref: ARCGIS.DESKTOP.MAPPING.STYLEHELPER.SEARCHSCALEBARS
      // cref: ArcGIS.Desktop.Layouts.ScaleBar
      #region Create Scale Bar

      //Must be on QueuedTask.Run(() => { ...

      //Build geometry
      Coordinate2D ll = new Coordinate2D(5.0, 6);
      Coordinate2D ur = new Coordinate2D(6.0, 7);
      Envelope sbEnv = EnvelopeBuilderEx.CreateEnvelope(ll, ur);

      //Reference a Scale Bar in a style
      StyleProjectItem stylePrjItm = Project.Current.GetItems<StyleProjectItem>()
                    .FirstOrDefault(item => item.Name == "ArcGIS 2D");
      ScaleBarStyleItem sbStyleItm = stylePrjItm.SearchScaleBars(
                   "Alternating Scale Bar 1")[0];
      //ScaleBarStyleItem sbStyleItm = stylePrjItm.SearchScaleBars(
      //                                   "Double Alternating Scale Bar 1")[0];
      //ScaleBarStyleItem sbStyleItm = stylePrjItm.SearchScaleBars(
      //                                    "Hollow Scale Bar 1")[0];

      //Create Scale Bar
      ScaleBarInfo sbInfo = new ScaleBarInfo()
      {
        MapFrameName = mapFrame.Name
      };

      var sbElm = ElementFactory.Instance.CreateMapSurroundElement(
                                         layout, sbEnv, sbInfo) as ScaleBar;
      #endregion
    }

    public static void CreateScaleLine(IElementContainer layout, MapFrame mapFrame)
    {
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateMapSurroundElement
      // cref: ArcGIS.Desktop.Layouts.ScaleBarInfo
      // cref: ArcGIS.Desktop.Layouts.ScaleBarInfo.ScaleBarStyleItem
      // cref: ArcGIS.Desktop.Layouts.MapSurroundInfo.MapFrameName
      // cref: ArcGIS.Desktop.Mapping.StyleProjectItem
      // cref: ArcGIS.Desktop.Mapping.ScaleBarStyleItem
      // cref: ARCGIS.DESKTOP.MAPPING.STYLEHELPER.SEARCHSCALEBARS
      // cref: ArcGIS.Desktop.Layouts.ScaleBar
      #region Create Scale Line

      //Must be on QueuedTask.Run(() => { ...

      //Build geometry
      Coordinate2D ll = new Coordinate2D(5.0, 8);
      Coordinate2D ur = new Coordinate2D(6.0, 9);
      Envelope sbEnv = EnvelopeBuilderEx.CreateEnvelope(ll, ur);

      //Reference a Scale Bar in a style
      StyleProjectItem stylePrjItm = Project.Current.GetItems<StyleProjectItem>()
                                   .FirstOrDefault(item => item.Name == "ArcGIS 2D");
      ScaleBarStyleItem sbStyleItm = stylePrjItm.SearchScaleBars(
                                    "Scale Line 1")[0];
      //ScaleBarStyleItem sbStyleItm = stylePrjItm.SearchScaleBars(
      //                                           "Stepped Scale Line")[0];
      //ScaleBarStyleItem sbStyleItm = stylePrjItm.SearchScaleBars(
      //                                            "Scale Line 2")[0];

      //Create Scale Bar
      ScaleBarInfo sbInfo = new ScaleBarInfo()
      {
        MapFrameName = mapFrame.Name,
        ScaleBarStyleItem = sbStyleItm
      };

      var sbElm = ElementFactory.Instance.CreateMapSurroundElement(
                        layout, sbEnv, sbInfo, "ScaleBar Line") as ScaleBar;
      #endregion
    }

    #region ProSnippet Group: Group Elements
    #endregion

    public async void snippets_CreateGroupElements()
    {
      LayoutView layoutView = LayoutView.Active;
      Layout container = layoutView.Layout;

      // cref: ArcGIS.Desktop.Layouts.GroupElement
      // cref: ArcGIS.Desktop.Layouts.Layout.FindElement
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateGroupElement
      #region Creating empty group elements
      //Create an empty group element at the root level of the contents pane

      //Create on worker thread
      await QueuedTask.Run(() =>
      {
        //At 2.x - GroupElement grp1 =
        //             LayoutElementFactory.Instance.CreateGroupElement(layout);
        //         grp1.SetName("Group"); 

        //container is IElementContainer - GroupLayer or Layout
        GroupElement grp1 = ElementFactory.Instance.CreateGroupElement(
                              container, null, "Group");
      });

      // *** or ***

      //Create a group element inside another group element

      //Find an existing group element
      //container is IElementContainer - GroupLayer or Layout
      GroupElement existingGroup = container.FindElement("Group") as GroupElement;

      //Create on worker thread
      await QueuedTask.Run(() =>
      {
        //At 2.x - GroupElement grp2 =
        //      LayoutElementFactory.Instance.CreateGroupElement(existingGroup);
        //         grp2.SetName("Group in Group");
        GroupElement grp2 = ElementFactory.Instance.CreateGroupElement(
          existingGroup, null, "Group in Group");
      });
      #endregion

      // cref: ArcGIS.Desktop.Layouts.GroupElement
      // cref: ArcGIS.Desktop.Layouts.Layout.FindElement(string)
      // cref: ArcGIS.Desktop.Layouts.Layout.FindElements(IEnumerable<string>)
      // cref: ArcGIS.Desktop.Mapping.IElementContainer.FindElement(string, bool)
      // cref: ArcGIS.Desktop.Layouts.Layout.FindElements
      // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateGroupElement
      #region Create a group element with elements
      //Create a group with a list of elements at the root level of the contents pane.

      //Find an existing elements
      //container is IElementContainer - GroupLayer or Layout
      var elem1 = container.FindElement("Polygon 1");
      var elem2 = container.FindElement("Bezier Text");
      var elem3 = container.FindElement("Cloud Shape 2");

      //Construct a list and add the elements
      var elmList = new List<Element>
      {
        elem1,
        elem2,
        elem3
      };

      //Perform on the worker thread
      await QueuedTask.Run(() =>
      {
        //At 2.x - GroupElement groupWithListOfElementsAtRoot =
        //             LayoutElementFactory.Instance.CreateGroupElement(layout, elmList);
        //groupWithListOfElementsAtRoot.SetName("Group with list of elements at root");
        //
        GroupElement groupWithListOfElementsAtRoot =
                ElementFactory.Instance.CreateGroupElement(
                         container, elmList, "Group with list of elements at root");
      });

      // *** or ***

      //Create a group using a list of element names at the root level of the contents pane.

      //List of element names
      var elmNameList = new[] { "Para Text1", "Line 3" };

      //Perform on the worker thread
      await QueuedTask.Run(() =>
      {
        //At 2.x - GroupElement groupWithListOfElementNamesAtRoot =
        //   LayoutElementFactory.Instance.CreateGroupElement(layout, elmNameList);
        //         groupWithListOfElementNamesAtRoot.SetName(
        //                  "Group with list of element names at root");

        //At 3.x, use the names to find the relevant elements first
        //container is IElementContainer - GroupLayer or Layout
        var elems = container.FindElements(elmNameList);
        GroupElement groupWithListOfElementNamesAtRoot =
            ElementFactory.Instance.CreateGroupElement(
                 container, elems, "Group with list of element names at root");
      });
      #endregion
    }

    #region ProSnippet Group: Layout Elements and Selection
    #endregion
    public void snippets_elements( Layout layout)
    {
      // cref: ArcGIS.Desktop.Layouts.Layout.FindElement
      // cref: ArcGIS.Desktop.Layouts.Layout.Elements
      #region Find an element on a layout
      //Find an element on a layout.

      // Reference a layout project item by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals("MyLayout"));
      if (layoutItem != null)
      {
        QueuedTask.Run(() =>
        {
          // Reference and load the layout associated with the layout item
          Layout mylayout = layoutItem.GetLayout();
          if (mylayout != null)
          {
            //Find a single specific element
            Element rect = mylayout.FindElement("Rectangle") as Element;

            //Or use the Elements collection
            Element rect2 = mylayout.Elements.FirstOrDefault(item => item.Name.Equals("Rectangle"));
          }
        });
      }
      #endregion

      QueuedTask.Run(() => {
        // cref: ArcGIS.Desktop.Layouts.Layout.FindElements
        // cref: ArcGIS.Desktop.Layouts.Layout.GetElements
        // cref: ArcGIS.Desktop.Layouts.Layout.GetElementsAsFlattenedList
        // cref: ArcGIS.Desktop.Layouts.GraphicElement.GetGraphic
        // cref: ArcGIS.Desktop.Layouts.GraphicElement
        #region Find layout elements
        //on the QueuedTask
        //Find elements by name
        var layoutElementsToFind = layout.FindElements(new List<string>() { "Point 1", "Line 3", "Text 1" });
        //Get the collection of elements from the page layout. Nesting within GroupElement is preserved.
        var elementCollection = layout.GetElements();
        //Get the collection of Element from the page layout as a flattened list. Nested groups within GroupElement are not preserved.
        var elements = layout.GetElementsAsFlattenedList();
        //Convert collection of the elements to a collection of GraphicElements.
        var graphicElements = elements.ToList().ConvertAll(x => (GraphicElement)x);
        //Find elements by type
        //Find all point graphics in the Layout
        var pointGraphics = graphicElements.Where(elem => elem.GetGraphic() is CIMPointGraphic);
        //Find all line graphics in the Graphics Layer
        var lineGraphics = graphicElements.Where(elem => elem.GetGraphic() is CIMLineGraphic);
        ////Find all polygon graphics in the Graphics Layer
        var polyGraphics = graphicElements.Where(elem => elem.GetGraphic() is CIMPolygonGraphic);
        ////Find all text graphics in the Graphics Layer
        var textGraphics = graphicElements.Where(elem => elem.GetGraphic() is CIMTextGraphic);
        ////Find all picture graphics in the Graphics Layer
        var pictureGraphic = graphicElements.Where(elem => elem.GetGraphic() is CIMPictureGraphic);
        #endregion
      });

      Element element = null;

      // cref: ArcGIS.Desktop.Layouts.Element.SetName
      // cref: ArcGIS.Desktop.Layouts.Element.SetVisible
      #region Update element properties
      //Update an element's properties.

      //Performed on worker thread
      QueuedTask.Run(() =>
      {
        // update an element's name
        element.SetName("New Name");

        // update and element's visibility
        element.SetVisible(true);
      });
      #endregion 
     {
        // cref: ArcGIS.Desktop.Layouts.LayoutView
        // cref: ArcGIS.Desktop.Layouts.LayoutView.GetSelectedElements
        #region Get element selection count
        //Get element's selection count.

        //Count the number of selected elements on the active layout view
        LayoutView activeLayoutView = LayoutView.Active;
        if (activeLayoutView != null)
        {
          var selectedElements = activeLayoutView.GetSelectedElements();
          ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show($@"Selected elements: {selectedElements.Count}");
        }
        #endregion
      }
      {
        // cref: ArcGIS.Desktop.Layouts.LayoutView
        // cref: ArcGIS.Desktop.Layouts.LayoutView.SelectElements
        #region Set element selection
        //Set the active layout view's selection to include 2 rectangle elements.

        //Reference the active view 
        LayoutView activeLayoutView = LayoutView.Active;
        if (activeLayoutView != null)
        {

          //Perform on the worker thread
          QueuedTask.Run(() =>
          {

            //Reference the layout
            Layout lyt = activeLayoutView.Layout;

            //Reference the two rectangle elements
            Element rec = lyt.FindElement("Rectangle");
            Element rec2 = lyt.FindElement("Rectangle 2");

            //Construct a list and add the elements
            List<Element> elmList = new List<Element>
            {
              rec,
              rec2
            };

            //Set the selection
            activeLayoutView.SelectElements(elmList);
          });
        }
        #endregion
      }

      // cref: ArcGIS.Desktop.Layouts.Layout.FindElements
      // cref: ArcGIS.Desktop.Layouts.Layout.UnSelectElement
      // cref: ArcGIS.Desktop.Layouts.Layout.UnSelectElements
      #region UnSelect elements on the Layout
      //Unselect one element.
      var elementToUnSelect = layout.FindElements(new List<string>() { "MyPoint" }).FirstOrDefault();
      layout.UnSelectElement(elementToUnSelect);
      //Unselect multiple elements.
      var elementsToUnSelect = layout.FindElements(new List<string>() { "Point 1", "Line 3", "Text 1" });
      layout.UnSelectElements(elementsToUnSelect);
      #endregion

      // cref: ArcGIS.Desktop.Layouts.LayoutView.Active
      // cref: ArcGIS.Desktop.Layouts.Layout.FindElements
      // cref: ArcGIS.Desktop.Layouts.LayoutView.UnSelectElement
      // cref: ArcGIS.Desktop.Layouts.LayoutView.UnSelectElements
      #region UnSelect elements on the LayoutView
      LayoutView layoutView = LayoutView.Active;
      //Unselect one element.
      var elementToUnSelectInView = layout.FindElements(new List<string>() { "MyPoint" }).FirstOrDefault();
      layoutView.UnSelectElement(elementToUnSelect);
      //Unselect multiple elements.
      var elementsToUnSelectInView = layout.FindElements(new List<string>() { "Point 1", "Line 3", "Text 1" });
      layoutView.UnSelectElements(elementsToUnSelect);
      #endregion

      {
        // cref: ArcGIS.Desktop.Layouts.LayoutView.ClearElementSelection
        #region Clear the selection in a layout view
        //If the a layout view is active, clear its selection
        LayoutView activeLayoutView = LayoutView.Active;
        if (activeLayoutView != null)
        {          
          activeLayoutView.ClearElementSelection();
        }
        #endregion

        // cref: ArcGIS.Desktop.Layouts.Layout.ClearElementSelection
        #region Clear the selection in a layout 
        //Clear the layout selection.
        layout.ClearElementSelection();
        #endregion
      }
      Layout aLayout = null;
      Element elm = null;

      // cref: ArcGIS.Desktop.Layouts.Layout.CopyElements
      #region Copy Layout Elements
      //on the QueuedTask
      var elems = layout.FindElements(new List<string>() { "Point 1", "Line 3", "Text 1" });
      var copiedElements = layout.CopyElements(elems);
      #endregion

      // cref: ArcGIS.Desktop.Layouts.Layout.GetSelectedElements
      // cref: ArcGIS.Desktop.Layouts.Layout.DeleteElements
      #region Delete Layout Elements
      //on the QueuedTask  
      var elementsToRemove = layout.GetSelectedElements();
      layout.DeleteElements(elementsToRemove);
      #endregion

      // cref: ArcGIS.Desktop.Layouts.Layout.DeleteElement
      // cref: ArcGIS.Desktop.Layouts.Layout.DeleteElements
      #region Delete an element or elements on a layout
      //Delete an element or elements on a layout.

      //Perform on the worker thread
      QueuedTask.Run(() =>
      {
        //Delete a specific element on a layout
        aLayout.DeleteElement(elm);
       
        //Or delete a group of elements using a filter
        aLayout.DeleteElements(item => item.Name.Contains("Clone"));

        //Or delete all elements on a layout
        aLayout.DeleteElements(item => true);
      });
      #endregion

      // cref: ArcGIS.Desktop.Layouts.Layout.FindElements
      // cref: ArcGIS.Desktop.Layouts.LayoutView.ZoomToElements
      #region Zoom to elements
      LayoutView lytView = LayoutView.Active;
      //Zoom to an element
      var elementToZoomTo = layout.FindElements(new List<string>() { "MyPoint" }).FirstOrDefault();
      lytView.ZoomToElement(elementToZoomTo);
      //Zoom to  multiple elements.
      var elementsToZoomTo = layout.FindElements(new List<string>() { "Point 1", "Line 3", "Text 1" });
      lytView.ZoomToElements(elementsToZoomTo);

      #endregion

      // cref: ArcGIS.Desktop.Layouts.Element.GetDefinition
      // cref: ArcGIS.Desktop.Layouts.Element.SetDefinition
      // cref: ArcGIS.Desktop.Layouts.LayoutView.GetSelectedElements
      // cref: ArcGIS.Core.CIM.CIMMarkerNorthArrow
      // cref: ArcGIS.Core.CIM.CIMMarkerNorthArrow.PointSymbol
      // cref: ArcGIS.Core.CIM.CIMPointSymbol.HaloSymbol
      // cref: ArcGIS.Core.CIM.CIMPointSymbol.HaloSize
      #region Set halo property of north arrow
      //Set the CIM halo properties of a north arrow.

      //Reference the first selected element (assumption is it is a north arrow)
      Element northArrow = LayoutView.Active.GetSelectedElements().First();

      //Perform on the worker thread
      QueuedTask.Run(() =>
      {
        //Get definition of north arrow...
        var cim = northArrow.GetDefinition() as CIMMarkerNorthArrow;
        
        //this halo symbol is 50% transparent, no outline (i.e. 0 width)
        //First construct a polygon symbol to use in the Halo
        //Polygon symbol will need a fill and a stroke
        var polyFill = SymbolFactory.Instance.ConstructSolidFill(ColorFactory.Instance.CreateRGBColor(0, 0, 0, 50));
        var polyStroke = SymbolFactory.Instance.ConstructStroke(ColorFactory.Instance.BlackRGB, 0);
        var haloPoly = SymbolFactory.Instance.ConstructPolygonSymbol(polyFill, polyStroke);
        
        //Set the north arrow defintion of HaloSymbol and HaloSize 
        ((CIMPointSymbol)cim.PointSymbol.Symbol).HaloSymbol = haloPoly;
        ((CIMPointSymbol)cim.PointSymbol.Symbol).HaloSize = 3;//size of the halo
          
        //Apply the CIM changes back to the element
        northArrow.SetDefinition(cim);
      });
      #endregion
    }

    private void groupingElements(Layout layout)
    {
      #region ProSnippet Group: Grouping and Ordering Graphic Elements
      #endregion
      QueuedTask.Run(() => {

        // cref: ArcGIS.Desktop.Layouts.Layout.GroupElements
        // cref: ArcGIS.Desktop.Layouts.Layout.GetSelectedElements
        #region Group Graphic Elements
        //on the QueuedTask
        var elemsToGroup = layout.GetSelectedElements();
        //Note: run within the QueuedTask
        //group  elements
        var groupElement = layout.GroupElements(elemsToGroup);
        #endregion

        // cref: ArcGIS.Desktop.Layouts.Layout.UnGroupElement
        // cref: ArcGIS.Desktop.Layouts.Layout.UnGroupElements
        #region Un-Group Graphic Elements
        var selectedElements = layout.GetSelectedElements().ToList(); 
        if (selectedElements?.Any() == false)//must be at least 1.
          return;
        var elementsToUnGroup = new List<GroupElement>();
        //All selected elements should be grouped elements.
        if (selectedElements.Count() == selectedElements.OfType<GroupElement>().Count())
        {
          //Convert to a GroupElement list.
          elementsToUnGroup = selectedElements.ConvertAll(x => (GroupElement)x);
        }
        if (elementsToUnGroup.Count() == 0)
          return;
        //UnGroup many grouped elements
        layout.UnGroupElements(elementsToUnGroup);
        //Ungroup one grouped element
        layout.UnGroupElement(elementsToUnGroup.FirstOrDefault());
        #endregion

        // cref: ArcGIS.Desktop.Layouts.GroupElement.Elements
        // cref: ArcGIS.Desktop.Layouts.Element.GetParent
        #region Parent of GroupElement
        //check the parent
        var parent = groupElement.Elements.First().GetParent();//will be the group element
        //top-most parent
        //will be a GraphicsLayer or Layout
        var top_most = groupElement.Elements.First().GetParent(true);
        #endregion

        // cref: ArcGIS.Desktop.Layouts.GroupElement.GetElementsAsFlattenedList
        #region Children in a Group Element
        // Nested groups within ArcGIS.Desktop.Layouts.GroupElement are not preserved.
        var children = groupElement.GetElementsAsFlattenedList();
        #endregion

        // cref: ArcGIS.Desktop.Layouts.Layout.GetSelectedElements
        // cref: ArcGIS.Desktop.Layouts.Layout.CanBringForward
        // cref: ArcGIS.Desktop.Layouts.Layout.BringForward
        // cref: ArcGIS.Desktop.Layouts.Layout.CanSendBackward
        // cref: ArcGIS.Desktop.Layouts.Layout.SendBackward
        #region Ordering: Send backward and Bring forward
        //On the QueuedTask
        //get the current selection set
        var sel_elems = layout.GetSelectedElements();
        //can they be brought forward? This will also check that all elements have the same parent
        if (layout.CanBringForward(sel_elems))
        {
          //bring forward
          layout.BringForward(sel_elems);
          //bring to front (of parent)
          //graphicsLayer.BringToFront(sel_elems);
        }
        else if (layout.CanSendBackward(sel_elems))
        {
          //send back
          layout.SendBackward(sel_elems);
          //send to the back (of parent)
          //graphicsLayer.SendToBack(sel_elems);
        }
        #endregion

        // cref: ArcGIS.Desktop.Layouts.Layout.GetSelectedElements
        // cref: ArcGIS.Desktop.Layouts.Element.ZOrder
        #region Get Z-Order
        var selElementsZOrder = layout.GetSelectedElements();
        //list out the z order
        foreach (var elem in selElementsZOrder)
          //At 2.x - System.Diagnostics.Debug.WriteLine($"{elem.Name}: z-order {elem.GetZOrder()}");
          System.Diagnostics.Debug.WriteLine($"{elem.Name}: z-order {elem.ZOrder}");
        #endregion
      });
    }

    #region ProSnippet Group: Update Layout Elements
    #endregion

    public void snippets_UpdateElements()
    {
      double x = 0;
      double y = 0;

      // cref: ArcGIS.Desktop.Layouts.TextElement.SetTextProperties(ArcGIS.Desktop.Layouts.TextProperties)
      // cref: ArcGIS.Desktop.Layouts.TextProperties.#ctor(System.String,System.String,System.Double,System.String)
      // cref: ArcGIS.Desktop.Layouts.TextElement
      // cref: ArcGIS.Desktop.Layouts.Element.SetX
      // cref: ArcGIS.Desktop.Layouts.Element.SetY
      // cref: ArcGIS.Desktop.Layouts.Element.SetAnchor
      // cref: ArcGIS.Core.CIM.Anchor
      #region Update text element properties
      //Update text element properties for an existing text element.

      // Reference a layoutitem in a project by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>()
                            .FirstOrDefault(item => item.Name.Equals("MyLayout"));
      if (layoutItem != null)
      {

        //Perform on the worker thread
        QueuedTask.Run(() =>
        {
          // Reference and load the layout associated with the layout item
          Layout layout = layoutItem.GetLayout();
          if (layout != null)
          {
            // Reference a text element by name
            TextElement txtElm = layout.FindElement("MyTextElement") as TextElement;
            if (txtElm != null)
            {
              // Change placement properties
              txtElm.SetAnchor(Anchor.CenterPoint);
              txtElm.SetX(x);
              txtElm.SetY(y);

              // Change TextProperties
              TextProperties txtProperties = new TextProperties(
                               "Hello world", "Times New Roman", 48, "Regular");
              txtElm.SetTextProperties(txtProperties);
            }
          }
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Layouts.PictureElement
      // cref: ArcGIS.Desktop.Layouts.PictureElement.SetSourcePath
      #region Update a picture element
      //Update a picture element.

      //Perform on the worker thread
      QueuedTask.Run(() =>
      {
        // Reference and load the layout associated with the layout item
        Layout layout = layoutItem.GetLayout();
        if (layout != null)
        {
          // Reference a picture element by name
          PictureElement picElm = layout.FindElement("MyPicture") as PictureElement;
          // Change the path to a new source
          if (picElm != null)
            picElm.SetSourcePath(@"D:\MyData\Pics\somePic.jpg");
        }
      });
      #endregion

      // cref: ArcGIS.Desktop.Layouts.Element.GETDEFINITION
      // cref: ArcGIS.Desktop.Layouts.Element.SETDEFINITION
      // cref: ArcGIS.Core.CIM.CIMMapFrame
      // cref: ArcGIS.Core.CIM.CIMFrameElement.GraphicFrame
      // cref: ArcGIS.Core.CIM.CIMGraphicFrame.BackgroundSymbol
      #region Apply a Background Color to a MapFrame
      //Apply a background color to the map frame element using the CIM.

      //Perform on the worker thread
      QueuedTask.Run(() =>
      {
        //Get the layout
        var myLayout = Project.Current.GetItems<LayoutProjectItem>()?.First().GetLayout();
        if (myLayout == null) return;

        //Get the map frame in the layout
        MapFrame mapFrame = myLayout.FindElement("New Map Frame") as MapFrame;
        if (mapFrame == null)
        {
          //TODO Handle null mapframe
          return;
        }

        //Get the map frame's definition in order to modify the background.
        var mapFrameDefn = mapFrame.GetDefinition() as CIMMapFrame;

        //Construct the polygon symbol to use to create a background
        var polySymbol = SymbolFactory.Instance.ConstructPolygonSymbol(
                       ColorFactory.Instance.BlueRGB, SimpleFillStyle.Solid);

        //Set the background
        mapFrameDefn.GraphicFrame.BackgroundSymbol =
                                         polySymbol.MakeSymbolReference();

        //Set the map frame definition
        mapFrame.SetDefinition(mapFrameDefn);
      });
      #endregion

      // cref: ArcGIS.Desktop.Layouts.MapSurround
      // cref: ArcGIS.Desktop.Layouts.ScaleBar
      // cref: ArcGIS.Desktop.Layouts.MapSurround.SetMapFrame
      #region Update a map surround
      //Update a map surround.

      //Perform on the worker thread
      QueuedTask.Run(() =>
      {
        // Reference and load the layout associated with the layout item
        Layout layout = layoutItem.GetLayout();
        if (layout != null)
        {
          // Reference a scale bar element by name
          MapSurround scaleBar = layout.FindElement("MyScaleBar") as MapSurround;

          // Reference a map frame element by name
          MapFrame mf = layout.FindElement("MyMapFrame") as MapFrame;

          if ((scaleBar != null) && (mf != null))
            //Set the scale bar to the newly referenced map frame
            scaleBar.SetMapFrame(mf);
        }
      });
      #endregion

      // cref: ArcGIS.Desktop.Layouts.Element.GetDefinition
      // cref: ArcGIS.Desktop.Layouts.Element.SetDefinition
      // cref: ArcGIS.Core.CIM.CIMElement
      // cref: ArcGIS.Core.CIM.CIMElement.Locked
      #region Lock an element
      // The Locked property is displayed in the TOC as a lock symbol next
      // to each element.  If locked the element can't be selected in the layout
      // using the graphic selection tools.

      //Perform on the worker thread
      QueuedTask.Run(() =>
      {
        // Reference and load the layout associated with the layout item
        Layout layout = layoutItem.GetLayout();
        if (layout != null)
        {
          //Reference an element by name
          Element element = layout.FindElement("MyElement");
          if (element != null)
          {
            // Modify the Locked property via the CIM
            CIMElement CIMElement = element.GetDefinition() as CIMElement;
            CIMElement.Locked = true;
            element.SetDefinition(CIMElement);
          }
        }
      });

      #endregion

      // cref: ArcGIS.Desktop.Layouts.GraphicElement.GetGraphic
      // cref: ArcGIS.Desktop.Layouts.GraphicElement.SetGraphic
      // cref: ArcGIS.Core.CIM.CIMGraphic
      // cref: ArcGIS.Core.CIM.CIMGraphic.Transparency
      #region Update an elements transparency
      //Update an element's transparency using the CIM.

      //Perform on the worker thread
      QueuedTask.Run(() =>
      {
        // Reference and load the layout associated with the layout item
        Layout layout = layoutItem.GetLayout();
        if (layout != null)
        {
          // Reference a element by name
          GraphicElement graphicElement = layout.FindElement("MyElement") as GraphicElement;
          if (graphicElement != null)
          {
            // Modify the Transparency property that exists only in the CIMGraphic class.
            CIMGraphic CIMGraphic = graphicElement.GetGraphic() as CIMGraphic;
            CIMGraphic.Transparency = 50; // mark it 50% transparent
            graphicElement.SetGraphic(CIMGraphic);
          }
        }
      });
      #endregion

      double xOffset = 0;
      double yOffset = 0;

      // cref: ArcGIS.Desktop.Layouts.GraphicElement.Clone
      // cref: ArcGIS.Desktop.Layouts.Element.SetX
      // cref: ArcGIS.Desktop.Layouts.Element.SetY
      // cref: ArcGIS.Desktop.Layouts.Element.GetX
      // cref: ArcGIS.Desktop.Layouts.Element.GetY
      #region Clone an element
      //Clone a layout graphic element and apply an offset.

      //Perform on the worker thread
      QueuedTask.Run(() =>
      {
        // Reference and load the layout associated with the layout item
        Layout layout = layoutItem.GetLayout();
        if (layout != null)
        {
          // Reference a graphic element by name
          GraphicElement graphicElement = 
                              layout.FindElement("MyElement") as GraphicElement;
          if (graphicElement != null)
          {

            //Clone and set the new x,y
            GraphicElement cloneElement = graphicElement.Clone("Clone");
            cloneElement.SetX(cloneElement.GetX() + xOffset);
            cloneElement.SetY(cloneElement.GetY() + yOffset);
          }
        }
      });
      #endregion
    }

    #region ProSnippet Group: Style Layout Elements
    #endregion
    public void ApplyNorthArrowStyle()
    {
      var layout = LayoutView.Active?.Layout;
      if (layout == null) return;
      QueuedTask.Run(() => {
        // cref: ArcGIS.Desktop.Layouts.Element.CanApplyStyle 
        // cref: ArcGIS.Desktop.Layouts.Element.ApplyStyle
        #region Apply a style to a North Arrow
        //Run within QueuedTask context.
        //Get the Style project items in the project
        var styleProjectItems = Project.Current?.GetItems<StyleProjectItem>();
        //Get the ArcGIS 2D Style Project Item
        var styleProjectItem = 
        styleProjectItems.FirstOrDefault(s => s.Name == "ArcGIS 2D");
        if (styleProjectItem == null) return;
        //Get the north arrow style item you need
        var northArrowStyleItem = 
        styleProjectItem.SearchSymbols(StyleItemType.NorthArrow, "ArcGIS North 18").FirstOrDefault();
        if (northArrowStyleItem == null) return;
        //Select a North arrow layout element
        var northArrowElement = layout.GetSelectedElements().OfType<NorthArrow>().FirstOrDefault();
        if (northArrowElement != null)
        {
          //Check if the input style can be applied to the element
          if (northArrowElement.CanApplyStyle(northArrowStyleItem))
            //Apply the style
            northArrowElement.ApplyStyle(northArrowStyleItem);
        }
        #endregion
      });
    }

    public void ApplyGridAndGraticulesStyle()
    {
      var layout = LayoutView.Active?.Layout;
      if (layout == null) return;
      QueuedTask.Run(() => {
        // cref: ArcGIS.Core.CIM.CIMGraticule 
        // cref: ArcGIS.Core.CIM.CIMMapGrid
        // cref: ArcGIS.Core.CIM.CIMMapGrid
        #region Apply a style to Grid and Graticules
        //Run within QueuedTask context.
        //Get the Style project items in the project
        var styleProjectItems = Project.Current?.GetItems<StyleProjectItem>();
        //Get the ArcGIS 2D Style Project Item
        var styleProjectItem = 
        styleProjectItems.OfType<StyleProjectItem>().FirstOrDefault(s => s.Name == "ArcGIS 2D");
        if (styleProjectItem == null) return;
        //Get the grid style item you need
        var gridStyleItem = 
        styleProjectItem.SearchSymbols(StyleItemType.Grid, "Blue Vertical Label Graticule").FirstOrDefault();
        if (gridStyleItem == null) return;
        var symbolItemName = gridStyleItem.Name;
        var girdGraticuleObject = gridStyleItem.GetObject() as CIMMapGrid;

        var mapFrame = layout.GetElements().OfType<MapFrame>().FirstOrDefault();
        var cmf = mapFrame.GetDefinition() as CIMMapFrame;
        //note, if page units are _not_ inches then grid's gridline
        //lengths and offsets would need to be converted to the page units
        var mapGrids = new List<CIMMapGrid>();
        if (cmf.Grids != null)
          mapGrids.AddRange(cmf.Grids);

        //var cimMapGrid = SymbolStyleItem.GetObject() as CIMMapGrid;

        switch (girdGraticuleObject)
        {
          case CIMGraticule:
            var gridGraticule = girdGraticuleObject as CIMGraticule;
            gridGraticule.Name = symbolItemName;
            gridGraticule.SetGeographicCoordinateSystem(mapFrame.Map.SpatialReference);
            //assign grid to the frame             
            mapGrids.Add(gridGraticule);

            break;
          case CIMMeasuredGrid:
            var gridMeasure = girdGraticuleObject as CIMMeasuredGrid;
            gridMeasure.Name = symbolItemName;
            gridMeasure.SetProjectedCoordinateSystem(mapFrame.Map.SpatialReference);
            //assign grid to the frame
            mapGrids.Add(gridMeasure);

            break;
          case CIMReferenceGrid:
            var gridReference = girdGraticuleObject as CIMReferenceGrid;
            gridReference.Name = symbolItemName;
            //assign grid to the frame
            mapGrids.Add(gridReference);
            break;
        }

        cmf.Grids = mapGrids.ToArray();
        mapFrame.SetDefinition(cmf);
        #endregion
      });

    }

    public void ApplyStyleToGraphicElements()
    {
      var layout = LayoutView.Active?.Layout;
      if (layout == null) return;
      QueuedTask.Run(() => {
        // cref: ArcGIS.Desktop.Layouts.GraphicElement.CanApplyStyle 
        // cref: ArcGIS.Desktop.Layouts.GraphicElement.ApplyStyle
        #region Apply a style to a Graphic Element
        //Run within QueuedTask context.
        //Get the Style project items in the project
        var styleProjectItems = Project.Current?.GetItems<StyleProjectItem>();
        //Get the ArcGIS 2D Style Project Item
        var styleProjectItem = 
        styleProjectItems.OfType<StyleProjectItem>().FirstOrDefault(s => s.Name == "ArcGIS 2D");
        if (styleProjectItem == null) return;
        //Get the north arrow style item you need
        var pointStyleItem = 
        styleProjectItem.SearchSymbols(StyleItemType.PointSymbol, "Circle 3").FirstOrDefault();
        if (pointStyleItem == null) return;
        //Select a North arrow layout element
        var layoutPointElement = layout.GetSelectedElements().FirstOrDefault();
        if (layoutPointElement != null && layoutPointElement is GraphicElement ge)
        {
          if (layoutPointElement.CanApplyStyle(pointStyleItem))
          {
            //The magic happens here
            //for Graphic Elements such as Point, Lines, Polys, text, preserve size.           
            ge.ApplyStyle(pointStyleItem, true); 
          }
        }
        #endregion
      });
    }

    #region ProSnippet Group: Layout Snapping
    #endregion

    private void LayoutSnapping()
    {

      // cref: ArcGIS.Desktop.Layouts.LayoutSnapping.IsEnabled
      #region Configure Snapping - Turn Snapping on or off

      //enable snapping
      ArcGIS.Desktop.Layouts.LayoutSnapping.IsEnabled = true;

      // disable snapping
      ArcGIS.Desktop.Layouts.LayoutSnapping.IsEnabled = false;
      #endregion

      // cref: ArcGIS.Desktop.Layouts.LayoutSnapping.SetSnapModes(System.Collections.Generic.IEnumerable<ArcGIS.Desktop.Layouts.LayoutSnapMode>)
      // cref: ArcGIS.Desktop.Layouts.LayoutSnapping.SetSnapMode(ArcGIS.Desktop.Layouts.LayoutSnapMode, System.Boolean)
      // cref: ArcGIS.Desktop.Layouts.LayoutSnapping.SnapModes
      // cref: ArcGIS.Desktop.Layouts.LayoutSnapping.GetSnapMode(ArcGIS.Desktop.Layouts.LayoutSnapMode)
      // cref: ArcGIS.Desktop.Layouts.LayoutSnapMode
      #region Configure Snapping - Application SnapModes

      // sets only the Guide snapping mode 
      ArcGIS.Desktop.Layouts.LayoutSnapping.SetSnapModes(new[] { LayoutSnapMode.Guide });
      // sets only Element and Page snapping modes
      ArcGIS.Desktop.Layouts.LayoutSnapping.SetSnapModes(new[] { LayoutSnapMode.Element, LayoutSnapMode.Page }); 

      // clear all snap modes
      ArcGIS.Desktop.Layouts.LayoutSnapping.SetSnapModes(null);


      // set snap modes one at a time
      ArcGIS.Desktop.Layouts.LayoutSnapping.SetSnapMode(LayoutSnapMode.Margins, true);
      ArcGIS.Desktop.Layouts.LayoutSnapping.SetSnapMode(LayoutSnapMode.Guide, true);
      /// LayoutSnapping.SetSnapModes(new[]{ LayoutSnapMode.Guide }); // sets only the Guide snapping mode 

      // get current snap modes
      var snapModes = ArcGIS.Desktop.Layouts.LayoutSnapping.SnapModes;

      // get state of a specific snap mode
      bool isOn = ArcGIS.Desktop.Layouts.LayoutSnapping.GetSnapMode(LayoutSnapMode.Guide);

      #endregion

    }

    #region ProSnippet Group: Layout Metadata
    #endregion

    private void LayoutMetadata(Layout layout)
    {
      // cref: ArcGIS.Desktop.Layouts.Layout.GetMetadata
      // cref: ArcGIS.Desktop.Layouts.Layout.GetCanEditMetadata
      // cref: ArcGIS.Desktop.Layouts.Layout.SetMetadata
      #region Layout Metadata
      //var layout = ...;
      //Must be on the QueuedTask.Run()

      //Gets the Layout metadata.
      var layout_xml = layout.GetMetadata();
      //Can metadata be edited?
      if (layout.GetCanEditMetadata())
        //Set the metadata back
        layout.SetMetadata(layout_xml);

      #endregion
    }

    #region ProSnippet Group: Layout MapFrame
    #endregion

    async public void snippets_MapFrame()
    {
      Layout layout = LayoutView.Active.Layout;

      // cref: ArcGIS.Desktop.Layouts.MapFrame.SetMap(ArcGIS.Desktop.Mapping.Map)
      #region Change the map associated with a map frame
      //Change the map associated with a map frame

      //Reference a map frame on a layout
      MapFrame mfrm = layout.FindElement("Map Frame") as MapFrame;

      //Peform on worker thread
      await QueuedTask.Run(() =>
      {
        //Reference map from the project item 
        Map map = Project.Current.GetItems<MapProjectItem>().FirstOrDefault(m => m.Name.Equals("Map1")).GetMap();

        //Set the map to the map frame
        mfrm.SetMap(map);
      });

      #endregion

      // cref: ArcGIS.Desktop.Layouts.MapFrame.SetCamera(ArcGIS.Desktop.Mapping.Camera)
      // cref: ArcGIS.Desktop.Mapping.Camera.Scale
      #region Change map frame camera settings
      //Change a map frame's camera settings.

      //Perform on the worker thread
      await QueuedTask.Run(() =>
      {
        //Reference MapFrame
        MapFrame mf = layout.FindElement("Map Frame") as MapFrame;

        //Reference the camera associated with the map frame and change the scale
        Camera cam = mf.Camera;
        cam.Scale = 100000;

        //Set the map frame extent based on the new camera info
        mf.SetCamera(cam);
      });
      #endregion

      // cref: ArcGIS.Desktop.Layouts.MapFrame.SetCamera(ArcGIS.Desktop.Mapping.Layer,System.Boolean)
      // cref: ArcGIS.Desktop.Layouts.MapFrame.Map
      // cref: ArcGIS.Desktop.Mapping.Map.FindLayers
      #region Zoom map frame to extent of a single layer
      //Zoom map frame to the extent of a single layer.

      //Perform on the worker thread
      await QueuedTask.Run(() =>
      {
        //Reference MapFrame
        MapFrame mf = layout.FindElement("Map Frame") as MapFrame;

        //Reference map and layer
        Map m = mf.Map;
        FeatureLayer lyr = m.FindLayers("GreatLakes").First() as FeatureLayer;

        //Set the map frame extent to all features in the layer
        mf.SetCamera(lyr, false);
      });
      #endregion

      // cref: ArcGIS.Desktop.Layouts.MapFrame.SetCamera(System.Collections.Generic.IEnumerable{ArcGIS.Desktop.Mapping.Layer},System.Boolean)
      // cref: ArcGIS.Desktop.Mapping.Map.FindLayers
      #region Change map frame extent to selected features in multiple layers
      //Change the extent of a map frame to the selected features multiple layers.

      //Perform on the worker thread
      await QueuedTask.Run(() =>
      {
        //Reference MapFrame
        MapFrame mf = layout.FindElement("Map Frame") as MapFrame;

        //Reference map, layers and create layer list
        Map m = mf.Map;
        FeatureLayer fl_1 = m.FindLayers("GreatLakes").First() as FeatureLayer;
        FeatureLayer fl_2 = m.FindLayers("States_WithRegions").First() as FeatureLayer;
        var layers = new[] { fl_1, fl_2 };
        //IEnumerable<Layer> layers = m.Layers;  //This creates a list of ALL layers in map.

        //Set the map frame extent to the selected features in the list of layers
        mf.SetCamera(layers, true);
      });
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Map.FindLayers
      // cref: ArcGIS.Desktop.Layouts.MapFrame.SetCamera
      // cref: ArcGIS.Core.Data.Feature.GetShape
      // cref: ArcGIS.Desktop.Mapping.Camera.Scale
      #region Change map frame extent to single feature with 15 percent buffer
      //Change map frame extent to single feature with 10 percent buffer

      //Process on the worker thread
      await QueuedTask.Run(() =>
      {
        //Reference the mapframe and its associated map
        MapFrame mf = layout.FindElement("Map Frame") as MapFrame;
        Map m = mf.Map;

        //Reference a feature layer and build a query (to return a single feature)
        FeatureLayer fl = m.FindLayers("GreatLakes").First() as FeatureLayer;
        QueryFilter qf = new QueryFilter();
        string whereClause = "NAME = 'Lake Erie'";
        qf.WhereClause = whereClause;

        //Zoom to the feature
        using (ArcGIS.Core.Data.RowCursor rowCursor = fl.Search(qf))
        {
          while (rowCursor.MoveNext())
          {
            //Get the shape from the row and set extent
            using (var feature = rowCursor.Current as ArcGIS.Core.Data.Feature)
            {
              Polygon polygon = feature.GetShape() as Polygon;
              Envelope env = polygon.Extent as Envelope;
              mf.SetCamera(env);

              //Zoom out 15 percent
              Camera cam = mf.Camera;
              cam.Scale = cam.Scale * 1.15;
              mf.SetCamera(cam);
            }
          }
        }
      });
      #endregion
    }

    public void snippets_MapFrame2()
    {
      // cref: ArcGIS.Desktop.Layouts.LayoutView.CanActivateMapFrame
      // cref: ArcGIS.Desktop.Layouts.LayoutView.ActivateMapFrame
      #region Activate Map Frame

      //The active view must be a layout view.
      var lv = LayoutView.Active;
      if (lv == null)
        return;
      var layout = lv.Layout;
      if (layout == null)
        return;

      //We can activate a map frame on the layout of the active view
      var map_frame = layout.GetElementsAsFlattenedList()
                         .OfType<MapFrame>().FirstOrDefault(mf => mf.Name == "Map 1");
      if (map_frame == null)
        return;
      //can we activate the map frame?
      if (lv.CanActivateMapFrame(map_frame))
        //activate it - Note: we are on the UI thread!
        lv.ActivateMapFrame(map_frame);

      #endregion
    }

    public void snippets_MapFrame3()
    {
      // cref: ArcGIS.Desktop.Layouts.LayoutView.ActivatedMapFrame
      // cref: ArcGIS.Desktop.Layouts.LayoutView.DeactivateMapFrame
      #region Deactivate Map Frame

      //The active view must be a layout view.
      var lv = LayoutView.Active;
      if (lv == null)
        return;
      var layout = lv.Layout;
      if (layout == null)
        return;

      //Deactivate any activated map frame
      //Note: we are on the UI thread!
      lv.DeactivateMapFrame();//no-op if nothing activated

      //or - check if a  map frame is activated first...
      if (lv.ActivatedMapFrame != null)
        //Note: we are on the UI thread!
        lv.DeactivateMapFrame();

      #endregion
    }

    public void snippets_MapFrame4()
    {
      // cref: ArcGIS.Desktop.Layouts.LayoutView.ActivatedMapFrame
      // cref: ArcGIS.Desktop.Layouts.LayoutView.ActivatedMapView
      #region Get the Activated Map Frame and MapView

      //The active view must be a layout view.
      var lv = LayoutView.Active;
      if (lv == null)
        return;

      var map_view = lv.ActivatedMapView;
      if (map_view != null)
      {
        //TODO - use activated map view
      }
      var map_frame = lv.ActivatedMapFrame;
      if (map_frame != null)
      {
        //TODO - use activated map frame
      }

      #endregion
    }

    private void TranslatePointInMapFrameToMapView()
    {

      QueuedTask.Run(() => {
        var pointSymbol = SymbolFactory.Instance.ConstructPointSymbol(ColorFactory.Instance.BlackRGB, 8);
        var graphicsLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<GraphicsLayer>().FirstOrDefault();

        // cref: ArcGIS.Desktop.Layouts.Element.GetBounds
        // cref: ArcGIS.Desktop.Layouts.MapFrame.PageToMap
        // cref: ArcGIS.Desktop.Mapping.GraphicsLayerExtensions.AddElement 
        #region Translates a point in page coordinates to a point in map coordinates. 
        //On the QueuedTask
        var layout = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault().GetLayout();
        var mapFrame = layout.FindElement("New Map Frame") as MapFrame;

        //Get a point in the center of the Map frame
        var mapFrameCenterPoint = mapFrame.GetBounds().CenterCoordinate;
        //Convert to MapPoint
        //At 2.x - var pointInMapFrame = MapPointBuilder.CreateMapPoint(mapFrameCenterPoint);
        var pointInMapFrame = MapPointBuilderEx.CreateMapPoint(mapFrameCenterPoint);

        //Find the corresponding point in the MapView
        var pointOnMap = mapFrame.PageToMap(pointInMapFrame);

        //Create a point graphic on the MapView.
        var cimGraphicElement = new CIMPointGraphic
        {
          Location = pointOnMap,
          Symbol = pointSymbol.MakeSymbolReference()
        };
        graphicsLayer.AddElement(cimGraphicElement);
        #endregion
      });
    }

    // cref: ARCGIS.DESKTOP.MAPPING.MAPTOOL.ONTOOLMOUSEDOWN
    // cref: ARCGIS.DESKTOP.MAPPING.MAPTOOL.HandleMouseDownAsync
    // cref: ARCGIS.DESKTOP.MAPPING.MapViewMouseButtonEventArgs
    // cref: ARCGIS.DESKTOP.MAPPING.MapView.ClientToMap
    // cref: ArcGIS.Desktop.Layouts.MapFrame.MapToPage
    // cref: ArcGIS.Desktop.Layouts.GraphicFactory.CreateSimpleGraphic
    // cref: ArcGIS.Desktop.Layouts.ElementFactory.CreateGraphicElement
    #region Translates a point in map coordinates to a point in page coordinates
    internal class GetMapCoordinates : MapTool
    {
      protected override void OnToolMouseDown(MapViewMouseButtonEventArgs e)
      {
        if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
          e.Handled = true; //Handle the event args to get the call to the corresponding async method
      }

      protected override Task HandleMouseDownAsync(MapViewMouseButtonEventArgs e)
      {
        return QueuedTask.Run(() =>
        {
          var pointSymbol = SymbolFactory.Instance.ConstructPointSymbol(ColorFactory.Instance.BlackRGB, 8);
          var layout = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault().GetLayout();
          
          //Convert the clicked point in client coordinates to the corresponding map coordinates.
          var mapPoint = MapView.Active.ClientToMap(e.ClientPoint);
          ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(string.Format("X: {0} Y: {1} Z: {2}",
              mapPoint.X, mapPoint.Y, mapPoint.Z), "Map Coordinates");
          //Get the corresponding layout point
          var mapFrame = layout.FindElement("New Map Frame") as MapFrame;
          var pointOnLayoutFrame = mapFrame.MapToPage(mapPoint);

          //Create a point graphic on the Layout.
          var cimGraphicElement = new CIMPointGraphic
          {
            Location = pointOnLayoutFrame,
            Symbol = pointSymbol.MakeSymbolReference()
          };
          //Or use GraphicFactory
          var cimGraphicElement2 = GraphicFactory.Instance.CreateSimpleGraphic(
                  pointOnLayoutFrame, pointSymbol);

          //At 2.x - LayoutElementFactory.Instance.CreateGraphicElement(layout, cimGraphicElement);

          ElementFactory.Instance.CreateGraphicElement(layout, cimGraphicElement);
          ElementFactory.Instance.CreateGraphicElement(layout, cimGraphicElement2);

        });
        
      }
    }
    #endregion


    #region ProSnippet Group: Layout MapSeries
    #endregion
    async public void snippets_MapSeries()
    {
      Layout layout = LayoutView.Active.Layout;

      // cref: ArcGIS.Desktop.Layouts.MapSeries
      // cref: ArcGIS.Desktop.Layouts.SpatialMapSeries
      // cref: ArcGIS.Desktop.Layouts.SpatialMapSeries.SortField
      // cref: ArcGIS.Desktop.Layouts.SpatialMapSeries.SortAscending
      // cref: ArcGIS.Desktop.Layouts.SpatialMapSeries.PageNumberField
      // cref: ArcGIS.Desktop.Layouts.Layout.SetMapSeries
      #region Modify an existing map series
      //Modify the currently active map series and changes its sort field and page number field.

      //Perform on the worker thread
      await QueuedTask.Run(() =>
      {
        SpatialMapSeries SMS = layout.MapSeries as SpatialMapSeries; //cast as spatial map seris for additional members
        SMS.SortField = "State_Name";
        SMS.SortAscending = true;
        SMS.PageNumberField = "PageNum";

        //Overwrite the current map series with these new settings
        layout.SetMapSeries(SMS); 
      });
      #endregion

      // cref: ArcGIS.Desktop.Layouts.MapSeries.CreateSpatialMapSeries(ArcGIS.Desktop.Layouts.Layout,ArcGIS.Desktop.Layouts.MapFrame,ArcGIS.Desktop.Mapping.BasicFeatureLayer,System.String)
      // cref: ArcGIS.Desktop.Layouts.MapFrame
      // cref: ArcGIS.Desktop.Layouts.MapSeries
      // cref: ArcGIS.Desktop.Layouts.SpatialMapSeries
      // cref: ArcGIS.Desktop.Layouts.SpatialMapSeries.SortField
      // cref: ArcGIS.Desktop.Layouts.SpatialMapSeries.SortAscending
      // cref: ArcGIS.Desktop.Layouts.SpatialMapSeries.CategoryField
      // cref: ArcGIS.Desktop.Layouts.SpatialMapSeries.ExtentOptions
      // cref: ArcGIS.Desktop.Layouts.SpatialMapSeries.MarginType
      // cref: ArcGIS.Desktop.Layouts.SpatialMapSeries.MarginUnits
      // cref: ArcGIS.Desktop.Layouts.SpatialMapSeries.ScaleRounding
      // cref: ArcGIS.Desktop.Layouts.Layout.SetMapSeries
      // cref: ArcGIS.Core.CIM.UnitType
      // cref: ArcGIS.Core.CIM.ExtentFitType
      // cref: ArcGIS.Core.Geometry.LinearUnit
      #region Create a new spatial map series
      // This example create a new spatial map series and then applies it to the active layout. This will automatically 
      // overwrite an existing map series if one is already present.

      //Reference map frame and index layer
      MapFrame mf = layout.FindElement("Map Frame") as MapFrame;
      Map m = mf.Map;
      BasicFeatureLayer indexLyr = m.FindLayers("Countries").FirstOrDefault() as BasicFeatureLayer;

      //Construct map series on worker thread
      await QueuedTask.Run(() =>
      {
        //SpatialMapSeries constructor - required parameters
        SpatialMapSeries SMS = MapSeries.CreateSpatialMapSeries(layout, mf, indexLyr, "Name");
        
        //Set optional, non-default values
        SMS.CategoryField = "Continent";
        SMS.SortField = "Population";
        SMS.ExtentOptions = ExtentFitType.BestFit;
        SMS.MarginType = ArcGIS.Core.CIM.UnitType.PageUnits;
        SMS.MarginUnits = ArcGIS.Core.Geometry.LinearUnit.Centimeters;
        SMS.Margin = 1;
        SMS.ScaleRounding = 1000;
        layout.SetMapSeries(SMS);  //Overwrite existing map series.
      });
      #endregion
    }

        #region ProSnippet Group: Layout Export
        #endregion
        async public void snippets_StandardExport()
    {
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals("Layout Name"));
      Layout layout = await QueuedTask.Run(() => layoutItem.GetLayout());
      String filePath = null;

      // cref: ArcGIS.Desktop.Layouts.Layout.Export(ArcGIS.Desktop.Mapping.ExportFormat)
      // cref: ARCGIS.DESKTOP.MAPPING.PDFFORMAT
      // cref: ARCGIS.DESKTOP.MAPPING.PDFFORMAT.DoCompressVectorGraphics
      // cref: ARCGIS.DESKTOP.MAPPING.PDFFORMAT.DoEmbedFonts
      // cref: ARCGIS.DESKTOP.MAPPING.PDFFORMAT.HasGeoRefInfo
      // cref: ARCGIS.DESKTOP.MAPPING.PDFFORMAT.ImageCompression
      // cref: ARCGIS.DESKTOP.MAPPING.PDFFORMAT.ImageQuality
      // cref: ARCGIS.DESKTOP.MAPPING.PDFFORMAT.LayersAndAttributes
      // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT
      // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.OutputFileName
      // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.VALIDATEOUTPUTFILEPATH
      // cref: ARCGIS.DESKTOP.MAPPING.ImageCompression
      // cref: ARCGIS.DESKTOP.MAPPING.ImageQuality
      // cref: ARCGIS.DESKTOP.MAPPING.LayersAndAttributes
      #region Export a layout to PDF
      //Export a single page layout to PDF.

      //Create a PDF format with appropriate settings
      //BMP, EMF, EPS, GIF, JPEG, PNG, SVG, TGA, and TFF formats are also available for export
      PDFFormat PDF = new PDFFormat()
      {
        OutputFileName = filePath,
        Resolution = 300,
        DoCompressVectorGraphics = true,
        DoEmbedFonts = true,
        HasGeoRefInfo = true,
        ImageCompression = ImageCompression.Adaptive,
        ImageQuality = ImageQuality.Best,
        LayersAndAttributes = LayersAndAttributes.LayersAndAttributes
      };

      //Check to see if the path is valid and export
      if (PDF.ValidateOutputFilePath())
      {
        await QueuedTask.Run(() => layout.Export(PDF));  //Export the layout to PDF on the worker thread
      }
      #endregion

      // cref: ArcGIS.Desktop.Layouts.MapFrame.Export(ArcGIS.Desktop.Mapping.ExportFormat)
      // cref: ARCGIS.DESKTOP.MAPPING.JPEGFormat
      // cref: ARCGIS.DESKTOP.MAPPING.JPEGFormat.HasWorldFile
      // cref: ARCGIS.DESKTOP.MAPPING.JPEGFormat.ColorMode
      // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT
      // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.OutputFileName
      // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.VALIDATEOUTPUTFILEPATH
      // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.Resolution
      // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.Height
      // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.Width
      // cref: ARCGIS.DESKTOP.MAPPING.JPEGColorMode
      #region Export a map frame to JPG
      //Export a map frame to JPG.

      //Create JPEG format with appropriate settings
      //BMP, EMF, EPS, GIF, PDF, PNG, SVG, TGA, and TFF formats are also available for export
      //at 2.x - JPEGFormat JPG = new JPEGFormat()
      //{
      //  HasWorldFile = true,
      //  Resolution = 300,
      //  OutputFileName = filePath,
      //  JPEGColorMode = JPEGColorMode.TwentyFourBitTrueColor,
      //  Height = 800,
      //  Width = 1200
      //};
      JPEGFormat JPG = new JPEGFormat()
      {
        HasWorldFile = true,
        Resolution = 300,
        OutputFileName = filePath,
        ColorMode = JPEGColorMode.TwentyFourBitTrueColor,
        Height = 800,
        Width = 1200
      };

      //Reference the map frame
      MapFrame mf = layout.FindElement("MyMapFrame") as MapFrame;

      //Export on the worker thread
      await QueuedTask.Run(() =>
      {
        //Check to see if the path is valid and export
        if (JPG.ValidateOutputFilePath())
        {
          mf.Export(JPG);  //Export the map frame to JPG
        }
      });
      #endregion

      // cref: ArcGIS.Desktop.Layouts.MapFrame.GetMapView(ArcGIS.Desktop.Layouts.LayoutView)
      // cref: ArcGIS.Desktop.Layouts.MapFrame.Export(ArcGIS.Desktop.Mapping.ExportFormat)
      // cref: ARCGIS.DESKTOP.MAPPING.BMPFormat
      // cref: ARCGIS.DESKTOP.MAPPING.BMPFormat.HasWorldFile
      // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT
      // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.OutputFileName
      // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.VALIDATEOUTPUTFILEPATH
      // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.Resolution
      // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.Height
      // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.Width
      // cref: ARCGIS.DESKTOP.MAPPING.MapView.Export
      #region Export the map view associated with a map frame to BMP
      //Export the map view associated with a map frame to BMP.

      //Create BMP format with appropriate settings
      //EMF, EPS, GIF, JPEG, PDF, PNG, SVG, TGA, and TFF formats are also available for export
      BMPFormat BMP = new BMPFormat()
      {
        Resolution = 300,
        Height = 500,
        Width = 800,
        HasWorldFile = true,
        OutputFileName = filePath
      };

      //Reference the active layout view
      LayoutView lytView = LayoutView.Active;

      //Reference the map frame and its map view
      MapFrame mf_bmp = layout.FindElement("Map Frame") as MapFrame;
      MapView mv_bmp = mf_bmp.GetMapView(lytView);

      if (mv_bmp != null)
      {
        //Export on the worker thread
        await QueuedTask.Run(() =>
        {

          //Check to see if the path is valid and export
          if (BMP.ValidateOutputFilePath())
          {
            mv_bmp.Export(BMP);  //Export to BMP
          }
        });
      }
      #endregion
    }

    async public void snippets_Export()
    {
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals("Layout Name"));
      Layout layout = await QueuedTask.Run(() => layoutItem.GetLayout());
      String filePath = null;

      // cref: ArcGIS.Desktop.Layouts.MapSeriesExportOptions
      // cref: ArcGIS.Desktop.Layouts.MapSeriesExportOptions.ExportPages
      // cref: ArcGIS.Desktop.Layouts.MapSeriesExportOptions.CustomPages
      // cref: ArcGIS.Desktop.Layouts.MapSeriesExportOptions.ExportFileOptions
      // cref: ArcGIS.Desktop.Layouts.MapSeriesExportOptions.ShowSelectedSymbology
      // cref: ArcGIS.Desktop.Layouts.Layout.Export(ArcGIS.Desktop.Mapping.ExportFormat)
      // cref: ArcGIS.Desktop.Layouts.ExportPages
      // cref: ArcGIS.Desktop.MAPPING.ExportFileOptions
      // cref: ARCGIS.DESKTOP.MAPPING.PDFFORMAT
      // cref: ARCGIS.DESKTOP.MAPPING.PDFFORMAT.DoCompressVectorGraphics
      // cref: ARCGIS.DESKTOP.MAPPING.PDFFORMAT.DoEmbedFonts
      // cref: ARCGIS.DESKTOP.MAPPING.PDFFORMAT.HasGeoRefInfo
      // cref: ARCGIS.DESKTOP.MAPPING.PDFFORMAT.ImageCompression
      // cref: ARCGIS.DESKTOP.MAPPING.PDFFORMAT.ImageQuality
      // cref: ARCGIS.DESKTOP.MAPPING.PDFFORMAT.LayersAndAttributes
      // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT
      // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.OutputFileName
      // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.VALIDATEOUTPUTFILEPATH
      // cref: ARCGIS.DESKTOP.MAPPING.ImageCompression
      // cref: ARCGIS.DESKTOP.MAPPING.ImageQuality
      // cref: ARCGIS.DESKTOP.MAPPING.LayersAndAttributes
      #region Export a map series to single PDF
      //Export a map series with multiple pages to a single PDF.

      //Create PDF format with appropriate settings
      PDFFormat MS_PDF = new PDFFormat()
      {
        OutputFileName = filePath,
        Resolution = 300,
        DoCompressVectorGraphics = true,
        DoEmbedFonts = true,
        HasGeoRefInfo = true,
        ImageCompression = ImageCompression.Adaptive,
        ImageQuality = ImageQuality.Best,
        LayersAndAttributes = LayersAndAttributes.LayersAndAttributes
      };

      //Set up map series export options
      MapSeriesExportOptions MS_ExportOptions = new MapSeriesExportOptions()
      {
        ExportPages = ExportPages.Custom,  //Provide a specific list of pages
        CustomPages = "1-3, 5",  //Only used if ExportPages.Custom is set
        ExportFileOptions = ExportFileOptions.ExportAsSinglePDF,  //Export all pages to a single, multi-page PDF
        ShowSelectedSymbology = false  //Do no show selection symbology in the output
      };

      //Export on the worker thread
      await QueuedTask.Run(() =>
      {
        //Check to see if the path is valid and export
        if (MS_PDF.ValidateOutputFilePath())
        {
          layout.Export(MS_PDF, MS_ExportOptions);  //Export to PDF
        }
      });
      #endregion

      // cref: ArcGIS.Desktop.Layouts.MapSeriesExportOptions
      // cref: ArcGIS.Desktop.Layouts.MapSeriesExportOptions.ExportPages
      // cref: ArcGIS.Desktop.Layouts.MapSeriesExportOptions.ExportFileOptions
      // cref: ArcGIS.Desktop.Layouts.MapSeriesExportOptions.ShowSelectedSymbology
      // cref: ArcGIS.Desktop.Layouts.Layout.Export(ArcGIS.Desktop.Mapping.ExportFormat)
      // cref: ArcGIS.Desktop.Layouts.ExportPages
      // cref: ArcGIS.Desktop.MAPPING.ExportFileOptions
      // cref: ARCGIS.DESKTOP.MAPPING.TIFFFormat
      // cref: ARCGIS.DESKTOP.MAPPING.TIFFFormat.ColorMode
      // cref: ARCGIS.DESKTOP.MAPPING.TIFFFormat.ImageCompression
      // cref: ARCGIS.DESKTOP.MAPPING.TIFFFormat.HasWorldFile
      // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT
      // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.OutputFileName
      // cref: ARCGIS.DESKTOP.MAPPING.EXPORTFORMAT.VALIDATEOUTPUTFILEPATH
      // cref: ARCGIS.DESKTOP.MAPPING.TIFFColorMode
      // cref: ARCGIS.DESKTOP.MAPPING.TIFFImageCompression
      #region Export a map series to individual TIFF files
      //Export each page of a map series to an individual TIFF file.

      //Create TIFF format with appropriate settings
      TIFFFormat TIFF = new TIFFFormat()
      {
        OutputFileName = filePath,
        Resolution = 300,
        ColorMode = TIFFColorMode.TwentyFourBitTrueColor,
        HasGeoTiffTags = true,
        HasWorldFile = true,
        ImageCompression = TIFFImageCompression.LZW
      };

      //Set up map series export options
      MapSeriesExportOptions MSExportOptions_TIFF = new MapSeriesExportOptions()
      {
        ExportPages = ExportPages.All,  //All pages
        ExportFileOptions = ExportFileOptions.ExportMultipleNames,  //Export each page to an individual file using page name as a suffix.
        ShowSelectedSymbology = true  //Include selection symbology in the output
      };

      //Export on the worker thread
      await QueuedTask.Run(() =>
      {
        //Check to see if the path is valid and export
        if (TIFF.ValidateOutputFilePath())
        {
          layout.Export(TIFF, MSExportOptions_TIFF);  //Export to TIFF
        }
      });
      #endregion
    }

    #region ProSnippet Group: LayoutOptions
    #endregion

    public void LayoutOptions1()
		{
      // cref: ArcGIS.Desktop.Core.ApplicationOptions
      // cref: ArcGIS.Desktop.Core.ApplicationOptions.LayoutOptions
      // cref: ArcGIS.Desktop.Core.LayoutOptions
      // cref: ArcGIS.Desktop.Core.LayoutOptions.KeepLastToolActive
      // cref: ArcGIS.Desktop.Core.LayoutOptions.WarnAboutAssociatedSurrounds
      // cref: ArcGIS.Desktop.Core.LayoutOptions.LayoutTemplatePath
      #region Get LayoutOptions

      var lastToolActive = ApplicationOptions.LayoutOptions.KeepLastToolActive;
      var warnOnSurrounds = ApplicationOptions.LayoutOptions.WarnAboutAssociatedSurrounds;
      //eg <Install_Path>\Resources\LayoutTemplates\en-US
      var gallery_path = ApplicationOptions.LayoutOptions.LayoutTemplatePath;

      #endregion

      // cref: ArcGIS.Desktop.Core.ApplicationOptions
      // cref: ArcGIS.Desktop.Core.ApplicationOptions.LayoutOptions
      // cref: ArcGIS.Desktop.Core.LayoutOptions
      // cref: ArcGIS.Desktop.Core.LayoutOptions.KeepLastToolActive
      // cref: ArcGIS.Desktop.Core.LayoutOptions.WarnAboutAssociatedSurrounds
      // cref: ArcGIS.Desktop.Core.LayoutOptions.LayoutTemplatePath
      #region Set LayoutOptions

      //keep graphic element insert tool active
      ApplicationOptions.LayoutOptions.KeepLastToolActive = true;
      //no warning when deleting a map frame results in other elements being deleted
      ApplicationOptions.LayoutOptions.WarnAboutAssociatedSurrounds = false;
      //path to .pagx files used as templates
      ApplicationOptions.LayoutOptions.LayoutTemplatePath = @"D:\data\layout_templates";

      #endregion

    }


		#region ProSnippet Group: TextAndGraphicsElementsOptions
		#endregion

    public void TextAndGraphicsElementsOptions()
		{
      // cref: ArcGIS.Desktop.Core.ApplicationOptions
      // cref: ArcGIS.Desktop.Core.ApplicationOptions.TextAndGraphicsElementsOptions
      // cref: ArcGIS.Desktop.Core.TextAndGraphicsElementsOptions
      // cref: ArcGIS.Desktop.Core.TextAndGraphicsElementsOptions.GetAvailableFonts
      #region Get All Available Fonts

      //Note: see also SymbolFactory.Instance.GetAvailableFonts() which returns the
      //same list. Use for TextAndGraphicsElementsOptions.GetAvailableFonts() convenience

      QueuedTask.Run(() =>
      {
        //A list of tuples of Font name + associated Font Styles, one tuple per
        //font, is returned
        var fonts = ApplicationOptions.TextAndGraphicsElementsOptions.GetAvailableFonts();
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Pro Fonts\r\n============================");
        foreach (var font in fonts)
        {
          var styles = string.Join(",", font.fontStyles);
          sb.AppendLine($"{font.fontName}, [{styles}]");
        }
        System.Diagnostics.Debug.WriteLine(sb.ToString());
      });

      #endregion

      // cref: ArcGIS.Desktop.Core.ApplicationOptions
      // cref: ArcGIS.Desktop.Core.ApplicationOptions.TextAndGraphicsElementsOptions
      // cref: ArcGIS.Desktop.Core.TextAndGraphicsElementsOptions
      // cref: ArcGIS.Desktop.Core.TextAndGraphicsElementsOptions.GetDefaultFont
      // cref: ArcGIS.Desktop.Core.TextAndGraphicsElementsOptions.GetDefaultPointSymbol
      // cref: ArcGIS.Desktop.Core.TextAndGraphicsElementsOptions.GetDefaultLineSymbol
      // cref: ArcGIS.Desktop.Core.TextAndGraphicsElementsOptions.GetDefaultPolygonSymbol
      // cref: ArcGIS.Desktop.Core.TextAndGraphicsElementsOptions.GetDefaultTextSymbol
      #region Get TextAndGraphicsElementsOptions

      QueuedTask.Run(() =>
      {
        //Get the default font (see also 'SymbolFactory.Instance.DefaultFont')
        var def_font = ApplicationOptions.TextAndGraphicsElementsOptions.GetDefaultFont();
        System.Diagnostics.Debug.WriteLine(
          $"\r\ndefault font: {def_font.fontName}, {def_font.styleName}");

        //Get the default graphics element symbols - point, line, poly, text
        var ptSymbol = ApplicationOptions.TextAndGraphicsElementsOptions.GetDefaultPointSymbol();
        var lineSymbol = ApplicationOptions.TextAndGraphicsElementsOptions.GetDefaultLineSymbol();
        var polySymbol = ApplicationOptions.TextAndGraphicsElementsOptions.GetDefaultPolygonSymbol();
        var textSymbol = ApplicationOptions.TextAndGraphicsElementsOptions.GetDefaultTextSymbol();
      });

      #endregion

      // cref: ArcGIS.Desktop.Core.ApplicationOptions
      // cref: ArcGIS.Desktop.Core.ApplicationOptions.TextAndGraphicsElementsOptions
      // cref: ArcGIS.Desktop.Core.TextAndGraphicsElementsOptions
      // cref: ArcGIS.Desktop.Core.TextAndGraphicsElementsOptions.SetDefaultPointSymbol
      // cref: ArcGIS.Desktop.Core.TextAndGraphicsElementsOptions.SetDefaultLineSymbol
      // cref: ArcGIS.Desktop.Core.TextAndGraphicsElementsOptions.SetDefaultPolygonSymbol
      // cref: ArcGIS.Desktop.Core.TextAndGraphicsElementsOptions.SetDefaultTextSymbol
      #region Set TextAndGraphicsElementsOptions

      QueuedTask.Run(() =>
      {
        //Set a default font. Use its default style
        ApplicationOptions.TextAndGraphicsElementsOptions.SetDefaultFont("tahoma");
        //or specify an explicit style
        ApplicationOptions.TextAndGraphicsElementsOptions.SetDefaultFont("tahoma", "bold");

        //Create symbols
        var ptSymbol2 = SymbolFactory.Instance.ConstructPointSymbol(
          ColorFactory.Instance.RedRGB, 14, SimpleMarkerStyle.Diamond);
        var lineSymbol2 = SymbolFactory.Instance.ConstructLineSymbol(
          ColorFactory.Instance.RedRGB, 2, SimpleLineStyle.Dash);
        var polySymbol2 = SymbolFactory.Instance.ConstructPolygonSymbol(
          ColorFactory.Instance.RedRGB, SimpleFillStyle.DiagonalCross);
        var textSymbol2 = SymbolFactory.Instance.ConstructTextSymbol(
          ColorFactory.Instance.RedRGB, 12);

        //Set default point, line, poly, text graphics element symbols
        ApplicationOptions.TextAndGraphicsElementsOptions.SetDefaultPointSymbol(ptSymbol2);
        ApplicationOptions.TextAndGraphicsElementsOptions.SetDefaultLineSymbol(lineSymbol2);
        ApplicationOptions.TextAndGraphicsElementsOptions.SetDefaultPolygonSymbol(polySymbol2);
        ApplicationOptions.TextAndGraphicsElementsOptions.SetDefaultTextSymbol(textSymbol2);
      });

      #endregion
    }

    #region ProSnippet Group: MapFrame_Display_Constraints
    #endregion

    public void SetAutoCameraNone(string mapFrame)
    {
      // cref: ArcGIS.Desktop.Layouts.MapFrame.GetAutoCamera
      // cref: ArcGIS.Desktop.Layouts.MapFrame.SetAutoCamera
      // cref: ArcGIS.Desktop.Layouts.MapFrame.IsMapSeriesMapFrame
      // cref: ArcGIS.Desktop.Layouts.MapFrame.ValidateAutoCamera
      // cref: ArcGIS.Core.CIM.CIMAutoCamera
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.Source
      // cref: ArcGIS.Core.CIM.AutoCameraSource
      #region SetAutoCameraNone
      var layout = LayoutView.Active.Layout;
      var mf = layout.GetElementsAsFlattenedList().OfType<MapFrame>()
        .First(mf => mf.Name == mapFrame);
      var autoCamera = mf.GetAutoCamera();
      autoCamera.Source = AutoCameraSource.None;
      if (mf.ValidateAutoCamera(autoCamera) &&
        !mf.IsMapSeriesMapFrame())
        mf.SetAutoCamera(autoCamera);

      #endregion
    }

    public void SetAutoCameraFixedExtent(string mapFrame)
    {
      // cref: ArcGIS.Desktop.Layouts.MapFrame.GetAutoCamera
      // cref: ArcGIS.Desktop.Layouts.MapFrame.SetAutoCamera
      // cref: ArcGIS.Desktop.Layouts.MapFrame.IsMapSeriesMapFrame
      // cref: ArcGIS.Desktop.Layouts.MapFrame.ValidateAutoCamera
      // cref: ArcGIS.Desktop.Layouts.MapFrame.GetViewExtent
      // cref: ArcGIS.Core.CIM.CIMAutoCamera
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.Source
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.AutoCameraType
      // cref: ArcGIS.Core.CIM.AutoCameraType
      // cref: ArcGIS.Core.CIM.AutoCameraSource
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.Extent
      #region SetAutoCameraFixedExtent
      var layout = LayoutView.Active.Layout;
      var mf = layout.GetElementsAsFlattenedList().OfType<MapFrame>()
        .First(mf => mf.Name == mapFrame);
      var autoCamera = mf.GetAutoCamera();
      autoCamera.Source = AutoCameraSource.Fixed;
      autoCamera.AutoCameraType = AutoCameraType.Extent;

      var mf_extent = mf.GetViewExtent();

      var extent = EnvelopeBuilderEx.CreateEnvelope(
        400748.62, 800296.4, 1310669.05, 1424520.74, mf.Map.SpatialReference);
      autoCamera.Extent = extent;

      if (mf.ValidateAutoCamera(autoCamera) &&
        !mf.IsMapSeriesMapFrame())
        mf.SetAutoCamera(autoCamera);

      #endregion
    }

    public void SetAutoCameraFixedCenter(string mapFrame)
    {
      // cref: ArcGIS.Desktop.Layouts.MapFrame.GetAutoCamera
      // cref: ArcGIS.Desktop.Layouts.MapFrame.SetAutoCamera
      // cref: ArcGIS.Desktop.Layouts.MapFrame.IsMapSeriesMapFrame
      // cref: ArcGIS.Desktop.Layouts.MapFrame.ValidateAutoCamera
      // cref: ArcGIS.Desktop.Layouts.MapFrame.GetViewCenter
      // cref: ArcGIS.Desktop.Layouts.MapFrame.GetMapView
      // cref: ArcGIS.Core.CIM.CIMAutoCamera
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.Source
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.AutoCameraType
      // cref: ArcGIS.Core.CIM.AutoCameraType
      // cref: ArcGIS.Core.CIM.AutoCameraSource
      // cref: ArcGIS.Core.CIM.CIMViewCamera
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.Camera
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.IntersectLayerPath
      // cref: ArcGIS.Desktop.Mapping.MapView.Camera
      #region SetAutoCameraFixedCenter
      var layout = LayoutView.Active.Layout;
      var mf = layout.GetElementsAsFlattenedList().OfType<MapFrame>()
        .First(mf => mf.Name == mapFrame);
      var autoCamera = mf.GetAutoCamera();
      autoCamera.Source = AutoCameraSource.Fixed;
      autoCamera.AutoCameraType = AutoCameraType.Center;

      var camera = mf.GetMapView(LayoutView.Active).Camera;
      var center = mf.GetViewCenter();

      //var extent = EnvelopeBuilderEx.CreateEnvelope(
      //	400748.62, 800296.4, 1310669.05, 1424520.74, mf.Map.SpatialReference);
      //autoCamera.Extent = extent;

      var camera2 = new CIMViewCamera()
      {
        Heading = 0,
        Pitch = -90,
        Roll = 0,
        Scale = 21169571,
        X = 855708,
        Y = 1112409,
        Z = double.NaN
      };
      autoCamera.Camera = camera2;
      var states = mf.Map.GetLayersAsFlattenedList().First(l => l.Name == "State_Polygons");
      autoCamera.IntersectLayerPath = states.URI;

      if (mf.ValidateAutoCamera(autoCamera) &&
        !mf.IsMapSeriesMapFrame())
        mf.SetAutoCamera(autoCamera);

      #endregion
    }

    public void SetAutoCameraFixedCenterAndScale(string mapFrame)
    {
      // cref: ArcGIS.Desktop.Layouts.MapFrame.GetAutoCamera
      // cref: ArcGIS.Desktop.Layouts.MapFrame.SetAutoCamera
      // cref: ArcGIS.Desktop.Layouts.MapFrame.IsMapSeriesMapFrame
      // cref: ArcGIS.Desktop.Layouts.MapFrame.ValidateAutoCamera
      // cref: ArcGIS.Desktop.Layouts.MapFrame.GetViewCenter
      // cref: ArcGIS.Desktop.Layouts.MapFrame.GetMapView
      // cref: ArcGIS.Core.CIM.CIMAutoCamera
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.Source
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.AutoCameraType
      // cref: ArcGIS.Core.CIM.AutoCameraType
      // cref: ArcGIS.Core.CIM.AutoCameraSource
      // cref: ArcGIS.Core.CIM.CIMViewCamera
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.Camera
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.IntersectLayerPath
      // cref: ArcGIS.Desktop.Mapping.MapView.Camera
      #region SetAutoCameraFixedCenterAndScale
      var layout = LayoutView.Active.Layout;
      var mf = layout.GetElementsAsFlattenedList().OfType<MapFrame>()
        .First(mf => mf.Name == mapFrame);
      var autoCamera = mf.GetAutoCamera();
      autoCamera.Source = AutoCameraSource.Fixed;
      autoCamera.AutoCameraType = AutoCameraType.CenterAndScale;

      var camera = mf.GetMapView(LayoutView.Active).Camera;
      var center = mf.GetViewCenter();

      //var extent = EnvelopeBuilderEx.CreateEnvelope(
      //	400748.62, 800296.4, 1310669.05, 1424520.74, mf.Map.SpatialReference);
      //autoCamera.Extent = extent;

      var camera2 = new CIMViewCamera()
      {
        Heading = 0,
        Pitch = -90,
        Roll = 0,
        Scale = 21169571,
        X = 1310669.0 + ((400748.5 - 1310669.0) / 2.0),
        Y = 800296.4 + ((1424520.74 - 800296.4) / 2.0),
        Z = double.NaN
      };
      autoCamera.Camera = camera2;
      var states = mf.Map.GetLayersAsFlattenedList().First(l => l.Name == "State_Polygons");
      //autoCamera.IntersectLayerPath = states.URI;


      if (mf.ValidateAutoCamera(autoCamera) &&
        !mf.IsMapSeriesMapFrame())
        mf.SetAutoCamera(autoCamera);

      #endregion
    }

    public void SetAutoCameraFixedScale(string mapFrame)
    {
      // cref: ArcGIS.Desktop.Layouts.MapFrame.GetAutoCamera
      // cref: ArcGIS.Desktop.Layouts.MapFrame.SetAutoCamera
      // cref: ArcGIS.Desktop.Layouts.MapFrame.IsMapSeriesMapFrame
      // cref: ArcGIS.Desktop.Layouts.MapFrame.ValidateAutoCamera
      // cref: ArcGIS.Desktop.Layouts.MapFrame.GetViewCenter
      // cref: ArcGIS.Desktop.Layouts.MapFrame.GetMapView
      // cref: ArcGIS.Core.CIM.CIMAutoCamera
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.Source
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.AutoCameraType
      // cref: ArcGIS.Core.CIM.AutoCameraType
      // cref: ArcGIS.Core.CIM.AutoCameraSource
      // cref: ArcGIS.Core.CIM.CIMViewCamera
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.Camera
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.IntersectLayerPath
      // cref: ArcGIS.Desktop.Mapping.MapView.Camera
      #region SetAutoCameraFixedScale
      var layout = LayoutView.Active.Layout;
      var mf = layout.GetElementsAsFlattenedList().OfType<MapFrame>()
        .First(mf => mf.Name == mapFrame);
      var autoCamera = mf.GetAutoCamera();
      autoCamera.Source = AutoCameraSource.Fixed;
      autoCamera.AutoCameraType = AutoCameraType.Scale;

      var camera = mf.GetMapView(LayoutView.Active).Camera;
      var center = mf.GetViewCenter();

      //var extent = EnvelopeBuilderEx.CreateEnvelope(
      //	400748.62, 800296.4, 1310669.05, 1424520.74, mf.Map.SpatialReference);
      //autoCamera.Extent = extent;

      var camera2 = new CIMViewCamera()
      {
        Heading = 0,
        Pitch = -90,
        Roll = 0,
        Scale = 20000571,
        X = 1310669.0 + ((400748.5 - 1310669.0) / 2.0),
        Y = 800296.4 + ((1424520.74 - 800296.4) / 2.0),
        Z = double.NaN
      };
      autoCamera.Camera = camera2;
      var states = mf.Map.GetLayersAsFlattenedList().First(l => l.Name == "State_Polygons");
      //autoCamera.IntersectLayerPath = states.URI;

      if (mf.ValidateAutoCamera(autoCamera) &&
        !mf.IsMapSeriesMapFrame())
        mf.SetAutoCamera(autoCamera);

      #endregion
    }

    public void SetAutoCameraLinkedExtent(string mapFrame, string mapFrameLink)
    {
      // cref: ArcGIS.Desktop.Layouts.MapFrame.GetAutoCamera
      // cref: ArcGIS.Desktop.Layouts.MapFrame.SetAutoCamera
      // cref: ArcGIS.Desktop.Layouts.MapFrame.IsMapSeriesMapFrame
      // cref: ArcGIS.Desktop.Layouts.MapFrame.ValidateAutoCamera
      // cref: ArcGIS.Core.CIM.CIMAutoCamera
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.Source
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.AutoCameraType
      // cref: ArcGIS.Core.CIM.AutoCameraType
      // cref: ArcGIS.Core.CIM.AutoCameraSource
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.Camera
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.MapFrameLinkName
      #region SetAutoCameraLinkedExtent

      var layout = LayoutView.Active.Layout;
      var mf = layout.GetElementsAsFlattenedList().OfType<MapFrame>()
        .First(mf => mf.Name == mapFrame);
      var autoCamera = mf.GetAutoCamera();
      autoCamera.Source = AutoCameraSource.MapFrameLink;
      autoCamera.AutoCameraType = AutoCameraType.Extent;
      autoCamera.MapFrameLinkName = mapFrameLink;

      if (mf.ValidateAutoCamera(autoCamera) &&
        !mf.IsMapSeriesMapFrame())
        mf.SetAutoCamera(autoCamera);

      #endregion
    }

    public void SetAutoCameraLinkedCenter(string mapFrame, string mapFrameLink)
    {
      // cref: ArcGIS.Desktop.Layouts.MapFrame.GetAutoCamera
      // cref: ArcGIS.Desktop.Layouts.MapFrame.SetAutoCamera
      // cref: ArcGIS.Desktop.Layouts.MapFrame.IsMapSeriesMapFrame
      // cref: ArcGIS.Desktop.Layouts.MapFrame.ValidateAutoCamera
      // cref: ArcGIS.Core.CIM.CIMAutoCamera
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.Source
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.AutoCameraType
      // cref: ArcGIS.Core.CIM.AutoCameraType
      // cref: ArcGIS.Core.CIM.AutoCameraSource
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.Camera
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.MapFrameLinkName
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.IntersectLayerPath
      #region SetAutoCameraLinkedCenter

      var layout = LayoutView.Active.Layout;
      var mf = layout.GetElementsAsFlattenedList().OfType<MapFrame>()
        .First(mf => mf.Name == mapFrame);
      var autoCamera = mf.GetAutoCamera();
      autoCamera.Source = AutoCameraSource.MapFrameLink;
      autoCamera.AutoCameraType = AutoCameraType.Center;
      autoCamera.MapFrameLinkName = mapFrameLink;
      var states = mf.Map.GetLayersAsFlattenedList().First(l => l.Name == "State_Polygons");
      autoCamera.IntersectLayerPath = states.URI;

      if (mf.ValidateAutoCamera(autoCamera) &&
        !mf.IsMapSeriesMapFrame())
        mf.SetAutoCamera(autoCamera);

      #endregion
    }

    public void SetAutoCameraLinkedCenterAndScale(string mapFrame, string mapFrameLink)
    {
      // cref: ArcGIS.Desktop.Layouts.MapFrame.GetAutoCamera
      // cref: ArcGIS.Desktop.Layouts.MapFrame.SetAutoCamera
      // cref: ArcGIS.Desktop.Layouts.MapFrame.IsMapSeriesMapFrame
      // cref: ArcGIS.Desktop.Layouts.MapFrame.ValidateAutoCamera
      // cref: ArcGIS.Core.CIM.CIMAutoCamera
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.Source
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.AutoCameraType
      // cref: ArcGIS.Core.CIM.AutoCameraType
      // cref: ArcGIS.Core.CIM.AutoCameraSource
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.Camera
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.MapFrameLinkName
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.IntersectLayerPath
      #region SetAutoCameraLinkedCenterAndScale

      var layout = LayoutView.Active.Layout;
      var mf = layout.GetElementsAsFlattenedList().OfType<MapFrame>()
        .First(mf => mf.Name == mapFrame);
      var autoCamera = mf.GetAutoCamera();
      autoCamera.Source = AutoCameraSource.MapFrameLink;
      autoCamera.AutoCameraType = AutoCameraType.CenterAndScale;
      autoCamera.MapFrameLinkName = mapFrameLink;
      var states = mf.Map.GetLayersAsFlattenedList().First(l => l.Name == "State_Polygons");
      autoCamera.IntersectLayerPath = states.URI;

      if (mf.ValidateAutoCamera(autoCamera) &&
        !mf.IsMapSeriesMapFrame())
        mf.SetAutoCamera(autoCamera);

      #endregion
    }

    public void SetAutoCameraLinkedScale(string mapFrame, string mapFrameLink)
    {
      // cref: ArcGIS.Desktop.Layouts.MapFrame.GetAutoCamera
      // cref: ArcGIS.Desktop.Layouts.MapFrame.SetAutoCamera
      // cref: ArcGIS.Desktop.Layouts.MapFrame.IsMapSeriesMapFrame
      // cref: ArcGIS.Desktop.Layouts.MapFrame.ValidateAutoCamera
      // cref: ArcGIS.Core.CIM.CIMAutoCamera
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.Source
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.AutoCameraType
      // cref: ArcGIS.Core.CIM.AutoCameraType
      // cref: ArcGIS.Core.CIM.AutoCameraSource
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.Camera
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.MapFrameLinkName
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.IntersectLayerPath
      #region SetAutoCameraLinkedScale

      var layout = LayoutView.Active.Layout;
      var mf = layout.GetElementsAsFlattenedList().OfType<MapFrame>()
        .First(mf => mf.Name == mapFrame);
      var autoCamera = mf.GetAutoCamera();
      autoCamera.Source = AutoCameraSource.MapFrameLink;
      autoCamera.AutoCameraType = AutoCameraType.Scale;
      autoCamera.MapFrameLinkName = mapFrameLink;
      var states = mf.Map.GetLayersAsFlattenedList().First(l => l.Name == "State_Polygons");
      autoCamera.IntersectLayerPath = states.URI;

      if (mf.ValidateAutoCamera(autoCamera) &&
        !mf.IsMapSeriesMapFrame())
        mf.SetAutoCamera(autoCamera);

      #endregion
    }

    public void SetAutoCameraLinkedMapSeriesShape(string mapFrame)
    {
      // cref: ArcGIS.Desktop.Layouts.MapFrame.GetAutoCamera
      // cref: ArcGIS.Desktop.Layouts.MapFrame.SetAutoCamera
      // cref: ArcGIS.Desktop.Layouts.MapFrame.IsMapSeriesMapFrame
      // cref: ArcGIS.Desktop.Layouts.MapFrame.ValidateAutoCamera
      // cref: ArcGIS.Core.CIM.CIMAutoCamera
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.Source
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.AutoCameraType
      // cref: ArcGIS.Core.CIM.AutoCameraType
      // cref: ArcGIS.Core.CIM.AutoCameraSource
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.Camera
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.IntersectLayerPath
      #region SetAutoCameraLinkedMapSeriesShape

      var layout = LayoutView.Active.Layout;
      var mf = layout.GetElementsAsFlattenedList().OfType<MapFrame>()
        .First(mf => mf.Name == mapFrame);
      var autoCamera = mf.GetAutoCamera();
      autoCamera.Source = AutoCameraSource.MapSeriesLink;
      autoCamera.AutoCameraType = AutoCameraType.Extent;
      //autoCamera.MapFrameLinkName = mapFrameLink;
      var states = mf.Map.GetLayersAsFlattenedList().First(l => l.Name == "State_Polygons");
      autoCamera.IntersectLayerPath = states.URI;

      if (mf.ValidateAutoCamera(autoCamera) &&
        !mf.IsMapSeriesMapFrame())
        mf.SetAutoCamera(autoCamera);

      #endregion
    }

    public void SetAutoCameraLinkedMapSeriesCenter(string mapFrame)
    {
      // cref: ArcGIS.Desktop.Layouts.MapFrame.GetAutoCamera
      // cref: ArcGIS.Desktop.Layouts.MapFrame.SetAutoCamera
      // cref: ArcGIS.Desktop.Layouts.MapFrame.IsMapSeriesMapFrame
      // cref: ArcGIS.Desktop.Layouts.MapFrame.ValidateAutoCamera
      // cref: ArcGIS.Core.CIM.CIMAutoCamera
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.Source
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.AutoCameraType
      // cref: ArcGIS.Core.CIM.AutoCameraType
      // cref: ArcGIS.Core.CIM.AutoCameraSource
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.Camera
      // cref: ArcGIS.Core.CIM.CIMAutoCamera.IntersectLayerPath
      #region SetAutoCameraLinkedMapSeriesCenter

      var layout = LayoutView.Active.Layout;
      var mf = layout.GetElementsAsFlattenedList().OfType<MapFrame>()
        .First(mf => mf.Name == mapFrame);
      var autoCamera = mf.GetAutoCamera();
      autoCamera.Source = AutoCameraSource.MapSeriesLink;
      autoCamera.AutoCameraType = AutoCameraType.Center;

      var states = mf.Map.GetLayersAsFlattenedList().First(l => l.Name == "State_Polygons");
      autoCamera.IntersectLayerPath = states.URI;

      if (mf.ValidateAutoCamera(autoCamera) &&
        !mf.IsMapSeriesMapFrame())
        mf.SetAutoCamera(autoCamera);

      #endregion
    }

  }
}
