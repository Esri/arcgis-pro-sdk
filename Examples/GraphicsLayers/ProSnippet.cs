using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Layouts;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsLayer.GraphicsLayer
{
  class ProSnippet : MapTool
  {
    public void CreateGraphicsLayer()
    {
      #region Create GraphicsLayer
      var map = MapView.Active.Map;
      if (map.MapType != MapType.Map)
        return;// not 2D

      var gl_param = new GraphicsLayerCreationParams { Name = "Graphics Layer" };
      QueuedTask.Run(() =>
      {
        //By default will be added to the top of the TOC
        var graphicsLayer = LayerFactory.Instance.CreateLayer<ArcGIS.Desktop.Mapping.GraphicsLayer>(gl_param, map);

        //Add to the bottom of the TOC
        LayerFactory.Instance.CreateLayer<ArcGIS.Desktop.Mapping.GraphicsLayer>(gl_param, map,
                                                           LayerPosition.AddToBottom);

        //Add a graphics layer to a group layer...
        var group_layer = map.GetLayersAsFlattenedList().OfType<GroupLayer>().First();
        LayerFactory.Instance.CreateLayer<ArcGIS.Desktop.Mapping.GraphicsLayer>(gl_param, group_layer);

        //TODO...use the graphics layer        
      });
      #endregion
    }

    public void AccessingGraphicLayer()
    {
      var map = MapView.Active.Map;
      if (map.MapType != MapType.Map)
        return;// not 2D
      #region Accessing GraphicsLayer
      //get the first graphics layer in the map's collection of graphics layers
      var graphicsLayer = map.GetLayersAsFlattenedList().OfType<ArcGIS.Desktop.Mapping.GraphicsLayer>().FirstOrDefault();
      if (graphicsLayer != null)
      {
        //TODO...use the graphics layer      
      }
      #endregion
    }
    public void CopyGraphicsLayer()
    {
      var sourceGraphicsLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<ArcGIS.Desktop.Mapping.GraphicsLayer>().Where(gl => gl.Name == "SourceLayer").FirstOrDefault();
      var targetGraphicsLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<ArcGIS.Desktop.Mapping.GraphicsLayer>().Where(gl => gl.Name == "TargetLayer").FirstOrDefault();
      #region Copy Graphic elements
      //on the QueuedTask
      var elems = sourceGraphicsLayer.FindElements(new List<string>() { "Point 1", "Line 3", "Text 1" });
      var copiedElements = targetGraphicsLayer.CopyElements(elems);
      #endregion
    }
    public void RemoveGraphicsLayer()
    {
      var graphicsLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<ArcGIS.Desktop.Mapping.GraphicsLayer>().FirstOrDefault();
      #region Remove Graphic elements
      //on the QueuedTask      
      graphicsLayer.RemoveElements(graphicsLayer.GetSelectedElements());
      #endregion
    }
    #region ProSnippet Group: Create Graphic Elements
    #endregion

    public void CreateGraphicElementUsingCIMGraphic()
    {
      #region Point Graphic Element using CIMGraphic
      var graphicsLayer = MapView.Active.Map.GetLayersAsFlattenedList()
  .OfType<ArcGIS.Desktop.Mapping.GraphicsLayer>().FirstOrDefault();
      if (graphicsLayer == null)
        return;
      QueuedTask.Run(() =>
      {
        //Place symbol in the center of the map
        var extent = MapView.Active.Extent;
        var location = extent.Center;

        //specify a symbol
        var pt_symbol = SymbolFactory.Instance.ConstructPointSymbol(
                              ColorFactory.Instance.GreenRGB);

        //create a CIMGraphic 
        var graphic = new CIMPointGraphic()
        {
          Symbol = pt_symbol.MakeSymbolReference(),
          Location = location //center of map
        };
        graphicsLayer.AddElement(graphic);
      });
      #endregion
    }
    public void CreateLineGraphic()
    {
      
      var graphicsLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<ArcGIS.Desktop.Mapping.GraphicsLayer>().FirstOrDefault();
      if (graphicsLayer == null)
        return;
      QueuedTask.Run(() =>
      {
        #region Line Graphic Element using CIMGraphic
        //On the QueuedTask
        //Place a line symbol using the extent's lower left and upper right corner.
        var extent = MapView.Active.Extent;
        //get the lower left corner of the extent
        var pointFromCoordinates = new Coordinate2D(extent.XMin, extent.YMin);
        //get the upper right corner of the extent
        var pointToCoordinates = new Coordinate2D(extent.XMax, extent.YMax);
        List<Coordinate2D> points = new List<Coordinate2D> { pointFromCoordinates, pointToCoordinates };
        //create the polyline
        var lineSegment = PolylineBuilder.CreatePolyline(points);

        //specify a symbol
        var line_symbol = SymbolFactory.Instance.ConstructLineSymbol(
                              ColorFactory.Instance.GreenRGB);

        //create a CIMGraphic 
        var graphic = new CIMLineGraphic()
        {
          Symbol = line_symbol.MakeSymbolReference(),
          Line = lineSegment,
        };
        graphicsLayer.AddElement(graphic);
        #endregion
      });
    }
    public void CreatePolyGraphic()
    {
      var graphicsLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<ArcGIS.Desktop.Mapping.GraphicsLayer>().FirstOrDefault();
      if (graphicsLayer == null)
        return;
      QueuedTask.Run(() =>
      {
        #region Polygon Graphic Element using CIMGraphic
        //On the QueuedTask
        //Place a polygon symbol using the mapview extent geometry
        var extent = MapView.Active.Extent;
        //Contract the extent
        var polygonEnv = extent.Expand(-100000, -90000, false);
        //create a polygon using the envelope
        var polygon = PolygonBuilder.CreatePolygon(polygonEnv);

        //specify a symbol
        var poly_symbol = SymbolFactory.Instance.ConstructPolygonSymbol(
                              ColorFactory.Instance.GreenRGB);

        //create a CIMGraphic 
        var graphic = new CIMPolygonGraphic()
        {
          Symbol = poly_symbol.MakeSymbolReference(),
          Polygon = polygon,
        };
        graphicsLayer.AddElement(graphic);
        #endregion
      });
    }
    public void CreateMultiPointGraphics()
    {
      var graphicsLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<ArcGIS.Desktop.Mapping.GraphicsLayer>().FirstOrDefault();
      if (graphicsLayer == null)
        return;
      QueuedTask.Run(() =>
      {
        #region Multi-point Graphic Element using CIMGraphic
        //On the QueuedTask
        //Place a multipoint graphic using the mapview extent geometry
        var extent = MapView.Active.Extent;
        //Contract the extent
        var polygonEnv = extent.Expand(-100000, -90000, false);
        //create a polygon using the envelope
        var polygon = PolygonBuilder.CreatePolygon(polygonEnv);
        //Create MultipPoints from the polygon
        var multiPoints = MultipointBuilder.CreateMultipoint(polygon);
        //specify a symbol
        var point_symbol = SymbolFactory.Instance.ConstructPointSymbol(
                              ColorFactory.Instance.GreenRGB);

        //create a CIMGraphic 
        var graphic = new CIMMultipointGraphic
        {
          Symbol = point_symbol.MakeSymbolReference(),
          Multipoint = multiPoints
        };
        graphicsLayer.AddElement(graphic);
        #endregion
      });
    }
    public void CreateGraphicElementUsingCIMSymbol()
    {
      #region Graphic Element using CIMSymbol
      var graphicsLayer = MapView.Active.Map.GetLayersAsFlattenedList()
  .OfType<ArcGIS.Desktop.Mapping.GraphicsLayer>().FirstOrDefault();
      if (graphicsLayer == null)
        return;
      QueuedTask.Run(() =>
      {
        //Place symbol in the center of the map
        var extent = MapView.Active.Extent;
        var location = extent.Center;

        //specify a symbol
        var pt_symbol = SymbolFactory.Instance.ConstructPointSymbol(
                              ColorFactory.Instance.GreenRGB);        
        graphicsLayer.AddElement(location, pt_symbol);
      });
      #endregion
    }

    public void CreateTextGraphicElement()
    {
      #region Text Graphic Element
      var graphicsLayer = MapView.Active.Map.GetLayersAsFlattenedList()
                          .OfType<ArcGIS.Desktop.Mapping.GraphicsLayer>().FirstOrDefault();
      if (graphicsLayer == null)
        return;
      QueuedTask.Run(() =>
      {
        //Place symbol in the center of the map
        var extent = MapView.Active.Extent;
        var location = extent.Center;

        //specify a text symbol
        var text_symbol = SymbolFactory.Instance.ConstructTextSymbol
        (ColorFactory.Instance.BlackRGB, 8.5, "Corbel", "Regular");

        graphicsLayer.AddElement(location, "Text Example", text_symbol);
      });
      #endregion
    }
    private void BulkGraphicsCreation()
    {
      #region Bulk Graphics creation
      //Point Feature layer to convert into graphics
      var lyr = MapView.Active?.Map?.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();
      //Graphics layer to store the graphics to
      var gl = MapView.Active.Map.GetLayersAsFlattenedList().OfType<ArcGIS.Desktop.Mapping.GraphicsLayer>().FirstOrDefault();
      if (lyr == null) return;
      QueuedTask.Run(() =>
      {
        //Point symbol for graphics
        var pointSymbol = SymbolFactory.Instance.ConstructPointSymbol(CIMColor.CreateRGBColor(100, 255, 40), 10, SimpleMarkerStyle.Circle);
        //Collection to hold the point graphics
        var listGraphicElements = new List<CIMGraphic>();
        //Iterate through each point feature in the feature layer
        using (RowCursor rows = lyr.Search()) //execute
        {
          int i = 0;
          while (rows.MoveNext())
          {
            using (var feature = rows.Current as Feature)
            {
              //Create a point graphic for the feature
              var crimePt = feature.GetShape() as MapPoint;
              if (crimePt != null)
              {
                var cimGraphicElement = new CIMPointGraphic
                {
                  Location = crimePt, //MapPoint
                  Symbol = pointSymbol.MakeSymbolReference()
                };
                //Add the point feature to the collection
                listGraphicElements.Add(cimGraphicElement);
                i++;
              }
            }
          }
        }
        //Magic happens...Add all the features to the Graphics layer 
        gl.AddElements(listGraphicElements);
      });
      #endregion
    }

    #region ProSnippet Group: Graphic Elements Selection
    #endregion
    public void ProgrammaticSelection()
    {
      #region Select Graphic Elements
      var graphicsLayer = MapView.Active.Map.GetLayersAsFlattenedList()
                          .OfType<ArcGIS.Desktop.Mapping.GraphicsLayer>().FirstOrDefault();
      if (graphicsLayer == null)
        return;
      var elements = graphicsLayer.GetElementsAsFlattenedList()
                          .Where(e => e.Name.StartsWith("Text"));
      QueuedTask.Run(() =>
      {
        graphicsLayer.SelectElements(elements);
        //or select one element
        graphicsLayer.SelectElement(elements.FirstOrDefault());
      });
      #endregion
    }
    public void FindGraphicElement()
    {
      var graphicsLayer = MapView.Active?.Map.GetLayersAsFlattenedList().OfType<ArcGIS.Desktop.Mapping.GraphicsLayer>().FirstOrDefault();
      QueuedTask.Run( () => {
        #region Find Graphic elements
        //on the QueuedTask
        //Find elements by name
        var elems = graphicsLayer.FindElements(new List<string>() { "Point 1", "Line 3", "Text 1" });
        //Find elements by type
        //Find all point graphics in the Graphics Layer
        var pointGraphics = graphicsLayer.GetElementsAsFlattenedList().Where(elem => elem.GetGraphic() is CIMPointGraphic);
        //Find all line graphics in the Graphics Layer
        var lineGraphics = graphicsLayer.GetElementsAsFlattenedList().Where(elem => elem.GetGraphic() is CIMLineGraphic);
        //Find all polygon graphics in the Graphics Layer
        var polygonGraphics = graphicsLayer.GetElementsAsFlattenedList().Where(elem => elem.GetGraphic() is CIMPolygonGraphic);
        //Find all text graphics in the Graphics Layer
        var textGraphics = graphicsLayer.GetElementsAsFlattenedList().Where(elem => elem.GetGraphic() is CIMTextGraphic);
        //Find all picture graphics in the Graphics Layer
        var pictureGraphic = graphicsLayer.GetElementsAsFlattenedList().Where(elem => elem.GetGraphic() is CIMPictureGraphic);
        #endregion
      });
    }
    #region Spatial selection of elements in all Graphics Layers
    //Map Tool is used to perform Spatial selection.
    //Graphic selection uses the selection geometry 
    //to intersect the geometries of those elements (graphic or group) 
    //that will be selected and then highlights them. 
    protected override async Task<bool> OnSketchCompleteAsync(Geometry geometry)
    {
      var selPoly = geometry as Polygon;
      return await QueuedTask.Run(() =>
      {     
        //note: the selected elements may belong to more than one layer...
        var elems = MapView.Active.SelectElements(selPoly, SelectionCombinationMethod.New);
        return true;
      });
    }
    #endregion
    
    public void SpatialSelection()
    {
      var graphicsLayer = MapView.Active?.Map.GetLayersAsFlattenedList().OfType<ArcGIS.Desktop.Mapping.GraphicsLayer>().FirstOrDefault();
      QueuedTask.Run(() => {
        #region Spatial selection of elements in one graphics layer
        //on the QueuedTask
        //Create an extent to use for the spatial selection
        var extent = MapView.Active.Extent;
        var selectionExtent = extent.Expand(0.5, 0.5, true);
        //Select elements in specified graphics layer using the selection extent.
        var selectedElements = MapView.Active.SelectElements(graphicsLayer, selectionExtent, SelectionCombinationMethod.Add);
        #endregion
      });
    }

    public void SelectTextGraphicElements()
    {
      #region Select Text Graphic Elements
      var graphicsLayer = MapView.Active.Map.GetLayersAsFlattenedList()
                          .OfType<ArcGIS.Desktop.Mapping.GraphicsLayer>().FirstOrDefault();
      if (graphicsLayer == null)
        return;
      var all_text = graphicsLayer.GetElementsAsFlattenedList()
                    .Where(e => e.GetGraphic() is CIMTextGraphic);
      #endregion
    }

    public void UnSelectGraphicElements()
    {
      #region Un-Select Graphic elements
      var graphicsLayer = MapView.Active.Map.GetLayersAsFlattenedList()
                          .OfType<ArcGIS.Desktop.Mapping.GraphicsLayer>().FirstOrDefault();
      if (graphicsLayer == null)
        return;
      //unselect the first element in the currently selected elements
      var elem = graphicsLayer.GetSelectedElements().FirstOrDefault();
      QueuedTask.Run( () => {
        if (elem != null)
          //Unselect one element
          graphicsLayer.UnSelectElement(elem);

        //unselect all elements
        graphicsLayer.UnSelectElements();
        //equivalent to
        graphicsLayer.ClearSelection();
      });
      #endregion
    }
    #region ProSnippet Group: Graphic Element Events
    #endregion
    public void SubscribeElementSelectionChanged()
    {
      #region Subscribe to ElementSelectionChangedEvent
      ArcGIS.Desktop.Mapping.Events.ElementSelectionChangedEvent.Subscribe((args) => {
        //check if the container is a graphics layer - could be a Layout (or even map view)
        if (args.ElementContainer is ArcGIS.Desktop.Mapping.GraphicsLayer graphicsLayer)
        {
          //get the total selection count for the container
          var count = args.SelectedElementCount;
          //Check count - could have been an unselect or clearselect
          if (count > 0)
          {
            //this is a selection or add to selection
            var elems = graphicsLayer.GetSelectedElements();
            //TODO process the selection...
          }
          else
          {
            //This is an unselect or clear selection
            //TODO process the unselect or clear select
          }
        }
      });
      #endregion
    }
    #region ProSnippet Group: Grouping and Ordering Graphic Elements
    #endregion
    public void GroupUnGroupElements()
    {
      QueuedTask.Run( () => {
        #region Group Graphic Elements
        var graphicsLayer = MapView.Active.Map.GetLayersAsFlattenedList()
                            .OfType<ArcGIS.Desktop.Mapping.GraphicsLayer>().FirstOrDefault();
        if (graphicsLayer == null)
          return;

        var elemsToGroup = graphicsLayer.GetSelectedElements();
        //Note: run within the QueuedTask
        //group  elements
        var groupElement = graphicsLayer.GroupElements(elemsToGroup);
        #endregion

        #region Un-Group Graphic Elements
        var selectedElements = graphicsLayer.GetSelectedElements().ToList(); ;
        if (selectedElements?.Any() == false)//must be at least 1.
          return;
        var elementsToUnGroup = new List<GroupElement>();
        //All selected elements should be grouped.
        if (selectedElements.Count() == selectedElements.OfType<GroupElement>().Count())
        {
          //Convert to a GroupElement list.
          elementsToUnGroup = selectedElements.ConvertAll(x => (GroupElement)x);
        }
        if (elementsToUnGroup.Count() == 0)
          return;
        //UnGroup
        graphicsLayer.UnGroupElements(elementsToUnGroup); 
        #endregion

        #region Parent of GroupElement
        //check the parent
        var parent = groupElement.Elements.First().GetParent();//will be the group element
        //top-most parent
        var top_most = groupElement.Elements.First().GetParent(true);//will be the GraphicsLayer
        #endregion

        #region Ordering: Send backward and Bring forward
        //On the QueuedTask
        //get the current selection set
        var sel_elems = graphicsLayer.GetSelectedElements();
        //can they be brought forward? This will also check that all elements have the same parent
        if (graphicsLayer.CanBringForward(sel_elems))
        {
          //bring forward
          graphicsLayer.BringForward(sel_elems);
          //bring to front (of parent)
          //graphicsLayer.BringToFront(sel_elems);
        }
        else if (graphicsLayer.CanSendBackward(sel_elems))
        {
          //send back
          graphicsLayer.SendBackward(sel_elems);
          //send to the back (of parent)
          //graphicsLayer.SendToBack(sel_elems);
        }
        #endregion

        #region Get Z-Order
        var selElementsZOrder = graphicsLayer.GetSelectedElements();
        //list out the z order
        foreach (var elem in selElementsZOrder)
          System.Diagnostics.Debug.WriteLine($"{elem.Name}: z-order {elem.GetZOrder()}");
        #endregion
      });
    }
    #region ProSnippet Group: Modifying Graphic Elements
    #endregion
    public void MoveGraphicElements()
    {
      var graphicsLayer = MapView.Active.Map.GetLayersAsFlattenedList()
                    .OfType<ArcGIS.Desktop.Mapping.GraphicsLayer>().FirstOrDefault();
      QueuedTask.Run( () => {
        #region Move Graphic Elements
        //Each selected element will move to a set distance to the upper right.
        var selElements = graphicsLayer.GetSelectedElements();
        if (selElements.Count == 0) return;
        //Move the element up
        foreach (var selElement in selElements)
        {
          //Get the element's bounds
          var elementPoly = PolygonBuilder.CreatePolygon(selElement.GetBounds());
          //get the coordinates of the element bounding envelope.
          var pointsList = elementPoly.Copy2DCoordinatesToList();
          //Move the element's Anchor point to the upper right.
          selElement.SetAnchorPoint(pointsList[1]);
        }
        #endregion
      });
      
    }

    public void ChangeGraphicSymbology()
    {
      var graphicsLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<ArcGIS.Desktop.Mapping.GraphicsLayer>().FirstOrDefault();
      QueuedTask.Run( () => {
        #region Modify symbology of a Graphic Element
        //within a queued Task
        //get the first line element in the layer
        var ge = graphicsLayer.FindElement("Line 10") as GraphicElement;
        var graphic = ge.GetGraphic();
        if (graphic is CIMLineGraphic lineGraphic)
        {
          //change its symbol
          lineGraphic.Symbol =
           SymbolFactory.Instance.ConstructLineSymbol(
             SymbolFactory.Instance.ConstructStroke(
         ColorFactory.Instance.BlueRGB, 2, SimpleLineStyle.DashDot)).MakeSymbolReference();
          //apply the change
          ge.SetGraphic(lineGraphic);
        }
        #endregion
      });
      
    }
  }
}
