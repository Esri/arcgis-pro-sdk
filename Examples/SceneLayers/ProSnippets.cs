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
    public async void Examples()
    {

      #region Create a Scene Layer

      var sceneLayerUrl = @"https://myportal.com/server/rest/services/Hosted/SceneLayerServiceName/SceneServer";
      //portal items also ok as long as the portal is the current active portal...
      //var sceneLayerUrl = @"https://myportal.com/home/item.html?id=123456789abcdef1234567890abcdef0";

      await QueuedTask.Run(() =>
      {
        //Create with initial visibility set to false. Add to current scene
        var createparams = new LayerCreationParams(new Uri(sceneLayerUrl, UriKind.Absolute))
        {
          IsVisible = false
        };

        //cast to specific type of scene layer being created - in this case FeatureSceneLayer
        var sceneLayer = LayerFactory.Instance.CreateLayer<Layer>(createparams, MapView.Active.Map) as FeatureSceneLayer;
        //or...specify the cast directly
        var sceneLayer2 = LayerFactory.Instance.CreateLayer<FeatureSceneLayer>(createparams, MapView.Active.Map);
        //ditto for BuildingSceneLayer, PointCloudSceneLayer, IntegratedMeshSceneLayer
        //...
      });
      
      #endregion

    }
  }
}
