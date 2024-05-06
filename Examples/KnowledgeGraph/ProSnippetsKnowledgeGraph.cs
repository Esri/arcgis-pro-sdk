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
using ArcGIS.Core.Data.Knowledge;
using ArcGIS.Core.Data.Realtime;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.KnowledgeGraph;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KnowledgeGraphSnippets
{
  internal class ProSnippetsKnowledgeGraph
  {

    #region ProSnippet Group: KnowledgeGraph Datastore
    #endregion

    public static void Datastore1()
    {
      var featLayer = MapView.Active.Map.GetLayersAsFlattenedList()
                .OfType<FeatureLayer>().FirstOrDefault();
      if (featLayer == null)
        return;

      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.#CTOR(ArcGIS.Core.Data.Knowledge.KnowledgeGraphConnectionProperties)
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphConnectionProperties.#CTOR(System.Uri)
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph
      #region Opening a Connection to a KnowledgeGraph

      string url = 
        @"https://acme.server.com/server/rest/services/Hosted/AcmeKnowledgeGraph/KnowledgeGraphServer";

      QueuedTask.Run(() =>
      {
        //Create a connection properties
        var kg_props =
            new KnowledgeGraphConnectionProperties(new Uri(url));
        try
        {
          //Open a connection
          using (var kg = new KnowledgeGraph(kg_props))
          {
            //TODO...use the KnowledgeGraph
          }
        }
        catch (GeodatabaseNotFoundOrOpenedException ex)
        {
          System.Diagnostics.Debug.WriteLine(ex.ToString());
        }
      });

      #endregion
    }

    public static void Datastore2()
    {
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph
      //cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayer
      //cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayer.GetDatastore
      #region Getting a Connection from a KnowledgeGraphLayer 

      var kgLayer = MapView.Active.Map.GetLayersAsFlattenedList()
              .OfType<KnowledgeGraphLayer>().FirstOrDefault();

      
      QueuedTask.Run(() =>
      {
        // use the layer directly
        var datastore = kgLayer.GetDatastore();

        // or you can use any of the sub items since
        //KnowledgeGraphLayer is a composite layer - get the first 
        // child feature layer or standalone table
        var featlayer = kgLayer?.GetLayersAsFlattenedList()?
                        .OfType<FeatureLayer>()?.FirstOrDefault();
        KnowledgeGraph kg = null;
        if (featlayer != null)
        {
          using (var fc = featlayer.GetFeatureClass())
            kg = fc.GetDatastore() as KnowledgeGraph;
          //TODO use KnowledgeGraph
        }
        else
        {
          //try standalone table
          var stbl = kgLayer?.GetStandaloneTablesAsFlattenedList()?
                          .FirstOrDefault();
          if (stbl != null)
          {
            using (var tbl = stbl.GetTable())
              kg = tbl.GetDatastore() as KnowledgeGraph;
            //TODO use KnowledgeGraph
          }
        }
      });
      #endregion
    }

    public void Datastore3()
    {
      var url = "";
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.#CTOR(ArcGIS.Core.Data.Knowledge.KnowledgeGraphConnectionProperties)
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphConnectionProperties.#CTOR(System.Uri)
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.GetDefinitions<T>
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.OpenDataset<T>(string)
      #region Retrieving GDB FeatureClasses and Definitions 

      QueuedTask.Run(() =>
      {
        //Create a connection properties
        var kg_props =
            new KnowledgeGraphConnectionProperties(new Uri(url));
        //Connect to the KnowledgeGraph datastore
        //KnowledgeGraph datastores contain tables and feature classes
        using (var kg = new KnowledgeGraph(kg_props))
        {
          //Get the featureclass definitions from the KG datastore
          var fc_defs = kg.GetDefinitions<FeatureClassDefinition>();
          //For each feature class definition, get the corresponding
          //feature class. Note: The name of the feature class will match 
          //the name of its corresponding KnowledgeGraph named object type
          //in the KnowledgeGraph graph data model
          foreach (var fc_def in fc_defs)
          {
            var fc_name = fc_def.GetName();
            using (var fc = kg.OpenDataset<FeatureClass>(fc_name))
            {
              //TODO - use the feature class
            }
          }
        }
      });
      #endregion
    }

    public void Datastore4()
    {
      var url = "";
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.#CTOR(ArcGIS.Core.Data.Knowledge.KnowledgeGraphConnectionProperties)
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphConnectionProperties.#CTOR(System.Uri)
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.GetDefinitions<T>
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.OpenDataset<T>(string)
      #region Retrieving GDB Tables and Definitions 

      QueuedTask.Run(() =>
      {
        //Create a connection properties
        var kg_props =
            new KnowledgeGraphConnectionProperties(new Uri(url));
        //Connect to the KnowledgeGraph datastore
        //KnowledgeGraph datastores contain tables and feature classes
        using (var kg = new KnowledgeGraph(kg_props))
        {
          //Get the table definitions from the KG datastore
          var tbl_defs = kg.GetDefinitions<TableDefinition>();
          //For each table definition, get the corresponding
          //table. Note: The name of the table will match the name
          //of its corresponding KnowledgeGraph named object type in
          //the KnowledgeGraph graph data model
          foreach (var tbl_def in tbl_defs)
          {
            var tbl_name = tbl_def.GetName();
            using (var fc = kg.OpenDataset<Table>(tbl_name))
            {
              //TODO - use the table
            }
          }
        }
      });
      #endregion
    }

    public void Datastore5() 
    {
      KnowledgeGraph kg = null;


      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.GetConnector()
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphConnectionProperties
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphConnectionProperties.URL
      #region Get service Uri from KG datastore
      QueuedTask.Run(() =>
      {
        var connector = kg.GetConnector() as KnowledgeGraphConnectionProperties;
        var uri = connector.URL;
        var serviceUri = uri.AbsoluteUri;
      });
      #endregion
    }

    public void Datastore6()
    {
      KnowledgeGraph kg = null;
      string entityName = "";


      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.TransformToIDs(System.string, System.IEnumerable<System.Int64>)
      #region Transform a set of objectIDs to IDs for an entity
      QueuedTask.Run(() =>
      {
        var oidList = new List<long>() { 260294, 678, 3523, 3, 669, 93754 };
        var idList = kg.TransformToIDs(entityName, oidList);

      });
      #endregion

      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.TransformToObjectIDs(System.string, System.IEnumerable<System.Object>)
      #region Transform a set of IDs to objectIDs for an entity
      QueuedTask.Run(() =>
      {
        var idList = new List<object>() { "{CA2EF786-A10E-4B40-9737-9BDDDEA127B0}", 
                                          "{14B5AD65-890D-4062-90A7-C42C23B0066E}",
                                          "{A428D1F6-CB00-4559-AAFD-40885A4F2340}"};
        var oidList = kg.TransformToObjectIDs(entityName, idList);

      });
      #endregion
    }

    #region ProSnippet Group: KnowledgeGraph Graph Data Model
    #endregion

    public void DataModel1()
    {
      var url = "";
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.GetDataModel()
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphDataModel
      #region Retrieving the Data Model 

      QueuedTask.Run(() =>
      {
        //Create a connection properties
        var kg_props =
            new KnowledgeGraphConnectionProperties(new Uri(url));
        using (var kg = new KnowledgeGraph(kg_props))
        {
          //Get the KnowledgeGraph Data Model
          using(var kg_dm = kg.GetDataModel())
          {
            //TODO use KG data model...
          }
        }
      });
      
      #endregion
    }

    public void DataModel2()
    {
      var url = "";
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.GetDataModel()
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphDataModel.GetTimestamp
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphDataModel.GetSpatialReference()
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphDataModel.GetIsArcGISManaged()
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphDataModel.GetOIDPropertyName()
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphDataModel.GetIsStrict()
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphDataModel
      #region Get Data Model Properties

      QueuedTask.Run(() =>
      {
        //Create a connection properties
        var kg_props =
            new KnowledgeGraphConnectionProperties(new Uri(url));
        using (var kg = new KnowledgeGraph(kg_props))
        {
          //Get the KnowledgeGraph Data Model
          using (var kg_dm = kg.GetDataModel())
          {
            var kg_name = System.IO.Path.GetFileName(
                     System.IO.Path.GetDirectoryName(kg_props.URL.ToString()));

            System.Diagnostics.Debug.WriteLine(
              $"\r\n'{kg_name}' Datamodel:\r\n-----------------");
            var time_stamp = kg_dm.GetTimestamp();
            var sr = kg_dm.GetSpatialReference();
            System.Diagnostics.Debug.WriteLine($"Timestamp: {time_stamp}");
            System.Diagnostics.Debug.WriteLine($"Sref: {sr.Wkid}");
            System.Diagnostics.Debug.WriteLine(
              $"IsStrict: {kg_dm.GetIsStrict()}");
            System.Diagnostics.Debug.WriteLine(
              $"OIDPropertyName: {kg_dm.GetOIDPropertyName()}");
            System.Diagnostics.Debug.WriteLine(
              $"IsArcGISManaged: {kg_dm.GetIsArcGISManaged()}");
          }
        }
      });

      #endregion
    }

    public void DataModel3()
    {
      var url = "";
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphDataModel.GetIdentifierInfo
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphIdentifierInfo
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphIdentifierInfo.GetIdentifierGeneration
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphIdentifierGeneration
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNativeIdentifier
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphUniformIdentifier
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphUniformIdentifier.GetIdentifierName
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphIdentifierGeneration.GetMethodHint
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphUUIDMethodHint
      #region Get Data Model Identifier Info

      QueuedTask.Run(() =>
      {
        //Create a connection properties
        var kg_props =
            new KnowledgeGraphConnectionProperties(new Uri(url));
        using (var kg = new KnowledgeGraph(kg_props))
        {
          //Get the KnowledgeGraph Data Model
          using (var kg_dm = kg.GetDataModel())
          {
            var kg_id_info = kg_dm.GetIdentifierInfo();
            var kg_id_gen = kg_id_info.GetIdentifierGeneration();
            if (kg_id_info is KnowledgeGraphNativeIdentifier kg_ni)
            {
              System.Diagnostics.Debug.WriteLine(
                $"IdentifierInfo: KnowledgeGraphNativeIdentifier");
            }
            else if (kg_id_info is KnowledgeGraphUniformIdentifier kg_ui)
            {
              System.Diagnostics.Debug.WriteLine(
                $"IdentifierInfo: KnowledgeGraphUniformIdentifier");
              System.Diagnostics.Debug.WriteLine(
                $"IdentifierName: '{kg_ui.GetIdentifierName()}'");
            }
            System.Diagnostics.Debug.WriteLine(
              $"Identifier MethodHint: {kg_id_gen.GetMethodHint()}");
          }
        }
      });

      #endregion
    }

    public void DataModel4()
    {
      var url = "";
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphDataModel.GetMetaEntityTypes
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphEntityType
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectType.GetRole
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectType.GetName
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectTypeRole
      #region Get Data Model MetaEntityTypes/Provenance

      //Provenance entity type is stored as a MetaEntityType 
      QueuedTask.Run(() =>
      {
        //Create a connection properties
        var kg_props =
            new KnowledgeGraphConnectionProperties(new Uri(url));
        using (var kg = new KnowledgeGraph(kg_props))
        {
          //Get the KnowledgeGraph Data Model
          using (var kg_dm = kg.GetDataModel())
          {
            var dict_types = kg_dm.GetMetaEntityTypes();
            //If there is no provenance then MetaEntityTypes will be
            //an empty collection
            foreach (var kvp in dict_types)
            {
              var meta_entity_type = kvp.Value;
              if (meta_entity_type.GetRole() == 
                  KnowledgeGraphNamedObjectTypeRole.Provenance)
              {
                //TODO - use the provenance entity type
                var name = meta_entity_type.GetName();

              }
            }
            
          }
        }
      });

      #endregion
    }

    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphDataModel.GetMetaEntityTypes
    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphEntityType
    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectType.GetRole
    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectType.GetName
    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectTypeRole
    #region Get Whether KG Supports Provenance
    internal string GetProvenanceEntityTypeName(KnowledgeGraphDataModel kg_dm)
    {
      var entity_types = kg_dm.GetMetaEntityTypes();
      foreach (var entity_type in entity_types)
      {
        if (entity_type.Value.GetRole() == KnowledgeGraphNamedObjectTypeRole.Provenance)
          return entity_type.Value.GetName();
      }
      return "";
    }
    internal bool KnowledgeGraphSupportsProvenance(KnowledgeGraph kg)
    {
      //if there is a provenance entity type then the KnowledgeGraph
      //supports provenance
      return !string.IsNullOrEmpty(
        GetProvenanceEntityTypeName(kg.GetDataModel()));
    }
    #endregion

    public void DataModel5()
    {
      var url = "";
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphDataModel.GetEntityTypes
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphEntityType
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectType.GetRole
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectType.GetName
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectType.GetAliasName
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectType.GetObjectIDPropertyName
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectTypeRole
      #region Get KnowledgeGraph Entity Types

      QueuedTask.Run(() =>
      {
        //Create a connection properties
        var kg_props =
            new KnowledgeGraphConnectionProperties(new Uri(url));
        using (var kg = new KnowledgeGraph(kg_props))
        {
          //Get the KnowledgeGraph Data Model
          using (var kg_dm = kg.GetDataModel())
          {
            var dict_types = kg_dm.GetEntityTypes();

            foreach (var kvp in dict_types)
            {
              var entity_type = kvp.Value;
              var role = entity_type.GetRole();
              //note "name" will be the same name as the corresponding
              //feature class or table in the KG's relational gdb model
              var name = entity_type.GetName();
              var alias = entity_type.GetAliasName();
              var objectIDPropertyName = entity_type.GetObjectIDPropertyName();
              //etc

            }

          }
        }
      });

      #endregion
    }

    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphDataModel.GetMetaEntityTypes
    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphEntityType
    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectType.GetRole
    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectType.GetName
    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectTypeRole
    #region Get Whether KG Has a Document Type 
    internal string GetDocumentEntityTypeName(KnowledgeGraphDataModel kg_dm)
    {
      var entity_types = kg_dm.GetEntityTypes();
      foreach (var entity_type in entity_types)
      {
        var role = entity_type.Value.GetRole();
        if (role == KnowledgeGraphNamedObjectTypeRole.Document)
          return entity_type.Value.GetName();
      }
      return "";//Unusual - probably Neo4j user-managed
    }

    internal bool KnowledgeGraphHasDocumentType(KnowledgeGraph kg)
    {
      //uncommon for there not to be a document type
      return !string.IsNullOrEmpty(
        GetDocumentEntityTypeName(kg.GetDataModel()));
    }
    #endregion

    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphEntityValue
    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphEntityType
    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectValue.GetTypeName
    #region Check Whether A KG Entity Is a Document 

    //Use GetDocumentEntityTypeName(KnowledgeGraphDataModel kg_dm) from
    //the 'Get Whether KG Has a Document Type' snippet to
    //get the documentNameType parameter
    protected bool GetEntityIsDocument(KnowledgeGraphEntityValue entity,
      string documentNameType = "")
    {
      if (string.IsNullOrEmpty(documentNameType))
        return false;
      return entity.GetTypeName() == documentNameType;
    }

    #endregion

    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphEntityValue
    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphRelationshipType
    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectType.GetProperties
    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectType
    #region Check Whether A Graph Type has a Spatial Property 

    //Use GetDocumentEntityTypeName(KnowledgeGraphDataModel kg_dm) from
    //the 'Get Whether KG Has a Document Type' snippet to
    //get the documentNameType parameter
    protected bool HasGeometry(KnowledgeGraphNamedObjectType kg_named_obj)
    {
      var props = kg_named_obj.GetProperties();
      return props.Any(prop => prop.FieldType == FieldType.Geometry);
    }

    #endregion

    public void DataModel6()
    {
      var url = "";
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphDataModel.GetRelationshipTypes
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphRelationshipType
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectType.GetRole
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectType.GetName
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectTypeRole
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphRelationshipType.GetEndPoints
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphEndPoint
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphEndPoint.GetOriginEntityTypeName
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphEndPoint.GetDestinationEntityTypeName
      #region Get KnowledgeGraph Relationship Types

      QueuedTask.Run(() =>
      {
        //Create a connection properties
        var kg_props =
            new KnowledgeGraphConnectionProperties(new Uri(url));
        using (var kg = new KnowledgeGraph(kg_props))
        {
          //Get the KnowledgeGraph Data Model
          using (var kg_dm = kg.GetDataModel())
          {
            var dict_types = kg_dm.GetRelationshipTypes();

            foreach (var kvp in dict_types)
            {
              var rel_type = kvp.Value;
              var role = rel_type.GetRole();
              //note "name" will be the same name as the corresponding
              //feature class or table in the KG's relational gdb model
              var name = rel_type.GetName();
              //etc.
              //Get relationship end points
              var end_points = rel_type.GetEndPoints();
              foreach (var end_point in end_points)
              {
                System.Diagnostics.Debug.WriteLine(
                  $"Origin: '{end_point.GetOriginEntityTypeName()}', " +
                  $"Destination: '{end_point.GetDestinationEntityTypeName()}'");
              }
            }

          }
        }
      });

      #endregion
    }

    public void DataModel7()
    {
      var url = "";
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphDataModel.GetEntityTypes
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphDataModel.GetRelationshipTypes
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphDataModel.GetMetaEntityTypes
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectType.GetRole
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectType.GetName
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectType.GetProperties
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectTypeRole
      #region Get All KnowledgeGraph Graph Types

      QueuedTask.Run(() =>
      {
        //Create a connection properties
        var kg_props =
            new KnowledgeGraphConnectionProperties(new Uri(url));
        using (var kg = new KnowledgeGraph(kg_props))
        {
          //Get the KnowledgeGraph Data Model
          using (var kg_datamodel = kg.GetDataModel())
          {
            var entities = kg_datamodel.GetEntityTypes();
            var relationships = kg_datamodel.GetRelationshipTypes();
            var provenance = kg_datamodel.GetMetaEntityTypes();

            var all_graph_types = new List<KnowledgeGraphNamedObjectType>();
            all_graph_types.AddRange(entities.Values);
            all_graph_types.AddRange(relationships.Values);
            all_graph_types.AddRange(provenance.Values);

            System.Diagnostics.Debug.WriteLine("\r\nGraph Types");

            int c = 0;
            foreach (var graph_type in all_graph_types)
            {
              var type_name = graph_type.GetName();
              var role = graph_type.GetRole().ToString();

              //equivalent to:
              //var fields = featClassDef.GetFields().Select(f => f.Name).ToList();
              //var field_names = string.Join(",", fields);
              var props = graph_type.GetProperties().Select(p => p.Name).ToList();
              var prop_names = string.Join(",", props);

              System.Diagnostics.Debug.WriteLine($"[{c++}]: " +
                  $"{type_name}, {role}, {prop_names}");
            }
          }
        }
      });

      #endregion
    }

    #region ProSnippet Group: KnowledgeGraphLayer Creation with Maps
    #endregion

    public void KGLayerCreate1()
    {
      var url = "";
      KnowledgeGraph kg = null;
      Map map = null;

      //cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerCreationParams
      //cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerCreationParams.#ctor(ArcGIS.Core.Data.Knowledge.KnowledgeGraph)
      //cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerCreationParams.#ctor(System.Uri)
      //cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayer
      #region Create a KG Layer containing all Entity and Relate types

      QueuedTask.Run(() =>
      {
        //With a connection to a KG established or source uri available...
        //Create a KnowledgeGraphLayerCreationParams
        var kg_params = new KnowledgeGraphLayerCreationParams(kg)
        {
          Name = "KG_With_All_Types",
          IsVisible = false
        };
        //Or
        var kg_params2 = new KnowledgeGraphLayerCreationParams(new Uri(url))
        {
          Name = "KG_With_All_Types",
          IsVisible = false
        };
        //Call layer factory with map or group layer container. 
        //A KG layer containing a feature layer and/or standalone table per
        //entity and relate type (except provenance) is created
        var kg_layer = LayerFactory.Instance.CreateLayer<KnowledgeGraphLayer>(
            kg_params, map);

      });
      #endregion
    }

    public void KGLayerCreate2()
    {
      var url = "";
      KnowledgeGraph kg = null;
      Map map = null;

      //cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerCreationParams
      //cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerCreationParams.IDSet
      //cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet.FromDictionary
      //cref: ArcGIS.Desktop.Mapping.ILayerFactory.CreateLayer``1(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
      #region Create a KG Layer containing a subset of Entity and Relate types

      QueuedTask.Run(() =>
      {
        //To create a KG layer (on a map or link chart) with a subset of
        //entities and relates, you must create an "id set". The workflow
        //is very similar to how you would create a SelectionSet.

        //First, create a dictionary containing the names of the types to be
        //added plus a corresponding list of ids (just like with a selection set).
        //Note: null or an empty list means add all records for "that" type..
        var kg_datamodel = kg.GetDataModel();
        //Arbitrarily get the name of the first entity and relate type
        var first_entity = kg_datamodel.GetEntityTypes().Keys.First();
        var first_relate = kg_datamodel.GetRelationshipTypes().Keys.First();

        //Make entries in the dictionary for each
        var dict = new Dictionary<string, List<long>>();
        dict.Add(first_entity, new List<long>());//Empty list means all records
        dict.Add(first_relate, null);//null list means all records
        //or specific records - however the ids are obtained
        //dict.Add(entity_or_relate_name, new List<long>() { 1, 5, 18, 36, 78});

        //Create the id set...
        var idSet = KnowledgeGraphLayerIDSet.FromDictionary(kg, dict);

        //Create the layer params and assign the id set
        var kg_params = new KnowledgeGraphLayerCreationParams(kg)
        {
          Name = "KG_With_ID_Set",
          IsVisible = false,
          IDSet = idSet
        };

        //Call layer factory with map or group layer container
        //A KG layer containing just the feature layer(s) and/or standalone table(s)
        //for the entity and/or relate types + associated records will be created
        var kg_layer = LayerFactory.Instance.CreateLayer<KnowledgeGraphLayer>(
            kg_params, map);

      });
      #endregion
    }

    public void KGLayerCreate3()
    {
      var url = "";
      KnowledgeGraph kg = null;
      Map map = null;

      //cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerCreationParams
      //cref: ArcGIS.Desktop.Mapping.LayerFactory.CanCreateLayer<T>
      #region Using LayerFactory.Instance.CanCreateLayer with KG Create Layer Params

      QueuedTask.Run(() =>
      {
        //Feature class and/or standalone tables representing KG entity and
        //relate types can only be added to a map (or link chart) as a child
        //of a KnowledgeGraph layer....

        //For example:
        var fc = kg.OpenDataset<FeatureClass>("Some_Entity_Or_Relate_Name");
        try
        {
          //Attempting to create a feature layer containing the returned fc
          //NOT ALLOWED - can only be a child of a KG layer
          var fl_params_w_kg = new FeatureLayerCreationParams(fc);
          //CanCreateLayer will return false
          if (!(LayerFactory.Instance.CanCreateLayer<FeatureLayer>(
            fl_params_w_kg, map)))
          {
            System.Diagnostics.Debug.WriteLine(
              $"Cannot create a feature layer for {fc.GetName()}");
            return;
          }
          //This will throw an exception
          LayerFactory.Instance.CreateLayer<FeatureLayer>(fl_params_w_kg, map);
        }
        catch (Exception ex)
        {
          System.Diagnostics.Debug.WriteLine(ex.ToString());
        }

        //Can only be added as a child of a parent KG
        var dict = new Dictionary<string, List<long>>();
        dict.Add(fc.GetName(), new List<long>());
        var kg_params = new KnowledgeGraphLayerCreationParams(kg)
        {
          Name = $"KG_With_Just_{fc.GetName()}",
          IsVisible = false,
          IDSet = KnowledgeGraphLayerIDSet.FromDictionary(kg, dict)
        };
        var kg_layer = LayerFactory.Instance.CreateLayer<KnowledgeGraphLayer>(
          kg_params, map);

      });
      #endregion
    }

    #region  ProSnippet Group: KnowledgeGraph Layer
    #endregion

    public void KGLayer1()
    {
      var url = "";
      KnowledgeGraph kg = null;
      Map map = null;

      //cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet.ToOIDDictionary
      //cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet.FromDictionary(ArcGIS.Core.Data.Knowledge.KnowledgeGraph,System.Collections.Generic.Dictionary{System.String,System.Collections.Generic.List{System.Int64}})
      #region Get and Set a KG Layer IDSet

      var kg_layer = MapView.Active.Map.GetLayersAsFlattenedList()
                      .OfType<KnowledgeGraphLayer>().FirstOrDefault();
      if (kg_layer == null)
        return;

      QueuedTask.Run(() =>
      {
        //Get the existing kg layer id set and convert to a dictionary
        var layer_id_set = kg_layer.GetIDSet();
        var dict = layer_id_set.ToOIDDictionary();//Empty list means all records

        //Create an id set from a dictionary
        var dict2 = new Dictionary<string, List<long>>();
        dict2.Add("Enity_Or_Relate_Type_Name1", null);//Null means all records
        dict2.Add("Enity_Or_Relate_Type_Name2", new List<long>());//Empty list means all records
        dict2.Add("Enity_Or_Relate_Type_Name3", 
          new List<long>() {  3,5,9, 101, 34});//Explicit list of ids
        //dict2.Add("Enity_Or_Relate_Type_Name4",
        // new List<long>() { etc, etc });

        //Create the id set
        var idset = KnowledgeGraphLayerIDSet.FromDictionary(kg, dict2);

        //Can be used to create a layer, link chart, append to link chart, etc...
      });
      #endregion

      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphExtensions.GetIsKnowledgeGraphDataset
      #region Is a dataset part of a Knowledge Graph
      var featureLyer = MapView.Active.Map.GetLayersAsFlattenedList()
                      .OfType<FeatureLayer>().FirstOrDefault();
      if (featureLyer == null)
        return;

      QueuedTask.Run(() =>
      {
        //Get the feature class
        var fc = featureLyer.GetFeatureClass();

        // is it part of a KnowledgeGraph?
        var isPartOfKG = fc.GetIsKnowledgeGraphDataset();
        
      });
      #endregion

      #region Get KG Datastore

      var kgLayer = MapView.Active.Map.GetLayersAsFlattenedList()
                      .OfType<KnowledgeGraphLayer>().FirstOrDefault();
      if (kgLayer == null)
        return;

      QueuedTask.Run(() =>
      {
        // get the datastore
        var kg = kgLayer.GetDatastore();

        // now submit a search or a query
        // kg.SubmitSearch
        // kg.SubmitQuery
      });
      #endregion

      #region Get KG Service uri
      kgLayer.GetServiceUri();


      #endregion

    }

    public void KGLayer2()
    {

      //cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayer
      //cref: ArcGIS.Desktop.Mapping.LinkChartFeatureLayer
      //cref: ArcGIS.Desktop.Mapping.LinkChartFeatureLayer.IsEntity
      //cref: ArcGIS.Desktop.Mapping.LinkChartFeatureLayer.IsRelationship
      #region SubLayers of a KnowledgeGraph Layer

      var map = MapView.Active.Map;
      var kgLayer = map.GetLayersAsFlattenedList().OfType<KnowledgeGraphLayer>().FirstOrDefault();
      if (kgLayer == null)
        return;

      if (map.MapType == MapType.LinkChart)
      {
        // if map is of MapType.LinkChart then the first level
        // children of the kgLayer are of type LinkChartFeatureLayer
        var childLayers = kgLayer.Layers;
        foreach (var childLayer in childLayers)
        {
          if (childLayer is LinkChartFeatureLayer lcFeatureLayer)
          {
            var isEntity = lcFeatureLayer.IsEntity;
            var isRel = lcFeatureLayer.IsRelationship;

            // TODO - continue processing
          }
        }
      }
      else if (map.MapType == MapType.Map)
      {
        // if map is of MapType.Map then the children of the
        // kgLayer are the standard Featurelayer and StandAloneTable
        var chidlren = kgLayer.GetMapMembersAsFlattenedList();
        foreach (var child in chidlren)
        {
          if (child is FeatureLayer fl)
          {
            // TODO - process the feature layer
          }
          else if (child is StandaloneTable st)
          {
            // TODO - process the standalone table
          }
        }
      }


      #endregion
    }

    public void KGLayer3()
    {

      //cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayer.GetIDSet
      //cref: ArcGIS.Desktop.Mapping.LinkChartFeatureLayer.GetTypeName
      //cref: ArcGIS.Desktop.Mapping.MapFactory.CreateLinkChart
      #region Create a LinkChart from a subset of an existing LinkChart IDSet

      var map = MapView.Active.Map;
      if (map.MapType != MapType.LinkChart)
        return;

      // find the KG layer
      var kgLayer = map.GetLayersAsFlattenedList().OfType<KnowledgeGraphLayer>().FirstOrDefault();
      if (kgLayer == null)
        return;

      // find the first LinkChartFeatureLayer in the KG layer
      var lcFeatureLayer = kgLayer.GetLayersAsFlattenedList().OfType<LinkChartFeatureLayer>().FirstOrDefault();
      if (lcFeatureLayer != null)
        return;

      QueuedTask.Run(() =>
      {
        // get the KG
        var kg = kgLayer.GetDatastore();

        // get the ID set of the KG layer
        var idSet = kgLayer.GetIDSet();

        // get the named object type in the KG that the LinkChartFeatureLayer represents
        var typeName = lcFeatureLayer.GetTypeName();
        // if there are items in the ID Set for this type
        if (idSet.Contains(typeName))
        {
          // build a new ID set containing only these records
          var dict = idSet.ToOIDDictionary();
          var oids = dict[typeName];

          var newDict = new Dictionary<string, List<long>>();
          newDict.Add(typeName, oids);

          var newIDSet = KnowledgeGraphLayerIDSet.FromDictionary(kg, newDict);

          // now create a new link chart using just this subset of records
          MapFactory.Instance.CreateLinkChart("subset LinkChart", kg, newIDSet);
        }
      });
      #endregion
    }


    #region ProSnippet Group: Graph Query and Text Search
    #endregion

    public async void QueryAndSearch1()
    {
      KnowledgeGraph kg = null;
      bool includeProvenanceIfPresent = false;
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.SubmitQuery
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphQueryFilter
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphQueryFilter.#Ctor
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphQueryFilter.QueryText
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphQueryFilter.ProvenanceBehavior
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphProvenanceBehavior
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphCursor.MoveNext
      //cref: ArcGIS.Core.Data.Realtime.RealtimeCursorBase.WaitForRowsAsync()
      //cref: ArcGIS.Core.Data.Realtime.RealtimeCursorBase.WaitForRowsAsync(System.Threading.CancellationToken)
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphCursor.Current
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphRow.GetCount
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphRow.Item(System.Int32)
      #region Submit a Graph Query

      //On the QueuedTask...
      //and assuming you have established a connection to a knowledge graph
      //...
      //Construct an openCypher query - return the first 10 entities (whatever
      //they are...)
      var query = "MATCH (n) RETURN n LIMIT 10";//default limit is 100 if not specified
      //other examples...
      //query = "MATCH (a:Person) RETURN [a.name, a.age] ORDER BY a.age DESC LIMIT 50";
      //query = "MATCH (b:Person) RETURN { Xperson: { Xname: b.name, Xage: b.age } } ORDER BY b.name DESC";
      //query = "MATCH p = (c:Person)-[:HasCar]-() RETURN p ORDER BY c.name DESC";

      //Create a query filter
      //Note: OutputSpatialReference is currently ignored
      var kg_qf = new KnowledgeGraphQueryFilter()
      {
        QueryText = query
      };
      //Optionally - u can choose to include provenance in the results
      //(_if_ the KG has provenance - otherwise the query will fail)
      if (includeProvenanceIfPresent)
      {
        //see "Get Whether KG Supports Provenance" snippet
        if (KnowledgeGraphSupportsProvenance(kg))
        {
          //Only include if the KG has provenance
          kg_qf.ProvenanceBehavior = 
            KnowledgeGraphProvenanceBehavior.Include;//default is exclude
        }
      }
      //submit the query - returns a KnowledgeGraphCursor
      using (var kg_rc = kg.SubmitQuery(kg_qf))
      {
        //wait for rows to be returned from the server
        //note the "await"...
        while (await kg_rc.WaitForRowsAsync())
        {
          //Rows have been retrieved - process this "batch"...
          while (kg_rc.MoveNext())
          {
            //Get the current KnowledgeGraphRow
            using (var graph_row = kg_rc.Current)
            {
              //Graph row is an array, process all returned values...
              var val_count = (int)graph_row.GetCount();
              for (int i = 0; i < val_count; i++)
              {
                var retval = graph_row[i];
                //Process row value (note: recursive)
                //See "Process a KnowledgeGraphRow Value" snippet
                ProcessKnowledgeGraphRowValue(retval);
              }
            }
          }
        }//WaitForRowsAsync
      }//SubmitQuery
      #endregion
    }

    public async void QueryAndSearch2()
    {
      KnowledgeGraph kg = null;
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.SubmitSearch
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphRow.GetCount
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphRow.Item(System.Int32)
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphSearchFilter.#ctor
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphSearchFilter.SearchTarget
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphSearchFilter.SearchText
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphSearchFilter.ReturnSearchContext
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphSearchFilter.MaxRowCount
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedTypeCategory
      #region Submit a Text Search

      //On the QueuedTask...
      //and assuming you have established a connection to a knowledge graph
      //...
      //Construct a KG search filter. Search text uses Apache Lucene Query Parser
      //syntax - https://lucene.apache.org/core/2_9_4/queryparsersyntax.html
      var kg_sf = new KnowledgeGraphSearchFilter()
      {
        SearchTarget = KnowledgeGraphNamedTypeCategory.Entity,
        SearchText = "Acme Electric Co.",
        ReturnSearchContext = true,
        MaxRowCount = 10 //Default is 100 if not specified
      };

      //submit the search - returns a KnowledgeGraphCursor
      var e = 0;
      using (var kg_rc = kg.SubmitSearch(kg_sf))
      {
        //wait for rows to be returned from the server
        //note the "await"...
        while (await kg_rc.WaitForRowsAsync())
        {
          //Rows have been retrieved - process this "batch"...
          while (kg_rc.MoveNext())
          {
            //Get the current KnowledgeGraphRow
            using (var graph_row = kg_rc.Current)
            {
              //We are returning entities from this search
              var entity = graph_row[0] as KnowledgeGraphEntityValue;
              var entity_type = entity.GetTypeName();
              var record = new List<string>();
              //discover keys(aka "fields") dynamically via GetKeys
              foreach (var prop_name in entity.GetKeys())
              {
                var obj_val = entity[prop_name] ?? "null";
                record.Add(obj_val.ToString());
              }
              System.Diagnostics.Debug.WriteLine(
                $"{entity_type}[{e++}] " + string.Join(", ", record));
              //or use "Process a KnowledgeGraphRow Value" snippet
              //ProcessKnowledgeGraphRowValue(entity);
            }
          }
        }//WaitForRowsAsync
      }//SubmitSearch
      #endregion
    }

    public void QueryAndSearch3()
    {
      var url = "";
      KnowledgeGraph kg = null;
      KnowledgeGraphLayer kg_layer = null;

      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphQueryFilter
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.SubmitQuery(ArcGIS.Core.Data.Knowledge.KnowledgeGraphQueryFilter)
      #region Convert an Open Cypher Query Result to a Selection

      QueuedTask.Run(async () =>
      {
        //Given an open-cypher qry against an entity or relationship type
        var qry = @"MATCH (p:PhoneNumber) RETURN p LIMIT 10";

        //create a KG query filter
        var kg_qry_filter = new KnowledgeGraphQueryFilter()
        {
          QueryText = qry
        };

        //save a list of the ids
        var oids = new List<long>();
        using (var kgRowCursor = kg.SubmitQuery(kg_qry_filter))
        {
          //wait for rows to be returned asynchronously from the server
          while (await kgRowCursor.WaitForRowsAsync())
          {
            //get the rows using "standard" move next
            while (kgRowCursor.MoveNext())
            {
              //current row is accessible via ".Current" prop of the cursor
              using (var graphRow = kgRowCursor.Current)
              {
                var cell_phone = graphRow[0] as KnowledgeGraphEntityValue;
                //note: some user-managed graphs do not have objectids
                oids.Add(cell_phone.GetObjectID());
              }
            }
          }
        }
        //create a query filter using the oids
        if (oids.Count > 0)
        {
          //select them on the layer
          var qf = new QueryFilter()
          {
            ObjectIDs = oids //apply the oids to the ObjectIds property
          };
          //select the child feature layer or standalone table representing
          //the given entity or relate type whose records are to be selected
          var phone_number_fl = kg_layer.GetLayersAsFlattenedList()
              .OfType<FeatureLayer>().First(l => l.Name == "PhoneNumber");

          //perform the selection
          phone_number_fl.Select(qf);
        }
      });

      #endregion
    }

    public void QueryAndSearch4()
    {
      var url = "";
      KnowledgeGraph kg = null;

      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphQueryFilter.BindParameters
      #region Use Bind Parameters with an Open Cypher Query

      QueuedTask.Run(async () =>
      {
        //assume we have, in this case, a list of ids (perhaps retrieved
        //via a selection, hardcoded (like here), etc.
        var oids = new List<long>() { 3,4,7,8,9,11,12,14,15,19,21,25,29,
            31,32,36,37,51,53,54,55,56,59,63,75,78,80,84,86,88,96,97,98,101,
            103,106};
        //In the query, we refer to the "bind parameter" with the
        //"$" and a variable name - '$object_ids' in this example
        var qry = @"MATCH (p:PhoneNumber) " +
                  @" WHERE p.objectid IN $object_ids " +
                  @"RETURN p";

        //we provide the values to be substituted for the variable via the
        //KnowledgeGraphQueryFilter BindParameter property...
        var kg_qry_filter = new KnowledgeGraphQueryFilter()
        {
          QueryText = qry
        };
        //the bind parameter added to the query filter must refer to
        //the variable name used in the query string (without the "$")
        //Note:
        //Collections must be converted to a KnowledgeGraphArrayValue before
        //being assigned to a BindParameter
        var kg_oid_array = new KnowledgeGraphArrayValue();
        kg_oid_array.AddRange(oids);
        oids.Clear();

        kg_qry_filter.BindParameters["object_ids"] = kg_oid_array;

        //submit the query
        using (var kgRowCursor = kg.SubmitQuery(kg_qry_filter))
        {
          //wait for rows to be returned asynchronously from the server
          while (await kgRowCursor.WaitForRowsAsync())
          {
            //get the rows using "standard" move next
            while (kgRowCursor.MoveNext())
            {
              //current row is accessible via ".Current" prop of the cursor
              using (var graphRow = kgRowCursor.Current)
              {
                var cell_phone = graphRow[0] as KnowledgeGraphEntityValue;
                var oid = cell_phone.GetObjectID();

                var name = (string)cell_phone["FULL_NAME"];
                var ph_number = (string)cell_phone["PHONE_NUMBER"];
                System.Diagnostics.Debug.WriteLine(
                  $"[{oid}] {name}, {ph_number}");
              }
            }
          }
        }
      });

      #endregion
    }

    public void QueryAndSearch5()
    {
      var url = "";
      KnowledgeGraph kg = null;
      Polygon poly = null;

      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.KnowledgeGraphQueryFilter.BindParameters
      #region Use Bind Parameters with an Open Cypher Query2

      QueuedTask.Run(async () =>
      {
        //assume we have, in this case, a list of ids (perhaps retrieved
        //via a selection, hardcoded (like here), etc.
        var oids = new List<long>() { 3,4,7,8,9,11,12,14,15,19,21,25,29,
            31,32,36,37,51,53,54,55,56,59,63,75,78,80,84,86,88,96,97,98,101,
            103,106};
        //In the query, we refer to the "bind parameter" with the
        //"$" and a variable name - '$object_ids' and '$sel_geom'
        //in this example
        var qry = @"MATCH (p:PhoneNumber) " +
                    @"WHERE p.objectid IN $object_ids AND " +
                    @"esri.graph.ST_Intersects($sel_geom, p.shape) " +
                    @"RETURN p";
        //create a KG query filter
        var kg_qry_filter = new KnowledgeGraphQueryFilter()
        {
          QueryText = qry
        };

        //the bind parameter added to the query filter must refer to
        //the variable name used in the query string (without the "$")
        //Note:
        //Collections must be converted to a KnowledgeGraphArrayValue before
        //being assigned to a BindParameter
        var kg_oid_array = new KnowledgeGraphArrayValue();
        kg_oid_array.AddRange(oids);
        kg_qry_filter.BindParameters["object_ids"] = kg_oid_array;
        kg_qry_filter.BindParameters["sel_geom"] = poly;
        oids.Clear();

        //submit the query
        using (var kgRowCursor = kg.SubmitQuery(kg_qry_filter))
        {
          //wait for rows to be returned asynchronously from the server
          while (await kgRowCursor.WaitForRowsAsync())
          {
            //get the rows using "standard" move next
            while (kgRowCursor.MoveNext())
            {
              //current row is accessible via ".Current" prop of the cursor
              using (var graphRow = kgRowCursor.Current)
              {
                #region Process Row

                var cell_phone = graphRow[0] as KnowledgeGraphEntityValue;
                var oid = cell_phone.GetObjectID();

                var name = (string)cell_phone["FULL_NAME"];
                var ph_number = (string)cell_phone["PHONE_NUMBER"];
                System.Diagnostics.Debug.WriteLine(
                  $"[{oid}] {name}, {ph_number}");

                #endregion
              }
            }
          }
        }
      });

      #endregion
    }

    public async void QueryAndSearch6()
    {
      KnowledgeGraph kg = null;
      KnowledgeGraphCursor kg_rc = null;
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.SubmitQuery
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.SubmitSearch
      //cref: ArcGIS.Core.Data.Realtime.RealtimeCursorBase.WaitForRowsAsync(System.Threading.CancellationToken)
      #region Call WaitForRowsAsync With Cancellation

      //On the QueuedTask...
      //and assuming you have established a connection to a knowledge graph
      //...
      //submit query or search to return a KnowledgeGraphCursor
      //using (var kg_rc = kg.SubmitQuery(kg_qf)) {
      //using (var kg_rc = kg.SubmitSearch(kg_sf)) {
      //...
      //wait for rows to be returned from the server
      //"auto-cancel" after 20 seconds
      var cancel = new CancellationTokenSource(new TimeSpan(0, 0, 20));
      //catch TaskCanceledException
      try
      {
        while (await kg_rc.WaitForRowsAsync(cancel.Token))
        {
          //check for row events
          while (kg_rc.MoveNext())
          {
            using (var graph_row = kg_rc.Current)
            {
              //Graph row is an array, process all returned values...
              var val_count = (int)graph_row.GetCount();
              for (int i = 0; i < val_count; i++)
              {
                var retval = graph_row[i];
                //Process row value (note: recursive)
                //See "Process a KnowledgeGraphRow Value" snippet
                ProcessKnowledgeGraphRowValue(retval);
              }
            }
          }
        }
      }
      //Timeout expired
      catch (TaskCanceledException tce)
      {
        //Handle cancellation as needed
      }
      cancel.Dispose();
      #endregion
    }

    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphValue
    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphValueType
    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphPrimitiveValue.GetValue
    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphArrayValue.GetSize
    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphArrayValue.Item(System.UInt64)
    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphPathValue.GetEntityCount
    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphPathValue.GetEntity
    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphPathValue.GetRelationshipCount
    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphPathValue.GetRelationship
    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphObjectValue.GetKeys
    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphObjectValue.Item(System.String)
    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectValue.GetID
    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectValue.GetObjectID
    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectValue.GetTypeName
    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphEntityValue.GetLabel
    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphRelationshipValue
    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphRelationshipValue.GetHasRelatedEntityIDs
    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphRelationshipValue.GetOriginID
    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphRelationshipValue.GetDestinationID
    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphRow.GetCount
    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphRow.Item(System.Int32)
    #region Process a KnowledgeGraphRow Value

    //Base class for entities and relationships
    //(including documents and provenance)
    public void ProcessGraphNamedObjectValue(
      KnowledgeGraphNamedObjectValue kg_named_obj_val)
    {
      if (kg_named_obj_val is KnowledgeGraphEntityValue kg_entity)
      {
        var label = kg_entity.GetLabel();
        //TODO - use label
      }
      else if (kg_named_obj_val is KnowledgeGraphRelationshipValue kg_rel)
      {
        var has_entity_ids = kg_rel.GetHasRelatedEntityIDs();
        if (kg_rel.GetHasRelatedEntityIDs())
        {
          var origin_id = kg_rel.GetOriginID();
          var dest_id = kg_rel.GetDestinationID();
          //TODO - use ids
        }
      }
      var id = kg_named_obj_val.GetID();
      var oid = kg_named_obj_val.GetObjectID();
      //Note: Typename corresponds to the name of the feature class or table
      //in the relational gdb model -and- to the name of the KnowledgeGraphNamedObjectType
      //in the knowledge graph data model
      var type_name = kg_named_obj_val.GetTypeName();
      //TODO use id, object id, etc.
    }

    //Object values include entities, relationships, and anonymous objects
    public void ProcessGraphObjectValue(KnowledgeGraphObjectValue kg_obj_val)
    {
      switch (kg_obj_val)
      {
        case KnowledgeGraphEntityValue kg_entity:
          ProcessGraphNamedObjectValue(kg_entity);
          break;
        case KnowledgeGraphRelationshipValue kg_rel:
          ProcessGraphNamedObjectValue(kg_rel);
          break;
        default:
          //Anonymous objects
          break;
      }
      //graph object values have a set of properties (equivalent
      //to a collection of key/value pairs)
      var keys = kg_obj_val.GetKeys();
      foreach (var key in keys)
        ProcessKnowledgeGraphRowValue(kg_obj_val[key]);//Recurse
    }

    //Process a KnowledgeGraphValue from a query or search
    public void ProcessGraphValue(KnowledgeGraphValue kg_val)
    {
      switch (kg_val)
      {
        case KnowledgeGraphPrimitiveValue kg_prim:
          //KnowledgeGraphPrimitiveValue not currently used in
          //query and search 
          ProcessKnowledgeGraphRowValue(kg_prim.GetValue());//Recurse
          return;
        case KnowledgeGraphArrayValue kg_array:
          var count = kg_array.GetSize();
          //Recursively process each value in the array
          for (ulong i = 0; i < count; i++)
            ProcessKnowledgeGraphRowValue(kg_array[i]);//Recurse
          return;
        case KnowledgeGraphPathValue kg_path:
          //Entities
          var entity_count = kg_path.GetEntityCount();
          //Recursively process each entity value in the path
          for (ulong i = 0; i < entity_count; i++)
            ProcessGraphObjectValue(kg_path.GetEntity(i));//Recurse

          //Recursively process each relationship value in the path
          var relate_count = kg_path.GetRelationshipCount();
          for (ulong i = 0; i < relate_count; i++)
            ProcessGraphObjectValue(kg_path.GetRelationship(i));//Recurse
          return;
        case KnowledgeGraphObjectValue kg_object:
          ProcessGraphObjectValue(kg_object);//Recurse
          return;
        default:
          var type_string = kg_val.GetType().ToString();
          System.Diagnostics.Debug.WriteLine(
            $"Unknown: '{type_string}'");
          return;
      }
    }

    //Process each value from the KnowledgeGraphRow array
    public void ProcessKnowledgeGraphRowValue(object value)
    {
      switch (value)
      {
        //Graph value?
        case KnowledgeGraphValue kg_val:
          var kg_type = kg_val.KnowledgeGraphValueType.ToString();
          System.Diagnostics.Debug.WriteLine(
            $"KnowledgeGraphValue: '{kg_type}'");
          ProcessGraphValue(kg_val);//Recurse
          return;
        //Primitive types...add additional logic as needed
        case System.DBNull dbn:
          System.Diagnostics.Debug.WriteLine("DBNull.Value");
          return;
        case string str:
          System.Diagnostics.Debug.WriteLine($"'{str}' (string)");
          return;
        case long l_val:
          System.Diagnostics.Debug.WriteLine($"{l_val} (long)");
          return;
        case int i_val:
          System.Diagnostics.Debug.WriteLine($"{i_val} (int)");
          return;
        case short s_val:
          System.Diagnostics.Debug.WriteLine($"{s_val} (short)");
          return;
        case double d_val:
          System.Diagnostics.Debug.WriteLine($"{d_val} (double)");
          return;
        case float f_val:
          System.Diagnostics.Debug.WriteLine($"{f_val} (float)");
          return;
        case DateTime dt_val:
          System.Diagnostics.Debug.WriteLine($"{dt_val} (DateTime)");
          return;
        case DateOnly dt_only_val:
          System.Diagnostics.Debug.WriteLine($"{dt_only_val} (DateOnly)");
          return;
        case TimeOnly tm_only_val:
          System.Diagnostics.Debug.WriteLine($"{tm_only_val} (TimeOnly)");
          return;
        case DateTimeOffset dt_tm_offset_val:
          System.Diagnostics.Debug.WriteLine(
            $"{dt_tm_offset_val} (DateTimeOffset)");
          return;
        case System.Guid guid_val:
          var guid_string = guid_val.ToString("B");
          System.Diagnostics.Debug.WriteLine($"'{guid_string}' (Guid)");
          return;
        case Geometry geom_val:
          var geom_type = geom_val.GeometryType.ToString();
          var is_empty = geom_val.IsEmpty;
          var wkid = geom_val.SpatialReference?.Wkid ?? 0;
          System.Diagnostics.Debug.WriteLine(
            $"geometry: {geom_type}, empty: {is_empty}, sr_wkid {wkid} (shape)");
          return;
        default:
          //Blob? Others?
          var type_str = value.GetType().ToString();
          System.Diagnostics.Debug.WriteLine($"Primitive: {type_str}");
          return;
      }
    }

    // ...submit query or search
    //using (var kg_rc = kg.SubmitQuery(kg_qf)) {
    //using (var kg_rc = kg.SubmitSearch(kg_sf)) {
    //  ...wait for rows ...
    //  while (await kg_rc.WaitForRowsAsync()) {
    //   ...rows have been retrieved
    //   while (kg_rc.MoveNext()) {
    //     ...get the current KnowledgeGraphRow
    //     using (var graph_row = kg_rc.Current) {
    //        var val_count = (int)graph_row.GetCount();
    //        for (int i = 0; i<val_count; i++)
    //           ProcessKnowledgeGraphRowValue(graph_row[i]);

    #endregion

    #region ProSnippet Group: Link Charts
    #endregion

    private void LinkChart1()
    {
      #region Find link chart project items

      // find all the link chart project items
      var linkChartItems = Project.Current.GetItems<MapProjectItem>().Where(pi => pi.MapType == MapType.LinkChart);

      // find a link chart project item by name
      var linkChartItem = Project.Current.GetItems<MapProjectItem>().FirstOrDefault(pi => pi.Name == "Acme Link Chart");
      #endregion

      #region Find link chart map by name
      var projectItem = Project.Current.GetItems<MapProjectItem>().FirstOrDefault(pi => pi.Name == "Acme Link Chart");
      var linkChartMap = projectItem?.GetMap();
      #endregion

      //cref: ArcGIS.Desktop.Mapping.MapView.IsLinkChartView
      //cref: ArcGIS.Desktop.Mapping.Map.IsLinkChart
      #region Does Active MapView contain a link chart map
      var mv = MapView.Active;
      // check the view
      var isLinkChartView = mv.IsLinkChartView;

      // or alternatively get the map and check that
      var map = MapView.Active.Map;
      // check the MapType to determine if it's a link chart map
      var isLinkChart = map.MapType == MapType.LinkChart;
      // or you could use the following
      // var isLinkChart = map.IsLinkChart;

      #endregion

      //cref: ArcGIS.Desktop.Mapping.MapView.IsLinkChartView
      #region Find Link Chart from Map panes
      var mapPanes = FrameworkApplication.Panes.OfType<IMapPane>().ToList();
      var mapPane = mapPanes.FirstOrDefault(
          mp => mp.MapView.IsLinkChartView && mp.MapView.Map.Name == "Acme Link Chart");
      var lcMap = mapPane.MapView.Map;

      #endregion

    }

    private void LinkChart2()
    {

      //cref: ArcGIS.Desktop.Mapping.KnowledgeGraphMappingExtensions.GetLinkChartLayout 
      //cref: ArcGIS.Desktop.Mapping.KnowledgeGraphMappingExtensions.SetLinkChartLayoutAsync 
      //cref: ArcGIS.Core.CIM.KnowledgeLinkChartLayoutAlgorithm
      //cref: ArcGIS.Core.CIM.MapType
      //cref: ArcGIS.Desktop.Mapping.Map.IsLinkChart
      #region Get and set the link chart layout

      var mv = MapView.Active;

      // a MapView can encapsulate a link chart IF it's map
      // is of type MapType.LinkChart
      var map = mv.Map;
      var isLinkChart = map.MapType == MapType.LinkChart;
      // or use the following
      // var isLinkChart = map.IsLinkChart;

      QueuedTask.Run(() =>
      {
        if (isLinkChart)
        {
          // get the layout algorithm
          var layoutAlgorithm = mv.GetLinkChartLayout();

          // toggle the value
          if (layoutAlgorithm == KnowledgeLinkChartLayoutAlgorithm.Geographic_Organic_Standard)
            layoutAlgorithm = KnowledgeLinkChartLayoutAlgorithm.Organic_Standard;
          else
            layoutAlgorithm = KnowledgeLinkChartLayoutAlgorithm.Geographic_Organic_Standard;

          // set it
          mv.SetLinkChartLayoutAsync(layoutAlgorithm);

          // OR set it and force a redraw / update
          // await mv.SetLinkChartLayoutAsync(layoutAlgorithm, true);
        }
      });

      #endregion
    }

    #region ProSnippet Group: Create and Append to Link Charts
    #endregion

    public void CreateLinkChart1()
    {
      var map = MapView.Active?.Map;
      if (map == null) return;
      var kg_layer = map.GetLayersAsFlattenedList()
                        .OfType<KnowledgeGraphLayer>().FirstOrDefault();
      if (kg_layer == null)
        return;
      KnowledgeGraph kg = null;

      //cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet.FromKnowledgeGraph
      //cref: ArcGIS.Desktop.Mapping.MapFactory.CreateLinkChart(System.String,System.Uri,ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet)
      //cref: ArcGIS.Desktop.Mapping.MapFactory.CreateLinkChart(System.String,ArcGIS.Core.Data.Knowledge.KnowledgeGraph,ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet)
      //cref: ArcGIS.Desktop.Mapping.KnowledgeGraphFilterType.AllNamedObjects
      #region Create a Link Chart Containing All Records for a KG

      QueuedTask.Run(() =>
      {
        //Create the link chart and show it
        //build the IDSet using KnowledgeGraphFilterType.AllNamedObjects
        var idSet = KnowledgeGraphLayerIDSet.FromKnowledgeGraph(
          kg, KnowledgeGraphFilterType.AllNamedObjects);
        var linkChart = MapFactory.Instance.CreateLinkChart(
                          "KG Link Chart", kg, idSet);
        FrameworkApplication.Panes.CreateMapPaneAsync(linkChart);
      });

      #endregion
    }

    public void CreateLinkChart1b()
    {
      var map = MapView.Active?.Map;
      if (map == null) return;
      var kg_layer = map.GetLayersAsFlattenedList()
                        .OfType<KnowledgeGraphLayer>().FirstOrDefault();
      if (kg_layer == null)
        return;
      KnowledgeGraph kg = null;

      //cref: ArcGIS.Desktop.Mapping.MapFactory.CreateLinkChart(System.String,System.Uri,ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet)
      //cref: ArcGIS.Desktop.Mapping.MapFactory.CreateLinkChart(System.String,ArcGIS.Core.Data.Knowledge.KnowledgeGraph,ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet)
      #region Create a Link Chart With an Empty KG Layer

      QueuedTask.Run(() =>
      {
        //Create the link chart with a -null- id set
        //This will create a KG Layer with empty sub-layers
        //(Note: you cannot create an empty KG layer on a map - only on a link chart)
        var linkChart = MapFactory.Instance.CreateLinkChart(
                          "KG Link Chart", kg, null);
        FrameworkApplication.Panes.CreateMapPaneAsync(linkChart);
      });

      #endregion
    }

    protected void CreateLinkChart2()
    {

      //cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet.FromKnowledgeGraph
      //cref: ArcGIS.Desktop.Mapping.MapFactory.CreateLinkChart(System.String,System.Uri,ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet)
      //cref: ArcGIS.Desktop.Mapping.MapFactory.CreateLinkChart(System.String,ArcGIS.Core.Data.Knowledge.KnowledgeGraph,ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet)
      //cref: ArcGIS.Desktop.Mapping.KnowledgeGraphFilterType.AllEntities
      #region Create a link chart with all the entities of the Knowledge Graph
      string url =
              @"https://acme.server.com/server/rest/services/Hosted/AcmeKnowledgeGraph/KnowledgeGraphServer";

      QueuedTask.Run(() =>
      {
        using (var kg = new KnowledgeGraph(new KnowledgeGraphConnectionProperties(new Uri(url))))
        {
          var idSet = KnowledgeGraphLayerIDSet.FromKnowledgeGraph(
            kg, KnowledgeGraphFilterType.AllEntities);
          var newLinkChart = MapFactory.Instance.CreateLinkChart(
            "All_Entities link chart", kg, idSet);
        };
      });

      #endregion
    }

    public void CreateLinkChart3()
    {
      var map = MapView.Active?.Map;
      if (map == null) return;
      var kg_layer = map.GetLayersAsFlattenedList()
                        .OfType<KnowledgeGraphLayer>().FirstOrDefault();
      if (kg_layer == null)
        return;

      //cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet.FromDictionary
      //cref: ArcGIS.Desktop.Mapping.MapFactory.CreateLinkChart(System.String,System.Uri,ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet)
      //cref: ArcGIS.Desktop.Mapping.MapFactory.CreateLinkChart(System.String,ArcGIS.Core.Data.Knowledge.KnowledgeGraph,ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet)
      #region Create a Link Chart from a query

      //use the results of a query to create an idset. Create the link chart
      //containing just records corresponding to the query results
      var qry = @"MATCH (p1:PhoneNumber)-[r1:MADE_CALL|RECEIVED_CALL]->(c1:PhoneCall)<-" +
                @"[r2:MADE_CALL|RECEIVED_CALL]-(p2:PhoneNumber)-[r3:MADE_CALL|RECEIVED_CALL]" +
                @"->(c2:PhoneCall)<-[r4:MADE_CALL|RECEIVED_CALL]-(p3:PhoneNumber) " +
                @"WHERE p1.FULL_NAME = ""Robert Johnson"" AND " +
                @"p3.FULL_NAME= ""Dan Brown"" AND " +
                @"p1.globalid <> p2.globalid AND " +
                @"p2.globalid <> p3.globalid " +
                @"RETURN p1, r1, c1, r2, p2, r3, c2, r4, p3";

      var dict = new Dictionary<string, List<long>>();

      QueuedTask.Run(async () =>
      {
        using (var kg = kg_layer.GetDatastore())
        {
          var graphQuery = new KnowledgeGraphQueryFilter()
          {
            QueryText = qry
          };

          using (var kgRowCursor = kg.SubmitQuery(graphQuery))
          {
            while (await kgRowCursor.WaitForRowsAsync())
            {
              while (kgRowCursor.MoveNext())
              {
                using (var graphRow = kgRowCursor.Current)
                {
                  // process the row
                  var cnt_val = (int)graphRow.GetCount();
                  for (int v = 0; v < cnt_val; v++)
                  {
                    var obj_val = graphRow[v] as KnowledgeGraphNamedObjectValue;
                    var type_name = obj_val.GetTypeName();
                    var oid = (long)obj_val.GetObjectID();
                    if (!dict.ContainsKey(type_name))
                    {
                      dict[type_name] = new List<long>();
                    }
                    if (!dict[type_name].Contains(oid))
                      dict[type_name].Add(oid);
                  }
                }
              }
            }
          }
          //make an ID Set to create the LinkChart
          var idSet = KnowledgeGraphLayerIDSet.FromDictionary(kg, dict);

          //Create the link chart and show it
          var linkChart = MapFactory.Instance.CreateLinkChart(
                            "KG With ID Set", kg, idSet);
          FrameworkApplication.Panes.CreateMapPaneAsync(linkChart);
        }
      });

      #endregion
    }

    protected void CreateLinkChart4()
    {
      KnowledgeGraphLayerIDSet idSet = null;

      //cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet.FromKnowledgeGraph
      //cref: ArcGIS.Desktop.Mapping.MapFactory.CreateLinkChart(System.String,System.Uri,ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet,System.String)
      //cref: ArcGIS.Desktop.Mapping.MapFactory.CreateLinkChart(System.String,ArcGIS.Core.Data.Knowledge.KnowledgeGraph,ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet,System.String)
      //cref: ArcGIS.Desktop.Mapping.KnowledgeGraphFilterType.AllEntities
      #region Create a link chart based on a template link chart

      // note that the template link chart MUST use the same KG server

      string url =
              @"https://acme.server.com/server/rest/services/Hosted/AcmeKnowledgeGraph/KnowledgeGraphServer";

      QueuedTask.Run(() =>
      {
        // find the existing link chart by name
        var projectItem = Project.Current.GetItems<MapProjectItem>()
        .FirstOrDefault(pi => pi.Name == "Acme Link Chart");
        var linkChartMap = projectItem?.GetMap();
        if (linkChartMap == null)
          return;

        //Create a connection properties
        var kg_props =
            new KnowledgeGraphConnectionProperties(new Uri(url));
        try
        {
          //Open a connection
          using (var kg = new KnowledgeGraph(kg_props))
          {
            //Add all entities to the link chart
            var idSet = KnowledgeGraphLayerIDSet.FromKnowledgeGraph(
                  kg, KnowledgeGraphFilterType.AllEntities);
            //Create the new link chart and show it
            var newLinkChart = MapFactory.Instance.CreateLinkChart(
                              "KG from Template", kg, idSet, linkChartMap.URI);
            FrameworkApplication.Panes.CreateMapPaneAsync(newLinkChart);
          }
        }
        catch (Exception ex)
        {
          System.Diagnostics.Debug.WriteLine(ex.ToString());
        }
      });



      #endregion
    }

    protected void CreateLinkChart5()
    {
      KnowledgeGraph kg = null;

      //cref: ArcGIS.Desktop.KnowledgeGraph.KnowledgeGraphLayerException
      //cref: ArcGIS.Desktop.Mapping.MapFactory.CreateLinkChart(System.String,System.Uri,ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet,System.String)
      //cref: ArcGIS.Desktop.Mapping.MapFactory.CreateLinkChart(System.String,ArcGIS.Core.Data.Knowledge.KnowledgeGraph,ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet,System.String)
      #region Checking KnowledgeGraphLayerException

      // running on QueuedTask

      var dict = new Dictionary<string, List<long>>();
      dict.Add("person", new List<long>());  //Empty list means all records
      dict.Add("made_call", null);  //null list means all records

      // or specific records - however the ids are obtained
      dict.Add("phone_call", new List<long>() { 1, 5, 18, 36, 78 });

      // make the id set
      var idSet = KnowledgeGraphLayerIDSet.FromDictionary(kg, dict);

      try
      {
        //Create the link chart and show it
        var linkChart = MapFactory.Instance.CreateLinkChart(
                          "KG With ID Set", kg, idSet);
        FrameworkApplication.Panes.CreateMapPaneAsync(linkChart);
      }
      catch (KnowledgeGraphLayerException e)
      {
        // get the invalid named types
        //   remember that the named types are case-sensitive
        var invalidNamedTypes = e.InvalidNamedTypes;

        // do something with the invalid named types 
        // for example - log or return to caller to show message to user
      }

      #endregion
    }

    protected void AppendLinkChart1()
    {
      var map = MapView.Active?.Map;
      if (map == null) return;
      var kg_layer = map.GetLayersAsFlattenedList()
                        .OfType<KnowledgeGraphLayer>().FirstOrDefault();
      if (kg_layer == null)
        return;

      //cref: ArcGIS.Desktop.Mapping.KnowledgeGraphMappingExtensions.CanAppendToLinkChart
      //cref: ArcGIS.Desktop.Mapping.KnowledgeGraphMappingExtensions.AppendToLinkChart
      #region Append to Link Chart

      //We create an id set to contain the records to be appended
      var dict = new Dictionary<string, List<long>>();
      dict["Suspects"] = new List<long>(); 

      //In this case, via results from a query...
      var qry2 = "MATCH (s:Suspects) RETURN s";

      QueuedTask.Run(async () =>
      {
        using (var kg = kg_layer.GetDatastore())
        {
          var graphQuery = new KnowledgeGraphQueryFilter()
          {
            QueryText = qry2
          };

          using (var kgRowCursor = kg.SubmitQuery(graphQuery))
          {
            while (await kgRowCursor.WaitForRowsAsync())
            {
              while (kgRowCursor.MoveNext())
              {
                using (var graphRow = kgRowCursor.Current)
                {
                  var obj_val = graphRow[0] as KnowledgeGraphNamedObjectValue;
                  var oid = (long)obj_val.GetObjectID();
                  dict["Suspects"].Add(oid);
                }
              }
            }
          }

          //make an ID Set to append to the LinkChart
          var idSet = KnowledgeGraphLayerIDSet.FromDictionary(kg, dict);
          //Get the relevant link chart to which records will be
          //appended...in this case, from an open map pane in the
          //Pro application...
          var mapPanes = FrameworkApplication.Panes.OfType<IMapPane>().ToList();
          var mapPane = mapPanes.First(
            mp => mp.MapView.IsLinkChartView && 
            mp.MapView.Map.Name == "Acme Link Chart");
          var linkChartMap = mapPane.MapView.Map;

          //or get the link chart from an item in the catalog...etc.,etc.
          //var projectItem = Project.Current.GetItems<MapProjectItem>()
          //      .FirstOrDefault(pi => pi.Name == "Acme Link Chart");
          //var linkChartMap = projectItem?.GetMap();

          //Call AppendToLinkChart with the id set
          if (linkChartMap.CanAppendToLinkChart(idSet))
            linkChartMap.AppendToLinkChart(idSet);
        }
      });

      #endregion
    }

    #region ProSnippet Group: ID Sets
    #endregion

    public void IDSet1()
    {
      KnowledgeGraphLayer kgLayer = null;
      Map map = null;

      //cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayer.GetIDSet
      //cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet.IsEmpty
      //cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet.NamedObjectTypeCount
      //cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet.Contains
      //cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet.ToOIDDictionary
      //cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet.ToUIDDictionary
      #region Get the ID Set of a KG layer

      QueuedTask.Run(() =>
      {
        var idSet = kgLayer.GetIDSet();

        // is the set empty?
        var isEmpty = idSet.IsEmpty;
        // get the count of named object types
        var countNamedObjects = idSet.NamedObjectTypeCount;
        // does it contain the entity "Species";
        var contains = idSet.Contains("Species");

        // get the idSet as a dictionary of namedObjectType and oids
        var oidDict = idSet.ToOIDDictionary();
        var speciesOIDs = oidDict["Species"];

        // alternatively get the idSet as a dictionary of 
        // namedObjectTypes and uuids
        var uuidDict = idSet.ToUIDDictionary();
        var speciesUuids = uuidDict["Species"];

      });
      #endregion

      //cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet
      //cref: ArcGIS.Desktop.Mapping.KnowledgeGraphLayerIDSet.FromSelectionSet
      #region Create an ID set from a SelectionSet

      QueuedTask.Run(() =>
      {
        // get the selection set
        var sSet = map.GetSelection();

        // translate to an KnowledgeGraphLayerIDSet
        //  if the selectionset does not contain any KG entity or relationship records
        //    then idSet will be null  
        var idSet = KnowledgeGraphLayerIDSet.FromSelectionSet(sSet);
        if (idSet == null)
          return;


        // you can use the idSet to create a new linkChart
        //   (using MapFactory.Instance.CreateLinkChart)
      });
      #endregion
    }
  }
}
