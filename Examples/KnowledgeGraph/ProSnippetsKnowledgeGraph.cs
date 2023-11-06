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
using ArcGIS.Desktop.Framework.Threading.Tasks;
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
      #region Getting a Connection from a KnowledgeGraphLayer 

      var kgLayer = MapView.Active.Map.GetLayersAsFlattenedList()
              .OfType<KnowledgeGraphLayer>().FirstOrDefault();
      //KnowledgeGraphLayer is a composite layer - get the first 
      //child feature layer or standalone table
      QueuedTask.Run(() =>
      {
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
      #region Retrieving FeatureClasses and Definitions 

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
      #region Retrieving Tables and Definitions 

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
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.GetSpatialReference()
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

    public void DataModel5()
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

    public void DataModel6()
    {
      var url = "";
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphDataModel.GetEntityTypes
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphEntityType
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectType.GetRole
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphNamedObjectType.GetName
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

    public void DataModel7()
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

    #region ProSnippet Group: Graph Query and Text Search
    #endregion

    public async void QueryAndSearch1()
    {
      KnowledgeGraph kg = null;
      bool includeProvenanceIfPresent = false;
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.SubmitQuery
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraph.KnowledgeGraphQueryFilter.#Ctor
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
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphCursor.MoveNext
      //cref: ArcGIS.Core.Data.Realtime.RealtimeCursorBase.WaitForRowsAsync()
      //cref: ArcGIS.Core.Data.Realtime.RealtimeCursorBase.WaitForRowsAsync(System.Threading.CancellationToken)
      //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphCursor.Current
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
      }//SubmitSearch
      #endregion
    }

    public async void QueryAndSearch3()
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
    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphValue.KnowledgeGraphValueType
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
    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphRelationshipValue.GetHasRelatedEntityIDs
    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphRelationshipValue.GetOriginID
    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphRelationshipValue.GetDestinationID
    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphRelationshipValue.GetHasRelatedEntityIDs
    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphCursor.MoveNext
    //cref: ArcGIS.Core.Data.Knowledge.KnowledgeGraphCursor.Current
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
  }
}
