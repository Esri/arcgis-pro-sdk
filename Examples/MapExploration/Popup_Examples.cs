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
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Framework.Dialogs;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using ArcGIS.Desktop.Framework.Threading.Tasks;

namespace Examples
{
  class Popup_Examples
  {
    /// MapView.ShowCustomPopup(IEnumerable<PopupContent>), PopupContent(string, string), PopupContent(Uri, string)
    /// <example>
    /// <code title="Show A Custom Pop-up" description="Show a custom pop-up." region="Show A Custom Pop-up" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\Popup_Examples.cs" lang="CS"/>
    /// </example>
    #region Show A Custom Pop-up
    public void ShowCustomPopup()
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return;

      //Create custom popup content
      var popups = new List<PopupContent>();
      popups.Add(new PopupContent("<b>This text is bold.</b>", "Custom tooltip from HTML string"));
      popups.Add(new PopupContent(new Uri("http://www.esri.com/"), "Custom tooltip from Uri"));
      
      mapView.ShowCustomPopup(popups);
    }
    #endregion

    /// MapView.ShowPopup(MapMember, long)
    /// <example>
    /// <code title="Show A Pop-up For A Feature" description="Show a pop-up for a feature." region="Show A Pop-up For A Feature" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\Popup_Examples.cs" lang="CS"/>
    /// </example>
    #region Show A Pop-up For A Feature
    public void ShowPopup(MapMember mapMember, long objectID)
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return;

      mapView.ShowPopup(mapMember, objectID);
    }
    #endregion

    /// PopupContent(MapMember, long), MapView.ShowCustomPopup(IEnumerable<PopupContent>, IEnumerable<PopupCommand>, bool)
    /// <example>
    /// <code title="Show A Pop-up With Custom Commands" description="Show a pop-up with custom commands." region="Show A Pop-up With Custom Commands" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\Popup_Examples.cs" lang="CS"/>
    /// </example>
    #region Show A Pop-up With Custom Commands
    public void ShowCustomPopup(MapMember mapMember, long objectID)
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return;

      //Create custom popup content from existing map member and object id
      var popups = new List<PopupContent>();
      popups.Add(new PopupContent(mapMember, objectID));
      
      //Create a new custom command to add to the popup window
      var commands = new List<PopupCommand>();
      commands.Add(new PopupCommand(
        p => MessageBox.Show(string.Format("Map Member: {0}, ID: {1}", p.MapMember, p.ID)),
        p => { return p != null; },
        "My custom command",
        new BitmapImage(new Uri("pack://application:,,,/ArcGIS.Desktop.Resources;component/Images/GenericCheckMark16.png")) as ImageSource));

      mapView.ShowCustomPopup(popups, commands, true);
    }
    #endregion

    /// PopupContent.IsDynamicContent, PopupContent.OnCreateHtmlContent
    /// <example>
    /// <code title="Show A Dynamic Pop-up" description="Show a custom pop-up with dynamically generated content." region="Show A Dynamic Pop-up" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\Popup_Examples.cs" lang="CS"/>
    /// </example>
    #region Show A Dynamic Pop-up
    public void ShowDynamicPopup(MapMember mapMember, List<long> objectIDs)
    {
      //Get the active map view.
      var mapView = MapView.Active;
      if (mapView == null)
        return;

      //Create popup whose content is created the first time the item is requested.
      var popups = new List<PopupContent>();
      foreach (var id in objectIDs)
      {
        popups.Add(new DynamicPopupContent(mapMember, id));
      }
      
      mapView.ShowCustomPopup(popups);
    }

    internal class DynamicPopupContent : PopupContent
    {
      public DynamicPopupContent(MapMember mapMember, long objectID)
      {
        MapMember = mapMember;
        ID = objectID;
        IsDynamicContent = true;
      }
      
      //Called when the pop-up is loaded in the window.
      protected override Task<string> OnCreateHtmlContent()
      {
        return QueuedTask.Run(() => string.Format("<b>Map Member: {0}, ID: {1}</b>", MapMember, ID));
      }
    }

    #endregion

  }
}
