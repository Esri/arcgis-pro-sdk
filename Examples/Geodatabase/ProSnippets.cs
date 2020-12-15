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
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Mapping;
using Version = ArcGIS.Core.Data.Version;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Core.Internal.Data.DDL;
using FieldDescription = ArcGIS.Core.Internal.Data.DDL.FieldDescription;


namespace GeodatabaseSDK.GeodatabaseSDK.Snippets
{

  class Snippets
  {
    #region ProSnippet Group: Geodatabases and Datastores
    #endregion

    #region Opening a File Geodatabase given the path

    public async Task OpenFileGDB()
    {
      try {
        await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => {
          // Opens a file geodatabase. This will open the geodatabase if the folder exists and contains a valid geodatabase.
          using (
            Geodatabase geodatabase =
              new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(@"C:\Data\LocalGovernment.gdb")))) {
            // Use the geodatabase.
          }
        });
      }
      catch (GeodatabaseNotFoundOrOpenedException exception) {
        // Handle Exception.
      }

    }

    #endregion Opening a File Geodatabase given the path

    #region Opening an Enterprise Geodatabase using connection properties

    public async Task OpenEnterpriseGeodatabase()
    {
      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => {
        // Opening a Non-Versioned SQL Server instance.
        DatabaseConnectionProperties connectionProperties = new DatabaseConnectionProperties(EnterpriseDatabaseType.SQLServer) {
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

        using (Geodatabase geodatabase = new Geodatabase(connectionProperties)) {
          // Use the geodatabase
        }
      });
    }

    #endregion Opening an Enterprise Geodatabase using connection properties

    #region Opening an Enterprise Geodatabase using sde file path

    public async Task OpenEnterpriseGeodatabaseUsingSDEFilePath()
    {
      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => {
        using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde")))) {
          // Use the geodatabase.
        }
      });
    }

    #endregion Opening an Enterprise Geodatabase using sde file path

    #region Obtaining Geodatabase from Project Item

    public async Task ObtainingGeodatabaseFromProjectItem()
    {
      IEnumerable<GDBProjectItem> gdbProjectItems = Project.Current.GetItems<GDBProjectItem>();

      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => {
        foreach (GDBProjectItem gdbProjectItem in gdbProjectItems) {
          using (Datastore datastore = gdbProjectItem.GetDatastore()) {
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

    public void GettingConnectionProperties()
    {
      #region Getting Database Connection Properties from a Connection File

      DatabaseConnectionFile connectionFile = new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"));
      DatabaseConnectionProperties connectionProperties = DatabaseClient.GetDatabaseConnectionProperties(connectionFile);

      // Now you could, for example, change the user name and password in the connection properties prior to use them to open a geodatabase

      #endregion
    }

    #region Obtaining Geodatabase from FeatureLayer

    public async Task ObtainingGeodatabaseFromFeatureLayer()
    {
      IEnumerable<Layer> layers = MapView.Active.Map.Layers.Where(layer => layer is FeatureLayer);

      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => {
        foreach (FeatureLayer featureLayer in layers) {
          using (Table table = featureLayer.GetTable())
          using (Datastore datastore = table.GetDatastore()) {
            if (datastore is UnknownDatastore)
              continue;

            Geodatabase geodatabase = datastore as Geodatabase;
          }
        }
      });
    }

    #endregion Obtaining Geodatabase from FeatureLayer

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

    #region Obtaining Definition from Geodatabase

    public async Task ObtainingDefinitionFromGeodatabase()
    {
      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => {
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

    #region Obtaining List of Defintions from Geodatabase

    public async Task ObtainingDefinitionsFromGeodatabase()
    {
      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => {
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

    #region Obtaining Related Definitions from Geodatabase
    public async Task ObtainingRelatedDefinitionsFromGeodatabase()
    {
      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => {
        using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
        {
          // Remember the qualification of DatabaseName. for the RelationshipClass.

          RelationshipClassDefinition enterpriseDefinition = geodatabase.GetDefinition<RelationshipClassDefinition>("LocalGovernment.GDB.AddressPointHasSiteAddresses");
          IReadOnlyList<Definition> enterpriseDefinitions = geodatabase.GetRelatedDefinitions(enterpriseDefinition, DefinitionRelationshipType.DatasetsRelatedThrough);
          FeatureClassDefinition enterpriseAddressPointDefinition = enterpriseDefinitions.First(
                  defn => defn.GetName().Equals("LocalGovernment.GDB.AddressPoint")) as FeatureClassDefinition;

          FeatureDatasetDefinition featureDatasetDefinition = geodatabase.GetDefinition<FeatureDatasetDefinition>("LocalGovernment.GDB.Address");
          IReadOnlyList<Definition> datasetsInAddressDataset = geodatabase.GetRelatedDefinitions(featureDatasetDefinition, DefinitionRelationshipType.DatasetInFeatureDataset);
          FeatureClassDefinition addressPointInAddressDataset = datasetsInAddressDataset.First(
                  defn => defn.GetName().Equals("LocalGovernment.GDB.AddressPoint")) as FeatureClassDefinition;

          RelationshipClassDefinition addressPointHasSiteAddressInAddressDataset = datasetsInAddressDataset.First(
                  defn => defn.GetName().Equals("LocalGovernment.GDB.AddressPointHasSiteAddresses")) as RelationshipClassDefinition;
        }
      });
    }

    #endregion Obtaining Related Definitions from Geodatabase

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


    #region Opening datasets from Geodatabase

    public async Task OpeningDatasetsFromGeodatabase()
    {
      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => {
        using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde")))) {
          using (Table table = geodatabase.OpenDataset<Table>("LocalGovernment.GDB.EmployeeInfo")) {
          }

          // Open a featureClass (within a feature dataset or outside a feature dataset).
          using (FeatureClass featureClass = geodatabase.OpenDataset<FeatureClass>("LocalGovernment.GDB.AddressPoint")) {
          }

          // You can open a FeatureClass as a Table which will give you a Table Reference.
          using (Table featureClassAsTable = geodatabase.OpenDataset<Table>("LocalGovernment.GDB.AddressPoint")) {
            // But it is really a FeatureClass object.
            FeatureClass featureClassOpenedAsTable = featureClassAsTable as FeatureClass;
          }

          // Open a FeatureDataset.
          using (FeatureDataset featureDataset = geodatabase.OpenDataset<FeatureDataset>("LocalGovernment.GDB.Address")) {
          }

          // Open a RelationsipClass.  Just as you can open a FeatureClass as a Table, you can also open an AttributedRelationshipClass as a RelationshipClass.
          using (RelationshipClass relationshipClass = geodatabase.OpenDataset<RelationshipClass>("LocalGovernment.GDB.AddressPointHasSiteAddresses")) {
          }
        }
      });
    }

    #endregion Opening datasets from Geodatabase

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

    #region Opening RelationshipClass between two Tables

    // Must be called within QueuedTask.Run().  
    // When used with file or enterprise geodatabases, this routine takes two table names.
    // When used with feature services, this routine takes layer IDs, or the names of the tables as they are exposed through the service (e.g., "L0States")
    public IReadOnlyList<RelationshipClass> OpenRelationshipClassFeatureServices(Geodatabase geodatabase, string originClass, string destinationClass)
    {
      return geodatabase.OpenRelationshipClass(originClass, destinationClass);
    }
    #endregion Opening RelationshipClass between two Tables

    #region Obtaining related Feature Classes from a Relationship Class
    public async Task GetFeatureClassesInRelationshipClassAsync()
    {
      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
      {
        using (Geodatabase geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(@"C:\Data\LocalGovernment.gdb"))))
        {
          IReadOnlyList<RelationshipClassDefinition> relationshipClassDefinitions = geodatabase.GetDefinitions<RelationshipClassDefinition>();
          foreach (var relationshipClassDefintion in relationshipClassDefinitions)
          {
            IReadOnlyList<Definition> definitions = geodatabase.GetRelatedDefinitions(relationshipClassDefintion,
                DefinitionRelationshipType.DatasetsRelatedThrough);
            foreach (var definition in definitions)
            {
              MessageBox.Show($"Feature class in the RelationshipClass is:{definition.GetName()}");
            }
          }
        }
      });
    }
    #endregion

    #region Opening a FeatureClass from a ShapeFile Datastore

    public async Task OpenShapefileFeatureClass()
    {
      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => {
        var fileConnection = new FileSystemConnectionPath(new Uri("path\\to\\folder\\containing\\shapefiles"), FileSystemDatastoreType.Shapefile);
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

    #region ProSnippet Group: Queries
    #endregion


    #region Searching a Table using QueryFilter

    public async Task SearchingATable()
    {
      try
      {
        await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => {
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

      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => {

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


    #region Searching a FeatureClass using SpatialQueryFilter

    public async Task SearchingAFeatureClass()
    {
      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => {
        using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
        using (FeatureClass schoolBoundaryFeatureClass = geodatabase.OpenDataset<FeatureClass>("LocalGovernment.GDB.SchoolBoundary"))
        {
          // Using a spatial query filter to find all features which have a certain district name and lying within a given Polygon.
          SpatialQueryFilter spatialQueryFilter = new SpatialQueryFilter
          {
            WhereClause = "DISTRCTNAME = 'Indian Prairie School District 204'",
            FilterGeometry = new PolygonBuilder(new List<Coordinate2D>
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

    #region Selecting Rows from a Table

    public async Task SelectingRowsFromATable()
    {
      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => {
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

    #region Selecting Features from a FeatureClass

    public async Task SelectingFeaturesFromAFeatureClass()
    {
      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => {
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
            FilterGeometry = new PolygonBuilder(newCoordinates).ToGeometry(),
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
      QueuedTask.Run(() => {
        #region Gets the count of how many rows are currently in a Table  
        //Note: call within QueuedTask.Run()
        var table = featureLayer.GetTable();
        var count = table.GetCount();
        #endregion
      });
      #region Gets the feature count of a layer
      var lyr = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();
      QueuedTask.Run(() =>
      {
        FeatureClass featureClass = lyr.GetFeatureClass();
        int nCount = featureClass.GetCount();
      });
      #endregion
    }

    #region Sorting a Table
    public RowCursor SortWorldCities(FeatureClass worldCitiesTable)
    {
      using (FeatureClassDefinition featureClassDefinition = worldCitiesTable.GetDefinition())
      {
        Field countryField = featureClassDefinition.GetFields().First(x => x.Name.Equals("COUNTRY_NAME"));
        Field cityNameField = featureClassDefinition.GetFields().First(x => x.Name.Equals("CITY_NAME"));

        // Create SortDescription for Country field
        SortDescription countrySortDescription = new SortDescription(countryField);
        countrySortDescription.CaseSensitivity = CaseSensitivity.Insensitive;
        countrySortDescription.SortOrder = SortOrder.Ascending;

        // Create SortDescription for City field
        SortDescription citySortDescription = new SortDescription(cityNameField);
        citySortDescription.CaseSensitivity = CaseSensitivity.Insensitive;
        citySortDescription.SortOrder = SortOrder.Ascending;

        // Create our TableSortDescription
        TableSortDescription tableSortDescription = new TableSortDescription(new List<SortDescription>() { countrySortDescription, citySortDescription });

        return worldCitiesTable.Sort(tableSortDescription);
      }
    }
    #endregion

    #region Calculating Statistics on a Table
    // Calculate the Sum and Average of the Population_1990 and Population_2000 fields, grouped and ordered by Region
    public void CalculateStatistics(FeatureClass countryFeatureClass)
    {
      using (FeatureClassDefinition featureClassDefinition = countryFeatureClass.GetDefinition())
      {
        // Get fields
        Field regionField = featureClassDefinition.GetFields().First(x => x.Name.Equals("Region"));
        Field pop1990Field = featureClassDefinition.GetFields().First(x => x.Name.Equals("Population_1990"));
        Field pop2000Field = featureClassDefinition.GetFields().First(x => x.Name.Equals("Population_2000"));

        // Create StatisticsDescriptions
        StatisticsDescription pop1990StatisticsDescription = new StatisticsDescription(pop1990Field, new List<StatisticsFunction>() { StatisticsFunction.Sum, StatisticsFunction.Average });
        StatisticsDescription pop2000StatisticsDescription = new StatisticsDescription(pop2000Field, new List<StatisticsFunction>() { StatisticsFunction.Sum, StatisticsFunction.Average });

        // Create TableStatisticsDescription
        TableStatisticsDescription tableStatisticsDescription = new TableStatisticsDescription(new List<StatisticsDescription>() { pop1990StatisticsDescription, pop2000StatisticsDescription });
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


    #region Evaluating a QueryDef on a single table

    public async Task SimpleQueryDef()
    {
      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => {
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

    #region Evaluating a QueryDef on a Join using WHERE Clause

    public async Task JoiningWithWhereQueryDef()
    {
      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => {
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

    #region Evaluating a QueryDef on a OUTER JOIN

    public async Task EvaluatingQueryDefWithOuterJoin()
    {
      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => {
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


    #region Evaluating a QueryDef on a nested (INNER & OUTER) join

    public async Task EvaluatingQueryDefWithNestedJoin()
    {
      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
      {
        using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file"))))
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

    #region Create Default QueryDescription for a Database table and obtain the ArcGIS.Core.Data.Table for the QueryDescription

    public async Task DefaultQueryDescription()
    {
      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => {
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

    #region Create QueryDescription from a custom query for a Database table

    public async Task CustomQueryDescription()
    {
      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => {
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

    #region Create QueryDescription from a join query where there is no non-nullable unique id column

    public async Task JoinQueryDescription()
    {
      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => {
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

    #region Create QueryDescription from a query for a Database table which has more than one shape type

    public async Task MultiGeometryQueryDescription()
    {
      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => {
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

    #region Create QueryDescription from a query for an SQLite Database table

    public async Task SqliteQueryDescription()
    {
      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => {
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

    #region Using SQLSyntax to form platform agnostic queries

    public async Task UsingSqlSyntaxToFormPlatformAgnosticQueries()
    {
      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => {
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

    #region Joining a file geodatabase feature class to an Oracle database query layer feature class with a virtual relationship class

    public async Task JoiningFileGeodatabaseFeatureClassToOracleQueryLayer()
    {
      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => {
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

          VirtualRelationshipClassDescription description = new VirtualRelationshipClassDescription(originPrimaryKey, destinationForeignKey, RelationshipCardinality.OneToOne);

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

    #region Creating a QueryTable using a query which joins two versioned tables in a geodatabase

    public async Task QueryTableJoinWithVersionedData()
    {
      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => {
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
      #region Checking a field value for null
      var val = row[field.Name];
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
          var varSubtypeCode = row[subtypeFieldName];
          long subtypeCode = (long)varSubtypeCode ;

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
    
    #region ProSnippet Group: Editing
    #endregion


    #region Creating a Row

    public async Task CreatingARow()
    {
      string message = String.Empty;
      bool creationResult = false;
      EditOperation editOperation = new EditOperation();

      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => {

        using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
        using (Table enterpriseTable = geodatabase.OpenDataset<Table>("LocalGovernment.GDB.piCIPCost"))
        {

          //var geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(uri)) for a File GDB
          //
          //var shapeFileConnPath = new FileSystemConnectionPath(uri, FileSystemDatastoreType.Shapefile);
          //var shapefile = new FileSystemDatastore(shapeFileConnPath);
          //var table = shapefile.OpenDataset<Table>(strShapeFileName); for a Shape file

          //declare the callback here. We are not executing it ~yet~
          editOperation.Callback(context => {
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

    #region Creating a Feature

    public async Task CreatingAFeature()
    {
      string message = String.Empty;
      bool creationResult = false;

      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => {
        using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
        using (FeatureClass enterpriseFeatureClass = geodatabase.OpenDataset<FeatureClass>("LocalGovernment.GDB.FacilitySite"))
        {
          //var geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(uri)) for a File GDB
          //
          //var shapeFileConnPath = new FileSystemConnectionPath(uri, FileSystemDatastoreType.Shapefile);
          //var shapefile = new FileSystemDatastore(shapeFileConnPath);
          //var table = shapefile.OpenDataset<Table>(strShapeFileName); for a Shape file

          //declare the callback here. We are not executing it ~yet~
          EditOperation editOperation = new EditOperation();
          editOperation.Callback(context => {
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

              rowBuffer[facilitySiteDefinition.GetShapeField()] = new PolygonBuilder(newCoordinates).ToGeometry();

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

    #region Modifying a Row

    public async Task ModifyingARow()
    {
      string message = String.Empty;
      bool modificationResult = false;

      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => {
        using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
        using (Table enterpriseTable = geodatabase.OpenDataset<Table>("LocalGovernment.GDB.piCIPCost"))
        {
          //var geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(uri)) for a File GDB
          //
          //var shapeFileConnPath = new FileSystemConnectionPath(uri, FileSystemDatastoreType.Shapefile);
          //var shapefile = new FileSystemDatastore(shapeFileConnPath);
          //var table = shapefile.OpenDataset<Table>(strShapeFileName); for a Shape file

          EditOperation editOperation = new EditOperation();
          editOperation.Callback(context => {
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

    #region Modifying a Feature

    public async Task ModifyingAFeature()
    {
      string message = String.Empty;
      bool modificationResult = false;

      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => {
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
          editOperation.Callback(context => {
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

                    feature.SetShape(new PolygonBuilder(newCoordinates).ToGeometry());
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
      #region Writing a value into a Guid column
      row[field.Name] = "{" + guid.ToString() + "}";
      #endregion Writing a value into a Guid column
    }


    #region Deleting a Row/Feature

    public async Task DeletingARowOrFeature()
    {
      string message = String.Empty;
      bool deletionResult = false;

      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => {
        using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file\\sdefile.sde"))))
        using (Table enterpriseTable = geodatabase.OpenDataset<Table>("LocalGovernment.GDB.piCIPCost"))
        {

          //var geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(uri)) for a File GDB
          //
          //var shapeFileConnPath = new FileSystemConnectionPath(uri, FileSystemDatastoreType.Shapefile);
          //var shapefile = new FileSystemDatastore(shapeFileConnPath);
          //var table = shapefile.OpenDataset<Table>(strShapeFileName); for a Shape file

          EditOperation editOperation = new EditOperation();
          editOperation.Callback(context => {
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

    #region Obtaining a memory stream to modify or create Attachment data

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

    #endregion Obtaining a memory stream to modify or create Attachment data

    #region Adding Attachments

    public async Task AddingAttachments()
    {
      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => {
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

    #endregion Adding Attachments

    #region Updating Attachments

    public async Task UpdatingAttachments()
    {
      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => {
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

    #region Deleting Attachments

    public async Task DeletingAttachments()
    {
      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => {
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

                var attachmentIDs = attachments.Select(attachment => attachment.GetAttachmentID()) as IReadOnlyList<long>;
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


    #region Getting Rows related by RelationshipClass
    public async Task GettingRowsRelatedByRelationshipClass()
    {
      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => {
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

    #region Creating a Relationship

    public async Task CreatingARelationship()
    {
      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => {
        using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file"))))
        using (RelationshipClass relationshipClass = geodatabase.OpenDataset<RelationshipClass>("LocalGovernment.GDB.OverviewToProject"))
        using (FeatureClass projectsFeatureClass = geodatabase.OpenDataset<FeatureClass>("LocalGovernment.GDB.CIPProjects"))
        using (FeatureClass overviewFeatureClass = geodatabase.OpenDataset<FeatureClass>("LocalGovernment.GDB.CIPProjectsOverview"))
        {
          // This will be PROJNAME. This can be used to get the field index or used directly as the field name.
          string originKeyField = relationshipClass.GetDefinition().GetOriginKeyField();

          EditOperation editOperation = new EditOperation();
          editOperation.Callback(context => {
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

    #region Deleting a Relationship

    public async Task DeletingARelationship()
    {
      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => {
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
                editOperation.Callback(context => {
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

    #region ProSnippet Group: Versioning
    #endregion

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

    #region Reconciling and Posting a Version with its Parent
    public void ReconcileAndPost(Geodatabase geodatabase)
    {
      // Get a reference to our version and our parent
      if (geodatabase.IsVersioningSupported())
      {
        using (VersionManager versionManager = geodatabase.GetVersionManager())
        using (Version currentVersion = versionManager.GetCurrentVersion())
        using (Version parentVersion = currentVersion.GetParent())
        {

          // Create a ReconcileDescription object
          ReconcileDescription reconcileDescription = new ReconcileDescription(parentVersion);
          reconcileDescription.ConflictResolutionMethod = ConflictResolutionMethod.Continue; // continue if conflicts are found
          reconcileDescription.WithPost = true;

          // Reconcile and post
          ReconcileResult reconcileResult = currentVersion.Reconcile(reconcileDescription);

          // ReconcileResult.HasConflicts can be checked as-needed
        }
      }
    }

    #endregion

    #region Working with Versions

    public async Task WorkingWithVersions()
    {
      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => {
        using (Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri("path\\to\\sde\\file"))))
        using (VersionManager versionManager = geodatabase.GetVersionManager()) {
          IReadOnlyList<Version> versionList = versionManager.GetVersions();

          //The default version will have a null Parent
          Version defaultVersion = versionList.First(version => version.GetParent() == null);

          IEnumerable<Version> publicVersions = versionList.Where(version => version.GetAccessType() == VersionAccessType.Public);
          Version qaVersion = defaultVersion.GetChildren().First(version => version.GetName().Contains("QA"));

          Geodatabase qaVersionGeodatabase = qaVersion.Connect();

          FeatureClass currentFeatureClass = geodatabase.OpenDataset<FeatureClass>("featureClassName");
          FeatureClass qaFeatureClass = qaVersionGeodatabase.OpenDataset<FeatureClass>("featureClassName");
        }
      });
    }

    #endregion Working with Versions

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


    #endregion

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

    public void PartialPostingSnippet(Version designVersion, FeatureClass supportStructureFeatureClass, List<long> deletedSupportStructureObjectIDs)
    {
      #region Partial Posting

      // Partial posting allows developers to post a subset of changes made in a version. One sample use case is an electric utility that uses a version to design the facilities in a new
      // housing subdivision. At some point in the process, one block of new houses have been completed, while the rest of the subdivision remains unbuilt.  Partial posting allows the user
      // to post the completed work, while leaving not yet constructed features in the version to be posted later.
      // Partial posting requires a branch-versioned feature service using ArcGIS Enterprise 10.9 and higher

      // Specify a set of features that were constructed
      QueryFilter constructedFilter = new QueryFilter()
      {
        WhereClause = "ConstructedStatus = 'True'"
      };
      // This selection represents the inserts and updates to the support structure feature class that we wish to post
      using (Selection constructedSupportStructures = supportStructureFeatureClass.Select(constructedFilter, SelectionType.ObjectID, SelectionOption.Normal))
      { 
        // Specifying which feature deletions you wish to post is slightly trickier, since you cannot issue a query to fetch a set of deleted features
        // Instead, a list of ObjectIDs must be used
        using (Selection deletedSupportStructures = supportStructureFeatureClass.Select(null, SelectionType.ObjectID, SelectionOption.Empty))
        {
          deletedSupportStructures.Add(deletedSupportStructureObjectIDs);  // deletedSupportStructureObjectIDs is defined as List<long>

          //Perform the reconcile with partial post
          
          ReconcileDescription reconcileDescription = new ReconcileDescription();
          reconcileDescription.ConflictDetectionType = ConflictDetectionType.ByColumn;
          reconcileDescription.ConflictResolutionMethod = ConflictResolutionMethod.Continue;
          reconcileDescription.ConflictResolutionType = ConflictResolutionType.FavorEditVersion;
          reconcileDescription.PartialPostSelections = new List<Selection>() { constructedSupportStructures, deletedSupportStructures };
          reconcileDescription.WithPost = true;

          ReconcileResult reconcileResult = designVersion.Reconcile(reconcileDescription);
        }
      }
      #endregion
    }

    #region ProSnippet Group: DDL
    #endregion

    public void CreateTableSnippet(Geodatabase geodatabase, CodedValueDomain inspectionResultsDomain)
    {
      #region Creating a Table
      // Geodatabase DDL is pre-release for Pro 2.7. See https://github.com/esri/arcgis-pro-sdk/wiki/ProConcepts-DDL for more information

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

      #endregion
    }


    public void CreateFeatureClassSnippet(Geodatabase geodatabase, FeatureClass existingFeatureClass, SpatialReference spatialReference)
    {
      #region Creating a feature class
      // Geodatabase DDL is pre-release for Pro 2.7. See https://github.com/esri/arcgis-pro-sdk/wiki/ProConcepts-DDL for more information

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

      // Alternately, ShapeDescriptions can be created from another feature class.  In this case, the new feature class will inherit the same shape properties of the existing class
      ShapeDescription alternateShapeDescription = new ShapeDescription(existingFeatureClass.GetDefinition());

      // Create a FeatureClassDescription object to describe the feature class to create
      FeatureClassDescription featureClassDescription = new FeatureClassDescription("Cities", fieldDescriptions, shapeDescription);

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

      #endregion
    }

    public void DeleteTableSnippet(Geodatabase geodatabase, Table table)
    {
      #region Deleting a Table

      // Create a TableDescription object
      TableDescription tableDescription = new TableDescription(table.GetDefinition());

      // Create a SchemaBuilder object
      SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);

      // Add the deletion of the table to our list of DDL tasks
      schemaBuilder.Delete(tableDescription);

      // Execute the DDL
      bool success = schemaBuilder.Build();

      #endregion
    }

    public void DeleteFeatureClassSnippet(Geodatabase geodatabase, FeatureClass featureClass)
    {
      #region Deleting a Feature Class

      // Create a FeatureClassDescription object
      FeatureClassDescription featureClassDescription = new FeatureClassDescription(featureClass.GetDefinition());

      // Create a SchemaBuilder object
      SchemaBuilder schemaBuilder = new SchemaBuilder(geodatabase);

      // Add the deletion fo the feature class to our list of DDL tasks
      schemaBuilder.Delete(featureClassDescription);

      // Execute the DDL
      bool success = schemaBuilder.Build();

      #endregion
    }

    public void CreateFeatureGeodatabaseSnippet()
    {
      #region Creating a File Geodatabase
      // Create a FileGeodatabaseConnectionPath with the name of the file geodatabase you wish to create
      FileGeodatabaseConnectionPath fileGeodatabaseConnectionPath = new FileGeodatabaseConnectionPath(new Uri(@"C:\Path-To-File-Geodatabase\YourName.gdb"));

      // Create and use the file geodatabase
      using (Geodatabase geodatabase = SchemaBuilder.CreateGeodatabase(fileGeodatabaseConnectionPath))
      {
        // Create additional schema here
      }
      #endregion
    }

    public void DeleteFeatureGeodatabaseSnippet()
    {
      #region Deleting a File Geodatabase
      // Create a FileGeodatabaseConnectionPath with the name of the file geodatabase you wish to delete
      FileGeodatabaseConnectionPath fileGeodatabaseConnectionPath = new FileGeodatabaseConnectionPath(new Uri(@"C:\Path-To-File-Geodatabase\YourName.gdb"));

      // Delete the file geodatabase
      SchemaBuilder.DeleteGeodatabase(fileGeodatabaseConnectionPath);

      #endregion
    }



  }
}