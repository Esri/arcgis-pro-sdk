/*

   Copyright 2023 Esri

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
using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Data.Realtime;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MapAuthoring
{
  internal class ProSnippets3DAnalystLayers
  {
    #region PrSnippet Group: Layer Methods for TIN, Terrain, LasDataset
    #endregion

    public void GetLayers()
    {
      // cref: ArcGIS.Desktop.Mapping.TinLayer
      // cref: ArcGIS.Desktop.Mapping.TerrainLayer
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer
      #region Retrieve layers

      // find the first TIN layer
      var tinLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<TinLayer>().FirstOrDefault();

      // find the first Terrain layer
      var terrainLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<TerrainLayer>().FirstOrDefault();

      // find the first LAS dataset layer
      var lasDatasetLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<LasDatasetLayer>().FirstOrDefault();

      // find the set of surface layers
      var surfacelayers = MapView.Active.Map.GetLayersAsFlattenedList().OfType<SurfaceLayer>();
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TinLayer.GetTinDataset
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.GetDefinition
      // cref: ArcGIS.Core.Data.Analyst3D.TinDatasetDefinition
      // cref: ArcGIS.Core.Data.Analyst3D.TinDatasetDefinition.GetExtent
      // cref: ArcGIS.Core.Data.Analyst3D.TinDatasetDefinition.GetSpatialReference
      // cref: ArcGIS.Desktop.Mapping.TerrainLayer.GetTerrain
      // cref: ArcGIS.Core.Data.Analyst3D.Terrain
      // cref: ArcGIS.Core.Data.Analyst3D.Terrain.GetDefinition
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainDefinition
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainDefinition.GetExtent
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainDefinition.GetSpatialReference
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.GetLasDataset
      // cref: ArcGIS.Core.Data.Analyst3D.LasDataset
      // cref: ArcGIS.Core.Data.Analyst3D.LasDataset.GetDefinition
      // cref: ArcGIS.Core.Data.Analyst3D.LasDatasetDefinition
      // cref: ArcGIS.Core.Data.Analyst3D.LasDatasetDefinition.GetExtent
      // cref: ArcGIS.Core.Data.Analyst3D.LasDatasetDefinition.GetSpatialReference
      #region Retrieve dataset objects

      //Must be on the QueuedTask.Run()

      Envelope extent;
      SpatialReference sr;
      using (var tin = tinLayer.GetTinDataset())
      {
        using (var tinDef = tin.GetDefinition())
        {
          extent = tinDef.GetExtent();
          sr = tinDef.GetSpatialReference();
        }
      }

      using (var terrain = terrainLayer.GetTerrain())
      {
        using (var terrainDef = terrain.GetDefinition())
        {
          extent = terrainDef.GetExtent();
          sr = terrainDef.GetSpatialReference();
        }
      }

      using (var lasDataset = lasDatasetLayer.GetLasDataset())
      {
        using (var lasDatasetDef = lasDataset.GetDefinition())
        {
          extent = lasDatasetDef.GetExtent();
          sr = lasDatasetDef.GetSpatialReference();
        }
      }
      #endregion
    }

    public void SurfaceLayerCreation()
    {
      Map map = null;

      // cref: ArcGIS.Desktop.Mapping.TinLayerCreationParams
      // cref: ArcGIS.Desktop.Mapping.TinLayerCreationParams.#ctor(System.Uri)
      // cref: ArcGIS.Desktop.Mapping.TinLayer
      #region Create a TinLayer
      //Must be on the QueuedTask.Run()

      string tinPath = @"d:\Data\Tin\TinDataset";
      var tinURI = new Uri(tinPath);

      var tinCP = new TinLayerCreationParams(tinURI);
      tinCP.Name = "My TIN Layer";
      tinCP.IsVisible = false;

      //Create the layer to the TIN
      var tinLayer = LayerFactory.Instance.CreateLayer<TinLayer>(tinCP, map);
      #endregion

      ArcGIS.Core.Data.Analyst3D.TinDataset tinDataset = null;

      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset
      // cref: ArcGIS.Desktop.Mapping.TinLayerCreationParams
      // cref: ArcGIS.Desktop.Mapping.TinLayerCreationParams.#ctor(ArcGIS.Core.Data.Analyst3D.TinDataset)
      // cref: ArcGIS.Desktop.Mapping.TinLayer
      #region Create a TinLayer from a dataset
      //Must be on the QueuedTask.Run()

      var tinCP_ds = new TinLayerCreationParams(tinDataset);
      tinCP_ds.Name = "My TIN Layer";
      tinCP_ds.IsVisible = false;

      //Create the layer to the TIN

      var tinLayer_ds = LayerFactory.Instance.CreateLayer<TinLayer>(tinCP_ds, map);
      #endregion

      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset
      // cref: ArcGIS.Desktop.Mapping.TinLayerCreationParams
      // cref: ArcGIS.Desktop.Mapping.TinLayerCreationParams.#ctor(ArcGIS.Core.Data.Analyst3D.TinDataset)
      // cref: ArcGIS.Desktop.Mapping.TinLayer
      // cref: ArcGIS.Desktop.Mapping.TinLayerCreationParams.RendererDefinitions
      // cref: ArcGIS.Desktop.Mapping.TinRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      // cref: ArcGIS.Desktop.Mapping.TinNodeRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.TinNodeRendererDefinition.#ctor
      // cref: ArcGIS.Desktop.Mapping.TinFaceClassBreaksRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.TinFaceClassBreaksRendererDefinition.#ctor
      // cref: ArcGIS.Desktop.Mapping.TinColorRampRendererDefinition.ClassificationMethod
      #region Create a TinLayer with renderers
      //Must be on the QueuedTask.Run()

      var tinCP_renderers = new TinLayerCreationParams(tinDataset);
      tinCP_renderers.Name = "My TIN layer";
      tinCP_renderers.IsVisible = true;

      // define the node renderer - use defaults
      var node_rd = new TinNodeRendererDefinition();

      // define the face/surface renderer
      var face_rd = new TinFaceClassBreaksRendererDefinition();
      face_rd.ClassificationMethod = ClassificationMethod.NaturalBreaks;
      // accept default color ramp, breakCount

      // set up the renderer dictionary
      var rendererDict = new Dictionary<SurfaceRendererTarget, TinRendererDefinition>();
      rendererDict.Add(SurfaceRendererTarget.Points, node_rd);
      rendererDict.Add(SurfaceRendererTarget.Surface, face_rd);

      // assign the dictionary to the creation params
      tinCP_renderers.RendererDefinitions = rendererDict;

      // create the layer
      var tinLayer_rd = LayerFactory.Instance.CreateLayer<TinLayer>(tinCP_renderers, MapView.Active.Map);
      #endregion


      // cref: ArcGIS.Desktop.Mapping.TerrainLayerCreationParams
      // cref: ArcGIS.Desktop.Mapping.TerrainLayerCreationParams.#ctor(System.Uri)
      // cref: ArcGIS.Desktop.Mapping.TerrainLayer
      #region Create a TerrainLayer
      //Must be on the QueuedTask.Run()

      string terrainPath = @"d:\Data\Terrain\filegdb_Containing_A_Terrain.gdb";
      var terrainURI = new Uri(terrainPath);

      var terrainCP = new TerrainLayerCreationParams(terrainURI);
      terrainCP.Name = "My Terrain Layer";
      terrainCP.IsVisible = false;

      //Create the layer to the terrain
      var terrainLayer = LayerFactory.Instance.CreateLayer<TerrainLayer>(terrainCP, map);

      #endregion

      ArcGIS.Core.Data.Analyst3D.Terrain terrain = null;

      // cref: ArcGIS.Core.Data.Analyst3D.Terrain
      // cref: ArcGIS.Desktop.Mapping.TerrainLayerCreationParams
      // cref: ArcGIS.Desktop.Mapping.TerrainLayerCreationParams.#ctor(ArcGIS.Core.Data.Analyst3D.Terrain)
      // cref: ArcGIS.Desktop.Mapping.TerrainLayer
      #region Create a TerrainLayer from a dataset
      //Must be on the QueuedTask.Run()

      var terrainCP_ds = new TerrainLayerCreationParams(terrain);
      terrainCP_ds.Name = "My Terrain Layer";
      terrainCP_ds.IsVisible = true;

      //Create the layer to the terrain
      var terrainLayer_ds = LayerFactory.Instance.CreateLayer<TerrainLayer>(terrainCP_ds, map);
      #endregion

      // cref: ArcGIS.Core.Data.Analyst3D.Terrain
      // cref: ArcGIS.Desktop.Mapping.TerrainLayerCreationParams
      // cref: ArcGIS.Desktop.Mapping.TerrainLayerCreationParams.#ctor(ArcGIS.Core.Data.Analyst3D.Terrain)
      // cref: ArcGIS.Desktop.Mapping.TerrainLayer
      // cref: ArcGIS.Desktop.Mapping.TerrainLayerCreationParams.RendererDefinitions
      // cref: ArcGIS.Desktop.Mapping.TinRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      // cref: ArcGIS.Desktop.Mapping.TinBreaklineRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.TinBreaklineRendererDefinition.#ctor
      // cref: ArcGIS.Desktop.Mapping.TinFaceClassBreaksRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.TinFaceClassBreaksRendererDefinition.#ctor
      // cref: ArcGIS.Desktop.Mapping.TinColorRampRendererDefinition.ClassificationMethod
      // cref: ArcGIS.Desktop.Mapping.TerrainDirtyAreaRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.TerrainDirtyAreaRendererDefinition.#ctor
      #region Create a TerrainLayer with renderers
      //Must be on the QueuedTask.Run()

      var terrainCP_renderers = new TerrainLayerCreationParams(terrain);
      terrainCP_renderers.Name = "My LAS Layer";
      terrainCP_renderers.IsVisible = true;

      // define the edge type renderer - use defaults
      var edgeRD = new TinBreaklineRendererDefinition();

      // define the face/surface renderer
      var faceRD = new TinFaceClassBreaksRendererDefinition();
      faceRD.ClassificationMethod = ClassificationMethod.NaturalBreaks;
      // accept default color ramp, breakCount

      // define the dirty area renderer - use defaults
      var dirtyAreaRD = new TerrainDirtyAreaRendererDefinition();

      // add renderers to dictionary
      var t_dict = new Dictionary<SurfaceRendererTarget, TinRendererDefinition>();
      t_dict.Add(SurfaceRendererTarget.Edges, edgeRD);
      t_dict.Add(SurfaceRendererTarget.Surface, faceRD);
      t_dict.Add(SurfaceRendererTarget.DirtyArea, dirtyAreaRD);

      // assign dictionary to creation params
      terrainCP_renderers.RendererDefinitions = t_dict;

      //Create the layer to the terrain
      var terrainLayer_rd = LayerFactory.Instance.CreateLayer<TerrainLayer>(terrainCP_renderers, map);
      #endregion


      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayerCreationParams
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayerCreationParams.#ctor(System.Uri)
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer
      #region Create a LasDatasetLayer
      //Must be on the QueuedTask.Run()

      string lasPath = @"d:\Data\LASDataset.lasd";
      var lasURI = new Uri(lasPath);

      var lasCP = new LasDatasetLayerCreationParams(lasURI);
      lasCP.Name = "My LAS Layer";
      lasCP.IsVisible = false;

      //Create the layer to the LAS dataset
      var lasDatasetLayer = LayerFactory.Instance.CreateLayer<LasDatasetLayer>(lasCP, map);

      #endregion

      ArcGIS.Core.Data.Analyst3D.LasDataset lasDataset = null;

      // cref: ArcGIS.Core.Data.Analyst3D.LasDataset
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayerCreationParams
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayerCreationParams.#ctor(ArcGIS.Core.Data.Analyst3D.LasDataset)
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer
      #region Create a LasDatasetLayer from a LasDataset
      //Must be on the QueuedTask.Run()

      var lasCP_ds = new LasDatasetLayerCreationParams(lasDataset);
      lasCP_ds.Name = "My LAS Layer";
      lasCP_ds.IsVisible = false;

      //Create the layer to the LAS dataset
      var lasDatasetLayer_ds = LayerFactory.Instance.CreateLayer<LasDatasetLayer>(lasCP_ds, map);

      #endregion


      // cref: ArcGIS.Core.Data.Analyst3D.LasDataset
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayerCreationParams
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayerCreationParams.#ctor(ArcGIS.Core.Data.Analyst3D.LasDataset)
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayerCreationParams.RendererDefinitions
      // cref: ArcGIS.Desktop.Mapping.TinRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      // cref: ArcGIS.Desktop.Mapping.LasStretchRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.LasStretchRendererDefinition.#ctor
      // cref: ArcGIS.Desktop.Mapping.TinEdgeRendererDefintion
      // cref: ArcGIS.Desktop.Mapping.TinEdgeRendererDefintion.#ctor
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer
      #region Create a LasDatasetLayer with renderers
      //Must be on the QueuedTask.Run()

      var lasCP_renderers = new LasDatasetLayerCreationParams(lasDataset);
      lasCP_renderers.Name = "My LAS Layer";
      lasCP_renderers.IsVisible = false;

      // create a point elevation renderer
      var ptR = new LasStretchRendererDefinition();
      // accept all defaults

      // create a simple edge renderer
      var edgeR = new TinEdgeRendererDefintion();
      // accept all defaults

      // add renderers to dictionary
      var l_dict = new Dictionary<SurfaceRendererTarget, TinRendererDefinition>();
      l_dict.Add(SurfaceRendererTarget.Points, ptR);
      l_dict.Add(SurfaceRendererTarget.Edges, edgeR);

      // assign dictionary to creation params
      lasCP_renderers.RendererDefinitions = l_dict;

      //Create the layer to the LAS dataset
      var lasDatasetLayer_rd = LayerFactory.Instance.CreateLayer<LasDatasetLayer>(lasCP_renderers, map);

      #endregion
    }

    #region ProSnippet Group: Renderers for TinLayer, TerrainLayer, LasDatasetLayer
    #endregion

    public void SurfaceLayerRenderers()
    {
      SurfaceLayer surfaceLayer = null;

      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.GetRenderers
      // cref: ArcGIS.Core.CIM.CIMTinRenderer
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.GetRenderersAsDictionary
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      #region Get Renderers
      // get the list of renderers
      IReadOnlyList<CIMTinRenderer> renderers = surfaceLayer.GetRenderers();

      // get the renderers as a dictionary
      Dictionary<SurfaceRendererTarget, CIMTinRenderer> dict = surfaceLayer.GetRenderersAsDictionary();
      #endregion

      CIMPointSymbol nodeSymbol = null;
      // cref: ArcGIS.Desktop.Mapping.TinNodeRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.TinNodeRendererDefinition.#ctor
      // cref: ArcGIS.Desktop.Mapping.TinSimpleRendererDefinition.Label
      // cref: ArcGIS.Desktop.Mapping.TinSimpleRendererDefinition.Description
      // cref: ArcGIS.Desktop.Mapping.TinSimpleRendererDefinition.SymbolTemplate
      // cref: ArcGIS.Core.CIM.CIMTinRenderer
      // cref: ArcGIS.Core.CIM.CIMTinNodeRenderer
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanCreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanSetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.SetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      // cref: ArcGIS.Desktop.Mapping.TinLayer
      #region Simple Node Renderer
      // applies to TIN layers only

      var nodeRendererDef = new TinNodeRendererDefinition();
      nodeRendererDef.Description = "Nodes";
      nodeRendererDef.Label = "Nodes";
      nodeRendererDef.SymbolTemplate = nodeSymbol.MakeSymbolReference();

      var tinLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<TinLayer>().FirstOrDefault();
      if (tinLayer == null)
        return;

      if (tinLayer.CanCreateRenderer(nodeRendererDef))
      {
        CIMTinRenderer renderer = tinLayer.CreateRenderer(nodeRendererDef);
        if (tinLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Points))
          tinLayer.SetRenderer(renderer, SurfaceRendererTarget.Points);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TinNodeClassBreaksRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.TinNodeClassBreaksRendererDefinition.#ctor
      // cref: ArcGIS.Desktop.Mapping.TinColorRampRendererDefinition.BreakCount
      // cref: ArcGIS.Core.CIM.CIMTinRenderer
      // cref: ArcGIS.Core.CIM.CIMTinNodeElevationRenderer
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanCreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanSetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.SetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      #region Elevation Node Renderer - Equal Breaks
      // applies to TIN layers only

      var equalBreaksNodeRendererDef = new TinNodeClassBreaksRendererDefinition();
      equalBreaksNodeRendererDef.BreakCount = 7;

      if (tinLayer.CanCreateRenderer(equalBreaksNodeRendererDef))
      {
        CIMTinRenderer renderer = tinLayer.CreateRenderer(equalBreaksNodeRendererDef);
        if (tinLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Edges))
          tinLayer.SetRenderer(renderer, SurfaceRendererTarget.Edges);
      }

      #endregion

      // cref: ArcGIS.Desktop.Mapping.TinNodeClassBreaksRendererDefinition
      // cref: ArcGIS.Core.CIM.ClassificationMethod
      // cref: ArcGIS.Core.CIM.CIMTinRenderer
      // cref: ArcGIS.Core.CIM.CIMTinNodeElevationRenderer
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanCreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanSetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.SetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      #region Elevation Node Renderer - Defined Interval
      // applies to TIN layers only

      var defiendIntervalNodeRendererDef = new TinNodeClassBreaksRendererDefinition();
      defiendIntervalNodeRendererDef.ClassificationMethod = ClassificationMethod.DefinedInterval;
      defiendIntervalNodeRendererDef.IntervalSize = 4;
      defiendIntervalNodeRendererDef.SymbolTemplate = nodeSymbol.MakeSymbolReference();

      if (tinLayer.CanCreateRenderer(defiendIntervalNodeRendererDef))
      {
        CIMTinRenderer renderer = tinLayer.CreateRenderer(defiendIntervalNodeRendererDef);
        if (tinLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Edges))
          tinLayer.SetRenderer(renderer, SurfaceRendererTarget.Edges);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TinNodeClassBreaksRendererDefinition
      // cref: ArcGIS.Core.CIM.ClassificationMethod
      // cref: ArcGIS.Desktop.Mapping.StandardDeviationInterval
      // cref: ArcGIS.Core.CIM.CIMTinRenderer
      // cref: ArcGIS.Core.CIM.CIMTinNodeElevationRenderer
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanCreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanSetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.SetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      #region Elevation Node Renderer - Standard Deviation
      // applies to TIN layers only

      var stdDevNodeRendererDef = new TinNodeClassBreaksRendererDefinition();
      stdDevNodeRendererDef.ClassificationMethod = ClassificationMethod.StandardDeviation;
      stdDevNodeRendererDef.DeviationInterval = StandardDeviationInterval.OneHalf;
      stdDevNodeRendererDef.ColorRamp = ColorFactory.Instance.GetColorRamp("Cyan to Purple");

      if (tinLayer.CanCreateRenderer(stdDevNodeRendererDef))
      {
        CIMTinRenderer renderer = tinLayer.CreateRenderer(stdDevNodeRendererDef);
        if (tinLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Edges))
          tinLayer.SetRenderer(renderer, SurfaceRendererTarget.Edges);
      }

      #endregion

      CIMLineSymbol lineSymbol = null;
      // cref: ArcGIS.Desktop.Mapping.TinEdgeRendererDefintion
      // cref: ArcGIS.Desktop.Mapping.TinEdgeRendererDefintion.#ctor
      // cref: ArcGIS.Desktop.Mapping.TinSimpleRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.TinSimpleRendererDefinition.Description
      // cref: ArcGIS.Desktop.Mapping.TinSimpleRendererDefinition.Label
      // cref: ArcGIS.Desktop.Mapping.TinSimpleRendererDefinition.SymbolTemplate
      // cref: ArcGIS.Core.CIM.CIMTinRenderer
      // cref: ArcGIS.Core.CIM.CIMTinEdgeRenderer
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanCreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanSetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.SetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      #region Simple Edge Renderer
      // applies to TIN or LAS dataset layers only

      var edgeRendererDef = new TinEdgeRendererDefintion();
      edgeRendererDef.Description = "Edges";
      edgeRendererDef.Label = "Edges";
      edgeRendererDef.SymbolTemplate = lineSymbol.MakeSymbolReference();

      if (surfaceLayer.CanCreateRenderer(edgeRendererDef))
      {
        CIMTinRenderer renderer = surfaceLayer.CreateRenderer(edgeRendererDef);
        if (surfaceLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Edges))
          surfaceLayer.SetRenderer(renderer, SurfaceRendererTarget.Edges);
      }
      #endregion

      CIMLineSymbol hardEdgeSymbol = null;
      CIMLineSymbol softEdgeSymbol = null;
      CIMLineSymbol outsideEdgeSymbol = null;
      // cref: ArcGIS.Desktop.Mapping.TinBreaklineRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.TinBreaklineRendererDefinition.#ctor
      // cref: ArcGIS.Desktop.Mapping.TinBreaklineRendererDefinition.HardEdgeSymbol
      // cref: ArcGIS.Desktop.Mapping.TinBreaklineRendererDefinition.SoftEdgeSymbol
      // cref: ArcGIS.Desktop.Mapping.TinBreaklineRendererDefinition.OutsideEdgeSymbol
      // cref: ArcGIS.Desktop.Mapping.TinBreaklineRendererDefinition.RegularEdgeSymbol
      // cref: ArcGIS.Core.CIM.CIMTinRenderer
      // cref: ArcGIS.Core.CIM.CIMTinBreaklineRenderer
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanCreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanSetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.SetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      #region Edge Type Renderer
      var breaklineRendererDef = new TinBreaklineRendererDefinition();
      // use default symbol for regular edge but specific symbols for hard,soft,outside
      breaklineRendererDef.HardEdgeSymbol = hardEdgeSymbol.MakeSymbolReference();
      breaklineRendererDef.SoftEdgeSymbol = softEdgeSymbol.MakeSymbolReference();
      breaklineRendererDef.OutsideEdgeSymbol = outsideEdgeSymbol.MakeSymbolReference();

      if (surfaceLayer.CanCreateRenderer(breaklineRendererDef))
      {
        CIMTinRenderer renderer = surfaceLayer.CreateRenderer(breaklineRendererDef);
        if (surfaceLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Edges))
          surfaceLayer.SetRenderer(renderer, SurfaceRendererTarget.Edges);
      }
      #endregion

      CIMLineSymbol contourLineSymbol = null;
      CIMLineSymbol indexLineSymbol = null;
      // cref: ArcGIS.Desktop.Mapping.TinContourRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.TinContourRendererDefinition.#ctor
      // cref: ArcGIS.Desktop.Mapping.TinContourRendererDefinition.Label
      // cref: ArcGIS.Desktop.Mapping.TinContourRendererDefinition.SymbolTemplate
      // cref: ArcGIS.Desktop.Mapping.TinContourRendererDefinition.ContourInterval
      // cref: ArcGIS.Desktop.Mapping.TinContourRendererDefinition.IndexLabel
      // cref: ArcGIS.Desktop.Mapping.TinContourRendererDefinition.IndexSymbolTemplate
      // cref: ArcGIS.Desktop.Mapping.TinContourRendererDefinition.ContourFactor
      // cref: ArcGIS.Desktop.Mapping.TinContourRendererDefinition.ReferenceHeight
      // cref: ArcGIS.Core.CIM.CIMTinRenderer
      // cref: ArcGIS.Core.CIM.CIMTinContourRenderer
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanCreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanSetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.SetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      #region Contour Renderer

      var contourDef = new TinContourRendererDefinition();

      // now customize with a symbol
      contourDef.Label = "Contours";
      contourDef.SymbolTemplate = contourLineSymbol.MakeSymbolReference();
      contourDef.ContourInterval = 6;

      contourDef.IndexLabel = "Index Contours";
      contourDef.IndexSymbolTemplate = indexLineSymbol.MakeSymbolReference();
      contourDef.ContourFactor = 4;
      contourDef.ReferenceHeight = 7;

      if (surfaceLayer.CanCreateRenderer(contourDef))
      {
        CIMTinRenderer renderer = surfaceLayer.CreateRenderer(contourDef);
        if (surfaceLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Contours))
          surfaceLayer.SetRenderer(renderer, SurfaceRendererTarget.Contours);
      }

      #endregion

      CIMPolygonSymbol polySymbol = null;
      // cref: ArcGIS.Desktop.Mapping.TinFaceRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.TinFaceRendererDefinition.#ctor
      // cref: ArcGIS.Desktop.Mapping.TinSimpleRendererDefinition.SymbolTemplate
      // cref: ArcGIS.Core.CIM.CIMTinRenderer
      // cref: ArcGIS.Core.CIM.CIMTinFaceRenderer 
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanCreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanSetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.SetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      #region Simple Face Renderer
      var simpleFaceRendererDef = new TinFaceRendererDefinition();
      simpleFaceRendererDef.SymbolTemplate = polySymbol.MakeSymbolReference();

      if (surfaceLayer.CanCreateRenderer(simpleFaceRendererDef))
      {
        CIMTinRenderer renderer = surfaceLayer.CreateRenderer(simpleFaceRendererDef);
        if (surfaceLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Surface))
          surfaceLayer.SetRenderer(renderer, SurfaceRendererTarget.Surface);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TinFaceClassBreaksAspectRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.TinFaceClassBreaksAspectRendererDefinition.#ctor
      // cref: ArcGIS.Desktop.Mapping.TinFaceClassBreaksAspectRendererDefinition.SymbolTemplate
      // cref: ArcGIS.Core.CIM.CIMTinRenderer
      // cref: ArcGIS.Core.CIM.CIMTinFaceClassBreaksRenderer 
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanCreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanSetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.SetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      #region Aspect Face Renderer 
      var aspectFaceRendererDef = new TinFaceClassBreaksAspectRendererDefinition();
      aspectFaceRendererDef.SymbolTemplate = polySymbol.MakeSymbolReference();
      // accept default color ramp

      if (surfaceLayer.CanCreateRenderer(aspectFaceRendererDef))
      {
        CIMTinRenderer renderer = surfaceLayer.CreateRenderer(aspectFaceRendererDef);
        if (surfaceLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Surface))
          surfaceLayer.SetRenderer(renderer, SurfaceRendererTarget.Surface);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TinFaceClassBreaksRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.TinFaceClassBreaksRendererDefinition.#ctor(ArcGIS.Core.CIM.TerrainDrawCursorType, ArcGIS.Core.CIM.ClassificationMethod, System.Int32, ArcGIS.Core.CIM.CIMSymbolReference, ArcGIS.Core.CIM.CIMColorRamp)
      // cref: ArcGIS.Core.CIM.CIMTinRenderer
      // cref: ArcGIS.Core.CIM.CIMTinFaceClassBreaksRenderer 
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanCreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanSetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.SetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      #region Slope Face Renderer - Equal Interval
      var slopeFaceClassBreaksEqual = new TinFaceClassBreaksRendererDefinition(TerrainDrawCursorType.FaceSlope);
      // accept default breakCount, symbolTemplate, color ramp

      if (surfaceLayer.CanCreateRenderer(slopeFaceClassBreaksEqual))
      {
        CIMTinRenderer renderer = surfaceLayer.CreateRenderer(slopeFaceClassBreaksEqual);
        if (surfaceLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Surface))
          surfaceLayer.SetRenderer(renderer, SurfaceRendererTarget.Surface);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TinFaceClassBreaksRendererDefinition
      // cref: ArcGIS.Core.CIM.TerrainDrawCursorType
      // cref: ArcGIS.Core.CIM.ClassificationMethod
      // cref: ArcGIS.Core.CIM.CIMTinRenderer
      // cref: ArcGIS.Core.CIM.CIMTinFaceClassBreaksRenderer 
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanCreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanSetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.SetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      #region Slope Face Renderer - Quantile
      var slopeFaceClassBreaksQuantile = new TinFaceClassBreaksRendererDefinition(TerrainDrawCursorType.FaceSlope);
      slopeFaceClassBreaksQuantile.ClassificationMethod = ClassificationMethod.Quantile;
      // accept default breakCount, symbolTemplate, color ramp

      if (surfaceLayer.CanCreateRenderer(slopeFaceClassBreaksQuantile))
      {
        CIMTinRenderer renderer = surfaceLayer.CreateRenderer(slopeFaceClassBreaksQuantile);
        if (surfaceLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Surface))
          surfaceLayer.SetRenderer(renderer, SurfaceRendererTarget.Surface);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TinFaceClassBreaksRendererDefinition
      // cref: ArcGIS.Core.CIM.CIMTinRenderer
      // cref: ArcGIS.Core.CIM.CIMTinFaceClassBreaksRenderer 
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanCreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanSetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.SetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      #region Elevation Face Renderer - Equal Interval

      var elevFaceClassBreaksEqual = new TinFaceClassBreaksRendererDefinition();
      // accept default breakCount, symbolTemplate, color ramp

      if (surfaceLayer.CanCreateRenderer(slopeFaceClassBreaksEqual))
      {
        CIMTinRenderer renderer = surfaceLayer.CreateRenderer(slopeFaceClassBreaksEqual);
        if (surfaceLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Surface))
          surfaceLayer.SetRenderer(renderer, SurfaceRendererTarget.Surface);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TerrainDirtyAreaRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.TerrainDirtyAreaRendererDefinition.#ctor
      // cref: ArcGIS.Core.CIM.CIMTinRenderer
      // cref: ArcGIS.Core.CIM.CIMTerrainDirtyAreaRenderer
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanCreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanSetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.SetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      // cref: ArcGIS.Desktop.Mapping.TerrainLayer
      #region Dirty Area Renderer 
      // applies to Terrain layers only

      var dirtyAreaRendererDef = new TerrainDirtyAreaRendererDefinition();
      // accept default labels, symbolTemplate

      var terrainLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<TerrainLayer>().FirstOrDefault();
      if (terrainLayer == null)
        return;

      if (terrainLayer.CanCreateRenderer(dirtyAreaRendererDef))
      {
        CIMTinRenderer renderer = terrainLayer.CreateRenderer(dirtyAreaRendererDef);
        if (terrainLayer.CanSetRenderer(renderer, SurfaceRendererTarget.DirtyArea))
          terrainLayer.SetRenderer(renderer, SurfaceRendererTarget.DirtyArea);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TerrainPointClassBreaksRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.TerrainPointClassBreaksRendererDefinition.#ctor
      // cref: ArcGIS.Core.CIM.CIMTinRenderer
      // cref: ArcGIS.Core.CIM.CIMTerrainPointElevationRenderer 
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanCreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanSetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.SetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      #region Terrain Point Class Breaks Renderer
      // applies to Terrain layers only

      var terrainPointClassBreaks = new TerrainPointClassBreaksRendererDefinition();
      // accept defaults

      if (terrainLayer.CanCreateRenderer(terrainPointClassBreaks))
      {
        CIMTinRenderer renderer = terrainLayer.CreateRenderer(terrainPointClassBreaks);
        if (terrainLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Points))
          terrainLayer.SetRenderer(renderer, SurfaceRendererTarget.Points);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.LasUniqueValueRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.LasUniqueValueRendererDefinition.#ctor(ArcGIS.Desktop.Mapping.LasAttributeType,System.Boolean,ArcGIS.Core.CIM.CIMSymbolReference,System.Double,ArcGIS.Core.CIM.CIMColorRamp)
      // cref: ArcGIS.Desktop.Mapping.LasAttributeType
      // cref: ArcGIS.Core.CIM.CIMTinRenderer
      // cref: ArcGIS.Core.CIM.CIMLASUniqueValueRenderer 
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanCreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanSetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.SetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer
      #region LAS Points Classification Unique Value Renderer
      // applies to LAS dataset layers only

      var lasPointsClassificationRendererDef = new LasUniqueValueRendererDefinition(LasAttributeType.Classification);
      // accept the defaults for color ramp, symbolTemplate, symbol scale factor

      var lasDatasetLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<LasDatasetLayer>().FirstOrDefault();
      if (lasDatasetLayer == null)
        return;

      if (lasDatasetLayer.CanCreateRenderer(lasPointsClassificationRendererDef))
      {
        CIMTinRenderer renderer = lasDatasetLayer.CreateRenderer(lasPointsClassificationRendererDef);
        if (lasDatasetLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Points))
          lasDatasetLayer.SetRenderer(renderer, SurfaceRendererTarget.Points);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.LasUniqueValueRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.LasUniqueValueRendererDefinition.#ctor(ArcGIS.Desktop.Mapping.LasAttributeType,System.Boolean,ArcGIS.Core.CIM.CIMSymbolReference,System.Double,ArcGIS.Core.CIM.CIMColorRamp)
      // cref: ArcGIS.Desktop.Mapping.LasAttributeType
      // cref: ArcGIS.Desktop.Mapping.LasUniqueValueRendererDefinition.ModulateUsingIntensity
      // cref: ArcGIS.Desktop.Mapping.LasUniqueValueRendererDefinition.SymbolScaleFactor
      // cref: ArcGIS.Core.CIM.CIMTinRenderer
      // cref: ArcGIS.Core.CIM.CIMLASUniqueValueRenderer 
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanCreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanSetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.SetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      #region LAS Points Returns Unique Value Renderer
      // applies to LAS dataset layers only

      var lasPointsReturnsRendererDef = new LasUniqueValueRendererDefinition(LasAttributeType.ReturnNumber);
      lasPointsReturnsRendererDef.ModulateUsingIntensity = true;
      lasPointsReturnsRendererDef.SymbolScaleFactor = 1.0;
      // accept the defaults for color ramp, symbolTemplate

      if (lasDatasetLayer.CanCreateRenderer(lasPointsReturnsRendererDef))
      {
        CIMTinRenderer renderer = lasDatasetLayer.CreateRenderer(lasPointsReturnsRendererDef);
        if (lasDatasetLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Points))
          lasDatasetLayer.SetRenderer(renderer, SurfaceRendererTarget.Points);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.LasStretchRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.LasStretchRendererDefinition.#ctor(ArcGIS.Core.CIM.LASStretchAttribute,ArcGIS.Core.CIM.LASStretchType,System.Double,ArcGIS.Core.CIM.CIMColorRamp)
      // cref: ArcGIS.Core.CIM.LASStretchAttribute
      // cref: ArcGIS.Core.CIM.CIMTinRenderer
      // cref: ArcGIS.Core.CIM.CIMLASStretchRenderer 
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanCreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanSetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.SetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      // cref: ArcGIS.Desktop.Mapping.LasStretchRendererDefinition.StretchType
      // cref: ArcGIS.Core.CIM.LASStretchType
      // cref: ArcGIS.Desktop.Mapping.LasStretchRendererDefinition.NumberOfStandardDeviations
      #region LAS Points Elevation Stretch Renderer
      // applies to LAS dataset layers only

      var elevLasStretchRendererDef = new LasStretchRendererDefinition(ArcGIS.Core.CIM.LASStretchAttribute.Elevation);
      // accept the defaults for color ramp, etc

      if (lasDatasetLayer.CanCreateRenderer(elevLasStretchRendererDef))
      {
        CIMTinRenderer renderer = lasDatasetLayer.CreateRenderer(elevLasStretchRendererDef);
        if (lasDatasetLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Points))
          lasDatasetLayer.SetRenderer(renderer, SurfaceRendererTarget.Points);
      }


      // OR use a stretch renderer with stretchType standard Deviations
      var elevLasStretchStdDevRendererDef = new LasStretchRendererDefinition(ArcGIS.Core.CIM.LASStretchAttribute.Elevation);
      elevLasStretchStdDevRendererDef.StretchType = LASStretchType.StandardDeviations;
      elevLasStretchStdDevRendererDef.NumberOfStandardDeviations = 2;
      // accept the defaults for color ramp,  etc

      if (lasDatasetLayer.CanCreateRenderer(elevLasStretchStdDevRendererDef))
      {
        CIMTinRenderer renderer = lasDatasetLayer.CreateRenderer(elevLasStretchStdDevRendererDef);
        if (lasDatasetLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Points))
          lasDatasetLayer.SetRenderer(renderer, SurfaceRendererTarget.Points);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.LasPointClassBreaksRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.LasPointClassBreaksRendererDefinition.#ctor
      // cref: ArcGIS.Desktop.Mapping.TinColorRampRendererDefinition.ClassificationMethod
      // cref: ArcGIS.Core.CIM.ClassificationMethod
      // cref: ArcGIS.Desktop.Mapping.LasPointClassBreaksRendererDefinition.ModulateUsingIntensity
      // cref: ArcGIS.Desktop.Mapping.LasPointClassBreaksRendererDefinition.SymbolScaleFactor
      // cref: ArcGIS.Core.CIM.CIMTinRenderer
      // cref: ArcGIS.Core.CIM.CIMLASPointElevationRenderer 
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanCreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanSetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CreateRenderer(ArcGIS.Desktop.Mapping.TinRendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.SetRenderer(ArcGIS.Core.CIM.CIMTinRenderer, ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
      #region LAS Points Classified Elevation Renderer
      // applies to LAS dataset layers only

      var lasPointsClassBreaksRendererDef = new LasPointClassBreaksRendererDefinition();
      lasPointsClassBreaksRendererDef.ClassificationMethod = ClassificationMethod.NaturalBreaks;
      lasPointsClassBreaksRendererDef.ModulateUsingIntensity = true;
      // increase the symbol size by a factor
      lasPointsClassBreaksRendererDef.SymbolScaleFactor = 1.0;

      if (lasDatasetLayer.CanCreateRenderer(lasPointsClassBreaksRendererDef))
      {
        CIMTinRenderer renderer = lasDatasetLayer.CreateRenderer(lasPointsClassBreaksRendererDef);
        if (lasDatasetLayer.CanSetRenderer(renderer, SurfaceRendererTarget.Points))
          lasDatasetLayer.SetRenderer(renderer, SurfaceRendererTarget.Points);
      }
      #endregion

      {
        // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.RemoveRenderer(ArcGIS.Desktop.Mapping.SurfaceRendererTarget)
        // cref: ArcGIS.Desktop.Mapping.SurfaceRendererTarget
        #region Remove an edge renderer
        var layer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<SurfaceLayer>().FirstOrDefault();
        if (layer == null)
          return;

        QueuedTask.Run(() =>
        {
          layer.RemoveRenderer(SurfaceRendererTarget.Edges);
        });

        #endregion
      }
    }


    #region ProSnippet Group: TIN Layer Searching
    #endregion

    public void TinLayer_Search()
    {
      TinLayer tinLayer = null;
      Envelope envelope = null;
      ArcGIS.Core.Data.Analyst3D.TinDataset tinDataset = null;

      // cref: ArcGIS.Desktop.Mapping.TinLayer.SearchNodes
      // cref: ArcGIS.Core.Data.Analyst3D.TinNodeCursor
      // cref: ArcGIS.Core.Data.Analyst3D.TinNode
      // cref: ArcGIS.Core.Data.Analyst3D.TinNodeFilter
      // cref: ArcGIS.Core.Data.Analyst3D.TinNodeFilter.#ctor
      // cref: ArcGIS.Core.Data.Analyst3D.TinFilter.FilterEnvelope
      // cref: ArcGIS.Core.Data.Analyst3D.TinFilter.DataElementsOnly
      // cref: ArcGIS.Core.Data.Analyst3D.TinNodeFilter.SuperNode
      // cref: ArcGIS.Desktop.Mapping.TinLayer.SearchEdges
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdgeCursor
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdge
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdgeFilter
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdgeFilter.#ctor
      // cref: ArcGIS.Core.Data.Analyst3D.TinFilter.FilterEnvelope
      // cref: ArcGIS.Core.Data.Analyst3D.TinFilter.DataElementsOnly
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdgeFilter.FilterByEdgeType
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdgeFilter.EdgeType
      // cref: ArcGIS.Desktop.Mapping.TinLayer.SearchTriangles
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangleCursor
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangle
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangleFilter
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangleFilter.#ctor
      // cref: ArcGIS.Core.Data.Analyst3D.TinFilter.FilterEnvelope
      // cref: ArcGIS.Core.Data.Analyst3D.TinFilter.DataElementsOnly
      #region Seach for TIN Nodes, Edges, Triangles
      // search all "inside" nodes
      using (ArcGIS.Core.Data.Analyst3D.TinNodeCursor nodeCursor = tinLayer.SearchNodes(null))
      {
        while (nodeCursor.MoveNext())
        {
          using (ArcGIS.Core.Data.Analyst3D.TinNode node = nodeCursor.Current)
          {

          }
        }
      }

      // search "inside" nodes with an extent
      ArcGIS.Core.Data.Analyst3D.TinNodeFilter nodeFilter = new ArcGIS.Core.Data.Analyst3D.TinNodeFilter();
      nodeFilter.FilterEnvelope = envelope;
      using (ArcGIS.Core.Data.Analyst3D.TinNodeCursor nodeCursor = tinLayer.SearchNodes(nodeFilter))
      {
        while (nodeCursor.MoveNext())
        {
          using (ArcGIS.Core.Data.Analyst3D.TinNode node = nodeCursor.Current)
          {

          }
        }
      }

      // search for super nodes only
      var supernodeFilter = new ArcGIS.Core.Data.Analyst3D.TinNodeFilter();
      supernodeFilter.FilterEnvelope = tinDataset.GetSuperNodeExtent();
      supernodeFilter.DataElementsOnly = false;
      supernodeFilter.SuperNode = true;
      using (ArcGIS.Core.Data.Analyst3D.TinNodeCursor nodeCursor = tinLayer.SearchNodes(nodeFilter))
      {
        while (nodeCursor.MoveNext())
        {
          using (ArcGIS.Core.Data.Analyst3D.TinNode node = nodeCursor.Current)
          {

          }
        }
      }


      // search all edges within an extent
      //    this could include outside or edges attached to super nodes depending upon the extent
      ArcGIS.Core.Data.Analyst3D.TinEdgeFilter edgeFilterAll = new ArcGIS.Core.Data.Analyst3D.TinEdgeFilter();
      edgeFilterAll.FilterEnvelope = envelope;
      edgeFilterAll.DataElementsOnly = false;
      using (ArcGIS.Core.Data.Analyst3D.TinEdgeCursor edgeCursor = tinLayer.SearchEdges(edgeFilterAll))
      {
        while (edgeCursor.MoveNext())
        {
          using (ArcGIS.Core.Data.Analyst3D.TinEdge edge = edgeCursor.Current)
          {
          }
        }
      }


      // search for hard edges in the TIN
      var edgeFilter = new ArcGIS.Core.Data.Analyst3D.TinEdgeFilter();
      edgeFilter.FilterByEdgeType = true;
      edgeFilter.EdgeType = ArcGIS.Core.Data.Analyst3D.TinEdgeType.HardEdge;
      using (ArcGIS.Core.Data.Analyst3D.TinEdgeCursor edgeCursor = tinLayer.SearchEdges(edgeFilter))
      {
        while (edgeCursor.MoveNext())
        {
          using (ArcGIS.Core.Data.Analyst3D.TinEdge edge = edgeCursor.Current)
          {

          }
        }
      }


      // search for "inside" triangles in an extent
      ArcGIS.Core.Data.Analyst3D.TinTriangleFilter triangleFilter = new ArcGIS.Core.Data.Analyst3D.TinTriangleFilter();
      triangleFilter.FilterEnvelope = envelope;
      triangleFilter.DataElementsOnly = true;
      using (ArcGIS.Core.Data.Analyst3D.TinTriangleCursor triangleCursor = tinLayer.SearchTriangles(triangleFilter))
      {
        while (triangleCursor.MoveNext())
        {
          using (ArcGIS.Core.Data.Analyst3D.TinTriangle triangle = triangleCursor.Current)
          {
          }
        }
      }
      #endregion

    }


    #region ProSnippet Group: LAS Dataset Layer Display Filter
    #endregion

    public void SetLasDisplayFilter()
    {
      LasDatasetLayer lasDatasetLayer = null;

      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.GetDisplayFilter
      // cref: ArcGIS.Desktop.Mapping.LasPointDisplayFilter
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.SetDisplayFilter(ArcGIS.Desktop.Mapping.LasPointDisplayFilterType)
      // cref: ArcGIS.Desktop.Mapping.LasPointDisplayFilterType
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.SetDisplayFilter(List<System.Int32>)
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.SetDisplayFilter(List<ArcGIS.Core.Data.Analyst3D.LasReturnType>)
      // cref: ArcGIS.Desktop.Mapping.LasPointDisplayFilter.#ctor
      // cref: ArcGIS.Desktop.Mapping.LasPointDisplayFilter.Returns
      // cref: ArcGIS.Desktop.Mapping.LasPointDisplayFilter.ClassCodes
      // cref: ArcGIS.Desktop.Mapping.LasPointDisplayFilter.KeyPoints
      // cref: ArcGIS.Desktop.Mapping.LasPointDisplayFilter.SyntheticPoints
      // cref: ArcGIS.Desktop.Mapping.LasPointDisplayFilter.NotFlagged
      // cref: ArcGIS.Desktop.Mapping.LasPointDisplayFilter.WithheldPoints
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.SetDisplayFilter(ArcGIS.Desktop.Mapping.LasPointDisplayFilter)
      #region Get and Set Display Filter

      // get the current display filter
      LasPointDisplayFilter ptFilter = lasDatasetLayer.GetDisplayFilter();


      // display only ground points
      lasDatasetLayer.SetDisplayFilter(LasPointDisplayFilterType.Ground);

      // display first return points
      lasDatasetLayer.SetDisplayFilter(LasPointDisplayFilterType.FirstReturnPoints);

      // set display filter to a set of classification codes
      List<int> classifications = new List<int>() { 4, 5, 7, 10 };
      lasDatasetLayer.SetDisplayFilter(classifications);

      // set display filter to a set of returns
      List<ArcGIS.Core.Data.Analyst3D.LasReturnType> returns = new List<ArcGIS.Core.Data.Analyst3D.LasReturnType>()
              { ArcGIS.Core.Data.Analyst3D.LasReturnType.ReturnFirstOfMany};
      lasDatasetLayer.SetDisplayFilter(returns);

      // set up a display filter
      var newDisplayFilter = new LasPointDisplayFilter();
      newDisplayFilter.Returns = new List<ArcGIS.Core.Data.Analyst3D.LasReturnType>()
              { ArcGIS.Core.Data.Analyst3D.LasReturnType.ReturnFirstOfMany, ArcGIS.Core.Data.Analyst3D.LasReturnType.ReturnLastOfMany};
      newDisplayFilter.ClassCodes = new List<int>() { 2, 4 };
      newDisplayFilter.KeyPoints = true;
      newDisplayFilter.WithheldPoints = false;
      newDisplayFilter.SyntheticPoints = false;
      newDisplayFilter.NotFlagged = false;
      lasDatasetLayer.SetDisplayFilter(returns);

      #endregion

      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.GetActiveSurfaceConstraints 
      // cref: ArcGIS.Core.Data.Analyst3D.SurfaceConstraint
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.SetActiveSurfaceConstraints(List<System.String>) 
      #region Active Surface Constraints
      var activeSurfaceConstraints = lasDatasetLayer.GetActiveSurfaceConstraints();

      // clear all surface constraints (i.e. none are active)
      lasDatasetLayer.SetActiveSurfaceConstraints(null);

      // set all surface constraints active
      using (var lasDataset = lasDatasetLayer.GetLasDataset())
      {
        var surfaceConstraints = lasDataset.GetSurfaceConstraints();
        var names = surfaceConstraints.Select(sc => sc.DataSourceName).ToList();
        lasDatasetLayer.SetActiveSurfaceConstraints(names);
      }
      #endregion
    }

    #region ProSnippet Group: LAS Dataset Layer Searching
    #endregion

    public void LasDatasetLayer_Search()
    {
      LasDatasetLayer lasDatasetLayer = null;
      Envelope envelope = null;

      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.SearchPoints(LasPointFilter)
      // cref: ArcGIS.Core.Data.Analyst3D.LasPointFilter
      // cref: ArcGIS.Core.Data.Analyst3D.LasPointFilter.#ctor
      // cref: ArcGIS.Core.Data.Analyst3D.LasPointFilter.FilterGeometry
      // cref: ArcGIS.Core.Data.Analyst3D.LasPointFilter.ClassCodes
      // cref: ArcGIS.Core.Data.Analyst3D.LasPointCursor
      // cref: ArcGIS.Core.Data.Analyst3D.LasPointCursor.MoveNext
      // cref: ArcGIS.Core.Data.Analyst3D.LasPointCursor.Current
      // cref: ArcGIS.Core.Data.Analyst3D.LasPoint
      #region Search for LAS Points

      // searching on the LasDatasetLayer will honor any LasPointDisplayFilter

      // search all points
      using (ArcGIS.Core.Data.Analyst3D.LasPointCursor ptCursor = lasDatasetLayer.SearchPoints(null))
      {
        while (ptCursor.MoveNext())
        {
          using (ArcGIS.Core.Data.Analyst3D.LasPoint point = ptCursor.Current)
          {

          }
        }
      }

      // search within an extent
      ArcGIS.Core.Data.Analyst3D.LasPointFilter pointFilter = new ArcGIS.Core.Data.Analyst3D.LasPointFilter();
      pointFilter.FilterGeometry = envelope;
      using (ArcGIS.Core.Data.Analyst3D.LasPointCursor ptCursor = lasDatasetLayer.SearchPoints(pointFilter))
      {
        while (ptCursor.MoveNext())
        {
          using (ArcGIS.Core.Data.Analyst3D.LasPoint point = ptCursor.Current)
          {

          }
        }
      }

      // search within an extent and limited to specific classsification codes
      pointFilter = new ArcGIS.Core.Data.Analyst3D.LasPointFilter();
      pointFilter.FilterGeometry = envelope;
      pointFilter.ClassCodes = new List<int> { 4, 5 };
      using (ArcGIS.Core.Data.Analyst3D.LasPointCursor ptCursor = lasDatasetLayer.SearchPoints(pointFilter))
      {
        while (ptCursor.MoveNext())
        {
          using (ArcGIS.Core.Data.Analyst3D.LasPoint point = ptCursor.Current)
          {

          }
        }
      }

      #endregion 
    }

    #region ProSnippet Group: LAS Dataset Layer Eye Dome Lighting
    #endregion

    public void LasDatasetLayer_EDL()
    {
      LasDatasetLayer lasDatasetLayer = null;


      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.IsEyeDomeLightingEnabled
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.EyeDomeLightingRadius
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.EyeDomeLightingStrength
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.SetEyeDomeLightingEnabled(System.Boolean)
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.SetEyeDomeLightingStrength(System.Double)
      // cref: ArcGIS.Desktop.Mapping.LasDatasetLayer.SetEyeDomeLightingRadius(System.Double)
      #region Eye Dome Lighting

      // get current EDL settings
      bool isEnabled = lasDatasetLayer.IsEyeDomeLightingEnabled;
      var radius = lasDatasetLayer.EyeDomeLightingRadius;
      var strength = lasDatasetLayer.EyeDomeLightingStrength;

      // set EDL values
      lasDatasetLayer.SetEyeDomeLightingEnabled(true);
      lasDatasetLayer.SetEyeDomeLightingStrength(65.0);
      lasDatasetLayer.SetEyeDomeLightingRadius(2.0);

      #endregion

    }

    #region ProSnippet Group: Line of Sight
    #endregion
    public void GetLineOfSight()
    {
      TinLayer tinLayer = null;
      MapPoint observerPoint = null;
      MapPoint targetPoint = null;
      CIMPointSymbol obstructionPointSymbol = null;
      CIMLineSymbol visibleLineSymbol = null;
      CIMLineSymbol invisibleLineSymbol = null;

      // cref: ArcGIS.Desktop.Mapping.LineOfSightParams
      // cref: ArcGIS.Desktop.Mapping.LineOfSightParams.#ctor
      // cref: ArcGIS.Desktop.Mapping.LineOfSightParams.ObserverPoint
      // cref: ArcGIS.Desktop.Mapping.LineOfSightParams.TargetPoint
      // cref: ArcGIS.Desktop.Mapping.LineOfSightParams.ObserverHeightOffset
      // cref: ArcGIS.Desktop.Mapping.LineOfSightParams.TargetHeightOffset
      // cref: ArcGIS.Desktop.Mapping.LineOfSightParams.OutputSpatialReference
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.CanGetLineOfSight(ArcGIS.Desktop.Mapping.LineOfSightParams)
      // cref: ArcGIS.Desktop.Mapping.SurfaceLayer.GetLineOfSight(ArcGIS.Desktop.Mapping.LineOfSightParams)
      // cref: ArcGIS.Desktop.Mapping.LineOfSightResult
      // cref: ArcGIS.Desktop.Mapping.LineOfSightResult.IsTargetVisibleFromObserverPoint
      // cref: ArcGIS.Desktop.Mapping.LineOfSightResult.VisibleLine
      // cref: ArcGIS.Desktop.Mapping.LineOfSightResult.InvisibleLine
      // cref: ArcGIS.Desktop.Mapping.LineOfSightResult.ObstructionPoint
      // cref: ArcGIS.Desktop.Mapping.LineOfSightResult.IsTargetVisibleFromVisibleLine
      // cref: ArcGIS.Desktop.Mapping.LineOfSightResult.IsTargetVisibleFromInvisibleLine
      #region Get Line of Sight
      var losParams = new LineOfSightParams();
      losParams.ObserverPoint = observerPoint;
      losParams.TargetPoint = targetPoint;

      // add offsets if appropriate
      // losParams.ObserverHeightOffset = observerOffset;
      // losParams.TargetHeightOffset = targerOffset;

      // set output spatial reference
      losParams.OutputSpatialReference = MapView.Active.Map.SpatialReference;

      LineOfSightResult results = null;
      try
      {
        if (tinLayer.CanGetLineOfSight(losParams))
          results = tinLayer.GetLineOfSight(losParams);
      }
      catch (Exception ex)
      {
        // log exception message
      }

      if (results != null)
      {
        bool targetIsVisibleFromObserverPoint = results.IsTargetVisibleFromObserverPoint;
        bool targetVisibleFromVisibleLine = results.IsTargetVisibleFromVisibleLine;
        bool targetVisibleFromInVisibleLine = results.IsTargetVisibleFromInvisibleLine;


        if (results.VisibleLine != null)
          MapView.Active.AddOverlay(results.VisibleLine, visibleLineSymbol.MakeSymbolReference());
        if (results.InvisibleLine != null)
          MapView.Active.AddOverlay(results.VisibleLine, invisibleLineSymbol.MakeSymbolReference());
        if (results.ObstructionPoint != null)
          MapView.Active.AddOverlay(results.ObstructionPoint, obstructionPointSymbol.MakeSymbolReference());
      }
      #endregion

    }
  }
}



