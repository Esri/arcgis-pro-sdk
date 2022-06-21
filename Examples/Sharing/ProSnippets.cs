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

namespace Content.Sharing.ProSnippet
{
  internal class ProSnippet
  {
    public static void EsriHttpClientMethods()
    {
      // cref: ArcGIS.Desktop.Core.ArcGISPortalManager
      // cref: ArcGIS.Desktop.Core.ArcGISPortalManager.Current
      // cref: ArcGIS.Desktop.Core.ArcGISPortalManager.GetActivePortal()
      // cref: ArcGIS.Desktop.Core.ArcGISPortal
      // cref: ArcGIS.Desktop.Core.ArcGISPortal.PortalUri
      #region ArcGISPortalManager: Get the Current Active Portal

      var active_portal = ArcGISPortalManager.Current.GetActivePortal();
      string uri = active_portal.PortalUri.ToString();

      #endregion

      // cref: ArcGIS.Desktop.Core.ArcGISPortalManager
      // cref: ArcGIS.Desktop.Core.ArcGISPortalManager.Current
      // cref: ArcGIS.Desktop.Core.ArcGISPortalManager.GetPortals()
      // cref: ArcGIS.Desktop.Core.ArcGISPortal
      // cref: ArcGIS.Desktop.Core.ArcGISPortal.PortalUri
      #region  ArcGISPortalManager: Get a list of all your Portals

      var portals = ArcGISPortalManager.Current.GetPortals();
      //Make a list of all the Uris
      var portalUris = portals.Select(p => p.PortalUri.ToString()).ToList();

      #endregion

      // cref: ArcGIS.Desktop.Core.ArcGISPortalManager
      // cref: ArcGIS.Desktop.Core.ArcGISPortalManager.Current
      // cref: ArcGIS.Desktop.Core.ArcGISPortalManager.AddPortal(System.Uri)
      #region ArcGISPortalManager: Add a portal to the list of portals

      var portalUri = new Uri("http://myportal.esri.com/portal/", UriKind.Absolute);
      ArcGISPortalManager.Current.AddPortal(portalUri);

      #endregion

      // cref: ArcGIS.Desktop.Core.ArcGISPortalManager
      // cref: ArcGIS.Desktop.Core.ArcGISPortalManager.Current
      // cref: ArcGIS.Desktop.Core.ArcGISPortalManager.GetPortal(System.Uri)
      // cref: ArcGIS.Desktop.Core.ArcGISPortal
      // cref: ArcGIS.Desktop.Core.ArcGISPortal.IsSignedOn()
      // cref: ArcGIS.Desktop.Core.ArcGISPortal.SignIn()
      // cref: ArcGIS.Desktop.Core.SignInResult
      // cref: ArcGIS.Desktop.Core.SignInResult.success
      // cref: ArcGIS.Desktop.Core.ArcGISPortalManager.SetActivePortal(ArcGIS.Desktop.Core.ArcGISPortal)
      #region ArcGISPortalManager: Get a portal and Sign In, Set it Active

      //Find the portal to sign in with using its Uri...
      var portal = ArcGISPortalManager.Current.GetPortal(new Uri(uri, UriKind.Absolute));
      if (!portal.IsSignedOn())
      {
        //Calling "SignIn" will trigger the OAuth popup if your credentials are
        //not cached (eg from a previous sign in in the session)
        if (portal.SignIn().success)
        {
          //Set this portal as my active portal
          ArcGISPortalManager.Current.SetActivePortal(portal);
        }
      }

      #endregion

      // cref: ArcGIS.Desktop.Core.Events.ActivePortalChangedEvent
      // cref: ArcGIS.Desktop.Core.Events.ActivePortalChangedEvent.Subscribe
      // cref: ArcGIS.Desktop.Core.Events.ActivePortalChangedEventArgs
      // cref: ArcGIS.Desktop.Core.Events.ActivePortalChangedEventArgs.ActivePortal
      // cref: ArcGIS.Desktop.Core.Events.ArcGISPortalAddedEvent
      // cref: ArcGIS.Desktop.Core.Events.ArcGISPortalAddedEvent.Subscribe
      // cref: ArcGIS.Desktop.Core.Events.ArcGISPortalAddedEventArgs
      // cref: ArcGIS.Desktop.Core.Events.ArcGISPortalAddedEventArgs.Portal
      // cref: ArcGIS.Desktop.Core.Events.ArcGISPortalRemovedEvent
      // cref: ArcGIS.Desktop.Core.Events.ArcGISPortalRemovedEvent.Subscribe
      // cref: ArcGIS.Desktop.Core.Events.ArcGISPortalRemovedEventArgs
      // cref: ArcGIS.Desktop.Core.Events.ArcGISPortalRemovedEventArgs.RemovedPortalUri
      // cref: ArcGIS.Desktop.Core.Events.PortalSignOnChangedEvent
      // cref: ArcGIS.Desktop.Core.Events.PortalSignOnChangedEvent.Subscribe
      // cref: ArcGIS.Desktop.Core.Events.PortalSignOnChangedEventArgs
      // cref: ArcGIS.Desktop.Core.Events.PortalSignOnChangedEventArgs.Portal
      // cref: ArcGIS.Desktop.Core.Events.PortalSignOnChangedEventArgs.IsSignedOn
      #region ArcGISPortalManager: Listen for the Portal Events

      ArcGIS.Desktop.Core.Events.ActivePortalChangedEvent.Subscribe((args) =>
      {

        var active_uri = args.ActivePortal?.PortalUri.ToString();
        //etc
      });

      ArcGIS.Desktop.Core.Events.ArcGISPortalAddedEvent.Subscribe((args) =>
      {
        var added_portal = args.Portal;
        //etc
      });

      ArcGIS.Desktop.Core.Events.ArcGISPortalRemovedEvent.Subscribe((args) =>
      {
        var old_uri = args.RemovedPortalUri;
        //etc
      });

      ArcGIS.Desktop.Core.Events.PortalSignOnChangedEvent.Subscribe((args) =>
      {
        var portal = args.Portal;
        var isSignedOn = args.IsSignedOn;
        //etc
      });

      #endregion
    }

  }
}