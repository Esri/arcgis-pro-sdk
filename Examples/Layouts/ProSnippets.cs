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
      #region Reference layout project items and their associated layout
      //Reference layout project items and their associated layout.
      //A layout project item is an item that appears in the Layouts folder in the Catalog pane.

      //Reference all the layout project items
      IEnumerable<LayoutProjectItem> layouts = Project.Current.GetItems<LayoutProjectItem>();

      //Or reference a specific layout project item by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals("MyLayout"));
      #endregion

      #region Open a layout project item in a new view
      //Open a layout project item in a new view.
      //A layout project item may exist but it may not be open in a view. 

      //Reference a layout project item by name
      LayoutProjectItem someLytItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals("MyLayout"));

      //Get the layout associated with the layout project item
      Layout layout = await QueuedTask.Run(() => someLytItem.GetLayout());  //Worker thread

      //Create the new pane
      ILayoutPane iNewLayoutPane = await ProApp.Panes.CreateLayoutPaneAsync(layout); //GUI thread
      #endregion

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
        if (layoutPane.LayoutView.Layout == layout) //if there is a match, activate the view
        {
          (layoutPane as Pane).Activate();
          return;
        }
      }
      #endregion

      #region Reference the active layout view
      //Reference the active layout view.

      //Confirm if the current, active view is a layout view.  If it is, do something.
      LayoutView activeLayoutView = LayoutView.Active;
      if (activeLayoutView != null)
      {
        // do something
      }
      #endregion

      #region Import a pagx into a project
      //Import a pagx into a project.

      //Create a layout project item from importing a pagx file
      IProjectItem pagx = ItemFactory.Instance.Create(@"C:\Temp\Layout.pagx") as IProjectItem;
      Project.Current.AddItem(pagx);
      #endregion

      #region Remove a layout project item
      //Remove a layout project item.

      //Remove the layout fro the project
      Project.Current.RemoveItem(layoutItem);
      #endregion

      #region Create a new, basic layout and open it
      //Create a new, basic layout and open it.

      //Create layout with minimum set of parameters on the worker thread
      Layout newLayout = await QueuedTask.Run<Layout>(() =>
      {
        newLayout = LayoutFactory.Instance.CreateLayout(8.5, 11, LinearUnit.Inches);
        newLayout.SetName("New 8.5x11 Layout");
        return newLayout;
      });
      
      //Open new layout on the GUI thread
      await ProApp.Panes.CreateLayoutPaneAsync(newLayout);
      #endregion

      #region Create a new layout using a modified CIM and open it
      //Create a new layout using a modified CIM and open it.
      //The CIM exposes additional members that may not be available through the managed API.  
      //In this example, optional guides are added.

      //Create a new CIMLayout on the worker thread
      Layout newCIMLayout = await QueuedTask.Run<Layout>(() =>
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
        newCIMLayout = LayoutFactory.Instance.CreateLayout(newPage);
        newCIMLayout.SetName("New 8.5x11 Layout");
        return newCIMLayout;
      });

      //Open new layout on the GUI thread
      await ProApp.Panes.CreateLayoutPaneAsync(newCIMLayout);
      #endregion

      #region Change the layout page size
      //Change the layout page size.

      //Reference the layout project item
      LayoutProjectItem lytItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals("MyLayout"));
      if (layoutItem != null)
      {
        await QueuedTask.Run(() =>
        {
          //Get the layout
          Layout lyt = lytItem.GetLayout();
          if (layout != null)
          {
            //Change properties
            CIMPage page = layout.GetPage();
            page.Width = 8.5;
            page.Height = 11;

            //Apply the changes to the layout
            layout.SetPage(page);
          }
        });
      }
      #endregion
    }
        #region ProSnippet Group: Create Layout Elements
        #endregion
    public async void snippets_CreateLayoutElements()
    {
      LayoutView layoutView = LayoutView.Active;
      Layout layout = layoutView.Layout;
      await QueuedTask.Run(() =>
      {
        #region Create Element using the CIMElement
        //on the QueuedTask
        //Place symbol on the layout
        MapPoint point = MapPointBuilder.CreateMapPoint(new Coordinate2D(9, 1));

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
        LayoutElementFactory.Instance.CreateElement(layout, graphic);
        #endregion
      });

      await QueuedTask.Run(() =>
      {
        #region Create Graphic Element using CIMGraphic
        //on the QueuedTask
        //Place symbol on the layout
        MapPoint location = MapPointBuilder.CreateMapPoint(new Coordinate2D(9, 1));

        //specify a symbol
        var pt_symbol = SymbolFactory.Instance.ConstructPointSymbol(
                              ColorFactory.Instance.GreenRGB);

        //create a CIMGraphic 
        var graphic = new CIMPointGraphic()
        {
          Symbol = pt_symbol.MakeSymbolReference(),
          Location = location //center of map
        };
        LayoutElementFactory.Instance.CreateGraphicElement(layout, graphic);
        #endregion
      });      

      await QueuedTask.Run(() =>
      {
        #region Create Graphic Element using CIMSymbol
        //on the QueuedTask
        //Place symbol on the layout
        MapPoint location = MapPointBuilder.CreateMapPoint(new Coordinate2D(9, 1));

        //specify a symbol
        var pt_symbol = SymbolFactory.Instance.ConstructPointSymbol(
                              ColorFactory.Instance.GreenRGB);
        LayoutElementFactory.Instance.CreateGraphicElement(layout, location, pt_symbol);
        #endregion
      });
      await QueuedTask.Run(() => {
        #region Bulk Element creation
        //on the QueuedTask
        //List of Point graphics
        var listGraphics = new List<CIMPointGraphic>();
        //Symbol
        var pointSymbol = SymbolFactory.Instance.ConstructPointSymbol(ColorFactory.Instance.BlackRGB);
        //Define size of the array
        int dx = 5;
        int dy = 5;
        MapPoint point = null;
        //Create the List of graphics for the array
        for (int row = 0; row <= dx; ++row)
        {
          for (int col = 0; col <= dy; ++col)
          {
            point = MapPointBuilder.CreateMapPoint(col, row);
            //create a CIMGraphic 
            var graphic = new CIMPointGraphic()
            {
              Symbol = pointSymbol.MakeSymbolReference(),
              Location = point //center of map
            };
            listGraphics.Add(graphic);
          }
        }
        //Draw the array of graphics
        var bulkgraphics = LayoutElementFactory.Instance.CreateGraphicElements(layout, listGraphics, null);
        #endregion
      });

      #region Create point graphic with symbology
      //Create a simple 2D point graphic and apply an existing point style item as the symbology.

      //Construct on the worker thread
      await QueuedTask.Run(() =>
      {
        //Build 2D point geometry  
        Coordinate2D coord2D = new Coordinate2D(2.0, 10.0);

        //(optionally) Reference a point symbol in a style
        StyleProjectItem ptStylePrjItm = Project.Current.GetItems<StyleProjectItem>().FirstOrDefault(item => item.Name == "ArcGIS 2D");
        SymbolStyleItem ptSymStyleItm = ptStylePrjItm.SearchSymbols(StyleItemType.PointSymbol, "City Hall")[0];
        CIMPointSymbol pointSym = ptSymStyleItm.Symbol as CIMPointSymbol;
        pointSym.SetSize(50);

        //Set symbolology, create and add element to layout

        //An alternative simple symbol is also commented out below.  This would elminate the four 
        //optional lines of code above that reference a style.

        //CIMPointSymbol pointSym = SymbolFactory.Instance.ConstructPointSymbol(ColorFactory.Instance.RedRGB, 25.0, SimpleMarkerStyle.Star);  //Alternative simple symbol
        GraphicElement ptElm = LayoutElementFactory.Instance.CreatePointGraphicElement(layout, coord2D, pointSym);
        ptElm.SetName("New Point");
      });
      #endregion

      #region Create line graphic with symbology
      //Create a simple 2D line graphic and apply an existing line style item as the symbology.

      //Construct on the worker thread
      await QueuedTask.Run(() =>
      {
        //Build 2d line geometry
        List<Coordinate2D> plCoords = new List<Coordinate2D>();
        plCoords.Add(new Coordinate2D(1, 8.5));
        plCoords.Add(new Coordinate2D(1.66, 9));
        plCoords.Add(new Coordinate2D(2.33, 8.1));
        plCoords.Add(new Coordinate2D(3, 8.5));
        Polyline linePl = PolylineBuilder.CreatePolyline(plCoords);

        //(optionally) Reference a line symbol in a style
        StyleProjectItem lnStylePrjItm = Project.Current.GetItems<StyleProjectItem>().FirstOrDefault(item => item.Name == "ArcGIS 2D");
        SymbolStyleItem lnSymStyleItm = lnStylePrjItm.SearchSymbols(StyleItemType.LineSymbol, "Line with 2 Markers")[0];
        CIMLineSymbol lineSym = lnSymStyleItm.Symbol as CIMLineSymbol;
        lineSym.SetSize(20);

        //Set symbolology, create and add element to layout

        //An alternative simple symbol is also commented out below.  This would elminate the four 
        //optional lines of code above that reference a style.

        //CIMLineSymbol lineSym = SymbolFactory.Instance.ConstructLineSymbol(ColorFactory.Instance.BlueRGB, 4.0, SimpleLineStyle.Solid);  //Alternative simple symbol
        GraphicElement lineElm = LayoutElementFactory.Instance.CreateLineGraphicElement(layout, linePl, lineSym);
        lineElm.SetName("New Line");
      });
      #endregion

      #region Create rectangle graphic with simple symbology
      //Create a simple 2D rectangle graphic and apply simple fill and outline symbols.

      //Construct on the worker thread
      await QueuedTask.Run(() =>
      {
        //Build 2D envelope geometry
        Coordinate2D rec_ll = new Coordinate2D(1.0, 4.75);
        Coordinate2D rec_ur = new Coordinate2D(3.0, 5.75);
        Envelope rec_env = EnvelopeBuilder.CreateEnvelope(rec_ll, rec_ur);

        //Set symbolology, create and add element to layout
        CIMStroke outline = SymbolFactory.Instance.ConstructStroke(ColorFactory.Instance.BlackRGB, 5.0, SimpleLineStyle.Solid);
        CIMPolygonSymbol polySym = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.GreenRGB, SimpleFillStyle.DiagonalCross, outline);
        GraphicElement recElm = LayoutElementFactory.Instance.CreateRectangleGraphicElement(layout, rec_env, polySym);
        recElm.SetName("New Rectangle");
      });
      #endregion

      #region Create text element with basic font properties
      //Create a simple point text element and assign basic symbology and text settings.

      //Construct on the worker thread
      await QueuedTask.Run(() =>
      {
        //Build 2D point geometry
        Coordinate2D coord2D = new Coordinate2D(3.5, 10);

        //Set symbolology, create and add element to layout
        CIMTextSymbol sym = SymbolFactory.Instance.ConstructTextSymbol(ColorFactory.Instance.RedRGB, 32, "Arial", "Regular");
        string textString = "Point text";
        GraphicElement ptTxtElm = LayoutElementFactory.Instance.CreatePointTextGraphicElement(layout, coord2D, textString, sym);
        ptTxtElm.SetName("New Point Text");

        //Change additional text properties
        ptTxtElm.SetAnchor(Anchor.CenterPoint);
        ptTxtElm.SetX(4.5);
        ptTxtElm.SetY(9.5);
        ptTxtElm.SetRotation(45);
      });
      #endregion

      #region Create rectangle text with more advanced symbol settings
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
        Polygon poly = PolygonBuilder.CreatePolygon(plyCoords);

        //Set symbolology, create and add element to layout
        //Also notice how formatting tags are using within the text string.
        CIMTextSymbol sym = SymbolFactory.Instance.ConstructTextSymbol(ColorFactory.Instance.GreyRGB, 10, "Arial", "Regular");
        string text = "Some Text String that is really long and is <BOL>forced to wrap to other lines</BOL> so that we can see the effects." as String;
        GraphicElement polyTxtElm = LayoutElementFactory.Instance.CreatePolygonParagraphGraphicElement(layout, poly, text, sym);
        polyTxtElm.SetName("New Polygon Text");

        //(Optionally) Modify paragraph border 
        CIMGraphic polyTxtGra = polyTxtElm.GetGraphic();
        CIMParagraphTextGraphic cimPolyTxtGra = polyTxtGra as CIMParagraphTextGraphic;
        cimPolyTxtGra.Frame.BorderSymbol = new CIMSymbolReference();
        cimPolyTxtGra.Frame.BorderSymbol.Symbol = SymbolFactory.Instance.ConstructLineSymbol(ColorFactory.Instance.GreyRGB, 1.0, SimpleLineStyle.Solid);
        polyTxtElm.SetGraphic(polyTxtGra);
      });
      #endregion

      #region Create a new picture element with advanced symbol settings
      //Create a picture element and also set background and border symbology.

      //Construct on the worker thread
      await QueuedTask.Run(() =>
      {
        //Build 2D envelope geometry
        Coordinate2D pic_ll = new Coordinate2D(6, 1);
        Coordinate2D pic_ur = new Coordinate2D(8, 2);
        Envelope env = EnvelopeBuilder.CreateEnvelope(pic_ll, pic_ur);

        //Create and add element to layout
        string picPath = @"C:\Temp\WhitePass.jpg";
        GraphicElement picElm = LayoutElementFactory.Instance.CreatePictureGraphicElement(layout, env, picPath);
        picElm.SetName("New Picture");

        //(Optionally) Modify the border and shadow 
        CIMGraphic picGra = picElm.GetGraphic();
        CIMPictureGraphic cimPicGra = picGra as CIMPictureGraphic;
        cimPicGra.Frame.BorderSymbol = new CIMSymbolReference();
        cimPicGra.Frame.BorderSymbol.Symbol = SymbolFactory.Instance.ConstructLineSymbol(ColorFactory.Instance.BlueRGB, 2.0, SimpleLineStyle.Solid);

        cimPicGra.Frame.ShadowSymbol = new CIMSymbolReference();
        cimPicGra.Frame.ShadowSymbol.Symbol = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.BlackRGB, SimpleFillStyle.Solid);

        //Update the element
        picElm.SetGraphic(picGra);
      });
      #endregion

      #region Create a map frame and zoom to a bookmark
      //Create a map frame and set its camera by zooming to the extent of an existing bookmark.

      //Construct on the worker thread
      await QueuedTask.Run(() =>
      {
        //Build 2D envelope geometry
        Coordinate2D mf_ll = new Coordinate2D(6.0, 8.5);
        Coordinate2D mf_ur = new Coordinate2D(8.0, 10.5);
        Envelope mf_env = EnvelopeBuilder.CreateEnvelope(mf_ll, mf_ur);

        //Reference map, create MF and add to layout
        MapProjectItem mapPrjItem = Project.Current.GetItems<MapProjectItem>().FirstOrDefault(item => item.Name.Equals("Map"));
        Map mfMap = mapPrjItem.GetMap();
        MapFrame mfElm = LayoutElementFactory.Instance.CreateMapFrame(layout, mf_env, mfMap);
        mfElm.SetName("New Map Frame");

        //Zoom to bookmark
        Bookmark bookmark = mfElm.Map.GetBookmarks().FirstOrDefault(b => b.Name == "Great Lakes");
        mfElm.SetCamera(bookmark);
      });
      #endregion

      #region Apply a background color to a MapFrame element
      //Apply a background color to the map frame element using the CIM.

      //Perform on the worker thread
      await QueuedTask.Run(() =>
      {
        //Get the layout
        var myLayout = Project.Current.GetItems<LayoutProjectItem>()?.First().GetLayout();
        if (myLayout == null) return;

        //Get the map frame in the layout
        MapFrame mapFrame = myLayout.FindElement("New Map Frame") as MapFrame;
        if (mapFrame == null)
        {
          ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Map frame not found", "WARNING");
          return;
        }

        //Get the map frame's definition in order to modify the background.
        var mapFrameDefn = mapFrame.GetDefinition() as CIMMapFrame;

        //Construct the polygon symbol to use to create a background
        var polySymbol = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.BlueRGB, SimpleFillStyle.Solid);

        //Set the background
        mapFrameDefn.GraphicFrame.BackgroundSymbol = polySymbol.MakeSymbolReference();

        //Set the map frame definition
        mapFrame.SetDefinition(mapFrameDefn);
      });
      #endregion

      #region Create a legend for a specific map frame
      //Create a legend for an associated map frame.

      //Construct on the worker thread
      await QueuedTask.Run(() =>
      {
        //Build 2D envelope geometry
        Coordinate2D leg_ll = new Coordinate2D(6, 2.5);
        Coordinate2D leg_ur = new Coordinate2D(8, 4.5);
        Envelope leg_env = EnvelopeBuilder.CreateEnvelope(leg_ll, leg_ur);

        //Reference MF, create legend and add to layout
        MapFrame mapFrame = layout.FindElement("New Map Frame") as MapFrame;
        if (mapFrame == null)
        {
          ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Map frame not found", "WARNING");
          return;
        }
        Legend legendElm = LayoutElementFactory.Instance.CreateLegend(layout, leg_env, mapFrame);
        legendElm.SetName("New Legend");
      });
      #endregion

      #region Creating empty group elements
      //Create an empty group element at the root level of the contents pane

      //Create on worker thread
      await QueuedTask.Run(() =>
      {
        GroupElement grp1 = LayoutElementFactory.Instance.CreateGroupElement(layout);
        grp1.SetName("Group");
      });

      // *** or ***

      //Create a group element inside another group element

      //Find an existing group element
      GroupElement existingGroup = layout.FindElement("Group") as GroupElement;
      
      //Create on worker thread
      await QueuedTask.Run(() =>
      {
        GroupElement grp2 = LayoutElementFactory.Instance.CreateGroupElement(existingGroup);
        grp2.SetName("Group in Group");
      });
      #endregion

      #region Create a group element with elements
      //Create a group with a list of elements at the root level of the contents pane.

      //Find an existing elements
      Element scaleBar = layout.FindElement("Scale Bar") as Element;
      Element northArrow = layout.FindElement("North Arrow") as Element;
      Element legend = layout.FindElement("Legend") as Element;

      //Construct a list and add the elements
      List<Element> elmList = new List<Element>
      {
        scaleBar,
        northArrow,
        legend
      };

      //Perform on the worker thread
      await QueuedTask.Run(() =>
      {
        GroupElement groupWithListOfElementsAtRoot = LayoutElementFactory.Instance.CreateGroupElement(layout, elmList);
        groupWithListOfElementsAtRoot.SetName("Group with list of elements at root");
      });


      // *** or ***


      //Create a group using a list of element names at the root level of the contents pane.

      //List of element names
      var elmNameList = new[] { "Table Frame", "Chart Frame" };

      //Perform on the worker thread
      await QueuedTask.Run(() =>
      {
        GroupElement groupWithListOfElementNamesAtRoot = LayoutElementFactory.Instance.CreateGroupElement(layout, elmNameList);
        groupWithListOfElementNamesAtRoot.SetName("Group with list of element names at root");
      });
      #endregion

      #region Create a scale bar using a style
      //Create a scale bar using a style.

      //Search for a style project item by name
      StyleProjectItem arcgis_2dStyle = Project.Current.GetItems<StyleProjectItem>().First(si => si.Name == "ArcGIS 2D");

      //Construct on the worker thread
      await QueuedTask.Run(() =>
      {
        //Reference the specific scale bar by name 
        ScaleBarStyleItem scaleBarItem = arcgis_2dStyle.SearchScaleBars("Double Alternating Scale Bar").FirstOrDefault();

        //Reference the map frame and define the location
        MapFrame myMapFrame = layout.FindElement("Map Frame") as MapFrame;
        Coordinate2D coord2D = new Coordinate2D(10.0, 7.0);

        //Construct the scale bar
        LayoutElementFactory.Instance.CreateScaleBar(layout, coord2D, myMapFrame, scaleBarItem);
      });
      #endregion

      #region Create a north arrow using a style
      //Create a north arrow using a style.

      //Search for a style project item by name
      StyleProjectItem arcgis2dStyles = Project.Current.GetItems<StyleProjectItem>().First(si => si.Name == "ArcGIS 2D");
      
      //Construct on the worker thread
      await QueuedTask.Run(() => 
      {
        NorthArrowStyleItem naStyleItem = arcgis2dStyles.SearchNorthArrows("ArcGIS North 13").FirstOrDefault();

        //Reference the map frame and define the location
        MapFrame newFrame = layout.FindElement("New Map Frame") as MapFrame;
        Coordinate2D nArrow = new Coordinate2D(6, 2.5);
        
        //Construct the north arrow
        var newNorthArrow = LayoutElementFactory.Instance.CreateNorthArrow(layout, nArrow, newFrame, naStyleItem);
      });
      #endregion

      #region Create a dynamic text element
      //Create a dynamic text element.

      //Set the string with tags and the location
      String title = @"<dyn type = ""page"" property = ""name"" />";
      Coordinate2D llTitle = new Coordinate2D(6, 2.5);

      //Construct element on worker thread
      await QueuedTask.Run(() =>
      {
        //Create with default text properties
        TextElement titleGraphics = LayoutElementFactory.Instance.CreatePointTextGraphicElement(layout, llTitle, null) as TextElement;

        //Modify the text properties
        titleGraphics.SetTextProperties(new TextProperties(title, "Arial", 24, "Bold"));
      });
      #endregion

      #region Create a table frame
      //Create a table frame.

      //Construct on the worker thread
      await QueuedTask.Run(() =>
      {
        //Build 2D envelope geometry
        Coordinate2D rec_ll = new Coordinate2D(1.0, 3.5);
        Coordinate2D rec_ur = new Coordinate2D(7.5, 4.5);
        Envelope rec_env = EnvelopeBuilder.CreateEnvelope(rec_ll, rec_ur);

        //Reference map frame and layer
        MapFrame mf = layout.FindElement("Map Frame") as MapFrame;
        Map m = mf.Map;
        FeatureLayer lyr = m.FindLayers("GreatLakes").First() as FeatureLayer;

        //Build fields list
        var fields = new[] { "NAME", "Shape_Area", "Shape_Length" };

        //Construct the table frame
        TableFrame tabFrame = LayoutElementFactory.Instance.CreateTableFrame(layout, rec_env, mf, lyr, fields);
      });
      #endregion        
    }


        #region ProSnippet Group: Layout Elements & Selection
        #endregion
        public void snippets_elements( Layout layout)
    {
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
          if (layout != null)
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
        ////Find all text graphics in the Graphics Layer
        var textGraphics = graphicElements.Where(elem => elem.GetGraphic() is CIMTextGraphic);
        ////Find all picture graphics in the Graphics Layer
        var pictureGraphic = graphicElements.Where(elem => elem.GetGraphic() is CIMPictureGraphic);
        #endregion
      });

      Element element = null;
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
      #region UnSelect elements on the Layout
      //Unselect one element.
      var elementToUnSelect = layout.FindElements(new List<string>() { "MyPoint" }).FirstOrDefault();
      layout.UnSelectElement(elementToUnSelect);
      //Unselect multiple elements.
      var elementsToUnSelect = layout.FindElements(new List<string>() { "Point 1", "Line 3", "Text 1" });
      layout.UnSelectElements(elementsToUnSelect);
      #endregion

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
        #region Clear the selection in a layout view
        //If the a layout view is active, clear its selection
        LayoutView activeLayoutView = LayoutView.Active;
        if (activeLayoutView != null)
        {          
          activeLayoutView.ClearElementSelection();
        }
        #endregion
        #region Clear the selection in a layout 
        //Clear the layout selection.
        layout.ClearElementSelection();
        #endregion
      }
      Layout aLayout = null;
      Element elm = null;

      #region Copy Layout Elements
      //on the QueuedTask
      var elems = layout.FindElements(new List<string>() { "Point 1", "Line 3", "Text 1" });
      var copiedElements = layout.CopyElements(elems);
      #endregion
      #region Delete Layout Elements
      //on the QueuedTask  
      var elementsToRemove = layout.GetSelectedElements();
      layout.DeleteElements(elementsToRemove);
      #endregion
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

      #region Zoom to elements
      LayoutView lytView = LayoutView.Active;
      //Zoom to an element
      var elementToZoomTo = layout.FindElements(new List<string>() { "MyPoint" }).FirstOrDefault();
      lytView.ZoomToElement(elementToZoomTo);
      //Zoom to  multiple elements.
      var elementsToZoomTo = layout.FindElements(new List<string>() { "Point 1", "Line 3", "Text 1" });
      lytView.ZoomToElements(elementsToZoomTo);

      #endregion



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
        
        #region Group Graphic Elements
        //on the QueuedTask
        var elemsToGroup = layout.GetSelectedElements();
        //Note: run within the QueuedTask
        //group  elements
        var groupElement = layout.GroupElements(elemsToGroup);
        #endregion

        #region Un-Group Graphic Elements
        var selectedElements = layout.GetSelectedElements().ToList(); ;
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

        #region Parent of GroupElement
        //check the parent
        var parent = groupElement.Elements.First().GetParent();//will be the group element
        //top-most parent
        var top_most = groupElement.Elements.First().GetParent(true);//will be the GraphicsLayer
        #endregion
        #region Children in a Group Element
        // Nested groups within ArcGIS.Desktop.Layouts.GroupElement are not preserved.
        var children = groupElement.GetElementsAsFlattenedList();
        #endregion

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

        #region Get Z-Order
        var selElementsZOrder = layout.GetSelectedElements();
        //list out the z order
        foreach (var elem in selElementsZOrder)
          System.Diagnostics.Debug.WriteLine($"{elem.Name}: z-order {elem.GetZOrder()}");
        #endregion
      });
    }

    #region ProSnippet Group: Update Layout Elements
    #endregion
    public void snippets_UpdateElements()
    {
      double x = 0;
      double y = 0;

      #region Update text element properties
      //Update text element properties for an existing text element.

      // Reference a layoutitem in a project by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals("MyLayout"));
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
              TextProperties txtProperties = new TextProperties("Hello world", "Times New Roman", 48, "Regular");
              txtElm.SetTextProperties(txtProperties);
            }
          }
        });
      }
      #endregion

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

      #region Lock an element
      // The Locked property is displayed in the TOC as a lock symbol next to each element.  
      // If locked the element can't be selected in the layout using the graphic selection tools.

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
            CIMGraphic.Transparency = 50;             // mark it 50% transparent
            graphicElement.SetGraphic(CIMGraphic);
          }
        }
      });
      #endregion

      double xOffset = 0;
      double yOffset = 0;
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
          GraphicElement graphicElement = layout.FindElement("MyElement") as GraphicElement;
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

    #region ProSnippet Group: Layout Metadata
    #endregion

    private void LayoutMetadata(Layout layout)
    {
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

    private void TranslatePointInMapFrameToMapView()
    {

      QueuedTask.Run(() => {
        var pointSymbol = SymbolFactory.Instance.ConstructPointSymbol(ColorFactory.Instance.BlackRGB, 8);
        var graphicsLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<GraphicsLayer>().FirstOrDefault();
        #region Translates a point in page coordinates to a point in map coordinates. 
        //On the QueuedTask
        var layout = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault().GetLayout();
        var mapFrame = layout.FindElement("New Map Frame") as MapFrame;

        //Get a point in the center of the Map frame
        var mapFrameCenterPoint = mapFrame.GetBounds().CenterCoordinate;
        //Convert to MapPoint
        var pointInMapFrame = MapPointBuilder.CreateMapPoint(mapFrameCenterPoint);

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
          LayoutElementFactory.Instance.CreateGraphicElement(layout, cimGraphicElement);
        });
        
      }
    }
    #endregion

    #region ProSnippet Group: Layout MapSeries
    #endregion
    async public void snippets_MapSeries()
    {
      Layout layout = LayoutView.Active.Layout;

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

      #region Export a map frame to JPG
      //Export a map frame to JPG.

      //Create JPEG format with appropriate settings
      //BMP, EMF, EPS, GIF, PDF, PNG, SVG, TGA, and TFF formats are also available for export
      JPEGFormat JPG = new JPEGFormat()
      {
        HasWorldFile = true,
        Resolution = 300,
        OutputFileName = filePath,
        JPEGColorMode = JPEGColorMode.TwentyFourBitTrueColor,
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

      #region Export a map series to individual TIFF files
      //Export each page of a map series to an individual TIFF file.

      //Create TIFF format with appropriate settings
      TIFFFormat TIFF = new TIFFFormat()
      {
        OutputFileName = filePath,
        Resolution = 300,
        ColorMode = ColorMode.TwentyFourBitTrueColor,
        HasGeoTiffTags = true,
        HasWorldFile = true,
        TIFFImageCompression = TIFFImageCompression.LZW
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
  }
}
