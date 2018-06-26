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
using ArcGIS.Core.Hosting;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Mapping;
using Version = ArcGIS.Core.Data.Version;

namespace GeodatabaseSDK.GeodatabaseSDK.Snippets
{
    class SnippetsCS6
    {
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
    }
}
