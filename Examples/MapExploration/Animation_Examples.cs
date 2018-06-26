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
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples
{
  class Animation_Examples
  {
    /// Animation.ScaleDuration(double), Animation.Duration
    /// <example>
    /// <code title="Set the length of the animation" description="Set the length of the animation." region="Set Animation Length" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\Animation_Examples.cs" lang="CS"/>
    /// </example>
    #region Set Animation Length
    public void SetAnimationLength(TimeSpan length)
    {
      var mapView = MapView.Active;
      if (mapView != null)
        return;

      var animation = mapView.Map.Animation;
      var duration = animation.Duration;
      if (duration == TimeSpan.Zero)
        return;

      var factor = length.TotalSeconds / duration.TotalSeconds;
      animation.ScaleDuration(factor);
    }
    #endregion

    /// Animation.ScaleDuration(double)
    /// <example>
    /// <code title="Scale animation" description="Scale the length of the animation between a start and end time." region="Scale Animation" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\Animation_Examples.cs" lang="CS"/>
    /// </example>
    #region Scale Animation
    public void ScaleAnimationAfterTime(TimeSpan afterTime, TimeSpan length)
    {
      var mapView = MapView.Active;
      if (mapView != null)
        return;

      var animation = mapView.Map.Animation;
      var duration = animation.Duration;
      if (duration == TimeSpan.Zero || duration <= afterTime)
        return;

      var factor = length.TotalSeconds / (duration.TotalSeconds - afterTime.TotalSeconds);
      animation.ScaleDuration(afterTime, duration, factor);
    }
    #endregion

    /// Animation.Tracks, Track.Keyframes
    /// <example>
    /// <code title="Get the camera keyframes" description="Get the camera keyframes from the animation." region="Camera Keyframes" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\Animation_Examples.cs" lang="CS"/>
    /// </example>
    #region Camera Keyframes
    public List<CameraKeyframe> GetCameraKeyframes()
    {
      var mapView = MapView.Active;
      if (mapView != null)
        return null;

      var animation = mapView.Map.Animation;
      var cameraTrack = animation.Tracks.OfType<CameraTrack>().First(); //There will always be only 1 CameraTrack in the animation.
      return cameraTrack.Keyframes.OfType<CameraKeyframe>().ToList();
    }
    #endregion

    /// Animation.GetCameraAtTime
    /// <example>
    /// <code title="Get the camera at each frame" description="Get the camera at each frame in the Animation." region="Interpolate Camera" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\Animation_Examples.cs" lang="CS"/>
    /// </example>
    #region Interpolate Camera
    public Task<List<Camera>> GetInterpolatedCameras()
    {
      //Return the collection representing the camera for each frame in animation.
      return QueuedTask.Run(() =>
      {
        var mapView = MapView.Active;
        if (mapView != null || mapView.Animation == null)
          return null;

        var animation = mapView.Map.Animation;

        var cameras = new List<Camera>();
        //We will use ticks here rather than milliseconds to get the highest precision possible.
        var ticksPerFrame = Convert.ToInt64(animation.Duration.Ticks / (animation.NumberOfFrames - 1));
        for (int i = 0; i < animation.NumberOfFrames; i++)
        {
          var time = TimeSpan.FromTicks(i * ticksPerFrame);
          //Because of rounding for ticks the last calculated time may be greating than the duration.
          if (time > animation.Duration)
            time = animation.Duration;
          cameras.Add(mapView.Animation.GetCameraAtTime(time));
        }
        return cameras;
      });
    }
    #endregion

    /// Animation.GetCurrentTimeAtTime
    /// <example>
    /// <code title="Get the time at each frame" description="Get the time at each frame in the Animation." region="Interpolate Time" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\Animation_Examples.cs" lang="CS"/>
    /// </example>
    #region Interpolate Time
    public Task<List<TimeRange>> GetInterpolatedMapTimes()
    {
      //Return the collection representing the map time for each frame in animation.
      return QueuedTask.Run(() =>
      {
        var mapView = MapView.Active;
        if (mapView != null || mapView.Animation == null)
          return null;

        var animation = mapView.Map.Animation;

        var timeRanges = new List<TimeRange>();
        //We will use ticks here rather than milliseconds to get the highest precision possible.
        var ticksPerFrame = Convert.ToInt64(animation.Duration.Ticks / (animation.NumberOfFrames - 1));
        for (int i = 0; i < animation.NumberOfFrames; i++)
        {
          var time = TimeSpan.FromTicks(i * ticksPerFrame);
          //Because of rounding for ticks the last calculated time may be greating than the duration.
          if (time > animation.Duration)
            time = animation.Duration;
          timeRanges.Add(mapView.Animation.GetCurrentTimeAtTime(time));
        }
        return timeRanges;
      });
    }
    #endregion

    /// Animation.GetCurrentRangeAtTime
    /// <example>
    /// <code title="Get the range at each frame" description="Get the range at each frame in the Animation." region="Interpolate Range" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\Animation_Examples.cs" lang="CS"/>
    /// </example>
    #region Interpolate Range
    public Task<List<Range>> GetInterpolatedMapRanges()
    {
      //Return the collection representing the map time for each frame in animation.
      return QueuedTask.Run(() =>
      {
        var mapView = MapView.Active;
        if (mapView != null || mapView.Animation == null)
          return null;

        var animation = mapView.Map.Animation;

        var ranges = new List<Range>();
        //We will use ticks here rather than milliseconds to get the highest precision possible.
        var ticksPerFrame = Convert.ToInt64(animation.Duration.Ticks / (animation.NumberOfFrames - 1));
        for (int i = 0; i < animation.NumberOfFrames; i++)
        {
          var time = TimeSpan.FromTicks(i * ticksPerFrame);
          //Because of rounding for ticks the last calculated time may be greating than the duration.
          if (time > animation.Duration)
            time = animation.Duration;
          ranges.Add(mapView.Animation.GetCurrentRangeAtTime(time));
        }
        return ranges;
      });
    }
    #endregion

    /// CameraTrack.CreateKeyframe
    /// <example>
    /// <code title="Create a new camera keyframe" description="Create a new camera keyframe from the active map view." region="Create Camera Keyframe" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\Animation_Examples.cs" lang="CS"/>
    /// </example>
    #region Create Camera Keyframe
    public void CreateCameraKeyframe(TimeSpan atTime)
    {
      var mapView = MapView.Active;
      if (mapView != null)
        return;

      var animation = mapView.Map.Animation;
      var cameraTrack = animation.Tracks.OfType<CameraTrack>().First(); //There will always be only 1 CameraTrack in the animation.
      cameraTrack.CreateKeyframe(mapView.Camera, atTime, ArcGIS.Core.CIM.AnimationTransition.FixedArc);    
    }
    #endregion

    /// TimeTrack.CreateKeyframe
    /// <example>
    /// <code title="Create a new time keyframe" description="Create a new time keyframe from the active map view." region="Create Time Keyframe" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\Animation_Examples.cs" lang="CS"/>
    /// </example>
    #region Create Time Keyframe
    public void CreateTimeKeyframe(TimeSpan atTime)
    {
      var mapView = MapView.Active;
      if (mapView != null)
        return;

      var animation = mapView.Map.Animation;
      var timeTrack = animation.Tracks.OfType<TimeTrack>().First(); //There will always be only 1 TimeTrack in the animation.
      timeTrack.CreateKeyframe(mapView.Time, atTime, ArcGIS.Core.CIM.AnimationTransition.Linear);
    }
    #endregion

    /// RangeTrack.CreateKeyframe
    /// <example>
    /// <code title="Create a new range keyframe" description="Create a new range keyframe." region="Create Range Keyframe" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\Animation_Examples.cs" lang="CS"/>
    /// </example>
    #region Create Range Keyframe
    public void CreateRangeKeyframe(Range range, TimeSpan atTime)
    {
      var mapView = MapView.Active;
      if (mapView != null)
        return;

      var animation = mapView.Map.Animation;
      var rangeTrack = animation.Tracks.OfType<RangeTrack>().First(); //There will always be only 1 RangeTrack in the animation.
      rangeTrack.CreateKeyframe(range, atTime, ArcGIS.Core.CIM.AnimationTransition.Linear);
    }
    #endregion

    /// RangeTrack.CreateKeyframe
    /// <example>
    /// <code title="Create a new layer keyframe" description="Create a new layer keyframe." region="Create Layer Keyframe" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\Animation_Examples.cs" lang="CS"/>
    /// </example>
    #region Create Layer Keyframe
    public void CreateLayerKeyframe(Layer layer, double transparency, TimeSpan atTime)
    {
      var mapView = MapView.Active;
      if (mapView != null)
        return;

      var animation = mapView.Map.Animation;
      var layerTrack = animation.Tracks.OfType<LayerTrack>().First(); //There will always be only 1 LayerTrack in the animation.
      layerTrack.CreateKeyframe(layer, atTime, true, transparency, ArcGIS.Core.CIM.AnimationTransition.Linear);
    }
    #endregion
  }
}
