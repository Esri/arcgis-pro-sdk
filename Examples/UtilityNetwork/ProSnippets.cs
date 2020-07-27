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
using ArcGIS.Core.Data.UtilityNetwork.NetworkDiagrams;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Core.CIM;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Editing;
using ArcGIS.Core.Data.UtilityNetwork.Extensions;
  

namespace UtilityNetworkProSnippets { 

  class ProSnippetsUtilityNetwork
  {

    #region ProSnippet Group: Obtaining Utility Networks
    #endregion

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

      #region Create utility network features and associations in a single edit operation

      // Create an EditOperation
      EditOperation editOperation = new EditOperation();
      editOperation.Name = "Create pole; create transformer bank; attach transformer bank to pole";

      // Create the transformer bank
      RowToken transformerBankToken = editOperation.CreateEx(transformerBankLayer, transformerBankAttributes);

      // Create a pole
      RowToken poleToken = editOperation.CreateEx(poleLayer, poleAttributes);

      // Create a structural attachment association between the pole and the transformer bank
      RowHandle poleHandle = new RowHandle(poleToken);
      RowHandle transformerBankHandle = new RowHandle(transformerBankToken);

      AssociationDescription poleAttachment = new AssociationDescription(AssociationType.Attachment, poleHandle, transformerBankHandle);

      editOperation.Create(poleAttachment);

      // Execute the EditOperation
      editOperation.Execute();


      #endregion

    }

    #region ProSnippet Group: Subnetworks and Tiers
    #endregion

    void FindATierFromDomainNetworkNameAndTierName(UtilityNetwork utilityNetwork, string domainNetworkName, string tierName)
    {
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
      #region Create a DownstreamTracer

      using (TraceManager traceManager = utilityNetwork.GetTraceManager())
      {
        DownstreamTracer downstreamTracer = traceManager.GetTracer<DownstreamTracer>();
      }



      #endregion

    }

    void CreateTraceArgument()
    {
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
      #region Create an output condition

      // Create an output category to filter the trace results to only include
      // features with the "Service Point" category assigned
      traceConfiguration.OutputCondition = new CategoryComparison(CategoryOperator.IsEqual, "Service Point");

      #endregion

    }

    private void CreatePropagator(UtilityNetworkDefinition utilityNetworkDefinition, TraceConfiguration traceConfiguration)
    {
      const int ABCPhase = 7;

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

    #region ProSnippet Group: Network Diagrams
    #endregion

    #region Get a list of inconsistent Network Diagrams
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

    #region Retrieve Diagram Elements
    public void RetrieveDiagramElements(MapView mapView, NetworkDiagram networkDiagram)
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


    }
    #endregion

    void TranslateDiagramElements(NetworkDiagramSubset subset)
    { }

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

    public async void CreateDiagramLayerFromNetworkDiagram(NetworkDiagram myDiagram)
    {

      #region Open a diagram window from a Network Diagram

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


  }




}
