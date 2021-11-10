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
using ArcGIS.Core.Geometry;

namespace ForGeometryAPI {
    class GeometryCodeExamples {

        public void CodeExamples() {

            #region Example1

            // methods need to run on the MCT
            ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
              List<MapPoint> list = new List<MapPoint>();
              MapPoint minPt = MapPointBuilder.CreateMapPoint(1.0, 1.0);
              MapPoint maxPt = MapPointBuilder.CreateMapPoint(2.0, 2.0);

              // create an envelope
              Envelope env = EnvelopeBuilder.CreateEnvelope(minPt, maxPt);

              // create a polygon from the envelope
              using (PolygonBuilder polygonBuilder = new PolygonBuilder(env))
              {
                Polygon poly = polygonBuilder.ToGeometry();
              }
            });

            #endregion Example1
			
			      // cref: Example2;ArcGIS.Core.Geometry.PolygonBuilder.CreatePolygon(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Coordinate3D},ArcGIS.Core.Geometry.SpatialReference)
			      #region Example2

            // methods need to run on the MCT
            ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
              List<MapPoint> list3D = new List<MapPoint>();
              list3D.Add(MapPointBuilder.CreateMapPoint(1.0, 1.0, 1.0, 2.0));
              list3D.Add(MapPointBuilder.CreateMapPoint(1.0, 2.0, 1.0, 2.0));
              list3D.Add(MapPointBuilder.CreateMapPoint(2.0, 2.0, 1.0, 2.0));
              list3D.Add(MapPointBuilder.CreateMapPoint(2.0, 1.0, 1.0, 2.0));

              var polygon = PolygonBuilder.CreatePolygon(list3D);
            });


            #endregion Example2
        }

    }
}
