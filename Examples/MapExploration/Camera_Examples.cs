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
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples
{
  class Camera_Examples
  {
    /// Camera
    /// <example>
    /// <code title="Camera Dock Pane" description="Create a dock pane showing the camera properties for the active map view." source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\CameraDockPane.xaml" lang="XAML"/>
    /// <code source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\CameraDockPane.xaml.cs" lang="CS"/>
    /// <code source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\CameraDockPaneViewModel.cs" lang="CS"/>
    /// </example>

    /// Camera.Heading
    /// <example>
    /// <code title="Rotate Map View" description="Rotate the active map view." region="Rotate Map View Asynchronous" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\MapView_Examples.cs" lang="CS"/>
    /// </example>

    /// Camera.SpatialReference
    /// <example>
    /// <code title="Project Camera" description="Project a camera into a new spatial reference." region="Project Camera" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\Camera_Examples.cs" lang="CS"/>
    /// </example>
    // cref: Project Camera;ArcGIS.Desktop.Mapping.Camera.SpatialReference
    #region Project Camera
    public Task<Camera> ProjectCamera(Camera camera, ArcGIS.Core.Geometry.SpatialReference spatialReference)
    {
      return QueuedTask.Run(() =>
      {
        var mapPoint = MapPointBuilder.CreateMapPoint(camera.X, camera.Y, camera.Z, camera.SpatialReference);
        var newPoint = GeometryEngine.Instance.Project(mapPoint, spatialReference) as MapPoint;
        var newCamera = new Camera()
        {
          X = newPoint.X,
          Y = newPoint.Y,
          Z = newPoint.Z,
          Scale = camera.Scale,
          Pitch = camera.Pitch,
          Heading = camera.Heading,
          Roll = camera.Roll,
          Viewpoint = camera.Viewpoint,
          SpatialReference = spatialReference
        };
        return newCamera;
      });
    }
    #endregion
  }
}
