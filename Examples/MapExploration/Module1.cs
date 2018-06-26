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
using System.Windows.Input;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using System.Threading.Tasks;
using ArcGIS.Desktop.Mapping.Events;
using ArcGIS.Desktop.Mapping;

namespace Examples
{
  internal class Module1 : Module
  {
    private static Module1 _this = null;

    public Module1()
    {

    }

    /// <summary>
    /// Retrieve the singleton instance to this module here
    /// </summary>
    public static Module1 Current
    {
      get
      {
        return _this ?? (_this = (Module1)FrameworkApplication.FindModule("ProAppModule4_Module"));
      }
    }

    /// <summary>
    /// Show the Camera DockPane.
    /// </summary>
    internal static void ShowCameraPane()
    {
      DockPane pane = FrameworkApplication.DockPaneManager.Find("mapExploration_CameraDockPane");
      if (pane == null)
        return;

      pane.Activate();
    }

    /// <summary>
    /// Show the Bookmark DockPane.
    /// </summary>
    internal static void ShowBookmarkPane()
    {
      DockPane pane = FrameworkApplication.DockPaneManager.Find("mapExploration_BookmarksDockPane");
      if (pane == null)
        return;

      pane.Activate();
    }

    #region Overrides
    /// <summary>
    /// Called by Framework when ArcGIS Pro is closing
    /// </summary>
    /// <returns>False to prevent Pro from closing, otherwise True</returns>
    protected override bool CanUnload()
    {
      //TODO - add your business logic
      //return false to ~cancel~ Application close
      return true;
    }

    /// <summary>
    /// Generic implementation of ExecuteCommand to allow calls to
    /// <see cref="FrameworkApplication.ExecuteCommand"/> to execute commands in
    /// your Module.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    protected override Func<Task> ExecuteCommand(string id)
    {

      //TODO: replace generic implementation with custom logic
      //etc as needed for your Module
      var command = FrameworkApplication.GetPlugInWrapper(id) as ICommand;
      if (command == null)
        return () => Task.FromResult(0);
      if (!command.CanExecute(null))
        return () => Task.FromResult(0);

      return () =>
      {
        command.Execute(null); // if it is a tool, execute will set current tool
        return Task.FromResult(0);
      };
    }
    #endregion Overrides

  }
}
