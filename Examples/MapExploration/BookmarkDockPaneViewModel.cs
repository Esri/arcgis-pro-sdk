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
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using System.Collections.ObjectModel;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Core.Events;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework.Threading.Tasks;

namespace Examples
{
  /// <summary>
  /// ViewModel for the Bookmarks DockPane.
  /// </summary>
  internal class BookmarkDockPaneViewModel : DockPane
  {
    /// <summary>
    /// Subscribe to the ProjectOpenedEvent when the DockPane is created.
    /// </summary>
    protected BookmarkDockPaneViewModel()
    {
      ProjectOpenedEvent.Subscribe(OnProjectOpened);
    }

    /// <summary>
    /// Unsubscribe from the ProjectOpenedEvent when the DockPane is destroyed.
    /// </summary>
    ~BookmarkDockPaneViewModel()
    {
      ProjectOpenedEvent.Unsubscribe(OnProjectOpened);
    }

    /// <summary>
    /// Called by the framework when the DockPane initializes.
    /// </summary>
    protected override Task InitializeAsync()
    {
      return UpdateBookmarksAsync();
    }

    /// <summary>
    /// Bindable property for the collection of bookmarks for the project.
    /// </summary>
    private ReadOnlyObservableCollection<Bookmark> _bookmarks;
    public ReadOnlyObservableCollection<Bookmark> Bookmarks
    {
      get { return _bookmarks; }
    }

    /// <summary>
    /// Bindable property for the currently selected bookmark.
    /// </summary>
    private Bookmark _selectedBookmark;
    public Bookmark SelectedBookmark
    {
      get { return _selectedBookmark; }
      set
      {
        SetProperty(ref _selectedBookmark, value, () => SelectedBookmark);
      }
    }

    /// <summary>
    /// Zoom the active map view to the selected bookmark.
    /// </summary>
    /// <returns>True if the navigation completes successfully.</returns>
    internal Task<bool> ZoomToSelectedBookmarkAsync()
    {
      if (MapView.Active == null)
        return Task.FromResult(false);

      return MapView.Active.ZoomToAsync(SelectedBookmark, TimeSpan.FromSeconds(1.5));
    }

    /// <summary>
    /// Updates the collection of bookmarks which is called when the DockPane is initialized or a project is opened.
    /// </summary>
    private async Task UpdateBookmarksAsync()
    {
      _bookmarks = await QueuedTask.Run(() => Project.Current.GetBookmarks());
      NotifyPropertyChanged(() => Bookmarks);
    }

    /// <summary>
    /// Event delegate for the ProjectOpenedEvent
    /// </summary>
    private void OnProjectOpened(ProjectEventArgs obj)
    {
      Task t = UpdateBookmarksAsync();
    }
  }
}
