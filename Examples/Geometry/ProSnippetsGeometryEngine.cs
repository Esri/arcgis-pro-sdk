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
using System.IO;
using System.Linq;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;


namespace ProSnippetsGeometry
{
  class ProSnippetsGeometryEngine
  {
    #region ProSnippet Group: GeometryEngine functions
    #endregion

    public void AccelerateGeomtries()
    {
      Polygon polygon = null;
      IEnumerable<Polygon> testPolygons = null;

      // cref: ArcGIS.Core.Geometry.GeometryEngine.AccelerateForRelationalOperations(ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.AccelerateForRelationalOperations(ArcGIS.Core.Geometry.Geometry)
      #region Accelerate Geometries

      // Use acceleration to speed up relational operations.  Accelerate your source geometry only if you are going to test many other geometries against it. 
      // Acceleration is applicable for polylines and polygons only. Note that accelerated geometries take more memory so if you aren't going to get any
      // benefit from accelerating it, don't do it. 

      // The performance of the following GeometryEngine functions are the only ones which can be improved with an accelerated geometry.
      //    GeometryEngine.Instance.Contains
      //    GeometryEngine.Instance.Crosses
      //    GeometryEngine.Instance.Disjoint
      //    GeometryEngine.Instance.Disjoint3D
      //    GeometryEngine.Instance.Equals
      //    GeometryEngine.Instance.Intersects
      //    GeometryEngine.Instance.Relate
      //    GeometryEngine.Instance.Touches
      //    GeometryEngine.Instance.Within


      // methods need to run on the MCT
      ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
      {
        // accelerate the geometry to test
        var acceleratedPoly = GeometryEngine.Instance.AccelerateForRelationalOperations(polygon);

        // loop through all the geometries to test against
        foreach (var testPolygon in testPolygons)
        {
          bool contains = GeometryEngine.Instance.Contains(acceleratedPoly, testPolygon);
          bool within = GeometryEngine.Instance.Within(acceleratedPoly, testPolygon);
          bool crosses = GeometryEngine.Instance.Crosses(acceleratedPoly, testPolygon);
        }
      });

      #endregion
    }

    public void Area()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Area(ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Area(ArcGIS.Core.Geometry.Geometry)
      #region Determine area of a polygon

      var g1 = PolygonBuilderEx.FromJson("{\"rings\": [ [ [0, 0], [10, 0], [10, 10], [0, 10] ] ] }");
      double d = GeometryEngine.Instance.Area(g1);
      // d = -100.0         //negative due to wrong ring orientation
      d = GeometryEngine.Instance.Area(GeometryEngine.Instance.SimplifyAsFeature(g1));
      // d = 100.0        // feature has been simplifed; ring orientation is correct

      #endregion
    }

    public void Boundary()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Boundary(ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Boundary(ArcGIS.Core.Geometry.Geometry)
      #region Determine the boundary of a multi-part Polygon

      // create a donut polygon.  Must use the PolygonBuilderEx object

      List<Coordinate2D> outerPts = new List<Coordinate2D>();
      outerPts.Add(new Coordinate2D(10.0, 10.0));
      outerPts.Add(new Coordinate2D(10.0, 20.0));
      outerPts.Add(new Coordinate2D(20.0, 20.0));
      outerPts.Add(new Coordinate2D(20.0, 10.0));

      List<Coordinate2D> innerPts = new List<Coordinate2D>();
      innerPts.Add(new Coordinate2D(13.0, 13.0));
      innerPts.Add(new Coordinate2D(17.0, 13.0));
      innerPts.Add(new Coordinate2D(17.0, 17.0));
      innerPts.Add(new Coordinate2D(13.0, 17.0));

      Polygon donut = null;

      // add the outer points
      PolygonBuilderEx pb = new PolygonBuilderEx(outerPts);
      // add the inner points (note they are defined anticlockwise)
      pb.AddPart(innerPts);
      // get the polygon
      donut = pb.ToGeometry();

      // get the boundary 
      Geometry g = GeometryEngine.Instance.Boundary(donut);
      Polyline boundary = g as Polyline;

      #endregion
    }

    public void Buffer()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Buffer(ArcGIS.Core.Geometry.Geometry,System.Double)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Buffer(ArcGIS.Core.Geometry.Geometry,System.Double)
      #region Buffer a MapPoint

      // buffer a point
      MapPoint pt = MapPointBuilderEx.CreateMapPoint(1.0, 1.0, SpatialReferences.WGS84);
      Geometry ptBuffer = GeometryEngine.Instance.Buffer(pt, 5.0);
      Polygon buffer = ptBuffer as Polygon;
      #endregion

      // cref: ArcGIS.Core.Geometry.GeometryEngine.Buffer(ArcGIS.Core.Geometry.Geometry,System.Double)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Buffer(ArcGIS.Core.Geometry.Geometry,System.Double)
      // cref: ArcGIS.Core.Geometry.PolylineBuilderEx.CreatePolyline(ArcGIS.Core.Geometry.Segment,ArcGIS.Core.Geometry.SpatialReference)
      #region Buffer a Circular Arc

      // create the circular arc
      MapPoint fromPt = MapPointBuilderEx.CreateMapPoint(2, 1);
      MapPoint toPt = MapPointBuilderEx.CreateMapPoint(1, 2);
      Coordinate2D interiorPt = new Coordinate2D(1 + Math.Sqrt(2) / 2, 1 + Math.Sqrt(2) / 2);

      EllipticArcSegment circularArc = EllipticArcBuilderEx.CreateCircularArc(fromPt, toPt, interiorPt);

      // buffer the arc
      Polyline polyline = PolylineBuilderEx.CreatePolyline(circularArc);
      Geometry lineBuffer = GeometryEngine.Instance.Buffer(polyline, 10);

      #endregion

      // cref: Buffer multiple MapPoints;ArcGIS.Core.Geometry.GeometryEngine.Buffer(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Geometry},System.Double)
      // cref: Buffer multiple MapPoints;ArcGIS.Core.Geometry.IGeometryEngine.Buffer(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Geometry},System.Double)
      #region Buffer multiple MapPoints

      // creates a buffer around each MapPoint

      List<MapPoint> pts = new List<MapPoint>();
      pts.Add(MapPointBuilderEx.CreateMapPoint(1.0, 1.0));
      pts.Add(MapPointBuilderEx.CreateMapPoint(1.0, 2.0));
      pts.Add(MapPointBuilderEx.CreateMapPoint(2.0, 2.0));
      pts.Add(MapPointBuilderEx.CreateMapPoint(2.0, 1.0));

      Geometry ptsBuffer = GeometryEngine.Instance.Buffer(pts, 0.25);
      Polygon bufferResult = ptsBuffer as Polygon;      // bufferResult will have 4 parts

      #endregion

      // cref: ArcGIS.Core.Geometry.GeometryEngine.Buffer(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Geometry},System.Double)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Buffer(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Geometry},System.Double)
      #region Buffer many different Geometry Types

      List<Coordinate2D> coords = new List<Coordinate2D>()
      {
          new Coordinate2D(1, 2), new Coordinate2D(3, 4), new Coordinate2D(4, 2),
          new Coordinate2D(5, 6), new Coordinate2D(7, 8), new Coordinate2D(8, 4),
          new Coordinate2D(9, 10), new Coordinate2D(11, 12), new Coordinate2D(12, 8),
          new Coordinate2D(10, 8), new Coordinate2D(12, 12), new Coordinate2D(14, 10)
      };

      List<Geometry> manyGeometries = new List<Geometry>
      {
          MapPointBuilderEx.CreateMapPoint(coords[9]),
          PolylineBuilderEx.CreatePolyline(new List<Coordinate2D>(){coords[0], coords[1], coords[2]}, SpatialReferences.WGS84),
          PolylineBuilderEx.CreatePolyline(new List<Coordinate2D>(){coords[3], coords[4], coords[5]}),
          PolygonBuilderEx.CreatePolygon(new List<Coordinate2D>(){coords[6], coords[7], coords[8]})
      };

      Geometry manyGeomBuffer = GeometryEngine.Instance.Buffer(manyGeometries, 0.25);

      #endregion
    }

    public void CalculateNonSimpleZs_Ms()
    {
      {
        // cref: ArcGIS.Core.Geometry.GeometryEngine.CalculateNonSimpleZs(ArcGIS.Core.Geometry.Multipart,System.Double)
        // cref: ArcGIS.Core.Geometry.IGeometryEngine.CalculateNonSimpleZs(ArcGIS.Core.Geometry.Multipart,System.Double)
        #region Interpolate Z values on a polyline

        List<Coordinate3D> coords2 = new List<Coordinate3D>()
        {
          new Coordinate3D(0, 0, 0),
          new Coordinate3D(0, 1000, double.NaN),
          new Coordinate3D(1000, 1000, 50),
          new Coordinate3D(1000, 1000, 76),
          new Coordinate3D(0, 1000, double.NaN),
          new Coordinate3D(0, 0, 0)
        };

        SpatialReference sr = SpatialReferences.WebMercator;

        Polyline polyline = PolylineBuilderEx.CreatePolyline(coords2, sr);

        // polyline.HasZ = true
        // polyline.Points[1].HasZ = true
        // polyline.Points[1].Z  = NaN   
        // polyline.Points[4].HasZ = true
        // polyline.Points[4].Z  = NaN   

        Polyline polylineNoNaNZs = GeometryEngine.Instance.CalculateNonSimpleZs(polyline, 0) as Polyline;

        // polylineNoNaNZs.Points[1].HasZ = true
        // polylineNoNaNZs.Points[1].Z = 25  (halfway between 0 and 50)
        // polylineNoNaNZs.Points[4].HasZ = true
        // polylineNoNaNZs.Points[4].Z = 38  (halfway between 76 and 0)
        #endregion
      }

      {
        // cref: ArcGIS.Core.Geometry.GeometryEngine.CalculateNonSimpleMs(ArcGIS.Core.Geometry.Multipart,System.Double)
        // cref: ArcGIS.Core.Geometry.IGeometryEngine.CalculateNonSimpleMs(ArcGIS.Core.Geometry.Multipart,System.Double)
        #region Interpolate M values on a polygon

        List<MapPoint> coords = new List<MapPoint>()
        {
          MapPointBuilderEx.CreateMapPoint(0, 0, 0, 0),
          MapPointBuilderEx.CreateMapPoint(0, 1000),
          MapPointBuilderEx.CreateMapPoint(1000, 1000, 10, 50)
        };

        SpatialReference sr = SpatialReferences.WebMercator;

        Polygon polygon = PolygonBuilderEx.CreatePolygon(coords, sr);

        // polygon.HasM = true
        // polygon.Points[1].HasM = true
        // polygon.Points[1].M = NaN

        Polygon polygonNoNaNMs = GeometryEngine.Instance.CalculateNonSimpleMs(polygon, 0) as Polygon;

        // polygonNoNaNMs.Points[1].HasM = true
        // polygonNoNaNMs.Points[1].M = 25  (halfway between 0 and 50)

        #endregion
      }
    }

    public void CenterAt()
    {
      // cref: ArcGIS.Core.Geometry.Envelope.CenterAt(System.Double,System.Double)
      // cref: ArcGIS.Core.Geometry.GeometryEngine.CenterAt(ArcGIS.Core.Geometry.Envelope,System.Double,System.Double)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.CenterAt(ArcGIS.Core.Geometry.Envelope,System.Double,System.Double)
      #region Center an envelope around X,Y

      Envelope env = EnvelopeBuilderEx.CreateEnvelope(1.0, 1.0, 5.0, 5.0);
      Envelope centered = GeometryEngine.Instance.CenterAt(env, 2.0, 2.0);

      // centered.Center.X = 2.0
      // centered.Center.Y = 2.0
      // centered.XMin = 0
      // centered.YMin = 0
      // centered.XMax = 4
      // centered.YMax = 4

      centered = env.CenterAt(4.0, 3.0);
      // centered.Center.X == 4.0
      // centered.Center.Y == 3.0
      // centered.XMin == 2.0
      // centered.YMin == 1.0
      // centered.XMax == 6.0
      // centered.YMax == 5.0
      #endregion
    }

    public void Centroid()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Centroid(ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Centroid(ArcGIS.Core.Geometry.Geometry)
      #region Find the centroid of geometries

      // simple polygon
      List<Coordinate2D> list2D = new List<Coordinate2D>();
      list2D.Add(new Coordinate2D(0, 0));
      list2D.Add(new Coordinate2D(0, 2));
      list2D.Add(new Coordinate2D(2, 2));
      list2D.Add(new Coordinate2D(2, 0));

      Polygon polygon = PolygonBuilderEx.CreatePolygon(list2D, SpatialReferences.WGS84);

      // verify it is simple
      bool isSimple = GeometryEngine.Instance.IsSimpleAsFeature(polygon);
      // find the centroid
      MapPoint centroid = GeometryEngine.Instance.Centroid(polygon);
      // centroid.X = 1
      // centroid.Y = 1

      // map Point
      MapPoint pt1 = MapPointBuilderEx.CreateMapPoint(1, 2, 3, 4, SpatialReferences.WGS84);
      MapPoint pt2 = MapPointBuilderEx.CreateMapPoint(5, 2, double.NaN, 7);

      // pt1.HasZ = true
      // pt1.HasM = true
      centroid = GeometryEngine.Instance.Centroid(pt1);
      // centroid.HasZ = true
      // centroid.HasM = true
      // pt1.IsEqual(centroid) = true

      // multipoint
      List<MapPoint> list = new List<MapPoint>() { pt1, pt2 };
      Multipoint multipoint = MultipointBuilderEx.CreateMultipoint(list);
      // multipoint.HasZ = true
      // multipoint.HasM = true

      centroid = GeometryEngine.Instance.Centroid(multipoint);
      // centroid.X = 3
      // centroid.Y = 2
      // centroid.HasZ = false
      // centroid.HasM = false
      #endregion
    }

    public void Clip()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Clip(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Envelope)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Clip(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Envelope)
      #region Clip a Polyline

      // clip a polyline by an envelope

      Envelope env = EnvelopeBuilderEx.CreateEnvelope(2.0, 2.0, 4.0, 4.0);
      LineSegment line = LineBuilderEx.CreateLineSegment(new Coordinate2D(0, 3), new Coordinate2D(5.0, 3.0));
      Polyline polyline = PolylineBuilderEx.CreatePolyline(line);

      Geometry clipGeom = GeometryEngine.Instance.Clip(polyline, env);
      #endregion

      // cref: ArcGIS.Core.Geometry.GeometryEngine.Clip(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Envelope)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Clip(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Envelope)
      // cref: ArcGIS.Core.Geometry.PolygonBuilderEx.CreatePolygon(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Coordinate2D},ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.Multipart.Extent
      #region Clip a Polyline by a Polygon

      // clip a polyline by a polygon

      List<Coordinate2D> list = new List<Coordinate2D>();
      list.Add(new Coordinate2D(1.0, 1.0));
      list.Add(new Coordinate2D(1.0, 4.0));
      list.Add(new Coordinate2D(4.0, 4.0));
      list.Add(new Coordinate2D(4.0, 1.0));

      Polygon polygon = PolygonBuilderEx.CreatePolygon(list, SpatialReferences.WGS84);

      LineSegment crossingLine = LineBuilderEx.CreateLineSegment(MapPointBuilderEx.CreateMapPoint(0, 3), MapPointBuilderEx.CreateMapPoint(5.0, 3.0));
      Polyline p = PolylineBuilderEx.CreatePolyline(crossingLine);
      Geometry geometry = GeometryEngine.Instance.Clip(p, polygon.Extent);
      #endregion
    }

    // ConstructMultipatchExtrude()
    // ConstructMultipatchExtrudeAlongLine
    // ConstructMultipatchExtrudeAlongVector3D
    // ConstructMultipathExtrudeFromToZ
    // ConstructMultipatchExtrudeToZ
    //      see Multipatch above

    public void ConstructGeodeticLineFromDistance()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.ConstructGeodeticLineFromDistance(ArcGIS.Core.Geometry.GeodeticCurveType,ArcGIS.Core.Geometry.MapPoint,System.Double,System.Double,ArcGIS.Core.Geometry.LinearUnit,ArcGIS.Core.Geometry.CurveDensifyMethod,System.Double)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.ConstructGeodeticLineFromDistance(ArcGIS.Core.Geometry.GeodeticCurveType,ArcGIS.Core.Geometry.MapPoint,System.Double,System.Double,ArcGIS.Core.Geometry.LinearUnit,ArcGIS.Core.Geometry.CurveDensifyMethod,System.Double)
      #region Construct a geodetic line with specified distance and azimuth
      var sr = SpatialReferenceBuilder.CreateSpatialReference(4326);
      var mapPoint = MapPointBuilderEx.CreateMapPoint(15, 60, sr);

      // calculate
      var polylineGeodetic = GeometryEngine.Instance.ConstructGeodeticLineFromDistance(GeodeticCurveType.Loxodrome, mapPoint, 5000000, 45, null, CurveDensifyMethod.ByLength, 300000);

      #endregion
    }

    public void ConstructGeodeticLineFromPoints()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.ConstructGeodeticLineFromPoints(ArcGIS.Core.Geometry.GeodeticCurveType,ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.LinearUnit,ArcGIS.Core.Geometry.CurveDensifyMethod,System.Double)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.ConstructGeodeticLineFromPoints(ArcGIS.Core.Geometry.GeodeticCurveType,ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.LinearUnit,ArcGIS.Core.Geometry.CurveDensifyMethod,System.Double)
      #region Construct a geodetic line connecting points
      var sr = SpatialReferenceBuilder.CreateSpatialReference(4326);

      var pt1 = MapPointBuilderEx.CreateMapPoint(60, 180, sr);
      var pt2 = MapPointBuilderEx.CreateMapPoint(60, 0, sr);

      // densify by length
      var gl = GeometryEngine.Instance.ConstructGeodeticLineFromPoints(GeodeticCurveType.Geodesic, pt1, pt2, null, CurveDensifyMethod.ByLength, -3.356);

      // densify by deviation
      gl = GeometryEngine.Instance.ConstructGeodeticLineFromPoints(GeodeticCurveType.Geodesic, pt1, pt2, null, CurveDensifyMethod.ByDeviation, -0.0026);

      #endregion
    }

    public void ConstructPointFromAngleDistance()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.ConstructPointFromAngleDistance(ArcGIS.Core.Geometry.MapPoint,System.Double,System.Double,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.ConstructPointFromAngleDistance(ArcGIS.Core.Geometry.MapPoint,System.Double,System.Double,ArcGIS.Core.Geometry.SpatialReference)
      #region Construct a Point at a distance and angle from an existing Point

      MapPoint inPoint = MapPointBuilderEx.CreateMapPoint(3, 4);
      double angle = 0;
      double distance = 10;

      MapPoint outPoint = GeometryEngine.Instance.ConstructPointFromAngleDistance(inPoint, angle, distance);
      // outPoint.X = 13
      // outPoint.Y = 4

      SpatialReference sr = SpatialReferences.WGS84;
      inPoint = MapPointBuilderEx.CreateMapPoint(0, 0, sr);
      angle = Math.PI;
      distance = 1;

      outPoint = GeometryEngine.Instance.ConstructPointFromAngleDistance(inPoint, angle, distance, sr);
      // outPoint.X = -1
      // outPoint.Y = 0

      #endregion
    }

    public void ConstructPolygonsFromPolylines()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.ConstructPolygonsFromPolylines(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Polyline})
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.ConstructPolygonsFromPolylines(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Polyline})
      #region Construct a Polygon from a set of Polylines

      List<Coordinate2D> firstLinePts = new List<Coordinate2D>();
      firstLinePts.Add(new Coordinate2D(1.0, 1.0));
      firstLinePts.Add(new Coordinate2D(1.0, 4.0));

      List<Coordinate2D> secondLinePts = new List<Coordinate2D>();
      secondLinePts.Add(new Coordinate2D(4.0, 4.0));
      secondLinePts.Add(new Coordinate2D(4.0, 1.0));

      List<Coordinate2D> thirdLinePts = new List<Coordinate2D>();
      thirdLinePts.Add(new Coordinate2D(0.0, 2.0));
      thirdLinePts.Add(new Coordinate2D(5.0, 2.0));
      
      List<Coordinate2D> fourthLinePts = new List<Coordinate2D>();
      fourthLinePts.Add(new Coordinate2D(0.0, 3.0));
      fourthLinePts.Add(new Coordinate2D(5.0, 3.0));

      // build the polylines
      List<Polyline> polylines = new List<Polyline>();
      polylines.Add(PolylineBuilderEx.CreatePolyline(firstLinePts));
      polylines.Add(PolylineBuilderEx.CreatePolyline(secondLinePts));
      polylines.Add(PolylineBuilderEx.CreatePolyline(thirdLinePts));
      polylines.Add(PolylineBuilderEx.CreatePolyline(fourthLinePts));

      // construct polygons from the polylines
      var polygons = GeometryEngine.Instance.ConstructPolygonsFromPolylines(polylines);

      // polygons.Count = 1
      // polygon coordinates are (1.0, 2.0), (1.0, 3.0), (4.0, 3.0), (4.0, 2.0)

      #endregion
    }

    public void Contains()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Contains(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Contains(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.ReadOnlyPointCollection.Item(System.Int32)
      #region Polygon contains MapPoints, Polylines, Polygons

      // build a polygon      
      List<MapPoint> pts = new List<MapPoint>();
      pts.Add(MapPointBuilderEx.CreateMapPoint(1.0, 1.0));
      pts.Add(MapPointBuilderEx.CreateMapPoint(1.0, 2.0));
      pts.Add(MapPointBuilderEx.CreateMapPoint(2.0, 2.0));
      pts.Add(MapPointBuilderEx.CreateMapPoint(2.0, 1.0));

      Polygon poly = PolygonBuilderEx.CreatePolygon(pts);

      // test if an inner point is contained
      MapPoint innerPt = MapPointBuilderEx.CreateMapPoint(1.5, 1.5);
      bool contains = GeometryEngine.Instance.Contains(poly, innerPt);   // contains = true

      // test a point on a boundary
      contains = GeometryEngine.Instance.Contains(poly, poly.Points[0]);     // contains = false

      // test an interior line
      MapPoint innerPt2 = MapPointBuilderEx.CreateMapPoint(1.25, 1.75);
      List<MapPoint> innerLinePts = new List<MapPoint>();
      innerLinePts.Add(innerPt);
      innerLinePts.Add(innerPt2);

      // test an inner polyline
      Polyline polyline = PolylineBuilderEx.CreatePolyline(innerLinePts);
      contains = GeometryEngine.Instance.Contains(poly, polyline);   // contains = true

      // test a line that crosses the boundary
      MapPoint outerPt = MapPointBuilderEx.CreateMapPoint(3, 1.5);
      List<MapPoint> crossingLinePts = new List<MapPoint>();
      crossingLinePts.Add(innerPt);
      crossingLinePts.Add(outerPt);

      polyline = PolylineBuilderEx.CreatePolyline(crossingLinePts);
      contains = GeometryEngine.Instance.Contains(poly, polyline);     // contains = false

      // test a polygon in polygon
      Envelope env = EnvelopeBuilderEx.CreateEnvelope(innerPt, innerPt2);
      contains = GeometryEngine.Instance.Contains(poly, env);      // contains = true
      #endregion
    }

    public void ConvexHull()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.ConvexHull(ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.ConvexHull(ArcGIS.Core.Geometry.Geometry)
      #region Determine convex hull

      //
      // convex hull around a point - returns a point
      //

      MapPoint pt = MapPointBuilderEx.CreateMapPoint(2.0, 2.0);
      Geometry hull = GeometryEngine.Instance.ConvexHull(pt);
      MapPoint hullPt = hull as MapPoint;
      // nullPt.X = 2
      // hullPt.Y = 2


      List<MapPoint> list = new List<MapPoint>();
      list.Add(MapPointBuilderEx.CreateMapPoint(1.0, 1.0));
      list.Add(MapPointBuilderEx.CreateMapPoint(1.0, 2.0));
      list.Add(MapPointBuilderEx.CreateMapPoint(2.0, 2.0));
      list.Add(MapPointBuilderEx.CreateMapPoint(2.0, 1.0));

      //
      // convex hull around a multipoint - returns a polygon
      //

      // build a multiPoint
      Multipoint multiPoint = MultipointBuilderEx.CreateMultipoint(list);

      hull = GeometryEngine.Instance.ConvexHull(multiPoint);
      Polygon hullPoly = hull as Polygon;
      // hullPoly.Area = 1
      // hullPoly.PointCount = 5

      // 
      // convex hull around a line - returns a polyline or polygon
      // 

      List<MapPoint> polylineList = new List<MapPoint>();
      polylineList.Add(MapPointBuilderEx.CreateMapPoint(1.0, 1.0));
      polylineList.Add(MapPointBuilderEx.CreateMapPoint(2.0, 2.0));

      // 2 point straight line
      Polyline polyline = PolylineBuilderEx.CreatePolyline(polylineList);
      hull = GeometryEngine.Instance.ConvexHull(polyline);
      Polyline hullPolyline = hull as Polyline;
      // hullPolyline.Length = Math.Sqrt(2)
      // hullPolyline.PointCount = 2

      // 3 point angular line
      polylineList.Add(MapPointBuilderEx.CreateMapPoint(2.0, 1.0));
      polyline = PolylineBuilderEx.CreatePolyline(polylineList);
      hull = GeometryEngine.Instance.ConvexHull(polyline);
      hullPoly = hull as Polygon;
      // hullPoly.Length = 2 + Math.Sqrt(2)
      // hullPoly.Area = 0.5
      // hullPoly.PointCount = 4

      //
      // convex hull around a polygon - returns a polygon
      //

      // simple polygon
      Polygon poly = PolygonBuilderEx.CreatePolygon(list);
      hull = GeometryEngine.Instance.ConvexHull(poly);
      hullPoly = hull as Polygon;

      // hullPoly.Length = 4.0
      // hullPoly.Area = 1.0
      // hullPoly.PointCount = 5

      // polygon with concave angles
      List<MapPoint> funkyList = new List<MapPoint>();
      funkyList.Add(MapPointBuilderEx.CreateMapPoint(1.0, 1.0));
      funkyList.Add(MapPointBuilderEx.CreateMapPoint(1.5, 1.5));
      funkyList.Add(MapPointBuilderEx.CreateMapPoint(1.0, 2.0));
      funkyList.Add(MapPointBuilderEx.CreateMapPoint(2.0, 2.0));
      funkyList.Add(MapPointBuilderEx.CreateMapPoint(1.5, 1.5));
      funkyList.Add(MapPointBuilderEx.CreateMapPoint(2.0, 1.0));
      funkyList.Add(MapPointBuilderEx.CreateMapPoint(1.0, 1.0));

      Polygon funkyPoly = PolygonBuilderEx.CreatePolygon(funkyList);
      hull = GeometryEngine.Instance.ConvexHull(funkyPoly);
      hullPoly = hull as Polygon;
      // hullPoly.Length = 4.0
      // hullPoly.Area = 1.0
      // hullPoly.PointCount = 5
      // hullPoly.Points[0] = 1.0, 1.0
      // hullPoly.Points[1] = 1.0, 2.0
      // hullPoly.Points[2] = 2.0, 2.0
      // hullPoly.Points[3] = 2.0, 1.0
      // hullPoly.Points[4] = 1.0, 1.0
      #endregion
    }

    public void Crosses()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Crosses(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Crosses(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Geometry)
      #region Determine if two geometries cross

      //
      // pt on pt
      //

      MapPoint pt = MapPointBuilderEx.CreateMapPoint(1.0, 1.0);
      MapPoint pt2 = MapPointBuilderEx.CreateMapPoint(2.0, 2.0);

      bool crosses = GeometryEngine.Instance.Crosses(pt, pt2);         // crosses = false
      crosses = GeometryEngine.Instance.Crosses(pt, pt);               // crosses = false

      // 
      // pt and line
      // 

      List<MapPoint> list = new List<MapPoint>();
      list.Add(MapPointBuilderEx.CreateMapPoint(1.0, 1.0));
      list.Add(MapPointBuilderEx.CreateMapPoint(3.0, 3.0));
      list.Add(MapPointBuilderEx.CreateMapPoint(5.0, 1.0));

      Polyline line1 = PolylineBuilderEx.CreatePolyline(list);
      crosses = GeometryEngine.Instance.Crosses(line1, pt2);           // crosses = false
      crosses = GeometryEngine.Instance.Crosses(pt2, line1);           // crosses = false
                                                                       // end pt of line
      crosses = GeometryEngine.Instance.Crosses(line1, pt);            // crosses = false

      //
      // pt and polygon
      //
      List<MapPoint> polyPts = new List<MapPoint>();
      polyPts.Add(MapPointBuilderEx.CreateMapPoint(3.0, 2.0));
      polyPts.Add(MapPointBuilderEx.CreateMapPoint(3.0, 6.0));
      polyPts.Add(MapPointBuilderEx.CreateMapPoint(6.0, 6.0));
      polyPts.Add(MapPointBuilderEx.CreateMapPoint(6.0, 2.0));

      Polygon poly1 = PolygonBuilderEx.CreatePolygon(polyPts);
      crosses = GeometryEngine.Instance.Crosses(poly1, pt);              // crosses = false
      crosses = GeometryEngine.Instance.Crosses(pt, poly1);              // crosses = false

      // 
      // line and line
      //
      List<MapPoint> list2 = new List<MapPoint>();
      list2.Add(MapPointBuilderEx.CreateMapPoint(1.0, 3.0));
      list2.Add(MapPointBuilderEx.CreateMapPoint(3.0, 1.0));
      list2.Add(MapPointBuilderEx.CreateMapPoint(5.0, 3.0));

      Polyline line2 = PolylineBuilderEx.CreatePolyline(list2);
      crosses = GeometryEngine.Instance.Crosses(line1, line2);           // crosses = true

      //
      // line and polygon
      //
      crosses = GeometryEngine.Instance.Crosses(poly1, line1);           // crosses = true

      //
      // polygon and polygon
      //
      Envelope env = EnvelopeBuilderEx.CreateEnvelope(MapPointBuilderEx.CreateMapPoint(1.0, 1.0), MapPointBuilderEx.CreateMapPoint(4, 4));
      Polygon poly2 = PolygonBuilderEx.CreatePolygon(env);
      crosses = GeometryEngine.Instance.Crosses(poly1, poly2);           // crosses = false

      #endregion
    }

    public void Cut()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Cut(ArcGIS.Core.Geometry.Multipart,ArcGIS.Core.Geometry.Polyline)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Cut(ArcGIS.Core.Geometry.Multipart,ArcGIS.Core.Geometry.Polyline)
      #region Cut a geometry with a polyline

      SpatialReference sr = SpatialReferences.WGS84;

      List<MapPoint> list = new List<MapPoint>();
      list.Add(MapPointBuilderEx.CreateMapPoint(1.0, 1.0, sr));
      list.Add(MapPointBuilderEx.CreateMapPoint(1.0, 4.0, sr));
      list.Add(MapPointBuilderEx.CreateMapPoint(4.0, 4.0, sr));
      list.Add(MapPointBuilderEx.CreateMapPoint(4.0, 1.0, sr));

      List<Geometry> cutGeometries;

      LineSegment line = LineBuilderEx.CreateLineSegment(MapPointBuilderEx.CreateMapPoint(0, 0, sr), MapPointBuilderEx.CreateMapPoint(3, 6, sr));
      Polyline cutter = PolylineBuilderEx.CreatePolyline(line);

      // polyline
      Polyline polyline = PolylineBuilderEx.CreatePolyline(list);
      bool isSimple = GeometryEngine.Instance.IsSimpleAsFeature(polyline);
      cutGeometries = GeometryEngine.Instance.Cut(polyline, cutter) as List<Geometry>;

      Polyline leftPolyline = cutGeometries[0] as Polyline;
      Polyline rightPolyline = cutGeometries[1] as Polyline;

      // leftPolyline.Points[0] = 1, 2
      // leftPolyline.Points[1] = 1, 4
      // leftPolyline.Points[2] = 2, 4

      // rightPolyline.Points.Count = 5
      // rightPolyline.Parts.Count = 2

      ReadOnlySegmentCollection segments0 = rightPolyline.Parts[0];
      // segments0[0].StartCoordinate = 1, 1
      // segments0[0].EndCoordinate = 1, 2

      ReadOnlySegmentCollection segments1 = rightPolyline.Parts[1];
      // segments1.Count = 2
      // segments1[0].StartCoordinate = 2, 4
      // segments1[0].EndCoordinate = 4, 4
      // segments1[1].StartCoordinate = 4, 4
      // segments1[1].EndCoordinate = 4, 1

      // polygon 
      Polygon polygon = PolygonBuilderEx.CreatePolygon(list);
      isSimple = GeometryEngine.Instance.IsSimpleAsFeature(polygon);
      cutGeometries = GeometryEngine.Instance.Cut(polygon, cutter) as List<Geometry>;
      #endregion
    }

    public void DensifyByLength()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.DensifyByLength(ArcGIS.Core.Geometry.Geometry,System.Double)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.DensifyByLength(ArcGIS.Core.Geometry.Geometry,System.Double)
      #region Densify by Length

      // densify a line segment
      MapPoint startPt = MapPointBuilderEx.CreateMapPoint(1.0, 1.0);
      MapPoint endPt = MapPointBuilderEx.CreateMapPoint(1, 21);
      LineSegment line = LineBuilderEx.CreateLineSegment(startPt, endPt);
      Polyline polyline = PolylineBuilderEx.CreatePolyline(line);

      Geometry geom = GeometryEngine.Instance.DensifyByLength(polyline, 2);
      Polyline result = geom as Polyline;


      // densify a circular arc
      MapPoint fromPt = MapPointBuilderEx.CreateMapPoint(2, 1);
      MapPoint toPt = MapPointBuilderEx.CreateMapPoint(1, 2);
      Coordinate2D interiorPt = new Coordinate2D(1 + Math.Sqrt(2) / 2, 1 + Math.Sqrt(2) / 2);

      EllipticArcBuilderEx cab = new EllipticArcBuilderEx(fromPt, toPt, interiorPt);
      EllipticArcSegment circularArc = cab.ToSegment();
      polyline = PolylineBuilderEx.CreatePolyline(circularArc);
      geom = GeometryEngine.Instance.DensifyByLength(polyline, 2);
      result = geom as Polyline;

      #endregion
    }

    public void Difference()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Difference(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Difference(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Geometry)
      #region Difference between two Polygons

      List<MapPoint> polyPts = new List<MapPoint>();
      polyPts.Add(MapPointBuilderEx.CreateMapPoint(3.0, 2.0));
      polyPts.Add(MapPointBuilderEx.CreateMapPoint(3.0, 6.0));
      polyPts.Add(MapPointBuilderEx.CreateMapPoint(6.0, 6.0));
      polyPts.Add(MapPointBuilderEx.CreateMapPoint(6.0, 2.0));

      Polygon poly1 = PolygonBuilderEx.CreatePolygon(polyPts);

      Envelope env = EnvelopeBuilderEx.CreateEnvelope(MapPointBuilderEx.CreateMapPoint(1.0, 1.0), MapPointBuilderEx.CreateMapPoint(4, 4));
      Polygon poly2 = PolygonBuilderEx.CreatePolygon(env);

      Geometry result = GeometryEngine.Instance.Difference(poly1, poly2);
      Polygon polyResult = result as Polygon;
      // polyResult.Area = 10.0

      result = GeometryEngine.Instance.Difference(poly2, poly1);
      polyResult = result as Polygon;
      // polyResult.Area = 7.0
      #endregion
    }

    public void Disjoint_Disjoint3D()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Disjoint(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Disjoint(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Disjoint3D(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Disjoint3D(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Geometry)
      #region Determine if two Geometries are disjoint

      //
      // pt on pt
      //

      MapPoint pt = MapPointBuilderEx.CreateMapPoint(1.0, 1.0);
      MapPoint pt2 = MapPointBuilderEx.CreateMapPoint(2.0, 2.5);

      bool disjoint = GeometryEngine.Instance.Disjoint(pt, pt2);       // result is true

      MultipointBuilderEx mpb = new MultipointBuilderEx();
      mpb.AddPoint(pt);
      mpb.AddPoint(pt2);
      Multipoint multiPoint = mpb.ToGeometry();

      disjoint = GeometryEngine.Instance.Disjoint(multiPoint, pt);     // result is false

      // 
      // pt and line
      // 

      List<MapPoint> list = new List<MapPoint>();
      list.Add(MapPointBuilderEx.CreateMapPoint(1.0, 1.0));
      list.Add(MapPointBuilderEx.CreateMapPoint(3.0, 3.0));
      list.Add(MapPointBuilderEx.CreateMapPoint(5.0, 1.0));

      Polyline line1 = PolylineBuilderEx.CreatePolyline(list);
      disjoint = GeometryEngine.Instance.Disjoint(line1, pt2);       // result is true

      disjoint = GeometryEngine.Instance.Disjoint(pt2, line1);       // result is true

      // end pt of line
      disjoint = GeometryEngine.Instance.Disjoint(line1, pt);        // result is false

      //
      // pt and polygon
      //
      List<MapPoint> polyPts = new List<MapPoint>();
      polyPts.Add(MapPointBuilderEx.CreateMapPoint(3.0, 2.0));
      polyPts.Add(MapPointBuilderEx.CreateMapPoint(3.0, 6.0));
      polyPts.Add(MapPointBuilderEx.CreateMapPoint(6.0, 6.0));
      polyPts.Add(MapPointBuilderEx.CreateMapPoint(6.0, 2.0));

      Polygon poly1 = PolygonBuilderEx.CreatePolygon(polyPts);
      disjoint = GeometryEngine.Instance.Disjoint(poly1, pt);          // result is true
      disjoint = GeometryEngine.Instance.Disjoint(pt, poly1);          // result is true

      // 
      // line and line
      //

      List<MapPoint> list2 = new List<MapPoint>();
      list2.Add(MapPointBuilderEx.CreateMapPoint(1.0, 3.0));
      list2.Add(MapPointBuilderEx.CreateMapPoint(3.0, 1.0));
      list2.Add(MapPointBuilderEx.CreateMapPoint(5.0, 3.0));

      Polyline line2 = PolylineBuilderEx.CreatePolyline(list2);
      disjoint = GeometryEngine.Instance.Disjoint(line1, line2);         // result is false

      //
      // line and polygon
      //
      disjoint = GeometryEngine.Instance.Disjoint(poly1, line1);         // result is false
      disjoint = GeometryEngine.Instance.Disjoint(line1, poly1);         // result is false

      //
      // polygon and polygon
      //
      Envelope env = EnvelopeBuilderEx.CreateEnvelope(MapPointBuilderEx.CreateMapPoint(1.0, 1.0), MapPointBuilderEx.CreateMapPoint(4, 4));
      Polygon poly2 = PolygonBuilderEx.CreatePolygon(env);

      disjoint = GeometryEngine.Instance.Disjoint(poly1, poly2);         // result is false


      // disjoint3D

      SpatialReference sr = SpatialReferences.WGS84;

      MapPoint pt3D_1 = MapPointBuilderEx.CreateMapPoint(1, 1, 1, sr);
      MapPoint pt3D_2 = MapPointBuilderEx.CreateMapPoint(2, 2, 2, sr);
      MapPoint pt3D_3 = MapPointBuilderEx.CreateMapPoint(1, 1, 2, sr);

      MultipointBuilderEx mpbEx = new MultipointBuilderEx();
      mpbEx.AddPoint(pt3D_1);
      mpbEx.AddPoint(pt3D_2);
      mpbEx.HasZ = true;

      multiPoint = mpbEx.ToGeometry();
      disjoint = GeometryEngine.Instance.Disjoint3D(multiPoint, pt3D_2);     // disjoint = false
      disjoint = GeometryEngine.Instance.Disjoint3D(multiPoint, pt3D_3);     // disjoint = true
      #endregion
    }

    public void Distance()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Distance(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Distance(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Geometry)
      #region Determine distance between two Geometries

      MapPoint pt1 = MapPointBuilderEx.CreateMapPoint(1.0, 1.0);
      MapPoint pt2 = MapPointBuilderEx.CreateMapPoint(2.0, 2.0);
      MapPoint pt3 = MapPointBuilderEx.CreateMapPoint(4.0, 2.0);

      //
      // pt and pt 
      //
      double d = GeometryEngine.Instance.Distance(pt1, pt2);         // d = Math.Sqrt(2)

      //
      // pt and multipoint
      //
      List<MapPoint> multiPts = new List<MapPoint>();
      multiPts.Add(pt2);
      multiPts.Add(pt3);
      Multipoint multiPoint = MultipointBuilderEx.CreateMultipoint(multiPts);
      d = GeometryEngine.Instance.Distance(pt1, multiPoint);         // d = Math.Sqrt(2)

      //
      // pt and envelope
      //
      Envelope env = EnvelopeBuilderEx.CreateEnvelope(pt1, pt2);
      d = GeometryEngine.Instance.Distance(pt1, env);                // d = 0

      //
      // pt and polyline
      //
      List<MapPoint> polylinePts = new List<MapPoint>();
      polylinePts.Add(MapPointBuilderEx.CreateMapPoint(2.0, 1.0));
      polylinePts.Add(pt2);
      Polyline polyline = PolylineBuilderEx.CreatePolyline(polylinePts);

      d = GeometryEngine.Instance.Distance(pt1, polyline);             // d = 1.0

      //
      // pt and polygon
      //
      Envelope env2 = EnvelopeBuilderEx.CreateEnvelope(MapPointBuilderEx.CreateMapPoint(3.0, 3.0), MapPointBuilderEx.CreateMapPoint(5.0, 5.0));
      Polygon poly = PolygonBuilderEx.CreatePolygon(env2);
      d = GeometryEngine.Instance.Distance(pt1, poly);                 // d = Math.Sqrt(8)

      //
      // envelope and polyline
      //
      d = GeometryEngine.Instance.Distance(env, polyline);             // d = 0

      //
      // polyline and polyline
      //
      List<MapPoint> polylineList = new List<MapPoint>();
      polylineList.Add(MapPointBuilderEx.CreateMapPoint(4, 3));
      polylineList.Add(MapPointBuilderEx.CreateMapPoint(4, 4));
      Polyline polyline2 = PolylineBuilderEx.CreatePolyline(polylineList);

      d = GeometryEngine.Instance.Distance(polyline, polyline2);           // d = Math.Sqrt(5)


      //
      // polygon and polygon
      //
      Polygon poly2 = PolygonBuilderEx.CreatePolygon(env);
      d = GeometryEngine.Instance.Distance(poly, poly2);                 // d = Math.Sqrt(2)

      #endregion
    }

    public void Distance3D()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Distance3D(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Distance3D(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Geometry)
      #region Determine 3D distance between two Geometries

      // between points 
      MapPoint pt1 = MapPointBuilderEx.CreateMapPoint(1, 1, 1);
      MapPoint pt2 = MapPointBuilderEx.CreateMapPoint(2, 2, 2);
      MapPoint pt3 = MapPointBuilderEx.CreateMapPoint(10, 2, 1);

      // pt1 to pt2
      double d = GeometryEngine.Instance.Distance3D(pt1, pt2);        // d = Math.Sqrt(3)

      // pt1 to pt3
      d = GeometryEngine.Instance.Distance3D(pt1, pt3);        // d = Math.Sqrt(82)

      // pt2 to pt3
      d = GeometryEngine.Instance.Distance3D(pt2, pt3);        // d = Math.Sqrt(65)

      // intersecting lines

      List<MapPoint> list = new List<MapPoint>();
      list.Add(MapPointBuilderEx.CreateMapPoint(1.0, 1.0, 1.0));
      list.Add(MapPointBuilderEx.CreateMapPoint(3.0, 3.0, 1.0));
      list.Add(MapPointBuilderEx.CreateMapPoint(5.0, 1.0, 1.0));

      Polyline line1 = PolylineBuilderEx.CreatePolyline(list);

      List<MapPoint> list2 = new List<MapPoint>();
      list2.Add(MapPointBuilderEx.CreateMapPoint(1.0, 3.0, 1.0));
      list2.Add(MapPointBuilderEx.CreateMapPoint(3.0, 1.0, 1.0));
      list2.Add(MapPointBuilderEx.CreateMapPoint(5.0, 3.0, 1.0));

      Polyline line2 = PolylineBuilderEx.CreatePolyline(list2);

      bool intersects = GeometryEngine.Instance.Intersects(line1, line2);    // intersects = true
      d = GeometryEngine.Instance.Distance3D(line1, line2);                  // d = 0   (distance is 0 when geomtries intersect)

      #endregion
    }

    public void Expand()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Expand(ArcGIS.Core.Geometry.Envelope,System.Double,System.Double,System.Boolean)
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Expand(ArcGIS.Core.Geometry.Envelope,System.Double,System.Double,System.Double,System.Boolean)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Expand(ArcGIS.Core.Geometry.Envelope,System.Double,System.Double,System.Boolean)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Expand(ArcGIS.Core.Geometry.Envelope,System.Double,System.Double,System.Double,System.Boolean)
      #region Expand envelopes

      Envelope env = EnvelopeBuilderEx.CreateEnvelope(100.0, 100.0, 500.0, 500.0);
      // env.HasZ = false

      Envelope result = GeometryEngine.Instance.Expand(env, 0.5, 0.5, true);
      // result.Center = 300, 300
      // result.XMin = 200
      // result.YMin = 200
      // result.XMax = 400
      // result.YMax = 400

      result = GeometryEngine.Instance.Expand(env, 100, 200, false);
      // result.Center = 300, 300
      // result.XMin = 0
      // result.YMin = -100
      // result.XMax = 600
      // result.YMax = 700

      try
      {
        // expand in 3 dimensions
        result = GeometryEngine.Instance.Expand(env, 0.5, 0.5, 0.5, true);
      }
      catch (InvalidOperationException e)
      {
        // the geometry is not Z-Aware
      }

      // expand a 3d envelope
      MapPoint pt1 = MapPointBuilderEx.CreateMapPoint(100, 100, 100);
      MapPoint pt2 = MapPointBuilderEx.CreateMapPoint(200, 200, 200);

      env = EnvelopeBuilderEx.CreateEnvelope(pt1, pt2);
      // env.HasZ = true

      result = GeometryEngine.Instance.Expand(env, 0.5, 0.5, 0.5, true);

      // result.Center = 150, 150, 150
      // result.XMin = 125
      // result.YMin = 125
      // result.ZMin = 125
      // result.XMax = 175
      // result.YMax = 175
      // result.ZMax = 175

      #endregion
    }

    public void Extend()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Extend(ArcGIS.Core.Geometry.Polyline,ArcGIS.Core.Geometry.Polyline,ArcGIS.Core.Geometry.ExtendFlags)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Extend(ArcGIS.Core.Geometry.Polyline,ArcGIS.Core.Geometry.Polyline,ArcGIS.Core.Geometry.ExtendFlags)
      #region Extend a polyline

      // build a polyline
      var polyline = PolylineBuilderEx.CreatePolyline(new[]
      {
        MapPointBuilderEx.CreateMapPoint(1, 1, 10, 20),
        MapPointBuilderEx.CreateMapPoint(0, 0, 10, 20),
        MapPointBuilderEx.CreateMapPoint(1, -1, 10, 20)
      });

      // build the extender line
      var extender = PolylineBuilderEx.CreatePolyline(new[]
      {
        MapPointBuilderEx.CreateMapPoint(2, 2),
        MapPointBuilderEx.CreateMapPoint(2, -2),
      });

      // extend
      var result = GeometryEngine.Instance.Extend(polyline, extender, ExtendFlags.KeepEndAttributes);
      Polyline extendedLine = result as Polyline;
      // result.Parts[0].Points[0] = 2, 2, 10, 20
      // result.parts[0].Points[1] = 1, 1, 10, 20
      // result.Parts[0].Points[2] = 0, 0, 10, 20
      // result.Parts[0].Points[3] = 1, -1, 10, 20
      // result.Parts[0].Points[4] = 2, -2, 10, 20

      // change the flags
      result = GeometryEngine.Instance.Extend(polyline, extender, ExtendFlags.NoEndAttributes);
      extendedLine = result as Polyline;
      // result.Parts[0].Points[0] = 2, 2, 0
      // result.parts[0].Points[1] = 1, 1, 10, 20
      // result.Parts[0].Points[2] = 0, 0, 10, 20
      // result.Parts[0].Points[3] = 1, -1, 10, 20
      // result.Parts[0].Points[4] = 2, -2, 0

      // extend
      result = GeometryEngine.Instance.Extend(polyline, extender, ExtendFlags.KeepEndAttributes | ExtendFlags.NoExtendAtTo);
      extendedLine = result as Polyline;
      // result.Parts[0].Points[0] = 2, 2, 10, 20
      // result.parts[0].Points[1] = 1, 1, 10, 20
      // result.Parts[0].Points[2] = 0, 0, 10, 20
      // result.Parts[0].Points[3] = 1, -1, 10, 20

      // extend with no intersection 

      polyline = PolylineBuilderEx.CreatePolyline(new[]
      {
        MapPointBuilderEx.CreateMapPoint(1, 1),
        MapPointBuilderEx.CreateMapPoint(3, 1)
      });

      extender = PolylineBuilderEx.CreatePolyline(new[]
      {
        MapPointBuilderEx.CreateMapPoint(1, 4),
        MapPointBuilderEx.CreateMapPoint(3, 4)
      });

      result = GeometryEngine.Instance.Extend(polyline, extender, ExtendFlags.Default);
      // result = null

      #endregion
    }

    public void Generalize()
    {
      Polyline polylineWithZ = null;

      // cref: ArcGIS.Core.Geometry.GeometryEngine.Generalize(ArcGIS.Core.Geometry.Geometry,System.Double,System.Boolean,System.Boolean)
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Generalize3D(ArcGIS.Core.Geometry.Geometry,System.Double)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Generalize(ArcGIS.Core.Geometry.Geometry,System.Double,System.Boolean,System.Boolean)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Generalize3D(ArcGIS.Core.Geometry.Geometry,System.Double)
      #region Generalize

      Polyline generalizedPolyline = GeometryEngine.Instance.Generalize(polylineWithZ, 200) as Polyline;
      // generalizedPolyline.HasZ = true

      Polygon generalized3DPolyline = GeometryEngine.Instance.Generalize3D(polylineWithZ, 200) as Polygon;
      // generalized3DPolyline.HasZ = true

      // note that generalized3DPolyline and generalizedPolyline will have different definitions
      //  ie generalizedPolyline.IsEqual(generalized3DPolyline) = false

      #endregion 
    }

    public void GeodesicArea()
    {
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.GeodesicArea(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.AreaUnit)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.GeodesicArea(ArcGIS.Core.Geometry.Geometry)
      #region Calculate the Geodesic Area of a polygon
      var polygon = PolygonBuilderEx.CreatePolygon(new[]
      {
        MapPointBuilderEx.CreateMapPoint(-10018754.1713946, 10018754.1713946),
        MapPointBuilderEx.CreateMapPoint(10018754.1713946, 10018754.1713946),
        MapPointBuilderEx.CreateMapPoint(10018754.1713946, -10018754.1713946),
        MapPointBuilderEx.CreateMapPoint(-10018754.1713946, -10018754.1713946)
      }, SpatialReferences.WebMercator);
      var area = GeometryEngine.Instance.GeodesicArea(polygon);

      // area is close to 255032810857732.31

      area = GeometryEngine.Instance.GeodesicArea(polygon, AreaUnit.SquareKilometers);
      // area is close to 255032810.85953, 

      #endregion
    }

    public void GeodesicBuffer()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.GeodesicBuffer(ArcGIS.Core.Geometry.Geometry,System.Double)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.GeodesicBuffer(ArcGIS.Core.Geometry.Geometry,System.Double)
      // cref: ArcGIS.Core.Geometry.GeometryEngine.GeodesicBuffer(ArcGIS.Core.Geometry.Geometry,System.Double,ArcGIS.Core.Geometry.LinearUnit)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.GeodesicBuffer(ArcGIS.Core.Geometry.Geometry,System.Double,ArcGIS.Core.Geometry.LinearUnit)
      // cref: ArcGIS.Core.Geometry.GeometryEngine.GeodesicBuffer(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Geometry},System.Double)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.GeodesicBuffer(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Geometry},System.Double)
      // cref: ArcGIS.Core.Geometry.GeometryEngine.GeodesicBuffer(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Geometry},System.Double,ArcGIS.Core.Geometry.LinearUnit)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.GeodesicBuffer(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Geometry},System.Double,ArcGIS.Core.Geometry.LinearUnit)
      #region Create a buffer polygon at the specified geodesic distance

      // buffer a point
      MapPoint pt = MapPointBuilderEx.CreateMapPoint(1.0, 1.0, SpatialReferences.WGS84);
      Polygon outPolygon = GeometryEngine.Instance.GeodesicBuffer(pt, 5) as Polygon;

      double delta = SpatialReferences.WGS84.XYTolerance * 2 * Math.Sqrt(2);
      ReadOnlyPointCollection points = outPolygon.Points;
      foreach (MapPoint p in points)
      {
        double d = GeometryEngine.Instance.GeodesicDistance(pt, p);
        // d = 5 (+- delta)
      }

      // specify a unit for the distance
      outPolygon = GeometryEngine.Instance.GeodesicBuffer(pt, 5000, LinearUnit.Millimeters) as Polygon;

      // buffer of 0 distance produces an empty geometry
      Geometry g = GeometryEngine.Instance.GeodesicBuffer(pt, 0);
      // g.IsEmpty = true

      // buffer many points
      List<MapPoint> list = new List<MapPoint>();
      list.Add(MapPointBuilderEx.CreateMapPoint(1.0, 1.0, SpatialReferences.WGS84));
      list.Add(MapPointBuilderEx.CreateMapPoint(10.0, 20.0));
      list.Add(MapPointBuilderEx.CreateMapPoint(40.0, 40.0));
      list.Add(MapPointBuilderEx.CreateMapPoint(60.0, 60.0));

      outPolygon = GeometryEngine.Instance.GeodesicBuffer(list, 10000) as Polygon;
      // outPolygon.PartCount = 4

      // buffer different geometry types
      List<Coordinate2D> coords = new List<Coordinate2D>()
      {
        new Coordinate2D(1, 2), new Coordinate2D(10, 20), new Coordinate2D(20, 30),
        new Coordinate2D(50, 60), new Coordinate2D(70, 80), new Coordinate2D(80, 40),
        new Coordinate2D(90, 10), new Coordinate2D(110, 15), new Coordinate2D(120, 30),
        new Coordinate2D(10, 40), new Coordinate2D(-10, 40), new Coordinate2D(-10, 50)
      };

      List<Geometry> manyGeometries = new List<Geometry>
      {
        MapPointBuilderEx.CreateMapPoint(1.0, 1.0, SpatialReferences.WGS84),
        PolylineBuilderEx.CreatePolyline(new List<Coordinate2D>(){coords[0], coords[1], coords[2]}, SpatialReferences.WGS84),
        PolylineBuilderEx.CreatePolyline(new List<Coordinate2D>(){coords[3], coords[4], coords[5]}),
        PolygonBuilderEx.CreatePolygon(new List<Coordinate2D>(){coords[9], coords[10], coords[11]})
      };

      outPolygon = GeometryEngine.Instance.GeodesicBuffer(manyGeometries, 20000) as Polygon;

      // specify unit types
      outPolygon = GeometryEngine.Instance.GeodesicBuffer(manyGeometries, 20, LinearUnit.Miles) as Polygon;


      #endregion
    }

    public void GeodesicDistance()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.GeodesicDistance(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.GeometryEngine.GeodesicDistance(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.LinearUnit)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.GeodesicDistance(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.GeodesicDistance(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.LinearUnit)
      #region Determine geodesic distance between two Geometries
      var point1 = MapPointBuilderEx.CreateMapPoint(-170, 45, SpatialReferences.WGS84);
      var point2 = MapPointBuilderEx.CreateMapPoint(170, 45, SpatialReferences.WGS84);

      var distances_meters = GeometryEngine.Instance.GeodesicDistance(point1, point2);
      // distance is approximately 1572912.2066940258 in meters

      var distance_feet = GeometryEngine.Instance.GeodesicDistance(point1, point2, LinearUnit.Feet);
      // distance is approximately  5160473.11904786 in feet

      #endregion
    }

    public void GeodesicEllipse()
    {
      // cref: ArcGIS.Core.Geometry.GeodesicEllipseParameter
      // cref: ArcGIS.Core.Geometry.GeodesicEllipseParameter.AxisDirection
      // cref: ArcGIS.Core.Geometry.GeodesicEllipseParameter.Center
      // cref: ArcGIS.Core.Geometry.GeodesicEllipseParameter.LinearUnit
      // cref: ArcGIS.Core.Geometry.GeodesicEllipseParameter.SemiAxis1Length
      // cref: ArcGIS.Core.Geometry.GeodesicEllipseParameter.SemiAxis2Length
      // cref: ArcGIS.Core.Geometry.GeodesicEllipseParameter.VertexCount
      // cref: ArcGIS.Core.Geometry.GeodesicEllipseParameter.OutGeometryType
      // cref: ArcGIS.Core.Geometry.GeometryEngine.GeodesicEllipse(ArcGIS.Core.Geometry.GeodesicEllipseParameter,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.GeodesicEllipse(ArcGIS.Core.Geometry.GeodesicEllipseParameter,ArcGIS.Core.Geometry.SpatialReference)
      #region GeodesicEllipse

      GeodesicEllipseParameter param = new GeodesicEllipseParameter();
      param.AxisDirection = 4 * Math.PI / 3;
      param.Center = new Coordinate2D(-120, 60);
      param.LinearUnit = LinearUnit.Meters;
      param.OutGeometryType = GeometryType.Polyline;
      param.SemiAxis1Length = 6500000;
      param.SemiAxis2Length = 1500000;
      param.VertexCount = 800;

      Geometry geometry = GeometryEngine.Instance.GeodesicEllipse(param, SpatialReferences.WGS84);
      // geometry.IsEmpty = false

      Polyline polyline = geometry as Polyline;
      // polyline.PointCount = 801
      // polyline.PartCount = 1
      #endregion
    }
    public void GeodesicLength()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.GeodesicLength(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.LinearUnit)
      // cref: ArcGIS.Core.Geometry.GeometryEngine.GeodesicLength(ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.GeodesicLength(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.LinearUnit)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.GeodesicLength(ArcGIS.Core.Geometry.Geometry)
      #region Determine geodesic length of a line

      var polyline = PolylineBuilderEx.CreatePolyline(new[]
      {
          MapPointBuilderEx.CreateMapPoint(-10018754.1713946, 10018754.1713946),
          MapPointBuilderEx.CreateMapPoint(10018754.1713946, 10018754.1713946)
      }, SpatialReferences.WebMercator);

      var length = GeometryEngine.Instance.GeodesicLength(polyline);
      // length is approx 5243784.5551844323 in meters

      length = GeometryEngine.Instance.GeodesicLength(polyline, LinearUnit.Miles);
      // length is approx 3258.33666089067 in miles

      var polyline2 = GeometryEngine.Instance.Project(polyline, SpatialReferences.WGS84);
      length = GeometryEngine.Instance.GeodesicLength(polyline2);
      // length is approx 5243784.55518443 in meters after projecting

      #endregion
    }

    public void GeodesicSector()
    {
      // cref: ArcGIS.Core.Geometry.GeodesicSectorParameter
      // cref: ArcGIS.Core.Geometry.GeodesicSectorParameter.ArcVertexCount
      // cref: ArcGIS.Core.Geometry.GeodesicSectorParameter.AxisDirection
      // cref: ArcGIS.Core.Geometry.GeodesicSectorParameter.Center
      // cref: ArcGIS.Core.Geometry.GeodesicSectorParameter.LinearUnit
      // cref: ArcGIS.Core.Geometry.GeodesicSectorParameter.OutGeometryType
      // cref: ArcGIS.Core.Geometry.GeodesicSectorParameter.RadiusVertexCount
      // cref: ArcGIS.Core.Geometry.GeodesicSectorParameter.SectorAngle
      // cref: ArcGIS.Core.Geometry.GeodesicSectorParameter.SemiAxis1Length
      // cref: ArcGIS.Core.Geometry.GeodesicSectorParameter.SemiAxis2Length
      // cref: ArcGIS.Core.Geometry.GeodesicSectorParameter.StartDirection
      // cref: ArcGIS.Core.Geometry.GeometryEngine.GeodesicSector(ArcGIS.Core.Geometry.GeodesicSectorParameter,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.GeodesicSector(ArcGIS.Core.Geometry.GeodesicSectorParameter,ArcGIS.Core.Geometry.SpatialReference)
      #region GeodesicSector

      GeodesicSectorParameter param = new GeodesicSectorParameter();
      param.ArcVertexCount = 50;
      param.AxisDirection = Math.PI / 4;
      param.Center = new Coordinate2D(0, 0);
      param.LinearUnit = LinearUnit.Meters;
      param.OutGeometryType = GeometryType.Polygon;
      param.RadiusVertexCount = 10;
      param.SectorAngle = 5 * Math.PI / 3;
      param.SemiAxis1Length = 100000;
      param.SemiAxis2Length = 20000;
      param.StartDirection = 11 * Math.PI / 6;

      Geometry geometry = GeometryEngine.Instance.GeodesicSector(param, SpatialReferences.WGS84);
      // geometry.IsEmpty = false

      Polygon polygon = geometry as Polygon;
      // polygon.PointCount = 68
      // polygon.PartCount = 1
      #endregion
    }

    public void GeodeticDensifyByDeviation()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.GeodeticDensifyByDeviation(ArcGIS.Core.Geometry.Geometry,System.Double,ArcGIS.Core.Geometry.LinearUnit,ArcGIS.Core.Geometry.GeodeticCurveType)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.GeodeticDensifyByDeviation(ArcGIS.Core.Geometry.Geometry,System.Double,ArcGIS.Core.Geometry.LinearUnit,ArcGIS.Core.Geometry.GeodeticCurveType)
      #region GeodeticDensifyByDeviation - polyline

      List<Coordinate2D> coords = new List<Coordinate2D>()
      {
        new Coordinate2D(-80, 0),
        new Coordinate2D(-20, 60),
        new Coordinate2D(40, 20),
        new Coordinate2D(0, -20),
        new Coordinate2D(-80, 0)
      };

      SpatialReference sr = SpatialReferences.WGS84;

      // create a polyline
      Polyline polyline = PolylineBuilderEx.CreatePolyline(coords, sr);

      // densify in km
      Polyline geodesicPolyline = GeometryEngine.Instance.GeodeticDensifyByDeviation(polyline, 200, LinearUnit.Kilometers, GeodeticCurveType.Geodesic) as Polyline;
      // densify in m
      geodesicPolyline = GeometryEngine.Instance.GeodeticDensifyByDeviation(polyline, 200, LinearUnit.Meters, GeodeticCurveType.Geodesic) as Polyline;

      // Change curve type to Loxodrome
      Polyline loxodromePolyline = GeometryEngine.Instance.GeodeticDensifyByDeviation(polyline, 200, LinearUnit.Meters, GeodeticCurveType.Loxodrome) as Polyline;

      #endregion
    }

    public void GeodeticDensifyByLength()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.GeodesicLength(ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.GeodesicLength(ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.GeometryEngine.GeodeticDensifyByLength(ArcGIS.Core.Geometry.Geometry,System.Double,ArcGIS.Core.Geometry.LinearUnit,ArcGIS.Core.Geometry.GeodeticCurveType)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.GeodeticDensifyByLength(ArcGIS.Core.Geometry.Geometry,System.Double,ArcGIS.Core.Geometry.LinearUnit,ArcGIS.Core.Geometry.GeodeticCurveType)
      #region GeodeticDensifyByLength - polygon

      List<Coordinate2D> coords = new List<Coordinate2D>()
      {
        new Coordinate2D(-80, 0),
        new Coordinate2D(-20, 60),
        new Coordinate2D(40, 20),
        new Coordinate2D(0, -20),
        new Coordinate2D(-80, 0)
      };

      SpatialReference sr = SpatialReferences.WGS84;

      // create a polygon
      Polygon polygon = PolygonBuilderEx.CreatePolygon(coords, sr);

      // get the geodesic lengths of the polygon segments
      ReadOnlySegmentCollection segments = polygon.Parts[0];
      List<Double> geoLengths = new List<Double>(segments.Count);
      foreach (Segment s in segments)
      {
        Polyline line = PolylineBuilderEx.CreatePolyline(s, sr);
        double geoLen = GeometryEngine.Instance.GeodesicLength(line);
        geoLengths.Add(geoLen);
      }

      // find the max length
      geoLengths.Sort();
      double maxLen = geoLengths[geoLengths.Count - 1];

      // densify the polygon (in meters)
      Polygon densifiedPoly = GeometryEngine.Instance.GeodeticDensifyByLength(polygon, maxLen, LinearUnit.Meters, GeodeticCurveType.Geodesic) as Polygon;

      // densify the polygon (in km)
      double maxSegmentLength = maxLen / 10000;
      densifiedPoly = GeometryEngine.Instance.GeodeticDensifyByLength(polygon, maxSegmentLength, LinearUnit.Kilometers, GeodeticCurveType.Geodesic) as Polygon;

      #endregion
    }

    public void GeodeticDistanceAndAzimuth()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.GeodeticDistanceAndAzimuth(ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.GeodeticCurveType,System.Double@,System.Double@)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.GeodeticDistanceAndAzimuth(ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.GeodeticCurveType,System.Double@,System.Double@)
      // cref: ArcGIS.Core.Geometry.GeometryEngine.GeodeticDistanceAndAzimuth(ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.GeodeticCurveType,ArcGIS.Core.Geometry.LinearUnit,System.Double@,System.Double@)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.GeodeticDistanceAndAzimuth(ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.GeodeticCurveType,ArcGIS.Core.Geometry.LinearUnit,System.Double@,System.Double@)
      #region Calculate geodetic distance, azimuth between two points

      SpatialReference sr = SpatialReferences.WGS84;

      MapPoint p1 = MapPointBuilderEx.CreateMapPoint(111, 55, sr);
      MapPoint p2 = MapPointBuilderEx.CreateMapPoint(95.5845, 64.2285, sr);

      double distance, az12, az21;

      distance = GeometryEngine.Instance.GeodeticDistanceAndAzimuth(p1, p2, GeodeticCurveType.Geodesic, out az12, out az21);
      // distance - 1339728.0303777338
      // az12 - 326.235780421405
      // az21 - 132.87425637913955

      distance = GeometryEngine.Instance.GeodeticDistanceAndAzimuth(p1, p2, GeodeticCurveType.Loxodrome, out az12, out az21);
      // distance - 1342745.9687172563
      // az12 - 319.966400332104
      // az21- 139.96640033210397

      distance = GeometryEngine.Instance.GeodeticDistanceAndAzimuth(p1, p2, GeodeticCurveType.GreatElliptic, out az12, out az21);
      // distance - 1339728.038837946
      // az12 - 326.22479262335418
      // az21 - 132.88558894347742



      MapPoint p3 = MapPointBuilderEx.CreateMapPoint(0, 0, sr);
      MapPoint p4 = MapPointBuilderEx.CreateMapPoint(1, 0, sr);

      distance = GeometryEngine.Instance.GeodeticDistanceAndAzimuth(p3, p4, GeodeticCurveType.Geodesic, out az12, out az21);
      // distance - 111319.49079327342
      // az12 - 90
      // az21 - 270

      distance = GeometryEngine.Instance.GeodeticDistanceAndAzimuth(p3, p4, GeodeticCurveType.Geodesic, LinearUnit.Miles, out az12, out az21);
      // distance - 69.1707247134693
      // az12 - 90
      // az21 - 270

      #endregion
    }

    public void GeodeticMove()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.GeodeticMove(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.MapPoint},ArcGIS.Core.Geometry.SpatialReference,System.Double,ArcGIS.Core.Geometry.LinearUnit,System.Double,ArcGIS.Core.Geometry.GeodeticCurveType)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.GeodeticMove(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.MapPoint},ArcGIS.Core.Geometry.SpatialReference,System.Double,ArcGIS.Core.Geometry.LinearUnit,System.Double,ArcGIS.Core.Geometry.GeodeticCurveType)
      #region Perform Geodetic Move on a set of MapPoints

      SpatialReference sr = SpatialReferences.WebMercator;
      var points = new[] { MapPointBuilderEx.CreateMapPoint(0, 0, sr) };
      double distance = 10;
      double azimuth = Math.PI / 2;
      var resultPoints = GeometryEngine.Instance.GeodeticMove(points, sr, distance, LinearUnit.Meters, azimuth, GeodeticCurveType.Geodesic);

      // resultPoints.First().X = 10
      // resultPoints.First().Y = 0
      // resultPoints.First().SpatialReference.Wkid = sr.Wkid

      // Change LinearUnit to Miles
      resultPoints = GeometryEngine.Instance.GeodeticMove(points, sr, distance, LinearUnit.Miles, azimuth, GeodeticCurveType.Geodesic);
      // resultPoints.First().X = 16093.44
      // resultPoints.First().Y = 0

      // Change curve type
      resultPoints = GeometryEngine.Instance.GeodeticMove(points, sr, distance, LinearUnit.Miles, azimuth, GeodeticCurveType.Loxodrome);
      // resultPoints.First().X = 16093.44
      // resultPoints.First().Y = 0
      #endregion
    }

    public void GetPredefinedCoordinateSystemList()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.GetPredefinedCoordinateSystemList(ArcGIS.Core.Geometry.CoordinateSystemFilter)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.GetPredefinedCoordinateSystemList(ArcGIS.Core.Geometry.CoordinateSystemFilter)
      // cref: ArcGIS.Core.Geometry.CoordinateSystemListEntry.Category
      // cref: ArcGIS.Core.Geometry.CoordinateSystemListEntry.Name
      // cref: ArcGIS.Core.Geometry.CoordinateSystemListEntry.Wkid
      #region Retrieve coordinate systems

      // get all the geographic coordinate systems
      IReadOnlyList<CoordinateSystemListEntry> gcs_list = GeometryEngine.Instance.GetPredefinedCoordinateSystemList(CoordinateSystemFilter.GeographicCoordinateSystem);

      // get the projected coordinate systems
      IReadOnlyList<CoordinateSystemListEntry> proj_list = GeometryEngine.Instance.GetPredefinedCoordinateSystemList(CoordinateSystemFilter.ProjectedCoordinateSystem);

      // get the vertical coordinate systems
      IReadOnlyList<CoordinateSystemListEntry> vert_list = GeometryEngine.Instance.GetPredefinedCoordinateSystemList(CoordinateSystemFilter.VerticalCoordinateSystem);

      // get geographic and projected coordinate systems
      IReadOnlyList<CoordinateSystemListEntry> combined_list = GeometryEngine.Instance.GetPredefinedCoordinateSystemList(CoordinateSystemFilter.GeographicCoordinateSystem | CoordinateSystemFilter.ProjectedCoordinateSystem);

      // investigate one of the entries
      CoordinateSystemListEntry entry = gcs_list[0];
      int wkid = entry.Wkid;
      string category = entry.Category;
      string name = entry.Name;
      #endregion
    }

    public void GetPredefinedGeographicTransformationList()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.GetPredefinedGeographicTransformationList
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.GetPredefinedGeographicTransformationList
      // cref: ArcGIS.Core.Geometry.GeographicTransformationListEntry.FromSRWkid
      // cref: ArcGIS.Core.Geometry.GeographicTransformationListEntry.Name
      // cref: ArcGIS.Core.Geometry.GeographicTransformationListEntry.ToSRWkid
      // cref: ArcGIS.Core.Geometry.GeographicTransformationListEntry.Wkid
      #region Retrieve system geographic transformations

      // a geographic transformation is the definition of how to project from one spatial reference to another
      IReadOnlyList<GeographicTransformationListEntry> list = GeometryEngine.Instance.GetPredefinedGeographicTransformationList();

      // a GeographicTransformationListEntry consists of Name, Wkid, the From SpatialReference Wkid, the To SpatialReference Wkid
      GeographicTransformationListEntry entry = list[0];

      int fromWkid = entry.FromSRWkid;
      int toWkid = entry.ToSRWkid;
      int wkid = entry.Wkid;
      string name = entry.Name;
      #endregion
    }

    public void GetSubCurve_GetSubCurve3D()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.GetSubCurve(ArcGIS.Core.Geometry.Multipart,System.Double,System.Double,ArcGIS.Core.Geometry.AsRatioOrLength)
      // cref: ArcGIS.Core.Geometry.GeometryEngine.GetSubCurve3D(ArcGIS.Core.Geometry.Multipart,System.Double,System.Double,ArcGIS.Core.Geometry.AsRatioOrLength)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.GetSubCurve(ArcGIS.Core.Geometry.Multipart,System.Double,System.Double,ArcGIS.Core.Geometry.AsRatioOrLength)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.GetSubCurve3D(ArcGIS.Core.Geometry.Multipart,System.Double,System.Double,ArcGIS.Core.Geometry.AsRatioOrLength)
      #region Get Sub-curve of a polyline or polygon

      SpatialReference sr = SpatialReferences.WGS84;

      List<Coordinate2D> coords = new List<Coordinate2D>()
      {
        new Coordinate2D(-111, 72),
        new Coordinate2D(-108, 68),
        new Coordinate2D(-114, 68)
      };

      Polyline polyline = PolylineBuilderEx.CreatePolyline(coords, sr);

      Polyline subCurve = GeometryEngine.Instance.GetSubCurve(polyline, 0, 5, AsRatioOrLength.AsLength);
      // subCurve.PartCount = 1
      // subCurve.PointCount = 2

      ReadOnlyPointCollection points = subCurve.Points;
      // points[0] = -111, 72
      // points[1] = -108, 68

      subCurve = GeometryEngine.Instance.GetSubCurve(polyline, 0, 0.5, AsRatioOrLength.AsRatio);
      // subCurve.PointCount = 3

      points = subCurve.Points;
      // points[0] = -111, 72
      // points[1] = -108, 68
      // points[2] = -108.5, 68


      List<Coordinate3D> coords3D = new List<Coordinate3D>()
      {
        new Coordinate3D(0, 0, 0),
        new Coordinate3D(0, 1, 1),
        new Coordinate3D(1, 1, 2),
        new Coordinate3D(1, 0, 3)
      };

      Polygon polygon = PolygonBuilderEx.CreatePolygon(coords3D, sr);

      subCurve = GeometryEngine.Instance.GetSubCurve3D(polygon, 0, 1, AsRatioOrLength.AsLength);
      // subCurve.HasZ = true

      points = subCurve.Points;
      // points.Count = 2
      // points[0] = 0, 0, 0
      // points[1] = 0, 0.70710678118654746, 0.70710678118654746

      #endregion
    }

    public void GraphicBuffer()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.GraphicBuffer(ArcGIS.Core.Geometry.Geometry,System.Double,ArcGIS.Core.Geometry.LineJoinType,ArcGIS.Core.Geometry.LineCapType,System.Double,System.Double,System.Int32)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.GraphicBuffer(ArcGIS.Core.Geometry.Geometry,System.Double,ArcGIS.Core.Geometry.LineJoinType,ArcGIS.Core.Geometry.LineCapType,System.Double,System.Double,System.Int32)
      #region GraphicBuffer

      // mitered join and butt caps
      SpatialReference sr = SpatialReferenceBuilder.CreateSpatialReference(102010);
      List<Coordinate2D> coords = new List<Coordinate2D>()
      {
          new Coordinate2D(1400,6200),
          new Coordinate2D(1600,6300),
          new Coordinate2D(1800,6200)
      };

      Polyline polyline = PolylineBuilderEx.CreatePolyline(coords, sr);

      Polygon polygon = GeometryEngine.Instance.GraphicBuffer(polyline, 50, LineJoinType.Miter, LineCapType.Butt, 10, 0, -1) as Polygon;

      // bevelled join and round caps
      Envelope envelope = EnvelopeBuilderEx.CreateEnvelope(0, 0, 10000, 10000, SpatialReferences.WebMercator);

      Polygon outPolygon = GeometryEngine.Instance.GraphicBuffer(envelope, 1000, LineJoinType.Bevel, LineCapType.Round, 4, 0, 96) as Polygon;
      #endregion

      // cref: ArcGIS.Core.Geometry.GeometryEngine.GraphicBuffer(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Geometry},System.Double,ArcGIS.Core.Geometry.LineJoinType,ArcGIS.Core.Geometry.LineCapType,System.Double,System.Double,System.Int32)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.GraphicBuffer(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Geometry},System.Double,ArcGIS.Core.Geometry.LineJoinType,ArcGIS.Core.Geometry.LineCapType,System.Double,System.Double,System.Int32)
      #region GraphicBuffer Many

      // round join and round caps
      MapPoint point1 = MapPointBuilderEx.CreateMapPoint(0, 0);
      MapPoint point2 = MapPointBuilderEx.CreateMapPoint(1, 1, SpatialReferences.WGS84);
      MapPoint point3 = MapPointBuilderEx.CreateMapPoint(1000000, 1200000, SpatialReferences.WebMercator);
      List<MapPoint> points = new List<MapPoint>() { point1, point2, point3 };

      IReadOnlyList<Geometry> geometries = GeometryEngine.Instance.GraphicBuffer(points, 5, LineJoinType.Round, LineCapType.Round, 0, 0, 3000);
      #endregion
    }

    public void Intersection()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Intersection(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.GeometryDimensionType)
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Intersection(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Intersects(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Intersection(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.GeometryDimensionType)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Intersection(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Intersects(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Geometry)
      #region Intersection between two Polylines

      // determine intersection between two polylines

      List<MapPoint> pts = new List<MapPoint>();
      pts.Add(MapPointBuilderEx.CreateMapPoint(1.0, 1.0));
      pts.Add(MapPointBuilderEx.CreateMapPoint(3.0, 3.0));
      pts.Add(MapPointBuilderEx.CreateMapPoint(5.0, 1.0));

      Polyline line1 = PolylineBuilderEx.CreatePolyline(pts);

      List<MapPoint> pts2 = new List<MapPoint>();
      pts2.Add(MapPointBuilderEx.CreateMapPoint(1.0, 3.0));
      pts2.Add(MapPointBuilderEx.CreateMapPoint(3.0, 1.0));
      pts2.Add(MapPointBuilderEx.CreateMapPoint(5.0, 3.0));

      Polyline line2 = PolylineBuilderEx.CreatePolyline(pts2);

      bool intersects = GeometryEngine.Instance.Intersects(line1, line2);    // intersects = true
      Geometry g = GeometryEngine.Instance.Intersection(line1, line2, GeometryDimensionType.EsriGeometry0Dimension);
      Multipoint resultMultipoint = g as Multipoint;

      // result is a multiPoint that intersects at (2,2) and (4,2)
      #endregion

      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.CreateEnvelope(ArcGIS.Core.Geometry.Coordinate2D,ArcGIS.Core.Geometry.Coordinate2D,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Intersection(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Intersection(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Geometry)
      #region Intersection between two Polygons

      // determine intersection between two polygons

      Envelope env1 = EnvelopeBuilderEx.CreateEnvelope(new Coordinate2D(3.0, 2.0), new Coordinate2D(6.0, 6.0));
      Polygon poly1 = PolygonBuilderEx.CreatePolygon(env1);

      Envelope env2 = EnvelopeBuilderEx.CreateEnvelope(new Coordinate2D(1.0, 1.0), new Coordinate2D(4.0, 4.0));
      Polygon poly2 = PolygonBuilderEx.CreatePolygon(env2);

      Polygon polyResult = GeometryEngine.Instance.Intersection(poly1, poly2) as Polygon;
      #endregion
    }

    public void LabelPoint()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.LabelPoint(ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.LabelPoint(ArcGIS.Core.Geometry.Geometry)
      #region Determine label point for a Polygon

      // create a polygon
      List<Coordinate2D> list2D = new List<Coordinate2D>();
      list2D.Add(new Coordinate2D(1.0, 1.0));
      list2D.Add(new Coordinate2D(1.0, 2.0));
      list2D.Add(new Coordinate2D(2.0, 2.0));
      list2D.Add(new Coordinate2D(2.0, 1.0));

      Polygon polygon = PolygonBuilderEx.CreatePolygon(list2D);
      bool isSimple = GeometryEngine.Instance.IsSimpleAsFeature(polygon);
      MapPoint pt = GeometryEngine.Instance.LabelPoint(polygon);
      #endregion
    }

    public void Length_Length3D()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Length(ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Length3D(ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Length(ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Length3D(ArcGIS.Core.Geometry.Geometry)
      #region Determine Length, Length3D of line

      MapPoint c1 = MapPointBuilderEx.CreateMapPoint(1, 2, 3);
      MapPoint c2 = MapPointBuilderEx.CreateMapPoint(4, 2, 4);

      // line segment
      LineSegment line = LineBuilderEx.CreateLineSegment(c1, c2);
      double len = line.Length;     // = 3
      double len3D = line.Length3D;   // = Math.Sqrt(10)

      // polyline
      Polyline p = PolylineBuilderEx.CreatePolyline(line);
      double p_len = p.Length;        // = len = 3
      double p_len3D = p.Length3D;    // = len3D = Math.Sqrt(10)

      double ge_len = GeometryEngine.Instance.Length(p);      // = p_len = len = 3
      double ge_len3d = GeometryEngine.Instance.Length3D(p);    // = p_len3D = len3D = Math.Sqrt(10)
      #endregion
    }

    public void GetMinMaxM()
    {
      // GetMinMaxM, GetMMonotonic, GetPointsAtM, GetSubCurveBetweenMs, GetNormalsAtM, SetMsAsDistance, SetAndInterpolateMsBetween

      // cref: ArcGIS.Core.Geometry.GeometryEngine.GetMinMaxM(ArcGIS.Core.Geometry.Multipart,System.Double@,System.Double@)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.GetMinMaxM(ArcGIS.Core.Geometry.Multipart,System.Double@,System.Double@)
      #region Get the minimum and maximum M values - GetMinMaxM

      string json = "{\"hasM\":true,\"rings\":[[[-3000,-2000,10],[-2000,-2000,15],[-1000,-2000,20],[0,-2000,0],[1000,-2000,-20],[2000,-2000,-30],[3000,-2000,10],[4000,-2000,5]]],\"spatialReference\":{\"wkid\":3857}}";
      Polygon polygon = PolygonBuilderEx.FromJson(json);

      double minM, maxM;
      GeometryEngine.Instance.GetMinMaxM(polygon, out minM, out maxM);
      // minM = -30 
      // maxM = 20

      json = "{\"hasM\":true,\"paths\":[[[-3000,-2000,10],[-2000,-2000,null],[-1000,-2000,null]]]}";
      Polyline polyline = PolylineBuilderEx.FromJson(json);

      GeometryEngine.Instance.GetMinMaxM(polyline, out minM, out maxM);
      // minM = 10
      // maxM = 10

      json = "{\"hasM\":true,\"paths\":[[[-3000,-2000,null],[-2000,-2000,null],[-1000,-2000,null]]]}";
      polyline = PolylineBuilderEx.FromJson(json);

      GeometryEngine.Instance.GetMinMaxM(polyline, out minM, out maxM);
      // minM = double.Nan
      // maxM = double.Nan
      #endregion
    }

    public void GetMMonotonic()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.GetMMonotonic(ArcGIS.Core.Geometry.Multipart)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.GetMMonotonic(ArcGIS.Core.Geometry.Multipart)
      #region Determine whether Ms are monotonic and whether ascending or descending - GetMMonotonic

      string json = "{\"hasM\":true,\"paths\":[[[-3000,-2000,10],[-2000,-2000,15],[-1000,-2000,20]]]}";
      Polyline polyline = PolylineBuilderEx.FromJson(json);

      MonotonicType monotonic = GeometryEngine.Instance.GetMMonotonic(polyline);
      // monotonic = Monotonic.Ascending

      json = "{\"hasM\":true,\"paths\":[[[-3000,-2000,10],[-2000,-2000,5],[-1000,-2000,0]]]}";
      polyline = PolylineBuilderEx.FromJson(json);

      monotonic = GeometryEngine.Instance.GetMMonotonic(polyline);
      // monotonic = Monotonic.Descending

      json = "{\"hasM\":true,\"paths\":[[[-3000,-2000,10],[-2000,-2000,15],[-1000,-2000,0]]]}";
      polyline = PolylineBuilderEx.FromJson(json);

      monotonic = GeometryEngine.Instance.GetMMonotonic(polyline);
      // monotonic = Monotonic.NotMonotonic

      #endregion
    }

    public void GetPointsAtM()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.GetPointsAtM(ArcGIS.Core.Geometry.Multipart,System.Double,System.Double)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.GetPointsAtM(ArcGIS.Core.Geometry.Multipart,System.Double,System.Double)
      #region Get a multipoint corresponding to the locations where the specified M values occur along the geometry - GetPointsAtM

      string json = "{\"hasM\":true,\"paths\":[[[-3000,-2000,10],[-2000,-2000,15],[-1000,-2000,20],[0,-2000,0],[1000,-2000,20],[2000,-2000,30],[3000,-2000,10],[4000,-2000,5]]],\"spatialReference\":{\"wkid\":3857}}";
      Polyline polyline = PolylineBuilderEx.FromJson(json);

      Multipoint multipoint = GeometryEngine.Instance.GetPointsAtM(polyline, 10, 500);
      // multiPoint.PointCount = 4
      // multipoint.Points[0]  X= -3000, Y= -2500  M= 10
      // multipoint.Points[1]  X= -500, Y= -2500  M= 10
      // multipoint.Points[2]  X= 500, Y= -2500  M= 10
      // multipoint.Points[3]  X= 3000, Y= -2500  M= 10
      #endregion
    }

    public void GetSubCurveBetweenMs()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.GetSubCurveBetweenMs(ArcGIS.Core.Geometry.Multipart,System.Double,System.Double)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.GetSubCurveBetweenMs(ArcGIS.Core.Geometry.Multipart,System.Double,System.Double)
      #region Get a polyline corresponding to the subcurves between specified M values - GetSubCurveBetweenMs

      string json = "{\"hasM\":true,\"paths\":[[[-2000,0,1],[-1000,1000,2],[-1000,0,3],[1000,1000,4],[2000,1000,5],[2000,2000,6],[3000,2000,7],[4000,0,8]]],\"spatialReference\":{\"wkid\":3857}}";
      Polyline polyline = PolylineBuilderEx.FromJson(json);

      Polyline subCurve = GeometryEngine.Instance.GetSubCurveBetweenMs(polyline, 2, 6);
      // subCurve.PointCount = 5
      // subCurve.Points[0]  X= --1000, Y= 1000 M= 2
      // subCurve.Points[1]  X= --1000, Y= 0  M= 3
      // subCurve.Points[2]  X= 1000, Y= 1000  M= 4
      // subCurve.Points[3]  X= 2000, Y= -1000  M= 5
      // subCurve.Points[4]  X= 2000, Y= -2000  M= 6

      subCurve = GeometryEngine.Instance.GetSubCurveBetweenMs(polyline, double.NaN, 5);
      // subCurve.PointCount = 0
      // subCurve.IsEmpty = true
      #endregion
    }

    public void GetNormalsAtM()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.GetNormalsAtM(ArcGIS.Core.Geometry.Multipart,System.Double,System.Double)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.GetNormalsAtM(ArcGIS.Core.Geometry.Multipart,System.Double,System.Double)
      #region Get line segments corresponding to the normal at the locations where the specified M values occur along the geometry - GetNormalsAtM

      IList<MapPoint> inPoints = new List<MapPoint>()
      {
        MapPointBuilderEx.CreateMapPoint(-3000, -2000, 0, 100),
        MapPointBuilderEx.CreateMapPoint(-3000, 0, 0, 200),
        MapPointBuilderEx.CreateMapPoint(-1000, 0, 0, 300),
        MapPointBuilderEx.CreateMapPoint(-1000, 2000, 0, 100),
        MapPointBuilderEx.CreateMapPoint(3000, 2000, 0, 200),
        MapPointBuilderEx.CreateMapPoint(3000, 0, 0, 300),
        MapPointBuilderEx.CreateMapPoint(1000, 0, 0, 100),
        MapPointBuilderEx.CreateMapPoint(1000, -2000, 0, 200),
        MapPointBuilderEx.CreateMapPoint(-3000, -2000, 0, 300)
      };

      Polygon polygon = PolygonBuilderEx.CreatePolygon(inPoints);
      // polygon.HasM = true

      Polyline polyline = GeometryEngine.Instance.GetNormalsAtM(polygon, 150, 100);
      // polyline.PartCount = 5
      // polyline.PointCount = 10
      // polyline.HasM = false

      ReadOnlyPartCollection parts = polyline.Parts;
      ReadOnlySegmentCollection segments = parts[0];
      LineSegment line = segments[0] as LineSegment;
      // line.StartCoordinate = (-3000, -1000)
      // line.EndCoordinate = (-2900, -1000)

      segments = parts[1];
      line = segments[0] as LineSegment;
      // line.StartCoordinate = (-1000, 1500)
      // line.EndCoordinate = (-900, 1500)

      segments = parts[2];
      line = segments[0] as LineSegment;
      // line.StartCoordinate = (1000, 2000)
      // line.EndCoordinate = (1000, 1900)

      segments = parts[3];
      line = segments[0] as LineSegment;
      // line.StartCoordinate = (1500, 0)
      // line.EndCoordinate = (1500, 100)

      segments = parts[4];
      line = segments[0] as LineSegment;
      // line.StartCoordinate = (1000, -1000)
      // line.EndCoordinate = (900, -1000)
      #endregion
    }

    public void GetMsAtDistance()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.GetMsAtDistance(ArcGIS.Core.Geometry.Multipart,System.Double,ArcGIS.Core.Geometry.AsRatioOrLength,System.Double@,System.Double@)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.GetMsAtDistance(ArcGIS.Core.Geometry.Multipart,System.Double,ArcGIS.Core.Geometry.AsRatioOrLength,System.Double@,System.Double@)
      #region Get the M values at the specified distance along the multipart

      string json = "{\"hasM\":true,\"paths\":[[[-3000,-2000,-3],[-2000,-2000,-2]],[[-2000,-2000,1],[-2000,1000,2]]],\"spatialReference\":{\"wkid\":3857}}";
      Polyline polyline = PolylineBuilderEx.FromJson(json);
      
      // polyline has 2 parts

      double m1, m2;
      GeometryEngine.Instance.GetMsAtDistance(polyline, 0, AsRatioOrLength.AsLength, out m1, out m2);
      // m1 = -3
      // m2 = NaN

      GeometryEngine.Instance.GetMsAtDistance(polyline, 500, AsRatioOrLength.AsLength, out m1, out m2);
      // m1 = -2.5
      // m2 = NaN

      GeometryEngine.Instance.GetMsAtDistance(polyline, 1000, AsRatioOrLength.AsLength, out m1, out m2);
      // m1 = -2
      // m2 = 1     // m2 has a value because distance 1000 is at the end of the first part, beginning of the second part

      #endregion
    }

    public void SetMsAsDistance()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.SetMsAsDistance(ArcGIS.Core.Geometry.Multipart,ArcGIS.Core.Geometry.AsRatioOrLength)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.SetMsAsDistance(ArcGIS.Core.Geometry.Multipart,ArcGIS.Core.Geometry.AsRatioOrLength)
      #region Set M values to the cumulative length from the start of the multipart - SetMsAsDistance

      string json = "{\"hasM\":true,\"rings\":[[[0,0],[0,3000],[4000,3000],[4000,0],[0,0]]],\"spatialReference\":{\"wkid\":3857}}";
      Polygon polygon = PolygonBuilderEx.FromJson(json);

      Polygon outPolygon = GeometryEngine.Instance.SetMsAsDistance(polygon, AsRatioOrLength.AsLength) as Polygon;
      ReadOnlyPointCollection outPoints = outPolygon.Points;
      // outPoints M values are { 0, 3000, 7000, 10000, 14000 };
      #endregion
    }

    public void InsertMAtDistance()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.InsertMAtDistance(ArcGIS.Core.Geometry.Multipart,System.Double,System.Double,ArcGIS.Core.Geometry.AsRatioOrLength,System.Boolean,System.Boolean@,System.Int32@,System.Int32@)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.InsertMAtDistance(ArcGIS.Core.Geometry.Multipart,System.Double,System.Double,ArcGIS.Core.Geometry.AsRatioOrLength,System.Boolean,System.Boolean@,System.Int32@,System.Int32@)
      #region Insert M value at the given distance - InsertMAtDistance

      string json = "{\"hasM\":true,\"paths\":[[[-3000,-2000,-3],[-2000,-2000,-2],[-1000,-2000,null]]]}";
      Polyline polyline = PolylineBuilderEx.FromJson(json);
      bool splitHappened;
      int partIndex, segmentIndex;

      // A point already exists at the given distance
      double m = -1;
      double distance = 2000;
      bool createNewPart = false;
      Polyline outputPolyline = GeometryEngine.Instance.InsertMAtDistance(polyline, m, distance, AsRatioOrLength.AsLength, createNewPart, out splitHappened, out partIndex, out segmentIndex) as Polyline;

      // splitHappened = false, partIndex = 0, segmentIndex = 2
      // outputPolyline.Points[2].M = -1

      json = "{\"hasM\":true,\"paths\":[[[-3000,-2000,-3],[-2000,-2000,-2],[-1000,-2000,-1]],[[0,0,0],[0,1000,0],[0,2000,2]]],\"spatialReference\":{\"wkid\":3857}}";
      polyline = PolylineBuilderEx.FromJson(json);

      // A point already exists at the given distance, but createNewPart = true
      m = 1;
      distance = 3000;
      createNewPart = true;
      outputPolyline = GeometryEngine.Instance.InsertMAtDistance(polyline, m, distance, AsRatioOrLength.AsLength, createNewPart, out splitHappened, out partIndex, out segmentIndex) as Polyline;
      string outputJson = outputPolyline.ToJson();

      // splitHappened = true, partIndex = 2, segmentIndex = 0
      // outputJson = {"hasM":true,"paths":[[[-3000,-2000,-3],[-2000,-2000,-2],[-1000,-2000,-1]],[[0,0,0],[0,1000,1]],[[0,1000,1],[0,2000,2]]]}}
      // A new part has been created and the M values for outputPolyline.Points[4] and outputPolyline.Points[5] have been modified

      // A point does not exist at the given distance
      m = 1;
      distance = 3500;
      createNewPart = false;
      outputPolyline = GeometryEngine.Instance.InsertMAtDistance(polyline, m, distance, AsRatioOrLength.AsLength, createNewPart, out splitHappened, out partIndex, out segmentIndex) as Polyline;
      outputJson = outputPolyline.ToJson();

      // splitHappened = true even though createNewPart = false because a new point was created
      // partIndex = 1, segmentIndex = 2
      // outputJson = {"hasM":true,"paths":[[[-3000,-2000,-3],[-2000,-2000,-2],[-1000,-2000,-1]],[[0,0,0],[0,1000,0],[0,1500,1],[0,2000,2]]]}
      // A new point has been inserted (0, 1500, 1) by interpolating the X and Y coordinates and M value set to the input M value.

      #endregion
    }

    public void CalibrateByMs()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.CalibrateByMs(ArcGIS.Core.Geometry.Multipart,System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.MapPoint},ArcGIS.Core.Geometry.UpdateMMethod,System.Double)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.CalibrateByMs(ArcGIS.Core.Geometry.Multipart,System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.MapPoint},ArcGIS.Core.Geometry.UpdateMMethod,System.Double)
      #region Calibrate M values using M values from input points - CalibrateByMs

      string json = "{\"hasM\":true,\"paths\":[[[0,0,-1],[1,0,0],[1,1,1],[1,2,2],[3,1,3],[5,3,4],[9,5,5],[7,6,6]]],\"spatialReference\":{\"wkid\":4326}}";
      Polyline polyline = PolylineBuilderEx.FromJson(json);

      // Interpolate using points (0, 0, 17), (1, 0, 42), (7, 6, 18) 
      List<MapPoint> updatePoints = new List<MapPoint>(3);
      MapPointBuilderEx builder = new MapPointBuilderEx(0, 0);
      builder.M = 17;
      updatePoints.Add(builder.ToGeometry() as MapPoint);

      builder.X = 1;
      builder.M = 42;
      updatePoints.Add(builder.ToGeometry() as MapPoint);

      builder.X = 7;
      builder.Y = 6;
      builder.M = 18;
      updatePoints.Add(builder.ToGeometry() as MapPoint);

      // Calibrate all the points in the polyline
      double cutOffDistance = polyline.Length;

      Polyline updatedPolyline = GeometryEngine.Instance.CalibrateByMs(polyline, updatePoints, UpdateMMethod.Interpolate, cutOffDistance) as Polyline;
      // The points in the updated polyline are
      // (0, 0, 17 ), ( 1, 0, 42 ), ( 1, 1, 38 ), ( 1, 2, 34 ), ( 3, 1, 30 ), ( 5, 3, 26 ), ( 9, 5, 22 ), ( 7, 6, 18 )

      // ExtrapolateBefore using points (1, 2, 42), (9, 5, 18)
      builder.X = 1;
      builder.Y = 2;
      builder.M = 42;
      updatePoints[0] = builder.ToGeometry() as MapPoint;

      builder.X = 9;
      builder.Y = 5;
      builder.M = 18;
      updatePoints[1] = builder.ToGeometry() as MapPoint;

      updatePoints.RemoveAt(2);

      updatedPolyline = GeometryEngine.Instance.CalibrateByMs(polyline, updatePoints, UpdateMMethod.ExtrapolateBefore, cutOffDistance) as Polyline;
      // The points in the updated polyline are
      // ( 0, 0, 66 ), ( 1, 0, 58 ), ( 1, 1, 50 ), ( 1, 2, 42 ), ( 3, 1, 3 ), ( 5, 3, 4 ), ( 9, 5, 18 ), ( 7, 6, 6 )

      // ExtrapolateAfter using points (0, 0, 17), (1, 2, 42)
      builder.X = 0;
      builder.Y = 0;
      builder.M = 17;
      updatePoints.Insert(0, builder.ToGeometry() as MapPoint);

      updatePoints.RemoveAt(2);

      updatedPolyline = GeometryEngine.Instance.CalibrateByMs(polyline, updatePoints, UpdateMMethod.ExtrapolateAfter, cutOffDistance) as Polyline;
      // The points in the updated polyline are
      // ( 0, 0, 17 ), ( 1, 0, 0 ), ( 1, 1, 1 ), ( 1, 2, 42 ), ( 3, 1, 50.333333333333333 ), ( 5, 3, 58.666666666666671 ), ( 9, 5, 67 ), ( 7, 6, 75.333333333333343 )

      // ExtrapolateAfter and Interpolate using points (0, 0, 17), (1, 2, 42)
      updatedPolyline = GeometryEngine.Instance.CalibrateByMs(polyline, updatePoints, UpdateMMethod.ExtrapolateAfter | UpdateMMethod.Interpolate, cutOffDistance) as Polyline;
      // The points in the updated polyline are
      // (0,0,17),(1,0,25.333333333333336),(1,1,33.666666666666671),(1,2,42),(3,1,50.333333333333336),(5,3,58.666666666666671),(9,5,67),(7,6,75.333333333333343)

      #endregion
    }

    public void InterpolateMsBetween()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.InterpolateMsBetween(ArcGIS.Core.Geometry.Multipart,System.Int32,System.Int32,System.Int32,System.Int32)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.InterpolateMsBetween(ArcGIS.Core.Geometry.Multipart,System.Int32,System.Int32,System.Int32,System.Int32)
      #region Generates M values by linear interpolation over a range of points - InterpolateMsBetween

      string json = "{\"hasM\":true,\"paths\":[[[0,0,-1],[1,0,0],[1,1,1],[1,2,2],[3,1,3],[5,3,4],[9,5,5],[7,6,6]]],\"spatialReference\":{\"wkid\":4326}}";
      Polyline polyline = PolylineBuilderEx.FromJson(json);

      // Interpolate between points 2 and 6
      Polyline outPolyline = GeometryEngine.Instance.InterpolateMsBetween(polyline, 0, 2, 0, 6) as Polyline;
      // The points of the output polyline are
      // (0, 0, -1), (1, 0, 0), (1, 1, 1), (1, 2, 1.3796279833912741), (3, 1, 2.2285019604153242), (5, 3, 3.3022520459518998), (9, 5, 5), (7, 6, 6)

      #endregion
    }

    public void SetAndInterpolateMsBetween()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.SetAndInterpolateMsBetween(ArcGIS.Core.Geometry.Multipart,System.Double,System.Double)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.SetAndInterpolateMsBetween(ArcGIS.Core.Geometry.Multipart,System.Double,System.Double)
      #region Set Ms at the beginning and end of the geometry and interpolate M values between the two values - SetAndInterpolateMsBetween

      string json = "{\"hasM\":true,\"paths\":[[[-3000,-2000],[-2000,-2000],[-1000,-2000],[0,-2000],[1000,-2000],[2000,-2000],[3000,-2000],[4000,-2000]]],\"spatialReference\":{\"wkid\":3857}}";
      Polyline polyline = PolylineBuilderEx.FromJson(json);

      Polyline outPolyline = GeometryEngine.Instance.SetAndInterpolateMsBetween(polyline, 100, 800) as Polyline;
      ReadOnlyPointCollection outPoints = outPolyline.Points;
      // outPoints M values are { 100, 200, 300, 400, 500, 600, 700, 800 };
      #endregion
    }

    public void Move()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Move (ArcGIS.Core.Geometry.Geometry,System.Double,System.Double)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Move (ArcGIS.Core.Geometry.Geometry,System.Double,System.Double)
      #region Move a MapPoint

      MapPoint pt = MapPointBuilderEx.CreateMapPoint(1.0, 3.0);
      MapPoint ptResult = GeometryEngine.Instance.Move(pt, -3.5, 2.5) as MapPoint;
      // ptResult is (-2.5, 5.5)
      #endregion

      // cref: ArcGIS.Core.Geometry.GeometryEngine.Move(ArcGIS.Core.Geometry.Geometry,System.Double,System.Double,System.Double)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Move(ArcGIS.Core.Geometry.Geometry,System.Double,System.Double,System.Double)
      #region Move a z-aware MapPoint

      MapPoint zPt = MapPointBuilderEx.CreateMapPoint(1.0, 3.0, 2.0);
      MapPoint zPtResult = GeometryEngine.Instance.Move(zPt, 4, 0.25, 0.5) as MapPoint;
      // zPtResult is (5.0, 3.25, 2.5);
      #endregion

      // cref: ArcGIS.Core.Geometry.GeometryEngine.Move(ArcGIS.Core.Geometry.Geometry,System.Double,System.Double)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Move(ArcGIS.Core.Geometry.Geometry,System.Double,System.Double)
      #region Move a Polyline
      List<MapPoint> pts = new List<MapPoint>();
      pts.Add(MapPointBuilderEx.CreateMapPoint(1.0, 1.0, 3.0));
      pts.Add(MapPointBuilderEx.CreateMapPoint(3.0, 3.0, 3.0));
      pts.Add(MapPointBuilderEx.CreateMapPoint(3, 2, 3.0));
      pts.Add(MapPointBuilderEx.CreateMapPoint(4.0, 2.0, 3.0));

      Polyline polyline = PolylineBuilderEx.CreatePolyline(pts);

      Geometry geometry = GeometryEngine.Instance.Move(polyline, 3, 2);
      Polyline polylineResult = geometry as Polyline;
      // polylineResult.Points[0] = 4.0, 3.0, 3.0
      // polylineResult.Points[1] = 6.0, 5.0, 3.0
      // polylineResult.Points[2] = 6.0, 4.0, 3.0
      // polylineResult.Points[3] = 7.0, 4.0, 3.0
      #endregion
    }

    public void MovePointAlongLine()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.MovePointAlongLine(ArcGIS.Core.Geometry.Multipart,System.Double,System.Boolean,System.Double,ArcGIS.Core.Geometry.SegmentExtensionType)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.MovePointAlongLine(ArcGIS.Core.Geometry.Multipart,System.Double,System.Boolean,System.Double,ArcGIS.Core.Geometry.SegmentExtensionType)
      #region MovePointAlongLine

      LineSegment line = LineBuilderEx.CreateLineSegment(MapPointBuilderEx.CreateMapPoint(0, 3), MapPointBuilderEx.CreateMapPoint(5.0, 3.0));
      Polyline polyline = PolylineBuilderEx.CreatePolyline(line);
      bool simple = GeometryEngine.Instance.IsSimpleAsFeature(polyline);

      // ratio = false
      MapPoint pt = GeometryEngine.Instance.MovePointAlongLine(polyline, 1.0, false, 0.0, SegmentExtensionType.NoExtension);
      // pt = 1.0, 3.0

      pt = GeometryEngine.Instance.MovePointAlongLine(polyline, 1.0, false, -1.0, SegmentExtensionType.NoExtension);
      // pt = 1.0, 4.0

      pt = GeometryEngine.Instance.MovePointAlongLine(polyline, 1.0, false, 2.0, SegmentExtensionType.NoExtension);
      // pt = 1.0, 1.0

      // ratio = true
      pt = GeometryEngine.Instance.MovePointAlongLine(polyline, 0.5, true, 0, SegmentExtensionType.NoExtension);
      // pt = 2.5, 3.0

      // move past the line
      pt = GeometryEngine.Instance.MovePointAlongLine(polyline, 7, false, 0, SegmentExtensionType.NoExtension);
      // pt = 5.0, 3.0

      // move past the line with extension at "to" point
      pt = GeometryEngine.Instance.MovePointAlongLine(polyline, 7, false, 0, SegmentExtensionType.ExtendEmbeddedAtTo);
      // pt = 7.0, 3.0

      // negative distance with extension at "from" point
      pt = GeometryEngine.Instance.MovePointAlongLine(polyline, -2, false, 0, SegmentExtensionType.ExtendEmbeddedAtFrom);
      // pt = -2.0, 3.0

      // ratio = true
      pt = GeometryEngine.Instance.MovePointAlongLine(polyline, 0.5, true, 0, SegmentExtensionType.NoExtension);
      // pt = 2.5, 3.0

      // line with Z
      List<Coordinate3D> coords3D = new List<Coordinate3D> { new Coordinate3D(0, 0, 0), new Coordinate3D(1113195, 1118890, 5000) };
      Polyline polylineZ = PolylineBuilderEx.CreatePolyline(coords3D, SpatialReferences.WebMercator);
      // polylineZ.HasZ = true

      // ratio = true, no offset
      pt = GeometryEngine.Instance.MovePointAlongLine(polylineZ, 0.5, true, 0, SegmentExtensionType.NoExtension);
      // pt.X = 556597.5
      // pt.Y = 559445
      // pt.Z = 2500

      // ratio = true, past the line with "to" extension, no offset
      pt = GeometryEngine.Instance.MovePointAlongLine(polylineZ, 1.5, true, 0, SegmentExtensionType.ExtendEmbeddedAtTo);
      // pt.X = 1669792.5
      // pt.Y = 1678335
      // pt.Z = 7500

      // ratio = true, negative distance past the line with no extension, no offset
      pt = GeometryEngine.Instance.MovePointAlongLine(polylineZ, -1.5, true, 0, SegmentExtensionType.NoExtension);
      // pt.X = 0
      // pt.Y = 0
      // pt.Z = -7500

      // polyline with Z but 2d distance = 0
      MapPoint pt3 = MapPointBuilderEx.CreateMapPoint(5, 5, 0);
      MapPoint pt4 = MapPointBuilderEx.CreateMapPoint(5, 5, 10);
      List<MapPoint> pts = new List<MapPoint>() { pt3, pt4 };

      polyline = PolylineBuilderEx.CreatePolyline(pts);
      // polyline.HasZ = true
      // polyline.Length3D = 10
      // polyline.Length = 0

      MapPoint result = GeometryEngine.Instance.MovePointAlongLine(polyline, 2, false, 0, SegmentExtensionType.NoExtension);
      // result = 5, 5, 2

      // polyline with length2d = 0 and length3d = 0
      MapPoint pt5 = MapPointBuilderEx.CreateMapPoint(5, 5, 10);
      MapPoint pt6 = MapPointBuilderEx.CreateMapPoint(5, 5, 10);
      pts.Clear();
      pts.Add(pt5);
      pts.Add(pt6);

      polyline = PolylineBuilderEx.CreatePolyline(pts);
      // polyline.HasZ = true
      // polyline.Length3D = 0
      // polyline.Length = 0

      result = GeometryEngine.Instance.MovePointAlongLine(polyline, 3, true, 0, SegmentExtensionType.NoExtension);
      // result = 5, 5, 10

      result = GeometryEngine.Instance.MovePointAlongLine(polyline, 3, true, 0, SegmentExtensionType.ExtendEmbeddedAtFrom);
      // result = 5, 5, 10

      // polyline with Z and M
      List<MapPoint> inputPoints = new List<MapPoint>()
      {
          MapPointBuilderEx.CreateMapPoint(1, 2, 3, 4),
          MapPointBuilderEx.CreateMapPoint(1, 2, 33, 44),
      };

      Polyline polylineZM = PolylineBuilderEx.CreatePolyline(inputPoints, SpatialReferences.WGS84);
      // polylineZM.HasZ = true
      // polylineZM.HasM = true

      // ratio = true, no offset
      MapPoint pointAlong = GeometryEngine.Instance.MovePointAlongLine(polylineZM, 0.5, true, 0, SegmentExtensionType.NoExtension);
      // pointAlong = 1, 2, 18, 24

      // ratio = true with offset
      pointAlong = GeometryEngine.Instance.MovePointAlongLine(polylineZM, 0.2, true, 2.23606797749979, SegmentExtensionType.NoExtension);
      // pointAlong = 1, 2, 9, 12
      #endregion
    }

    public void MultipartToSinglePart()
    {
      Polygon multipartPolygon = null;

      // cref: ArcGIS.Core.Geometry.GeometryEngine.MultipartToSinglePart(ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.MultipartToSinglePart(ArcGIS.Core.Geometry.Geometry)
      #region Separate components of a geometry into single component geometries

      List<Coordinate2D> coords2D = new List<Coordinate2D>()
        {
          new Coordinate2D(0, 0),
          new Coordinate2D(1, 4),
          new Coordinate2D(2, 7),
          new Coordinate2D(-10, 3)
        };

      Multipoint multipoint = MultipointBuilderEx.CreateMultipoint(coords2D, SpatialReferences.WGS84);

      IReadOnlyList<Geometry> result = GeometryEngine.Instance.MultipartToSinglePart(multipoint);
      // result.Count = 4, 


      // 'explode' a multipart polygon
      result = GeometryEngine.Instance.MultipartToSinglePart(multipartPolygon);


      // create a bag of geometries
      Polygon polygon = PolygonBuilderEx.CreatePolygon(coords2D, SpatialReferences.WGS84);
      //At 2.x - GeometryBag bag = GeometryBagBuilder.CreateGeometryBag(new List<Geometry>() { multipoint, polygon });
      var bag = GeometryBagBuilderEx.CreateGeometryBag(new List<Geometry>() { multipoint, polygon });
      // bag.PartCount = =2

      result = GeometryEngine.Instance.MultipartToSinglePart(bag);
      // result.Count == 2
      // result[0] is MultiPoint
      // result[1] is Polygon

      #endregion
    }

    public void NearestPoint_NearestVertex()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.NearestPoint(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.MapPoint)
      // cref: ArcGIS.Core.Geometry.GeometryEngine.NearestVertex(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.MapPoint)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.NearestPoint(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.MapPoint)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.NearestVertex(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.MapPoint)
      #region Nearest Point versus Nearest Vertex

      SpatialReference sr = SpatialReferences.WGS84;
      MapPoint pt = MapPointBuilderEx.CreateMapPoint(5, 5, sr);

      List<Coordinate2D> coords = new List<Coordinate2D>()
      {
        new Coordinate2D(10, 1),
        new Coordinate2D(10, -4),
        new Coordinate2D(0, -4),
        new Coordinate2D(0, 1),
        new Coordinate2D(10, 1)
      };

      Polygon polygon = PolygonBuilderEx.CreatePolygon(coords);

      // find the nearest point in the polygon geomtry to the pt
      ProximityResult result = GeometryEngine.Instance.NearestPoint(polygon, pt);
      // result.Point = 5, 1
      // result.SegmentIndex = 3
      // result.PartIndex = 0
      // result.PointIndex = null
      //result.Distance = 4
      //result.RightSide = false

      // find the nearest vertex in the polgyon geometry to the pt
      result = GeometryEngine.Instance.NearestVertex(polygon, pt);
      // result.Point = 10, 1
      // result.PointIndex = 0
      // result.SegmentIndex = null
      // result.PartIndex = 0
      // result.Distance = Math.Sqrt(41)
      // result.RightSide = false
      #endregion
    }

    public void NearestPoint3D()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.NearestPoint3D(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.MapPoint)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.NearestPoint3D(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.MapPoint)
      #region Determine Nearest Point in 3D

      MapPoint pt1 = MapPointBuilderEx.CreateMapPoint(1, 1, 1);
      MapPoint pt2 = MapPointBuilderEx.CreateMapPoint(2, 2, 2);
      MapPoint pt3 = MapPointBuilderEx.CreateMapPoint(10, 2, 1);

      //
      // test pt1 to pt2
      //
      ProximityResult result = GeometryEngine.Instance.NearestPoint3D(pt1, pt2);
      // result.Point = 1,1,1
      // result.Distance = Math.Sqrt(3)
      // result.SegmentIndex = null
      // result.PartIndex = 0
      // result.PointIndex = 0
      // result.RightSide = false

      // 
      // multipoint built from pt1, pt2.   should be closer to pt2
      // 
      Multipoint multipoint = MultipointBuilderEx.CreateMultipoint(new List<MapPoint>() { pt1, pt2 });
      result = GeometryEngine.Instance.NearestPoint3D(multipoint, pt3);
      // result.Point = 2, 2, 2
      // result.Distance = Math.Sqrt(65)
      // result.SegmentIndex = null
      // result.PartIndex = 1
      // result.PointIndex = 1
      // result.RightSide = false
      #endregion
    }

    public void Offset()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Offset(ArcGIS.Core.Geometry.Geometry,System.Double,ArcGIS.Core.Geometry.OffsetType,System.Double)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Offset(ArcGIS.Core.Geometry.Geometry,System.Double,ArcGIS.Core.Geometry.OffsetType,System.Double)
      #region Calculate a geometry offset from the source

      List<MapPoint> linePts = new List<MapPoint>();
      linePts.Add(MapPointBuilderEx.CreateMapPoint(1.0, 1.0, SpatialReferences.WGS84));
      linePts.Add(MapPointBuilderEx.CreateMapPoint(10.0, 1.0, SpatialReferences.WGS84));

      Polyline polyline = PolylineBuilderEx.CreatePolyline(linePts);

      Geometry g = GeometryEngine.Instance.Offset(polyline, 10, OffsetType.Square, 0);
      Polyline gResult = g as Polyline;
      // gResult.PointCount = 2
      // gResult.Points[0] = (1, -9)
      // gResult.Points[1] = (10, -9)

      g = GeometryEngine.Instance.Offset(polyline, -10, OffsetType.Round, 0.5);
      gResult = g as Polyline;
      // gResult.PointCount = 2
      // gResult.Points[0] = (1, -11
      // gResult.Points[1] = (10, 11)


      //
      // elliptic arc curve
      //
      Coordinate2D fromPt = new Coordinate2D(2, 1);
      Coordinate2D toPt = new Coordinate2D(1, 2);
      Coordinate2D interiorPt = new Coordinate2D(1 + Math.Sqrt(2) / 2, 1 + Math.Sqrt(2) / 2);

      EllipticArcSegment circularArc = EllipticArcBuilderEx.CreateCircularArc(fromPt.ToMapPoint(), toPt.ToMapPoint(), interiorPt);

      polyline = PolylineBuilderEx.CreatePolyline(circularArc);
      g = GeometryEngine.Instance.Offset(polyline, -0.25, OffsetType.Miter, 0.5);
      gResult = g as Polyline;

      g = GeometryEngine.Instance.Offset(polyline, 0.25, OffsetType.Bevel, 0.5);
      gResult = g as Polyline;


      //
      //  offset for a polygon
      //
      List<MapPoint> list = new List<MapPoint>();
      list.Add(MapPointBuilderEx.CreateMapPoint(10.0, 10.0, SpatialReferences.WGS84));
      list.Add(MapPointBuilderEx.CreateMapPoint(10.0, 20.0, SpatialReferences.WGS84));
      list.Add(MapPointBuilderEx.CreateMapPoint(20.0, 20.0, SpatialReferences.WGS84));
      list.Add(MapPointBuilderEx.CreateMapPoint(20.0, 10.0, SpatialReferences.WGS84));

      Polygon polygon = PolygonBuilderEx.CreatePolygon(list);

      g = GeometryEngine.Instance.Offset(polygon, 2, OffsetType.Square, 0);
      Polygon gPolygon = g as Polygon;

      g = GeometryEngine.Instance.Offset(polygon, -2, OffsetType.Round, 0.3);
      gPolygon = g as Polygon;

      g = GeometryEngine.Instance.Offset(polygon, -0.5, OffsetType.Miter, 0.6);
      gPolygon = g as Polygon;

      #endregion
    }

    public void Overlaps()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Overlaps(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Overlaps(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Geometry)
      #region Determine if geometries overlap

      MapPoint pt1 = MapPointBuilderEx.CreateMapPoint(1.5, 1.5);
      MapPoint pt2 = MapPointBuilderEx.CreateMapPoint(1.25, 1.75);
      MapPoint pt3 = MapPointBuilderEx.CreateMapPoint(3, 1.5);
      MapPoint pt4 = MapPointBuilderEx.CreateMapPoint(1.5, 2);

      //
      // point and point overlap
      //
      bool overlaps = GeometryEngine.Instance.Overlaps(pt1, pt2);        // overlaps = false
      overlaps = GeometryEngine.Instance.Overlaps(pt1, pt1);             // overlaps = false
                                                                         // Two geometries overlap if the region of their intersection is of the same dimension as the geometries involved and 
                                                                         // is not equivalent to either of the geometries.  

      List<MapPoint> pts = new List<MapPoint>();
      pts.Add(pt1);
      pts.Add(pt2);
      pts.Add(pt3);

      List<MapPoint> pts2 = new List<MapPoint>();
      pts2.Add(pt2);
      pts2.Add(pt3);
      pts2.Add(pt4);

      //
      // pt and line overlap
      //
      Polyline polyline = PolylineBuilderEx.CreatePolyline(pts);
      bool isSimple = GeometryEngine.Instance.IsSimpleAsFeature(polyline);         // isSimple = true
      overlaps = GeometryEngine.Instance.Overlaps(polyline, pt1);                  // overlaps = false

      //
      // line and line
      //
      Polyline polyline2 = PolylineBuilderEx.CreatePolyline(pts2);
      isSimple = GeometryEngine.Instance.IsSimpleAsFeature(polyline2);             // isSimple = true
      overlaps = GeometryEngine.Instance.Overlaps(polyline, polyline2);            // overlaps = true

      #endregion
    }

    public void Project()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Project(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Project(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.SpatialReferences.WebMercator
      #region Project from WGS84 to WebMercator

      MapPoint pt = MapPointBuilderEx.CreateMapPoint(1.0, 3.0, SpatialReferences.WGS84);
      Geometry result = GeometryEngine.Instance.Project(pt, SpatialReferences.WebMercator);
      MapPoint projectedPt = result as MapPoint;
      #endregion

      // cref: ArcGIS.Core.Geometry.GeometryEngine.IsSimpleAsFeature(ArcGIS.Core.Geometry.Geometry,System.Boolean)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.IsSimpleAsFeature(ArcGIS.Core.Geometry.Geometry,System.Boolean)
      // cref: ArcGIS.Core.Geometry.SpatialReferenceBuilder.CreateSpatialReference(System.Int32)
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Project(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Project(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.SpatialReference)
      #region Project from WGS84

      // create the polygon
      List<MapPoint> pts = new List<MapPoint>();
      pts.Add(MapPointBuilderEx.CreateMapPoint(1.0, 1.0, SpatialReferences.WGS84));
      pts.Add(MapPointBuilderEx.CreateMapPoint(1.0, 2.0, SpatialReferences.WGS84));
      pts.Add(MapPointBuilderEx.CreateMapPoint(2.0, 2.0, SpatialReferences.WGS84));
      pts.Add(MapPointBuilderEx.CreateMapPoint(2.0, 1.0, SpatialReferences.WGS84));

      Polygon polygon = PolygonBuilderEx.CreatePolygon(pts);
      // ensure it is simple
      bool isSimple = GeometryEngine.Instance.IsSimpleAsFeature(polygon);

      // create the spatial reference to project to
      SpatialReference northPole = SpatialReferenceBuilder.CreateSpatialReference(102018);   // north pole stereographic 

      // project
      Geometry geometry = GeometryEngine.Instance.Project(polygon, northPole);
      #endregion
    }

    public void QueryNormal()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.QueryNormal(ArcGIS.Core.Geometry.Multipart,ArcGIS.Core.Geometry.SegmentExtensionType,System.Double,ArcGIS.Core.Geometry.AsRatioOrLength,System.Double)
      // cref: ArcGIS.Core.Geometry.GeometryEngine.QueryNormal(ArcGIS.Core.Geometry.Segment,ArcGIS.Core.Geometry.SegmentExtensionType,System.Double,ArcGIS.Core.Geometry.AsRatioOrLength,System.Double)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.QueryNormal(ArcGIS.Core.Geometry.Multipart,ArcGIS.Core.Geometry.SegmentExtensionType,System.Double,ArcGIS.Core.Geometry.AsRatioOrLength,System.Double)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.QueryNormal(ArcGIS.Core.Geometry.Segment,ArcGIS.Core.Geometry.SegmentExtensionType,System.Double,ArcGIS.Core.Geometry.AsRatioOrLength,System.Double)
      #region QueryNormal

      string json = "{\"curvePaths\":[[[-13046586.8335,4036570.6796000004]," +
                    "{\"c\":[[-13046645.107099999,4037152.5873000026]," +
                    "[-13046132.776277589,4036932.1325614937]]}]],\"spatialReference\":{\"wkid\":3857}}";
      Polyline polyline = PolylineBuilderEx.FromJson(json);

      EllipticArcSegment arc = polyline.Parts[0][0] as EllipticArcSegment;

      // No extension, distanceAlongCurve = 0.5

      // use the polyline
      Polyline poly_normal = GeometryEngine.Instance.QueryNormal(polyline, SegmentExtensionType.NoExtension, 0.5, AsRatioOrLength.AsRatio, 1000);
      // or a segment
      LineSegment seg_normal = GeometryEngine.Instance.QueryNormal(arc, SegmentExtensionType.NoExtension, 0.5, AsRatioOrLength.AsRatio, 1000);

      // TangentAtFrom, distanceAlongCurve = -1.2
      poly_normal = GeometryEngine.Instance.QueryNormal(polyline, SegmentExtensionType.ExtendTangentAtFrom, -1.2, AsRatioOrLength.AsRatio, 1000);
      seg_normal = GeometryEngine.Instance.QueryNormal(arc, SegmentExtensionType.ExtendTangentAtFrom, -1.2, AsRatioOrLength.AsRatio, 1000);

      // TangentAtTo (ignored because distanceAlongCurve < 0), distanceAlongCurve = -1.2
      poly_normal = GeometryEngine.Instance.QueryNormal(polyline, SegmentExtensionType.ExtendTangentAtTo, -1.2, AsRatioOrLength.AsRatio, 1000);
      seg_normal = GeometryEngine.Instance.QueryNormal(arc, SegmentExtensionType.ExtendTangentAtTo, -1.2, AsRatioOrLength.AsRatio, 1000);

      // TangentAtTo, distanceAlongCurve = 1.2
      poly_normal = GeometryEngine.Instance.QueryNormal(polyline, SegmentExtensionType.ExtendTangentAtTo, 1.2, AsRatioOrLength.AsRatio, 1000);
      seg_normal = GeometryEngine.Instance.QueryNormal(arc, SegmentExtensionType.ExtendTangentAtTo, 1.2, AsRatioOrLength.AsRatio, 1000);

      // TangentAtFrom (ignored because distanceAlongCurve > 0), distanceAlongCurve = 1.2
      poly_normal = GeometryEngine.Instance.QueryNormal(polyline, SegmentExtensionType.ExtendTangentAtFrom, 1.2, AsRatioOrLength.AsRatio, 1000);
      seg_normal = GeometryEngine.Instance.QueryNormal(arc, SegmentExtensionType.ExtendTangentAtFrom, 1.2, AsRatioOrLength.AsRatio, 1000);

      // EmbeddedAtTo, distanceAlongCurve = 1.2
      poly_normal = GeometryEngine.Instance.QueryNormal(polyline, SegmentExtensionType.ExtendEmbeddedAtTo, 1.2, AsRatioOrLength.AsRatio, 1000);
      seg_normal = GeometryEngine.Instance.QueryNormal(arc, SegmentExtensionType.ExtendEmbeddedAtTo, 1.2, AsRatioOrLength.AsRatio, 1000);

      // EmbeddedAtFrom, distanceAlongCurve = -0.2
      poly_normal = GeometryEngine.Instance.QueryNormal(polyline, SegmentExtensionType.ExtendEmbeddedAtFrom, -0.2, AsRatioOrLength.AsRatio, 1000);
      seg_normal = GeometryEngine.Instance.QueryNormal(arc, SegmentExtensionType.ExtendEmbeddedAtFrom, -0.2, AsRatioOrLength.AsRatio, 1000);
      #endregion
    }

    public void QueryPoint()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.QueryPoint(ArcGIS.Core.Geometry.Multipart,ArcGIS.Core.Geometry.SegmentExtensionType,System.Double,ArcGIS.Core.Geometry.AsRatioOrLength)
      // cref: ArcGIS.Core.Geometry.GeometryEngine.QueryPoint(ArcGIS.Core.Geometry.Segment,ArcGIS.Core.Geometry.SegmentExtensionType,System.Double,ArcGIS.Core.Geometry.AsRatioOrLength)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.QueryPoint(ArcGIS.Core.Geometry.Multipart,ArcGIS.Core.Geometry.SegmentExtensionType,System.Double,ArcGIS.Core.Geometry.AsRatioOrLength)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.QueryPoint(ArcGIS.Core.Geometry.Segment,ArcGIS.Core.Geometry.SegmentExtensionType,System.Double,ArcGIS.Core.Geometry.AsRatioOrLength)
      #region QueryPoint

      SpatialReference sr = SpatialReferences.WGS84;

      // Horizontal line segment
      Coordinate2D start = new Coordinate2D(1, 1);
      Coordinate2D end = new Coordinate2D(11, 1);
      LineSegment line = LineBuilderEx.CreateLineSegment(start, end, sr);

      Polyline polyline = PolylineBuilderEx.CreatePolyline(line);

      // Don't extend the segment

      MapPoint outPoint = GeometryEngine.Instance.QueryPoint(polyline, SegmentExtensionType.NoExtension, 1.0, AsRatioOrLength.AsLength);
      // outPoint = (2, 1)

      // or the segment
      MapPoint outPoint_seg = GeometryEngine.Instance.QueryPoint(line, SegmentExtensionType.NoExtension, 1.0, AsRatioOrLength.AsLength);
      // outPoint_seg = (2, 1)

      // Extend infinitely in both directions
      outPoint = GeometryEngine.Instance.QueryPoint(polyline, SegmentExtensionType.ExtendTangents, 1.5, AsRatioOrLength.AsRatio);
      // outPoint = (16, 1)
      outPoint_seg = GeometryEngine.Instance.QueryPoint(line, SegmentExtensionType.ExtendTangents, 1.5, AsRatioOrLength.AsRatio);
      // outPoint_seg = (16, 1)
      #endregion
    }

    public void QueryPointAndDistance()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.QueryPointAndDistance(ArcGIS.Core.Geometry.Multipart,ArcGIS.Core.Geometry.SegmentExtensionType,ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.AsRatioOrLength,System.Double@,System.Double@,ArcGIS.Core.Geometry.LeftOrRightSide@)
      // cref: ArcGIS.Core.Geometry.GeometryEngine.QueryPointAndDistance(ArcGIS.Core.Geometry.Segment,ArcGIS.Core.Geometry.SegmentExtensionType,ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.AsRatioOrLength,System.Double@,System.Double@,ArcGIS.Core.Geometry.LeftOrRightSide@)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.QueryPointAndDistance(ArcGIS.Core.Geometry.Multipart,ArcGIS.Core.Geometry.SegmentExtensionType,ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.AsRatioOrLength,System.Double@,System.Double@,ArcGIS.Core.Geometry.LeftOrRightSide@)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.QueryPointAndDistance(ArcGIS.Core.Geometry.Segment,ArcGIS.Core.Geometry.SegmentExtensionType,ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.AsRatioOrLength,System.Double@,System.Double@,ArcGIS.Core.Geometry.LeftOrRightSide@)
      #region QueryPointAndDistance

      // Horizontal line segment
      List<MapPoint> linePts = new List<MapPoint>();
      linePts.Add(MapPointBuilderEx.CreateMapPoint(1.0, 1.0, SpatialReferences.WGS84));
      linePts.Add(MapPointBuilderEx.CreateMapPoint(11.0, 1.0, SpatialReferences.WGS84));
      Polyline polyline = PolylineBuilderEx.CreatePolyline(linePts);
      bool isSimple = GeometryEngine.Instance.IsSimpleAsFeature(polyline);

      // Don't extent the segment
      SegmentExtensionType extension = SegmentExtensionType.NoExtension;

      // A point on the line segment
      MapPoint inPoint = MapPointBuilderEx.CreateMapPoint(2, 1, SpatialReferences.WGS84);

      double distanceAlongCurve, distanceFromCurve;
      LeftOrRightSide whichSide;
      AsRatioOrLength asRatioOrLength = AsRatioOrLength.AsLength;

      MapPoint outPoint = GeometryEngine.Instance.QueryPointAndDistance(polyline, extension, inPoint, asRatioOrLength, out distanceAlongCurve, out distanceFromCurve, out whichSide);
      // outPoint = 2, 1
      // distanceAlongCurve = 1
      // distanceFromCurve = 0
      // whichSide = GeometryEngine.Instance.LeftOrRightSide.LeftSide


      // Extend infinitely in both directions
      extension = SegmentExtensionType.ExtendTangents;

      // A point on the left side
      inPoint = MapPointBuilderEx.CreateMapPoint(16, 6, SpatialReferences.WGS84);
      asRatioOrLength = AsRatioOrLength.AsRatio;

      outPoint = GeometryEngine.Instance.QueryPointAndDistance(polyline, extension, inPoint, asRatioOrLength, out distanceAlongCurve, out distanceFromCurve, out whichSide);
      // outPoint = 16, 1
      // distanceAlongCurve = 1.5
      // distanceFromCurve = 5
      // whichSide = GeometryEngine.Instance.LeftOrRightSide.LeftSide

      #endregion
    }

    public void QueryTangent()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.QueryTangent(ArcGIS.Core.Geometry.Multipart,ArcGIS.Core.Geometry.SegmentExtensionType,System.Double,ArcGIS.Core.Geometry.AsRatioOrLength,System.Double)
      // cref: ArcGIS.Core.Geometry.GeometryEngine.QueryTangent(ArcGIS.Core.Geometry.Segment,ArcGIS.Core.Geometry.SegmentExtensionType,System.Double,ArcGIS.Core.Geometry.AsRatioOrLength,System.Double)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.QueryTangent(ArcGIS.Core.Geometry.Multipart,ArcGIS.Core.Geometry.SegmentExtensionType,System.Double,ArcGIS.Core.Geometry.AsRatioOrLength,System.Double)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.QueryTangent(ArcGIS.Core.Geometry.Segment,ArcGIS.Core.Geometry.SegmentExtensionType,System.Double,ArcGIS.Core.Geometry.AsRatioOrLength,System.Double)
      #region QueryTangent

      LineSegment line = LineBuilderEx.CreateLineSegment(new Coordinate2D(0, 0), new Coordinate2D(1, 0));

      // No extension, distanceAlongCurve = 0.5
      LineSegment tangent = GeometryEngine.Instance.QueryTangent(line, SegmentExtensionType.NoExtension, 0.5, AsRatioOrLength.AsRatio, 1);
      // tangent.StartCoordinate = (0.5, 0.0)
      // tangent.EndCoordinate = (1.5, 0.0)

      tangent = GeometryEngine.Instance.QueryTangent(line, SegmentExtensionType.NoExtension, 1.5, AsRatioOrLength.AsLength, 1);
      // tangent.StartCoordinate = (1.0, 0.0)
      // tangent.EndCoordinate = (2.0, 0.0)

      tangent = GeometryEngine.Instance.QueryTangent(line, SegmentExtensionType.ExtendTangentAtTo, 1.5, AsRatioOrLength.AsLength, 1);
      // tangent.StartCoordinate = (1.5, 0.0)
      // tangent.EndCoordinate = (2.5, 0.0)

      tangent = GeometryEngine.Instance.QueryTangent(line, SegmentExtensionType.ExtendTangentAtFrom, -1.5, AsRatioOrLength.AsLength, 1);
      // tangent.StartCoordinate = (-1.5, 0.0)
      // tangent.EndCoordinate = (-0.5, 0.0)

      tangent = GeometryEngine.Instance.QueryTangent(line, SegmentExtensionType.ExtendTangentAtFrom, -0.5, AsRatioOrLength.AsRatio, 1);
      // tangent.StartCoordinate = (-0.5, 0.0)
      // tangent.EndCoordinate = (0.5, 0.0)
      #endregion
    }

    public void ReflectAboutLine()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.ReflectAboutLine(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.LineSegment)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.ReflectAboutLine(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.LineSegment)
      #region Reflect a polygon about a line

      SpatialReference sr = SpatialReferences.WGS84;

      Coordinate2D start = new Coordinate2D(0, 0);
      Coordinate2D end = new Coordinate2D(4, 4);
      LineSegment line = LineBuilderEx.CreateLineSegment(start, end, sr);

      Coordinate2D[] coords = new Coordinate2D[]
      {
        new Coordinate2D(-1, 2),
        new Coordinate2D(-1, 4),
        new Coordinate2D(1, 4),
        new Coordinate2D(-1, 2)
      };

      Polygon polygon = PolygonBuilderEx.CreatePolygon(coords, sr);

      // reflect a polygon about the line
      Polygon reflectedPolygon = GeometryEngine.Instance.ReflectAboutLine(polygon, line) as Polygon;

      // reflectedPolygon points are 
      //    (2, -1), (4, -1), (4, 1), (2, -1)
      #endregion
    }

    public void Related()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Relate(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Geometry,System.String)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Relate(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Geometry,System.String)
      #region Determine relationship between two geometries

      // set up some geometries

      // points
      MapPoint point0 = MapPointBuilderEx.CreateMapPoint(0, 0, SpatialReferences.WGS84);
      MapPoint point1 = MapPointBuilderEx.CreateMapPoint(1, 1, SpatialReferences.WGS84);
      MapPoint point2 = MapPointBuilderEx.CreateMapPoint(-5, 5, SpatialReferences.WGS84);

      // multipoint
      List<MapPoint> points = new List<MapPoint>() { point0, point1, point2 };
      Multipoint multipoint = MultipointBuilderEx.CreateMultipoint(points, SpatialReferences.WGS84);

      // polygon 
      List<Coordinate2D> polygonCoords = new List<Coordinate2D>()
      {
          new Coordinate2D(-10, 0),
          new Coordinate2D(0, 10),
          new Coordinate2D(10, 0),
          new Coordinate2D(-10, 0)
      };
      Polygon polygon = PolygonBuilderEx.CreatePolygon(polygonCoords, SpatialReferences.WGS84);

      // polylines
      Polyline polyline1 = PolylineBuilderEx.CreatePolyline(LineBuilderEx.CreateLineSegment(new Coordinate2D(-9.1, 0.1), new Coordinate2D(0, 9)), SpatialReferences.WGS84);
      Polyline polyline2 = PolylineBuilderEx.CreatePolyline(LineBuilderEx.CreateLineSegment(new Coordinate2D(-5, 5), new Coordinate2D(0, 5)), SpatialReferences.WGS84);
      Polyline polyline3 = PolylineBuilderEx.CreatePolyline(LineBuilderEx.CreateLineSegment(new Coordinate2D(2.09, -2.04), new Coordinate2D(5, 10)), SpatialReferences.WGS84);
      Polyline polyline4 = PolylineBuilderEx.CreatePolyline(LineBuilderEx.CreateLineSegment(new Coordinate2D(10, -5), new Coordinate2D(10, 5)), SpatialReferences.WGS84);

      List<Segment> segments = new List<Segment>()
      {
          LineBuilderEx.CreateLineSegment(new Coordinate2D(5.05, -2.87), new Coordinate2D(6.35, 1.57)),
          LineBuilderEx.CreateLineSegment(new Coordinate2D(6.35, 1.57), new Coordinate2D(4.13, 2.59)),
          LineBuilderEx.CreateLineSegment(new Coordinate2D(4.13, 2.59), new Coordinate2D(5, 5))
      };
      Polyline polyline5 = PolylineBuilderEx.CreatePolyline(segments, SpatialReferences.WGS84);

      segments.Add(LineBuilderEx.CreateLineSegment(new Coordinate2D(5, 5), new Coordinate2D(10, 10)));

      Polyline polyline6 = PolylineBuilderEx.CreatePolyline(segments, SpatialReferences.WGS84);
      Polyline polyline7 = PolylineBuilderEx.CreatePolyline(polyline5);
      Polyline polyline8 = PolylineBuilderEx.CreatePolyline(LineBuilderEx.CreateLineSegment(new Coordinate2D(5, 5), new Coordinate2D(10, 10)), SpatialReferences.WGS84);

      segments.Clear();
      segments.Add(LineBuilderEx.CreateLineSegment(new Coordinate2D(0.6, 3.5), new Coordinate2D(0.7, 7)));
      segments.Add(LineBuilderEx.CreateLineSegment(new Coordinate2D(0.7, 7), new Coordinate2D(3, 9)));

      Polyline polyline9 = PolylineBuilderEx.CreatePolyline(segments, SpatialReferences.WGS84);

      // now do the Related tests

      // Interior/Interior Intersects
      string scl = "T********";
      bool related = GeometryEngine.Instance.Relate(polygon, polyline1, scl);     // related = true
      related = GeometryEngine.Instance.Relate(point0, point1, scl);              // related = false
      related = GeometryEngine.Instance.Relate(point0, multipoint, scl);          // related = true
      related = GeometryEngine.Instance.Relate(multipoint, polygon, scl);         // related = true
      related = GeometryEngine.Instance.Relate(multipoint, polyline1, scl);       // related = false
      related = GeometryEngine.Instance.Relate(polyline2, point2, scl);           // related = false
      related = GeometryEngine.Instance.Relate(point1, polygon, scl);             // related = true

      // Interior/Boundary Intersects
      scl = "*T*******";
      related = GeometryEngine.Instance.Relate(polygon, polyline2, scl);          // related = true
      related = GeometryEngine.Instance.Relate(polygon, polyline3, scl);          // related = false
      related = GeometryEngine.Instance.Relate(point1, polygon, scl);             // related = false

      // Boundary/Boundary Interior intersects
      scl = "***T*****";
      related = GeometryEngine.Instance.Relate(polygon, polyline4, scl);          // related = true

      // Overlaps Dim1
      scl = "1*T***T**";
      related = GeometryEngine.Instance.Relate(polygon, polyline5, scl);          // related = true

      // Crosses Area/Line (LineB crosses PolygonA)
      scl = "1020F1102";
      related = GeometryEngine.Instance.Relate(polygon, polyline6, scl);          // related = false
      related = GeometryEngine.Instance.Relate(polygon, polyline9, scl);          // related = true

      // Boundary/Boundary Touches
      scl = "F***T****";
      related = GeometryEngine.Instance.Relate(polygon, polyline7, scl);          // related = false
      related = GeometryEngine.Instance.Relate(polygon, polyline8, scl);          // related = true

      #endregion
    }

    public void ReplaceNaNZs()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.ReplaceNaNZs(ArcGIS.Core.Geometry.Geometry,System.Double)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.ReplaceNaNZs(ArcGIS.Core.Geometry.Geometry,System.Double)
      #region Replace NaN Zs in a polygon

      List<Coordinate3D> coordsZ = new List<Coordinate3D>()
      {
        new Coordinate3D(1, 2, double.NaN),
        new Coordinate3D(4, 5, 3),
        new Coordinate3D(7, 8, double.NaN)
      };

      Polygon polygon = PolygonBuilderEx.CreatePolygon(coordsZ);
      // polygon.HasZ = true

      Polygon polygonZReplaced = GeometryEngine.Instance.ReplaceNaNZs(polygon, -1) as Polygon;

      // polygonZReplaced.Points[0].Z = -1
      // polygonZReplaced.Points[1].Z = 3
      // polygonZReplaced.Points[2].Z = -1
      #endregion
    }

    public void Reshape()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Reshape(ArcGIS.Core.Geometry.Multipart,ArcGIS.Core.Geometry.Polyline)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Reshape(ArcGIS.Core.Geometry.Multipart,ArcGIS.Core.Geometry.Polyline)
      #region Reshape a polygon

      List<Coordinate2D> polygon1Coords = new List<Coordinate2D>()
      {
        new Coordinate2D(0, -11000),
        new Coordinate2D(1000, -11000),
        new Coordinate2D(1000, -12000),
        new Coordinate2D(0, -12000),
        new Coordinate2D(0, -11000)
      };

      // reshaper coordinates intersect the polygon
      List<Coordinate2D> reshaperCoords = new List<Coordinate2D>()
      {
        new Coordinate2D(1500, -11800),
        new Coordinate2D(-2600, -11800)
      };

      SpatialReference sr = SpatialReferenceBuilder.CreateSpatialReference(102010);
      Polygon polygon1 = PolygonBuilderEx.CreatePolygon(polygon1Coords, sr);
      Polyline reshaper = PolylineBuilderEx.CreatePolyline(reshaperCoords, sr);

      Polygon outPolygon = GeometryEngine.Instance.Reshape(polygon1, reshaper) as Polygon;
      // outPolygon.PartCount = 1

      ReadOnlySegmentCollection segments = outPolygon.Parts[0];
      // segments.Count = 4
      // outPolygon.PointCount = 5

      string json = GeometryEngine.Instance.ExportToJson(JsonExportFlags.JsonExportSkipCRS, outPolygon);
      // json = "{\"rings\":[[[0,-11800],[0,-11000],[1000,-11000],[1000,-11800],[0,-11800]]]}";


      // example where the Reshaper polyline doesn't intersect the input

      reshaperCoords.Clear();
      reshaperCoords.Add(new Coordinate2D(1000, 1000));
      reshaperCoords.Add(new Coordinate2D(2000, 1000));

      reshaper = PolylineBuilderEx.CreatePolyline(reshaperCoords, sr);

      outPolygon = GeometryEngine.Instance.Reshape(polygon1, reshaper) as Polygon;
      // outPolygon = null

      #endregion
    }

    public void ReverseOrientation()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.ReverseOrientation(ArcGIS.Core.Geometry.Multipart)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.ReverseOrientation(ArcGIS.Core.Geometry.Multipart)
      #region Reverse the order of points in a Polygon

      List<Coordinate2D> list2D = new List<Coordinate2D>();
      list2D.Add(new Coordinate2D(1.0, 1.0));
      list2D.Add(new Coordinate2D(1.0, 2.0));
      list2D.Add(new Coordinate2D(2.0, 2.0));
      list2D.Add(new Coordinate2D(2.0, 1.0));

      Polygon polygon = PolygonBuilderEx.CreatePolygon(list2D);

      Geometry g = GeometryEngine.Instance.ReverseOrientation(polygon);
      Polygon gPolygon = g as Polygon;

      // gPolygon.Points[0] = 1.0, 1.0
      // gPolygon.Points[1] = 2.0, 1.0
      // gPolygon.Points[2] = 2.0, 2.0
      // gPolygon.Points[3] = 1.0, 2.0
      // gPolygon.Points[4] = 1.0, 1.0
      // gPolygon.Area = -1 * polygon.Area
      #endregion
    }

    public void Rotate()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Rotate(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.MapPoint,System.Double)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Rotate(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.MapPoint,System.Double)
      #region Rotate a MapPoint

      MapPoint pt = MapPointBuilderEx.CreateMapPoint(1.0, 3.0);
      MapPoint rotatePt = MapPointBuilderEx.CreateMapPoint(3.0, 3.0);

      Geometry result = GeometryEngine.Instance.Rotate(pt, rotatePt, Math.PI / 2);
      // result point is (3, 1)
      #endregion

      // cref: ArcGIS.Core.Geometry.GeometryEngine.Rotate(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.MapPoint,System.Double)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Rotate(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.MapPoint,System.Double)
      #region Rotate a Polyline

      // rotate a polyline

      MapPoint fixedPt = MapPointBuilderEx.CreateMapPoint(3.0, 3.0);

      List<MapPoint> pts = new List<MapPoint>();
      pts.Add(MapPointBuilderEx.CreateMapPoint(1.0, 1.0));
      pts.Add(MapPointBuilderEx.CreateMapPoint(1.0, 5.0));
      pts.Add(MapPointBuilderEx.CreateMapPoint(5, 5));
      pts.Add(MapPointBuilderEx.CreateMapPoint(5.0, 1.0));
      Polyline polyline = PolylineBuilderEx.CreatePolyline(pts);

      Polyline rotated = GeometryEngine.Instance.Rotate(polyline, fixedPt, Math.PI / 4) as Polyline;  // rotate 45 deg
      #endregion
    }

    public void Scale()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Scale(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.MapPoint,System.Double,System.Double,System.Double)
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Scale(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.MapPoint,System.Double,System.Double)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Scale(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.MapPoint,System.Double,System.Double,System.Double)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Scale(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.MapPoint,System.Double,System.Double)
      #region Scale a geometry

      List<MapPoint> pts = new List<MapPoint>();
      pts.Add(MapPointBuilderEx.CreateMapPoint(1.0, 1.0, 3.0));
      pts.Add(MapPointBuilderEx.CreateMapPoint(1.0, 3.0, 3.0));
      pts.Add(MapPointBuilderEx.CreateMapPoint(3, 3, 3.0));
      pts.Add(MapPointBuilderEx.CreateMapPoint(3.0, 1.0, 3.0));

      MapPoint midPt = MapPointBuilderEx.CreateMapPoint(1.5, 1.5);

      // polyline
      Polyline polyline = PolylineBuilderEx.CreatePolyline(pts);
      // polyline.Length = 6
      // polyline.Length3D = 0
      Geometry g = GeometryEngine.Instance.Scale(polyline, midPt, 0.5, 0.5);
      Polyline resultPolyline = g as Polyline;
      // resultPolyline.length  = 3
      // resultPolyline.Points[0] = 1.25, 1.25, 3
      // resultPolyline.Points[1] = 1.25, 2.25, 3
      // resultPolyline.Points[2] = 2.25, 2.25, 3
      // resultPolyline.Points[3] = 2.25, 1.25, 3

      // 3D point - scale in 3d
      MapPoint midPtZ = MapPointBuilderEx.CreateMapPoint(1.5, 1.5, 1);
      g = GeometryEngine.Instance.Scale(polyline, midPtZ, 0.5, 0.5, 0.25);
      resultPolyline = g as Polyline;
      // resultPolyline.Points[0] = 1.25, 1.25, 1.5
      // resultPolyline.Points[1] = 1.25, 2.25, 1.5
      // resultPolyline.Points[2] = 2.25, 2.25, 1.5
      // resultPolyline.Points[3] = 2.25, 1.25, 1.5

      #endregion
    }

    public void SetConstantZ()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.SetConstantZ(ArcGIS.Core.Geometry.Multipart,System.Double)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.SetConstantZ(ArcGIS.Core.Geometry.Multipart,System.Double)
      #region Set all Zs in a polyline

      List<Coordinate3D> coordsZ = new List<Coordinate3D>()
      {
        new Coordinate3D(1, 2, 3),
        new Coordinate3D(4, 5, 6),
        new Coordinate3D(7, 8, double.NaN)
      };

      Polyline polyline = PolylineBuilderEx.CreatePolyline(coordsZ);
      // polyline.HasZ = true

      Polyline polylineSetZ = GeometryEngine.Instance.SetConstantZ(polyline, -1) as Polyline;
      // polylineSetZ.Points[0].Z = -1
      // polylineSetZ.Points[1].Z = -1
      // polylineSetZ.Points[2].Z = -1

      polylineSetZ = GeometryEngine.Instance.SetConstantZ(polyline, double.NaN) as Polyline;
      // polyline.HasZ = true
      // polylineSetZ.Points[0].HasZ = true
      // polylineSetZ.Points[0].Z = NaN
      // polylineSetZ.Points[1].HasZ = true
      // polylineSetZ.Points[1].Z = NaN
      // polylineSetZ.Points[2].HasZ = true
      // polylineSetZ.Points[2].Z = NaN

      polylineSetZ = GeometryEngine.Instance.SetConstantZ(polyline, double.PositiveInfinity) as Polyline;
      // polyline.HasZ = true
      // polylineSetZ.Points[0].HasZ = true
      // polylineSetZ.Points[0].Z = double.PositiveInfinity
      // polylineSetZ.Points[1].HasZ = true
      // polylineSetZ.Points[1].Z = double.PositiveInfinity
      // polylineSetZ.Points[2].HasZ = true
      // polylineSetZ.Points[2].Z = double.PositiveInfinity
      #endregion
    }

    public void ShapePreservingArea()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.ShapePreservingArea(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.AreaUnit)
      // cref: ArcGIS.Core.Geometry.GeometryEngine.ShapePreservingArea(ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.ShapePreservingArea(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.AreaUnit)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.ShapePreservingArea(ArcGIS.Core.Geometry.Geometry)
      #region Calculate area of geometry on surface of Earth's ellipsoid - ShapePreservingArea

      // pt
      MapPoint pt = MapPointBuilderEx.CreateMapPoint(1.0, 3.0, SpatialReferences.WebMercator);
      double area = GeometryEngine.Instance.ShapePreservingArea(pt);         // area = 0

      List<MapPoint> pts = new List<MapPoint>();
      pts.Add(MapPointBuilderEx.CreateMapPoint(1.0, 1.0, 3.0));
      pts.Add(MapPointBuilderEx.CreateMapPoint(1.0, 3.0, 3.0));
      pts.Add(MapPointBuilderEx.CreateMapPoint(3, 3, 3.0));
      pts.Add(MapPointBuilderEx.CreateMapPoint(3.0, 1.0, 3.0));

      // multipoint
      Multipoint mPt = MultipointBuilderEx.CreateMultipoint(pts);
      area = GeometryEngine.Instance.ShapePreservingArea(mPt);               // area = 0

      // polyline
      Polyline polyline = PolylineBuilderEx.CreatePolyline(pts);
      area = GeometryEngine.Instance.ShapePreservingArea(polyline);          // area = 0

      // polygon
      Polygon polygon = PolygonBuilderEx.CreatePolygon(pts, SpatialReferences.WGS84);
      area = GeometryEngine.Instance.ShapePreservingArea(polygon);

      polygon = PolygonBuilderEx.CreatePolygon(pts, SpatialReferences.WebMercator);
      area = GeometryEngine.Instance.ShapePreservingArea(polygon);


      polygon = PolygonBuilderEx.CreatePolygon(new[]
      {
          MapPointBuilderEx.CreateMapPoint( -170, 45),
          MapPointBuilderEx.CreateMapPoint( 170, 45),
          MapPointBuilderEx.CreateMapPoint( 170, -45),
          MapPointBuilderEx.CreateMapPoint( -170, -54)
      }, SpatialReferences.WGS84);

      var area_meters = GeometryEngine.Instance.ShapePreservingArea(polygon);// , AreaUnits.SquareMeters);
      var area_miles = GeometryEngine.Instance.ShapePreservingArea(polygon, AreaUnit.SquareMiles);

      // area_meters - 352556425383104.37
      // area_miles - 136122796.848425

      #endregion
    }

    public void ShapePreservingLength()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.ShapePreservingLength(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.LinearUnit)
      // cref: ArcGIS.Core.Geometry.GeometryEngine.ShapePreservingLength(ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.ShapePreservingLength(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.LinearUnit)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.ShapePreservingLength(ArcGIS.Core.Geometry.Geometry)
      #region Calculate length of geometry on surface of Earth's ellipsoid - ShapePreservingLength

      // pt
      MapPoint pt = MapPointBuilderEx.CreateMapPoint(1.0, 3.0, SpatialReferences.WebMercator);
      double len = GeometryEngine.Instance.ShapePreservingLength(pt);          // len = 0

      List<MapPoint> pts = new List<MapPoint>();
      pts.Add(MapPointBuilderEx.CreateMapPoint(1.0, 1.0, 3.0));
      pts.Add(MapPointBuilderEx.CreateMapPoint(1.0, 3.0, 3.0));
      pts.Add(MapPointBuilderEx.CreateMapPoint(3, 3, 3.0));
      pts.Add(MapPointBuilderEx.CreateMapPoint(3.0, 1.0, 3.0));

      // multipoint
      Multipoint mPt = MultipointBuilderEx.CreateMultipoint(pts);
      len = GeometryEngine.Instance.ShapePreservingLength(mPt);                // len = 0

      // polyline
      Polyline polyline = PolylineBuilderEx.CreatePolyline(pts, SpatialReferences.WGS84);
      len = GeometryEngine.Instance.ShapePreservingLength(polyline);

      // polygon
      Polygon polygon = PolygonBuilderEx.CreatePolygon(pts, SpatialReferences.WGS84);
      len = GeometryEngine.Instance.ShapePreservingLength(polygon);



      polyline = PolylineBuilderEx.CreatePolyline(new[]
      {
          MapPointBuilderEx.CreateMapPoint( -170, 0),
          MapPointBuilderEx.CreateMapPoint( 170, 0)
      }, SpatialReferences.WGS84);


      var length_meters = GeometryEngine.Instance.ShapePreservingLength(polyline); // , LinearUnits.Meters);
      var length_miles = GeometryEngine.Instance.ShapePreservingLength(polyline, LinearUnit.Miles);

      // length_meters - 37848626.869713023
      // length_miles - 23518.046402579574

      #endregion
    }

    public void SideBuffer()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.SideBuffer(ArcGIS.Core.Geometry.Polyline,System.Double,ArcGIS.Core.Geometry.LeftOrRightSide,ArcGIS.Core.Geometry.LineCapType)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.SideBuffer(ArcGIS.Core.Geometry.Polyline,System.Double,ArcGIS.Core.Geometry.LeftOrRightSide,ArcGIS.Core.Geometry.LineCapType)
      #region SideBuffer

      // right side, round caps
      SpatialReference sr = SpatialReferenceBuilder.CreateSpatialReference(102010);
      List<Coordinate2D> coords = new List<Coordinate2D>()
      {
        new Coordinate2D(1200, 5800),
        new Coordinate2D(1400, 5800),
        new Coordinate2D(1400, 6000),
        new Coordinate2D(1300, 6000),
        new Coordinate2D(1300, 5700)
      };

      Polyline polyline = PolylineBuilderEx.CreatePolyline(coords, sr);
      Polygon output = GeometryEngine.Instance.SideBuffer(polyline, 20, LeftOrRightSide.RightSide, LineCapType.Round) as Polygon;
      #endregion

      // cref: ArcGIS.Core.Geometry.GeometryEngine.SideBuffer(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Polyline},System.Double,ArcGIS.Core.Geometry.LeftOrRightSide,ArcGIS.Core.Geometry.LineCapType)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.SideBuffer(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Polyline},System.Double,ArcGIS.Core.Geometry.LeftOrRightSide,ArcGIS.Core.Geometry.LineCapType)
      #region SideBuffer Many

      SpatialReference spatialReference = SpatialReferenceBuilder.CreateSpatialReference(102010);
      List<Coordinate2D> coordinates = new List<Coordinate2D>()
      {
        new Coordinate2D(1200, 5800),
        new Coordinate2D(1400, 5800),
        new Coordinate2D(1400, 6000),
        new Coordinate2D(1300, 6000),
        new Coordinate2D(1300, 5700)
      };

      Polyline polyline1 = PolylineBuilderEx.CreatePolyline(coordinates, spatialReference);

      coordinates.Clear();
      coordinates.Add(new Coordinate2D(1400, 6050));
      coordinates.Add(new Coordinate2D(1600, 6150));
      coordinates.Add(new Coordinate2D(1800, 6050));

      Polyline polyline2 = PolylineBuilderEx.CreatePolyline(coordinates, spatialReference);
      List<Polyline> polylines = new List<Polyline>() { polyline1, polyline2 };
      IReadOnlyList<Geometry> outGeometries = GeometryEngine.Instance.SideBuffer(polylines, 10, LeftOrRightSide.RightSide, LineCapType.Round);
      #endregion
    }

    public void SimplifyAsFeature()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.SimplifyAsFeature(ArcGIS.Core.Geometry.Geometry,System.Boolean)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.SimplifyAsFeature(ArcGIS.Core.Geometry.Geometry,System.Boolean)
      #region Simplify a polygon 

      var g1 = PolygonBuilderEx.FromJson("{\"rings\": [ [ [0, 0], [10, 0], [10, 10], [0, 10] ] ] }");
      var result = GeometryEngine.Instance.Area(g1);      // result = -100.0   - negative due to wrong ring orientation
                                                          // simplify it
      var result2 = GeometryEngine.Instance.Area(GeometryEngine.Instance.SimplifyAsFeature(g1, true));
      // result2 = 100.0  - positive due to correct ring orientation (clockwise)
      #endregion
    }

    public void SimplifyPolyline()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.SimplifyPolyline(ArcGIS.Core.Geometry.Polyline,ArcGIS.Core.Geometry.SimplifyType,System.Boolean)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.SimplifyPolyline(ArcGIS.Core.Geometry.Polyline,ArcGIS.Core.Geometry.SimplifyType,System.Boolean)
      #region Simplify a polyline with intersections, overlaps

      List<Coordinate2D> coords = new List<Coordinate2D>()
      {
        new Coordinate2D(8, 0),
        new Coordinate2D(8, 4),
        new Coordinate2D(6, 4),
        new Coordinate2D(8, 4),
        new Coordinate2D(10, 4),
        new Coordinate2D(8, 4)
      };

      SpatialReference sr = SpatialReferences.WGS84;

      // build a line that has segments that cross over each other
      Polyline polyline = PolylineBuilderEx.CreatePolyline(coords, sr);
      // polyline.PartCount = 1
      ReadOnlyPartCollection parts = polyline.Parts;
      ReadOnlySegmentCollection segments = parts[0];
      // segments.Count = 5

      //  note there is a difference between SimpleAsFeature (doesn't detect intersections and overlaps, determines if it's simple enough for gdb storage)
      //  and SimplifyPolyline  (does detect intersections etc)
      bool isSimple = GeometryEngine.Instance.IsSimpleAsFeature(polyline, false);
      // isSimple = true

      // simplify it (with force = false)
      // because it has already been deemed 'simple' (previous IsSimpleAsFeature call) no detection of intersections, overlaps occur
      Polyline simplePolyline = GeometryEngine.Instance.SimplifyPolyline(polyline, SimplifyType.Planar, false);
      // simplePolyline.PartCount = 1
      ReadOnlyPartCollection simpleParts = simplePolyline.Parts;
      ReadOnlySegmentCollection simpleSegments = simpleParts[0];
      // simpleSegments.Count = 5

      // simplify it (with force = true)
      // detection of intersections, overlaps occur 
      simplePolyline = GeometryEngine.Instance.SimplifyPolyline(polyline, SimplifyType.Planar, true);
      // simplePolyline.PartCount = 3
      simpleParts = simplePolyline.Parts;
      simpleSegments = simpleParts[0];
      // simpleSegments.Count = 1
      #endregion
    }

    public void SlicePolygonIntoEqualParts()
    {
      Polygon polygon = null;

      // cref: ArcGIS.Core.Geometry.GeometryEngine.SlicePolygonIntoEqualParts(ArcGIS.Core.Geometry.Polygon,System.Int32,System.Double,ArcGIS.Core.Geometry.SliceType)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.SlicePolygonIntoEqualParts(ArcGIS.Core.Geometry.Polygon,System.Int32,System.Double,ArcGIS.Core.Geometry.SliceType)
      #region Slice a Polygon into equal parts
      
      var slices = GeometryEngine.Instance.SlicePolygonIntoEqualParts(polygon, 3, 0, SliceType.Blocks);
      // slices.Count = 3


      // simple polygon
      List<Coordinate2D> list2D = new List<Coordinate2D>();
      list2D.Add(new Coordinate2D(1.0, 1.0));
      list2D.Add(new Coordinate2D(1.0, 2.0));
      list2D.Add(new Coordinate2D(2.0, 2.0));
      list2D.Add(new Coordinate2D(2.0, 1.0));

      Polygon p = PolygonBuilderEx.CreatePolygon(list2D);
      slices = GeometryEngine.Instance.SlicePolygonIntoEqualParts(p, 2, 0, SliceType.Strips);

      // slice[0] coordinates - (1.0, 1.0), (1.0, 1.5), (2.0, 1.5), (2.0, 1.0), (1.0, 1.0) 
      // slice[1] coordinates - (1.0, 1.5), (1.0, 2.0), (2.0, 2.0), (2.0, 1.5), (1.0, 1.5)

      slices = GeometryEngine.Instance.SlicePolygonIntoEqualParts(p, 2, Math.PI / 4, SliceType.Strips);

      // slice[0] coordinates - (1.0, 1.0), (1.0, 2.0), (2.0, 1.0), (1.0, 1.0)
      // slice[1] coordinates - (1.0, 2.0), (2.0, 2.0), (2.0, 1.0), (1.0, 2.0)

      #endregion
    }

    public void SplitAtPoint()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.SplitAtPoint(ArcGIS.Core.Geometry.Multipart,ArcGIS.Core.Geometry.MapPoint,System.Boolean,System.Boolean,System.Boolean@,System.Int32@,System.Int32@)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.SplitAtPoint(ArcGIS.Core.Geometry.Multipart,ArcGIS.Core.Geometry.MapPoint,System.Boolean,System.Boolean,System.Boolean@,System.Int32@,System.Int32@)
      #region Split multipart at point

      // define a polyline
      MapPoint startPointZ = MapPointBuilderEx.CreateMapPoint(1, 1, 5);
      MapPoint endPointZ = MapPointBuilderEx.CreateMapPoint(20, 1, 5);

      Polyline polylineZ = PolylineBuilderEx.CreatePolyline(new List<MapPoint>() { startPointZ, endPointZ });

      // define a split point
      MapPoint splitPointAboveLine = MapPointBuilderEx.CreateMapPoint(10, 10, 10);

      bool splitOccurred;
      int partIndex;
      int segmentIndex;

      // split the polyline at the point.  dont project the split point onto the line, don't create a new part
      var splitPolyline = GeometryEngine.Instance.SplitAtPoint(polylineZ, splitPointAboveLine, false, false, out splitOccurred, out partIndex, out segmentIndex);

      // splitOccurred = true
      // partIndex = 0
      // segmentIndex = 1
      // splitPolyline.PointCount = 3
      // splitPolyline.PartCount = 1
      // splitPolyline coordinates are (1, 1, 5), (10, 10, 10), (20, 1, 5)

      // split the polyline at the point.  dont project the split point onto the line, do create a new part
      splitPolyline = GeometryEngine.Instance.SplitAtPoint(polylineZ, splitPointAboveLine, false, false, out splitOccurred, out partIndex, out segmentIndex);

      // splitOccurred = true
      // partIndex = 1
      // segmentIndex = 0
      // splitPolyline.PointCount = 4
      // splitPolyline.PartCount = 2
      // splitPolyline first part coordinates are (1, 1, 5), (10, 10, 10)
      // splitPolyline second part coordinates are (10, 10, 10), (20, 1, 5)


      // split the polyline at the point.  do project the split point onto the line, don't create a new part
      splitPolyline = GeometryEngine.Instance.SplitAtPoint(polylineZ, splitPointAboveLine, false, false, out splitOccurred, out partIndex, out segmentIndex);

      // splitOccurred = true
      // partIndex = 0
      // segmentIndex = 1
      // splitPolyline.PointCount = 3
      // splitPolyline.PartCount = 1
      // splitPolyline coordinates are (1, 1, 5), (10, 10, 5), (20, 1, 5)

      // split the polyline at the point.  do project the split point onto the line, do create a new part
      splitPolyline = GeometryEngine.Instance.SplitAtPoint(polylineZ, splitPointAboveLine, false, false, out splitOccurred, out partIndex, out segmentIndex);

      // splitOccurred = true
      // partIndex = 1
      // segmentIndex = 0
      // splitPolyline.PointCount = 4
      // splitPolyline.PartCount = 2
      // splitPolyline first part coordinates are (1, 1, 5), (10, 10, 5)
      // splitPolyline second part coordinates are (10, 10, 5), (20, 1, 5)


      //
      // try to split with a point that won't split the line  - pt extends beyond the line
      //

      var pointAfterLine = MapPointBuilderEx.CreateMapPoint(50, 1, 10);
      splitPolyline = GeometryEngine.Instance.SplitAtPoint(polylineZ, pointAfterLine, false, false, out splitOccurred, out partIndex, out segmentIndex);

      // splitOccurred = false
      // ignore partIndex, sgementIndex
      // splitPolyline is the same as polylineZ


      ///
      ///  multipart polygon
      ///
      List<Coordinate3D> coordsZ = new List<Coordinate3D>()
      {
        new Coordinate3D(10,10,5),
        new Coordinate3D(10,20,5),
        new Coordinate3D(20,20,5),
        new Coordinate3D(20,10,5)
      };

      List<Coordinate3D> coordsZ_2ndPart = new List<Coordinate3D>()
      {
        new Coordinate3D(30,20,10),
        new Coordinate3D(30,30,10),
        new Coordinate3D(35,28,10),
        new Coordinate3D(40,30,10),
        new Coordinate3D(40,20,10),
      };

      var builder = new PolygonBuilderEx();
      builder.HasZ = true;
      builder.AddPart(coordsZ);
      builder.AddPart(coordsZ_2ndPart);

      Polygon multipart = builder.ToGeometry();

      // pointA is closer to the first part of the multipart - the split occurs in the first part
      var pointA = MapPointBuilderEx.CreateMapPoint(22, 18, 7);
      var splitPolygon = GeometryEngine.Instance.SplitAtPoint(multipart, pointA, false, false, out splitOccurred, out partIndex, out segmentIndex);

      // splitPolygon.PointCount = 12
      // splitPolygon.PartCount = 2
      // splitPolygon first part coordinates  (10, 10, 5), (10, 20, 5), (20, 20, 5), (22, 18, 7), (20, 10, 5), (10, 10, 5)


      // pointB is midPoint between the 2 parts - no split will occur
      var pointB = MapPointBuilderEx.CreateMapPoint(25, 20, 7);
      splitPolygon = GeometryEngine.Instance.SplitAtPoint(multipart, pointB, true, false, out splitOccurred, out partIndex, out segmentIndex);

      // splitOccurred = false
      // ignore partIndex, sgementIndex
      // splitPolyline is the same as polylineZ

      #endregion
    }

    public void Touches()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Touches(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Touches(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Geometry)
      #region Polygon touches another Polygon

      // two disjoint polygons
      Envelope env = EnvelopeBuilderEx.CreateEnvelope(MapPointBuilderEx.CreateMapPoint(4.0, 4.0), MapPointBuilderEx.CreateMapPoint(8, 8));
      Polygon poly1 = PolygonBuilderEx.CreatePolygon(env);

      Envelope env2 = EnvelopeBuilderEx.CreateEnvelope(MapPointBuilderEx.CreateMapPoint(1.0, 1.0), MapPointBuilderEx.CreateMapPoint(5, 5));
      Polygon poly2 = PolygonBuilderEx.CreatePolygon(env2);

      bool touches = GeometryEngine.Instance.Touches(poly1, poly2);    // touches = false

      // another polygon that touches the first
      Envelope env3 = EnvelopeBuilderEx.CreateEnvelope(MapPointBuilderEx.CreateMapPoint(1.0, 1.0), MapPointBuilderEx.CreateMapPoint(4, 4));
      Polygon poly3 = PolygonBuilderEx.CreatePolygon(env3);

      touches = GeometryEngine.Instance.Touches(poly1, poly3);         // touches = true
      #endregion
    }

    public void Transform2D()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Transform2D(ArcGIS.Core.Geometry.Coordinate2D[],ArcGIS.Core.Geometry.ProjectionTransformation,ArcGIS.Core.Geometry.Coordinate2D[]@,System.Boolean)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Transform2D(ArcGIS.Core.Geometry.Coordinate2D[],ArcGIS.Core.Geometry.ProjectionTransformation,ArcGIS.Core.Geometry.Coordinate2D[]@,System.Boolean)
      #region Transform2D

      // Not all of the input points are transformed as some of them are outside of the GCS horizon.
      Coordinate2D[] inCoords2D = new Coordinate2D[]
      {
        new Coordinate2D(-1, -1),
        new Coordinate2D(-2, -5),
        new Coordinate2D(-5, -11),
        new Coordinate2D(-10, -19),
        new Coordinate2D(-17, -29),
        new Coordinate2D(-26, -41),
        new Coordinate2D(-37, -5),
        new Coordinate2D(-50, -21),
        new Coordinate2D(-65, -39),
        new Coordinate2D(-82, -9)
      };

      int arraySize = inCoords2D.Length;

      ProjectionTransformation projTrans = ProjectionTransformation.Create(SpatialReferences.WGS84, SpatialReferenceBuilder.CreateSpatialReference(24891));

      Coordinate2D[] outCoords2D = new Coordinate2D[arraySize];

      // transform and choose to remove the clipped coordinates
      int numPointsTransformed = GeometryEngine.Instance.Transform2D(inCoords2D, projTrans, ref outCoords2D, true);

      // numPointsTransformed = 4
      // outCoords2D.Length = 4

      // outCoords2D[0] = {5580417.6876455201, 1328841.2376554986}
      // outCoords2D[1] = {3508774.290814558, -568027.23444226268}
      // outCoords2D[2] = {1568096.0886155984, -2343435.4394415971}
      // outCoords2D[3] = {57325.827391741652, 1095146.8917508761}

      // transform and don't remove the clipped coordinates
      numPointsTransformed = GeometryEngine.Instance.Transform2D(inCoords2D, projTrans, ref outCoords2D, false);

      // numPointsTransformed = 4
      // outCoords2D.Length = 10

      // outCoords2D[0] = {double.Nan, double.Nan}
      // outCoords2D[1] = {double.Nan, double.Nan}
      // outCoords2D[2] = {double.Nan, double.Nan}
      // outCoords2D[3] = {double.Nan, double.Nan}
      // outCoords2D[4] = {double.Nan, double.Nan}
      // outCoords2D[5] = {double.Nan, double.Nan}
      // outCoords2D[6] = {5580417.6876455201, 1328841.2376554986}
      // outCoords2D[7] = {3508774.290814558, -568027.23444226268}
      // outCoords2D[8] = {1568096.0886155984, -2343435.4394415971}
      // outCoords2D[9] = {57325.827391741652, 1095146.8917508761}
      #endregion
    }

    public void Transform3D()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Transform3D(ArcGIS.Core.Geometry.Coordinate3D[],ArcGIS.Core.Geometry.ProjectionTransformation,ArcGIS.Core.Geometry.Coordinate3D[]@,System.Boolean)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Transform3D(ArcGIS.Core.Geometry.Coordinate3D[],ArcGIS.Core.Geometry.ProjectionTransformation,ArcGIS.Core.Geometry.Coordinate3D[]@,System.Boolean)
      #region Transform3D

      // Not all of the input points are transformed as some of them are outside of the GCS horizon.
      Coordinate3D[] inCoords3D = new Coordinate3D[]
      {
        new Coordinate3D(-1, -1, 0),
        new Coordinate3D(-2, -5, 1),
        new Coordinate3D(-5, -11, 2),
        new Coordinate3D(-10, -19, 3),
        new Coordinate3D(-17, -29, 4),
        new Coordinate3D(-26, -41, 5),
        new Coordinate3D(-37, -5, 6),
        new Coordinate3D(-50, -21, 7),
        new Coordinate3D(-65, -39, 8),
        new Coordinate3D(-82, -9, 9)
      };

      int arraySize = inCoords3D.Length;

      ProjectionTransformation projTrans = ProjectionTransformation.Create(SpatialReferences.WGS84, SpatialReferenceBuilder.CreateSpatialReference(24891));

      Coordinate3D[] outCoords3D = new Coordinate3D[arraySize];

      // transform and choose to remove the clipped coordinates
      int numPointsTransformed = GeometryEngine.Instance.Transform3D(inCoords3D, projTrans, ref outCoords3D);

      // numPointsTransformed = 4
      // outCoords2D.Length = 4

      // outCoords2D[0] = {5580417.6876455201, 1328841.2376554986, 7}
      // outCoords2D[1] = {3508774.290814558, -568027.23444226268, 8}
      // outCoords2D[2] = {1568096.0886155984, -2343435.4394415971, 9}
      // outCoords2D[3] = {57325.827391741652, 1095146.8917508761, 10}

      #endregion
    }

    public void UnionMapPoints()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Union(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Union(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Geometry)
      #region Union two MapPoints - creates a Multipoint

      MapPoint pt1 = MapPointBuilderEx.CreateMapPoint(1.0, 1.0);
      MapPoint pt2 = MapPointBuilderEx.CreateMapPoint(2.0, 2.5);

      Geometry geometry = GeometryEngine.Instance.Union(pt1, pt2);
      Multipoint multipoint = geometry as Multipoint;   // multipoint has point count of 2
      #endregion
    }

    public void UnionPolygons()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Union(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Union(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Geometry)
      #region Union two Polygons

      // union two polygons

      List<MapPoint> polyPts = new List<MapPoint>();
      polyPts.Add(MapPointBuilderEx.CreateMapPoint(3.0, 2.0));
      polyPts.Add(MapPointBuilderEx.CreateMapPoint(3.0, 6.0));
      polyPts.Add(MapPointBuilderEx.CreateMapPoint(6.0, 6.0));
      polyPts.Add(MapPointBuilderEx.CreateMapPoint(6.0, 2.0));

      Polygon poly1 = PolygonBuilderEx.CreatePolygon(polyPts);
      bool isSimple = GeometryEngine.Instance.IsSimpleAsFeature(poly1);

      Envelope env = EnvelopeBuilderEx.CreateEnvelope(MapPointBuilderEx.CreateMapPoint(4.0, 4.0), MapPointBuilderEx.CreateMapPoint(8, 8));
      Polygon poly2 = PolygonBuilderEx.CreatePolygon(env);
      isSimple = GeometryEngine.Instance.IsSimpleAsFeature(poly2);

      Geometry g = GeometryEngine.Instance.Union(poly1, poly2);
      Polygon polyResult = g as Polygon;
      #endregion
    }

    public void UnionManyPolylines()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Union(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Geometry})
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Union(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Geometry})
      #region Union many Polylines

      // union many polylines

      List<Coordinate2D> coords = new List<Coordinate2D>()
      {
        new Coordinate2D(1, 2), new Coordinate2D(3, 4), new Coordinate2D(4, 2),
        new Coordinate2D(5, 6), new Coordinate2D(7, 8), new Coordinate2D(8, 4),
        new Coordinate2D(9, 10), new Coordinate2D(11, 12), new Coordinate2D(12, 8),
        new Coordinate2D(10, 8), new Coordinate2D(12, 12), new Coordinate2D(14, 10)
      };

      // create Disjoint lines
      List<Polyline> manyLines = new List<Polyline>
      {
        PolylineBuilderEx.CreatePolyline(new List<Coordinate2D>(){coords[0], coords[1], coords[2]}, SpatialReferences.WGS84),
        PolylineBuilderEx.CreatePolyline(new List<Coordinate2D>(){coords[3], coords[4], coords[5]}),
        PolylineBuilderEx.CreatePolyline(new List<Coordinate2D>(){coords[6], coords[7], coords[8]})
      };

      Polyline polyline = GeometryEngine.Instance.Union(manyLines) as Polyline;
      #endregion
    }

    public void UnionManyPolygons()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Union(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Geometry})
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Union(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Geometry})
      #region Union many Polygons

      // union many polygons

      List<Coordinate3D> coordsZ = new List<Coordinate3D>()
      {
        new Coordinate3D(1, 2, 0), new Coordinate3D(3, 4, 1), new Coordinate3D(4, 2, 2),
        new Coordinate3D(5, 6, 3), new Coordinate3D(7, 8, 4), new Coordinate3D(8, 4, 5),
        new Coordinate3D(9, 10, 6), new Coordinate3D(11, 12, 7), new Coordinate3D(12, 8, 8),
        new Coordinate3D(10, 8, 9), new Coordinate3D(12, 12, 10), new Coordinate3D(14, 10, 11)
      };

      // create polygons
      List<Polygon> manyPolygonsZ = new List<Polygon>
      {
        PolygonBuilderEx.CreatePolygon(new List<Coordinate3D>(){coordsZ[0], coordsZ[1], coordsZ[2]}, SpatialReferences.WGS84),
        PolygonBuilderEx.CreatePolygon(new List<Coordinate3D>(){coordsZ[3], coordsZ[4], coordsZ[5]}),
        PolygonBuilderEx.CreatePolygon(new List<Coordinate3D>(){coordsZ[6], coordsZ[7], coordsZ[8]})
      };

      Polygon polygon = GeometryEngine.Instance.Union(manyPolygonsZ) as Polygon;
      #endregion
    }

    public void Within()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Within(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Within(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.Geometry)
      #region MapPoints, Polylines, Polygons within Polygon

      // build a polygon      
      List<MapPoint> pts = new List<MapPoint>();
      pts.Add(MapPointBuilderEx.CreateMapPoint(1.0, 1.0));
      pts.Add(MapPointBuilderEx.CreateMapPoint(1.0, 2.0));
      pts.Add(MapPointBuilderEx.CreateMapPoint(2.0, 2.0));
      pts.Add(MapPointBuilderEx.CreateMapPoint(2.0, 1.0));

      Polygon poly = PolygonBuilderEx.CreatePolygon(pts);

      // an inner point
      MapPoint innerPt = MapPointBuilderEx.CreateMapPoint(1.5, 1.5);
      bool within = GeometryEngine.Instance.Within(innerPt, poly);   // within = true

      // point on a boundary
      within = GeometryEngine.Instance.Within(pts[0], poly);     // within = false

      // an interior line
      MapPoint innerPt2 = MapPointBuilderEx.CreateMapPoint(1.25, 1.75);
      List<MapPoint> innerLinePts = new List<MapPoint>();
      innerLinePts.Add(innerPt);
      innerLinePts.Add(innerPt2);

      Polyline polyline = PolylineBuilderEx.CreatePolyline(innerLinePts);
      within = GeometryEngine.Instance.Within(polyline, poly);   // within = true

      // a line that crosses the boundary
      MapPoint outerPt = MapPointBuilderEx.CreateMapPoint(3, 1.5);
      List<MapPoint> crossingLinePts = new List<MapPoint>();
      crossingLinePts.Add(innerPt);
      crossingLinePts.Add(outerPt);

      polyline = PolylineBuilderEx.CreatePolyline(crossingLinePts);
      within = GeometryEngine.Instance.Within(polyline, poly);     // within = false


      // polygon in polygon
      Envelope env = EnvelopeBuilderEx.CreateEnvelope(innerPt, innerPt2);
      within = GeometryEngine.Instance.Within(env, poly);      // within = true
      #endregion
    }

  }
}
