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
using ArcGIS.Desktop.Framework.Threading.Tasks;
using System.Collections.ObjectModel;
using ArcGIS.Desktop.Core;
using ArcGIS.Core.CIM;
using System.Windows.Media.Imaging;

namespace Examples
{
  class Bookmark_Examples
  {
    /// Bookmark
    /// <example>
    /// <code title="Bookmark Dock Pane" description="Create a dock pane showing the bookmarks for the project." source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\BookmarkDockPane.xaml" lang="XAML"/>
    /// <code source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\BookmarkDockPane.xaml.cs" lang="CS"/>
    /// <code source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\BookmarkDockPaneViewModel.cs" lang="CS"/>
    /// </example>

    /// ProjectExtender.GetBookmarks()
    /// <example>
    /// <code title="Get Project Bookmarks" description="Get the collection of bookmarks for the project." region="Get Project Bookmarks" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\Bookmark_Examples.cs" lang="CS"/>
    /// </example>
    #region Get Project Bookmarks
    public Task<ReadOnlyObservableCollection<Bookmark>> GetProjectBookmarksAsync()
    {
      //Get the collection of bookmarks for the project.
      return QueuedTask.Run(() => Project.Current.GetBookmarks());
    }
    #endregion
    
    /// Map.GetBookmarks()
    /// <example>
    /// <code title="Get Map Bookmarks" description="Get the collection of bookmarks for a map." region="Get Map Bookmarks" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\Bookmark_Examples.cs" lang="CS"/>
    /// </example>
    #region Get Map Bookmarks
    public Task<ReadOnlyObservableCollection<Bookmark>> GetActiveMapBookmarksAsync()
    {
      return QueuedTask.Run(() =>
      {
        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return null;

        //Return the collection of bookmarks for the map.
        return mapView.Map.GetBookmarks();
      });
    }
    #endregion

    /// Bookmark.AddBookmark(MapView, string)
    /// <example>
    /// <code title="Add New Bookmark from MapView" description="Create a new bookmark from the active map view." region="Add New Bookmark from MapView" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\Bookmark_Examples.cs" lang="CS"/>
    /// </example>
    #region Add New Bookmark from MapView
    public Task<Bookmark> AddBookmarkAsync(string name)
    {
      return QueuedTask.Run(() =>
      {
        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return null;

        //Adding a new bookmark using the active view.
        return mapView.Map.AddBookmark(mapView, name);
      });
    }
    #endregion

    /// Bookmark.AddBookmark(CIMBookmark)
    /// <example>
    /// <code title="Add New Bookmark from CIMBookmark" description="Create a new bookmark from a camera." region="Add New Bookmark from CIMBookmark" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\Bookmark_Examples.cs" lang="CS"/>
    /// </example>
    #region Add New Bookmark from CIMBookmark
    public Task<Bookmark> AddBookmarkFromCameraAsync(Camera camera, string name)
    {
      return QueuedTask.Run(() =>
      {
        //Set properties for Camera
        CIMViewCamera cimCamera = new CIMViewCamera()
        {
          X = camera.X,
          Y = camera.Y,
          Z = camera.Z,
          Scale = camera.Scale,
          Pitch = camera.Pitch,
          Heading = camera.Heading,
          Roll = camera.Roll
        };

        //Create new CIM bookmark and populate its properties
        var cimBookmark = new CIMBookmark() { Camera = cimCamera, Name = name, ThumbnailImagePath = "" };

        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return null;

        //Add a new bookmark for the active map.
        return mapView.Map.AddBookmark(cimBookmark);
      });
    }
    #endregion

    /// Map.Remove(Bookmark)
    /// <example>
    /// <code title="Remove Bookmark with Name" description="Remove a bookmark for a map with a given name." region="Remove Bookmark with Name" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\Bookmark_Examples.cs" lang="CS"/>
    /// </example>
    #region Remove Bookmark with Name
    public Task RemoveBookmarkAsync(Map map, string name)
    {
      return QueuedTask.Run(() =>
      {
        //Find the first bookmark with the name
        var bookmark = map.GetBookmarks().FirstOrDefault(b => b.Name == name);
        if (bookmark == null)
          return;

        //Remove the bookmark
        map.RemoveBookmark(bookmark);
      });
    }
    #endregion

    /// Map.Move(Bookmark, int)
    /// <example>
    /// <code title="Move Bookmark to the Top" description="Move a bookmark for a map with a given name to the top." region="Move Bookmark to the Top" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\Bookmark_Examples.cs" lang="CS"/>
    /// </example>
    #region Move Bookmark to the Top
    public Task MoveBookmarkToTopAsync(Map map, string name)
    {
      return QueuedTask.Run(() =>
      {
        //Find the first bookmark with the name
        var bookmark = map.GetBookmarks().FirstOrDefault(b => b.Name == name);
        if (bookmark == null)
          return;

        //Remove the bookmark
        map.MoveBookmark(bookmark, 0);
      });
    }
    #endregion

    /// Bookmark.GetDefinition(), Bookmark.SetDefinition(CIMBookmark)
    /// <example>
    /// <code title="Set Bookmark Definition" description="Update the definition of a bookmark using a new extent." region="Update Extent for a Bookmark" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\Bookmark_Examples.cs" lang="CS"/>
    /// </example>
    #region Update Extent for a Bookmark
    public Task UpdateBookmarkExtentAsync(Bookmark bookmark, ArcGIS.Core.Geometry.Envelope envelope)
    {
      return QueuedTask.Run(() =>
      {
        //Get the bookmark's definition
        var bookmarkDef = bookmark.GetDefinition();

        //Modify the bookmark's location
        bookmarkDef.Location = envelope;

        //Clear the camera as it is no longer valid.
        bookmarkDef.Camera = null;

        //Set the bookmark definition
        bookmark.SetDefinition(bookmarkDef);
      });
    }
    #endregion

    /// Bookmark.MapURI
    /// <example>
    /// <code title="Find Map Bookmarks" description="Find all bookmarks associated with a particular map." region="Find Map Bookmarks" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\Bookmark_Examples.cs" lang="CS"/>
    /// </example>
    #region Find Map Bookmarks
    public IEnumerable<Bookmark> FindMapBookmarks(List<Bookmark> bookmarks, Map map)
    {
      //Return all the bookmarks from a collection that are stored with a given map.
      return bookmarks.Where(b => b.MapURI == map.URI);
    }
    #endregion

    /// Bookmark.Update(MapView)
    /// <example>
    /// <code title="Update Bookmark" description="Update a bookmark using the active map view." region="Update Bookmark" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\Bookmark_Examples.cs" lang="CS"/>
    /// </example>
    #region Update Bookmark
    public Task UpdateBookmarkAsync(Bookmark bookmark)
    {
      return QueuedTask.Run(() =>
      {
        //Get the active map view.
        var mapView = MapView.Active;
        if (mapView == null)
          return;

        //Update the bookmark using the active map view.
        bookmark.Update(mapView);
      });
    }
    #endregion

    /// Bookmark.Rename(string)
    /// <example>
    /// <code title="Rename Bookmark" description="Rename a bookmark." region="Rename Bookmark" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\Bookmark_Examples.cs" lang="CS"/>
    /// </example>
    #region Rename Bookmark
    public Task RenameBookmarkAsync(Bookmark bookmark, string newName)
    {
      return QueuedTask.Run(() => bookmark.Rename(newName));
    }
    #endregion

    /// Bookmark.SetThumbnail()
    /// <example>
    /// <code title="Change Thumbnail" description="Set the thumbnail for a bookmark." region="Change Thumbnail" source="..\..\ArcGIS\SharedArcGIS\SDK\Examples\ArcGIS.Desktop.Mapping\MapExploration\Bookmark_Examples.cs" lang="CS"/>
    /// </example>
    #region Change Thumbnail
    public Task SetThumbnailAsync(Bookmark bookmark, string imagePath)
    {
      //Set the thumbnail to an image on disk, ie. C:\Pictures\MyPicture.png.
      BitmapImage image = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
      return QueuedTask.Run(() => bookmark.SetThumbnail(image));
    }
    #endregion
  }
}

