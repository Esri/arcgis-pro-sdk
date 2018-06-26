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
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples
{
  class Time_Examples
  {
    /// MapView.Time, TimeRange.Offset
    /// <example>
    /// <code title="Step Map Time" description="Step forward in time by 1 month in the active map view." region="Step Map Time" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\Time_Examples.cs" lang="CS"/>
    /// </example>
    #region Step Map Time
    public void StepMapTime()
    {
      //Get the active view
      MapView mapView = MapView.Active;
      if (mapView == null)
        return;

      //Step current map time forward by 1 month
      TimeDelta timeDelta = new TimeDelta(1, TimeUnit.Months);
      mapView.Time = mapView.Time.Offset(timeDelta);
    }
    #endregion
  }
}
