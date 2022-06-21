/*

   Copyright 2020 Esri

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
using ArcGIS.Core.Data.Topology;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Framework.Threading.Tasks;

namespace GeodatabaseSDK.Topo
{
  class ProSnippetTopology
  {
    // cref: ArcGIS.Core.Data.Topology.Topology
    // cref: ArcGIS.Core.Data.Topology.Topology.GetDefinition
    // cref: ArcGIS.Core.Data.Topology.TopologyDefinition
    // cref: ArcGIS.Core.Data.Topology.TopologyDefinition.GetFeatureClassNames
    // cref: ArcGIS.Core.Data.Topology.TopologyDefinition.GetClusterTolerance
    // cref: ArcGIS.Core.Data.Topology.TopologyDefinition.GetZClusterTolerance
    #region OpenTopologyAndProcessDefinition

    public void OpenTopologyAndProcessDefinition()
    {
      // Open a geodatabase topology from a file geodatabase and process the topology definition.

      using (Geodatabase geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(@"C:\TestData\GrandTeton.gdb"))))
      using (Topology topology = geodatabase.OpenDataset<Topology>("Backcountry_Topology"))
      {
        ProcessDefinition(geodatabase, topology);
      }

      // Open a feature service topology and process the topology definition.

      const string TOPOLOGY_LAYER_ID = "0";

      using (Geodatabase geodatabase = new Geodatabase(new ServiceConnectionProperties(new Uri("https://sdkexamples.esri.com/server/rest/services/GrandTeton/FeatureServer"))))
      using (Topology topology = geodatabase.OpenDataset<Topology>(TOPOLOGY_LAYER_ID))
      {
        ProcessDefinition(geodatabase, topology);
      }
    }

    private void ProcessDefinition(Geodatabase geodatabase, Topology topology)
    {
      // Similar to the rest of the Definition objects in the Core.Data API, there are two ways to open a dataset's 
      // definition -- via the Topology dataset itself or via the Geodatabase.

      using (TopologyDefinition definitionViaTopology = topology.GetDefinition())
      {
        OutputDefinition(geodatabase, definitionViaTopology);
      }

      using (TopologyDefinition definitionViaGeodatabase = 
        geodatabase.GetDefinition<TopologyDefinition>("Backcountry_Topology"))
      {
        OutputDefinition(geodatabase, definitionViaGeodatabase);
      }
    }

    private void OutputDefinition(Geodatabase geodatabase, TopologyDefinition topologyDefinition)
    {
      Console.WriteLine($"Topology cluster tolerance => {topologyDefinition.GetClusterTolerance()}");
      Console.WriteLine($"Topology Z cluster tolerance => {topologyDefinition.GetZClusterTolerance()}");

      IReadOnlyList<string> featureClassNames = topologyDefinition.GetFeatureClassNames();
      Console.WriteLine($"There are {featureClassNames.Count} feature classes that are participating in the topology:");

      foreach (string name in featureClassNames)
      {
        // Open each feature class that participates in the topology.

        using (FeatureClass featureClass = geodatabase.OpenDataset<FeatureClass>(name))
        using (FeatureClassDefinition featureClassDefinition = featureClass.GetDefinition())
        {
          Console.WriteLine($"\t{featureClass.GetName()} ({featureClassDefinition.GetShapeType()})");
        }
      }
    }

    #endregion OpenTopologyAndProcessDefinition

    public void GetTopologyRules()
    {
      using (Geodatabase geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(@"C:\TestData\GrandTeton.gdb"))))
      using (Topology topology = geodatabase.OpenDataset<Topology>("Backcountry_Topology"))
      {
        // cref: ArcGIS.Core.Data.Topology.TopologyDefinition.GetRules
        // cref: ArcGIS.Core.Data.Topology.TopologyRule
        // cref: ArcGIS.Core.Data.Topology.TopologyRule.ID
        // cref: ArcGIS.Core.Data.Topology.TopologyRule.OriginClass
        // cref: ArcGIS.Core.Data.Topology.TopologyRule.OriginSubtype
        // cref: ArcGIS.Core.Data.Topology.TopologyRule.DestinationClass
        // cref: ArcGIS.Core.Data.Topology.TopologyRule.DestinationSubtype
        // cref: ArcGIS.Core.Data.Topology.TopologyRule.RuleType
        #region GetTopologyRules

        using (TopologyDefinition topologyDefinition = topology.GetDefinition())
        {
          IReadOnlyList<TopologyRule> rules = topologyDefinition.GetRules();

          Console.WriteLine($"There are {rules.Count} topology rules defined for the topology:");
          Console.WriteLine("ID \t Origin Class \t Origin Subtype \t Destination Class \t Destination Subtype \t Rule Type");

          foreach (TopologyRule rule in rules)
          {
            Console.Write($"{rule.ID}");

            Console.Write(!String.IsNullOrEmpty(rule.OriginClass) ? $"\t{rule.OriginClass}" : "\t\"\"");

            Console.Write(rule.OriginSubtype != null ? $"\t{rule.OriginSubtype.GetName()}" : "\t\"\"");

            Console.Write(!String.IsNullOrEmpty(rule.DestinationClass) ? $"\t{rule.DestinationClass}" : "\t\"\"");

            Console.Write(rule.DestinationSubtype != null ? $"\t{rule.DestinationSubtype.GetName()}" : "\t\"\"");

            Console.Write($"\t{rule.RuleType}");

            Console.WriteLine();
          }
        }

        #endregion GetTopologyRules
      }
    }

    // cref: ArcGIS.Core.Data.Topology.ValidationDescription.#ctor
    // cref: ArcGIS.Core.Data.Topology.Topology.Validate(ArcGIS.Core.Data.Topology.ValidationDescription)
    // cref: ArcGIS.Core.Data.Topology.Topology.GetExtent
    // cref: ArcGIS.Core.Data.Topology.Topology.GetState
    // cref: ArcGIS.Core.Data.Topology.ValidationResult
    #region ValidateTopology

    public void ValidateTopology()
    {
      using (Geodatabase geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(@"C:\TestData\GrandTeton.gdb"))))
      using (Topology topology = geodatabase.OpenDataset<Topology>("Backcountry_Topology"))
      {
        // If the topology currently does not have dirty areas, calling Validate() returns an empty envelope.

        ValidationResult result = topology.Validate(new ValidationDescription(topology.GetExtent()));
        Console.WriteLine($"'AffectedArea' after validating a topology that has not been edited => {result.AffectedArea.ToJson()}");

        // Now create a feature that purposely violates the "PointProperlyInsideArea" topology rule.  This action will
        // create dirty areas.

        Feature newFeature = null;

        try
        {
          // Fetch the feature in the Campsites feature class whose objectID is 2.  Then create a new geometry slightly
          // altered from this and use it to create a new feature.

          using (Feature featureViaCampsites2 = GetFeature(geodatabase, "Campsites", 2))
          {
            Geometry currentGeometry = featureViaCampsites2.GetShape();
            Geometry newGeometry = GeometryEngine.Instance.Move(currentGeometry, (currentGeometry.Extent.XMax / 8),
              (currentGeometry.Extent.YMax / 8));

            using (FeatureClass campsitesFeatureClass = featureViaCampsites2.GetTable())
            using (FeatureClassDefinition definition = campsitesFeatureClass.GetDefinition())
            using (RowBuffer rowBuffer = campsitesFeatureClass.CreateRowBuffer())
            {
              rowBuffer[definition.GetShapeField()] = newGeometry;

              geodatabase.ApplyEdits(() =>
              {
                newFeature = campsitesFeatureClass.CreateRow(rowBuffer);
              });
            }
          }

          // After creating a new feature in the 'Campsites' participating feature class, the topology's state should be 
          // "Unanalyzed" because it has not been validated.

          Console.WriteLine($"The topology state after an edit has been applied => {topology.GetState()}");

          // Now validate the topology.  The result envelope corresponds to the dirty areas.

          result = topology.Validate(new ValidationDescription(topology.GetExtent()));
          Console.WriteLine($"'AffectedArea' after validating a topology that has just been edited => {result.AffectedArea.ToJson()}");

          // After Validate(), the topology's state should be "AnalyzedWithErrors" because the topology currently has errors.

          Console.WriteLine($"The topology state after validate topology => {topology.GetState()}");

          // If there are no dirty areas, the result envelope should be empty.

          result = topology.Validate(new ValidationDescription(topology.GetExtent()));
          Console.WriteLine($"'AffectedArea' after validating a topology that has just been validated => {result.AffectedArea.ToJson()}");
        }
        finally
        {
          if (newFeature != null)
          {
            geodatabase.ApplyEdits(() =>
            {
              newFeature.Delete();
            });

            newFeature.Dispose();
          }
        }

        // Validate again after deleting the newly-created feature.

        topology.Validate(new ValidationDescription(topology.GetExtent()));
      }
    }

    private Feature GetFeature(Geodatabase geodatabase, string featureClassName, long objectID)
    {
      using (FeatureClass featureClass = geodatabase.OpenDataset<FeatureClass>(featureClassName))
      {
        QueryFilter queryFilter = new QueryFilter()
        {
          ObjectIDs = new List<long>() { objectID }
        };

        using (RowCursor cursor = featureClass.Search(queryFilter))
        {
          System.Diagnostics.Debug.Assert(cursor.MoveNext());
          return (Feature)cursor.Current;
        }
      }
    }

    #endregion ValidateTopology

    public void WorkWithTopologyErrors()
    {
      using (Geodatabase geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(@"C:\TestData\GrandTeton.gdb"))))
      using (Topology topology = geodatabase.OpenDataset<Topology>("Backcountry_Topology"))
      {
        // cref: ArcGIS.Core.Data.Topology.Topology.GetErrors(ArcGIS.Core.Data.Topology.ErrorDescription)
        // cref: ArcGIS.Core.Data.Topology.TopologyError
        // cref: ArcGIS.Core.Data.Topology.ErrorDescription
        // cref: ArcGIS.Core.Data.Topology.TopologyError.OriginClassName
        // cref: ArcGIS.Core.Data.Topology.TopologyError.OriginObjectID
        // cref: ArcGIS.Core.Data.Topology.TopologyError.DestinationClassName
        // cref: ArcGIS.Core.Data.Topology.TopologyError.DestinationObjectID
        // cref: ArcGIS.Core.Data.Topology.TopologyError.RuleType
        // cref: ArcGIS.Core.Data.Topology.TopologyError.IsException
        // cref: ArcGIS.Core.Data.Topology.TopologyError.Shape
        // cref: ArcGIS.Core.Data.Topology.TopologyError.RuleID
        #region GetTopologyErrors

        // Get all the errors and exceptions currently associated with the topology.

        IReadOnlyList<TopologyError> allErrorsAndExceptions = topology.GetErrors(new ErrorDescription(topology.GetExtent()));
        Console.WriteLine($"errors and exceptions count => {allErrorsAndExceptions.Count}");
        
        Console.WriteLine("OriginClassName \t OriginObjectID \t DestinationClassName \t DestinationObjectID \t RuleType \t IsException \t Shape type \t Shape width & height \t  Rule ID \t");

        foreach (TopologyError error in allErrorsAndExceptions)
        {
          Console.WriteLine($"'{error.OriginClassName}' \t {error.OriginObjectID} \t '{error.DestinationClassName}' \t " +
                            $"{error.DestinationObjectID} \t {error.RuleType} \t {error.IsException} \t {error.Shape.GeometryType} \t " +
                            $"{error.Shape.Extent.Width},{error.Shape.Extent.Height} \t {error.RuleID}");
        }

        #endregion GetTopologyErrors

        // cref: ArcGIS.Core.Data.Topology.Topology.MarkAsException(ArcGIS.Core.Data.Topology.TopologyError)
        // cref: ArcGIS.Core.Data.Topology.Topology.UnmarkAsException(ArcGIS.Core.Data.Topology.TopologyError)
        // cref: ArcGIS.Core.Data.Topology.Topology.GetExtent
        // cref: ArcGIS.Core.Data.Topology.Topology.GetErrors
        // cref: ArcGIS.Core.Data.Topology.TopologyRule.RuleType
        // cref: ArcGIS.Core.Data.Topology.TopologyDefinition.GetRules
        // cref: ArcGIS.Core.Data.Topology.TopologyRuleType
        // cref: ArcGIS.Core.Data.Topology.ErrorDescription.TopologyRule
        // cref: ArcGIS.Core.Data.Topology.ErrorDescription.ErrorType
        // cref: ArcGIS.Core.Data.Topology.ErrorType
        // cref: ArcGIS.Core.Data.Topology.TopologyError
        #region MarkAndUnmarkAsErrors

        // Get all the errors due to features violating the "PointProperlyInsideArea" topology rule.

        using (TopologyDefinition topologyDefinition = topology.GetDefinition())
        {
          TopologyRule pointProperlyInsideAreaRule = topologyDefinition.GetRules().First(rule => rule.RuleType == TopologyRuleType.PointProperlyInsideArea);

          ErrorDescription errorDescription = new ErrorDescription(topology.GetExtent())
          {
            TopologyRule = pointProperlyInsideAreaRule
          };

          IReadOnlyList<TopologyError> errorsDueToViolatingPointProperlyInsideAreaRule = topology.GetErrors(errorDescription);
          Console.WriteLine($"There are {errorsDueToViolatingPointProperlyInsideAreaRule.Count} feature violating the 'PointProperlyInsideArea' topology rule.");

          // Mark all errors from features violating the 'PointProperlyInsideArea' topology rule as exceptions.

          foreach (TopologyError error in errorsDueToViolatingPointProperlyInsideAreaRule)
          {
            topology.MarkAsException(error);
          }

          // Now verify all the errors from features violating the 'PointProperlyInsideArea' topology rule have indeed been
          // marked as exceptions.
          //
          // By default, ErrorDescription is initialized to ErrorType.ErrorAndException.  Here we want ErrorType.ErrorOnly.

          errorDescription = new ErrorDescription(topology.GetExtent())
          {
            ErrorType = ErrorType.ErrorOnly,
            TopologyRule = pointProperlyInsideAreaRule
          };

          IReadOnlyList<TopologyError> errorsAfterMarkedAsExceptions = topology.GetErrors(errorDescription);
          Console.WriteLine($"There are {errorsAfterMarkedAsExceptions.Count} feature violating the 'PointProperlyInsideArea' topology rule after all the errors have been marked as exceptions.");

          // Finally, reset all the exceptions as errors by unmarking them as exceptions.

          foreach (TopologyError error in errorsDueToViolatingPointProperlyInsideAreaRule)
          {
            topology.UnmarkAsException(error);
          }

          IReadOnlyList<TopologyError> errorsAfterUnmarkedAsExceptions = topology.GetErrors(errorDescription);
          Console.WriteLine($"There are {errorsAfterUnmarkedAsExceptions.Count} feature violating the 'PointProperlyInsideArea' topology rule after all the exceptions have been reset as errors.");
        }

        #endregion MarkAndUnmarkAsErrors
      }
    }

    // cref: ArcGIS.Core.Data.Topology.Topology.BuildGraph(ArcGIS.Core.Geometry.Geometry,System.Action{ArcGIS.Core.Data.Topology.TopologyGraph})
    // cref: ArcGIS.Core.Data.Topology.TopologyGraph.GetEdges
    // cref: ArcGIS.Core.Data.Topology.TopologyEdge.GetFromNode
    // cref: ArcGIS.Core.Data.Topology.TopologyEdge.GetToNode
    // cref: ArcGIS.Core.Data.Topology.TopologyEdge.GetLeftParentFeatures
    // cref: ArcGIS.Core.Data.Topology.TopologyEdge.GetRightParentFeatures
    // cref: ArcGIS.Core.Data.Topology.TopologyNode
    // cref: ArcGIS.Core.Data.Topology.FeatureInfo
    // cref: ArcGIS.Core.Data.Topology.FeatureInfo.FeatureClassName
    // cref: ArcGIS.Core.Data.Topology.FeatureInfo.ObjectID
    #region ExploreTopologyGraph

    public void ExploreTopologyGraph()
    {
      using (Geodatabase geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(@"C:\TestData\GrandTeton.gdb"))))
      using (Topology topology = geodatabase.OpenDataset<Topology>("Backcountry_Topology"))
      {
        // Build a topology graph using the extent of the topology dataset.

        topology.BuildGraph(topology.GetExtent(), (topologyGraph) =>
        {
          using (Feature campsites12 = GetFeature(geodatabase, "Campsites", 12))
          {
            IReadOnlyList<TopologyNode> topologyNodesViaCampsites12 = topologyGraph.GetNodes(campsites12);

            TopologyNode topologyNodeViaCampsites12 = topologyNodesViaCampsites12[0];

            IReadOnlyList<TopologyEdge> allEdgesConnectedToNodeViaCampsites12 = topologyNodeViaCampsites12.GetEdges();
            IReadOnlyList<TopologyEdge> allEdgesConnectedToNodeViaCampsites12CounterClockwise = topologyNodeViaCampsites12.GetEdges(false);

            System.Diagnostics.Debug.Assert(allEdgesConnectedToNodeViaCampsites12.Count == allEdgesConnectedToNodeViaCampsites12CounterClockwise.Count);

            foreach (TopologyEdge edgeConnectedToNodeViaCampsites12 in allEdgesConnectedToNodeViaCampsites12)
            {
              TopologyNode fromNode = edgeConnectedToNodeViaCampsites12.GetFromNode();
              TopologyNode toNode = edgeConnectedToNodeViaCampsites12.GetToNode();

              bool fromNodeIsTheSameAsTopologyNodeViaCampsites12 = (fromNode == topologyNodeViaCampsites12);
              bool toNodeIsTheSameAsTopologyNodeViaCampsites12 = (toNode == topologyNodeViaCampsites12);

              System.Diagnostics.Debug.Assert(fromNodeIsTheSameAsTopologyNodeViaCampsites12 || toNodeIsTheSameAsTopologyNodeViaCampsites12,
                "The FromNode *or* ToNode of each edge connected to 'topologyNodeViaCampsites12' should be the same as 'topologyNodeViaCampsites12' itself.");

              IReadOnlyList<FeatureInfo> leftParentFeaturesBoundedByEdge = edgeConnectedToNodeViaCampsites12.GetLeftParentFeatures();
              foreach (FeatureInfo featureInfo in leftParentFeaturesBoundedByEdge)
              {
                System.Diagnostics.Debug.Assert(!String.IsNullOrEmpty(featureInfo.FeatureClassName));
                System.Diagnostics.Debug.Assert(featureInfo.ObjectID > 0);
                EnsureShapeIsNotEmpty(featureInfo);
              }

              IReadOnlyList<FeatureInfo> leftParentFeaturesNotBoundedByEdge = edgeConnectedToNodeViaCampsites12.GetLeftParentFeatures(false);
              foreach (FeatureInfo featureInfo in leftParentFeaturesNotBoundedByEdge)
              {
                System.Diagnostics.Debug.Assert(!String.IsNullOrEmpty(featureInfo.FeatureClassName));
                System.Diagnostics.Debug.Assert(featureInfo.ObjectID > 0);
                EnsureShapeIsNotEmpty(featureInfo);
              }

              IReadOnlyList<FeatureInfo> rightParentFeaturesBoundedByEdge = edgeConnectedToNodeViaCampsites12.GetRightParentFeatures();
              foreach (FeatureInfo featureInfo in rightParentFeaturesBoundedByEdge)
              {
                System.Diagnostics.Debug.Assert(!String.IsNullOrEmpty(featureInfo.FeatureClassName));
                System.Diagnostics.Debug.Assert(featureInfo.ObjectID > 0);
                EnsureShapeIsNotEmpty(featureInfo);
              }

              IReadOnlyList<FeatureInfo> rightParentFeaturesNotBoundedByEdge = edgeConnectedToNodeViaCampsites12.GetRightParentFeatures(false);
              foreach (FeatureInfo featureInfo in rightParentFeaturesNotBoundedByEdge)
              {
                System.Diagnostics.Debug.Assert(!String.IsNullOrEmpty(featureInfo.FeatureClassName));
                System.Diagnostics.Debug.Assert(featureInfo.ObjectID > 0);
                EnsureShapeIsNotEmpty(featureInfo);
              }
            }
          }
        });
      }
    }

    private void EnsureShapeIsNotEmpty(FeatureInfo featureInfo)
    {
      using (Feature feature = featureInfo.GetFeature())
      {
        System.Diagnostics.Debug.Assert(!feature.GetShape().IsEmpty, "The feature's shape should not be empty.");
      }
    }

    #endregion ExploreTopologyGraph

    // cref: ArcGIS.Core.Data.Topology.TopologyGraph.FindClosestElement
    // cref: ArcGIS.Core.Data.Topology.BuildGraph
    // cref: ArcGIS.Core.Data.Topology.GetExtent
    // cref: ArcGIS.Core.Data.Topology.TopologyElement
    // cref: ArcGIS.Core.Data.Topology.TopologyElement.GetParentFeatures
    // cref: ArcGIS.Core.Data.Topology.FeatureInfo.FeatureClassName
    // cref: ArcGIS.Core.Data.Topology.FeatureInfo.ObjectID
    #region FindClosestElement

    public void FindClosestElement()
    {
      using (Geodatabase geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(@"C:\TestData\GrandTeton.gdb"))))
      using (Topology topology = geodatabase.OpenDataset<Topology>("Backcountry_Topology"))
      {
        // Build a topology graph using the extent of the topology dataset.

        topology.BuildGraph(topology.GetExtent(), (topologyGraph) =>
        {
          MapPoint queryPointViaCampsites12 = null;

          using (Feature campsites12 = GetFeature(geodatabase, "Campsites", 12))
          {
            queryPointViaCampsites12 = campsites12.GetShape() as MapPoint;
          }

          double searchRadius = 1.0;

          TopologyElement topologyElementViaCampsites12 = 
              topologyGraph.FindClosestElement<TopologyElement>(
                        queryPointViaCampsites12, searchRadius);

          System.Diagnostics.Debug.Assert(
            topologyElementViaCampsites12 != null, "There should be a topology element corresponding to 'queryPointViaCampsites12' within the 'searchRadius' units.");

          IReadOnlyList<FeatureInfo> parentFeatures = topologyElementViaCampsites12.GetParentFeatures();

          Console.WriteLine("The parent features that spawn 'topologyElementViaCampsites12' are:");
          foreach (FeatureInfo parentFeature in parentFeatures)
          {
            Console.WriteLine($"\t{parentFeature.FeatureClassName}; OID: {parentFeature.ObjectID}");
          }

          TopologyNode topologyNodeViaCampsites12 = topologyGraph.FindClosestElement<TopologyNode>(queryPointViaCampsites12, searchRadius);

          if (topologyNodeViaCampsites12 != null)
          {
            // There exists a TopologyNode nearest to the query point within searchRadius units.
          }

          TopologyEdge topologyEdgeViaCampsites12 = topologyGraph.FindClosestElement<TopologyEdge>(queryPointViaCampsites12, searchRadius);

          if (topologyEdgeViaCampsites12 != null)
          {
            // There exists a TopologyEdge nearest to the query point within searchRadius units.
          }
        });
      }
    }

    #endregion FindClosestElement
  }
}
