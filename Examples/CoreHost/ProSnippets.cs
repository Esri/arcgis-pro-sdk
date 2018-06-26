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

namespace ConsoleApplication1 {

    #region Initializing Core Host
    using ArcGIS.Core.Data;
    //There must be a reference to ArcGIS.CoreHost.dll
    using ArcGIS.Core.Hosting;

    class Program {
        //[STAThread] must be present on the Application entry point
        [STAThread]
        static void Main(string[] args) {

            //Call Host.Initialize before constructing any objects from ArcGIS.Core
            try {
                Host.Initialize();
            }
            catch (Exception e) {
                // Error (missing installation, no license, 64 bit mismatch, etc.)
                Console.WriteLine(string.Format("Initialization failed: {0}",e.Message));
                return;
            }

            //if we are here, ArcGIS.Core is successfully initialized
            Geodatabase gdb = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(@"C:\Data\SDK\GDB\MySampleData.gdb")));
            IReadOnlyList<TableDefinition> definitions = gdb.GetDefinitions<FeatureClassDefinition>();

            foreach (var fdsDef in definitions) {
                Console.WriteLine(TableString(fdsDef as TableDefinition));
            }
            Console.Read();
        }

        private static string TableString(TableDefinition table) {
            string alias = table.GetAliasName();
            string name = table.GetName();
            return string.Format("{0} ({1})", alias.Length > 0 ? alias : name, name);
        }

    }
    #endregion
}
