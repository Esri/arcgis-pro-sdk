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
using ArcGIS.Core.Data.Exceptions;
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
using System.Windows.Input;

namespace GeodatabaseSDK._3DAnalystData
{
  internal class ProSnippets3DAnalystData
  {

    #region ProSnippet Group: TIN
    #endregion

    // cref: ArcGIS.Core.Data.FileSystemDatastoreType.Tin
    // cref: ArcGIS.Core.Data.Analyst3D.TinDataset
    // cref: ArcGIS.Core.Data.FileSystemDatastore.#ctor(ArcGIS.Core.Data.FileSystemConnectionPath)
    // cref: ArcGIS.Core.Data.FileSystemDatastore.OpenDataset``1(System.String)
    #region Open a TIN Dataset

    public async Task OpenTinDataset()
    {
      try
      {
        await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          string path = @"d:\Data\Tin";
          var fileConnection = new FileSystemConnectionPath(new Uri(path), FileSystemDatastoreType.Tin);

          using (FileSystemDatastore dataStore = new FileSystemDatastore(fileConnection))
          {
            // TIN is in a folder at d:\Data\Tin\TinDataset

            string dsName = "TinDataset";

            using (var dataset = dataStore.OpenDataset<ArcGIS.Core.Data.Analyst3D.TinDataset>(dsName))
            {

            }
          }
        });
      }
      catch (GeodatabaseNotFoundOrOpenedException exception)
      {
        // Handle Exception.
      }
    }
    #endregion

    // cref: ArcGIS.Core.Data.FileSystemDatastoreType.Tin
    // cref: ArcGIS.Core.Data.Analyst3D.TinDatasetDefinition
    // cref: ArcGIS.Core.Data.FileSystemDatastore.#ctor(ArcGIS.Core.Data.FileSystemConnectionPath)
    // cref: ArcGIS.Core.Data.FileSystemDatastore.GetDefinition``1(System.String)
    #region Get a TIN Defintion
    public async Task GetTinDatasetDefinition()
    {
      try
      {
        await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          string path = @"d:\Data\Tin";
          var fileConnection = new FileSystemConnectionPath(new Uri(path), FileSystemDatastoreType.Tin);

          using (FileSystemDatastore dataStore = new FileSystemDatastore(fileConnection))
          {
            // TIN is in a folder at d:\Data\Tin\TinDataset

            string dsName = "TinDataset";

            using (var def = dataStore.GetDefinition<ArcGIS.Core.Data.Analyst3D.TinDatasetDefinition>(dsName))
            {

            }
          }
        });
      }
      catch (GeodatabaseNotFoundOrOpenedException exception)
      {
        // Handle Exception.
      }
    }
    #endregion

    public void TinProperties(ArcGIS.Core.Data.Analyst3D.TinDataset tinDataset)
    {
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.GetSuperNodeExtent
      #region Get Super Node Extent
      var superNodeExtent = tinDataset.GetSuperNodeExtent();

      #endregion

      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.GetDataArea
      #region Get Data Area
      var dataArea = tinDataset.GetDataArea();
      #endregion

      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.GetNodeCount
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.GetOutsideNodeCount
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.GetEdgeCount
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.GetOutsideEdgeCount
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.GetTriangleCount
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.GetOutsideTriangleCount
      #region Element Counts

      var nodeCount = tinDataset.GetNodeCount();
      var outsideNodeCount = tinDataset.GetOutsideNodeCount();
      var edgeCount = tinDataset.GetEdgeCount();
      var outsideEdgecount = tinDataset.GetOutsideEdgeCount();
      var triCount = tinDataset.GetTriangleCount();
      var outsideTriCount = tinDataset.GetOutsideTriangleCount();

      #endregion

      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.GetIsEmpty
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.HasHardEdges
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.HasSoftEdges
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.UsesConstrainedDelaunay
      #region Tin Properties

      var isEmpty = tinDataset.GetIsEmpty();
      var hasHardEdges = tinDataset.HasHardEdges();
      var hasSoftEdges = tinDataset.HasSoftEdges();

      var isConstrainedDelaunay = tinDataset.UsesConstrainedDelaunay();

      #endregion

      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.GetNodeByIndex(System.Int32)
      // cref: ArcGIS.Core.Data.Analyst3D.TinNode
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.GetEdgeByIndex(System.Int32)
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdge
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.GetTriangleByIndex(System.Int32)
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangle
      #region Access TIN Elements By Index

      using (ArcGIS.Core.Data.Analyst3D.TinNode node = tinDataset.GetNodeByIndex(23))
      {

      }

      using (ArcGIS.Core.Data.Analyst3D.TinEdge edge = tinDataset.GetEdgeByIndex(45))
      {

      }
      using (ArcGIS.Core.Data.Analyst3D.TinTriangle triangle = tinDataset.GetTriangleByIndex(22))
      {

      }

      #endregion

      Envelope envelope = null;
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.SearchNodes(ArcGIS.Core.Data.Analyst3D.TinNodeFilter)
      // cref: ArcGIS.Core.Data.Analyst3D.TinNodeCursor
      // cref: ArcGIS.Core.Data.Analyst3D.TinNodeCursor.MoveNext
      // cref: ArcGIS.Core.Data.Analyst3D.TinNodeCursor.Current
      // cref: ArcGIS.Core.Data.Analyst3D.TinNode
      // cref: ArcGIS.Core.Data.Analyst3D.TinNodeFilter
      // cref: ArcGIS.Core.Data.Analyst3D.TinNodeFilter.#ctor
      // cref: ArcGIS.Core.Data.Analyst3D.TinNodeFilter.SuperNode
      // cref: ArcGIS.Core.Data.Analyst3D.TinFilter.FilterType
      // cref: ArcGIS.Core.Data.Analyst3D.TinFilter.FilterEnvelope
      // cref: ArcGIS.Core.Data.Analyst3D.TinFilterType
      #region Search for TIN Nodes

      // search all nodes
      using (ArcGIS.Core.Data.Analyst3D.TinNodeCursor nodeCursor = tinDataset.SearchNodes(null))
      {
        while (nodeCursor.MoveNext())
        {
          using (ArcGIS.Core.Data.Analyst3D.TinNode node = nodeCursor.Current)
          {

          }
        }
      }

      // search within an extent
      ArcGIS.Core.Data.Analyst3D.TinNodeFilter nodeFilter = new ArcGIS.Core.Data.Analyst3D.TinNodeFilter();
      nodeFilter.FilterEnvelope = envelope;
      using (ArcGIS.Core.Data.Analyst3D.TinNodeCursor nodeCursor = tinDataset.SearchNodes(nodeFilter))
      {
        while (nodeCursor.MoveNext())
        {
          using (ArcGIS.Core.Data.Analyst3D.TinNode node = nodeCursor.Current)
          {

          }
        }
      }

      // search all "inside" nodes
      nodeFilter = new ArcGIS.Core.Data.Analyst3D.TinNodeFilter();
      nodeFilter.FilterType = ArcGIS.Core.Data.Analyst3D.TinFilterType.InsideDataArea;
      using (ArcGIS.Core.Data.Analyst3D.TinNodeCursor nodeCursor = tinDataset.SearchNodes(nodeFilter))
      {
        while (nodeCursor.MoveNext())
        {
          using (ArcGIS.Core.Data.Analyst3D.TinNode node = nodeCursor.Current)
          {

          }
        }
      }

      // search for super nodes only
      nodeFilter = new ArcGIS.Core.Data.Analyst3D.TinNodeFilter();
      nodeFilter.FilterEnvelope = tinDataset.GetSuperNodeExtent();
      nodeFilter.SuperNode = true;
      using (ArcGIS.Core.Data.Analyst3D.TinNodeCursor nodeCursor = tinDataset.SearchNodes(nodeFilter))
      {
        while (nodeCursor.MoveNext())
        {
          using (ArcGIS.Core.Data.Analyst3D.TinNode node = nodeCursor.Current)
          {

          }
        }
      }

      #endregion

      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.SearchEdges(ArcGIS.Core.Data.Analyst3D.TinEdgeFilter)
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdgeCursor
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdge
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdgeFilter
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdgeFilter.#ctor
      // cref: ArcGIS.Core.Data.Analyst3D.TinFilter.FilterType
      // cref: ArcGIS.Core.Data.Analyst3D.TinFilter.FilterEnvelope
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdgeFilter.FilterByEdgeType
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdgeFilter.EdgeType
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdgeType
      // cref: ArcGIS.Core.Data.Analyst3D.TinFilterType
      #region Search for TIN Edges

      // search all edges
      using (ArcGIS.Core.Data.Analyst3D.TinEdgeCursor edgeCursor = tinDataset.SearchEdges(null))
      {
        while (edgeCursor.MoveNext())
        {
          using (ArcGIS.Core.Data.Analyst3D.TinEdge edge = edgeCursor.Current)
          {

          }
        }
      }

      // search within an extent
      ArcGIS.Core.Data.Analyst3D.TinEdgeFilter edgeFilter = new ArcGIS.Core.Data.Analyst3D.TinEdgeFilter();
      edgeFilter.FilterEnvelope = envelope;
      using (ArcGIS.Core.Data.Analyst3D.TinEdgeCursor edgeCursor = tinDataset.SearchEdges(edgeFilter))
      {
        while (edgeCursor.MoveNext())
        {
          using (ArcGIS.Core.Data.Analyst3D.TinEdge edge = edgeCursor.Current)
          {

          }
        }
      }

      // search all "inside" edges
      edgeFilter = new ArcGIS.Core.Data.Analyst3D.TinEdgeFilter();
      edgeFilter.FilterType = ArcGIS.Core.Data.Analyst3D.TinFilterType.InsideDataArea;
      using (ArcGIS.Core.Data.Analyst3D.TinEdgeCursor edgeCursor = tinDataset.SearchEdges(edgeFilter))
      {
        while (edgeCursor.MoveNext())
        {
          using (ArcGIS.Core.Data.Analyst3D.TinEdge edge = edgeCursor.Current)
          {

          }
        }
      }

      // search for hard edges
      edgeFilter = new ArcGIS.Core.Data.Analyst3D.TinEdgeFilter();
      edgeFilter.FilterByEdgeType = true;
      edgeFilter.EdgeType = ArcGIS.Core.Data.Analyst3D.TinEdgeType.HardEdge;
      using (ArcGIS.Core.Data.Analyst3D.TinEdgeCursor edgeCursor = tinDataset.SearchEdges(edgeFilter))
      {
        while (edgeCursor.MoveNext())
        {
          using (ArcGIS.Core.Data.Analyst3D.TinEdge edge = edgeCursor.Current)
          {

          }
        }
      }
      #endregion

      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.SearchTriangles(ArcGIS.Core.Data.Analyst3D.TinTriangleFilter)
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangleCursor
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangle
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangleFilter
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangleFilter.#ctor
      // cref: ArcGIS.Core.Data.Analyst3D.TinFilter.FilterType
      // cref: ArcGIS.Core.Data.Analyst3D.TinFilter.FilterEnvelope
      // cref: ArcGIS.Core.Data.Analyst3D.TinFilterType
      #region Search for TIN Triangles

      // search all triangles
      using (ArcGIS.Core.Data.Analyst3D.TinTriangleCursor triangleCursor = tinDataset.SearchTriangles(null))
      {
        while (triangleCursor.MoveNext())
        {
          using (ArcGIS.Core.Data.Analyst3D.TinTriangle triangle = triangleCursor.Current)
          {

          }
        }
      }

      // search within an extent
      ArcGIS.Core.Data.Analyst3D.TinTriangleFilter triangleFilter = new ArcGIS.Core.Data.Analyst3D.TinTriangleFilter();
      triangleFilter.FilterEnvelope = envelope;
      using (ArcGIS.Core.Data.Analyst3D.TinTriangleCursor triangleCursor = tinDataset.SearchTriangles(triangleFilter))
      {
        while (triangleCursor.MoveNext())
        {
          using (ArcGIS.Core.Data.Analyst3D.TinTriangle triangle = triangleCursor.Current)
          {

          }
        }
      }

      // search all "inside" triangles
      triangleFilter = new ArcGIS.Core.Data.Analyst3D.TinTriangleFilter();
      triangleFilter.FilterType = ArcGIS.Core.Data.Analyst3D.TinFilterType.InsideDataArea;
      using (ArcGIS.Core.Data.Analyst3D.TinTriangleCursor triangleCursor = tinDataset.SearchTriangles(triangleFilter))
      {
        while (triangleCursor.MoveNext())
        {
          using (ArcGIS.Core.Data.Analyst3D.TinTriangle triangle = triangleCursor.Current)
          {

          }
        }
      }

      #endregion


      MapPoint mapPoint = null;
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.GetNearestNode(ArcGIS.Core.Geometry.MapPoint)
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.GetNearestEdge(ArcGIS.Core.Geometry.MapPoint)
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.GetTriangleByPoint(ArcGIS.Core.Geometry.MapPoint)
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.GetNaturalNeighbors(ArcGIS.Core.Geometry.MapPoint)
      // cref: ArcGIS.Core.Data.Analyst3D.TinDataset.GetTriangleNeighborhood(ArcGIS.Core.Geometry.MapPoint)
      #region Access TIN Elements by MapPoint

      // "identify" the closest node, edge, triangle
      using (var nearestNode = tinDataset.GetNearestNode(mapPoint))
      {
      }

      using (var nearestEdge = tinDataset.GetNearestEdge(mapPoint))
      {
      }
      using (var triangle = tinDataset.GetTriangleByPoint(mapPoint))
      {

      }

      // get the set of natural neighbours 
      // (set of nodes that "mapPoint" would connect with to form triangles if it was added to the TIN)
      IReadOnlyList<ArcGIS.Core.Data.Analyst3D.TinNode> naturalNeighbors = tinDataset.GetNaturalNeighbors(mapPoint);

      // get the set of triangles whose circumscribed circle contains "mapPoint" 
      IReadOnlyList<ArcGIS.Core.Data.Analyst3D.TinTriangle> triangles = tinDataset.GetTriangleNeighborhood(mapPoint);
      #endregion
    }

    public void TinElements(ArcGIS.Core.Data.Analyst3D.TinNode node, ArcGIS.Core.Data.Analyst3D.TinEdge edge, ArcGIS.Core.Data.Analyst3D.TinTriangle triangle)
    {
      // cref: ArcGIS.Core.Data.Analyst3D.TinNode
      // cref: ArcGIS.Core.Data.Analyst3D.TinNode.Coordinate3D
      // cref: ArcGIS.Core.Data.Analyst3D.TinNode.ToMapPoint
      // cref: ArcGIS.Core.Data.Analyst3D.TinNode.IsInsideDataArea
      // cref: ArcGIS.Core.Data.Analyst3D.TinNode.GetAdjacentNodes
      // cref: ArcGIS.Core.Data.Analyst3D.TinNode.GetIncidentEdges
      // cref: ArcGIS.Core.Data.Analyst3D.TinNode.GetIncidentTriangles
      #region TIN Nodes

      // node coordinates
      var coord3D = node.Coordinate3D;
      var mapPoint = node.ToMapPoint();
      // is the node "inside"
      var isInsideNode = node.IsInsideDataArea;

      // get all other nodes connected to "node" 
      IReadOnlyList<ArcGIS.Core.Data.Analyst3D.TinNode> adjNodes = node.GetAdjacentNodes();

      // get all edges that share "node" as a from node. 
      IReadOnlyList<ArcGIS.Core.Data.Analyst3D.TinEdge> edges = node.GetIncidentEdges();

      // get all triangles that share "node"
      IReadOnlyList<ArcGIS.Core.Data.Analyst3D.TinTriangle> triangles = node.GetIncidentTriangles();

      #endregion

      // cref: ArcGIS.Core.Data.Analyst3D.TinEdge
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdge.Nodes
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdge.ToPolyline
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdge.Length
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdge.IsInsideDataArea
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdge.EdgeType
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdgeType
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdge.GetNextEdgeInTriangle
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdge.GetPreviousEdgeInTriangle
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdge.GeNeighbor
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdge.LeftTriangle
      // cref: ArcGIS.Core.Data.Analyst3D.TinEdge.RightTriangle
      #region TIN Edges

      // nodes of the edge
      var nodes = edge.Nodes;

      // edge geometry
      var polyline = edge.ToPolyline();
      // edge length
      var length = edge.Length;
      // is the edge "inside"
      var isInsideEdge = edge.IsInsideDataArea;
      // edge type - regular/hard/soft
      var edgeType = edge.EdgeType;

      // get next (clockwise) edge in the triangle
      var nextEdge = edge.GetNextEdgeInTriangle();
      // get previous (anti-clockwise) edge in the triangle
      var prevEdge = edge.GetPreviousEdgeInTriangle();

      // get opposite edge
      var oppEdge = edge.GeNeighbor();

      // get left triangle
      var leftTriangle = edge.LeftTriangle;
      // get right triangle
      var rightTriangle = edge.RightTriangle;
      #endregion

      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangle
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangle.Nodes
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangle.Edges
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangle.ToPolygon
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangle.Length
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangle.Area
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangle.IsInsidedataArea
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangle.Aspect
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangle.Slope
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangle.GetCentroid
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangle.GetNormal
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangle.GetAdjacentTriangles
      // cref: ArcGIS.Core.Data.Analyst3D.TinTriangle.GetPointsBetweenZs
      #region TIN Triangles
      // nodes, edges of the triangle
      var triNnodes = triangle.Nodes;
      var triEdges = triangle.Edges;

      // triangle geometry
      var polygon = triangle.ToPolygon();
      // triangle length
      var triLength = triangle.Length;
      // triangle area 
      var triArea = triangle.Area;
      // is the triangle "inside"
      var isInsideTriangle = triangle.IsInsideDataArea;

      // triangle aspect and slope  (radians)
      var aspect = triangle.Aspect;
      var slope = triangle.Slope;

      // get centroid
      var centroid = triangle.GetCentroid();

      // get normal
      var normal = triangle.GetNormal();

      // get adjacent triangles
      var adjTriangles = triangle.GetAdjacentTriangles();

      // get area of triangle that falls between the z values
      double minZ = 1.0;
      double maxZ = 3.0;
      IReadOnlyList<Coordinate3D> coords = triangle.GetPointsBetweenZs(minZ, maxZ);
      #endregion
    }

    #region ProSnippet Group: Terrain
    #endregion

    // cref: ArcGIS.Core.Data.Analyst3D.Terrain
    // cref: ArcGIS.Core.Data.Geodatabase.OpenDataset``1(System.String)
    #region Open a Terrain

    public async Task OpenTerrain()
    {
      try
      {
        await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          string path = @"d:\Data\Terrain\filegdb_Containing_A_Terrain.gdb";
          var fileConnection = new FileGeodatabaseConnectionPath(new Uri(path));

          using (Geodatabase dataStore = new Geodatabase(fileConnection))
          {
            string dsName = "nameOfTerrain";

            using (var dataset = dataStore.OpenDataset<ArcGIS.Core.Data.Analyst3D.Terrain>(dsName))
            {
            }
          }
        });
      }
      catch (GeodatabaseNotFoundOrOpenedException exception)
      {
        // Handle Exception.
      }
    }

    #endregion

    // cref: ArcGIS.Core.Data.Analyst3D.TerrainDefinition
    // cref: ArcGIS.Core.Data.Analyst3D.Terrain.GetDefinition
    // cref: ArcGIS.Core.Data.Analyst3D.TerrainDefinition.GetFeatureClassNames
    #region Get a Terrain Definition

    public async Task GetTerrainDefinition()
    {
      try
      {
        await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          string path = @"d:\Data\Terrain\filegdb_Containing_A_Terrain.gdb";
          var fileConnection = new FileGeodatabaseConnectionPath(new Uri(path));

          using (Geodatabase dataStore = new Geodatabase(fileConnection))
          {
            string dsName = "nameOfTerrain";

            using (var terrainDef = dataStore.GetDefinition<ArcGIS.Core.Data.Analyst3D.TerrainDefinition>(dsName))
            {
              // get the feature class names that are used in the terrain
              var fcNames = terrainDef.GetFeatureClassNames();
            }
          }
        });
      }
      catch (GeodatabaseNotFoundOrOpenedException exception)
      {
        // Handle Exception.
      }
    }

    #endregion

    public void TerrainProperties(ArcGIS.Core.Data.Analyst3D.Terrain terrain)
    {
      // cref: ArcGIS.Core.Data.Analyst3D.Terrain.GetDataSourceCount
      // cref: ArcGIS.Core.Data.Analyst3D.Terrain.GetDataSources
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainDataSource
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainDataSource.DataSourceName
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainDataSource.SurfaceType
      // cref: ArcGIS.Core.Data.Analyst3D.TinSurfaceType
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainDataSource.MinimumResolution
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainDataSource.MaximumResolution
      #region Get datasources from a Terrain
      var dsCount = terrain.GetDataSourceCount();
      IReadOnlyList<ArcGIS.Core.Data.Analyst3D.TerrainDataSource> dataSources = terrain.GetDataSources();
      foreach (var ds in dataSources)
      {
        var dsName = ds.DataSourceName;
        var surfaceType = ds.SurfaceType;
        var maxResolution = ds.MaximumResolution;
        var minResolution = ds.MinimumResolution;
      }

      #endregion

      // cref: ArcGIS.Core.Data.Analyst3D.Terrain.GetPyramidLevelCount
      // cref: ArcGIS.Core.Data.Analyst3D.Terrain.GetPyramidLevels
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainPyramidLevel
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainPyramidLevel.Resolution
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainPyramidLevel.MaximumScale
      #region Get Pyramid Level Information from a Terrain
      var levelCount = terrain.GetPyramidLevelCount();
      IReadOnlyList<ArcGIS.Core.Data.Analyst3D.TerrainPyramidLevel> pyramidLevels = terrain.GetPyramidLevels();
      foreach (var pyramidLevel in pyramidLevels)
      {
        var resolution = pyramidLevel.Resolution;
        var maxScale = pyramidLevel.MaximumScale;
      }
      #endregion

      // cref: ArcGIS.Core.Data.Analyst3D.Terrain.GetTileProperties
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainTileProperties 
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainTileProperties.ColumnCount
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainTileProperties.RowCount
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainTileProperties.TileSize
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainTileProperties.TileCount
      #region Get Tile Information from a Terrain
      var tileInfo = terrain.GetTileProperties();
      var colCount = tileInfo.ColumnCount;
      var rowCount = tileInfo.RowCount;
      var tileSize = tileInfo.TileSize;
      var tileCount = tileInfo.TileCount;

      #endregion

    }

    public void TerrainDefinitionProperties(ArcGIS.Core.Data.Analyst3D.TerrainDefinition terrainDef)
    {
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainDefinition.GetPyramidType
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainPyramidType
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainDefinition.GetPyramidWindowSizeProperties
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainWindowSizeProperties
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainWindowSizeProperties.Method
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainWindowSizeMethod
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainWindowSizeProperties.ZThreshold
      // cref: ArcGIS.Core.Data.Analyst3D.TerrainWindowSizeProperties.ZThresholdStrategy
      #region Get Pyramid Information from a TerrainDefinition
      var pyramidType = terrainDef.GetPyramidType();
      var pyramidProps = terrainDef.GetPyramidWindowSizeProperties();

      var method = pyramidProps.Method;
      var threshold = pyramidProps.ZThreshold;
      var strategy = pyramidProps.ZThresholdStrategy;
      #endregion
    }

    #region ProSnippet Group: LAS Dataset
    #endregion

    // cref: ArcGIS.Core.Data.FileSystemDatastoreType.LasDataset
    // cref: ArcGIS.Core.Data.Analyst3D.LasDataset
    // cref: ArcGIS.Core.Data.FileSystemDatastore.#ctor(ArcGIS.Core.Data.FileSystemConnectionPath)
    // cref: ArcGIS.Core.Data.FileSystemDatastore.OpenDataset``1(System.String)
    #region Open a LAS Dataset

    public async Task OpenLasDataset()
    {
      try
      {
        await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          string path = @"d:\Data\LASDataset";
          var fileConnection = new FileSystemConnectionPath(new Uri(path), FileSystemDatastoreType.LasDataset);

          using (FileSystemDatastore dataStore = new FileSystemDatastore(fileConnection))
          {
            string name = "utrecht_tile.lasd";      // can specify with or without the .lasd extension

            using (var dataset = dataStore.OpenDataset<ArcGIS.Core.Data.Analyst3D.LasDataset>(name))
            {

            }
          }
        });
      }
      catch (GeodatabaseNotFoundOrOpenedException exception)
      {
        // Handle Exception.
      }
    }

    #endregion

    // cref: ArcGIS.Core.Data.FileSystemDatastoreType.LasDataset
    // cref: ArcGIS.Core.Data.Analyst3D.LasDatasetDefinition
    // cref: ArcGIS.Core.Data.FileSystemDatastore.#ctor(ArcGIS.Core.Data.FileSystemConnectionPath)
    // cref: ArcGIS.Core.Data.FileSystemDatastore.GetDefinition``1(System.String)
    #region Get a LAS Dataset Defintion
    public async Task GetLasDatasetDefinition()
    {
      try
      {
        await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
        {
          string path = @"d:\Data\LASDataset";
          var fileConnection = new FileSystemConnectionPath(new Uri(path), FileSystemDatastoreType.LasDataset);

          using (FileSystemDatastore dataStore = new FileSystemDatastore(fileConnection))
          {
            string name = "utrecht_tile.lasd";      // can specify with or without the .lasd extension

            using (var dataset = dataStore.GetDefinition<ArcGIS.Core.Data.Analyst3D.LasDatasetDefinition>(name))
            {

            }
          }
        });
      }
      catch (GeodatabaseNotFoundOrOpenedException exception)
      {
        // Handle Exception.
      }
    }
    #endregion

    public void LasDatasetProperties(ArcGIS.Core.Data.Analyst3D.LasDataset lasDataset)
    {
      // cref: ArcGIS.Core.Data.Analyst3D.LasDataset.GetFileCounts
      // cref: ArcGIS.Core.Data.Analyst3D.LasDataset.GetFiles
      // cref: ArcGIS.Core.Data.Analyst3D.LasFile
      // cref: ArcGIS.Core.Data.Analyst3D.LasFile.FilePath
      // cref: ArcGIS.Core.Data.Analyst3D.LasFile.FileName
      // cref: ArcGIS.Core.Data.Analyst3D.LasFile.PointCount
      // cref: ArcGIS.Core.Data.Analyst3D.LasFile.ZMin
      // cref: ArcGIS.Core.Data.Analyst3D.LasFile.ZMax
      #region Get Individual File Information from a LAS Dataset

      var (lasFileCount, zLasFileCount) = lasDataset.GetFileCounts();
      IReadOnlyList<ArcGIS.Core.Data.Analyst3D.LasFile> fileInfo = lasDataset.GetFiles();
      foreach (var file in fileInfo)
      {
        var path = file.FilePath;
        var name = file.FileName;
        var ptCount = file.PointCount;
        var zMin = file.ZMin;
        var zMax = file.ZMax;
      }
      #endregion

      // cref: ArcGIS.Core.Data.Analyst3D.LasDataset.GetSurfaceConstraintCount
      // cref: ArcGIS.Core.Data.Analyst3D.LasDataset.GetSurfaceConstraints
      // cref: ArcGIS.Core.Data.Analyst3D.SurfaceConstraint
      // cref: ArcGIS.Core.Data.Analyst3D.SurfaceConstraint.DataSourceName
      // cref: ArcGIS.Core.Data.Analyst3D.SurfaceConstraint.WorkspacePath
      // cref: ArcGIS.Core.Data.Analyst3D.SurfaceConstraint.HeightField
      // cref: ArcGIS.Core.Data.Analyst3D.SurfaceConstraint.SurfaceType
      // cref: ArcGIS.Core.Data.Analyst3D.TinSurfaceType
      #region Get Surface Constraint information from a LAS Dataset

      var constraintCount = lasDataset.GetSurfaceConstraintCount();
      IReadOnlyList<ArcGIS.Core.Data.Analyst3D.SurfaceConstraint> constraints = lasDataset.GetSurfaceConstraints();
      foreach (var constraint in constraints)
      {
        var dsName = constraint.DataSourceName;
        var wksPath = constraint.WorkspacePath;
        var heightField = constraint.HeightField;
        var surfaceType = constraint.SurfaceType;
      }
      #endregion


      // cref: ArcGIS.Core.Data.Analyst3D.LasDataset.GetUniqueClassCodes
      // cref: ArcGIS.Core.Data.Analyst3D.LasDataset.GetUniqueReturns
      // cref: ArcGIS.Core.Data.Analyst3D.LasReturnType
      #region Get classification codes / Returns from a LAS Dataset

      var classCodes = lasDataset.GetUniqueClassCodes();
      var returns = lasDataset.GetUniqueReturns();

      #endregion

      Envelope envelope = null;

      // cref: ArcGIS.Core.Data.Analyst3D.LasDataset.GetPointByID(System.Double, ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Core.Data.Analyst3D.LasPoint
      // cref: ArcGIS.Core.Data.Analyst3D.LasPoint.Coordinate3D
      // cref: ArcGIS.Core.Data.Analyst3D.LasPoint.ToMapPoint
      #region Access LAS Points by ID

      // access by ID
      IReadOnlyList<ArcGIS.Core.Data.Analyst3D.LasPoint> pts = lasDataset.GetPointByID(123456);

      pts = lasDataset.GetPointByID(123456, envelope);
      ArcGIS.Core.Data.Analyst3D.LasPoint pt = pts.FirstOrDefault();

      var coords = pt.Coordinate3D;
      var mapPoint = pt.ToMapPoint();
      #endregion

      // cref: ArcGIS.Core.Data.Analyst3D.LasDataset.SearchPoints(ArcGIS.Core.Data.Analyst3D.LasPointFilter, Systen.Double, System.Double)
      // cref: ArcGIS.Core.Data.Analyst3D.LasPointCursor
      // cref: ArcGIS.Core.Data.Analyst3D.LasPointCursor.MoveNext
      // cref: ArcGIS.Core.Data.Analyst3D.LasPointCursor.Current
      // cref: ArcGIS.Core.Data.Analyst3D.LasPoint
      // cref: ArcGIS.Core.Data.Analyst3D.LasPointFilter
      // cref: ArcGIS.Core.Data.Analyst3D.LasPointFilter.#ctor
      // cref: ArcGIS.Core.Data.Analyst3D.LasPointFilter.FilterGeometry
      // cref: ArcGIS.Core.Data.Analyst3D.LasPointFilter.ClassCodes
      #region Search LAS Points

      // search all points
      using (ArcGIS.Core.Data.Analyst3D.LasPointCursor ptCursor = lasDataset.SearchPoints(null))
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
      using (ArcGIS.Core.Data.Analyst3D.LasPointCursor ptCursor = lasDataset.SearchPoints(pointFilter))
      {
        while (ptCursor.MoveNext())
        {
          using (ArcGIS.Core.Data.Analyst3D.LasPoint point = ptCursor.Current)
          {

          }
        }
      }

      // search within an extent and limited to specific classification codes
      pointFilter = new ArcGIS.Core.Data.Analyst3D.LasPointFilter();
      pointFilter.FilterGeometry = envelope;
      pointFilter.ClassCodes = new List<int> { 4, 5 };
      using (ArcGIS.Core.Data.Analyst3D.LasPointCursor ptCursor = lasDataset.SearchPoints(pointFilter))
      {
        while (ptCursor.MoveNext())
        {
          using (ArcGIS.Core.Data.Analyst3D.LasPoint point = ptCursor.Current)
          {

          }
        }
      }

      #endregion

      // cref: ArcGIS.Core.Data.Analyst3D.LasDataset.SearchPoints(ArcGIS.Core.Data.Analyst3D.LasPointFilter, Systen.Double, System.Double)
      // cref: ArcGIS.Core.Data.Analyst3D.LasPointCursor
      // cref: ArcGIS.Core.Data.Analyst3D.LasPointCursor.MoveNextArray
      // cref: ArcGIS.Core.Data.Analyst3D.LasPointFilter
      // cref: ArcGIS.Core.Data.Analyst3D.LasPointFilter.#ctor
      // cref: ArcGIS.Core.Data.Analyst3D.LasPointFilter.FilterGeometry
      #region Search using pre initialized arrays

      // search all points
      using (ArcGIS.Core.Data.Analyst3D.LasPointCursor ptCursor = lasDataset.SearchPoints(null))
      {
        int count;
        Coordinate3D[] lasPointsRetrieved = new Coordinate3D[10000];
        while (ptCursor.MoveNextArray(lasPointsRetrieved, null, null, null, out count))
        {
          var points = lasPointsRetrieved.ToList();
        
          // ...
        }
      }

      // search within an extent
      // use MoveNextArray retrieving coordinates, fileIndex and pointIds
      ArcGIS.Core.Data.Analyst3D.LasPointFilter filter = new ArcGIS.Core.Data.Analyst3D.LasPointFilter();
      filter.FilterGeometry = envelope;
      using (ArcGIS.Core.Data.Analyst3D.LasPointCursor ptCursor = lasDataset.SearchPoints(filter))
      {
        int count;
        Coordinate3D[] lasPointsRetrieved = new Coordinate3D[50000];
        int[] fileIndexes = new int[50000];
        double[] pointIds = new double[50000];
        while (ptCursor.MoveNextArray(lasPointsRetrieved, null, fileIndexes, pointIds, out count))
        {
          var points = lasPointsRetrieved.ToList();

        }
      }
      
      #endregion
    }
  }
}
