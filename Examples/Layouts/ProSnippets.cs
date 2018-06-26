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

namespace ProSnippetsTasks
{
  class Snippets
  {
    async public void snippets_ProjectItems()
    {
      #region Reference layout project items

      //A layout project item is an item that appears in the Layouts folder in the Catalog pane

      //Reference all the layout project items
      IEnumerable<LayoutProjectItem> layouts = Project.Current.GetItems<LayoutProjectItem>();

      //Or reference a specific layout project item by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals("MyLayout"));
      #endregion

      #region Open a layout project item in a new view
      //A layout project item may be in a project but it may not be open in a view and/or active        
      //First get the layout associated with the layout project item
      Layout layout = layoutItem.GetLayout();
      //Open a new pane
      ILayoutPane iNewLayoutPane = await ProApp.Panes.CreateLayoutPaneAsync(layout);
      #endregion
      
      #region Activate an already open layout view
      //A layout view may exist but it may not be active

      //Iterate through each pane in the application and check to see if the layout is already open and if so, activate it
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
      //First check to see if the current, active view is a layout view.  If it is, use it
      LayoutView activeLayoutView = LayoutView.Active;
      if (activeLayoutView != null)
      {
        // use activeLayoutView
      }
      #endregion



      #region Create a new, basic layout and open it
      //Create a new layout project item layout in the project
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
      //Create a new layout project item layout in the project
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

        //Create a new page
        newCIMLayout = LayoutFactory.Instance.CreateLayout(newPage);
        newCIMLayout.SetName("New 8.5x11 Layout");
        return newCIMLayout;
      });
      //Open new layout on the GUI thread
      await ProApp.Panes.CreateLayoutPaneAsync(newCIMLayout);
      #endregion
    
	

	    #region Import a pagx into a project
      //Create a layout project item from importing a pagx file
      IProjectItem pagx = ItemFactory.Instance.Create(@"C:\Temp\Layout.pagx") as IProjectItem;
      Project.Current.AddItem(pagx);
      #endregion


      #region Remove a layout project item
      //Remove a layout from the project completely
      Project.Current.RemoveItem(layoutItem);
      #endregion

    }

    public void snippets_CreateLayoutElements()
    {
      LayoutView layoutView = LayoutView.Active;
      Layout layout = layoutView.Layout;

      #region Create point graphic with symbology

      //Create a simple 2D point graphic and apply an existing point style item as the symbology.
      //An alternative simple symbol is also provided below.  This would completely elminate the 4 lines of code that reference a style.

      QueuedTask.Run(() =>
      {
        //Build 2D point geometry  
        Coordinate2D coord2D = new Coordinate2D(2.0, 10.0);

        //Reference a point symbol in a style
        StyleProjectItem ptStylePrjItm = Project.Current.GetItems<StyleProjectItem>().FirstOrDefault(item => item.Name == "ArcGIS 2D");
        SymbolStyleItem ptSymStyleItm = ptStylePrjItm.SearchSymbols(StyleItemType.PointSymbol, "City Hall")[0];
        CIMPointSymbol pointSym = ptSymStyleItm.Symbol as CIMPointSymbol;
        pointSym.SetSize(50);

        //Set symbolology, create and add element to layout
        //CIMPointSymbol pointSym = SymbolFactory.Instance.ConstructPointSymbol(ColorFactory.Instance.RedRGB, 25.0, SimpleMarkerStyle.Star);  //Alternative simple symbol
        GraphicElement ptElm = LayoutElementFactory.Instance.CreatePointGraphicElement(layout, coord2D, pointSym);
        ptElm.SetName("New Point");
      });
      #endregion

      #region Create line graphic with symbology

      //Create a simple 2D line graphic and apply an existing line style item as the symbology.
      //An alternative simple symbol is also provided below.  This would completely elminate the 4 lines of code that reference a style.

      QueuedTask.Run(() =>
      { 
        //Build 2d line geometry
        List<Coordinate2D> plCoords = new List<Coordinate2D>();
        plCoords.Add(new Coordinate2D(1, 8.5));
        plCoords.Add(new Coordinate2D(1.66, 9));
        plCoords.Add(new Coordinate2D(2.33, 8.1));
        plCoords.Add(new Coordinate2D(3, 8.5));
        Polyline linePl = PolylineBuilder.CreatePolyline(plCoords);

        //Reference a line symbol in a style
        StyleProjectItem lnStylePrjItm = Project.Current.GetItems<StyleProjectItem>().FirstOrDefault(item => item.Name == "ArcGIS 2D");
        SymbolStyleItem lnSymStyleItm = lnStylePrjItm.SearchSymbols(StyleItemType.LineSymbol, "Line with 2 Markers")[0];
        CIMLineSymbol lineSym = lnSymStyleItm.Symbol as CIMLineSymbol;
        lineSym.SetSize(20);

        //Set symbolology, create and add element to layout
        //CIMLineSymbol lineSym = SymbolFactory.Instance.ConstructLineSymbol(ColorFactory.Instance.BlueRGB, 4.0, SimpleLineStyle.Solid);  //Alternative simple symbol
        GraphicElement lineElm = LayoutElementFactory.Instance.CreateLineGraphicElement(layout, linePl, lineSym);
        lineElm.SetName("New Line");
      });
      #endregion

      #region Create rectangle graphic with simple symbology

      //Create a simple 2D rectangle graphic and apply simple fill and outline symbols.

      QueuedTask.Run(() =>
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
     
      //Create a simple point text element and assign basic symbology as well as basic text settings.

      QueuedTask.Run(() =>
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

      //Create rectangle text with background and border symbology.  Also notice how formatting tags are using within the text string.

      QueuedTask.Run(() =>
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
        CIMTextSymbol sym = SymbolFactory.Instance.ConstructTextSymbol(ColorFactory.Instance.GreyRGB, 10, "Arial", "Regular");
        string text = "Some Text String that is really long and is <BOL>forced to wrap to other lines</BOL> so that we can see the effects." as String;
        GraphicElement polyTxtElm = LayoutElementFactory.Instance.CreatePolygonParagraphGraphicElement(layout, poly, text, sym);
        polyTxtElm.SetName("New Polygon Text");

        //(Optionally) Modify paragraph border 
        CIMGraphic polyTxtGra = polyTxtElm.Graphic;
        CIMParagraphTextGraphic cimPolyTxtGra = polyTxtGra as CIMParagraphTextGraphic;
        cimPolyTxtGra.Frame.BorderSymbol = new CIMSymbolReference();
        cimPolyTxtGra.Frame.BorderSymbol.Symbol = SymbolFactory.Instance.ConstructLineSymbol(ColorFactory.Instance.GreyRGB, 1.0, SimpleLineStyle.Solid);
        polyTxtElm.SetGraphic(polyTxtGra);
      });
      #endregion

      #region Create a new picture element with advanced symbol settings

      //Create a picture element and also set background and border symbology.

      QueuedTask.Run(() =>
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
        CIMGraphic picGra = picElm.Graphic;
        CIMPictureGraphic cimPicGra = picGra as CIMPictureGraphic;
        cimPicGra.Frame.BorderSymbol = new CIMSymbolReference();
        cimPicGra.Frame.BorderSymbol.Symbol = SymbolFactory.Instance.ConstructLineSymbol(ColorFactory.Instance.BlueRGB, 2.0, SimpleLineStyle.Solid);

        cimPicGra.Frame.ShadowSymbol = new CIMSymbolReference();
        cimPicGra.Frame.ShadowSymbol.Symbol = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.BlackRGB, SimpleFillStyle.Solid);

        picElm.SetGraphic(picGra);
      });
      #endregion

      #region Create a map frame and zoom to a bookmark

      //Create a map frame and also sets its extent by zooming the the extent of an existing bookmark.

      QueuedTask.Run(() =>
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

      #region Create a legend for a specifc map frame

      //Create a legend for an associated map frame.

      QueuedTask.Run(() =>
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

      #region Creating group elements
      //Create an empty group element at the root level of the contents pane
      //Note: call within QueuedTask.Run()
      GroupElement grp1 = LayoutElementFactory.Instance.CreateGroupElement(layout);
      grp1.SetName("Group");

      //Create a group element inside another group element
      //Note: call within QueuedTask.Run()
      GroupElement grp2 = LayoutElementFactory.Instance.CreateGroupElement(grp1);
      grp2.SetName("Group in Group");
      #endregion Creating group elements

      {
        #region Create scale bar
        Coordinate2D llScalebar = new Coordinate2D(6, 2.5);
        MapFrame mapframe = layout.FindElement("New Map Frame") as MapFrame;
        //Note: call within QueuedTask.Run()
        LayoutElementFactory.Instance.CreateScaleBar(layout, llScalebar, mapframe);
        #endregion
      }
        #region How to search for scale bars in a style
        var arcgis_2d = Project.Current.GetItems<StyleProjectItem>().First(si => si.Name == "ArcGIS 2D");
        QueuedTask.Run(() => {
            var scaleBarItems = arcgis_2d.SearchScaleBars("Double Alternating Scale Bar");
        });

        #endregion

        #region How to add a scale bar from a style to a layout
        var arcgis_2dStyle = Project.Current.GetItems<StyleProjectItem>().First(si => si.Name == "ArcGIS 2D");
            QueuedTask.Run(() =>
        {
            //Imperial Double Alternating Scale Bar
            //Metric Double Alternating Scale Bar
            //or just use empty string to list them all...
            var scaleBarItem = arcgis_2d.SearchScaleBars("Double Alternating Scale Bar").FirstOrDefault();
            Coordinate2D coord2D = new Coordinate2D(10.0, 7.0);
            MapFrame myMapFrame = layout.FindElement("Map Frame") as MapFrame;
            LayoutElementFactory.Instance.CreateScaleBar(layout, coord2D, myMapFrame, scaleBarItem);
        });
        #endregion

        #region Create NorthArrow
        Coordinate2D llNorthArrow = new Coordinate2D(6, 2.5);
        MapFrame mf = layout.FindElement("New Map Frame") as MapFrame;
        //Note: call within QueuedTask.Run()
        var northArrow = LayoutElementFactory.Instance.CreateNorthArrow(layout, llNorthArrow, mf);
        #endregion
     
        #region How to search for North Arrows in a style
        var arcgis_2dStyles = Project.Current.GetItems<StyleProjectItem>().First(si => si.Name == "ArcGIS 2D");
        QueuedTask.Run(() => {
            var scaleBarItems = arcgis_2dStyles.SearchNorthArrows("ArcGIS North 13");
        });
            #endregion

            #region How to add a North Arrow from a style to a layout
            var arcgis2dStyles = Project.Current.GetItems<StyleProjectItem>().First(si => si.Name == "ArcGIS 2D");
            QueuedTask.Run(() => {
                var northArrowStyleItem = arcgis2dStyles.SearchNorthArrows("ArcGIS North 13").FirstOrDefault();
                Coordinate2D nArrow = new Coordinate2D(6, 2.5);
                MapFrame newFrame = layout.FindElement("New Map Frame") as MapFrame;
                //Note: call within QueuedTask.Run()
                var newNorthArrow = LayoutElementFactory.Instance.CreateNorthArrow(layout, nArrow, newFrame, northArrowStyleItem);
            });

            #endregion

            #region Create dynamic text
            var title = @"<dyn type = ""page"" property = ""name"" />";
        Coordinate2D llTitle = new Coordinate2D(6, 2.5);
        //Note: call within QueuedTask.Run()
        var titleGraphics = LayoutElementFactory.Instance.CreatePointTextGraphicElement(layout, llTitle, null) as TextElement;
        titleGraphics.SetTextProperties(new TextProperties(title, "Arial", 24, "Bold"));
        #endregion
 

        #region Create dynamic table

        QueuedTask.Run(() =>
        {
          //Build 2D envelope geometry
          Coordinate2D tab_ll = new Coordinate2D(6, 2.5);
          Coordinate2D tab_ur = new Coordinate2D(12, 6.5);
          Envelope tab_env = EnvelopeBuilder.CreateEnvelope(tab_ll, tab_ur);
          MapFrame mapFrame = layout.FindElement("New Map Frame") as MapFrame;
          // get the layer
          MapProjectItem mapPrjItem = Project.Current.GetItems<MapProjectItem>().FirstOrDefault(item => item.Name.Equals("Map"));
          Map theMap = mapPrjItem?.GetMap();
          var lyrs = theMap?.FindLayers("Inspection Point Layer", true);
          if (lyrs?.Count > 0) {
            Layer lyr = lyrs[0];
            var table1 = LayoutElementFactory.Instance.CreateTableFrame(layout, tab_env, mapFrame, lyr, new string[] { "No", "Type", "Description" });
          }
        });        
        #endregion
    }

    public void snippets_CIMChanges()
    {
      #region Change layout page size

      //Get the project item
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals("MyLayout"));
      if (layoutItem != null)
      {
        QueuedTask.Run(() =>
          {
            //Get the layout
            Layout layout = layoutItem.GetLayout();
            if (layout != null)
            {
              //Change properties
              CIMPage page = layout.GetPage();
              page.Width = 8.5;
              page.Height = 11;
              layout.SetPage(page);
            }
          });
      }
      #endregion
    }

    public void snippets_CIMSpatialMapSeries()
    {
      #region Create a Spatial Map Series for a Layout

      //Get the project item
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals("MyLayout"));
      if (layoutItem != null)
      {
        QueuedTask.Run(() =>
        {
          //Get the layout
          Layout layout = layoutItem.GetLayout();
          if (layout != null)
          {
            // Define CIMSpatialMapSeries in CIMLayout
            CIMLayout layCIM = layout.GetDefinition();

            layCIM.MapSeries = new CIMSpatialMapSeries();
            CIMSpatialMapSeries ms = layCIM.MapSeries as CIMSpatialMapSeries;
            ms.Enabled = true;
            ms.MapFrameName = "Railroad Map Frame";
            ms.StartingPageNumber = 1;
            ms.CurrentPageID = 1;
            ms.IndexLayerURI = "CIMPATH=map/railroadmaps.xml";
            ms.NameField = "ServiceAreaName";
            ms.SortField = "SeqId";
            ms.RotationField = "Angle";
            ms.SortAscending = true;
            ms.ScaleRounding = 1000;
            ms.ExtentOptions = ExtentFitType.BestFit;
            ms.MarginType = ArcGIS.Core.CIM.UnitType.Percent;
            ms.Margin = 2;

            layout.SetDefinition(layCIM);
          }
        });
      }
      #endregion
    }

    public void snippets_elements()
    {
      #region Find an element on a layout
      // Reference a layoutitem in a project by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals("MyLayout"));
      if (layoutItem != null)
      {
        QueuedTask.Run(() =>
        {
          // Reference and load the layout associated with the layout item
          Layout layout = layoutItem.GetLayout();
          if (layout != null)
          {
            //Find a single specific element
            Element rect = layout.FindElement("Rectangle") as Element;

            //Or use the Elements collection
            Element rect2 = layout.Elements.FirstOrDefault(item => item.Name.Equals("Rectangle"));
          }
        });
      }
      #endregion

      Element element = null;
      #region Update element properties
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
        //The the active layout view's selection to include 2 rectangle elements
        LayoutView activeLayoutView = LayoutView.Active;
        if (activeLayoutView != null)
        {
          QueuedTask.Run(() =>
          {
            Layout lyt = activeLayoutView.Layout;

            Element rec = lyt.FindElement("Rectangle");
            Element rec2 = lyt.FindElement("Rectangle 2");

            List<Element> elmList = new List<Element>
            {
              rec,
              rec2
            };

            activeLayoutView.SelectElements(elmList);
          });
        }
        #endregion
      }
      {
        #region Clear the layout selection
        //If the a layout view is active, the clear its selection
        LayoutView activeLayoutView = LayoutView.Active;
        if (activeLayoutView != null)
        {          
          activeLayoutView.ClearElementSelection();
        }
        #endregion
      }
      Layout aLayout = null;
      Element elm = null;
      #region Delete an element or elements on a layout

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
            #region Set Halo property of North Arrow
            //Assuming the selected item is a north arrow
            var northArrow = LayoutView.Active.GetSelectedElements().First();
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
                                                                      //set it back
                northArrow.SetDefinition(cim);
            });
            #endregion

        }

        public void snippets_UpdateElements()
    {
      double x = 0;
      double y = 0;

      #region Update Text Element properties

      // Reference a layoutitem in a project by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals("MyLayout"));
      if (layoutItem != null)
      {
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
      // If locked the element can't be selected in the layout using the graphic 
      // selection tools.

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
            CIMGraphic CIMGraphic = graphicElement.Graphic as CIMGraphic;
            CIMGraphic.Transparency = 50;             // mark it 50% transparent
            graphicElement.SetGraphic(CIMGraphic);
          }
        }
      });
      #endregion

      double xOffset = 0;
      double yOffset = 0;
      #region Clone an element

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

            // clone and set the new x,y
            GraphicElement cloneElement = graphicElement.Clone("Clone");
            cloneElement.SetX(cloneElement.GetX() + xOffset);
            cloneElement.SetY(cloneElement.GetY() + yOffset);
          }
        }
      });
      #endregion
    }

    public void snippets_MapFrame()
    {
      #region Access map frame and map bookmarks from Layout

      // Reference a layoutitem in a project by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals("MyLayout"));
      if (layoutItem != null)
      {
        QueuedTask.Run(() =>
        {
          // Reference and load the layout associated with the layout item
          Layout layout = layoutItem.GetLayout();
          if (layout == null)
            return;

          // Reference a mapframe by name
          MapFrame mf = layout.Elements.FirstOrDefault(item => item.Name.Equals("MapFrame")) as MapFrame;
          if (mf == null)
            return;

          // get the map and the bookmarks
          Bookmark bookmark = mf.Map.GetBookmarks().FirstOrDefault(b => b.Name == "Great Lakes");
          if (bookmark == null)
            return;

          // Set up a PDF format and set its properties
          PDFFormat PDF = new PDFFormat();
          String path = String.Format(@"C:\Temp\{0}.pdf", bookmark.Name);
          PDF.OutputFileName = path;

          // Zoom to the bookmark 
          mf.SetCamera(bookmark);

          // Export to PDF
          if (PDF.ValidateOutputFilePath())
              mf.Export(PDF);
        });
      }
      #endregion

    }

    public void snippets_exportLayout()
    {
      #region Export a layout

      // Reference a layoutitem in a project by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals("MyLayout"));
      if (layoutItem != null)
      {
        QueuedTask.Run(() =>
        {
          Layout layout = layoutItem.GetLayout();
          if (layout == null)
            return;

          // Create BMP format with appropriate settings
          BMPFormat BMP = new BMPFormat()
          {
            Resolution = 300,
            OutputFileName = @"C:\temp\Layout.bmp"
          };
          if (BMP.ValidateOutputFilePath())
          {
            layout.Export(BMP);
          }

          // Create EMF format with appropriate settings
          EMFFormat EMF = new EMFFormat()
          {
            Resolution = 300,
            OutputFileName = @"C:\temp\Layout.emf"
          };
          if (EMF.ValidateOutputFilePath())
          {
            layout.Export(EMF);
          }

          // create eps format with appropriate settings
          EPSFormat EPS = new EPSFormat()
          {
            Resolution = 300,
            OutputFileName = @"C:\temp\Layout.eps"
          };
          if (EPS.ValidateOutputFilePath())
          {
            layout.Export(EPS);
          }

          // Create GIF format with appropriate settings
          GIFFormat GIF = new GIFFormat()
          {
            Resolution = 300,
            OutputFileName = @"C:\temp\Layout.gif"
          };
          if (GIF.ValidateOutputFilePath())
          {
            layout.Export(GIF);
          }

          // Create JPEG format with appropriate settings
          JPEGFormat JPEG = new JPEGFormat()
          {
            Resolution = 300,
            OutputFileName = @"C:\temp\Layout.jpg"
          };
          if (JPEG.ValidateOutputFilePath())
          {
            layout.Export(JPEG);
          }

          // Create PDF format with appropriate settings
          PDFFormat PDF = new PDFFormat()
          {
            Resolution = 300,
            OutputFileName = @"C:\temp\Layout.pdf"
          };
          if (PDF.ValidateOutputFilePath())
          {
            layout.Export(PDF);
          }

          // Create PNG format with appropriate settings
          PNGFormat PNG = new PNGFormat()
          {
            Resolution = 300,
            OutputFileName = @"C:\temp\Layout.png"
          };
          if (PNG.ValidateOutputFilePath())
          {
            layout.Export(PNG);
          }

          // Create SVG format with appropriate settings
          SVGFormat SVG = new SVGFormat()
          {
            Resolution = 300,
            OutputFileName = @"C:\temp\Layout.svg"
          };
          if (SVG.ValidateOutputFilePath())
          {
            layout.Export(SVG);
          }

          // Create TGA format with appropriate settings
          TGAFormat TGA = new TGAFormat()
          {
            Resolution = 300,
            OutputFileName = @"C:\temp\Layout.tga"
          };
          if (TGA.ValidateOutputFilePath())
          {
            layout.Export(TGA);
          }

          // Create TIFF format with appropriate settings
          TIFFFormat TIFF = new TIFFFormat()
          {
            Resolution = 300,
            OutputFileName = @"C:\temp\Layout.tif"
          };
          if (TIFF.ValidateOutputFilePath())
          {
            layout.Export(TIFF);
          }
        });
      }
      #endregion
    }

    public void snippets_Export()
    {
      #region Export a map frame 

      // Reference a layoutitem in a project by name
      LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals("MyLayout"));
      if (layoutItem != null)
      {
        QueuedTask.Run(() =>
        {
          // get the layout 
          Layout layout = layoutItem.GetLayout();
          if (layout == null)
            return;

          // get the map frame
          MapFrame mf = layout.FindElement("MyMapFrame") as MapFrame;

          if (mf != null)
          {
            // Create BMP format with appropriate settings
            BMPFormat BMP = new BMPFormat()
            {
              HasWorldFile = true,
              Resolution = 300,
              OutputFileName = @"C:\temp\MapFrame.bmp"
            };
            if (BMP.ValidateOutputFilePath())
            {
              mf.Export(BMP);
            }

            // emf, eps, gif, jpeg, pdf, png, svg, tga, tiff formats are also available for export
          }
        });
      }

      #endregion

      #region Export the active Mapview
      QueuedTask.Run(() =>
      {
        MapView activeMapView = MapView.Active;
        if (activeMapView != null)
        {
          //Create BMP format with appropriate settings
          BMPFormat bmp = new BMPFormat()
          {
            Resolution = 300,
            Height = 500,
            Width = 800,
            HasWorldFile = true,
            OutputFileName = @"C:\temp\MapView.bmp"
          };

          //Export active map view
          if (bmp.ValidateOutputFilePath())
          {
            activeMapView.Export(bmp);
          }

          // emf, eps, gif, jpeg, pdf, png, svg, tga, tiff formats also available for export
        }
      });
      #endregion
    }
  }
}
