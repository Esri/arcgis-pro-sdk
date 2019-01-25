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
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;

namespace ProSnippetsGeometry {
    class GeometryCodeExamples {

      public void SpatialReference()
      {
      #region Construct a SpatialReference - from a well-known ID

      // methods need to run on the MCT
      ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // use the builder constructor
          SpatialReference sr3857 = null;
          using (SpatialReferenceBuilder srBuilder = new SpatialReferenceBuilder(3857))
            sr3857 = srBuilder.ToSpatialReference();

          // or use the convenience method
          SpatialReference sr_3857 = SpatialReferenceBuilder.CreateSpatialReference(3857);
        });
        #endregion

        #region Construct a SpatialReference - from a string

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          string wkt = "GEOGCS[\"MyGCS84\",DATUM[\"D_WGS_1984\",SPHEROID[\"WGS_1984\",6378137.0,298.257223563]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Radian\",1.0]]";

          // use the builder constructor
          using (SpatialReferenceBuilder builder = new SpatialReferenceBuilder(wkt))
          {
            SpatialReference sr = builder.ToSpatialReference();
          }

          // or use the convenience method
          SpatialReference anotherSR = SpatialReferenceBuilder.CreateSpatialReference(wkt);
        });
        #endregion

        #region Use WGS84 SpatialReference

        SpatialReference wgs84 = SpatialReferences.WGS84;
        bool isProjected = wgs84.IsProjected;     // false
        bool isGeographic = wgs84.IsGeographic;   // true

      #endregion

      #region Construct a SpatialReference with a vertical coordinate system - from well-known IDs

      // see a list of vertical coordinate systems at http://resources.arcgis.com/en/help/arcgis-rest-api/index.html#/Vertical_coordinate_systems/02r3000000rn000000/

      // methods need to run on the MCT
      ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // use the builder constructor
          // 4326 = GCS_WGS_1984
          // 115700 = vertical WGS_1984
          using (SpatialReferenceBuilder sb = new SpatialReferenceBuilder(4326, 115700))
          {
            SpatialReference sr = sb.ToSpatialReference();

            // spatialReferenceBuilder properties
            int wkid = sb.Wkid;             // wkid = 4326
            string wkt = sb.Wkt;          
            string name = sb.Name;          // name = GCS_WGS_1984
            int vcsWkid = sb.VcsWkid;       // vcsWkid = 115700
            string vcsWkt = sb.VcsWkt;
          }

          // or use the convenience method
          SpatialReference sr_4326 = SpatialReferenceBuilder.CreateSpatialReference(4326, 115700);
        });

        #endregion

        #region Construct a SpatialReference with a vertical coordinate system - from a string 

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // custom VCS - use vertical shift of -1.23 instead of 0
          string custom_vWkt = @"VERTCS[""SHD_height"",VDATUM[""Singapore_Height_Datum""],PARAMETER[""Vertical_Shift"",-1.23],PARAMETER[""Direction"",-1.0],UNIT[""Meter"",1.0]]";

          using (SpatialReferenceBuilder sb = new SpatialReferenceBuilder(4326, custom_vWkt))
          {
            SpatialReference sr = sb.ToSpatialReference();

            int wkid = sr.Wkid;             // wkid = 4326
            int vert_wkid = sr.VcsWkid;     // vert_wkid = 0
            string vert_wkt = sr.VcsWkt;    // vert_wkt = custom_vWkt

            bool hasVcs = sr.HasVcs;        // hasVcs = true
          }

          // or use the convenience method
          SpatialReference sr_4326 = SpatialReferenceBuilder.CreateSpatialReference(4326, custom_vWkt);
        });

      #endregion

        #region Construct a SpatialReference with a custom PCS - from a string
        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // Custom PCS, Predefined GCS
          string wkt = "PROJCS[\"WebMercatorMile\",GEOGCS[\"GCS_WGS_1984\",DATUM[\"D_WGS_1984\",SPHEROID[\"WGS_1984\",6378137.0,298.257223563]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]],PROJECTION[\"Mercator_Auxiliary_Sphere\"],PARAMETER[\"False_Easting\",0.0],PARAMETER[\"False_Northing\",0.0],PARAMETER[\"Central_Meridian\",0.0],PARAMETER[\"Standard_Parallel_1\",0.0],PARAMETER[\"Auxiliary_Sphere_Type\",0.0],UNIT[\"Mile\",1609.344000614692]]";

          SpatialReference sr = SpatialReferenceBuilder.CreateSpatialReference(wkt);
          // sr.Wkt = wkt
          // sr.Wkid = 0
          // sr.VcsWkid = 0
          // sr.GcsWkid = 4326

          var gcs = sr.Gcs;
          // gcs.Wkid = 4326
          // gcs.IsGeographic = true
          // sr.GcsWkt = gcs.Wkt
        });
        #endregion

        #region SpatialReference Properties

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // use the builder constructor
          using (SpatialReferenceBuilder srBuilder = new SpatialReferenceBuilder(3857))
          {
            // spatialReferenceBuilder properties
            int wkid = srBuilder.Wkid;
            string wkt = srBuilder.Wkt;
            string name = srBuilder.Name;

            double xyScale = srBuilder.XYScale;
            double xyTolerance = srBuilder.XYTolerance;
            double xyResolution = srBuilder.XYResolution;
            Unit unit = srBuilder.Unit;

            double zScale = srBuilder.ZScale;
            double zTolerance = srBuilder.ZTolerance;
            Unit zUnit = srBuilder.ZUnit;

            double mScale = srBuilder.MScale;
            double mTolerance = srBuilder.MTolerance;

            double falseX = srBuilder.FalseX;
            double falseY = srBuilder.FalseY;
            double falseZ = srBuilder.FalseZ;
            double falseM = srBuilder.FalseM;

            // get the spatial reference
            SpatialReference sr3857 = srBuilder.ToSpatialReference();

            // spatial reference properties
            wkid = sr3857.Wkid;
            wkt = sr3857.Wkt;
            name = sr3857.Name;

            xyScale = sr3857.XYScale;
            xyTolerance = sr3857.XYTolerance;
            xyResolution = sr3857.XYResolution;
            unit = sr3857.Unit;

            zScale = sr3857.ZScale;
            zTolerance = sr3857.ZTolerance;
            zUnit = sr3857.ZUnit;

            mScale = sr3857.MScale;
            mTolerance = sr3857.MTolerance;

            falseX = sr3857.FalseX;
            falseY = sr3857.FalseY;
            falseZ = sr3857.FalseZ;
            falseM = sr3857.FalseM;

            bool hasVcs = sr3857.HasVcs;
          }
        });

        #endregion

        #region Import and Export Spatial Reference

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          using (SpatialReferenceBuilder sb = new SpatialReferenceBuilder(4326, 6916))
          {
            SpatialReference sr = sb.ToSpatialReference();

            string xml = sr.ToXML();
            SpatialReference importedSR = SpatialReferenceBuilder.FromXML(xml);
            // importedSR.Wkid = 4326
            // importedSR.VcsWkid = 6916

            string json = sr.ToJson();
            importedSR = SpatialReferenceBuilder.FromJson(json);
            // importedSR.Wkid = 4326
            // importedSR.VcsWkid = 6916
          }
        });

      #endregion

        #region Determine grid convergence for a SpatialReference at a given point

        Coordinate2D coordinate = new Coordinate2D(10, 30);
        double angle = SpatialReferences.WGS84.ConvergenceAngle(coordinate);
        // angle = 0


        SpatialReference srUTM30N = SpatialReferenceBuilder.CreateSpatialReference(32630);
        coordinate.X = 500000;
        coordinate.Y = 550000;
        angle = srUTM30N.ConvergenceAngle(coordinate);
        // angle = 0

        MapPoint pointWGS84 = MapPointBuilder.CreateMapPoint(10, 50, SpatialReferences.WGS84);
        MapPoint pointUTM30N = GeometryEngine.Instance.Project(pointWGS84, srUTM30N) as MapPoint;

        coordinate = (Coordinate2D)pointUTM30N;
        // get convergence angle and convert to degrees
        angle = srUTM30N.ConvergenceAngle(coordinate) * 180 / Math.PI;
            // angle = 10.03

        #endregion
        #region Datum
        var cimMapDefinition = MapView.Active.Map.GetDefinition();
        // use if map's sr does not have a vertical coordinate system
        var datumTransformations = cimMapDefinition.DatumTransforms;
        // use if map's sr has a vertical coordinate system
        var hvDatumTransformations = cimMapDefinition.HVDatumTransforms;  
        #endregion

        #region SpatialReference Datum and datum properties

            // methods need to run on the MCT
            ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // get datum of a spatial reference
          SpatialReference sr = SpatialReferences.WGS84;
          Datum datum = sr.Datum;
          // datum.Name = "D_WGS_1984"
          // datum.Wkid = 6326
          // datum.SpheroidName = "WGS_1984"
          // datum.SpheroidWkid = 7030
          // datum.SpheroidFlattening = 0.0033528106647474805
          // datum.SpheroidSemiMajorAxis = 6378137.0
          // datum.SpheroidSemiMinorAxis = 6356752.3142451793
          
          // Custom WKT
          string wkt = "PROJCS[\"Wyoming_State_Pl_NAD_1927\",GEOGCS[\"GCS_North_American_1927\",DATUM[\"D_North_American_1927_Perry\",SPHEROID[\"Clarke_1866_Chase\",6378210.0,250.0]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]],PROJECTION[\"Transverse_Mercator\"],PARAMETER[\"false_easting\",500000.0],PARAMETER[\"false_northing\",0.0],PARAMETER[\"central_meridian\",-107.3333333],PARAMETER[\"scale_factor\",0.9999412],PARAMETER[\"latitude_of_origin\",40.66666667],UNIT[\"Foot_US\",0.3048006096012192]]";
          sr = SpatialReferenceBuilder.CreateSpatialReference(wkt);
          datum = sr.Datum;

          // datum.Name = "D_North_American_1927_Perry"
          // datum.Wkid = 0
          // datum.SpheroidName = "Clarke_1866_Chase"
          // datum.SpheroidWkid = 0
          // datum.SpheroidFlattening = 0.004
          // datum.SpheroidSemiMajorAxis = 6378210.0
          // datum.SpheroidSemiMinorAxis = 6352697.16
        });

        #endregion

      }

      public void Coordinate3D()
      {
        #region Vector Polar Coordinates
        Coordinate3D polarVector = new Coordinate3D(0, 7, 0);
        Tuple<double, double, double> polarComponents = polarVector.QueryPolarComponents();
        // polarComponents.Item1 = 0  (azimuth)
        // polarComponents.Item2 = 0 (inclination)
        // polarComponents.Item3 = 7 (magnitude)

        polarVector.SetPolarComponents(Math.PI / 4, Math.PI / 2, 8);
        polarComponents = polarVector.QueryComponents();
        // polarComponents.Item1 = 0 (x)
        // polarComponents.Item2 = 0 (y)
        // polarComponents.Item3 = 7 (z)

        #endregion

        #region Getting vector inclination

        Coordinate3D v = new Coordinate3D(0, 0, 7);
        double inclination = v.Inclination;         // inclination = PI/2

        v.SetComponents(-2, -3, 0);
        inclination = v.Inclination;                // inclination = 0

        v.SetComponents(0, 0, -2);
        inclination = v.Inclination;                // inclination = -PI/2

        #endregion

        #region Getting vector azimuth
        Coordinate3D vector = new Coordinate3D(0, 7, 0);
        double azimuth = vector.Azimuth;      // azimuth = 0

        vector.SetComponents(1, 1, 42);
        azimuth = vector.Azimuth;
        double degrees = AngularUnit.Degrees.ConvertFromRadians(azimuth);       // degrees = 45

        vector.SetComponents(-8, 8, 2);
        azimuth = vector.Azimuth;
        degrees = AngularUnit.Degrees.ConvertFromRadians(azimuth);              // degrees = 315

        #endregion
      }

      public void Coordinate3D_Operations()
      {
        #region Vector Operations

        // Easy 3D vectors
        Coordinate3D v = new Coordinate3D(0, 1, 0);
        // v.Magnitude = 1

        Coordinate3D other = new Coordinate3D(-1, 0, 0);
        // other.Magnitude = -1

        double dotProduct = v.DotProduct(other);      // dotProduct = 0

        Coordinate3D crossProduct = v.CrossProduct(other);
        // crossProduct.X = 0
        // crossProduct.Y = 0
        // crossProduct.Z = 1

        Coordinate3D addVector = v.AddCoordinate3D(other);
        // addVector.X = -1
        // addVector.Y = 1
        // addVector.Z = 0

        // Rotate around x-axis
        Coordinate3D w = v;
        w.Rotate(Math.PI, other);
        // w.X = 0
        // w.Y = -1
        // w.Z = 0

        w.Scale(0.5);
        // w.X = 0
        // w.Y = -0.5
        // w.Z = 0

        w.Scale(-4);
        // w.X = 0
        // ww.Y = 2
        // w.Z = 0
        // w.Magnitude = 2

        w.Move(3, 2, 0);
        // w.X = 3
        // w.Y = 4
        // w.Z = 0
        // w.Magnitude = 5
        #endregion
      }

      public void Builders()
      {
        #region Builder Properties

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // list of points
          List<MapPoint> points = new List<MapPoint> 
          { 
            MapPointBuilder.CreateMapPoint(0, 0, 2, 3, 1), 
            MapPointBuilder.CreateMapPoint(1, 1, 5, 6),
            MapPointBuilder.CreateMapPoint(2, 1, 6),
            MapPointBuilder.CreateMapPoint(0, 0)
          };

          // will have attributes because it is created with convenience method
          Polyline polylineWithAttrs = PolylineBuilder.CreatePolyline(points);

          bool hasZ = polylineWithAttrs.HasZ;          // hasZ = true
          bool hasM = polylineWithAttrs.HasM;          // hasM = true
          bool hasID = polylineWithAttrs.HasID;        // hasID = true

          // will not have attributes because it is passed something other than an attributed polyline
          using (PolylineBuilder polylineB = new PolylineBuilder(points))
          {
            hasZ = polylineB.HasZ;                      // hasZ = false
            hasM = polylineB.HasM;                      // hasM = false
            hasID = polylineB.HasID;                    // hasID = false
          }

          // will have attributes because it is passed an attributed polyline
          using (PolylineBuilder polylineB = new PolylineBuilder(polylineWithAttrs))
          {
            hasZ = polylineB.HasZ;                      // hasZ = true
            hasM = polylineB.HasM;                      // hasM = true
            hasID = polylineB.HasID;                    // hasID = true
          }

          // will have attributes because it is created with convenience method
          Polygon polygonWithAttrs = PolygonBuilder.CreatePolygon(points);
          hasZ = polygonWithAttrs.HasZ;               // hasZ = true
          hasM = polygonWithAttrs.HasM;               // hasM = true
          hasID = polygonWithAttrs.HasID;             // hasID = true

          // will not have attributes because it is passed something other than an attributed polygon
          using (PolygonBuilder polygonB = new PolygonBuilder(points))
          {
            hasZ = polygonB.HasZ;                       // hasZ = false
            hasM = polygonB.HasM;                       // hasM = false
            hasID = polygonB.HasID;                     // hasID = false
          }

          // will have attributes because it is passed an attributed polygon
          using (PolygonBuilder polygonB = new PolygonBuilder(polygonWithAttrs))
          {
            hasZ = polygonB.HasZ;                       // hasZ = true
            hasM = polygonB.HasM;                       // hasM = true
            hasID = polygonB.HasID;                     // hasID = true
          }
        });

        #endregion
      }

      public void MapPoint()
      {
        #region Construct a MapPoint

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // create a 3d point with M
          // use the builder constructor
          using (MapPointBuilder mb = new MapPointBuilder(1.0, 2.0, 3.0, 4.0))
          {
            MapPoint ptWithM = mb.ToGeometry();

            MapPoint clone = ptWithM.Clone() as MapPoint;

            // or use the convenience methods
            MapPoint anotherPt = MapPointBuilder.CreateMapPoint(1.0, 2.0, 3.0, 4.0);

            MapPoint anotherM = MapPointBuilder.CreateMapPoint(ptWithM);
          }
        });

        #endregion

        #region MapPoint Builder Properties

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          using (MapPointBuilder mb = new MapPointBuilder(1.0, 2.0, 3.0))
          {
            bool hasZ = mb.HasZ;          // hasZ = true
            bool hasM = mb.HasM;          // hasM = false
            bool hasID = mb.HasID;        // hasID = false

            MapPoint pt = mb.ToGeometry();
            double x = pt.X;                  // x = 1.0
            double y = pt.Y;                  // y = 2.0
            double z = pt.Z;                  // z = 3.0
            double m = pt.M;                  // m = Nan
            int ID = pt.ID;                   // ID = 0
            hasZ = pt.HasZ;                   // hasZ = true
            hasM = pt.HasM;                   // hasM = false
            hasID = pt.HasID;                 // hasID = false
            bool isEmpty = pt.IsEmpty;        // isEmpty = false

            MapPoint pt2 = MapPointBuilder.CreateMapPoint(pt);
            x = pt.X;                   // x = 1.0
            y = pt.Y;                   // y = 2.0
            z = pt.Z;                   // z = 3.0
            m = pt.M;                   // m = Nan
            hasZ = pt2.HasZ;            // hasZ = true
            hasM = pt2.HasM;            // hasM = false
            hasID = pt2.HasID;          // hasID = false
          }
        });

        #endregion

        #region MapPoint IsEqual

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          MapPoint pt = MapPointBuilder.CreateMapPoint(1, 2, 3, 4, 5);
          int ID = pt.ID;           // ID = 5
          bool hasID = pt.HasID;    // hasID = true

          MapPoint pt2 = MapPointBuilder.CreateMapPoint(1, 2, 3, 4, 0);
          ID = pt2.ID;              // ID = 0
          hasID = pt2.HasID;        // hasID = true

          MapPoint pt3 = MapPointBuilder.CreateMapPoint(1, 2, 3, 4);
          ID = pt3.ID;              // ID = 0
          hasID = pt3.HasID;        // hasID = false

          MapPoint pt4 = MapPointBuilder.CreateMapPoint(1, 2, 3, 44);
          ID = pt4.ID;              // ID = 0
          hasID = pt4.HasID;        // hasID = false
          bool hasM = pt4.HasM;     // hasM = true

          MapPoint pt5 = MapPointBuilder.CreateMapPoint(1, 2, 3);
          ID = pt5.ID;              // ID = 0
          hasID = pt5.HasID;        // hasID = false
          hasM = pt5.HasM;          // hasM = false

          bool isEqual = pt.IsEqual(pt2);     // isEqual = false      // different IDs
          isEqual = pt2.IsEqual(pt3);         // isEqual = false      // HasId is different
          isEqual = pt4.IsEqual(pt3);         // isEqual = false      // different Ms
          isEqual = pt.IsEqual(pt5);          // isEqual = false      // pt has M, id but pt5 does not.  
        });

        #endregion
      }

      private static void ZoomToGeographicCoordinates(double x, double y, double buffer_size)
      {
          QueuedTask.Run(() => {
              #region Zoom to a specified point
              //Note: Run within QueuedTask
              //Create a point
              var pt = MapPointBuilder.CreateMapPoint(x, y, SpatialReferenceBuilder.CreateSpatialReference(4326));
              //Buffer it - for purpose of zoom
              var poly = GeometryEngine.Instance.Buffer(pt, buffer_size);

              //do we need to project the buffer polygon?

              if (!MapView.Active.Map.SpatialReference.IsEqual(poly.SpatialReference))
              {
                  //project the polygon
                  poly = GeometryEngine.Instance.Project(poly, MapView.Active.Map.SpatialReference);
              }

              //Zoom - add in a delay for animation effect
              MapView.Active.ZoomTo(poly, new TimeSpan(0, 0, 0, 3));
              #endregion
          });
      }

      public void Polyline()
      {
        #region Construct a Polyline - from an enumeration of MapPoints

        // methods need to run on MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          MapPoint startPt = MapPointBuilder.CreateMapPoint(1.0, 1.0);
          MapPoint endPt = MapPointBuilder.CreateMapPoint(2.0, 1.0);

          List<MapPoint> list = new List<MapPoint>();
          list.Add(startPt);
          list.Add(endPt);

          // use the builder constructor
          using (PolylineBuilder pb = new PolylineBuilder(list))
          {
            pb.SpatialReference = SpatialReferences.WGS84;
            Polyline polyline1 = pb.ToGeometry();
          }

          // or use the convenience method
          Polyline polyline2 = PolylineBuilder.CreatePolyline(list, SpatialReferences.WGS84);
        });
        #endregion

        Polyline polyline = null;
        #region Get the points of a Polyline

        // get the points as a readonly Collection
        ReadOnlyPointCollection pts = polyline.Points;
        int numPts = polyline.PointCount;

        // OR   get an enumeration of the points
        IEnumerator<MapPoint> enumPts = polyline.Points.GetEnumerator();

        // OR   get the point coordinates as a readonly list of Coordinate2D
        IReadOnlyList<Coordinate2D> coordinates = polyline.Copy2DCoordinatesToList();

        // OR   get the point coordinates as a readonly list of Coordinate3D
        IReadOnlyList<Coordinate3D> coordinates3D = polyline.Copy3DCoordinatesToList();

        // OR   get a subset of the collection as Coordinate2D using preallocated memory

        IList<Coordinate2D> coordinate2Ds = new List<Coordinate2D>(10);   // allocate some space
        ICollection<Coordinate2D> subsetCoordinates2D = coordinate2Ds;    // assign
        pts.Copy2DCoordinatesToList(1, 2, ref subsetCoordinates2D);       // copy 2 elements from index 1 into the allocated list
        // coordinate2Ds.Count = 2
        // do something with the coordinate2Ds

        // without allocating more space, obtain a different set of coordinates
        pts.Copy2DCoordinatesToList(5, 9, ref subsetCoordinates2D);       // copy 9 elements from index 5 into the allocated list
        // coordinate2Ds.Count = 9
    

        // OR   get a subset of the collection as Coordinate3D using preallocated memory

        IList<Coordinate3D> coordinate3Ds = new List<Coordinate3D>(15);   // allocate some space
        ICollection<Coordinate3D> subsetCoordinates3D = coordinate3Ds;    // assign
        pts.Copy3DCoordinatesToList(3, 5, ref subsetCoordinates3D);       // copy 5 elements from index 3 into the allocated list
        // coordinate3Ds.Count = 5


        // OR   get a subset of the collection as MapPoint using preallocated memory

        IList<MapPoint> mapPoints = new List<MapPoint>(7);   // allocate some space
        ICollection<MapPoint> subsetMapPoint = mapPoints;    // assign
        pts.CopyPointsToList(1, 4, ref subsetMapPoint);      // copy 4 elements from index 1 into the allocated list
        // mapPoints.Count = 4
        
        #endregion

        #region Get the parts of a Polyline

        int numParts = polyline.PartCount;
        // get the parts as a readonly collection
        ReadOnlyPartCollection parts = polyline.Parts;

        #endregion

        #region Enumerate the parts of a Polyline

        ReadOnlyPartCollection polylineParts = polyline.Parts;

        // enumerate the segments to get the length
        double len = 0;
        IEnumerator<ReadOnlySegmentCollection> segments = polylineParts.GetEnumerator();
        while (segments.MoveNext())
        {
          ReadOnlySegmentCollection seg = segments.Current;
          foreach (Segment s in seg)
          {
            len += s.Length;

            // perhaps do something specific per segment type
            switch (s.SegmentType)
            {
              case SegmentType.Line:
                break;
              case SegmentType.Bezier:
                break;
              case SegmentType.EllipticArc:
                break;
            }
          }
        }
        
        // or use foreach pattern
        foreach (var part in polyline.Parts)
        {
          foreach (var segment in part)
          {
           len += segment.Length;

            // perhaps do something specific per segment type
            switch (segment.SegmentType)
            {
              case SegmentType.Line:
                break;
              case SegmentType.Bezier:
                break;
              case SegmentType.EllipticArc:
                break;
            }
          }
        }
        #endregion

        #region Reverse the order of points in a Polyline

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          using (PolylineBuilder polylineBuilder = new PolylineBuilder(polyline))
          {
            polylineBuilder.ReverseOrientation();
            Polyline reversedPolyline = polylineBuilder.ToGeometry();
          }
        });
        #endregion

        #region Build a multi-part Polyline

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          List<MapPoint> firstPoints = new List<MapPoint>();
          firstPoints.Add(MapPointBuilder.CreateMapPoint(1.0, 1.0));
          firstPoints.Add(MapPointBuilder.CreateMapPoint(1.0, 2.0));
          firstPoints.Add(MapPointBuilder.CreateMapPoint(2.0, 2.0));
          firstPoints.Add(MapPointBuilder.CreateMapPoint(2.0, 1.0));

          List<MapPoint> nextPoints = new List<MapPoint>();
          nextPoints.Add(MapPointBuilder.CreateMapPoint(11.0, 1.0));
          nextPoints.Add(MapPointBuilder.CreateMapPoint(11.0, 2.0));
          nextPoints.Add(MapPointBuilder.CreateMapPoint(12.0, 2.0));
          nextPoints.Add(MapPointBuilder.CreateMapPoint(12.0, 1.0));

          using (PolylineBuilder pBuilder = new PolylineBuilder(firstPoints))
          {
            pBuilder.AddPart(nextPoints);

            Polyline p = pBuilder.ToGeometry();
            // polyline p has 2 parts

            pBuilder.RemovePart(0);
            p = pBuilder.ToGeometry();
            // polyline p has 1 part
          }
        });

        #endregion
            
        Geometry sketchGeometry = null;
        #region StartPoint of a Polyline
        //Method 1: Get the start point of the polyline by converting the polyline into a collection of points and getting the first point
        //sketchGeometry is a Polyline
        //get the sketch as a point collection
        var pointCol = ((Multipart)sketchGeometry).Points;
        //Get the start point of the line
        var firstPoint = pointCol[0];

        //Method 2: Convert polyline into a collection of linesegments and getting the "StartPoint" of the first line segment.
        var polylineGeom = sketchGeometry as ArcGIS.Core.Geometry.Polyline;
        var polyLineParts = polylineGeom.Parts;

        ReadOnlySegmentCollection polylineSegments = polyLineParts.First();

        //get the first segment as a LineSegment
        var firsLineSegment = polylineSegments.First() as LineSegment;

        //Now get the start Point
        var startPoint = firsLineSegment.StartPoint;

        #endregion
      }

      public void Clothoid()
      {
        #region Construct a Clothoid by Angle

        // methods need to run on MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          SpatialReference sr = SpatialReferences.WGS84;
          MapPoint startPoint = MapPointBuilder.CreateMapPoint(0, 0, sr);

          double tangentDirection = Math.PI / 6;
          esriArcOrientation orientation = esriArcOrientation.esriArcCounterClockwise;
          double startRadius = double.PositiveInfinity;
          double endRadius = 0.2;
          esriClothoidCreateMethod createMethod = esriClothoidCreateMethod.ByAngle;
          double angle = Math.PI / 2;
          esriCurveDensifyMethod densifyMethod = esriCurveDensifyMethod.ByLength;
          double densifyParameter = 0.1;

          Polyline polyline = PolylineBuilder.CreatePolyline(startPoint, tangentDirection, startRadius, endRadius, orientation, createMethod, angle, densifyMethod, densifyParameter, sr);

          int numPoints = polyline.PointCount;
          MapPoint queryPoint = polyline.Points[numPoints - 2];

          MapPoint pointOnPath;
          double radiusCalculated, tangentDirectionCalculated, lengthCalculated, angleCalculated;

          PolylineBuilder.QueryClothoidParameters(queryPoint, startPoint, tangentDirection, startRadius, endRadius, orientation, createMethod, angle, out pointOnPath, out radiusCalculated, out tangentDirectionCalculated, out lengthCalculated, out angleCalculated, sr);
       
        });

        #endregion

        #region Construct a Clothoid by Length
        // methods need to run on MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          SpatialReference sr = SpatialReferences.WGS84;
          MapPoint startPoint = MapPointBuilder.CreateMapPoint(0, 0, sr);
          MapPoint queryPoint = MapPointBuilder.CreateMapPoint(3.8, 1);
          double tangentDirection = 0;
          esriArcOrientation orientation = esriArcOrientation.esriArcCounterClockwise;
          double startRadius = double.PositiveInfinity;
          double endRadius = 1;
          esriClothoidCreateMethod createMethod = esriClothoidCreateMethod.ByLength;
          double curveLength = 10;

          MapPoint pointOnPath;
          double radiusCalculated, tangentDirectionCalculated, lengthCalculated, angleCalculated;

          PolylineBuilder.QueryClothoidParameters(queryPoint, startPoint, tangentDirection, startRadius, endRadius, orientation, createMethod, curveLength, out pointOnPath, out radiusCalculated, out tangentDirectionCalculated, out lengthCalculated, out angleCalculated, sr);

          // pointOnPath = (3.7652656620171379, 1.0332006103128575)
          // radiusCalculated = 2.4876382887687227
          // tangentDirectionCalculated = 0.80797056423543978
          // lengthCalculated = 4.0198770235802987
          // angleCalculated = 0.80797056423544011

          queryPoint = MapPointBuilder.CreateMapPoint(1.85, 2.6);

          PolylineBuilder.QueryClothoidParameters(queryPoint, startPoint, tangentDirection, startRadius, endRadius, orientation, createMethod, curveLength, out pointOnPath, out radiusCalculated, out tangentDirectionCalculated, out lengthCalculated, out angleCalculated, sr);

          // pointOnPath = (1.8409964973501549, 2.6115979967308132)
          // radiusCalculated = 1
          // tangentDirectionCalculated = -1.2831853071795867
          // lengthCalculated = 10
          // angleCalculated = 5


          tangentDirection = Math.PI / 4;
          orientation = esriArcOrientation.esriArcClockwise;
          startRadius = double.PositiveInfinity;
          endRadius = 0.8;
          createMethod = esriClothoidCreateMethod.ByLength;
          curveLength = 10;

          using (PolylineBuilder pb = new PolylineBuilder(startPoint, tangentDirection, startRadius, endRadius, orientation, createMethod, curveLength, esriCurveDensifyMethod.ByLength, 0.5, sr))
          {
            Polyline polyline = pb.ToGeometry();
          }

        });
        #endregion
      }

      public void SplitPolyline()
      {
        #region Split Polyline at distance

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // create list of points
          MapPoint startPt = MapPointBuilder.CreateMapPoint(1.0, 1.0);
          MapPoint endPt = MapPointBuilder.CreateMapPoint(2.0, 1.0);

          List<MapPoint> list = new List<MapPoint>();
          list.Add(startPt);
          list.Add(endPt);

          // use the PolylineBuilder as we wish to manipulate the geometry
          using (PolylineBuilder polylineBuilder = new PolylineBuilder(list))
          {
            // split at a distance 0.75
            polylineBuilder.SplitAtDistance(0.75, false);
            // get the polyline
            Polyline p = polylineBuilder.ToGeometry();
            // polyline p should have 3 points  (1,1), (1.75, 1), (2,1)

            // add another path
            MapPoint p1 = MapPointBuilder.CreateMapPoint(4.0, 1.0);
            MapPoint p2 = MapPointBuilder.CreateMapPoint(6.0, 1.0);
            MapPoint p3 = MapPointBuilder.CreateMapPoint(7.0, 1.0);
            List<MapPoint> pts = new List<MapPoint>();
            pts.Add(p1);
            pts.Add(p2);
            pts.Add(p3);

            polylineBuilder.AddPart(pts);
            p = polylineBuilder.ToGeometry();

            // polyline p has 2 parts.  Each part has 3 points

            // split the 2nd path half way - dont create a new path
            polylineBuilder.SplitPartAtDistance(1, 0.5, true, false);

            p = polylineBuilder.ToGeometry();

            // polyline p still has 2 parts; but now has 7 points 
          }
        });

        #endregion
      }

      public void ConstructPolygon()
      {
        #region Construct a Polygon - from an enumeration of MapPoints

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          MapPoint pt1 = MapPointBuilder.CreateMapPoint(1.0, 1.0);
          MapPoint pt2 = MapPointBuilder.CreateMapPoint(1.0, 2.0);
          MapPoint pt3 = MapPointBuilder.CreateMapPoint(2.0, 2.0);
          MapPoint pt4 = MapPointBuilder.CreateMapPoint(2.0, 1.0);

          List<MapPoint> list = new List<MapPoint>();
          list.Add(pt1);
          list.Add(pt2);
          list.Add(pt3);
          list.Add(pt4);

          // use the builder constructor
          using (PolygonBuilder pb = new PolygonBuilder(list))
          {
            pb.SpatialReference = SpatialReferences.WGS84;
            Polygon polygon = pb.ToGeometry();
          }

          // or use the convenience method
          Polygon anotherPolygon = PolygonBuilder.CreatePolygon(list, SpatialReferences.WGS84);
        });

        #endregion

        #region Construct a Polygon - from an Envelope

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          MapPoint minPt = MapPointBuilder.CreateMapPoint(1.0, 1.0);
          MapPoint maxPt = MapPointBuilder.CreateMapPoint(2.0, 2.0);

          // Create an envelope
          Envelope env = EnvelopeBuilder.CreateEnvelope(minPt, maxPt);
          // Create a polygon from the envelope
          Polygon polygon = PolygonBuilder.CreatePolygon(env);
        });

        #endregion Create a Polygon from an Envelope

        Polygon poly = null;
        #region Get the points of a Polygon

        // get the points as a readonly Collection
        ReadOnlyPointCollection pts = poly.Points;

        // get an enumeration of the points
        IEnumerator<MapPoint> enumPts = poly.Points.GetEnumerator();

        // get the point coordinates as a readonly list of Coordinate2D
        IReadOnlyList<Coordinate2D> coordinates = poly.Copy2DCoordinatesToList();

        // get the point coordinates as a readonly list of Coordinate3D
        IReadOnlyList<Coordinate3D> coordinates3D = poly.Copy3DCoordinatesToList();
        #endregion

        #region Get the parts of a Polygon
        // get the parts as a readonly collection
        ReadOnlyPartCollection parts = poly.Parts;

        #endregion

        #region Enumerate the parts of a Polygon

        int numSegments = 0;
        IEnumerator<ReadOnlySegmentCollection> segments = poly.Parts.GetEnumerator();
        while (segments.MoveNext())
        {
          ReadOnlySegmentCollection seg = segments.Current;
          numSegments += seg.Count;
          foreach (Segment s in seg)
          {
            // do something with the segment
          }
        }

        #endregion

        #region Build a donut polygon

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          List<Coordinate2D> outerCoordinates = new List<Coordinate2D>();
          outerCoordinates.Add(new Coordinate2D(10.0, 10.0));
          outerCoordinates.Add(new Coordinate2D(10.0, 20.0));
          outerCoordinates.Add(new Coordinate2D(20.0, 20.0));
          outerCoordinates.Add(new Coordinate2D(20.0, 10.0));

          // use the PolygonBuilder as we wish to manipulate the parts
          using (PolygonBuilder pb = new PolygonBuilder(outerCoordinates))
          {
            Polygon donut = pb.ToGeometry();
            double area = donut.Area;       // area = 100

            // define the inner polygon as anti-clockwise
            List<Coordinate2D> innerCoordinates = new List<Coordinate2D>();
            innerCoordinates.Add(new Coordinate2D(13.0, 13.0));
            innerCoordinates.Add(new Coordinate2D(17.0, 13.0));
            innerCoordinates.Add(new Coordinate2D(17.0, 17.0));
            innerCoordinates.Add(new Coordinate2D(13.0, 17.0));

            pb.AddPart(innerCoordinates);
            donut = pb.ToGeometry();

            area = donut.Area;    // area = 84.0

            area = GeometryEngine.Instance.Area(donut);    // area = 84.0
          }
        });

        #endregion
      }

      public void ConstructEnvelope()
      {
        #region Construct an Envelope 

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          MapPoint minPt = MapPointBuilder.CreateMapPoint(1.0, 1.0);
          MapPoint maxPt = MapPointBuilder.CreateMapPoint(2.0, 2.0);

          using (EnvelopeBuilder ev = new EnvelopeBuilder(minPt, maxPt))
          {
            Envelope env = ev.ToGeometry();
          }
        });

        #endregion

        #region Construct an Envelope - from a JSON string

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          string jsonString = "{ \"xmin\" : 1, \"ymin\" : 2,\"xmax\":3,\"ymax\":4,\"spatialReference\":{\"wkid\":4326}}";

          Envelope envFromJson = EnvelopeBuilder.FromJson(jsonString);
        });

        #endregion

        #region Union two Envelopes

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          Envelope env1 = EnvelopeBuilder.CreateEnvelope(0, 0, 1, 1, SpatialReferences.WGS84);
          Envelope env2 = EnvelopeBuilder.CreateEnvelope(0.5, 0.5, 1.5, 1.5, SpatialReferences.WGS84);

          Envelope env3 = env1.Union(env2);
        });

        #endregion

        #region Expand an Envelope

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          Envelope envelope = EnvelopeBuilder.CreateEnvelope(100.0, 100.0, 500.0, 500.0);

          // shrink the envelope by 50%
          Envelope result = envelope.Expand(0.5, 0.5, true);

          // or use the builder
          using (EnvelopeBuilder eBuilder = new EnvelopeBuilder(100.0, 100.0, 500.0, 500.0))
          {
            // shrink by 50%
            eBuilder.Expand(0.5, 0.5, true);

            result = eBuilder.ToGeometry();
          }
        });
  
        #endregion

        #region Update Coordinates of an Envelope
        
        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          Coordinate2D minCoord = new Coordinate2D(1, 3);
          Coordinate2D maxCoord = new Coordinate2D(2, 4);
          using (EnvelopeBuilder builder = new EnvelopeBuilder(minCoord, maxCoord))
          {
            // builder.XMin, YMin, Zmin, MMin  = 1, 3, 0, double.Nan
            // builder.XMax, YMax, ZMax, MMax = 2, 4, 0, double.Nan

            // set XMin.  if XMin > XMax; both XMin and XMax change
            builder.XMin = 6;
            // builder.XMin, YMin, ZMin, MMin  = 6, 3, 0, double.Nan
            // builder.XMax, YMax, ZMax, MMax = 6, 4, 0, double.Nan

            // set XMax
            builder.XMax = 8;
            // builder.XMin, YMin, ZMin, MMin  = 6, 3, 0, double.Nan
            // builder.XMax, YMax, ZMax, MMax = 8, 4, 0, double.Nan

            // set XMax.  if XMax < XMin, both XMin and XMax change
            builder.XMax = 3;
            // builder.XMin, YMin, ZMin, MMin  = 3, 3, 0, double.Nan
            // builder.XMax, YMax, ZMax, MMax = 3, 4, 0, double.Nan

            // set YMin
            builder.YMin = 2;
            // builder.XMin, YMin, ZMin, MMin  = 3, 2, 0, double.Nan
            // builder.XMax, YMax, ZMax, MMax = 3, 4, 0, double.Nan

            // set ZMin.  if ZMin > ZMax, both ZMin and ZMax change
            builder.ZMin = 3;
            // builder.XMin, YMin, ZMin, MMin  = 3, 2, 3, double.Nan
            // builder.XMax, YMax, ZMax, MMax = 3, 4, 3, double.Nan

            // set ZMax.  if ZMax < ZMin. both ZMin and ZMax change
            builder.ZMax = -1;
            // builder.XMin, YMin, ZMin, MMin  = 3, 2, -1, double.Nan
            // builder.XMax, YMax, ZMax, MMax = 3, 4, -1, double.Nan

            builder.SetZCoords(8, -5);
            // builder.XMin, YMin, ZMin, MMin  = 3, 2, -5, double.Nan
            // builder.XMax, YMax, ZMax, MMax = 3, 4, 8, double.Nan

            Coordinate2D c1 = new Coordinate2D(0, 5);
            Coordinate2D c2 = new Coordinate2D(1, 3);
            builder.SetXYCoords(c1, c2);
            // builder.XMin, YMin, ZMin, MMin  = 0, 3, -5, double.Nan
            // builder.XMax, YMax, ZMax, MMax = 1, 5, 8, double.Nan

          }
        });
        #endregion
      }

      public void ConstructMultiPoint()
      {
        #region Construct a Multipoint - from an enumeration of MapPoints

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          List<MapPoint> list = new List<MapPoint>();
          list.Add(MapPointBuilder.CreateMapPoint(1.0, 1.0));
          list.Add(MapPointBuilder.CreateMapPoint(1.0, 2.0));
          list.Add(MapPointBuilder.CreateMapPoint(2.0, 2.0));
          list.Add(MapPointBuilder.CreateMapPoint(2.0, 1.0));

          // use the builder constructor
          using (MultipointBuilder mpb = new MultipointBuilder(list))
          {
            Multipoint mPt = mpb.ToGeometry();
          }

          // or use the convenience method
          Multipoint multiPoint = MultipointBuilder.CreateMultipoint(list);
        });

        #endregion

        Multipoint multiPt = null;
        #region Modify the points of a Multipoint

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // assume a multiPoint has been built from 4 points
          // the modified multiPoint will have the first point removed and the last point moved

          using (MultipointBuilder mpb = new MultipointBuilder(multiPt))
          {
            // remove the first point
            mpb.RemovePoint(0);

            // modify the coordinates of the last point
            MapPoint pt = mpb.GetMapPoint(mpb.PointCount - 1);
            mpb.RemovePoint(mpb.PointCount - 1);

            MapPoint newPt = MapPointBuilder.CreateMapPoint(pt.X + 1.0, pt.Y + 2.0);
            mpb.Add(newPt);

            Multipoint modifiedMultiPoint = mpb.ToGeometry();
          }
        });

        #endregion

        #region Retrieve Points, 2D Coordinates, 3D Coordinates from a multipoint

        ReadOnlyPointCollection points = multiPt.Points;
        IReadOnlyList<Coordinate2D> coords2d = multiPt.Copy2DCoordinatesToList();
        IReadOnlyList<Coordinate3D> coords3d = multiPt.Copy3DCoordinatesToList();

        #endregion
      }

      public void ConstructLineSegment()
      {
        #region Construct a LineSegment using two MapPoints

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          MapPoint startPt = MapPointBuilder.CreateMapPoint(1.0, 1.0);
          MapPoint endPt = MapPointBuilder.CreateMapPoint(2.0, 1.0);

          // use the builder constructor
          using (LineBuilder lb = new LineBuilder(startPt, endPt))
          {
            LineSegment line = lb.ToSegment();
          }

          // or use the convenience method
          LineSegment anotherLine = LineBuilder.CreateLineSegment(startPt, endPt);
        });
        #endregion
      }

      public void ConstructCubicBezier()
      {
        #region Construct a Cubic Bezier - from Coordinates

        // builders need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          MapPoint startPt = MapPointBuilder.CreateMapPoint(1.0, 1.0, 3.0);
          MapPoint endPt = MapPointBuilder.CreateMapPoint(2.0, 2.0, 3.0);

          Coordinate2D ctrl1Pt = new Coordinate2D(1.0, 2.0);
          Coordinate2D ctrl2Pt = new Coordinate2D(2.0, 1.0);

          // use the builder constructor 
          using (CubicBezierBuilder cbb = new CubicBezierBuilder(startPt, ctrl1Pt, ctrl2Pt, endPt))
          {
            CubicBezierSegment bezier = cbb.ToSegment();
          }

          // or use the convenience method
          CubicBezierSegment anotherBezier = CubicBezierBuilder.CreateCubicBezierSegment(startPt, ctrl1Pt, ctrl2Pt, endPt, SpatialReferences.WGS84);

        });
        #endregion
      }

      public void ConstructCubicBezier2()
      {
        #region Construct a Cubic Bezier - from MapPoints

        // builders need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          MapPoint startPt = MapPointBuilder.CreateMapPoint(1.0, 1.0, SpatialReferences.WGS84);
          MapPoint endPt = MapPointBuilder.CreateMapPoint(2.0, 2.0, SpatialReferences.WGS84);

          MapPoint ctrl1Pt = MapPointBuilder.CreateMapPoint(1.0, 2.0, SpatialReferences.WGS84);
          MapPoint ctrl2Pt = MapPointBuilder.CreateMapPoint(2.0, 1.0, SpatialReferences.WGS84);

          // use the builder constructor
          using (CubicBezierBuilder cbb = new CubicBezierBuilder(startPt, ctrl1Pt, ctrl2Pt, endPt))
          {
            CubicBezierSegment bezier = cbb.ToSegment();
          }

          // or use the convenience method
          CubicBezierSegment anotherBezier = CubicBezierBuilder.CreateCubicBezierSegment(startPt, ctrl1Pt, ctrl2Pt, endPt);
        });
        #endregion
      }

      public void ConstructCubicBezier3()
      {
        #region Construct a Cubic Bezier - from an enumeration of MapPoints

        // builders need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          MapPoint startPt = MapPointBuilder.CreateMapPoint(1.0, 1.0, SpatialReferences.WGS84);
          MapPoint endPt = MapPointBuilder.CreateMapPoint(2.0, 2.0, SpatialReferences.WGS84);

          MapPoint ctrl1Pt = MapPointBuilder.CreateMapPoint(1.0, 2.0, SpatialReferences.WGS84);
          MapPoint ctrl2Pt = MapPointBuilder.CreateMapPoint(2.0, 1.0, SpatialReferences.WGS84);

          List<MapPoint> listMapPoints = new List<MapPoint>();
          listMapPoints.Add(startPt);
          listMapPoints.Add(ctrl1Pt);
          listMapPoints.Add(ctrl2Pt);
          listMapPoints.Add(endPt);

          // use the builder constructor
          using (CubicBezierBuilder cbb = new CubicBezierBuilder(listMapPoints))
          {
            CubicBezierSegment bezier = cbb.ToSegment();
          }

          // or use the convenience method
          CubicBezierSegment anotherBezier = CubicBezierBuilder.CreateCubicBezierSegment(listMapPoints);
        });
        #endregion
      }

      public void ConstructCubicBezierBuilderUtils()
      {
        CubicBezierSegment bezierSegment = null;

        #region Cubic Bezier Builder Properties

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // retrieve the bezier curve's control points
          using (CubicBezierBuilder cbb = new CubicBezierBuilder(bezierSegment))
          {
            MapPoint startPt = cbb.StartPoint;
            Coordinate2D ctrlPt1 = cbb.ControlPoint1;
            Coordinate2D ctrlPt2 = cbb.ControlPoint2;
            MapPoint endPt = cbb.EndPoint;

            // or use the QueryCoords method
            cbb.QueryCoords(out startPt, out ctrlPt1, out ctrlPt2, out endPt);
          }
        });

        #endregion
      }

      public void ConstructCubicBezierUtils()
      {
        CubicBezierSegment bezierSegment = null;

        #region Cubic Bezier Properties

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // retrieve the bezier curve's control points
          CubicBezierSegment cb = CubicBezierBuilder.CreateCubicBezierSegment(bezierSegment);
          MapPoint startPt = cb.StartPoint;
          Coordinate2D ctrlPt1 = cb.ControlPoint1;
          Coordinate2D ctrlPt2 = cb.ControlPoint2;
          MapPoint endPt = cb.EndPoint;

          bool isCurve = cb.IsCurve;
          double len = cb.Length;          
        });

        #endregion

        #region Construct a Polyline (from a Cubic Bezier)

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          Polyline polyline = PolylineBuilder.CreatePolyline(bezierSegment);
        });

        #endregion
      }

      public void ConstructArc()
      {
        #region Construct a Circular Arc - using an interior point

        //  methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // Construct a circular arc from (2, 1) to (1, 2) with interior pt (1 + sqrt(2)/2, 1 + sqrt(2)/2).

          MapPoint fromPt = MapPointBuilder.CreateMapPoint(2, 1);
          MapPoint toPt = MapPointBuilder.CreateMapPoint(1, 2);
          Coordinate2D interiorPt = new Coordinate2D(1 + Math.Sqrt(2) / 2, 1 + Math.Sqrt(2) / 2);

          // use the builder constructor
          using (EllipticArcBuilder cab = new EllipticArcBuilder(fromPt, toPt, interiorPt))
          {
            EllipticArcSegment circularArc = cab.ToSegment();
          }

          // or use the convenience method
          EllipticArcSegment anotherCircularArc = EllipticArcBuilder.CreateEllipticArcSegment(fromPt, toPt, interiorPt);
        });
        #endregion

        #region Construct a Circular Arc - using a chord length and bearing

        //  methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // Construct a circular arc counterclockwise from (2, 1) to (1, 2) such that the embedded 
          // circle has center point at (1, 1) and radius = 1

          MapPoint fromPt = MapPointBuilder.CreateMapPoint(2, 1, SpatialReferences.WGS84);
          double chordLength = Math.Sqrt(2);
          double chordBearing = 3 * Math.PI / 4;
          double radius = 1;
          esriArcOrientation orientation = esriArcOrientation.esriArcCounterClockwise;
          MinorOrMajor minorOrMajor = MinorOrMajor.Minor;

          // use the builder constructor
          using (EllipticArcBuilder cab = new EllipticArcBuilder(fromPt, chordLength, chordBearing, radius, orientation, minorOrMajor))
          {
            EllipticArcSegment circularArc = cab.ToSegment();
          }

          // or use the convenience method
          EllipticArcSegment anotherCircularArc = EllipticArcBuilder.CreateEllipticArcSegment(fromPt, chordLength, chordBearing, radius, orientation, minorOrMajor);
        });
        #endregion

        #region Construct a Circular Arc - using a center point, angle and radius

        //  methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {

          // Construct a circular arc with center point at (0, 0), from angle = 0, 
          // central angle = pi/2, radius = 1

          SpatialReference sr4326 = SpatialReferences.WGS84;

          Coordinate2D centerPt = new Coordinate2D(0, 0);
          double fromAngle = 0;
          double centralAngle = Math.PI / 2;
          double radius = 1;

          // use the builder constructor
          using (EllipticArcBuilder cab = new EllipticArcBuilder(fromAngle, centralAngle, centerPt, radius, sr4326))
          {
            EllipticArcSegment circularArc = cab.ToSegment();
          }

          // or use the convenience method
          EllipticArcSegment anotherCircularArc = EllipticArcBuilder.CreateEllipticArcSegment(fromAngle, centralAngle, centerPt, radius, sr4326);
        });
      #endregion

        #region Construct a Circular Arc - using a center point and rotation angle

        //  methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {

          // Construct an elliptic arc centered at (1,1), startAngle = 0, centralAngle = PI/2, 
          // rotationAngle = 0, semiMajorAxis = 1, minorMajorRatio = 0.5

          Coordinate2D centerPt = new Coordinate2D(1, 1);

          // use the builder constructor
          using (EllipticArcBuilder cab = new EllipticArcBuilder(centerPt, 0, Math.PI / 2, 0, 1, 0.5))
          {
            EllipticArcSegment circularArc = cab.ToSegment();
          }

          // or use the convenience method
          EllipticArcSegment anotherCircularArc = EllipticArcBuilder.CreateEllipticArcSegment(centerPt, 0, Math.PI / 2, 0, 1, 0.5);

          double semiMajor;
          double semiMinor;
          anotherCircularArc.GetAxes(out semiMajor, out semiMinor);
          // semiMajor = 1, semiMinor = 0.5

        });
        #endregion

        #region Construct a Circular Arc - using a center point and orientation

        //  methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
          {

            // Construct a circular arc from (2, 1) to (1, 2) 
            // with center point at (1, 1) and orientation counterclockwise.

            MapPoint fromPt = MapPointBuilder.CreateMapPoint(2, 1);
            MapPoint toPt = MapPointBuilder.CreateMapPoint(1, 2);
            Coordinate2D centerPtCoord = new Coordinate2D(1, 1);

            // use the builder constructor
            using (EllipticArcBuilder cab = new EllipticArcBuilder(fromPt, toPt, centerPtCoord, esriArcOrientation.esriArcCounterClockwise))
            {
              EllipticArcSegment circularArc = cab.ToSegment();
            }

            // or use the convenience method
            EllipticArcSegment anotherCircularArc = EllipticArcBuilder.CreateEllipticArcSegment(fromPt, toPt, centerPtCoord, esriArcOrientation.esriArcCounterClockwise);
          });
        #endregion

        #region Construct a Circular Arc - using two segments and radius

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // Construct a segment from (100, 100) to (50, 50) and another segment from (100, 100) to (150, 50).

          LineSegment segment1 = LineBuilder.CreateLineSegment(new Coordinate2D(100, 100), new Coordinate2D(50, 50));
          LineSegment segment2 = LineBuilder.CreateLineSegment(new Coordinate2D(100, 100), new Coordinate2D(150, 50));

          // Construct the hint point to determine where the arc will be constructed.

          Coordinate2D hintPoint = new Coordinate2D(100, 75);

          // Call QueryFilletRadius to get the minimum and maximum radii that can be used with these segments.

          var minMaxRadii = EllipticArcBuilder.QueryFilletRadiusRange(segment1, segment2, hintPoint);

          // Use the maximum radius to create the arc.

          double maxRadius = minMaxRadii.Item2;

          // use the builder constructor
          using (EllipticArcBuilder cab = new EllipticArcBuilder(segment1, segment2, maxRadius, hintPoint))
          {
            EllipticArcSegment circularArc = cab.ToSegment();
          }

          // or use the convenience method
          EllipticArcSegment anotherCircularArc = EllipticArcBuilder.CreateEllipticArcSegment(segment1, segment2, maxRadius, hintPoint);
        });
        #endregion

        #region Construct a Circle

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // Construct a circle with center at (-1,-1), radius = 2, and oriented clockwise.

          Coordinate2D centerPtCoord = new Coordinate2D(-1, -1);

          // use the builder constructor
          using (EllipticArcBuilder builder = new EllipticArcBuilder(centerPtCoord, 2, esriArcOrientation.esriArcClockwise))
          {
            EllipticArcSegment circle = builder.ToSegment();
            // circle.IsCircular = true
            // circle.IsCounterClockwise = false
            // circle.IsMinor = false

            double startAngle, centralAngle, rotationAngle;
            double semiMajor, semiMinor;
            Coordinate2D actualCenterPt;
            circle.QueryCoords(out actualCenterPt, out startAngle, out centralAngle, out rotationAngle, out semiMajor, out semiMinor);

            // semiMajor = 2.0
            // semiMinor = 2.0
            // startAngle = PI/2
            // centralAngle = -2*PI
            // rotationAngle = 0
            // endAngle = PI/2
          }

          // or use the convenience method
          EllipticArcSegment anotherCircle = EllipticArcBuilder.CreateEllipticArcSegment(centerPtCoord, 2, esriArcOrientation.esriArcClockwise);
        });
        #endregion

        #region Construct an Ellipse

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // Construct an ellipse centered at (1, 2) with rotationAngle = -pi/6,  
          // semiMajorAxis = 5, minorMajorRatio = 0.2, oriented clockwise

          Coordinate2D centerPt = new Coordinate2D(1, 2);

          // use the builder constructor
          using (EllipticArcBuilder builder = new EllipticArcBuilder(centerPt, -1 * Math.PI / 6, 5, 0.2, esriArcOrientation.esriArcClockwise))
          {
            EllipticArcSegment ellipse = builder.ToSegment();
          }

          // or use the convenience method
          EllipticArcSegment anotherEllipse = EllipticArcBuilder.CreateEllipticArcSegment(centerPt, -1 * Math.PI / 6, 5, 0.2, esriArcOrientation.esriArcClockwise);
        });
        #endregion
      }

      public void ConstructArcBuilderUtils()
      {
        EllipticArcSegment arcSegment = null;

        #region Elliptic Arc Builder Properties

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // retrieve the curve's control points
          using (EllipticArcBuilder builder = new EllipticArcBuilder(arcSegment))
          {
            MapPoint startPt = builder.StartPoint;
            MapPoint endPt = builder.EndPoint;
            Coordinate2D centerPt = builder.CenterPoint;
            bool isCircular = builder.IsCircular;
            bool isMinor = builder.IsMinor;
            double startAngle = builder.StartAngle;
            double endAngle = builder.EndAngle;
            double centralAngle = builder.CentralAngle;
            double rotationAngle = builder.RotationAngle;
          }
        });

        #endregion
      }

      public void ConstructArcUtils()
      {
        EllipticArcSegment arcSegment = null;

        #region Elliptic Arc Properties

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // retrieve the curve's control points
          EllipticArcSegment arc = EllipticArcBuilder.CreateEllipticArcSegment(arcSegment);
          MapPoint startPt = arc.StartPoint;
          MapPoint endPt = arc.EndPoint;
          Coordinate2D centerPt = arc.CenterPoint;
          bool isCircular = arc.IsCircular;
          bool isMinor = arc.IsMinor;

          double startAngle, centralAngle, rotationAngle, semiMajorAxis, semiMinorAxis;
          // or use QueryCoords
          arc.QueryCoords(out centerPt, out startAngle, out centralAngle, out rotationAngle, out semiMajorAxis, out semiMinorAxis);
        });

        #endregion
      }

      public void GeometryBag()
      {
        #region Construct GeometryBag

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          using (GeometryBagBuilder builder = new GeometryBagBuilder(SpatialReferences.WGS84))
          {
            GeometryBag bag = builder.ToGeometry();
            // bag.IsEmpty = true

            MapPoint point = MapPointBuilder.CreateMapPoint(1, 2, SpatialReferences.WebMercator);
            builder.AddGeometry(point);
            // builder.CountGeometries = 1

            bag = builder.ToGeometry();
            // bag.PartCount = 1
            // bag.PointCount = 1
            // bag.IsEmpty = false

            IReadOnlyList<Geometry> geometries = bag.Geometries;
            // geometries.Count = 1
            // geometries[0] is MapPoint with a sr of WGS84

            List<Coordinate2D> coords2D = new List<Coordinate2D>()
            {
              new Coordinate2D(0, 0),
              new Coordinate2D(0, 1),
              new Coordinate2D(1, 1),
              new Coordinate2D(1, 0)
            };

            Multipoint multipoint = MultipointBuilder.CreateMultipoint(coords2D, SpatialReferences.WGS84);
            builder.InsertGeometry(0, multipoint);
            bag = builder.ToGeometry();
            // bag.PartCount = 2

            geometries = bag.Geometries;
            // geometries.Count = 2
            // geometries[0] is Multipoint
            // geometries[1] is MapPoint

            Polyline polyline = PolylineBuilder.CreatePolyline(coords2D, SpatialReferences.WebMercator);
            builder.AddGeometry(polyline);
            builder.RemoveGeometry(1);
            bag = builder.ToGeometry();
            // bag.PartCount = 2

            geometries = bag.Geometries;
            // geometries.Count = 2
            // geometries[0] is Multipoint
            // geometries[1] is Polyline          }
          }
        });

        #endregion

        #region Construct GeometryBag - from an enumeration of geometries

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          MapPoint point = MapPointBuilder.CreateMapPoint(10, 20);

          List<Coordinate2D> coords = new List<Coordinate2D>() { new Coordinate2D(50, 60), new Coordinate2D(-120, -70), new Coordinate2D(40, 60) };
          Multipoint multipoint = MultipointBuilder.CreateMultipoint(coords, SpatialReferences.WebMercator);
          Polyline polyline = PolylineBuilder.CreatePolyline(coords);

          string json = "{\"rings\":[[[0,0],[0,1],[1,1],[1,0],[0,0]],[[3,0],[3,1],[4,1],[4,0],[3,0]]]}";
          Polygon polygon = PolygonBuilder.FromJson(json);

          var geometries = new List<Geometry>() { point, multipoint, polyline, polygon };
          using (var builder = new GeometryBagBuilder(geometries, SpatialReferences.WGS84))
          {
            // builder.CountGeometries = 4

            var bag = builder.ToGeometry();
            // bag.PartCount = 4
            // bag.PointCount = 17
          }

          // or use the convenience method
          var bag2 = GeometryBagBuilder.CreateGeometryBag(geometries, SpatialReferences.WGS84);
        });

      #endregion

        #region Construct GeometryBag - from JSON, Xml

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          const string json = "{\"geometries\":[{\"x\":1,\"y\":2},{\"rings\":[[[0,0],[0,4],[3,4],[3,0],[0,0]]]}],\"spatialReference\":{\"wkid\":4326,\"latestWkid\":4326}}";
          GeometryBag bag = GeometryBagBuilder.FromJson(json);

          string xml = bag.ToXML();
          GeometryBag bag_xml = GeometryBagBuilder.FromXML(xml);

        });

        #endregion

        #region Construct GeometryBag - adding or inserting an enumeration of geometries

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          MapPoint point = MapPointBuilder.CreateMapPoint(10, 20);

          List<Coordinate2D> coords = new List<Coordinate2D>() { new Coordinate2D(50, 60), new Coordinate2D(-120, -70), new Coordinate2D(40, 60) };
          Multipoint multipoint = MultipointBuilder.CreateMultipoint(coords, SpatialReferences.WebMercator);
          Polyline polyline = PolylineBuilder.CreatePolyline(coords);

          string json = "{\"rings\":[[[0,0],[0,1],[1,1],[1,0],[0,0]],[[3,0],[3,1],[4,1],[4,0],[3,0]]]}";
          Polygon polygon = PolygonBuilder.FromJson(json);

          var geometries = new List<Geometry>() { point, multipoint, polyline, polygon };

          using (var builder = new GeometryBagBuilder(SpatialReferences.WGS84))
          {
            // builder.CountGeometries = 0

            builder.AddGeometries(geometries);
            // builder.CountGeometries = 4

            var bag = builder.ToGeometry();
            // bag.PartCount = 4    (point, multipoint, polyline, polygon)

            geometries = new List<Geometry>() { point, polyline };
            builder.InsertGeometries(1, geometries);
            // builder.CountGeometries = 6
            bag = builder.ToGeometry();
            // bag.PartCount = 6    (point, point, polyline, multipoint, polyline, polygon)
          }
        });

        #endregion
      }

      #region Get the individual parts of a multipart feature

      /// <summary>
      /// This method takes an input multi part geometry and breaks the parts into individual standalone geometries.
      /// This method must be called on the MCT. Use QueuedTask.Run.
      /// </summary>
      /// <param name="inputGeometry">The geometry to be exploded into the individual parts.</param>
      /// <returns>An enumeration of individual parts as standalone geometries. The type of geometry is maintained, i.e.
      /// if the input geometry is of type Polyline then each geometry in the return is of type Polyline as well.
      /// If the input geometry is of type Unknown then an empty list is returned.</returns>
      /// <remarks>This method must be called on the MCT. Use QueuedTask.Run.</remarks>
      public IEnumerable<Geometry> MultipartToSinglePart(Geometry inputGeometry)
      {
        // list holding the part(s) of the input geometry
        List<Geometry> singleParts = new List<Geometry>();

        // check if the input is a null pointer or if the geometry is empty
        if (inputGeometry == null || inputGeometry.IsEmpty)
          return singleParts;

        // based on the type of geometry, take the parts/points and add them individually into a list
        switch (inputGeometry.GeometryType)
        {
          case GeometryType.Envelope:
            singleParts.Add(inputGeometry.Clone() as Envelope);
            break;
          case GeometryType.Multipatch:
            singleParts.Add(inputGeometry.Clone() as Multipatch);
            break;
          case GeometryType.Multipoint:
            var multiPoint = inputGeometry as Multipoint;

            foreach (var point in multiPoint.Points)
            {
              // add each point of collection as a standalone point into the list
              singleParts.Add(point);
            }
            break;
          case GeometryType.Point:
            singleParts.Add(inputGeometry.Clone() as MapPoint);
            break;
          case GeometryType.Polygon:
            var polygon = inputGeometry as Polygon;

            foreach (var polygonPart in polygon.Parts)
            {
              // use the PolygonBuilder turning the segments into a standalone 
              // polygon instance
              singleParts.Add(PolygonBuilder.CreatePolygon(polygonPart));
            }
            break;
          case GeometryType.Polyline:
            var polyline = inputGeometry as Polyline;

            foreach (var polylinePart in polyline.Parts)
            {
              // use the PolylineBuilder turning the segments into a standalone
              // polyline instance
              singleParts.Add(PolylineBuilder.CreatePolyline(polylinePart));
            }
            break;
          case GeometryType.Unknown:
            break;
          default:
            break;
        }

        return singleParts;
      }

      #endregion

      #region Get the outermost rings of a polygon

      /// <summary>
      /// The methods retrieves the outer ring(s) of the input polygon.
      /// This method must be called on the MCT. Use QueuedTask.Run.
      /// </summary>
      /// <param name="inputPolygon">Input Polygon.</param>
      /// <returns>The outer most (exterior, clockwise) ring(s) of the polygon. If the input is null or empty, a null pointer is returned.</returns>
      /// <remarks>This method must be called on the MCT. Use QueuedTask.Run.</remarks>
      public Polygon GetOutermostRings(Polygon inputPolygon)
      {
        if (inputPolygon == null || inputPolygon.IsEmpty)
          return null;

        List<Polygon> internalRings = new List<Polygon>();

        // explode the parts of the polygon into a list of individual geometries
        // see the "Get the individual parts of a multipart feature" snippet for MultipartToSinglePart method defintion
        var parts = MultipartToSinglePart(inputPolygon);

        // get an enumeration of clockwise geometries (area > 0) ordered by the area
        var clockwiseParts = parts.Where(geom => ((Polygon)geom).Area > 0).OrderByDescending(geom => ((Polygon)geom).Area);

        // for each of the exterior rings
        foreach (var part in clockwiseParts)
        {
          // add the first (the largest) ring into the internal collection
          if (internalRings.Count == 0)
            internalRings.Add(part as Polygon);

          // use flag to indicate if current part is within the already selection polygons
          bool isWithin = false;

          foreach (var item in internalRings)
          {
            if (GeometryEngine.Instance.Within(part, item))
              isWithin = true;
          }

          // if the current polygon is not within any polygon of the internal collection
          // then it is disjoint and needs to be added to 
          if (isWithin == false)
            internalRings.Add(part as Polygon);
        }

        using (PolygonBuilder outerRings = new PolygonBuilder())
        {
          // now assemble a new polygon geometry based on the internal polygon collection
          foreach (var ring in internalRings)
          {
            outerRings.AddParts(ring.Parts);
          }

          // return the final geometry of the outer rings
          return outerRings.ToGeometry();
      }
    }

      #endregion

      #region Determine if a polygon ring is an outer ring or an inner ring

      /// <summary>
      /// Based on the shoelace formula (or aka Gauss's area formula) https://en.wikipedia.org/wiki/Shoelace_formula
      /// It computes the area of a polygon by determining the area of ordered coordinate pairs.
      /// This method is more efficient than using the Polygon.Area methodology.
      /// </summary>
      /// <param name="PolyToCheck">The geometry to investigate.</param>
      private void FindOrientationSimple(Polygon PolyToCheck)
      {
        foreach (var part in PolyToCheck.Parts)
        {
          // construct a list of ordered coordinate pairs
          List<Coordinate2D> ringCoordinates = new List<Coordinate2D>(PolyToCheck.PointCount);

          foreach (var segment in part)
          {
            ringCoordinates.Add(segment.StartCoordinate);
            ringCoordinates.Add(segment.EndCoordinate);
          }

          // this is not the true area of the part
          // a negative number indicates an outer ring and a positive number represents an inner ring
          // (this is the opposite from the ArcGIS.Core.Geometry understanding)
          double signedArea = 0;

          // for all coordinates pairs compute the area
          // the last coordinate needs to reach back to the starting coordinate to complete
          for (int cIndex = 0; cIndex < ringCoordinates.Count - 1; cIndex++)
          {
            double x1 = ringCoordinates[cIndex].X;
            double y1 = ringCoordinates[cIndex].Y;

            double x2, y2;

            if (cIndex == ringCoordinates.Count - 2)
            {
              x2 = ringCoordinates[0].X;
              y2 = ringCoordinates[0].Y;
            }
            else
            {
              x2 = ringCoordinates[cIndex + 1].X;
              y2 = ringCoordinates[cIndex + 1].Y;
            }

            signedArea += ((x1 * y2) - (x2 * y1));
          }

          // if signedArea is a negative number => indicates an outer ring 
          // if signedArea is a positive number => indicates an inner ring
          // (this is the opposite from the ArcGIS.Core.Geometry understanding)

        }
      }

      #endregion

      public void RetrieveGeometryFromGeodatabase()
      {
        #region Retrieve Geometry from Geodatabase

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {

          try
          {
            // open a gdb
            using (ArcGIS.Core.Data.Geodatabase gdb = new ArcGIS.Core.Data.Geodatabase(new FileGeodatabaseConnectionPath(new Uri(@"c:\Temp\MyDatabase.gdb"))))
            {
              //Open a featureClass 
              using (ArcGIS.Core.Data.FeatureClass featureClass = gdb.OpenDataset<ArcGIS.Core.Data.FeatureClass>("Polygon"))
              {
                // find a field 
                ArcGIS.Core.Data.FeatureClassDefinition featureClassDefinition = featureClass.GetDefinition();
                int fldIndex = featureClassDefinition.FindField("SomeField");
                if (fldIndex == -1)
                {
                  return;
                }

                ArcGIS.Core.Data.QueryFilter filter = new ArcGIS.Core.Data.QueryFilter
                {
                  WhereClause = "OBJECTID = 6"
                };

                // get the row
                using (ArcGIS.Core.Data.RowCursor rowCursor = featureClass.Search(filter, false))
                {
                  while (rowCursor.MoveNext())
                  {
                    long oid = rowCursor.Current.GetObjectID();

                    // get the shape from the row
                    ArcGIS.Core.Data.Feature feature = rowCursor.Current as ArcGIS.Core.Data.Feature;
                    Polygon polygon = feature.GetShape() as Polygon;

                    // get the attribute from the row (assume it's a double field)
                    double value = (double)rowCursor.Current.GetOriginalValue(fldIndex);

                    // do something here
                  }
                }
              }
            }
          }
          catch (Exception ex)
          {
            // error - handle appropriately
          }
        });

        #endregion
      }

      public void ImportExport()
      {
      #region Import and Export Geometries to well-known Text

      // methods need to run on the MCT
      ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // create a point with z, m
          MapPoint point = MapPointBuilder.CreateMapPoint(100, 200, 300, 400, SpatialReferences.WebMercator);

          // set the flags
          WKTExportFlags exportFlags = WKTExportFlags.wktExportDefaults;
          WKTImportFlags importFlags = WKTImportFlags.wktImportDefaults;

          // export and import
          string wktString = GeometryEngine.Instance.ExportToWKT(exportFlags, point);
          MapPoint importPoint = GeometryEngine.Instance.ImportFromWKT(importFlags, wktString, SpatialReferences.WebMercator) as MapPoint;

          double x = importPoint.X;       // x = 100
          double y = importPoint.Y;       // y = 200
          bool hasZ = importPoint.HasZ;   // hasZ = true
          double z = importPoint.Z;       // z = 300
          bool hasM = importPoint.HasM;   // hasM = true
          double m = importPoint.M;       // m = 400

          // export without z
          WKTExportFlags exportFlagsNoZ = WKTExportFlags.wktExportStripZs;
          wktString = GeometryEngine.Instance.ExportToWKT(exportFlagsNoZ, point);
          importPoint = GeometryEngine.Instance.ImportFromWKT(importFlags, wktString, SpatialReferences.WebMercator) as MapPoint;

          x = importPoint.X;        // x = 100
          y = importPoint.Y;        // y = 200
          hasZ = importPoint.HasZ;  // hasZ = false
          z = importPoint.Z;        // z = 0
          hasM = importPoint.HasM;  // hasM = true
          m = importPoint.M;        // m = 400

          // export without m
          WKTExportFlags exportFlagsNoM = WKTExportFlags.wktExportStripMs;
          wktString = GeometryEngine.Instance.ExportToWKT(exportFlagsNoM, point);
          importPoint = GeometryEngine.Instance.ImportFromWKT(importFlags, wktString, SpatialReferences.WebMercator) as MapPoint;

          x = importPoint.X;        // x = 100
          y = importPoint.Y;        // y = 200
          hasZ = importPoint.HasZ;  // hasZ = true
          z = importPoint.Z;        // z = 300
          hasM = importPoint.HasM;  // hasM = false
          m = importPoint.M;        // m = Nan

          // export without z, m
          wktString = GeometryEngine.Instance.ExportToWKT(exportFlagsNoZ | exportFlagsNoM, point);
          importPoint = GeometryEngine.Instance.ImportFromWKT(importFlags, wktString, SpatialReferences.WebMercator) as MapPoint;

          x = importPoint.X;        // x = 100
          y = importPoint.Y;        // y = 200
          hasZ = importPoint.HasZ;  // hasZ = false
          z = importPoint.Z;        // z = 0
          hasM = importPoint.HasM;  // hasM = false
          m = importPoint.M;        // m = Nan

        });

      #endregion

      #region Import and Export Geometries to well-known Binary

      // methods need to run on the MCT
      ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // create a polyline
          List<Coordinate2D> coords = new List<Coordinate2D> 
          {
            new Coordinate2D(0, 0), 
            new Coordinate2D(0, 1),
            new Coordinate2D(1, 1),
            new Coordinate2D(1, 0)
          };

          Polyline polyline = PolylineBuilder.CreatePolyline(coords, SpatialReferences.WGS84);

          WKBExportFlags exportFlags = WKBExportFlags.wkbExportDefaults;
          WKBImportFlags importFlags = WKBImportFlags.wkbImportDefaults;

          // export and import
          byte[] buffer = GeometryEngine.Instance.ExportToWKB(exportFlags, polyline);
          Geometry geometry = GeometryEngine.Instance.ImportFromWKB(importFlags, buffer, SpatialReferences.WGS84);
          Polyline importPolyline = geometry as Polyline;


          // alternatively, determine the size for the buffer
          int bufferSize = GeometryEngine.Instance.GetWKBSize(exportFlags, polyline);
          buffer = new byte[bufferSize];
          // export
          bufferSize = GeometryEngine.Instance.ExportToWKB(exportFlags, polyline, ref buffer);
          // import
          importPolyline = GeometryEngine.Instance.ImportFromWKB(importFlags, buffer, SpatialReferences.WGS84) as Polyline;

        });


        #endregion

        #region Import and Export Geometries to EsriShape

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // create an envelope
          List<MapPoint> coordsZM = new List<MapPoint> 
          {
            MapPointBuilder.CreateMapPoint(1001, 1002, 1003, 1004), 
            MapPointBuilder.CreateMapPoint(2001, 2002, Double.NaN, 2004),
            MapPointBuilder.CreateMapPoint(3001, -3002, 3003, 3004),
            MapPointBuilder.CreateMapPoint(1001, -4002, 4003, 4004)
          };

          Envelope envelope = EnvelopeBuilder.CreateEnvelope(coordsZM[0], coordsZM[2], SpatialReferences.WGS84);


          // export and import
          EsriShapeExportFlags exportFlags = EsriShapeExportFlags.esriShapeExportDefaults;
          EsriShapeImportFlags importFlags = EsriShapeImportFlags.esriShapeImportDefaults;
          byte[] buffer = GeometryEngine.Instance.ExportToEsriShape(exportFlags, envelope);
          Polygon importedPolygon = GeometryEngine.Instance.ImportFromEsriShape(importFlags, buffer, envelope.SpatialReference) as Polygon;
          Envelope importedEnvelope = importedPolygon.Extent;

          // export without z,m
          buffer = GeometryEngine.Instance.ExportToEsriShape(EsriShapeExportFlags.esriShapeExportStripZs | EsriShapeExportFlags.esriShapeExportStripMs, envelope);
          importedPolygon = GeometryEngine.Instance.ImportFromEsriShape(importFlags, buffer, SpatialReferences.WGS84) as Polygon;
          importedEnvelope = importedPolygon.Extent;

          bool hasZ = importedEnvelope.HasZ;      // hasZ = false
          bool hasM = importedEnvelope.HasM;      // hasM = false

          // export with shapeSize
          int bufferSize = GeometryEngine.Instance.GetEsriShapeSize(exportFlags, envelope);
          buffer = new byte[bufferSize];

          bufferSize = GeometryEngine.Instance.ExportToEsriShape(exportFlags, envelope, ref buffer);
          importedPolygon = GeometryEngine.Instance.ImportFromEsriShape(importFlags, buffer, envelope.SpatialReference) as Polygon;
          importedEnvelope = importedPolygon.Extent;


          // or use the envelope and envelopeBuilder classes
          buffer = envelope.ToEsriShape();
          // buffer represents a polygon as there is not an envelope Esri shape buffer
          // EnvelopeBuilder.FromEsriShape takes a polygon Esri shape buffer and returns the extent of the polygon.
          importedEnvelope = EnvelopeBuilder.FromEsriShape(buffer);
        });

        #endregion

        #region Import and Export Geometries to JSON

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // MapPoint
          string inputString = "{\"x\":1,\"y\":2,\"spatialReference\":{\"wkid\":4326,\"latestWkid\":4326}}";
          Geometry geometry = GeometryEngine.Instance.ImportFromJSON(JSONImportFlags.jsonImportDefaults, inputString);

          MapPoint importPoint = geometry as MapPoint;
          // importPoint = 1, 2
          // importPoint.SpatialReference.WKid = 4326

          // use the MapPointBuilder convenience method
          MapPoint importPoint2 = MapPointBuilder.FromJson(inputString);
          // importPoint2 = 1, 2
          // impointPoint2.SpatialReference.Wkid = 4326

          string outputString = GeometryEngine.Instance.ExportToJSON(JSONExportFlags.jsonExportDefaults, importPoint);
          // outputString =  "{\"x\":1,\"y\":2,\"spatialReference\":{\"wkid\":4326,\"latestWkid\":4326}}"

          string outputString2 = importPoint.ToJson();


          inputString = "{\"spatialReference\":{\"wkid\":4326},\"z\":3,\"m\":4,\"x\":1,\"y\":2}";
          importPoint = GeometryEngine.Instance.ImportFromJSON(JSONImportFlags.jsonImportDefaults, inputString) as MapPoint;
          // importPoint.HasM = true
          // importPoint.HasZ = true
          // importPoint.X = 1
          // importPoint.Y = 2
          // importPoint.M = 4
          // importPoint.Z = 3

          importPoint2 = MapPointBuilder.FromJson(inputString);

          // export to json - skip spatial reference
          outputString = GeometryEngine.Instance.ExportToJSON(JSONExportFlags.jsonExportSkipCRS, importPoint);
          // outputString = "{\"x\":1,\"y\":2,\"z\":3,\"m\":4}"

          // export from mappoint, skipping the sr - same as GeometryEngine.Instance.ExportToJSON w JSONExportFlags.jsonExportSkipCRS
          outputString2 = importPoint.ToJson(true);


          //
          // Multipoint
          //
          List<Coordinate2D> coords = new List<Coordinate2D>
          {
            new Coordinate2D(100, 200),
            new Coordinate2D(201, 300),
            new Coordinate2D(301, 400),
            new Coordinate2D(401, 500)
          };

          Multipoint multipoint = MultipointBuilder.CreateMultipoint(coords, SpatialReferences.WebMercator);

          inputString = "{\"points\":[[100,200],[201,300],[301,400],[401,500]],\"spatialReference\":{\"wkid\":3857}}";
          Multipoint importMultipoint = GeometryEngine.Instance.ImportFromJSON(JSONImportFlags.jsonImportDefaults, inputString) as Multipoint;
          // importMultipoint.IsEqual(multipoint) = true

          ReadOnlyPointCollection points = importMultipoint.Points;
          // points.Count = 4
          // points[0] = 100, 200
          // points[1] = 201, 300
          // points[2] = 301, 400
          // points[3] = 401, 500

          // use the Multipointbuilder convenience method
          Multipoint importMultipoint2 = MultipointBuilder.FromJson(inputString);
          // importMultipoint2.IsEqual(multipoint) = true

          // export to json
          outputString = GeometryEngine.Instance.ExportToJSON(JSONExportFlags.jsonExportDefaults, multipoint);
          // outputString = inputString

          // or use the multipoint itself
          outputString2 = multipoint.ToJson();


          //
          // Polyline
          //
          Polyline polyline = PolylineBuilder.CreatePolyline(coords, SpatialReferences.WebMercator);

          // export without the spatial reference
          outputString = GeometryEngine.Instance.ExportToJSON(JSONExportFlags.jsonExportSkipCRS, polyline);
          // import
          geometry = GeometryEngine.Instance.ImportFromJSON(JSONImportFlags.jsonImportDefaults, outputString);
          Polyline importPolyline = geometry as Polyline;
          // importPolyline.SpatialReference = null


          points = importPolyline.Points;
          // points.Count = 4
          // points[0] = 100, 200
          // points[1] = 201, 300
          // points[2] = 301, 400
          // points[3] = 401, 500

          // use the polylineBuilder convenience method 
          Polyline importPolyline2 = PolylineBuilder.FromJson(outputString);
          // importPolyline2 = importPolyline

          outputString2 = importPolyline2.ToJson();
          // outputString2 = outputString


          //
          // Polygon
          //
          Polygon polygon = PolygonBuilder.CreatePolygon(coords, SpatialReferences.WebMercator);

          // export without the spatial reference
          outputString = GeometryEngine.Instance.ExportToJSON(JSONExportFlags.jsonExportSkipCRS, polygon);
          // import
          geometry = GeometryEngine.Instance.ImportFromJSON(JSONImportFlags.jsonImportDefaults, outputString);

          Polygon importPolygon = geometry as Polygon;
          // importPolygon.SpatialReference = null
          points = importPolygon.Points;
          // points.Count = 5

          // polygonBuilder convenience method
          Polygon importPolyon2 = PolygonBuilder.FromJson(outputString);
          // importPolygon2 = importPolygon

          // export from the polygon
          outputString2 = importPolyon2.ToJson(true);


          // Empty polygon
          polygon = PolygonBuilder.CreatePolygon(SpatialReferences.WebMercator);
          outputString = GeometryEngine.Instance.ExportToJSON(JSONExportFlags.jsonExportDefaults, polygon);
          importPolygon = GeometryEngine.Instance.ImportFromJSON(JSONImportFlags.jsonImportDefaults, outputString) as Polygon;

          // importPolygon.IsEmpty = true
          // importPolygon.SpatialReference.Wkid = 3857
        });

        #endregion

        #region Import and Export Geometries to XML

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {

          MapPoint minPoint = MapPointBuilder.CreateMapPoint(1, 1, 1, 1, 3);
          MapPoint maxPoint = MapPointBuilder.CreateMapPoint(5, 5, 5);

          // 
          //  mapPoint
          // 
          string xml = minPoint.ToXML();
          MapPoint minPointImport = MapPointBuilder.FromXML(xml);
          // minPointImport = minPoint

          //
          // envelope
          //
          Envelope envelopeWithID = EnvelopeBuilder.CreateEnvelope(minPoint, maxPoint);

          // Envelopes don't have IDs
          // envelopeWithID.HasID = false
          // envelopeWithID.HasM = true
          // envelopeWithID.HasZ = true

          xml = envelopeWithID.ToXML();
          Envelope envelopeImport = EnvelopeBuilder.FromXML(xml);

          //
          // multipoint
          //
          List<MapPoint> list = new List<MapPoint>();
          list.Add(minPoint);
          list.Add(maxPoint);

          Multipoint multiPoint = MultipointBuilder.CreateMultipoint(list);

          xml = multiPoint.ToXML();
          Multipoint multipointImport = MultipointBuilder.FromXML(xml);
          // multipointImport.PointCount == 2
          // multipointImport.HasID = true
          // multipointImport.HasM = true
          // multipointImport.HasZ= true
        });
        #endregion
      }

      public void AccelerateGeomtries()
      {
        Polygon polygon = null;
        IEnumerable<Polygon> testPolygons = null;

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
        #region Determine area of a polygon
        
        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          var g1 = PolygonBuilder.FromJson("{\"rings\": [ [ [0, 0], [10, 0], [10, 10], [0, 10] ] ] }");
          double d = GeometryEngine.Instance.Area(g1);
          // d = -100.0         //negative due to wrong ring orientation
          d = GeometryEngine.Instance.Area(GeometryEngine.Instance.SimplifyAsFeature(g1));
          // d = 100.0        // feature has been simplifed; ring orientation is correct
        });

        #endregion
      }

      public void Boundary()
      {
        #region Determine the boundary of a multi-part Polygon

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // create a donut polygon.  Must use the PolygonBuilder object

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

          // add the outer points
          using (PolygonBuilder pb = new PolygonBuilder(outerPts))
          {
            // add the inner points (note they are defined anticlockwise)
            pb.AddPart(innerPts);
            // get the polygon
            Polygon donut = pb.ToGeometry();

            // get the boundary 
            Geometry g = GeometryEngine.Instance.Boundary(donut);
            Polyline boundary = g as Polyline;
          }
        });

        #endregion
      }

      public void Buffer() 
      {
        #region Buffer a MapPoint

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // buffer a point
          MapPoint pt = MapPointBuilder.CreateMapPoint(1.0, 1.0, SpatialReferences.WGS84);
          Geometry ptBuffer = GeometryEngine.Instance.Buffer(pt, 5.0);
          Polygon buffer = ptBuffer as Polygon;
        });
        #endregion

        #region Buffer a Circular Arc

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // create the circular arc
          MapPoint fromPt = MapPointBuilder.CreateMapPoint(2, 1);
          MapPoint toPt = MapPointBuilder.CreateMapPoint(1, 2);
          Coordinate2D interiorPt = new Coordinate2D(1 + Math.Sqrt(2) / 2, 1 + Math.Sqrt(2) / 2);

          EllipticArcSegment circularArc = EllipticArcBuilder.CreateEllipticArcSegment(fromPt, toPt, interiorPt);

          // buffer the arc
          Polyline polyline = PolylineBuilder.CreatePolyline(circularArc);
          Geometry lineBuffer = GeometryEngine.Instance.Buffer(polyline, 10);
        });

        #endregion

        #region Buffer multiple MapPoints

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // creates a buffer around each MapPoint

          List<MapPoint> pts = new List<MapPoint>();
          pts.Add(MapPointBuilder.CreateMapPoint(1.0, 1.0));
          pts.Add(MapPointBuilder.CreateMapPoint(1.0, 2.0));
          pts.Add(MapPointBuilder.CreateMapPoint(2.0, 2.0));
          pts.Add(MapPointBuilder.CreateMapPoint(2.0, 1.0));

          Geometry ptsBuffer = GeometryEngine.Instance.Buffer(pts, 0.25);
          Polygon bufferResult = ptsBuffer as Polygon;      // bufferResult will have 4 parts
        });

        #endregion

        #region Buffer many different Geometry Types
        
        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          List<Coordinate2D> coords = new List<Coordinate2D>()
          {
            new Coordinate2D(1, 2), new Coordinate2D(3, 4), new Coordinate2D(4, 2),
            new Coordinate2D(5, 6), new Coordinate2D(7, 8), new Coordinate2D(8, 4),
            new Coordinate2D(9, 10), new Coordinate2D(11, 12), new Coordinate2D(12, 8),
            new Coordinate2D(10, 8), new Coordinate2D(12, 12), new Coordinate2D(14, 10)
          };

          List<Geometry> manyGeometries = new List<Geometry>
          {
            MapPointBuilder.CreateMapPoint(coords[9]),
            PolylineBuilder.CreatePolyline(new List<Coordinate2D>(){coords[0], coords[1], coords[2]}, SpatialReferences.WGS84),
            PolylineBuilder.CreatePolyline(new List<Coordinate2D>(){coords[3], coords[4], coords[5]}),
            PolygonBuilder.CreatePolygon(new List<Coordinate2D>(){coords[6], coords[7], coords[8]})
          };

          Geometry manyGeomBuffer = GeometryEngine.Instance.Buffer(manyGeometries, 0.25);
        });

        #endregion
      }

      public void CalculateNonSimpleZs_Ms()
      {
        #region Interpolate Z values on a polyline

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
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

          Polyline polyline = PolylineBuilder.CreatePolyline(coords2, sr);

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
        });
        #endregion

        #region Interpolate M values on a polygon

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          List<MapPoint> coords = new List<MapPoint>()
          {
            MapPointBuilder.CreateMapPoint(0, 0, 0, 0),
            MapPointBuilder.CreateMapPoint(0, 1000),
            MapPointBuilder.CreateMapPoint(1000, 1000, 10, 50)
          };

          SpatialReference sr2 = SpatialReferences.WebMercator;

          Polygon polygon = PolygonBuilder.CreatePolygon(coords, sr2);

          // polygon.HasM = true
          // polygon.Points[1].HasM = true
          // polygon.Points[1].M = NaN

          Polygon polygonNoNaNMs = GeometryEngine.Instance.CalculateNonSimpleMs(polygon, 0) as Polygon;

          // polygonNoNaNMs.Points[1].HasM = true
          // polygonNoNaNMs.Points[1].M = 25  (halfway between 0 and 50)

        });

        #endregion
      }

      public void CenterAt()
      {
        #region Center an envelope around X,Y

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          Envelope env = EnvelopeBuilder.CreateEnvelope(1.0, 1.0, 5.0, 5.0);
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
        });
        #endregion
      }

      public void Centroid()
      {
        #region Find the centroid of geometries
        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // simple polygon
          List<Coordinate2D> list2D = new List<Coordinate2D>();
          list2D.Add(new Coordinate2D(0, 0));
          list2D.Add(new Coordinate2D(0, 2));
          list2D.Add(new Coordinate2D(2, 2));
          list2D.Add(new Coordinate2D(2, 0));

          Polygon polygon = PolygonBuilder.CreatePolygon(list2D, SpatialReferences.WGS84);

          // verify it is simple
          bool isSimple = GeometryEngine.Instance.IsSimpleAsFeature(polygon);
          // find the centroid
          MapPoint centroid = GeometryEngine.Instance.Centroid(polygon);
          // centroid.X = 1
          // centroid.Y = 1


          // map Point
          MapPoint pt1 = MapPointBuilder.CreateMapPoint(1, 2, 3, 4, SpatialReferences.WGS84);
          MapPoint pt2 = MapPointBuilder.CreateMapPoint(5, 2, double.NaN, 7);

          // pt1.HasZ = true
          // pt1.HasM = true
          centroid = GeometryEngine.Instance.Centroid(pt1);
          // centroid.HasZ = true
          // centroid.HasM = true
          // pt1.IsEqual(centroid) = true


          // multipoint
          List<MapPoint> list = new List<MapPoint>() { pt1, pt2 };
          Multipoint multipoint = MultipointBuilder.CreateMultipoint(list);
          // multipoint.HasZ = true
          // multipoint.HasM = true

          centroid = GeometryEngine.Instance.Centroid(multipoint);
          // centroid.X = 3
          // centroid.Y = 2
          // centroid.HasZ = false
          // centroid.HasM = false
        });
        #endregion
      }

      public void Clip()
      {
        #region Clip a Polyline

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // clip a polyline by an envelope

          Envelope env = EnvelopeBuilder.CreateEnvelope(2.0, 2.0, 4.0, 4.0);
          LineSegment line = LineBuilder.CreateLineSegment(new Coordinate2D(0, 3), new Coordinate2D(5.0, 3.0));
          Polyline polyline = PolylineBuilder.CreatePolyline(line);

          Geometry clipGeom = GeometryEngine.Instance.Clip(polyline, env);
        });
        #endregion

        #region Clip a Polyline by a Polygon

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // clip a polyline by a polygon

          List<Coordinate2D> list = new List<Coordinate2D>();
          list.Add(new Coordinate2D(1.0, 1.0));
          list.Add(new Coordinate2D(1.0, 4.0));
          list.Add(new Coordinate2D(4.0, 4.0));
          list.Add(new Coordinate2D(4.0, 1.0));

          Polygon polygon = PolygonBuilder.CreatePolygon(list, SpatialReferences.WGS84);

          LineSegment crossingLine = LineBuilder.CreateLineSegment(MapPointBuilder.CreateMapPoint(0, 3), MapPointBuilder.CreateMapPoint(5.0, 3.0));
          Polyline p = PolylineBuilder.CreatePolyline(crossingLine);
          Geometry geometry = GeometryEngine.Instance.Clip(p, polygon.Extent);
        });
        #endregion
      }

      public void Contains()
      {
        #region Polygon contains MapPoints, Polylines, Polygons

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {

          // build a polygon      
          List<MapPoint> pts = new List<MapPoint>();
          pts.Add(MapPointBuilder.CreateMapPoint(1.0, 1.0));
          pts.Add(MapPointBuilder.CreateMapPoint(1.0, 2.0));
          pts.Add(MapPointBuilder.CreateMapPoint(2.0, 2.0));
          pts.Add(MapPointBuilder.CreateMapPoint(2.0, 1.0));

          Polygon poly = PolygonBuilder.CreatePolygon(pts);

          // test if an inner point is contained
          MapPoint innerPt = MapPointBuilder.CreateMapPoint(1.5, 1.5);
          bool contains = GeometryEngine.Instance.Contains(poly, innerPt);   // contains = true

          // test a point on a boundary
          contains = GeometryEngine.Instance.Contains(poly, poly.Points[0]);     // contains = false

          // test an interior line
          MapPoint innerPt2 = MapPointBuilder.CreateMapPoint(1.25, 1.75);
          List<MapPoint> innerLinePts = new List<MapPoint>();
          innerLinePts.Add(innerPt);
          innerLinePts.Add(innerPt2);

          // test an inner polyline
          Polyline polyline = PolylineBuilder.CreatePolyline(innerLinePts);
          contains = GeometryEngine.Instance.Contains(poly, polyline);   // contains = true

          // test a line that crosses the boundary
          MapPoint outerPt = MapPointBuilder.CreateMapPoint(3, 1.5);
          List<MapPoint> crossingLinePts = new List<MapPoint>();
          crossingLinePts.Add(innerPt);
          crossingLinePts.Add(outerPt);

          polyline = PolylineBuilder.CreatePolyline(crossingLinePts);
          contains = GeometryEngine.Instance.Contains(poly, polyline);     // contains = false

          // test a polygon in polygon
          Envelope env = EnvelopeBuilder.CreateEnvelope(innerPt, innerPt2);
          contains = GeometryEngine.Instance.Contains(poly, env);      // contains = true
        });
        #endregion
      }

      public void ConvexHull()
      {
        #region Determine convex hull

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          //
          // convex hull around a point - returns a point
          //

          MapPoint pt = MapPointBuilder.CreateMapPoint(2.0, 2.0);
          Geometry hull = GeometryEngine.Instance.ConvexHull(pt);
          MapPoint hullPt = hull as MapPoint;
          // nullPt.X = 2
          // hullPt.Y = 2


          List<MapPoint> list = new List<MapPoint>();
          list.Add(MapPointBuilder.CreateMapPoint(1.0, 1.0));
          list.Add(MapPointBuilder.CreateMapPoint(1.0, 2.0));
          list.Add(MapPointBuilder.CreateMapPoint(2.0, 2.0));
          list.Add(MapPointBuilder.CreateMapPoint(2.0, 1.0));

          //
          // convex hull around a multipoint - returns a polygon
          //

          // build a multiPoint
          Multipoint multiPoint = MultipointBuilder.CreateMultipoint(list);

          hull = GeometryEngine.Instance.ConvexHull(multiPoint);
          Polygon hullPoly = hull as Polygon;
          // hullPoly.Area = 1
          // hullPoly.PointCount = 5


          // 
          // convex hull around a line - returns a polyline or polygon
          // 

          List<MapPoint> polylineList = new List<MapPoint>();
          polylineList.Add(MapPointBuilder.CreateMapPoint(1.0, 1.0));
          polylineList.Add(MapPointBuilder.CreateMapPoint(2.0, 2.0));

          // 2 point straight line
          Polyline polyline = PolylineBuilder.CreatePolyline(polylineList);
          hull = GeometryEngine.Instance.ConvexHull(polyline);
          Polyline hullPolyline = hull as Polyline;
          // hullPolyline.Length = Math.Sqrt(2)
          // hullPolyline.PointCount = 2

          // 3 point angular line
          polylineList.Add(MapPointBuilder.CreateMapPoint(2.0, 1.0));
          polyline = PolylineBuilder.CreatePolyline(polylineList);
          hull = GeometryEngine.Instance.ConvexHull(polyline);
          hullPoly = hull as Polygon;
          // hullPoly.Length = 2 + Math.Sqrt(2)
          // hullPoly.Area = 0.5
          // hullPoly.PointCount = 4


          //
          // convex hull around a polygon - returns a polygon
          //

          // simple polygon
          Polygon poly = PolygonBuilder.CreatePolygon(list);
          hull = GeometryEngine.Instance.ConvexHull(poly);
          hullPoly = hull as Polygon;
          
          // hullPoly.Length = 4.0
          // hullPoly.Area = 1.0
          // hullPoly.PointCount = 5


          // polygon with concave angles
          List<MapPoint> funkyList = new List<MapPoint>();
          funkyList.Add(MapPointBuilder.CreateMapPoint(1.0, 1.0));
          funkyList.Add(MapPointBuilder.CreateMapPoint(1.5, 1.5));
          funkyList.Add(MapPointBuilder.CreateMapPoint(1.0, 2.0));
          funkyList.Add(MapPointBuilder.CreateMapPoint(2.0, 2.0));
          funkyList.Add(MapPointBuilder.CreateMapPoint(1.5, 1.5));
          funkyList.Add(MapPointBuilder.CreateMapPoint(2.0, 1.0));
          funkyList.Add(MapPointBuilder.CreateMapPoint(1.0, 1.0));

          Polygon funkyPoly = PolygonBuilder.CreatePolygon(funkyList);
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
        });
        #endregion
      }

      public void Crosses()
      {
        #region Determine if two geometries cross

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          //
          // pt on pt
          //

          MapPoint pt = MapPointBuilder.CreateMapPoint(1.0, 1.0);
          MapPoint pt2 = MapPointBuilder.CreateMapPoint(2.0, 2.0);

          bool crosses = GeometryEngine.Instance.Crosses(pt, pt2);         // crosses = false
          crosses = GeometryEngine.Instance.Crosses(pt, pt);               // crosses = false

          // 
          // pt and line
          // 

          List<MapPoint> list = new List<MapPoint>();
          list.Add(MapPointBuilder.CreateMapPoint(1.0, 1.0));
          list.Add(MapPointBuilder.CreateMapPoint(3.0, 3.0));
          list.Add(MapPointBuilder.CreateMapPoint(5.0, 1.0));

          Polyline line1 = PolylineBuilder.CreatePolyline(list);
          crosses = GeometryEngine.Instance.Crosses(line1, pt2);           // crosses = false
          crosses = GeometryEngine.Instance.Crosses(pt2, line1);           // crosses = false
          // end pt of line
          crosses = GeometryEngine.Instance.Crosses(line1, pt);            // crosses = false

          //
          // pt and polygon
          //
          List<MapPoint> polyPts = new List<MapPoint>();
          polyPts.Add(MapPointBuilder.CreateMapPoint(3.0, 2.0));
          polyPts.Add(MapPointBuilder.CreateMapPoint(3.0, 6.0));
          polyPts.Add(MapPointBuilder.CreateMapPoint(6.0, 6.0));
          polyPts.Add(MapPointBuilder.CreateMapPoint(6.0, 2.0));

          Polygon poly1 = PolygonBuilder.CreatePolygon(polyPts);
          crosses = GeometryEngine.Instance.Crosses(poly1, pt);              // crosses = false
          crosses = GeometryEngine.Instance.Crosses(pt, poly1);              // crosses = false

          // 
          // line and line
          //
          List<MapPoint> list2 = new List<MapPoint>();
          list2.Add(MapPointBuilder.CreateMapPoint(1.0, 3.0));
          list2.Add(MapPointBuilder.CreateMapPoint(3.0, 1.0));
          list2.Add(MapPointBuilder.CreateMapPoint(5.0, 3.0));

          Polyline line2 = PolylineBuilder.CreatePolyline(list2);
          crosses = GeometryEngine.Instance.Crosses(line1, line2);           // crosses = true

          //
          // line and polygon
          //
          crosses = GeometryEngine.Instance.Crosses(poly1, line1);           // crosses = true

          //
          // polygon and polygon
          //
          Envelope env = EnvelopeBuilder.CreateEnvelope(MapPointBuilder.CreateMapPoint(1.0, 1.0), MapPointBuilder.CreateMapPoint(4, 4));
          Polygon poly2 = PolygonBuilder.CreatePolygon(env);
          crosses = GeometryEngine.Instance.Crosses(poly1, poly2);           // crosses = false
        });

        #endregion
      }

      public void Cut()
      {
        #region Cut a geometry with a polyline

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          SpatialReference sr = SpatialReferences.WGS84;

          List<MapPoint> list = new List<MapPoint>();
          list.Add(MapPointBuilder.CreateMapPoint(1.0, 1.0, sr));
          list.Add(MapPointBuilder.CreateMapPoint(1.0, 4.0, sr));
          list.Add(MapPointBuilder.CreateMapPoint(4.0, 4.0, sr));
          list.Add(MapPointBuilder.CreateMapPoint(4.0, 1.0, sr));

          List<Geometry> cutGeometries;

          LineSegment line = LineBuilder.CreateLineSegment(MapPointBuilder.CreateMapPoint(0, 0, sr), MapPointBuilder.CreateMapPoint(3, 6, sr));
          Polyline cutter = PolylineBuilder.CreatePolyline(line);

          // polyline
          Polyline polyline = PolylineBuilder.CreatePolyline(list);
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
          Polygon polygon = PolygonBuilder.CreatePolygon(list);
          isSimple = GeometryEngine.Instance.IsSimpleAsFeature(polygon);
          cutGeometries = GeometryEngine.Instance.Cut(polygon, cutter) as List<Geometry>;
        });
        #endregion
      }

      public void DensifyByLength()
      {
        #region Densify by Length

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // densify a line segment
          MapPoint startPt = MapPointBuilder.CreateMapPoint(1.0, 1.0);
          MapPoint endPt = MapPointBuilder.CreateMapPoint(1, 21);
          LineSegment line = LineBuilder.CreateLineSegment(startPt, endPt);
          Polyline polyline = PolylineBuilder.CreatePolyline(line);

          Geometry geom = GeometryEngine.Instance.DensifyByLength(polyline, 2);
          Polyline result = geom as Polyline;

          
          // densify a circular arc
          MapPoint fromPt = MapPointBuilder.CreateMapPoint(2, 1);
          MapPoint toPt = MapPointBuilder.CreateMapPoint(1, 2);
          Coordinate2D interiorPt = new Coordinate2D(1 + Math.Sqrt(2) / 2, 1 + Math.Sqrt(2) / 2);

          using (EllipticArcBuilder cab = new EllipticArcBuilder(fromPt, toPt, interiorPt))
          {
            EllipticArcSegment circularArc = cab.ToSegment();
            polyline = PolylineBuilder.CreatePolyline(circularArc);
            geom = GeometryEngine.Instance.DensifyByLength(polyline, 2);
            result = geom as Polyline;
          }
        });

        #endregion
      }

      public void Difference()
      {
        #region Difference between two Polygons

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          List<MapPoint> polyPts = new List<MapPoint>();
          polyPts.Add(MapPointBuilder.CreateMapPoint(3.0, 2.0));
          polyPts.Add(MapPointBuilder.CreateMapPoint(3.0, 6.0));
          polyPts.Add(MapPointBuilder.CreateMapPoint(6.0, 6.0));
          polyPts.Add(MapPointBuilder.CreateMapPoint(6.0, 2.0));

          Polygon poly1 = PolygonBuilder.CreatePolygon(polyPts);

          Envelope env = EnvelopeBuilder.CreateEnvelope(MapPointBuilder.CreateMapPoint(1.0, 1.0), MapPointBuilder.CreateMapPoint(4, 4));
          Polygon poly2 = PolygonBuilder.CreatePolygon(env);

          Geometry result = GeometryEngine.Instance.Difference(poly1, poly2);
          Polygon polyResult = result as Polygon;
          // polyResult.Area = 10.0

          result = GeometryEngine.Instance.Difference(poly2, poly1);
          polyResult = result as Polygon;
          // polyResult.Area = 7.0

        });
        #endregion
      }

      public void Disjoint_Disjoint3D()
      {
        #region Determine if two Geometries are disjoint

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {

          //
          // pt on pt
          //

          MapPoint pt = MapPointBuilder.CreateMapPoint(1.0, 1.0);
          MapPoint pt2 = MapPointBuilder.CreateMapPoint(2.0, 2.5);

          bool disjoint = GeometryEngine.Instance.Disjoint(pt, pt2);       // result is true

          using (MultipointBuilder mpb = new MultipointBuilder())
          {
            mpb.Add(pt);
            mpb.Add(pt2);
            Multipoint multiPoint = mpb.ToGeometry();

            disjoint = GeometryEngine.Instance.Disjoint(multiPoint, pt);     // result is false
          }

          // 
          // pt and line
          // 

          List<MapPoint> list = new List<MapPoint>();
          list.Add(MapPointBuilder.CreateMapPoint(1.0, 1.0));
          list.Add(MapPointBuilder.CreateMapPoint(3.0, 3.0));
          list.Add(MapPointBuilder.CreateMapPoint(5.0, 1.0));

          Polyline line1 = PolylineBuilder.CreatePolyline(list);
          disjoint = GeometryEngine.Instance.Disjoint(line1, pt2);       // result is true

          disjoint = GeometryEngine.Instance.Disjoint(pt2, line1);       // result is true

          // end pt of line
          disjoint = GeometryEngine.Instance.Disjoint(line1, pt);        // result is false

          //
          // pt and polygon
          //
          List<MapPoint> polyPts = new List<MapPoint>();
          polyPts.Add(MapPointBuilder.CreateMapPoint(3.0, 2.0));
          polyPts.Add(MapPointBuilder.CreateMapPoint(3.0, 6.0));
          polyPts.Add(MapPointBuilder.CreateMapPoint(6.0, 6.0));
          polyPts.Add(MapPointBuilder.CreateMapPoint(6.0, 2.0));

          Polygon poly1 = PolygonBuilder.CreatePolygon(polyPts);
          disjoint = GeometryEngine.Instance.Disjoint(poly1, pt);          // result is true
          disjoint = GeometryEngine.Instance.Disjoint(pt, poly1);          // result is true

          // 
          // line and line
          //

          List<MapPoint> list2 = new List<MapPoint>();
          list2.Add(MapPointBuilder.CreateMapPoint(1.0, 3.0));
          list2.Add(MapPointBuilder.CreateMapPoint(3.0, 1.0));
          list2.Add(MapPointBuilder.CreateMapPoint(5.0, 3.0));

          Polyline line2 = PolylineBuilder.CreatePolyline(list2);
          disjoint = GeometryEngine.Instance.Disjoint(line1, line2);         // result is false

          //
          // line and polygon
          //
          disjoint = GeometryEngine.Instance.Disjoint(poly1, line1);         // result is false
          disjoint = GeometryEngine.Instance.Disjoint(line1, poly1);         // result is false

          //
          // polygon and polygon
          //
          Envelope env = EnvelopeBuilder.CreateEnvelope(MapPointBuilder.CreateMapPoint(1.0, 1.0), MapPointBuilder.CreateMapPoint(4, 4));
          Polygon poly2 = PolygonBuilder.CreatePolygon(env);

          disjoint = GeometryEngine.Instance.Disjoint(poly1, poly2);         // result is false


          // disjoint3D

          SpatialReference sr = SpatialReferences.WGS84;

          MapPoint pt3D_1 = MapPointBuilder.CreateMapPoint(1, 1, 1, sr);
          MapPoint pt3D_2 = MapPointBuilder.CreateMapPoint(2, 2, 2, sr);
          MapPoint pt3D_3 = MapPointBuilder.CreateMapPoint(1, 1, 2, sr);

          using (MultipointBuilder mpb = new MultipointBuilder())
          {
            mpb.Add(pt3D_1);
            mpb.Add(pt3D_2);
            mpb.HasZ = true;

            Multipoint multiPoint = mpb.ToGeometry();
            disjoint = GeometryEngine.Instance.Disjoint3D(multiPoint, pt3D_2);     // disjoint = false
            disjoint = GeometryEngine.Instance.Disjoint3D(multiPoint, pt3D_3);     // disjoint = true
          }
        });
        #endregion
      }

      public void Distance()
      {
        #region Determine distance between two Geometries
        
        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          MapPoint pt1 = MapPointBuilder.CreateMapPoint(1.0, 1.0);
          MapPoint pt2 = MapPointBuilder.CreateMapPoint(2.0, 2.0);
          MapPoint pt3 = MapPointBuilder.CreateMapPoint(4.0, 2.0);

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
          Multipoint multiPoint = MultipointBuilder.CreateMultipoint(multiPts);
          d = GeometryEngine.Instance.Distance(pt1, multiPoint);         // d = Math.Sqrt(2)

          //
          // pt and envelope
          //
          Envelope env = EnvelopeBuilder.CreateEnvelope(pt1, pt2);
          d = GeometryEngine.Instance.Distance(pt1, env);                // d = 0

          //
          // pt and polyline
          //
          List<MapPoint> polylinePts = new List<MapPoint>();
          polylinePts.Add(MapPointBuilder.CreateMapPoint(2.0, 1.0));
          polylinePts.Add(pt2);
          Polyline polyline = PolylineBuilder.CreatePolyline(polylinePts);

          d = GeometryEngine.Instance.Distance(pt1, polyline);             // d = 1.0

          //
          // pt and polygon
          //
          Envelope env2 = EnvelopeBuilder.CreateEnvelope(MapPointBuilder.CreateMapPoint(3.0, 3.0), MapPointBuilder.CreateMapPoint(5.0, 5.0));
          Polygon poly = PolygonBuilder.CreatePolygon(env2);
          d = GeometryEngine.Instance.Distance(pt1, poly);                 // d = Math.Sqrt(8)

          //
          // envelope and polyline
          //
          d = GeometryEngine.Instance.Distance(env, polyline);             // d = 0

          //
          // polyline and polyline
          //
          List<MapPoint> polylineList = new List<MapPoint>();
          polylineList.Add(MapPointBuilder.CreateMapPoint(4, 3));
          polylineList.Add(MapPointBuilder.CreateMapPoint(4, 4));
          Polyline polyline2 = PolylineBuilder.CreatePolyline(polylineList);

          d = GeometryEngine.Instance.Distance(polyline, polyline2);           // d = Math.Sqrt(5)


          //
          // polygon and polygon
          //
          Polygon poly2 = PolygonBuilder.CreatePolygon(env);
          d = GeometryEngine.Instance.Distance(poly, poly2);                 // d = Math.Sqrt(2)
        });

        #endregion
      }

      public void Distance3D()
      {
         #region Determine 3D distance between two Geometries

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {

          // between points 
          MapPoint pt1 = MapPointBuilder.CreateMapPoint(1, 1, 1);
          MapPoint pt2 = MapPointBuilder.CreateMapPoint(2, 2, 2);
          MapPoint pt3 = MapPointBuilder.CreateMapPoint(10, 2, 1);

          // pt1 to pt2
          double d = GeometryEngine.Instance.Distance3D(pt1, pt2);        // d = Math.Sqrt(3)

          // pt1 to pt3
          d = GeometryEngine.Instance.Distance3D(pt1, pt3);        // d = Math.Sqrt(82)

          // pt2 to pt3
          d = GeometryEngine.Instance.Distance3D(pt2, pt3);        // d = Math.Sqrt(65)



          // intersecting lines

          List<MapPoint> list = new List<MapPoint>();
          list.Add(MapPointBuilder.CreateMapPoint(1.0, 1.0, 1.0));
          list.Add(MapPointBuilder.CreateMapPoint(3.0, 3.0, 1.0));
          list.Add(MapPointBuilder.CreateMapPoint(5.0, 1.0, 1.0));

          Polyline line1 = PolylineBuilder.CreatePolyline(list);

          List<MapPoint> list2 = new List<MapPoint>();
          list2.Add(MapPointBuilder.CreateMapPoint(1.0, 3.0, 1.0));
          list2.Add(MapPointBuilder.CreateMapPoint(3.0, 1.0, 1.0));
          list2.Add(MapPointBuilder.CreateMapPoint(5.0, 3.0, 1.0));

          Polyline line2 = PolylineBuilder.CreatePolyline(list2);

          bool intersects = GeometryEngine.Instance.Intersects(line1, line2);    // intersects = true
          d = GeometryEngine.Instance.Distance3D(line1, line2);                  // d = 0   (distance is 0 when geomtries intersect)

        });
        #endregion
      }

      public void Expand()
      {
        #region Expand envelopes

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          Envelope env = EnvelopeBuilder.CreateEnvelope(100.0, 100.0, 500.0, 500.0);
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
          MapPoint pt1 = MapPointBuilder.CreateMapPoint(100, 100, 100);
          MapPoint pt2 = MapPointBuilder.CreateMapPoint(200, 200, 200);

          using (EnvelopeBuilder eb = new EnvelopeBuilder(pt1, pt2))
          {
            eb.HasZ = true;
            env = eb.ToGeometry();
            result = GeometryEngine.Instance.Expand(env, 0.5, 0.5, 0.5, true);

            // result.Center = 150, 150, 150
            // result.XMin = 125
            // result.YMin = 125
            // result.ZMin = 125
            // result.XMax = 175
            // result.YMax = 175
            // result.ZMax = 175
          }
        });
        #endregion
      }

      public void Extend()
      {
        #region Extend a polyline

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // build a polyline
          var polyline = PolylineBuilder.CreatePolyline(new[]
          {
              MapPointBuilder.CreateMapPoint( 1,  1, 10, 20),
              MapPointBuilder.CreateMapPoint( 0,  0, 10, 20),
              MapPointBuilder.CreateMapPoint( 1, -1, 10, 20)
          });

          // build the extender line
          var extender = PolylineBuilder.CreatePolyline(new[]
          {
              MapPointBuilder.CreateMapPoint( 2, 2),
              MapPointBuilder.CreateMapPoint( 2,-2), 
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

          polyline = PolylineBuilder.CreatePolyline(new[]
          {
              MapPointBuilder.CreateMapPoint( 1,  1),
              MapPointBuilder.CreateMapPoint( 3,  1),
          });

          extender = PolylineBuilder.CreatePolyline(new[]
          {
              MapPointBuilder.CreateMapPoint( 1, 4),
              MapPointBuilder.CreateMapPoint( 3, 4), 
          });

          result = GeometryEngine.Instance.Extend(polyline, extender, ExtendFlags.Default);
          // result = null
        });

        #endregion
      }

      public void GeodesicArea()
      {
        #region Calculate the Geodesic Area of a polygon
        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          var polygon = PolygonBuilder.CreatePolygon(new[]
        {
                  MapPointBuilder.CreateMapPoint(-10018754.1713946, 10018754.1713946),
                  MapPointBuilder.CreateMapPoint(10018754.1713946, 10018754.1713946),
                  MapPointBuilder.CreateMapPoint(10018754.1713946, -10018754.1713946),
                  MapPointBuilder.CreateMapPoint(-10018754.1713946, -10018754.1713946)
              }, SpatialReferences.WebMercator);
          var area = GeometryEngine.Instance.GeodesicArea(polygon);
        
          // area is close to 255032810857732.31
        });
        #endregion
      }

      public void GeodesicBuffer()
      {
        #region Create a buffer polygon at the specified geodesic distance

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // buffer a point
          MapPoint pt = MapPointBuilder.CreateMapPoint(1.0, 1.0, SpatialReferences.WGS84);
          Polygon outPolygon = GeometryEngine.Instance.GeodesicBuffer(pt, 5) as Polygon;

          double delta = SpatialReferences.WGS84.XYTolerance * 2 * Math.Sqrt(2);
          ReadOnlyPointCollection points = outPolygon.Points;
          foreach (MapPoint p in points)
          {
            double d = GeometryEngine.Instance.GeodesicDistance(pt, p);
            // d = 5 (+- delta)
          }

          // buffer of 0 distance produces an empty geometry
          Geometry g = GeometryEngine.Instance.GeodesicBuffer(pt, 0);
          // g.IsEmpty = true


          // buffer many points
          List<MapPoint> list = new List<MapPoint>();
          list.Add(MapPointBuilder.CreateMapPoint(1.0, 1.0, SpatialReferences.WGS84));
          list.Add(MapPointBuilder.CreateMapPoint(10.0, 20.0));
          list.Add(MapPointBuilder.CreateMapPoint(40.0, 40.0));
          list.Add(MapPointBuilder.CreateMapPoint(60.0, 60.0));

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
            MapPointBuilder.CreateMapPoint(1.0, 1.0, SpatialReferences.WGS84), 
            PolylineBuilder.CreatePolyline(new List<Coordinate2D>(){coords[0], coords[1], coords[2]}, SpatialReferences.WGS84),
            PolylineBuilder.CreatePolyline(new List<Coordinate2D>(){coords[3], coords[4], coords[5]}),
            PolygonBuilder.CreatePolygon(new List<Coordinate2D>(){coords[9], coords[10], coords[11]})
          };

          outPolygon = GeometryEngine.Instance.GeodesicBuffer(manyGeometries, 20000) as Polygon;
        });
        #endregion
      }

      public void GeodesicDistance()
      {
        #region Determine geodesic distance between two Geometries
        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          var point1 = MapPointBuilder.CreateMapPoint(-170, 45, SpatialReferences.WGS84);
          var point2 = MapPointBuilder.CreateMapPoint(170, 45, SpatialReferences.WGS84);

          var distances_meters = GeometryEngine.Instance.GeodesicDistance(point1, point2);
          // distance is approximately 1572912.2066940258 in meters
        });
        #endregion
      }

      public void GeodesicEllipse()
      {
        #region GeodesicEllipse

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
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
        });
        #endregion
      }

      public void GeodesicSector()
      {
        #region GeodesicSector

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
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
        });

        #endregion
      }

      public void GeodeticDensifyByDeviation()
      {
        #region GeodeticDensifyByDeviation - polyline
        
        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
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
          Polyline polyline = PolylineBuilder.CreatePolyline(coords, sr);

          // densify in km
          Polyline geodesicPolyline = GeometryEngine.Instance.GeodeticDensifyByDeviation(polyline, 200, LinearUnit.Kilometers, GeodeticCurveType.Geodesic) as Polyline;
          // densify in m
          geodesicPolyline = GeometryEngine.Instance.GeodeticDensifyByDeviation(polyline, 200, LinearUnit.Meters, GeodeticCurveType.Geodesic) as Polyline;

          // Change curve type to Loxodrome
          Polyline loxodromePolyline = GeometryEngine.Instance.GeodeticDensifyByDeviation(polyline, 200, LinearUnit.Meters, GeodeticCurveType.Loxodrome) as Polyline;
        });

        #endregion
      }

      public void GeodeticDensifyByLength()
      {
        #region GeodeticDensifyByLength - polygon

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
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
          Polygon polygon = PolygonBuilder.CreatePolygon(coords, sr);

          // get the geodesic lengths of the polygon segments
          ReadOnlySegmentCollection segments = polygon.Parts[0];
          List<Double> geoLengths = new List<Double>(segments.Count);
          foreach (Segment s in segments)
          {
            Polyline line = PolylineBuilder.CreatePolyline(s, sr);
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
        });

        #endregion
      }

      public void GeodeticMove()
      {
        #region Perform Geodetic Move on a set of MapPoints

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          SpatialReference sr = SpatialReferences.WebMercator;
          var points = new[] { MapPointBuilder.CreateMapPoint(0, 0, sr) };
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
        });
        #endregion
      }

      public void GetPredefinedCoordinateSystemList()
      {
        #region Retrieve coordinate systems

        // get all the geographic coordinate systems
        IReadOnlyList<CoordinateSystemListEntry> gcs_list = GeometryEngine.Instance.GetPredefinedCoordinateSystemList(CoordinateSystemFilter.GeographicCoordinateSystem);

        // get the projected coordinate systems
        IReadOnlyList<CoordinateSystemListEntry> proj_list = GeometryEngine.Instance.GetPredefinedCoordinateSystemList(CoordinateSystemFilter.ProjectedCoordinateSystem);

        // get the vertical coordinate systems
        IReadOnlyList<CoordinateSystemListEntry> vert_list = GeometryEngine.Instance.GetPredefinedCoordinateSystemList(CoordinateSystemFilter.VerticalCoordinateSystem);

        // get geographic and projected coordinate systems
        IReadOnlyList<CoordinateSystemListEntry> combined_list = GeometryEngine.Instance.GetPredefinedCoordinateSystemList(CoordinateSystemFilter.GeographicCoordinateSystem | CoordinateSystemFilter.ProjectedCoordinateSystem);

        #endregion
      }

      public void GetPredefinedGeographicTransformationList()
      {
        #region Retrieve system geographic transformations

        // a geographic transformation is the definition of how to project from one spatial reference to another
        IReadOnlyList<GeographicTransformationListEntry> list = GeometryEngine.Instance.GetPredefinedGeographicTransformationList();

        // a GeographicTransformationListEntry consists of Name, Wkid, the From SpatialReference Wkid, the To SpatialReference Wkid

        #endregion
      }

      public void GetSubCurve_GetSubCurve3D()
      {
        #region Get Sub-curve of a polyline or polygon

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          SpatialReference sr = SpatialReferences.WGS84;

          List<Coordinate2D> coords = new List<Coordinate2D>()
          {
            new Coordinate2D(-111, 72),
            new Coordinate2D(-108, 68),
            new Coordinate2D(-114, 68)
          };

          Polyline polyline = PolylineBuilder.CreatePolyline(coords, sr);

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

          Polygon polygon = PolygonBuilder.CreatePolygon(coords3D, sr);

          subCurve = GeometryEngine.Instance.GetSubCurve3D(polygon, 0, 1, AsRatioOrLength.AsLength);
          // subCurve.HasZ = true

          points = subCurve.Points;
          // points.Count = 2
          // points[0] = 0, 0, 0
          // points[1] = 0, 0.70710678118654746, 0.70710678118654746

        });
        #endregion
      }

      public void GraphicBuffer()
      {
        #region GraphicBuffer

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // mitered join and butt caps
          SpatialReference sr = SpatialReferenceBuilder.CreateSpatialReference(102010);
          List<Coordinate2D> coords = new List<Coordinate2D>()
          {
            new Coordinate2D(1400,6200),
            new Coordinate2D(1600,6300),
            new Coordinate2D(1800,6200)
          };

          Polyline polyline = PolylineBuilder.CreatePolyline(coords, sr);

          Polygon polygon = GeometryEngine.Instance.GraphicBuffer(polyline, 50, LineJoinType.Miter, LineCapType.Butt, 10, 0, -1) as Polygon;

          // bevelled join and round caps
          Envelope envelope = EnvelopeBuilder.CreateEnvelope(0, 0, 10000, 10000, SpatialReferences.WebMercator);

          Polygon outPolygon = GeometryEngine.Instance.GraphicBuffer(envelope, 1000, LineJoinType.Bevel, LineCapType.Round, 4, 0, 96) as Polygon;
        });
        #endregion

        #region GraphicBuffer Many

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // round join and round caps
          MapPoint point1 = MapPointBuilder.CreateMapPoint(0, 0);
          MapPoint point2 = MapPointBuilder.CreateMapPoint(1, 1, SpatialReferences.WGS84);
          MapPoint point3 = MapPointBuilder.CreateMapPoint(1000000, 1200000, SpatialReferences.WebMercator);
          List<MapPoint> points = new List<MapPoint>() { point1, point2, point3 };

          IReadOnlyList<Geometry> geometries = GeometryEngine.Instance.GraphicBuffer(points, 5, LineJoinType.Round, LineCapType.Round, 0, 0, 3000);
        });
       #endregion
      }

      public void Intersection()
      {
        #region Intersection between two Polylines

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // determine intersection between two polylines

          List<MapPoint> pts = new List<MapPoint>();
          pts.Add(MapPointBuilder.CreateMapPoint(1.0, 1.0));
          pts.Add(MapPointBuilder.CreateMapPoint(3.0, 3.0));
          pts.Add(MapPointBuilder.CreateMapPoint(5.0, 1.0));

          Polyline line1 = PolylineBuilder.CreatePolyline(pts);

          List<MapPoint> pts2 = new List<MapPoint>();
          pts2.Add(MapPointBuilder.CreateMapPoint(1.0, 3.0));
          pts2.Add(MapPointBuilder.CreateMapPoint(3.0, 1.0));
          pts2.Add(MapPointBuilder.CreateMapPoint(5.0, 3.0));

          Polyline line2 = PolylineBuilder.CreatePolyline(pts2);

          bool intersects = GeometryEngine.Instance.Intersects(line1, line2);    // intersects = true
          Geometry g = GeometryEngine.Instance.Intersection(line1, line2, GeometryDimension.esriGeometry0Dimension);
          Multipoint resultMultipoint = g as Multipoint;

          // result is a multiPoint that intersects at (2,2) and (4,2)
        });
        #endregion

        #region Intersection between two Polygons

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // determine intersection between two polygons

          Envelope env1 = EnvelopeBuilder.CreateEnvelope(new Coordinate2D(3.0, 2.0), new Coordinate2D(6.0, 6.0));
          Polygon poly1 = PolygonBuilder.CreatePolygon(env1);

          Envelope env2 = EnvelopeBuilder.CreateEnvelope(new Coordinate2D(1.0, 1.0), new Coordinate2D(4.0, 4.0));
          Polygon poly2 = PolygonBuilder.CreatePolygon(env2);

          Polygon polyResult = GeometryEngine.Instance.Intersection(poly1, poly2) as Polygon;
        });
        #endregion
      }

      public void LabelPoint()
      {
        #region Determine label point for a Polygon

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // create a polygon
          List<Coordinate2D> list2D = new List<Coordinate2D>();
          list2D.Add(new Coordinate2D(1.0, 1.0));
          list2D.Add(new Coordinate2D(1.0, 2.0));
          list2D.Add(new Coordinate2D(2.0, 2.0));
          list2D.Add(new Coordinate2D(2.0, 1.0));

          Polygon polygon = PolygonBuilder.CreatePolygon(list2D);
          bool isSimple = GeometryEngine.Instance.IsSimpleAsFeature(polygon);
          MapPoint pt = GeometryEngine.Instance.LabelPoint(polygon); 
        });
        #endregion
      }

      public void M_Functions()
      {
        // GetMinMaxM, GetMMonotonic, GetPointsAtM, GetSubCurveBetweenMs, GetNormalsAtM, SetMsAsDistance, SetAndInterpolateMsBetween

        #region Get the minimum and maximum M values (GetMinMaxM)

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          string json = "{\"hasM\":true,\"rings\":[[[-3000,-2000,10],[-2000,-2000,15],[-1000,-2000,20],[0,-2000,0],[1000,-2000,-20],[2000,-2000,-30],[3000,-2000,10],[4000,-2000,5]]],\"spatialReference\":{\"wkid\":3857}}";
          Polygon polygon = PolygonBuilder.FromJson(json);

          double minM, maxM;
          GeometryEngine.Instance.GetMinMaxM(polygon, out minM, out maxM);
          // minM = -30 
          // maxM = 20

          json = "{\"hasM\":true,\"paths\":[[[-3000,-2000,10],[-2000,-2000,null],[-1000,-2000,null]]]}";
          Polyline polyline = PolylineBuilder.FromJson(json);

          GeometryEngine.Instance.GetMinMaxM(polyline, out minM, out maxM);
          // minM = 10
          // maxM = 10

          json = "{\"hasM\":true,\"paths\":[[[-3000,-2000,null],[-2000,-2000,null],[-1000,-2000,null]]]}";
          polyline = PolylineBuilder.FromJson(json);

          GeometryEngine.Instance.GetMinMaxM(polyline, out minM, out maxM);
          // minM = double.Nan
          // maxM = double.Nan
        });

        #endregion

        #region Determine whether Ms are monotonic and whether ascending or descending (GetMMonotonic)

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          string json = "{\"hasM\":true,\"paths\":[[[-3000,-2000,10],[-2000,-2000,15],[-1000,-2000,20]]]}";
          Polyline polyline = PolylineBuilder.FromJson(json);

          Monotonic monotonic = GeometryEngine.Instance.GetMMonotonic(polyline);
          // monotonic = Monotonic.Ascending

          json = "{\"hasM\":true,\"paths\":[[[-3000,-2000,10],[-2000,-2000,5],[-1000,-2000,0]]]}";
          polyline = PolylineBuilder.FromJson(json);

          monotonic = GeometryEngine.Instance.GetMMonotonic(polyline);
          // monotonic = Monotonic.Descending

          json = "{\"hasM\":true,\"paths\":[[[-3000,-2000,10],[-2000,-2000,15],[-1000,-2000,0]]]}";
          polyline = PolylineBuilder.FromJson(json);

          monotonic = GeometryEngine.Instance.GetMMonotonic(polyline);
          // monotonic = Monotonic.NotMonotonic

        });

        #endregion

        #region Get a multipoint corresponding to the locations where the specified M values occur along the geometry (GetPointsAtM)

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          string json = "{\"hasM\":true,\"paths\":[[[-3000,-2000,10],[-2000,-2000,15],[-1000,-2000,20],[0,-2000,0],[1000,-2000,20],[2000,-2000,30],[3000,-2000,10],[4000,-2000,5]]],\"spatialReference\":{\"wkid\":3857}}";
          Polyline polyline = PolylineBuilder.FromJson(json);

          Multipoint multipoint = GeometryEngine.Instance.GetPointsAtM(polyline, 10, 500);
          // multiPoint.PointCount = 4
          // multipoint.Points[0]  X= -3000, Y= -2500  M= 10
          // multipoint.Points[1]  X= -500, Y= -2500  M= 10
          // multipoint.Points[2]  X= 500, Y= -2500  M= 10
          // multipoint.Points[3]  X= 3000, Y= -2500  M= 10

        });

        #endregion

        #region Get a polyline corresponding to the subcurve(s) between specified M values (GetSubCurveBetweenMs)
        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          string json = "{\"hasM\":true,\"paths\":[[[-2000,0,1],[-1000,1000,2],[-1000,0,3],[1000,1000,4],[2000,1000,5],[2000,2000,6],[3000,2000,7],[4000,0,8]]],\"spatialReference\":{\"wkid\":3857}}";
          Polyline polyline = PolylineBuilder.FromJson(json);

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

        });
        #endregion

        #region Get line segments corresponding to the normal at the locations where the specified M values occur along the geometry (GetNormalsAtM)

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {

          IList<MapPoint> inPoints = new List<MapPoint>()
          {
            MapPointBuilder.CreateMapPoint(-3000, -2000, 0, 100),
            MapPointBuilder.CreateMapPoint(-3000, 0, 0, 200),
            MapPointBuilder.CreateMapPoint(-1000, 0, 0, 300),
            MapPointBuilder.CreateMapPoint(-1000, 2000, 0, 100),
            MapPointBuilder.CreateMapPoint(3000, 2000, 0, 200),
            MapPointBuilder.CreateMapPoint(3000, 0, 0, 300),
            MapPointBuilder.CreateMapPoint(1000, 0, 0, 100),
            MapPointBuilder.CreateMapPoint(1000, -2000, 0, 200),
            MapPointBuilder.CreateMapPoint(-3000, -2000, 0, 300)
          };

          using (PolygonBuilder builder = new PolygonBuilder(inPoints))
          {
            builder.HasM = true;
            Polygon polygon = builder.ToGeometry();

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
          }
        });

        #endregion

        #region Set M values to the cumulative length from the start of the multipart (SetMsAsDistance)

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          string json = "{\"hasM\":true,\"rings\":[[[0,0],[0,3000],[4000,3000],[4000,0],[0,0]]],\"spatialReference\":{\"wkid\":3857}}";
          Polygon polygon = PolygonBuilder.FromJson(json);

          Polygon outPolygon = GeometryEngine.Instance.SetMsAsDistance(polygon, AsRatioOrLength.AsLength) as Polygon;
          ReadOnlyPointCollection outPoints = outPolygon.Points;
          // outPoints M values are { 0, 3000, 7000, 10000, 14000 };
        });

        #endregion

        #region Set Ms at the beginning and end of the geometry and interpolate M values between the two values (SetAndInterpolateMsBetween)

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          string json = "{\"hasM\":true,\"paths\":[[[-3000,-2000],[-2000,-2000],[-1000,-2000],[0,-2000],[1000,-2000],[2000,-2000],[3000,-2000],[4000,-2000]]],\"spatialReference\":{\"wkid\":3857}}";
          Polyline polyline = PolylineBuilder.FromJson(json);

          Polyline outPolyline = GeometryEngine.Instance.SetAndInterpolateMsBetween(polyline, 100, 800) as Polyline;
          ReadOnlyPointCollection outPoints = outPolyline.Points;
          // outPoints M values are { 100, 200, 300, 400, 500, 600, 700, 800 };
        });

        #endregion
      }

      public void Move()
      {
        #region Move a MapPoint

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          MapPoint pt = MapPointBuilder.CreateMapPoint(1.0, 3.0);

          MapPoint ptResult = GeometryEngine.Instance.Move(pt, -3.5, 2.5) as MapPoint;
          // ptResult is (-2.5, 5.5)
        });
        #endregion

        #region Move a z-aware MapPoint

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          MapPoint zPt = MapPointBuilder.CreateMapPoint(1.0, 3.0, 2.0);
          MapPoint zPtResult = GeometryEngine.Instance.Move(zPt, 4, 0.25, 0.5) as MapPoint;
          // zPtResult is (5.0, 3.25, 2.5);
        });
        #endregion

        #region Move a Polyline
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          List<MapPoint> pts = new List<MapPoint>();
          pts.Add(MapPointBuilder.CreateMapPoint(1.0, 1.0, 3.0));
          pts.Add(MapPointBuilder.CreateMapPoint(3.0, 3.0, 3.0));
          pts.Add(MapPointBuilder.CreateMapPoint(3, 2, 3.0));
          pts.Add(MapPointBuilder.CreateMapPoint(4.0, 2.0, 3.0));

          Polyline polyline = PolylineBuilder.CreatePolyline(pts);

          Geometry g = GeometryEngine.Instance.Move(polyline, 3, 2);
          Polyline polylineResult = g as Polyline;

          // polylineResult.Points[0] = 4.0, 3.0, 3.0
          // polylineResult.Points[1] = 6.0, 5.0, 3.0
          // polylineResult.Points[2] = 6.0, 4.0, 3.0
          // polylineResult.Points[3] = 7.0, 4.0, 3.0
        });
        #endregion
      }

      public void MovePointAlongLine()
      {
        #region MovePointAlongLine

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          LineSegment line = LineBuilder.CreateLineSegment(MapPointBuilder.CreateMapPoint(0, 3), MapPointBuilder.CreateMapPoint(5.0, 3.0));
          Polyline polyline = PolylineBuilder.CreatePolyline(line);
          bool simple = GeometryEngine.Instance.IsSimpleAsFeature(polyline);

          // ratio = false
          MapPoint pt = GeometryEngine.Instance.MovePointAlongLine(polyline, 1.0, false, 0.0, SegmentExtension.NoExtension);
          // pt = 1.0, 3.0

          pt = GeometryEngine.Instance.MovePointAlongLine(polyline, 1.0, false, -1.0, SegmentExtension.NoExtension);
          // pt = 1.0, 4.0

          pt = GeometryEngine.Instance.MovePointAlongLine(polyline, 1.0, false, 2.0, SegmentExtension.NoExtension);
          // pt = 1.0, 1.0

          // ratio = true
          pt = GeometryEngine.Instance.MovePointAlongLine(polyline, 0.5, true, 0, SegmentExtension.NoExtension);
          // pt = 2.5, 3.0

          // move past the line
          pt = GeometryEngine.Instance.MovePointAlongLine(polyline, 7, false, 0, SegmentExtension.NoExtension);
          // pt = 5.0, 3.0

          // move past the line with extension at "to" point
          pt = GeometryEngine.Instance.MovePointAlongLine(polyline, 7, false, 0, SegmentExtension.ExtendEmbeddedAtTo);
          // pt = 7.0, 3.0

          // negative distance with extension at "from" point
          pt = GeometryEngine.Instance.MovePointAlongLine(polyline, -2, false, 0, SegmentExtension.ExtendEmbeddedAtFrom);
          // pt = -2.0, 3.0

          // ratio = true
          pt = GeometryEngine.Instance.MovePointAlongLine(polyline, 0.5, true, 0, SegmentExtension.NoExtension);
          // pt = 2.5, 3.0


          // line with Z
          List<Coordinate3D> coords3D = new List<Coordinate3D> { new Coordinate3D(0, 0, 0), new Coordinate3D(1113195, 1118890, 5000) };
          Polyline polylineZ = PolylineBuilder.CreatePolyline(coords3D, SpatialReferences.WebMercator);
          // polylineZ.HasZ = true

          // ratio = true, no offset
          pt = GeometryEngine.Instance.MovePointAlongLine(polylineZ, 0.5, true, 0, SegmentExtension.NoExtension);
          // pt.X = 556597.5
          // pt.Y = 559445
          // pt.Z = 2500

          // ratio = true, past the line with "to" extension, no offset
          pt = GeometryEngine.Instance.MovePointAlongLine(polylineZ, 1.5, true, 0, SegmentExtension.ExtendEmbeddedAtTo);
          // pt.X = 1669792.5
          // pt.Y = 1678335
          // pt.Z = 7500

          // ratio = true, negative distance past the line with no extension, no offset
          pt = GeometryEngine.Instance.MovePointAlongLine(polylineZ, -1.5, true, 0, SegmentExtension.NoExtension);
          // pt.X = 0
          // pt.Y = 0
          // pt.Z = -7500


          // polyline with Z but 2d distance = 0
          MapPoint pt3 = MapPointBuilder.CreateMapPoint(5, 5, 0);
          MapPoint pt4 = MapPointBuilder.CreateMapPoint(5, 5, 10);
          List<MapPoint> pts = new List<MapPoint>() { pt3, pt4 };

          using (PolylineBuilder pb = new PolylineBuilder())
          {
            pb.SetEmpty();
            pb.AddPart(pts);
            pb.HasZ = true;
            polyline = pb.ToGeometry();
            // polyline.Length3D = 10
            // polyline.Length = 0

            MapPoint result = GeometryEngine.Instance.MovePointAlongLine(polyline, 2, false, 0, SegmentExtension.NoExtension);
            // result = 5, 5, 2

            // polyline with length2d = 0 and length3d = 0
            MapPoint pt5 = MapPointBuilder.CreateMapPoint(5, 5, 10);
            MapPoint pt6 = MapPointBuilder.CreateMapPoint(5, 5, 10);
            pts.Clear();
            pts.Add(pt5);
            pts.Add(pt6);

            pb.SetEmpty();
            pb.AddPart(pts);
            pb.HasZ = true;
            polyline = pb.ToGeometry();
            // polyline.Length3D = 0
            // polyline.Length = 0

            result = GeometryEngine.Instance.MovePointAlongLine(polyline, 3, true, 0, SegmentExtension.NoExtension);
            // result = 5, 5, 10

            result = GeometryEngine.Instance.MovePointAlongLine(polyline, 3, true, 0, SegmentExtension.ExtendEmbeddedAtFrom);
            // result = 5, 5, 10

          }


          // polyline with Z and M
          List<MapPoint> inputPoints = new List<MapPoint>()
          {
            MapPointBuilder.CreateMapPoint(1, 2, 3, 4),
            MapPointBuilder.CreateMapPoint(1, 2, 33, 44),
          };

          Polyline polylineZM = PolylineBuilder.CreatePolyline(inputPoints, SpatialReferences.WGS84);
          // polylineZM.HasZ = true
          // polylineZM.HasM = true

          // ratio = true, no offset
          MapPoint pointAlong = GeometryEngine.Instance.MovePointAlongLine(polylineZM, 0.5, true, 0, SegmentExtension.NoExtension);
          // pointAlong = 1, 2, 18, 24

          // ratio = true with offset
          pointAlong = GeometryEngine.Instance.MovePointAlongLine(polylineZM, 0.2, true, 2.23606797749979, SegmentExtension.NoExtension);
          // pointAlong = 1, 2, 9, 12
        });
        #endregion
      }

      public void MultipartToSinglePart()
      {
        Polygon multipartPolygon = null;

        #region Separate components of a geometry into single component geometries

        List<Coordinate2D> coords2D = new List<Coordinate2D>()
        {
          new Coordinate2D(0, 0),
          new Coordinate2D(1, 4),
          new Coordinate2D(2, 7),
          new Coordinate2D(-10, 3)
        };

        Multipoint multipoint = MultipointBuilder.CreateMultipoint(coords2D, SpatialReferences.WGS84);

        IReadOnlyList<Geometry> result = GeometryEngine.Instance.MultipartToSinglePart(multipoint);
        // result.Count = 4, 


        // 'explode' a multipart polygon
        result = GeometryEngine.Instance.MultipartToSinglePart(multipartPolygon);


        // create a bag of geometries
        Polygon polygon = PolygonBuilder.CreatePolygon(coords2D, SpatialReferences.WGS84);
        GeometryBag bag = GeometryBagBuilder.CreateGeometryBag(new List<Geometry>() { multipoint, polygon });
        // bag.PartCount = 2
        
        result = GeometryEngine.Instance.MultipartToSinglePart(bag);
        // result.Count = 2
        // result[0] is MultiPoint
        // result[1] is Polygon

      #endregion
      }

      public void NearestPoint_NearestVertex()
      {
        #region Nearest Point versus Nearest Vertex

        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          SpatialReference sr = SpatialReferences.WGS84;
          MapPoint pt = MapPointBuilder.CreateMapPoint(5, 5, sr);

          List<Coordinate2D> coords = new List<Coordinate2D>()
          {
            new Coordinate2D(10, 1),
            new Coordinate2D(10, -4),
            new Coordinate2D(0, -4),
            new Coordinate2D(0, 1),
            new Coordinate2D(10, 1)
          };

          Polygon polygon = PolygonBuilder.CreatePolygon(coords);

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

        });
        #endregion
      }

      public void NearestPoint3D()
      {
        #region Determine Nearest Point in 3D

        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          MapPoint pt1 = MapPointBuilder.CreateMapPoint(1, 1, 1);
          MapPoint pt2 = MapPointBuilder.CreateMapPoint(2, 2, 2);
          MapPoint pt3 = MapPointBuilder.CreateMapPoint(10, 2, 1);

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
          using (MultipointBuilder mpb = new MultipointBuilder(pt1))
          {
            mpb.Add(pt2);
            mpb.HasZ = true;
            Multipoint multipoint = mpb.ToGeometry();
            result = GeometryEngine.Instance.NearestPoint3D(multipoint, pt3);

            // result.Point = 2, 2, 2
            // result.Distance = Math.Sqrt(65)
            // result.SegmentIndex = null
            // result.PartIndex = 1
            // result.PointIndex = 1
            // result.RightSide = false
          }
        });
        #endregion
      }

      public void Offset()
      {
        #region Calculate a geometry offset from the source

        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          List<MapPoint> linePts = new List<MapPoint>();
          linePts.Add(MapPointBuilder.CreateMapPoint(1.0, 1.0, SpatialReferences.WGS84));
          linePts.Add(MapPointBuilder.CreateMapPoint(10.0, 1.0, SpatialReferences.WGS84));

          Polyline polyline = PolylineBuilder.CreatePolyline(linePts);

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

          EllipticArcBuilder cab = new EllipticArcBuilder(fromPt.ToMapPoint(), toPt.ToMapPoint(), interiorPt);
          EllipticArcSegment circularArc = cab.ToSegment();

          polyline = PolylineBuilder.CreatePolyline(circularArc);
          g = GeometryEngine.Instance.Offset(polyline, -0.25, OffsetType.Miter, 0.5);
          gResult = g as Polyline;

          g = GeometryEngine.Instance.Offset(polyline, 0.25, OffsetType.Bevel, 0.5);
          gResult = g as Polyline;


          //
          //  offset for a polygon
          //
          List<MapPoint> list = new List<MapPoint>();
          list.Add(MapPointBuilder.CreateMapPoint(10.0, 10.0, SpatialReferences.WGS84));
          list.Add(MapPointBuilder.CreateMapPoint(10.0, 20.0, SpatialReferences.WGS84));
          list.Add(MapPointBuilder.CreateMapPoint(20.0, 20.0, SpatialReferences.WGS84));
          list.Add(MapPointBuilder.CreateMapPoint(20.0, 10.0, SpatialReferences.WGS84));

          Polygon polygon = PolygonBuilder.CreatePolygon(list);

          g = GeometryEngine.Instance.Offset(polygon, 2, OffsetType.Square, 0);
          Polygon gPolygon = g as Polygon;

          g = GeometryEngine.Instance.Offset(polygon, -2, OffsetType.Round, 0.3);
          gPolygon = g as Polygon;

          g = GeometryEngine.Instance.Offset(polygon, -0.5, OffsetType.Miter, 0.6);
          gPolygon = g as Polygon;
        });

        #endregion
      }

      public void Overlaps()
      {
        #region Determine if geometries overlap
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          MapPoint pt1 = MapPointBuilder.CreateMapPoint(1.5, 1.5);
          MapPoint pt2 = MapPointBuilder.CreateMapPoint(1.25, 1.75);
          MapPoint pt3 = MapPointBuilder.CreateMapPoint(3, 1.5);
          MapPoint pt4 = MapPointBuilder.CreateMapPoint(1.5, 2);

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
          Polyline polyline = PolylineBuilder.CreatePolyline(pts);
          bool isSimple = GeometryEngine.Instance.IsSimpleAsFeature(polyline);         // isSimple = true
          overlaps = GeometryEngine.Instance.Overlaps(polyline, pt1);                  // overlaps = false

          //
          // line and line
          //
          Polyline polyline2 = PolylineBuilder.CreatePolyline(pts2);
          isSimple = GeometryEngine.Instance.IsSimpleAsFeature(polyline2);             // isSimple = true
          overlaps = GeometryEngine.Instance.Overlaps(polyline, polyline2);            // overlaps = true

        });
        #endregion
      }

      public void Project()
      {
        #region Project from WGS84 to WebMercator

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          MapPoint pt = MapPointBuilder.CreateMapPoint(1.0, 3.0, SpatialReferences.WGS84);
          Geometry result = GeometryEngine.Instance.Project(pt, SpatialReferences.WebMercator);
          MapPoint projectedPt = result as MapPoint;
        });
        #endregion

        #region Project from WGS84

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // create the polygon
          List<MapPoint> pts = new List<MapPoint>();
          pts.Add(MapPointBuilder.CreateMapPoint(1.0, 1.0, SpatialReferences.WGS84));
          pts.Add(MapPointBuilder.CreateMapPoint(1.0, 2.0, SpatialReferences.WGS84));
          pts.Add(MapPointBuilder.CreateMapPoint(2.0, 2.0, SpatialReferences.WGS84));
          pts.Add(MapPointBuilder.CreateMapPoint(2.0, 1.0, SpatialReferences.WGS84));

          Polygon polygon = PolygonBuilder.CreatePolygon(pts);
          // ensure it is simple
          bool isSimple = GeometryEngine.Instance.IsSimpleAsFeature(polygon);

          // create the spatial reference to project to
          SpatialReference northPole = SpatialReferenceBuilder.CreateSpatialReference(102018);   // north pole stereographic 

          // project
          Geometry g = GeometryEngine.Instance.Project(polygon, northPole);
        });
        #endregion
      }

      public void QueryNormal()
      {
        #region QueryNormal

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          string json = "{\"curvePaths\":[[[-13046586.8335,4036570.6796000004]," +
                        "{\"c\":[[-13046645.107099999,4037152.5873000026]," +
                        "[-13046132.776277589,4036932.1325614937]]}]],\"spatialReference\":{\"wkid\":3857}}";
          Polyline polyline = PolylineBuilder.FromJson(json);

          EllipticArcSegment arc = polyline.Parts[0][0] as EllipticArcSegment;

          // No extension, distanceAlongCurve = 0.5

          // use the polyline
          Polyline poly_normal = GeometryEngine.Instance.QueryNormal(polyline, SegmentExtension.NoExtension, 0.5, AsRatioOrLength.AsRatio, 1000);
          // or a segment
          LineSegment seg_normal = GeometryEngine.Instance.QueryNormal(arc, SegmentExtension.NoExtension, 0.5, AsRatioOrLength.AsRatio, 1000);

          // TangentAtFrom, distanceAlongCurve = -1.2
          poly_normal = GeometryEngine.Instance.QueryNormal(polyline, SegmentExtension.ExtendTangentAtFrom, -1.2, AsRatioOrLength.AsRatio, 1000);
          seg_normal = GeometryEngine.Instance.QueryNormal(arc, SegmentExtension.ExtendTangentAtFrom, -1.2, AsRatioOrLength.AsRatio, 1000);

          // TangentAtTo (ignored because distanceAlongCurve < 0), distanceAlongCurve = -1.2
          poly_normal = GeometryEngine.Instance.QueryNormal(polyline, SegmentExtension.ExtendTangentAtTo, -1.2, AsRatioOrLength.AsRatio, 1000);
          seg_normal = GeometryEngine.Instance.QueryNormal(arc, SegmentExtension.ExtendTangentAtTo, -1.2, AsRatioOrLength.AsRatio, 1000);

          // TangentAtTo, distanceAlongCurve = 1.2
          poly_normal = GeometryEngine.Instance.QueryNormal(polyline, SegmentExtension.ExtendTangentAtTo, 1.2, AsRatioOrLength.AsRatio, 1000);
          seg_normal = GeometryEngine.Instance.QueryNormal(arc, SegmentExtension.ExtendTangentAtTo, 1.2, AsRatioOrLength.AsRatio, 1000);

          // TangentAtFrom (ignored because distanceAlongCurve > 0), distanceAlongCurve = 1.2
          poly_normal = GeometryEngine.Instance.QueryNormal(polyline, SegmentExtension.ExtendTangentAtFrom, 1.2, AsRatioOrLength.AsRatio, 1000);
          seg_normal = GeometryEngine.Instance.QueryNormal(arc, SegmentExtension.ExtendTangentAtFrom, 1.2, AsRatioOrLength.AsRatio, 1000);

          // EmbeddedAtTo, distanceAlongCurve = 1.2
          poly_normal = GeometryEngine.Instance.QueryNormal(polyline, SegmentExtension.ExtendEmbeddedAtTo, 1.2, AsRatioOrLength.AsRatio, 1000);
          seg_normal = GeometryEngine.Instance.QueryNormal(arc, SegmentExtension.ExtendEmbeddedAtTo, 1.2, AsRatioOrLength.AsRatio, 1000);

          // EmbeddedAtFrom, distanceAlongCurve = -0.2
          poly_normal = GeometryEngine.Instance.QueryNormal(polyline, SegmentExtension.ExtendEmbeddedAtFrom, -0.2, AsRatioOrLength.AsRatio, 1000);
          seg_normal = GeometryEngine.Instance.QueryNormal(arc, SegmentExtension.ExtendEmbeddedAtFrom, -0.2, AsRatioOrLength.AsRatio, 1000);
        });
        #endregion
      }

      public void QueryPoint()
      {
        #region QueryPoint
        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          SpatialReference sr = SpatialReferences.WGS84;

          // Horizontal line segment
          Coordinate2D start = new Coordinate2D(1, 1);
          Coordinate2D end = new Coordinate2D(11, 1);
          LineSegment line = LineBuilder.CreateLineSegment(start, end, sr);

          Polyline polyline = PolylineBuilder.CreatePolyline(line);

          // Don't extend the segment

          MapPoint outPoint = GeometryEngine.Instance.QueryPoint(polyline, SegmentExtension.NoExtension, 1.0, AsRatioOrLength.AsLength);
          // outPoint = (2, 1)

          // or the segment
          MapPoint outPoint_seg = GeometryEngine.Instance.QueryPoint(line, SegmentExtension.NoExtension, 1.0, AsRatioOrLength.AsLength);
          // outPoint_seg = (2, 1)

          // Extend infinitely in both directions
          outPoint = GeometryEngine.Instance.QueryPoint(polyline, SegmentExtension.ExtendTangents, 1.5, AsRatioOrLength.AsRatio);
          // outPoint = (16, 1)
          outPoint_seg = GeometryEngine.Instance.QueryPoint(line, SegmentExtension.ExtendTangents, 1.5, AsRatioOrLength.AsRatio);
          // outPoint_seg = (16, 1)
        });
        #endregion
      }

      public void QueryPointAndDistance()
      {
        #region QueryPointAndDistance

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // Horizontal line segment
          List<MapPoint> linePts = new List<MapPoint>();
          linePts.Add(MapPointBuilder.CreateMapPoint(1.0, 1.0, SpatialReferences.WGS84));
          linePts.Add(MapPointBuilder.CreateMapPoint(11.0, 1.0, SpatialReferences.WGS84));
          Polyline polyline = PolylineBuilder.CreatePolyline(linePts);
          bool isSimple = GeometryEngine.Instance.IsSimpleAsFeature(polyline);

          // Don't extent the segment
          SegmentExtension extension = SegmentExtension.NoExtension;

          // A point on the line segment
          MapPoint inPoint = MapPointBuilder.CreateMapPoint(2, 1, SpatialReferences.WGS84);

          double distanceAlongCurve, distanceFromCurve;
          LeftOrRightSide whichSide;
          AsRatioOrLength asRatioOrLength = AsRatioOrLength.AsLength;

          MapPoint outPoint = GeometryEngine.Instance.QueryPointAndDistance(polyline, extension, inPoint, asRatioOrLength, out distanceAlongCurve, out distanceFromCurve, out whichSide);
          // outPoint = 2, 1
          // distanceAlongCurve = 1
          // distanceFromCurve = 0
          // whichSide = GeometryEngine.Instance.LeftOrRightSide.LeftSide


          // Extend infinitely in both directions
          extension = SegmentExtension.ExtendTangents;

          // A point on the left side
          inPoint = MapPointBuilder.CreateMapPoint(16, 6, SpatialReferences.WGS84);
          asRatioOrLength = AsRatioOrLength.AsRatio;

          outPoint = GeometryEngine.Instance.QueryPointAndDistance(polyline, extension, inPoint, asRatioOrLength, out distanceAlongCurve, out distanceFromCurve, out whichSide);
          // outPoint = 16, 1
          // distanceAlongCurve = 1.5
          // distanceFromCurve = 5
          // whichSide = GeometryEngine.Instance.LeftOrRightSide.LeftSide

        });
        #endregion
      }

      public void QueryTangent()
      {
        #region QueryTangent
        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          LineSegment line = LineBuilder.CreateLineSegment(new Coordinate2D(0, 0), new Coordinate2D(1, 0));

          // No extension, distanceAlongCurve = 0.5
          LineSegment tangent = GeometryEngine.Instance.QueryTangent(line, SegmentExtension.NoExtension, 0.5, AsRatioOrLength.AsRatio, 1);
          // tangent.StartCoordinate = (0.5, 0.0)
          // tangent.EndCoordinate = (1.5, 0.0)

          tangent = GeometryEngine.Instance.QueryTangent(line, SegmentExtension.NoExtension, 1.5, AsRatioOrLength.AsLength, 1);
          // tangent.StartCoordinate = (1.0, 0.0)
          // tangent.EndCoordinate = (2.0, 0.0)

          tangent = GeometryEngine.Instance.QueryTangent(line, SegmentExtension.ExtendTangentAtTo, 1.5, AsRatioOrLength.AsLength, 1);
          // tangent.StartCoordinate = (1.5, 0.0)
          // tangent.EndCoordinate = (2.5, 0.0)

          tangent = GeometryEngine.Instance.QueryTangent(line, SegmentExtension.ExtendTangentAtFrom, -1.5, AsRatioOrLength.AsLength, 1);
          // tangent.StartCoordinate = (-1.5, 0.0)
          // tangent.EndCoordinate = (-0.5, 0.0)

          tangent = GeometryEngine.Instance.QueryTangent(line, SegmentExtension.ExtendTangentAtFrom, -0.5, AsRatioOrLength.AsRatio, 1);
          // tangent.StartCoordinate = (-0.5, 0.0)
          // tangent.EndCoordinate = (0.5, 0.0)
        });
        #endregion
      }

      public void Related()
      {
        #region Determine relationship between two geometries

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // set up some geometries

          // points
          MapPoint point0 = MapPointBuilder.CreateMapPoint(0, 0, SpatialReferences.WGS84);
          MapPoint point1 = MapPointBuilder.CreateMapPoint(1, 1, SpatialReferences.WGS84);
          MapPoint point2 = MapPointBuilder.CreateMapPoint(-5, 5, SpatialReferences.WGS84);

          // multipoint
          List<MapPoint> points = new List<MapPoint>() { point0, point1, point2 };
          Multipoint multipoint = MultipointBuilder.CreateMultipoint(points, SpatialReferences.WGS84);

          // polygon 
          List<Coordinate2D> polygonCoords = new List<Coordinate2D>()
          {
            new Coordinate2D(-10, 0),
            new Coordinate2D(0, 10),
            new Coordinate2D(10, 0),
            new Coordinate2D(-10, 0)
          };
          Polygon polygon = PolygonBuilder.CreatePolygon(polygonCoords, SpatialReferences.WGS84);

          // polylines
          Polyline polyline1 = PolylineBuilder.CreatePolyline(LineBuilder.CreateLineSegment(new Coordinate2D(-9.1, 0.1), new Coordinate2D(0, 9)), SpatialReferences.WGS84);
          Polyline polyline2 = PolylineBuilder.CreatePolyline(LineBuilder.CreateLineSegment(new Coordinate2D(-5, 5), new Coordinate2D(0, 5)), SpatialReferences.WGS84);
          Polyline polyline3 = PolylineBuilder.CreatePolyline(LineBuilder.CreateLineSegment(new Coordinate2D(2.09, -2.04), new Coordinate2D(5, 10)), SpatialReferences.WGS84);
          Polyline polyline4 = PolylineBuilder.CreatePolyline(LineBuilder.CreateLineSegment(new Coordinate2D(10, -5), new Coordinate2D(10, 5)), SpatialReferences.WGS84);

          List<Segment> segments = new List<Segment>()
          {
            LineBuilder.CreateLineSegment(new Coordinate2D(5.05, -2.87), new Coordinate2D(6.35, 1.57)),
            LineBuilder.CreateLineSegment(new Coordinate2D(6.35, 1.57), new Coordinate2D(4.13, 2.59)),
            LineBuilder.CreateLineSegment(new Coordinate2D(4.13, 2.59), new Coordinate2D(5, 5))
          };
          Polyline polyline5 = PolylineBuilder.CreatePolyline(segments, SpatialReferences.WGS84);

          segments.Add(LineBuilder.CreateLineSegment(new Coordinate2D(5, 5), new Coordinate2D(10, 10)));

          Polyline polyline6 = PolylineBuilder.CreatePolyline(segments, SpatialReferences.WGS84);
          Polyline polyline7 = PolylineBuilder.CreatePolyline(polyline5);
          Polyline polyline8 = PolylineBuilder.CreatePolyline(LineBuilder.CreateLineSegment(new Coordinate2D(5, 5), new Coordinate2D(10, 10)), SpatialReferences.WGS84);

          segments.Clear();
          segments.Add(LineBuilder.CreateLineSegment(new Coordinate2D(0.6, 3.5), new Coordinate2D(0.7, 7)));
          segments.Add(LineBuilder.CreateLineSegment(new Coordinate2D(0.7, 7), new Coordinate2D(3, 9)));

          Polyline polyline9 = PolylineBuilder.CreatePolyline(segments, SpatialReferences.WGS84);

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
        });

        #endregion
      }

      public void ReplaceNaNZs()
      {
        #region Replace NaN Zs in a polygon

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          List<Coordinate3D> coordsZ = new List<Coordinate3D>() 
          { 
            new Coordinate3D(1, 2, double.NaN), 
            new Coordinate3D(4, 5, 3), 
            new Coordinate3D(7, 8, double.NaN) 
          };

          Polygon polygon = PolygonBuilder.CreatePolygon(coordsZ);
          // polygon.HasZ = true

          Polygon polygonZReplaced = GeometryEngine.Instance.ReplaceNaNZs(polygon, -1) as Polygon;

          // polygonZReplaced.Points[0].Z = -1
          // polygonZReplaced.Points[1].Z = 3
          // polygonZReplaced.Points[2].Z = -1
        });

        #endregion
      }

      public void ReverseOrientation()
      {
        #region Reverse the order of points in a Polygon

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          List<Coordinate2D> list2D = new List<Coordinate2D>();
          list2D.Add(new Coordinate2D(1.0, 1.0));
          list2D.Add(new Coordinate2D(1.0, 2.0));
          list2D.Add(new Coordinate2D(2.0, 2.0));
          list2D.Add(new Coordinate2D(2.0, 1.0));

          Polygon polygon = PolygonBuilder.CreatePolygon(list2D);

          Geometry g = GeometryEngine.Instance.ReverseOrientation(polygon);
          Polygon gPolygon = g as Polygon;

          // gPolygon.Points[0] = 1.0, 1.0
          // gPolygon.Points[1] = 2.0, 1.0
          // gPolygon.Points[2] = 2.0, 2.0
          // gPolygon.Points[3] = 1.0, 2.0
          // gPolygon.Points[4] = 1.0, 1.0
          // gPolygon.Area = -1 * polygon.Area
        });
        #endregion      
      }

      public void Rotate()
      {
        #region Rotate a MapPoint

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          MapPoint pt = MapPointBuilder.CreateMapPoint(1.0, 3.0);
          MapPoint rotatePt = MapPointBuilder.CreateMapPoint(3.0, 3.0);

          Geometry result = GeometryEngine.Instance.Rotate(pt, rotatePt, Math.PI / 2);
          // result point is (3, 1)
        });
        #endregion

        #region Rotate a Polyline

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // rotate a polyline

          MapPoint fixedPt = MapPointBuilder.CreateMapPoint(3.0, 3.0);
          
          List<MapPoint> pts = new List<MapPoint>();
          pts.Add(MapPointBuilder.CreateMapPoint(1.0, 1.0));
          pts.Add(MapPointBuilder.CreateMapPoint(1.0, 5.0));
          pts.Add(MapPointBuilder.CreateMapPoint(5, 5));
          pts.Add(MapPointBuilder.CreateMapPoint(5.0, 1.0));
          Polyline polyline = PolylineBuilder.CreatePolyline(pts);

          Polyline rotated = GeometryEngine.Instance.Rotate(polyline, fixedPt, Math.PI / 4) as Polyline;  // rotate 45 deg
        });
        #endregion
      }

      public void Scale()
      {
        #region Scale a geometry

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          List<MapPoint> pts = new List<MapPoint>();
          pts.Add(MapPointBuilder.CreateMapPoint(1.0, 1.0, 3.0));
          pts.Add(MapPointBuilder.CreateMapPoint(1.0, 3.0, 3.0));
          pts.Add(MapPointBuilder.CreateMapPoint(3, 3, 3.0));
          pts.Add(MapPointBuilder.CreateMapPoint(3.0, 1.0, 3.0));

          MapPoint midPt = MapPointBuilder.CreateMapPoint(1.5, 1.5);

          // polyline
          Polyline polyline = PolylineBuilder.CreatePolyline(pts);
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
          MapPoint midPtZ = MapPointBuilder.CreateMapPoint(1.5, 1.5, 1);
          g = GeometryEngine.Instance.Scale(polyline, midPtZ, 0.5, 0.5, 0.25);
          resultPolyline = g as Polyline;
          // resultPolyline.Points[0] = 1.25, 1.25, 1.5
          // resultPolyline.Points[1] = 1.25, 2.25, 1.5
          // resultPolyline.Points[2] = 2.25, 2.25, 1.5
          // resultPolyline.Points[3] = 2.25, 1.25, 1.5

        });
        #endregion
      }

     public void SetConstantZ()
      {
        #region Set all Zs in a polyline

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          List<Coordinate3D> coordsZ = new List<Coordinate3D>() 
          { 
            new Coordinate3D(1, 2, 3), 
            new Coordinate3D(4, 5, 6), 
            new Coordinate3D(7, 8, double.NaN) 
          };

          Polyline polyline = PolylineBuilder.CreatePolyline(coordsZ);
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
        });

        #endregion
      }

      public void ShapePreservingArea()
      {
        #region Calculate area of geometry on surface of Earth's ellipsoid - ShapePreservingArea

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // pt
          MapPoint pt = MapPointBuilder.CreateMapPoint(1.0, 3.0, SpatialReferences.WebMercator);
          double area = GeometryEngine.Instance.ShapePreservingArea(pt);         // area = 0


          List<MapPoint> pts = new List<MapPoint>();
          pts.Add(MapPointBuilder.CreateMapPoint(1.0, 1.0, 3.0));
          pts.Add(MapPointBuilder.CreateMapPoint(1.0, 3.0, 3.0));
          pts.Add(MapPointBuilder.CreateMapPoint(3, 3, 3.0));
          pts.Add(MapPointBuilder.CreateMapPoint(3.0, 1.0, 3.0));

          // multipoint
          Multipoint mPt = MultipointBuilder.CreateMultipoint(pts);
          area = GeometryEngine.Instance.ShapePreservingArea(mPt);               // area = 0

          // polyline
          Polyline polyline = PolylineBuilder.CreatePolyline(pts);
          area = GeometryEngine.Instance.ShapePreservingArea(polyline);          // area = 0

          // polygon
          Polygon polygon = PolygonBuilder.CreatePolygon(pts, SpatialReferences.WGS84);
          area = GeometryEngine.Instance.ShapePreservingArea(polygon);

          polygon = PolygonBuilder.CreatePolygon(pts, SpatialReferences.WebMercator);
          area = GeometryEngine.Instance.ShapePreservingArea(polygon);
        });
        #endregion
      }

      public void ShapePreservingLength()
      {
        #region Calculate length of geometry on surface of Earth's ellipsoid - ShapePreservingLength

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // pt
          MapPoint pt = MapPointBuilder.CreateMapPoint(1.0, 3.0, SpatialReferences.WebMercator);
          double len = GeometryEngine.Instance.ShapePreservingLength(pt);          // len = 0


          List<MapPoint> pts = new List<MapPoint>();
          pts.Add(MapPointBuilder.CreateMapPoint(1.0, 1.0, 3.0));
          pts.Add(MapPointBuilder.CreateMapPoint(1.0, 3.0, 3.0));
          pts.Add(MapPointBuilder.CreateMapPoint(3, 3, 3.0));
          pts.Add(MapPointBuilder.CreateMapPoint(3.0, 1.0, 3.0));

          // multipoint
          Multipoint mPt = MultipointBuilder.CreateMultipoint(pts);
          len = GeometryEngine.Instance.ShapePreservingLength(mPt);                // len = 0

          // polyline
          Polyline polyline = PolylineBuilder.CreatePolyline(pts, SpatialReferences.WGS84);
          len = GeometryEngine.Instance.ShapePreservingLength(polyline);

          // polygon
          Polygon polygon = PolygonBuilder.CreatePolygon(pts, SpatialReferences.WGS84);
          len = GeometryEngine.Instance.ShapePreservingLength(polygon);
        });
        #endregion
      }

      public void SideBuffer()
      {
        #region SideBuffer

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
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

          Polyline polyline = PolylineBuilder.CreatePolyline(coords, sr);

          Polygon output = GeometryEngine.Instance.SideBuffer(polyline, 20, LeftOrRightSide.RightSide, LineCapType.Round) as Polygon;
        });
        #endregion

        #region SideBuffer Many

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          SpatialReference sr = SpatialReferenceBuilder.CreateSpatialReference(102010);
          List<Coordinate2D> coords = new List<Coordinate2D>()
          {
            new Coordinate2D(1200, 5800),
            new Coordinate2D(1400, 5800),
            new Coordinate2D(1400, 6000),
            new Coordinate2D(1300, 6000),
            new Coordinate2D(1300, 5700)
          };

          Polyline polyline1 = PolylineBuilder.CreatePolyline(coords, sr);

          coords.Clear();
          coords.Add(new Coordinate2D(1400, 6050));
          coords.Add(new Coordinate2D(1600, 6150));
          coords.Add(new Coordinate2D(1800, 6050));

          Polyline polyline2 = PolylineBuilder.CreatePolyline(coords, sr);

          List<Polyline> polylines = new List<Polyline>() { polyline1, polyline2};

          IReadOnlyList<Geometry> outGeometries = GeometryEngine.Instance.SideBuffer(polylines, 10, LeftOrRightSide.RightSide, LineCapType.Round);
        });
       #endregion
      }

      public void SimplifyAsFeature()
      {
        #region Simplify a polygon 

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          var g1 = PolygonBuilder.FromJson("{\"rings\": [ [ [0, 0], [10, 0], [10, 10], [0, 10] ] ] }");
          var result = GeometryEngine.Instance.Area(g1);      // result = -100.0   - negative due to wrong ring orientation
          // simplify it
          var result2 = GeometryEngine.Instance.Area(GeometryEngine.Instance.SimplifyAsFeature(g1, true));
          // result2 = 100.0  - positive due to correct ring orientation (clockwise)
        });
        #endregion
      }

      public void SimplifyPolyline()
      {
        #region Simplify a polyline with intersections, overlaps

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
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
          Polyline polyline = PolylineBuilder.CreatePolyline(coords, sr);
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
        });
        #endregion
      }

      public void Touches()
      {
        #region Polygon touches another Polygon

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // two disjoint polygons
          Envelope env = EnvelopeBuilder.CreateEnvelope(MapPointBuilder.CreateMapPoint(4.0, 4.0), MapPointBuilder.CreateMapPoint(8, 8));
          Polygon poly1 = PolygonBuilder.CreatePolygon(env);

          Envelope env2 = EnvelopeBuilder.CreateEnvelope(MapPointBuilder.CreateMapPoint(1.0, 1.0), MapPointBuilder.CreateMapPoint(5, 5));
          Polygon poly2 = PolygonBuilder.CreatePolygon(env2);

          bool touches = GeometryEngine.Instance.Touches(poly1, poly2);    // touches = false

          // another polygon that touches the first
          Envelope env3 = EnvelopeBuilder.CreateEnvelope(MapPointBuilder.CreateMapPoint(1.0, 1.0), MapPointBuilder.CreateMapPoint(4, 4));
          Polygon poly3 = PolygonBuilder.CreatePolygon(env3);

          touches = GeometryEngine.Instance.Touches(poly1, poly3);         // touches = true
        });
        #endregion
      }

      public void Transform2D_Transform3D()
      {
        #region Transform2D

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
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
        });

      #endregion

        #region Transform3D

      // methods need to run on the MCT
      ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
      {
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

      });
      #endregion
      }

      public void Union()
      {
        #region Union two MapPoints - creates a Multipoint

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          MapPoint pt1 = MapPointBuilder.CreateMapPoint(1.0, 1.0);
          MapPoint pt2 = MapPointBuilder.CreateMapPoint(2.0, 2.5);

          Geometry geometry = GeometryEngine.Instance.Union(pt1, pt2);
          Multipoint multipoint = geometry as Multipoint;   // multipoint has point count of 2
        });
        #endregion

        #region Union two Polygons

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // union two polygons

          List<MapPoint> polyPts = new List<MapPoint>();
          polyPts.Add(MapPointBuilder.CreateMapPoint(3.0, 2.0));
          polyPts.Add(MapPointBuilder.CreateMapPoint(3.0, 6.0));
          polyPts.Add(MapPointBuilder.CreateMapPoint(6.0, 6.0));
          polyPts.Add(MapPointBuilder.CreateMapPoint(6.0, 2.0));

          Polygon poly1 = PolygonBuilder.CreatePolygon(polyPts);
          bool isSimple = GeometryEngine.Instance.IsSimpleAsFeature(poly1);

          Envelope env = EnvelopeBuilder.CreateEnvelope(MapPointBuilder.CreateMapPoint(4.0, 4.0), MapPointBuilder.CreateMapPoint(8, 8));
          Polygon poly2 = PolygonBuilder.CreatePolygon(env);
          isSimple = GeometryEngine.Instance.IsSimpleAsFeature(poly2);

          Geometry g = GeometryEngine.Instance.Union(poly1, poly2);
          Polygon polyResult = g as Polygon;
        });
        #endregion

        #region Union many Polylines

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
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
            PolylineBuilder.CreatePolyline(new List<Coordinate2D>(){coords[0], coords[1], coords[2]}, SpatialReferences.WGS84),
            PolylineBuilder.CreatePolyline(new List<Coordinate2D>(){coords[3], coords[4], coords[5]}),
            PolylineBuilder.CreatePolyline(new List<Coordinate2D>(){coords[6], coords[7], coords[8]})
          };

          Polyline polyline = GeometryEngine.Instance.Union(manyLines) as Polyline;
        });

        #endregion

        #region Union many Polygons

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
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
            PolygonBuilder.CreatePolygon(new List<Coordinate3D>(){coordsZ[0], coordsZ[1], coordsZ[2]}, SpatialReferences.WGS84),
            PolygonBuilder.CreatePolygon(new List<Coordinate3D>(){coordsZ[3], coordsZ[4], coordsZ[5]}),
            PolygonBuilder.CreatePolygon(new List<Coordinate3D>(){coordsZ[6], coordsZ[7], coordsZ[8]})
          };

          Polygon polygon = GeometryEngine.Instance.Union(manyPolygonsZ) as Polygon;
        });

        #endregion
      }

      public void Within()
      {
        #region MapPoints, Polylines, Polygons within Polygon

        // methods need to run on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // build a polygon      
          List<MapPoint> pts = new List<MapPoint>();
          pts.Add(MapPointBuilder.CreateMapPoint(1.0, 1.0));
          pts.Add(MapPointBuilder.CreateMapPoint(1.0, 2.0));
          pts.Add(MapPointBuilder.CreateMapPoint(2.0, 2.0));
          pts.Add(MapPointBuilder.CreateMapPoint(2.0, 1.0));

          Polygon poly = PolygonBuilder.CreatePolygon(pts);

          // an inner point
          MapPoint innerPt = MapPointBuilder.CreateMapPoint(1.5, 1.5);
          bool within = GeometryEngine.Instance.Within(innerPt, poly);   // within = true

          // point on a boundary
          within = GeometryEngine.Instance.Within(pts[0], poly);     // within = false

          // an interior line
          MapPoint innerPt2 = MapPointBuilder.CreateMapPoint(1.25, 1.75);
          List<MapPoint> innerLinePts = new List<MapPoint>();
          innerLinePts.Add(innerPt);
          innerLinePts.Add(innerPt2);

          Polyline polyline = PolylineBuilder.CreatePolyline(innerLinePts);
          within = GeometryEngine.Instance.Within(polyline, poly);   // within = true

          // a line that crosses the boundary
          MapPoint outerPt = MapPointBuilder.CreateMapPoint(3, 1.5);
          List<MapPoint> crossingLinePts = new List<MapPoint>();
          crossingLinePts.Add(innerPt);
          crossingLinePts.Add(outerPt);

          polyline = PolylineBuilder.CreatePolyline(crossingLinePts);
          within = GeometryEngine.Instance.Within(polyline, poly);     // within = false


          // polygon in polygon
          Envelope env = EnvelopeBuilder.CreateEnvelope(innerPt, innerPt2);
          within = GeometryEngine.Instance.Within(env, poly);      // within = true
        });
        #endregion
      }

      public void Create_GeographicTransformation()
      {
        #region Create Geographic Transformation

        // create from wkid
        GeographicTransformation gt1478 = ArcGIS.Core.Geometry.GeographicTransformation.Create(1478);
        string name = gt1478.Name;
        string wkt = gt1478.Wkt;

        // create from wkt
        GeographicTransformation another_gt1478 = ArcGIS.Core.Geometry.GeographicTransformation.Create(wkt);

        // inverse
        GeographicTransformation inverse_gt148 = another_gt1478.GetInverse() as GeographicTransformation;
        bool isForward = inverse_gt148.IsForward;

        #endregion
      }

      public void Create_CompositeGeographicTransformation()
      {
        #region Create Composite Geographic Transformation

        // Create singleton from wkid
        CompositeGeographicTransformation cgt = ArcGIS.Core.Geometry.CompositeGeographicTransformation.Create(108272);
        int count = cgt.Count;    // count = 1

        IList<GeographicTransformation> gts = cgt.Transformations as IList<GeographicTransformation>;
        gts.Add(ArcGIS.Core.Geometry.GeographicTransformation.Create(1437, false));
        count = cgt.Count;        // count = 2

        // create from an enumeration
        CompositeGeographicTransformation another_cgt = ArcGIS.Core.Geometry.CompositeGeographicTransformation.Create(gts);
        GeographicTransformation gt0 = another_cgt[0];
        GeographicTransformation gt1 = another_cgt[1];

        // get the inverse
        CompositeGeographicTransformation inversed_cgt = another_cgt.GetInverse() as CompositeGeographicTransformation;
        // inversed_cgt[0] is same as gt1
        // inversed_cgt[1] is same as gt0

        #endregion
      }

      public void Create_ProjectionTransformation()
      {
        #region Create Projection Transformation

        // methods need to be on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          SpatialReference sr4267 = SpatialReferenceBuilder.CreateSpatialReference(4267);
          SpatialReference sr4326 = SpatialReferences.WGS84;
          SpatialReference sr3857 = SpatialReferences.WebMercator;

          // Create transformation from  4267 -> 3857
          ProjectionTransformation projTransFromSRs = ArcGIS.Core.Geometry.ProjectionTransformation.Create(sr4267, sr3857);

          // create an envelope
          Envelope env = EnvelopeBuilder.CreateEnvelope(new Coordinate2D(2, 2), new Coordinate2D(3, 3), sr4267);

          // Project with one geo transform 4267 -> 3857
          Envelope projectedEnvEx = GeometryEngine.Instance.ProjectEx(env, projTransFromSRs) as Envelope;

          // Create inverse transformation, 3857 -> 4267
          ProjectionTransformation projTransFromSRsInverse = ArcGIS.Core.Geometry.ProjectionTransformation.Create(sr3857, sr4267);
          // Project the projected envelope back using the inverse transformation
          Envelope projectedEnvBack = GeometryEngine.Instance.ProjectEx(projectedEnvEx, projTransFromSRsInverse) as Envelope;

          bool isEqual = env.IsEqual(projectedEnvBack);
        });

        #endregion
      }

      public void Create_HVDatumTransformation()
      {
        #region Create HV Datum Transformation

        // Create from wkid
        HVDatumTransformation hv110018 = HVDatumTransformation.Create(110018);
        int wkid = hv110018.Wkid;
        bool isForward = hv110018.IsForward;    // isForward = true
        string name = hv110018.Name;            // Name = WGS_1984_To_WGS_1984_EGM2008_1x1_Height

        // Create from wkt
        string wkt = hv110018.Wkt;
        HVDatumTransformation hv110018FromWkt = HVDatumTransformation.Create(wkt);

        // Get the inverse
        HVDatumTransformation hv110018Inverse = hv110018.GetInverse() as HVDatumTransformation;
        // hv110018Inverse.IsForward = false

        #endregion
      }

      public void Create_CompositeHVDatumTransformation()
      {
        #region Create Composite HV Datum Transformation

        HVDatumTransformation hv1 = HVDatumTransformation.Create(108034);
        HVDatumTransformation hv2 = HVDatumTransformation.Create(108033, false);
        List<HVDatumTransformation> hvs = new List<HVDatumTransformation>() { hv1, hv2 };

        // create from enumeration
        CompositeHVDatumTransformation compositehv = CompositeHVDatumTransformation.Create(hvs);
        int count = compositehv.Count;      // count = 2

        List<HVDatumTransformation> transforms = compositehv.Transformations as List<HVDatumTransformation>;
        HVDatumTransformation tranform = transforms[0];
        // transform.Wkid = 108034

        string xml = compositehv.ToXML();

        // create from xml
        CompositeHVDatumTransformation another_compositehv = CompositeHVDatumTransformation.CreateFromXML(xml);

        // get inverse
        CompositeHVDatumTransformation inverse_compositehv = another_compositehv.GetInverse() as CompositeHVDatumTransformation;

        #endregion
      }

      public void Determine_Transformations()
      {
      #region Determine Transformations

      // methods need to run on the MCT
      ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          //
          // find the first transformation used between spatial references 4267 and 4326
          //
          SpatialReference sr4267 = SpatialReferenceBuilder.CreateSpatialReference(4267);
          SpatialReference sr4326 = SpatialReferences.WGS84;

          List<ProjectionTransformation> transformations = ProjectionTransformation.FindTransformations(sr4267, sr4326);
          // transformations.Count = 1
          ProjectionTransformation projTrans = transformations[0];
          CompositeGeographicTransformation compositeGT = projTrans.Transformation as CompositeGeographicTransformation;
          GeographicTransformation gt = compositeGT[0];
          // gt.Wkid = 15851
          // gt.Name = "NAD_1927_To_WGS_1984_79_CONUS"
          // gt.IsForward = true


          //
          // find the first five transformation used between spatial references 4267 and 4326
          //
          transformations = ProjectionTransformation.FindTransformations(sr4267, sr4326, numResults: 5);
          // transformations.Count = 5
          projTrans = transformations[0];
          compositeGT = projTrans.Transformation as CompositeGeographicTransformation;
          // compositeGT.Count = 1
          // compositeGT[0].Wkid = 15851
          // compositeGT[0].Name = "NAD_1927_To_WGS_1984_79_CONUS"
          // compositeGT[0].IsForward = true

          projTrans = transformations[1];
          compositeGT = projTrans.Transformation as CompositeGeographicTransformation;
          // compositeGT.Count = 1
          // compositeGT[0].Wkid = 1173
          // compositeGT[0].Name = "NAD_1927_To_WGS_1984_4"
          // compositeGT[0].IsForward = true

          projTrans = transformations[2];
          compositeGT = projTrans.Transformation as CompositeGeographicTransformation;
          // compositeGT.Count = 1
          // compositeGT[0].Wkid = 1172
          // compositeGT[0].Name = "NAD_1927_To_WGS_1984_3"
          // compositeGT[0].IsForward = true

          projTrans = transformations[3];
          compositeGT = projTrans.Transformation as CompositeGeographicTransformation;
          // compositeGT.Count = 2
          // compositeGT[0].Wkid = 1241
          // compositeGT[0].Name = "NAD_1927_To_NAD_1983_NADCON"
          // compositeGT[0].IsForward = true

          // compositeGT[1].Wkid = 108190
          // compositeGT[1].Name = "WGS_1984_(ITRF00)_To_NAD_1983"
          // compositeGT[1].IsForward = false

          projTrans = transformations[4];
          compositeGT = projTrans.Transformation as CompositeGeographicTransformation;
          // compositeGT.Count = 2
          // compositeGT[0].Wkid = 1241
          // compositeGT[0].Name = "NAD_1927_To_NAD_1983_NADCON"
          // compositeGT[0].IsForward = true

          // compositeGT[1].Wkid = 1515
          // compositeGT[1].Name = "NAD_1983_To_WGS_1984_5"
          // compositeGT[1].IsForward = true


          //
          // find the first transformation used between spatial references 4267 and 4326 within Alaska
          //

          // Alaska
          Envelope envelope = EnvelopeBuilder.CreateEnvelope(-161, 61, -145, 69);
          transformations = ProjectionTransformation.FindTransformations(sr4267, sr4326, envelope);
          // transformations.Count = 1
          projTrans = transformations[0];
          compositeGT = projTrans.Transformation as CompositeGeographicTransformation;
          // compositeGT.Count = 2
          // compositeGT[0].Wkid = 1243
          // compositeGT[0].Name = "NAD_1927_To_NAD_1983_Alaska"
          // compositeGT[0].IsForward = true

          // compositeGT[1].Wkid = 108190
          // compositeGT[1].Name = "WGS_1984_(ITRF00)_To_NAD_1983"
          // compositeGT[1].IsForward = false


          //
          // find the first geographic transformation used between two spatial references with VCS  (use vertical = false)
          //

          SpatialReference inSR = SpatialReferenceBuilder.CreateSpatialReference(4269, 115702);
          SpatialReference outSR = SpatialReferenceBuilder.CreateSpatialReference(4326, 3855);

          // Even though each spatial reference has a VCS, vertical = false should return geographic transformations.
          transformations = ProjectionTransformation.FindTransformations(inSR, outSR);
          // transformations.Count = 1
          projTrans = transformations[0];
          compositeGT = projTrans.Transformation as CompositeGeographicTransformation;
          // compositeGT.Count = 1
          // compositeGT[0].Wkid = 108190
          // compositeGT[0].Name = ""WGS_1984_(ITRF00)_To_NAD_1983"
          // compositeGT[0].IsForward = false


          //
          // find the first vertical transformation used between two spatial references with VCS  (use vertical = true)
          //

          transformations = ProjectionTransformation.FindTransformations(inSR, outSR, vertical: true);
          // transformations.Count = 1
          projTrans = transformations[0];

          CompositeHVDatumTransformation compositeHV = projTrans.Transformation as CompositeHVDatumTransformation;
          // compositeHV.Count = 2
          // compositeHV[0].Wkid = 1188
          // compositeHV[0].Name = "NAD_1983_To_WGS_1984_1"
          // compositeHV[0].IsForward = true

          // compositeHV[1].Wkid = 110019
          // compositeHV[1].Name = "WGS_1984_To_WGS_1984_EGM2008_2.5x2.5_Height"
          // compositeHV[1].IsForward = true
        });
      #endregion
      }

      public void GeoCoordinateStringConversion()
      {
        #region MapPoint - GeoCoordinateString Conversion

        // methods need to be on the MCT
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          SpatialReference sr = SpatialReferences.WGS84;
          SpatialReference sr2 = SpatialReferences.WebMercator;

          // create some points
          MapPoint point0 = MapPointBuilder.CreateMapPoint(0, 0, sr);
          MapPoint point1 = MapPointBuilder.CreateMapPoint(10, 20, sr);
          MapPoint point2 = GeometryEngine.Instance.Project(point1, sr2) as MapPoint;
          MapPoint pointEmpty = MapPointBuilder.CreateMapPoint(sr);
          MapPoint pointwithNoSR = MapPointBuilder.CreateMapPoint(1, 1);
          MapPoint pointZM = MapPointBuilder.CreateMapPoint(1, 2, 3, 4, sr);

          // convert to MGRS
          ToGeoCoordinateParameter mgrsParam = new ToGeoCoordinateParameter(GeoCoordinateType.MGRS);
          string geoCoordString = point0.ToGeoCoordinateString(mgrsParam);        // 31NAA6602100000

          // use the builder to create a new point from the string.  Coordinates are the same 
          MapPoint outPoint = MapPointBuilder.FromGeoCoordinateString(geoCoordString, sr, GeoCoordinateType.MGRS);    // outPoint.x = 0; outPoint.Y = 0

          geoCoordString = point1.ToGeoCoordinateString(mgrsParam);             // 32QPH0460911794
          outPoint = MapPointBuilder.FromGeoCoordinateString(geoCoordString, sr, GeoCoordinateType.MGRS);       // outPoint.X = 10; outPoint.Y = 20

          // z, m are not transformed
          geoCoordString = pointZM.ToGeoCoordinateString(mgrsParam);
          outPoint = MapPointBuilder.FromGeoCoordinateString(geoCoordString, sr, GeoCoordinateType.MGRS);     // outPoint.X = 1; outPoint.Y = 2; outPoint.Z = Nan; outPoint.M = Nan;

          // set the number of digits to 2 and convert
          mgrsParam.NumDigits = 2;
          geoCoordString = point1.ToGeoCoordinateString(mgrsParam);             // 32QPH0512
          outPoint = MapPointBuilder.FromGeoCoordinateString(geoCoordString, sr, GeoCoordinateType.MGRS);     // outPoint.X = 10; outPoint.Y = 20


          // convert to UTM
          ToGeoCoordinateParameter utmParam = new ToGeoCoordinateParameter(GeoCoordinateType.UTM);
          geoCoordString = point0.ToGeoCoordinateString(utmParam);        // 31N 166021 0000000
          geoCoordString = point1.ToGeoCoordinateString(utmParam);        // 32Q 604609 2211793

          // convert to DMS
          ToGeoCoordinateParameter dmsParam = new ToGeoCoordinateParameter(GeoCoordinateType.DMS);
          geoCoordString = point0.ToGeoCoordinateString(dmsParam);        // 00 00 00.00N 000 00 00.00E
          geoCoordString = point1.ToGeoCoordinateString(dmsParam);        // 20 00 00.00N 010 00 00.00E

          // convert to DDM
          ToGeoCoordinateParameter ddmParam = new ToGeoCoordinateParameter(GeoCoordinateType.DDM);
          geoCoordString = point0.ToGeoCoordinateString(ddmParam);        // 00 00.0000N 000 00.0000E
          geoCoordString = point1.ToGeoCoordinateString(ddmParam);        // 20 00.0000N 010 00.0000E

          // convert to DD
          ToGeoCoordinateParameter ddParam = new ToGeoCoordinateParameter(GeoCoordinateType.DD);
          geoCoordString = point0.ToGeoCoordinateString(ddParam);       // 00.000000N 000.000000E
          geoCoordString = point1.ToGeoCoordinateString(ddParam);       // 20.000000N 010.000000E
        });

        #endregion
      }

      public void OtherUtilities()
      {
        #region AngularUnit - Convert between degrees and radians

        // convert 45 degrees to radians
        double radians = AngularUnit.Degrees.ConvertToRadians(45);

        // convert PI to degrees
        double degrees = AngularUnit.Degrees.ConvertFromRadians(Math.PI);

        #endregion

        #region AngularUnit - Create an AngularUnit with a factory code

        try
        {
          // create a Grad unit
          var grad = AngularUnit.CreateAngularUnit(9105);
          string unitName = grad.Name;                        // Grad
          double conversionFactor = grad.ConversionFactor;    // 0.015708
          double radiansPerUnit = grad.RadiansPerUnit;
          int factoryCode = grad.FactoryCode;                 // 9105

          // convert 10 grads to degrees
          double val = grad.ConvertTo(10, AngularUnit.Degrees);

          // convert 10 radians to grads
          val = grad.ConvertFromRadians(10);
        }
        catch (ArgumentException)
        {
          // ArgumentException will be thrown by CreateAngularUnit in the following scenarios
          // - if the factory code used is a non-angular factory code  (i.e. it corresponds to square meters which is an area unit code)
          // - if the factory code used is invalid (i.e. it is negative or doesn't correspond to any factory code)
        }

        #endregion

        #region AngularUnit - Create a Custom AngularUnit

        // custom unit - 3 radians per unit
        var myAngularUnit = AngularUnit.CreateAngularUnit("myCustomAngularUnit", 3);
        string Name = myAngularUnit.Name;               // myCustomAngularUnit
        double Factor = myAngularUnit.ConversionFactor;   // 3
        int Code = myAngularUnit.FactoryCode;             // 0 because it is a custom angular unit
        double radiansUnit = myAngularUnit.RadiansPerUnit; // 3

        // convert 10 degrees to my unit
        double converted = AngularUnit.Degrees.ConvertTo(10, myAngularUnit);
        // convert it back to degrees
        converted = myAngularUnit.ConvertTo(converted, AngularUnit.Degrees);

        // convert 1 radian into my angular units
        converted = myAngularUnit.ConvertFromRadians(1);

        #endregion

        #region LinearUnit - Convert between feet and meters
        // convert 10 feet to meters
        double metres = LinearUnit.Feet.ConvertToMeters(10);

        // convert 20 meters to feet
        double feet = LinearUnit.Feet.ConvertFromMeters(20.0);
        #endregion

        #region LinearUnit - Convert between centimeters and millimeters

        // convert 11 centimeters to millimeters
        double mm = LinearUnit.Centimeters.ConvertTo(11, LinearUnit.Millimeters);

        // convert the result back to centimeters
        double cm = LinearUnit.Millimeters.ConvertTo(mm, LinearUnit.Centimeters);

        // convert the millimeter result back to meters
        double meters = LinearUnit.Millimeters.ConvertToMeters(mm);

        #endregion

        #region LinearUnit - Create a LinearUnit with a factory code

        try
        {
          // create a british 1936 foot
          var britFoot = LinearUnit.CreateLinearUnit(9095);
          string unitName = britFoot.Name;                        //  "Foot_British_1936"
          double conversionFactor = britFoot.ConversionFactor;    // 0.3048007491
          double metersPerUnit = britFoot.MetersPerUnit;
          int factoryCode = britFoot.FactoryCode;                 // 9095

          // convert 10 british 1936 feet to centimeters
          double val = britFoot.ConvertTo(10, LinearUnit.Centimeters);

          // convert 10 m to british 1936 feet
          val = britFoot.ConvertFromMeters(10);
        }
        catch (ArgumentException)
        {
          // ArgumentException will be thrown by CreateLinearUnit in the following scenarios
          // - if the factory code used is a non-linear factory code  (i.e. it corresponds to square meters which is an area unit code)
          // - if the factory code used is invalid (i.e. it is negative or doesn't correspond to any factory code)
        }

        #endregion

        #region LinearUnit - Create a Custom LinearUnit

        // create a custom linear unit - there are 0.33 meters per myLinearUnit
        var myLinearUnit = LinearUnit.CreateLinearUnit("myCustomLinearUnit", 0.33);
        string name = myLinearUnit.Name;                          // myCustomLinearUnit
        double convFactor = myLinearUnit.ConversionFactor;        // 0.33
        int code = myLinearUnit.FactoryCode;                      // 0 for custom units
        double metersUnit = myLinearUnit.MetersPerUnit;           // 0.33
        string toString = myLinearUnit.ToString();                // same as Name - myCustomLinearUnit

        // convert 10 centimeters to myLinearUnit 
        double convertedVal = LinearUnit.Centimeters.ConvertTo(10, myLinearUnit);

        #endregion

        #region AreaUnit - Convert between square feet and square meters

        // convert 700 square meters to square feet
        double sqFeet = AreaUnit.SquareFeet.ConvertFromSquareMeters(700);

        // convert 1100 square feet to square meters
        double sqMeters = AreaUnit.SquareFeet.ConvertToSquareMeters(1000);
        #endregion

        #region AreaUnit - Convert between hectares and acres

        // convert 2 hectares to acres
        double acres = AreaUnit.Hectares.ConvertTo(2, AreaUnit.Acres);

        #endregion

        #region AreaUnit - Convert between hectares and square miles
        // convert 300 hectares to square miles
        double sqMiles = AreaUnit.Hectares.ConvertTo(300, AreaUnit.SquareMiles);
        #endregion

        #region AreaUnit - How many Square meters in various units

        double sqMetersPerUnit = AreaUnit.Acres.SquareMetersPerUnit;
        sqMetersPerUnit = AreaUnit.Ares.SquareMetersPerUnit;
        sqMetersPerUnit = AreaUnit.Hectares.SquareMetersPerUnit;
        sqMetersPerUnit = AreaUnit.SquareKilometers.SquareMetersPerUnit;
        sqMetersPerUnit = AreaUnit.SquareMiles.SquareMetersPerUnit;
        sqMetersPerUnit = AreaUnit.SquareYards.SquareMetersPerUnit;

      #endregion

    }
  }
}
