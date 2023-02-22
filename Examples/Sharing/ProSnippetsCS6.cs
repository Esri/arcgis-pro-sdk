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
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json.Linq;
using ArcGIS.Desktop.Core.Portal;

namespace Content.Sharing.ProSnippet
{
  class ProSnippetCS6
  {
    public static void PortalMethods()
    {
      // cref: ArcGIS.Desktop.Core.ArcGISPortal.IsSignedOn()
      // cref: ArcGIS.Desktop.Core.ArcGISPortal.SignIn()
      // cref: ArcGIS.Desktop.Core.ArcGISPortal.GetSignOnUsername()
      #region Portal: Get the Current signed in User from the active portal

      var portal = ArcGISPortalManager.Current.GetActivePortal();
      //Force login
      if (!portal.IsSignedOn())
      {
        portal.SignIn();
      }
      var user = portal.GetSignOnUsername();

      #endregion
    }

    public static async void PortalMethods2()
    {
      // cref: ArcGIS.Desktop.Core.ArcGISPortalExtensions.GetPortalInfoAsync(ArcGIS.Desktop.Core.ArcGISPortal)
      // cref: ArcGIS.Desktop.Core.Portal.PortalInfo
      #region Portal: Get the "online" portal view for the current user
      //If no-one is signed in, this will be the default view for
      //the anonymous user.
      var online = ArcGISPortalManager.Current.GetPortal(new Uri("http://www.arcgis.com"));
      var portalInfo = await online.GetPortalInfoAsync();

      #endregion
    }


    public static async void PortalMethods4()
    {
      string portalUri = "";

      // cref: ArcGIS.Desktop.Core.ArcGISPortalManager.GetPortal(System.Uri)
      // cref: ArcGIS.Desktop.Core.ArcGISPortalExtensions.GetPortalInfoAsync(ArcGIS.Desktop.Core.ArcGISPortal)
      // cref: ArcGIS.Desktop.Core.Portal.PortalInfo
      // cref: ArcGIS.Desktop.Core.Portal.PortalInfo.OrganizationId
      #region Portal: Get the organization id for the current user

      var portal = ArcGISPortalManager.Current.GetPortal(new Uri(portalUri));
      var portalInfo = await portal.GetPortalInfoAsync();
      var orgid = portalInfo.OrganizationId;

      #endregion
    }

    public static async void PortalMethods5()
    {

      // cref: ArcGIS.Desktop.Core.ArcGISPortal.GetSignOnUsername()
      // cref: ArcGIS.Desktop.Core.ArcGISPortalExtensions.GetUserContentAsync(ArcGIS.Desktop.Core.ArcGISPortal, System.String, System.String)
      // cref: ArcGIS.Desktop.Core.Portal.PortalUserContent
      // cref: ArcGIS.Desktop.Core.Portal.PortalUserContent.PortalFolders
      // cref: ArcGIS.Desktop.Core.Portal.PortalFolder
      // cref: ArcGIS.Desktop.Core.Portal.PortalUserContent.PortalItems
      // cref: ArcGIS.Desktop.Core.Portal.PortalItem
      #region Portal: Get the user content for the active user from the active portal

      var portal = ArcGISPortalManager.Current.GetActivePortal();
      var owner = portal.GetSignOnUsername();
      var userContent = await portal.GetUserContentAsync(owner);
      //Get content for a specific folder (identified by its folder id)
      //var userContent = await portal.GetUserContentAsync(owner, folderId);

      //Get all the folders
      foreach (var pf in userContent.PortalFolders)
      {
        //Do something with the folders

      }

      //Get all the content items
      foreach (var pi in userContent.PortalItems)
      {
        //Do something with the portal items
      }

      #endregion

      // cref: ArcGIS.Desktop.Core.ArcGISPortalExtensions.GetUserContentAsync(ArcGIS.Desktop.Core.ArcGISPortal, System.String, System.String)
      // cref: ArcGIS.Desktop.Core.Portal.PortalUserContent.PortalItems
      // cref: ArcGIS.Desktop.Core.Portal.PortalItemType
      // cref: ArcGIS.Desktop.Core.Portal.PortalItem.PortalItemType
      // cref: ArcGIS.Desktop.Core.Portal.PortalItem.GetItemDataAsync(System.String)
      #region Portal: Download any package items in the user content

      //user content previously from...
      //var userContent = await portal.GetUserContentAsync(owner);

      var packages = new List<PortalItemType>
      {
        PortalItemType.BasemapPackage,
        PortalItemType.GeoprocessingPackage,
        PortalItemType.LayerPackage,
        PortalItemType.LocatorPackage,
        PortalItemType.MapPackage,
        PortalItemType.ProjectPackage,
        PortalItemType.ScenePackage,
        PortalItemType.RulePackage,
        PortalItemType.VectorTilePackage
      };
      var folder = @"E:\Temp\PortalAPITest\";
      foreach (var di in userContent.PortalItems.Where(pi => packages.Contains(pi.PortalItemType)))
      {
        var path = System.IO.Path.Combine(folder, di.Name);
        await di.GetItemDataAsync(path);
      }

      #endregion

      // cref: ArcGIS.Desktop.Core.ArcGISPortalExtensions.GetGroupsFromUserAsync(ArcGIS.Desktop.Core.ArcGISPortal, System.String)
      // cref: ArcGIS.Desktop.Core.Portal.PortalGroup
      #region Portal: Get the groups for the specified user

      //elsewhere...
      //var owner = portal.GetSignOnUsername();
      var groups = await portal.GetGroupsFromUserAsync(owner);
      foreach (var group in groups)
      {
        //Do something with the portal groups
      }

      #endregion

    }

    public static async void PortalMethods6()
    {
      Uri portalUri = new Uri("");

      // cref: ArcGIS.Desktop.Core.ArcGISPortal.GetSignOnUsername()
      // cref: ArcGIS.Desktop.Core.ArcGISPortalExtensions.GetPortalInfoAsync(ArcGIS.Desktop.Core.ArcGISPortal)
      // cref: ArcGIS.Desktop.Core.Portal.PortalQueryParameters.CreateForItemsOfType(ArcGIS.Desktop.Core.Portal.PortalItemType, string)
      // cref: ArcGIS.Desktop.Core.Portal.PortalQueryParameters
      // cref: ArcGIS.Desktop.Core.Portal.PortalQueryParameters.OrganizationId
      // cref: ArcGIS.Desktop.Core.Portal.PortalQueryParameters.Limit
      // cref: ArcGIS.Desktop.Core.ArcGISPortalExtensions.SearchForContentAsync(ArcGIS.Desktop.Core.ArcGISPortal, ArcGIS.Desktop.Core.Portal.PortalQueryParameters)
      // cref: ArcGIS.Desktop.Core.Portal.PortalQueryResultSet<T>
      #region Portal: Execute a portal search

      var portal = ArcGISPortalManager.Current.GetPortal(portalUri);
      var owner = portal.GetSignOnUsername();
      var portalInfo = await portal.GetPortalInfoAsync();

      //1. Get all web maps
      var query1 = PortalQueryParameters.CreateForItemsOfType(PortalItemType.WebMap);

      //2. Get all web maps and map services - include user, organization
      // and "usa" in the title
      var query2 = PortalQueryParameters.CreateForItemsOfTypes(new List<PortalItemType>() {
        PortalItemType.WebMap, PortalItemType.MapService}, owner, "", "title:usa");
      query2.OrganizationId = portalInfo.OrganizationId;

      //retrieve in batches of up to a 100 each time
      query2.Limit = 100;

      //Loop until done
      var portalItems = new List<PortalItem>();
      while (query2 != null)
      {
        //run the search
        PortalQueryResultSet<PortalItem> results = await portal.SearchForContentAsync(query2);
        portalItems.AddRange(results.Results);
        query2 = results.NextQueryParameters;
      }

      //process results
      foreach (var pi in portalItems)
      {
        //Do something with the portal items
      }

      #endregion
    }

    public static async Task EsriHttpClientMethods()
    {

      // cref: ArcGIS.Desktop.Core.EsriHttpClient
      // cref: ArcGIS.Desktop.Core.EsriHttpClient.#ctor
      // cref: ArcGIS.Desktop.Core.EsriHttpClient.Get(System.String)
      // cref: ArcGIS.Desktop.Core.EsriHttpResponseMessage
      // cref: ArcGIS.Desktop.Core.EsriHttpResponseMessage.Content
      #region EsriHttpClient: Get the Current signed on User

      //Reference Newtonsoft - Json.Net
      //Reference System.Net.Http
      UriBuilder selfURL = new UriBuilder(ArcGISPortalManager.Current.GetActivePortal().PortalUri)
      {
        Path = "sharing/rest/portals/self",
        Query = "f=json"
      };
      EsriHttpResponseMessage response = new EsriHttpClient().Get(selfURL.Uri.ToString());

      dynamic portalSelf = JObject.Parse(await response.Content.ReadAsStringAsync());
      // if the response doesn't contain the user information then it is essentially
      // an anonymous request against the portal
      if (portalSelf.user == null)
        return;
      string userName = portalSelf.user.username;

      #endregion

      // cref: ArcGIS.Desktop.Core.EsriHttpClient
      // cref: ArcGIS.Desktop.Core.EsriHttpClient.#ctor
      // cref: ArcGIS.Desktop.Core.EsriHttpClient.Get(System.String)
      // cref: ArcGIS.Desktop.Core.EsriHttpResponseMessage
      // cref: ArcGIS.Desktop.Core.EsriHttpResponseMessage.Content
      #region Get the Groups for the Current Signed on User

      //Assume that you have executed the "Get the Current signed on User" snippet and have 'userName'
      UriBuilder groupsURL = new UriBuilder(ArcGISPortalManager.Current.GetActivePortal().PortalUri)
      {
        Path = String.Format("sharing/rest/community/users/{0}", userName),
        Query = "f=json"
      };
      var groupResponse = new EsriHttpClient().Get(groupsURL.Uri.ToString());
      dynamic portalGroups = JObject.Parse(await groupResponse.Content.ReadAsStringAsync());

      string groups = portalGroups.groups.ToString();

      #endregion

      // cref: ArcGIS.Desktop.Core.EsriHttpClient
      // cref: ArcGIS.Desktop.Core.EsriHttpClient.#ctor
      // cref: ArcGIS.Desktop.Core.EsriHttpClient.Get(System.String)
      // cref: ArcGIS.Desktop.Core.EsriHttpResponseMessage
      // cref: ArcGIS.Desktop.Core.EsriHttpResponseMessage.Content
      #region EsriHttpClient: Query for esri content on the active Portal
      //http://www.arcgis.com/sharing/search?q=owner:esri&f=json

      UriBuilder searchURL = new UriBuilder(ArcGISPortalManager.Current.GetActivePortal().PortalUri)
      {
        Path = "sharing/rest/search",
        Query = "q=owner:esri&f=json"
      };
      EsriHttpClient httpClient = new EsriHttpClient();
      var searchResponse = httpClient.Get(searchURL.Uri.ToString());
      dynamic resultItems = JObject.Parse(await searchResponse.Content.ReadAsStringAsync());

      long numberOfTotalItems = resultItems.total.Value;
      long currentCount = 0;

      List<dynamic> resultItemList = new List<dynamic>();
      // store the first results in the list
      resultItemList.AddRange(resultItems.results);
      currentCount = currentCount + resultItems.num.Value;
      //Up to 50
      while (currentCount < numberOfTotalItems && currentCount <= 50)
      {
        searchURL.Query = String.Format("q=owner:esri&start={0}&f=json", resultItems.nextStart.Value);
        searchResponse = httpClient.Get(searchURL.Uri.ToString());
        resultItems = JObject.Parse(await searchResponse.Content.ReadAsStringAsync());
        resultItemList.AddRange(resultItems.results);
        currentCount = currentCount + resultItems.num.Value;
      }

      #endregion
    }

    public static async Task EsriHttpClientMethods2()
    {
      // cref: ArcGIS.Desktop.Core.EsriHttpClient
      // cref: ArcGIS.Desktop.Core.EsriHttpClient.#ctor
      // cref: ArcGIS.Desktop.Core.EsriHttpClient.Get(System.String)
      // cref: ArcGIS.Desktop.Core.EsriHttpResponseMessage
      // cref: ArcGIS.Desktop.Core.EsriHttpResponseMessage.Content
      #region EsriHttpClient: Get a Web Map for the Current User and Add it to Pro

      UriBuilder searchURL = new UriBuilder(ArcGISPortalManager.Current.GetActivePortal().PortalUri)
      {
        Path = "sharing/rest/portals/self",
        Query = "f=json"
      };
      EsriHttpClient httpClient = new EsriHttpClient();
      EsriHttpResponseMessage response = httpClient.Get(searchURL.Uri.ToString());

      dynamic portalSelf = JObject.Parse(await response.Content.ReadAsStringAsync());
      // if the response doesn't contain the user information then it is essentially
      // an anonymous request against the portal
      if (portalSelf.user == null)
        return;
      string userName = portalSelf.user.username;

      searchURL.Path = "sharing/rest/search";
      string webMaps = "(type:\"Web Map\" OR type:\"Explorer Map\" OR type:\"Web Mapping Application\" OR type:\"Online Map\")";
      searchURL.Query = string.Format("q=owner:{0} {1}&f=json", userName, webMaps);

      var searchResponse = httpClient.Get(searchURL.Uri.ToString());
      dynamic resultItems = JObject.Parse(await searchResponse.Content.ReadAsStringAsync());

      long numberOfTotalItems = resultItems.total.Value;
      if (numberOfTotalItems == 0)
        return;

      List<dynamic> resultItemList = new List<dynamic>();
      resultItemList.AddRange(resultItems.results);
      //get the first result
      dynamic item = resultItemList[0];

      string itemID = item.id;
      Item currentItem = ItemFactory.Instance.Create(itemID, ItemFactory.ItemType.PortalItem);

      if (MapFactory.Instance.CanCreateMapFrom(currentItem))
      {
        Map newMap = MapFactory.Instance.CreateMapFromItem(currentItem);
        await ProApp.Panes.CreateMapPaneAsync(newMap);
      }

      #endregion
    }

    public static async Task EsriHttpClientMethods3()
    {
      // cref: ArcGIS.Desktop.Core.EsriHttpClient
      // cref: ArcGIS.Desktop.Core.EsriHttpClient.#ctor
      // cref: ArcGIS.Desktop.Core.EsriHttpClient.Get(System.String)
      // cref: ArcGIS.Desktop.Core.EsriHttpResponseMessage
      // cref: ArcGIS.Desktop.Core.EsriHttpResponseMessage.Content
      // cref: ArcGIS.Desktop.Core.ItemFactory.Create(System.String, ArcGIS.Desktop.Core.ItemFactory.ItemType)
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer<T>(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
      // cref: ArcGIS.Desktop.Mapping.LayerFactory
      #region EsriHttpClient: Get a Service Layer and Add it to Pro

      UriBuilder searchURL = new UriBuilder(ArcGISPortalManager.Current.GetActivePortal().PortalUri)
      {
        Path = "sharing/rest/search"
      };
      string layers = "(type:\"Map Service\" OR type:\"Image Service\" OR type:\"Feature Service\" OR type:\"WMS\" OR type:\"KML\")";
      //any public layer content
      searchURL.Query = string.Format("q={0}&f=json", layers);

      EsriHttpClient httpClient = new EsriHttpClient();

      var searchResponse = httpClient.Get(searchURL.Uri.ToString());
      dynamic resultItems = JObject.Parse(await searchResponse.Content.ReadAsStringAsync());

      long numberOfTotalItems = resultItems.total.Value;
      if (numberOfTotalItems == 0)
        return;

      List<dynamic> resultItemList = new List<dynamic>();
      resultItemList.AddRange(resultItems.results);
      //get the first result
      dynamic item = resultItemList[0];

      string itemID = item.id;
      Item currentItem = ItemFactory.Instance.Create(itemID, ItemFactory.ItemType.PortalItem);

      await QueuedTask.Run(() =>
      {
            //Create a LayerCreationParam
            var layerParam = new LayerCreationParams(currentItem);
            // if we have an item that can be turned into a layer
            // add it to the map
            if (LayerFactory.Instance.CanCreateLayerFrom(currentItem))
          LayerFactory.Instance.CreateLayer<FeatureLayer>(layerParam, MapView.Active.Map);
      });

      #endregion
    }
  }
}














