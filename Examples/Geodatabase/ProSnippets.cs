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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Mapping;
using Version = ArcGIS.Core.Data.Version;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Core.Data.DDL;
using ArcGIS.Core.Data.Mapping;
using FieldDescription = ArcGIS.Core.Data.DDL.FieldDescription;
using ArcGIS.Core.Data.Exceptions;
using SpatialReference = ArcGIS.Core.Geometry.SpatialReference;

namespace GeodatabaseSDK.GeodatabaseSDK.Snippets
{

    class Snippets
    {
        #region ProSnippet Group: Geodatabases and Datastores
        #endregion

        // cref: ARCGIS.CORE.DATA.FILEGEODATABASECONNECTIONPATH.#CTOR
        // cref: ARCGIS.CORE.DATA.GEODATABASE.#CTOR(ARCGIS.CORE.DATA.FILEGEODATABASECONNECTIONPATH)
        #region Opening a File Geodatabase given the path
        public async Task OpenFileGDB()
        {
            try
            {
                await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
                {
                    // Opens a file geodatabase. This will open the geodatabase if the folder exists and contains a valid geodatabase.
                    using (
              Geodatabase geodatabase =
                new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(@"C:\Data\LocalGovernment.gdb"))))
                    {
                        // Use the geodatabase.
                    }
                });
            }
            catch (GeodatabaseNotFoundOrOpenedException exception)
            {
                // Handle Exception.
            }

        }

        #endregion Opening a File Geodatabase given the path

        // cref: ARCGIS.CORE.DATA.DATABASE.#CTOR(ARCGIS.CORE.DATA.DATABASECONNECTIONPROPERTIES)
        // cref: ARCGIS.CORE.DATA.DATABASECONNECTIONPROPERTIES.#CTOR(ARCGIS.CORE.DATA.ENTERPRISEDATABASETYPE)
        // cref: ARCGIS.CORE.DATA.GEODATABASE.#CTOR(ARCGIS.CORE.DATA.DATABASECONNECTIONPROPERTIES)
        // cref: ARCGIS.CORE.DATA.DATABASECONNECTIONPROPERTIES.AuthenticationMode
        // cref: ARCGIS.CORE.DATA.DATABASECONNECTIONPROPERTIES.Instance
        // cref: ARCGIS.CORE.DATA.DATABASECONNECTIONPROPERTIES.Database
        // cref: ARCGIS.CORE.DATA.DATABASECONNECTIONPROPERTIES.User
        // cref: ARCGIS.CORE.DATA.DATABASECONNECTIONPROPERTIES.Password
        // cref: ARCGIS.CORE.DATA.DATABASECONNECTIONPROPERTIES.Version
        // cref: ARCGIS.CORE.DATA.AuthenticationMode
        #region Opening an Enterprise Geodatabase using connection properties
        public async Task OpenEnterpriseGeodatabase()
        {
            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                // Opening a Non-Versioned SQL Server instance.
                DatabaseConnectionProperties connectionProperties = new DatabaseConnectionProperties(EnterpriseDatabaseType.SQLServer)
                {
                    AuthenticationMode = AuthenticationMode.DBMS,

                    // Where testMachine is the machine where the instance is running and testInstance is the name of the SqlServer instance.
                    Instance = @"testMachine\testInstance",

                    // Provided that a database called LocalGovernment has been created on the testInstance and geodatabase has been enabled on the database.
                    Database = "LocalGovernment",

                    // Provided that a login called gdb has been created and corresponding schema has been created with the required permissions.
                    User = "gdb",
                    Password = "password",
                    Version = "dbo.DEFAULT"
                };

                using (Geodatabase geodatabase = new Geodatabase(connectionProperties))
                {
                    // Use the geodatabase
                }
            });
        }

        #endregion Opening an Enterprise Geodatabase using connection properties

        // cref: ARCGIS.CORE.DATA.DATABASECONNECTIONFILE.#CTOR
        // cref: ARCGIS.CORE.DATA.GEODATABASE.#CTOR(ARCGIS.CORE.DATA.DATABASECONNECTIONFILE)
        #region Opening an Enterprise Geodatabase using sde file path
        public async Task OpenEnterpriseGeodatabaseUsingSDEFilePath()
        {
            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
                {
                    // Use the geodatabase.
                }
            });
        }
        #endregion Opening an Enterprise Geodatabase using sde file path

        // cref: ARCGIS.DESKTOP.CORE.ITEM.GETITEMS
        // cref: ARCGIS.DESKTOP.CATALOG.GDBPROJECTITEM
        // cref: ARCGIS.DESKTOP.CATALOG.GDBPROJECTITEM.GetDatastore
        // cref: ARCGIS.CORE.DATA.UnknownDatastore
        #region Obtaining Geodatabase from Project Item
        public async Task ObtainingGeodatabaseFromProjectItem()
        {
            IEnumerable<GDBProjectItem> gdbProjectItems = Project.Current.GetItems<GDBProjectItem>();

            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                foreach (GDBProjectItem gdbProjectItem in gdbProjectItems)
                {
                    using (Datastore datastore = gdbProjectItem.GetDatastore())
                    {
                        //Unsupported datastores (non File GDB and non Enterprise GDB) will be of type UnknownDatastore
                        if (datastore is UnknownDatastore)
                            continue;

                        Geodatabase geodatabase = datastore as Geodatabase;
                        // Use the geodatabase.
                    }
                }
            });
        }
        #endregion Obtaining Geodatabase from Project Item

        // cref: ArcGIS.Core.Data.DatabaseClient.GetDatabaseConnectionProperties
        public void GettingConnectionProperties()
        {
            #region Getting Database Connection Properties from a Connection File

            DatabaseConnectionFile connectionFile = new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"));
            DatabaseConnectionProperties connectionProperties = DatabaseClient.GetDatabaseConnectionProperties(connectionFile);

            // Now you could, for example, change the user name and password in the connection properties prior to use them to open a geodatabase

            #endregion
        }

        // cref: ARCGIS.DESKTOP.MAPPING.MAP.LAYERS
        // cref: ARCGIS.DESKTOP.MAPPING.BasicFeatureLayer.GetTable
        // cref: ARCGIS.CORE.DATA.DATASET.GETDATASTORE
        #region Obtaining Geodatabase from FeatureLayer
        public async Task ObtainingGeodatabaseFromFeatureLayer()
        {
            IEnumerable<Layer> layers = MapView.Active.Map.Layers.Where(layer => layer is FeatureLayer);

            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                foreach (FeatureLayer featureLayer in layers)
                {
                    using (Table table = featureLayer.GetTable())
                    using (Datastore datastore = table.GetDatastore())
                    {
                        if (datastore is UnknownDatastore)
                            continue;

                        Geodatabase geodatabase = datastore as Geodatabase;
                    }
                }
            });
        }
        #endregion Obtaining Geodatabase from FeatureLayer

        // cref: ARCGIS.CORE.DATA.DATABASECLIENT.EXECUTESTATEMENT
        #region Executing SQL Statements
        // Executes raw SQL on the underlying database management system.
        //  Any SQL is permitted (DDL or DML), but no results can be returned
        public void ExecuteSQLOnGeodatabase(Geodatabase geodatabase, string statement)
        {
            QueuedTask.Run(() =>
            {
                DatabaseClient.ExecuteStatement(geodatabase, statement);
            });
        }
        #endregion

        #region ProSnippet Group: Definitions
        #endregion

        // cref: ARCGIS.CORE.DATA.TABLEDEFINITION
        // cref: ARCGIS.CORE.DATA.FEATURECLASSDEFINITION
        // cref: ARCGIS.CORE.DATA.RELATIONSHIPCLASSDEFINITION
        // cref: ARCGIS.CORE.DATA.FEATUREDATASETDEFINITION
        // cref: ARCGIS.CORE.DATA.GEODATABASE.GETDEFINITION<T>(string)
        // cref: ARCGIS.CORE.DATA.DatabaseConnectionFile.#Ctor
        #region Obtaining Definition from Geodatabase
        public async Task ObtainingDefinitionFromGeodatabase()
        {
            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
                {
                    // Remember that for Enterprise databases you have to qualify your dataset names with the DatabaseName and UserName.
                    TableDefinition enterpriseTableDefinition = geodatabase.GetDefinition<TableDefinition>("LocalGovernment.GDB.CitizenContactInfo");

                    // It does not matter if the dataset is within a FeatureDataset or not.
                    FeatureClassDefinition featureClassDefinition = geodatabase.GetDefinition<FeatureClassDefinition>("LocalGovernment.GDB.FireStation");

                    // GetDefinition For a RelationshipClass.
                    RelationshipClassDefinition relationshipClassDefinition = geodatabase.GetDefinition<RelationshipClassDefinition>("LocalGovernment.GDB.AddressPointHasSiteAddresses");

                    // GetDefinition For a FeatureDataset.
                    FeatureDatasetDefinition featureDatasetDefinition = geodatabase.GetDefinition<FeatureDatasetDefinition>("LocalGovernment.GDB.Address");
                }
            });
        }
        #endregion Obtaining Definition from Geodatabase

        // cref: ARCGIS.CORE.DATA.TABLEDEFINITION
        // cref: ARCGIS.CORE.DATA.FEATURECLASSDEFINITION
        // cref: ARCGIS.CORE.DATA.RELATIONSHIPCLASSDEFINITION
        // cref: ARCGIS.CORE.DATA.FEATUREDATASETDEFINITION
        // cref: ARCGIS.CORE.DATA.GEODATABASE.GETDEFINITIONS<T>()
        // cref: ARCGIS.CORE.DATA.TableDefinition.HasGlobalID()
        // cref: ARCGIS.CORE.DATA.DEFINITION.GETNAME
        #region Obtaining List of Definitions from Geodatabase
        public async Task ObtainingDefinitionsFromGeodatabase()
        {
            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
                {
                    IReadOnlyList<FeatureClassDefinition> enterpriseDefinitions = geodatabase.GetDefinitions<FeatureClassDefinition>();
                    IEnumerable<Definition> featureClassesHavingGlobalID = enterpriseDefinitions.Where(definition => definition.HasGlobalID());

                    IReadOnlyList<FeatureDatasetDefinition> featureDatasetDefinitions = geodatabase.GetDefinitions<FeatureDatasetDefinition>();
                    bool electionRelatedFeatureDatasets = featureDatasetDefinitions.Any(definition => definition.GetName().Contains("Election"));

                    IReadOnlyList<AttributedRelationshipClassDefinition> attributedRelationshipClassDefinitions = geodatabase.GetDefinitions<AttributedRelationshipClassDefinition>();

                    IReadOnlyList<RelationshipClassDefinition> relationshipClassDefinitions = geodatabase.GetDefinitions<RelationshipClassDefinition>();
                }
            });
        }
        #endregion Obtaining List of Defintions from Geodatabase

        // cref: ARCGIS.CORE.DATA.RELATIONSHIPCLASSDEFINITION
        // cref: ARCGIS.CORE.DATA.GEODATABASE.GETRELATEDDEFINITIONS
        // cref: ARCGIS.CORE.DATA.FEATURECLASSDEFINITION
        // cref: ARCGIS.CORE.DATA.DefinitionRelationshipType
        #region Obtaining Related Definitions from Geodatabase
        public async Task ObtainingRelatedDefinitionsFromGeodatabase()
        {
            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
                {
                    // Remember the qualification of DatabaseName. for the RelationshipClass.

                    RelationshipClassDefinition enterpriseDefinition = geodatabase.GetDefinition<RelationshipClassDefinition>("LocalGovernment.GDB.AddressPointHasSiteAddresses");
                    IReadOnlyList<Definition> enterpriseDefinitions = geodatabase.GetRelatedDefinitions(enterpriseDefinition, DefinitionRelationshipType.DatasetsRelatedThrough);
                    FeatureClassDefinition enterpriseAddressPointDefinition = enterpriseDefinitions.First(defn => defn.GetName().Equals("LocalGovernment.GDB.AddressPoint"))
                as FeatureClassDefinition;

                    FeatureDatasetDefinition featureDatasetDefinition = geodatabase.GetDefinition<FeatureDatasetDefinition>("LocalGovernment.GDB.Address");
                    IReadOnlyList<Definition> datasetsInAddressDataset = geodatabase.GetRelatedDefinitions(featureDatasetDefinition, DefinitionRelationshipType.DatasetInFeatureDataset);
                    FeatureClassDefinition addressPointInAddressDataset = datasetsInAddressDataset.First(defn => defn.GetName().Equals("LocalGovernment.GDB.AddressPoint"))
                as FeatureClassDefinition;

                    RelationshipClassDefinition addressPointHasSiteAddressInAddressDataset = datasetsInAddressDataset.First(defn => defn.GetName().Equals("LocalGovernment.GDB.AddressPointHasSiteAddresses"))
                as RelationshipClassDefinition;
                }
            });
        }
        #endregion Obtaining Related Definitions from Geodatabase

        // cref: ARCGIS.CORE.DATA.TABLE.ISJOINEDTABLE
        // cref: ARCGIS.CORE.DATA.JOIN.GETORIGINTABLE
        // cref: ARCGIS.CORE.DATA.TABLE.GETJOIN
        // cref: ARCGIS.CORE.DATA.TABLE.GetDefinition
        // cref: ARCGIS.CORE.DATA.FeatureClass.GetDefinition
        #region Getting a Table Definition from a Layer
        // GetDefinitionFromLayer - This code works even if the layer has a join to another table
        private TableDefinition GetDefinitionFromLayer(FeatureLayer featureLayer)
        {
            // Get feature class from the layer
            FeatureClass featureClass = featureLayer.GetFeatureClass();

            // Determine if feature class is a join
            if (featureClass.IsJoinedTable())
            {
                // Get join from feature class
                Join join = featureClass.GetJoin();

                // Get origin table from join
                Table originTable = join.GetOriginTable();

                // Return feature class definition from the join's origin table
                return originTable.GetDefinition();
            }
            else
            {
                return featureClass.GetDefinition();
            }
        }
        #endregion

        #region ProSnippet Group: Datasets
        #endregion

        // cref: ARCGIS.CORE.DATA.GEODATABASE.OPENDATASET<T>(string)
        #region Opening datasets from Geodatabase
        public async Task OpeningDatasetsFromGeodatabase()
        {
            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
                {
                    using (Table table = geodatabase.OpenDataset<Table>("LocalGovernment.GDB.EmployeeInfo"))
                    {
                    }

                    // Open a featureClass (within a feature dataset or outside a feature dataset).
                    using (FeatureClass featureClass = geodatabase.OpenDataset<FeatureClass>("LocalGovernment.GDB.AddressPoint"))
                    {
                    }

                    // You can open a FeatureClass as a Table which will give you a Table Reference.
                    using (Table featureClassAsTable = geodatabase.OpenDataset<Table>("LocalGovernment.GDB.AddressPoint"))
                    {
                        // But it is really a FeatureClass object.
                        FeatureClass featureClassOpenedAsTable = featureClassAsTable as FeatureClass;
                    }

                    // Open a FeatureDataset.
                    using (FeatureDataset featureDataset = geodatabase.OpenDataset<FeatureDataset>("LocalGovernment.GDB.Address"))
                    {
                    }

                    // Open a RelationsipClass.  Just as you can open a FeatureClass as a Table, you can also open an AttributedRelationshipClass as a RelationshipClass.
                    using (RelationshipClass relationshipClass = geodatabase.OpenDataset<RelationshipClass>("LocalGovernment.GDB.AddressPointHasSiteAddresses"))
                    {
                    }
                }
            });
        }
        #endregion Opening datasets from Geodatabase

        // cref: ARCGIS.CORE.DATA.GEODATABASE.GETDEFINITION<T>(string)
        // cref: ArcGIS.Core.CoreObjectsBase.Dispose
        #region Checking for the existence of a Table
        // Must be called within QueuedTask.Run9)
        public bool TableExists(Geodatabase geodatabase, string tableName)
        {
            try
            {
                TableDefinition tableDefinition = geodatabase.GetDefinition<TableDefinition>(tableName);
                tableDefinition.Dispose();
                return true;
            }
            catch
            {
                // GetDefinition throws an exception if the definition doesn't exist
                return false;
            }
        }
        #endregion

        // cref: ARCGIS.CORE.DATA.GEODATABASE.GETDEFINITION<T>(string)
        // cref: ArcGIS.Core.CoreObjectsBase.Dispose
        #region Checking for the existence of a Feature Class
        // Must be called within QueuedTask.Run()
        public bool FeatureClassExists(Geodatabase geodatabase, string featureClassName)
        {
            try
            {
                FeatureClassDefinition featureClassDefinition = geodatabase.GetDefinition<FeatureClassDefinition>(featureClassName);
                featureClassDefinition.Dispose();
                return true;
            }
            catch
            {
                // GetDefinition throws an exception if the definition doesn't exist
                return false;
            }
        }
        #endregion

        // cref: ArcGIS.Core.Data.Geodatabase.OpenRelationshipClasses(ArcGIS.Core.Data.Table,ArcGIS.Core.Data.Table)
        // cref: ArcGIS.Core.Data.Geodatabase.OpenRelationshipClasses(System.String,System.String)
        #region Opening RelationshipClass between two Tables

        // Must be called within QueuedTask.Run().  
        // When used with file or enterprise geodatabases, this routine takes two table names.
        // When used with feature services, this routine takes layer IDs, or the names of the tables as they are exposed through the service (e.g., "L0States")
        public IReadOnlyList<RelationshipClass> OpenRelationshipClassFeatureServices(Geodatabase geodatabase, string originClass, string destinationClass)
        {
            return geodatabase.OpenRelationshipClasses(originClass, destinationClass);
        }
        #endregion Opening RelationshipClass between two Tables

        // cref: ARCGIS.CORE.DATA.GEODATABASE.GETRELATEDDEFINITIONS
        // cref: ARCGIS.CORE.DATA.GEODATABASE.GETDEFINITIONS<T>()
        // cref: ARCGIS.CORE.DATA.RelationshipClassDefinition
        // cref: ARCGIS.CORE.DATA.DefinitionRelationshipType
        #region Obtaining related Feature Classes from a Relationship Class
        public async Task GetFeatureClassesInRelationshipClassAsync()
        {
            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                using (Geodatabase geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(@"C:\Data\LocalGovernment.gdb"))))
                {
                    IReadOnlyList<RelationshipClassDefinition> relationshipClassDefinitions = geodatabase.GetDefinitions<RelationshipClassDefinition>();

                    foreach (RelationshipClassDefinition relationshipClassDefinition in relationshipClassDefinitions)
                    {
                        IReadOnlyList<Definition> definitions = geodatabase.GetRelatedDefinitions(relationshipClassDefinition, DefinitionRelationshipType.DatasetsRelatedThrough);

                        foreach (Definition definition in definitions)
                        {
                            System.Diagnostics.Debug.WriteLine($"Feature class in the RelationshipClass is:{definition.GetName()}");
                        }
                    }
                }
            });
        }
        #endregion

        // cref: ArcGIS.Core.Data.FileSystemConnectionPath.#ctor(System.Uri, ArcGIS.Core.Data.FileSystemDatastoreType)
        // cref: ArcGIS.Core.Data.FileSystemDatastoreType
        // cref: ARCGIS.CORE.DATA.FILESYSTEMDATASTORE.OPENDATASET<T>(string)
        // cref: ARCGIS.CORE.DATA.FILESYSTEMDATASTORE.#CTOR(ARCGIS.CORE.DATA.FILESYSTEMCONNECTIONPATH)
        #region Opening a FeatureClass from a ShapeFile Datastore
        public async Task OpenShapefileFeatureClass()
        {
            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                FileSystemConnectionPath fileConnection = new FileSystemConnectionPath(new Uri("path\\to\\folder\\containing\\shapefiles"), FileSystemDatastoreType.Shapefile);
                using (FileSystemDatastore shapefile = new FileSystemDatastore(fileConnection))
                {
                    FeatureClass taxLotsFeatureClass = shapefile.OpenDataset<FeatureClass>("TaxLots");
                    FeatureClass manHolesFeatureClass = shapefile.OpenDataset<FeatureClass>("ManHoles.shp"); // Can use the .shp extension, but its not needed.
                    Table taxDetailsTableWithoutExtension = shapefile.OpenDataset<Table>("TaxDetails");
                    Table taxDetailsTable = shapefile.OpenDataset<Table>("TaxDetails.dbf");
                }
            });
        }
        #endregion Opening a FeatureClass from a ShapeFile Datastore

        // cref: ArcGIS.Core.Data.FileSystemConnectionPath.#ctor(System.Uri, ArcGIS.Core.Data.FileSystemDatastoreType)
        // cref: ArcGIS.Core.Data.FileSystemDatastoreType
        // cref: ARCGIS.CORE.DATA.FILESYSTEMDATASTORE.OPENDATASET<T>(string)
        // cref: ARCGIS.CORE.DATA.FILESYSTEMDATASTORE.#CTOR(ARCGIS.CORE.DATA.FILESYSTEMCONNECTIONPATH)
        #region Opening a CAD Datastore
        public async Task OpenCADFeatureClass()
        {
            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                FileSystemConnectionPath fileConnection = new FileSystemConnectionPath(new Uri("path\\to\\folder\\containing\\CAD"), FileSystemDatastoreType.Cad);
                using (FileSystemDatastore cadDatastore = new FileSystemDatastore(fileConnection))
                {
                    // note - extension is required
                    FeatureClass cadDataset = cadDatastore.OpenDataset<FeatureClass>("hatchplayboundaries.dwg");
                    // take note of the pattern for referencing a feature class. 
                    FeatureClass cadfeatureClass = cadDatastore.OpenDataset<FeatureClass>("hatchplayboundaries.dwg:Polyline");

                    int numRows = 0;
                    using (RowCursor cursor = cadfeatureClass.Search())
                    {
                        while (cursor.MoveNext())
                            numRows++;
                    }
                }
            });
        }
        #endregion Opening a CAD Datastore


        #region ProSnippet Group: Queries
        #endregion

        // cref: ARCGIS.CORE.DATA.TABLE.SEARCH
        // cref: ARCGIS.CORE.DATA.QUERYFILTER.#CTOR
        // cref: ARCGIS.CORE.DATA.QUERYFILTER.WhereClause
        // cref: ARCGIS.CORE.DATA.QUERYFILTER.SubFields
        // cref: ARCGIS.CORE.DATA.QUERYFILTER.PostfixClause
        // cref: ARCGIS.CORE.DATA.RowCursor
        // cref: ARCGIS.CORE.DATA.RowCursor.MoveNext
        // cref: ARCGIS.CORE.DATA.RowCursor.Current
        #region Searching a Table using QueryFilter
        public async Task SearchingATable()
        {
            try
            {
                await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
                {
                    using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
                    using (Table table = geodatabase.OpenDataset<Table>("EmployeeInfo"))
                    {

                        QueryFilter queryFilter = new QueryFilter
                        {
                            WhereClause = "COSTCTRN = 'Information Technology'",
                            SubFields = "KNOWNAS, OFFICE, LOCATION",
                            PostfixClause = "ORDER BY OFFICE"
                        };

                        using (RowCursor rowCursor = table.Search(queryFilter, false))
                        {
                            while (rowCursor.MoveNext())
                            {
                                using (Row row = rowCursor.Current)
                                {
                                    string location = Convert.ToString(row["LOCATION"]);
                                    string knownAs = Convert.ToString(row["KNOWNAS"]);
                                }
                            }
                        }
                    }
                });
            }
            catch (GeodatabaseFieldException fieldException)
            {
                // One of the fields in the where clause might not exist. There are multiple ways this can be handled:
                // Handle error appropriately
            }
            catch (Exception exception)
            {
                // logger.Error(exception.Message);
            }
        }
        #endregion

        public async Task SearchingATableWithNonLatinCharacters()
        {
            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                // cref: ARCGIS.CORE.DATA.QUERYFILTER.#CTOR
                // cref: ARCGIS.CORE.DATA.SQLSYNTAX.GETSUPPORTEDSTRINGS
                // cref: ARCGIS.CORE.DATA.DATASTORE.GETSQLSYNTAX
                #region Searching a Table for non-Latin characters

                using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
                using (Table table = geodatabase.OpenDataset<Table>("TableWithChineseCharacters"))
                {
                    // This will fail with many database systems that expect Latin characters by default
                    string incorrectWhereClause = "颜色 = '绿'";

                    // Correct solution is to prepend the 'National String Prefix' to the attribute value
                    // For example, with SQL Server this value is 'N'
                    // This value is obtained using the SQLSyntax class
                    string nationalStringPrefix = "";
                    SQLSyntax sqlSyntax = geodatabase.GetSQLSyntax();
                    nationalStringPrefix = sqlSyntax.GetSupportedStrings(SQLStringType.NationalStringPrefix).First();

                    // This Where clause will work
                    QueryFilter queryFilter = new QueryFilter()
                    {
                        WhereClause = "颜色 = " + nationalStringPrefix + "'绿'"
                    };
                }

                #endregion
            });
        }

        // cref: ARCGIS.CORE.DATA.TABLE.SEARCH
        // cref: ARCGIS.CORE.DATA.QUERYFILTER.#CTOR
        // cref: ARCGIS.CORE.DATA.QUERYFILTER.ObjectIDs
        #region Searching a Table using a set of ObjectIDs
        public RowCursor SearchingATable(Table table, IReadOnlyList<long> objectIDs)
        {
            QueryFilter queryFilter = new QueryFilter()
            {
                ObjectIDs = objectIDs
            };

            return table.Search(queryFilter);
        }
        #endregion

        // cref: ARCGIS.CORE.DATA.TABLE.SEARCH
        // cref: ARCGIS.CORE.DATA.SPATIALQUERYFILTER.#CTOR
        // cref: ARCGIS.CORE.DATA.SPATIALQUERYFILTER.FilterGeometry
        // cref: ARCGIS.CORE.DATA.SPATIALQUERYFILTER.SpatialRelationship
        // cref: ARCGIS.CORE.DATA.SpatialRelationship
        #region Searching a FeatureClass using SpatialQueryFilter
        public async Task SearchingAFeatureClass()
        {
            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
                using (FeatureClass schoolBoundaryFeatureClass = geodatabase.OpenDataset<FeatureClass>("LocalGovernment.GDB.SchoolBoundary"))
                {
                    // Using a spatial query filter to find all features which have a certain district name and lying within a given Polygon.
                    SpatialQueryFilter spatialQueryFilter = new SpatialQueryFilter
                    {
                        WhereClause = "DISTRCTNAME = 'Indian Prairie School District 204'",
                        FilterGeometry = new PolygonBuilderEx(new List<Coordinate2D>
                  {
              new Coordinate2D(1021880, 1867396),
              new Coordinate2D(1028223, 1870705),
              new Coordinate2D(1031165, 1866844),
              new Coordinate2D(1025373, 1860501),
              new Coordinate2D(1021788, 1863810)
                  }).ToGeometry(),

                        SpatialRelationship = SpatialRelationship.Within
                    };

                    using (RowCursor indianPrairieCursor = schoolBoundaryFeatureClass.Search(spatialQueryFilter, false))
                    {
                        while (indianPrairieCursor.MoveNext())
                        {
                            using (Feature feature = (Feature)indianPrairieCursor.Current)
                            {
                                // Process the feature. For example...
                                Console.WriteLine(feature.GetObjectID());
                            }
                        }
                    }
                }
            });
        }
        #endregion Searching a FeatureClass using SpatialQueryFilter

        // cref: ARCGIS.CORE.DATA.Selection
        // cref: ARCGIS.CORE.DATA.SelectionType
        // cref: ARCGIS.CORE.DATA.SelectionOption
        // cref: ARCGIS.CORE.DATA.TABLE.SELECT(ARCGIS.CORE.DATA.QUERYFILTER,ARCGIS.CORE.DATA.SELECTIONTYPE,ARCGIS.CORE.DATA.SELECTIONOPTION)
        #region Selecting Rows from a Table
        public async Task SelectingRowsFromATable()
        {
            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
                using (Table enterpriseTable = geodatabase.OpenDataset<Table>("LocalGovernment.GDB.piCIPCost"))
                {
                    QueryFilter anotherQueryFilter = new QueryFilter { WhereClause = "FLOOR = 1 AND WING = 'E'" };

                    // For Selecting all matching entries.
                    using (Selection anotherSelection = enterpriseTable.Select(anotherQueryFilter, SelectionType.ObjectID, SelectionOption.Normal))
                    {
                    }

                    // This can be used to get one record which matches the criteria. No assumptions can be made about which record satisfying the criteria is selected.
                    using (Selection onlyOneSelection = enterpriseTable.Select(anotherQueryFilter, SelectionType.ObjectID, SelectionOption.OnlyOne))
                    {
                    }

                    // This can be used to obtain a empty selction which can be used as a container to combine results from different selections.
                    using (Selection emptySelection = enterpriseTable.Select(anotherQueryFilter, SelectionType.ObjectID, SelectionOption.Empty))
                    {
                    }

                    // If you want to select all the records in a table.
                    using (Selection allRecordSelection = enterpriseTable.Select(null, SelectionType.ObjectID, SelectionOption.Normal))
                    {
                    }
                }
            });
        }
        #endregion Selecting Rows from a Table

        // cref: ARCGIS.CORE.DATA.TABLE.SELECT(ARCGIS.CORE.DATA.QUERYFILTER,ARCGIS.CORE.DATA.SELECTIONTYPE,ARCGIS.CORE.DATA.SELECTIONOPTION)
        // cref: ARCGIS.CORE.DATA.SPATIALQUERYFILTER.FilterGeometry
        // cref: ARCGIS.CORE.DATA.SPATIALQUERYFILTER.SpatialRelationship
        // cref: ARCGIS.CORE.DATA.SpatialRelationship
        // cref: ARCGIS.CORE.DATA.SelectionType
        // cref: ARCGIS.CORE.DATA.SelectionOption
        #region Selecting Features from a FeatureClass
        public async Task SelectingFeaturesFromAFeatureClass()
        {
            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
                using (FeatureClass enterpriseFeatureClass = geodatabase.OpenDataset<FeatureClass>("LocalGovernment.GDB.FacilitySite"))
                {
                    List<Coordinate2D> newCoordinates = new List<Coordinate2D>
                {
            new Coordinate2D(1021570, 1880583),
            new Coordinate2D(1028730, 1880994),
            new Coordinate2D(1029718, 1875644),
            new Coordinate2D(1021405, 1875397)
                };

                    SpatialQueryFilter spatialFilter = new SpatialQueryFilter
                    {
                        WhereClause = "FCODE = 'Park'",
                        FilterGeometry = new PolygonBuilderEx(newCoordinates).ToGeometry(),
                        SpatialRelationship = SpatialRelationship.Crosses
                    };

                    // For Selecting all matching entries.
                    using (Selection anotherSelection = enterpriseFeatureClass.Select(spatialFilter, SelectionType.ObjectID, SelectionOption.Normal))
                    {
                    }

                    // This can be used to get one record which matches the criteria. No assumptions can be made about which record satisfying the 
                    // criteria is selected.
                    using (Selection onlyOneSelection = enterpriseFeatureClass.Select(spatialFilter, SelectionType.ObjectID, SelectionOption.OnlyOne))
                    {
                    }

                    // This can be used to obtain a empty selction which can be used as a container to combine results from different selections.
                    using (Selection emptySelection = enterpriseFeatureClass.Select(spatialFilter, SelectionType.ObjectID, SelectionOption.Empty))
                    {
                    }

                    // If you want to select all the records in a table.
                    using (Selection allRecordSelection = enterpriseFeatureClass.Select(null, SelectionType.ObjectID, SelectionOption.Normal))
                    {
                    }
                }
            });
        }
        #endregion Selecting Features from a FeatureClass

        public void GetCount(FeatureLayer featureLayer)
        {
            QueuedTask.Run(() =>
            {
                // cref: ArcGIS.Core.Data.Table.GetCount
                // cref: ARCGIS.DESKTOP.MAPPING.BASICFEATURELAYER.GETTABLE
                #region Gets the count of how many rows are currently in a Table  
                //Note: call within QueuedTask.Run()
                Table table = featureLayer.GetTable();
                long count = table.GetCount();
                #endregion
            });


            // cref: ArcGIS.Core.Data.Table.GetCount
            // cref: ARCGIS.DESKTOP.MAPPING.FEATURELAYER.GETFEATURECLASS
            #region Gets the feature count of a layer
            FeatureLayer lyr = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();
            QueuedTask.Run(() =>
            {
                FeatureClass featureClass = lyr.GetFeatureClass();
                long nCount = featureClass.GetCount();
            });
            #endregion
        }

        // cref: ArcGIS.Core.Data.TableSortDescription.#ctor
        // cref: ArcGIS.Core.Data.SortDescription.#ctor
        // cref: ArcGIS.Core.Data.FeatureClass.GetDefinition
        // cref: ArcGIS.Core.Data.Table.Sort
        // cref: ArcGIS.Core.Data.FeatureClassDefinition
        // cref: ArcGIS.Core.Data.TableDefinition.GetFields
        // cref: ArcGIS.Core.Data.SortDescription.CaseSensitivity
        // cref: ArcGIS.Core.Data.SortDescription.SortOrder
        // cref: ArcGIS.Core.Data.CaseSensitivity
        // cref: ArcGIS.Core.Data.SortOrder
        #region Sorting a Table
        public RowCursor SortWorldCities(FeatureClass worldCitiesTable)
        {
            using (FeatureClassDefinition featureClassDefinition = worldCitiesTable.GetDefinition())
            {
                Field countryField = featureClassDefinition.GetFields()
                  .First(x => x.Name.Equals("COUNTRY_NAME"));
                Field cityNameField = featureClassDefinition.GetFields()
                  .First(x => x.Name.Equals("CITY_NAME"));

                // Create SortDescription for Country field
                SortDescription countrySortDescription = new SortDescription(countryField);
                countrySortDescription.CaseSensitivity = CaseSensitivity.Insensitive;
                countrySortDescription.SortOrder = SortOrder.Ascending;

                // Create SortDescription for City field
                SortDescription citySortDescription = new SortDescription(cityNameField);
                citySortDescription.CaseSensitivity = CaseSensitivity.Insensitive;
                citySortDescription.SortOrder = SortOrder.Ascending;

                // Create our TableSortDescription
                TableSortDescription tableSortDescription = new TableSortDescription(
                  new List<SortDescription>() { countrySortDescription, citySortDescription });

                return worldCitiesTable.Sort(tableSortDescription);
            }
        }
        #endregion

        // cref: ArcGIS.Core.Data.TableStatisticsDescription.#ctor
        // cref: ArcGIS.Core.Data.TableStatisticsDescription.GroupBy
        // cref: ArcGIS.Core.Data.TableStatisticsDescription.OrderBy
        // cref: ArcGIS.Core.Data.StatisticsDescription.#ctor
        // cref: ArcGIS.Core.Data.Table.CalculateStatistics
        // cref: ArcGIS.Core.Data.StatisticsFunction
        // cref: ArcGIS.Core.Data.TableStatisticsResult
        // cref: ArcGIS.Core.Data.TableStatisticsResult.StatisticsResults
        #region Calculating Statistics on a Table
        // Calculate the Sum and Average of the Population_1990 and Population_2000 fields, grouped and ordered by Region
        public void CalculateStatistics(FeatureClass countryFeatureClass)
        {
            using (FeatureClassDefinition featureClassDefinition = countryFeatureClass.GetDefinition())
            {
                // Get fields
                Field regionField = featureClassDefinition.GetFields()
                  .First(x => x.Name.Equals("Region"));
                Field pop1990Field = featureClassDefinition.GetFields()
                  .First(x => x.Name.Equals("Population_1990"));
                Field pop2000Field = featureClassDefinition.GetFields()
                  .First(x => x.Name.Equals("Population_2000"));

                // Create StatisticsDescriptions
                StatisticsDescription pop1990StatisticsDescription = new StatisticsDescription(pop1990Field,
                          new List<StatisticsFunction>() { StatisticsFunction.Sum,
                                                    StatisticsFunction.Average });

                StatisticsDescription pop2000StatisticsDescription = new StatisticsDescription(pop2000Field,
                          new List<StatisticsFunction>() { StatisticsFunction.Sum,
                                                    StatisticsFunction.Average });

                // Create TableStatisticsDescription
                TableStatisticsDescription tableStatisticsDescription = new TableStatisticsDescription(new List<StatisticsDescription>() {
                      pop1990StatisticsDescription, pop2000StatisticsDescription });
                tableStatisticsDescription.GroupBy = new List<Field>() { regionField };
                tableStatisticsDescription.OrderBy = new List<SortDescription>() { new SortDescription(regionField) };

                // Calculate Statistics
                IReadOnlyList<TableStatisticsResult> tableStatisticsResults = countryFeatureClass.CalculateStatistics(tableStatisticsDescription);

                foreach (TableStatisticsResult tableStatisticsResult in tableStatisticsResults)
                {
                    // Get the Region name
                    // If multiple fields had been passed into TableStatisticsDescription.GroupBy, there would be multiple values in TableStatisticsResult.GroupBy
                    string regionName = tableStatisticsResult.GroupBy.First().Value.ToString();

                    // Get the statistics results for the Population_1990 field
                    StatisticsResult pop1990Statistics = tableStatisticsResult.StatisticsResults[0];
                    double population1990Sum = pop1990Statistics.Sum;
                    double population1990Average = pop1990Statistics.Average;

                    // Get the statistics results for the Population_2000 field
                    StatisticsResult pop2000Statistics = tableStatisticsResult.StatisticsResults[1];
                    double population2000Sum = pop2000Statistics.Sum;
                    double population2000Average = pop2000Statistics.Average;

                    // Do something with the results here...
                }
            }
        }
        #endregion

        // cref: ARCGIS.CORE.DATA.GEODATABASE.EVALUATE
        // cref: ARCGIS.CORE.DATA.QUERYDEF
        // cref: ARCGIS.CORE.DATA.QUERYDEF.Tables
        // cref: ARCGIS.CORE.DATA.QUERYDEF.WhereClause
        #region Evaluating a QueryDef on a single table
        public async Task SimpleQueryDef()
        {
            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file"))))
                {
                    QueryDef adaCompilantParksQueryDef = new QueryDef
                    {
                        Tables = "Park",
                        WhereClause = "ADACOMPLY = 'Yes'",
                    };

                    using (RowCursor rowCursor = geodatabase.Evaluate(adaCompilantParksQueryDef, false))
                    {
                        while (rowCursor.MoveNext())
                        {
                            using (Row row = rowCursor.Current)
                            {
                                Feature feature = row as Feature;
                                Geometry shape = feature.GetShape();

                                String type = Convert.ToString(row["ADACOMPLY"]); // will be "Yes" for each row.

                                try
                                {
                                    Table table = row.GetTable(); // Will always throw exception.
                                }
                                catch (NotSupportedException exception)
                                {
                                    // Handle not supported exception.
                                }
                            }
                        }
                    }
                }
            });
        }
        #endregion Evaluating a QueryDef on a single table

        // cref: ARCGIS.CORE.DATA.GEODATABASE.EVALUATE
        // cref: ARCGIS.CORE.DATA.QUERYDEF
        // cref: ARCGIS.CORE.DATA.QUERYDEF.Tables
        // cref: ARCGIS.CORE.DATA.QUERYDEF.WhereClause
        // cref: ARCGIS.CORE.DATA.QUERYDEF.SubFields
        // cref: ARCGIS.CORE.DATA.Row.GetFields
        #region Evaluating a QueryDef on a Join using WHERE Clause
        public async Task JoiningWithWhereQueryDef()
        {
            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file"))))
                {
                    QueryDef municipalEmergencyFacilitiesQueryDef = new QueryDef
                    {
                        SubFields = "EmergencyFacility.OBJECTID, EmergencyFacility.Shape, EmergencyFacility.FACILITYID, FacilitySite.FACILITYID, FacilitySite.FCODE",
                        Tables = "EmergencyFacility, FacilitySite",
                        WhereClause = "EmergencyFacility.FACNAME = FacilitySite.NAME AND EmergencyFacility.JURISDICT = 'Municipal'",
                    };

                    using (RowCursor rowCursor = geodatabase.Evaluate(municipalEmergencyFacilitiesQueryDef, false))
                    {
                        while (rowCursor.MoveNext())
                        {
                            using (Row row = rowCursor.Current)
                            {
                                Feature feature = row as Feature;
                                Geometry shape = feature.GetShape();

                                long objectID = Convert.ToInt64(row["EmergencyFacility.OBJECTID"]);
                                String featureCode = Convert.ToString(row["FacilitySite.FCODE"]);

                                IReadOnlyList<Field> fields = feature.GetFields(); //Contains one ArcGIS.Core.Data.Field objects for every subfield
                            }
                        }
                    }
                }
            });
        }
        #endregion Evaluating a QueryDef on a Join using WHERE Clause

        // cref: ARCGIS.CORE.DATA.GEODATABASE.EVALUATE
        // cref: ARCGIS.CORE.DATA.QUERYDEF.Tables
        // cref: ARCGIS.CORE.DATA.QUERYDEF.SubFields
        #region Evaluating a QueryDef on a OUTER JOIN
        public async Task EvaluatingQueryDefWithOuterJoin()
        {
            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file"))))
                {
                    QueryDef queryDefWithLeftOuterJoin = new QueryDef
                    {
                        Tables = "CommunityAddress LEFT OUTER JOIN MunicipalBoundary on CommunityAddress.Municipality = MunicipalBoundary.Name",
                        SubFields = "CommunityAddress.OBJECTID, CommunityAddress.Shape, CommunityAddress.SITEADDID, CommunityAddress.ADDRNUM, CommunityAddress.FULLNAME, CommunityAddress.FULLADDR, CommunityAddress.MUNICIPALITY, MunicipalBoundary.Name, MunicipalBoundary.MUNITYP, MunicipalBoundary.LOCALFIPS",
                    };

                    using (RowCursor rowCursor = geodatabase.Evaluate(queryDefWithLeftOuterJoin, false))
                    {
                        while (rowCursor.MoveNext())
                        {
                            using (Row row = rowCursor.Current)
                            {
                                Feature feature = row as Feature;
                                Geometry shape = feature.GetShape();

                                int siteAddressId = Convert.ToInt32(row["CommunityAddress.SITEADDID"]);
                                String stateName = Convert.ToString(row["MunicipalBoundary.name"]);
                            }
                        }
                    }
                }
            });
        }
        #endregion Evaluating a QueryDef on a OUTER JOIN

        // cref: ARCGIS.CORE.DATA.GEODATABASE.EVALUATE
        // cref: ARCGIS.CORE.DATA.QUERYDEF.Tables
        // cref: ARCGIS.CORE.DATA.QUERYDEF.SubFields
        #region Evaluating a QueryDef on a INNER join
        public async Task EvaluatingQueryDefWithInnerJoin()
        {
            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file"))))
                {
                    QueryDef queryDef = new QueryDef()
                    {
                        Tables = "People INNER JOIN States ON People.FK_STATE_ID = States.OBJECTID",
                        SubFields = "People.OBJECTID, People.First_Name, People.Last_Name, People.City, States.State_Name"
                    };

                    using (RowCursor cursor = geodatabase.Evaluate(queryDef))
                    {
                        while (cursor.MoveNext())
                        {
                            using (Row row = cursor.Current)
                            {
                                // Handle row
                            }
                        }
                    }
                }
            });
        }
        #endregion Evaluating a QueryDef on a INNER join

        // cref: ARCGIS.CORE.DATA.GEODATABASE.EVALUATE
        // cref: ARCGIS.CORE.DATA.QUERYDEF.Tables
        // cref: ARCGIS.CORE.DATA.QUERYDEF.SubFields
        #region Evaluating a QueryDef on a nested - INNER  and  OUTER join
        public async Task EvaluatingQueryDefWithNestedJoin()
        {
            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                using (Geodatabase geodatabase = new Geodatabase(
            new DatabaseConnectionFile(new Uri("path\\to\\sde\\file"))))
                {
                    QueryDef queryDef = new QueryDef()
                    {
                        Tables = "((People INNER JOIN States ON People.FK_STATE_ID = States.OBJECTID) LEFT OUTER JOIN Homes ON People.OBJECTID = Homes.FK_People_ID)",
                        SubFields = "People.OBJECTID, People.First_Name, People.Last_Name, States.State_Name, Homes.Address"
                    };

                    using (RowCursor cursor = geodatabase.Evaluate(queryDef, false))
                    {
                        while (cursor.MoveNext())
                        {
                            using (Row row = cursor.Current)
                            {
                                // Handle row
                            }
                        }
                    }
                }
            });
        }
        #endregion Evaluating a QueryDef on nested (INNER & OUTER) join

        // cref: ARCGIS.CORE.DATA.DATABASE.GETQUERYDESCRIPTION(System.String)
        // cref: ARCGIS.CORE.DATA.DATABASECONNECTIONPROPERTIES.AuthenticationMode
        // cref: ARCGIS.CORE.DATA.DATABASECONNECTIONPROPERTIES.Instance
        // cref: ARCGIS.CORE.DATA.DATABASECONNECTIONPROPERTIES.Database
        // cref: ARCGIS.CORE.DATA.DATABASECONNECTIONPROPERTIES.User
        // cref: ARCGIS.CORE.DATA.DATABASECONNECTIONPROPERTIES.Password
        // cref: ARCGIS.CORE.DATA.AuthenticationMode
        // cref: ARCGIS.CORE.DATA.DATABASE.#CTOR(DATABASECONNECTIONPROPERTIES)
        // cref: ARCGIS.CORE.DATA.DATABASE.OpenTable
        // cref: ARCGIS.CORE.DATA.EnterpriseDatabaseType
        // cref: ARCGIS.CORE.DATA.QueryDescription
        #region Create Default QueryDescription for a Database table and obtain the ArcGIS.Core.Data.Table for the QueryDescription
        public async Task DefaultQueryDescription()
        {
            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                DatabaseConnectionProperties databaseConnectionProperties = new DatabaseConnectionProperties(EnterpriseDatabaseType.SQLServer)
                {
                    AuthenticationMode = AuthenticationMode.DBMS,
                    Instance = "instance",
                    Database = "database",
                    User = "user",
                    Password = "password"
                };

                using (Database database = new Database(databaseConnectionProperties))
                {
                    QueryDescription queryDescription = database.GetQueryDescription("CUSTOMERS");

                    using (Table table = database.OpenTable(queryDescription))
                    {
                        //use table
                    }
                }
            });
        }
        #endregion Create Default QueryDescription for a Database table and obtain the ArcGIS.Core.Data.Table for the QueryDescription

        // cref: ARCGIS.CORE.DATA.DATABASE.GETQUERYDESCRIPTION(System.String,System.String)
        // cref: ARCGIS.CORE.DATA.EnterpriseDatabaseType
        // cref: ARCGIS.CORE.DATA.QueryDescription
        // cref: ARCGIS.CORE.DATA.DATABASE.OpenTable
        #region Create QueryDescription from a custom query for a Database table
        public async Task CustomQueryDescription()
        {
            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                DatabaseConnectionProperties databaseConnectionProperties = new DatabaseConnectionProperties(EnterpriseDatabaseType.SQLServer)
                {
                    AuthenticationMode = AuthenticationMode.DBMS,
                    Instance = "instance",
                    Database = "database",
                    User = "user",
                    Password = "password"
                };

                using (Database database = new Database(databaseConnectionProperties))
                {
                    QueryDescription queryDescription = database.GetQueryDescription("SELECT OBJECTID, Shape, FACILITYID FROM EmergencyFacility WHERE JURISDICT = 'Municipal'", "MunicipalEmergencyFacilities");

                    using (Table table = database.OpenTable(queryDescription))
                    {
                        // Use the table.
                    }
                }
            });
        }
        #endregion Create QueryDescription from a custom query for a Database table

        // cref: ARCGIS.CORE.DATA.DATABASE.GETQUERYDESCRIPTION(System.String,System.String)
        // cref: ARCGIS.CORE.DATA.QueryDescription.SetObjectIDFields
        #region Create QueryDescription from a join query where there is no non-nullable unique id column
        public async Task JoinQueryDescription()
        {
            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                DatabaseConnectionProperties databaseConnectionProperties = new DatabaseConnectionProperties(EnterpriseDatabaseType.SQLServer)
                {
                    AuthenticationMode = AuthenticationMode.DBMS,
                    Instance = "instance",
                    Database = "database",
                    User = "user",
                    Password = "password"
                };

                using (Database database = new Database(databaseConnectionProperties))
                {
                    QueryDescription queryDescription = database.GetQueryDescription("SELECT BUSLINES.ID as BUSLINESID, BUSSTOPS.ID as BUSSTOPSID, BUSLINES.RTE_DESC, BUSLINES.DIR, BUSSTOPS.JURISDIC, BUSSTOPS.LOCATION, BUSSTOPS.ROUTE,BUSSTOPS.SHAPE from demosql.dbo.BUSSTOPS JOIN demosql.dbo.BUSLINES ON BUSSTOPS.ROUTE = BUSLINES.ROUTE", "BusInfo");

                    queryDescription.SetObjectIDFields("BUSLINESID,BUSSTOPSID");

                    using (Table table = database.OpenTable(queryDescription))
                    {
                        // Use the table.
                    }
                }
            });
        }
        #endregion Create QueryDescription from a join query where there is no non-nullable unique id column

        // cref: ARCGIS.CORE.DATA.DATABASE.GETQUERYDESCRIPTION(System.String,System.String)
        // cref: ARCGIS.CORE.DATA.QUERYDESCRIPTION.SETSHAPETYPE
        #region Create QueryDescription from a query for a Database table which has more than one shape type
        public async Task MultiGeometryQueryDescription()
        {
            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                DatabaseConnectionProperties databaseConnectionProperties = new DatabaseConnectionProperties(EnterpriseDatabaseType.SQLServer)
                {
                    AuthenticationMode = AuthenticationMode.DBMS,
                    Instance = "instance",
                    Database = "database",
                    User = "user",
                    Password = "password"
                };

                using (Database database = new Database(databaseConnectionProperties))
                {
                    QueryDescription pointQueryDescription = database.GetQueryDescription("select Description, SHAPE, UniqueID from MULTIGEOMETRYTEST", "MultiGeometryPoint");
                    pointQueryDescription.SetShapeType(GeometryType.Point);
                    using (Table pointTable = database.OpenTable(pointQueryDescription))
                    {
                        //use pointTable
                    }

                    QueryDescription polygonQueryDescription = database.GetQueryDescription("select Description, SHAPE, UniqueID from MULTIGEOMETRYTEST", "MultiGeometryPolygon");
                    polygonQueryDescription.SetShapeType(GeometryType.Polygon);
                    using (Table polygonTable = database.OpenTable(polygonQueryDescription))
                    {
                        //use polygonTable
                    }
                }
            });
        }
        #endregion Create QueryDescription from a query for a Database table which has more than one shape type

        // cref: ARCGIS.CORE.DATA.SQLITECONNECTIONPATH.#CTOR
        // cref: ARCGIS.CORE.DATA.DATABASE.GETQUERYDESCRIPTION(System.String,System.String)
        // cref: ARCGIS.CORE.DATA.DATABASE.OPENTABLE
        #region Create QueryDescription from a query for an SQLite Database table
        public async Task SqliteQueryDescription()
        {
            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                using (Database database = new Database(new SQLiteConnectionPath(new Uri("Path\\To\\Sqlite\\Database\\USA.sqlite"))))
                {
                    QueryDescription washingtonCitiesQueryDescription = database.GetQueryDescription("select OBJECTID, Shape, CITY_FIPS, CITY_NAME, STATE_FIPS, STATE_CITY, TYPE, CAPITAL from main.cities where STATE_NAME='Washington'", "WashingtonCities");

                    using (Table washingtonTable = database.OpenTable(washingtonCitiesQueryDescription))
                    {
                        // Use washingtonTable.
                    }
                }
            });
        }
        #endregion Create QueryDescription from a query for a SQLite Database table

        // cref: ARCGIS.CORE.DATA.SQLSYNTAX.GETFUNCTIONNAME
        // cref: ARCGIS.CORE.DATA.DATASTORE.GETSQLSYNTAX
        #region Using SQLSyntax to form platform agnostic queries
        public async Task UsingSqlSyntaxToFormPlatformAgnosticQueries()
        {
            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                using (Geodatabase geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri("C:\\Data\\LocalGovernment.gdb"))))
                using (FeatureClass featureClass = geodatabase.OpenDataset<FeatureClass>("FacilitySite"))
                {
                    SQLSyntax sqlSyntax = geodatabase.GetSQLSyntax();
                    string substringFunctionName = sqlSyntax.GetFunctionName(SQLFunction.Substring);
                    string upperFunctionName = sqlSyntax.GetFunctionName(SQLFunction.Upper);
                    string substringfunction = string.Format("{0}({1}(FCODE, 1, 6)) = 'SCHOOL'", upperFunctionName, substringFunctionName);

                    QueryFilter queryFilter = new QueryFilter
                    {
                        WhereClause = substringfunction
                    };
                    using (Selection selection = featureClass.Select(queryFilter, SelectionType.ObjectID, SelectionOption.Normal))
                    {
                        // work with the selection.
                    }
                }
            });
        }
        #endregion Using SQLSyntax to form platform agnostic queries

        // cref: ARCGIS.CORE.DATA.VIRTUALRELATIONSHIPCLASSDESCRIPTION
        // cref: ARCGIS.CORE.DATA.JOIN.GETJOINEDTABLE
        // cref: ARCGIS.CORE.DATA.Table.RelateTo
        // cref: ARCGIS.CORE.DATA.JOIN.#CTOR(ARCGIS.CORE.DATA.JOINDESCRIPTION)
        // cref: ARCGIS.CORE.DATA.JOIN.GetJoinedTable
        // cref: ARCGIS.CORE.DATA.RelationshipCardinality
        // cref: ARCGIS.CORE.DATA.RelationshipClass
        // cref: ARCGIS.CORE.DATA.JoinDescription
        // cref: ARCGIS.CORE.DATA.JoinDescription.JoinDirection
        // cref: ARCGIS.CORE.DATA.JoinDescription.JoinType
        // cref: ARCGIS.CORE.DATA.JoinDirection
        // cref: ARCGIS.CORE.DATA.JoinType
        #region Joining a file geodatabase feature class to an Oracle database query layer feature class with a virtual relationship class
        public async Task JoiningFileGeodatabaseFeatureClassToOracleQueryLayer()
        {
            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                using (Geodatabase geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri("C:\\Data\\LocalGovernment.gdb"))))
                using (Database database = new Database(new DatabaseConnectionProperties(EnterpriseDatabaseType.Oracle)
                {
                    AuthenticationMode = AuthenticationMode.DBMS,
                    Instance = "instance",
                    User = "user",
                    Password = "password",
                    Database = "database"
                }))

                using (FeatureClass leftFeatureClass = geodatabase.OpenDataset<FeatureClass>("Hospital"))
                using (Table rightTable = database.OpenTable(database.GetQueryDescription("FacilitySite")))
                {
                    Field originPrimaryKey = leftFeatureClass.GetDefinition().GetFields().FirstOrDefault(field => field.Name.Equals("facilityId"));
                    Field destinationForeignKey = rightTable.GetDefinition().GetFields().FirstOrDefault(field => field.Name.Equals("hospitalID"));

                    VirtualRelationshipClassDescription description = new VirtualRelationshipClassDescription(
                originPrimaryKey, destinationForeignKey, RelationshipCardinality.OneToOne);

                    using (RelationshipClass relationshipClass = leftFeatureClass.RelateTo(rightTable, description))
                    {
                        JoinDescription joinDescription = new JoinDescription(relationshipClass)
                        {
                            JoinDirection = JoinDirection.Forward,
                            JoinType = JoinType.LeftOuterJoin
                        };

                        Join join = new Join(joinDescription);

                        using (Table joinedTable = join.GetJoinedTable())
                        {
                            // Perform operation on joined table.
                        }
                    }
                }
            });
        }
        #endregion Joining a file geodatabase feature class to an Oracle database query layer feature class with a virtual relationship class

        // cref: ARCGIS.CORE.DATA.VIRTUALRELATIONSHIPCLASSDESCRIPTION
        // cref: ARCGIS.CORE.DATA.JOIN.GETJOINEDTABLE
        // cref: ARCGIS.CORE.DATA.JOIN.#CTOR(ARCGIS.CORE.DATA.JOINDESCRIPTION)
        // cref: ARCGIS.CORE.DATA.JoinDescription
        // cref: ARCGIS.CORE.DATA.JoinDescription.JoinDirection
        // cref: ARCGIS.CORE.DATA.JoinDescription.JoinType
        // cref: ARCGIS.CORE.DATA.JoinDescription.TargetFields
        // cref: ARCGIS.CORE.DATA.JoinDirection
        // cref: ARCGIS.CORE.DATA.JoinType
        #region Joining two tables from different geodatabases
        public async Task JoinTablesFromDifferentGeodatabases()
        {
            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                using (Geodatabase sourceGeodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri("Path \\ to \\Geodatabase \\ one"))))
                using (Geodatabase destinationGeodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri("Path \\ to \\Geodatabase \\ two"))))
                using (Table sourceTable = sourceGeodatabase.OpenDataset<Table>("State"))
                using (Table destinationTable = destinationGeodatabase.OpenDataset<Table>("Cities"))
                {
                    Field primaryKeyField = sourceTable.GetDefinition().GetFields().FirstOrDefault(field => field.Name.Equals("State.State_Abbreviation"));
                    Field foreignKeyField = destinationTable.GetDefinition().GetFields().FirstOrDefault(field => field.Name.Equals("Cities.State"));

                    VirtualRelationshipClassDescription virtualRelationshipClassDescription = new VirtualRelationshipClassDescription(primaryKeyField, foreignKeyField, RelationshipCardinality.OneToMany);

                    using (RelationshipClass relationshipClass = sourceTable.RelateTo(destinationTable, virtualRelationshipClassDescription))
                    {
                        JoinDescription joinDescription = new JoinDescription(relationshipClass)
                        {
                            JoinDirection = JoinDirection.Forward,
                            JoinType = JoinType.InnerJoin,
                            TargetFields = sourceTable.GetDefinition().GetFields()
                        };

                        using (Join join = new Join(joinDescription))
                        {
                            Table joinedTable = join.GetJoinedTable();

                            //Process the joined table. For example ..
                            using (RowCursor cursor = joinedTable.Search())
                            {
                                while (cursor.MoveNext())
                                {
                                    using (Row row = cursor.Current)
                                    {
                                        // Use Row
                                    }
                                }
                            }
                        }
                    }
                }
            });
        }
        #endregion Joining two tables from different geodatabases

        // cref: ARCGIS.CORE.DATA.QUERYTABLEDESCRIPTION
        // cref: ARCGIS.CORE.DATA.QUERYTABLEDESCRIPTION.Name
        // cref: ARCGIS.CORE.DATA.QUERYTABLEDESCRIPTION.PrimaryKeys
        // cref: ARCGIS.CORE.DATA.GEODATABASE.OPENQUERYTABLE
        // cref: ARCGIS.CORE.DATA.QueryDef
        // cref: ARCGIS.CORE.DATA.QueryDef.Tables
        // cref: ARCGIS.CORE.DATA.QueryDef.SubFields
        // cref: ARCGIS.CORE.DATA.SQLSyntax.QualifyColumnName
        #region Creating a QueryTable using a query which joins two versioned tables in a geodatabase
        public async Task QueryTableJoinWithVersionedData()
        {
            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                QueryDef queryDef = new QueryDef
                {
                    Tables = "CommunityAddress JOIN MunicipalBoundary on CommunityAddress.Municipality = MunicipalBoundary.Name",
                    SubFields = "CommunityAddress.OBJECTID, CommunityAddress.Shape, CommunityAddress.SITEADDID, CommunityAddress.ADDRNUM, CommunityAddress.FULLNAME, CommunityAddress.FULLADDR, CommunityAddress.MUNICIPALITY, MunicipalBoundary.Name, MunicipalBoundary.MUNITYP, MunicipalBoundary.LOCALFIPS",
                };

                using (Geodatabase testVersion1Geodatabase = new Geodatabase(new DatabaseConnectionProperties(EnterpriseDatabaseType.Oracle)
                {
                    AuthenticationMode = AuthenticationMode.DBMS,
                    Instance = "instance",
                    User = "user",
                    Password = "password",
                    Database = "database",
                    Version = "user.testVersion1"
                }))
                {
                    QueryTableDescription queryTableDescription = new QueryTableDescription(queryDef)
                    {
                        Name = "CommunityAddrJounMunicipalBoundr",
                        PrimaryKeys = testVersion1Geodatabase.GetSQLSyntax().QualifyColumnName("CommunityAddress", "OBJECTID")
                    };

                    // Will be based on testVersion1.
                    using (Table queryTable = testVersion1Geodatabase.OpenQueryTable(queryTableDescription))
                    {
                        // Use queryTable.
                    }
                }

                using (Geodatabase testVersion2Geodatabase = new Geodatabase(new DatabaseConnectionProperties(EnterpriseDatabaseType.Oracle)
                {
                    AuthenticationMode = AuthenticationMode.DBMS,
                    Instance = "instance",
                    User = "user",
                    Password = "password",
                    Database = "database",
                    Version = "user.testVersion2"
                }))
                {
                    QueryTableDescription queryTableDescription = new QueryTableDescription(queryDef)
                    {
                        Name = "CommunityAddrJounMunicipalBoundr",
                        PrimaryKeys = testVersion2Geodatabase.GetSQLSyntax().QualifyColumnName("CommunityAddress", "OBJECTID")
                    };

                    // Will be based on testVersion2.
                    using (Table queryTable = testVersion2Geodatabase.OpenQueryTable(queryTableDescription))
                    {
                        // Use queryTable.
                    }
                }
            });
        }
        #endregion Creating a QueryTable using a query which joins two versioned tables in a geodatabase

        public void CheckColumnForNull(Row row, Field field)
        {
            // cref: ARCGIS.CORE.DATA.ROW.ITEM(System.String)
            #region Checking a field value for null

            object val = row[field.Name];
            if (val is DBNull || val == null)
            {
                // field value is null
            }
            else
            {
                // field value is not null
            }

            #endregion Checking a field value for null
        }

        // cref: ARCGIS.CORE.DATA.FIELD.GETDOMAIN
        // cref: ARCGIS.CORE.DATA.TABLEDEFINITION.GETSUBTYPES
        // cref: ARCGIS.CORE.DATA.Subtype
        // cref: ARCGIS.CORE.DATA.CodedValueDomain
        #region Get domain string from a field
        public string GetDomainStringFromField(Row row, Field field)
        {
            // Get the table and table definition from the Row
            using (Table table = row.GetTable())
            using (TableDefinition tableDefinition = table.GetDefinition())
            {
                // Get name of subtype field
                string subtypeFieldName = tableDefinition.GetSubtypeField();

                // Get subtype, if any
                Subtype subtype = null;

                if (subtypeFieldName.Length != 0)
                {
                    // Get value of subtype field for this row
                    object varSubtypeCode = row[subtypeFieldName];
                    long subtypeCode = (long)varSubtypeCode;

                    // Get subtype for this row
                    subtype = tableDefinition.GetSubtypes().First(x => x.GetCode() == subtypeCode);
                }

                // Get the coded value domain for this field
                CodedValueDomain domain = field.GetDomain(subtype) as CodedValueDomain;

                // Return the text string for this field
                if (domain != null)
                {
                    return domain.GetName(row[field.Name]);
                }
                else
                {
                    return row[field.Name].ToString();
                }
            }
        }
        #endregion Get domain string from a field

        // cref: ArcGIS.Core.Data.DatastoreProperties
        // cref: ArcGIS.Core.Data.DatastoreProperties.SupportsBigInteger
        // cref: ArcGIS.Core.Data.DatastoreProperties.SupportsQueryPagination
        // cref: ArcGIS.Core.Data.DatastoreProperties.CanEdit
        // cref: ArcGIS.Core.Data.DatastoreProperties.SupportsBigObjectID
        // cref: ArcGIS.Core.Data.DatastoreProperties.SupportsDateOnly
        // cref: ArcGIS.Core.Data.Datastore.GetDatastoreProperties
        // cref: ArcGIS.Core.Data.Datastore.AreDatastorePropertiesSupported
        #region Get datastore or workspace properties

        public void GetDatastoreProperties(Datastore geodatabase)
        {
            // Check if a data store supports datastore properties
            bool areDatastorePropertiesSupported = geodatabase.AreDatastorePropertiesSupported();

            if (areDatastorePropertiesSupported)
            {
                DatastoreProperties datastoreProperties = geodatabase.GetDatastoreProperties();

                // Supports 64-bit integer field
                bool supportsBigInteger = datastoreProperties.SupportsBigInteger;

                // Supports pagination
                bool supportsQueryPagination = datastoreProperties.SupportsQueryPagination;

                // Supports datastore edit 
                bool canEdit = datastoreProperties.CanEdit;

                // Supports 64-bit Object ID
                bool supportsBigObjectId = datastoreProperties.SupportsBigObjectID;

                // Supports DateOnly field
                bool supportsDateOnly = datastoreProperties.SupportsDateOnly;

                // Supports TimeOnly field
                bool supportsTimeOnly = datastoreProperties.SupportsTimeOnly;

                // Supports TimestampOffset field
                bool supportsTimestampOffset = datastoreProperties.SupportsTimestampOffset;
            }
        }

        #endregion

        // cref: ARCGIS.CORE.DATA.QUERYFILTER.#CTOR
        // cref: ArcGIS.Core.Data.QueryFilter.Offset
        // cref: ArcGIS.Core.Data.QueryFilter.RowCount
        #region Pagination in QueryFilter

        public void QueryFilterWithPagination(Table table, List<long> objectIDs)
        {
            int rowsPerBatch = 100;
            int offset = 0;

            // Query filter
            // Some datastores support pagination only through an SQL postfix clause
            QueryFilter queryFilter = new QueryFilter()
            {
                ObjectIDs = objectIDs,
                PostfixClause = "ORDER BY OBJECTID"
            };

            // Fetch rows in a batch from a table
            for (int index = offset; index <= objectIDs.Count; index += rowsPerBatch)
            {
                // Set number of rows to return from a table
                queryFilter.RowCount = rowsPerBatch;

                // Set positional offset to skip number of rows from a table 
                queryFilter.Offset = index;

                using (RowCursor cursor = table.Search(queryFilter))
                {
                    while (cursor.MoveNext())
                    {
                        using (Row row = cursor.Current)
                        {
                            Console.WriteLine(row.GetObjectID());
                        }
                    }
                }
            }
        }

        #endregion

        // cref: ArcGIS.Core.Data.Conflict
        // cref: ArcGIS.Core.Data.Version.Reconcile(ArcGIS.Core.Data.ReconcileOptions,ArcGIS.Core.Data.PostOptions)
        // cref: ArcGIS.Core.Data.ConflictResolutionMethod
        // cref: ArcGIS.Core.Data.ConflictDetectionType
        // cref: ArcGIS.Core.Data.ConflictResolutionType
        // cref: ArcGIS.Core.Data.ReconcileResult
        // cref: ArcGIS.Core.Data.ReconcileResult.HasConflicts
        // cref: ArcGIS.Core.Data.Version.GetConflicts
        // cref: ArcGIS.Core.Data.PostOptions.ServiceSynchronizationType
        // cref: ArcGIS.Core.Data.Version.Reconcile(ArcGIS.Core.Data.ReconcileOptions)
        // cref: ArcGIS.Core.Data.Version.HasConflicts
        #region Illustrate version conflict information from a reconcile operation

        public void GetVersionConflictsInfoInUpdateDeleteType(ServiceConnectionProperties featureServiceConnectionProperties, string featureClassName)
        {
            // To illustrate the conflict between versions,
            // the feature is updated in the child version and deleted in the parent version.

            long featureObjectIDForEdit = Int64.MinValue;

            // Get branch versioned service
            using (Geodatabase fsGeodatabase = new Geodatabase(featureServiceConnectionProperties))
            using (VersionManager versionManager = fsGeodatabase.GetVersionManager())
            using (Version defaultVersion = versionManager.GetDefaultVersion())
            using (Geodatabase defaultGeodatabase = defaultVersion.Connect())
            using (FeatureClass defaultFeatureClass = defaultGeodatabase.OpenDataset<FeatureClass>(featureClassName))
            using (FeatureClassDefinition defaultFeatureClassDefinition = defaultFeatureClass.GetDefinition())
            {
                // Create a feature in the default version to edit in a branch
                defaultGeodatabase.ApplyEdits(() =>
                {
                    using (RowBuffer rowBuffer = defaultFeatureClass.CreateRowBuffer())
                    {
                        rowBuffer["NAME"] = "Loblolly Pine";
                        rowBuffer["TREEAGE"] = 1;
                        rowBuffer[defaultFeatureClassDefinition.GetShapeField()] = new MapPointBuilderEx(new Coordinate2D(1, 1),
                  SpatialReferenceBuilder.CreateSpatialReference(4152, 0)).ToGeometry();

                        using (Feature feature = defaultFeatureClass.CreateRow(rowBuffer))
                        {
                            featureObjectIDForEdit = feature.GetObjectID();
                        }
                    }
                });

                // Add newly created feature in the filter
                QueryFilter queryFilter = new QueryFilter { ObjectIDs = new List<long> { featureObjectIDForEdit } };

                // Create a branch version
                VersionDescription versionDescription = new VersionDescription("UpdateDeleteConflictType",
                  "Update-Delete version conflict type", VersionAccessType.Private);

                // Edit the feature in the branch 
                using (Version editVersion = versionManager.CreateVersion(versionDescription))
                using (Geodatabase branchGeodatabase = editVersion.Connect())
                using (FeatureClass featureClass = branchGeodatabase.OpenDataset<FeatureClass>(featureClassName))
                using (RowCursor rowCursor = featureClass.Search(queryFilter, false))
                {
                    branchGeodatabase.ApplyEdits(() =>
                    {
                        while (rowCursor.MoveNext())
                        {
                            using (Row row = rowCursor.Current)
                            {
                                row["TREEAGE"] = 100;
                                row["NAME"] = $"{row["Name"]}_EditInBranch";
                                row.Store();
                            }
                        }
                    });

                    // Delete the feature from the default version
                    defaultFeatureClass.DeleteRows(queryFilter);

                    // Reconcile options
                    ReconcileOptions reconcileOptions = new ReconcileOptions(defaultVersion)
                    {
                        ConflictResolutionType = ConflictResolutionType.FavorEditVersion,
                        ConflictDetectionType = ConflictDetectionType.ByRow,
                        ConflictResolutionMethod = ConflictResolutionMethod.Continue
                    };

                    // Reconcile with default
                    ReconcileResult reconcileResult = editVersion.Reconcile(reconcileOptions);

                    // Check for conflicts
                    bool hasConflictsReconcileResults = reconcileResult.HasConflicts;
                    bool hasConflictsAfterReconcile = editVersion.HasConflicts();

                    // Fetch conflicts
                    IReadOnlyList<Conflict> conflictsAfterReconcile = editVersion.GetConflicts();

                    // Iterate conflicts
                    foreach (Conflict conflict in conflictsAfterReconcile)
                    {
                        // Object ID of row where conflict occurs
                        long objectId = conflict.ObjectID;

                        ConflictType conflictType = conflict.ConflictType;

                        IReadOnlyList<FieldValue> ancestorVersionValues = conflict.AncestorVersionValues;
                        object nameAncestor = ancestorVersionValues.FirstOrDefault(f => f.FieldName.Contains("NAME")).Value;
                        object treeAgeAncestor = ancestorVersionValues.FirstOrDefault(f => f.FieldName.Contains("TREEAGE")).Value;

                        IReadOnlyList<FieldValue> childVersionValues = conflict.ChildVersionValues;
                        object nameChild = childVersionValues.FirstOrDefault(f => f.FieldName.Contains("NAME")).Value;
                        object treeAgeChild = childVersionValues.FirstOrDefault(f => f.FieldName.Contains("TREEAGE")).Value;

                        IReadOnlyList<FieldValue> parentVersionValues = conflict.ParentVersionValues;

                        IReadOnlyList<Field> originalFields = defaultFeatureClassDefinition.GetFields();

                        string datasetName = conflict.DatasetName;
                    }
                }
            }
        }

        #endregion


        // cref: ArcGIS.Core.Data.ContingentCodedValue
        // cref: ArcGIS.Core.Data.ContingentAnyValue
        // cref: ArcGIS.Core.Data.ContingentNullValue
        // cref: ArcGIS.Core.Data.ContingentRangeValue
        // cref: ArcGIS.Core.Data.TableDefinition.GetContingencies
        #region Explore cotingent attribute values

        public void ExploreContingentValues(Table table)
        {
            using (TableDefinition tableDefinition = table.GetDefinition())
            {
                IReadOnlyList<Contingency> contingencies = tableDefinition.GetContingencies();
                foreach (Contingency contingency in contingencies)
                {
                    // Field group 
                    FieldGroup filedGroup = contingency.FieldGroup;
                    string fieldGroupName = filedGroup.Name;
                    IReadOnlyList<string> fieldInFieldGroup = filedGroup.FieldNames;
                    bool isEditRestriction = filedGroup.IsRestrictive;

                    int contingencyId = contingency.ID;
                    Subtype subtype = contingency.Subtype;
                    bool isContingencyRetired = contingency.IsRetired;

                    // Contingent values 
                    IReadOnlyDictionary<string, ContingentValue> contingentValuesByFieldName = contingency.GetContingentValues();
                    foreach (KeyValuePair<string, ContingentValue> contingentValueKeyValuePair in contingentValuesByFieldName)
                    {
                        string attributeFieldName = contingentValueKeyValuePair.Key;

                        // Contingent value type associated with the attribute field
                        ContingentValue contingentValue = contingentValueKeyValuePair.Value;

                        switch (contingentValue)
                        {
                            case ContingentCodedValue contingentCodedValue:
                                string codedValueDomainName = contingentCodedValue.Name;
                                object codedValueDomainValue = contingentCodedValue.CodedValue;
                                break;
                            case ContingentRangeValue contingentRangeValue:
                                object rangeDomainMaxValue = contingentRangeValue.Max;
                                object rangeDomainMinValue = contingentRangeValue.Min;
                                break;
                            case ContingentAnyValue contingentAnyValue:
                                // Any value type
                                break;
                            case ContingentNullValue contingentNullValue:
                                // Null value
                                break;
                        }
                    }
                }
            }
        }
        #endregion

        // cref: ArcGIS.Core.Data.ContingencyValidationResult.Matches
        // cref: ArcGIS.Core.Data.ContingencyValidationResult.Violations
        // cref: ArcGIS.Core.Data.Table.ValidateContingencies(ArcGIS.Core.Data.RowBuffer)
        #region Validate contingent attribute values

        public void ValidateContingentValues(FeatureClass parcels, string zoningFieldName = "Zone", string taxCodeFieldName = "TaxCode")
        {
            using (RowBuffer rowBuffer = parcels.CreateRowBuffer())
            {
                // Insert values in a row buffer
                rowBuffer[zoningFieldName] = "Business";
                rowBuffer[taxCodeFieldName] = "TaxB";

                // Validate contingency values of the parcels' row 
                ContingencyValidationResult contingencyValidationResult = parcels.ValidateContingencies(rowBuffer);

                // Valid contingencies
                IReadOnlyList<Contingency> matchedContingencies = contingencyValidationResult.Matches;
                if (matchedContingencies.Count > 0)
                {
                    // Create a row with valid contingency values
                    parcels.CreateRow(rowBuffer);
                }

                // Invalid contingencies
                IReadOnlyList<ContingencyViolation> violatedContingencies = contingencyValidationResult.Violations;
                foreach (ContingencyViolation contingencyViolation in violatedContingencies)
                {
                    ContingencyViolationType violationType = contingencyViolation.Type;
                    Contingency violatedContingency = contingencyViolation.Contingency;
                }
            }
        }


        #endregion

        // cref: ArcGIS.Core.Data.ContingentCodedValue
        // cref: ArcGIS.Core.Data.ContingentAnyValue
        // cref: ArcGIS.Core.Data.ContingentNullValue
        // cref: ArcGIS.Core.Data.ContingentRangeValue
        // cref: ArcGIS.Core.Data.Table.GetContingentValues(ArcGIS.Core.Data.RowBuffer,System.String)
        // cref: ArcGIS.Core.Data.Contingency.GetContingentValues
        #region Get possible contingent values

        public void GetPossibleContingentValues(FeatureClass parcels, string zoningFieldName = "Zone")
        {
            using (RowBuffer rowBuffer = parcels.CreateRowBuffer())
            {
                IReadOnlyDictionary<FieldGroup, IReadOnlyList<ContingentValue>> possibleZonings = parcels.GetContingentValues(rowBuffer, zoningFieldName);
                IEnumerable<FieldGroup> possibleFieldGroups = possibleZonings.Keys;
                foreach (FieldGroup possibleFieldGroup in possibleFieldGroups)
                {
                    IReadOnlyList<ContingentValue> possibleZoningValues = possibleZonings[possibleFieldGroup];
                    foreach (ContingentValue possibleZoningValue in possibleZoningValues)
                    {
                        switch (possibleZoningValue)
                        {
                            case ContingentCodedValue codedValue:
                                string codedValueDomainName = codedValue.Name;
                                object codedValueDomainValue = codedValue.CodedValue;
                                break;
                            case ContingentRangeValue rangeValue:
                                object rangeDomainMaxValue = rangeValue.Max;
                                object rangeDomainMinValue = rangeValue.Min;
                                break;
                            case ContingentAnyValue contingentAnyValue:
                                // Any value type
                                break;
                            case ContingentNullValue contingentNullValue:
                                // Null value
                                break;
                        }
                    }
                }
            }
        }

        #endregion

        #region ProSnippet Group: Editing
        #endregion

        // cref: ARCGIS.CORE.DATA.TABLE.CREATEROWBUFFER
        // cref: ARCGIS.CORE.DATA.TABLE.CREATEROW
        // cref: ARCGIS.Desktop.Editing.EditOperation.Callback
        // cref: ARCGIS.DESKTOP.EDITING.EDITOPERATION.IEDITCONTEXT.INVALIDATE
        // cref: ARCGIS.CORE.DATA.RowBuffer
        #region Creating a Row
        public async Task CreatingARow()
        {
            string message = String.Empty;
            bool creationResult = false;
            EditOperation editOperation = new EditOperation();

            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {

                using (Geodatabase geodatabase = new Geodatabase(
            new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
                using (Table enterpriseTable = geodatabase.OpenDataset<Table>("LocalGovernment.GDB.piCIPCost"))
                {

                    //var geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(uri)) for a File GDB
                    //
                    //var shapeFileConnPath = new FileSystemConnectionPath(uri, FileSystemDatastoreType.Shapefile);
                    //var shapefile = new FileSystemDatastore(shapeFileConnPath);
                    //var table = shapefile.OpenDataset<Table>(strShapeFileName); for a Shape file

                    //declare the callback here. We are not executing it .yet.
                    editOperation.Callback(context =>
              {
                  TableDefinition tableDefinition = enterpriseTable.GetDefinition();
                  int assetNameIndex = tableDefinition.FindField("ASSETNA");

                  using (RowBuffer rowBuffer = enterpriseTable.CreateRowBuffer())
                  {
                      // Either the field index or the field name can be used in the indexer.
                      rowBuffer[assetNameIndex] = "wMain";
                      rowBuffer["COST"] = 700;
                      rowBuffer["ACTION"] = "Open Cut";

                      // subtype value for "Abandon".
                      rowBuffer[tableDefinition.GetSubtypeField()] = 3;

                      using (Row row = enterpriseTable.CreateRow(rowBuffer))
                      {
                          // To Indicate that the attribute table has to be updated.
                          context.Invalidate(row);
                      }
                  }
              }, enterpriseTable);

                    try
                    {
                        creationResult = editOperation.Execute();
                        if (!creationResult) message = editOperation.ErrorMessage;
                    }
                    catch (GeodatabaseException exObj)
                    {
                        message = exObj.Message;
                    }
                }
            });

            if (!string.IsNullOrEmpty(message))
                MessageBox.Show(message);

        }
        #endregion Creating a Row

        // cref: ARCGIS.CORE.DATA.TABLE.CREATEROWBUFFER
        // cref: ARCGIS.CORE.DATA.FEATURECLASS.CREATEROW
        // cref: ARCGIS.CORE.DATA.RowBuffer
        #region Creating a Feature
        public async Task CreatingAFeature()
        {
            string message = String.Empty;
            bool creationResult = false;

            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
                using (FeatureClass enterpriseFeatureClass = geodatabase.OpenDataset<FeatureClass>("LocalGovernment.GDB.FacilitySite"))
                {
                    //var geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(uri)) for a File GDB
                    //
                    //var shapeFileConnPath = new FileSystemConnectionPath(uri, FileSystemDatastoreType.Shapefile);
                    //var shapefile = new FileSystemDatastore(shapeFileConnPath);
                    //var table = shapefile.OpenDataset<Table>(strShapeFileName); for a Shape file

                    //declare the callback here. We are not executing it yet
                    EditOperation editOperation = new EditOperation();
                    editOperation.Callback(context =>
              {
                  FeatureClassDefinition facilitySiteDefinition = enterpriseFeatureClass.GetDefinition();
                  int facilityIdIndex = facilitySiteDefinition.FindField("FACILITYID");

                  using (RowBuffer rowBuffer = enterpriseFeatureClass.CreateRowBuffer())
                  {
                      // Either the field index or the field name can be used in the indexer.
                      rowBuffer[facilityIdIndex] = "wMain";
                      rowBuffer["NAME"] = "Griffith Park";
                      rowBuffer["OWNTYPE"] = "Municipal";
                      rowBuffer["FCODE"] = "Park";
                      // Add it to Public Attractions Subtype.
                      rowBuffer[facilitySiteDefinition.GetSubtypeField()] = 820;

                      List<Coordinate2D> newCoordinates = new List<Coordinate2D>
                    {
                  new Coordinate2D(1021570, 1880583),
                  new Coordinate2D(1028730, 1880994),
                  new Coordinate2D(1029718, 1875644),
                  new Coordinate2D(1021405, 1875397)
                      };

                      rowBuffer[facilitySiteDefinition.GetShapeField()] = new PolygonBuilderEx(newCoordinates).ToGeometry();

                      using (Feature feature = enterpriseFeatureClass.CreateRow(rowBuffer))
                      {
                          //To Indicate that the attribute table has to be updated
                          context.Invalidate(feature);
                      }
                  }

              }, enterpriseFeatureClass);

                    try
                    {
                        creationResult = editOperation.Execute();
                        if (!creationResult) message = editOperation.ErrorMessage;
                    }
                    catch (GeodatabaseException exObj)
                    {
                        message = exObj.Message;
                    }
                }
            });

            if (!string.IsNullOrEmpty(message))
                MessageBox.Show(message);
        }
        #endregion Creating a Feature

        // cref: ARCGIS.DESKTOP.EDITING.EDITOPERATION.IEDITCONTEXT.INVALIDATE(ARCGIS.CORE.DATA.ROW)
        // cref: ArcGIS.Core.Data.Row.Store
        // cref: ARCGIS.DESKTOP.EDITING.EDITOPERATION.IEDITCONTEXT.INVALIDATE(Row)
        #region Modifying a Row
        public async Task ModifyingARow()
        {
            string message = String.Empty;
            bool modificationResult = false;

            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
                using (Table enterpriseTable = geodatabase.OpenDataset<Table>("LocalGovernment.GDB.piCIPCost"))
                {
                    //var geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(uri)) for a File GDB
                    //
                    //var shapeFileConnPath = new FileSystemConnectionPath(uri, FileSystemDatastoreType.Shapefile);
                    //var shapefile = new FileSystemDatastore(shapeFileConnPath);
                    //var table = shapefile.OpenDataset<Table>(strShapeFileName); for a Shape file

                    EditOperation editOperation = new EditOperation();
                    editOperation.Callback(context =>
              {
                  QueryFilter openCutFilter = new QueryFilter { WhereClause = "ACTION = 'Open Cut'" };

                  using (RowCursor rowCursor = enterpriseTable.Search(openCutFilter, false))
                  {
                      TableDefinition tableDefinition = enterpriseTable.GetDefinition();
                      int subtypeFieldIndex = tableDefinition.FindField(tableDefinition.GetSubtypeField());

                      while (rowCursor.MoveNext())
                      {
                          using (Row row = rowCursor.Current)
                          {
                              // In order to update the Map and/or the attribute table.
                              // Has to be called before any changes are made to the row.
                              context.Invalidate(row);

                              row["ASSETNA"] = "wMainOpenCut";

                              if (Convert.ToDouble(row["COST"]) > 700)
                              {
                                  // Abandon asset if cost is higher than 700 (if that is what you want to do).
                                  row["ACTION"] = "Open Cut Abandon";
                                  row[subtypeFieldIndex] = 3; //subtype value for "Abandon"   
                              }

                              //After all the changes are done, persist it.
                              row.Store();

                              // Has to be called after the store too.
                              context.Invalidate(row);
                          }
                      }
                  }
              }, enterpriseTable);

                    try
                    {
                        modificationResult = editOperation.Execute();
                        if (!modificationResult) message = editOperation.ErrorMessage;
                    }
                    catch (GeodatabaseException exObj)
                    {
                        message = exObj.Message;
                    }
                }
            });

            if (!string.IsNullOrEmpty(message))
                MessageBox.Show(message);
        }
        #endregion Modifying a Row

        // cref: ARCGIS.DESKTOP.EDITING.EDITOPERATION.IEDITCONTEXT.INVALIDATE(ARCGIS.CORE.DATA.ROW)
        // cref: ARCGIS.CORE.DATA.ROW.STORE
        // cref: ARCGIS.CORE.DATA.FEATURE.SETSHAPE
        #region Modifying a Feature
        public async Task ModifyingAFeature()
        {
            string message = String.Empty;
            bool modificationResult = false;

            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
                using (FeatureClass enterpriseFeatureClass = geodatabase.OpenDataset<FeatureClass>("LocalGovernment.GDB.FacilitySite"))
                {

                    //var geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(uri)) for a File GDB
                    //
                    //var shapeFileConnPath = new FileSystemConnectionPath(uri, FileSystemDatastoreType.Shapefile);
                    //var shapefile = new FileSystemDatastore(shapeFileConnPath);
                    //var table = shapefile.OpenDataset<Table>(strShapeFileName); for a Shape file

                    FeatureClassDefinition facilitySiteDefinition = enterpriseFeatureClass.GetDefinition();

                    int ownTypeIndex = facilitySiteDefinition.FindField("OWNTYPE");
                    int areaIndex = facilitySiteDefinition.FindField(facilitySiteDefinition.GetAreaField());

                    EditOperation editOperation = new EditOperation();
                    editOperation.Callback(context =>
              {
                  QueryFilter queryFilter = new QueryFilter { WhereClause = "FCODE = 'Hazardous Materials Facility' AND OWNTYPE = 'Private'" };

                  using (RowCursor rowCursor = enterpriseFeatureClass.Search(queryFilter, false))
                  {
                      while (rowCursor.MoveNext())
                      {
                          using (Feature feature = (Feature)rowCursor.Current)
                          {
                              // In order to update the Map and/or the attribute table.
                              // Has to be called before any changes are made to the row
                              context.Invalidate(feature);

                              // Transfer all Hazardous Material Facilities to the City.
                              feature[ownTypeIndex] = "Municipal";

                              if (Convert.ToDouble(feature[areaIndex]) > 50000)
                              {
                                  // Set the Shape of the feature to whatever you need.
                                  List<Coordinate2D> newCoordinates = new List<Coordinate2D>
                          {
                      new Coordinate2D(1021570, 1880583),
                      new Coordinate2D(1028730, 1880994),
                      new Coordinate2D(1029718, 1875644),
                      new Coordinate2D(1021405, 1875397)
                          };

                                  feature.SetShape(new PolygonBuilderEx(newCoordinates).ToGeometry());
                              }

                              feature.Store();

                              // Has to be called after the store too
                              context.Invalidate(feature);
                          }
                      }
                  }
              }, enterpriseFeatureClass);

                    try
                    {
                        modificationResult = editOperation.Execute();
                        if (!modificationResult) message = editOperation.ErrorMessage;
                    }
                    catch (GeodatabaseException exObj)
                    {
                        message = exObj.Message;
                    }
                }
            });

            if (!string.IsNullOrEmpty(message))
                MessageBox.Show(message);
        }
        #endregion Modifying a Feature

        public void WritingIntoGuidColumn(Row row, Field field, Guid guid)
        {
            // cref: ARCGIS.CORE.DATA.ROW.FINDFIELD
            #region Writing a value into a Guid column
            row[field.Name] = "{" + guid.ToString() + "}";
            #endregion Writing a value into a Guid column
        }

        // cref: ARCGIS.DESKTOP.EDITING.EDITOPERATION.IEDITCONTEXT.INVALIDATE(ARCGIS.CORE.DATA.ROW)
        // cref: ARCGIS.CORE.DATA.ROW.DELETE
        #region Deleting a Row/Feature
        public async Task DeletingARowOrFeature()
        {
            string message = String.Empty;
            bool deletionResult = false;

            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
                using (Table enterpriseTable = geodatabase.OpenDataset<Table>("LocalGovernment.GDB.piCIPCost"))
                {
                    //var geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(uri)) for a File GDB
                    //
                    //var shapeFileConnPath = new FileSystemConnectionPath(uri, FileSystemDatastoreType.Shapefile);
                    //var shapefile = new FileSystemDatastore(shapeFileConnPath);
                    //var table = shapefile.OpenDataset<Table>(strShapeFileName); for a Shape file

                    EditOperation editOperation = new EditOperation();
                    editOperation.Callback(context =>
              {
                  QueryFilter openCutFilter = new QueryFilter { WhereClause = "ACTION = 'Open Cut'" };

                  using (RowCursor rowCursor = enterpriseTable.Search(openCutFilter, false))
                  {
                      while (rowCursor.MoveNext())
                      {
                          using (Row row = rowCursor.Current)
                          {
                              // In order to update the Map and/or the attribute table. Has to be called before the delete.
                              context.Invalidate(row);

                              row.Delete();
                          }
                      }
                  }
              }, enterpriseTable);

                    try
                    {
                        deletionResult = editOperation.Execute();
                        if (!deletionResult) message = editOperation.ErrorMessage;
                    }
                    catch (GeodatabaseException exObj)
                    {
                        message = exObj.Message;
                    }
                }
            });

            if (!string.IsNullOrEmpty(message))
                MessageBox.Show(message);
        }
        #endregion Deleting a Row/Feature

        // cref: ArcGIS.Core.Data.Feature.Split(ArcGIS.Core.Geometry.Geometry)
        #region Split a feature by geometry

        public void SplitALineByPoint(FeatureClass lineFeatureClass, MapPoint xPoint)
        {
            using (RowCursor rowCursor = lineFeatureClass.Search(new QueryFilter() { ObjectIDs = new List<long>() { 1 } }))
            {
                if (rowCursor.MoveNext())
                {
                    using (Feature feature = rowCursor.Current as Feature)
                    {
                        // ObjectIDs of newly created lines
                        IReadOnlyList<long> splits = feature.Split(xPoint);
                    }
                }
            }
        }

        #endregion

        // cref: ARCGIS.CORE.DATA.ROW.ADDATTACHMENT
        // cref: ARCGIS.CORE.DATA.Attachment
        #region Adding Attachments
        public async Task AddingAttachments()
        {
            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
                using (FeatureClass parkFeatureClass = geodatabase.OpenDataset<FeatureClass>("LocalGovernment.GDB.Park"))
                {
                    QueryFilter filter = new QueryFilter { WhereClause = "NUMPARKING > 0" };

                    using (RowCursor parkingCursor = parkFeatureClass.Search(filter, false))
                    {
                        while (parkingCursor.MoveNext())
                        {
                            using (MemoryStream stream = CreateMemoryStreamFromContentsOf("Sample.xml"))
                            {
                                Attachment attachment = new Attachment("Sample.xml", "text/xml", stream);

                                using (Row row = parkingCursor.Current)
                                {
                                    long attachmentId = row.AddAttachment(attachment);
                                }
                            }
                        }
                    }
                }
            });
        }

        private MemoryStream CreateMemoryStreamFromContentsOf(String fileNameWithPath)
        {
            MemoryStream memoryStream = new MemoryStream();

            using (FileStream file = new FileStream(fileNameWithPath, FileMode.Open, FileAccess.Read))
            {
                byte[] bytes = new byte[file.Length];
                file.Read(bytes, 0, (int)file.Length);
                memoryStream.Write(bytes, 0, (int)file.Length);
            }

            return memoryStream;
        }
        #endregion Adding Attachments

        // cref: ARCGIS.CORE.DATA.ROW.UPDATEATTACHMENT
        // cref: ARCGIS.CORE.DATA.ROW.GETATTACHMENTS
        // cref: ARCGIS.CORE.DATA.Attachment
        // cref: ARCGIS.CORE.DATA.Attachment.SetName
        #region Updating Attachments
        public async Task UpdatingAttachments()
        {
            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
                using (FeatureClass landUseCaseFeatureClass = geodatabase.OpenDataset<FeatureClass>("LocalGovernment.GDB.LandUseCase"))
                {
                    QueryFilter filter = new QueryFilter { WhereClause = "CASETYPE = 'Rezoning'" };

                    using (RowCursor landUseCursor = landUseCaseFeatureClass.Search(filter, false))
                    {
                        while (landUseCursor.MoveNext())
                        {
                            using (Feature rezoningUseCase = (Feature)landUseCursor.Current)
                            {
                                IReadOnlyList<Attachment> rezoningAttachments = rezoningUseCase.GetAttachments();
                                IEnumerable<Attachment> filteredAttachments = rezoningAttachments.Where(attachment => !attachment.GetName().Contains("rezoning"));

                                foreach (Attachment attachmentToUpdate in filteredAttachments)
                                {
                                    attachmentToUpdate.SetName(attachmentToUpdate.GetName().Replace(".pdf", "Rezoning.pdf"));
                                    rezoningUseCase.UpdateAttachment(attachmentToUpdate);
                                }
                            }
                        }
                    }
                }
            });
        }
        #endregion Updating Attachments

        // cref: ARCGIS.CORE.DATA.ROW.DELETEATTACHMENTS
        // cref: ARCGIS.CORE.DATA.ROW.GETATTACHMENTS
        // cref: ARCGIS.CORE.DATA.ATTACHMENT.GETATTACHMENTID
        // cref: ARCGIS.CORE.DATA.Attachment.GetContentType
        #region Deleting Attachments
        public async Task DeletingAttachments()
        {
            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
                using (Table inspectionTable = geodatabase.OpenDataset<Table>("luCodeInspection"))
                {
                    QueryFilter queryFilter = new QueryFilter { WhereClause = "ACTION = '1st Notice'" };

                    using (RowCursor cursor = inspectionTable.Search(queryFilter, false))
                    {
                        while (cursor.MoveNext())
                        {
                            using (Row currentRow = cursor.Current)
                            {
                                IReadOnlyList<Attachment> rowAttachments = currentRow.GetAttachments(null, true);
                                IEnumerable<Attachment> attachments = rowAttachments.Where(attachment => attachment.GetContentType().Equals("application/pdf"));

                                IReadOnlyList<long> attachmentIDs = attachments.Select(attachment => attachment.GetAttachmentID()) as IReadOnlyList<long>;
                                IReadOnlyDictionary<long, Exception> failures = currentRow.DeleteAttachments(attachmentIDs);

                                if (failures.Count > 0)
                                {
                                    //process errors
                                }
                            }
                        }
                    }
                }
            });
        }
        #endregion Deleting Attachments

        // cref: ARCGIS.CORE.DATA.TABLE.CREATEROWBUFFER
        // cref: ARCGIS.CORE.DATA.TABLE.CREATEROW
        #region Writing a Blob field
        public async Task WriteBlobField(Table table, string blobFieldName, string imageFileName)
        {
            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                // Read the image file into a MemoryStream
                MemoryStream memoryStream = new MemoryStream(); ;
                using (FileStream imageFile = new FileStream(imageFileName, FileMode.Open, FileAccess.Read))
                {
                    imageFile.CopyTo(memoryStream);
                }

                // Create a new row in the table, and write the Memory Stream into a blob fiele
                using (RowBuffer rowBuffer = table.CreateRowBuffer())
                {
                    rowBuffer[blobFieldName] = memoryStream;
                    table.CreateRow(rowBuffer).Dispose();
                }
            });
        }
        #endregion

        // cref: ARCGIS.CORE.DATA.Feature
        // cref: ARCGIS.CORE.DATA.Row
        #region Reading a Blob field
        public async Task ReadBlobField(Table table, QueryFilter queryFilter, string blobFieldName)
        {
            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                const string imageFileBaseName = "C:\\path\\to\\image\\directory\\Image";

                // for each row that satisfies the search criteria, write the blob field out to an image file
                using (RowCursor rowCursor = table.Search(queryFilter))
                {
                    int fileCount = 0;
                    while (rowCursor.MoveNext())
                    {
                        using (Row row = rowCursor.Current)
                        {
                            // Read the blob field into a MemoryStream
                            MemoryStream memoryStream = row[blobFieldName] as MemoryStream;

                            // Create a file
                            using (FileStream outputFile = new FileStream(imageFileBaseName + fileCount.ToString(), FileMode.Create, FileAccess.Write))
                            {
                                // Write the MemoryStream into the file
                                memoryStream.WriteTo(outputFile);
                            }
                        }
                    }
                }
            });
        }
        #endregion

        // cref: ARCGIS.CORE.DATA.RELATIONSHIPCLASS.GETROWSRELATEDTODESTINATIONROWS
        // cref: ARCGIS.CORE.DATA.RELATIONSHIPCLASS.GETROWSRELATEDTOORIGINROWS
        #region Getting Rows related by RelationshipClass
        public async Task GettingRowsRelatedByRelationshipClass()
        {
            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file"))))
                using (RelationshipClass relationshipClass = geodatabase.OpenDataset<RelationshipClass>("LocalGovernment.GDB.luCodeViolationHasInspections"))
                using (FeatureClass violationsFeatureClass = geodatabase.OpenDataset<FeatureClass>("LocalGovernment.GDB.luCodeViolation"))
                using (Table inspectionTable = geodatabase.OpenDataset<Table>("LocalGovernment.GDB.luCodeInspection"))
                {
                    List<Row> jeffersonAveViolations = new List<Row>();
                    QueryFilter queryFilter = new QueryFilter { WhereClause = "LOCDESC LIKE '///%Jefferson///%'" };

                    using (RowCursor rowCursor = violationsFeatureClass.Search(queryFilter, false))
                    {
                        while (rowCursor.MoveNext())
                        {
                            jeffersonAveViolations.Add(rowCursor.Current);
                        }
                    }

                    IReadOnlyList<Row> relatedOriginRows = null;
                    IReadOnlyList<Row> relatedDestinationRows = null;

                    try
                    {
                        QueryFilter filter = new QueryFilter { WhereClause = "ACTION = '1st Notice'" };

                        using (Selection selection = inspectionTable.Select(filter, SelectionType.ObjectID, SelectionOption.Normal))
                        {
                            relatedOriginRows = relationshipClass.GetRowsRelatedToDestinationRows(selection.GetObjectIDs());
                        }

                        bool containsJeffersonAve = relatedOriginRows.Any(row => Convert.ToString(row["LOCDESC"]).Contains("Jefferson"));

                        List<long> jeffersonAveViolationObjectIds = jeffersonAveViolations.Select(row => row.GetObjectID()).ToList();

                        relatedDestinationRows = relationshipClass.GetRowsRelatedToOriginRows(jeffersonAveViolationObjectIds);
                        bool hasFirstNoticeInspections = relatedDestinationRows.Any(row => Convert.ToString(row["ACTION"]).Contains("1st Notice"));
                    }
                    finally
                    {
                        Dispose(jeffersonAveViolations);
                        Dispose(relatedOriginRows);
                        Dispose(relatedDestinationRows);
                    }
                }
            });
        }

        private static void Dispose(IEnumerable<Row> rows)
        {
            foreach (Row row in rows)
                row.Dispose();
        }
        #endregion Getting Rows related by RelationshipClass

        // cref: ARCGIS.CORE.DATA.RELATIONSHIPCLASS.CREATERELATIONSHIP
        // cref: ARCGIS.CORE.DATA.RELATIONSHIPCLASSDEFINITION.GETORIGINKEYFIELD
        // cref: ARCGIS.CORE.DATA.RELATIONSHIP
        #region Creating a Relationship
        public async Task CreatingARelationship()
        {
            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file"))))
                using (RelationshipClass relationshipClass = geodatabase.OpenDataset<RelationshipClass>("LocalGovernment.GDB.OverviewToProject"))
                using (FeatureClass projectsFeatureClass = geodatabase.OpenDataset<FeatureClass>("LocalGovernment.GDB.CIPProjects"))
                using (FeatureClass overviewFeatureClass = geodatabase.OpenDataset<FeatureClass>("LocalGovernment.GDB.CIPProjectsOverview"))
                {
                    // This will be PROJNAME. This can be used to get the field index or used directly as the field name.
                    string originKeyField = relationshipClass.GetDefinition().GetOriginKeyField();

                    EditOperation editOperation = new EditOperation();
                    editOperation.Callback(context =>
              {
                  // The rows are being added to illustrate adding relationships. If one has existing rows, those can be used to add a relationship.
                  using (RowBuffer projectsRowBuffer = projectsFeatureClass.CreateRowBuffer())
                  using (RowBuffer overviewRowBuffer = overviewFeatureClass.CreateRowBuffer())
                  {
                      projectsRowBuffer["TOTCOST"] = 500000;

                      overviewRowBuffer[originKeyField] = "LibraryConstruction";
                      overviewRowBuffer["PROJECTMAN"] = "John Doe";
                      overviewRowBuffer["FUNDSOUR"] = "Public";

                      using (Row projectsRow = projectsFeatureClass.CreateRow(projectsRowBuffer))
                      using (Row overviewRow = overviewFeatureClass.CreateRow(overviewRowBuffer))
                      {
                          Relationship relationship = relationshipClass.CreateRelationship(overviewRow, projectsRow);

                          //To Indicate that the Map has to draw this feature/row and/or the attribute table has to be updated
                          context.Invalidate(projectsRow);
                          context.Invalidate(overviewRow);
                          context.Invalidate(relationshipClass);
                      }
                  }
              }, projectsFeatureClass, overviewFeatureClass);

                    bool editResult = editOperation.Execute();
                }
            });
        }
        #endregion Creating a Relationship

        // cref: ARCGIS.CORE.DATA.RELATIONSHIPCLASS.DELETERELATIONSHIP
        // cref: ARCGIS.CORE.DATA.RELATIONSHIPCLASS.GETROWSRELATEDTOORIGINROWS
        #region Deleting a Relationship
        public async Task DeletingARelationship()
        {
            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file"))))
                using (RelationshipClass relationshipClass = geodatabase.OpenDataset<RelationshipClass>("LocalGovernment.GDB.luCodeViolationHasInspections"))
                using (FeatureClass violationsFeatureClass = geodatabase.OpenDataset<FeatureClass>("LocalGovernment.GDB.luCodeViolation"))
                {
                    QueryFilter queryFilter = new QueryFilter { WhereClause = "LOCDESC LIKE '///%Jefferson///%'" };

                    using (RowCursor rowCursor = violationsFeatureClass.Search(queryFilter, false))
                    {
                        if (!rowCursor.MoveNext())
                            return;

                        using (Row jeffersonAveViolation = rowCursor.Current)
                        {
                            IReadOnlyList<Row> relatedDestinationRows = relationshipClass.GetRowsRelatedToOriginRows(new List<long> { jeffersonAveViolation.GetObjectID() });

                            try
                            {
                                EditOperation editOperation = new EditOperation();
                                editOperation.Callback(context =>
                          {
                              foreach (Row relatedDestinationRow in relatedDestinationRows)
                              {
                                  try
                                  {
                                      relationshipClass.DeleteRelationship(jeffersonAveViolation, relatedDestinationRow);
                                  }
                                  catch (GeodatabaseRelationshipClassException exception)
                                  {
                                      Console.WriteLine(exception);
                                  }
                              }
                          }, relationshipClass);

                                bool editResult = editOperation.Execute();
                            }
                            finally
                            {
                                foreach (Row row in relatedDestinationRows)
                                    row.Dispose();
                            }
                        }
                    }
                }
            });
        }
        #endregion Deleting a Relationship

        // cref: ArcGIS.Core.Data.Table.CreateInsertCursor
        // cref: ArcGIS.Core.Data.InsertCursor.Insert
        // cref: ArcGIS.Core.Data.InsertCursor.Flush
        #region Using an Insert Cursor
        // Insert Cursors are intended for use in CoreHost applications, not Pro Add-ins
        public void UsingInsertCursor()
        {
            using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file"))))
            using (Table citiesTable = geodatabase.OpenDataset<Table>("name\\of\\cities_table"))
            {
                geodatabase.ApplyEdits(() =>
                {
                    using (InsertCursor insertCursor = citiesTable.CreateInsertCursor())
                    using (RowBuffer rowBuffer = citiesTable.CreateRowBuffer())
                    {
                        rowBuffer["State"] = "Colorado";

                        rowBuffer["Name"] = "Fort Collins";
                        rowBuffer["Population"] = 167830;
                        insertCursor.Insert(rowBuffer);

                        rowBuffer["Name"] = "Denver";
                        rowBuffer["Population"] = 727211;
                        insertCursor.Insert(rowBuffer);

                        // Insert more rows here
                        // A more realistic example would be reading source data from a file

                        insertCursor.Flush();
                    }
                });
            }
        }
        #endregion

        // cref: ArcGIS.Core.Data.Mapping.AnnotationFeatureClass.CreateRow(ArcGIS.Core.Data.RowBuffer)
        // cref: ARCGIS.CORE.DATA.TABLE.CREATEROWBUFFER
        // cref: ArcGIS.Core.Data.Mapping.AnnotationFeature.SetAnnotationClassID
        // cref: ArcGIS.Core.Data.Mapping.AnnotationFeature.SetStatus
        // cref: ArcGIS.Core.Data.Mapping.AnnotationFeature.SetGraphic
        // cref: ArcGIS.Core.Data.AnnotationStatus
        // cref: ArcGIS.Core.Data.Mapping.AnnotationFeatureClass.GetDefinition
        // cref: ArcGIS.Core.Data.Mapping.AnnotationFeatureClassDefinition
        // cref: ArcGIS.Core.Data.Mapping.AnnotationFeatureClassDefinition.GetLabelClassCollection
        // cref: ArcGIS.Core.CIM.CIMLabelClass
        // cref: ArcGIS.Core.CIM.CIMLabelClass.TextSymbol
        #region Creating a new Annotation Feature in an Annotation FeatureClass using a RowBuffer
        public async Task CreatingAnAnnotationFeature(Geodatabase geodatabase)
        {
            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                using (AnnotationFeatureClass annotationFeatureClass = geodatabase.OpenDataset<AnnotationFeatureClass>("Annotation // feature // class // name"))
                using (AnnotationFeatureClassDefinition annotationFeatureClassDefinition = annotationFeatureClass.GetDefinition())
                using (RowBuffer rowBuffer = annotationFeatureClass.CreateRowBuffer())
                using (AnnotationFeature annotationFeature = annotationFeatureClass.CreateRow(rowBuffer))
                {
                    annotationFeature.SetAnnotationClassID(0);
                    annotationFeature.SetStatus(AnnotationStatus.Placed);

                    // Get the annotation labels from the label collection
                    IReadOnlyList<CIMLabelClass> labelClasses =
                annotationFeatureClassDefinition.GetLabelClassCollection();

                    // Setup the symbol reference with the symbol id and the text symbol
                    CIMSymbolReference cimSymbolReference = new CIMSymbolReference();
                    cimSymbolReference.Symbol = labelClasses[0].TextSymbol.Symbol;
                    cimSymbolReference.SymbolName = labelClasses[0].TextSymbol.SymbolName;

                    // Setup the text graphic
                    CIMTextGraphic cimTextGraphic = new CIMTextGraphic();
                    cimTextGraphic.Text = "Charlotte, North Carolina";
                    cimTextGraphic.Shape = new MapPointBuilderEx(new Coordinate2D(-80.843, 35.234), SpatialReferences.WGS84).ToGeometry();
                    cimTextGraphic.Symbol = cimSymbolReference;

                    // Set the symbol reference on the graphic and store
                    annotationFeature.SetGraphic(cimTextGraphic);
                    annotationFeature.Store();
                }
            });
        }
        #endregion Creating a new Annotation Feature in an Annotation FeatureClass using a RowBuffer

        #region ProSnippet Group: Versioning
        #endregion

        // cref: ARCGIS.CORE.DATA.VERSIONMANAGER.GETVERSION
        // cref: ARCGIS.CORE.DATA.GEODATABASE.GETVERSIONMANAGER
        // cref: ARCGIS.CORE.DATA.GEODATABASE.IsVersioningSupported
        // cref: ARCGIS.CORE.DATA.VERSION.CONNECT
        #region Connecting to a Version
        public Geodatabase ConnectToVersion(Geodatabase geodatabase, string versionName)
        {
            Geodatabase connectedVersion = null;

            if (geodatabase.IsVersioningSupported())
            {
                using (VersionManager versionManager = geodatabase.GetVersionManager())
                using (Version version = versionManager.GetVersion(versionName))
                {
                    connectedVersion = version.Connect();
                }
            }
            return connectedVersion;
        }
        #endregion

        // cref: ARCGIS.CORE.DATA.GEODATABASE.IsVersioningSupported
        // cref: ARCGIS.CORE.DATA.VERSIONMANAGER.GetCurrentVersion
        // cref: ArcGIS.Core.Data.Version.Reconcile(ArcGIS.Core.Data.ReconcileOptions)
        // cref: ArcGIS.Core.Data.Version.Post
        // cref: ArcGIS.Core.Data.Version.GetParent
        // cref: ArcGIS.Core.Data.ReconcileOptions
        // cref: ArcGIS.Core.Data.ReconcileOptions.ConflictResolutionMethod
        // cref: ArcGIS.Core.Data.ReconcileOptions.ConflictDetectionType
        // cref: ArcGIS.Core.Data.ReconcileOptions.ConflictResolutionType
        // cref: ArcGIS.Core.Data.ConflictResolutionMethod
        // cref: ArcGIS.Core.Data.ConflictDetectionType
        // cref: ArcGIS.Core.Data.ConflictResolutionType
        // cref: ArcGIS.Core.Data.ReconcileResult
        // cref: ArcGIS.Core.Data.ReconcileResult.HasConflicts
        // cref: ArcGIS.Core.Data.PostOptions
        // cref: ArcGIS.Core.Data.PostOptions.ServiceSynchronizationType
        // cref: ArcGIS.Core.Data.ServiceSynchronizationType
        #region Reconciling and Posting a Version with its Parent in separate edit sessions
        public void ReconcileAndPost(Geodatabase geodatabase)
        {
            // Get a reference to our version and our parent
            if (geodatabase.IsVersioningSupported())
            {
                using (VersionManager versionManager = geodatabase.GetVersionManager())
                using (Version currentVersion = versionManager.GetCurrentVersion())
                using (Version parentVersion = currentVersion.GetParent())
                {

                    //// Create a ReconcileDescription object
                    //At 2.x - 
                    //ReconcileDescription reconcileDescription = new ReconcileDescription(parentVersion);
                    //reconcileDescription.ConflictResolutionMethod = ConflictResolutionMethod.Continue; // continue if conflicts are found
                    //reconcileDescription.WithPost = true;

                    //// Reconcile and post
                    //ReconcileResult reconcileResult = currentVersion.Reconcile(reconcileDescription);

                    // ReconcileResult.HasConflicts can be checked as-needed

                    // Create a ReconcileOptions object
                    ReconcileOptions reconcileOptions = new ReconcileOptions(parentVersion);
                    reconcileOptions.ConflictResolutionMethod = ConflictResolutionMethod.Continue; // continue if conflicts are found
                    reconcileOptions.ConflictDetectionType = ConflictDetectionType.ByRow; //Default
                    reconcileOptions.ConflictResolutionType = ConflictResolutionType.FavorTargetVersion;//or FavorEditVersion

                    // Reconcile
                    ReconcileResult reconcileResult = currentVersion.Reconcile(reconcileOptions);
                    if (!reconcileResult.HasConflicts)
                    {
                        //No conflicts, perform the post
                        PostOptions postOptions = new PostOptions(parentVersion);
                        //var postOptions = new PostOptions(); for default version
                        postOptions.ServiceSynchronizationType = ServiceSynchronizationType.Synchronous;//Default
                        currentVersion.Post(postOptions);
                    }

                }
            }
        }
        #endregion

        // cref: ARCGIS.CORE.DATA.GEODATABASE.IsVersioningSupported
        // cref: ARCGIS.CORE.DATA.VERSIONMANAGER.GetCurrentVersion
        // cref: ArcGIS.Core.Data.Version.Reconcile(ArcGIS.Core.Data.ReconcileOptions,ArcGIS.Core.Data.PostOptions)
        // cref: ArcGIS.Core.Data.Version.Post
        // cref: ArcGIS.Core.Data.Version.GetParent
        // cref: ArcGIS.Core.Data.ReconcileOptions
        // cref: ArcGIS.Core.Data.ReconcileOptions.ConflictResolutionMethod
        // cref: ArcGIS.Core.Data.ReconcileOptions.ConflictDetectionType
        // cref: ArcGIS.Core.Data.ReconcileOptions.ConflictResolutionType
        // cref: ArcGIS.Core.Data.ConflictResolutionMethod
        // cref: ArcGIS.Core.Data.ConflictDetectionType
        // cref: ArcGIS.Core.Data.ConflictResolutionType
        // cref: ArcGIS.Core.Data.ReconcileResult
        // cref: ArcGIS.Core.Data.ReconcileResult.HasConflicts
        // cref: ArcGIS.Core.Data.PostOptions
        // cref: ArcGIS.Core.Data.PostOptions.ServiceSynchronizationType
        // cref: ArcGIS.Core.Data.ServiceSynchronizationType
        #region Reconciling and Posting a Version with its Parent in the same edit session
        public void ReconcileAndPost2(Geodatabase geodatabase)
        {
            // Get a reference to our version and our parent
            if (geodatabase.IsVersioningSupported())
            {
                using (VersionManager versionManager = geodatabase.GetVersionManager())
                using (Version currentVersion = versionManager.GetCurrentVersion())
                using (Version parentVersion = currentVersion.GetParent())
                {

                    //// Create a ReconcileDescription object
                    //At 2.x - 
                    //ReconcileDescription reconcileDescription = new ReconcileDescription(parentVersion);
                    //reconcileDescription.ConflictResolutionMethod = ConflictResolutionMethod.Continue; // continue if conflicts are found
                    //reconcileDescription.WithPost = true;

                    //// Reconcile and post
                    //ReconcileResult reconcileResult = currentVersion.Reconcile(reconcileDescription);

                    // ReconcileResult.HasConflicts can be checked as-needed

                    // Create a ReconcileOptions object
                    ReconcileOptions reconcileOptions = new ReconcileOptions(parentVersion);
                    reconcileOptions.ConflictResolutionMethod = ConflictResolutionMethod.Continue; // continue if conflicts are found
                    reconcileOptions.ConflictDetectionType = ConflictDetectionType.ByRow; //Default
                    reconcileOptions.ConflictResolutionType = ConflictResolutionType.FavorTargetVersion;//or FavorEditVersion

                    PostOptions postOptions = new PostOptions(parentVersion);
                    //var postOptions = new PostOptions(); for default version
                    postOptions.ServiceSynchronizationType = ServiceSynchronizationType.Synchronous;//Default

                    // Reconcile
                    ReconcileResult reconcileResult = currentVersion.Reconcile(reconcileOptions, postOptions);
                    if (reconcileResult.HasConflicts)
                    {
                        //TODO resolve conflicts

                    }

                }
            }
        }
        #endregion

        // cref: ARCGIS.CORE.DATA.VERSIONMANAGER.GETVERSIONNAMES
        // cref: ARCGIS.CORE.DATA.GEODATABASE.GETVERSIONMANAGER
        // cref: ARCGIS.CORE.DATA.VERSION.GetParent
        // cref: ARCGIS.CORE.DATA.VERSION.Connect
        // cref: ARCGIS.CORE.DATA.VERSION.GetAccessType
        // cref: ARCGIS.CORE.DATA.VERSION.GetName
        // cref: ARCGIS.CORE.DATA.VERSION.GetChildren
        // cref: ARCGIS.CORE.DATA.VersionAccessType
        #region Working with Versions
        public async Task WorkingWithVersions()
        {
            await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
            {
                using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file"))))
                using (VersionManager versionManager = geodatabase.GetVersionManager())
                {
                    IReadOnlyList<string> versionNames = versionManager.GetVersionNames();

                    Version defaultVersion = versionManager.GetDefaultVersion();

                    string testVersionName = versionNames.First(v => v.Contains("Test"));
                    Version testVersion= versionManager.GetVersion(testVersionName);

                    Version qaVersion = defaultVersion.GetChildren().First(version => version.GetName().Contains("QA"));

                    Geodatabase qaVersionGeodatabase = qaVersion.Connect();

                    FeatureClass currentFeatureClass = geodatabase.OpenDataset<FeatureClass>("featureClassName");
                    FeatureClass qaFeatureClass = qaVersionGeodatabase.OpenDataset<FeatureClass>("featureClassName");
                }
            });
        }
        #endregion Working with Versions

        // cref: ARCGIS.CORE.DATA.VERSION.GETPARENT
        // cref: ARCGIS.CORE.DATA.Geodatabase.GetVersionManager
        // cref: ARCGIS.CORE.DATA.VERSIONMANAGER.GETCURRENTVERSION
        #region Working with the Default Version
        // Check to see if the current version is default.
        // Works with both branch and traditional versioning.
        public bool IsDefaultVersion(Version version)
        {
            Version parentVersion = version.GetParent();
            if (parentVersion == null)
            {
                return true;
            }
            parentVersion.Dispose();
            return false;
        }

        public bool IsDefaultVersion(Geodatabase geodatabase)
        {
            if (!geodatabase.IsVersioningSupported()) return false;
            using (VersionManager versionManager = geodatabase.GetVersionManager())
            using (Version currentVersion = versionManager.GetCurrentVersion())
            {
                return IsDefaultVersion(currentVersion);
            }
        }

        // Gets the default version.
        // Works with both branch and traditional versioning.
        // Note that this routine depends on IsDefaultVersion(), above.
        public Version GetDefaultVersion(Version version)
        {
            if (IsDefaultVersion(version))
            {
                return version;
            }
            else
            {
                Version parent = version.GetParent();
                Version ancestor = GetDefaultVersion(parent);
                if (parent != ancestor)
                {
                    parent.Dispose(); //If the versioning tree is more than 2 deep, we want to dispose any intermediary versions
                }
                return ancestor;
            }
        }

        public Version GetDefaultVersion(Geodatabase geodatabase)
        {
            if (!geodatabase.IsVersioningSupported()) return null;

            using (VersionManager versionManager = geodatabase.GetVersionManager())
            {
                Version currentVersion = versionManager.GetCurrentVersion();
                Version defaultVersion = GetDefaultVersion(currentVersion);
                if (currentVersion != defaultVersion)
                {
                    currentVersion.Dispose(); // If we are not pointing to default, we want to dispose this Version object
                }
                return defaultVersion;
            }
        }
        #endregion Working with the Default Version

        // cref: ARCGIS.CORE.DATA.VERSIONMANAGER.CREATEVERSION
        // cref: ARCGIS.CORE.DATA.VERSIONDESCRIPTION
        #region Creating a Version
        public Version CreateVersion(Geodatabase geodatabase, string versionName, string description, VersionAccessType versionAccessType)
        {
            if (!geodatabase.IsVersioningSupported()) return null;

            using (VersionManager versionManager = geodatabase.GetVersionManager())
            {
                VersionDescription versionDescription = new VersionDescription(versionName, description, versionAccessType);
                return versionManager.CreateVersion(versionDescription);
            }
        }
        #endregion Creating a Version

        // cref: ArcGIS.Core.Data.VersionManager.CreateHistoricalVersion
        // cref: ArcGIS.Core.Data.HistoricalVersionDescription
        // cref: ArcGIS.Core.Data.HistoricalVersion
        #region Creating a Historical version
        public HistoricalVersion CreateHistoricalVersion(Geodatabase geodatabase, string versionName)
        {
            using (VersionManager versionManager = geodatabase.GetVersionManager())
            {
                HistoricalVersionDescription historicalVersionDescription = new HistoricalVersionDescription(versionName, DateTime.Now);
                HistoricalVersion historicalVersion = versionManager.CreateHistoricalVersion(historicalVersionDescription);

                return historicalVersion;
            }
        }
        #endregion Creating a Historical version

        // cref: ArcGIS.Desktop.Mapping.Map.ChangeVersion(ArcGIS.Core.Data.VersionBase,ArcGIS.Core.Data.VersionBase)
        // cref: ArcGIS.Core.Data.VersionManager.GetCurrentVersionBaseType
        // cref: ArcGIS.Core.Data.VersionManager.GetCurrentHistoricalVersion
        // cref: ArcGIS.Core.Data.VersionManager.GetHistoricalVersion
        // cref: ArcGIS.Core.Data.VersionBaseType
        // cref: ArcGIS.Core.Data.HistoricalVersion
        #region Switching between versions
        public void ChangeVersions(Geodatabase geodatabase, string toVersionName)
        {
            using (VersionManager versionManager = geodatabase.GetVersionManager())
            {
                VersionBaseType versionBaseType = versionManager.GetCurrentVersionBaseType();

                if (versionBaseType == VersionBaseType.Version)
                {
                    Version fromVersion = versionManager.GetCurrentVersion();
                    Version toVersion = versionManager.GetVersion(toVersionName);

                    // Switch between versions
                    MapView.Active.Map.ChangeVersion(fromVersion, toVersion);
                }

                if (versionBaseType == VersionBaseType.HistoricalVersion)
                {
                    HistoricalVersion fromHistoricalVersion = versionManager.GetCurrentHistoricalVersion();
                    HistoricalVersion toHistoricalVersion = versionManager.GetHistoricalVersion(toVersionName);

                    // Switch between historical versions
                    MapView.Active.Map.ChangeVersion(fromHistoricalVersion, toHistoricalVersion);
                }

                // Switch from HistoricalVersion to Version and vice-versa 
                // MapView.Active.Map.ChangeVersion(fromHistoricalVersion, toVersion);
                // MapView.Active.Map.ChangeVersion(fromVersion, toHistoricalVersion);

            }
        }
        #endregion Switching between versions

        public void PartialPostingSnippet(Version designVersion, FeatureClass supportStructureFeatureClass, List<long> deletedSupportStructureObjectIDs)
        {
            // cref: ArcGIS.Core.Data.ReconcileOptions
            // cref: ArcGIS.Core.Data.PostOptions
            // cref: ArcGIS.Core.Data.PostOptions.PartialPostSelections
            // cref: ArcGIS.Core.Data.Version.Reconcile(ArcGIS.Core.Data.ReconcileOptions,ArcGIS.Core.Data.PostOptions)
            #region Partial Posting

            // Partial posting allows developers to post a subset of changes made in a version.
            // One sample use case is an electric utility that uses a version to design the facilities in
            // a new housing subdivision. At some point in the process, one block of new houses have been
            // completed, while the rest of the subdivision remains unbuilt.  Partial posting allows the user
            // to post the completed work, while leaving not yet constructed features in the version to be
            // posted later. Partial posting requires a branch-versioned feature service using ArcGIS
            // Enterprise 10.9 and higher

            // Specify a set of features that were constructed
            QueryFilter constructedFilter = new QueryFilter()
            {
                WhereClause = "ConstructedStatus = 'True'"
            };
            // This selection represents the inserts and updates to the support
            // structure feature class that we wish to post
            using (Selection constructedSupportStructures = supportStructureFeatureClass.Select(constructedFilter, SelectionType.ObjectID, SelectionOption.Normal))
            {
                // Specifying which feature deletions you wish to post is slightly trickier, since you cannot issue
                // a query to fetch a set of deleted features Instead, a list of ObjectIDs must be used
                using (Selection deletedSupportStructures = supportStructureFeatureClass.Select(
                                                    null, SelectionType.ObjectID, SelectionOption.Empty))
                {
                    deletedSupportStructures.Add(deletedSupportStructureObjectIDs);  //deletedSupportStructureObjectIDs is
                                                                                     //defined as List<long>

                    //Perform the reconcile with partial post
                    //At 2.x - 
                    //ReconcileDescription reconcileDescription = new ReconcileDescription();
                    //reconcileDescription.ConflictDetectionType = ConflictDetectionType.ByColumn;
                    //reconcileDescription.ConflictResolutionMethod = ConflictResolutionMethod.Continue;
                    //reconcileDescription.ConflictResolutionType = ConflictResolutionType.FavorEditVersion;
                    //reconcileDescription.PartialPostSelections = new List<Selection>() { constructedSupportStructures, deletedSupportStructures };
                    //reconcileDescription.WithPost = true;

                    //ReconcileResult reconcileResult = designVersion.Reconcile(reconcileDescription);

                    ReconcileOptions reconcileOptions = new ReconcileOptions();//reconcile against Default
                    reconcileOptions.ConflictDetectionType = ConflictDetectionType.ByColumn;
                    reconcileOptions.ConflictResolutionMethod = ConflictResolutionMethod.Continue;
                    reconcileOptions.ConflictResolutionType = ConflictResolutionType.FavorEditVersion;

                    PostOptions postOptions = new PostOptions();//post against Default
                    postOptions.PartialPostSelections = new List<Selection>() {
                          constructedSupportStructures, deletedSupportStructures };
                    postOptions.ServiceSynchronizationType = ServiceSynchronizationType.Synchronous;

                    ReconcileResult reconcileResult = designVersion.Reconcile(reconcileOptions, postOptions);

                    //TODO process result(s)
                }
            }

            #endregion
        }

        // cref: ArcGIS.Core.Data.FeatureDataset.GetDefinition``1(String)
        // cref: ArcGIS.Core.Data.FeatureDataset.GetDefinition
        #region Iterate datasets inside a feature dataset

        public void IterateDatasetsFromAFeatureDataset(Geodatabase geodatabase, string featureDatasetName = "City", string featureClassInFeatureDataset = "Buildings")
        {
            // Open a feature dataset
            using (FeatureDataset cityFeatureDataset = geodatabase.OpenDataset<FeatureDataset>(featureDatasetName))
            {
                // Get a feature class definition from a feature dataset
                FeatureClassDefinition buildingsFeatureClassDefinition = cityFeatureDataset.GetDefinition<FeatureClassDefinition>(featureClassInFeatureDataset);

                // Iterate dataset definition
                IReadOnlyList<FeatureClassDefinition> cityFeatureClassDefinitions = cityFeatureDataset.GetDefinitions<FeatureClassDefinition>();
                foreach (FeatureClassDefinition cityFeatureClassDefinition in cityFeatureClassDefinitions)
                {
                    // Use feature class definition
                }
            }
        }

        #endregion

        // cref: ArcGIS.Core.Data.AttributeRuleDefinition.GetIsFieldEditable
        // cref: ArcGIS.Core.Data.AttributeRuleDefinition.GetEvaluationOrder
        // cref: ArcGIS.Core.Data.AttributeRuleDefinition.GetTriggeringEvents
        // cref: ArcGIS.Core.Data.AttributeRuleDefinition.GetScriptExpression
        // cref: ArcGIS.Core.Data.TableDefinition.GetAttributeRules
        #region Get attribute rules of a dataset

        public void GetAttributeRules(Geodatabase geodatabase, string tableName)
        {
            using (TableDefinition tableDefinition = geodatabase.GetDefinition<TableDefinition>(tableName))
            {
                // Get all attribute rule types
                IReadOnlyList<AttributeRuleDefinition> ruleDefinitions = tableDefinition.GetAttributeRules();

                // Iterate rule definitions
                foreach (AttributeRuleDefinition ruleDefinition in ruleDefinitions)
                {
                    AttributeRuleType ruleType = ruleDefinition.GetAttributeRuleType();
                    string ruleDescription = ruleDefinition.GetDescription();
                    bool isAttributeFieldEditable = ruleDefinition.GetIsFieldEditable();
                    string arcadeVersionToSupportRule = ruleDefinition.GetMinimumArcadeVersion();
                    int ruleEvaluationOrder = ruleDefinition.GetEvaluationOrder();
                    AttributeRuleTriggers triggeringEvents = ruleDefinition.GetTriggeringEvents();
                    string scriptExpression = ruleDefinition.GetScriptExpression();

                    // more properties
                }
            }
        }

        #endregion

        // cref: ARCGIS.CORE.DATA.TABLE.CREATEROWBUFFER
        // cref: ARCGIS.CORE.DATA.TABLE.CREATEROWBUFFER()
        #region Creating  a row buffer from a template row

        public void CreateRowBufferFromARow(Table table)
        {
            using (RowCursor rowCursor = table.Search())
            {
                if (rowCursor.MoveNext())
                {
                    using (Row templateRow = rowCursor.Current)
                    {
                        RowBuffer rowBuffer = table.CreateRowBuffer(templateRow);

                        // Manipulate row buffer
                        // Doesn't allow copying values of ObjectID and GlobalID
                        //
                        // rowBuffer["Field"] = "Update";
                        //

                        // create a new row
                        table.CreateRow(rowBuffer);
                    }
                }
            }
        }

        #endregion


        #region ProSnippet Group: Extension Ids
        #endregion

        // cref: ArcGIS.Core.Data.CoreDataExtensions.AddActivationExtension
        // cref: ArcGIS.Core.Data.CoreDataExtensions.RemoveActivationExtension
        // cref: ArcGIS.Core.Data.CoreDataExtensions.GetHasActivationExtension
        // cref: ArcGIS.Core.Data.CoreDataExtensions.GetActivationExtensions
        #region Table Extension Ids
        public void ExtensionIds1(Table table)
        {
            //Add an extension id to a table
            //Check documentation for restrictions on backward compatibility - backward
            //compatibility is limited to ArcGIS Pro 3.1 if an extension id is added.
            //Note: This is an extension method. It is for use in addins only and not CoreHost.
            string extension_id_string = "52d8f3be-b73d-4140-beaf-23d4f9b697ea";
            Guid extension_id = Guid.Parse(extension_id_string);

            //Note: Must be within the lambda of QueuedTask.Run(() => { ...

            //register the extension id with the relevant table
            table.AddActivationExtension(extension_id);

            //Remove an extension id from a table
            //Restores backward compatibility assuming no other compatibility limitations are
            //in place.
            //Note: This is an extension method. It is for use in addins only and not CoreHost.
            table.RemoveActivationExtension(extension_id);

            //Check if a given extension id  is registered with a particular table.
            //Note: This is an extension method. It is for use in addins only and not CoreHost.
            if (table.GetHasActivationExtension(extension_id))
            {
                //TODO - implement custom logic relevant to presence of extension_id
            }

            //Enumerate all extension ids on a given table.
            //Note: This is an extension method. It is for use in addins only and not CoreHost.
            foreach (Guid ext_id in table.GetActivationExtensions())
            {
                //TODO - logic based on extension ids
            }
        }
        #endregion

        #region ProSnippet Group: DDL
        #endregion

        // cref: ArcGIS.Core.Data.DDL.FeatureClassDescription
        // cref: ArcGIS.Core.Data.DDL.FieldDescription
        // cref: ArcGIS.Core.Data.DDL.ShapeDescription
        // cref: ArcGIS.Core.Data.FieldType
        #region 64-bit Integer field

        FieldDescription bigIntegerFieldDescription = new FieldDescription("BigInteger_64", FieldType.BigInteger);

        #endregion

        // cref: ArcGIS.Core.Data.DDL.FeatureClassDescription
        // cref: ArcGIS.Core.Data.DDL.FieldDescription
        // cref: ArcGIS.Core.Data.DDL.ShapeDescription
        // cref: ArcGIS.Core.Data.FieldType
        #region ObjectID field

        // 64-bit
        FieldDescription oidFieldDescription_64 = new FieldDescription("ObjectID_64", FieldType.OID)
        {
            Length = 8
        };

        // 32-bit
        FieldDescription oidFieldDescription_32 = new FieldDescription("ObjectID_32", FieldType.OID)
        {
            Length = 4
        };

        #endregion

        // cref: ArcGIS.Core.Data.DDL.FeatureClassDescription
        // cref: ArcGIS.Core.Data.DDL.FieldDescription
        // cref: ArcGIS.Core.Data.DDL.ShapeDescription
        // cref: ArcGIS.Core.Data.FieldType
        #region DateOnly, TimeOnly, and TimestampOffset  field
        // Earthquake occurrences date and time

        // 9/28/2014 (DateOnly)
        FieldDescription earthquakeDateOnlyFieldDescription = new FieldDescription("Earthquake_DateOnly", FieldType.DateOnly);

        // 1:16:42 AM (TimeOnly)
        FieldDescription earthquakeTimeOnlyFieldDescription = new FieldDescription("Earthquake_TimeOnly", FieldType.TimeOnly);

        // 9/28/2014 1:16:42.000 AM -09:00 (Timestamp with Offset)
        FieldDescription earthquakeTimestampOffsetFieldDescription = new FieldDescription("Earthquake_TimestampOffset_Local", FieldType.TimestampOffset);

        // 9/28/2014 1:16:42 AM (DateTime)
        FieldDescription earthquakeDateFieldDescription = new FieldDescription("Earthquake_Date", FieldType.Date);

        #endregion

        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Create(ArcGIS.Core.Data.DDL.TableDescription)
        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Build
        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.ErrorMessages
        // cref: ArcGIS.Core.Data.DDL.TableDescription.#ctor(System.String,IEnumerable{ArcGIS.Core.Data.DDL.FieldDescription})
        // cref: ArcGIS.Core.Data.DDL.FieldDescription.CreateGlobalIDField
        // cref: ArcGIS.Core.Data.DDL.FieldDescription.CreateObjectIDField
        // cref: ArcGIS.Core.Data.DDL.FieldDescription.CreateDomainField
        // cref: ArcGIS.Core.Data.DDL.FieldDescription.CreateStringField
        // cref: ArcGIS.Core.Data.DDL.FieldDescription.#ctor
        // cref: ArcGIS.Core.Data.DDL.FieldDescription.AliasName
        #region Creating a Table
        public void CreateTableSnippet(Geodatabase geodatabase, CodedValueDomain inspectionResultsDomain)
        {
            // Create a PoleInspection table with the following fields
            //  GlobalID
            //  ObjectID
            //  InspectionDate (date)
            //  InspectionResults (pre-existing InspectionResults coded value domain)
            //  InspectionNotes (string)

            // This static helper routine creates a FieldDescription for a GlobalID field with default values
            FieldDescription globalIDFieldDescription = FieldDescription.CreateGlobalIDField();

            // This static helper routine creates a FieldDescription for an ObjectID field with default values
            FieldDescription objectIDFieldDescription = FieldDescription.CreateObjectIDField();

            // Create a FieldDescription for the InspectionDate field
            FieldDescription inspectionDateFieldDescription = new FieldDescription("InspectionDate", FieldType.Date)
            {
                AliasName = "Inspection Date"
            };

            // This static helper routine creates a FieldDescription for a Domain field (from a pre-existing domain)
            FieldDescription inspectionResultsFieldDescription = FieldDescription.CreateDomainField("InspectionResults", new CodedValueDomainDescription(inspectionResultsDomain));
            inspectionResultsFieldDescription.AliasName = "Inspection Results";

            // This static helper routine creates a FieldDescription for a string field
            FieldDescription inspectionNotesFieldDescription = FieldDescription.CreateStringField("InspectionNotes", 512);
            inspectionNotesFieldDescription.AliasName = "Inspection Notes";

            // Assemble a list of all of our field descriptions
            List<FieldDescription> fieldDescriptions = new List<FieldDescription>()
        { globalIDFieldDescription, objectIDFieldDescription, inspectionDateFieldDescription, inspectionResultsFieldDescription, inspectionNotesFieldDescription };

            // Create a TableDescription object to describe the table to create
            TableDescription tableDescription = new TableDescription("PoleInspection", fieldDescriptions);

            // Create a SchemaBuilder object
            SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);

            // Add the creation of PoleInspection to our list of DDL tasks
            schemaBuilder.Create(tableDescription);

            // Execute the DDL
            bool success = schemaBuilder.Build();

            // Inspect error messages
            if (!success)
            {
                IReadOnlyList<string> errorMessages = schemaBuilder.ErrorMessages;
                //etc.
            }
        }
        #endregion

        // cref: ArcGIS.Core.Data.DDL.FeatureClassDescription.#ctor(System.String,System.Collections.Generic.IEnumerable{ArcGIS.Core.Data.DDL.FieldDescription},ArcGIS.Core.Data.DDL.ShapeDescription)
        // cref: ArcGIS.Core.Data.DDL.FieldDescription.#ctor(System.String,ArcGIS.Core.Data.FieldType)
        // cref: ArcGIS.Core.Data.DDL.FieldDescription.CreateGlobalIDField
        // cref: ArcGIS.Core.Data.DDL.FieldDescription.CreateIntegerField(System.String)
        // cref: ArcGIS.Core.Data.DDL.FieldDescription.CreateObjectIDField
        // cref: ArcGIS.Core.Data.DDL.FieldDescription.CreateStringField(System.String,System.Int32)
        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Build
        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Create(ArcGIS.Core.Data.DDL.FeatureClassDescription)
        // cref: ArcGIS.Core.Data.DDL.ShapeDescription.#ctor(ArcGIS.Core.Data.FeatureClassDefinition)
        // cref: ArcGIS.Core.Data.DDL.ShapeDescription.#ctor(ArcGIS.Core.Geometry.GeometryType,ArcGIS.Core.Geometry.SpatialReference)
        #region Creating a feature class
        public void CreateFeatureClassSnippet(Geodatabase geodatabase, FeatureClass existingFeatureClass, SpatialReference spatialReference)
        {
            // Create a Cities feature class with the following fields
            //  GlobalID
            //  ObjectID
            //  Name (string)
            //  Population (integer)

            // This static helper routine creates a FieldDescription for a GlobalID field with default values
            FieldDescription globalIDFieldDescription = FieldDescription.CreateGlobalIDField();

            // This static helper routine creates a FieldDescription for an ObjectID field with default values
            FieldDescription objectIDFieldDescription = FieldDescription.CreateObjectIDField();

            // This static helper routine creates a FieldDescription for a string field
            FieldDescription nameFieldDescription = FieldDescription.CreateStringField("Name", 255);

            // This static helper routine creates a FieldDescription for an integer field
            FieldDescription populationFieldDescription = FieldDescription.CreateIntegerField("Population");

            // Assemble a list of all of our field descriptions
            List<FieldDescription> fieldDescriptions = new List<FieldDescription>()
        { globalIDFieldDescription, objectIDFieldDescription, nameFieldDescription, populationFieldDescription };

            // Create a ShapeDescription object
            ShapeDescription shapeDescription = new ShapeDescription(GeometryType.Point, spatialReference);

            // Alternatively, ShapeDescriptions can be created from another feature class.  In this case, the new feature class will inherit the same shape properties of the existing class
            ShapeDescription alternativeShapeDescription = new ShapeDescription(existingFeatureClass.GetDefinition());

            // Create a FeatureClassDescription object to describe the feature class to create
            FeatureClassDescription featureClassDescription =
              new FeatureClassDescription("Cities", fieldDescriptions, shapeDescription);

            // Create a SchemaBuilder object
            SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);

            // Add the creation of the Cities feature class to our list of DDL tasks
            schemaBuilder.Create(featureClassDescription);

            // Execute the DDL
            bool success = schemaBuilder.Build();

            // Inspect error messages
            if (!success)
            {
                IReadOnlyList<string> errorMessages = schemaBuilder.ErrorMessages;
                //etc.
            }
        }
        #endregion

        // cref:ArcGIS.Core.Data.DDL.SchemaBuilder.Delete
        // cref:ArcGIS.Core.Data.DDL.SchemaBuilder.Build
        #region Deleting a Table
        public void DeleteTableSnippet(Geodatabase geodatabase, Table table)
        {
            // Create a TableDescription object
            TableDescription tableDescription = new TableDescription(table.GetDefinition());

            // Create a SchemaBuilder object
            SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);

            // Add the deletion of the table to our list of DDL tasks
            schemaBuilder.Delete(tableDescription);

            // Execute the DDL
            bool success = schemaBuilder.Build();
        }
        #endregion

        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Delete
        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Build
        #region Deleting a Feature Class
        public void DeleteFeatureClassSnippet(Geodatabase geodatabase, FeatureClass featureClass)
        {
            // Create a FeatureClassDescription object
            FeatureClassDescription featureClassDescription = new FeatureClassDescription(featureClass.GetDefinition());

            // Create a SchemaBuilder object
            SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);

            // Add the deletion fo the feature class to our list of DDL tasks
            schemaBuilder.Delete(featureClassDescription);

            // Execute the DDL
            bool success = schemaBuilder.Build();
        }
        #endregion


        // cref: ArcGIS.Core.Data.MemoryConnectionProperties
        // cref: ArcGIS.Core.Data.Geodatabase.#ctor(ArcGIS.Core.Data.MemoryConnectionProperties)
        #region Opens a memory geodatabase
        public void OpenMemoryGeodatabase()
        {
            // Connects to the default memory geodatabase, if exists otherwise throws exception
            MemoryConnectionProperties memoryConnectionProperties = new MemoryConnectionProperties();

            // Alternatively, connects to memory geodatabase named as 'InterimMemoryGeodatabase'
            // MemoryConnectionProperties memoryConnectionProperties = new MemoryConnectionProperties("InterimMemoryGeodatabase");

            // Opens the memory geodatabase
            using (Geodatabase geodatabase = new Geodatabase(memoryConnectionProperties))
            {
                // Use memory geodatabase
            }
        }
        #endregion

        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.CreateGeodatabase(ArcGIS.Core.Data.MemoryConnectionProperties)
        // cref: ArcGIS.Core.Data.MemoryConnectionProperties
        #region Creating a memory Geodatabase
        public void CreateMemoryGeodatabaseSnippet()
        {
            // Create the memory connection properties to connect to  default memory geodatabase
            MemoryConnectionProperties memoryConnectionProperties = new MemoryConnectionProperties();

            // Alternatively create the memory connection properties to connect to memory geodatabase named as 'InterimMemoryGeodatabase'
            // MemoryConnectionProperties memoryConnectionProperties = new MemoryConnectionProperties("InterimMemoryGeodatabase");

            // Creates the new memory geodatabase if it does not exist or connects to an existing one if it already exists
            using (Geodatabase geodatabase = new Geodatabase(memoryConnectionProperties))
            {
                // Create additional schema here
            }
        }
        #endregion

        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.DeleteGeodatabase(ArcGIS.Core.Data.MemoryConnectionProperties)
        // cref: ArcGIS.Core.Data.MemoryConnectionProperties
        #region Deleting a memory Geodatabase
        public void DeleteMemoryGeodatabaseSnippet()
        {
            // Create the memory connection properties to connect to default memory geodatabase
            MemoryConnectionProperties memoryConnectionProperties = new MemoryConnectionProperties();

            // Delete the memory geodatabase
            SchemaBuilder.DeleteGeodatabase(memoryConnectionProperties);
        }
        #endregion

        // cref: ArcGIS.Core.Data.FileGeodatabaseConnectionPath
        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.CreateGeodatabase(ArcGIS.Core.Data.FileGeodatabaseConnectionPath)
        #region Creating a File Geodatabase
        public void CreateFileGeodatabaseSnippet()
        {
            // Create a FileGeodatabaseConnectionPath with the name of the file geodatabase you wish to create
            FileGeodatabaseConnectionPath fileGeodatabaseConnectionPath =
              new FileGeodatabaseConnectionPath(new Uri(@"C:\Path-To-File-Geodatabase\YourName.gdb"));

            // Create and use the file geodatabase
            using (Geodatabase geodatabase =
              SchemaBuilder.CreateGeodatabase(fileGeodatabaseConnectionPath))
            {
                // Create additional schema here
            }
        }
        #endregion

        // cref: ArcGIS.Core.Data.FileGeodatabaseConnectionPath
        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.DeleteGeodatabase(ArcGIS.Core.Data.FileGeodatabaseConnectionPath)
        #region Deleting a File Geodatabase
        public void DeleteFileGeodatabaseSnippet()
        {
            // Create a FileGeodatabaseConnectionPath with the name of the file geodatabase you wish to delete
            FileGeodatabaseConnectionPath fileGeodatabaseConnectionPath = new FileGeodatabaseConnectionPath(new Uri(@"C:\Path-To-File-Geodatabase\YourName.gdb"));

            // Delete the file geodatabase
            SchemaBuilder.DeleteGeodatabase(fileGeodatabaseConnectionPath);
        }
        #endregion

        // cref: ArcGIS.Core.Data.MobileGeodatabaseConnectionPath
        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.CreateGeodatabase(ArcGIS.Core.Data.MobileGeodatabaseConnectionPath)
        #region Creating a Mobile Geodatabase
        public void CreateMobileGeodatabase()
        {
            // Create a MobileGeodatabaseConnectionPath with the name of the mobile geodatabase you wish to create
            MobileGeodatabaseConnectionPath mobileGeodatabaseConnectionPath = new MobileGeodatabaseConnectionPath(new Uri(@"C:\Path-To-Mobile-Geodatabase\YourName.geodatabase"));

            // Create and use the mobile geodatabase
            using (Geodatabase geodatabase = SchemaBuilder.CreateGeodatabase(mobileGeodatabaseConnectionPath))
            {
                // Create additional schema here
            }
        }
        #endregion

        // cref: ArcGIS.Core.Data.MobileGeodatabaseConnectionPath
        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.DeleteGeodatabase(ArcGIS.Core.Data.MobileGeodatabaseConnectionPath)
        #region Deleting a Mobile Geodatabase
        public void DeleteMobileGeodatabase()
        {
            // Create a MobileGeodatabaseConnectionPath with the name of the mobile geodatabase you wish to delete
            MobileGeodatabaseConnectionPath mobileGeodatabaseConnectionPath =
              new MobileGeodatabaseConnectionPath(new Uri(@"C:\Path-To-Mobile-Geodatabase\YourName.geodatabase"));

            // Delete the mobile geodatabase
            SchemaBuilder.DeleteGeodatabase(mobileGeodatabaseConnectionPath);
        }
        #endregion

        // cref: ArcGIS.Core.Data.DDL.RangeDomainDescription.#ctor(System.String,ArcGIS.Core.Data.FieldType,System.Object,System.Object)
        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Create(ArcGIS.Core.Data.DDL.RangeDomainDescription)
        // cref: ArcGIS.Core.Data.DDL.DomainDescription.Description
        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Build
        #region Creating a Range domain
        public void CreateRangeDomainSnippet(Geodatabase geodatabase)
        {
            // Create a range description with minimum value = 0 and maximum value = 1000
            RangeDomainDescription rangeDomainDescriptionMinMax = new RangeDomainDescription("RangeDomain_0_1000",
              FieldType.Integer, 0, 1000)
            { Description = "Domain value ranges from 0 to 1000" };

            SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);

            // Create  a range domain 
            schemaBuilder.Create(rangeDomainDescriptionMinMax);
            schemaBuilder.Build();
        }
        #endregion

        // cref: ArcGIS.Core.Data.DDL.CodedValueDomainDescription.#ctor(System.String,ArcGIS.Core.Data.FieldType,System.Collections.Generic.SortedList{System.Object,System.String})
        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Create(ArcGIS.Core.Data.DDL.CodedValueDomainDescription)
        // cref: ArcGIS.Core.Data.DDL.DomainDescription.SplitPolicy
        // cref: ArcGIS.Core.Data.DDL.DomainDescription.MergePolicy
        // cref: ArcGIS.Core.Data.SplitPolicy
        // cref: ArcGIS.Core.Data.MergePolicy
        // cref: ArcGIS.Core.Data.DDL.CodedValueDomainToken
        #region Creating a CodedValue domain 
        public void CreateCodedDomainSnippet(Geodatabase geodatabase)
        {
            // Create a CodedValueDomain description for water pipes
            CodedValueDomainDescription codedValueDomainDescription = new CodedValueDomainDescription("WaterPipeTypes", FieldType.String,
              new SortedList<object, string> { { "C_1", "Copper" }, { "S_2", "Steel" } })
            {
                SplitPolicy = SplitPolicy.Duplicate,
                MergePolicy = MergePolicy.DefaultValue
            };

            SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);

            // Create a coded value domain 
            CodedValueDomainToken codedValueDomainToken = schemaBuilder.Create(codedValueDomainDescription);
            schemaBuilder.Build();
        }
        #endregion

        // cref: ArcGIS.Core.Data.DDL.FeatureDatasetDescription.#ctor(System.String,ArcGIS.Core.Geometry.SpatialReference)
        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Create(ArcGIS.Core.Data.DDL.FeatureDatasetDescription)
        #region Creating a FeatureDataset
        public void CreateFeatureDatasetSnippet(Geodatabase geodatabase)
        {
            // Creating a FeatureDataset named as 'Parcel_Information'

            SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);

            // Create a FeatureDataset named as 'Parcel Information'
            FeatureDatasetDescription featureDatasetDescription =
              new FeatureDatasetDescription("Parcel_Information", SpatialReferences.WGS84);
            schemaBuilder.Create(featureDatasetDescription);

            // Build status
            bool buildStatus = schemaBuilder.Build();

            // Build errors
            if (!buildStatus)
            {
                IReadOnlyList<string> errors = schemaBuilder.ErrorMessages;
            }
        }
        #endregion

        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Delete(ArcGIS.Core.Data.DDL.Description)
        // cref: ArcGIS.Core.Data.FeatureDatasetDefinition
        #region Deleting a FeatureDataset
        public void DeleteFeatureDatasetSnippet(Geodatabase geodatabase)
        {
            // Deleting a FeatureDataset named as 'Parcel_Information'

            FeatureDatasetDefinition featureDatasetDefinition = geodatabase.GetDefinition<FeatureDatasetDefinition>("Parcel_Information");
            FeatureDatasetDescription featureDatasetDescription =
              new FeatureDatasetDescription(featureDatasetDefinition);

            SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);

            // Delete an existing feature dataset named as 'Parcel_Information'
            schemaBuilder.Delete(featureDatasetDescription);
            schemaBuilder.Build();
        }
        #endregion

        // cref:ArcGIS.Core.Data.DDL.SchemaBuilder.Rename
        #region Renaming a FeatureDataset
        public void RenameFeatureDatasetSnippet(Geodatabase geodatabase)
        {
            // Renaming a FeatureDataset from 'Parcel_Information' to 'Parcel_Information_With_Tax_Jurisdiction'

            string originalDatasetName = "Parcel_Information";
            string datasetRenameAs = "Parcel_Information_With_Tax_Jurisdiction";

            FeatureDatasetDefinition originalDatasetDefinition =
              geodatabase.GetDefinition<FeatureDatasetDefinition>(originalDatasetName);
            FeatureDatasetDescription originalFeatureDatasetDescription = new FeatureDatasetDescription(originalDatasetDefinition);

            SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);

            // Rename the existing FeatureDataset, 'Parcel_Information' to 'Parcel_Information_With_Tax_Jurisdiction'
            schemaBuilder.Rename(originalFeatureDatasetDescription, datasetRenameAs);
            schemaBuilder.Build();
        }
        #endregion

        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Create(ArcGIS.Core.Data.DDL.FeatureDatasetDescription,ArcGIS.Core.Data.DDL.FeatureClassDescription)
        // cref: ArcGIS.Core.Data.DDL.FeatureDatasetToken
        // cref: ArcGIS.Core.Data.DDL.FeatureClassDescription
        // cref: ArcGIS.Core.Data.DDL.FieldDescription
        // cref: ArcGIS.Core.Data.DDL.ShapeDescription
        // cref: ArcGIS.Core.Data.DDL.FeatureClassToken
        // cref: ArcGIS.Core.Data.FieldType
        #region Creating a FeatureDataset with a FeatureClass in one operation
        public void CreateFeatureDatasetWithFeatureClassSnippet(Geodatabase geodatabase)
        {
            // Creating a FeatureDataset named as 'Parcel_Information' and a FeatureClass with name 'Parcels' in one operation

            string featureDatasetName = "Parcel_Information";
            string featureClassName = "Parcels";

            SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);

            // Create a FeatureDataset token
            FeatureDatasetDescription featureDatasetDescription = new FeatureDatasetDescription(featureDatasetName, SpatialReferences.WGS84);
            FeatureDatasetToken featureDatasetToken = schemaBuilder.Create(featureDatasetDescription);

            // Create a FeatureClass description
            FeatureClassDescription featureClassDescription = new FeatureClassDescription(featureClassName,
              new List<FieldDescription>()
              {
          new FieldDescription("Id", FieldType.Integer),
          new FieldDescription("Address", FieldType.String)
              },
              new ShapeDescription(GeometryType.Point, SpatialReferences.WGS84));

            // Create a FeatureClass inside a FeatureDataset
            FeatureClassToken featureClassToken = schemaBuilder.Create(new FeatureDatasetDescription(featureDatasetToken), featureClassDescription);

            // Build status
            bool buildStatus = schemaBuilder.Build();

            // Build errors
            if (!buildStatus)
            {
                IReadOnlyList<string> errors = schemaBuilder.ErrorMessages;
            }
        }
        #endregion

        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Create(ArcGIS.Core.Data.DDL.FeatureDatasetDescription,ArcGIS.Core.Data.DDL.FeatureClassDescription)
        // cref: ArcGIS.Core.Data.DDL.FeatureClassDescription
        // cref: ArcGIS.Core.Data.DDL.FieldDescription
        // cref: ArcGIS.Core.Data.DDL.ShapeDescription
        // cref: ArcGIS.Core.Data.FieldType
        #region Creating a FeatureClass in existing FeatureDataset 
        public void CreateFeatureClassInsideFeatureDatasetSnippet(Geodatabase geodatabase)
        {
            // Creating a FeatureClass named as 'Tax_Jurisdiction' in existing FeatureDataset with name 'Parcels_Information'
            string featureDatasetName = "Parcels_Information";
            string featureClassName = "Tax_Jurisdiction";

            // Create a FeatureClass description
            FeatureClassDescription featureClassDescription = new FeatureClassDescription(featureClassName,
              new List<FieldDescription>()
              {
          new FieldDescription("Tax_Id", FieldType.Integer),
          new FieldDescription("Address", FieldType.String)
              },
              new ShapeDescription(GeometryType.Point, SpatialReferences.WGS84));

            FeatureDatasetDefinition featureDatasetDefinition = geodatabase.GetDefinition<FeatureDatasetDefinition>(featureDatasetName);

            SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);

            // Create a FeatureClass inside a FeatureDataset using a FeatureDatasetDefinition
            schemaBuilder.Create(new FeatureDatasetDescription(featureDatasetDefinition), featureClassDescription);

            // Build status
            bool buildStatus = schemaBuilder.Build();

            // Build errors
            if (!buildStatus)
            {
                IReadOnlyList<string> errors = schemaBuilder.ErrorMessages;
            }
        }
        #endregion

        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.AddFeatureClass(ArcGIS.Core.Data.DDL.FeatureDatasetDescription,ArcGIS.Core.Data.DDL.FeatureClassDescription)
        #region Adding a FeatureClass to a FeatureDataset
        public void AddFeatureClassToFeatureDatasetSnippet(Geodatabase geodatabase)
        {
            // Adding a FeatureClass with name 'Tax_Jurisdiction' into a FeatureDataset named as 'Parcels_Information'

            string featureDatasetName = "Parcels_Information";
            string featureClassNameToAdd = "Tax_Jurisdiction";

            FeatureDatasetDefinition featureDatasetDefinition = geodatabase.GetDefinition<FeatureDatasetDefinition>(featureDatasetName);
            FeatureDatasetDescription featureDatasetDescription = new FeatureDatasetDescription(featureDatasetDefinition);

            FeatureClassDefinition featureClassDefinition = geodatabase.GetDefinition<FeatureClassDefinition>(featureClassNameToAdd);
            FeatureClassDescription featureClassDescription = new FeatureClassDescription(featureClassDefinition);

            SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);

            // Add the 'Tax_Jurisdiction' FeatureClass to the 'Parcels_Information' FeatureDataset 
            schemaBuilder.AddFeatureClass(featureDatasetDescription, featureClassDescription);
            bool addStatus = schemaBuilder.Build();

            if (!addStatus)
            {
                IReadOnlyList<string> errors = schemaBuilder.ErrorMessages;
            }
        }
        #endregion

        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Rename(ArcGIS.Core.Data.DDL.Description,System.String)
        // cref: ArcGIS.Core.Data.DDL.TableDescription
        #region Renaming a Table
        public void RenameTableSnippet(Geodatabase geodatabase)
        {
            //Renaming a table from 'Original_Table' to 'Renamed_Table'

            string tableToBeRenamed = "Original_Table";
            string tableRenameAs = "Renamed_Table";

            TableDefinition tableDefinition = geodatabase.GetDefinition<TableDefinition>(tableToBeRenamed);

            SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);

            // Table rename 
            schemaBuilder.Rename(new TableDescription(tableDefinition), tableRenameAs);
            schemaBuilder.Build();
        }
        #endregion

        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Modify(ArcGIS.Core.Data.DDL.FeatureClassDescription)
        // cref: ArcGIS.Core.Data.DDL.FieldDescription.CreateGlobalIDField
        // cref: ArcGIS.Core.Data.DDL.FieldDescription.CreateIntegerField
        // cref: ArcGIS.Core.Data.DDL.FieldDescription.CreateStringField
        #region Adding fields to a FeatureClass
        public void AddFieldsInFeatureClassSnippet(Geodatabase geodatabase)
        {
            // Adding following fields to the 'Parcels' FeatureClass
            // Global ID
            // Parcel_ID
            // Tax_Code
            // Parcel_Address

            // The FeatureClass to add fields
            string featureClassName = "Parcels";

            FeatureClassDefinition originalFeatureClassDefinition = geodatabase.GetDefinition<FeatureClassDefinition>(featureClassName);
            FeatureClassDescription originalFeatureClassDescription = new FeatureClassDescription(originalFeatureClassDefinition);

            // The four new fields to add on the 'Parcels' FeatureClass
            FieldDescription globalIdField = FieldDescription.CreateGlobalIDField();
            FieldDescription parcelIdDescription = new FieldDescription("Parcel_ID", FieldType.GUID);
            FieldDescription taxCodeDescription = FieldDescription.CreateIntegerField("Tax_Code");
            FieldDescription addressDescription = FieldDescription.CreateStringField("Parcel_Address", 150);

            List<FieldDescription> fieldsToAdd = new List<FieldDescription> { globalIdField, parcelIdDescription,
        taxCodeDescription, addressDescription };

            // Add new fields on the new FieldDescription list
            List<FieldDescription> modifiedFieldDescriptions = new List<FieldDescription>(originalFeatureClassDescription.FieldDescriptions);
            modifiedFieldDescriptions.AddRange(fieldsToAdd);

            // The new FeatureClassDescription with additional fields
            FeatureClassDescription modifiedFeatureClassDescription = new FeatureClassDescription(originalFeatureClassDescription.Name,
              modifiedFieldDescriptions, originalFeatureClassDescription.ShapeDescription);

            SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);

            // Update the 'Parcels' FeatureClass with newly added fields
            schemaBuilder.Modify(modifiedFeatureClassDescription);
            bool modifyStatus = schemaBuilder.Build();

            if (!modifyStatus)
            {
                IReadOnlyList<string> errors = schemaBuilder.ErrorMessages;
            }
        }
        #endregion

        // cref: ArcGIS.Core.Data.DDL.FieldDescription.CreateDomainField(System.String,ArcGIS.Core.Data.DDL.DomainDescription)
        // cref: ArcGIS.Core.Data.DDL.CodedValueDomainToken
        // cref: ArcGIS.Core.Data.DDL.CodedValueDomainDescription
        // cref: ArcGIS.Core.Data.DDL.FieldDescription.DomainDescription
        // cref: ArcGIS.Core.Data.DDL.FeatureClassDescription
        // cref: ArcGIS.Core.Data.DDL.FeatureClassDescription.ShapeDescription
        #region Adding a Field that uses a domain
        public void AddFieldWithDomainSnippet(Geodatabase geodatabase)
        {
            // Adding a field,'PipeType', which uses the coded value domain to the 'Pipes' FeatureClass

            //The FeatureClass to add field
            string featureClassName = "Pipes";

            SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);

            // Create a CodedValueDomain description for water pipes
            CodedValueDomainDescription pipeDomainDescription =
              new CodedValueDomainDescription("WaterPipeTypes", FieldType.String,
              new SortedList<object, string> { { "C_1", "Copper" },
          { "S_2", "Steel" } })
              {
                  SplitPolicy = SplitPolicy.Duplicate,
                  MergePolicy = MergePolicy.DefaultValue
              };

            // Create a coded value domain token
            CodedValueDomainToken codedValueDomainToken = schemaBuilder.Create(pipeDomainDescription);

            // Create a new description from domain token
            CodedValueDomainDescription codedValueDomainDescription = new CodedValueDomainDescription(codedValueDomainToken);

            // Create a field named as 'PipeType' using a domain description
            FieldDescription domainFieldDescription = new FieldDescription("PipeType", FieldType.String);
            domainFieldDescription.SetDomainDescription(codedValueDomainDescription);

            //Retrieve existing information for 'Pipes' FeatureClass
            FeatureClassDefinition originalFeatureClassDefinition = geodatabase.GetDefinition<FeatureClassDefinition>(featureClassName);
            FeatureClassDescription originalFeatureClassDescription =
              new FeatureClassDescription(originalFeatureClassDefinition);

            // Add domain field on existing fields
            List<FieldDescription> modifiedFieldDescriptions = new List<FieldDescription>(originalFeatureClassDescription.FieldDescriptions) { domainFieldDescription };

            // Create a new description with updated fields for 'Pipes' FeatureClass 
            FeatureClassDescription featureClassDescription =
              new FeatureClassDescription(originalFeatureClassDescription.Name, modifiedFieldDescriptions,
                                             originalFeatureClassDescription.ShapeDescription);

            // Update the 'Pipes' FeatureClass with domain field
            schemaBuilder.Modify(featureClassDescription);

            // Build status
            bool buildStatus = schemaBuilder.Build();

            // Build errors
            if (!buildStatus)
            {
                IReadOnlyList<string> errors = schemaBuilder.ErrorMessages;
            }
        }
        #endregion

        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Modify(ArcGIS.Core.Data.DDL.TableDescription)
        // cref: ArcGIS.Core.Data.TableDefinition.GetFields
        // cref: ArcGIS.Core.Data.DDL.FieldDescription
        // cref: ArcGIS.Core.Data.DDL.TableDescription
        #region Removing fields from a Table
        public void RemoveFieldTableSnippet(Geodatabase geodatabase)
        {
            // Removing all fields from 'Parcels' table except following 
            // Tax_Code
            // Parcel_Address

            // The table to remove fields
            string tableName = "Parcels";

            TableDefinition tableDefinition = geodatabase.GetDefinition<TableDefinition>(tableName);
            IReadOnlyList<Field> fields = tableDefinition.GetFields();

            // Existing fields from 'Parcels' table
            Field taxCodeField = fields.First(f => f.Name.Equals("Tax_Code"));
            Field parcelAddressField = fields.First(f => f.Name.Equals("Parcel_Address"));

            FieldDescription taxFieldDescription = new FieldDescription(taxCodeField);
            FieldDescription parcelAddressFieldDescription = new FieldDescription(parcelAddressField);

            // Fields to retain in modified table
            List<FieldDescription> fieldsToBeRetained = new List<FieldDescription>()
      {
        taxFieldDescription, parcelAddressFieldDescription
      };

            // New description of the 'Parcels' table with the 'Tax_Code' and 'Parcel_Address' fields
            TableDescription modifiedTableDescription = new TableDescription(tableName, fieldsToBeRetained);

            SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);

            // Remove all fields except the 'Tax_Code' and 'Parcel_Address' fields
            schemaBuilder.Modify(modifiedTableDescription);
            schemaBuilder.Build();
        }
        #endregion

        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Create(ArcGIS.Core.Data.DDL.AnnotationFeatureClassDescription)
        // cref: ArcGIS.Core.Data.DDL.AnnotationFeatureClassDescription.#ctor(System.String,IEnumerable{ArcGIS.Core.Data.DDL.FieldDescription},ArcGIS.Core.Data.DDL.ShapeDescription,ARCGIS.CORE.CIM.CIMGENERALPLACEMENTPROPERTIES,IEnumerable{ArcGIS.Core.CIM.CIMLabelClass})
        // cref: ArcGIS.Core.Data.DDL.FieldDescription.CreateGlobalIDField
        // cref: ArcGIS.Core.Data.DDL.FieldDescription.CreateStringField
        // cref: ArcGIS.Core.Data.DDL.ShapeDescription
        // cref: ArcGIS.Core.CIM.CIMGeneralPlacementProperties 
        // cref: ArcGIS.Core.CIM.CIMMaplexGeneralPlacementProperties.AllowBorderOverlap
        // cref: ArcGIS.Core.CIM.CIMMaplexGeneralPlacementProperties.PlacementQuality
        // cref: ArcGIS.Core.CIM.CIMGeneralPlacementProperties.DrawUnplacedLabels
        // cref: ArcGIS.Core.CIM.CIMGeneralPlacementProperties.InvertedLabelTolerance
        // cref: ArcGIS.Core.CIM.CIMGeneralPlacementProperties.RotateLabelWithDisplay
        // cref: ArcGIS.Core.CIM.CIMGeneralPlacementProperties.UnplacedLabelColor
        // cref: ArcGIS.Core.CIM.MaplexQualityType
        // cref: ArcGIS.Core.CIM.CIMLabelClass
        // cref: ArcGIS.Core.CIM.LabelExpressionEngine
        // cref: ArcGIS.Core.CIM.CIMStandardLabelPlacementProperties
        // cref: ArcGIS.Core.CIM.CIMStandardLabelPlacementProperties.AllowOverlappingLabels
        // cref: ArcGIS.Core.CIM.CIMStandardLabelPlacementProperties.LineOffset
        // cref: ArcGIS.Core.CIM.CIMMaplexLabelPlacementProperties
        // cref: ArcGIS.Core.CIM.CIMMaplexLabelPlacementProperties.AlignLabelToLineDirection
        // cref: ArcGIS.Core.CIM.CIMMaplexLabelPlacementProperties.AvoidPolygonHoles
        // cref: ArcGIS.Core.Data.DDL.AnnotationFeatureClassDescription.IsAutoCreate
        // cref: ArcGIS.Core.Data.DDL.AnnotationFeatureClassDescription.IsSymbolIDRequired
        // cref: ArcGIS.Core.Data.DDL.AnnotationFeatureClassDescription.IsUpdatedOnShapeChange
        #region Creating an annotation feature class 
        public void CreateStandAloneAnnotationFeatureClass(Geodatabase geodatabase, SpatialReference spatialReference)
        {
            // Creating a Cities annotation feature class
            // with following user defined fields
            // Name 
            // GlobalID

            // Annotation feature class name
            string annotationFeatureClassName = "CitiesAnnotation";

            // Create user defined attribute fields for annotation feature class 
            FieldDescription globalIDFieldDescription = FieldDescription.CreateGlobalIDField();
            FieldDescription nameFieldDescription = FieldDescription.CreateStringField("Name", 255);

            // Create a list of all field descriptions
            List<FieldDescription> fieldDescriptions = new List<FieldDescription> { globalIDFieldDescription, nameFieldDescription };

            // Create a ShapeDescription object
            ShapeDescription shapeDescription = new ShapeDescription(GeometryType.Polygon, spatialReference);

            // Create general placement properties for Maplex engine 
            CIMMaplexGeneralPlacementProperties generalPlacementProperties =
              new CIMMaplexGeneralPlacementProperties
              {
                  AllowBorderOverlap = true,
                  PlacementQuality = MaplexQualityType.High,
                  DrawUnplacedLabels = true,
                  InvertedLabelTolerance = 1.0,
                  RotateLabelWithDisplay = true,
                  UnplacedLabelColor = new CIMRGBColor
                  {
                      R = 0,
                      G = 255,
                      B = 0,
                      Alpha = 0.5f // Green
                  }
              };

            // Create general placement properties for Standard engine

            //CIMStandardGeneralPlacementProperties generalPlacementProperties =
            //  new CIMStandardGeneralPlacementProperties
            //  {
            //    DrawUnplacedLabels = true,
            //    InvertedLabelTolerance = 3.0,
            //    RotateLabelWithDisplay = true,
            //    UnplacedLabelColor = new CIMRGBColor
            //    {
            //      R = 255, G = 0, B = 0, Alpha = 0.5f // Red
            //    } 
            //   };


            // Create annotation  label classes
            // Green label
            CIMLabelClass greenLabelClass = new CIMLabelClass
            {
                Name = "Green",
                ExpressionTitle = "Expression-Green",
                ExpressionEngine = LabelExpressionEngine.Arcade,
                Expression = "$feature.OBJECTID",
                ID = 1,
                Priority = 0,
                Visibility = true,
                TextSymbol = new CIMSymbolReference
                {
                    Symbol = new CIMTextSymbol()
                    {
                        Angle = 45,
                        FontType = FontType.Type1,
                        FontFamilyName = "Tahoma",
                        FontEffects = FontEffects.Normal,
                        HaloSize = 2.0,

                        Symbol = new CIMPolygonSymbol
                        {
                            SymbolLayers = new CIMSymbolLayer[]
                    {
                new CIMSolidFill
                {
                  Color = CIMColor.CreateRGBColor(0, 255, 0)
                }
                    },
                            UseRealWorldSymbolSizes = true
                        }
                    },
                    MaxScale = 0,
                    MinScale = 0,
                    SymbolName = "TextSymbol-Green"
                },
                StandardLabelPlacementProperties = new CIMStandardLabelPlacementProperties
                {
                    AllowOverlappingLabels = true,
                    LineOffset = 1.0
                },
                MaplexLabelPlacementProperties = new CIMMaplexLabelPlacementProperties
                {
                    AlignLabelToLineDirection = true,
                    AvoidPolygonHoles = true
                }
            };

            // Blue label
            CIMLabelClass blueLabelClass = new CIMLabelClass
            {
                Name = "Blue",
                ExpressionTitle = "Expression-Blue",
                ExpressionEngine = LabelExpressionEngine.Arcade,
                Expression = "$feature.OBJECTID",
                ID = 2,
                Priority = 0,
                Visibility = true,
                TextSymbol = new CIMSymbolReference
                {
                    Symbol = new CIMTextSymbol()
                    {
                        Angle = 45,
                        FontType = FontType.Type1,
                        FontFamilyName = "Consolas",
                        FontEffects = FontEffects.Normal,
                        HaloSize = 2.0,

                        Symbol = new CIMPolygonSymbol
                        {
                            SymbolLayers = new CIMSymbolLayer[]
                    {
                new CIMSolidFill
                {
                  Color = CIMColor.CreateRGBColor(0, 0, 255)
                }
                    },
                            UseRealWorldSymbolSizes = true
                        }
                    },
                    MaxScale = 0,
                    MinScale = 0,
                    SymbolName = "TextSymbol-Blue"
                },
                StandardLabelPlacementProperties = new CIMStandardLabelPlacementProperties
                {
                    AllowOverlappingLabels = true,
                    LineOffset = 1.0
                },
                MaplexLabelPlacementProperties = new CIMMaplexLabelPlacementProperties
                {
                    AlignLabelToLineDirection = true,
                    AvoidPolygonHoles = true
                }
            };

            // Create a list of labels
            List<CIMLabelClass> labelClasses = new List<CIMLabelClass> { greenLabelClass, blueLabelClass };

            // Create an annotation feature class description object to describe the feature class to create
            AnnotationFeatureClassDescription annotationFeatureClassDescription =
              new AnnotationFeatureClassDescription(annotationFeatureClassName, fieldDescriptions, shapeDescription,
                generalPlacementProperties, labelClasses)
              {
                  IsAutoCreate = true,
                  IsSymbolIDRequired = false,
                  IsUpdatedOnShapeChange = true
              };

            // Create a SchemaBuilder object
            SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);

            // Add the creation of the Cities annotation feature class to the list of DDL tasks
            schemaBuilder.Create(annotationFeatureClassDescription);

            // Execute the DDL
            bool success = schemaBuilder.Build();

            // Inspect error messages
            if (!success)
            {
                IReadOnlyList<string> errorMessages = schemaBuilder.ErrorMessages;
                //etc.
            }
        }
        #endregion

        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Create(ArcGIS.Core.Data.DDL.AnnotationFeatureClassDescription)
        // cref: ArcGIS.Core.Data.DDL.AnnotationFeatureClassDescription.#ctor(System.String,IEnumerable{ArcGIS.Core.Data.DDL.FieldDescription},ArcGIS.Core.Data.DDL.ShapeDescription,ARCGIS.CORE.CIM.CIMGENERALPLACEMENTPROPERTIES,IEnumerable{ArcGIS.Core.CIM.CIMLabelClass})
        // cref: ArcGIS.Core.Data.DDL.FieldDescription.CreateGlobalIDField
        // cref: ArcGIS.Core.Data.DDL.FieldDescription.CreateStringField
        // cref: ArcGIS.Core.Data.DDL.ShapeDescription
        // cref: ArcGIS.Core.CIM.CIMMaplexGeneralPlacementProperties
        // cref: ArcGIS.Core.CIM.CIMMaplexGeneralPlacementProperties.AllowBorderOverlap
        // cref: ArcGIS.Core.CIM.CIMMaplexGeneralPlacementProperties.PlacementQuality
        // cref: ArcGIS.Core.CIM.CIMGeneralPlacementProperties.DrawUnplacedLabels
        // cref: ArcGIS.Core.CIM.CIMGeneralPlacementProperties.InvertedLabelTolerance
        // cref: ArcGIS.Core.CIM.CIMGeneralPlacementProperties.RotateLabelWithDisplay
        // cref: ArcGIS.Core.CIM.CIMGeneralPlacementProperties.UnplacedLabelColor
        // cref: ArcGIS.Core.CIM.MaplexQualityType
        // cref: ArcGIS.Core.CIM.CIMLabelClass
        // cref: ArcGIS.Core.CIM.LabelExpressionEngine
        // cref: ArcGIS.Core.CIM.CIMStandardLabelPlacementProperties
        // cref: ArcGIS.Core.CIM.CIMStandardLabelPlacementProperties.AllowOverlappingLabels
        // cref: ArcGIS.Core.CIM.CIMStandardLabelPlacementProperties.LineOffset
        // cref: ArcGIS.Core.CIM.CIMMaplexLabelPlacementProperties
        // cref: ArcGIS.Core.CIM.CIMMaplexLabelPlacementProperties.AlignLabelToLineDirection
        // cref: ArcGIS.Core.CIM.CIMMaplexLabelPlacementProperties.AvoidPolygonHoles
        // cref: ArcGIS.Core.Data.DDL.AnnotationFeatureClassDescription.IsAutoCreate
        // cref: ArcGIS.Core.Data.DDL.AnnotationFeatureClassDescription.IsSymbolIDRequired
        // cref: ArcGIS.Core.Data.DDL.AnnotationFeatureClassDescription.IsUpdatedOnShapeChange
        #region Creating a feature-linked annotation feature class 
        public void CreateFeatureLinkedAnnotationFeatureClass(Geodatabase geodatabase, SpatialReference spatialReference)
        {
            // Creating a feature-linked annotation feature class between water pipe and valve in water distribution network
            // with following user defined fields
            // PipeName 
            // GlobalID

            // Annotation feature class name
            string annotationFeatureClassName = "WaterPipeAnnotation";

            // Create user defined attribute fields for annotation feature class
            FieldDescription pipeGlobalID = FieldDescription.CreateGlobalIDField();
            FieldDescription nameFieldDescription = FieldDescription.CreateStringField("Name", 255);

            // Create a list of all field descriptions
            List<FieldDescription> fieldDescriptions = new List<FieldDescription> { pipeGlobalID, nameFieldDescription };

            // Create a ShapeDescription object
            ShapeDescription shapeDescription = new ShapeDescription(GeometryType.Polygon, spatialReference);

            // Create general placement properties for Maplex engine 
            CIMMaplexGeneralPlacementProperties generalPlacementProperties = new CIMMaplexGeneralPlacementProperties
            {
                AllowBorderOverlap = true,
                PlacementQuality = MaplexQualityType.High,
                DrawUnplacedLabels = true,
                InvertedLabelTolerance = 1.0,
                RotateLabelWithDisplay = true,
                UnplacedLabelColor = new CIMRGBColor
                {
                    R = 255,
                    G = 0,
                    B = 0,
                    Alpha = 0.5f
                }
            };

            // Create annotation  label classes
            // Green label
            CIMLabelClass greenLabelClass = new CIMLabelClass
            {
                Name = "Green",
                ExpressionTitle = "Expression-Green",
                ExpressionEngine = LabelExpressionEngine.Arcade,
                Expression = "$feature.OBJECTID",
                ID = 1,
                Priority = 0,
                Visibility = true,
                TextSymbol = new CIMSymbolReference
                {
                    Symbol = new CIMTextSymbol()
                    {
                        Angle = 45,
                        FontType = FontType.Type1,
                        FontFamilyName = "Tahoma",
                        FontEffects = FontEffects.Normal,
                        HaloSize = 2.0,

                        Symbol = new CIMPolygonSymbol
                        {
                            SymbolLayers = new CIMSymbolLayer[]
                    {
                new CIMSolidFill
                {
                  Color = CIMColor.CreateRGBColor(0, 255, 0)
                }
                    },
                            UseRealWorldSymbolSizes = true
                        }
                    },
                    MaxScale = 0,
                    MinScale = 0,
                    SymbolName = "TextSymbol-Green"
                },
                StandardLabelPlacementProperties = new CIMStandardLabelPlacementProperties
                {
                    AllowOverlappingLabels = true,
                    LineOffset = 1.0
                },
                MaplexLabelPlacementProperties = new CIMMaplexLabelPlacementProperties
                {
                    AlignLabelToLineDirection = true,
                    AvoidPolygonHoles = true
                }
            };

            // Blue label
            CIMLabelClass blueLabelClass = new CIMLabelClass
            {
                Name = "Blue",
                ExpressionTitle = "Expression-Blue",
                ExpressionEngine = LabelExpressionEngine.Arcade,
                Expression = "$feature.OBJECTID",
                ID = 2,
                Priority = 0,
                Visibility = true,
                TextSymbol = new CIMSymbolReference
                {
                    Symbol = new CIMTextSymbol()
                    {
                        Angle = 45,
                        FontType = FontType.Type1,
                        FontFamilyName = "Consolas",
                        FontEffects = FontEffects.Normal,
                        HaloSize = 2.0,

                        Symbol = new CIMPolygonSymbol
                        {
                            SymbolLayers = new CIMSymbolLayer[]
                    {
                new CIMSolidFill
                {
                  Color = CIMColor.CreateRGBColor(0, 0, 255)
                }
                    },
                            UseRealWorldSymbolSizes = true
                        }
                    },
                    MaxScale = 0,
                    MinScale = 0,
                    SymbolName = "TextSymbol-Blue"
                },
                StandardLabelPlacementProperties = new CIMStandardLabelPlacementProperties
                {
                    AllowOverlappingLabels = true,
                    LineOffset = 1.0
                },
                MaplexLabelPlacementProperties = new CIMMaplexLabelPlacementProperties
                {
                    AlignLabelToLineDirection = true,
                    AvoidPolygonHoles = true
                }
            };

            // Create a list of labels
            List<CIMLabelClass> labelClasses = new List<CIMLabelClass> { greenLabelClass, blueLabelClass };


            // Create linked feature description
            // Linked feature class name
            string linkedFeatureClassName = "WaterPipe";

            // Create fields for water pipe
            FieldDescription waterPipeGlobalID = FieldDescription.CreateGlobalIDField();
            FieldDescription pipeName = FieldDescription.CreateStringField("PipeName", 255);

            // Create a list of water pipe field descriptions
            List<FieldDescription> pipeFieldDescriptions = new List<FieldDescription> { waterPipeGlobalID, pipeName };

            // Create a linked feature class description
            FeatureClassDescription linkedFeatureClassDescription = new FeatureClassDescription(linkedFeatureClassName, pipeFieldDescriptions,
              new ShapeDescription(GeometryType.Polyline, spatialReference));

            // Create a SchemaBuilder object
            SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);

            // Add the creation of the linked feature class to the list of DDL tasks
            FeatureClassToken linkedFeatureClassToken = schemaBuilder.Create(linkedFeatureClassDescription);

            // Create an annotation feature class description object to describe the feature class to create
            AnnotationFeatureClassDescription annotationFeatureClassDescription =
              new AnnotationFeatureClassDescription(annotationFeatureClassName, fieldDescriptions, shapeDescription,
                generalPlacementProperties, labelClasses, new FeatureClassDescription(linkedFeatureClassToken))
              {
                  IsAutoCreate = true,
                  IsSymbolIDRequired = false,
                  IsUpdatedOnShapeChange = true
              };

            // Add the creation of the annotation feature class to the list of DDL tasks
            schemaBuilder.Create(annotationFeatureClassDescription);

            // Execute the DDL
            bool success = schemaBuilder.Build();

            // Inspect error messages
            if (!success)
            {
                IReadOnlyList<string> errorMessages = schemaBuilder.ErrorMessages;
                //etc.
            }
        }
        #endregion

        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Create(ArcGIS.Core.Data.DDL.FeatureDatasetDescription,ArcGIS.Core.Data.DDL.AnnotationFeatureClassDescription)
        // cref: ArcGIS.Core.Data.DDL.AnnotationFeatureClassDescription.#ctor(System.String, ArcGIS.Core.Data.Mapping.AnnotationFeatureClassDefinition)
        // cref: ArcGIS.Core.Data.DDL.FeatureDatasetDescription
        // cref: ArcGIS.Core.Data.DDL.AnnotationFeatureClassDescription
        // cref: ArcGIS.Core.Data.DDL.AnnotationFeatureClassDescription.IsAutoCreate
        // cref: ArcGIS.Core.Data.DDL.AnnotationFeatureClassDescription.IsSymbolIDRequired
        // cref: ArcGIS.Core.Data.DDL.AnnotationFeatureClassDescription.IsUpdatedOnShapeChange
        #region Creating an annotation feature class inside feature dataset
        public void CreateAnnotationFeatureClassUsingExistingAnnotationFeatureClassInDataset(Geodatabase geodatabase)
        {
            // Create a Cities annotation feature class inside Places feature dataset using existing annotation feature class 

            // Feature dataset name
            string featureDatasetName = "Places";

            // Annotation feature class name
            string annotationFeatureClassName = "CitiesAnnotation";

            // Create a SchemaBuilder object
            SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);

            // Open existing annotation feature class name
            using (AnnotationFeatureClass existingAnnotationFeatureClass = geodatabase.OpenDataset<AnnotationFeatureClass>("ExistingAnnotationFeatureClass"))
            {

                // Create Feature dataset description
                FeatureDatasetDescription featureDatasetDescription =
                  new FeatureDatasetDescription(featureDatasetName, existingAnnotationFeatureClass.GetDefinition().GetSpatialReference());

                // Add the creation of the Places dataset to DDL task
                FeatureDatasetToken featureDatasetToken = schemaBuilder.Create(featureDatasetDescription);

                // Create an annotation feature class description using existing annotation feature class
                AnnotationFeatureClassDescription annotationFeatureClassDescription = new AnnotationFeatureClassDescription(annotationFeatureClassName, existingAnnotationFeatureClass.GetDefinition())
                {
                    IsAutoCreate = true,
                    IsSymbolIDRequired = false,
                    IsUpdatedOnShapeChange = true
                };

                // Add the creation of the Cities annotation feature class inside Places feature dataset
                schemaBuilder.Create(new FeatureDatasetDescription(featureDatasetToken), annotationFeatureClassDescription);

                // Execute the DDL
                bool success = schemaBuilder.Build();

                // Inspect error messages
                if (!success)
                {
                    IReadOnlyList<string> errorMessages = schemaBuilder.ErrorMessages;
                    //etc.
                }
            }
        }
        #endregion

        // cref: ArcGIS.Core.Data.MemoryConnectionProperties.#ctor
        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.CreateGeodatabase(ArcGIS.Core.Data.MemoryConnectionProperties)
        #region Creating the memory geodatabase
        public Geodatabase GetMemoryGeodatabase()
        {
            // Creates the default memory geodatabase if not exist or connects to an existing one if already exists
            Geodatabase memoryGeodatabase = new Geodatabase(new MemoryConnectionProperties());

            // Creates schema
            SchemaBuilder schemaBuilder = new SchemaBuilder(memoryGeodatabase);
            schemaBuilder.Create(new TableDescription("MyTable", new List<FieldDescription>()));
            schemaBuilder.Build();

            return memoryGeodatabase;
        }
        #endregion

        // cref: ArcGIS.Core.Data.SPATIALQUERYFILTER.SPATIALRELATIONSHIP
        // cref: ArcGIS.Core.Data.SpatialQueryFilter.SpatialRelationshipDescription
        #region Spatial query filter with DE9-IM spatial relationships 
        public void FindSpatiallyRelatedFeaturesUsingDE9IMPredicate(Geodatabase geodatabase, FeatureClass polygonFeatureClass, FeatureClass polylineFeatureClass)
        {
            using (RowCursor polygonRowCursor = polygonFeatureClass.Search(new QueryFilter()))
            {
                if (polygonRowCursor.MoveNext())
                {
                    using (Feature polygonFeature = polygonRowCursor.Current as Feature)
                    {
                        // DE9IM predicate string to find overlapping features
                        string overlappingDE9IM = "1*T***T**";

                        SpatialQueryFilter spatialQueryFilter = new SpatialQueryFilter()
                        {
                            FilterGeometry = polygonFeature.GetShape(),
                            SpatialRelationship = SpatialRelationship.Relation,
                            SpatialRelationshipDescription = overlappingDE9IM
                        };

                        using (RowCursor overlappingPolyline = polylineFeatureClass.Search(spatialQueryFilter))
                        {
                            while (overlappingPolyline.MoveNext())
                            {
                                // Overlapping polylines on the polygon
                            }
                        }
                    }
                }
            }
        }
        #endregion

        // cref: ArcGIS.Core.Data.GEODATABASE.OpenDataset``1(System.String)
        // cref: ArcGIS.Core.Data.REGISTRATIONTYPE
        #region Check if table is versioned
        public bool IsTableVersioned(Geodatabase geodatabase, string tableName)
        {
            using (Table table = geodatabase.OpenDataset<Table>(tableName))
            {
                // Check table version type
                RegistrationType registrationType = table.GetRegistrationType();
                if (registrationType == RegistrationType.Versioned)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        // cref: ArcGIS.Core.Data.DDL.AttributeIndexDescription.#ctor(System.String,ArcGIS.Core.Data.DDL.TableDescription,System.Collections.Generic.IEnumerable{System.String})
        // cref: ArcGIS.Core.Data.DDL.AttributeIndexDescription.#ctor(Index,TableDescription)
        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Create(IndexDescription)
        #region Creating a table with index from scratch
        public void CreatingTableWithIndex(SchemaBuilder schemaBuilder)
        {
            FieldDescription nameFieldDescription = FieldDescription.CreateStringField("Name", 50);
            FieldDescription addressFieldDescription = FieldDescription.CreateStringField("Address", 200);

            // Creating a feature class, 'Buildings' with two fields
            TableDescription tableDescription = new TableDescription("Buildings", new List<FieldDescription>() { nameFieldDescription, addressFieldDescription });

            // Enqueue DDL operation to create a table
            TableToken tableToken = schemaBuilder.Create(tableDescription);

            // Creating an attribute index named as 'Idx'
            AttributeIndexDescription attributeIndexDescription = new AttributeIndexDescription("Idx", new TableDescription(tableToken),
              new List<string> { nameFieldDescription.Name, addressFieldDescription.Name });

            // Enqueue DDL operation to create an attribute index
            schemaBuilder.Create(attributeIndexDescription);

            // Execute build indexes operation
            bool isBuildSuccess = schemaBuilder.Build();
        }
        #endregion

        // cref: ArcGIS.Core.Data.DDL.AttributeIndexDescription.#ctor(String,TableDescription,IEnumerable{String})
        // cref: ArcGIS.Core.Data.DDL.SpatialIndexDescription.#ctor(ArcGIS.Core.Data.DDL.FeatureClassDescription)
        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Create(IndexDescription)
        #region Adding indexes in pre-existing dataset
        public void AddingIndexes(SchemaBuilder schemaBuilder, FeatureClassDefinition featureClassDefinition)
        {
            // Field names to add in the attribute index
            string fieldName = featureClassDefinition.GetFields().First(f => f.AliasName.Contains("Name")).Name;
            string fieldAddress = featureClassDefinition.GetFields().First(f => f.AliasName.Contains("Address")).Name;

            // Creating an attribute index with index name 'Idx' and two participating fields' name
            AttributeIndexDescription attributeIndexDescription = new AttributeIndexDescription("Idx", new TableDescription(featureClassDefinition), new List<string> { fieldName, fieldAddress });

            // Enqueue DDL operation for an attribute index creation 
            schemaBuilder.Create(attributeIndexDescription);

            // Creating the spatial index 
            SpatialIndexDescription spatialIndexDescription = new SpatialIndexDescription(new FeatureClassDescription(featureClassDefinition));

            // Enqueue DDL operation for the spatial index creation
            schemaBuilder.Create(spatialIndexDescription);

            // Execute build indexes operation
            bool isBuildSuccess = schemaBuilder.Build();

            if (!isBuildSuccess)
            {
                IReadOnlyList<string> errors = schemaBuilder.ErrorMessages;
                // Iterate and handle errors 
            }
        }
        #endregion

        // cref: ArcGIS.Core.Data.DDL.AttributeIndexDescription.#ctor(Index,TableDescription)
        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Delete
        #region Removing attribute index
        public void RemoveAttributeIndex(SchemaBuilder schemaBuilder, FeatureClassDefinition featureClassDefinition, string attributeIndexName)
        {
            // Find a index to be removed 
            ArcGIS.Core.Data.Index indexToRemove = featureClassDefinition.GetIndexes().First(f => f.GetName().Equals(attributeIndexName));

            // Index description of the index to be removed 
            AttributeIndexDescription indexDescriptionToRemove = new AttributeIndexDescription(indexToRemove, new TableDescription(featureClassDefinition));

            // Enqueue the DDL operation to remove index 
            schemaBuilder.Delete(indexDescriptionToRemove);

            // Execute the delete index operation
            bool isDeleteIndexSuccess = schemaBuilder.Build();
        }
        #endregion

        // cref: ArcGIS.Core.Data.DDL.SpatialIndexDescription.#ctor
        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Delete
        #region Removing spatial index
        public void RemoveSpatialIndex(SchemaBuilder schemaBuilder, FeatureClassDefinition featureClassDefinition)
        {
            // Create a spatial description  
            SpatialIndexDescription spatialIndexDescription = new SpatialIndexDescription(new FeatureClassDescription(featureClassDefinition));

            // Enqueue the DDL operation to remove index 
            schemaBuilder.Delete(spatialIndexDescription);

            // Execute the delete index operation
            bool isDeleteIndexSuccess = schemaBuilder.Build();
        }
        #endregion

        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Modify(CodedValueDomainDescription,SortBy,SortOrder)
        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Modify(DomainDescription)
        #region Modifying domain
        public void ModifyDomain(Geodatabase geodatabase, string codedValueDomainName = "Pipe")
        {
            SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);
            CodedValueDomain codedValueDomain = geodatabase.GetDomains().First(f => f.GetName().Equals(codedValueDomainName)) as CodedValueDomain;
            CodedValueDomainDescription codedValueDomainDescription = new CodedValueDomainDescription(codedValueDomain);

            // Update domain description
            codedValueDomainDescription.Description = "Water Pipe Domain";

            // Adding code/value pair
            codedValueDomainDescription.CodedValuePairs.Add("C", "Copper");

            schemaBuilder.Modify(codedValueDomainDescription);

            // To modify the orders of coded value domain
            // schemaBuilder.Modify(codedValueDomainDescription,SortBy.Name,SortOrder.Ascending);

            schemaBuilder.Build();
        }
        #endregion

        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Rename
        #region Rename domain
        public void RenameDomain(Geodatabase geodatabase, string rangeDomainOldName = "PipeDiameter", string rangeDomainNewName = "PipeDiam")
        {
            SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);
            RangeDomain rangeDomain = geodatabase.GetDomains().First(f => f.GetName().Equals(rangeDomainOldName)) as RangeDomain;

            // Renaming a domain
            schemaBuilder.Rename(new RangeDomainDescription(rangeDomain), rangeDomainNewName);
            schemaBuilder.Build();
        }
        #endregion

        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Delete
        #region Delete domain
        public void DeleteDomain(Geodatabase geodatabase, string domainNameToBeDeleted = "PipeMaterial")
        {
            SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);
            CodedValueDomain codedValueDomain = geodatabase.GetDomains().First(f => f.GetName().Equals(domainNameToBeDeleted)) as CodedValueDomain;
            CodedValueDomainDescription codedValueDomainDescription = new CodedValueDomainDescription(codedValueDomain);

            // Deleting a coded value domain
            schemaBuilder.Delete(codedValueDomainDescription);
            schemaBuilder.Build();
        }
        #endregion

        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Create(TableDescription)
        // cref: ArcGIS.Core.Data.DDL.SubtypeFieldDescription.#ctor
        #region Creating table with subtypes
        public void CreateTableWithSubtypes(SchemaBuilder schemaBuilder)
        {
            // Creating a 'Building' table with the subtype field 'BuildingType'
            FieldDescription buildingType = new FieldDescription("BuildingType", FieldType.Integer);
            FieldDescription buildingName = new FieldDescription("Name", FieldType.String);

            TableDescription tableDescription = new TableDescription("Building", new List<FieldDescription> { buildingName, buildingType });

            // Add the building type subtype with three subtypes - Business, Marketing, Security
            tableDescription.SubtypeFieldDescription = new SubtypeFieldDescription(buildingType.Name, new Dictionary<int, string> { { 1, "Business" }, { 2, "Marketing" }, { 3, "Security" } })
            {
                DefaultSubtypeCode = 3 // Assigning 'Security' building type as the default subtype
            };

            schemaBuilder.Create(tableDescription);
            schemaBuilder.Build();
        }
        #endregion

        // cref: ArcGIS.Core.Data.DDL.TableDescription.SubtypeFieldDescription
        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Modify(TableDescription)
        #region Removing subtype field designation
        public void DeleteSubtypeField(SchemaBuilder schemaBuilder, FeatureClassDefinition featureClassDefinition)
        {
            FeatureClassDescription featureClassDescription = new FeatureClassDescription(featureClassDefinition);

            // Set subtype field to null to remove the subtype field designation 
            featureClassDescription.SubtypeFieldDescription = null;

            schemaBuilder.Modify(featureClassDescription);
            schemaBuilder.Build();
        }
        #endregion

        // cref: ArcGIS.Core.Data.DDL.TableDescription.SubtypeFieldDescription
        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Modify(TableDescription)
        #region Modifying subtypes
        public void ModifySubtypes(SchemaBuilder schemaBuilder, TableDefinition tableDefinition)
        {
            TableDescription tableDescription = new TableDescription(tableDefinition);

            // Remove the first subtype from the table
            IReadOnlyList<Subtype> subtypes = tableDefinition.GetSubtypes();
            tableDescription.SubtypeFieldDescription.Subtypes.Remove(subtypes.First().GetCode());

            // Adding a new subtype, 'Utility', in the existing table
            tableDescription.SubtypeFieldDescription.Subtypes.Add(4, "Utility");

            // Assigning 'Utility' subtype as the default subtype
            tableDescription.SubtypeFieldDescription.DefaultSubtypeCode = 4;

            schemaBuilder.Modify(tableDescription);
            schemaBuilder.Build();
        }
        #endregion

        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Create(RelationshipClassDescription)
        // cref: ArcGIS.Core.Data.DDL.RelationshipClassDescription.#ctor
        // cref: ArcGIS.Core.Data.DDL.SubtypeFieldDescription.#ctor
        #region Creating relationship class
        public void CreateRelationshipWithRelationshipRules(SchemaBuilder schemaBuilder)
        {
            // Creating a 'BuildingType' table with two fields - BuildingType and BuildingTypeDescription
            FieldDescription buildingType = FieldDescription.CreateIntegerField("BuildingType");
            FieldDescription buildingTypeeDescription = FieldDescription.CreateStringField("BuildingTypeDescription", 100);
            TableDescription buildingTypeDescription = new TableDescription("BuildingType", new List<FieldDescription>() { buildingType, buildingTypeeDescription });
            TableToken buildingtypeToken = schemaBuilder.Create(buildingTypeDescription);

            // Creating a 'Building' feature class with three fields - BuildingId, Address, and BuildingType
            FieldDescription buildingId = FieldDescription.CreateIntegerField("BuildingId");
            FieldDescription buildingAddress = FieldDescription.CreateStringField("Address", 100);
            FieldDescription usageSubType = FieldDescription.CreateIntegerField("UsageSubtype");
            FeatureClassDescription featureClassDescription = new FeatureClassDescription("Building", new List<FieldDescription> { buildingId, buildingAddress, buildingType, usageSubType }, new ShapeDescription(GeometryType.Polygon, SpatialReferences.WGS84));

            // Set subtype details (optional)
            featureClassDescription.SubtypeFieldDescription = new SubtypeFieldDescription(usageSubType.Name, new Dictionary<int, string> { { 1, "Marketing" }, { 2, "Utility" } });

            FeatureClassToken buildingToken = schemaBuilder.Create(featureClassDescription);

            // Creating a 1:M relationship between the 'Building' feature class and 'BuildingType' table
            RelationshipClassDescription relationshipClassDescription = new RelationshipClassDescription("BuildingToBuildingType", new FeatureClassDescription(buildingToken), new TableDescription(buildingtypeToken),
              RelationshipCardinality.OneToMany, buildingType.Name, buildingType.Name)
            {
                RelationshipType = RelationshipType.Composite
            };

            // Adding relationship rules for the 'Marketing' subtype
            relationshipClassDescription.RelationshipRuleDescriptions.Add(new RelationshipRuleDescription(1, null));

            schemaBuilder.Create(relationshipClassDescription);
            schemaBuilder.Build();
        }
        #endregion

        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Create(AttributedRelationshipClassDescription)
        #region Creating attributed relationship class
        public void CreateAttributedRelationship(SchemaBuilder schemaBuilder)
        {
            // Creating a 'BuildingType' table with two fields - BuildingType and BuildingTypeDescription
            FieldDescription buildingType = FieldDescription.CreateIntegerField("BuildingType");
            FieldDescription buildingTypeeDescription = FieldDescription.CreateStringField("BuildingTypeDescription", 100);
            TableDescription buildingTypeDescription = new TableDescription("BuildingType", new List<FieldDescription>() { buildingType, buildingTypeeDescription });
            TableToken buildingtypeToken = schemaBuilder.Create(buildingTypeDescription);

            // Creating a 'Building' feature class with three fields - BuildingId, Address, and BuildingType
            FieldDescription buildingId = FieldDescription.CreateIntegerField("BuildingId");
            FieldDescription buildingAddress = FieldDescription.CreateStringField("Address", 100);
            FeatureClassDescription featureClassDescription = new FeatureClassDescription("Building", new List<FieldDescription> { buildingId, buildingAddress, buildingType }, new ShapeDescription(GeometryType.Polygon, SpatialReferences.WGS84));
            FeatureClassToken buildingToken = schemaBuilder.Create(featureClassDescription);

            // Creating M:M relationship between the 'Building' feature class and 'BuildingType' table
            AttributedRelationshipClassDescription attributedRelationshipClassDescription = new AttributedRelationshipClassDescription("BuildingToBuildingType", new FeatureClassDescription(buildingToken),
                new TableDescription(buildingtypeToken), RelationshipCardinality.ManyToMany, "OBJECTID", "BuildingID", "OBJECTID", "BuildingTypeID");

            // Adding optional attribute field in the intermediate table - 'OwnershipPercentage' field
            attributedRelationshipClassDescription.FieldDescriptions.Add(FieldDescription.CreateIntegerField("OwnershipPercentage"));

            schemaBuilder.Create(attributedRelationshipClassDescription);
            schemaBuilder.Build();
        }
        #endregion

        // cref: ArcGIS.Core.Data.DDL.RelationshipClassDescription.RelationshipRuleDescriptions
        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Modify(AttributedRelationshipClassDescription)
        #region Add relationship rules to a relationship class
        public void ModifyRelationshipClass(SchemaBuilder schemaBuilder, AttributedRelationshipClassDefinition attributedRelationshipClassDefinition)
        {
            AttributedRelationshipClassDescription attributedRelationshipClassDescription = new AttributedRelationshipClassDescription(attributedRelationshipClassDefinition);

            // Update the relationship split policy
            attributedRelationshipClassDescription.RelationshipSplitPolicy = RelationshipSplitPolicy.UseDefault;

            // Add field in the intermediate table
            attributedRelationshipClassDescription.FieldDescriptions.Add(FieldDescription.CreateIntegerField("RelationshipStatus"));

            // Add relationship rules based on subtypes,if available
            // Assuming origin class has subtype with code 1
            attributedRelationshipClassDescription.RelationshipRuleDescriptions.Add(new RelationshipRuleDescription(1, null));

            // Enqueue modify operation
            schemaBuilder.Modify(attributedRelationshipClassDescription);

            // Execute modify DDL operation
            schemaBuilder.Build();
        }
        #endregion

        // cref:ArcGIS.Core.Data.DDL.SchemaBuilder.Delete
        #region Deleting a relationship class
        public void DeleteRelationshipClass(SchemaBuilder schemaBuilder, RelationshipClassDefinition relationshipClassDefinition)
        {
            schemaBuilder.Delete(new RelationshipClassDescription(relationshipClassDefinition));
            schemaBuilder.Build();
        }
        #endregion

        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.AddRelationshipClass(FeatureDatasetDescription,RelationshipClassDescription)
        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.RemoveRelationshipClass(FeatureDatasetDescription,RelationshipClassDescription)
        #region Adding/Removing Relationship class in/out of a feature dataset
        public void MoveRelationshipClass(SchemaBuilder schemaBuilder, FeatureDatasetDefinition featureDatasetDefinition, RelationshipClassDefinition relationshipClassDefinition)
        {
            FeatureDatasetDescription featureDatasetDescription = new FeatureDatasetDescription(featureDatasetDefinition);
            RelationshipClassDescription relationshipClassDescription = new RelationshipClassDescription(relationshipClassDefinition);

            // Remove relationship class from the feature dataset
            schemaBuilder.RemoveRelationshipClass(featureDatasetDescription, relationshipClassDescription);

            // Add relationship class inside the feature dataset
            // schemaBuilder.AddRelationshipClass(featureDatasetDescription, relationshipClassDescription);

            schemaBuilder.Build();
        }
        #endregion

        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Modify(AttributedRelationshipClassDescription)
        #region Modifying annotation labels and symbols
        public void ModifyAnnotationLabelAndSymbols(SchemaBuilder schemaBuilder, AnnotationFeatureClassDefinition annotationFeatureClassDefinition)
        {
            AnnotationFeatureClassDescription annotationFeatureClassDescription = new AnnotationFeatureClassDescription(annotationFeatureClassDefinition);
            IReadOnlyList<CIMLabelClass> labelClasses = annotationFeatureClassDescription.LabelClasses;

            // Adding a new annotation label class 
            List<CIMLabelClass> modifiedLabelClasses = new List<CIMLabelClass>(labelClasses);
            modifiedLabelClasses.Add(new CIMLabelClass()
            {
                Name = "RedSymbol",
                TextSymbol = new CIMSymbolReference
                {
                    Symbol = new CIMTextSymbol()
                    {
                        Angle = 45,
                        FontType = FontType.Type1,
                        FontFamilyName = "Arial",
                        FontEffects = FontEffects.Normal,
                        HaloSize = 2.0,

                        Symbol = new CIMPolygonSymbol { SymbolLayers = new CIMSymbolLayer[] { new CIMSolidFill { Color = CIMColor.CreateRGBColor(255, 0, 0) } }, UseRealWorldSymbolSizes = true }
                    },
                    MaxScale = 0,
                    MinScale = 0,
                    SymbolName = "TextSymbol-RED"
                },
            });

            // Adding a  new symbol
            annotationFeatureClassDescription.Symbols.Add(new CIMSymbolIdentifier()
            {
                ID = 1001,
                Name = "ID_10001",
                Symbol = new CIMTextSymbol()
                {
                    Angle = 43,
                    FontEffects = FontEffects.Subscript,
                    FontType = FontType.TTOpenType,
                    FontStyleName = "Regular",
                    FontFamilyName = "Tahoma",
                    TextCase = TextCase.Allcaps
                }
            });

            // Modify annotation feature class 
            AnnotationFeatureClassDescription modifiedAnnotationFeatureClassDescription = new AnnotationFeatureClassDescription(annotationFeatureClassDescription.Name, annotationFeatureClassDescription.FieldDescriptions, annotationFeatureClassDescription.ShapeDescription, annotationFeatureClassDescription.GeneralPlacementProperties, modifiedLabelClasses);

            // Enqueue modify
            schemaBuilder.Modify(modifiedAnnotationFeatureClassDescription);

            // DDL execute
            schemaBuilder.Build();

        }
        #endregion

        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Rename
        #region Rename annotation feature class
        public void Rename(SchemaBuilder schemaBuilder, AnnotationFeatureClassDefinition annotationFeatureClassDefinition, string featureClassNewName)
        {
            AnnotationFeatureClassDescription annotationFeatureClassDescription = new AnnotationFeatureClassDescription(annotationFeatureClassDefinition);

            // Enqueue rename operation
            schemaBuilder.Rename(annotationFeatureClassDescription, featureClassNewName);

            // Execute DDL
            schemaBuilder.Build();
        }
        #endregion

        // cref: ArcGIS.Core.Data.DDL.SchemaBuilder.Modify(TableDescription,String,FieldDescription)
        // cref: ArcGIS.Core.Data.DDL.FieldDescription.SetDefaultValue
        #region Modifying an existing field
        private void ModifyExistingField(SchemaBuilder schemaBuilder, TableDefinition tableDefinition, string fieldNameToBeModified = "PropertyAddress")
        {
            Field field = tableDefinition.GetFields().FirstOrDefault(f => f.Name.Contains(fieldNameToBeModified));

            // Update field's alias name and length
            FieldDescription fieldDescription = new FieldDescription(field)
            {
                AliasName = "Physical Property Address",
                Length = 50
            };

            // Update the default value
            fieldDescription.SetDefaultValue("123 Main St");

            // Enqueue modify operation
            schemaBuilder.Modify(new TableDescription(tableDefinition), field.Name, fieldDescription);

            // Execute DDL
            schemaBuilder.Build();
        }
        #endregion
        
    }
}
