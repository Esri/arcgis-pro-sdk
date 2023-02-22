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
using ArcGIS.Core.Data;
using ArcGIS.Core.Data.UtilityNetwork;
using ArcGIS.Core.Data.UtilityNetwork.Trace;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Core.Data.NetworkDiagrams;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Core.CIM;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Editing;
using ArcGIS.Core.Geometry;

namespace UtilityNetworkProSnippets { 

  class ProSnippetsUtilityNetwork
  {

    #region ProSnippet Group: Obtaining Utility Networks
    #endregion

    // cref: ArcGIS.Core.Data.Table.IsControllerDatasetSupported()
    // cref: ArcGIS.Core.Data.Table.GetControllerDatasets()
    // cref: ArcGIS.Core.Data.UtilityNetwork.UtilityNetwork
    #region Get a Utility Network from a Table
    public static UtilityNetwork GetUtilityNetworkFromTable(Table table)
    {
      UtilityNetwork utilityNetwork = null;

      if (table.IsControllerDatasetSupported())
      {
        // Tables can belong to multiple controller datasets, but at most one of them will be a UtilityNetwork
        IReadOnlyList<Dataset> controllerDatasets = table.GetControllerDatasets();

        foreach (Dataset controllerDataset in controllerDatasets)
        {
          if (controllerDataset is UtilityNetwork)
          {
            utilityNetwork = controllerDataset as UtilityNetwork;
          }
          else
          {
            controllerDataset.Dispose();
          }
        }
      }
      return utilityNetwork;
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.UtilityNetworkLayer
    // cref: ArcGIS.Desktop.Mapping.UtilityNetworkLayer.GetUtilityNetwork()
    // cref: ArcGIS.Core.Data.Table.IsControllerDatasetSupported()
    // cref: ArcGIS.Core.Data.Table.GetControllerDatasets()
    // cref: ArcGIS.Core.Data.UtilityNetwork.UtilityNetwork
    #region Get a Utility Network from a Layer
    // This routine obtains a utility network from a FeatureLayer, SubtypeGroupLayer, or UtilityNetworkLayer
    public static UtilityNetwork GetUtilityNetworkFromLayer(Layer layer)
    {
      UtilityNetwork utilityNetwork = null;

      if (layer is UtilityNetworkLayer)
      {
        UtilityNetworkLayer utilityNetworkLayer = layer as UtilityNetworkLayer;
        utilityNetwork = utilityNetworkLayer.GetUtilityNetwork();
      }

      else if (layer is SubtypeGroupLayer)
      {
        CompositeLayer compositeLayer = layer as CompositeLayer;
        utilityNetwork = GetUtilityNetworkFromLayer(compositeLayer.Layers.First());
      }

      else if (layer is FeatureLayer)
      {
        FeatureLayer featureLayer = layer as FeatureLayer;
        using (FeatureClass featureClass = featureLayer.GetFeatureClass())
        {
          if (featureClass.IsControllerDatasetSupported())
          {
            IReadOnlyList<Dataset> controllerDatasets = new List<Dataset>();
            controllerDatasets = featureClass.GetControllerDatasets();
            foreach (Dataset controllerDataset in controllerDatasets)
            {
              if (controllerDataset is UtilityNetwork)
              {
                utilityNetwork = controllerDataset as UtilityNetwork;
              }
              else
              {
                controllerDataset.Dispose();
              }
            }
          }
        }
      }
      return utilityNetwork;
    }
    #endregion


    #region ProSnippet Group: Elements
    #endregion

    // cref: ArcGIS.Core.Data.UtilityNetwork.Element
    // cref: ArcGIS.Core.Data.UtilityNetwork.Element.NetworkSource
    // cref: ArcGIS.Core.Data.UtilityNetwork.UtilityNetwork.GetTable(ArcGIS.Core.Data.UtilityNetwork.Element.NetworkSource)
    // cref: ArcGIS.Core.Data.QueryFilter.#ctor
    // cref: ArcGIS.Core.Data.Table.Search
    #region Fetching a Row from an Element
    // usage :   using (var row = FetchRowFromElement(...))
    public static Row FetchRowFromElement(UtilityNetwork utilityNetwork, Element element)
    {
      // Get the table from the element
      using (Table table = utilityNetwork.GetTable(element.NetworkSource))
      {
        // Create a query filter to fetch the appropriate row
        QueryFilter queryFilter = new QueryFilter()
        {
          ObjectIDs = new List<long>() { element.ObjectID }
        };

        // Fetch and return the row
        using (RowCursor rowCursor = table.Search(queryFilter))
        {
          if (rowCursor.MoveNext())
          {
            return rowCursor.Current;
          }
          return null;
        }
      }
    }
    #endregion
    

    #region ProSnippet Group: Editing Associations
    #endregion


    public void EditOperation(UtilityNetwork utilityNetwork, AssetType poleAssetType, Guid poleGlobalID, AssetType transformerBankAssetType, Guid transformerBankGlobalID)
    {
      // cref: ArcGIS.Core.Data.UtilityNetwork.AssetType
      // cref: ArcGIS.Core.Data.UtilityNetwork.UtilityNetwork.CreateElement(ArcGIS.Core.Data.UtilityNetwork.AssetType, System.Guid)
      // cref: ArcGIS.Desktop.Editing.RowHandle.#ctor(ArcGIS.Core.Data.UtilityNetwork.Element, ArcGIS.Core.Data.UtilityNetwork.UtilityNetwork)
      // cref: ArcGIS.Core.Data.UtilityNetwork.AssociationType
      // cref: ArcGIS.Desktop.Editing.AssociationDescription
      // cref: ArcGIS.Desktop.Editing.AssociationDescription.#ctor(ArcGIS.Core.Data.UtilityNetwork.AssociationType, ArcGIS.Desktop.Editing.RowHandle, ArcGIS.Desktop.Editing.RowHandle)
      // cref: ArcGIS.Desktop.Editing.EditOperation.Create(ArcGIS.Desktop.Editing.AssociationDescription)
      #region Create a utility network association

      // Create edit operation
      EditOperation editOperation = new EditOperation();
      editOperation.Name = "Create structural attachment association";

      // Create a RowHandle for the pole

      Element poleElement = utilityNetwork.CreateElement(poleAssetType, poleGlobalID);
      RowHandle poleRowHandle = new RowHandle(poleElement, utilityNetwork);

      // Create a RowHandle for the transformer bank

      Element transformerBankElement = utilityNetwork.CreateElement(transformerBankAssetType, transformerBankGlobalID);
      RowHandle transformerBankRowHandle = new RowHandle(transformerBankElement, utilityNetwork);

      // Attach the transformer bank to the pole

      AssociationDescription structuralAttachmentAssociationDescription = new AssociationDescription(AssociationType.Attachment, poleRowHandle, transformerBankRowHandle);
      editOperation.Create(structuralAttachmentAssociationDescription);
      editOperation.Execute();

      #endregion
    }

    public void EditOperation2(FeatureLayer transformerBankLayer, Dictionary<string, object> transformerBankAttributes, FeatureLayer poleLayer, Dictionary<string, object> poleAttributes)
    {
      // cref: ArcGIS.Desktop.Editing.RowToken
      // cref: ArcGIS.Desktop.Editing.RowHandle.#ctor(ArcGIS.Desktop.Editing.RowToken)
      // cref: ArcGIS.Desktop.Editing.AssociationDescription.#ctor(ArcGIS.Core.Data.UtilityNetwork.AssociationType, ArcGIS.Desktop.Editing.RowHandle, ArcGIS.Desktop.Editing.RowHandle)
      // cref: ArcGIS.Desktop.Editing.AssociationDescription.#ctor
      #region Create utility network features and associations in a single edit operation

      // Create an EditOperation
      EditOperation editOperation = new EditOperation();
      editOperation.Name = "Create pole; create transformer bank; attach transformer bank to pole";

      // Create the transformer bank
      RowToken transformerBankToken = editOperation.Create(transformerBankLayer, transformerBankAttributes);

      // Create a pole
      RowToken poleToken = editOperation.Create(poleLayer, poleAttributes);

      // Create a structural attachment association between the pole and the transformer bank
      RowHandle poleHandle = new RowHandle(poleToken);
      RowHandle transformerBankHandle = new RowHandle(transformerBankToken);

      AssociationDescription poleAttachment = new AssociationDescription(AssociationType.Attachment, poleHandle, transformerBankHandle);

      editOperation.Create(poleAttachment);

      // Execute the EditOperation
      editOperation.Execute();

      #endregion
    }

    #region ProSnippet Group: Traverse Associations
    #endregion

    // cref: ArcGIS.Core.Data.UtilityNetwork.TraversalDirection
    // cref: ArcGIS.Core.Data.UtilityNetwork.TraverseAssociationsDescription.#ctor(ArcGIS.Core.Data.UtilityNetwork.TraversalDirection,System.Int32)
    // cref: ArcGIS.Core.Data.UtilityNetwork.UtilityNetwork.TraverseAssociations(System.Collections.Generic.IEnumerable{ArcGIS.Core.Data.UtilityNetwork.Element},ArcGIS.Core.Data.UtilityNetwork.TraverseAssociationsDescription)
    // cref: ArcGIS.Core.Data.UtilityNetwork.TraverseAssociationsResult
    // cref: ArcGIS.Core.Data.UtilityNetwork.TraverseAssociationsResult.Associations
    // cref: ArcGIS.Core.Data.UtilityNetwork.Association
    #region Get traverse associations result from downward traversal
    public static void GetTraverseAssociationsResultFromDownwardTraversal(UtilityNetwork utilityNetwork, IReadOnlyList<Element> startingElements)
    {
      // Set downward traversal with maximum depth
      TraverseAssociationsDescription traverseAssociationsDescription = new TraverseAssociationsDescription(TraversalDirection.Descending);

      // Get traverse associations result from the staring element up to maximum depth
      TraverseAssociationsResult traverseAssociationsResult = utilityNetwork.TraverseAssociations(startingElements, traverseAssociationsDescription);

      // Get associations participated in traversal
      IReadOnlyList<Association> associations = traverseAssociationsResult.Associations;
    }
    #endregion

    // cref: ArcGIS.Core.Data.UtilityNetwork.TraversalDirection
    // cref: ArcGIS.Core.Data.UtilityNetwork.TraverseAssociationsDescription.#ctor(ArcGIS.Core.Data.UtilityNetwork.TraversalDirection,System.Int32)
    // cref: ArcGIS.Core.Data.UtilityNetwork.UtilityNetwork.TraverseAssociations(System.Collections.Generic.IEnumerable{ArcGIS.Core.Data.UtilityNetwork.Element},ArcGIS.Core.Data.UtilityNetwork.TraverseAssociationsDescription)
    // cref: ArcGIS.Core.Data.UtilityNetwork.TraverseAssociationsResult
    // cref: ArcGIS.Core.Data.UtilityNetwork.TraverseAssociationsResult.Associations
    // cref: ArcGIS.Core.Data.UtilityNetwork.Association
    // cref: ArcGIS.Core.Data.UtilityNetwork.TraverseAssociationsResult.AdditionalFieldValues
    #region Get traverse associations result from upward traversal with depth limit
    public static void GetTraverseAssociationsResultFromUpwardTraversalWithDepthLimit(UtilityNetwork utilityNetwork, IReadOnlyList<Element> startingElements)
    {
      // List of fields whose values will be fetched as name-values pairs during association traversal operation
      List<string> additionalFieldsToFetch = new List<string> { "ObjectId", "AssetName", "AssetGroup", "AssetType" };

      // Set downward traversal with maximum depth level of 3 
      TraverseAssociationsDescription traverseAssociationsDescription = new TraverseAssociationsDescription(TraversalDirection.Ascending, 3) 
      { 
        AdditionalFields = additionalFieldsToFetch 
      };

      // Get traverse associations result from the staring element up to depth level 3
      TraverseAssociationsResult traverseAssociationsResult = utilityNetwork.TraverseAssociations(startingElements, traverseAssociationsDescription);

      // List of associations participated in traversal
      IReadOnlyList<Association> associations = traverseAssociationsResult.Associations;

      // KeyValue mapping between involved elements and their field name-values 
      //At 2.x - IReadOnlyDictionary<Element, IReadOnlyList<AssociationElementFieldValue>> associationElementValuePairs = traverseAssociationsResult.AdditionalFieldValues;
      IReadOnlyDictionary<Element, IReadOnlyList<FieldValue>> associationElementValuePairs = 
        traverseAssociationsResult.AdditionalFieldValues;

      foreach (KeyValuePair<Element, IReadOnlyList<FieldValue>> keyValuePair in associationElementValuePairs)
      {
        // Element 
        Element element = keyValuePair.Key;

        // List of field names and their values 
        //At 2.x - IReadOnlyList<AssociationElementFieldValue> elementFieldValues = keyValuePair.Value;
        IReadOnlyList<FieldValue> elementFieldValues = keyValuePair.Value;
      }
    }
    #endregion

    #region ProSnippet Group: Subnetworks and Tiers
    #endregion

    void FindATierFromDomainNetworkNameAndTierName(UtilityNetwork utilityNetwork, string domainNetworkName, string tierName)
    {
      // cref: ArcGIS.Core.Data.UtilityNetwork.UtilityNetwork.GetDefinition
      // cref: ArcGIS.Core.Data.UtilityNetwork.UtilityNetworkDefinition.GetDomainNetwork(System.String)
      // cref: ArcGIS.Core.Data.UtilityNetwork.DomainNetwork
      // cref: ArcGIS.Core.Data.UtilityNetwork.DomainNetwork.GetTier(System.String)
      #region Find a Tier given a Domain Network name and Tier name

      using (UtilityNetworkDefinition utilityNetworkDefinition = utilityNetwork.GetDefinition())
      {
        DomainNetwork domainNetwork = utilityNetworkDefinition.GetDomainNetwork(domainNetworkName);
        Tier tier = domainNetwork.GetTier(tierName);
      }
      
      #endregion
    }

    void UpdateAllDirtySubnetworks(UtilityNetwork utilityNetwork, Tier tier, MapView mapView)
    {
      // cref: ArcGIS.Core.Data.UtilityNetwork.Tier
      // cref: ArcGIS.Core.Data.UtilityNetwork.UtilityNetwork.GetSubnetworkManager()
      // cref: ArcGIS.Core.Data.UtilityNetwork.SubnetworkManager.UpdateAllSubnetworks(ArcGIS.Core.Data.UtilityNetwork.Tier, System.Boolean)
      // cref: ArcGIS.Desktop.Mapping.MapView.Redraw
      #region Update all dirty subnetworks in a tier

      using (SubnetworkManager subnetworkManager = utilityNetwork.GetSubnetworkManager())
      {
        subnetworkManager.UpdateAllSubnetworks(tier, true);

        mapView.Redraw(true);
      }
      
      #endregion
    }

    void RadialNetworkLifecycle(SubnetworkManager subnetworkManager, Tier mediumVoltageTier, Element elementR1)
    {
      // cref: ArcGIS.Core.Data.UtilityNetwork.UtilityNetworkExtensions.EnableControllerInEditOperation(ArcGIS.Core.Data.UtilityNetwork.SubnetworkManager, ArcGIS.Core.Data.UtilityNetwork.Tier, ArcGIS.Core.Data.UtilityNetwork.Element, System.String, System.String, System.String, System.String)
      // cref: ArcGIS.Core.Data.UtilityNetwork.Subnetwork
      // cref: ArcGIS.Core.Data.UtilityNetwork.Subnetwork.Update()
      // cref: ArcGIS.Core.Data.UtilityNetwork.UtilityNetworkExtensions.DisableControllerInEditOperation(ArcGIS.Core.Data.UtilityNetwork.SubnetworkManager,ArcGIS.Core.Data.UtilityNetwork.Element)
      // cref: ArcGIS.Desktop.Mapping.MapView.Redraw(System.Boolean)
      #region Life cycle for a simple radial subnetwork with one controller

      // Create a subnetwork named "Radial1" with a single controller
      // elementR1 represents the device that serves as the subnetwork controller (e.g., circuit breaker)
      Subnetwork subnetworkRadial1 = subnetworkManager.EnableControllerInEditOperation(mediumVoltageTier, elementR1, "Radial1", "R1", "my description", "my notes");

      // ...

      // Update the subnetwork and refresh the map
      subnetworkRadial1.Update();
      MapView.Active.Redraw(true);

      // ...

      // At some point, a subnetwork will need to be deleted.
      
      // First step is to disable the controller
      subnetworkManager.DisableControllerInEditOperation(elementR1);

      // At this point, the subnetwork is deleted, but all of the rows that have been labeled with the subnetwork ID need to be updated
      subnetworkRadial1.Update();
      MapView.Active.Redraw(true);

      // The final step is to notify external systems (if any) using the Export Subnetwork geoprocessing tool

      #endregion
    }

    void MeshNetworkLifeCycle(SubnetworkManager subnetworkManager, Tier lowVoltageMeshTier, Element elementM1, Element elementM2, Element elementM3)
    {
      // cref: ArcGIS.Core.Data.UtilityNetwork.SubnetworkManager.EnableController(ArcGIS.Core.Data.UtilityNetwork.Tier, ArcGIS.Core.Data.UtilityNetwork.Element, System.String, System.String, System.String, System.String)
      // cref: ArcGIS.Core.Data.UtilityNetwork.Subnetwork
      // cref: ArcGIS.Core.Data.UtilityNetwork.Subnetwork.Update()
      // cref: ArcGIS.Desktop.Mapping.MapView.Redraw(System.Boolean)
      // cref: ArcGIS.Core.Data.UtilityNetwork.UtilityNetworkExtensions.DisableControllerInEditOperation(ArcGIS.Core.Data.UtilityNetwork.SubnetworkManager,ArcGIS.Core.Data.UtilityNetwork.Element)
      #region Life cycle for a mesh subnetwork with multiple controllers

      // Create a subnetwork named "Mesh1" from three controllers
      // elementM1, elementM2, and elementM3 represent the devices that serve as subnetwork controllers (e.g., network protectors)
      subnetworkManager.EnableController(lowVoltageMeshTier, elementM1, "Mesh1", "M1", "my description", "my notes");
      subnetworkManager.EnableController(lowVoltageMeshTier, elementM2, "Mesh1", "M2", "my description", "my notes");
      Subnetwork subnetworkMesh1 = subnetworkManager.EnableController(lowVoltageMeshTier, elementM3, "Mesh1", "M3", "my description", "my notes");
      subnetworkMesh1.Update();
      MapView.Active.Redraw(true);

      // ...

      // When deleting the subnetwork, each controller must be disabled before the subnetwork itself is deleted
      subnetworkManager.DisableControllerInEditOperation(elementM1);
      subnetworkManager.DisableControllerInEditOperation(elementM2);
      subnetworkManager.DisableControllerInEditOperation(elementM3);

      // After the subnetwork is deleted, all of the rows that have been labeled with the subnetwork ID need to be updated
      subnetworkMesh1.Update();
      MapView.Active.Redraw(true);

      // The final step is to notify external systems (if any) using the Export Subnetwork geoprocessing tool
      #endregion
    }

    void MultifeedRadialLifeCycle(SubnetworkManager subnetworkManager, Tier mediumVoltageTier, Element elementR2, Element elementR3)
    {
      // cref: ArcGIS.Core.Data.UtilityNetwork.UtilityNetworkExtensions.EnableControllerInEditOperation(ArcGIS.Core.Data.UtilityNetwork.SubnetworkManager, ArcGIS.Core.Data.UtilityNetwork.Tier, ArcGIS.Core.Data.UtilityNetwork.Element, System.String, System.String, System.String, System.String)
      // cref: ArcGIS.Core.Data.UtilityNetwork.UtilityNetworkExtensions.DisableControllerInEditOperation(ArcGIS.Core.Data.UtilityNetwork.SubnetworkManager,ArcGIS.Core.Data.UtilityNetwork.Element)
      // cref: ArcGIS.Core.Data.UtilityNetwork.Subnetwork
      // cref: ArcGIS.Core.Data.UtilityNetwork.Subnetwork.Update()
      // cref: ArcGIS.Desktop.Mapping.MapView.Redraw(System.Boolean)
      #region Life cycle for a multifeed radial subnetwork with two controllers

      // Create a subnetwork named "R2, R3" from two controllers
      // elementR2 and elementR3 represent the devices that serve as subnetwork controllers (e.g., circuit breakers)
      subnetworkManager.EnableControllerInEditOperation(mediumVoltageTier, elementR2, "R2, R3", "R2", "my description", "my notes");
      subnetworkManager.EnableControllerInEditOperation(mediumVoltageTier, elementR3, "R2, R3", "R3", "my description", "my notes");

      // If the tie switch between them is opened, the original subnetwork controllers must be disabled and re-enabled with different names
      // This will create two new subnetworks, named "R2" and "R3"
      subnetworkManager.DisableControllerInEditOperation(elementR2);
      subnetworkManager.DisableControllerInEditOperation(elementR3);

      Subnetwork subnetworkR2 = subnetworkManager.EnableControllerInEditOperation(mediumVoltageTier, elementR2, "R2", "R2", "my description", "my notes");
      Subnetwork subnetworkR3 = subnetworkManager.EnableControllerInEditOperation(mediumVoltageTier, elementR3, "R3", "R3", "my description", "my notes");

      subnetworkR2.Update();
      subnetworkR3.Update();
      MapView.Active.Redraw(true);

      #endregion
    }

    #region ProSnippet Group: Tracing
    #endregion

    void CreateADownstreamTracerObject(UtilityNetwork utilityNetwork)
    {
      // cref: ArcGIS.Core.Data.UtilityNetwork.UtilityNetwork.GetTraceManager()
      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.TraceManager
      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.TraceManager.GetTracer()
      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.DownstreamTracer
      #region Create a DownstreamTracer

      using (TraceManager traceManager = utilityNetwork.GetTraceManager())
      {
        DownstreamTracer downstreamTracer = traceManager.GetTracer<DownstreamTracer>();
      }

      #endregion
    }

    void CreateTraceArgument()
    {
      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.TraceArgument
      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.TraceArgument.#ctor(System.Collections.Generic.IEnumerable<ArcGIS.Core.Data.UtilityNetwork.Element>)
      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.TraceConfiguration
      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.TraceConfiguration.#ctor()
      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.TraceArgument.Configuration
      #region Create a Trace Argument

      IReadOnlyList<Element> startingPointList = new List<Element>();

      // Code to fill in list of starting points goes here...
      TraceArgument traceArgument = new TraceArgument(startingPointList);

      TraceConfiguration traceConfiguration = new TraceConfiguration();

      // Code to fill in trace configuration goes here...
      traceArgument.Configuration = traceConfiguration;

      #endregion
    }

    private void CreateNetworkAttributeComparison(UtilityNetworkDefinition utilityNetworkDefinition, TraceConfiguration traceConfiguration)
    {
      const int InDesign = 4;
      const int InService = 8;

      // cref: ArcGIS.Core.Data.UtilityNetwork.UtilityNetworkDefinition.GetNetworkAttribute(System.String)
      // cref: ArcGIS.Core.Data.UtilityNetwork.NetworkAttribute
      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.NetworkAttributeComparison.#ctor(ArcGIS.Core.Data.UtilityNetwork.NetworkAttribute, ArcGIS.Core.Data.UtilityNetwork.Trace.Operator, System.Object)
      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.Operator
      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.NetworkAttributeComparison
      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.And.#ctor(ArcGIS.Core.Data.UtilityNetwork.Trace.ConditionalExpression, ArcGIS.Core.Data.UtilityNetwork.Trace.ConditionalExpression)
      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.TraceConfiguration.Traversability
      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.Traversability.Barriers
      #region Create a Condition to compare a Network Attribute against a set of values

      // Create a NetworkAttribute object for the Lifecycle network attribute from the UtilityNetworkDefinition
      using (NetworkAttribute lifecycleNetworkAttribute = utilityNetworkDefinition.GetNetworkAttribute("Lifecycle"))
      {
        // Create a NetworkAttributeComparison that stops traversal if Lifecycle <> "In Design" (represented by the constant InDesign)
        NetworkAttributeComparison inDesignNetworkAttributeComparison = new NetworkAttributeComparison(lifecycleNetworkAttribute, Operator.NotEqual, InDesign);

        // Create a NetworkAttributeComparison to stop traversal if Lifecycle <> "In Service" (represented by the constant InService)
        NetworkAttributeComparison inServiceNetworkAttributeComparison = new NetworkAttributeComparison(lifecycleNetworkAttribute, Operator.NotEqual, InService);

        // Combine these two comparisons together with "And"
        And lifecycleFilter = new And(inDesignNetworkAttributeComparison, inServiceNetworkAttributeComparison);
        
        // Final condition stops traversal if Lifecycle <> "In Design" and Lifecycle <> "In Service"
        traceConfiguration.Traversability.Barriers = lifecycleFilter;
      }

      #endregion
    }
    
    private void ApplyFunction(UtilityNetworkDefinition utilityNetworkDefinition, TraceConfiguration traceConfiguration)
    {
      // cref: ArcGIS.Core.Data.UtilityNetwork.UtilityNetworkDefinition.GetNetworkAttribute(System.String)
      // cref: ArcGIS.Core.Data.UtilityNetwork.NetworkAttribute
      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.Add.#ctor(ArcGIS.Core.Data.UtilityNetwork.NetworkAttribute)
      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.Function
      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.TraceConfiguration.Functions
      #region Create a Function

      // Get a NetworkAttribute object for the Load network attribute from the UtilityNetworkDefinition
      using (NetworkAttribute loadNetworkAttribute = utilityNetworkDefinition.GetNetworkAttribute("Load"))
      {
        // Create a function to sum the Load
        Add sumLoadFunction = new Add(loadNetworkAttribute);

        // Add this function to our trace configuration
        traceConfiguration.Functions = new List<Function>() { sumLoadFunction };
      }

      #endregion
    }

    private void CreateFunctionBarrier(UtilityNetworkDefinition utilityNetworkDefinition, TraceConfiguration traceConfiguration)
    {
      // cref: ArcGIS.Core.Data.UtilityNetwork.UtilityNetworkDefinition.GetNetworkAttribute(System.String)
      // cref: ArcGIS.Core.Data.UtilityNetwork.NetworkAttribute
      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.Add.#ctor(ArcGIS.Core.Data.UtilityNetwork.NetworkAttribute)
      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.FunctionBarrier.#ctor(ArcGIS.Core.Data.UtilityNetwork.Trace.Function, ArcGIS.Core.Data.UtilityNetwork.Trace.Operator, System.Double)
      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.Function
      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.TraceConfiguration.Traversability
      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.Traversability.FunctionBarriers
      #region Create a FunctionBarrier

      // Create a NetworkAttribute object for the Shape length network attribute from the UtilityNetworkDefinition
      using (NetworkAttribute shapeLengthNetworkAttribute = utilityNetworkDefinition.GetNetworkAttribute("Shape length"))
      {
        // Create a function that adds up shape length
        Add lengthFunction = new Add(shapeLengthNetworkAttribute);

        // Create a function barrier that stops traversal after 1000 feet
        FunctionBarrier distanceBarrier = new FunctionBarrier(lengthFunction, Operator.GreaterThan, 1000.0);

        // Set this function barrier
        traceConfiguration.Traversability.FunctionBarriers = new List<FunctionBarrier>() { distanceBarrier };
      }

      #endregion
    }

    private void CreateOutputFilter(TraceConfiguration traceConfiguration)
    {
      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.CategoryOperator
      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.CategoryComparison.#ctor(ArcGIS.Core.Data.UtilityNetwork.Trace.CategoryOperator, System.String)
      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.TraceConfiguration.OutputCondition
      #region Create an output condition

      // Create an output category to filter the trace results to only include
      // features with the "Service Point" category assigned
      traceConfiguration.OutputCondition = new CategoryComparison(CategoryOperator.IsEqual, "Service Point");

      #endregion
    }

    private void CreatePropagator(UtilityNetworkDefinition utilityNetworkDefinition, TraceConfiguration traceConfiguration)
    {
      const int ABCPhase = 7;

      // cref: ArcGIS.Core.Data.UtilityNetwork.UtilityNetworkDefinition.GetNetworkAttribute(System.String)
      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.Propagator
      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.PropagatorFunction
      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.Propagator.#ctor(ArcGIS.Core.Data.UtilityNetwork.NetworkAttribute, ArcGIS.Core.Data.UtilityNetwork.Trace.PropagatorFunction, ArcGIS.Core.Data.UtilityNetwork.Trace.Operator, System.Double)
      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.TraceConfiguration.Propagators
      #region Create a Propagator

      // Get a NetworkAttribute object for the Phases Normal attribute from the UtilityNetworkDefinition
      using (NetworkAttribute normalPhaseAttribute = utilityNetworkDefinition.GetNetworkAttribute("Phases Normal"))
      {
        // Create a propagator to propagate the Phases Normal attribute downstream from the source, using a Bitwise And function
        // Allow traversal to continue as long as the Phases Normal value includes any of the ABC phases
        // (represented by the constant ABCPhase)
        Propagator phasePropagator = new Propagator(normalPhaseAttribute, PropagatorFunction.BitwiseAnd, Operator.IncludesAny, ABCPhase);

        // Assign this propagator to our trace configuration
        traceConfiguration.Propagators = new List<Propagator>() { phasePropagator };
      }

      #endregion
    }

    private void UseFunctionResults(IReadOnlyList<Result> traceResults)
    {
      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.FunctionOutputResult
      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.FunctionOutputResult.FunctionOutputs
      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.FunctionOutput
      // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.FunctionOutput.Value
      #region Using Function Results

      // Get the FunctionOutputResult from the trace results
      FunctionOutputResult functionOutputResult = traceResults.OfType<FunctionOutputResult>().First();

      // First() can be used here if only one Function was included in the TraceConfiguration.Functions collection.
      // Otherwise you will have to search the list for the correct FunctionOutput object.
      FunctionOutput functionOutput = functionOutputResult.FunctionOutputs.First();

      // Extract the total load from the GlobalValue property
      double totalLoad = (double)functionOutput.Value;

      #endregion
    }

    // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.NamedTraceConfigurationQuery.#ctor
    // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.NamedTraceConfigurationQuery.Names
    // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.TraceManager.GetNamedTraceConfigurations(ArcGIS.Core.Data.UtilityNetwork.Trace.NamedTraceConfigurationQuery)
    // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.NamedTraceConfiguration
    #region Fetch a named trace configuration by name
    private void GetNamedTraceConfigurationsByName(UtilityNetwork utilityNetwork, string configurationName)
    {
      // Query to find named trace configurations
      NamedTraceConfigurationQuery namedTraceConfigurationQuery = new NamedTraceConfigurationQuery { Names = new List<string> { configurationName } };

      // Get the trace manager from the utility network
      using (TraceManager traceManager = utilityNetwork.GetTraceManager())
      {
        // A set of named trace configurations specified by the named traced configuration query 
        IReadOnlyList<NamedTraceConfiguration> namedTraceConfigurations = traceManager.GetNamedTraceConfigurations(namedTraceConfigurationQuery);

        foreach (NamedTraceConfiguration namedTraceConfiguration in namedTraceConfigurations)
        {
          // Use NamedTraceConfiguration's object
        }
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.UtilityNetworkLayer.GetNamedTraceConfigurations()
    // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.NamedTraceConfiguration
    #region Fetch named trace configurations from a utility network layer
    private void GetNamedTraceConfigurationsFromUtilityNetworkLayer(UtilityNetworkLayer utilityNetworkLayer)
    {
      // Get all named trace configurations in the utility network
      IReadOnlyList<NamedTraceConfiguration> namedTraceConfigurations = utilityNetworkLayer.GetNamedTraceConfigurations();

      foreach (NamedTraceConfiguration namedTraceConfiguration in namedTraceConfigurations)
      {
        // Use NamedTraceConfiguration's object
      }
    }
    #endregion

    // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.TraceManager.GetTracer(ArcGIS.Core.Data.UtilityNetwork.Trace.NamedTraceConfiguration)
    // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.Tracer
    // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.TraceArgument.#ctor(ArcGIS.Core.Data.UtilityNetwork.Trace.NamedTraceConfiguration,IEnumerable{ArcGIS.Core.Data.UtilityNetwork.Element})
    // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.Tracer.Trace(ArcGIS.Core.Data.UtilityNetwork.Trace.TraceArgument)
    // cref: ArcGIS.Core.Data.UtilityNetwork.Trace.Result
    #region Trace a utility network using a named trace configuration
    private void TraceUtilityNetworkUsingNamedTraceConfiguration(UtilityNetwork utilityNetwork, NamedTraceConfiguration namedTraceConfiguration, Element startElement)
    {
      // Get the trace manager from the utility network
      using (TraceManager traceManager = utilityNetwork.GetTraceManager())
      {
        // Get a tracer from the trace manager using the named trace configuration
        Tracer upstreamTracer = traceManager.GetTracer(namedTraceConfiguration);
        
        // Trace argument holding the trace input parameters
        TraceArgument upstreamTraceArgument = new TraceArgument(namedTraceConfiguration, new List<Element> {startElement});
        
        // Trace results 
        IReadOnlyList<Result> upstreamTraceResults = upstreamTracer.Trace(upstreamTraceArgument);
      }
    }
    #endregion



    #region ProSnippet Group: Network Diagrams
    #endregion

    // cref: ArcGIS.Core.Data.UtilityNetwork.UtilityNetwork.GetDiagramManager
    // cref: ArcGIS.Core.Data.NetworkDiagrams.DiagramManager
    public void GetDiagramManager(UtilityNetwork utilityNetwork)
    {
      #region Get the Diagram Manager
      using (DiagramManager diagramManager = utilityNetwork.GetDiagramManager())
      {
        // Todo - do something
      }
      #endregion
    }

    private string diagrameName = "";
    ArcGIS.Core.Geometry.Envelope extentOfInterest = null;

    // cref: ArcGIS.Core.Data.UtilityNetwork.UtilityNetwork.GetDiagramManager
    // cref: ArcGIS.Core.Data.NetworkDiagrams.DiagramManager
    // cref: ArcGIS.Core.Data.NetworkDiagrams.NetworkDiagram
    // cref: ArcGIS.Core.Data.NetworkDiagrams.DiagramManager.GetNetworkDiagrams()
    // cref: ArcGIS.Core.Data.NetworkDiagrams.DiagramManager.GetNetworkDiagram(System.String)
    // cref: ArcGIS.Core.Data.NetworkDiagrams.DiagramManager.GetNetworkDiagrams(ArcGIS.Core.Geometry.Envelope)
    // cref: ArcGIS.Core.Data.NetworkDiagrams.DiagramManager.GetNetworkDiagrams(System.Collections.Generic.IEnumerable<System.Guid>)
    // cref: ArcGIS.Core.Data.NetworkDiagrams.DiagramManager.GetNetworkDiagrams(ArcGIS.Core.Geometry.Envelope, System.Collections.Generic.IEnumerable<System.Guid>)
    public void GetDiagram(UtilityNetwork utilityNetwork, IEnumerable<Guid> globalIDs)
    {
      #region Get Network Diagrams
      using (DiagramManager diagramManager = utilityNetwork.GetDiagramManager())
      {
        // get all the diagrams
        IReadOnlyList<NetworkDiagram> diagrams = diagramManager.GetNetworkDiagrams();

        // get a diagram by name
        NetworkDiagram diagram = diagramManager.GetNetworkDiagram(diagrameName);

        // get diagrams by extent
        diagrams = diagramManager.GetNetworkDiagrams(extentOfInterest);

        // get diagrams from a set of utility network feature GlobalIDs
        diagrams = diagramManager.GetNetworkDiagrams(globalIDs);

        // get diagrams from a set of utility network feature GlobalIDs within an extent
        diagrams = diagramManager.GetNetworkDiagrams(extentOfInterest, globalIDs);
      }
      #endregion
    }

    // cref: ArcGIS.Core.Data.UtilityNetwork.UtilityNetwork.GetDiagramManager
    // cref: ArcGIS.Core.Data.NetworkDiagrams.DiagramManager
    // cref: ArcGIS.Core.Data.NetworkDiagrams.NetworkDiagram
    // cref: ArcGIS.Core.Data.NetworkDiagrams.DiagramManager.GetNetworkDiagrams()
    // cref: ArcGIS.Core.Data.NetworkDiagrams.NetworkDiagram.GetDiagramInfo()
    // cref: ArcGIS.Core.Data.NetworkDiagrams.NetworkDiagramInfo
    // cref: ArcGIS.Core.Data.NetworkDiagrams.NetworkDiagramInfo.IsSystem
    // cref: ArcGIS.Core.Data.NetworkDiagrams.NetworkDiagram.GetConsistencyState()
    // cref: ArcGIS.Core.Data.NetworkDiagrams.NetworkDiagramConsistencyState
    #region Get a list of Network Diagrams with inconsistent ConsistencyState
    public List<NetworkDiagram> GetInconsistentDiagrams(UtilityNetwork utilityNetwork)
    {
      // Get the DiagramManager from the utility network

      using (DiagramManager diagramManager = utilityNetwork.GetDiagramManager())
      {
        List<NetworkDiagram> myList = new List<NetworkDiagram>();

        // Loop through the network diagrams in the diagram manager

        foreach (NetworkDiagram diagram in diagramManager.GetNetworkDiagrams())
        {
          NetworkDiagramInfo diagramInfo = diagram.GetDiagramInfo();

          // If the diagram is not a system diagram and is in an inconsistent state, add it to our list

          if (!diagramInfo.IsSystem && diagram.GetConsistencyState() != NetworkDiagramConsistencyState.DiagramIsConsistent)
          {
            myList.Add(diagram);
          }
          else
          {
            diagram.Dispose(); // If we are not returning it we need to Dispose it
          }
        }

        return myList;
      }
    }
    #endregion

    public async void CreateDiagramLayerFromNetworkDiagram(NetworkDiagram myDiagram)
    {
      // cref: ArcGIS.Core.Data.NetworkDiagrams.NetworkDiagram
      // cref: ArcGIS.Desktop.Mapping.MapFactory.CreateMap(System.String,ArcGIS.Core.CIM.MapType,ArcGIS.Core.CIM.MapViewingMode,ArcGIS.Desktop.Mapping.Basemap)
      // cref: ArcGIS.Desktop.Mapping.MappingExtensions.AddDiagramLayer(ArcGIS.Desktop.Mapping.Map, ArcGIS.Core.Data.NetworkDiagrams.NetworkDiagram)
      #region Open a diagram pane from a Network Diagram

      // Create a diagram layer from a NetworkDiagram (myDiagram)
      DiagramLayer diagramLayer = await QueuedTask.Run<DiagramLayer>(() =>
      {
        // Create the diagram map
        var newMap = MapFactory.Instance.CreateMap(myDiagram.Name, ArcGIS.Core.CIM.MapType.NetworkDiagram, MapViewingMode.Map);
        if (newMap == null)
          return null;

        // Open the diagram map
        var mapPane = ArcGIS.Desktop.Core.ProApp.Panes.CreateMapPaneAsync(newMap, MapViewingMode.Map);
        if (mapPane == null)
          return null;

        //Add the diagram to the map
        return newMap.AddDiagramLayer(myDiagram);
      });

      #endregion
    }

    // cref: ArcGIS.Desktop.Mapping.DiagramLayer.GetNetworkDiagram
    // cref: ArcGIS.Core.Data.NetworkDiagrams.NetworkDiagram
    // cref: ArcGIS.Desktop.Mapping.DiagramLayer.ConsistencyState
    // cref: ArcGIS.Core.Data.NetworkDiagrams.NetworkDiagram.GetConsistencyState
    // cref: ArcGIS.Desktop.Mapping.DiagramLayerConsistencyState
    #region Get Diagram from DiagramLayer
    public void GetDiagram(DiagramLayer diagramLayer)
    {
      // note - methods need to run on MCT

      NetworkDiagram diagram = diagramLayer.GetNetworkDiagram();

      // get the consistency state from the layer
      DiagramLayerConsistencyState dlState = diagramLayer.ConsistencyState;

      // or from the diagram
      NetworkDiagramConsistencyState ndState = diagram.GetConsistencyState();
    }
    #endregion

    private string templateName = "";

    // cref: ArcGIS.Core.Data.UtilityNetwork.UtilityNetwork.GetDiagramManager
    // cref: ArcGIS.Core.Data.NetworkDiagrams.DiagramManager
    // cref: ArcGIS.Core.Data.NetworkDiagrams.DiagramTemplate
    // cref: ArcGIS.Core.Data.NetworkDiagrams.DiagramManager.GetDiagramTemplates()
    // cref: ArcGIS.Core.Data.NetworkDiagrams.DiagramManager.GetDiagramTemplate(System.String)
    #region Get Diagram Templates

    public void RetrieveDiagramTemplates(UtilityNetwork utilityNetwork)
    {
      using (DiagramManager diagramManager = utilityNetwork.GetDiagramManager())
      {
        // get all templates
        IReadOnlyList<DiagramTemplate> templates = diagramManager.GetDiagramTemplates();

        // get a template by name
        DiagramTemplate template = diagramManager.GetDiagramTemplate(templateName);
      }
    }
    #endregion

    // cref: ArcGIS.Core.Data.UtilityNetwork.UtilityNetwork.GetDiagramManager
    // cref: ArcGIS.Core.Data.NetworkDiagrams.DiagramManager
    // cref: ArcGIS.Core.Data.NetworkDiagrams.DiagramManager.GetDiagramTemplates()
    // cref: ArcGIS.Core.Data.NetworkDiagrams.DiagramTemplate.GetNetworkDiagrams()
    // cref: ArcGIS.Core.Data.NetworkDiagrams.DiagramTemplate.GetNetworkDiagram(System.String)
    #region Get Network Diagrams from a Diagram Template

    public void GetNetworkDiagramFromDiagramTemplates(UtilityNetwork utilityNetwork)
    {
      using (DiagramManager diagramManager = utilityNetwork.GetDiagramManager())
      {
        // get the first templates
        DiagramTemplate template = diagramManager.GetDiagramTemplates().FirstOrDefault();

        // get the network diagrams fromt he template
        IEnumerable<NetworkDiagram> diagrams = template.GetNetworkDiagrams();

        // or get a network diagram by name
        NetworkDiagram diagram = template.GetNetworkDiagram(diagrameName);
      }
    }
    #endregion

    // cref: ArcGIS.Core.Data.UtilityNetwork.UtilityNetwork.GetDiagramManager
    // cref: ArcGIS.Core.Data.NetworkDiagrams.DiagramManager
    // cref: ArcGIS.Core.Data.NetworkDiagrams.DiagramManager.CreateNetworkDiagram(ArcGIS.Core.Data.NetworkDiagrams.DiagramTemplate, System.Collections.Generic.IEnumerable<System.Guid>)
    // cref: ArcGIS.Core.Data.NetworkDiagrams.NetworkDiagram
    #region Create a Network Diagram
    public void CreateNetworkDiagram(UtilityNetwork utilityNetwork, IEnumerable<Guid> globalIDs)
    {
      using (DiagramManager diagramManager = utilityNetwork.GetDiagramManager())
      {
        // get the template
        DiagramTemplate template = diagramManager.GetDiagramTemplate(templateName);

        // create the diagram
        NetworkDiagram diagram = diagramManager.CreateNetworkDiagram(template, globalIDs);
      }
    }
    #endregion

    // cref: ArcGIS.Core.Data.UtilityNetwork.UtilityNetwork.GetDiagramManager
    // cref: ArcGIS.Core.Data.NetworkDiagrams.DiagramManager
    // cref: ArcGIS.Core.Data.NetworkDiagrams.DiagramTemplate.GetNetworkDiagrams
    // cref: ArcGIS.Core.Data.NetworkDiagrams.NetworkDiagram
    // cref: ArcGIS.Core.Data.NetworkDiagrams.NetworkDiagram.GetContent(System.Boolean, System.Boolean, System.Boolean, System.Boolean)
    #region Get Network Diagram Information as JSON string
    public void GetDiagramContent(UtilityNetwork utilityNetwork)
    {
      using (DiagramManager diagramManager = utilityNetwork.GetDiagramManager())
      {
        // get a diagram by name
        NetworkDiagram diagram = diagramManager.GetNetworkDiagram(templateName);

        string json_content = diagram.GetContent(true, true, true, true);
      }
    }

    #endregion


    // cref: ArcGIS.Core.Data.NetworkDiagrams.DiagramElementQueryByExtent
    // cref: ArcGIS.Core.Data.NetworkDiagrams.DiagramElementQueryByExtent.#ctor
    // cref: ArcGIS.Core.Data.NetworkDiagrams.DiagramElementQueryByExtent.ExtentOfInterest
    // cref: ArcGIS.Core.Data.NetworkDiagrams.DiagramElementQueryByExtent.AddContents
    // cref: ArcGIS.Core.Data.NetworkDiagrams.DiagramElementQueryByElementTypes.QueryDiagramJunctionElement
    // cref: ArcGIS.Core.Data.NetworkDiagrams.DiagramElementQueryByElementTypes.QueryDiagramEdgeElement
    // cref: ArcGIS.Core.Data.NetworkDiagrams.DiagramElementQueryByElementTypes.QueryDiagramContainerElement
    // cref: ArcGIS.Core.Data.NetworkDiagrams.NetworkDiagram.QueryDiagramElements(ArcGIS.Core.Data.NetworkDiagrams.DiagramElementQueryByExtent)
    // cref: ArcGIS.Core.Data.NetworkDiagrams.DiagramElementQueryResult
    // cref: ArcGIS.Core.Data.NetworkDiagrams.DiagramElementQueryResult.DiagramContainerElements
    // cref: ArcGIS.Core.Data.NetworkDiagrams.DiagramElementQueryResult.DiagramJunctionElements
    // cref: ArcGIS.Core.Data.NetworkDiagrams.DiagramElementQueryResult.DiagramEdgeElements
    #region Get Diagram Elements
    public void GetDiagramElements(MapView mapView, NetworkDiagram networkDiagram)
    {
      // Create a DiagramElementQueryByExtent to retrieve diagram element junctions whose extent
      // intersects the active map extent

      DiagramElementQueryByExtent elementQuery = new DiagramElementQueryByExtent();
      elementQuery.ExtentOfInterest = MapView.Active.Extent;
      elementQuery.AddContents = false;
      elementQuery.QueryDiagramJunctionElement = true;
      elementQuery.QueryDiagramEdgeElement = false;
      elementQuery.QueryDiagramContainerElement = false;

      // Use this DiagramElementQueryByExtent as an argument to the QueryDiagramElements method
      DiagramElementQueryResult result = networkDiagram.QueryDiagramElements(elementQuery);

      // get the container, junction, edge elements
      //    in this case result.DiagramJunctionElements and result.DiagramEdgeElements will be empty 
      //    since elementQuery.QueryDiagramEdgeElement and elementQuery.QueryDiagramContainerElement are set to false
      IReadOnlyList<DiagramContainerElement> containerElements = result.DiagramContainerElements;

      IReadOnlyList<DiagramJunctionElement> junctionElements = result.DiagramJunctionElements;

      IReadOnlyList<DiagramEdgeElement> edgeElements = result.DiagramEdgeElements;
    }
    #endregion

    // cref: ArcGIS.Core.Data.NetworkDiagrams.NetworkDiagram.GetAggregations()
    // cref: ArcGIS.Core.Data.NetworkDiagrams.DiagramAggregation
    // cref: ArcGIS.Core.Data.NetworkDiagrams.DiagramAggregation.AggregationType
    #region Get Diagram Aggregations

    public void GetDiagramAggregation(NetworkDiagram networkDiagram)
    {
      IReadOnlyList<DiagramAggregation> aggregations = networkDiagram.GetAggregations();
      foreach (var aggregation in aggregations)
      {
        var type = aggregation.AggregationType;
      }
    }
    #endregion

    // cref: ArcGIS.Core.Data.NetworkDiagrams.FindDiagramFeatureQuery
    // cref: ArcGIS.Core.Data.NetworkDiagrams.FindDiagramFeatureQuery.#ctor
    // cref: ArcGIS.Core.Data.NetworkDiagrams.FindDiagramFeatureQuery.NetworkRowGlobalIDs
    // cref: ArcGIS.Core.Data.NetworkDiagrams.FindDiagramFeatureQuery.AddAggregations
    // cref: ArcGIS.Core.Data.NetworkDiagrams.FindDiagramFeatureQuery.AddConnectivityAssociations
    // cref: ArcGIS.Core.Data.NetworkDiagrams.FindDiagramFeatureQuery.AddStructuralAttachments
    // cref: ArcGIS.Core.Data.NetworkDiagrams.NetworkDiagram.FindDiagramFeatures(ArcGIS.Core.Data.NetworkDiagrams.FindDiagramFeatureQuery)
    // cref: ArcGIS.Core.Data.NetworkDiagrams.FindResultItem
    // cref: ArcGIS.Core.Data.NetworkDiagrams.FindResultItem.SourceID
    // cref: ArcGIS.Core.Data.NetworkDiagrams.FindResultItem.ObjectID
    // cref: ArcGIS.Core.Data.NetworkDiagrams.FindResultItem.GlobalID
    // cref: ArcGIS.Core.Data.NetworkDiagrams.FindResultItem.GeometryType
    #region Find Diagram Features for a set of utility network rows
    public void FindDiagramFeatures(NetworkDiagram diagram, List<Guid> globalIDs)
    {
      FindDiagramFeatureQuery featureQuery = new FindDiagramFeatureQuery();
      featureQuery.NetworkRowGlobalIDs = globalIDs;
      featureQuery.AddAggregations = true;
      featureQuery.AddConnectivityAssociations = true;
      featureQuery.AddStructuralAttachments = true;

      IReadOnlyList<FindResultItem> features = diagram.FindDiagramFeatures(featureQuery);
      foreach (var findFeature in features)
      {
        long objectID = findFeature.ObjectID;
        Guid guid = findFeature.GlobalID;
        GeometryType geometryType = findFeature.GeometryType;
        int sourceID = findFeature.SourceID;
      }
    }
    #endregion

    // cref: ArcGIS.Core.Data.NetworkDiagrams.FindNetworkRowQuery
    // cref: ArcGIS.Core.Data.NetworkDiagrams.FindNetworkRowQuery.#ctor
    // cref: ArcGIS.Core.Data.NetworkDiagrams.FindNetworkRowQuery.DiagramFeatureGlobalIDs
    // cref: ArcGIS.Core.Data.NetworkDiagrams.FindNetworkRowQuery.AddAggregations
    // cref: ArcGIS.Core.Data.NetworkDiagrams.NetworkDiagram.FindNetworkRows(ArcGIS.Core.Data.NetworkDiagrams.FindNetworkRowQuery)
    // cref: ArcGIS.Core.Data.NetworkDiagrams.FindResultItem
    // cref: ArcGIS.Core.Data.NetworkDiagrams.FindResultItem.SourceID
    // cref: ArcGIS.Core.Data.NetworkDiagrams.FindResultItem.ObjectID
    // cref: ArcGIS.Core.Data.NetworkDiagrams.FindResultItem.GlobalID
    // cref: ArcGIS.Core.Data.NetworkDiagrams.FindResultItem.GeometryType
    #region Find Utility Network Rows for a set of diagram features
    public void FindDiagramRows(NetworkDiagram diagram, List<Guid> globalIDs)
    {
      FindNetworkRowQuery rowQuery = new FindNetworkRowQuery();
      rowQuery.DiagramFeatureGlobalIDs = globalIDs;
      rowQuery.AddAggregations = true;

      IReadOnlyList<FindResultItem> rows = diagram.FindNetworkRows(rowQuery);
      foreach (var findRow in rows)
      {
        long objectID = findRow.ObjectID;
        Guid guid = findRow.GlobalID;
        GeometryType geometryType = findRow.GeometryType;
        int sourceID = findRow.SourceID;
      }
    }
    #endregion

    // cref: ArcGIS.Core.Data.NetworkDiagrams.NetworkDiagram.FindInitialNetworkRows()
    // cref: ArcGIS.Core.Data.NetworkDiagrams.FindResultItem
    // cref: ArcGIS.Core.Data.NetworkDiagrams.FindResultItem.SourceID
    // cref: ArcGIS.Core.Data.NetworkDiagrams.FindResultItem.ObjectID
    // cref: ArcGIS.Core.Data.NetworkDiagrams.FindResultItem.GlobalID
    // cref: ArcGIS.Core.Data.NetworkDiagrams.FindResultItem.GeometryType
    #region Find Initial Network Rows Used to create a Network Diagram
    public void FindInitialNetworkRows(NetworkDiagram diagram)
    {
      IReadOnlyList<FindResultItem> rows = diagram.FindInitialNetworkRows();
      foreach (var findRow in rows)
      {
        long objectID = findRow.ObjectID;
        Guid guid = findRow.GlobalID;
        GeometryType geometryType = findRow.GeometryType;
        int sourceID = findRow.SourceID;
      }
    }
    #endregion

    void TranslateDiagramElements(NetworkDiagramSubset subset)
    { }

    // cref: ArcGIS.Core.Data.NetworkDiagrams.DiagramManager.GetNetworkDiagram(System.String)
    // cref: ArcGIS.Core.Data.NetworkDiagrams.NetworkDiagram
    // cref: ArcGIS.Core.Data.NetworkDiagrams.DiagramElementQueryByElementTypes
    // cref: ArcGIS.Core.Data.NetworkDiagrams.DiagramElementQueryByElementTypes.#ctor
    // cref: ArcGIS.Core.Data.NetworkDiagrams.DiagramElementQueryByElementTypes.QueryDiagramJunctionElement
    // cref: ArcGIS.Core.Data.NetworkDiagrams.DiagramElementQueryByElementTypes.QueryDiagramEdgeElement
    // cref: ArcGIS.Core.Data.NetworkDiagrams.DiagramElementQueryByElementTypes.QueryDiagramContainerElement
    // cref: ArcGIS.Core.Data.NetworkDiagrams.NetworkDiagram.QueryDiagramElements(ArcGIS.Core.Data.NetworkDiagrams.DiagramElementQueryByElementTypes)
    // cref: ArcGIS.Core.Data.NetworkDiagrams.NetworkDiagramSubset
    // cref: ArcGIS.Core.Data.NetworkDiagrams.NetworkDiagramSubset.#ctor
    // cref: ArcGIS.Core.Data.NetworkDiagrams.NetworkDiagramSubset.DiagramJunctionElements
    // cref: ArcGIS.Core.Data.NetworkDiagrams.NetworkDiagramSubset.DiagramEdgeElements
    // cref: ArcGIS.Core.Data.NetworkDiagrams.NetworkDiagramSubset.DiagramContainerElements
    // cref: ArcGIS.Core.Data.NetworkDiagrams.NetworkDiagram.SaveLayout(ArcGIS.Core.Data.NetworkDiagrams.NetworkDiagramSubset, System.Boolean)
    #region Change the Layout of a Network Diagram
    public void DiagramElementQueryResultAndNetworkDiagramSubsetClasses(Geodatabase geodatabase, DiagramManager diagramManager, string diagramName)
    {
      // Retrieve a diagram
      using (NetworkDiagram diagramTest = diagramManager.GetNetworkDiagram(diagramName))
      {
        // Create a DiagramElementQueryByElementTypes query object to get the diagram elements we want to work with
        DiagramElementQueryByElementTypes query = new DiagramElementQueryByElementTypes();
        query.QueryDiagramJunctionElement = true;
        query.QueryDiagramEdgeElement = true;
        query.QueryDiagramContainerElement = true;

        // Retrieve those diagram elements
        DiagramElementQueryResult elements = diagramTest.QueryDiagramElements(query);

        // Create a NetworkDiagramSubset object to edit this set of diagram elements
        NetworkDiagramSubset subset = new NetworkDiagramSubset();
        subset.DiagramJunctionElements = elements.DiagramJunctionElements;
        subset.DiagramEdgeElements = elements.DiagramEdgeElements;
        subset.DiagramContainerElements = elements.DiagramContainerElements;

        // Edit the shapes of the diagram elements - left as an exercise for the student
        TranslateDiagramElements(subset);

        // Save the new layout of the diagram elements
        diagramTest.SaveLayout(subset, true);
      }
    }
    #endregion

    // cref: ArcGIS.Core.Data.NetworkDiagrams.NetworkDiagram.Update()
    // cref: ArcGIS.Core.Data.NetworkDiagrams.NetworkDiagram.Append(System.Collections.Generic.IEnumerable<System.Guid>)
    // cref: ArcGIS.Core.Data.NetworkDiagrams.NetworkDiagram.Overwrite(System.Collections.Generic.IEnumerable<System.Guid>)
    // cref: ArcGIS.Core.Data.NetworkDiagrams.NetworkDiagram.GetDiagramInfo()
    // cref: ArcGIS.Core.Data.NetworkDiagrams.NetworkDiagramInfo
    // cref: ArcGIS.Core.Data.NetworkDiagrams.NetworkDiagramInfo.CanExtend
    // cref: ArcGIS.Core.Data.NetworkDiagrams.NetworkDiagram.Extend(ArcGIS.Core.Data.NetworkDiagrams.NetworkDiagramExtendType)
    // cref: ArcGIS.Core.Data.NetworkDiagrams.NetworkDiagram.Extend(ArcGIS.Core.Data.NetworkDiagrams.NetworkDiagramExtendType, System.Collections.Generic.IEnumerable<System.Guid>)
    // cref: ArcGIS.Core.Data.NetworkDiagrams.NetworkDiagramExtendType
    // cref: ArcGIS.Core.Data.NetworkDiagrams.NetworkDiagram.Delete()
    #region Editing Network Diagram

    public void EditDiagram(NetworkDiagram diagram, List<Guid> globalIDs)
    {
      //     These routines generate their own editing transaction, and therefore cannot be wrapped
      //     in a separate transaction. Because the editing performed by these routines cannot
      //     be undone, thise routines can also not be called within an editing session. All
      //     edits in the current edit session must be saved or discarded before calling these
      //     routines.

      // refresh the diagram - synchronizes it based on the latest network topology
      diagram.Update();

      // append features to the diagram
      diagram.Append(globalIDs);

      // overite the diagram with a set of features 
      diagram.Overwrite(globalIDs);

      NetworkDiagramInfo info = diagram.GetDiagramInfo();
      if (info.CanExtend)
      {
        diagram.Extend(NetworkDiagramExtendType.ExtendByContainment);

        // or extend for only a set of utility network globalIDs
        diagram.Extend(NetworkDiagramExtendType.ExtendByContainment, globalIDs);
      }
      // delete a diagran
      diagram.Delete();
    }
    #endregion

  }

}
