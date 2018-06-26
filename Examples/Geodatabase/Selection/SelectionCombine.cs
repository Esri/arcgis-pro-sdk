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
using System.Threading.Tasks;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Framework.Threading.Tasks;

namespace SDKExamples.GeodatabaseSDK
{
  /// <summary>
  /// Illustrates how to use Combine on a Selection object.
  /// </summary>
  /// 
  /// <remarks>
  /// <para>
  /// While it is true classes that are derived from the <see cref="ArcGIS.Core.CoreObjectsBase"/> super class 
  /// consumes native resources (e.g., <see cref="ArcGIS.Core.Data.Geodatabase"/> or <see cref="ArcGIS.Core.Data.FeatureClass"/>), 
  /// you can rest assured that the garbage collector will properly dispose of the unmanaged resources during 
  /// finalization.  However, there are certain workflows that require a <b>deterministic</b> finalization of the 
  /// <see cref="ArcGIS.Core.Data.Geodatabase"/>.  Consider the case of a file geodatabase that needs to be deleted 
  /// on the fly at a particular moment.  Because of the <b>indeterministic</b> nature of garbage collection, we can't
  /// count on the garbage collector to dispose of the Geodatabase object, thereby removing the <b>lock(s)</b> at the  
  /// moment we want. To ensure a deterministic finalization of important native resources such as a 
  /// <see cref="ArcGIS.Core.Data.Geodatabase"/> or <see cref="ArcGIS.Core.Data.FeatureClass"/>, you should declare 
  /// and instantiate said objects in a <b>using</b> statement.  Alternatively, you can achieve the same result by 
  /// putting the object inside a try block and then calling Dispose() in a finally block.
  /// </para>
  /// <para>
  /// In general, you should always call Dispose() on the following types of objects: 
  /// </para>
  /// <para>
  /// - Those that are derived from <see cref="ArcGIS.Core.Data.Datastore"/> (e.g., <see cref="ArcGIS.Core.Data.Geodatabase"/>).
  /// </para>
  /// <para>
  /// - Those that are derived from <see cref="ArcGIS.Core.Data.Dataset"/> (e.g., <see cref="ArcGIS.Core.Data.Table"/>).
  /// </para>
  /// <para>
  /// - <see cref="ArcGIS.Core.Data.RowCursor"/> and <see cref="ArcGIS.Core.Data.RowBuffer"/>.
  /// </para>
  /// <para>
  /// - <see cref="ArcGIS.Core.Data.Row"/> and <see cref="ArcGIS.Core.Data.Feature"/>.
  /// </para>
  /// <para>
  /// - <see cref="ArcGIS.Core.Data.Selection"/>.
  /// </para>
  /// <para>
  /// - <see cref="ArcGIS.Core.Data.VersionManager"/> and <see cref="ArcGIS.Core.Data.Version"/>.
  /// </para>
  /// </remarks>  
  public class SelectionCombine
  {
    /// <summary>
    /// In order to illustrate that Geodatabase calls have to be made on the MCT
    /// </summary>
    /// <returns></returns>
    public async Task SelectionRemoveAsync()
    {
      await QueuedTask.Run(() => MainMethodCode());
    }

    public void MainMethodCode()
    {
      using (Geodatabase fileGeodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(@"C:\Data\LocalGovernment.gdb"))))
      using (FeatureClass featureClass   = fileGeodatabase.OpenDataset<FeatureClass>("SchoolBoundary"))
      {
        SpatialQueryFilter spatialQueryFilter = new SpatialQueryFilter 
        {
          FilterGeometry = new PolygonBuilder(new List<Coordinate2D>
          {
            new Coordinate2D(1021880, 1867396),
            new Coordinate2D(1028223, 1870705),
            new Coordinate2D(1031165, 1866844),
            new Coordinate2D(1025373, 1860501),
            new Coordinate2D(1021788, 1863810),
          }).ToGeometry(),

          SpatialRelationship = SpatialRelationship.Intersects
        };

        QueryFilter anotherQueryFilter = new QueryFilter { WhereClause = "DISTRCTNAME = 'Indian Prairie School District 204'" };

        using (Selection spatialSelection       = featureClass.Select(spatialQueryFilter, SelectionType.ObjectID, SelectionOption.Normal))
        using (Selection indianPrairieSelection = featureClass.Select(anotherQueryFilter, SelectionType.ObjectID, SelectionOption.Normal))
        {
          // In order to find all features which are in the specified Polygon and in the specified district.
          using (Selection selection = spatialSelection.Combine(indianPrairieSelection, SetOperation.Intersection))
          {
            IEnumerable<long> oids = indianPrairieSelection.GetObjectIDs();
            selection.Add(oids.ToList());
          }
        }
      }
      
      //Opening a Non-Versioned SQL Server instance
      DatabaseConnectionProperties connectionProperties = new DatabaseConnectionProperties(EnterpriseDatabaseType.SQLServer) 
      {
        AuthenticationMode = AuthenticationMode.DBMS,
        
        // Where testMachine is the machine where the instance is running and testInstance is the name of the SqlServer instance.
        Instance = @"testMachine\testInstance",

        // Provided that a database called LocalGovernment has been created on the testInstance and geodatabase has been enabled on the database.
        Database = "LocalGovernment",

        // Provided that a login called gdb has been created and corresponding schema has been created with the required permissions.
        User     = "gdb",
        Password = "password",
        Version  = "dbo.DEFAULT"
      };

      using (Geodatabase geodatabase               = new Geodatabase(connectionProperties))
      using (FeatureClass facilitySiteFeatureClass = geodatabase.OpenDataset<FeatureClass>("LocalGovernment.GDB.FacilitySite"))
      {
        QueryFilter queryFilter        = new QueryFilter { WhereClause = "FCODE = 'Park'" };
        QueryFilter subtypeQueryFilter = new QueryFilter { WhereClause = "SUBTYPEFIELD = 730" };
        using (Selection parkSelection             = facilitySiteFeatureClass.Select(queryFilter, SelectionType.ObjectID, SelectionOption.Normal))
        using (Selection educationSubtypeSelection = facilitySiteFeatureClass.Select(subtypeQueryFilter, SelectionType.ObjectID, SelectionOption.Normal))
        using (Selection parksAndSchools           = educationSubtypeSelection.Combine(parkSelection, SetOperation.Union))
        { 
        }

        QueryFilter hazardousFacilityFilter    = new QueryFilter { WhereClause = "FCODE = 'Hazardous Materials Facility'" };
        QueryFilter industrySubtypeQueryFilter = new QueryFilter { WhereClause = "SUBTYPEFIELD = 710" };
        using (Selection hazardousFacilitySelection = facilitySiteFeatureClass.Select(hazardousFacilityFilter, SelectionType.ObjectID, SelectionOption.Normal))
        using (Selection industrySelection = facilitySiteFeatureClass.Select(industrySubtypeQueryFilter, SelectionType.ObjectID, SelectionOption.Normal))
        using (Selection nonHazardousIndustryFacilities = industrySelection.Combine(hazardousFacilitySelection, SetOperation.Difference))
        {
        }

        QueryFilter nonProfitFilter             = new QueryFilter { WhereClause = "OWNTYPE = 'Non-Profit'" };
        QueryFilter publicAttractionQueryFilter = new QueryFilter { WhereClause = "SUBTYPEFIELD = 820" };
        using (Selection nonProfitSelection               = facilitySiteFeatureClass.Select(nonProfitFilter, SelectionType.ObjectID, SelectionOption.Normal))
        using (Selection publicAttractionSelection        = facilitySiteFeatureClass.Select(publicAttractionQueryFilter, SelectionType.ObjectID, SelectionOption.Normal))
        using (Selection eitherPublicAttactionOrNonProfit = nonProfitSelection.Combine(publicAttractionSelection, SetOperation.SymmetricDifference))
        {
        }
      }
    }
  }
}