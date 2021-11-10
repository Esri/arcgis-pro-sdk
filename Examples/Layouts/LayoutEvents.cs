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
using System.Windows.Input;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;

using ArcGIS.Desktop.Layouts;
using ArcGIS.Desktop.Layouts.Events;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Core;

namespace LayoutEvents
{
  internal class LayoutEventSpyViewModel : DockPane
  {
    private const string _dockPaneID = "LayoutEvents_LayoutEventSpy";
    private static readonly object _lock = new object();
    private List<string> _entries = new List<string>();
    private ICommand _clearCmd = null;

    protected LayoutEventSpyViewModel() { }

    async protected override Task InitializeAsync()
    {
      LayoutView lytView = LayoutView.Active;
      await ProApp.Panes.CreateLayoutPaneAsync(lytView.Layout);
      // cref: ActivateMapFrameEvent;ArcGIS.Desktop.Layouts.Events.ActivateMapFrameEvent
      #region ActivateMapFrameEvent
      //Report the event args when a map frame is activated or deactivated.

      ArcGIS.Desktop.Layouts.Events.ActivateMapFrameEvent.Subscribe((args) =>
      {
        System.Windows.MessageBox.Show("ActiveMapFrameEvent:" +
        Environment.NewLine +
        "   arg IsActivated: " + args.IsActivated.ToString() +
        Environment.NewLine +
        "   arg Layout: " + args.Layout.Name +
        Environment.NewLine +
        "   arg MapFrame: " + args.MapFrame.Name);
      });
      #endregion ActivateMapFrameEvent

      // cref: ActiveLayoutViewChangedEvent;ArcGIS.Desktop.Layouts.Events.ActiveLayoutViewChangedEvent
      #region ActiveLayoutViewChangedEvent
      //Report the event args when a the layout view has changed.

      ArcGIS.Desktop.Layouts.Events.ActiveLayoutViewChangedEvent.Subscribe((args) =>
      {
        System.Windows.MessageBox.Show("ActiveLayoutViewChangedEvent:" +
          Environment.NewLine +
          "   arg Layoutview: " + args.LayoutView.Layout.Name +
          Environment.NewLine +
          "   arg Type: " + args.Type.ToString());
      });
      #endregion ActiveLayoutViewChangedEvent

      // cref: ElementAddedEvent;ArcGIS.Desktop.Layouts.Events.ElementAddedEvent
      #region ElementAddedEvent
      //Report the event args when an element is added to the layout.

      ArcGIS.Desktop.Layouts.Events.ElementAddedEvent.Subscribe((args) => 
      {
        var elements = args.Elements;
        string elemNames = "null";
        if (elements != null && elements.Count() > 0)
        {
          elemNames = string.Join(",", elements.Select(e => e.Name).ToList());
        }
        System.Windows.MessageBox.Show("ElementAddedEvent:" +
        Environment.NewLine +
        "   arg Elements: " + elemNames.ToString() +
        Environment.NewLine +
        "   arg Layout: " + args.Layout.Name);
      });
      #endregion ElementAddedEvent

      // cref: ElementRemovedEvent;ArcGIS.Desktop.Layouts.Events.ElementRemovedEvent
      #region ElementRemovedEvent
      //Report the event args when an element is removed from the layout.

      ArcGIS.Desktop.Layouts.Events.ElementRemovedEvent.Subscribe((args) => 
      {
        var elements = args.Elements;
        string elemNames = "null";
        if (elements != null && elements.Count() > 0)
        {
          elemNames = string.Join(",", elements.Select(e => e.Name).ToList());
        }
        System.Windows.MessageBox.Show("ElementRemovedEvent:" +
        Environment.NewLine +
        "   arg Elements: " + elemNames.ToString() +
        Environment.NewLine +
        "   arg Layout: " + args.Layout.Name);
      });
      #endregion ElementRemovedEvent

      // cref: ElementsPlacementChangedEvent;ArcGIS.Desktop.Layouts.Events.ElementsPlacementChangedEvent
      #region ElementsPlacementChangedEvent
      //Report the event args when an element's placement properties are changed.

      ArcGIS.Desktop.Layouts.Events.ElementsPlacementChangedEvent.Subscribe((args) => 
      {
        var elements = args.Elements;
        string elemNames = "null";
        if (elements != null && elements.Count() > 0)
        {
          elemNames = string.Join(",", elements.Select(e => e.Name).ToList());
        }
        System.Windows.MessageBox.Show("ElementsPlacementChangedEvent:" +
        Environment.NewLine +
        "   arg Elements: " + elemNames.ToString() +
        Environment.NewLine +
        "   arg Layout: " + args.Layout.Name);
      });
      #endregion ElementsPlacementChangedEvent

      // cref: ElementStyleChangedEvent;ArcGIS.Desktop.Layouts.Events.ElementStyleChangedEvent
      #region ElementStyleChangedEvent
      //Report the event args when an element's style is changed.

      ArcGIS.Desktop.Layouts.Events.ElementStyleChangedEvent.Subscribe((args) => 
      {
        var elements = args.Elements;
        string elemNames = "null";
        if (elements != null && elements.Count() > 0)
        {
          elemNames = string.Join(",", elements.Select(e => e.Name).ToList());
        }
        System.Windows.MessageBox.Show("ElementStyleChangedEvent:" +
        Environment.NewLine +
        "   arg Elements: " + elemNames.ToString() +
        Environment.NewLine +
        "   arg Layout: " + args.Layout.Name);
      });
      #endregion ElementStyleChangedEvent

      // cref: ElementsUpdatedEvent;ArcGIS.Desktop.Layouts.Events.ElementsUpdatedEvent
      #region ElementsUpdatedEvent
      //Report the event args when an element has been updated.

      ArcGIS.Desktop.Layouts.Events.ElementsUpdatedEvent.Subscribe((args) => 
      {
        var elements = args.Elements;
        string elemNames = "null";
        if (elements != null && elements.Count() > 0)
        {
          elemNames = string.Join(",", elements.Select(e => e.Name).ToList());
        }
        System.Windows.MessageBox.Show("ElementsUpdatedEvent:" +
        Environment.NewLine +
        "   arg Elements: " + elemNames.ToString() +
        Environment.NewLine +
        "   arg Layout: " + args.Layout.Name);
      });
      #endregion ElementsUpdatedEvent

      // cref: LayoutAddedEvent;ArcGIS.Desktop.Layouts.Events.LayoutAddedEvent
      #region LayoutAddedEvent
      //Report the event args when a layout is added.

      ArcGIS.Desktop.Layouts.Events.LayoutAddedEvent.Subscribe((args) =>
      {
        System.Windows.MessageBox.Show("LayoutAddedEvent:" +
          Environment.NewLine +
          "   arg Layout: " + args.Layout.Name);
      });
      #endregion LayoutAddedEvent

      // cref: LayoutChangedEvent;ArcGIS.Desktop.Layouts.Events.LayoutChangedEvent
      #region LayoutChangedEvent
      //Report the event args when a layout is changed.

      ArcGIS.Desktop.Layouts.Events.LayoutChangedEvent.Subscribe((args) =>
      {
        System.Windows.MessageBox.Show("LayoutChangedEvent:" +
        Environment.NewLine +
        "   arg Layout: " + args.Layout.Name);
      });
      #endregion LayoutChangedEvent

      // cref: LayoutClosedEvent;ArcGIS.Desktop.Layouts.Events.LayoutClosedEvent
      #region LayoutClosedEvent
      //Report the event args when a layout is closed.

      ArcGIS.Desktop.Layouts.Events.LayoutClosedEvent.Subscribe((args) =>
      {
        System.Windows.MessageBox.Show("LayoutClosedEvent:" +
        Environment.NewLine +
        "   arg LayoutPane: " + args.LayoutPane.Caption);
      });
      #endregion LayoutClosedEvent

      // cref: LayoutClosingEvent;ArcGIS.Desktop.Layouts.Events.LayoutClosingEvent
      #region LayoutClosingEvent
      //Report the event args when a layout is closing.

      ArcGIS.Desktop.Layouts.Events.LayoutClosingEvent.Subscribe((args) =>
      {
        System.Windows.MessageBox.Show("LayoutClosingEvent:" +
        Environment.NewLine +
        "   arg Layout: " + args.Layout.Name +
        Environment.NewLine +
        "   arg LayoutPane: " + args.LayoutPane.Caption);
      });
      #endregion LayoutClosingEvent

      // cref: LayoutPauseDrawingChangedEvent;ArcGIS.Desktop.Layouts.Events.LayoutPauseDrawingChangedEvent
      #region LayoutPauseDrawingChangedEvent
      //Report the event args when a layout's paused state is changed.

      ArcGIS.Desktop.Layouts.Events.LayoutPauseDrawingChangedEvent.Subscribe((args) =>
      {
        System.Windows.MessageBox.Show("LayoutPauseDrawingChangedEvent:" +
          Environment.NewLine +
          "   arg Layoutview: " + args.LayoutView.Layout.Name +
          Environment.NewLine +
          "   arg Type: " + args.Paused.ToString());
      });
      #endregion LayoutPauseDrawingChangedEvent

      // cref: LayoutRemovedEvent;ArcGIS.Desktop.Layouts.Events.LayoutRemovedEvent
      #region LayoutRemovedEvent
      //Report the event args when a layout is removed.

      ArcGIS.Desktop.Layouts.Events.LayoutRemovedEvent.Subscribe((args) =>
      {
        System.Windows.MessageBox.Show("LayoutViewEvent:" +
        Environment.NewLine +
        "   arg Layout: " + args.Layout.Name);
      });
      #endregion LayoutRemovedEvent

      // cref: LayoutRemovingEvent;ArcGIS.Desktop.Layouts.Events.LayoutRemovingEvent
      #region LayoutRemovingEvent
      //Report the event args when a layout is about to be removed.

      ArcGIS.Desktop.Layouts.Events.LayoutRemovingEvent.Subscribe((args) =>
      {
        if (args.LayoutPath == "CIMPATH=layout.xml")
        {
          args.Cancel = true;
        }
        return Task.FromResult(0);
      });
      #endregion LayoutRemovingEvent

      // cref: LayoutSelectionChangedEvent;ArcGIS.Desktop.Layouts.Events.LayoutSelectionChangedEvent
      #region LayoutSelectionChangedEvent
      //Report the event args when a layout's selection has changed.

      ArcGIS.Desktop.Layouts.Events.LayoutSelectionChangedEvent.Subscribe((args) => {
        var elements = args.Elements;
        string elemNames = "null";
        if (elements != null && elements.Count() > 0)
        {
          elemNames = string.Join(",", elements.Select(e => e.Name).ToList());
        }
        System.Windows.MessageBox.Show("LayoutSelectionChangedEvent:" +
        Environment.NewLine +
        "   arg Elements: " + elemNames.ToString() +
        Environment.NewLine +
        "   arg Layout: " + args.Layout.Name);
      });
      #endregion LayoutSelectionChangedEvent

      // cref: LayoutViewEvent;ArcGIS.Desktop.Layouts.Events.LayoutViewEvent
      #region LayoutViewEvent
      //Report the event args for the different types of layout view events.

      ArcGIS.Desktop.Layouts.Events.LayoutViewEvent.Subscribe((args) =>
      {
        System.Windows.MessageBox.Show("LayoutViewEvent:" +
        Environment.NewLine +
        "   arg Layoutview: " + args.LayoutView.Layout.Name +
        Environment.NewLine +
        "   arg Type: " + args.Type.ToString());
      });
      #endregion LayoutViewEvent

      // cref: MapSeriesEvent;ArcGIS.Desktop.Layouts.Events.MapSeriesEvent
      #region MapSeriesEvent
      //Report the event args when the map series properties have changed.

      ArcGIS.Desktop.Layouts.Events.MapSeriesEvent.Subscribe((args) =>
      {
        System.Windows.MessageBox.Show("MapSeriesEvent:" +
        Environment.NewLine +
        "   arg CurrentPageName: " + args.CurrentPageName.ToString() +
        Environment.NewLine +
        "   arg CurrentlPageNumber: " + args.CurrentPageNumber.ToString() +
        Environment.NewLine +
        "   arg Layout: " + args.Layout.Name +
        Environment.NewLine +
        "   arg Type: " + args.Type.ToString());
      });
      #endregion MapSeriesEvent

      // cref: PageChangedEvent;ArcGIS.Desktop.Layouts.Events.PageChangedEvent
      #region PageChangedEvent
      //Report the event args when a layout page properties are changed.

      ArcGIS.Desktop.Layouts.Events.PageChangedEvent.Subscribe((args) =>
      {
        System.Windows.MessageBox.Show("PageChangedEvent:" +
        Environment.NewLine +
        "   arg Layout: " + args.Layout.Name +
        Environment.NewLine +
        "   arg New CIMPage (height): " + args.NewPage.Height.ToString() +
        Environment.NewLine +
        "   arg Old CIMPage (height): " + args.OldPage.Height.ToString());
      });
      #endregion PageChangedEvent

    }

    private void AddEntry(string eventName, string entry) {
      lock (_lock)
      {
        _entries.Add($"{eventName} {entry}");
      }
      NotifyPropertyChanged(nameof(EventLog));
    }

    public ICommand ClearTextCmd
    {
      get
      {
        if (_clearCmd == null)
          _clearCmd = new RelayCommand(() => {
            lock (_lock) {
              _entries.Clear();
            }
            NotifyPropertyChanged(nameof(EventLog));
          });
        return _clearCmd;
      }
    }

    public string EventLog
    {
      get
      {
        string contents = "";
        lock (_lock) {
          contents = string.Join("\r\n", _entries.ToArray());
        }
        return contents;
      }
    }

    // Show the DockPane.
    internal static void Show()
    {
      DockPane pane = FrameworkApplication.DockPaneManager.Find(_dockPaneID);
      if (pane == null)
        return;

      pane.Activate();
    }
  }

  // Button implementation to show the DockPane.
	internal class LayoutEventSpy_ShowButton : Button
  {
    protected override void OnClick()
    {
      LayoutEventSpyViewModel.Show();
    }
  }
}
