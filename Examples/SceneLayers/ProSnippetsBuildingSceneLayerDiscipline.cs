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

namespace ProSnippetsBuildingSceneLayerDiscipline
{
  #region ProSnippet Group: Building Discipline Scene Layer
  #endregion

  class ProSnippetsBuildingSceneLayerDiscipline
  {
    public async void Examples()
    {
      #region Get BuildingDisciplineSceneLayer Discipline
      var bsl_discipline = MapView.Active.Map.GetLayersAsFlattenedList().OfType<BuildingDisciplineSceneLayer>().FirstOrDefault(l => l.Name == "Architectural");
      var disciplineName = bsl_discipline.GetDiscipline();
      #endregion

      }

    #region Enumerate the Discipline Layers from a Building SceneLayer
    public void QueryBuildingSceneLayer()
    {
      var bldgLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<BuildingSceneLayer>().First();
      var disciplines = new Dictionary<string, BuildingDisciplineSceneLayer>();
      //A Building layer has two children - Overview and FullModel
      //Overview is a FeatureSceneLayer
      //Full Model is a BuildingDisciplineSceneLayer that contains the disciplines
      
      var fullModel = bldgLayer.FindLayers("Full Model").First() as BuildingDisciplineSceneLayer;
      CollectDisciplineLayers(fullModel, disciplines);
      
    }

    internal void CollectDisciplineLayers(BuildingDisciplineSceneLayer disciplineLayer,
      Dictionary<string, BuildingDisciplineSceneLayer> disciplines)
    {
      //collect information on the disciplines
      var name = disciplineLayer.Name;
      var layerType = ((ISceneLayerInfo)disciplineLayer).SceneServiceLayerType.ToString();
      var discipline = disciplineLayer.GetDiscipline();
      //etc
      //TODO - use collected information

      disciplines.Add(discipline, disciplineLayer);

      //Discipline layers are composite layers too
      foreach (var childDiscipline in disciplineLayer.Layers.OfType<BuildingDisciplineSceneLayer>())
      {
        //Discipline layers can also contain FeatureSceneLayers that render the
        //individual full model contents
        var content_names = string.Join(", ", childDiscipline.Layers
             .OfType<FeatureSceneLayer>().Select(fl => fl.Name));
        CollectDisciplineLayers(childDiscipline, disciplines);
      }
    }

    #endregion
  }
}