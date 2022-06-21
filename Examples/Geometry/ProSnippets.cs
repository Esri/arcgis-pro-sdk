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

  class GeometryCodeExamples
  {
    #region ProSnippet Group: SpatialReference
    #endregion

    public void SpatialReference()
    {
      {
        // cref: ArcGIS.Core.Geometry.SpatialReferenceBuilder.#ctor(System.Int32)
        // cref: ArcGIS.Core.Geometry.SpatialReferenceBuilder.CreateSpatialReference(System.Int32)
        // cref: ArcGIS.Core.Geometry.SpatialReferenceBuilder.ToSpatialReference
        #region Construct a SpatialReference - from a well-known ID

        // Use a builder convenience method or use a builder constructor.

        // Builder convenience methods don't need to run on the MCT.
        SpatialReference sr3857 = SpatialReferenceBuilder.CreateSpatialReference(3857);

        // Builder constructors need to run on the MCT.
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          using (SpatialReferenceBuilder srBuilder = new SpatialReferenceBuilder(3857))
          {
            // do something with the builder

            sr3857 = srBuilder.ToSpatialReference();
          }
        });

        #endregion
      }

      {
        // cref: ArcGIS.Core.Geometry.SpatialReferenceBuilder.#ctor(System.String)
        // cref: ArcGIS.Core.Geometry.SpatialReferenceBuilder.CreateSpatialReference(System.String)
        // cref: ArcGIS.Core.Geometry.SpatialReferenceBuilder.ToSpatialReference
        #region Construct a SpatialReference - from a string

        // Use a builder convenience method or use a builder constructor.

        string wkt = "GEOGCS[\"MyGCS84\",DATUM[\"D_WGS_1984\",SPHEROID[\"WGS_1984\",6378137.0,298.257223563]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Radian\",1.0]]";

        // Builder convenience methods don't need to run on the MCT.
        SpatialReference sr = SpatialReferenceBuilder.CreateSpatialReference(wkt);

        // Builder constructors need to run on the MCT.
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          using (SpatialReferenceBuilder builder = new SpatialReferenceBuilder(wkt))
          {
            // do something with the builder

            SpatialReference anotherSR = builder.ToSpatialReference();
          }
        });

        #endregion
      }

      {
        // cref: ArcGIS.Core.Geometry.SpatialReference.IsGeographic
        // cref: ArcGIS.Core.Geometry.SpatialReference.IsProjected
        // cref: ArcGIS.Core.Geometry.SpatialReferences.WGS84
        #region Use WGS84 SpatialReference

        SpatialReference wgs84 = SpatialReferences.WGS84;
        bool isProjected = wgs84.IsProjected;     // false
        bool isGeographic = wgs84.IsGeographic;   // true

        #endregion
      }

      {
        // cref: ArcGIS.Core.Geometry.SpatialReferenceBuilder.#ctor(System.Int32,System.Int32)
        // cref: ArcGIS.Core.Geometry.SpatialReferenceBuilder.CreateSpatialReference(System.Int32,System.Int32)
        // cref: ArcGIS.Core.Geometry.SpatialReferenceBuilder.VcsWkid
        // cref: ArcGIS.Core.Geometry.SpatialReferenceBuilder.VcsWkt
        #region Construct a SpatialReference with a vertical coordinate system - from well-known IDs

        // Use a builder convenience method or use a builder constructor.

        // see a list of vertical coordinate systems at http://resources.arcgis.com/en/help/arcgis-rest-api/index.html#/Vertical_coordinate_systems/02r3000000rn000000/

        // Builder convenience methods don't need to run on the MCT.
        // 4326 = GCS_WGS_1984
        // 115700 = vertical WGS_1984
        SpatialReference sr4326_115700 = SpatialReferenceBuilder.CreateSpatialReference(4326, 115700);

        // Builder constructors need to run on the MCT.
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          using (SpatialReferenceBuilder sb = new SpatialReferenceBuilder(4326, 115700))
          {
            // SpatialReferenceBuilder properties
            //   sb.wkid == 4326
            //   sb.Wkt == "GEOGCS["MyGCS84",DATUM["D_WGS_1984",SPHEROID["WGS_1984",6378137.0,298.257223563]],PRIMEM["Greenwich",0.0],UNIT[\"Radian\",1.0]]"
            //   sb.name == GCS_WGS_1984
            //   sb.vcsWkid == 115700
            //   sb.VcsWkt == "VERTCS["WGS_1984",DATUM["D_WGS_1984",SPHEROID["WGS_1984",6378137.0,298.257223563]],PARAMETER["Vertical_Shift",0.0],PARAMETER["Direction",1.0],UNIT["Meter",1.0]]

            // do something with the builder

            sr4326_115700 = sb.ToSpatialReference();
          }
        });

        #endregion
      }

      {
        // cref: ArcGIS.Core.Geometry.SpatialReferenceBuilder.#ctor(System.Int32,System.String)
        // cref: ArcGIS.Core.Geometry.SpatialReferenceBuilder.CreateSpatialReference(System.Int32,System.String)
        // cref: ArcGIS.Core.Geometry.SpatialReference.VcsWkid
        // cref: ArcGIS.Core.Geometry.SpatialReference.VcsWkt
        #region Construct a SpatialReference with a vertical coordinate system - from a string 

        // Use a builder convenience method or use a builder constructor.

        // custom VCS - use vertical shift of -1.23 instead of 0
        string custom_vWkt = @"VERTCS[""SHD_height"",VDATUM[""Singapore_Height_Datum""],PARAMETER[""Vertical_Shift"",-1.23],PARAMETER[""Direction"",-1.0],UNIT[""Meter"",1.0]]";

        // Builder convenience methods don't need to run on the MCT.
        SpatialReference sr4326_customVertical = SpatialReferenceBuilder.CreateSpatialReference(4326, custom_vWkt);
        // SpatialReferenceBuilder properties
        //   sr4326_customVertical.wkid == 4326
        //   sr4326_customVertical.vert_wkid == 0
        //   sr4326_customVertical.vert_wkt == custom_vWkt
        //   sr4326_customVertical.hasVcs == true

        // Builder constructors need to run on the MCT.
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          using (SpatialReferenceBuilder sb = new SpatialReferenceBuilder(4326, custom_vWkt))
          {
            // do something with the builder

            sr4326_customVertical = sb.ToSpatialReference();
          }
        });

        #endregion
      }

      {
        // cref: ArcGIS.Core.Geometry.SpatialReferenceBuilder.#ctor(System.String)
        // cref: ArcGIS.Core.Geometry.SpatialReferenceBuilder.CreateSpatialReference(System.String)
        // cref: ArcGIS.Core.Geometry.SpatialReference.Gcs
        // cref: ArcGIS.Core.Geometry.SpatialReference.GcsWkid
        // cref: ArcGIS.Core.Geometry.SpatialReference.GcsWkt
        #region Construct a SpatialReference with a custom PCS - from a string

        // Use a builder convenience method or use a builder constructor.

        // Custom PCS, Predefined GCS
        string customWkt = "PROJCS[\"WebMercatorMile\",GEOGCS[\"GCS_WGS_1984\",DATUM[\"D_WGS_1984\",SPHEROID[\"WGS_1984\",6378137.0,298.257223563]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]],PROJECTION[\"Mercator_Auxiliary_Sphere\"],PARAMETER[\"False_Easting\",0.0],PARAMETER[\"False_Northing\",0.0],PARAMETER[\"Central_Meridian\",0.0],PARAMETER[\"Standard_Parallel_1\",0.0],PARAMETER[\"Auxiliary_Sphere_Type\",0.0],UNIT[\"Mile\",1609.344000614692]]";

        // Builder convenience methods don't need to run on the MCT.
        SpatialReference spatialReference = SpatialReferenceBuilder.CreateSpatialReference(customWkt);
        // SpatialReferenceBuilder properties
        //   spatialReference.Wkt == customWkt
        //   spatialReference.Wkid == 0
        //   spatialReference.VcsWkid == 0
        //   spatialReference.GcsWkid == 4326

        SpatialReference gcs = spatialReference.Gcs;
        // gcs.Wkid == 4326
        // gcs.IsGeographic == true
        // spatialReference.GcsWkt == gcs.Wkt

        // Builder constructors need to run on the MCT.
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          using (SpatialReferenceBuilder sb = new SpatialReferenceBuilder(customWkt))
          {
            // do something with the builder

            spatialReference = sb.ToSpatialReference();
          }
        });

        #endregion
      }

      {
        // cref: ArcGIS.Core.Geometry.SpatialReference.Wkid
        // cref: ArcGIS.Core.Geometry.SpatialReference.Wkt
        // cref: ArcGIS.Core.Geometry.SpatialReference.Name
        // cref: ArcGIS.Core.Geometry.SpatialReference.XYScale
        // cref: ArcGIS.Core.Geometry.SpatialReference.XYTolerance
        // cref: ArcGIS.Core.Geometry.SpatialReference.XYResolution
        // cref: ArcGIS.Core.Geometry.SpatialReference.Unit
        // cref: ArcGIS.Core.Geometry.SpatialReference.ZScale
        // cref: ArcGIS.Core.Geometry.SpatialReference.ZTolerance
        // cref: ArcGIS.Core.Geometry.SpatialReference.ZUnit
        // cref: ArcGIS.Core.Geometry.SpatialReference.MScale
        // cref: ArcGIS.Core.Geometry.SpatialReference.MTolerance
        // cref: ArcGIS.Core.Geometry.SpatialReference.FalseX
        // cref: ArcGIS.Core.Geometry.SpatialReference.FalseY
        // cref: ArcGIS.Core.Geometry.SpatialReference.FalseZ
        // cref: ArcGIS.Core.Geometry.SpatialReference.FalseM
        // cref: ArcGIS.Core.Geometry.SpatialReference.HasVcs
        // cref: ArcGIS.Core.Geometry.SpatialReferenceBuilder.Wkid
        // cref: ArcGIS.Core.Geometry.SpatialReferenceBuilder.Wkt
        // cref: ArcGIS.Core.Geometry.SpatialReferenceBuilder.Name
        // cref: ArcGIS.Core.Geometry.SpatialReferenceBuilder.XYScale
        // cref: ArcGIS.Core.Geometry.SpatialReferenceBuilder.XYTolerance
        // cref: ArcGIS.Core.Geometry.SpatialReferenceBuilder.XYResolution
        // cref: ArcGIS.Core.Geometry.SpatialReferenceBuilder.Unit
        // cref: ArcGIS.Core.Geometry.SpatialReferenceBuilder.ZScale
        // cref: ArcGIS.Core.Geometry.SpatialReferenceBuilder.ZTolerance
        // cref: ArcGIS.Core.Geometry.SpatialReferenceBuilder.ZUnit
        // cref: ArcGIS.Core.Geometry.SpatialReferenceBuilder.MScale
        // cref: ArcGIS.Core.Geometry.SpatialReferenceBuilder.MTolerance
        // cref: ArcGIS.Core.Geometry.SpatialReferenceBuilder.FalseX
        // cref: ArcGIS.Core.Geometry.SpatialReferenceBuilder.FalseY
        // cref: ArcGIS.Core.Geometry.SpatialReferenceBuilder.FalseZ
        // cref: ArcGIS.Core.Geometry.SpatialReferenceBuilder.FalseM
        #region SpatialReference Properties

        // Builder constructors need to run on the MCT.
        ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          // use the builder constructor
          using (SpatialReferenceBuilder srBuilder = new SpatialReferenceBuilder(3857))
          {
            // spatial reference builder properties
            int builderWkid = srBuilder.Wkid;
            string builderWkt = srBuilder.Wkt;
            string builderName = srBuilder.Name;

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
            int srWkid = sr3857.Wkid;
            string srWkt = sr3857.Wkt;
            string srName = sr3857.Name;

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
      }

      {
        // cref: ArcGIS.Core.Geometry.SpatialReference.ToJson
        // cref: ArcGIS.Core.Geometry.SpatialReference.ToXml
        // cref: ArcGIS.Core.Geometry.SpatialReferenceBuilder.FromJson(System.String)
        // cref: ArcGIS.Core.Geometry.SpatialReferenceBuilder.FromXml(System.String)
        #region Import and Export Spatial Reference

        SpatialReference srWithVertical = SpatialReferenceBuilder.CreateSpatialReference(4326, 6916);

        string xml = srWithVertical.ToXml();
        SpatialReference importedSR = SpatialReferenceBuilder.FromXml(xml);
        // importedSR.Wkid = 4326
        // importedSR.VcsWkid = 6916

        string json = srWithVertical.ToJson();
        importedSR = SpatialReferenceBuilder.FromJson(json);
        // importedSR.Wkid = 4326
        // importedSR.VcsWkid = 6916

        #endregion
      }

      {
        // cref: ArcGIS.Core.Geometry.GeometryEngine.Project
        // cref: ArcGIS.Core.Geometry.SpatialReference.GetConvergenceAngle(ArcGIS.Core.Geometry.Coordinate2D)
        #region Determine grid convergence for a SpatialReference at a given point

        Coordinate2D coordinate = new Coordinate2D(10, 30);
        double angle = SpatialReferences.WGS84.GetConvergenceAngle(coordinate);
        // angle = 0

        SpatialReference srUTM30N = SpatialReferenceBuilder.CreateSpatialReference(32630);
        coordinate.X = 500000;
        coordinate.Y = 550000;
        angle = srUTM30N.GetConvergenceAngle(coordinate);
        // angle = 0

        MapPoint pointWGS84 = MapPointBuilderEx.CreateMapPoint(10, 50, SpatialReferences.WGS84);
        MapPoint pointUTM30N = GeometryEngine.Instance.Project(
          pointWGS84, srUTM30N) as MapPoint;

        coordinate = (Coordinate2D)pointUTM30N;
        // get convergence angle and convert to degrees
        angle = srUTM30N.GetConvergenceAngle(coordinate) * 180 / Math.PI;
        // angle = 10.03

        #endregion
      }

      {
        // cref: ArcGIS.Core.CIM.CIMMap.DatumTransforms
        // cref: ArcGIS.Core.CIM.CIMDatumTransform
        // cref: ArcGIS.Core.CIM.CIMMap.HVDatumTransforms
        // cref: ArcGIS.Core.Geometry.CompositeHVDatumTransformation
        #region Datum

        var cimMapDefinition = MapView.Active.Map.GetDefinition();
        // use if map's sr does not have a vertical coordinate system
        var datumTransformations = cimMapDefinition.DatumTransforms;
        // use if map's sr has a vertical coordinate system
        var hvDatumTransformations = cimMapDefinition.HVDatumTransforms;

        #endregion
      }

      {
        // cref: ArcGIS.Core.Geometry.SpatialReference.Datum
        // cref: ArcGIS.Core.Geometry.SpatialReferences.WGS84
        // cref: ArcGIS.Core.Geometry.Datum.Name
        // cref: ArcGIS.Core.Geometry.Datum.Wkid
        // cref: ArcGIS.Core.Geometry.Datum.SpheroidName
        // cref: ArcGIS.Core.Geometry.Datum.SpheroidWkid
        // cref: ArcGIS.Core.Geometry.Datum.SpheroidFlattening
        // cref: ArcGIS.Core.Geometry.Datum.SpheroidSemiMajorAxis
        // cref: ArcGIS.Core.Geometry.Datum.SpheroidSemiMinorAxis
        #region SpatialReference Datum and datum properties

        // Get datum of a spatial reference
        SpatialReference srWgs84 = SpatialReferences.WGS84;
        Datum datum = srWgs84.Datum;
        // datum.Name = "D_WGS_1984"
        // datum.Wkid = 6326
        // datum.SpheroidName = "WGS_1984"
        // datum.SpheroidWkid = 7030
        // datum.SpheroidFlattening = 0.0033528106647474805
        // datum.SpheroidSemiMajorAxis = 6378137.0
        // datum.SpheroidSemiMinorAxis = 6356752.3142451793

        // Custom WKT
        string wyomingWkt = "PROJCS[\"Wyoming_State_Pl_NAD_1927\",GEOGCS[\"GCS_North_American_1927\",DATUM[\"D_North_American_1927_Perry\",SPHEROID[\"Clarke_1866_Chase\",6378210.0,250.0]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]],PROJECTION[\"Transverse_Mercator\"],PARAMETER[\"false_easting\",500000.0],PARAMETER[\"false_northing\",0.0],PARAMETER[\"central_meridian\",-107.3333333],PARAMETER[\"scale_factor\",0.9999412],PARAMETER[\"latitude_of_origin\",40.66666667],UNIT[\"Foot_US\",0.3048006096012192]]";
        SpatialReference srFromWkt = SpatialReferenceBuilder.CreateSpatialReference(wyomingWkt);
        datum = srWgs84.Datum;
        // datum.Name = "D_North_American_1927_Perry"
        // datum.Wkid = 0
        // datum.SpheroidName = "Clarke_1866_Chase"
        // datum.SpheroidWkid = 0
        // datum.SpheroidFlattening = 0.004
        // datum.SpheroidSemiMajorAxis = 6378210.0
        // datum.SpheroidSemiMinorAxis = 6352697.16

        #endregion
      }
    }

    #region ProSnippet Group: Coordinate3D
    #endregion

    public void Coordinate3D()
    {
      // cref: ArcGIS.Core.Geometry.Coordinate3D.QueryPolarComponents
      // cref: ArcGIS.Core.Geometry.Coordinate3D.SetPolarComponents(System.Double,System.Double,System.Double)
      // cref: ArcGIS.Core.Geometry.Coordinate3D.QueryComponents
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

      // cref: ArcGIS.Core.Geometry.Coordinate3D.SetComponents(System.Double,System.Double,System.Double)
      // cref: ArcGIS.Core.Geometry.Coordinate3D.Inclination
      #region Getting vector inclination

      Coordinate3D v = new Coordinate3D(0, 0, 7);
      double inclination = v.Inclination;         // inclination = PI/2

      v.SetComponents(-2, -3, 0);
      inclination = v.Inclination;                // inclination = 0

      v.SetComponents(0, 0, -2);
      inclination = v.Inclination;                // inclination = -PI/2

      #endregion

      // cref: ArcGIS.Core.Geometry.Coordinate3D.Azimuth
      // cref: ArcGIS.Core.Geometry.Coordinate3D.SetComponents
      // cref: ArcGIS.Core.Geometry.AngularUnit.ConvertFromRadians
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
      // cref: ArcGIS.Core.Geometry.Coordinate3D.#ctor(System.Double,System.Double,System.Double)
      // cref: ArcGIS.Core.Geometry.Coordinate3D.DotProduct(ArcGIS.Core.Geometry.Coordinate3D)
      // cref: ArcGIS.Core.Geometry.Coordinate3D.CrossProduct(ArcGIS.Core.Geometry.Coordinate3D)
      // cref: ArcGIS.Core.Geometry.Coordinate3D.AddCoordinate3D(ArcGIS.Core.Geometry.Coordinate3D)
      // cref: ArcGIS.Core.Geometry.Coordinate3D.Rotate(System.Double,ArcGIS.Core.Geometry.Coordinate3D)
      // cref: ArcGIS.Core.Geometry.Coordinate3D.Scale(System.Double)
      // cref: ArcGIS.Core.Geometry.Coordinate3D.SetEmpty
      // cref: ArcGIS.Core.Geometry.Coordinate3D.Move(System.Double,System.Double,System.Double)
      // cref: ArcGIS.Core.Geometry.Coordinate3D.op_Addition(ArcGIS.Core.Geometry.Coordinate3D,ArcGIS.Core.Geometry.Coordinate3D)
      // cref: ArcGIS.Core.Geometry.Coordinate3D.op_Subtraction(ArcGIS.Core.Geometry.Coordinate3D,ArcGIS.Core.Geometry.Coordinate3D)
      // cref: ArcGIS.Core.Geometry.Coordinate3D.op_Equality(ArcGIS.Core.Geometry.Coordinate3D,ArcGIS.Core.Geometry.Coordinate3D)
      // cref: ArcGIS.Core.Geometry.Coordinate3D.op_Inequality(ArcGIS.Core.Geometry.Coordinate3D,ArcGIS.Core.Geometry.Coordinate3D)
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


      Coordinate3D emptyVector = new Coordinate3D();
      // emptyVector = (0, 0, 0)
      emptyVector.SetEmpty();
      // emptyVector = (Nan, Nan, Nan)

      Coordinate3D c1 = new Coordinate3D(2, 3, 4);
      Coordinate3D c2 = new Coordinate3D(9, -1, 3);

      var result_add = c1 + c2;
      // result_add = (11, 2, 7)
      var result_sub = c1 - c2;
      // result_sub = (-7, 4, 1)

      var b = result_sub != result_add;
      // b = true

      result_add = emptyVector + c1;
      // result_add = (Nan, Nan, Nan)

      b = result_add == emptyVector;
      // b = true

      #endregion
    }

    public void Coordinate2D()
    {
      // cref: ArcGIS.Core.Geometry.Coordinate2D.#ctor(System.Double,System.Double)
      // cref: ArcGIS.Core.Geometry.Coordinate2D.Magnitude
      // cref: ArcGIS.Core.Geometry.Coordinate2D.DotProduct(ArcGIS.Core.Geometry.Coordinate2D)
      // cref: ArcGIS.Core.Geometry.Coordinate2D.op_Addition(ArcGIS.Core.Geometry.Coordinate2D,ArcGIS.Core.Geometry.Coordinate2D)
      // cref: ArcGIS.Core.Geometry.Coordinate2D.op_Subtraction(ArcGIS.Core.Geometry.Coordinate2D,ArcGIS.Core.Geometry.Coordinate2D)
      // cref: ArcGIS.Core.Geometry.Coordinate2D.Rotate(System.Double)
      // cref: ArcGIS.Core.Geometry.Coordinate2D.Rotate(System.Double,ArcGIS.Core.Geometry.Coordinate2D)
      // cref: ArcGIS.Core.Geometry.Coordinate2D.op_Equality(ArcGIS.Core.Geometry.Coordinate2D,ArcGIS.Core.Geometry.Coordinate2D)
      // cref: ArcGIS.Core.Geometry.Coordinate2D.op_Inequality(ArcGIS.Core.Geometry.Coordinate2D,ArcGIS.Core.Geometry.Coordinate2D)
      // cref: ArcGIS.Core.Geometry.Coordinate2D.Scale(System.Double)
      // cref: ArcGIS.Core.Geometry.Coordinate2D.Move(System.Double,System.Double)
      // cref: ArcGIS.Core.Geometry.Coordinate2D.QueryComponents
      // cref: ArcGIS.Core.Geometry.Coordinate2D.GetUnitVector
      // cref: ArcGIS.Core.Geometry.Coordinate2D.SetComponents(System.Double,System.Double)
      #region 2D Vector Operations
      Coordinate2D v = new Coordinate2D(0, 1);
      // v.Magnitude = 1

      Coordinate2D other = new Coordinate2D(-1, 0);
      double dotProduct = v.DotProduct(other);
      // dotProduct = 0

      Coordinate2D w = v + other;
      // w = (-1, 1)

      w += other;
      // w = (-2, 1)

      w -= other;
      // w = (-1, 1)

      w = v;
      w.Rotate(Math.PI, other);
      // w = (-2, -1)

      w = other;

      w.Scale(-4);
      // w = (4, 0)
      // w.Magnitude = 4

      w.Move(-1, 4);
      // w = (3, 4)
      // w.Magnitude = 5

      w.Move(-6, -1);
      Tuple<double, double> components = w.QueryComponents();
      // components = (-3, 3)
      // w.Magnitude = 3 * Math.Sqrt(2)

      Coordinate2D unitVector = w.GetUnitVector();
      // w = (-Math.Sqrt(2) / 2, Math.Sqrt(2) / 2)
      // w.Magnitude = 1


      w.SetComponents(3, 4);

      #endregion
    }

    #region ProSnippet Group: Builder Properties
    #endregion

    public void Builders()
    {
      // cref: ArcGIS.Core.Geometry.PolylineBuilderEx.CreatePolyline(IEnumerable<ArcGIS.Core.Geometry.MapPoint> points, ArcGIS.Core.Geometry.SpatialReference spatialReference = null)
      // cref: ArcGIS.Core.Geometry.PolylineBuilderEx.CreatePolyline(IEnumerable<ArcGIS.Core.Geometry.MapPoint> points, AttributeFlags attributes, ArcGIS.Core.Geometry.SpatialReference spatialReference = null)
      // cref: ArcGIS.Core.Geometry.MultipartBuilderEx.HasZ
      // cref: ArcGIS.Core.Geometry.MultipartBuilderEx.HasM
      // cref: ArcGIS.Core.Geometry.MultipartBuilderEx.HasID
      // cref: ArcGIS.Core.Geometry.PolygonBuilderEx.CreatePolygon(IEnumerable<ArcGIS.Core.Geometry.MapPoint> points, ArcGIS.Core.Geometry.SpatialReference spatialReference = null)
      // cref: ArcGIS.Core.Geometry.PolygonBuilderEx.CreatePolygon(IEnumerable<ArcGIS.Core.Geometry.MapPoint> points, AttributeFlags attributes, ArcGIS.Core.Geometry.SpatialReference spatialReference = null)
      // cref: ArcGIS.Core.Geometry.MultipartBuilderEx.HasZ
      // cref: ArcGIS.Core.Geometry.MultipartBuilderEx.HasM
      // cref: ArcGIS.Core.Geometry.MultipartBuilderEx.HasID
      // cref: ArcGIS.Core.Geometry.PolylineBuilderEx.#ctor(IEnumerable<ArcGIS.Core.Geometry.MapPoint> points, AttributeFlags attributes, ArcGIS.Core.Geometry.SpatialReference spatialReference = null)
      // cref: ArcGIS.Core.Geometry.PolylineBuilderEx.#ctor(ArcGIS.Core.Geometry.Polyline polyline)
      // cref: ArcGIS.Core.Geometry.PolygonBuilderEx.#ctor(IEnumerable<ArcGIS.Core.Geometry.MapPoint> points, AttributeFlags attributes, ArcGIS.Core.Geometry.SpatialReference spatialReference = null)
      // cref: ArcGIS.Core.Geometry.PolygonBuilderEx.#ctor(ArcGIS.Core.Geometry.Polygon polygon)
      // cref: ArcGIS.Core.Geometry.AttributeFlags
      #region Builder Properties

      // list of points
      List<MapPoint> points = new List<MapPoint>
      {
        MapPointBuilderEx.CreateMapPoint(0, 0, 2, 3, 1),
        MapPointBuilderEx.CreateMapPoint(1, 1, 5, 6),
        MapPointBuilderEx.CreateMapPoint(2, 1, 6),
        MapPointBuilderEx.CreateMapPoint(0, 0)
      };

      // will have attributes because it is created with convenience method
      Polyline polylineWithAttrs = PolylineBuilderEx.CreatePolyline(points);
      bool hasZ = polylineWithAttrs.HasZ;          // hasZ = true
      bool hasM = polylineWithAttrs.HasM;          // hasM = true
      bool hasID = polylineWithAttrs.HasID;        // hasID = true

      // will not have attributes because it is specified as a parameter
      Polyline polylineWithoutAttrs = 
        PolylineBuilderEx.CreatePolyline(points, AttributeFlags.None);
      hasZ = polylineWithoutAttrs.HasZ;          // hasZ = false
      hasM = polylineWithoutAttrs.HasM;          // hasM = false
      hasID = polylineWithoutAttrs.HasID;        // hasID = false

      // will have attributes because it is created with convenience method
      Polygon polygonWithAttrs = PolygonBuilderEx.CreatePolygon(points);
      hasZ = polygonWithAttrs.HasZ;               // hasZ = true
      hasM = polygonWithAttrs.HasM;               // hasM = true
      hasID = polygonWithAttrs.HasID;             // hasID = true

      // will not have attributes because it is specified as a parameter
      Polygon polygonWithoutAttrs = 
            PolygonBuilderEx.CreatePolygon(points, AttributeFlags.None);
      hasZ = polygonWithoutAttrs.HasZ;               // hasZ = false
      hasM = polygonWithoutAttrs.HasM;               // hasM = false
      hasID = polygonWithoutAttrs.HasID;             // hasID = false

      // will not have attributes because it is specified as a parameter
      PolylineBuilderEx polylineB = 
                 new PolylineBuilderEx(points, AttributeFlags.None);
      hasZ = polylineB.HasZ;                      // hasZ = false
      hasM = polylineB.HasM;                      // hasM = false
      hasID = polylineB.HasID;                    // hasID = false

      // will have attributes because it is passed an attributed polyline
      polylineB = new PolylineBuilderEx(polylineWithAttrs);
      hasZ = polylineB.HasZ;                      // hasZ = true
      hasM = polylineB.HasM;                      // hasM = true
      hasID = polylineB.HasID;                    // hasID = true

      // will not have attributes because it is passed a non-attributed polyline
      polylineB = new PolylineBuilderEx(polylineWithoutAttrs);
      hasZ = polylineB.HasZ;                      // hasZ = false
      hasM = polylineB.HasM;                      // hasM = false
      hasID = polylineB.HasID;                    // hasID = false

      // will not have attributes because it is specified as a parameter
      PolygonBuilderEx polygonB = new PolygonBuilderEx(points, AttributeFlags.None);
      hasZ = polygonB.HasZ;                       // hasZ = false
      hasM = polygonB.HasM;                       // hasM = false
      hasID = polygonB.HasID;                     // hasID = false

      // will have attributes because it is passed an attributed polygon
      polygonB = new PolygonBuilderEx(polygonWithAttrs);
      hasZ = polygonB.HasZ;                       // hasZ = true
      hasM = polygonB.HasM;                       // hasM = true
      hasID = polygonB.HasID;                     // hasID = true

      // will not have attributes because it is passed a non-attributed polygon
      polygonB = new PolygonBuilderEx(polygonWithoutAttrs);
      hasZ = polygonB.HasZ;                       // hasZ = true
      hasM = polygonB.HasM;                       // hasM = true
      hasID = polygonB.HasID;                     // hasID = true

      #endregion
    }

    #region ProSnippet Group: MapPoint
    #endregion

    public void MapPoint()
    {
      {
        // cref: ArcGIS.Core.Geometry.MapPointBuilderEx.CreateMapPoint(System.Double,System.Double,System.Double,System.Double,ArcGIS.Core.Geometry.SpatialReference)
        // cref: ArcGIS.Core.Geometry.MapPointBuilderEx.#ctor(System.Double,System.Double,System.Double,System.Double,ArcGIS.Core.Geometry.SpatialReference)
        // cref: ArcGIS.Core.Geometry.MapPointBuilderEx.ToGeometry
        // cref: ArcGIS.Core.Geometry.Geometry.Clone
        // cref: ArcGIS.Core.Geometry.MapPointBuilderEx.CreateMapPoint(ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.SpatialReference)
        // cref: ArcGIS.Core.Geometry.MapPointBuilderEx.#ctor(System.Double,System.Double,System.Double,ArcGIS.Core.Geometry.SpatialReference)
        // cref: ArcGIS.Core.Geometry.MapPointBuilderEx.#ctor(System.Double,System.Double,System.Boolean,System.Double,System.Boolean,System.Double,System.Boolean,System.Int32,ArcGIS.Core.Geometry.SpatialReference)
        #region Construct a MapPoint

        // Use a builder convenience method or use a builder constructor.

        // create a 3d point with M
        MapPoint pt = MapPointBuilderEx.CreateMapPoint(1.0, 2.0, 3.0, 4.0);


        // builderEx constructors don't need to run on the MCT.
        MapPointBuilderEx mb = new MapPointBuilderEx(1.0, 2.0, 3.0, 4.0);
        // do something with the builder

        MapPoint ptWithM = mb.ToGeometry();


        MapPoint clone = ptWithM.Clone() as MapPoint;
        MapPoint anotherM = MapPointBuilderEx.CreateMapPoint(ptWithM);


        MapPointBuilderEx builderEx = new MapPointBuilderEx(1.0, 2.0, 3.0);
        builderEx.HasM = true;
        builderEx.M = 4.0;

        pt = builderEx.ToGeometry() as MapPoint;


        // or another alternative with builderEx constructor
        builderEx = new MapPointBuilderEx(1.0, 2.0, true, 3.0, true, 4.0, false, 0);
        pt = builderEx.ToGeometry() as MapPoint;


        // or use a builderEx convenience method
        pt = MapPointBuilderEx.CreateMapPoint(1.0, 2.0, 3.0, 4.0);

        #endregion
      }

      {
        // cref: ArcGIS.Core.Geometry.MapPointBuilderEx.#ctor(System.Double,System.Double,System.Double,ArcGIS.Core.Geometry.SpatialReference)
        // cref: ArcGIS.Core.Geometry.MapPointBuilderEx.HasZ
        // cref: ArcGIS.Core.Geometry.MapPointBuilderEx.HasM
        // cref: ArcGIS.Core.Geometry.MapPointBuilderEx.HasID
        // cref: ArcGIS.Core.Geometry.MapPointBuilderEx.ToGeometry
        // cref: ArcGIS.Core.Geometry.MapPointBuilderEx.X
        // cref: ArcGIS.Core.Geometry.MapPointBuilderEx.Y
        // cref: ArcGIS.Core.Geometry.MapPointBuilderEx.Z
        // cref: ArcGIS.Core.Geometry.MapPointBuilderEx.M
        // cref: ArcGIS.Core.Geometry.MapPoint.X
        // cref: ArcGIS.Core.Geometry.MapPoint.Y
        // cref: ArcGIS.Core.Geometry.MapPoint.Z
        // cref: ArcGIS.Core.Geometry.MapPoint.M
        // cref: ArcGIS.Core.Geometry.MapPoint.ID
        // cref: ArcGIS.Core.Geometry.Geometry.HasZ
        // cref: ArcGIS.Core.Geometry.Geometry.HasM
        // cref: ArcGIS.Core.Geometry.Geometry.HasID
        // cref: ArcGIS.Core.Geometry.MapPoint.IsEmpty
        // cref: ArcGIS.Core.Geometry.MapPoint.IsEqual
        // cref: ArcGIS.Core.Geometry.MapPointBuilderEx.CreateMapPoint(ArcGIS.Core.Geometry.MapPoint, ArcGIS.Core.Geometry.SpatialReference)
        // cref: ArcGIS.Core.Geometry.MapPointBuilderEx.#ctor(ArcGIS.Core.Geometry.MapPoint, ArcGIS.Core.Geometry.SpatialReference)
        // cref: ArcGIS.Core.Geometry.MapPointBuilderEx.ID
        // cref: ArcGIS.Core.Geometry.MapPointBuilderEx.IsEmpty
        // cref: ArcGIS.Core.Geometry.GeometryBuilderEx.HasZ
        // cref: ArcGIS.Core.Geometry.GeometryBuilderEx.HasM
        // cref: ArcGIS.Core.Geometry.GeometryBuilderEx.HasID
        // cref: ArcGIS.Core.Geometry.GeometryBuilderEx.IsEmpty
        #region MapPoint Builder Properties

        // Use a builderEx convenience method or a builderEx constructor.
        //  neither need to run on the MCT.

        MapPoint point1 = null;
        MapPoint point2 = null;

        MapPointBuilderEx mb = new MapPointBuilderEx(1.0, 2.0, 3.0);
        bool bhasZ = mb.HasZ;          // hasZ = true
        bool bhasM = mb.HasM;          // hasM = false
        bool bhasID = mb.HasID;        // hasID = false

        // do something with the builder

        point1 = mb.ToGeometry();

        // change some of the builder properties
        mb.X = 11;
        mb.Y = 22;
        mb.HasZ = false;
        mb.HasM = true;
        mb.M = 44;
        // create another point
        point2 = mb.ToGeometry();

        double x = point1.X;                  // x = 1.0
        double y = point1.Y;                  // y = 2.0
        double z = point1.Z;                  // z = 3.0
        double m = point1.M;                  // m = Nan
        int ID = point1.ID;                   // ID = 0
        bool hasZ = point1.HasZ;              // hasZ = true
        bool hasM = point1.HasM;              // hasM = false
        bool hasID = point1.HasID;            // hasID = false
        bool isEmpty = point1.IsEmpty;        // isEmpty = false

        bool isEqual = point1.IsEqual(point2);    // isEqual = false


        // BuilderEx convenience methods 
        MapPoint point3 = MapPointBuilderEx.CreateMapPoint(point1);
        x = point3.X;                   // x = 1.0
        y = point3.Y;                   // y = 2.0
        z = point3.Z;                   // z = 3.0
        m = point3.M;                   // m = Nan
        ID = point3.ID;                 // ID = 0
        hasZ = point3.HasZ;             // hasZ = true
        hasM = point3.HasM;             // hasM = false
        hasID = point3.HasID;           // hasID = false


        MapPointBuilderEx builderEx = new MapPointBuilderEx(point1);
        x = builderEx.X;              // x = 1.0
        y = builderEx.Y;              // y = 2.0
        z = builderEx.Z;              // z = 3.0
        m = builderEx.M;              // m = Nan
        ID = builderEx.ID;            // ID = 0
        hasZ = builderEx.HasZ;        // hasZ = true
        hasM = builderEx.HasM;        // hasM = false
        hasID = builderEx.HasID;      // hasID = false
        isEmpty = builderEx.IsEmpty;     // isEmpty = false

        MapPoint point4 = builderEx.ToGeometry() as MapPoint;


        MapPoint point5 = MapPointBuilderEx.CreateMapPoint(point1);
        x = point5.X;              // x = 1.0
        y = point5.Y;              // y = 2.0
        z = point5.Z;              // z = 3.0
        m = point5.M;              // m = Nan
        ID = point5.ID;            // ID = 0
        hasZ = point5.HasZ;        // hasZ = true
        hasM = point5.HasM;        // hasM = false
        hasID = point5.HasID;      // hasID = false
        isEmpty = point5.IsEmpty;     // isEmpty = false

        #endregion
      }

      {
        // cref: ArcGIS.Core.Geometry.MapPoint.IsEqual(ArcGIS.Core.Geometry.MapPoint)
        #region MapPoint IsEqual

        MapPoint pt1 = MapPointBuilderEx.CreateMapPoint(1, 2, 3, 4, 5);
        int ID = pt1.ID;           // ID = 5
        bool hasID = pt1.HasID;     // hasID = true

        MapPoint pt2 = MapPointBuilderEx.CreateMapPoint(1, 2, 3, 4, 0);
        ID = pt2.ID;        // ID = 0
        hasID = pt2.HasID;  // hasID = true

        MapPoint pt3 = MapPointBuilderEx.CreateMapPoint(1, 2, 3, 4);
        ID = pt3.ID;          // ID = 0
        hasID = pt3.HasID;    // hasID = false

        MapPoint pt4 = MapPointBuilderEx.CreateMapPoint(1, 2, 3, 44);
        ID = pt4.ID;          // ID = 0
        hasID = pt4.HasID;    // hasID = false
        bool hasM = pt4.HasM; // hasM = true

        MapPoint pt5 = MapPointBuilderEx.CreateMapPoint(1, 2, 3);
        ID = pt5.ID;          // ID = 0
        hasID = pt5.HasID;    // hasID = false
        hasM = pt5.HasM;      // hasM = false

        bool isEqual = pt1.IsEqual(pt2);   // isEqual = false, different IDs
        isEqual = pt2.IsEqual(pt3);        // isEqual = false, HasId is different
        isEqual = pt4.IsEqual(pt3);        // isEqual = false, different Ms
        isEqual = pt1.IsEqual(pt5);        // isEqual = false, pt has M, id but pt5 does not.  

        #endregion
      }
    }

    private static void ZoomToGeographicCoordinates(double x, double y, double buffer_size)
    {
      // cref: ArcGIS.Desktop.Mapping.MapView.ZoomTo(ArcGIS.Core.Geometry.Geometry,Nullable(System.TimeSpan),System.Boolean)
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Buffer
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Project
      #region Zoom to a specified point

      //Create a point
      var pt = MapPointBuilderEx.CreateMapPoint(x, y, 
                     SpatialReferenceBuilder.CreateSpatialReference(4326));
      //Buffer it - for purpose of zoom
      var poly = GeometryEngine.Instance.Buffer(pt, buffer_size);

      //do we need to project the buffer polygon?
      if (!MapView.Active.Map.SpatialReference.IsEqual(poly.SpatialReference))
      {
        //project the polygon
        poly = GeometryEngine.Instance.Project(poly, MapView.Active.Map.SpatialReference);
      }

      // Must run on MCT.
      QueuedTask.Run(() =>
      {
        //Zoom - add in a delay for animation effect
        MapView.Active.ZoomTo(poly, new TimeSpan(0, 0, 0, 3));
      });

      #endregion
    }

    #region ProSnippet Group: Polyline
    #endregion

    public void Polyline()
    {
      {
        // cref: ArcGIS.Core.Geometry.MapPointBuilderEx.CreateMapPoint(System.Double,System.Double,ArcGIS.Core.Geometry.SpatialReference)
        // cref: ArcGIS.Core.Geometry.PolylineBuilderEx.CreatePolyline(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.MapPoint},ArcGIS.Core.Geometry.SpatialReference)
        // cref: ArcGIS.Core.Geometry.PolylineBuilderEx.#ctor(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.MapPoint},AttributeFlags attributes, ArcGIS.Core.Geometry.SpatialReference)
        // cref: ArcGIS.Core.Geometry.SpatialReferences.WGS84
        // cref: ArcGIS.Core.Geometry.AttributeFlags
        #region Construct a Polyline - from an enumeration of MapPoints

        // Use a builderEx convenience method or a builderEx constructor.
        // neither need to run on the MCT

        MapPoint startPt = MapPointBuilderEx.CreateMapPoint(1.0, 1.0);
        MapPoint endPt = MapPointBuilderEx.CreateMapPoint(2.0, 1.0);

        List<MapPoint> list = new List<MapPoint>();
        list.Add(startPt);
        list.Add(endPt);

        Polyline polyline = PolylineBuilderEx.CreatePolyline(list, SpatialReferences.WGS84);

        // use AttributeFlags.None since we only have 2D points in the list
        PolylineBuilderEx pb = new PolylineBuilderEx(list, AttributeFlags.None);
        pb.SpatialReference = SpatialReferences.WGS84;
        Polyline polyline2 = pb.ToGeometry();

        //     use AttributeFlags.NoAttributes because we only have 2d points in the list
        Polyline polyline4 = PolylineBuilderEx.CreatePolyline(list, AttributeFlags.None);

        #endregion
      }

      {
        Polyline polyline = null;

        // cref: ArcGIS.Core.Geometry.Multipart.Points
        // cref: ArcGIS.Core.Geometry.Multipart.PointCount
        // cref: ArcGIS.Core.Geometry.ReadOnlyPointCollection.GetEnumerator
        // cref: ArcGIS.Core.Geometry.Multipart.Copy2DCoordinatesToList
        // cref: ArcGIS.Core.Geometry.Multipart.Copy3DCoordinatesToList
        // cref: ArcGIS.Core.Geometry.ReadOnlyPointCollection.Copy2DCoordinatesToList(System.Int32,System.Int32,System.Collections.Generic.ICollection{ArcGIS.Core.Geometry.Coordinate2D}@)
        // cref: ArcGIS.Core.Geometry.ReadOnlyPointCollection.Copy3DCoordinatesToList(System.Int32,System.Int32,System.Collections.Generic.ICollection{ArcGIS.Core.Geometry.Coordinate3D}@)
        // cref: ArcGIS.Core.Geometry.ReadOnlyPointCollection.CopyPointsToList(System.Int32,System.Int32,System.Collections.Generic.ICollection{ArcGIS.Core.Geometry.MapPoint}@)
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

        // cref: ArcGIS.Core.Geometry.Multipart.PartCount
        // cref: ArcGIS.Core.Geometry.Multipart.Parts
        #region Get the parts of a Polyline

        int numParts = polyline.PartCount;
        // get the parts as a readonly collection
        ReadOnlyPartCollection parts = polyline.Parts;

        #endregion

        // cref: ArcGIS.Core.Geometry.Multipart.Parts
        // cref: ArcGIS.Core.Geometry.ReadOnlyPartCollection.GetEnumerator
        // cref: ArcGIS.Core.Geometry.ReadOnlySegmentCollection
        // cref: ArcGIS.Core.Geometry.SegmentType
        // cref: ArcGIS.Core.Geometry.Segment.Length
        // cref: ArcGIS.Core.Geometry.Segment.SegmentType
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

        // cref: ArcGIS.Core.Geometry.PolylineBuilderEx.#ctor(ArcGIS.Core.Geometry.Polyline)
        // cref: ArcGIS.Core.Geometry.MultipartBuilderEx.ReverseOrientation
        // cref: ArcGIS.Core.Geometry.PolylineBuilderEx.ToGeometry
        #region Reverse the order of points in a Polyline

        var polylineBuilder = new PolylineBuilderEx(polyline);
        polylineBuilder.ReverseOrientation();
        Polyline reversedPolyline = polylineBuilder.ToGeometry();
        #endregion

        // cref: ArcGIS.Core.Geometry.Multipart.GetAllSegments(System.Collections.Generic.ICollection{ArcGIS.Core.Geometry.Segment}@)
        // cref: ArcGIS.Core.Geometry.PolylineBuilderEx.CreatePolyline(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Segment},ArcGIS.Core.Geometry.SpatialReference)
        #region Get the segments of a Polyline

        ICollection<Segment> collection = new List<Segment>();
        polyline.GetAllSegments(ref collection);
        int numSegments = collection.Count;    // = 10

        IList<Segment> iList = collection as IList<Segment>;
        for (int i = 0; i < numSegments; i++)
        {
          // do something with iList[i]
        }

        // use the segments to build another polyline
        Polyline polylineFromSegments = PolylineBuilderEx.CreatePolyline(collection);

        #endregion
      }

      {
        // cref: ArcGIS.Core.Geometry.PolylineBuilderEx.#ctor(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.MapPoint},ArcGIS.Core.Geometry.AttributeFlags,ArcGIS.Core.Geometry.SpatialReference)
        // cref: ArcGIS.Core.Geometry.MultipartBuilderEx.AddPart(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.MapPoint})
        // cref: ArcGIS.Core.Geometry.MultipartBuilderEx.RemovePart(System.Int32)
        // cref: ArcGIS.Core.Geometry.PolylineBuilderEx.ToGeometry
        // cref: ArcGIS.Core.Geometry.AttributeFlags
        #region Build a multi-part Polyline

        List<MapPoint> firstPoints = new List<MapPoint>();
        firstPoints.Add(MapPointBuilderEx.CreateMapPoint(1.0, 1.0));
        firstPoints.Add(MapPointBuilderEx.CreateMapPoint(1.0, 2.0));
        firstPoints.Add(MapPointBuilderEx.CreateMapPoint(2.0, 2.0));
        firstPoints.Add(MapPointBuilderEx.CreateMapPoint(2.0, 1.0));

        List<MapPoint> nextPoints = new List<MapPoint>();
        nextPoints.Add(MapPointBuilderEx.CreateMapPoint(11.0, 1.0));
        nextPoints.Add(MapPointBuilderEx.CreateMapPoint(11.0, 2.0));
        nextPoints.Add(MapPointBuilderEx.CreateMapPoint(12.0, 2.0));
        nextPoints.Add(MapPointBuilderEx.CreateMapPoint(12.0, 1.0));

        // use AttributeFlags.None since we have 2D points in the list
        PolylineBuilderEx pBuilder = new PolylineBuilderEx(firstPoints, AttributeFlags.None);
        pBuilder.AddPart(nextPoints);

        Polyline polyline = pBuilder.ToGeometry();
        // polyline has 2 parts

        pBuilder.RemovePart(0);
        polyline = pBuilder.ToGeometry();
        // polyline has 1 part

        #endregion
      }

      Geometry sketchGeometry = null;
      {
        // cref: ArcGIS.Core.Geometry.Multipart.Points
        // cref: ArcGIS.Core.Geometry.Multipart.Parts
        // cref: ArcGIS.Core.Geometry.ReadOnlySegmentCollection
        // cref: ArcGIS.Core.Geometry.LineSegment
        // cref: ArcGIS.Core.Geometry.Segment.StartPoint
        #region StartPoint of a Polyline
        // Method 1: Get the start point of the polyline by converting the polyline 
        //    into a collection of points and getting the first point

        // sketchGeometry is a Polyline
        // get the sketch as a point collection
        var pointCol = ((Multipart)sketchGeometry).Points;
        // Get the start point of the line
        var firstPoint = pointCol[0];

        // Method 2: Convert polyline into a collection of line segments 
        //   and get the "StartPoint" of the first line segment.
        var polylineGeom = sketchGeometry as ArcGIS.Core.Geometry.Polyline;
        var polyLineParts = polylineGeom.Parts;

        ReadOnlySegmentCollection polylineSegments = polyLineParts.First();

        //get the first segment as a LineSegment
        var firsLineSegment = polylineSegments.First() as LineSegment;

        //Now get the start Point
        var startPoint = firsLineSegment.StartPoint;

        #endregion
      }
    }

    public void ClothoidByAngle()
    {
      // cref: ArcGIS.Core.Geometry.PolylineBuilderEx.CreatePolyline(ArcGIS.Core.Geometry.MapPoint,System.Double,System.Double,System.Double,ArcGIS.Core.Geometry.ArcOrientation,ArcGIS.Core.Geometry.ClothoidCreateMethod,System.Double,ArcGIS.Core.Geometry.CurveDensifyMethod,System.Double,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.PolylineBuilderEx.QueryClothoidParameters(ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.MapPoint,System.Double,System.Double,System.Double,ArcGIS.Core.Geometry.ArcOrientation,ArcGIS.Core.Geometry.ClothoidCreateMethod,System.Double,ArcGIS.Core.Geometry.MapPoint@,System.Double@,System.Double@,System.Double@,System.Double@,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.ClothoidCreateMethod
      // cref: ArcGIS.Core.Geometry.CurveDensifyMethod
      #region Construct a Clothoid by Angle

      MapPoint startPoint = MapPointBuilderEx.CreateMapPoint(0, 0);
      double tangentDirection = Math.PI / 6;
      ArcOrientation orientation = ArcOrientation.ArcCounterClockwise;
      double startRadius = double.PositiveInfinity;
      double endRadius = 0.2;
      ClothoidCreateMethod createMethod = ClothoidCreateMethod.ByAngle;
      double angle = Math.PI / 2;
      CurveDensifyMethod densifyMethod = CurveDensifyMethod.ByLength;
      double densifyParameter = 0.1;

      Polyline polyline = PolylineBuilderEx.CreatePolyline(startPoint, tangentDirection, startRadius, endRadius, orientation, createMethod, angle, densifyMethod, densifyParameter, SpatialReferences.WGS84);

      int numPoints = polyline.PointCount;
      MapPoint queryPoint = polyline.Points[numPoints - 2];

      MapPoint pointOnPath;
      double radiusCalculated, tangentDirectionCalculated, lengthCalculated, angleCalculated;

      PolylineBuilderEx.QueryClothoidParameters(queryPoint, startPoint, tangentDirection, startRadius, endRadius, orientation, createMethod, angle, out pointOnPath, out radiusCalculated, out tangentDirectionCalculated, out lengthCalculated, out angleCalculated, SpatialReferences.WGS84);

      #endregion
    }

    public void ClothoidByLength()
    {
      // cref: ArcGIS.Core.Geometry.PolylineBuilderEx.#ctor(ArcGIS.Core.Geometry.MapPoint,System.Double,System.Double,System.Double,ArcGIS.Core.Geometry.ArcOrientation,ArcGIS.Core.Geometry.ClothoidCreateMethod,System.Double,ArcGIS.Core.Geometry.CurveDensifyMethod,System.Double,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.PolylineBuilderEx.QueryClothoidParameters(ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.MapPoint,System.Double,System.Double,System.Double,ArcGIS.Core.Geometry.ArcOrientation,ArcGIS.Core.Geometry.ClothoidCreateMethod,System.Double,ArcGIS.Core.Geometry.MapPoint@,System.Double@,System.Double@,System.Double@,System.Double@,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.ClothoidCreateMethod
      // cref: ArcGIS.Core.Geometry.ArcOrientation
      #region Construct a Clothoid by Length

      MapPoint startPoint = MapPointBuilderEx.CreateMapPoint(0, 0);
      MapPoint queryPoint = MapPointBuilderEx.CreateMapPoint(3.8, 1);
      double tangentDirection = 0;
      ArcOrientation orientation = ArcOrientation.ArcCounterClockwise;
      double startRadius = double.PositiveInfinity;
      double endRadius = 1;
      ClothoidCreateMethod createMethod = ClothoidCreateMethod.ByLength;
      double curveLength = 10;
      MapPoint pointOnPath;
      double radiusCalculated, tangentDirectionCalculated, lengthCalculated, angleCalculated;


      PolylineBuilderEx.QueryClothoidParameters(queryPoint, startPoint, tangentDirection, startRadius, endRadius, orientation, createMethod, curveLength, out pointOnPath, out radiusCalculated, out tangentDirectionCalculated, out lengthCalculated, out angleCalculated, SpatialReferences.WGS84);

      pointOnPath = MapPointBuilderEx.CreateMapPoint(3.7652656620171379, 1.0332006103128575);
      radiusCalculated = 2.4876382887687227;
      tangentDirectionCalculated = 0.80797056423543978;
      lengthCalculated = 4.0198770235802987;
      angleCalculated = 0.80797056423544011;

      queryPoint = MapPointBuilderEx.CreateMapPoint(1.85, 2.6);

      PolylineBuilderEx.QueryClothoidParameters(queryPoint, startPoint, tangentDirection, startRadius, endRadius, orientation, createMethod, curveLength, out pointOnPath, out radiusCalculated, out tangentDirectionCalculated, out lengthCalculated, out angleCalculated, SpatialReferences.WGS84);

      pointOnPath = MapPointBuilderEx.CreateMapPoint(1.8409964973501549, 2.6115979967308132);
      radiusCalculated = 1;
      tangentDirectionCalculated = -1.2831853071795867;
      lengthCalculated = 10;
      angleCalculated = 5;
       
      tangentDirection = Math.PI / 4;
      orientation = ArcOrientation.ArcClockwise;
      startRadius = double.PositiveInfinity;
      endRadius = 0.8;
      createMethod = ClothoidCreateMethod.ByLength;
      curveLength = 10;

      Polyline polyline = PolylineBuilderEx.CreatePolyline(startPoint, tangentDirection, startRadius, endRadius, orientation, createMethod, curveLength, CurveDensifyMethod.ByLength, 0.5, SpatialReferences.WGS84);

      #endregion
    }

    public void SplitPolyline()
    {
      // cref: ArcGIS.Core.Geometry.PolylineBuilderEx.SplitAtDistance(System.Double,System.Boolean,System.Boolean)
      // cref: ArcGIS.Core.Geometry.PolylineBuilderEx.SplitPartAtDistance(System.Int32,System.Double,System.Boolean,System.Boolean)
      #region Split Polyline at distance

      // create list of points
      MapPoint startPt = MapPointBuilderEx.CreateMapPoint(1.0, 1.0);
      MapPoint endPt = MapPointBuilderEx.CreateMapPoint(2.0, 1.0);

      List<MapPoint> list = new List<MapPoint>();
      list.Add(startPt);
      list.Add(endPt);

      // BuilderEx constructors don't need to run on the MCT.

      // use the PolylineBuilder as we wish to manipulate the geometry
      // use AttributeFlags.None as we have 2D points
      PolylineBuilderEx polylineBuilder = new PolylineBuilderEx(list, AttributeFlags.None);
      // split at a distance 0.75
      polylineBuilder.SplitAtDistance(0.75, false);
      // get the polyline
      Polyline p = polylineBuilder.ToGeometry();
      // polyline p should have 3 points  (1,1), (1.75, 1), (2,1)

      // add another path
      MapPoint p1 = MapPointBuilderEx.CreateMapPoint(4.0, 1.0);
      MapPoint p2 = MapPointBuilderEx.CreateMapPoint(6.0, 1.0);
      MapPoint p3 = MapPointBuilderEx.CreateMapPoint(7.0, 1.0);
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

      #endregion
    }

    #region ProSnippet Group: Polygon
    #endregion

    public void ConstructPolygon()
    {
      {
        // cref: ArcGIS.Core.Geometry.PolygonBuilderEx.#ctor(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.MapPoint},ArcGIS.Core.Geometry.AttributeFlags,ArcGIS.Core.Geometry.SpatialReference)
        // cref: ArcGIS.Core.Geometry.PolygonBuilderEx.CreatePolygon(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.MapPoint},ArcGIS.Core.Geometry.SpatialReference)
        // cref: ArcGIS.Core.Geometry.PolygonBuilderEx.ToGeometry
        #region Construct a Polygon - from an enumeration of MapPoints

        // Use a builderEx convenience method or use a builderEx constructor.

        MapPoint pt1 = MapPointBuilderEx.CreateMapPoint(1.0, 1.0);
        MapPoint pt2 = MapPointBuilderEx.CreateMapPoint(1.0, 2.0);
        MapPoint pt3 = MapPointBuilderEx.CreateMapPoint(2.0, 2.0);
        MapPoint pt4 = MapPointBuilderEx.CreateMapPoint(2.0, 1.0);

        List<MapPoint> list = new List<MapPoint>() { pt1, pt2, pt3, pt4 };

        Polygon polygon = PolygonBuilderEx.CreatePolygon(list, SpatialReferences.WGS84);
        // polygon.HasZ will be false - it is determined by the HasZ flag of the points in the list

        // or specifically use AttributeFlags.NoAttributes
        polygon = PolygonBuilderEx.CreatePolygon(list, AttributeFlags.None);

        // use AttributeFlags.None as we have 2D points
        PolygonBuilderEx polygonBuilder = new PolygonBuilderEx(list, AttributeFlags.None);
        polygonBuilder.SpatialReference = SpatialReferences.WGS84;
        polygon = polygonBuilder.ToGeometry();

        #endregion
      }

      {
        // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.CreateEnvelope(ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.SpatialReference)
        // cref: ArcGIS.Core.Geometry.PolygonBuilderEx.CreatePolygon(ArcGIS.Core.Geometry.Envelope,ArcGIS.Core.Geometry.SpatialReference)
        // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.CreateEnvelope(ArcGIS.Core.Geometry.Envelope,ArcGIS.Core.Geometry.SpatialReference)
        #region Construct a Polygon - from an Envelope

        // Use a builderEx convenience method or use a builderEx constructor.

        MapPoint minPt = MapPointBuilderEx.CreateMapPoint(1.0, 1.0);
        MapPoint maxPt = MapPointBuilderEx.CreateMapPoint(2.0, 2.0);

        // Create an envelope
        Envelope env = EnvelopeBuilderEx.CreateEnvelope(minPt, maxPt);

        Polygon polygonFromEnv = PolygonBuilderEx.CreatePolygon(env);

        PolygonBuilderEx polygonBuilderEx = new PolygonBuilderEx(env);
        polygonBuilderEx.SpatialReference = SpatialReferences.WGS84;
        polygonFromEnv = polygonBuilderEx.ToGeometry() as Polygon;

        #endregion Create a Polygon from an Envelope
      }

      {
        Polygon polygon = null;

        // cref: ArcGIS.Core.Geometry.Multipart.Points
        // cref: ArcGIS.Core.Geometry.Multipart.Copy2DCoordinatesToList
        // cref: ArcGIS.Core.Geometry.Multipart.Copy3DCoordinatesToList
        #region Get the points of a Polygon

        // get the points as a readonly Collection
        ReadOnlyPointCollection pts = polygon.Points;

        // get an enumeration of the points
        IEnumerator<MapPoint> enumPts = polygon.Points.GetEnumerator();

        // get the point coordinates as a readonly list of Coordinate2D
        IReadOnlyList<Coordinate2D> coordinates = polygon.Copy2DCoordinatesToList();

        // get the point coordinates as a readonly list of Coordinate3D
        IReadOnlyList<Coordinate3D> coordinates3D = polygon.Copy3DCoordinatesToList();
        #endregion

        // cref: ArcGIS.Core.Geometry.Multipart.Parts
        #region Get the parts of a Polygon
        // get the parts as a readonly collection
        ReadOnlyPartCollection parts = polygon.Parts;

        #endregion

        // cref: ArcGIS.Core.Geometry.Multipart.Parts
        // cref: ArcGIS.Core.Geometry.ReadOnlySegmentCollection
        #region Enumerate the parts of a Polygon

        int numSegments = 0;
        IEnumerator<ReadOnlySegmentCollection> segments = polygon.Parts.GetEnumerator();
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

        // cref: ArcGIS.Core.Geometry.Multipart.GetAllSegments(System.Collections.Generic.ICollection{ArcGIS.Core.Geometry.Segment}@)
        // cref: ArcGIS.Core.Geometry.PolygonBuilderEx.CreatePolygon(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Segment},ArcGIS.Core.Geometry.SpatialReference)
        #region Get the segments of a Polygon

        List<Segment> segmentList = new List<Segment>(30);
        ICollection<Segment> collection = segmentList;
        polygon.GetAllSegments(ref collection);
        // segmentList.Count = 4
        // segmentList.Capacity = 30

        // use the segments to build another polygon
        Polygon polygonFromSegments = PolygonBuilderEx.CreatePolygon(collection);
        #endregion
      }

      // cref: ArcGIS.Core.Geometry.Coordinate2D.#ctor(System.Double,System.Double)
      // cref: ArcGIS.Core.Geometry.PolygonBuilderEx.#ctor(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Coordinate2D},ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.PolygonBuilderEx.ToGeometry
      // cref: ArcGIS.Core.Geometry.Polygon.Area
      // cref: ArcGIS.Core.Geometry.MultipartBuilderEx.AddPart(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Coordinate2D})
      // cref: ArcGIS.Core.Geometry.GeometryEngine.Area(ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.Area(ArcGIS.Core.Geometry.Geometry)
      #region Build a donut polygon

      List<Coordinate2D> outerCoordinates = new List<Coordinate2D>();
      outerCoordinates.Add(new Coordinate2D(10.0, 10.0));
      outerCoordinates.Add(new Coordinate2D(10.0, 20.0));
      outerCoordinates.Add(new Coordinate2D(20.0, 20.0));
      outerCoordinates.Add(new Coordinate2D(20.0, 10.0));

      // define the inner polygon as anti-clockwise
      List<Coordinate2D> innerCoordinates = new List<Coordinate2D>();
      innerCoordinates.Add(new Coordinate2D(13.0, 13.0));
      innerCoordinates.Add(new Coordinate2D(17.0, 13.0));
      innerCoordinates.Add(new Coordinate2D(17.0, 17.0));
      innerCoordinates.Add(new Coordinate2D(13.0, 17.0));

      PolygonBuilderEx pbEx = new PolygonBuilderEx(outerCoordinates);
      Polygon donutEx = pbEx.ToGeometry() as Polygon;
      double areaEx = donutEx.Area;       // area = 100

      pbEx.AddPart(innerCoordinates);
      donutEx = pbEx.ToGeometry() as Polygon;

      areaEx = donutEx.Area;    // area = 84.0

      areaEx = GeometryEngine.Instance.Area(donutEx);    // area = 84.0


      #endregion
    }

    // cref: ArcGIS.Core.Geometry.PolygonBuilderEx.CreatePolygon(IEnumerable{Polygon},SpatialReference)
    #region Create an N-sided regular polygon

    // <summary>
    // Create an N-sided regular polygon.  A regular sided polygon is a polygon that is equiangular (all angles are equal in measure) 
    // and equilateral (all sides are equal in length).  See https://en.wikipedia.org/wiki/Regular_polygon
    // </summary>
    // <param name="numSides">The number of sides in the polygon.</param>
    // <param name="center">The center of the polygon.</param>
    // <param name="radius">The distance from the center of the polygon to a vertex.</param>
    // <param name="rotation">The rotation angle in radians of the start point of the polygon. The start point will be
    // rotated counterclockwise from the positive x-axis.</param>
    // <returns>N-sided regular polygon.</returns>
    // <exception cref="ArgumentException">Number of sides is less than 3.</exception>
    public Polygon CreateRegularPolygon(int numSides, Coordinate2D center, double radius, double rotation)
    {
      if (numSides < 3)
        throw new ArgumentException();

      Coordinate2D[] coords = new Coordinate2D[numSides + 1];

      double centerX = center.X;
      double centerY = center.Y;
      double x = radius * Math.Cos(rotation) + centerX;
      double y = radius * Math.Sin(rotation) + centerY;
      Coordinate2D start = new Coordinate2D(x, y);
      coords[0] = start;

      double da = 2 * Math.PI / numSides;
      for (int i = 1; i < numSides; i++)
      {
        x = radius * Math.Cos(i * da + rotation) + centerX;
        y = radius * Math.Sin(i * da + rotation) + centerY;

        coords[i] = new Coordinate2D(x, y);
      }

      coords[numSides] = start;

      return PolygonBuilderEx.CreatePolygon(coords);
    }

    #endregion

    // cref: ArcGIS.Core.Geometry.Polygon.ExteriorRingCount
    // cref: ArcGIS.Core.Geometry.Polygon.GetExteriorRings(System.Boolean)
    // cref: ArcGIS.Core.Geometry.Polygon.IsExteriorRing(System.Int32)
    // cref: ArcGIS.Core.Geometry.Polygon.GetExteriorRing(System.Int32,System.Boolean)
    #region Get the exterior rings of a polygon - polygon.GetExteriorRing
    public void GetExteriorRings(Polygon inputPolygon)
    {
      if (inputPolygon == null || inputPolygon.IsEmpty)
        return;

      // polygon part count
      int partCount = inputPolygon.PartCount;
      // polygon exterior ring count
      int numExtRings = inputPolygon.ExteriorRingCount;
      // get set of exterior rings for the polygon
      IList<Polygon> extRings = inputPolygon.GetExteriorRings();

      // test each part for "exterior ring"
      for (int idx = 0; idx < partCount; idx++)
      {
        bool isExteriorRing = inputPolygon.IsExteriorRing(idx);
        var ring = inputPolygon.GetExteriorRing(idx);
      }
    }
    #endregion

    #region ProSnippet Group: Envelope
    #endregion

    public void ConstructEnvelope()
    {
      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.#ctor(ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.ToGeometry
      #region Construct an Envelope 

      // Use a builderEx convenience method or use a builderEx constructor.

      MapPoint minPt = MapPointBuilderEx.CreateMapPoint(1.0, 1.0);
      MapPoint maxPt = MapPointBuilderEx.CreateMapPoint(2.0, 2.0);

      Envelope envelope = EnvelopeBuilderEx.CreateEnvelope(minPt, maxPt);

      EnvelopeBuilderEx builderEx = new EnvelopeBuilderEx(minPt, maxPt);
      envelope = builderEx.ToGeometry() as Envelope;

      #endregion

      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.FromJson(System.String)
      #region Construct an Envelope - from a JSON string

      string jsonString = "{ \"xmin\" : 1, \"ymin\" : 2,\"xmax\":3,\"ymax\":4,\"spatialReference\":{\"wkid\":4326}}";
      Envelope envFromJson = EnvelopeBuilderEx.FromJson(jsonString);

      #endregion
    }

    public void EnvelopeUnion()
    {
      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.CreateEnvelope(System.Double,System.Double,System.Double,System.Double,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.Envelope.Union(ArcGIS.Core.Geometry.Envelope)
      // cref: ArcGIS.Core.Geometry.Envelope.Area
      // cref: ArcGIS.Core.Geometry.Envelope.Depth
      // cref: ArcGIS.Core.Geometry.Envelope.Height
      // cref: ArcGIS.Core.Geometry.Envelope.Width
      // cref: ArcGIS.Core.Geometry.Envelope.Length
      // cref: ArcGIS.Core.Geometry.Envelope.Center
      // cref: ArcGIS.Core.Geometry.Envelope.CenterCoordinate
      // cref: ArcGIS.Core.Geometry.Envelope.IsEmpty
      // cref: ArcGIS.Core.Geometry.Envelope.PointCount
      // cref: ArcGIS.Core.Geometry.Envelope.XMin
      // cref: ArcGIS.Core.Geometry.Envelope.XMax
      // cref: ArcGIS.Core.Geometry.Envelope.YMin
      // cref: ArcGIS.Core.Geometry.Envelope.YMax
      // cref: ArcGIS.Core.Geometry.Envelope.ZMin
      // cref: ArcGIS.Core.Geometry.Envelope.ZMax
      // cref: ArcGIS.Core.Geometry.Envelope.MMin
      // cref: ArcGIS.Core.Geometry.Envelope.MMax
      // cref: ArcGIS.Core.Geometry.Envelope.IDMin
      // cref: ArcGIS.Core.Geometry.Envelope.IDMax
      // cref: ArcGIS.Core.Geometry.Envelope.IsEqual(ArcGIS.Core.Geometry.Envelope)
      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.Union(ArcGIS.Core.Geometry.Envelope)
      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.Depth
      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.Height
      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.Width
      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.Center
      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.CenterCoordinate
      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.IsEmpty
      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.ToGeometry
      #region Union two Envelopes

      // use the convenience builders
      Envelope env1 = EnvelopeBuilderEx.CreateEnvelope(0, 0, 1, 1, SpatialReferences.WGS84);
      Envelope env2 = EnvelopeBuilderEx.CreateEnvelope(0.5, 0.5, 1.5, 1.5, SpatialReferences.WGS84);

      Envelope env3 = env1.Union(env2);

      double area = env3.Area;
      double depth = env3.Depth;
      double height = env3.Height;
      double width = env3.Width;
      double len = env3.Length;

      MapPoint centerPt = env3.Center;
      Coordinate2D coord = env3.CenterCoordinate;

      bool isEmpty = env3.IsEmpty;
      int pointCount = env3.PointCount;

      // coordinates
      //env3.XMin, env3.XMax, env3.YMin, env3.YMax
      //env3.ZMin, env3.ZMax, env3.MMin, env3.MMax

      bool isEqual = env1.IsEqual(env2);    // false


      // or use the builderEx constructors = don't need to run on the MCT.
      EnvelopeBuilderEx builderEx = new EnvelopeBuilderEx(0, 0, 1, 1, SpatialReferences.WGS84);
      builderEx.Union(env2);      // builder is updated to the result

      depth = builderEx.Depth;
      height = builderEx.Height;
      width = builderEx.Width;

      centerPt = builderEx.Center;
      coord = builderEx.CenterCoordinate;

      isEmpty = builderEx.IsEmpty;

      env3 = builderEx.ToGeometry() as Envelope;

      #endregion
    }

    public void EnvelopeIntersection()
    {
      // cref: ArcGIS.Core.Geometry.Envelope.Intersection(ArcGIS.Core.Geometry.Envelope)
      // cref: ArcGIS.Core.Geometry.Envelope.Intersects(ArcGIS.Core.Geometry.Envelope)
      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.Intersection(ArcGIS.Core.Geometry.Envelope)
      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.Intersects(ArcGIS.Core.Geometry.Envelope)
      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.ToGeometry
      #region Intersect two Envelopes

      // use the convenience builders
      Envelope env1 = EnvelopeBuilderEx.CreateEnvelope(0, 0, 1, 1, SpatialReferences.WGS84);
      Envelope env2 = EnvelopeBuilderEx.CreateEnvelope(0.5, 0.5, 1.5, 1.5, SpatialReferences.WGS84);

      bool intersects = env1.Intersects(env2); // true
      Envelope env3 = env1.Intersection(env2);


      // or use the builderEx constructors = don't need to run on the MCT.
      EnvelopeBuilderEx builderEx = new EnvelopeBuilderEx(0, 0, 1, 1, SpatialReferences.WGS84);
      intersects = builderEx.Intersects(env2);
      builderEx.Intersection(env2);   // note this sets the builder to the intersection
      env3 = builderEx.ToGeometry() as Envelope;

      #endregion
    }

    public void EnvelopeExpand()
    {
      // cref: Expand an Envelope;ArcGIS.Core.Geometry.Envelope.Expand(System.Double,System.Double,System.Boolean)
      // cref: Expand an Envelope;ArcGIS.Core.Geometry.EnvelopeBuilderEx.#ctor(System.Double,System.Double,System.Double,System.Double,ArcGIS.Core.Geometry.SpatialReference)
      // cref: Expand an Envelope;ArcGIS.Core.Geometry.EnvelopeBuilderEx.Expand(System.Double,System.Double,System.Boolean)
      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.ToGeometry
      #region Expand an Envelope

      // Use a builderEx convenience method or use a builderEx constructor.

      // convenience methods don't need to run on the MCT.
      Envelope envelope = EnvelopeBuilderEx.CreateEnvelope(100.0, 100.0, 500.0, 500.0);

      // shrink the envelope by 50%
      Envelope result = envelope.Expand(0.5, 0.5, true);


      // builderEx constructors don't need to run on the MCT.
      EnvelopeBuilderEx builderEx = new EnvelopeBuilderEx(100.0, 100.0, 500.0, 500.0);
      builderEx.Expand(0.5, 0.5, true);
      envelope = builderEx.ToGeometry() as Envelope;

      #endregion
    }

    public void EnvelopeUpdate()
    {
      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.#ctor(ArcGIS.Core.Geometry.Coordinate2D,ArcGIS.Core.Geometry.Coordinate2D,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.XMin
      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.XMax
      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.YMin
      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.YMax
      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.ZMin
      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.ZMax
      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.MMin
      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.MMax
      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.IDMax
      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.IDMin
      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.SetZCoords(System.Double,System.Double)
      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.SetXYCoords(ArcGIS.Core.Geometry.Coordinate2D,ArcGIS.Core.Geometry.Coordinate2D)
      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.HasM
      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.SetMCoords(System.Double,System.Double)
      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.SetIDCoords(System.Int32,System.Int32)
      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.ToGeometry
      #region Update Coordinates of an Envelope

      Coordinate2D minCoord = new Coordinate2D(1, 3);
      Coordinate2D maxCoord = new Coordinate2D(2, 4);

      Coordinate2D c1 = new Coordinate2D(0, 5);
      Coordinate2D c2 = new Coordinate2D(1, 3);

      
      // use the EnvelopeBuilderEx.  This constructor doesn't need to run on the MCT.

      EnvelopeBuilderEx builderEx = new EnvelopeBuilderEx(minCoord, maxCoord);
      // builderEx.XMin, YMin, Zmin, MMin  = 1, 3, 0, double.Nan
      // builderEx.XMax, YMax, ZMax, MMax = 2, 4, 0, double.Nan

      // set XMin.  if XMin > XMax; both XMin and XMax change
      builderEx.XMin = 6;
      // builderEx.XMin, YMin, ZMin, MMin  = 6, 3, 0, double.Nan
      // builderEx.XMax, YMax, ZMax, MMax = 6, 4, 0, double.Nan

      // set XMax
      builderEx.XMax = 8;
      // builderEx.XMin, YMin, ZMin, MMin  = 6, 3, 0, double.Nan
      // builderEx.XMax, YMax, ZMax, MMax = 8, 4, 0, double.Nan

      // set XMax.  if XMax < XMin, both XMin and XMax change
      builderEx.XMax = 3;
      // builderEx.XMin, YMin, ZMin, MMin  = 3, 3, 0, double.Nan
      // builderEx.XMax, YMax, ZMax, MMax = 3, 4, 0, double.Nan

      // set YMin
      builderEx.YMin = 2;
      // builderEx.XMin, YMin, ZMin, MMin  = 3, 2, 0, double.Nan
      // builderEx.XMax, YMax, ZMax, MMax = 3, 4, 0, double.Nan

      // set ZMin.  if ZMin > ZMax, both ZMin and ZMax change
      builderEx.ZMin = 3;
      // builderEx.XMin, YMin, ZMin, MMin  = 3, 2, 3, double.Nan
      // builderEx.XMax, YMax, ZMax, MMax = 3, 4, 3, double.Nan

      // set ZMax.  if ZMax < ZMin. both ZMin and ZMax change
      builderEx.ZMax = -1;
      // builderEx.XMin, YMin, ZMin, MMin  = 3, 2, -1, double.Nan
      // builderEx.XMax, YMax, ZMax, MMax = 3, 4, -1, double.Nan

      builderEx.SetZCoords(8, -5);
      // builderEx.XMin, YMin, ZMin, MMin  = 3, 2, -5, double.Nan
      // builderEx.XMax, YMax, ZMax, MMax = 3, 4, 8, double.Nan


      builderEx.SetXYCoords(c1, c2);
      // builderEx.XMin, YMin, ZMin, MMin  = 0, 3, -5, double.Nan
      // builderEx.XMax, YMax, ZMax, MMax = 1, 5, 8, double.Nan


      builderEx.HasM = true;
      builderEx.SetMCoords(2, 5);

      var geomEx = builderEx.ToGeometry();
      #endregion
    }

    #region ProSnippet Group: Multipoint
    #endregion

    public void ConstructMultiPoint()
    {
      // cref: ArcGIS.Core.Geometry.MultipointBuilderEx.#ctor(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.MapPoint},ArcGIS.Core.Geometry.AttributeFlags,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.MultipointBuilderEx.ToGeometry
      // cref: ArcGIS.Core.Geometry.MultipointBuilderEx.PointCount
      // cref: ArcGIS.Core.Geometry.MultipointBuilderEx.CreateMultipoint(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.MapPoint},ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.MultipointBuilderEx.CreateMultipoint(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.MapPoint},ArcGIS.Core.Geometry.AttributeFlags, ArcGIS.Core.Geometry.SpatialReference)
      #region Construct a Multipoint - from an enumeration of MapPoints

      // Use a builderEx convenience method or use a builderEx constructor.

      List<MapPoint> list = new List<MapPoint>();
      list.Add(MapPointBuilderEx.CreateMapPoint(1.0, 1.0));
      list.Add(MapPointBuilderEx.CreateMapPoint(1.0, 2.0));
      list.Add(MapPointBuilderEx.CreateMapPoint(2.0, 2.0));
      list.Add(MapPointBuilderEx.CreateMapPoint(2.0, 1.0));

      // use the builderEx constructors - don't need to run on the MCT.
      //  use AttributeFlags.NoAttributes - we have 2d points in the list
      MultipointBuilderEx builderEx = new MultipointBuilderEx(list, AttributeFlags.None);
      Multipoint multiPoint = builderEx.ToGeometry() as Multipoint;
      int ptCount = builderEx.PointCount;

      // builderEx convenience methods dont need to run on the MCT
      multiPoint = MultipointBuilderEx.CreateMultipoint(list);
      // multiPoint.HasZ, HasM, HasID will be false - the attributes are determined 
      //    based on the attribute state of the points in the list

      // or specifically set the state
      multiPoint = MultipointBuilderEx.CreateMultipoint(list, AttributeFlags.None);
      // multiPoint.HasM = false

      multiPoint = MultipointBuilderEx.CreateMultipoint(list, AttributeFlags.HasM);
      // multiPoint.HasM = true

      ptCount = multiPoint.PointCount;

      #endregion
    }

    public void MultipointBuilderEx_()
    {
      // cref: ArcGIS.Core.Geometry.MultipointBuilderEx.#ctor(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Coordinate2D},ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.MultipointBuilderEx.HasZ
      // cref: ArcGIS.Core.Geometry.MultipointBuilderEx.Zs
      // cref: ArcGIS.Core.Geometry.MultipointBuilderEx.HasM
      // cref: ArcGIS.Core.Geometry.MultipointBuilderEx.Ms
      // cref: ArcGIS.Core.Geometry.MultipointBuilderEx.HasID
      // cref: ArcGIS.Core.Geometry.MultipointBuilderEx.IDs
      // cref: ArcGIS.Core.Geometry.MultipointBuilderEx.GetPoint
      // cref: ArcGIS.Core.Geometry.MultipointBuilderEx.SetEmpty
      // cref: ArcGIS.Core.Geometry.MultipointBuilderEx.Coordinate2Ds
      // cref: ArcGIS.Core.Geometry.MultipointBuilderEx.SetPoint(System.Int32,ArcGIS.Core.Geometry.MapPoint)
      // cref: ArcGIS.Core.Geometry.MultipointBuilderEx.RemovePoints(System.Int32,System.Int32)
      #region Construct a Multipoint - using MultipointBuilderEx

      Coordinate2D[] coordinate2Ds = new Coordinate2D[] { new Coordinate2D(1, 2), new Coordinate2D(-1, -2) };
      SpatialReference sr = SpatialReferences.WGS84;

      MultipointBuilderEx builder = new MultipointBuilderEx(coordinate2Ds, sr);

      // builder.PointCount = 2

      builder.HasZ = true;
      // builder.Zs.Count = 2
      // builder.Zs[0] = 0
      // builder.Zs[1] = 0

      builder.HasM = true;
      // builder.Ms.Count = 2
      // builder.Ms[0] = NaN
      // builder.Ms[1] = NaN

      builder.HasID = true;
      // builder.IDs.Count = 2
      // builder.IDs[0] = 0
      // builder.IDs[1] = 0

      // set it empty
      builder.SetEmpty();
      // builder.Coords.Count = 0
      // builder.Zs.Count = 0
      // builder.Ms.Count = 0
      // builder.IDs.Count = 0


      // reset coordinates
      List<Coordinate2D> inCoords = new List<Coordinate2D>() { new Coordinate2D(1, 2), new Coordinate2D(3, 4), new Coordinate2D(5, 6) };
      builder.Coordinate2Ds = inCoords;
      // builder.Coords.Count = 3
      // builder.HasZ = true
      // builder.HasM = true
      // builder.HasID = true

      double[] zs = new double[] { 1, 2, 1, 2, 1, 2 };
      builder.Zs = zs;   
      // builder.Zs.Count = 6

      double[] ms = new double[] { 0, 1 };
      builder.Ms = ms;   
      // builder.Ms.Count = 2

      // coordinates are now   (x, y, z, m, id)
      //  (1, 2, 1, 0, 0), (3, 4, 2, 1, 0) (5, 6, 1, NaN, 0)

      MapPoint mapPoint = builder.GetPoint(2);
      // mapPoint.HasZ = true
      // mapPoint.HasM = true
      // mapPoint.HasID = true
      // mapPoint.Z  = 1
      // mapPoint.M = NaN
      // mapPoint.ID = 0

      // add an M to the list
      builder.Ms.Add(2);
      // builder.Ms.count = 3

      // coordinates are now   (x, y, z, m, id)
      //  (1, 2, 1, 0, 0), (3, 4, 2, 1, 0) (5, 6, 1, 2, 0)

      // now get the 2nd point again; it will now have an M value
      mapPoint = builder.GetPoint(2);
      // mapPoint.M = 2


      int[] ids = new int[] { -1, -2, -3 };
      // assign ID values
      builder.IDs = ids;

      // coordinates are now   (x, y, z, m, id)
      //  (1, 2, 1, 0, -1), (3, 4, 2, 1, -2) (5, 6, 1, 2, -3)


      // create a new point
      MapPoint point = MapPointBuilderEx.CreateMapPoint(-300, 400, 4);
      builder.SetPoint(2, point);

      // coordinates are now   (x, y, z, m, id)
      //  (1, 2, 1, 0, -1), (3, 4, 2, 1, -2) (-300, 400, 4, NaN, 0)


      builder.RemovePoints(1, 3);
      // builder.PointCount = 1

      #endregion
    }

    public void ModifyMultipoint()
    {
      Multipoint multipoint = null;
      // cref: ArcGIS.Core.Geometry.MultipointBuilderEx.#ctor(ArcGIS.Core.Geometry.Multipoint)
      // cref: ArcGIS.Core.Geometry.MultipointBuilderEx.RemovePoint(System.Int32)
      // cref: ArcGIS.Core.Geometry.MultipointBuilderEx.GetPoint(System.Int32)
      // cref: ArcGIS.Core.Geometry.MultipointBuilderEx.AddPoint(ArcGIS.Core.Geometry.MapPoint)
      // cref: ArcGIS.Core.Geometry.MultipointBuilderEx.ToGeometry
      // cref: ArcGIS.Core.Geometry.Multipoint.Copy2DCoordinatesToList
      // cref: ArcGIS.Core.Geometry.Multipoint.Copy3DCoordinatesToList
      #region Modify the points of a Multipoint

      // assume a multiPoint has been built from 4 points
      // the modified multiPoint will have the first point removed and the last point moved

      // use the builderEx constructors = don't need to run on the MCT.
      MultipointBuilderEx builderEx = new MultipointBuilderEx(multipoint);
      // remove the first point
      builderEx.RemovePoint(0);
      // modify the coordinates of the last point
      var ptEx = builderEx.GetPoint(builderEx.PointCount - 1);
      builderEx.RemovePoint(builderEx.PointCount - 1);

      var newPtEx = MapPointBuilderEx.CreateMapPoint(ptEx.X + 1.0, ptEx.Y + 2.0);
      builderEx.AddPoint(newPtEx);
      Multipoint modifiedMultiPointEx = builderEx.ToGeometry() as Multipoint;

      #endregion

      // cref: ArcGIS.Core.Geometry.MultiPoint.Copy2DCoordinatesToList
      // cref: ArcGIS.Core.Geometry.MultiPoint.Copy3DCoordinatesToList
      #region Retrieve Points, 2D Coordinates, 3D Coordinates from a multipoint

      ReadOnlyPointCollection points = multipoint.Points;
      IReadOnlyList<Coordinate2D> coords2d = multipoint.Copy2DCoordinatesToList();
      IReadOnlyList<Coordinate3D> coords3d = multipoint.Copy3DCoordinatesToList();

      #endregion
    }

    #region ProSnippet Group: Line Segment
    #endregion

    public void ConstructLineSegment()
    {
      // cref: ArcGIS.Core.Geometry.LineBuilderEx.CreateLineSegment(ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.LineBuilderEx.CreateLineSegment(ArcGIS.Core.Geometry.Coordinate2D,ArcGIS.Core.Geometry.Coordinate2D,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.LineBuilderEx.CreateLineSegment(ArcGIS.Core.Geometry.Coordinate3D,ArcGIS.Core.Geometry.Coordinate3D,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.LineBuilderEx.CreateLineSegment(ArcGIS.Core.Geometry.LineSegment,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.LineBuilderEx.#ctor(ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.LineBuilderEx.#ctor(ArcGIS.Core.Geometry.Coordinate2D,ArcGIS.Core.Geometry.Coordinate2D,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.LineBuilderEx.#ctor(ArcGIS.Core.Geometry.Coordinate3D,ArcGIS.Core.Geometry.Coordinate3D,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.LineBuilderEx.#ctor(ArcGIS.Core.Geometry.LineSegment,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.LineBuilderEx.ToSegment
      #region Construct a LineSegment using two MapPoints

      // Use a builderEx convenience method or use a builderEx constructor.

      MapPoint startPt = MapPointBuilderEx.CreateMapPoint(1.0, 1.0);
      MapPoint endPt = MapPointBuilderEx.CreateMapPoint(2.0, 1.0);

      // BuilderEx convenience methods don't need to run on the MCT.
      LineSegment lineFromMapPoint = LineBuilderEx.CreateLineSegment(startPt, endPt);

      // coordinate2D
      Coordinate2D start2d = (Coordinate2D)startPt;
      Coordinate2D end2d = (Coordinate2D)endPt;

      LineSegment lineFromCoordinate2D = LineBuilderEx.CreateLineSegment(start2d, end2d);

      // coordinate3D
      Coordinate3D start3d = (Coordinate3D)startPt;
      Coordinate3D end3d = (Coordinate3D)endPt;

      LineSegment lineFromCoordinate3D = LineBuilderEx.CreateLineSegment(start3d, end3d);

      // lineSegment
      LineSegment anotherLineFromLineSegment = LineBuilderEx.CreateLineSegment(lineFromCoordinate3D);


      // builderEx constructors don't need to run on the MCT
      LineBuilderEx lbEx = new LineBuilderEx(startPt, endPt);
      lineFromMapPoint = lbEx.ToSegment() as LineSegment;

      lbEx = new LineBuilderEx(start2d, end2d);
      lineFromCoordinate2D = lbEx.ToSegment() as LineSegment;

      lbEx = new LineBuilderEx(start3d, end3d);
      lineFromCoordinate3D = lbEx.ToSegment() as LineSegment;

      lbEx = new LineBuilderEx(startPt, endPt);
      lineFromMapPoint = lbEx.ToSegment() as LineSegment;

      lbEx = new LineBuilderEx(lineFromCoordinate3D);
      anotherLineFromLineSegment = lbEx.ToSegment() as LineSegment;

      #endregion

      LineSegment lineSegment = null;
      // cref: ArcGIS.Core.Geometry.LineBuilderEx.QueryCoords(ArcGIS.Core.Geometry.MapPoint@,ArcGIS.Core.Geometry.MapPoint@)
      // cref: ArcGIS.Core.Geometry.SegmentBuilderEx.StartPoint
      // cref: ArcGIS.Core.Geometry.SegmentBuilderEx.EndPoint
      // cref: ArcGIS.Core.Geometry.LineBuilderEx.SetCoords(ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.MapPoint)
      // cref: ArcGIS.Core.Geometry.LineBuilderEx.ToSegment
      #region Alter LineSegment Coordinates

      // builderEx constructors don't need to run on the MCT
      LineBuilderEx lbuilderEx = new LineBuilderEx(lineSegment);
      // find the existing coordinates
      lbuilderEx.QueryCoords(out startPt, out endPt);

      // or use 
      //startPt = lbuilderEx.StartPoint;
      //endPt = lbuilderEx.EndPoint;

      // update the coordinates
      lbuilderEx.SetCoords(GeometryEngine.Instance.Move(startPt, 10, 10) as MapPoint, GeometryEngine.Instance.Move(endPt, -10, -10) as MapPoint);

      // or use 
      //lbuilderEx.StartPoint = GeometryEngine.Instance.Move(startPt, 10, 10) as MapPoint;
      //lbuilderEx.EndPoint = GeometryEngine.Instance.Move(endPt, -10, -10) as MapPoint;

      LineSegment segment2 = lbuilderEx.ToSegment() as LineSegment;

      #endregion
    }

    #region ProSnippet Group: Cubic Bezier
    #endregion

    public void ConstructCubicBezier()
    {
      // cref: ArcGIS.Core.Geometry.CubicBezierBuilderEx.CreateCubicBezierSegment(ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.Coordinate2D,ArcGIS.Core.Geometry.Coordinate2D,ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.CubicBezierBuilderEx.#ctor(ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.Coordinate2D,ArcGIS.Core.Geometry.Coordinate2D,ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.CubicBezierBuilderEx.ToSegment
      // cref: ArcGIS.Core.Geometry.CubicBezierSegment
      #region Construct a Cubic Bezier - from Coordinates

      // Use a builderEx convenience method or a builderEx constructor.

      MapPoint startPt = MapPointBuilderEx.CreateMapPoint(1.0, 1.0, 3.0);
      MapPoint endPt = MapPointBuilderEx.CreateMapPoint(2.0, 2.0, 3.0);

      Coordinate2D ctrl1Pt = new Coordinate2D(1.0, 2.0);
      Coordinate2D ctrl2Pt = new Coordinate2D(2.0, 1.0);

      // BuilderEx convenience methods don't need to run on the MCT
      CubicBezierSegment bezier = CubicBezierBuilderEx.CreateCubicBezierSegment(startPt, ctrl1Pt, ctrl2Pt, endPt, SpatialReferences.WGS84);

      // without a SR
      bezier = CubicBezierBuilderEx.CreateCubicBezierSegment(startPt, ctrl1Pt, ctrl2Pt, endPt);


      // builderEx constructors dont need to run on the MCT
      CubicBezierBuilderEx cbbEx = new CubicBezierBuilderEx(startPt, ctrl1Pt, ctrl2Pt, endPt);
      bezier = cbbEx.ToSegment() as CubicBezierSegment;

      // another alternative
      cbbEx = new CubicBezierBuilderEx(startPt, ctrl1Pt.ToMapPoint(), ctrl2Pt.ToMapPoint(), endPt);
      bezier = cbbEx.ToSegment() as CubicBezierSegment;


      #endregion
    }

    public void ConstructCubicBezier2()
    {
      // cref: ArcGIS.Core.Geometry.CubicBezierBuilderEx.CreateCubicBezierSegment(ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.CubicBezierBuilderEx.#ctor(ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.CubicBezierBuilderEx.ToSegment
      #region Construct a Cubic Bezier - from MapPoints

      // Use a builderEx convenience method or a builderEx constructor.

      MapPoint startPt = MapPointBuilderEx.CreateMapPoint(1.0, 1.0, SpatialReferences.WGS84);
      MapPoint endPt = MapPointBuilderEx.CreateMapPoint(2.0, 2.0, SpatialReferences.WGS84);

      MapPoint ctrl1Pt = MapPointBuilderEx.CreateMapPoint(1.0, 2.0, SpatialReferences.WGS84);
      MapPoint ctrl2Pt = MapPointBuilderEx.CreateMapPoint(2.0, 1.0, SpatialReferences.WGS84);

      // BuilderEx convenience methods don't need to run on the MCT
      CubicBezierSegment bezier = CubicBezierBuilderEx.CreateCubicBezierSegment(startPt, ctrl1Pt, ctrl2Pt, endPt);

      // builderEx constructors dont need to run on the MCT
      CubicBezierBuilderEx cbbEx = new CubicBezierBuilderEx(startPt, ctrl1Pt, ctrl2Pt, endPt);
      bezier = cbbEx.ToSegment() as CubicBezierSegment;

      #endregion
    }

    public void ConstructCubicBezier3()
    {
      // cref: ArcGIS.Core.Geometry.CubicBezierBuilderEx.CreateCubicBezierSegment(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.MapPoint},ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.CubicBezierBuilderEx.#ctor(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.MapPoint},ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.CubicBezierBuilderEx.ToSegment
      #region Construct a Cubic Bezier - from an enumeration of MapPoints

      // Use a buildeExr convenience method or use a builderEx constructor.

      MapPoint startPt = MapPointBuilderEx.CreateMapPoint(1.0, 1.0, SpatialReferences.WGS84);
      MapPoint endPt = MapPointBuilderEx.CreateMapPoint(2.0, 2.0, SpatialReferences.WGS84);

      MapPoint ctrl1Pt = MapPointBuilderEx.CreateMapPoint(1.0, 2.0, SpatialReferences.WGS84);
      MapPoint ctrl2Pt = MapPointBuilderEx.CreateMapPoint(2.0, 1.0, SpatialReferences.WGS84);

      List<MapPoint> listMapPoints = new List<MapPoint>();
      listMapPoints.Add(startPt);
      listMapPoints.Add(ctrl1Pt);
      listMapPoints.Add(ctrl2Pt);
      listMapPoints.Add(endPt);

      // BuilderEx convenience methods don't need to run on the MCT
      CubicBezierSegment bezier = CubicBezierBuilderEx.CreateCubicBezierSegment(listMapPoints);

      // builderEx constructors dont need to run on the MCT
      CubicBezierBuilderEx cbbEx = new CubicBezierBuilderEx(listMapPoints);
      bezier = cbbEx.ToSegment() as CubicBezierSegment;

      #endregion
    }

    public void ConstructCubicBezierBuilderUtils()
    {
      CubicBezierSegment bezierSegment = null;

      // cref: ArcGIS.Core.Geometry.CubicBezierBuilderEx.#ctor(ArcGIS.Core.Geometry.CubicBezierSegment,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.SegmentBuilderEx.StartPoint
      // cref: ArcGIS.Core.Geometry.CubicBezierBuilderEx.ControlPoint1
      // cref: ArcGIS.Core.Geometry.CubicBezierBuilderEx.ControlPoint2
      // cref: ArcGIS.Core.Geometry.SegmentBuilderEx.EndPoint
      // cref: ArcGIS.Core.Geometry.CubicBezierBuilderEx.QueryCoords(ArcGIS.Core.Geometry.MapPoint@,ArcGIS.Core.Geometry.Coordinate2D@,ArcGIS.Core.Geometry.Coordinate2D@,ArcGIS.Core.Geometry.MapPoint@)
      #region Cubic Bezier Builder Properties

      // retrieve the bezier curve's control points

      CubicBezierBuilderEx cbbEx = new CubicBezierBuilderEx(bezierSegment);
      MapPoint startPtEx = cbbEx.StartPoint;
      Coordinate2D ctrlPt1Ex = cbbEx.ControlPoint1;
      Coordinate2D ctrlPt2Ex = cbbEx.ControlPoint2;
      MapPoint endPtEx = cbbEx.EndPoint;
     
      // or use the QueryCoords method
      cbbEx.QueryCoords(out startPtEx, out ctrlPt1Ex, out ctrlPt2Ex, out endPtEx);
      #endregion
    }

    public void ConstructCubicBezierUtils()
    {
      CubicBezierSegment bezierSegment = null;

      // cref: ArcGIS.Core.Geometry.Segment.StartPoint
      // cref: ArcGIS.Core.Geometry.CubicBezierSegment.ControlPoint1
      // cref: ArcGIS.Core.Geometry.CubicBezierSegment.ControlPoint2
      // cref: ArcGIS.Core.Geometry.Segment.EndPoint
      // cref: ArcGIS.Core.Geometry.CubicBezierSegment.IsCurve
      // cref: ArcGIS.Core.Geometry.CubicBezierSegment.Length
      #region Cubic Bezier Properties

      // retrieve the bezier curve's control points
      CubicBezierSegment cb = CubicBezierBuilderEx.CreateCubicBezierSegment(bezierSegment);
      MapPoint startPt = cb.StartPoint;
      Coordinate2D ctrlPt1 = cb.ControlPoint1;
      Coordinate2D ctrlPt2 = cb.ControlPoint2;
      MapPoint endPt = cb.EndPoint;

      bool isCurve = cb.IsCurve;
      double len = cb.Length;

      #endregion

      // cref: ArcGIS.Core.Geometry.PolylineBuilderEx.CreatePolyline(Segment,SpatialReference)
      #region Construct a Polyline - from a Cubic Bezier

      Polyline polyline = PolylineBuilderEx.CreatePolyline(bezierSegment);

      #endregion
    }

    #region ProSnippet Group: Arc
    #endregion

    public void ConstructArcUsingInteriorPt()
    {
      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx.CreateCircularArc(ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.Coordinate2D,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx.#ctor(ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.Coordinate2D,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx.ToSegment
      #region Construct a Circular Arc - using an interior point

      // Construct a circular arc from (2, 1) to (1, 2) with interior pt (1 + sqrt(2)/2, 1 + sqrt(2)/2).
      // Use a builderEx convenience method or use a builderEx constructor.

      MapPoint fromPt = MapPointBuilderEx.CreateMapPoint(2, 1);
      MapPoint toPt = MapPointBuilderEx.CreateMapPoint(1, 2);
      Coordinate2D interiorPt = new Coordinate2D(1 + Math.Sqrt(2) / 2, 1 + Math.Sqrt(2) / 2);

      // BuilderEx convenience methods don't need to run on the MCT.
      EllipticArcSegment circularArc = EllipticArcBuilderEx.CreateCircularArc(fromPt, toPt, interiorPt);

      // BuilderEx constructors dont need to run on the MCT.
      EllipticArcBuilderEx eab = new EllipticArcBuilderEx(fromPt, toPt, interiorPt);
      // do something with the builder

      EllipticArcSegment anotherCircularArc = eab.ToSegment();

      #endregion
    }

    public void ConstructArcUsingChordAndBearing()
    {
      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx.CreateCircularArc(ArcGIS.Core.Geometry.MapPoint,System.Double,System.Double,System.Double,ArcGIS.Core.Geometry.ArcOrientation,ArcGIS.Core.Geometry.MinorOrMajor,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx.#ctor(ArcGIS.Core.Geometry.MapPoint,System.Double,System.Double,System.Double,ArcGIS.Core.Geometry.ArcOrientation,ArcGIS.Core.Geometry.MinorOrMajor,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx.ToSegment
      #region Construct a Circular Arc - using a chord length and bearing

      // Construct a circular arc counterclockwise from (2, 1) to (1, 2) such that the embedded 
      // circle has center point at (1, 1) and radius = 1.
      // Use a builderEx convenience method or use a builderEx constructor.

      MapPoint fromPt = MapPointBuilderEx.CreateMapPoint(2, 1, SpatialReferences.WGS84);
      double chordLength = Math.Sqrt(2);
      double chordBearing = 3 * Math.PI / 4;
      double radius = 1;
      ArcOrientation orientation = ArcOrientation.ArcCounterClockwise;
      MinorOrMajor minorOrMajor = MinorOrMajor.Minor;

      // BuildeExr convenience methods don't need to run on the MCT.
      EllipticArcSegment circularArc = EllipticArcBuilderEx.CreateCircularArc(fromPt, chordLength, chordBearing, radius, orientation, minorOrMajor);

      // BuilderEx constructors need to run on the MCT.
      EllipticArcBuilderEx cab = new EllipticArcBuilderEx(fromPt, chordLength, chordBearing, radius, orientation, minorOrMajor);
      // do something with the builder

      EllipticArcSegment anotherCircularArc = cab.ToSegment();

      #endregion
    }

    public void ConstructArcUsingCenterPtAngleAndRadius()
    {
      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx.CreateCircularArc(System.Double,System.Double,ArcGIS.Core.Geometry.Coordinate2D,System.Double,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx.#ctor(System.Double,System.Double,ArcGIS.Core.Geometry.Coordinate2D,System.Double,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx.ToSegment
      #region Construct a Circular Arc - using a center point, angle and radius

      // Construct a circular arc with center point at (0, 0), from angle = 0, 
      // central angle = pi/2, radius = 1.
      // Use a builderEx convenience method or use a builderEx constructor.

      SpatialReference sr4326 = SpatialReferences.WGS84;
      Coordinate2D centerPt = new Coordinate2D(0, 0);
      double fromAngle = 0;
      double centralAngle = Math.PI / 2;
      double radius = 1;

      // BuilderEx convenience methods don't need to run on the MCT.
      EllipticArcSegment circularArc = EllipticArcBuilderEx.CreateCircularArc(fromAngle, centralAngle, centerPt, radius, sr4326);

      // BuilderEx constructors dont need to run on the MCT.
      EllipticArcBuilderEx cab = new EllipticArcBuilderEx(fromAngle, centralAngle, centerPt, radius, sr4326);
      EllipticArcSegment otherCircularArc = cab.ToSegment();

      #endregion
    }

    public void ConstructArcCenterPtRotationAngle()
    {
      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx.CreateEllipticArcSegment(ArcGIS.Core.Geometry.Coordinate2D,System.Double,System.Double,System.Double,System.Double,System.Double,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx.#ctor(ArcGIS.Core.Geometry.Coordinate2D,System.Double,System.Double,System.Double,System.Double,System.Double,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx.GetAxes(System.Double@,System.Double@)
      #region Construct an Elliptic Arc - using a center point and rotation angle

      // Construct an elliptic arc centered at (1,1), startAngle = 0, centralAngle = PI/2, 
      // rotationAngle = 0, semiMajorAxis = 1, minorMajorRatio = 0.5.
      // Use a builderEx convenience method or use a builderEx constructor.

      Coordinate2D centerPt = new Coordinate2D(1, 1);

      // BuilderEx convenience methods don't need to run on the MCT.
      EllipticArcSegment circularArc = EllipticArcBuilderEx.CreateEllipticArcSegment(centerPt, 0, Math.PI / 2, 0, 1, 0.5);

      double semiMajor;
      double semiMinor;
      circularArc.GetAxes(out semiMajor, out semiMinor);
      // semiMajor = 1, semiMinor = 0.5

      // BuilderEx constructors dont need to run on the MCT.
      EllipticArcBuilderEx cab = new EllipticArcBuilderEx(centerPt, 0, Math.PI / 2, 0, 1, 0.5);
      cab.GetAxes(out semiMajor, out semiMinor);
      EllipticArcSegment otherCircularArc = cab.ToSegment();

      #endregion
    }

    public void ConstructArcCenterPtOrientation()
    {
      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx.CreateCircularArc(ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.Coordinate2D,ArcGIS.Core.Geometry.ArcOrientation,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx.#ctor(ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.Coordinate2D,ArcGIS.Core.Geometry.ArcOrientation,ArcGIS.Core.Geometry.SpatialReference)
      #region Construct a Circular Arc - using a center point and orientation

      // Construct a circular arc from (2, 1) to (1, 2) 
      // with center point at (1, 1) and orientation counterclockwise.
      // Use a builderEx convenience method or use a builderEx constructor.

      MapPoint toPt = MapPointBuilderEx.CreateMapPoint(1, 2);
      MapPoint fromPt = MapPointBuilderEx.CreateMapPoint(2, 1);
      Coordinate2D centerPtCoord = new Coordinate2D(1, 1);

      // BuilderEx convenience methods don't need to run on the MCT.
      EllipticArcSegment circularArc = EllipticArcBuilderEx.CreateCircularArc(fromPt, toPt, centerPtCoord, ArcOrientation.ArcCounterClockwise);

      // BuilderEx constructors need to run on the MCT.
      EllipticArcBuilderEx cab = new EllipticArcBuilderEx(fromPt, toPt, centerPtCoord, ArcOrientation.ArcCounterClockwise);
      EllipticArcSegment otherCircularArc = cab.ToSegment();

      #endregion
    }

    public void ConstructArcSegmentsRadius()
    {
      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx.QueryFilletRadiusRange(ArcGIS.Core.Geometry.Segment,ArcGIS.Core.Geometry.Segment,ArcGIS.Core.Geometry.Coordinate2D)
      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx.CreateCircularArc(ArcGIS.Core.Geometry.Segment,ArcGIS.Core.Geometry.Segment,System.Double,ArcGIS.Core.Geometry.Coordinate2D,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx.#ctor(ArcGIS.Core.Geometry.Segment,ArcGIS.Core.Geometry.Segment,System.Double,ArcGIS.Core.Geometry.Coordinate2D,ArcGIS.Core.Geometry.SpatialReference)
      #region Construct a Circular Arc - using two segments and radius

      // Construct a segment from (100, 100) to (50, 50) and another segment from (100, 100) to (150, 50).
      // Use a builderEx convenience method or use a builderEx constructor.

      LineSegment segment1 = LineBuilderEx.CreateLineSegment(new Coordinate2D(100, 100), new Coordinate2D(50, 50));
      LineSegment segment2 = LineBuilderEx.CreateLineSegment(new Coordinate2D(100, 100), new Coordinate2D(150, 50));

      // Construct the hint point to determine where the arc will be constructed.
      Coordinate2D hintPoint = new Coordinate2D(100, 75);

      // Call QueryFilletRadius to get the minimum and maximum radii that can be used with these segments.
      var minMaxRadii = EllipticArcBuilderEx.QueryFilletRadiusRange(segment1, segment2, hintPoint);

      // Use the maximum radius to create the arc.
      double maxRadius = minMaxRadii.Item2;

      // BuilderEx convenience methods don't need to run on the MCT.
      //At 2.x - EllipticArcSegment circularArc = EllipticArcBuilderEx.CreateEllipticArcSegment(segment1, segment2, maxRadius, hintPoint);
      EllipticArcSegment circularArc = EllipticArcBuilderEx.CreateCircularArc(
                          segment1, segment2, maxRadius, hintPoint);

      // BuilderEx constructors need to run on the MCT.
      EllipticArcBuilderEx cab = new EllipticArcBuilderEx(segment1, segment2, maxRadius, hintPoint);
      EllipticArcSegment otherCircularArc = cab.ToSegment();

      #endregion
    }

    public void ConstructCircle()
    {
      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx.CreateCircle(ArcGIS.Core.Geometry.Coordinate2D,System.Double,ArcGIS.Core.Geometry.ArcOrientation,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.EllipticArcSegment.QueryCoords(ArcGIS.Core.Geometry.Coordinate2D@,System.Double@,System.Double@,System.Double@,System.Double@,System.Double@)
      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx.#ctor(ArcGIS.Core.Geometry.Coordinate2D,System.Double,ArcGIS.Core.Geometry.ArcOrientation,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.ArcOrientation
      #region Construct a Circle

      // Construct a circle with center at (-1,-1), radius = 2, and oriented clockwise.
      // Use a builderEx convenience method or use a builderEx constructor.

      Coordinate2D centerPtCoord = new Coordinate2D(-1, -1);

      // Builder convenience methods don't need to run on the MCT.
      EllipticArcSegment circle = EllipticArcBuilderEx.CreateCircle(centerPtCoord, 2, ArcOrientation.ArcClockwise);
      // circle.IsCircular = true
      // circle.IsCounterClockwise = false
      // circle.IsMinor = false

      double startAngle, rotationAngle, centralAngle, semiMajor, semiMinor;
      Coordinate2D actualCenterPt;
      circle.QueryCoords(out actualCenterPt, out startAngle, out centralAngle, out rotationAngle, out semiMajor, out semiMinor);

      // semiMajor = 2.0
      // semiMinor = 2.0
      // startAngle = PI/2
      // centralAngle = -2*PI
      // rotationAngle = 0
      // endAngle = PI/2

      // BuilderEx constructors need to run on the MCT.
      EllipticArcBuilderEx builder = new EllipticArcBuilderEx(centerPtCoord, 2, ArcOrientation.ArcClockwise);
      EllipticArcSegment otherCircle = builder.ToSegment();

      #endregion
    }

    public void ConstructEllipse()
    {
      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx.CreateEllipse(ArcGIS.Core.Geometry.Coordinate2D,System.Double,System.Double,System.Double,ArcGIS.Core.Geometry.ArcOrientation,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx.#ctor(ArcGIS.Core.Geometry.Coordinate2D,System.Double,System.Double,System.Double,ArcGIS.Core.Geometry.ArcOrientation,ArcGIS.Core.Geometry.SpatialReference)
      #region Construct an Ellipse

      // Construct an ellipse centered at (1, 2) with rotationAngle = -pi/6,  
      // semiMajorAxis = 5, minorMajorRatio = 0.2, oriented clockwise.
      // Use a builderEx convenience method or use a builderEx constructor.

      Coordinate2D centerPt = new Coordinate2D(1, 2);

      // BuilderEx convenience methods don't need to run on the MCT.
      EllipticArcSegment ellipse = EllipticArcBuilderEx.CreateEllipse(centerPt, -1 * Math.PI / 6, 5, 0.2, ArcOrientation.ArcClockwise);

      // BuilderEx constructors need to run on the MCT.
      EllipticArcBuilderEx builder = new EllipticArcBuilderEx(centerPt, -1 * Math.PI / 6, 5, 0.2, ArcOrientation.ArcClockwise);
      EllipticArcSegment anotherEllipse = builder.ToSegment();

      #endregion
    }

    public void ConstructArcBuilderUtils()
    {
      EllipticArcSegment arcSegment = null;

      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx.#ctor(ArcGIS.Core.Geometry.EllipticArcSegment,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx.StartPoint
      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx.EndPoint
      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx.CenterPoint
      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx.IsCircular
      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx.IsMinor
      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx.StartAngle
      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx.EndAngle
      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx.CentralAngle
      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx.RotationAngle
      // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx.Orientation
      #region Elliptic Arc Builder Properties

      // retrieve the curve's properties
      EllipticArcBuilderEx builder = new EllipticArcBuilderEx(arcSegment);
      MapPoint startPt = builder.StartPoint;
      MapPoint endPt = builder.EndPoint;
      Coordinate2D centerPt = builder.CenterPoint;
      bool isCircular = builder.IsCircular;
      bool isMinor = builder.IsMinor;
      double startAngle = builder.StartAngle;
      double endAngle = builder.EndAngle;
      double centralAngle = builder.CentralAngle;
      double rotationAngle = builder.RotationAngle;
      ArcOrientation orientation = builder.Orientation;

      #endregion
    }

    public void ConstructArcUtils()
    {
      EllipticArcSegment arcSegment = null;

      // cref: ArcGIS.Core.Geometry.EllipticArcSegment.StartAngle
      // cref: ArcGIS.Core.Geometry.EllipticArcSegment.EndAngle
      // cref: ArcGIS.Core.Geometry.EllipticArcSegment.CenterPoint
      // cref: ArcGIS.Core.Geometry.EllipticArcSegment.IsCircular
      // cref: ArcGIS.Core.Geometry.EllipticArcSegment.IsMinor
      // cref: ArcGIS.Core.Geometry.EllipticArcSegment.IsCounterClockwise
      // cref: ArcGIS.Core.Geometry.EllipticArcSegment.IsCurve
      // cref: ArcGIS.Core.Geometry.EllipticArcSegment.Length
      // cref: ArcGIS.Core.Geometry.EllipticArcSegment.MinorMajorRatio
      // cref: ArcGIS.Core.Geometry.EllipticArcSegment.GetAxes(System.Double@,System.Double@)
      // cref: ArcGIS.Core.Geometry.EllipticArcSegment.SemiMajorAxis
      // cref: ArcGIS.Core.Geometry.EllipticArcSegment.SemiMinorAxis
      // cref: ArcGIS.Core.Geometry.EllipticArcSegment.QueryCoords(ArcGIS.Core.Geometry.Coordinate2D@,System.Double@,System.Double@,System.Double@,System.Double@,System.Double@)
      // cref: ArcGIS.Core.Geometry.EllipticArcSegment.CentralAngle
      // cref: ArcGIS.Core.Geometry.EllipticArcSegment.RotationAngle
      #region Elliptic Arc Properties

      // retrieve the curve's control points
      EllipticArcSegment arc = EllipticArcBuilderEx.CreateEllipticArcSegment(arcSegment);
      MapPoint startPt = arc.StartPoint;
      MapPoint endPt = arc.EndPoint;
      Coordinate2D centerPt = arc.CenterPoint;
      bool isCircular = arc.IsCircular;
      bool isMinor = arc.IsMinor;
      bool isCounterClockwise = arc.IsCounterClockwise;
      bool isCurve = arc.IsCurve;
      double len = arc.Length;
      double ratio = arc.MinorMajorRatio;

      double semiMajorAxis, semiMinorAxis;
      // get the axes
      arc.GetAxes(out semiMajorAxis, out semiMinorAxis);
      // or use the properties
      // semiMajorAxis = arc.SemiMajorAxis;
      // semiMinorAxis = arc.SemiMinorAxis;

      double startAngle, centralAngle, rotationAngle;
      // or use QueryCoords to get complete information
      arc.QueryCoords(out centerPt, out startAngle, out centralAngle, out rotationAngle, out semiMajorAxis, out semiMinorAxis);

      // use properties to get angle information
      //double endAngle = arc.EndAngle;
      //centralAngle = arc.CentralAngle;
      //rotationAngle = arc.RotationAngle;
      //startAngle = arc.StartAngle;

      #endregion
    }

    #region ProSnippet Group: GeometryBag
    #endregion

    public void GeometryBag()
    {
      {
        // cref: ArcGIS.Core.Geometry.GeometryBagBuilderEx.#ctor(ArcGIS.Core.Geometry.SpatialReference)
        // cref: ArcGIS.Core.Geometry.GeometryBagBuilderEx.ToGeometry
        // cref: ArcGIS.Core.Geometry.GeometryBag.IsEmpty
        // cref: ArcGIS.Core.Geometry.GeometryBagBuilderEx.AddGeometry(ArcGIS.Core.Geometry.Geometry)
        // cref: ArcGIS.Core.Geometry.GeometryBagBuilderEx.GeometryCount
        // cref: ArcGIS.Core.Geometry.GeometryBag.PartCount
        // cref: ArcGIS.Core.Geometry.GeometryBag.PointCount
        // cref: ArcGIS.Core.Geometry.GeometryBag.Geometries
        // cref: ArcGIS.Core.Geometry.GeometryBag.IsEqual(ArcGIS.Core.Geometry.GeometryBag)
        // cref: ArcGIS.Core.Geometry.GeometryBagBuilderEx.RemoveGeometry(System.Int32)
        // cref: ArcGIS.Core.Geometry.GeometryBagBuilderEx.InsertGeometry(System.Int32,ArcGIS.Core.Geometry.Geometry)
        #region Construct GeometryBag

        MapPoint point = MapPointBuilderEx.CreateMapPoint(1, 2, SpatialReferences.WebMercator);

        List<Coordinate2D> coords2D = new List<Coordinate2D>()
        {
          new Coordinate2D(0, 0),
          new Coordinate2D(0, 1),
          new Coordinate2D(1, 1),
          new Coordinate2D(1, 0)
        };

        Multipoint multipoint = MultipointBuilderEx.CreateMultipoint(coords2D, SpatialReferences.WGS84);
        Polyline polyline = PolylineBuilderEx.CreatePolyline(coords2D, SpatialReferences.WebMercator);

        GeometryBagBuilderEx builder = new GeometryBagBuilderEx(SpatialReferences.WGS84);

        GeometryBag emptyBag = builder.ToGeometry();
        // emptyBag.IsEmpty = true

        builder.AddGeometry(point);
        // builder.GeometryCount = 1

        GeometryBag geometryBag = builder.ToGeometry();
        // geometryBag.PartCount = 1
        // geometryBag.PointCount = 1
        // geometryBag.IsEmpty = false

        IReadOnlyList<Geometry> geometries = geometryBag.Geometries;
        // geometries.Count = 1
        // geometries[0] is MapPoint with a sr of WGS84

        bool isEqual = geometryBag.IsEqual(emptyBag);   // isEqual = false

        builder.InsertGeometry(0, multipoint);
        geometryBag = builder.ToGeometry();
        // geometryBag.PartCount = 2

        geometries = geometryBag.Geometries;
        // geometries.Count = 2
        // geometries[0] is Multipoint
        // geometries[1] is MapPoint

        builder.AddGeometry(polyline);
        builder.RemoveGeometry(1);
        geometryBag = builder.ToGeometry();
        // geometryBag.PartCount = 2

        geometries = geometryBag.Geometries;
        // geometries.Count = 2
        // geometries[0] is Multipoint
        // geometries[1] is Polyline          

        #endregion
      }

      {
        // cref: ArcGIS.Core.Geometry.GeometryBagBuilderEx.CreateGeometryBag(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Geometry},ArcGIS.Core.Geometry.SpatialReference)
        // cref: ArcGIS.Core.Geometry.GeometryBagBuilderEx.#ctor(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Geometry},ArcGIS.Core.Geometry.SpatialReference)
        // cref: ArcGIS.Core.Geometry.GeometryBagBuilderEx.ToGeometry
        #region Construct GeometryBag - from an enumeration of geometries

        // Use a builder convenience method or use a builder constructor.

        MapPoint point = MapPointBuilderEx.CreateMapPoint(10, 20);
        List<Coordinate2D> coords = new List<Coordinate2D>() { new Coordinate2D(50, 60), new Coordinate2D(-120, -70), new Coordinate2D(40, 60) };
        Multipoint multipoint = MultipointBuilderEx.CreateMultipoint(coords, SpatialReferences.WebMercator);
        Polyline polyline = PolylineBuilderEx.CreatePolyline(coords);

        string json = "{\"rings\":[[[0,0],[0,1],[1,1],[1,0],[0,0]],[[3,0],[3,1],[4,1],[4,0],[3,0]]]}";
        Polygon polygon = PolygonBuilderEx.FromJson(json);

        var geometries = new List<Geometry>() { point, multipoint, polyline, polygon };

        // Builder convenience methods don't need to run on the MCT.
        //At 2.x - GeometryBag bag = GeometryBagBuilder.CreateGeometryBag(geometries, SpatialReferences.WGS84);
        var bag = GeometryBagBuilderEx.CreateGeometryBag(geometries, SpatialReferences.WGS84);

        //At 2.x - using (var builder = new GeometryBagBuilder(geometries, SpatialReferences.WGS84)) 
        var builder = new GeometryBagBuilderEx(geometries, SpatialReferences.WGS84);
        // do something with the builder
        bag = builder.ToGeometry();

        #endregion
      }

      {
        // cref: ArcGIS.Core.Geometry.GeometryBagBuilderEx.FromJson(System.String)
        // cref: ArcGIS.Core.Geometry.GeometryBagBuilderEx.FromXML(System.String)
        // cref: ArcGIS.Core.Geometry.Geometry.ToXml
        #region Construct GeometryBag - from JSON, Xml

        const string jsonString = "{\"geometries\":[{\"x\":1,\"y\":2},{\"rings\":[[[0,0],[0,4],[3,4],[3,0],[0,0]]]}],\"spatialReference\":{\"wkid\":4326,\"latestWkid\":4326}}";
        //At 2.x - GeometryBag geometryBag = GeometryBagBuilder.FromJson(jsonString);
        var geometryBag = GeometryBagBuilderEx.FromJson(jsonString);

        string xml = geometryBag.ToXml();
        //At 2.x - GeometryBag xmlString = GeometryBagBuilder.FromXML(xml);
        var xmlString = GeometryBagBuilderEx.FromXml(xml);

        #endregion
      }
    }

    public void GeometryBagInsert()
    {
      // cref: ArcGIS.Core.Geometry.GeometryBagBuilderEx.AddGeometries(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Geometry})
      // cref: ArcGIS.Core.Geometry.GeometryBagBuilderEx.InsertGeometries(System.Int32,System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Geometry})
      // cref: ArcGIS.Core.Geometry.GeometryBagBuilderEx.GeometryCount
      // cref: ArcGIS.Core.Geometry.GeometryBagBuilderEx.ToGeometry
      #region Construct GeometryBag - adding or inserting an enumeration of geometries

      MapPoint point = MapPointBuilderEx.CreateMapPoint(10, 20);
      List<Coordinate2D> coords = new List<Coordinate2D>() { new Coordinate2D(50, 60), new Coordinate2D(-120, -70), new Coordinate2D(40, 60) };
      Multipoint multipoint = MultipointBuilderEx.CreateMultipoint(coords, SpatialReferences.WebMercator);
      Polyline polyline = PolylineBuilderEx.CreatePolyline(coords);

      string json = "{\"rings\":[[[0,0],[0,1],[1,1],[1,0],[0,0]],[[3,0],[3,1],[4,1],[4,0],[3,0]]]}";
      Polygon polygon = PolygonBuilderEx.FromJson(json);

      var geometries = new List<Geometry>() { point, multipoint, polyline, polygon };

      //At 2.x - using (var builder = new GeometryBagBuilder(SpatialReferences.WGS84))
      var builder = new GeometryBagBuilderEx(SpatialReferences.WGS84);
      builder.AddGeometries(geometries);

      GeometryBag geomBag = builder.ToGeometry();
      // geomBag.PartCount == 4    (point, multipoint, polyline, polygon)

      geometries = new List<Geometry>() { point, polyline };
      builder.InsertGeometries(1, geometries);
      // builder.GeometryCount == 6
      geomBag = builder.ToGeometry();
      // geomBag.PartCount == 6    (point, point, polyline, multipoint, polyline, polygon)

      #endregion
    }

    #region ProSnippet Group: Multipatch
    #endregion

    public void Multipatch()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.ConstructMultipatchExtrude(ArcGIS.Core.Geometry.Multipart,System.Double)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.ConstructMultipatchExtrude(ArcGIS.Core.Geometry.Multipart,System.Double)
      // cref: ArcGIS.Core.Geometry.GeometryEngine.ConstructMultipatchExtrudeFromToZ(ArcGIS.Core.Geometry.Multipart,System.Double,System.Double)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.ConstructMultipatchExtrudeFromToZ(ArcGIS.Core.Geometry.Multipart,System.Double,System.Double)
      // cref: ArcGIS.Core.Geometry.GeometryEngine.ConstructMultipatchExtrudeAlongVector3D(ArcGIS.Core.Geometry.Multipart,ArcGIS.Core.Geometry.Coordinate3D)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.ConstructMultipatchExtrudeAlongVector3D(ArcGIS.Core.Geometry.Multipart,ArcGIS.Core.Geometry.Coordinate3D)
      // cref: ArcGIS.Core.Geometry.GeometryEngine.ConstructMultipatchExtrudeToZ(ArcGIS.Core.Geometry.Multipart,System.Double)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.ConstructMultipatchExtrudeToZ(ArcGIS.Core.Geometry.Multipart,System.Double)
      // cref: ArcGIS.Core.Geometry.GeometryEngine.ConstructMultipatchExtrudeAlongLine(ArcGIS.Core.Geometry.Multipart,ArcGIS.Core.Geometry.Coordinate3D,ArcGIS.Core.Geometry.Coordinate3D)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.ConstructMultipatchExtrudeAlongLine(ArcGIS.Core.Geometry.Multipart,ArcGIS.Core.Geometry.Coordinate3D,ArcGIS.Core.Geometry.Coordinate3D)
      #region Construct Multipatch via Extrusion of Polygon or Polyline

      // build a polygon
      string json = "{\"hasZ\":true,\"rings\":[[[0,0,0],[0,1,0],[1,1,0],[1,0,0],[0,0,0]]],\"spatialReference\":{\"wkid\":4326}}";
      Polygon polygon = PolygonBuilderEx.FromJson(json);

      // extrude the polygon by an offset to create a multipatch
      Multipatch multipatch = GeometryEngine.Instance.ConstructMultipatchExtrude(polygon, 2);

      // a different polygon
      json = "{\"hasZ\":true,\"rings\":[[[0,0,1],[0,1,2],[1,1,3],[1,0,4],[0,0,1]]],\"spatialReference\":{\"wkid\":4326}}";
      polygon = PolygonBuilderEx.FromJson(json);

      // extrude between z values to create a multipatch
      multipatch = GeometryEngine.Instance.ConstructMultipatchExtrudeFromToZ(polygon, -10, 20);

      // extrude along the axis defined by the coordinate to create a multipatch
      Coordinate3D coord = new Coordinate3D(10, 18, -10);
      multipatch = GeometryEngine.Instance.ConstructMultipatchExtrudeAlongVector3D(polygon, coord);

      // build a polyline
      json = "{\"hasZ\":true,\"paths\":[[[400,800,1000],[800,1400,1500],[1200,800,2000],[1800,1800,2500],[2200,800,3000]]],\"spatialReference\":{\"wkid\":3857}}";
      Polyline polyline = PolylineBuilderEx.FromJson(json);

      // extrude to a specific z value to create a multipatch
      multipatch = GeometryEngine.Instance.ConstructMultipatchExtrudeToZ(polyline, 500);

      Coordinate3D fromCoord = new Coordinate3D(50, 50, -500);
      Coordinate3D toCoord = new Coordinate3D(200, 50, 1000);

      // extrude between two coordinates to create a multipatch
      multipatch = GeometryEngine.Instance.ConstructMultipatchExtrudeAlongLine(polyline, fromCoord, toCoord);

      #endregion

      Multipatch multiPatch = null;
      int patchIndex = 0;

      // cref: ArcGIS.Core.Geometry.Multipatch.IsEmpty
      // cref: ArcGIS.Core.Geometry.Multipatch.PartCount
      // cref: ArcGIS.Core.Geometry.Multipatch.PointCount
      // cref: ArcGIS.Core.Geometry.Multipatch.Points
      // cref: ArcGIS.Core.Geometry.Multipatch.HasMaterials
      // cref: ArcGIS.Core.Geometry.Multipatch.MaterialCount
      // cref: ArcGIS.Core.Geometry.Multipatch.HasTextures
      // cref: ArcGIS.Core.Geometry.Multipatch.TextureVertexCount
      // cref: ArcGIS.Core.Geometry.Multipatch.GetMaterialColor
      // cref: ArcGIS.Core.Geometry.Multipatch.GetMaterialEdgeColor
      // cref: ArcGIS.Core.Geometry.Multipatch.GetMaterialEdgeWidth
      // cref: ArcGIS.Core.Geometry.Multipatch.GetMaterialShininess
      // cref: ArcGIS.Core.Geometry.Multipatch.GetMaterialTransparencyPercent
      // cref: ArcGIS.Core.Geometry.Multipatch.IsMaterialCullBackFace
      // cref: ArcGIS.Core.Geometry.Multipatch.GetMaterialTextureColumnCount
      // cref: ArcGIS.Core.Geometry.Multipatch.GetMaterialTextureRowCount
      // cref: ArcGIS.Core.Geometry.Multipatch.GetMaterialTextureBytesPerPixel
      // cref: ArcGIS.Core.Geometry.Multipatch.GetMaterialTextureCompressionType
      // cref: ArcGIS.Core.Geometry.Multipatch.GetMaterialTexture
      // cref: ArcGIS.Core.Geometry.TextureCompressionType
      // cref: ArcGIS.Core.Geometry.Multipatch.GetPatchPriority(System.Int32)
      // cref: ArcGIS.Core.Geometry.Multipatch.GetPatchType(System.Int32)
      // cref: ArcGIS.Core.Geometry.Multipatch.GetPatchPointCount(System.Int32)
      // cref: ArcGIS.Core.Geometry.Multipatch.GetPatchStartPointIndex(System.Int32)
      // cref: ArcGIS.Core.Geometry.Multipatch.GetPatchTextureVertexCount(System.Int32)
      // cref: ArcGIS.Core.Geometry.Multipatch.GetPatchTextureCoordinate(System.Int32,System.Int32)
      // cref: ArcGIS.Core.Geometry.Multipatch.GetPatchTextureCoordinates(System.Int32,System.Collections.Generic.ICollection{ArcGIS.Core.Geometry.Coordinate2D}@)
      // cref: ArcGIS.Core.Geometry.Multipatch.Copy3DCoordinatesToList
      #region Multipatch Properties

      // standard geometry properties
      bool hasZ = multipatch.HasZ;
      bool hasM = multipatch.HasM;
      bool hasID = multipatch.HasID;
      bool isEmpty = multipatch.IsEmpty;
      var sr = multipatch.SpatialReference;

      // number of patches (parts)
      int patchCount = multiPatch.PartCount;
      // number of points
      int pointCount = multiPatch.PointCount;

      // retrieve the points as MapPoints
      ReadOnlyPointCollection points = multipatch.Points;
      // or as 3D Coordinates
      IReadOnlyList<Coordinate3D> coordinates = multipatch.Copy3DCoordinatesToList();


      // multipatch materials
      bool hasMaterials = multiPatch.HasMaterials;
      int materialCount = multiPatch.MaterialCount;


      // multipatch textures
      bool hasTextures = multiPatch.HasTextures;
      int textureVertexCount = multiPatch.TextureVertexCount;

      // normals
      bool hasNormals = multiPatch.HasNormals;


      // properties for an individual patch (if multipatch.PartCount > 0)
      int patchPriority = multiPatch.GetPatchPriority(patchIndex);
      PatchType patchType = multiPatch.GetPatchType(patchIndex);

      // patch points
      int patchPointCount = multiPatch.GetPatchPointCount(patchIndex);
      int pointStartIndex = multiPatch.GetPatchStartPointIndex(patchIndex);
      // the patch Points are then the points in multipatch.Points from pointStartIndex to pointStartIndex + patchPointCount 

      // if the multipatch has materials 
      if (hasMaterials)
      {
        // does the patch have a material?  
        //   materialIndex = -1 if the patch does not have a material; 
        //   0 <= materialIndex < materialCount if the patch does have materials
        int materialIndex = multipatch.GetPatchMaterialIndex(patchIndex);


        // properties for an individual material (if multipatch.MaterialCount > 0)
        var color = multipatch.GetMaterialColor(materialIndex);
        var edgeColor = multipatch.GetMaterialEdgeColor(materialIndex);
        var edgeWidth = multipatch.GetMaterialEdgeWidth(materialIndex);
        var shiness = multipatch.GetMaterialShininess(materialIndex);
        var percent = multipatch.GetMaterialTransparencyPercent(materialIndex);
        var cullBackFace = multipatch.IsMaterialCullBackFace(materialIndex);

        // texture properties
        bool isTextured = multipatch.IsMaterialTextured(materialIndex);
        if (isTextured)
        {
          int columnCount = multipatch.GetMaterialTextureColumnCount(materialIndex);
          int rowCount = multipatch.GetMaterialTextureRowCount(materialIndex);
          int bpp = multipatch.GetMaterialTextureBytesPerPixel(materialIndex);
          TextureCompressionType compressionType = multipatch.GetMaterialTextureCompressionType(materialIndex);
          var texture = multipatch.GetMaterialTexture(materialIndex);
        }
      }

      // texture coordinates (if multipatch.HasTextures = true)
      if (hasTextures)
      {
        int numPatchTexturePoints = multiPatch.GetPatchTextureVertexCount(patchIndex);
        var coordinate2D = multiPatch.GetPatchTextureCoordinate(patchIndex, 0);

        ICollection<Coordinate2D> textureCoordinates = new List<Coordinate2D>(numPatchTexturePoints);
        multiPatch.GetPatchTextureCoordinates(patchIndex, ref textureCoordinates);
      }


      // patch normals (if multipatch.HasNormals = true)
      if (hasNormals)
      {
        //  number of normal coordinates = multipatch.GetPatchPointCount(patchIndex)
        Coordinate3D patchNormal = multipatch.GetPatchNormal(patchIndex, 0);
        ICollection<Coordinate3D> normalCoordinates = new List<Coordinate3D>(patchPointCount);
        multipatch.GetPatchNormals(patchIndex, ref normalCoordinates);
      }


      #endregion

      // cref: ArcGIS.Core.Geometry.Multipatch.ToBinaryXml
      // cref: ArcGIS.Core.Geometry.MultipatchBuilderEx.FromBinaryXml(System.String)
      // cref: ArcGIS.Core.Geometry.MultipatchBuilderEx.FromXml(System.String)
      // cref: ArcGIS.Core.Geometry.MultipatchBuilderEx.FromEsriShape(System.Byte[],ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.Multipatch.ToEsriShape
      #region Construct Multipatch

      // export to binary xml
      string binaryXml = multiPatch.ToBinaryXml();

      // import from binaryXML - methods need to run on the MCT
      Multipatch binaryMultipatch = MultipatchBuilderEx.FromBinaryXml(binaryXml);

      // xml export / import
      string xml = multiPatch.ToXml();
      Multipatch xmlMultipatch = MultipatchBuilderEx.FromXml(xml);

      // esriShape export/import
      byte[] buffer = multiPatch.ToEsriShape();
      Multipatch esriPatch = MultipatchBuilderEx.FromEsriShape(buffer);

      // or use GeometryEngine
      Multipatch patchImport = GeometryEngine.Instance.ImportFromEsriShape(EsriShapeImportFlags.EsriShapeImportDefaults, buffer, multiPatch.SpatialReference) as Multipatch;

      #endregion


      // cref: ArcGIS.Core.Geometry.BasicMaterial.#ctor
      // cref: ArcGIS.Core.Geometry.BasicMaterial.Color
      // cref: ArcGIS.Core.Geometry.BasicMaterial.TransparencyPercent
      // cref: ArcGIS.Core.Geometry.BasicMaterial.#ctor(ArcGIS.Core.Geometry.BasicMaterial)
      // cref: ArcGIS.Core.Geometry.MultipatchBuilderEx.#ctor(ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.MultipatchBuilderEx.MakePatch(ArcGIS.Core.Geometry.PatchType)
      // cref: ArcGIS.Core.Geometry.MultipatchBuilderEx.QueryPatchIndicesWithMaterial(ArcGIS.Core.Geometry.Material)
      // cref: ArcGIS.Core.Geometry.MultipatchBuilderEx.ToGeometry
      // cref: ArcGIS.Core.Geometry.Patch
      // cref: ArcGIS.Core.Geometry.PatchType
      #region Construct Multipatch via MultipatchBuilderEx

      var coords_face1 = new List<Coordinate3D>()
      {
        new Coordinate3D(12.495461061000071,41.902603910000039,62.552700000000186),
        new Coordinate3D(12.495461061000071,41.902603910000039,59.504700000004959),
        new Coordinate3D(12.495461061000071,41.902576344000067,59.504700000004959),
        new Coordinate3D(12.495461061000071,41.902603910000039,62.552700000000186),
        new Coordinate3D(12.495461061000071,41.902576344000067,59.504700000004959),
        new Coordinate3D(12.495461061000071,41.902576344000067,62.552700000000186),
      };

      var coords_face2 = new List<Coordinate3D>()
      {
        new Coordinate3D(12.495461061000071, 41.902576344000067, 62.552700000000186),
        new Coordinate3D(12.495461061000071, 41.902576344000067, 59.504700000004959),
        new Coordinate3D(12.495488442000067, 41.902576344000067, 59.504700000004959),
        new Coordinate3D(12.495461061000071, 41.902576344000067, 62.552700000000186),
        new Coordinate3D(12.495488442000067, 41.902576344000067, 59.504700000004959),
        new Coordinate3D(12.495488442000067, 41.902576344000067, 62.552700000000186),
      };

      var coords_face3 = new List<Coordinate3D>()
      {
        new Coordinate3D(12.495488442000067, 41.902576344000067, 62.552700000000186),
        new Coordinate3D(12.495488442000067, 41.902576344000067, 59.504700000004959),
        new Coordinate3D(12.495488442000067, 41.902603910000039, 59.504700000004959),
        new Coordinate3D(12.495488442000067, 41.902576344000067, 62.552700000000186),
        new Coordinate3D(12.495488442000067, 41.902603910000039, 59.504700000004959),
        new Coordinate3D(12.495488442000067, 41.902603910000039, 62.552700000000186),
      };

      var coords_face4 = new List<Coordinate3D>()
      {
        new Coordinate3D(12.495488442000067, 41.902576344000067, 59.504700000004959),
        new Coordinate3D(12.495461061000071, 41.902576344000067, 59.504700000004959),
        new Coordinate3D(12.495461061000071, 41.902603910000039, 59.504700000004959),
        new Coordinate3D(12.495488442000067, 41.902576344000067, 59.504700000004959),
        new Coordinate3D(12.495461061000071, 41.902603910000039, 59.504700000004959),
        new Coordinate3D(12.495488442000067, 41.902603910000039, 59.504700000004959),
      };

      var coords_face5 = new List<Coordinate3D>()
      {
        new Coordinate3D(12.495488442000067, 41.902603910000039, 59.504700000004959),
        new Coordinate3D(12.495461061000071, 41.902603910000039, 59.504700000004959),
        new Coordinate3D(12.495461061000071, 41.902603910000039, 62.552700000000186),
        new Coordinate3D(12.495488442000067, 41.902603910000039, 59.504700000004959),
        new Coordinate3D(12.495461061000071, 41.902603910000039, 62.552700000000186),
        new Coordinate3D(12.495488442000067, 41.902603910000039, 62.552700000000186),
      };

      var coords_face6 = new List<Coordinate3D>()
      {
        new Coordinate3D(12.495488442000067, 41.902603910000039, 62.552700000000186),
        new Coordinate3D(12.495461061000071, 41.902603910000039, 62.552700000000186),
        new Coordinate3D(12.495461061000071, 41.902576344000067, 62.552700000000186),
        new Coordinate3D(12.495488442000067, 41.902603910000039, 62.552700000000186),
        new Coordinate3D(12.495461061000071, 41.902576344000067, 62.552700000000186),
        new Coordinate3D(12.495488442000067, 41.902576344000067, 62.552700000000186),
      };

      // materials
      var materialRed = new BasicMaterial();
      materialRed.Color = System.Windows.Media.Colors.Red;

      var materialTransparent = new BasicMaterial();
      materialTransparent.Color = System.Windows.Media.Colors.White;
      materialTransparent.TransparencyPercent = 80;

      var blueTransparent = new BasicMaterial(materialTransparent);
      blueTransparent.Color = System.Windows.Media.Colors.SkyBlue;

      // create a list of patch objects
      var patches = new List<Patch>();

      // create the multipatchBuilderEx object
      var mpb = new ArcGIS.Core.Geometry.MultipatchBuilderEx();

      // make each patch using the appropriate coordinates and add to the patch list
      var patch = mpb.MakePatch(PatchType.Triangles);
      patch.Coords = coords_face1;
      patches.Add(patch);

      patch = mpb.MakePatch(PatchType.Triangles);
      patch.Coords = coords_face2;
      patches.Add(patch);

      patch = mpb.MakePatch(PatchType.Triangles);
      patch.Coords = coords_face3;
      patches.Add(patch);

      patch = mpb.MakePatch(PatchType.Triangles);
      patch.Coords = coords_face4;
      patches.Add(patch);

      patch = mpb.MakePatch(PatchType.Triangles);
      patch.Coords = coords_face5;
      patches.Add(patch);

      patch = mpb.MakePatch(PatchType.Triangles);
      patch.Coords = coords_face6;
      patches.Add(patch);

      patches[0].Material = materialRed;
      patches[1].Material = materialTransparent;
      patches[2].Material = materialRed;
      patches[3].Material = materialRed;
      patches[4].Material = materialRed;
      patches[5].Material = blueTransparent;

      // assign the patches to the multipatchBuilder
      mpb.Patches = patches;

      // check which patches currently contain the material
      var red = mpb.QueryPatchIndicesWithMaterial(materialRed);
      //   red should be [0, 2, 3, 4]


      // call ToGeometry to get the multipatch
      multipatch = mpb.ToGeometry() as Multipatch;

      #endregion
    }

    public void MultipatchBuilderEx_()
    {
      Multipatch multipatch = null;
      TextureResource brickTextureResource = null;
      BasicMaterial brickMaterialTexture = new BasicMaterial();
      MapPoint newPoint = null;
      var coords = new List<Coordinate3D>();

      // cref: ArcGIS.Core.Geometry.MultipatchBuilderEx.#ctor(ArcGIS.Core.Geometry.Multipatch)
      // cref: ArcGIS.Core.Geometry.MultipatchBuilderEx.HasM
      // cref: ArcGIS.Core.Geometry.MultipatchBuilderEx.HasZ
      // cref: ArcGIS.Core.Geometry.MultipatchBuilderEx.HasID
      // cref: ArcGIS.Core.Geometry.MultipatchBuilderEx.IsEmpty
      // cref: ArcGIS.Core.Geometry.MultipatchBuilderEx.HasNormals
      // cref: ArcGIS.Core.Geometry.MultipatchBuilderEx.Patches
      // cref: ArcGIS.Core.Geometry.MultipatchBuilderEx.ToGeometry
      // cref: ArcGIS.Core.Geometry.MultipatchBuilderEx.GetPatchPointCount(System.Int32)
      // cref: ArcGIS.Core.Geometry.MultipatchBuilderEx.GetPoint(System.Int32,System.Int32)
      // cref: ArcGIS.Core.Geometry.MultipatchBuilderEx.SetPoint(System.Int32,System.Int32,ArcGIS.Core.Geometry.MapPoint)
      // cref: ArcGIS.Core.Geometry.MultipatchBuilderEx.QueryPatchIndicesWithTexture(ArcGIS.Core.Geometry.TextureResource)
      // cref: ArcGIS.Core.Geometry.MultipatchBuilderEx.SynchronizeAttributeAwareness
      // cref: ArcGIS.Core.Geometry.Patch.Material
      #region Construct Multipatch from another Multipatch

      // create the multipatchBuilderEx object
      var builder = new ArcGIS.Core.Geometry.MultipatchBuilderEx(multipatch);

      // check some properties
      bool hasM = builder.HasM;
      bool hasZ = builder.HasZ;
      bool hasID = builder.HasID;
      bool isEmpty = builder.IsEmpty;
      bool hasNormals = builder.HasNormals;

      var patches = builder.Patches;
      int patchCount = patches.Count;

      // if there's some patches
      if (patchCount > 0)
      {
        int pointCount = builder.GetPatchPointCount(0);

        // replace the first point in the first patch
        if (pointCount > 0)
        {
          // get the first point
          var pt = builder.GetPoint(0, 0);
          builder.SetPoint(0, 0, newPoint);
        }

        // check which patches currently contain the texture
        var texture = builder.QueryPatchIndicesWithTexture(brickTextureResource);

        // assign a texture material
        patches[0].Material = brickMaterialTexture;
      }

      // update the builder for M awareness
      builder.HasM = true;
      // synchronize the patch attributes to match the builder attributes
      //   in this instance because we just set HasM to true on the builder, each patch will now get a default M value for it's set of coordinates
      builder.SynchronizeAttributeAwareness();

      // call ToGeometry to get the multipatch
      multipatch = builder.ToGeometry() as Multipatch;

      // multipatch.HasM will be true

      #endregion

      // cref: ArcGIS.Core.Geometry.MultipatchBuilderEx.From3DModelFile(System.String,ArcGIS.Core.Geometry.SpatialReference)
      #region Construct Multipatch from a 3D model file

      try
      {
        var model = ArcGIS.Core.Geometry.MultipatchBuilderEx.From3DModelFile(@"c:\Temp\My3dModelFile.dae");
        bool modelIsEmpty = model.IsEmpty;
      }
      catch (FileNotFoundException)
      {
        // file not found
      }
      catch (ArgumentException)
      {
        // File extension is unsupported or cannot read the file
      }
      #endregion

      // cref: ArcGIS.Core.Geometry.MultipatchBuilderEx.#ctor(ArcGIS.Core.Geometry.MultipatchConstructType,ArcGIS.Core.Geometry.MapPoint,System.Double,System.Double,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.MultipatchBuilderEx.CreateMultipatch(ArcGIS.Core.Geometry.MultipatchConstructType,ArcGIS.Core.Geometry.MapPoint,System.Double,System.Double,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.MultipatchConstructType
      // cref: ArcGIS.Core.Geometry.BasicMaterial
      // cref: ArcGIS.Core.Geometry.BasicMaterial.Color
      // cref: ArcGIS.Core.Geometry.BasicMaterial.Shininess
      // cref: ArcGIS.Core.Geometry.BasicMaterial.TransparencyPercent
      // cref: ArcGIS.Core.Geometry.BasicMaterial.EdgeWidth
      // cref: ArcGIS.Core.Geometry.Patch.Material
      #region Construct 3D special Multipatch shapes

      var sr = MapView.Active.Map.SpatialReference;

      var extent = MapView.Active.Extent;
      var center = extent.Center;
      var centerZ = MapPointBuilderEx.CreateMapPoint(center.X, center.Y, 500, sr);

      // cube
      multipatch = ArcGIS.Core.Geometry.MultipatchBuilderEx.CreateMultipatch(MultipatchConstructType.Cube, centerZ, 200, sr);
      // tetrahedron
      multipatch = ArcGIS.Core.Geometry.MultipatchBuilderEx.CreateMultipatch(MultipatchConstructType.Tetrahedron, centerZ, 200, sr);
      // diamond
      multipatch = ArcGIS.Core.Geometry.MultipatchBuilderEx.CreateMultipatch(MultipatchConstructType.Diamond, centerZ, 200, sr);
      // hexagon
      multipatch = ArcGIS.Core.Geometry.MultipatchBuilderEx.CreateMultipatch(MultipatchConstructType.Hexagon, centerZ, 200, sr);

      // sphere frame
      multipatch = ArcGIS.Core.Geometry.MultipatchBuilderEx.CreateMultipatch(MultipatchConstructType.SphereFrame, centerZ, 200, 0.8, sr);
      // sphere
      multipatch = ArcGIS.Core.Geometry.MultipatchBuilderEx.CreateMultipatch(MultipatchConstructType.Sphere, centerZ, 200, 0.8, sr);
      // cylinder
      multipatch = ArcGIS.Core.Geometry.MultipatchBuilderEx.CreateMultipatch(MultipatchConstructType.Cylinder, centerZ, 200, 0.8, sr);
      // cone
      multipatch = ArcGIS.Core.Geometry.MultipatchBuilderEx.CreateMultipatch(MultipatchConstructType.Cone, centerZ, 200, 0.8, sr);


      // use the builder to add materials or textures
      //   - create a cone with a material
      builder = new MultipatchBuilderEx(MultipatchConstructType.Cone, centerZ, 200, 0.8, sr);

      BasicMaterial faceMaterial = new BasicMaterial();
      faceMaterial.Color = System.Windows.Media.Color.FromRgb(255, 0, 0);
      faceMaterial.Shininess = 150;
      faceMaterial.TransparencyPercent = 50;
      faceMaterial.EdgeWidth = 20;

      foreach (var patch in builder.Patches)
        patch.Material = faceMaterial;

      multipatch = builder.ToGeometry() as Multipatch;
      #endregion

    }

    public void Material()
    {
      {
        // cref: ArcGIS.Core.Geometry.BasicMaterial.#ctor
        // cref: ArcGIS.Core.Geometry.BasicMaterial.#ctor(ArcGIS.Core.Geometry.BasicMaterial)
        // cref: ArcGIS.Core.Geometry.BasicMaterial.Color
        // cref: ArcGIS.Core.Geometry.BasicMaterial.EdgeColor
        // cref: ArcGIS.Core.Geometry.BasicMaterial.EdgeWidth
        // cref: ArcGIS.Core.Geometry.BasicMaterial.TransparencyPercent
        // cref: ArcGIS.Core.Geometry.BasicMaterial.Shininess
        // cref: ArcGIS.Core.Geometry.BasicMaterial.IsCullBackFace
        #region Create BasicMaterial

        // Create BasicMaterial with default values
        BasicMaterial material = new BasicMaterial();
        System.Windows.Media.Color color = material.Color;         // color = Colors.Black
        System.Windows.Media.Color edgeColor = material.EdgeColor; // edgeColor = Colors.Black
        int edgeWidth = material.EdgeWidth;                        // edgeWidth = 0
        int transparency = material.TransparencyPercent;           // transparency = 0
        int shininess = material.Shininess;                        // shininess = 0
        bool cullBackFace = material.IsCullBackFace;               // cullBackFace = false

        // Modify the properties
        material.Color = System.Windows.Media.Colors.Red;
        material.EdgeColor = System.Windows.Media.Colors.Blue;
        material.EdgeWidth = 10;
        material.TransparencyPercent = 50;
        material.Shininess = 25;
        material.IsCullBackFace = true;
        #endregion
      }

      {
        // cref: ArcGIS.Core.Geometry.JPEGTexture.#ctor(System.Byte[])
        // cref: ArcGIS.Core.Geometry.JPEGTexture.BytesPerPixel
        // cref: ArcGIS.Core.Geometry.JPEGTexture.ColumnCount
        // cref: ArcGIS.Core.Geometry.JPEGTexture.RowCount
        // cref: ArcGIS.Core.Geometry.TextureResource.#ctor(ArcGIS.Core.Geometry.Texture)
        // cref: ArcGIS.Core.Geometry.BasicMaterial.TextureResource
        #region Create BasicMaterial with JPEG texture

        // read the jpeg into a buffer
        //At 3.0 you need https://www.nuget.org/packages/Microsoft.Windows.Compatibility
        //System.Drawing
        System.Drawing.Image image = System.Drawing.Image.FromFile(@"C:\temp\myImageFile.jpg");
        MemoryStream memoryStream = new MemoryStream();

        System.Drawing.Imaging.ImageFormat format = System.Drawing.Imaging.ImageFormat.Jpeg;
        image.Save(memoryStream, format);
        byte[] imageBuffer = memoryStream.ToArray();

        var jpgTexture = new JPEGTexture(imageBuffer);

        // texture properties
        int bpp = jpgTexture.BytesPerPixel;
        int columnCount = jpgTexture.ColumnCount;
        int rowCount = jpgTexture.RowCount;

        // build the textureResource and the material
        BasicMaterial material = new BasicMaterial();
        material.TextureResource = new TextureResource(jpgTexture);

        #endregion
      }

      {
        // cref: ArcGIS.Core.Geometry.UncompressedTexture.#ctor(System.Byte[],System.Int32,System.Int32,System.Byte)
        // cref: ArcGIS.Core.Geometry.UncompressedTexture.BytesPerPixel
        // cref: ArcGIS.Core.Geometry.UncompressedTexture.ColumnCount
        // cref: ArcGIS.Core.Geometry.UncompressedTexture.RowCount
        // cref: ArcGIS.Core.Geometry.TextureResource.#ctor(ArcGIS.Core.Geometry.Texture)
        // cref: ArcGIS.Core.Geometry.BasicMaterial.TextureResource
        #region Create BasicMaterial with Uncompressed texture

        UncompressedTexture uncompressedTexture1 = new UncompressedTexture(new byte[10 * 12 * 3], 10, 12, 3);

        // texture properties
        int bpp = uncompressedTexture1.BytesPerPixel;
        int columnCount = uncompressedTexture1.ColumnCount;
        int rowCount = uncompressedTexture1.RowCount;

        // build the textureResource and the material
        TextureResource tr = new TextureResource(uncompressedTexture1);
        BasicMaterial material = new BasicMaterial();
        material.TextureResource = tr;

        #endregion
      }
    }

    // cref: ArcGIS.Core.Geometry.Multipatch.IsMaterialTextured(System.Int32)
    // cref: ArcGIS.Core.Geometry.Multipatch.GetMaterialTextureCompressionType(System.Int32)
    // cref: ArcGIS.Core.Geometry.TextureCompressionType
    // cref: ArcGIS.Core.Geometry.Multipatch.GetMaterialTexture(System.Int32)
    // cref: ArcGIS.Core.Geometry.Multipatch.GetPatchMaterialIndex
    #region Get the texture image of a multipatch 

    // <summary>
    // This method gets the material texture image of a multipatch.
    // This method must be called on the MCT. Use QueuedTask.Run.
    // </summary>
    // <param name="multipatch">The input multipatch.</param>
    // <param name="patchIndex">The index of the patch (part) for which to get the material texture.</param>
    public void GetMultipatchTextureImage(Multipatch multipatch, int patchIndex)
    {
      int materialIndex = multipatch.GetPatchMaterialIndex(patchIndex);
      if (!multipatch.IsMaterialTextured(materialIndex))
        return;

      TextureCompressionType compressionType = 
        multipatch.GetMaterialTextureCompressionType(materialIndex);

      string ext = compressionType == TextureCompressionType.CompressionJPEG ? ".jpg" : ".dat";
      byte[] textureBuffer = multipatch.GetMaterialTexture(materialIndex);

      Stream imageStream = new MemoryStream(textureBuffer);
      System.Drawing.Image image = System.Drawing.Image.FromStream(imageStream);
      image.Save(@"C:\temp\myImage" + ext);
    }

    #endregion

    // cref: ArcGIS.Core.Geometry.Multipatch.HasNormals
    // cref: ArcGIS.Core.Geometry.Multipatch.GetPatchNormal(System.Int32,System.Int32)
    #region Get the normal coordinate of a multipatch 

    // <summary>
    // This method gets the normal coordinate of a multipatch and does something with it.
    // This method must be called on the MCT. Use QueuedTask.Run.
    // </summary>
    // <param name="multipatch">The input multipatch.</param>
    // <param name="patchIndex">The index of the patch (part) for which to get the normal.</param>
    public void DoSomethingWithNormalCoordinate(Multipatch multipatch, int patchIndex)
    {
      if (multipatch.HasNormals)
      {
        // If the multipatch has normals, then the number of normals is equal to the number of points.
        int numNormals = multipatch.GetPatchPointCount(patchIndex);

        for (int pointIndex = 0; pointIndex < numNormals; pointIndex++)
        {
          Coordinate3D normal = multipatch.GetPatchNormal(patchIndex, pointIndex);

          // Do something with the normal coordinate.
        }
      }
    }

    #endregion

    // cref: ArcGIS.Core.Geometry.Multipatch.GetPatchNormals(System.Int32,System.Collections.Generic.ICollection{ArcGIS.Core.Geometry.Coordinate3D}@)
    #region Get the normals of a multipatch 

    // <summary>
    // This method gets the normal coordinate of a multipatch and does something with it.
    // This method must be called on the MCT. Use QueuedTask.Run.
    // </summary>
    // <param name="multipatch">The input multipatch.</param>
    public void DoSomethingWithNormalCoordinates(Multipatch multipatch)
    {
      if (multipatch.HasNormals)
      {
        // Allocate the list only once
        int numPoints = multipatch.PointCount;
        ICollection<Coordinate3D> normals = new List<Coordinate3D>(numPoints);

        // The parts of a multipatch are also called patches
        int numPatches = multipatch.PartCount;

        for (int patchIndex = 0; patchIndex < numPatches; patchIndex++)
        {
          multipatch.GetPatchNormals(patchIndex, ref normals);

          // Do something with the normals for this patch.
        }
      }
    }

    #endregion

    // cref: ArcGIS.Core.Geometry.Multipatch.GetPatchMaterialIndex(System.Int32)
    // cref: ArcGIS.Core.Geometry.Multipatch.GetMaterialTransparencyPercent(System.Int32)
    // cref: ArcGIS.Core.Geometry.Multipatch.IsMaterialCullBackFace(System.Int32)
    // cref: ArcGIS.Core.Geometry.Multipatch.GetMaterialTextureBytesPerPixel(System.Int32)
    // cref: ArcGIS.Core.Geometry.Multipatch.GetMaterialTextureColumnCount(System.Int32)
    // cref: ArcGIS.Core.Geometry.Multipatch.GetMaterialTextureRowCount(System.Int32)
    // cref: ArcGIS.Core.Geometry.Multipatch.IsMaterialTextured
    #region Get the material properties of a multipatch 

    /// <summary>
    /// This method gets several properties of a material in a multipatch.
    /// This method must be called on the MCT. Use QueuedTask.Run.
    /// </summary>
    /// <param name="multipatch">The input multipatch.</param>
    /// <param name="patchIndex">The index of the patch (part) from which to get the material properties.</param>
    public void GetMaterialProperties(Multipatch multipatch, int patchIndex)
    {
      if (multipatch.HasMaterials)
      {
        // Get the material index for the specified patch.
        int materialIndex = multipatch.GetPatchMaterialIndex(patchIndex);

        System.Windows.Media.Color color = multipatch.GetMaterialColor(materialIndex);
        int tranparencyPercent = multipatch.GetMaterialTransparencyPercent(materialIndex);
        bool isBackCulled = multipatch.IsMaterialCullBackFace(materialIndex);

        if (multipatch.IsMaterialTextured(materialIndex))
        {
          int bpp = multipatch.GetMaterialTextureBytesPerPixel(materialIndex);
          int columnCount = multipatch.GetMaterialTextureColumnCount(materialIndex);
          int rowCount = multipatch.GetMaterialTextureRowCount(materialIndex);
        }
      }
    }

    #endregion

    #region ProSnippet Group: Multiparts
    #endregion

    // cref: ArcGIS.Core.Geometry.Geometry.IsEmpty
    // cref: ArcGIS.Core.Geometry.Geometry.GeometryType
    // cref: ArcGIS.Core.Geometry.Geometry.Clone
    // cref: ArcGIS.Core.Geometry.GeometryType
    // cref: ArcGIS.Core.Geometry.MultiPoint.Points
    // cref: ArcGIS.Core.Geometry.MultiPart.Parts
    // cref: ArcGIS.Core.Geometry.PolygonBuilderEx.CreatePolygon(IEnumerable{Segment},SpatialReference)
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
            // use the PolygonBuilderEx turning the segments into a standalone 
            // polygon instance
            singleParts.Add(PolygonBuilderEx.CreatePolygon(polygonPart));
          }
          break;
        case GeometryType.Polyline:
          var polyline = inputGeometry as Polyline;

          foreach (var polylinePart in polyline.Parts)
          {
            // use the PolylineBuilderEx turning the segments into a standalone
            // polyline instance
            singleParts.Add(PolylineBuilderEx.CreatePolyline(polylinePart));
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

    // cref: ArcGIS.Core.Geometry.Polygon
    // cref: ArcGIS.Core.Geometry.GeometryEngine.Within
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
      // see the "Get the individual parts of a multipart feature"
      // snippet for MultipartToSinglePart
      var parts = MultipartToSinglePart(inputPolygon);

      // get an enumeration of clockwise geometries (area > 0) ordered by the area
      var clockwiseParts = parts.Where(geom => ((Polygon)geom).Area > 0)
                            .OrderByDescending(geom => ((Polygon)geom).Area);

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

      PolygonBuilderEx outerRings = new PolygonBuilderEx();
      // now assemble a new polygon geometry based on the internal polygon collection
      foreach (var ring in internalRings)
      {
        outerRings.AddParts(ring.Parts);
      }

      // return the final geometry of the outer rings
      return outerRings.ToGeometry();
    }

    #endregion

    #region ProSnippet Group: Retrieve Geometry from Geodatabase
    #endregion

    public void RetrieveGeometryFromGeodatabase()
    {
      // cref: ArcGIS.Core.Geometry.Polygon
      // cref: ArcGIS.Core.Geometry.Geometry
      // cref: ArcGIS.Core.Data.Feature.GetShape
      #region Retrieve Geometry from Geodatabase

      // methods need to run on the MCT
      ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
      {
        try
        {
          // open a gdb
          using (ArcGIS.Core.Data.Geodatabase gdb = 
                    new ArcGIS.Core.Data.Geodatabase(
                        new FileGeodatabaseConnectionPath(new Uri(@"c:\Temp\MyDatabase.gdb"))))
          {
            //Open a featureClass 
            using (ArcGIS.Core.Data.FeatureClass featureClass = 
                           gdb.OpenDataset<ArcGIS.Core.Data.FeatureClass>("Polygon"))
            {
              
              ArcGIS.Core.Data.QueryFilter filter = 
                         new ArcGIS.Core.Data.QueryFilter()
                          {
                            WhereClause = "OBJECTID = 6"
                          };

              // get the row
              using (ArcGIS.Core.Data.RowCursor rowCursor = 
                                     featureClass.Search(filter, false))
              {
                while (rowCursor.MoveNext())
                {
                  using (var row = rowCursor.Current)
                  {
                    long oid = row.GetObjectID();

                    // get the shape from the row
                    ArcGIS.Core.Data.Feature feature = row as ArcGIS.Core.Data.Feature;
                    Polygon polygon = feature.GetShape() as Polygon;

                    // do something here
                  }
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

    #region ProSnippet Group: Import, Export Geometries
    #endregion

    public void ImportExport()
    {
      {
        // cref: ArcGIS.Core.Geometry.GeometryEngine.ExportToWKT(ArcGIS.Core.Geometry.WktExportFlags,ArcGIS.Core.Geometry.Geometry)
        // cref: ArcGIS.Core.Geometry.IGeometryEngine.ExportToWKT(ArcGIS.Core.Geometry.WktExportFlags,ArcGIS.Core.Geometry.Geometry)
        // cref: ArcGIS.Core.Geometry.GeometryEngine.ImportFromWKT(ArcGIS.Core.Geometry.WktImportFlags,System.String,ArcGIS.Core.Geometry.SpatialReference)
        // cref: ArcGIS.Core.Geometry.IGeometryEngine.ImportFromWKT(ArcGIS.Core.Geometry.WktImportFlags,System.String,ArcGIS.Core.Geometry.SpatialReference)
        // cref: ArcGIS.Core.Geometry.SpatialReferences.WebMercator
        // cref: ArcGIS.Core.Geometry.WktExportFlags
        // cref: ArcGIS.Core.Geometry.WktImportFlags
        #region Import and Export Geometries to well-known Text

        // create a point with z, m
        MapPoint point = MapPointBuilderEx.CreateMapPoint(
                 100, 200, 300, 400, SpatialReferences.WebMercator);

        // set the flags
        WktExportFlags wktExportFlags = WktExportFlags.WktExportDefaults;
        WktImportFlags wktImportFlags = WktImportFlags.WktImportDefaults;

        // export and import
        string wktString = GeometryEngine.Instance.ExportToWKT(wktExportFlags, point);
        MapPoint importPoint = GeometryEngine.Instance.ImportFromWKT(
              wktImportFlags, wktString, SpatialReferences.WebMercator) as MapPoint;

        double x = importPoint.X;       // x = 100
        double y = importPoint.Y;       // y = 200
        bool hasZ = importPoint.HasZ;   // hasZ = true
        double z = importPoint.Z;       // z = 300
        bool hasM = importPoint.HasM;   // hasM = true
        double m = importPoint.M;       // m = 400

        // export without z
        WktExportFlags exportFlagsNoZ = WktExportFlags.WktExportStripZs;
        wktString = GeometryEngine.Instance.ExportToWKT(exportFlagsNoZ, point);
        importPoint = GeometryEngine.Instance.ImportFromWKT(
          wktImportFlags, wktString, SpatialReferences.WebMercator) as MapPoint;

        x = importPoint.X;        // x = 100
        y = importPoint.Y;        // y = 200
        hasZ = importPoint.HasZ;  // hasZ = false
        z = importPoint.Z;        // z = 0
        hasM = importPoint.HasM;  // hasM = true
        m = importPoint.M;        // m = 400

        // export without m
        WktExportFlags exportFlagsNoM = WktExportFlags.WktExportStripMs;
        wktString = GeometryEngine.Instance.ExportToWKT(exportFlagsNoM, point);
        importPoint = GeometryEngine.Instance.ImportFromWKT(
          wktImportFlags, wktString, SpatialReferences.WebMercator) as MapPoint;

        x = importPoint.X;        // x = 100
        y = importPoint.Y;        // y = 200
        hasZ = importPoint.HasZ;  // hasZ = true
        z = importPoint.Z;        // z = 300
        hasM = importPoint.HasM;  // hasM = false
        m = importPoint.M;        // m = Nan

        // export without z, m
        wktString = GeometryEngine.Instance.ExportToWKT(
          exportFlagsNoZ | exportFlagsNoM, point);
        importPoint = GeometryEngine.Instance.ImportFromWKT(
          wktImportFlags, wktString, SpatialReferences.WebMercator) as MapPoint;

        x = importPoint.X;        // x = 100
        y = importPoint.Y;        // y = 200
        hasZ = importPoint.HasZ;  // hasZ = false
        z = importPoint.Z;        // z = 0
        hasM = importPoint.HasM;  // hasM = false
        m = importPoint.M;        // m = Nan

        #endregion
      }

      {
        // cref: ArcGIS.Core.Geometry.GeometryEngine.ExportToWKB(ArcGIS.Core.Geometry.WkbExportFlags,ArcGIS.Core.Geometry.Geometry)
        // cref: ArcGIS.Core.Geometry.IGeometryEngine.ExportToWKB(ArcGIS.Core.Geometry.WkbExportFlags,ArcGIS.Core.Geometry.Geometry)
        // cref: ArcGIS.Core.Geometry.GeometryEngine.ImportFromWKB(ArcGIS.Core.Geometry.WkbImportFlags,System.Byte[],ArcGIS.Core.Geometry.SpatialReference)
        // cref: ArcGIS.Core.Geometry.IGeometryEngine.ImportFromWKB(ArcGIS.Core.Geometry.WkbImportFlags,System.Byte[],ArcGIS.Core.Geometry.SpatialReference)
        // cref: ArcGIS.Core.Geometry.GeometryEngine.GetWKBSize(ArcGIS.Core.Geometry.WkbExportFlags,ArcGIS.Core.Geometry.Geometry)
        // cref: ArcGIS.Core.Geometry.IGeometryEngine.GetWKBSize(ArcGIS.Core.Geometry.WkbExportFlags,ArcGIS.Core.Geometry.Geometry)
        // cref: ArcGIS.Core.Geometry.GeometryEngine.ExportToWKB(ArcGIS.Core.Geometry.WkbExportFlags,ArcGIS.Core.Geometry.Geometry,System.Byte[]@)
        // cref: ArcGIS.Core.Geometry.IGeometryEngine.ExportToWKB(ArcGIS.Core.Geometry.WkbExportFlags,ArcGIS.Core.Geometry.Geometry,System.Byte[]@)
        // cref: ArcGIS.Core.Geometry.SpatialReferences.WGS84
        // cref: ArcGIS.Core.Geometry.WktExportFlags
        // cref: ArcGIS.Core.Geometry.WktImportFlags
        #region Import and Export Geometries to well-known Binary

        // create a polyline
        List<Coordinate2D> coords = new List<Coordinate2D>
        {
          new Coordinate2D(0, 0),
          new Coordinate2D(0, 1),
          new Coordinate2D(1, 1),
          new Coordinate2D(1, 0)
        };

        Polyline polyline = PolylineBuilderEx.CreatePolyline(
                    coords, SpatialReferences.WGS84);

        WkbExportFlags wkbExportFlags = WkbExportFlags.WkbExportDefaults;
        WkbImportFlags wkbImportFlags = WkbImportFlags.WkbImportDefaults;

        // export and import
        byte[] buffer = GeometryEngine.Instance.ExportToWKB(wkbExportFlags, polyline);
        Geometry geometry = GeometryEngine.Instance.ImportFromWKB(
                       wkbImportFlags, buffer, SpatialReferences.WGS84);
        Polyline importPolyline = geometry as Polyline;


        // alternatively, determine the size for the buffer
        int bufferSize = GeometryEngine.Instance.GetWKBSize(wkbExportFlags, polyline);
        buffer = new byte[bufferSize];
        // export
        bufferSize = GeometryEngine.Instance.ExportToWKB(
                        wkbExportFlags, polyline, ref buffer);
        // import
        importPolyline = GeometryEngine.Instance.ImportFromWKB(
                 wkbImportFlags, buffer, SpatialReferences.WGS84) as Polyline;

        #endregion
      }
    }

    public void ImportExportEsriShape()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.ExportToEsriShape(ArcGIS.Core.Geometry.EsriShapeExportFlags,ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.ExportToEsriShape(ArcGIS.Core.Geometry.EsriShapeExportFlags,ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.GeometryEngine.ImportFromEsriShape(ArcGIS.Core.Geometry.EsriShapeImportFlags,System.Byte[],ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.ImportFromEsriShape(ArcGIS.Core.Geometry.EsriShapeImportFlags,System.Byte[],ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.GeometryEngine.GetEsriShapeSize(ArcGIS.Core.Geometry.EsriShapeExportFlags,ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.GetEsriShapeSize(ArcGIS.Core.Geometry.EsriShapeExportFlags,ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.GeometryEngine.ExportToEsriShape(ArcGIS.Core.Geometry.EsriShapeExportFlags,ArcGIS.Core.Geometry.Geometry,System.Byte[]@)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.ExportToEsriShape(ArcGIS.Core.Geometry.EsriShapeExportFlags,ArcGIS.Core.Geometry.Geometry,System.Byte[]@)
      // cref: ArcGIS.Core.Geometry.Envelope.ToEsriShape
      // cref: ArcGIS.Core.Geometry.Envelope.ToEsriShape(System.Byte[]@)
      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.FromEsriShape(System.Byte[],ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Core.Geometry.SpatialReferences.WGS84
      // cref: ArcGIS.Core.Geometry.EsriShapeExportFlags
      #region Import and Export Geometries to EsriShape

      // create an envelope
      List<MapPoint> coordsZM = new List<MapPoint>
      {
        MapPointBuilderEx.CreateMapPoint(1001, 1002, 1003, 1004),
        MapPointBuilderEx.CreateMapPoint(2001, 2002, Double.NaN, 2004),
        MapPointBuilderEx.CreateMapPoint(3001, -3002, 3003, 3004),
        MapPointBuilderEx.CreateMapPoint(1001, -4002, 4003, 4004)
      };

      Envelope envelope = EnvelopeBuilderEx.CreateEnvelope(
                 coordsZM[0], coordsZM[2], SpatialReferences.WGS84);

      // export and import
      EsriShapeExportFlags exportFlags = EsriShapeExportFlags.EsriShapeExportDefaults;
      EsriShapeImportFlags importFlags = EsriShapeImportFlags.EsriShapeImportDefaults;
      byte[] buffer = GeometryEngine.Instance.ExportToEsriShape(exportFlags, envelope);
      Polygon importedPolygon = GeometryEngine.Instance.ImportFromEsriShape(
                     importFlags, buffer, envelope.SpatialReference) as Polygon;
      Envelope importedEnvelope = importedPolygon.Extent;

      // export without z,m
      buffer = GeometryEngine.Instance.ExportToEsriShape(
              EsriShapeExportFlags.EsriShapeExportStripZs | 
              EsriShapeExportFlags.EsriShapeExportStripMs, envelope);
      importedPolygon = GeometryEngine.Instance.ImportFromEsriShape(
              importFlags, buffer, SpatialReferences.WGS84) as Polygon;
      importedEnvelope = importedPolygon.Extent;

      bool hasZ = importedEnvelope.HasZ;      // hasZ = false
      bool hasM = importedEnvelope.HasM;      // hasM = false

      // export with shapeSize
      int bufferSize = GeometryEngine.Instance.GetEsriShapeSize(
                                             exportFlags, envelope);
      buffer = new byte[bufferSize];

      bufferSize = GeometryEngine.Instance.ExportToEsriShape(
                                   exportFlags, envelope, ref buffer);
      importedPolygon = GeometryEngine.Instance.ImportFromEsriShape(
                          importFlags, buffer, envelope.SpatialReference) as Polygon;
      importedEnvelope = importedPolygon.Extent;


      // or use the envelope and envelopeBuilderEx classes
      buffer = envelope.ToEsriShape();
      // buffer represents a polygon as there is not an envelope Esri shape buffer
      // EnvelopeBuilderEx.FromEsriShape takes a polygon Esri shape buffer and returns the extent of the polygon.
      importedEnvelope = EnvelopeBuilderEx.FromEsriShape(buffer);

      #endregion
    }

    public void ImportExportJson()
    {
      // cref: ArcGIS.Core.Geometry.GeometryEngine.ImportFromJson(ArcGIS.Core.Geometry.JsonImportFlags,System.String)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.ImportFromJson(ArcGIS.Core.Geometry.JsonImportFlags,System.String)
      // cref: ArcGIS.Core.Geometry.MapPointBuilderEx.FromJson(System.String)
      // cref: ArcGIS.Core.Geometry.GeometryEngine.ExportToJson(ArcGIS.Core.Geometry.JsonExportFlags,ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.ExportToJson(ArcGIS.Core.Geometry.JsonExportFlags,ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Geometry.Geometry.ToJson(System.Boolean)
      // cref: ArcGIS.Core.Geometry.MultipointBuilderEx.FromJson(System.String)
      // cref: ArcGIS.Core.Geometry.PolylineBuilderEx.FromJson(System.String)
      // cref: ArcGIS.Core.Geometry.PolygonBuilderEx.FromJson(System.String)
      // cref: ArcGIS.Core.Geometry.JsonImportFlags
      // cref: ArcGIS.Core.Geometry.JsonExportFlags
      #region Import and Export Geometries to JSON

      // MapPoint
      string inputString = 
        "{\"x\":1,\"y\":2,\"spatialReference\":{\"wkid\":4326,\"latestWkid\":4326}}";
      Geometry geometry = GeometryEngine.Instance.ImportFromJson(
        JsonImportFlags.JsonImportDefaults, inputString);

      MapPoint importPoint = geometry as MapPoint;
      // importPoint = 1, 2
      // importPoint.SpatialReference.WKid = 4326

      // use the MapPointBuilderEx convenience method
      MapPoint importPoint2 = MapPointBuilderEx.FromJson(inputString);
      // importPoint2 = 1, 2
      // impointPoint2.SpatialReference.Wkid = 4326

      string outputString = GeometryEngine.Instance.ExportToJson(
        JsonExportFlags.JsonExportDefaults, importPoint);
      // outputString =
      //   "{\"x\":1,\"y\":2,\"spatialReference\":{\"wkid\":4326,\"latestWkid\":4326}}"

      string outputString2 = importPoint.ToJson();

      inputString = 
        "{\"spatialReference\":{\"wkid\":4326},\"z\":3,\"m\":4,\"x\":1,\"y\":2}";
      importPoint = GeometryEngine.Instance.ImportFromJson(
        JsonImportFlags.JsonImportDefaults, inputString) as MapPoint;
      // importPoint.HasM = true
      // importPoint.HasZ = true
      // importPoint.X = 1
      // importPoint.Y = 2
      // importPoint.M = 4
      // importPoint.Z = 3

      importPoint2 = MapPointBuilderEx.FromJson(inputString);

      // export to json - skip spatial reference
      outputString = GeometryEngine.Instance.ExportToJson(
                    JsonExportFlags.JsonExportSkipCRS, importPoint);
      // outputString = "{\"x\":1,\"y\":2,\"z\":3,\"m\":4}"

      // export from mappoint, skipping the sr - same as GeometryEngine.Instance.ExportToJson w JsonExportFlags.JsonExportSkipCRS
      outputString2 = importPoint.ToJson(true);

      //
      // Multipoint
      //
      List<Coordinate2D> coords = new List<Coordinate2D>()
      {
        new Coordinate2D(100, 200),
        new Coordinate2D(201, 300),
        new Coordinate2D(301, 400),
        new Coordinate2D(401, 500)
      };

      Multipoint multipoint = MultipointBuilderEx.CreateMultipoint(
        coords, SpatialReferences.WebMercator);

      inputString = 
        "{\"points\":[[100,200],[201,300],[301,400],[401,500]],\"spatialReference\":{\"wkid\":3857}}";
      Multipoint importMultipoint = 
        GeometryEngine.Instance.ImportFromJson(
               JsonImportFlags.JsonImportDefaults, inputString) as Multipoint;
      // importMultipoint.IsEqual(multipoint) = true

      ReadOnlyPointCollection points = importMultipoint.Points;
      // points.Count = 4
      // points[0] = 100, 200
      // points[1] = 201, 300
      // points[2] = 301, 400
      // points[3] = 401, 500

      // use the MultipointbuilderEx convenience method
      Multipoint importMultipoint2 = MultipointBuilderEx.FromJson(inputString);
      // importMultipoint2.IsEqual(multipoint) = true

      // export to json
      outputString = GeometryEngine.Instance.ExportToJson(
                      JsonExportFlags.JsonExportDefaults, multipoint);
      // outputString = inputString

      // or use the multipoint itself
      outputString2 = multipoint.ToJson();

      //
      // Polyline
      //
      Polyline polyline = PolylineBuilderEx.CreatePolyline(
                               coords, SpatialReferences.WebMercator);

      // export without the spatial reference
      outputString = GeometryEngine.Instance.ExportToJson(
                              JsonExportFlags.JsonExportSkipCRS, polyline);
      // import
      geometry = GeometryEngine.Instance.ImportFromJson(
                   JsonImportFlags.JsonImportDefaults, outputString);
      Polyline importPolyline = geometry as Polyline;
      // importPolyline.SpatialReference = null


      points = importPolyline.Points;
      // points.Count = 4
      // points[0] = 100, 200
      // points[1] = 201, 300
      // points[2] = 301, 400
      // points[3] = 401, 500

      // use the polylineBuilderEx convenience method 
      Polyline importPolyline2 = PolylineBuilderEx.FromJson(outputString);
      // importPolyline2 = importPolyline

      outputString2 = importPolyline2.ToJson();
      // outputString2 = outputString

      //
      // Polygon
      //
      Polygon polygon = PolygonBuilderEx.CreatePolygon(
                                    coords, SpatialReferences.WebMercator);

      // export without the spatial reference
      outputString = GeometryEngine.Instance.ExportToJson(
                                JsonExportFlags.JsonExportSkipCRS, polygon);
      // import
      geometry = GeometryEngine.Instance.ImportFromJson(
                           JsonImportFlags.JsonImportDefaults, outputString);

      Polygon importPolygon = geometry as Polygon;
      // importPolygon.SpatialReference = null
      points = importPolygon.Points;
      // points.Count = 5

      // polygonBuilderEx convenience method
      Polygon importPolyon2 = PolygonBuilderEx.FromJson(outputString);
      // importPolygon2 = importPolygon

      // export from the polygon
      outputString2 = importPolyon2.ToJson(true);

      // Empty polygon
      polygon = PolygonBuilderEx.CreatePolygon(SpatialReferences.WebMercator);
      outputString = GeometryEngine.Instance.ExportToJson(
                                     JsonExportFlags.JsonExportDefaults, polygon);
      importPolygon = GeometryEngine.Instance.ImportFromJson(
                          JsonImportFlags.JsonImportDefaults, outputString) as Polygon;

      // importPolygon.IsEmpty = true
      // importPolygon.SpatialReference.Wkid = 3857

      #endregion
    }

    public void ImportExportXML()
    {
      // cref: ArcGIS.Core.Geometry.Geometry.ToXml
      // cref: ArcGIS.Core.Geometry.MapPointBuilderEx.FromXml(System.String)
      // cref: ArcGIS.Core.Geometry.EnvelopeBuilderEx.FromXml(System.String)
      // cref: ArcGIS.Core.Geometry.MultipointBuilderEx.FromXml(System.String)
      #region Import and Export Geometries to XML

      MapPoint minPoint = MapPointBuilderEx.CreateMapPoint(1, 1, 1, 1, 3);
      MapPoint maxPoint = MapPointBuilderEx.CreateMapPoint(5, 5, 5);

      // 
      //  MapPoint
      // 
      string xml = minPoint.ToXml();
      MapPoint minPointImport = MapPointBuilderEx.FromXml(xml);
      // minPointImport = minPoint

      //
      // Envelope
      //
      Envelope envelopeWithID = EnvelopeBuilderEx.CreateEnvelope(minPoint, maxPoint);

      // Envelopes don't have IDs
      // envelopeWithID.HasID = false
      // envelopeWithID.HasM = true
      // envelopeWithID.HasZ = true

      xml = envelopeWithID.ToXml();
      Envelope envelopeImport = EnvelopeBuilderEx.FromXml(xml);

      //
      // Multipoint
      //
      List<MapPoint> list = new List<MapPoint>();
      list.Add(minPoint);
      list.Add(maxPoint);

      Multipoint multiPoint = MultipointBuilderEx.CreateMultipoint(list);

      xml = multiPoint.ToXml();
      Multipoint multipointImport = MultipointBuilderEx.FromXml(xml);
      // multipointImport.PointCount == 2
      // multipointImport.HasID = true
      // multipointImport.HasM = true
      // multipointImport.HasZ= true

      #endregion
    }


    #region ProSnippet Group: Transformations
    #endregion

    public void Create_GeographicTransformation()
    {
      // cref: ArcGIS.Core.Geometry.GeographicTransformation.Create(System.Int32,System.Boolean)
      // cref: ArcGIS.Core.Geometry.GeographicTransformation.Create(System.String,System.Boolean)
      // cref: ArcGIS.Core.Geometry.GeographicTransformation.Name
      // cref: ArcGIS.Core.Geometry.GeographicTransformation.Wkt
      // cref: ArcGIS.Core.Geometry.GeographicTransformation.Wkid
      // cref: ArcGIS.Core.Geometry.GeographicTransformation.GetInverse
      // cref: ArcGIS.Core.Geometry.GeographicTransformation.IsForward
      #region Create Geographic Transformation

      // create from wkid
      GeographicTransformation gt1478 = 
        ArcGIS.Core.Geometry.GeographicTransformation.Create(1478);
      string name = gt1478.Name;
      string wkt = gt1478.Wkt;
      int wkid = gt1478.Wkid;

      // create from wkt
      GeographicTransformation another_gt1478 = 
           ArcGIS.Core.Geometry.GeographicTransformation.Create(wkt);

      // inverse
      GeographicTransformation inverse_gt148 = 
                     another_gt1478.GetInverse() as GeographicTransformation;
      bool isForward = inverse_gt148.IsForward;

      #endregion
    }

    public void Create_CompositeGeographicTransformation()
    {
      // cref: ArcGIS.Core.Geometry.CompositeGeographicTransformation.Create(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.GeographicTransformation})
      // cref: ArcGIS.Core.Geometry.CompositeGeographicTransformation.Create(System.Int32,System.Boolean)
      // cref: ArcGIS.Core.Geometry.CompositeGeographicTransformation.Create(System.String,System.Boolean)
      // cref: ArcGIS.Core.Geometry.CompositeGeographicTransformation.Count
      // cref: ArcGIS.Core.Geometry.CompositeGeographicTransformation.Transformations
      // cref: ArcGIS.Core.Geometry.CompositeGeographicTransformation.Item(System.Int32)
      // cref: ArcGIS.Core.Geometry.CompositeGeographicTransformation.GetInverse
      // cref: ArcGIS.Core.Geometry.CompositeGeographicTransformation.ToJson
      // cref: ArcGIS.Core.Geometry.DatumTransformation.CreateFromJson(System.String)
      #region Create Composite Geographic Transformation

      // Create singleton from wkid
      CompositeGeographicTransformation cgt = 
          ArcGIS.Core.Geometry.CompositeGeographicTransformation.Create(108272);
      int count = cgt.Count;    // count = 1

      IList<GeographicTransformation> gts = cgt.Transformations 
                                      as IList<GeographicTransformation>;
      gts.Add(ArcGIS.Core.Geometry.GeographicTransformation.Create(1437, false));
      count = cgt.Count;        // count = 2

      // create from an enumeration
      CompositeGeographicTransformation another_cgt = 
                    ArcGIS.Core.Geometry.CompositeGeographicTransformation.Create(gts);
      GeographicTransformation gt0 = another_cgt[0];
      GeographicTransformation gt1 = another_cgt[1];

      // get the inverse
      CompositeGeographicTransformation inversed_cgt = another_cgt.GetInverse() as CompositeGeographicTransformation;
      // inversed_cgt[0] is same as gt1
      // inversed_cgt[1] is same as gt0

      var wkt = gt0.Wkt;
      // create from string 
      CompositeGeographicTransformation third_cgt = 
         ArcGIS.Core.Geometry.CompositeGeographicTransformation.Create(wkt, gt0.IsForward);
      count = third_cgt.Count;        // count = 1

      // create from josn
      string json = cgt.ToJson();
      CompositeGeographicTransformation joson_cgt = 
        DatumTransformation.CreateFromJson(json) as CompositeGeographicTransformation;

      #endregion
    }

    public void Create_ProjectionTransformation()
    {
      // cref: ArcGIS.Core.Geometry.ProjectionTransformation.Create(ArcGIS.Core.Geometry.SpatialReference,ArcGIS.Core.Geometry.SpatialReference,ArcGIS.Core.Geometry.Envelope)
      // cref: ArcGIS.Core.Geometry.GeometryEngine.ProjectEx(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.ProjectionTransformation)
      // cref: ArcGIS.Core.Geometry.IGeometryEngine.ProjectEx(ArcGIS.Core.Geometry.Geometry,ArcGIS.Core.Geometry.ProjectionTransformation)
      #region Create Projection Transformation

      // methods need to be on the MCT
      ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
      {
        SpatialReference sr4267 = SpatialReferenceBuilder.CreateSpatialReference(4267);
        SpatialReference sr4326 = SpatialReferences.WGS84;
        SpatialReference sr3857 = SpatialReferences.WebMercator;

        // Create transformation from  4267 -> 3857
        ProjectionTransformation projTransFromSRs = 
             ArcGIS.Core.Geometry.ProjectionTransformation.Create(sr4267, sr3857);

        // create an envelope
        Envelope env = EnvelopeBuilderEx.CreateEnvelope(
                   new Coordinate2D(2, 2), new Coordinate2D(3, 3), sr4267);

        // Project with one geo transform 4267 -> 3857
        Envelope projectedEnvEx = GeometryEngine.Instance.ProjectEx(
                                       env, projTransFromSRs) as Envelope;

        // Create inverse transformation, 3857 -> 4267
        ProjectionTransformation projTransFromSRsInverse = 
            ArcGIS.Core.Geometry.ProjectionTransformation.Create(sr3857, sr4267);
        // Project the projected envelope back using the inverse transformation
        Envelope projectedEnvBack = 
               GeometryEngine.Instance.ProjectEx(
                 projectedEnvEx, projTransFromSRsInverse) as Envelope;

        bool isEqual = env.IsEqual(projectedEnvBack);
      });

      #endregion
    }

    public void Create_HVDatumTransformation()
    {
      // cref: ArcGIS.Core.Geometry.HVDatumTransformation.Create(System.Int32,System.Boolean)
      // cref: ArcGIS.Core.Geometry.HVDatumTransformation.Create(System.String,System.Boolean)
      // cref: ArcGIS.Core.Geometry.HVDatumTransformation.IsForward
      // cref: ArcGIS.Core.Geometry.HVDatumTransformation.Wkid
      // cref: ArcGIS.Core.Geometry.HVDatumTransformation.Name
      // cref: ArcGIS.Core.Geometry.HVDatumTransformation.Wkt
      // cref: ArcGIS.Core.Geometry.HVDatumTransformation.GetInverse
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
      HVDatumTransformation hv110018Inverse = 
         hv110018.GetInverse() as HVDatumTransformation;
      // hv110018Inverse.IsForward = false

      #endregion
    }

    public void Create_CompositeHVDatumTransformation()
    {
      // cref: ArcGIS.Core.Geometry.CompositeHVDatumTransformation.Create(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.HVDatumTransformation})
      // cref: ArcGIS.Core.Geometry.CompositeHVDatumTransformation.Create(System.Int32,System.Boolean)
      // cref: ArcGIS.Core.Geometry.CompositeHVDatumTransformation.Create(System.String,System.Boolean)
      // cref: ArcGIS.Core.Geometry.CompositeHVDatumTransformation.Count
      // cref: ArcGIS.Core.Geometry.CompositeHVDatumTransformation.Transformations
      // cref: ArcGIS.Core.Geometry.CompositeHVDatumTransformation.GetInverse
      // cref: ArcGIS.Core.Geometry.CompositeHVDatumTransformation.ToXml
      // cref: ArcGIS.Core.Geometry.CompositeHVDatumTransformation.CreateFromXml(System.String)
      // cref: ArcGIS.Core.Geometry.CompositeHVDatumTransformation.ToJson
      // cref: ArcGIS.Core.Geometry.DatumTransformation.CreateFromJson(System.String)
      // cref: ArcGIS.Core.Geometry.CompositeHVDatumTransformation.Item(System.Int32)
      #region Create Composite HV Datum Transformation

      HVDatumTransformation hv1 = HVDatumTransformation.Create(108034);
      HVDatumTransformation hv2 = HVDatumTransformation.Create(108033, false);
      List<HVDatumTransformation> hvs = new List<HVDatumTransformation>() { hv1, hv2 };

      // create from enumeration
      CompositeHVDatumTransformation compositehv = 
                      CompositeHVDatumTransformation.Create(hvs);
      int count = compositehv.Count;      // count = 2

      List<HVDatumTransformation> transforms = 
                     compositehv.Transformations as List<HVDatumTransformation>;
      HVDatumTransformation tranform = transforms[0];
      // transform.Wkid = 108034

      // get inverse
      CompositeHVDatumTransformation inverse_compositehv = 
             compositehv.GetInverse() as CompositeHVDatumTransformation;

      // create from xml
      string xml = compositehv.ToXml();
      //At 2.x - CompositeHVDatumTransformation xml_compositehv =
      //      CompositeHVDatumTransformation.CreateFromXML(xml);

      var xml_compositehv = CompositeHVDatumTransformation.CreateFromXml(xml);

      // create from json
      string json = compositehv.ToJson();
      CompositeHVDatumTransformation json_compositehv = 
          DatumTransformation.CreateFromJson(json) as CompositeHVDatumTransformation;

      #endregion
    }

    public void Determine_Transformations()
    {
      // cref: ArcGIS.Core.Geometry.ProjectionTransformation.FindTransformations(ArcGIS.Core.Geometry.SpatialReference,ArcGIS.Core.Geometry.SpatialReference,ArcGIS.Core.Geometry.Envelope,System.Int32,System.Boolean)
      // cref: ArcGIS.Core.Geometry.SpatialReferenceBuilder.CreateSpatialReference
      // cref: ArcGIS.Core.Geometry.ProjectionTransformation.FindTransformations
      // cref: ArcGIS.Core.Geometry.ProjectionTransformation.Transformation
      // cref: ArcGIS.Core.Geometry.CompositeGeographicTransformation
      // cref: ArcGIS.Core.Geometry.GeographicTransformation
      #region Determine Transformations

      // methods need to run on the MCT
      ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
      {
        //
        // find the first transformation used between spatial references 4267 and 4326
        //
        SpatialReference sr4267 = 
             SpatialReferenceBuilder.CreateSpatialReference(4267);
        SpatialReference sr4326 = SpatialReferences.WGS84;

        List<ProjectionTransformation> transformations = 
               ProjectionTransformation.FindTransformations(sr4267, sr4326);
        // transformations.Count = 1
        ProjectionTransformation projTrans = transformations[0];
        CompositeGeographicTransformation compositeGT = 
             projTrans.Transformation as CompositeGeographicTransformation;
        GeographicTransformation gt = compositeGT[0];
        // gt.Wkid = 15851
        // gt.Name = "NAD_1927_To_WGS_1984_79_CONUS"
        // gt.IsForward = true


        //
        // find the first five transformation used between spatial references 4267 and 4326
        //
        transformations = ProjectionTransformation.FindTransformations(
                                           sr4267, sr4326, numResults: 5);
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
        // find the first transformation used between spatial
        // references 4267 and 4326 within Alaska
        //

        // Alaska
        Envelope envelope = EnvelopeBuilderEx.CreateEnvelope(-161, 61, -145, 69);
        transformations = ProjectionTransformation.FindTransformations(
                                                    sr4267, sr4326, envelope);
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
        SpatialReference inSR = 
                        SpatialReferenceBuilder.CreateSpatialReference(4269, 115702);
        SpatialReference outSR = 
                        SpatialReferenceBuilder.CreateSpatialReference(4326, 3855);

        // Even though each spatial reference has a VCS,
        // vertical = false should return geographic transformations.
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

        transformations = 
                  ProjectionTransformation.FindTransformations(
                                          inSR, outSR, vertical: true);
        // transformations.Count = 1
        projTrans = transformations[0];

        CompositeHVDatumTransformation compositeHV = 
                projTrans.Transformation as CompositeHVDatumTransformation;
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

    #region ProSnippet Group: MapPoint GeoCoordinateString
    #endregion

    public void GeoCoordinateStringConversion()
    {
      // cref: ArcGIS.Core.Geometry.ToGeoCoordinateParameter.#ctor(ArcGIS.Core.Geometry.GeoCoordinateType)
      // cref: ArcGIS.Core.Geometry.ToGeoCoordinateParameter.#ctor(ArcGIS.Core.Geometry.GeoCoordinateType,ArcGIS.Core.Geometry.ToGeoCoordinateMode)
      // cref: ArcGIS.Core.Geometry.ToGeoCoordinateParameter.#ctor(ArcGIS.Core.Geometry.GeoCoordinateType,ArcGIS.Core.Geometry.ToGeoCoordinateMode,System.Int32,System.Boolean,System.Boolean)
      // cref: ArcGIS.Core.Geometry.MapPoint.ToGeoCoordinateString(ArcGIS.Core.Geometry.ToGeoCoordinateParameter)
      // cref: ArcGIS.Core.Geometry.MapPointBuilderEx.FromGeoCoordinateString(System.String,ArcGIS.Core.Geometry.SpatialReference,ArcGIS.Core.Geometry.GeoCoordinateType,ArcGIS.Core.Geometry.FromGeoCoordinateMode)
      // cref: ArcGIS.Core.Geometry.GeoCoordinateType
      #region MapPoint - GeoCoordinateString Conversion

      SpatialReference sr = SpatialReferences.WGS84;
      SpatialReference sr2 = SpatialReferences.WebMercator;

      // create some points
      MapPoint point0 = MapPointBuilderEx.CreateMapPoint(0, 0, sr);
      MapPoint point1 = MapPointBuilderEx.CreateMapPoint(10, 20, sr);
      MapPoint point2 = GeometryEngine.Instance.Project(point1, sr2) as MapPoint;
      MapPoint pointEmpty = MapPointBuilderEx.CreateMapPoint(sr);
      MapPoint pointwithNoSR = MapPointBuilderEx.CreateMapPoint(1, 1);
      MapPoint pointZM = MapPointBuilderEx.CreateMapPoint(1, 2, 3, 4, sr);

      // convert to MGRS
      ToGeoCoordinateParameter mgrsParam = 
               new ToGeoCoordinateParameter(GeoCoordinateType.MGRS);
      // 31NAA6602100000
      string geoCoordString = point0.ToGeoCoordinateString(mgrsParam);

      // use the builder to create a new point from the string.
      // Coordinates are the same 
      // outPoint.x = 0; outPoint.Y = 0
      MapPoint outPoint = 
            MapPointBuilderEx.FromGeoCoordinateString(
                      geoCoordString, sr, GeoCoordinateType.MGRS);

      // 32QPH0460911794
      // outPoint.X = 10; outPoint.Y = 20
      geoCoordString = point1.ToGeoCoordinateString(mgrsParam);             
      outPoint = MapPointBuilderEx.FromGeoCoordinateString(
                 geoCoordString, sr, GeoCoordinateType.MGRS);

      // z, m are not transformed
      // outPoint.X = 1; outPoint.Y = 2; outPoint.Z = Nan; outPoint.M = Nan;
      geoCoordString = pointZM.ToGeoCoordinateString(mgrsParam);
      outPoint = MapPointBuilderEx.FromGeoCoordinateString(
        geoCoordString, sr, GeoCoordinateType.MGRS);

      // set the number of digits to 2 and convert
      // 32QPH0512
      // outPoint.X = 10; outPoint.Y = 20
      mgrsParam.NumDigits = 2;
      geoCoordString = point1.ToGeoCoordinateString(mgrsParam);             
      outPoint = MapPointBuilderEx.FromGeoCoordinateString(
        geoCoordString, sr, GeoCoordinateType.MGRS);     


      // convert to UTM
      ToGeoCoordinateParameter utmParam = 
           new ToGeoCoordinateParameter(GeoCoordinateType.UTM);
      // 31N 166021 0000000
      geoCoordString = point0.ToGeoCoordinateString(utmParam);
      // 32Q 604609 2211793
      geoCoordString = point1.ToGeoCoordinateString(utmParam);        

      // convert to DMS
      ToGeoCoordinateParameter dmsParam = 
        new ToGeoCoordinateParameter(GeoCoordinateType.DMS);
      // 00 00 00.00N 000 00 00.00E
      geoCoordString = point0.ToGeoCoordinateString(dmsParam);
      // 20 00 00.00N 010 00 00.00E
      geoCoordString = point1.ToGeoCoordinateString(dmsParam);        

      // convert to DDM
      ToGeoCoordinateParameter ddmParam = 
        new ToGeoCoordinateParameter(GeoCoordinateType.DDM);
      // 00 00.0000N 000 00.0000E
      geoCoordString = point0.ToGeoCoordinateString(ddmParam);
      // 20 00.0000N 010 00.0000E
      geoCoordString = point1.ToGeoCoordinateString(ddmParam);        

      // convert to DD
      ToGeoCoordinateParameter ddParam = 
        new ToGeoCoordinateParameter(GeoCoordinateType.DD);
      // 00.000000N 000.000000E
      geoCoordString = point0.ToGeoCoordinateString(ddParam);
      // 20.000000N 010.000000E
      geoCoordString = point1.ToGeoCoordinateString(ddParam);       

      #endregion
    }

    public void OtherUtilities()
    {
      #region ProSnippet Group: AngularUnit
      #endregion

      // cref: ArcGIS.Core.Geometry.AngularUnit.Degrees
      // cref: ArcGIS.Core.Geometry.AngularUnit.ConvertToRadians(System.Double)
      // cref: ArcGIS.Core.Geometry.AngularUnit.ConvertFromRadians(System.Double)
      #region AngularUnit - Convert between degrees and radians

      // convert 45 degrees to radians
      double radians = AngularUnit.Degrees.ConvertToRadians(45);

      // convert PI to degrees
      double degrees = AngularUnit.Degrees.ConvertFromRadians(Math.PI);

      #endregion

      // cref: ArcGIS.Core.Geometry.AngularUnit.CreateAngularUnit(System.Int32)
      // cref: ArcGIS.Core.Geometry.AngularUnit.Degrees
      // cref: ArcGIS.Core.Geometry.AngularUnit.ConvertFromRadians
      // cref: ArcGIS.Core.Geometry.Unit.Name
      // cref: ArcGIS.Core.Geometry.Unit.ConversionFactor
      // cref: ArcGIS.Core.Geometry.AngularUnit.RadiansPerUnit
      // cref: ArcGIS.Core.Geometry.Unit.FactoryCode
      // cref: ArcGIS.Core.Geometry.AngularUnit.ConvertTo(System.Double,ArcGIS.Core.Geometry.AngularUnit)
      // cref: ArcGIS.Core.Geometry.AngularUnit.ConvertFromRadians(System.Double)
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
        // ArgumentException will be thrown by CreateAngularUnit in
        // the following scenarios:
        // - if the factory code used is a non-angular factory code
        //   (i.e. it corresponds to square meters which is an area unit code)
        // - if the factory code used is invalid
        //   (i.e. it is negative or doesn't correspond to any factory code)
      }

      #endregion

      // cref: ArcGIS.Core.Geometry.AngularUnit.CreateAngularUnit(System.String,System.Double)
      // cref: ArcGIS.Core.Geometry.AngularUnit.Degrees
      // cref: ArcGIS.Core.Geometry.AngularUnit.ConvertFromRadians
      // cref: ArcGIS.Core.Geometry.Unit.FactoryCode
      // cref: ArcGIS.Core.Geometry.AngularUnit.RadiansPerUnit
      // cref: ArcGIS.Core.Geometry.AngularUnit.Degrees
      // cref: ArcGIS.Core.Geometry.AngularUnit.ConvertTo(System.Double,ArcGIS.Core.Geometry.AngularUnit)
      // cref: ArcGIS.Core.Geometry.AngularUnit.CreateAngularUnit(System.String)
      #region AngularUnit - Create a Custom AngularUnit

      // custom unit - 3 radians per unit
      var myAngularUnit = AngularUnit.CreateAngularUnit("myCustomAngularUnit", 3);
      string Name = myAngularUnit.Name;                   // myCustomAngularUnit
      double Factor = myAngularUnit.ConversionFactor;     // 3
      int Code = myAngularUnit.FactoryCode;               // 0 because it is a custom angular unit
      double radiansUnit = myAngularUnit.RadiansPerUnit;  // 3

      // convert 10 degrees to my unit
      double converted = AngularUnit.Degrees.ConvertTo(10, myAngularUnit);
      // convert it back to degrees
      converted = myAngularUnit.ConvertTo(converted, AngularUnit.Degrees);

      // convert 1 radian into my angular units
      converted = myAngularUnit.ConvertFromRadians(1);

      // get the wkt
      string wkt = myAngularUnit.Wkt;

      // create an angular unit from this wkt
      var anotherAngularUnit = AngularUnit.CreateAngularUnit(wkt);
      // anotherAngularUnit.ConversionFactor = 3
      // anotherAngularUnit.FactoryCode = 0    
      // anotherAngularUnit.RadiansPerUnit = 3

      #endregion

      #region ProSnippet Group: LinearUnit
      #endregion

      // cref: ArcGIS.Core.Geometry.LinearUnit.Feet
      // cref: ArcGIS.Core.Geometry.LinearUnit.ConvertToMeters(System.Double)
      // cref: ArcGIS.Core.Geometry.LinearUnit.ConvertFromMeters(System.Double)
      #region LinearUnit - Convert between feet and meters
      // convert 10 feet to meters
      double metres = LinearUnit.Feet.ConvertToMeters(10);

      // convert 20 meters to feet
      double feet = LinearUnit.Feet.ConvertFromMeters(20.0);
      #endregion

      // cref: ArcGIS.Core.Geometry.LinearUnit.Centimeters
      // cref: ArcGIS.Core.Geometry.LinearUnit.Millimeters
      // cref: ArcGIS.Core.Geometry.LinearUnit.ConvertTo(System.Double,ArcGIS.Core.Geometry.LinearUnit)
      // cref: ArcGIS.Core.Geometry.LinearUnit.ConvertToMeters(System.Double)
      #region LinearUnit - Convert between centimeters and millimeters

      // convert 11 centimeters to millimeters
      double mm = LinearUnit.Centimeters.ConvertTo(11, LinearUnit.Millimeters);

      // convert the result back to centimeters
      double cm = LinearUnit.Millimeters.ConvertTo(mm, LinearUnit.Centimeters);

      // convert the millimeter result back to meters
      double meters = LinearUnit.Millimeters.ConvertToMeters(mm);

      #endregion

      // cref: ArcGIS.Core.Geometry.LinearUnit.CreateLinearUnit(System.Int32)
      // cref: ArcGIS.Core.Geometry.Unit.Name
      // cref: ArcGIS.Core.Geometry.Unit.ConversionFactor
      // cref: ArcGIS.Core.Geometry.LinearUnit.MetersPerUnit
      // cref: ArcGIS.Core.Geometry.LinearUnit.Centimeters
      // cref: ArcGIS.Core.Geometry.Unit.FactoryCode
      // cref: ArcGIS.Core.Geometry.LinearUnit.ConvertFromMeters(System.Double)
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
        // ArgumentException will be thrown by CreateLinearUnit
        // in the following scenarios:
        // - if the factory code used is a non-linear factory code
        //   (i.e. it corresponds to square meters which is an area unit code)
        // - if the factory code used is invalid
        //   (i.e. it is negative or doesn't correspond to any factory code)
      }

      #endregion

      // cref: ArcGIS.Core.Geometry.LinearUnit.CreateLinearUnit(System.String,System.Double)
      // cref: ArcGIS.Core.Geometry.LinearUnit.MetersPerUnit
      // cref: ArcGIS.Core.Geometry.Unit.ToString
      // cref: ArcGIS.Core.Geometry.Unit.Wkt
      // cref: ArcGIS.Core.Geometry.Unit.ConversionFactor
      // cref: ArcGIS.Core.Geometry.Unit.FactoryCode
      // cref: ArcGIS.Core.Geometry.LinearUnit.ConvertTo(System.Double,ArcGIS.Core.Geometry.LinearUnit)
      // cref: ArcGIS.Core.Geometry.LinearUnit.CreateLinearUnit(System.String)
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


      // get the wkt
      string lu_wkt = myLinearUnit.Wkt;

      // create an angular unit from this wkt
      var anotherLinearUnit = LinearUnit.CreateLinearUnit(lu_wkt);
      // anotherLinearUnit.ConversionFactor = 0.33
      // anotherLinearUnit.FactoryCode = 0    
      // anotherLinearUnit.MetersPerUnit = 0.33
      #endregion

      #region ProSnippet Group: AreaUnit
      #endregion

      // cref: ArcGIS.Core.Geometry.AreaUnit.ConvertFromSquareMeters(System.Double)
      // cref: ArcGIS.Core.Geometry.AreaUnit.ConvertToSquareMeters(System.Double)
      // cref: ArcGIS.Core.Geometry.AreaUnit.SquareFeet
      #region AreaUnit - Convert between square feet and square meters

      // convert 700 square meters to square feet
      double sqFeet = AreaUnit.SquareFeet.ConvertFromSquareMeters(700);

      // convert 1100 square feet to square meters
      double sqMeters = AreaUnit.SquareFeet.ConvertToSquareMeters(1000);
      #endregion

      // cref: ArcGIS.Core.Geometry.AreaUnit.ConvertTo(System.Double,ArcGIS.Core.Geometry.AreaUnit)
      // cref: ArcGIS.Core.Geometry.AreaUnit.Hectares
      // cref: ArcGIS.Core.Geometry.AreaUnit.Acres
      #region AreaUnit - Convert between hectares and acres

      // convert 2 hectares to acres
      double acres = AreaUnit.Hectares.ConvertTo(2, AreaUnit.Acres);

      #endregion

      // cref: ArcGIS.Core.Geometry.AreaUnit.ConvertTo(System.Double,ArcGIS.Core.Geometry.AreaUnit)
      // cref: ArcGIS.Core.Geometry.AreaUnit.SquareMiles
      // cref: ArcGIS.Core.Geometry.AreaUnit.Hectares
      #region AreaUnit - Convert between hectares and square miles
      // convert 300 hectares to square miles
      double sqMiles = AreaUnit.Hectares.ConvertTo(300, AreaUnit.SquareMiles);
      #endregion

      // cref: ArcGIS.Core.Geometry.AreaUnit.SquareMetersPerUnit
      // cref: ArcGIS.Core.Geometry.AreaUnit.Ares
      // cref: ArcGIS.Core.Geometry.AreaUnit.Hectares
      // cref: ArcGIS.Core.Geometry.AreaUnit.SquareKilometers
      // cref: ArcGIS.Core.Geometry.AreaUnit.SquareMiles
      // cref: ArcGIS.Core.Geometry.AreaUnit.SquareYards
      #region AreaUnit - How many Square meters in various units

      double sqMetersPerUnit = AreaUnit.Acres.SquareMetersPerUnit;
      sqMetersPerUnit = AreaUnit.Ares.SquareMetersPerUnit;
      sqMetersPerUnit = AreaUnit.Hectares.SquareMetersPerUnit;
      sqMetersPerUnit = AreaUnit.SquareKilometers.SquareMetersPerUnit;
      sqMetersPerUnit = AreaUnit.SquareMiles.SquareMetersPerUnit;
      sqMetersPerUnit = AreaUnit.SquareYards.SquareMetersPerUnit;

      #endregion

      // cref: ArcGIS.Core.Geometry.AreaUnit.CreateAreaUnit(System.Int32)
      // cref: ArcGIS.Core.Geometry.AreaUnit.CreateAreaUnit(System.String,System.Double)
      // cref: ArcGIS.Core.Geometry.AreaUnit.CreateAreaUnit(System.String)
      #region AreaUnit - Create an AreaUnit 

      try
      {
        var myFactoryCodeInit = AreaUnit.CreateAreaUnit(109439);     // 109439 is the factory code for square miles

        var myWktUnit = AreaUnit.CreateAreaUnit("HECTARE_AREAUNIT[\"H\",10000.0]");

        var myCustomUnit = AreaUnit.CreateAreaUnit("myAreaUnit", 12);
      }
      catch (ArgumentException)
      {
        // ArgumentException will be thrown by CreateAreaUnit in the following scenarios
        // - if the factory code used is a non-areal factory code  (i.e. it corresponds to degrees which is an angular unit code)
        // - if the factory code used is invalid (i.e. it is negative or doesn't correspond to any factory code)
      }

      #endregion

    }
  }
}
